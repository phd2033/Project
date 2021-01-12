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
using LGCNS.iPharmMES.Common;
using System.Collections.ObjectModel;
using System.Linq;
using ShopFloorUI;
using C1.Silverlight.Data;
using System.Text;

namespace 보령
{
    public class 빈용기출고ViewModel : ViewModelBase
    {
        #region [Property]
        private 빈용기출고 _mainWnd;

        private string scaleid;

        private BR_PHR_SEL_EquipmentGroup.OUTDATA _VesselType;
        public BR_PHR_SEL_EquipmentGroup.OUTDATA VesselType
        {
            get { return _VesselType; }
            set
            {
                _VesselType = value;
                OnPropertyChanged("VesselType");
            }
        }

        private ObservableCollection<IBCInfo> _ListContainer;
        public ObservableCollection<IBCInfo> ListContainer
        {
            get { return _ListContainer; }
            set
            {
                _ListContainer = value;
                OnPropertyChanged("ListContainer");
            }
        }

        private ObservableCollection<IBCInfo> _CompleteComponent;
        public ObservableCollection<IBCInfo> CompleteComponent
        {
            get { return _CompleteComponent; }
            set
            {
                _CompleteComponent = value;
                OnPropertyChanged("CompleteComponent");
            }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        #endregion

        #region [BizRule] 
        private BR_PHR_SEL_ProductionOrderOutput_Define _BR_PHR_SEL_ProductionOrderOutput_Define;
        public BR_PHR_SEL_ProductionOrderOutput_Define BR_PHR_SEL_ProductionOrderOutput_Define
        {
            get { return _BR_PHR_SEL_ProductionOrderOutput_Define; }
            set
            {
                _BR_PHR_SEL_ProductionOrderOutput_Define = value;
                OnPropertyChanged("BR_PHR_SEL_ProductionOrderOutput_Define");
            }
        }

        private BR_BRS_SEL_ProductionOrder_IBCList _BR_BRS_SEL_ProductionOrder_IBCList;
        public BR_BRS_SEL_ProductionOrder_IBCList BR_BRS_SEL_ProductionOrder_IBCList
        {
            get { return _BR_BRS_SEL_ProductionOrder_IBCList; }
            set
            {
                _BR_BRS_SEL_ProductionOrder_IBCList = value;
                OnPropertyChanged("BR_BRS_SEL_ProductionOrder_IBCList");
            }
        }

        private BR_BRS_REG_WMS_Request_OUT _BR_BRS_REG_WMS_Request_OUT;
        public BR_BRS_REG_WMS_Request_OUT BR_BRS_REG_WMS_Request_OUT
        {
            get { return _BR_BRS_REG_WMS_Request_OUT; }
            set
            {
                _BR_BRS_REG_WMS_Request_OUT = value;
                OnPropertyChanged("BR_BRS_REG_WMS_Request_OUT");
            }
        }

        private BR_PHR_SEL_EquipmentGroup _BR_PHR_SEL_EquipmentGroup;
        public BR_PHR_SEL_EquipmentGroup BR_PHR_SEL_EquipmentGroup
        {
            get { return _BR_PHR_SEL_EquipmentGroup; }
            set
            {
                _BR_PHR_SEL_EquipmentGroup = value;
                OnPropertyChanged("BR_PHR_SEL_EquipmentGroup");
            }
        }

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

        private BR_BRS_REG_VESSEL_WEIGHT _BR_BRS_REG_VESSEL_WEIGHT;
        public BR_BRS_REG_VESSEL_WEIGHT BR_BRS_REG_VESSEL_WEIGHT
        {
            get { return _BR_BRS_REG_VESSEL_WEIGHT; }
            set
            {
                _BR_BRS_REG_VESSEL_WEIGHT = value;
                OnPropertyChanged("BR_BRS_REG_VESSEL_WEIGHT");
            }
        }

        private BR_BRS_GET_VESSEL_StorageOutList_EMPTY _BR_BRS_GET_VESSEL_StorageOutList_EMPTY;
        public BR_BRS_GET_VESSEL_StorageOutList_EMPTY BR_BRS_GET_VESSEL_StorageOutList_EMPTY
        {
            get { return _BR_BRS_GET_VESSEL_StorageOutList_EMPTY; }
            set
            {
                _BR_BRS_GET_VESSEL_StorageOutList_EMPTY = value;
                OnPropertyChanged("BR_BRS_GET_VESSEL_StorageOutList_EMPTY");
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
                    using (await AwaitableLocks["LoadedCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandCanExecutes["LoadedCommandAsync"] = false;
                            CommandResults["LoadedCommandAsync"] = false;

                            ///
                            IsBusy = true;

                            if (arg != null && arg is 빈용기출고)
                            {
                                _mainWnd = arg as 빈용기출고;

                                _BR_PHR_SEL_EquipmentGroup.INDATAs.Clear();
                                _BR_PHR_SEL_EquipmentGroup.OUTDATAs.Clear();
                                _BR_PHR_SEL_EquipmentGroup.INDATAs.Add(new BR_PHR_SEL_EquipmentGroup.INDATA
                                {
                                    LANGID = AuthRepositoryViewModel.Instance.LangID,
                                    EQPTTYPE = "EY001",
                                    EQPTGRPIUSE = "Y"

                                });
                                _BR_PHR_SEL_EquipmentGroup.INDATAs.Add(new BR_PHR_SEL_EquipmentGroup.INDATA
                                {
                                    LANGID = AuthRepositoryViewModel.Instance.LangID,
                                    EQPTTYPE = "EY010",
                                    EQPTGRPIUSE = "Y"

                                });

                                await _BR_PHR_SEL_EquipmentGroup.Execute();

                                _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.INDATAs.Clear();
                                _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.OUTDATAs.Clear();

                                _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.INDATAs.Add(new BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.INDATA
                                {
                                    EQPTID = AuthRepositoryViewModel.Instance.RoomID
                                });

                                if (await _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.Execute() && _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.OUTDATAs.Count > 0)
                                {
                                    for (int idx = 0; idx < _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.OUTDATAs.Count; idx++)
                                    {
                                        scaleid = _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.OUTDATAs[idx].MODEL.Contains("IFS4") ? _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo.OUTDATAs[idx].EQPTID : "";
                                    }
                                }

                                // xml조회 로직 필요
                                if (_mainWnd.CurrentInstruction.Raw.NOTE != null)
                                {
                                    var bytearray = _mainWnd.CurrentInstruction.Raw.NOTE;
                                    string xml = Encoding.UTF8.GetString(bytearray, 0, bytearray.Length);
                                    DataSet ds = new DataSet();
                                    ds.ReadXmlFromString(xml);

                                    if(ds.Tables.Count == 1 && ds.Tables[0].TableName == "DATA")
                                    {
                                        foreach (DataRow row in ds.Tables[0].Rows)
                                        {
                                            CompleteComponent.Add(new IBCInfo
                                            {
                                                CHK = false,
                                                VESSELID = row["용기번호"] != null ? row["용기번호"].ToString() : "",
                                                OPSGNAME = row["공정명"] != null ? row["공정명"].ToString() : "",
                                                OUTPUTID = row["공정중제품"] != null ? row["공정중제품"].ToString() : "",
                                                WASHINGDTTM = row["세척일시"] != null ? row["세척일시"].ToString() : "",
                                                CLEANEXPIREDTTM = row["세척유효기간"] != null ? row["세척유효기간"].ToString() : "",
                                                STATUS = row["상태"] != null ? row["상태"].ToString() : ""
                                            });
                                        }
                                    }
                                }
                            }
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
                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadedCommandAsync") ?
                        CommandCanExecutes["LoadedCommandAsync"] : (CommandCanExecutes["LoadedCommandAsync"] = true);
                });
            }
        }

        public ICommand SearchEmptyCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["SearchEmptyCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandCanExecutes["SearchEmptyCommandAsync"] = false;
                            CommandResults["SearchEmptyCommandAsync"] = false;

                            ///
                            IsBusy = true;
                            ListContainer.Clear();

                            _BR_PHR_SEL_ProductionOrderOutput_Define.INDATAs.Clear();
                            _BR_PHR_SEL_ProductionOrderOutput_Define.OUTDATAs.Clear();

                            _BR_BRS_SEL_ProductionOrder_IBCList.INDATAs.Clear();
                            _BR_BRS_SEL_ProductionOrder_IBCList.OUTDATAs.Clear();

                            _BR_PHR_SEL_ProductionOrderOutput_Define.INDATAs.Add(new BR_PHR_SEL_ProductionOrderOutput_Define.INDATA
                            {
                                POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID != null ? new Guid?(new Guid(_mainWnd.CurrentOrder.OrderProcessSegmentID)) : null
                            });

                            if (await _BR_PHR_SEL_ProductionOrderOutput_Define.Execute() == true && _BR_PHR_SEL_ProductionOrderOutput_Define.OUTDATAs.Count > 0)
                            {
                                foreach (var output in _BR_PHR_SEL_ProductionOrderOutput_Define.OUTDATAs)
                                {
                                    _BR_BRS_SEL_ProductionOrder_IBCList.INDATAs.Add(new BR_BRS_SEL_ProductionOrder_IBCList.INDATA
                                    {
                                        TYPE = "EMPTY",
                                        EQPTGRPID = this.VesselType != null ? this.VesselType.EQPTGRPID : "",
                                        POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                        OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                        OUTPUTGUID = output.OUTPUTGUID.ToString()
                                    });

                                    if (await _BR_BRS_SEL_ProductionOrder_IBCList.Execute() == true)
                                    {
                                        foreach (var item in _BR_BRS_SEL_ProductionOrder_IBCList.OUTDATAs)
                                        {
                                            ListContainer.Add(new IBCInfo
                                            {
                                                CHK = false,
                                                VESSELID = item.EQPTID,
                                                POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                                OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                                OPSGNAME = _mainWnd.CurrentOrder.OrderProcessSegmentName,
                                                OUTPUTID = output.OUTPUTID,
                                                OUTPUTGUID = output.OUTPUTGUID.ToString(),
                                                Weight = 0m,
                                                WASHINGDTTM = item.WASHINGDTTM,
                                                CLEANEXPIREDTTM = item.CLEANEXPIREDTTM,
                                                CVSTATUS = item.MTRLNAME,
                                                STATUS = "대기"
                                            });
                                        }
                                    }
                                }
                                
                            }
                            ///

                            CommandResults["SearchEmptyCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["SearchEmptyCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["SearchEmptyCommandAsync"] = true;
                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("SearchEmptyCommandAsync") ?
                        CommandCanExecutes["SearchEmptyCommandAsync"] : (CommandCanExecutes["SearchEmptyCommandAsync"] = true);
                });
            }
        }

