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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ShopFloorUI;
using C1.Silverlight.Data;
using System.Linq;

namespace 보령
{
    public class 포장자재정산ViewModel : ViewModelBase
    {
        #region [Property]
        private 포장자재정산 _mainWnd;

        private List<PackingInfo> _PackingInfoList;
        public List<PackingInfo> PackingInfoList
        {
            get { return _PackingInfoList; }
            set
            {
                _PackingInfoList = value;
                OnPropertyChanged("PackingInfoList");
            }
        }
        #endregion

        #region [BizRule]
        private BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New _BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New;
        public BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New
        {
            get { return _BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New; }
            set
            {
                _BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New = value;
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
                            CommandCanExecutes["LoadedCommandAsync"] = false;
                            CommandResults["LoadedCommandAsync"] = false;

                            ///
                            decimal temp = 0m; // tryparse

                            if (arg != null && arg is 포장자재정산)
                            {
                                _mainWnd = arg as 포장자재정산;

                                var inputValues = InstructionModel.GetParameterSender(_mainWnd.CurrentInstruction, _mainWnd.Instructions);

                                if (inputValues == null)
                                    return;

                                _BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New.INDATAs.Clear();
                                _BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New.OUTDATAs.Clear();

                                foreach (var item in inputValues)
                                {
                                    _BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New.INDATAs.Add(new BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New.INDATA
                                    {
                                        POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                        OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                        MTRLID = item.Raw.BOMID,
                                        CHGSEQ = Convert.ToDecimal(item.Raw.EXPRESSION)
                                    });
                                }

                                if (await _BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New.Execute() != false)
                                {
                                    foreach (var item in _BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New.OUTDATAs)
                                    {
                                        _PackingInfoList.Add(new PackingInfo
                                        {
                                            MTRLID = item.MTRLID != null ? item.MTRLID : "",
                                            MTRLNAME = item.MTRLNAME != null ? item.MTRLNAME : "",
                                            UOM = item.UOM != null ? item.UOM : "",
                                            PICKING = decimal.TryParse(item.TOTALPICKINGQTY, out temp) ? temp : 0m,
                                            ADDING = decimal.TryParse(item.ADDITIONALQTY, out temp) ? temp : 0m,
                                            SCRAP = 0m,
                                            SAMPLE = 0m,
                                            REMAIN = decimal.TryParse(item.RETURNQTY, out temp) ? temp : 0m,
                                            USING = 0m,
                                            Param = decimal.TryParse(inputValues.FirstOrDefault(x => x.Raw.BOMID.Equals(item.MTRLID)).Raw.TARGETVAL, out temp) ? temp : 1m,
                                        });
                                    }
                                }

                                _mainWnd.gridPackingInfo.ItemsSource = null;
                                _mainWnd.gridPackingInfo.ItemsSource = this.PackingInfoList;
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

                            if (_PackingInfoList != null && _PackingInfoList.Count > 0)
                            {
                                if (CheckDataSet())
                                    return;

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

                                var ds = new DataSet();
                                var dt = new DataTable("DATA");

                                ds.Tables.Add(dt);

                                dt.Columns.Add(new DataColumn("MTRLID"));
                                dt.Columns.Add(new DataColumn("MTRLNAME"));
                                dt.Columns.Add(new DataColumn("UOM"));
                                dt.Columns.Add(new DataColumn("PICKING"));
                                dt.Columns.Add(new DataColumn("ADDING"));
                                dt.Columns.Add(new DataColumn("SCRAP"));
                                dt.Columns.Add(new DataColumn("SAMPLE"));
                                dt.Columns.Add(new DataColumn("REMAIN"));
                                dt.Columns.Add(new DataColumn("USING"));

                                foreach (var item in _PackingInfoList)
                                {
                                    var row = dt.NewRow();

                                    row["MTRLID"] = item.MTRLID != null ? item.MTRLID : "";
                                    row["MTRLNAME"] = item.MTRLNAME != null ? item.MTRLNAME : "";
                                    row["UOM"] = item.UOM != null ? item.UOM : "";
                                    row["PICKING"] = item.PICKING.ToString("#0.00#");
                                    row["ADDING"] = item.ADDING.ToString("#0.00#");
                                    row["SCRAP"] = item.SCRAP.ToString("#0.00#");
                                    row["SAMPLE"] = item.SAMPLE.ToString("#0.00#");
                                    row["REMAIN"] = item.REMAIN.ToString("#0.00#");
                                    row["USING"] = item.USING.ToString("#0.00#");

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
        public 포장자재정산ViewModel()
        {
            _BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New = new BR_BRS_SEL_ProductionOrderBOM_PickingInfo_New();
            _PackingInfoList = new List<PackingInfo>();
        }
        #endregion

        #region [User Define]
        public class PackingInfo : ViewModelBase
        {
            private string _MTRLID;
            public string MTRLID
            {
                get { return _MTRLID; }
                set
                {
                    _MTRLID = value;
                    OnPropertyChanged("MTRLID");
                }
            }
            private string _MTRLNAME;
            public string MTRLNAME
            {
                get { return _MTRLNAME; }
                set
                {
                    _MTRLNAME = value;
                    OnPropertyChanged("MTRLNAME");
                }
            }
            private string _UOM;
            public string UOM
            {
                get { return _UOM; }
                set
                {
                    _UOM = value;
                    OnPropertyChanged("UOM");
                }
            }
            private decimal _PICKING;
            public decimal PICKING
            {
                get { return _PICKING; }
                set
                {
                    _PICKING = value;
                    OnPropertyChanged("PICKING");
                }
            }
            private decimal _ADDING;
            public decimal ADDING
            {
                get { return _ADDING; }
                set
                {
                    _ADDING = value;
                    OnPropertyChanged("ADDING");
                }
            }
            private decimal _SCRAP;
            public decimal SCRAP
            {
                get { return _SCRAP; }
                set
                {
                    _SCRAP = value;
                    OnPropertyChanged("SCRAP");
                }
            }
            private decimal _SAMPLE;
            public decimal SAMPLE
            {
                get { return _SAMPLE; }
                set
                {
                    _SAMPLE = value;
                    OnPropertyChanged("SAMPLE");
                }
            }
            private decimal _REMAIN;
            public decimal REMAIN
            {
                get { return _REMAIN; }
                set
                {
                    _REMAIN = value;
                    OnPropertyChanged("REMAIN");
                }
            }
            private decimal _USING;
            public decimal USING
            {
                get { return _USING; }
                set
                {
                    _USING = value;
                    OnPropertyChanged("USING");
                }
            }
            private decimal _Param;
            public decimal Param
            {
                get { return _Param; }
                set
                {
                    _Param = value;
                    OnPropertyChanged("Param");
                }
            }
        }

        public void ConvertResult()
        {

            PackingInfo output = new PackingInfo();

            foreach (var item in _PackingInfoList)
            {
                output = item;
                Calculation(null, output);
            }
            //int inx = 0;
            //PackingInfo input = new PackingInfo();
            //PackingInfo output = new PackingInfo();

            //Calculation(input, output);

            //if (_PackingInfoList.Count % 2 == 0)
            //{
            //    foreach (var item in _PackingInfoList)
            //    {
            //        if (inx % 2 == 1)
            //        {
            //            output = item;

            //            Calculation(input, output);
            //        }
            //        else
            //            input = item;

            //        inx++;
            //    }
            //}
        }
        public void Calculation(PackingInfo IN, PackingInfo OUT)
        {
            //if(IN.Param > 0)
            //{
            //OUT.SAMPLE = IN.SAMPLE * IN.Param;
            //OUT.USING = IN.USING * IN.Param;
            OUT.SCRAP = OUT.PICKING + OUT.ADDING - (OUT.SAMPLE + OUT.REMAIN + OUT.USING);
            //}
        }
        private bool CheckDataSet()
        {
            //int idx = 1;
            foreach (var item in _PackingInfoList)
            {
                //if (idx % 2 == 0)
                //{
                //    if (item.SCRAP != item.PICKING + item.ADDING - (item.SAMPLE + item.REMAIN + item.USING))
                //        return true;
                //}
                if (item.SCRAP != item.PICKING + item.ADDING - (item.SAMPLE + item.REMAIN + item.USING))
                    return true;

                if (item.PICKING < 0 || item.ADDING < 0 || item.SCRAP < 0 || item.SAMPLE < 0 || item.SAMPLE < 0 || item.REMAIN < 0 || item.USING < 0)
                    return true;

                //idx++;
            }

            return false;
        }
        #endregion
    }
}
