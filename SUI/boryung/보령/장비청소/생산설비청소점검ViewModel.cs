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
using LGCNS.iPharmMES.Common;
using C1.Silverlight.Data;
using ShopFloorUI;
using Equipment;
using System.Collections.Generic;
using System.Text;

namespace 보령
{
    public class 생산설비청소점검ViewModel : ViewModelBase
    {
        #region [Property]
        public 생산설비청소점검ViewModel()
        {
            _BR_BRS_SEL_EquipmentStatus_PROCEQPT = new BR_BRS_SEL_EquipmentStatus_PROCEQPT();
            _BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi = new BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi();
        }
        private 생산설비청소점검 _mainWnd;
        private List<InstructionModel> refInst;

        private bool _CANRECORDFLAG;
        public bool CANRECORDFLAG
        {
            get { return _CANRECORDFLAG; }
            set
            {
                _CANRECORDFLAG = value;
                OnPropertyChanged("CANRECORDFLAG");
            }
        }
        #endregion
        #region [BizRule]
        private BR_BRS_SEL_EquipmentStatus_PROCEQPT _BR_BRS_SEL_EquipmentStatus_PROCEQPT;
        public BR_BRS_SEL_EquipmentStatus_PROCEQPT BR_BRS_SEL_EquipmentStatus_PROCEQPT
        {
            get { return _BR_BRS_SEL_EquipmentStatus_PROCEQPT; }
            set
            {
                _BR_BRS_SEL_EquipmentStatus_PROCEQPT = value;
                OnPropertyChanged("BR_BRS_SEL_EquipmentStatus_PROCEQPT");
            }
        }
        private BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi _BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi;
        public BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi
        {
            get { return _BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi; }
            set
            {
                _BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi = value;
                OnPropertyChanged("BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi");
            }
        }
        #endregion
        #region [Command]
        public ICommand LoadCommandAsync // 생산설비 정보 조회
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["LoadCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["LoadCommandAsync"] = false;
                            CommandCanExecutes["LoadCommandAsync"] = false;

                            ///
                            IsBusy = true;

                            if (arg != null && arg is 생산설비청소점검)
                            {
                                this._mainWnd = arg as 생산설비청소점검;
                                CANRECORDFLAG = false;

                                refInst = InstructionModel.GetParameterSender(_mainWnd.CurrentInstruction, _mainWnd.Instructions);

                                _BR_BRS_SEL_EquipmentStatus_PROCEQPT.INDATAs.Clear();
                                if (refInst.Count > 0)
                                {
                                    foreach (InstructionModel Inst in refInst)
                                    {
                                        if(Inst.Raw.ACTVAL != "N/A")
                                        {
                                            _BR_BRS_SEL_EquipmentStatus_PROCEQPT.INDATAs.Add(new BR_BRS_SEL_EquipmentStatus_PROCEQPT.INDATA
                                            {
                                                LANGID = "ko-KR",
                                                ROOMNO = AuthRepositoryViewModel.Instance.RoomID,
                                                POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                                BATCHNO = _mainWnd.CurrentOrder.BatchNo,
                                                EQPTID = Inst.Raw.EQPTID,
                                                OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                                ACTVAL = Inst.Raw.ACTVAL
                                            });
                                        }
                                    }

                                    await BR_BRS_SEL_EquipmentStatus_PROCEQPT.Execute();
                                }

                                CheckRecordFlag();
                            }
                            IsBusy = false;
                            ///

                            CommandResults["LoadCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["LoadCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["LoadCommandAsync"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadCommandAsync") ?
                        CommandCanExecutes["LoadCommandAsync"] : (CommandCanExecutes["LoadCommandAsync"] = true);
                });
            }
        }
        public ICommand ActionPerformCommandAsync // 로그북 기록창
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ActionPerformCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["ActionPerformCommandAsync"] = false;
                            CommandCanExecutes["ActionPerformCommandAsync"] = false;

                            ///
                            IsBusy = true;

