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

        private decimal _InitSettingLoss1;
        public decimal InitSettingLoss1
        {
            get { return _InitSettingLoss1; }
            set
            {
                _InitSettingLoss1 = value;
                InitSettingLossSum =  SumFunction(_InitSettingLoss1, _InitSettingLoss2);
                //OnPropertyChanged("InitSettingLoss3");
            }
        }

        private decimal _InitSettingLoss2;
        public decimal InitSettingLoss2
        {
            get { return _InitSettingLoss2; }
            set
            {
                _InitSettingLoss2 = value;
                InitSettingLossSum = SumFunction(_InitSettingLoss1, _InitSettingLoss2);
                //OnPropertyChanged("InitSettingLoss3");
            }
        }

        private decimal _InitSettingLossSum;
        public decimal InitSettingLossSum
        {
            get { return _InitSettingLossSum; }
            set
            {
                _InitSettingLossSum = value;
                TotalLossSum = _InitSettingLossSum + _inspectionLossSum + _RemainingAmountLossSum + _BadTabletsLossSum + _UnknownCauseLossSum + _CurrentCollectorLossSum;
                OnPropertyChanged("InitSettingLossSum");
            }
        }

        private decimal _inspectionLoss1;
        public decimal inspectionLoss1
        {
            get { return _inspectionLoss1; }
            set
            {
                _inspectionLoss1 = value;
                inspectionLossSum = SumFunction(_inspectionLoss1, _inspectionLoss2);
            }
        }

        private decimal _inspectionLoss2;
        public decimal inspectionLoss2
        {
            get { return _inspectionLoss2; }
            set
            {
                _inspectionLoss2 = value;
                inspectionLossSum = SumFunction(_inspectionLoss1, _inspectionLoss2);
            }
        }

        private decimal _inspectionLossSum;
        public decimal inspectionLossSum
        {
            get { return _inspectionLossSum; }
            set
            {
                _inspectionLossSum = value;
                //TotalLossSum = SumFunction(_TotalLossSum, _inspectionLossSum);
                TotalLossSum = _InitSettingLossSum + _inspectionLossSum + _RemainingAmountLossSum + _BadTabletsLossSum + _UnknownCauseLossSum + _CurrentCollectorLossSum;
                OnPropertyChanged("inspectionLossSum");
            }
        }
        

        private decimal _RemainingAmountLoss1;
        public decimal RemainingAmountLoss1
        {
            get { return _RemainingAmountLoss1; }
            set
            {
                _RemainingAmountLoss1 = value;
                RemainingAmountLossSum = SumFunction(_RemainingAmountLoss1, _RemainingAmountLoss2);
            }
        }

        private decimal _RemainingAmountLoss2;
        public decimal RemainingAmountLoss2
        {
            get { return _RemainingAmountLoss2; }
            set
            {
                _RemainingAmountLoss2 = value;
                RemainingAmountLossSum = SumFunction(_RemainingAmountLoss1, _RemainingAmountLoss2);
            }
        }

        private decimal _RemainingAmountLossSum;
        public decimal RemainingAmountLossSum
        {
            get { return _RemainingAmountLossSum; }
            set
            {
                _RemainingAmountLossSum = value;
                //TotalLossSum = SumFunction(_TotalLossSum, _RemainingAmountLossSum);
                TotalLossSum = _InitSettingLossSum + _inspectionLossSum + _RemainingAmountLossSum + _BadTabletsLossSum + _UnknownCauseLossSum + _CurrentCollectorLossSum;
                OnPropertyChanged("RemainingAmountLossSum");
            }
        }

        private decimal _BadTabletsLoss1;
        public decimal BadTabletsLoss1
        {
            get { return _BadTabletsLoss1; }
            set
            {
                _BadTabletsLoss1 = value;
                BadTabletsLossSum = SumFunction(_BadTabletsLoss1, _BadTabletsLoss2);
            }
        }

        private decimal _BadTabletsLoss2;
        public decimal BadTabletsLoss2
        {
            get { return _BadTabletsLoss2; }
            set
            {
                _BadTabletsLoss2 = value;
                BadTabletsLossSum = SumFunction(_BadTabletsLoss1, _BadTabletsLoss2);
            }
        }

        private decimal _BadTabletsLossSum;
        public decimal BadTabletsLossSum
        {
            get { return _BadTabletsLossSum; }
            set
            {
                _BadTabletsLossSum = value;
                //TotalLossSum = SumFunction(_TotalLossSum, _BadTabletsLossSum);
                TotalLossSum = _InitSettingLossSum + _inspectionLossSum + _RemainingAmountLossSum + _BadTabletsLossSum + _UnknownCauseLossSum + _CurrentCollectorLossSum;
                OnPropertyChanged("BadTabletsLossSum");
            }
        }
        

        private decimal _UnknownCauseLoss1;
        public decimal UnknownCauseLoss1
        {
            get { return _UnknownCauseLoss1; }
            set
            {
                _UnknownCauseLoss1 = value;
                UnknownCauseLossSum = SumFunction(_UnknownCauseLoss1, _UnknownCauseLoss2);
            }
        }

        private decimal _UnknownCauseLoss2;
        public decimal UnknownCauseLoss2
        {
            get { return _UnknownCauseLoss2; }
            set
            {
                _UnknownCauseLoss2 = value;
                UnknownCauseLossSum = SumFunction(_UnknownCauseLoss1, _UnknownCauseLoss2);
            }
        }

        private decimal _UnknownCauseLossSum;
        public decimal UnknownCauseLossSum
        {
            get { return _UnknownCauseLossSum; }
            set
            {
                _UnknownCauseLossSum = value;
                //TotalLossSum = SumFunction(_TotalLossSum, _UnknownCauseLossSum);
                TotalLossSum = _InitSettingLossSum + _inspectionLossSum + _RemainingAmountLossSum + _BadTabletsLossSum + _UnknownCauseLossSum + _CurrentCollectorLossSum;
                OnPropertyChanged("UnknownCauseLossSum");
            }
        }

        private decimal _CurrentCollectorLoss1;
        public decimal CurrentCollectorLoss1
        {
            get { return _CurrentCollectorLoss1; }
            set
            {
                _CurrentCollectorLoss1 = value;
                CurrentCollectorLossSum = SumFunction(_CurrentCollectorLoss1, 0);
            }
        }
        
        private decimal _CurrentCollectorLossSum;
        public decimal CurrentCollectorLossSum
        {
            get { return _CurrentCollectorLossSum; }
            set
            {
                _CurrentCollectorLossSum = value;
                //TotalLossSum = SumFunction(_TotalLossSum, _CurrentCollectorLossSum);
                TotalLossSum = _InitSettingLossSum + _inspectionLossSum + _RemainingAmountLossSum + _BadTabletsLossSum + _UnknownCauseLossSum + _CurrentCollectorLossSum;
                OnPropertyChanged("CurrentCollectorLossSum");
            }
        }
        
        private string _UnderYield;
        public string UnderYield
        {
            get { return _UnderYield; }
            set
            {
                _UnderYield = value;
                NotifyPropertyChanged();
            }
        }

        private string _OverYield;
        public string OverYield
        {
            get { return _OverYield; }
            set
            {
                _OverYield = value;
                NotifyPropertyChanged();
            }
        }

        private string _REALQTY_T;
        public string REALQTY_T
        {
            get { return _REALQTY_T; }
            set
            {
                _REALQTY_T = value;
                NotifyPropertyChanged();
            }
        }

        private string _REALQTY_G;
        public string REALQTY_G
        {
            get { return _REALQTY_G; }
            set
            {
                _REALQTY_G = value;
                NotifyPropertyChanged();
            }
        }

        private decimal? _TotalYield;
        public decimal? TotalYield
        {
            get { return _TotalYield; }
            set
            {
                _TotalYield = value;
                NotifyPropertyChanged();
            }
        }

        private decimal _TotalLossSum;
        public decimal TotalLossSum
        {
            get { return _TotalLossSum; }
            set
            {
                _TotalLossSum = value;
                OnPropertyChanged("TotalLossSum");
                //NotifyPropertyChanged();
            }
        }

        private string _ArgumentAmount;
        public string ArgumentAmount
        {
            get { return _ArgumentAmount; }
            set
            {
                _ArgumentAmount = value;
                NotifyPropertyChanged();
            }
        }

        BR_BRS_SEL_Yield_Compress_Loss _BR_BRS_SEL_Yield_Compress_Loss;
        public BR_BRS_SEL_Yield_Compress_Loss BR_BRS_SEL_Yield_Compress_Loss
        {
            get { return _BR_BRS_SEL_Yield_Compress_Loss; }
            set { _BR_BRS_SEL_Yield_Compress_Loss = value; }
        }

        private ObservableCollection<LossInfo> _filteredComponents;
        public ObservableCollection<LossInfo> filteredComponents
        {
            get { return _filteredComponents; }
            set
            {
                _filteredComponents = value;
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

                            if (arg != null && arg is 타정손실수율)
                            {
                                _mainWnd = arg as 타정손실수율;


                                _BR_BRS_SEL_Yield_Compress_Loss.INDATAs.Clear();
                                _BR_BRS_SEL_Yield_Compress_Loss.OUTDATAs.Clear();

                                _BR_BRS_SEL_Yield_Compress_Loss.INDATAs.Add(new BR_BRS_SEL_Yield_Compress_Loss.INDATA()
                                {
                                    STD_OPTIONITEM = "SYS_MTATID_COMPRESS_STDWEIGHT",
                                    AVG_OPTIONITEM = "SYS_IPCTEST_ITEMID",
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                    MTRLID = _mainWnd.CurrentOrder.MaterialID,
                                    INSUSER = LGCNS.EZMES.Common.LogInInfo.UserID
                                });

                                await _BR_BRS_SEL_Yield_Compress_Loss.Execute();


                                //outputLabel = "타정 반제품 (kg)  ÷  타정 평균질량  × 1,000,000 (T)";
                                //inputLabel = "후혼합 반제품 (kg)  ÷  타정 기준질량  × 1,000,000 (T)";
                                // 타정공정수율 = ( (타정 반제품 (kg)  ÷  타정 평균질량  × 1,000,000 (T))   / (후혼합 반제품 (kg)  ÷  타정 기준질량  × 1,000,000 (T)) ) * 100                         
                                if (_BR_BRS_SEL_Yield_Compress_Loss.OUTDATAs.Count > 0)
                                {
                                    UnderYield = _BR_BRS_SEL_Yield_Compress_Loss.OUTDATAs[0].UNDERYIELD ?? "N/A";
                                    OverYield = _BR_BRS_SEL_Yield_Compress_Loss.OUTDATAs[0].OVERYIELD ?? "N/A";
                                    REALQTY_T = _BR_BRS_SEL_Yield_Compress_Loss.OUTDATAs[0].REALQTY_T ?? "0";
                                    REALQTY_G = _BR_BRS_SEL_Yield_Compress_Loss.OUTDATAs[0].REALQTY_G ?? "0";
                                    ArgumentAmount = _BR_BRS_SEL_Yield_Compress_Loss.OUTDATAs[0].ArgumentQty_T ?? "0";
                                    TotalYield = _BR_BRS_SEL_Yield_Compress_Loss.OUTDATAs[0].SumYield ?? 0;
                                    //TotalLossSum = 0;
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

                            if (_filteredComponents != null && _filteredComponents.Count > 0)
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

                                foreach (var item in _filteredComponents)
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
            _BR_BRS_SEL_Yield_Compress_Loss = new BR_BRS_SEL_Yield_Compress_Loss();
            _filteredComponents = new ObservableCollection<LossInfo>();
        }
        #endregion

        public decimal SumFunction(decimal a, decimal b)
        {
            decimal sum = 0;

            sum = a + b;

            return sum;
        }


        #region [User Define]
        public class LossInfo
        {
            private string _GUBUN;
            public string GUBUN
            {
                get { return _GUBUN; }
                set
                {
                    _GUBUN = value;
                    //OnPropertyChanged("GUBUN");
                }
            }
            private decimal _InitSettingLoss;
            public decimal InitSettingLoss
            {
                get { return _InitSettingLoss; }
                set
                {
                    _InitSettingLoss = value;
                    //OnPropertyChanged("InitSettingLoss");
                }
            }
            private decimal _inspectionLoss;
            public decimal inspectionLoss
            {
                get { return _inspectionLoss; }
                set
                {
                    _inspectionLoss = value;
                    //OnPropertyChanged("inspectionLoss");
                }
            }
            private decimal _RemainingAmountLoss;
            public decimal RemainingAmountLoss
            {
                get { return _RemainingAmountLoss; }
                set
                {
                    _RemainingAmountLoss = value;
                    //OnPropertyChanged("RemainingAmountLoss");
                }
            }
            private decimal _BadTabletsLoss;
            public decimal BadTabletsLoss
            {
                get { return _BadTabletsLoss; }
                set
                {
                    _BadTabletsLoss = value;
                    //OnPropertyChanged("BadTabletsLoss");
                }
            }
            private decimal _UnknownCauseLoss;
            public decimal UnknownCauseLoss
            {
                get { return _UnknownCauseLoss; }
                set
                {
                    _UnknownCauseLoss = value;
                    //OnPropertyChanged("UnknownCauseLoss");
                }
            }
            private decimal _CurrentCollectorLoss;
            public decimal CurrentCollectorLoss
            {
                get { return _CurrentCollectorLoss; }
                set
                {
                    _CurrentCollectorLoss = value;
                    //OnPropertyChanged("CurrentCollectorLoss");
                }
            }

            public LossInfo() { }
        }

        public void ConvertResult()
        {

            LossInfo output = new LossInfo();

            foreach (var item in _filteredComponents)
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