        public ICommand RequestOUTCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["RequestOUTCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandCanExecutes["RequestOUTCommandAsync"] = false;
                            CommandResults["RequestOUTCommandAsync"] = false;

                            ///
                            IsBusy = true;
                            _mainWnd.btnRequestOut.IsEnabled = false;

                            if (ListContainer.Count < 1)
                                return;

                            if (ListContainer.Count(x => x.CHK && (x.STATUS.Equals("대기") || x.STATUS.Equals("실패"))) > 0)
                            {

                                var authelper = new iPharmAuthCommandHelper();

                                authelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "EM_Equipment_InOut");

                                if (await authelper.ClickAsync(Common.enumCertificationType.Function
                                    , Common.enumAccessType.Create
                                    , "WMS 출고"
                                    , "WMS 출고"
                                    , false
                                    , "EM_Equipment_InOut"
                                    , ""
                                    , null
                                    , null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }

                                var RequestList = _ListContainer.Where(x => x.CHK && (x.STATUS.Equals("대기") || x.STATUS.Equals("실패")));

                                foreach (var item in RequestList)
                                {
                                    _BR_BRS_REG_WMS_Request_OUT.INDATAs.Clear();
                                    _BR_BRS_REG_WMS_Request_OUT.OUTDATAs.Clear();

                                    _BR_BRS_REG_WMS_Request_OUT.INDATAs.Add(new LGCNS.iPharmMES.Common.BR_BRS_REG_WMS_Request_OUT.INDATA
                                    {
                                        VESSELID = item.VESSELID,
                                        ROOMNO = AuthRepositoryViewModel.Instance.RoomID,
                                        USERID = AuthRepositoryViewModel.GetUserIDByFunctionCode("EM_Equipment_InOut"),
                                        POID = item.POID,
                                        OPSGGUID = item.OPSGGUID,
                                        OUTPUTGUID = item.OUTPUTGUID,
                                        OUTGUBUN = "EMPTY",
                                        GROSSWEIGHT = null,
                                        UOMID = null
                                    });

                                    if (await _BR_BRS_REG_WMS_Request_OUT.Execute() == true)
                                    {
                                        item.STATUS = "출고완료";
                                        item.CHK = false;
                                        CompleteComponent.Add(item);
                                    }
                                    else
                                    {
                                        item.STATUS = "실패";
                                        item.CHK = false;
                                    }
                                }
                            }
                            ///

                            CommandResults["RequestOUTCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["RequestOUTCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["RequestOUTCommandAsync"] = true;
                            _mainWnd.btnRequestOut.IsEnabled = true;
                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("RequestOUTCommandAsync") ?
                        CommandCanExecutes["RequestOUTCommandAsync"] : (CommandCanExecutes["RequestOUTCommandAsync"] = true);
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
                            CommandCanExecutes["ConfirmCommandAsync"] = false;
                            CommandResults["ConfirmCommandAsync"] = false;

                            ///
                            IsBusy = true;

                            if (CompleteComponent.Count > 0)
                            {
                                var authHelper = new iPharmAuthCommandHelper();

                                if (_mainWnd.CurrentInstruction.Raw.INSDTTM.Equals("Y") && _mainWnd.CurrentInstruction.PhaseState.Equals("COMP"))
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
                                    "빈용기출고",
                                    "빈용기출고",
                                    false,
                                    "OM_ProductionOrder_SUI",
                                    "", null, null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }

                                DataSet ds = new DataSet();
                                DataTable dt = new DataTable("DATA");
                                ds.Tables.Add(dt);

                                dt.Columns.Add(new DataColumn("상태"));
                                dt.Columns.Add(new DataColumn("용기번호"));
                                dt.Columns.Add(new DataColumn("공정중제품"));
                                dt.Columns.Add(new DataColumn("공정명"));
                                dt.Columns.Add(new DataColumn("세척일시"));
                                dt.Columns.Add(new DataColumn("세척유효기간"));

                                foreach (var item in CompleteComponent)
                                {
                                    if (item.STATUS.Equals("출고완료"))
                                    {
                                        var row = dt.NewRow();

                                        row["상태"] = item.STATUS != null ? item.STATUS : "";
                                        row["용기번호"] = item.VESSELID != null ? item.VESSELID : "";
                                        row["공정중제품"] = item.OUTPUTID != null ? item.OUTPUTID : "";
                                        row["공정명"] = item.OPSGNAME != null ? item.OPSGNAME : "";
                                        row["세척일시"] = item.WASHINGDTTM != null ? item.WASHINGDTTM : "";
                                        row["세척유효기간"] = item.CLEANEXPIREDTTM != null ? item.CLEANEXPIREDTTM : "";

                                        dt.Rows.Add(row);
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

                                if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                                else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);
                            }
                            else
                                OnMessage("출고된 용기정보가 없습니다.");
                            ///

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

        #region [Constructor]
        public 빈용기출고ViewModel()
        {
            _BR_BRS_SEL_ProductionOrder_IBCList = new BR_BRS_SEL_ProductionOrder_IBCList();
            _BR_PHR_SEL_ProductionOrderOutput_Define = new BR_PHR_SEL_ProductionOrderOutput_Define();
            _BR_BRS_REG_WMS_Request_OUT = new BR_BRS_REG_WMS_Request_OUT();
            _BR_PHR_SEL_EquipmentGroup = new BR_PHR_SEL_EquipmentGroup();
            _BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo = new BR_PHR_SEL_EquipmentCustomAttributeValue_ScaleInfo();
            _BR_BRS_GET_VESSEL_StorageOutList_EMPTY = new BR_BRS_GET_VESSEL_StorageOutList_EMPTY();

            _ListContainer = new ObservableCollection<IBCInfo>();
            _CompleteComponent = new ObservableCollection<IBCInfo>();
        }
        #endregion

        #region [UserDefine]
        public class IBCInfo : ViewModelBase
        {
            private bool _CHK;
            public bool CHK
            {
                get { return _CHK; }
                set
                {
                    _CHK = value;
                    OnPropertyChanged("CHK");
                }
            }

            private string _VESSELID;
            public string VESSELID
            {
                get { return _VESSELID; }
                set
                {
                    _VESSELID = value;
                    OnPropertyChanged("VESSELID");
                }
            }

            private string _POID;
            public string POID
            {
                get { return _POID; }
                set
                {
                    _POID = value;
                    OnPropertyChanged("POID");
                }
            }

            private string _OPSGGUID;
            public string OPSGGUID
            {
                get { return _OPSGGUID; }
                set
                {
                    _OPSGGUID = value;
                    OnPropertyChanged("OPSGGUID");
                }
            }
            private string _OPSGNAME;
            public string OPSGNAME
            {
                get { return _OPSGNAME; }
                set
                {
                    _OPSGNAME = value;
                    OnPropertyChanged("OPSGNAME");
                }
            }

            private string _OUTPUTID;
            public string OUTPUTID
            {
                get { return _OUTPUTID; }
                set
                {
                    _OUTPUTID = value;
                    OnPropertyChanged("OUTPUTID");
                }
            }

            private string _OUTPUTGUID;
            public string OUTPUTGUID
            {
                get { return _OUTPUTGUID; }
                set
                {
                    _OUTPUTGUID = value;
                    OnPropertyChanged("OUTPUTGUID");
                }
            }
            private string _CVSTATUS;
            public string CVSTATUS
            {
                get { return _CVSTATUS; }
                set
                {
                    _CVSTATUS = value;
                    OnPropertyChanged("CVSTATUS");
                }
            }
            private string _STATUS;
            public string STATUS
            {
                get { return _STATUS; }
                set
                {
                    _STATUS = value;
                    OnPropertyChanged("STATUS");
                }
            }

            private decimal _Weight;
            public decimal Weight
            {
                get { return _Weight; }
                set
                {
                    _Weight = value;
                    OnPropertyChanged("Weight");
                }
            }

            private string _WASHINGDTTM;
            public string WASHINGDTTM
            {
                get { return _WASHINGDTTM; }
                set
                {
                    _WASHINGDTTM = value;
                    OnPropertyChanged("WASHINGDTTM");
                }
            }

            private string _CLEANEXPIREDTTM;
            public string CLEANEXPIREDTTM
            {
                get { return _CLEANEXPIREDTTM; }
                set
                {
                    _CLEANEXPIREDTTM = value;
                    OnPropertyChanged("CLEANEXPIREDTTM");
                }

            }
        }
        #endregion
    }
}
