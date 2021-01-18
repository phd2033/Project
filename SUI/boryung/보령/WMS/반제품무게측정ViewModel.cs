using C1.Silverlight.Data;
using LGCNS.iPharmMES.Common;
using ShopFloorUI;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace 보령
{
    //[ShopFloorCustomHidden]
    public class 반제품무게측정ViewModel : ViewModelBase
    {
        #region [Property]
        반제품무게측정 _mainWnd;
        private string _SelectedScale = "BN-OS-005";
        private DispatcherTimer _repeater;
        private int _repeaterInterval = 2000;

        private string _VesselId;
        public string VesselId
        {
            get { return _VesselId; }
            set
            {
                _VesselId = value;
                OnPropertyChanged("VesselId");
            }
        }
        private string _TotalWeight;
        public string TotalWeight
        {
            get { return _TotalWeight; }
            set
            {
                _TotalWeight = value;
                OnPropertyChanged("TotalWeight");
            }
        }

        //private string _SUnit;
        //public string SUnit
        //{
        //    get { return _SUnit; }
        //    set
        //    {
        //        _SUnit = value;
        //        OnPropertyChanged("SUnit");
        //    }
        //}
        private ObservableCollection<IBCInfo> _IBCCollection;
        public ObservableCollection<IBCInfo> IBCCollection
        {
            get { return _IBCCollection; }
            set
            {
                _IBCCollection = value;
                OnPropertyChanged("IBCCollection");
            }
        }

        #endregion

        #region [BizRule]
        private BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo;
        public BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo
        {
            get { return _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo; }
            set
            {
                _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo = value;
                OnPropertyChanged("BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo");
            }
        }

        private BR_BRS_SEL_CurrentWeight _BR_BRS_SEL_CurrentWeight;
        public BR_BRS_SEL_CurrentWeight BR_BRS_SEL_CurrentWeight
        {
            get { return _BR_BRS_SEL_CurrentWeight; }
            set
            {
                _BR_BRS_SEL_CurrentWeight = value;
                OnPropertyChanged("BR_BRS_SEL_CurrentWeight");
            }
        }

        private BR_BRS_SEL_VESSEL_Info _BR_BRS_SEL_VESSEL_Info;
        public BR_BRS_SEL_VESSEL_Info BR_BRS_SEL_VESSEL_Info
        {
            get { return _BR_BRS_SEL_VESSEL_Info; }
            set
            {
                _BR_BRS_SEL_VESSEL_Info = value;
                OnPropertyChanged("BR_BRS_SEL_VESSEL_Info");
            }
        }
        private BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi _BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi;
        public BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi
        {
            get { return _BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi; }
            set
            {
                _BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi = value;
                OnPropertyChanged("_BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi");
            }
        }
        #endregion

        #region [Command]
        public ICommand LoadedCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    try
                    {
                        CommandCanExecutes["LoadedCommandAsync"] = false;
                        CommandResults["LoadedCommandAsync"] = false;

                        ///
                        if (arg != null && arg is 반제품무게측정)
                            _mainWnd = arg as 반제품무게측정;

                        // room scale 설정
                        _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.INDATAs.Add(new BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.INDATA
                        {
                            EQPTID = AuthRepositoryViewModel.Instance.RoomID,
                            LANGID = AuthRepositoryViewModel.Instance.LangID
                        });

                        if (await _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.Execute())
                        {
                            if (_BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.OUTDATAs.Count > 0)
                            {
                                //_SelectedScale = _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.OUTDATAs[0].EQPTID;
                                //바닥저울을 찾도록 로직 변경. 2021.01.18 phd
                                for (int idx = 0; idx < _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.OUTDATAs.Count; idx++)
                                {
                                    if (_BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.OUTDATAs[idx].MODEL.Contains("IFS4"))
                                        _SelectedScale = _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.OUTDATAs[idx].EQPTID;
                                }
                            }
                        }
                        _mainWnd.btnRecord.IsEnabled = false;
                        _mainWnd.txtVesselId.Focus();
                        ///

                        CommandResults["LoadedCommandAsync"] = true;
                    }
                    catch (Exception ex)
                    {
                        CommandResults["LoadedCommandAsync"] = false;
                        OnException(ex.Message, ex);
                    }
                    finally
                    {
                        CommandCanExecutes["LoadedCommandAsync"] = true;
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadedCommandAsync") ?
                        CommandCanExecutes["LoadedCommandAsync"] : (CommandCanExecutes["LoadedCommandAsync"] = true);
                });
            }
        }

        public ICommand WeighingCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    try
                    {
                        CommandCanExecutes["WeighingCommandAsync"] = false;
                        CommandResults["WeighingCommandAsync"] = false;

                        ///
                        VesselId = arg as string;

                        _BR_BRS_SEL_VESSEL_Info.INDATAs.Clear();
                        _BR_BRS_SEL_VESSEL_Info.OUTDATAs.Clear();

                        _BR_BRS_SEL_VESSEL_Info.INDATAs.Add(new BR_BRS_SEL_VESSEL_Info.INDATA
                        {
                            VESSELID = _VesselId
                        });

                        if (await _BR_BRS_SEL_VESSEL_Info.Execute())
                        {
                            if (_BR_BRS_SEL_VESSEL_Info.OUTDATAs.Count > 0)
                            {
                                if (_SelectedScale != null)
                                {
                                    if (_repeater == null || _repeater.IsEnabled == false)
                                    {
                                        _repeater = new DispatcherTimer();
                                        _repeater.Interval = new TimeSpan(0, 0, 0, 0, _repeaterInterval);
                                        _repeater.Tick += async (s, e) =>
                                        {
                                            try
                                            {
                                                _BR_BRS_SEL_CurrentWeight.INDATAs.Clear();
                                                _BR_BRS_SEL_CurrentWeight.INDATAs.Add(new BR_BRS_SEL_CurrentWeight.INDATA()
                                                {
                                                    ScaleID = _SelectedScale
                                                });

                                                if (await _BR_BRS_SEL_CurrentWeight.Execute(exceptionHandle: LGCNS.iPharmMES.Common.Common.enumBizRuleXceptionHandleType.FailEvent) && _BR_BRS_SEL_CurrentWeight.OUTDATAs.Count > 0)
                                                {
                                                    string curWeight = string.Format("{0:F3}", _BR_BRS_SEL_CurrentWeight.OUTDATAs[0].Weight);

                                                    if (_repeater != null && _repeater.IsEnabled)
                                                    {
                                                        TotalWeight = curWeight;
                                                    }
                                                    _mainWnd.btnRecord.IsEnabled = true;
                                                }
                                                else
                                                {
                                                    TotalWeight = "연결실패";
                                                    //SUnit = "";
                                                }
                                            }
                                            catch (TimeoutException er)
                                            {
                                                _repeater.Stop();
                                                _repeater = null;
                                                OnMessage(er.Message);
                                            }
                                            catch (FaultException ef)
                                            {
                                                _repeater.Stop();
                                                _repeater = null;
                                                OnMessage(ef.Message);
                                            }
                                        };
                                        _repeater.Start();
                                    }
                                    else
                                    {
                                        _repeater.Stop();
                                        Thread.Sleep(100);
                                        _repeater.Start();
                                    }
                                }
                                
                            }
                            else
                            {
                                OnMessage("조회된 BIN정보가 없습니다.");
                                InitializeData();
                            }
                        }

                        ///

                        CommandResults["WeighingCommandAsync"] = true;
                    }
                    catch (Exception ex)
                    {
                        CommandResults["WeighingCommandAsync"] = false;
                        OnException(ex.Message, ex);
                    }
                    finally
                    {
                        CommandCanExecutes["WeighingCommandAsync"] = true;
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("WeighingCommandAsync") ?
                        CommandCanExecutes["WeighingCommandAsync"] : (CommandCanExecutes["WeighingCommandAsync"] = true);
                });
            }
        }

        public ICommand RecordingCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    try
                    {
                        CommandCanExecutes["RecordingCommand"] = false;
                        CommandResults["RecordingCommand"] = false;

                        ///
                        if (!TotalWeight.Equals("연결실패") && !string.IsNullOrWhiteSpace(TotalWeight))
                        {
                            if (_repeater != null)
                            {
                                _repeater.Stop();
                                _repeater = null;
                            }

                            if (CheckVesselId(VesselId))
                            {
                                _mainWnd.btnRecord.IsEnabled = false;

                                _BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi.INDATAs.Clear();

                                _BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi.INDATAs.Add(new BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi.INDATA
                                {
                                    VESSELID = VesselId,
                                    USERID = AuthRepositoryViewModel.Instance.LoginedUserID,
                                    GROSSWEIGHT = decimal.Parse(this._TotalWeight),
                                    SCALEID = this._SelectedScale,
                                    ROOMNO = AuthRepositoryViewModel.Instance.RoomID
                                });

                                if (await _BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi.Execute() == true)
                                {
                                    IBCCollection.Add(new IBCInfo
                                    {
                                        VesselId = _BR_BRS_SEL_VESSEL_Info.OUTDATAs[0].VESSELID,
                                        ScaleId = this._SelectedScale,
                                        TotalWeight = this._TotalWeight,
                                        TareWeight = _BR_BRS_SEL_VESSEL_Info.OUTDATAs[0].TAREWEIGHT.ToString(),
                                        RawWeight = (decimal.Parse(TotalWeight) - _BR_BRS_SEL_VESSEL_Info.OUTDATAs[0].TAREWEIGHT).ToString()
                                    });

                                    InitializeData();
                                }
                                else
                                    _mainWnd.btnRecord.IsEnabled = true;
                            }
                            else
                                OnMessage("중복된 ID 입니다.");
                        }
                        ///

                        CommandResults["RecordingCommand"] = true;
                    }
                    catch (Exception ex)
                    {
                        CommandResults["RecordingCommand"] = false;
                        OnException(ex.Message, ex);
                    }
                    finally
                    {
                        CommandCanExecutes["RecordingCommand"] = true;
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("RecordingCommand") ?
                        CommandCanExecutes["RecordingCommand"] : (CommandCanExecutes["RecordingCommand"] = true);
                });
            }
        }

        public ICommand ComfirmCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    try
                    {
                        CommandCanExecutes["ComfirmCommandAsync"] = false;
                        CommandResults["ComfirmCommandAsync"] = false;

                        ///
                        if (IBCCollection.Count > 0)
                        {
                            var authHelper = new iPharmAuthCommandHelper(); // function code 입력
                            authHelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Function,
                                Common.enumAccessType.Create,
                                string.Format("반제품무게측정"),
                                string.Format("반제품무게측정"),
                                false,
                                "OM_ProductionOrder_SUI",
                                _mainWnd.CurrentOrderInfo.EquipmentID, _mainWnd.CurrentOrderInfo.RecipeID, null) == false)
                            {
                                throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                            }

                            DataSet ds = new DataSet();
                            DataTable dt = new DataTable("DATA");
                            ds.Tables.Add(dt);

                            dt.Columns.Add(new DataColumn("IBCID"));
                            dt.Columns.Add(new DataColumn("SCALEID"));
                            dt.Columns.Add(new DataColumn("TOTALWEIGHT"));
                            dt.Columns.Add(new DataColumn("TAREWEIGHT"));
                            dt.Columns.Add(new DataColumn("RAWWEIGHT"));

                            foreach (var item in IBCCollection)
                            {
                                var row = dt.NewRow();
                                row["IBCID"] = item.VesselId != null ? item.VesselId : "";
                                row["SCALEID"] = item.ScaleId != null ? item.ScaleId : "";
                                row["TOTALWEIGHT"] = item.TotalWeight != null ? item.TotalWeight : "";
                                row["TAREWEIGHT"] = item.TareWeight != null ? item.TareWeight : "";
                                row["RAWWEIGHT"] = item.RawWeight != null ? item.RawWeight : "";

                                dt.Rows.Add(row);
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

                            if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                            else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);
                            
                        }
                        else
                            OnMessage("조회된 데이터가 없습니다.");
                        ///

                        CommandResults["ComfirmCommandAsync"] = true;
                    }
                    catch (Exception ex)
                    {
                        CommandResults["ComfirmCommandAsync"] = false;
                        OnException(ex.Message, ex);
                    }
                    finally
                    {
                        CommandCanExecutes["ComfirmCommandAsync"] = true;
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ComfirmCommandAsync") ?
                        CommandCanExecutes["ComfirmCommandAsync"] : (CommandCanExecutes["ComfirmCommandAsync"] = true);
                });
            }
        }
        #endregion

        #region [Constructor]
        public 반제품무게측정ViewModel()
        {
            _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo = new BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo();
            
            _BR_BRS_SEL_CurrentWeight = new BR_BRS_SEL_CurrentWeight();
            _BR_BRS_SEL_VESSEL_Info = new BR_BRS_SEL_VESSEL_Info();
            _BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi = new BR_BRS_REG_ProductionOrderOutput_Scale_Weight_Multi();
            _IBCCollection = new ObservableCollection<IBCInfo>();
        }
        #endregion

        #region [etc]
        private bool CheckVesselId(string Id)
        {
            foreach (IBCInfo item in _IBCCollection)
            {
                if (Id == item.VesselId)
                    return false;
            }
            return true;
        }
        private void InitializeData()
        {
            VesselId = "";
            TotalWeight = "";
            _mainWnd.btnRecord.IsEnabled = false;
            _mainWnd.txtVesselId.Focus();
        }
        public class IBCInfo : ViewModelBase
        {
            private string _VesselId;
            public string VesselId
            {
                get { return this._VesselId; }
                set
                {
                    this._VesselId = value;
                    this.OnPropertyChanged("VesselId");
                }
            }

            private string _ScaleId;
            public string ScaleId
            {
                get { return this._ScaleId; }
                set
                {
                    this._ScaleId = value;
                    this.OnPropertyChanged("VesselId");
                }
            }

            private string _TotalWeight;
            public string TotalWeight
            {
                get { return this._TotalWeight; }
                set
                {
                    this._TotalWeight = value;
                    this.OnPropertyChanged("TotalWeight");
                }
            }

            private string _TareWeight;
            public string TareWeight
            {
                get { return this._TareWeight; }
                set
                {
                    this._TareWeight = value;
                    this.OnPropertyChanged("TareWeight");
                }
            }

            private string _RawWeight;
            public string RawWeight
            {
                get { return this._RawWeight; }
                set
                {
                    this._RawWeight = value;
                    this.OnPropertyChanged("RawWeighit");
                }
            }
        }
        #endregion
    }
}
