using C1.Silverlight.Data;
using LGCNS.iPharmMES.Common;
using ShopFloorUI;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace 보령
{
    public class 반제품입고ViewModel : ViewModelBase
    {
        #region [Property]
        private 반제품입고 _mainWnd;

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

        private string _IBCNo;
        public string IBCNo
        {
            get { return _IBCNo; }
            set
            {
                _IBCNo = value;
                OnPropertyChanged("IBCNo");
            }
        }

        private ObservableCollection<StrgOutInfo> _StrgOuts;
        public ObservableCollection<StrgOutInfo> StrgOuts
        {
            get { return _StrgOuts; }
            set
            {
                _StrgOuts = value;
                OnPropertyChanged("StrgOuts");
            }
        }

        #endregion
        #region [BizRule]
        private BR_PHR_GET_BIN_INFO _BR_PHR_GET_BIN_INFO;
        public BR_PHR_GET_BIN_INFO BR_PHR_GET_BIN_INFO
        {
            get { return _BR_PHR_GET_BIN_INFO; }
            set
            {
                _BR_PHR_GET_BIN_INFO = value;
                OnPropertyChanged("BR_PHR_GET_BIN_INFO");
            }
        }

        private BR_BRS_REG_WMS_Request_IN _BR_BRS_REG_WMS_Request_IN;
        public BR_BRS_REG_WMS_Request_IN BR_BRS_REG_WMS_Request_IN
        {
            get { return _BR_BRS_REG_WMS_Request_IN; }
            set
            {
                _BR_BRS_REG_WMS_Request_IN = value;
                OnPropertyChanged("BR_BRS_REG_WMS_Request_IN");
            }
        }

        private BR_BRS_GET_VESSEL_WMS_IN _BR_BRS_GET_VESSEL_WMS_IN;
        public BR_BRS_GET_VESSEL_WMS_IN BR_BRS_GET_VESSEL_WMS_IN
        {
            get { return _BR_BRS_GET_VESSEL_WMS_IN; }
            set
            {
                _BR_BRS_GET_VESSEL_WMS_IN = value;
                OnPropertyChanged("BR_BRS_GET_VESSEL_WMS_IN");
            }
        }

        #endregion
        #region [Command]
        public ICommand LoadedCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    try
                    {
                        CommandCanExecutes["LoadedCommand"] = false;
                        CommandResults["LoadedCommand"] = false;

                        ///
                        if (arg != null && arg is 반제품입고)
                            _mainWnd = arg as 반제품입고;

                        // 이전 데이터 load
                        _BR_BRS_GET_VESSEL_WMS_IN.INDATAs.Clear();
                        _BR_BRS_GET_VESSEL_WMS_IN.OUTDATAs.Clear();

                        _BR_BRS_GET_VESSEL_WMS_IN.INDATAs.Add(new BR_BRS_GET_VESSEL_WMS_IN.INDATA
                        {
                            RECIPEISTGUID = _mainWnd.CurrentInstruction.Raw.RECIPEISTGUID,
                            ACTIVITYID = _mainWnd.CurrentInstruction.Raw.ACTIVITYID,
                            IRTGUID = _mainWnd.CurrentInstruction.Raw.IRTGUID,
                            IRTRSTGUID = _mainWnd.CurrentInstruction.Raw.IRTRSTGUID
                        });

                        await _BR_BRS_GET_VESSEL_WMS_IN.Execute();

                        if (_BR_BRS_GET_VESSEL_WMS_IN.OUTDATAs.Count > 0)
                        {
                            foreach (var item in _BR_BRS_GET_VESSEL_WMS_IN.OUTDATAs)
                            {
                                StrgOuts.Add(new StrgOutInfo
                                {
                                    IBCID = item.IBCID,
                                    STRGDAY = item.STRGDAY,
                                    EXPIREDTTM = item.EXPIREDTTM,
                                    MLOTID = item.MLOTID
                                });
                            }
                        }

                        _mainWnd.txtVesselId.Focus();
                        ///

                        CommandResults["LoadedCommand"] = true;
                    }
                    catch (Exception ex)
                    {
                        CommandResults["LoadedCommand"] = false;
                        OnException(ex.Message, ex);
                    }
                    finally
                    {
                        CommandCanExecutes["LoadedCommand"] = true;
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadedCommand") ?
                        CommandCanExecutes["LoadedCommand"] : (CommandCanExecutes["LoadedCommand"] = true);
                });
            }
        }

        public ICommand CheckIBCInfoCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    try
                    {
                        CommandCanExecutes["CheckIBCInfoCommandAsync"] = false;
                        CommandResults["CheckIBCInfoCommandAsync"] = false;

                        ///
                        VesselId = arg as string;

                        _BR_PHR_GET_BIN_INFO.INDATAs.Clear();
                        _BR_PHR_GET_BIN_INFO.OUTDATAs.Clear();

                        _BR_PHR_GET_BIN_INFO.INDATAs.Add(new BR_PHR_GET_BIN_INFO.INDATA
                        {
                           POID = _mainWnd.CurrentOrder.ProductionOrderID,
                           EQPTID = VesselId
                        });

                        if (await _BR_PHR_GET_BIN_INFO.Execute())
                        {
                            if (_BR_PHR_GET_BIN_INFO.OUTDATAs.Count > 0)
                            {
                                IBCNo = _BR_PHR_GET_BIN_INFO.OUTDATAs[0].EQPTID;
                            }
                            else
                                OnMessage("BIN 정보가 없습니다.");
                        }
                        ///

                        CommandResults["CheckIBCInfoCommandAsync"] = true;
                    }
                    catch (Exception ex)
                    {
                        CommandResults["CheckIBCInfoCommandAsync"] = false;
                        OnException(ex.Message, ex);
                    }
                    finally
                    {
                        CommandCanExecutes["CheckIBCInfoCommandAsync"] = true;
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("CheckIBCInfoCommandAsync") ?
                        CommandCanExecutes["CheckIBCInfoCommandAsync"] : (CommandCanExecutes["CheckIBCInfoCommandAsync"] = true);
                });
            }
        }

        public ICommand StorageOutCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    try
                    {
                        CommandCanExecutes["StorageOutCommandAsync"] = false;
                        CommandResults["StorageOutCommandAsync"] = false;

                        ///
                        if (VesselId == IBCNo && CheckIBCId(IBCNo))
                        {
                            _BR_BRS_REG_WMS_Request_IN.INDATAs.Clear();
                            _BR_BRS_REG_WMS_Request_IN.OUTDATAs.Clear();

                            _BR_BRS_REG_WMS_Request_IN.INDATAs.Add(new BR_BRS_REG_WMS_Request_IN.INDATA
                            {
                                VESSELID = IBCNo,
                                ROOMNO = AuthRepositoryViewModel.Instance.RoomID,
                                USERID = AuthRepositoryViewModel.Instance.LoginedUserID,
                                POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID
                            });

                            if (await _BR_BRS_REG_WMS_Request_IN.Execute())
                            {
                                if (_BR_BRS_REG_WMS_Request_IN.OUTDATAs.Count > 0)
                                {
                                    StrgOuts.Add(new StrgOutInfo
                                    {
                                        IBCID = IBCNo,
                                        STRGDAY = _BR_BRS_REG_WMS_Request_IN.OUTDATAs[0].STRGDAY,
                                        EXPIREDTTM = _BR_BRS_REG_WMS_Request_IN.OUTDATAs[0].EXPIREDTTM,
                                        MLOTID = _BR_BRS_REG_WMS_Request_IN.OUTDATAs[0].MLOTID
                                    });
                                }
                            }
                        }
                        else
                            OnMessage("입력한 내용과 검색한 IBC ID가 다릅니다.");

                        ///

                        CommandResults["StorageOutCommandAsync"] = true;
                    }
                    catch (Exception ex)
                    {
                        CommandResults["StorageOutCommandAsync"] = false;
                        OnException(ex.Message, ex);
                    }
                    finally
                    {
                        CommandCanExecutes["StorageOutCommandAsync"] = true;
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("StorageOutCommandAsync") ?
                        CommandCanExecutes["StorageOutCommandAsync"] : (CommandCanExecutes["StorageOutCommandAsync"] = true);
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
                        if (StrgOuts.Count > 0)
                        {
                            var authHelper = new iPharmAuthCommandHelper(); // function code 입력
                            authHelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Function,
                                Common.enumAccessType.Create,
                                string.Format("반제품입고"),
                                string.Format("반제품입고"),
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
                            dt.Columns.Add(new DataColumn("STRGDAY"));
                            dt.Columns.Add(new DataColumn("EXPIREDTTM"));
                            dt.Columns.Add(new DataColumn("MLOTID"));

                            foreach (var item in StrgOuts)
                            {
                                var row = dt.NewRow();
                                row["IBCID"] = item.IBCID != null ? item.IBCID : "";
                                row["STRGDAY"] = item.STRGDAY != null ? item.STRGDAY : "";
                                row["EXPIREDTTM"] = item.EXPIREDTTM != null ? item.EXPIREDTTM : "";
                                row["MLOTID"] = item.MLOTID != null ? item.MLOTID : "";

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
        public 반제품입고ViewModel()
        {
            _BR_PHR_GET_BIN_INFO = new BR_PHR_GET_BIN_INFO();
            _BR_BRS_REG_WMS_Request_IN = new BR_BRS_REG_WMS_Request_IN();
            _BR_BRS_GET_VESSEL_WMS_IN = new BR_BRS_GET_VESSEL_WMS_IN();
            _StrgOuts = new ObservableCollection<StrgOutInfo>();
        }
        #endregion
        #region [etc]
        private bool CheckIBCId(string Id)
        {
            foreach (StrgOutInfo item in StrgOuts)
            {
                if (Id == item.IBCID)
                    return false;
            }
            return true;
        }
        public class StrgOutInfo : ViewModelBase
        {
            private string _IBCID;
            public string IBCID
            {
                get { return this._IBCID; }
                set
                {
                    this._IBCID = value;
                    this.OnPropertyChanged("IBCID");
                }
            }
            private string _STRGDAY;
            public string STRGDAY
            {
                get { return this._STRGDAY; }
                set
                {
                    this._STRGDAY = value;
                    this.OnPropertyChanged("STRGDAY");
                }
            }
            private string _EXPIREDTTM;
            public string EXPIREDTTM
            {
                get { return this._EXPIREDTTM; }
                set
                {
                    this._EXPIREDTTM = value;
                    this.OnPropertyChanged("EXPIREDTTM");
                }
            }
            private string _MLOTID;
            public string MLOTID
            {
                get { return this._MLOTID; }
                set
                {
                    this._MLOTID = value;
                    this.OnPropertyChanged("MLOTID");
                }
            }
        }
        #endregion
    }
}
