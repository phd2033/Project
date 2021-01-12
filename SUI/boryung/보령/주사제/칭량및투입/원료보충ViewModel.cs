using C1.Silverlight.Data;
using LGCNS.iPharmMES.Common;
using LGCNS.iPharmMES.Recipe.Common;
using ShopFloorUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace 보령
{
    public class 원료보충ViewModel : ViewModelBase
    {
        #region [Property]
        public 원료보충ViewModel()
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
            _BR_BRS_SEL_ProductionOrderBOM_CHGSEQ = new BR_BRS_SEL_ProductionOrderBOM_CHGSEQ();
            _BR_BRS_SEL_EquipmentStatusParameter_EQSTID = new BR_BRS_SEL_EquipmentStatusParameter_EQSTID();
            _BR_PHR_SEL_System_Printer = new BR_PHR_SEL_System_Printer();
            _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_FROZEN = new BR_BRS_UPD_MaterialSubLot_ChangeQuantity();
            _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_ADD = new BR_BRS_UPD_MaterialSubLot_ChangeQuantity();
        }

        원료보충 _mainWnd;
        DispatcherTimer _DispatcherTimer = new DispatcherTimer(); // 저울값 타이머

        /// <summary>
        /// frozen : 냉각 후 중량 측정 단계, add : 보충 단계, end : 종료
        /// </summary>
        enum state
        {
            frozen, add, end
        };
        private state _curstate;

        private string _printname;
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
        private string _HEADER1 = "N/A";
        public string HEADER1
        {
            get { return _HEADER1; }
            set
            {
                _HEADER1 = value;
                OnPropertyChanged("HEADER1");
            }
        }
        private string _HEADER2 = "N/A";
        public string HEADER2
        {
            get { return _HEADER2; }
            set
            {
                _HEADER2 = value;
                OnPropertyChanged("HEADER2");
            }
        }
        private string _UOM;
        private decimal _InitialWeight;
        public string InitialWeight
        {
            get { return string.Format("{0:0.##0} {1}", _InitialWeight, _UOM); }
        }
        private decimal _FrozenWeight;
        public string FrozenWeight
        {
            get { return string.Format("{0:0.##0} {1}", _FrozenWeight, _UOM); }
        }
        private decimal _LossWeight;
        public string LossWeight
        {
            get { return string.Format("{0:0.##0} {1}", _LossWeight, _UOM); }
        }
        private decimal _MinValue;
        public string MinWeight
        {
            get { return string.Format("{0:0.##0} {1}", _MinValue, _UOM); }
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
        /// <summary>
        /// 보충하는 원료를 제외한 무게(용기무게 포함)
        /// </summary>
        private decimal _srcWeight;
        private bool _btnNextIsEnable;
        public bool btnNextIsEnable
        {
            get { return _btnNextIsEnable; }
            set
            {
                _btnNextIsEnable = value;
                OnPropertyChanged("btnNextIsEnable");
            }
        }
        private bool _btnPrevIsEnable;
        public bool btnPrevIsEnable
        {
            get { return _btnPrevIsEnable; }
            set
            {
                _btnPrevIsEnable = value;
                OnPropertyChanged("btnPrevIsEnable");
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
        private BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.OUTDATA _targetMTRL;
        #endregion
        #region [BizRule]
        private BR_BRS_SEL_CurrentWeight _BR_BRS_SEL_CurrentWeight;
        private BR_BRS_SEL_ProductionOrderBOM_CHGSEQ _BR_BRS_SEL_ProductionOrderBOM_CHGSEQ;
        public BR_BRS_SEL_ProductionOrderBOM_CHGSEQ BR_BRS_SEL_ProductionOrderBOM_CHGSEQ
        {
            get { return _BR_BRS_SEL_ProductionOrderBOM_CHGSEQ; }
            set
            {
                _BR_BRS_SEL_ProductionOrderBOM_CHGSEQ = value;
                OnPropertyChanged("BR_BRS_SEL_ProductionOrderBOM_CHGSEQ");
            }
        }
        private BR_BRS_SEL_EquipmentStatusParameter_EQSTID _BR_BRS_SEL_EquipmentStatusParameter_EQSTID;
        private BR_PHR_SEL_System_Printer _BR_PHR_SEL_System_Printer;
        private BR_BRS_UPD_MaterialSubLot_ChangeQuantity _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_FROZEN;
        private BR_BRS_UPD_MaterialSubLot_ChangeQuantity _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_ADD;
        #endregion
        #region [Command]
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
                            if (arg != null && arg is 원료보충)
                            {
                                _mainWnd = arg as 원료보충;                                

                                btnPrevIsEnable = false;
                                btnNextIsEnable = false;
                                btnConfrimEnable = false;

                                // 조제액은 Parmeter로 전달받은 BOM의 기준량 합. 현재 Instrction의 원료가 보충하는 원료. 헤더 문구 조회
                                var inputvalue = InstructionModel.GetParameterSender(_mainWnd.CurrentInstruction, _mainWnd.Instructions);
                                _BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.INDATAs.Clear();
                                _BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.OUTDATAs.Clear();

                                if (inputvalue.Count > 0)
                                {
                                    // Header Text Set
                                    if(string.IsNullOrWhiteSpace(inputvalue[0].Raw.TARGETVAL))
                                        throw new Exception("헤더가 정해지지 않았습니다");
                                    else
                                    {
                                        var headers = inputvalue[0].Raw.TARGETVAL.Split(',');
                                        if(headers.Count() == 2)
                                        {
                                            HEADER1 = headers[0].ToString();
                                            HEADER2 = headers[1].ToString();
                                        }
                                        else
                                            throw new Exception("헤더가 잘못 정해졌습니다.");
                                    }

                                    // 조제액 원료
                                    foreach (var item in inputvalue)
                                    {
                                        _BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.INDATAs.Add(new BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.INDATA
                                        {
                                            POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                            OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                            MTRLID = item.Raw.BOMID,
                                            CHGSEQ = item.Raw.EXPRESSION
                                        });
                                    }
                                }
                                else
                                    throw new Exception("파라미터가 설정되지 않았습니다.");

                                // 보충 원료 정보 조회
                                _BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.INDATAs.Add(new BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.INDATA
                                {
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                    MTRLID = _mainWnd.CurrentInstruction.Raw.BOMID,
                                    CHGSEQ = _mainWnd.CurrentInstruction.Raw.EXPRESSION
                                });

                                if (await BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.Execute() && BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.OUTDATAs.Count > 0)
                                {
                                    _InitialWeight = 0m;
                                    _srcWeight = 0m;
                                    decimal component;
                                    foreach (var item in _BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.OUTDATAs)
                                    {
                                        if (decimal.TryParse(item.STDQTY, out component))
                                            _InitialWeight += component;

                                        if (item.MTRLID == _mainWnd.CurrentInstruction.Raw.BOMID)
                                            _targetMTRL = item;
                                        else
                                            _srcWeight += component;
                                    }
                                    _srcWeight += _targetMTRL.TAREWEIGHT.HasValue ? _targetMTRL.TAREWEIGHT.Value : 0m;
                                }

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

                            if (HEADER1 == "N/A" || HEADER2 == "N/A")
                                throw new Exception("헤더가 정해지지 않았습니다");

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
                                    _curstate = state.frozen;
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
                            ValueRefresh();
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
                                case state.frozen:
                                    // 무게 변경 및 라벨 출력
                                    _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_FROZEN.INDATA_MLOTs.Clear();
                                    _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_FROZEN.INDATA_MSUBLOTs.Clear();

                                    _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_FROZEN.INDATA_MLOTs.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.INDATA_MLOT
                                    {
                                        MTRLID = _targetMTRL.MTRLID,
                                        MLOTID = _targetMTRL.MLOTID,
                                        MLOTVER = _targetMTRL.MLOTVER,
                                        REASON = HEADER1,
                                        INDUSER = AuthRepositoryViewModel.Instance.LoginedUserID
                                    });
                                    _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_FROZEN.INDATA_MSUBLOTs.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.INDATA_MSUBLOT
                                    {
                                        MSUBLOTID = _targetMTRL.MSUBLOTID,
                                        //MSUBLOTVER 비즈룰 내에서 최신 버전 조회
                                        MSUBLOTSEQ = _targetMTRL.MSUBLOTSEQ,
                                        MSUBLOTQTY = _FrozenWeight - _srcWeight,
                                        UOMID = _targetMTRL.UOMID,
                                        TAREWEIGHT = _targetMTRL.TAREWEIGHT,
                                        TAREUOMID = _targetMTRL.TAREUOMID
                                    });

                                    await setLabelData(_BR_BRS_UPD_MaterialSubLot_ChangeQuantity_FROZEN);

                                    // 손실량 계산
                                    _LossWeight = _InitialWeight - _FrozenWeight;
                                    _MinValue = _LossWeight * 0.99m;
                                    _MaxValue = _LossWeight * 1.01m;   
                                    _DispatcherTimer.Start();
                                    _curstate = state.add;
                                    break;
                                case state.add:
                                    // 보충량 Validation
                                    if (_MinValue <= _AddWeight && _AddWeight <= _MaxValue)
                                    {
                                        // 무게 변경 및 라벨 출력
                                        _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_ADD.INDATA_MLOTs.Clear();
                                        _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_ADD.INDATA_MSUBLOTs.Clear();

                                        _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_ADD.INDATA_MLOTs.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.INDATA_MLOT
                                        {
                                            MTRLID = _targetMTRL.MTRLID,
                                            MLOTID = _targetMTRL.MLOTID,
                                            MLOTVER = _targetMTRL.MLOTVER,
                                            REASON = HEADER2,
                                            INDUSER = AuthRepositoryViewModel.Instance.LoginedUserID
                                        });
                                        _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_ADD.INDATA_MSUBLOTs.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.INDATA_MSUBLOT
                                        {
                                            MSUBLOTID = _targetMTRL.MSUBLOTID,
                                            //MSUBLOTVER 비즈룰 내에서 최신 버전 조회
                                            MSUBLOTSEQ = _targetMTRL.MSUBLOTSEQ,
                                            MSUBLOTQTY = _FinalWeight - _srcWeight,
                                            UOMID = _targetMTRL.UOMID,
                                            TAREWEIGHT = _targetMTRL.TAREWEIGHT,
                                            TAREUOMID = _targetMTRL.TAREUOMID
                                        });

                                        await setLabelData(_BR_BRS_UPD_MaterialSubLot_ChangeQuantity_ADD);
                                        _DispatcherTimer.Stop();
                                        _curstate = state.end;
                                    }
                                    else
                                        throw new Exception("보충량이 손실량 범위에 들어오지 않습니다.");                                    
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
                                case state.add:
                                    _AddWeight = 0;
                                    _FinalWeight = 0;
                                    _LossWeight = 0;
                                    _MinValue = 0;
                                    _MaxValue = 0;
                                    _curstate = state.frozen;
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

                            ///
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

                            authHelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Function,
                                Common.enumAccessType.Create,
                                "원료보충",
                                "원료보충",
                                false,
                                "OM_ProductionOrder_SUI",
                                "", null, null) == false)
                            {
                                throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                            }

                            // 감소된 무게 반영, 보충한 무게 반영
                            if(await _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_FROZEN.Execute() &&  await _BR_BRS_UPD_MaterialSubLot_ChangeQuantity_ADD.Execute())
                            {
                                // XML 저장
                                DataSet ds = new DataSet();
                                DataTable dt = new DataTable("DATA");
                                ds.Tables.Add(dt);

                                dt.Columns.Add(new DataColumn("냉각된무게"));
                                dt.Columns.Add(new DataColumn("하한"));
                                dt.Columns.Add(new DataColumn("손실량"));
                                dt.Columns.Add(new DataColumn("상한"));
                                dt.Columns.Add(new DataColumn("보충량"));
                                dt.Columns.Add(new DataColumn("저울번호"));
                                dt.Columns.Add(new DataColumn("최종무게"));

                                DataRow row = dt.NewRow();
                                row["냉각된무게"] = FrozenWeight ?? "";
                                row["하한"] = MinWeight ?? "";
                                row["손실량"] = LossWeight ?? "";
                                row["상한"] = MaxWeight ?? "";
                                row["보충량"] = AddWeight ?? "";
                                row["저울번호"] = ScaleId ?? "";
                                row["최종무게"] = FinalWeight ?? "";
                                dt.Rows.Add(row);

                                // BOM 정보
                                dt = new DataTable("DATA1");
                                ds.Tables.Add(dt);

                                dt.Columns.Add(new DataColumn("원료명"));
                                dt.Columns.Add(new DataColumn("기준량"));

                                foreach (var item in BR_BRS_SEL_ProductionOrderBOM_CHGSEQ.OUTDATAs)
                                {
                                    if (item.MTRLNAME != "비커")
                                    {
                                        var row1 = dt.NewRow();
                                        row1["원료명"] = item.MTRLNAME ?? "";
                                        row1["기준량"] = item.STDQTY ?? "";
                                        dt.Rows.Add(row1);
                                    }
                                }

                                var xml = BizActorRuleBase.CreateXMLStream(ds);
                                var bytesArray = System.Text.Encoding.UTF8.GetBytes(xml);

                                _mainWnd.CurrentInstruction.Raw.ACTVAL = _mainWnd.TableTypeName;
                                _mainWnd.CurrentInstruction.Raw.NOTE = bytesArray;

                                var result = await _mainWnd.Phase.RegistInstructionValue(_mainWnd.CurrentInstruction, true);
                                if (result != enumInstructionRegistErrorType.Ok)
                                {
                                    throw new Exception(string.Format("값 등록 실패, ID={0}, 사유={1}", _mainWnd.CurrentInstruction.Raw.IRTGUID, result));
                                }

                                _DispatcherTimer.Stop();
                                if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                                else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);
                            }                            

                            CommandResults["ConfirmCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
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
            OnPropertyChanged("FrozenWeight");
            OnPropertyChanged("LossWeight");
            OnPropertyChanged("MinWeight");
            OnPropertyChanged("MaxWeight");
            OnPropertyChanged("AddWeight");
            OnPropertyChanged("FinalWeight");

            // 버튼 새로고침
            switch (_curstate)
            {               
                case state.frozen:
                    btnNextIsEnable = true;
                    btnPrevIsEnable = false;
                    btnConfrimEnable = false;
                    break;
                case state.add:
                    btnNextIsEnable = true;
                    btnPrevIsEnable = true;
                    btnConfrimEnable = false;
                    break;
                case state.end:
                    btnNextIsEnable = false;
                    btnPrevIsEnable = true;
                    btnConfrimEnable = true;
                    break;
                default:
                    btnNextIsEnable = false;
                    btnPrevIsEnable = false;
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
                        setWeight(_BR_BRS_SEL_CurrentWeight.OUTDATAs[0].Weight.HasValue ? _BR_BRS_SEL_CurrentWeight.OUTDATAs[0].Weight.Value : 0m);
                    }

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
                    case state.frozen:
                        _FrozenWeight = weight;
                        break;
                    case state.add:
                        _AddWeight = weight - _FrozenWeight;
                        _FinalWeight = weight;
                        break;
                }

                ValueRefresh();
            }
            catch (Exception ex)
            {
                OnException(ex.Message, ex);
            }            
        }
        private async Task<bool> setLabelData(BR_BRS_UPD_MaterialSubLot_ChangeQuantity bizrule)
        {
            try
            {
                DateTime weighingdttm = await AuthRepositoryViewModel.GetDBDateTimeNow();

                bizrule.LABEL_INDATAs.Clear();
                bizrule.LABEL_Parameterss.Clear();

                bizrule.LABEL_INDATAs.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_INDATA
                {
                    ReportPath = "/Reports/Label/LABEL_C0402_018_10",
                    PrintName = _printname,
                    USERID = AuthRepositoryViewModel.Instance.LoginedUserID,
                    PRINTYN = true
                });

                bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                {
                    ParamName = "POID",
                    ParamValue = _mainWnd.CurrentOrder.ProductionOrderID
                });
                bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                {
                    ParamName = "WEIGHINGDTTM",
                    ParamValue = weighingdttm.ToString("yyyy-MM-dd HH:mm")
                });
                bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                {
                    ParamName = "WEIGHINGOPERATOR",
                    ParamValue = AuthRepositoryViewModel.Instance.LoginedUserID
                });

                if (_curstate == state.frozen)
                {
                    bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                    {
                        ParamName = "GUBUN",
                        ParamValue = HEADER1
                    });
                    bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                    {
                        ParamName = "NETWEIGHT",
                        ParamValue = string.Format("{0:0.##0} {1}", _FrozenWeight, _UOM)
                    });
                    bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                    {
                        ParamName = "TAREWEIGHT",
                        ParamValue = string.Format("{0:0.##0} {1}", 0m, _UOM)
                    });
                    bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                    {
                        ParamName = "GROSSWEIGHT",
                        ParamValue = FrozenWeight
                    });

                }
                else if (_curstate == state.add)
                {
                    bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                    {
                        ParamName = "GUBUN",
                        ParamValue = HEADER2
                    });
                    bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                    {
                        ParamName = "NETWEIGHT",
                        ParamValue = AddWeight
                    });
                    bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                    {
                        ParamName = "TAREWEIGHT",
                        ParamValue = FrozenWeight
                    });
                    bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
                    {
                        ParamName = "GROSSWEIGHT",
                        ParamValue = string.Format("{0:0.##0} {1}", _AddWeight + _FrozenWeight, _UOM)
                    });
                }
                bizrule.LABEL_Parameterss.Add(new BR_BRS_UPD_MaterialSubLot_ChangeQuantity.LABEL_Parameters
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
