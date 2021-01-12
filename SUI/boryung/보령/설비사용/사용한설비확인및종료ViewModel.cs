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
using C1.Silverlight.Data;
using ShopFloorUI;
using System.Collections.Generic;

namespace 보령
{
    public class 사용한설비확인및종료ViewModel : ViewModelBase
    {
        #region [Property]
        private 사용한설비확인및종료 _mainWnd;

        public 사용한설비확인및종료ViewModel()
        {
            _BR_BRS_SEL_EquipmentStatus_PROC_OPSG = new BR_BRS_SEL_EquipmentStatus_PROC_OPSG();
            _BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI = new BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI();
        }
        #endregion
        #region [BizRule]
        // 현재 오더, 공정에서 사용중인 설비목록 조회
        private BR_BRS_SEL_EquipmentStatus_PROC_OPSG _BR_BRS_SEL_EquipmentStatus_PROC_OPSG;
        public BR_BRS_SEL_EquipmentStatus_PROC_OPSG BR_BRS_SEL_EquipmentStatus_PROC_OPSG
        {
            get { return _BR_BRS_SEL_EquipmentStatus_PROC_OPSG; }
            set
            {
                _BR_BRS_SEL_EquipmentStatus_PROC_OPSG = value;
                NotifyPropertyChanged();
            }
        }
        // 설비의 생산완료 및 JOB 종료
        private BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI _BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI;
        public BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI
        {  
            get { return _BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI; }
            set
            {
                _BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI = value;
                NotifyPropertyChanged();
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
                            CommandResults["LoadedCommandAsync"] = false;
                            CommandCanExecutes["LoadedCommandAsync"] = false;

                            ///
                            if (arg == null || !(arg is 사용한설비확인및종료)) return;
                            else
                            {
                                _mainWnd = arg as 사용한설비확인및종료;

                                _BR_BRS_SEL_EquipmentStatus_PROC_OPSG.INDATAs.Clear();
                                _BR_BRS_SEL_EquipmentStatus_PROC_OPSG.OUTDATAs.Clear();

                                _BR_BRS_SEL_EquipmentStatus_PROC_OPSG.INDATAs.Add(new BR_BRS_SEL_EquipmentStatus_PROC_OPSG.INDATA
                                {
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                    ROOMID = AuthRepositoryViewModel.Instance.RoomID
                                });

                                if (await BR_BRS_SEL_EquipmentStatus_PROC_OPSG.Execute() == false)
                                    throw BR_BRS_SEL_EquipmentStatus_PROC_OPSG.Exception;
                            }

                            IsBusy = false;
                            ///

                            CommandResults["LoadedCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["LoadedCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["LoadedCommandAsync"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadedCommandAsync") ?
                        CommandCanExecutes["LoadedCommandAsync"] : (CommandCanExecutes["LoadedCommandAsync"] = true);
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
                            CommandResults["ConfirmCommandAsync"] = false;
                            CommandCanExecutes["ConfirmCommandAsync"] = false;

                            ///
                            IsBusy = true;

                            // 전자서명(기록값 변경)
                            var authHelper = new iPharmAuthCommandHelper();

                            if (_mainWnd.CurrentInstruction.Raw.INSERTEDYN.Equals("Y") && _mainWnd.Phase.CurrentPhase.STATE.Equals("COMP"))
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

                            // 조회내용 기록
                            authHelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "EM_BRS_EquipmentAction_PROCEND");

                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Function,
                                Common.enumAccessType.Create,
                                "사용한설비확인및종료",
                                "사용한설비확인및종료",
                                false,
                                "EM_BRS_EquipmentAction_PROCEND",
                                "", null, null) == false)
                            {
                                throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                            }

                            // XML 변환
                            var ds = new DataSet();
                            var dt = new DataTable("DATA");
                            ds.Tables.Add(dt);

                            dt.Columns.Add(new DataColumn("설비코드"));
                            dt.Columns.Add(new DataColumn("설비명"));

                            // XML 기록 및 설비사용종료 비즈룰 호출
                            string userid = AuthRepositoryViewModel.GetUserIDByFunctionCode("EM_BRS_EquipmentAction_PROCEND");
                            userid = !string.IsNullOrWhiteSpace(userid) ? userid : AuthRepositoryViewModel.Instance.LoginedUserID;

                            _BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI.INDATAs.Clear();
                            _BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI.PARAMDATAs.Clear();

                            foreach (var item in _BR_BRS_SEL_EquipmentStatus_PROC_OPSG.OUTDATAs)
                            {
                                if (item.SELFLAG)
                                {
                                    _BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI.INDATAs.Add(new BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI.INDATA
                                    {
                                        LANGID = AuthRepositoryViewModel.Instance.LangID,
                                        USER = userid,
                                        EQPTID = item.EQPTID,
                                        ROOMNO = AuthRepositoryViewModel.Instance.RoomID
                                    });

                                    var row = dt.NewRow();

                                    row["설비코드"] = item.EQPTID ?? "";
                                    row["설비명"] = item.EQPTNAME ?? "";

                                    dt.Rows.Add(row);
                                }
                            }

                            bool BRExecuteFlag = false; // 설비사용종료 비즈룰이 실행된 경우 true
                            if (_BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI.INDATAs.Count > 0)
                            {
                                if (await _BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI.Execute() == true)
                                    BRExecuteFlag = true;
                                else
                                    throw _BR_BRS_UPD_EquipmentAction_ShopFloor_PROCEND_MULTI.Exception;
                            }

                            var xml = BizActorRuleBase.CreateXMLStream(ds);
                            var bytesArray = System.Text.Encoding.UTF8.GetBytes(xml);

                            _mainWnd.CurrentInstruction.Raw.ACTVAL = BRExecuteFlag ? _mainWnd.TableTypeName : "종료한 설비가 없습니다.";
                            _mainWnd.CurrentInstruction.Raw.NOTE = BRExecuteFlag ? bytesArray : null;

                            var result = await _mainWnd.Phase.RegistInstructionValue(_mainWnd.CurrentInstruction, true);
                            if (result != enumInstructionRegistErrorType.Ok)
                            {
                                throw new Exception(string.Format("값 등록 실패, ID={0}, 사유={1}", _mainWnd.CurrentInstruction.Raw.IRTGUID, result));
                            }

                            if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                            else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);

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
    }
}