                            if (arg is Button)
                            {
                                string selectedEqptId = (((arg as Button).Parent as C1.Silverlight.DataGrid.DataGridCellPresenter).Row.DataItem as BR_BRS_SEL_EquipmentStatus_PROCEQPT.OUTDATA).EQPTID;

                                if ((arg as Button).Name == "btnClean")
                                {
                                    // ActionPerform 팝업창
                                    var ActionPerformPopup = new EM_EquipmentManagement_PerformAction();
                                    ActionPerformPopup.SelectedEquipmentData = selectedEqptId;
                                    ActionPerformPopup.DataContext = new EM_EquipmentManagementPerformActionViewModel();
                                    ActionPerformPopup.Closed += (s, e) =>
                                    {
                                        RefreshData(selectedEqptId, selectedEqptId);
                                    };
                                    ActionPerformPopup.Show();
                                }
                                else
                                {
                                    // 작업장생산시작 액션
                                    var authHelper = new iPharmAuthCommandHelper();
                                    authHelper = new iPharmAuthCommandHelper();
                                    authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "EM_EquipmentManagement_Action");

                                    if (await authHelper.ClickAsync(
                                        Common.enumCertificationType.Role,
                                        Common.enumAccessType.Create,
                                        string.Format("[{0}] 생산 시작", selectedEqptId),
                                        "생산시작",
                                        false,
                                        "EM_EquipmentManagement_Action",
                                        "",
                                        null, null) == false)
                                    {
                                        throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                    }
                                    // 생산시작 액션
                                    var bizRule = new BR_BRS_UPD_EquipmentAction_ShopFloor_PROCSTRT_MULTI();
                                    bizRule.INDATAs.Add(new BR_BRS_UPD_EquipmentAction_ShopFloor_PROCSTRT_MULTI.INDATA()
                                    {
                                        EQPTID = selectedEqptId,
                                        USER = AuthRepositoryViewModel.GetUserIDByFunctionCode("EM_EquipmentManagement_Action"),
                                        LANGID = AuthRepositoryViewModel.Instance.LangID,
                                        DTTM = await AuthRepositoryViewModel.GetDBDateTimeNow()
                                    });

                                    bizRule.PARAMDATAs.Add(new BR_BRS_UPD_EquipmentAction_ShopFloor_PROCSTRT_MULTI.PARAMDATA()
                                    {
                                        EQPAID = LGCNS.iPharmMES.Common.AuthRepositoryViewModel.GetSystemOptionValue("LOGBOOK_PRODUCTION_ORDER"),
                                        PAVAL = _mainWnd.CurrentOrder.OrderID,
                                        EQPTID = selectedEqptId
                                    });
                                    bizRule.PARAMDATAs.Add(new BR_BRS_UPD_EquipmentAction_ShopFloor_PROCSTRT_MULTI.PARAMDATA()
                                    {
                                        EQPAID = LGCNS.iPharmMES.Common.AuthRepositoryViewModel.GetSystemOptionValue("LOGBOOK_PRODUCTION_BATCHNO"),
                                        PAVAL = _mainWnd.CurrentOrder.BatchNo,
                                        EQPTID = selectedEqptId
                                    });
                                    bizRule.PARAMDATAs.Add(new BR_BRS_UPD_EquipmentAction_ShopFloor_PROCSTRT_MULTI.PARAMDATA()
                                    {
                                        EQPAID = LGCNS.iPharmMES.Common.AuthRepositoryViewModel.GetSystemOptionValue("LOGBOOK_PRODUCTION_PROCESS"),
                                        PAVAL = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                        EQPTID = selectedEqptId
                                    });

                                    if (await bizRule.Execute() == true)
                                        RefreshData(selectedEqptId, selectedEqptId);
                                }

