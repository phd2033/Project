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
using System.Collections.Generic;
using ShopFloorUI;
using C1.Silverlight.Data;
using System.Linq;


namespace 보령
{
    public class 타정손실수율ViewModel : ViewModelBase
    {
        #region [Property]
        private 타정손실수율 _mainWnd;

        private List<LossInfo> _TabletingInfoList;
        public List<LossInfo> TabletingInfoList
        {
            get { return _TabletingInfoList; }
            set
            {
                _TabletingInfoList = value;
                OnPropertyChanged("TabletingInfoList");
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

                            if (arg != null && arg is 타정손실수율)
                            {
                                _mainWnd = arg as 타정손실수율;
                                
                                _TabletingInfoList.Add(new LossInfo
                                {
                                    GUBUN = "Layer 1 (ATV 층)",
                                    InitSettingLoss = 0,
                                    inspectionLoss = 0,
                                    RemainingAmountLoss = 0,
                                    BadTabletsLoss = 0,
                                    UnknownCauseLoss = 0,
                                    CurrentCollectorLoss = 0
                                });
                                _TabletingInfoList.Add(new LossInfo
                                {
                                    GUBUN = "Layer 2 (FMS 층)",
                                    InitSettingLoss = 0,
                                    inspectionLoss = 0,
                                    RemainingAmountLoss = 0,
                                    BadTabletsLoss = 0,
                                    UnknownCauseLoss = 0,
                                    CurrentCollectorLoss = 0
                                });

                                _TabletingInfoList.Add(new LossInfo
                                {
                                    GUBUN = "합계",
                                    InitSettingLoss = 0,
                                    inspectionLoss = 0,
                                    RemainingAmountLoss = 0,
                                    BadTabletsLoss = 0,
                                    UnknownCauseLoss = 0,
                                    CurrentCollectorLoss = 0
                                });


                                //_mainWnd.gridTabletingInfo.ItemsSource = null;
                                //_mainWnd.gridTabletingInfo.ItemsSource = this.TabletingInfoList;


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

                            if (_TabletingInfoList != null && _TabletingInfoList.Count > 0)
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

                                dt.Columns.Add(new DataColumn("GUBUN"));
                                dt.Columns.Add(new DataColumn("InitSettingLoss"));
                                dt.Columns.Add(new DataColumn("inspectionLoss"));
                                dt.Columns.Add(new DataColumn("RemainingAmountLoss"));
                                dt.Columns.Add(new DataColumn("BadTabletsLoss"));
                                dt.Columns.Add(new DataColumn("UnknownCauseLoss"));
                                dt.Columns.Add(new DataColumn("CurrentCollectorLoss"));
                                dt.Columns.Add(new DataColumn("REMAIN"));
                                dt.Columns.Add(new DataColumn("USING"));

                                foreach (var item in _TabletingInfoList)
                                {
                                    var row = dt.NewRow();

                                    row["GUBUN"] = "";
                                    row["InitSettingLoss"] = 0;
                                    row["inspectionLoss"] = 0;
                                    row["RemainingAmountLoss"] = 0;
                                    row["BadTabletsLoss"] = 0;
                                    row["UnknownCauseLoss"] = 0;
                                    row["CurrentCollectorLoss"] = 0;

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
        public 타정손실수율ViewModel()
        {
            _TabletingInfoList = new List<LossInfo>();
        }
        #endregion

        #region [User Define]
        public class LossInfo : ViewModelBase
        {
            private string _GUBUN;
            public string GUBUN
            {
                get { return _GUBUN; }
                set
                {
                    _GUBUN = value;
                    OnPropertyChanged("GUBUN");
                }
            }
            private decimal _InitSettingLoss;
            public decimal InitSettingLoss
            {
                get { return _InitSettingLoss; }
                set
                {
                    _InitSettingLoss = value;
                    OnPropertyChanged("InitSettingLoss");
                }
            }
            private decimal _inspectionLoss;
            public decimal inspectionLoss
            {
                get { return _inspectionLoss; }
                set
                {
                    _inspectionLoss = value;
                    OnPropertyChanged("inspectionLoss");
                }
            }
            private decimal _RemainingAmountLoss;
            public decimal RemainingAmountLoss
            {
                get { return _RemainingAmountLoss; }
                set
                {
                    _RemainingAmountLoss = value;
                    OnPropertyChanged("RemainingAmountLoss");
                }
            }
            private decimal _BadTabletsLoss;
            public decimal BadTabletsLoss
            {
                get { return _BadTabletsLoss; }
                set
                {
                    _BadTabletsLoss = value;
                    OnPropertyChanged("BadTabletsLoss");
                }
            }
            private decimal _UnknownCauseLoss;
            public decimal UnknownCauseLoss
            {
                get { return _UnknownCauseLoss; }
                set
                {
                    _UnknownCauseLoss = value;
                    OnPropertyChanged("UnknownCauseLoss");
                }
            }
            private decimal _CurrentCollectorLoss;
            public decimal CurrentCollectorLoss
            {
                get { return _CurrentCollectorLoss; }
                set
                {
                    _CurrentCollectorLoss = value;
                    OnPropertyChanged("CurrentCollectorLoss");
                }
            }
        }

        public void ConvertResult()
        {

            LossInfo output = new LossInfo();

            foreach (var item in _TabletingInfoList)
            {
                output = item;
                Calculation(null, output);
            }
        }
        public void Calculation(LossInfo IN, LossInfo OUT)
        {
            //OUT.SCRAP = OUT.PICKING + OUT.ADDING - (OUT.SAMPLE + OUT.REMAIN + OUT.USING);
        }
        private bool CheckDataSet()
        {
            //foreach (var item in _PackingInfoList)
            //{
            //    if (item.SCRAP != item.PICKING + item.ADDING - (item.SAMPLE + item.REMAIN + item.USING))
            //        return true;

            //    if (item.PICKING < 0 || item.ADDING < 0 || item.SCRAP < 0 || item.SAMPLE < 0 || item.SAMPLE < 0 || item.REMAIN < 0 || item.USING < 0)
            //        return true;
            //}

            return false;
        }
        #endregion
    }
}
