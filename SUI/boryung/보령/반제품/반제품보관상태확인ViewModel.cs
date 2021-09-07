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
using ShopFloorUI;
using C1.Silverlight.Data;

namespace 보령
{
    public class 반제품보관상태확인ViewModel : ViewModelBase
    {
        #region [Property]
        public 반제품보관상태확인ViewModel()
        {
            _BR_BRS_SEL_ProductionOrderOutput_State = new BR_BRS_SEL_ProductionOrderOutput_State();
        }

        반제품보관상태확인 _mainWnd;
        
        #endregion

        #region [Bizrule]

        private BR_BRS_SEL_ProductionOrderOutput_State _BR_BRS_SEL_ProductionOrderOutput_State;
        public BR_BRS_SEL_ProductionOrderOutput_State BR_BRS_SEL_ProductionOrderOutput_State 
        {
            get { return _BR_BRS_SEL_ProductionOrderOutput_State; }
            set
            {
                _BR_BRS_SEL_ProductionOrderOutput_State = value;
                OnPropertyChanged("BR_BRS_SEL_ProductionOrderOutput_State ");
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
                    using (await AwaitableLocks["LoadedCommand"].EnterAsync())
                    {
                        try
                        {
                            CommandCanExecutes["LoadedCommand"] = false;
                            CommandResults["LoadedCommand"] = false;
                            IsBusy = true;

                            if (arg != null)
                            {
                                _mainWnd = arg as 반제품보관상태확인;

                                _BR_BRS_SEL_ProductionOrderOutput_State.INDATAs.Clear();
                                _BR_BRS_SEL_ProductionOrderOutput_State.OUTDATAs.Clear();
                                _BR_BRS_SEL_ProductionOrderOutput_State.INDATAs.Add(new BR_BRS_SEL_ProductionOrderOutput_State.INDATA
                                {
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID
                                });

                                await _BR_BRS_SEL_ProductionOrderOutput_State.Execute();
                            }
                            CommandResults["LoadedCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandCanExecutes["LoadedCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["LoadedCommand"] = true;
                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadedCommand") ?
                        CommandCanExecutes["LoadedCommand"] : (CommandCanExecutes["LoadedCommand"] = true);
                });
            }
        }

        public ICommand ConfirmCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ConfirmCommand"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["ConfirmCommand"] = false;
                            CommandCanExecutes["ConfirmCommand"] = false;
                            IsBusy = true;

                            if (_BR_BRS_SEL_ProductionOrderOutput_State.OUTDATAs.Count == 0)
                            {
                                OnMessage("기록할 데이터가 없습니다.");
                                return;
                            }

                            iPharmAuthCommandHelper authHelper = new iPharmAuthCommandHelper();

                            if (_mainWnd.CurrentInstruction.Raw.INSERTEDYN.Equals("Y") && _mainWnd.Phase.CurrentPhase.STATE.Equals("COMP")) // 값 수정
                            {
                                // 전자서명 요청
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

                            authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");
                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Role,
                                Common.enumAccessType.Create,
                                "반제품보관상태확인",
                                "반제품보관상태확인",
                                false,
                                "OM_ProductionOrder_SUI",
                                "",
                                null, null) == false)
                            {
                                throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                            }

                            var ds = new DataSet();
                            var dt = new DataTable("DATA");
                            ds.Tables.Add(dt);

                            dt.Columns.Add(new DataColumn("공정명"));
                            dt.Columns.Add(new DataColumn("관리번호"));
                            dt.Columns.Add(new DataColumn("보관기간기준"));
                            dt.Columns.Add(new DataColumn("보관기간"));
                            dt.Columns.Add(new DataColumn("일탈여부"));

                            if (_BR_BRS_SEL_ProductionOrderOutput_State.OUTDATAs.Count > 0)
                            {
                                foreach (var item in _BR_BRS_SEL_ProductionOrderOutput_State.OUTDATAs)
                                {
                                    var row = dt.NewRow();

                                    row["공정명"] = item.OPSGNAME ?? "";
                                    row["관리번호"] = item.VESSELNAME ?? "";
                                    row["보관기간기준"] = item.STRGSTANDARD ?? "";
                                    row["보관기간"] = item.STRGDATE ?? "";
                                    row["일탈여부"] = item.DEVIATIONYN ?? "";

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

                            CommandResults["ConfirmCommand"] = false;
                        }
                        catch (Exception ex)
                        {
                            CommandCanExecutes["ConfirmCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ConfirmCommand"] = true;
                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ConfirmCommand") ?
                        CommandCanExecutes["ConfirmCommand"] : (CommandCanExecutes["ConfirmCommand"] = true);
                });
            }
        }

        #endregion
    }
}