                                CheckRecordFlag();
                            }

                            IsBusy = false;
                            ///

                            CommandResults["ActionPerformCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["ActionPerformCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ActionPerformCommandAsync"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ActionPerformCommandAsync") ?
                        CommandCanExecutes["ActionPerformCommandAsync"] : (CommandCanExecutes["ActionPerformCommandAsync"] = true);
                });
            }
        }
        public ICommand ChangeWorkRoomCommandAsync // 생산설비 변경
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ChangeWorkRoomCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["ChangeWorkRoomCommandAsync"] = false;
                            CommandCanExecutes["ChangeWorkRoomCommandAsync"] = false;

                            ///
                            IsBusy = true;

                            string selectedEqptId = (((arg as Button).Parent as C1.Silverlight.DataGrid.DataGridCellPresenter).Row.DataItem as BR_BRS_SEL_EquipmentStatus_PROCEQPT.OUTDATA).EQPTID;
                            var Popup = new 설비상태팝업(_mainWnd.CurrentOrder.ProductionOrderID, _mainWnd.CurrentOrder.OrderProcessSegmentID, "PROCEQPT", selectedEqptId);
                            Popup.Closed += (s, e) =>
                            {
                                if (Popup.DialogResult.HasValue && Popup.DialogResult.Value)
                                    RefreshData(Popup.RSLT.EQPTID, selectedEqptId);
                            };
                            Popup.Show();

                            IsBusy = false;
                            ///

                            CommandResults["ChangeWorkRoomCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["ChangeWorkRoomCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ChangeWorkRoomCommandAsync"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ChangeWorkRoomCommandAsync") ?
                        CommandCanExecutes["ChangeWorkRoomCommandAsync"] : (CommandCanExecutes["ChangeWorkRoomCommandAsync"] = true);
                });
            }
        }
        public ICommand ConfirmCommandAsync // Datagird 정보를 XML로 변환하고 창 종료
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ConfirmCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["ConfirmCommandAsync"] = false;
                            CommandCanExecutes["ConfirmCommandAsync"] = false;

                            ///
                            IsBusy = true;

                            if (BR_BRS_SEL_EquipmentStatus_PROCEQPT.OUTDATAs.Count > 0)
                            {
                                // 주간, 월간 점검 확인 로직 추가. 2020.11.20 phd
                                foreach (var item in BR_BRS_SEL_EquipmentStatus_PROCEQPT.OUTDATAs)
                                {
                                    // item.WEEKLYCHKSTATUS, item.MONTHCHKSTATUS의 값이 null일수 있음. null인 이유는 주간점검 혹은 월간점검 액션이 없기 때문.2020.11.20 phd
                                    if ((item.WEEKLYCHKSTATUS != null && item.WEEKLYCHKSTATUS.ToString() == "부적합" ) 
                                    || (item.MONTHCHKSTATUS != null && item.MONTHCHKSTATUS.ToString() == "부적합"))
                                    {
                                        if (item.WEEKLYCHKSTATUS != null && item.WEEKLYCHKSTATUS.ToString() == "부적합")
                                        {
                                            throw new Exception(string.Format("{0}설비 주간 점검을 수행하지 않았습니다.", item.EQPTID));
                                        }
                                        if (item.MONTHCHKSTATUS != null && item.MONTHCHKSTATUS.ToString() == "부적합")
                                        {
                                            throw new Exception(string.Format("{0}설비 월간 점검을 수행하지 않았습니다.", item.EQPTID));
                                        }
                                    }
                                }
                                    var authHelper = new iPharmAuthCommandHelper();

                                // Phase 종료후 값 수정 시 전자서명
                                if (_mainWnd.CurrentInstruction.Raw.INSERTEDYN.Equals("Y") && _mainWnd.Phase.CurrentPhase.STATE.Equals("COMP"))
                                {
                                    // 전자서명 요청
                                    authHelper = new iPharmAuthCommandHelper();
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

                                // 생산설비청소점검 기록 전자서명
                                authHelper = new iPharmAuthCommandHelper();
                                authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");
                                if (await authHelper.ClickAsync(
                                    Common.enumCertificationType.Role,
                                    Common.enumAccessType.Create,
                                    "생산설비청소점검",
                                    "생산설비청소점검",
                                    false,
                                    "OM_ProductionOrder_SUI",
                                    "",
                                    null, null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }

                                // XML 변환
                                var ds = new DataSet();
                                var dt = new DataTable("DATA");
                                ds.Tables.Add(dt);
                                //2021.08.24 박희돈 EBR작업으로 인한 설비명 -> 장비 명, 설비코드 -> 장비 번호 로 변경.
                                dt.Columns.Add(new DataColumn("장비번호"));
                                dt.Columns.Add(new DataColumn("장비명"));
                                dt.Columns.Add(new DataColumn("이전제품명"));
                                dt.Columns.Add(new DataColumn("이전제조번호"));
                                dt.Columns.Add(new DataColumn("점검상태"));
                                dt.Columns.Add(new DataColumn("교정상태"));
                                dt.Columns.Add(new DataColumn("청소상태"));
                                dt.Columns.Add(new DataColumn("청소완료일시"));
                                dt.Columns.Add(new DataColumn("청소유효일시"));
                                dt.Columns.Add(new DataColumn("SOP문서번호"));

                                foreach (var item in BR_BRS_SEL_EquipmentStatus_PROCEQPT.OUTDATAs)
                                {
                                    var row = dt.NewRow();
                                    //2021.08.24 박희돈 EBR작업으로 인한 설비명 -> 장비 명, 설비코드 -> 장비 번호 로 변경.
                                    row["장비번호"] = item.EQPTID != null ? item.EQPTID : "";
                                    row["장비명"] = item.EQPTNAME != null ? item.EQPTNAME : "";
                                    row["이전제품명"] = item.MTRLNAME != null ? item.MTRLNAME : "";
                                    row["이전제조번호"] = item.BATCHNO != null ? item.BATCHNO : "";
                                    row["점검상태"] = item.SCALEDAILYSTATUS != null ? item.SCALEDAILYSTATUS : "";
                                    row["교정상태"] = item.CALIBRATIONSTATUS != null ? item.CALIBRATIONSTATUS : "";
                                    row["청소상태"] = item.STATUS != null ? item.STATUS : "";
                                    row["청소완료일시"] = item.CLEANDTTM != null ? item.CLEANDTTM : "";
                                    row["청소유효일시"] = item.EXPIREDTTM != null ? item.EXPIREDTTM : "";
                                    row["SOP문서번호"] = item.SOPDOC ?? "";

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

                            IsBusy = false;
                            ///

                            CommandResults["ConfirmCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["ConfirmCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ConfirmCommandAsync"] = true;
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
        private void CheckRecordFlag()
        {
            if (BR_BRS_SEL_EquipmentStatus_PROCEQPT != null && BR_BRS_SEL_EquipmentStatus_PROCEQPT.OUTDATAs.Count > 0)
            {
                CANRECORDFLAG = true;
                foreach (var item in BR_BRS_SEL_EquipmentStatus_PROCEQPT.OUTDATAs)
                {
                    if (item.AVAILFLAG == "N")
                    {
                        CANRECORDFLAG = false;
                        break;
                    }
                }
            }
        }
        private async void RefreshData(string prevEQPTID, string curEQPTID)
        {
            BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.INDATAs.Clear();
            BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs.Clear();

            BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.INDATAs.Add(new BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.INDATA
            {
                LANGID = "ko-KR",
                EQPTID = prevEQPTID,
                POID = _mainWnd.CurrentOrder.ProductionOrderID,
                BATCHNO = _mainWnd.CurrentOrder.BatchNo
            });

            if (await BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.Execute() == true && BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs.Count > 0)
            {
                foreach (var item in BR_BRS_SEL_EquipmentStatus_PROCEQPT.OUTDATAs)
                {
                    if (item.EQPTID == curEQPTID)
                    {
                        item.EQPTID = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].EQPTID;
                        item.EQPTNAME = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].EQPTNAME;
                        item.BATCHNO = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].BATCHNO;
                        item.MTRLNAME = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].MTRLNAME;
                        item.CHECKED = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].CHECKED;
                        item.ISCURRENT = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].ISCURRENT;
                        item.ATTACHGUID = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].ATTACHGUID;
                        item.FILENAME = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].FILENAME;
                        item.STATUS = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].STATUS;
                        item.CLEANINGSTATUS = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].CLEANINGSTATUS;
                        item.CLEANDTTM = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].CLEANDTTM;
                        item.CLEANAVAILFLAG = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].CLEANAVAILFLAG;
                        item.PROCAVAILFLAG = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].PROCAVAILFLAG;
                        item.EXPIREDTTM = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].EXPIREDTTM;
                        item.AVAILFLAG = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].AVAILFLAG;
                        item.CHECKEDDTTM = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].CHECKEDDTTM;
                        item.CLEANING_ONTIME = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].CLEANING_ONTIME;
                        item.VALIDFLAG = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].VALIDFLAG;
                        item.SCALEDAILYSTATUS = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].SCALEDAILYSTATUS;
                        item.WEEKLYCHKSTATUS = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].WEEKLYCHKSTATUS;
                        item.MONTHCHKSTATUS = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].MONTHCHKSTATUS;
                        item.CALIBRATIONSTATUS = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].CALIBRATIONSTATUS;
                        item.CALIBATIONDTTM = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].CALIBATIONDTTM;
                        item.NEXTCALIBATIONDTTM = BR_BRS_SEL_EquipmentStatus_CheckCleaned_Multi.OUTDATAs[0].NEXTCALIBATIONDTTM;
                    }
                }
            }
        }
        #endregion
    }
}