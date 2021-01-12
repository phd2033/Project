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

namespace 보령
{
    public class 포장자재출고기록ViewModel : ViewModelBase
    {
        #region [Property]
        private 포장자재출고기록 _mainWnd;
        #endregion
        #region [BizRule]
        private BR_BRS_SEL_ProductionOrder_Component_PickingInfo _BR_BRS_SEL_ProductionOrder_Component_PickingInfo;
        public BR_BRS_SEL_ProductionOrder_Component_PickingInfo BR_BRS_SEL_ProductionOrder_Component_PickingInfo
        {
            get { return _BR_BRS_SEL_ProductionOrder_Component_PickingInfo; }
            set
            {
                _BR_BRS_SEL_ProductionOrder_Component_PickingInfo = value;
                OnPropertyChanged("BR_BRS_SEL_ProductionOrder_Component_PickingInfo");
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

                            if (arg != null && arg is 포장자재출고기록)
                            {
                                _mainWnd = arg as 포장자재출고기록;

                                _BR_BRS_SEL_ProductionOrder_Component_PickingInfo.INDATAs.Clear();
                                _BR_BRS_SEL_ProductionOrder_Component_PickingInfo.OUTDATAs.Clear();

                                _BR_BRS_SEL_ProductionOrder_Component_PickingInfo.INDATAs.Add(new BR_BRS_SEL_ProductionOrder_Component_PickingInfo.INDATA
                                        {
                                            POID = _mainWnd.CurrentOrder.ProductionOrderID != null ? _mainWnd.CurrentOrder.ProductionOrderID : "",
                                            OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID != null ? _mainWnd.CurrentOrder.OrderProcessSegmentID : ""                                            
                                        });

                                //var inputValues = InstructionModel.GetParameterSender(_mainWnd.CurrentInstruction, _mainWnd.Instructions);

                                //if (inputValues.Count > 0)
                                //{
                                //    decimal temp;

                                //    foreach (var item in inputValues)
                                //    {
                                //        _BR_BRS_SEL_ProductionOrder_Component_PickingInfo.INDATAs.Add(new BR_BRS_SEL_ProductionOrder_Component_PickingInfo.INDATA
                                //        {
                                //            POID = _mainWnd.CurrentOrder.ProductionOrderID != null ? _mainWnd.CurrentOrder.ProductionOrderID : "",
                                //            OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID != null ? _mainWnd.CurrentOrder.OrderProcessSegmentID : "",
                                //            CHGSEQ = decimal.TryParse(item.Raw.EXPRESSION, out temp) ? temp : 0m,
                                //            MTRLID = item.Raw.BOMID != null ? item.Raw.BOMID : ""
                                //        });
                                //    }
                                //}
                                //else
                                //{
                                //    _BR_BRS_SEL_ProductionOrder_Component_PickingInfo.INDATAs.Add(new BR_BRS_SEL_ProductionOrder_Component_PickingInfo.INDATA
                                //    {
                                //        POID = _mainWnd.CurrentOrder.ProductionOrderID != null ? _mainWnd.CurrentOrder.ProductionOrderID : "",
                                //        OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID != null ? _mainWnd.CurrentOrder.OrderProcessSegmentID : "",
                                //        CHGSEQ = _mainWnd.CurrentInstruction.Raw.EXPRESSION != null ? Convert.ToDecimal(_mainWnd.CurrentInstruction.Raw.EXPRESSION) : 0m,
                                //        MTRLID = _mainWnd.CurrentInstruction.Raw.BOMID != null ? _mainWnd.CurrentInstruction.Raw.BOMID : ""
                                //    });
                                //}
                               
                                await _BR_BRS_SEL_ProductionOrder_Component_PickingInfo.Execute();
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
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadedCommandAsync") ?
                        CommandCanExecutes["LoadedCommandAsync"] : (CommandCanExecutes["LoadedCommandAsync"] = true);
                });
            }
        }

        public ICommand ComfirmCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ComfirmCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandCanExecutes["ComfirmCommandAsync"] = false;
                            CommandResults["ComfirmCommandAsync"] = false;

                            ///
                            if (_BR_BRS_SEL_ProductionOrder_Component_PickingInfo != null && _BR_BRS_SEL_ProductionOrder_Component_PickingInfo.OUTDATAs.Count > 0)
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
                                    "포장자재출고기록",
                                    "포장자재출고기록",
                                    false,
                                    "OM_ProductionOrder_SUI",
                                    "", null, null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }

                                string ACQUIREUSERID = AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_SUI");
                                DateTime ACQUIREDTTM = await AuthRepositoryViewModel.GetDBDateTimeNow();

                                var ds = new DataSet();
                                var dt = new DataTable("DATA");

                                ds.Tables.Add(dt);

                                dt.Columns.Add(new DataColumn("MTRLID"));
                                dt.Columns.Add(new DataColumn("MTRLNAME"));
                                dt.Columns.Add(new DataColumn("STDQTY"));
                                dt.Columns.Add(new DataColumn("MTLOTID"));
                                dt.Columns.Add(new DataColumn("ADJUSTQTY"));
                                dt.Columns.Add(new DataColumn("PICKINGQTY"));
                                dt.Columns.Add(new DataColumn("ADDQTY"));
                                dt.Columns.Add(new DataColumn("HANDOVERUSERID"));
                                dt.Columns.Add(new DataColumn("HANDOVERDTTM"));
                                dt.Columns.Add(new DataColumn("ACQUIREUSERID"));
                                dt.Columns.Add(new DataColumn("ACQUIREDTTM"));

                                foreach (var item in _BR_BRS_SEL_ProductionOrder_Component_PickingInfo.OUTDATAs)
                                {
                                    var row = dt.NewRow();

                                    row["MTRLID"] = item.MTRLID != null ? item.MTRLID : "";
                                    row["MTRLNAME"] = item.MTRLNAME != null ? item.MTRLNAME : "";
                                    row["STDQTY"] = item.STD != null ? item.STD : "";
                                    row["MTLOTID"] = item.MLOTID != null ? item.MLOTID : "";
                                    row["ADJUSTQTY"] = item.ADJUSTQTY != null ? item.ADJUSTQTY : "";
                                    row["PICKINGQTY"] = item.PICKINGQTY != null ? item.PICKINGQTY : "";
                                    row["ADDQTY"] = item.ADDQTY != null ? item.ADDQTY : "";
                                    row["HANDOVERUSERID"] = item.HANDOVERUSERID != null ? item.HANDOVERUSERID : "";
                                    row["HANDOVERDTTM"] = item.HANDOVERDTTM != null ? item.HANDOVERDTTM : "";
                                    row["ACQUIREUSERID"] = ACQUIREUSERID;
                                    row["ACQUIREDTTM"] = ACQUIREDTTM.ToString("yyyy-MM-dd HH:mm:ss");

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
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadedCommandAsync") ?
                        CommandCanExecutes["LoadedCommandAsync"] : (CommandCanExecutes["LoadedCommandAsync"] = true);
                });
            }
        }
        #endregion
        #region [Constructor]
        public 포장자재출고기록ViewModel()
        {
            _BR_BRS_SEL_ProductionOrder_Component_PickingInfo = new BR_BRS_SEL_ProductionOrder_Component_PickingInfo();
        }
        #endregion
        #region [User Define]
        
        #endregion
    }
}
