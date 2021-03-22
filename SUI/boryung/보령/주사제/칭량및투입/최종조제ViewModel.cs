﻿using System;
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
using 보령.UserControls;

namespace 보령
{
    public class 최종조제ViewModel : ViewModelBase
    {
        #region [Property]
        public 최종조제ViewModel()
        {
            _BR_BRS_SEL_CurrentWeight = new BR_BRS_SEL_CurrentWeight();
            _BR_PHR_SEL_System_Printer = new BR_PHR_SEL_System_Printer();
            _BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID = new BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID();
            _BR_PHR_REG_ScaleSetTare = new BR_PHR_REG_ScaleSetTare();
            _BR_BRS_REG_ProductionOrderOutput_Conditional = new BR_BRS_REG_ProductionOrderOutput_Conditional();
            _BR_PHR_SEL_ProductionOrderOutput_Define = new BR_PHR_SEL_ProductionOrderOutput_Define();
            _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW = new BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW();
            _BR_BRS_SEL_Charging_Solvent = new BR_BRS_SEL_Charging_Solvent();
            _BR_BRS_SEND_WMS_WEIGHINGRESULT = new BR_BRS_SEND_WMS_WEIGHINGRESULT();

            int interval = 2000;
            string interval_str = ShopFloorUI.App.Current.Resources["GetWeightInterval"].ToString();
            if (int.TryParse(interval_str, out interval) == false)
            {
                interval = 2000;
            }

            _DispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, interval);
            _DispatcherTimer.Tick += _DispatcherTimer_Tick;
        }

        private 최종조제 _mainWnd;
        private DispatcherTimer _DispatcherTimer = new DispatcherTimer(); // 저울값 타이머
        private ScaleWebAPIHelper _restScaleService = new ScaleWebAPIHelper();
        private BR_PHR_SEL_System_Printer.OUTDATA _selectedPrint;
        public string curPrintName
        {
            get
            {
                if (_selectedPrint != null)
                    return _selectedPrint.PRINTERNAME;
                else
                    return "N/A";
            }
        }

        /// <summary>
        /// setBeakerTare : 비커 Tare 측정단계, initial : 조제액 측정단계, add : 보충단계, end : 조제완료
        /// </summary>
        enum state
        {
            setBeakerTare, initial, add, end, exception
        };
        private state _curstate;
        public string CurrentStateSTR
        {
            get
            {
                switch (_curstate)
                {
                    case state.setBeakerTare:
                        return "용기무게측정";
                    case state.initial:
                        return "보충전조제액무게측정";
                    case state.add:
                        return "원료보충";
                    case state.end:
                        return "조제완료";
                    case state.exception:
                        return "N/A";
                    default:
                        return "N/A";
                }
            }
        }
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
        private Weight _BeakerTare = new Weight();
        public string BeakerTare
        {
            get { return _BeakerTare.WeightUOMString; }
        }

        private BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.OUTDATA _ScaleInfo;
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

