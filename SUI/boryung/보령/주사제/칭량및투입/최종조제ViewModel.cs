using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ShopFloorUI;
using LGCNS.iPharmMES.Common;
using System.Windows.Threading;
using C1.Silverlight.Data;
using System.Text;
using System.Threading.Tasks;

namespace 보령
{
    public class 최종조제ViewModel : ViewModelBase
    {
        #region Property
        public 최종조제ViewModel()
        {
            int interval = 2000;
            string interval_str = ShopFloorUI.App.Current.Resources["GetWeightInterval"].ToString();
            if (int.TryParse(interval_str, out interval) == false)
            {
                interval = 2000;
            }

            _DispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, interval);
            _DispatcherTimer.Tick += _DispatcherTimer_Tick;

            _BR_BRS_SEL_CurrentWeight = new BR_BRS_SEL_CurrentWeight();
            _BR_PHR_SEL_System_Printer = new BR_PHR_SEL_System_Printer();
            _BR_PHR_REG_ScaleSetTare = new BR_PHR_REG_ScaleSetTare();
            _BR_PHR_SEL_ProductionOrderBOM_ComponentInfo = new BR_PHR_SEL_ProductionOrderBOM_ComponentInfo();
            _BR_BRS_REG_ProductionOrderOutput_Conditional = new BR_BRS_REG_ProductionOrderOutput_Conditional();
            _BR_PHR_SEL_ProductionOrderOutput_Define = new BR_PHR_SEL_ProductionOrderOutput_Define();
            _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW = new BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW();
            _BR_BRS_SEL_Charging_Solvent = new BR_BRS_SEL_Charging_Solvent();
        }

        최종조제 _mainWnd;
        DispatcherTimer _DispatcherTimer = new DispatcherTimer(); // 저울값 타이머