        private Weight _ScaleWeight = new Weight();
        public string ScaleWeight
        {
            get { return _ScaleWeight.WeightUOMString; }
        }
        public int ScalePrecision
        {
            set
            {
                _ScaleWeight.Precision = value;
                _BeakerTare.Precision = value;
                _InitialWeight.Precision = value;
                _StandardWeight.Precision = value;
                _FinalWeight.Precision = value;
                _TargetWeight.Precision = value;
                _MinWeight.Precision = value;
                _MaxWeight.Precision = value;
                _AddWeight.Precision = value;

                WeightRefresh();
            }
        }
        public string ScaleUom
        {
            set
            {
                _ScaleWeight.Uom = value;
                _BeakerTare.Uom = value;
                _InitialWeight.Uom = value;
                _StandardWeight.Uom = value;
                _FinalWeight.Uom = value;
                _TargetWeight.Uom = value;
                _MinWeight.Uom = value;
                _MaxWeight.Uom = value;
                _AddWeight.Uom = value;

                WeightRefresh();
            }
        }       
        /// <summary>
        /// 조제액(최종조제 전)
        /// </summary>
        private Weight _InitialWeight = new Weight();
        public string InitialWeight
        {
            get { return _InitialWeight.WeightUOMString; }
        }
        /// <summary>
        /// 조제액 기준량(최종조제)
        /// </summary>
        private Weight _StandardWeight = new Weight();
        public string StandardWeight
        {
            get { return _StandardWeight.WeightUOMString; }
        }
        /// <summary>
        /// 최종조제량
        /// </summary>
        private Weight _FinalWeight = new Weight();
        public string FinalWeight
        {
            get { return _FinalWeight.WeightUOMString; }
        }
        /// <summary>
        /// 보충량(기준)
        /// </summary>
        private Weight _TargetWeight = new Weight();
        public string TargetWeight
        {
            get { return _TargetWeight.WeightUOMString; }
        }
        /// <summary>
        /// 보충량(기준) 최소값
        /// </summary>
        private Weight _MinWeight = new Weight();
        public string MinWeight
        {
            get { return _MinWeight.WeightUOMString; }
        }
        /// <summary>
        /// 보충량(기준) 최대값
        /// </summary>
        private Weight _MaxWeight = new Weight();
        public string MaxWeight
        {
            get { return _MaxWeight.WeightUOMString; }
        }
        /// <summary>
        /// 보충량
        /// </summary>
        private Weight _AddWeight = new Weight();
        public string AddWeight
        {
            get { return _AddWeight.WeightUOMString; }
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
        
        #endregion
        #region [BizRule]
        private BR_BRS_SEL_CurrentWeight _BR_BRS_SEL_CurrentWeight;
        private BR_PHR_SEL_System_Printer _BR_PHR_SEL_System_Printer;
        private BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID _BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID;
        private BR_PHR_REG_ScaleSetTare _BR_PHR_REG_ScaleSetTare;
        private BR_BRS_REG_ProductionOrderOutput_Conditional _BR_BRS_REG_ProductionOrderOutput_Conditional;
        private BR_PHR_SEL_ProductionOrderOutput_Define _BR_PHR_SEL_ProductionOrderOutput_Define;
        private BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW;
        private BR_BRS_SEL_Charging_Solvent _BR_BRS_SEL_Charging_Solvent;
        private BR_BRS_SEND_WMS_WEIGHINGRESULT _BR_BRS_SEND_WMS_WEIGHINGRESULT;
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
                            decimal chk;

                            if (arg != null && arg is 최종조제)
                            {
                                _mainWnd = arg as 최종조제;
                                _mainWnd.Closed += (s, e) =>
                                {
                                    if (_DispatcherTimer != null)
                                        _DispatcherTimer.Stop();

                                    _DispatcherTimer = null;
                                };
                                _curstate = state.setBeakerTare;
                                btnNextEnable = false;
                                btnPrevEnable = false;

                                // 기준량 조회
                                var curinst = _mainWnd.CurrentInstruction.Raw;

                                if (!string.IsNullOrWhiteSpace(curinst.TARGETVAL) && decimal.TryParse(curinst.TARGETVAL, out chk))
                                {
                                    _StandardWeight.Value = chk;
                                    _StandardWeight.Uom = string.IsNullOrWhiteSpace(curinst.UOMNOTATION) ? "g" : curinst.UOMNOTATION;
                                    if(_StandardWeight.Uom.ToUpper() == "KG")
                                        _StandardWeight.Uom = "g";
                                }
                                else
                                    throw new Exception("최종조제기준량이 설정되지 않았습니다.");                              

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
                                {
                                    _selectedPrint = _BR_PHR_SEL_System_Printer.OUTDATAs[0];
                                    OnPropertyChanged("curPrintName");
                                }
                                else
                                   OnMessage("연결된 프린트가 없습니다.");

                                IsBusy = true;
                            }

                            WeightRefresh();
                            IsBusy = false;                            
                            ///

                            CommandResults["LoadedCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            _curstate = state.exception;                            
                            CommandResults["LoadedCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            OnPropertyChanged("CurrentStateSTR");
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

                            _DispatcherTimer.Stop();

                            BarcodePopup popup = new BarcodePopup();
                            popup.tbMsg.Text = "저울바코드를 스캔하세요.";
                            popup.Closed += async (sender, e) =>
                            {
                                if (popup.DialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(popup.tbText.Text))
                                {
                                    string text = popup.tbText.Text.ToUpper();

                                    // 저울 정보 조회
                                    _BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.INDATAs.Clear();
                                    _BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.OUTDATAs.Clear();
                                    _BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.INDATAs.Add(new BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.INDATA
                                    {
                                        LANGID = AuthRepositoryViewModel.Instance.LangID,
                                        EQPTID = text
                                    });

                                    if (await _BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.Execute() && _BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.OUTDATAs.Count > 0)
                                    {
                                        _ScaleInfo = _BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.OUTDATAs[0];
                                        ScalePrecision = _BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.OUTDATAs[0].PRECISION.HasValue ? Convert.ToInt32(_BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.OUTDATAs[0].PRECISION.Value) : 3;
                                        ScaleUom = _BR_BRS_SEL_EquipmentCustomAttributeValue_ScaleInfo_EQPTID.OUTDATAs[0].NOTATION;
                                        ScaleId = text;
                                        _DispatcherTimer.Start();
                                    }
                                }
                                else
                                    ScaleId = "";
                            };

                            popup.Show();


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
                            OnPropertyChanged("CurrentStateSTR");
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
        public ICommand ScanBeakerCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ScanBeakerCommand"].EnterAsync())
                    {
                        try
                        {

                            CommandResults["ScanBeakerCommand"] = false;
                            CommandCanExecutes["ScanBeakerCommand"] = false;

                            ///
                            IsBusy = true;

                            _DispatcherTimer.Stop();

                            BarcodePopup popup = new BarcodePopup();
                            popup.tbMsg.Text = "비커코드를 입력하세요.";
                            popup.Closed += (sender, e) =>
                            {
                                if (popup.DialogResult.GetValueOrDefault() && !string.IsNullOrWhiteSpace(popup.tbText.Text))                                                                    
                                    BeakerId = popup.tbText.Text.ToUpper();

                                _DispatcherTimer.Start();
                            };

                            popup.Show();                           
                            IsBusy = false;
                            ///

                            CommandResults["ScanBeakerCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["ScanBeakerCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            OnPropertyChanged("CurrentStateSTR");
                            CommandCanExecutes["ScanBeakerCommand"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ScanBeakerCommand") ?
                        CommandCanExecutes["ScanBeakerCommand"] : (CommandCanExecutes["ScanBeakerCommand"] = true);
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
                            _DispatcherTimer.Stop();

                            if (string.IsNullOrWhiteSpace(BeakerId))
                                throw new Exception("사용중인 비커 정보가 없습니다.");

                            if (_ScaleInfo != null && !string.IsNullOrWhiteSpace(_ScaleId))
                            {
                                if(_ScaleInfo.TARE_MANDATORY == "Y")
                                {
                                    if (_ScaleWeight.Value > 0)
                                    {
                                        // 현재 저울값을 비커용기무게로 저장
                                        _BeakerTare = _ScaleWeight.Copy();
                                        OnPropertyChanged("BeakerTare");

                                        // 저울에 Tare 명령어 전달
                                        bool success = false;
                                        if (_ScaleInfo.INTERFACE.ToUpper() == "REST")
                                        {
                                            var result = await _restScaleService.DownloadString(_ScaleId, ScaleCommand.ST);

                                            success = result.code == "1" ? true : false;
                                        }
                                        else
                                        {
                                            _BR_PHR_REG_ScaleSetTare.INDATAs.Clear();
                                            _BR_PHR_REG_ScaleSetTare.INDATAs.Add(new BR_PHR_REG_ScaleSetTare.INDATA
                                            {
                                                ScaleID = _ScaleId
                                            });

                                            if (await _BR_PHR_REG_ScaleSetTare.Execute())
                                                success = true;
                                        }

                                        // 정상완료
                                        if (success)
                                        {
                                            _curstate = state.initial;
                                            btnNextEnable = true;
                                            btnPrevEnable = true;
                                        }
                                        else
                                            _BeakerTare.Value = 0;
                                    }
                                }
                                else if(_ScaleInfo.TARE_MANDATORY == "N")
                                {
                                    // 현재 저울값을 비커용기무게로 저장
                                    _BeakerTare = _ScaleWeight.Copy();
                                    OnPropertyChanged("BeakerTare");
                                    _curstate = state.initial;
                                    btnNextEnable = true;
                                    btnPrevEnable = true;
                                }
                            }

                            WeightRefresh();
                            _DispatcherTimer.Start();
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
                            OnPropertyChanged("CurrentStateSTR");
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
                                    if(_ScaleWeight.Subtract(_StandardWeight).Value > 0)
                                    {
                                        OnMessage("최종조제액 기준량 보다 클 수 없습니다.");
                                        _DispatcherTimer.Start();
                                        break;
                                    }

                                    // 조제액 무게 저장
                                    _InitialWeight = _ScaleWeight.Copy();
                                    _TargetWeight = _ScaleWeight.Copy();

                                    // 보충량 범위 지정
                                    _TargetWeight = _StandardWeight.Subtract(_InitialWeight);

                                    _MinWeight = _TargetWeight.Copy();
                                    _MaxWeight = _TargetWeight.Copy();
                                    _MinWeight.Value = _MinWeight.Value * 0.999m;
                                    _MaxWeight.Value = _MaxWeight.Value * 1.001m;
                                    
                                    _curstate = state.add;
                                    _DispatcherTimer.Start();
                                    break;                               
                                case state.add:
                                    if (curPrintName == "N/A")
                                    {
                                        OnMessage("지정된 프린트가 없습니다. 프린트를 지정 후 다시 다음 단계로 진행해주세요");
                                        break;
                                    }                                   

                                    var curAddweight = _ScaleWeight.Subtract(_InitialWeight);
                                    _FinalWeight = _ScaleWeight.Copy();

                                    if (curAddweight.Subtract(_MinWeight).Value >= 0 && curAddweight.Subtract(_MaxWeight).Value <= 0)
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
                                            MSUBLOTQTY = _FinalWeight.Value,
                                            TAREWEIGHT = _BeakerTare.Value,
                                            TAREUOMID = _BeakerTare.Uom,
                                            REASON = "최종조제액 생성",
                                            INSUSER = AuthRepositoryViewModel.Instance.LoginedUserID,
                                            IS_NEED_VESSELID = "N",
                                            IS_ONLY_TARE = "N",
                                            IS_NEW = "Y"
                                        });

                                        await setLabelData();

                                        _AddWeight = curAddweight;
                                        _curstate = state.end;
                                    }
                                    else
                                        OnMessage("보충량이 기준값을 벗어났습니다.");

                                    if(_curstate != state.end)
                                        _DispatcherTimer.Start();
                                    break;
                                default:
                                    break;
                            }

                            WeightRefresh();
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
                            OnPropertyChanged("CurrentStateSTR");
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
                                case state.initial: // 현재 저울값이 0이하면 Tare측정단계로 이동
                                    if (_ScaleWeight.Value <= 0)
                                    {
                                        _BeakerTare.Value = 0;
                                        _curstate = state.setBeakerTare;
                                    }
                                    else
                                        OnMessage("현재 저울값이 0보다 크면 Tare측정단계로 돌아갈 수 없습니다.");
                                     
                                    _DispatcherTimer.Start();
                                    break;
                                case state.add: // 보충전 조제액 무게보다 현재값이 작아야 이전단계로 변경 가능 
                                    if (_ScaleWeight.Subtract(_InitialWeight).Value <= 0)
                                    {
                                        _MinWeight.Value = 0;
                                        _MaxWeight.Value = 0;
                                        _TargetWeight.Value = 0;
                                        _curstate = state.initial;
                                    }
                                    else
                                        OnMessage("원료 보충전 조제액 무게보다 현재 저울값이 크면 보충전 조제액 측정단계로 돌아갈 수 없습니다.");

                                    _DispatcherTimer.Start();
                                    break;
                                case state.end:
                                    _AddWeight.Value = 0;
                                    _FinalWeight.Value = 0;
                                    _curstate = state.add;
                                    _DispatcherTimer.Start();
                                    break;
                                default:
                                    break;
                            }

                            WeightRefresh();
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
                            OnPropertyChanged("CurrentStateSTR");
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

                            // 최종조제액 생성
                            var bizrule = _BR_BRS_REG_ProductionOrderOutput_Conditional.IndataCopy();
                            if (await bizrule.Execute())
                            {
                                // 최종 조제시 보충원료 소분 및 투입
                                _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.INDATAs.Clear();
                                _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.INDATA_INVs.Clear();
                                _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.INDATAs.Add(new BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.INDATA
                                {
                                    INSUSER = string.IsNullOrWhiteSpace(AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_Charging")) ? AuthRepositoryViewModel.Instance.LoginedUserID : AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_Charging"),
                                    LANGID = AuthRepositoryViewModel.Instance.LangID,
                                    MSUBLOTBCD = _BR_BRS_SEL_Charging_Solvent.OUTDATAs[0].MSUBLOTBCD,
                                    MSUBLOTQTY = _AddWeight.Value,
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
                                    COMPONENTGUID = _BR_BRS_SEL_Charging_Solvent.OUTDATA_BOMs[0].COMPONENTGUID
                                });

                                await _BR_RHR_REG_MaterialSubLot_Dispense_Charging_NEW.Execute(exceptionHandle:Common.enumBizRuleXceptionHandleType.FailEvent);

                                _BR_BRS_SEND_WMS_WEIGHINGRESULT.INDATAs.Add(new BR_BRS_SEND_WMS_WEIGHINGRESULT.INDATA
                                {
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    WEIGHINGMETHOD = "WH007"
                                });
                                await _BR_BRS_SEND_WMS_WEIGHINGRESULT.Execute(exceptionHandle: Common.enumBizRuleXceptionHandleType.FailEvent);

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
                            CommandResults["ConfirmCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            OnPropertyChanged("CurrentStateSTR");
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
        public ICommand ChangePrintCommand
        {
            get
            {
                return new CommandBase(arg =>
                {
                    try
                    {
                        IsBusy = true;

                        CommandResults["ChangePrintCommand"] = false;
                        CommandCanExecutes["ChangePrintCommand"] = false;

                        ///
                        SelectPrinterPopup popup = new SelectPrinterPopup();

                        popup.Closed += (s, e) =>
                        {
                            if (popup.DialogResult.GetValueOrDefault())
                            {
                                if (popup.SourceGrid.SelectedItem != null && popup.SourceGrid.SelectedItem is BR_PHR_SEL_System_Printer.OUTDATA)
                                {
                                    _selectedPrint = popup.SourceGrid.SelectedItem as BR_PHR_SEL_System_Printer.OUTDATA;
                                    OnPropertyChanged("curPrintName");
                                }
                            }
                        };

                        popup.Show();
                        ///

                        CommandResults["ChangePrintCommand"] = true;
                    }
                    catch (Exception ex)
                    {
                        CommandResults["ChangePrintCommand"] = false;
                        OnException(ex.Message, ex);
                    }
                    finally
                    {
                        OnPropertyChanged("CurrentStateSTR");
                        CommandCanExecutes["ChangePrintCommand"] = true;
                        IsBusy = false;
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ChangePrintCommand") ?
                        CommandCanExecutes["ChangePrintCommand"] : (CommandCanExecutes["ChangePrintCommand"] = true);
                });
            }
        }
        #endregion
        #region [Custom]
        private void WeightRefresh()
        {
            OnPropertyChanged("ScaleWeight");
            OnPropertyChanged("BeakerTare");
            OnPropertyChanged("StandardWeight");
            OnPropertyChanged("InitialWeight");
            OnPropertyChanged("TargetWeight");
            OnPropertyChanged("MinWeight");
            OnPropertyChanged("MaxWeight");
            OnPropertyChanged("MaxWeight");
            OnPropertyChanged("AddWeight");
        }
        private async void _DispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                _DispatcherTimer.Stop();

                if (_ScaleInfo != null && !string.IsNullOrWhiteSpace(_ScaleId))
                {                    
                    // 저울에 Tare 명령어 전달
                    bool success = false;
                    string curWeight = string.Empty;
                    if (_ScaleInfo.INTERFACE.ToUpper() == "REST")
                    {
                        var result = await _restScaleService.DownloadString(_ScaleId, ScaleCommand.GW);

                        if (result.code == "1")
                        {
                            success = true;
                            curWeight = result.data;
                            ScaleUom = result.unit;

                            // 저울 유효숫자 설정
                            if (curWeight.Contains("."))
                            {
                                var splt = curWeight.Split('.');
                                if (splt.Length > 1)
                                    ScalePrecision = splt[1].Length;
                                else
                                    ScalePrecision = 0;
                            }
                            else
                                ScalePrecision = 0;
                        }                        
                    }
                    else
                    {
                        _BR_BRS_SEL_CurrentWeight.INDATAs.Clear();
                        _BR_BRS_SEL_CurrentWeight.OUTDATAs.Clear();

                        _BR_BRS_SEL_CurrentWeight.INDATAs.Add(new BR_BRS_SEL_CurrentWeight.INDATA
                        {
                            ScaleID = _ScaleId
                        });

                        if (await _BR_BRS_SEL_CurrentWeight.Execute(exceptionHandle: Common.enumBizRuleXceptionHandleType.FailEvent) == true && _BR_BRS_SEL_CurrentWeight.OUTDATAs.Count > 0)
                        {
                            success = true;
                            curWeight = string.Format(("{0:N" + _ScaleWeight.Precision + "}"), _BR_BRS_SEL_CurrentWeight.OUTDATAs[0].Weight);
                            ScaleUom = _BR_BRS_SEL_CurrentWeight.OUTDATAs[0].UOM;
                        }                       
                            
                    }                    

                    if (success)
                        _ScaleWeight.SetWeight(curWeight, _ScaleWeight.Uom);                     
                    else
                        _ScaleWeight.Value = 0;

                    WeightRefresh();
                    _DispatcherTimer.Start();
                }                      

                    
            }
            catch (Exception ex)
            {
                _DispatcherTimer.Stop();
                OnException(ex.Message, ex);
            }
        }
        private async Task<bool> setLabelData()
        {
            try
            {                
                DateTime weighingdttm = await AuthRepositoryViewModel.GetDBDateTimeNow();
                Weight gross = _BeakerTare.Add(_FinalWeight);

                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_INDATAs.Clear();
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_Parameters.Clear();
                _BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_INDATAs.Add(new BR_BRS_REG_ProductionOrderOutput_Conditional.LABEL_INDATA
                {
                    ReportPath = "/Reports/Label/LABEL_C0402_018_10",
                    PrintName = curPrintName,
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
                    ParamValue = gross.WeightUOMString
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
        #endregion        
    }
}