        /// <summary>
        /// setBeakerTare : 비커 Tare 측정단계, initial : 조제액 측정단계, add : 보충단계, end : 조제완료
        /// </summary>
        enum state
        {
            setBeakerTare, initial, add, end
        };
        private state _curstate;
        private string _printname;
        private Guid? _outputguid;
        private string _BeakerId;
        public string BeakerId
        {
            get { return _BeakerId; }
            set
            {
                _BeakerId = value;
                OnPropertyChanged("BeakerId");
            }
        }
        private decimal _BeakerTare = 0m;
        private string _BeakerUOM;
        public string BeakerTare
        {
            get { return string.Format("{0:0.##0} {1}", _BeakerTare, _BeakerUOM); }
        }
        private string _ScaleId;
        public string ScaleId
        {
            get { return _ScaleId; }
            set
            {
                _ScaleId = value;
                OnPropertyChanged("ScaleId");
            }
        }
        private string _UOM;
        private decimal _InitialWeight;
        public string InitialWeight
        {
            get { return string.Format("{0:0.##0} {1}", _InitialWeight, _UOM); }
        }     
        private decimal _MinValue;
        public string MinWeight
        {
            get { return string.Format("{0:0.##0} {1}", _MinValue, _UOM); }
        }
        private decimal _StandardValue;
        public string StandardWeight
        {
            get { return string.Format("{0:0.##0} {1}", _StandardValue, _UOM); }
        }
        private decimal _TargetValue;
        public string TargetWeight
        {
            get { return string.Format("{0:0.##0} {1}", _TargetValue, _UOM); }
        }
        private decimal _MaxValue;
        public string MaxWeight
        {
            get { return string.Format("{0:0.##0} {1}", _MaxValue, _UOM); }
        }
        private decimal _AddWeight;
        public string AddWeight
        {
            get { return string.Format("{0:0.##0} {1}", _AddWeight, _UOM); }
        }
        private decimal _FinalWeight;
        public string FinalWeight
        {
            get { return string.Format("{0:0.##0} {1}", _FinalWeight, _UOM); }
        }
        private bool _btnNextEnable;
        public bool btnNextEnable
        {
            get { return _btnNextEnable; }
            set
            {
                _btnNextEnable = value;
                OnPropertyChanged("btnNextEnable");
            }
        }
        private bool _btnPrevEnable;
        public bool btnPrevEnable
        {
            get { return _btnPrevEnable; }
            set
            {
                _btnPrevEnable = value;
                OnPropertyChanged("btnPrevEnable");
            }
        }       
        private bool _btnConfrimEnable;
        public bool btnConfrimEnable
        {
            get { return _btnConfrimEnable; }
            set
            {
                _btnConfrimEnable = value;
                OnPropertyChanged("btnConfrimEnable");
            }
        }
        #endregion
        #region [BizRule]
        private BR_BRS_SEL_CurrentWeight _BR_BRS_SEL_CurrentWeight;
        private BR_PHR_SEL_System_Printer _BR_PHR_SEL_System_Printer;
        private BR_PHR_REG_ScaleSetTare _BR_PHR_REG_ScaleSetTare;
        private BR_PHR_SEL_ProductionOrderBOM_ComponentInfo _BR_PHR_SEL_ProductionOrderBOM_ComponentInfo;
        private BR_BRS_REG_ProductionOrderOutput_Conditional _BR_BRS_REG_ProductionOrderOutput_Conditional;
        private BR_PHR_SEL_ProductionOrderOutput_Define _BR_PHR_SEL_ProductionOrderOutput_Define;
        private BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW;
        private BR_BRS_SEL_Charging_Solvent _BR_BRS_SEL_Charging_Solvent;
        #endregion
        #region Command
        public ICommand LoadedCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["LoadedCommand"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["LoadedCommand"] = false;
                            CommandCanExecutes["LoadedCommand"] = false;

                            ///
                            decimal chk;

                            if (arg != null && arg is 최종조제)
                            {
                                _mainWnd = arg as 최종조제;                               

                                // 기준량 조회
                                _BR_PHR_SEL_ProductionOrderBOM_ComponentInfo.INDATAs.Add(new BR_PHR_SEL_ProductionOrderBOM_ComponentInfo.INDATA
                                {
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID
                                });
                                if(await _BR_PHR_SEL_ProductionOrderBOM_ComponentInfo.Execute() && _BR_PHR_SEL_ProductionOrderBOM_ComponentInfo.OUTDATAs.Count > 0)
                                {

                                    _StandardValue = 0m;
                                    foreach (var item in _BR_PHR_SEL_ProductionOrderBOM_ComponentInfo.OUTDATAs)
                                    {
                                        if (decimal.TryParse(item.STDQTY, out chk))
                                            _StandardValue += chk;                                            
                                    }
                                }

                                // 보충원료 정보 조회
                                _BR_BRS_SEL_Charging_Solvent.INDATAs.Add(new BR_BRS_SEL_Charging_Solvent.INDATA
                                {
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                    CHGSEQ = _mainWnd.CurrentInstruction.Raw.EXPRESSION,
                                    ROOMNO = AuthRepositoryViewModel.Instance.RoomID,
                                    MTRLID = _mainWnd.CurrentInstruction.Raw.BOMID
                                });

                                if(!(await _BR_BRS_SEL_Charging_Solvent.Execute() && _BR_BRS_SEL_Charging_Solvent.OUTDATAs.Count > 0 && _BR_BRS_SEL_Charging_Solvent.OUTDATA_BOMs.Count > 0))
                                     throw new Exception("보충 원료 정보를 조회하지 못했습니다.");

                                // 공정중제품 정보 조회
                                _BR_PHR_SEL_ProductionOrderOutput_Define.INDATAs.Add(new BR_PHR_SEL_ProductionOrderOutput_Define.INDATA
                                {
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = new Guid(_mainWnd.CurrentOrder.OrderProcessSegmentID)
                                });
                                if (await _BR_PHR_SEL_ProductionOrderOutput_Define.Execute() == _BR_PHR_SEL_ProductionOrderOutput_Define.OUTDATAs.Count > 0)
                                    _outputguid = _BR_PHR_SEL_ProductionOrderOutput_Define.OUTDATAs[0].OUTPUTGUID;
                                else
                                    throw new Exception("공정중제품 정보를 조회하지 못했습니다.");

                                // 프린트 조회
                                _BR_PHR_SEL_System_Printer.INDATAs.Add(new BR_PHR_SEL_System_Printer.INDATA
                                {
                                    LANGID = AuthRepositoryViewModel.Instance.LangID,
                                    ROOMID = AuthRepositoryViewModel.Instance.RoomID
                                });
                                if (await _BR_PHR_SEL_System_Printer.Execute() && _BR_PHR_SEL_System_Printer.OUTDATAs.Count > 0)
                                    _printname = _BR_PHR_SEL_System_Printer.OUTDATAs[0].PRINTERNAME;
                                else
                                    throw new Exception("연결된 프린트가 없습니다.");

                                btnNextEnable = false;
                                btnPrevEnable = false;

                                IsBusy = true;
                            }

                            IsBusy = false;
                            ///

                            CommandResults["LoadedCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["LoadedCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            ValueRefresh();
                            CommandCanExecutes["LoadedCommand"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadedCommand") ?
                        CommandCanExecutes["LoadedCommand"] : (CommandCanExecutes["LoadedCommand"] = true);
                });
            }
        }
        public ICommand SetBeakerTareCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["SetBeakerTareCommand"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["SetBeakerTareCommand"] = false;
                            CommandCanExecutes["SetBeakerTareCommand"] = false;

                            ///
                            IsBusy = true;        

                            if (arg is string && !string.IsNullOrWhiteSpace(arg as string))
                            {
                                BeakerId = (arg as string).ToUpper().Trim();

                                _DispatcherTimer.Stop();
                                
                                // 저울에 Tare 명령어 전달
                                if(_BeakerTare > 0 && !string.IsNullOrWhiteSpace(_ScaleId))
                                {
                                    _BR_PHR_REG_ScaleSetTare.INDATAs.Clear();
                                    _BR_PHR_REG_ScaleSetTare.INDATAs.Add(new BR_PHR_REG_ScaleSetTare.INDATA
                                    {
                                        ScaleID = _ScaleId
                                    });

                                    if (await _BR_PHR_REG_ScaleSetTare.Execute())
                                    {
                                        _curstate = state.initial;
                                        btnNextEnable = true;
                                        btnPrevEnable = true;
                                    }
                                }

                                _DispatcherTimer.Start();
                            }

                            IsBusy = false;
                            ///

                            CommandResults["SetBeakerTareCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            _DispatcherTimer.Start();
                            CommandResults["SetBeakerTareCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            ValueRefresh();
                            CommandCanExecutes["SetBeakerTareCommand"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("SetBeakerTareCommand") ?
                        CommandCanExecutes["SetBeakerTareCommand"] : (CommandCanExecutes["SetBeakerTareCommand"] = true);
                });
            }
        }
        public ICommand ConnectScaleCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ConnectScaleCommand"].EnterAsync())
                    {
                        try
                        {

                            CommandResults["ConnectScaleCommand"] = false;
                            CommandCanExecutes["ConnectScaleCommand"] = false;

                            ///
                            IsBusy = true;                          

                            if (arg is string && !string.IsNullOrWhiteSpace(arg as string))
                            {
                                ScaleId = (arg as string).ToUpper().Trim();

                                _BR_BRS_SEL_CurrentWeight.INDATAs.Clear();
                                _BR_BRS_SEL_CurrentWeight.OUTDATAs.Clear();

                                _BR_BRS_SEL_CurrentWeight.INDATAs.Add(new BR_BRS_SEL_CurrentWeight.INDATA
                                {
                                    ScaleID = this.ScaleId
                                });

                                if (await _BR_BRS_SEL_CurrentWeight.Execute() == true && _BR_BRS_SEL_CurrentWeight.OUTDATAs.Count > 0)
                                {
                                    _UOM = _BR_BRS_SEL_CurrentWeight.OUTDATAs[0].UOM;
                                    setWeight(_BR_BRS_SEL_CurrentWeight.OUTDATAs[0].Weight.HasValue ? _BR_BRS_SEL_CurrentWeight.OUTDATAs[0].Weight.Value : 0m);

                                    _DispatcherTimer.Start();
                                }
                            }

                            IsBusy = false;
                            ///

                            CommandResults["ConnectScaleCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["ConnectScaleCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ConnectScaleCommand"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ConnectScaleCommand") ?
                        CommandCanExecutes["ConnectScaleCommand"] : (CommandCanExecutes["ConnectScaleCommand"] = true);
                });
            }
        }
        public ICommand NextPhaseCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["NextPhaseCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["NextPhaseCommand"] = false;
                            CommandCanExecutes["NextPhaseCommand"] = false;

                            ///
                            _DispatcherTimer.Stop();          

                            switch (_curstate)
                            {
                                case state.initial:
                                    // 보충량 범위 지정
                                    _TargetValue = _StandardValue - _InitialWeight;
                                    _MinValue = _TargetValue * 0.999m;
                                    _MaxValue = _TargetValue * 1.001m;
                                    _curstate = state.add;
                                    _DispatcherTimer.Start();
                                    break;                               
                                case state.add:
                                    if (_MinValue <= _AddWeight && _AddWeight <= _MaxValue)
                                    {
                                        // 반제품생성 및 라벨 출력
                                        _BR_BRS_REG_ProductionOrderOutput_Conditional.INDATAs.Clear();
                                        _BR_BRS_REG_ProductionOrderOutput_Conditional.INDATAs.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.INDATA
                                        {
                                            LOTTYPE = "WIP",
                                            POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                            OPSGGUID = new Guid(_mainWnd.CurrentOrder.OrderProcessSegmentID),
                                            OUTPUTGUID = _outputguid,
                                            VESSELID = BeakerId,
                                            MSUBLOTQTY = _FinalWeight,
                                            TAREWEIGHT = _BeakerTare,
                                            TAREUOMID = _BeakerUOM,
                                            REASON = "최종조제액 생성",
                                            INSUSER = AuthRepositoryViewModel.Instance.LoginedUserID,
                                            IS_NEED_VESSELID = "N",
                                            IS_ONLY_TARE = "N",
                                            IS_NEW = "Y"
                                        });

                                        await setLabelData();

                                        _curstate = state.end;
                                    }
                                    else
                                        throw new Exception("저울값 범위에 들어오지 않습니다.");
                                    break;
                                default:
                                    break;
                            }

                            IsBusy = false;
                            ///

                            CommandResults["NextPhaseCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            _DispatcherTimer.Start();
                            CommandResults["NextPhaseCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            ValueRefresh();
                            CommandCanExecutes["NextPhaseCommand"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("NextPhaseCommand") ?
                        CommandCanExecutes["NextPhaseCommand"] : (CommandCanExecutes["NextPhaseCommand"] = true);
                });
            }
        }
        public ICommand PrevPhaseCommnad
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["PrevPhaseCommnad"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["PrevPhaseCommnad"] = false;
                            CommandCanExecutes["PrevPhaseCommnad"] = false;

                            ///
                            _DispatcherTimer.Stop();
   
                            switch (_curstate)
                            {
                                case state.initial:
                                    _BeakerTare = 0;
                                    _curstate = state.setBeakerTare;
                                    _DispatcherTimer.Start();
                                    break;
                                case state.add:
                                    _MinValue = 0;
                                    _MaxValue = 0;
                                    _TargetValue = 0;
                                    _curstate = state.initial;
                                    _DispatcherTimer.Start();
                                    break;
                                case state.end:
                                    _AddWeight = 0;                                    
                                    _FinalWeight = 0;
                                    _curstate = state.add;
                                    _DispatcherTimer.Start();
                                    break;
                                default:
                                    break;
                            }

                            IsBusy = false;
                            ///

                            CommandResults["PrevPhaseCommnad"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            _DispatcherTimer.Start();
                            CommandResults["PrevPhaseCommnad"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            ValueRefresh();
                            CommandCanExecutes["PrevPhaseCommnad"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("PrevPhaseCommnad") ?
                        CommandCanExecutes["PrevPhaseCommnad"] : (CommandCanExecutes["PrevPhaseCommnad"] = true);
                });
            }
        }
        public ICommand ConfirmCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ConfirmCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["ConfirmCommandAsync"] = false;
                            CommandCanExecutes["ConfirmCommandAsync"] = false;

                            if (_curstate != state.end)
                                throw new Exception("칭량이 완료되지 않았습니다.");

                            _DispatcherTimer.Stop();
                            var authHelper = new iPharmAuthCommandHelper();

                            if (_mainWnd.CurrentInstruction.Raw.INSERTEDYN.Equals("Y") && _mainWnd.CurrentInstruction.PhaseState.Equals("COMP"))
                            {
                                authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                                if (await authHelper.ClickAsync(
                                    Common.enumCertificationType.Function,
                                    Common.enumAccessType.Create,
                                    string.Format("기록값을 변경합니다."),
                                    string.Format("기록값 변경"),
                                    true,
                                    "OM_ProductionOrder_SUI",
                                    "", _mainWnd.CurrentInstruction.Raw.RECIPEISTGUID, null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }
                            }

                            authHelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "OM_ProductionOrder_Charging");

                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Function,
                                Common.enumAccessType.Create,
                                "최종조제",
                                "최종조제",
                                false,
                                "OM_ProductionOrder_Charging",
                                "", null, null) == false)
                            {
                                throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                            }

                            // 최종조제액 생성
                            if(await _BR_BRS_REG_ProductionOrderOutput_Conditional.Execute())
                            {
                                // 최종 조제시 보충원료 소분 및 투입
                                _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.INDATAs.Clear();
                                _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.INDATA_INVs.Clear();
                                _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.INDATAs.Add(new BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.INDATA
                                {
                                    INSUSER = string.IsNullOrWhiteSpace(AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_Charging")) ? AuthRepositoryViewModel.Instance.LoginedUserID : AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_Charging"),
                                    LANGID = AuthRepositoryViewModel.Instance.LangID,
                                    MSUBLOTBCD = _BR_BRS_SEL_Charging_Solvent.OUTDATAs[0].MSUBLOTBCD,
                                    MSUBLOTQTY = _AddWeight,
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                    IS_NEED_CHKWEIGHT = "N",
                                    IS_FULL_CHARGE = "Y",
                                    IS_CHECKONLY = "N",
                                    IS_INVEN_CHARGE = "Y",
                                    IS_OUTPUT = "N",
                                    MSUBLOTID = _BR_BRS_SEL_Charging_Solvent.OUTDATAs[0].MSUBLOTID,
                                    CHECKINUSER = AuthRepositoryViewModel.GetSecondUserIDByFunctionCode("OM_ProductionOrder_Charging"),
                                });
                                _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.INDATA_INVs.Add(new BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.INDATA_INV
                                {
                                    COMPONENTGUID = _BR_BRS_SEL_Charging_Solvent.OUTDATA_BOMs[0].COMPONENTGUID.ToString()
                                });

                                await _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.Execute(exceptionHandle:Common.enumBizRuleXceptionHandleType.FailEvent);

                                // XML 변환
                                DataSet ds = new DataSet();
                                DataTable dt = new DataTable("DATA");
                                ds.Tables.Add(dt);

                                dt.Columns.Add(new DataColumn("하한"));
                                dt.Columns.Add(new DataColumn("보충량"));
                                dt.Columns.Add(new DataColumn("상한"));
                                dt.Columns.Add(new DataColumn("최종무게"));
                                dt.Columns.Add(new DataColumn("저울번호"));

                                DataRow row = dt.NewRow();
                                row["하한"] = MinWeight ?? "";
                                row["보충량"] = AddWeight ?? "";
                                row["상한"] = MaxWeight ?? "";
                                row["최종무게"] = FinalWeight ?? "";
                                row["저울번호"] = ScaleId ?? "";
                                dt.Rows.Add(row);

                                var xml = BizActorRuleBase.CreateXMLStream(ds);
                                var bytesArray = System.Text.Encoding.UTF8.GetBytes(xml);

                                _mainWnd.CurrentInstruction.Raw.ACTVAL = _mainWnd.TableTypeName;
                                _mainWnd.CurrentInstruction.Raw.NOTE = bytesArray;

                                var result = await _mainWnd.Phase.RegistInstructionValue(_mainWnd.CurrentInstruction, true);
                                if (result != enumInstructionRegistErrorType.Ok)
                                {
                                    throw new Exception(string.Format("값 등록 실패, ID={0}, 사유={1}", _mainWnd.CurrentInstruction.Raw.IRTGUID, result));
                                }

                                if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                                else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);
                            }                            

                            CommandResults["ConfirmCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            _DispatcherTimer.Start();
                            CommandResults["ConfirmCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ConfirmCommandAsync"] = true;

                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ConfirmCommandAsync") ?
                        CommandCanExecutes["ConfirmCommandAsync"] : (CommandCanExecutes["ConfirmCommandAsync"] = true);
                });
            }
        }
        #endregion
        #region [Custom]
        private void ValueRefresh()
        {
            // 무게값 새로고침
            OnPropertyChanged("InitialWeight");
            OnPropertyChanged("BeakerTare");
            OnPropertyChanged("MinWeight");
            OnPropertyChanged("MaxWeight");
            OnPropertyChanged("AddWeight");
            OnPropertyChanged("FinalWeight");
            OnPropertyChanged("TargetWeight");
            OnPropertyChanged("StandardWeight");
            // 버튼 새로고침
            switch (_curstate)
            {
                case state.initial:
                    btnNextEnable = true;
                    btnPrevEnable = true;
                    btnConfrimEnable = false;
                    break;
                case state.add:
                    btnNextEnable = true;
                    btnPrevEnable = true;
                    btnConfrimEnable = false;
                    break;
                case state.end:
                    btnNextEnable = false;
                    btnPrevEnable = true;
                    btnConfrimEnable = true;
                    break;
                default:
                    btnNextEnable = false;
                    btnPrevEnable = false;
                    btnConfrimEnable = false;
                    break;
            }
        }
        private async void _DispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                _DispatcherTimer.Stop();
                
                if (!string.IsNullOrWhiteSpace(ScaleId))
                {
                    _BR_BRS_SEL_CurrentWeight.INDATAs.Clear();
                    _BR_BRS_SEL_CurrentWeight.OUTDATAs.Clear();

                    _BR_BRS_SEL_CurrentWeight.INDATAs.Add(new BR_BRS_SEL_CurrentWeight.INDATA
                    {
                        ScaleID = this.ScaleId
                    });

                    if (await _BR_BRS_SEL_CurrentWeight.Execute(exceptionHandle: Common.enumBizRuleXceptionHandleType.FailEvent) == true && _BR_BRS_SEL_CurrentWeight.OUTDATAs.Count > 0)
                    {
                        _UOM = _BR_BRS_SEL_CurrentWeight.OUTDATAs[0].UOM;
                        _BeakerUOM = _BR_BRS_SEL_CurrentWeight.OUTDATAs[0].UOM;
                        setWeight(_BR_BRS_SEL_CurrentWeight.OUTDATAs[0].Weight.HasValue ? _BR_BRS_SEL_CurrentWeight.OUTDATAs[0].Weight.Value : -9999m);
                    }
                    else                   
                        setWeight(-9999m);                    

                    _DispatcherTimer.Start();
                }
            }
            catch (Exception ex)
            {
                _DispatcherTimer.Stop();
                OnException(ex.Message, ex);
            }
        }
        private void setWeight(decimal weight)
        {
            try
            {
                switch (_curstate)
                {
                    case state.setBeakerTare:
                        _BeakerTare = weight;
                        break;
                    case state.initial:
                        _InitialWeight = weight;
                        break;
                    case state.add:
                        _AddWeight = weight - _InitialWeight;
                        _FinalWeight = weight;
                        break;
                    default:
                        break;                      
                }

                ValueRefresh();
            }
            catch (Exception ex)
            {
                OnException(ex.Message, ex);
            }           
        }
        private async Task<bool> setLabelData()
        {
            try
            {
                DateTime weighingdttm = await AuthRepositoryViewModel.GetDBDateTimeNow();

                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_INDATAs.Clear();
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameters.Clear();
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_INDATAs.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_INDATA
                {
                    ReportPath = "/Reports/Label/LABEL_C0402_018_10",
                    PrintName = _printname,
                    USERID = AuthRepositoryViewModel.Instance.LoginedUserID,
                    PRINTYN = true
                });

                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameters.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameter
                {
                    ParamName = "POID",
                    ParamValue = _mainWnd.CurrentOrder.ProductionOrderID
                });
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameters.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameter
                {
                    ParamName = "GUBUN",
                    ParamValue = "최종 조제량"
                });
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameters.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameter
                {
                    ParamName = "WEIGHINGDTTM",
                    ParamValue = weighingdttm.ToString("yyyy-MM-dd HH:mm")
                });
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameters.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameter
                {
                    ParamName = "WEIGHINGOPERATOR",
                    ParamValue = AuthRepositoryViewModel.Instance.LoginedUserID
                });
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameters.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameter
                {
                    ParamName = "NETWEIGHT",
                    ParamValue = FinalWeight
                });
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameters.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameter
                {
                    ParamName = "TAREWEIGHT",
                    ParamValue = BeakerTare
                });
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameters.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameter
                {
                    ParamName = "GROSSWEIGHT",
                    ParamValue = string.Format("{0:0.##0} {1}", _FinalWeight + _BeakerTare, _UOM)
                });
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameters.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameter
                {
                    ParamName = "SCALEID",
                    ParamValue = ScaleId
                });

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex.Message, ex);
                return false;
            }            
        }
        public void StopTimer()
        {
            _DispatcherTimer.Stop();
        }
        #endregion        
    }
}
