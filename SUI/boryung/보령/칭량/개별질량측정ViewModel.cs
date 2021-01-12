﻿using LGCNS.iPharmMES.Common;
using Order;
using System;
using System.Windows.Input;
using System.Linq;
using ShopFloorUI;
using System.Text;
using C1.Silverlight.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace 보령
{
    public class 개별질량측정ViewModel : ViewModelBase
    {
        #region Property

        private string _eqptID;
        public string eqptID
        {
            get { return _eqptID; }
            set
            {
                _eqptID = value;
                NotifyPropertyChanged();
            }
        }

        private string _realEqptID;
        public string realEqptID
        {
            get { return _realEqptID; }
            set
            {
                _realEqptID = value;
                NotifyPropertyChanged();
            }
        }

        private string _sampleCount;
        public string sampleCount
        {
            get { return _sampleCount; }
            set
            {
                _sampleCount = value;
                //calAvgWeight();
                NotifyPropertyChanged();
            }
        }

        private string _curWeighing;
        public string curWeighing
        {
            get { return _curWeighing; }
            set
            {
                _curWeighing = value;
                NotifyPropertyChanged();
            }
        }

        private string _avgWeighing;
        public string avgWeighing
        {
            get { return _avgWeighing; }
            set
            {
                _avgWeighing = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isBtnEnable;
        public bool isBtnEnable
        {
            get { return _isBtnEnable; }
            set
            {
                _isBtnEnable = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isNumericEnable;
        public bool isNumericEnable
        {
            get { return _isNumericEnable; }
            set
            {
                _isNumericEnable = value;
                NotifyPropertyChanged();
            }
        }

        private int _maxValue;
        public int maxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                NotifyPropertyChanged();
            }
        }


        private string _btnName;
        public string btnName
        {
            get { return _btnName; }
            set
            {
                _btnName = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime? _startTime;
        public DateTime? startTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isSaveEnable;
        public bool isSaveEnable
        {
            get { return _isSaveEnable; }
            set
            {
                _isSaveEnable = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isEqptReadonly;
        public bool isEqptReadonly
        {
            get { return _isEqptReadonly; }
            set
            {
                _isEqptReadonly = value;
                NotifyPropertyChanged();
            }
        }

        private string _MAXValue;
        public string MAXValue
        {
            get { return _MAXValue; }
            set
            {
                _MAXValue = value;
                OnPropertyChanged("MAXValue");
            }
        }
        private string _MINValue;
        public string MINValue
        {
            get { return _MINValue; }
            set
            {
                _MINValue = value;
                OnPropertyChanged("MINValue");
            }
        }
        private string _AVGValue;
        public string AVGValue
        {
            get { return _AVGValue; }
            set
            {
                _AVGValue = value;
                OnPropertyChanged("AVGValue");
            }
        }

        private 개별질량측정 _mainWnd;
        private int inx;
        private DispatcherTimer _repeater = new DispatcherTimer();
        private int _repeaterInterval = 2000;   // 100ms -> 500ms -> 1000ms
        
        #endregion

        #region DataAccess

        private BR_BRS_CHK_Equipment_Is_Scale _BR_BRS_CHK_Equipment_Is_Scale;
        public BR_BRS_CHK_Equipment_Is_Scale BR_BRS_CHK_Equipment_Is_Scale
        {
            get { return _BR_BRS_CHK_Equipment_Is_Scale; }
            set { _BR_BRS_CHK_Equipment_Is_Scale = value; }
        }

        private BR_PHR_SEL_CurrentWeight _BR_PHR_SEL_CurrentWeight;
        public BR_PHR_SEL_CurrentWeight BR_PHR_SEL_CurrentWeight
        {
            get { return _BR_PHR_SEL_CurrentWeight; }
            set { _BR_PHR_SEL_CurrentWeight = value; }
        }

        private BR_BRS_REG_IPC_EACH_WEIGHT_MULTI _BR_BRS_REG_IPC_EACH_WEIGHT_MULTI;
        public BR_BRS_REG_IPC_EACH_WEIGHT_MULTI BR_BRS_REG_IPC_EACH_WEIGHT_MULTI
        {
            get { return _BR_BRS_REG_IPC_EACH_WEIGHT_MULTI; }
            set
            {
                _BR_BRS_REG_IPC_EACH_WEIGHT_MULTI = value;
                NotifyPropertyChanged();
            }
        }

        private BR_PHR_REG_ScaleSetTare _BR_PHR_REG_ScaleSetTare;
        public BR_PHR_REG_ScaleSetTare BR_PHR_REG_ScaleSetTare
        {
            get { return _BR_PHR_REG_ScaleSetTare; }
            set
            {
                _BR_PHR_REG_ScaleSetTare = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<EACH_INDATA> _filteredComponents;
        public ObservableCollection<EACH_INDATA> filteredComponents
        {
            get { return _filteredComponents; }
            set
            {
                _filteredComponents = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Command

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
                            IsBusy = true;

                            CommandResults["LoadedCommand"] = false;
                            CommandCanExecutes["LoadedCommand"] = false;

                            ///

                            if (arg == null || !(arg is 개별질량측정))
                                return;

                            isSaveEnable = false;
                            isEqptReadonly = false;

                            _mainWnd = arg as 개별질량측정;
                            inx = 1;

                            MAXValue = "";
                            MINValue = "";
                            AVGValue = "";

                            _startTime = await AuthRepositoryViewModel.GetDBDateTimeNow();
                            _mainWnd.txtEQPTID.Focus();

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

        public ICommand BarcodeChangedCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["BarcodeChangedCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["BarcodeChangedCommand"] = false;
                            CommandCanExecutes["BarcodeChangedCommand"] = false;

                            //유효한 내용인지 확인해야지.
                            _BR_BRS_CHK_Equipment_Is_Scale.INDATAs.Clear();
                            _BR_BRS_CHK_Equipment_Is_Scale.OUTDATAs.Clear();

                            _BR_BRS_CHK_Equipment_Is_Scale.INDATAs.Add(new BR_BRS_CHK_Equipment_Is_Scale.INDATA()
                            {
                                EQPTID = _mainWnd.txtEQPTID.Text
                            });


                            if (await _BR_BRS_CHK_Equipment_Is_Scale.Execute() && _BR_BRS_CHK_Equipment_Is_Scale.OUTDATAs[0].ISPROPER.Equals("Y"))
                            {
                                isBtnEnable = true;
                                isNumericEnable = true;
                                realEqptID = _mainWnd.txtEQPTID.Text;
                                eqptID = _mainWnd.txtEQPTID.Text;
                                _mainWnd.txtConnectionAlarm.Text = "저울 연결 성공";
                                _mainWnd.txtConnectionAlarm.Visibility = System.Windows.Visibility.Visible;

                                _repeater.Start();
                            }

                            //저울이 아니라면 값을 초기화, 저울이 아닌경우 경고창, 하지만 에러가 아니기 때문에 Catch문에 잡히지 않음.
                            if (_BR_BRS_CHK_Equipment_Is_Scale.OUTDATAs.Count == 0)
                            {
                                isBtnEnable = false;
                                isNumericEnable = false;
                                curWeighing = "";
                                _mainWnd.txtEQPTID.Text = "";
                                _mainWnd.txtEQPTID.Focus();
                                _mainWnd.txtConnectionAlarm.Visibility = System.Windows.Visibility.Visible;
                                _mainWnd.txtConnectionAlarm.Text = "저울 연결 실패";

                                _repeater.Stop();
                            }
                            ///

                            CommandResults["BarcodeChangedCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["BarcodeChangedCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["BarcodeChangedCommand"] = true;
                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("BarcodeChangedCommand") ?
                        CommandCanExecutes["BarcodeChangedCommand"] : (CommandCanExecutes["BarcodeChangedCommand"] = true);
                });
            }
        }

        public ICommand ClickConfirmCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ClickConfirmCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["ClickConfirmCommand"] = false;
                            CommandCanExecutes["ClickConfirmCommand"] = false;

                            ///
                            _repeater.Stop();

                            if (filteredComponents.Count > 0)
                            {
                                _BR_BRS_REG_IPC_EACH_WEIGHT_MULTI.INDATAs.Clear();

                                //XML 형식으로 저장
                                DataSet ds = new DataSet();
                                DataTable dt = new DataTable("DATA");
                                ds.Tables.Add(dt);

                                dt.Columns.Add(new DataColumn("순번"));
                                dt.Columns.Add(new DataColumn("장비번호"));
                                //dt.Columns.Add(new DataColumn("샘플수량"));
                                dt.Columns.Add(new DataColumn("현재무게"));
                                //dt.Columns.Add(new DataColumn("평균무게"));

                                foreach (var rowdata in filteredComponents)
                                {
                                    var row = dt.NewRow();
                                    row["순번"] = rowdata.INX != null ? rowdata.INX.ToString() : "";
                                    row["장비번호"] = rowdata.SCALEID != null ? rowdata.SCALEID : "";
                                    row["현재무게"] = rowdata.CUR_WEIGHT != null ? rowdata.CUR_WEIGHT : "";
                                    //row["샘플수량"] = rowdata.SMPQTY != null ? rowdata.SMPQTY : "";
                                    //row["평균무게"] = rowdata.AVG_WEIGHT != null ? rowdata.AVG_WEIGHT : "";
                                    dt.Rows.Add(row);

                                    BR_BRS_REG_IPC_EACH_WEIGHT_MULTI.INDATAs.Add(new BR_BRS_REG_IPC_EACH_WEIGHT_MULTI.INDATA()
                                    {
                                        SCALEID = rowdata.SCALEID,
                                        POID = ProductionOrderInfo.OrderID,
                                        OPSGGUID = ProductionOrderInfo.OrderProcessSegmentID,
                                        SMPQTY = short.Parse(rowdata.SMPQTY),
                                        AVG_WEIGHT = (rowdata.CUR_WEIGHT.Substring(0, rowdata.CUR_WEIGHT.Length - 2)).Replace(",", ""), // 
                                        SMPQTYUOMID = "",//ProductionOrderInfo.MaterialID,
                                        USERID = ProductionOrderInfo.UserID,
                                        LOCATIONID = "",
                                        STRTDTTM = startTime
                                        //STARTDTTM을 추가해줘여한다.
                                    });
                                }

                                if(await _BR_BRS_REG_IPC_EACH_WEIGHT_MULTI.Execute() == true)
                                {
                                    var xml = BizActorRuleBase.CreateXMLStream(ds);
                                    var bytesArray = System.Text.Encoding.UTF8.GetBytes(xml);

                                    _mainWnd.CurrentInstruction.Raw.ACTVAL = _mainWnd.TableTypeName;
                                    _mainWnd.CurrentInstruction.Raw.NOTE = bytesArray;

                                    var result = await _mainWnd.Phase.RegistInstructionValue(_mainWnd.CurrentInstruction);

                                    if (result != enumInstructionRegistErrorType.Ok)
                                    {
                                        throw new Exception(string.Format("값 등록 실패, ID={0}, 사유={1}", _mainWnd.CurrentInstruction.Raw.IRTGUID, result));
                                    }

                                    if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                                    else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);

                                    _mainWnd.Close();
                                }
                            }
                            else
                            {
                                throw new Exception("입력한 정보가 없습니다. 기록 버튼을 클릭하여 추가해 주시기 바랍니다.");
                            }
                            //
                            CommandResults["ClickConfirmCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["ClickConfirmCommand"] = false;
                            _repeater.Start();
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ClickConfirmCommand"] = true;

                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ClickConfirmCommand") ?
                        CommandCanExecutes["ClickConfirmCommand"] : (CommandCanExecutes["ClickConfirmCommand"] = true);
                });
            }
        }

        public ICommand NoRecordConfirmCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["NoRecordConfirmCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["NoRecordConfirmCommand"] = false;
                            CommandCanExecutes["NoRecordConfirmCommand"] = false;

                            ///
                            _repeater.Stop();

                            //XML 형식으로 저장
                            DataSet ds = new DataSet();
                            DataTable dt = new DataTable("DATA");
                            ds.Tables.Add(dt);

                            dt.Columns.Add(new DataColumn("순번"));
                            dt.Columns.Add(new DataColumn("장비번호"));
                            dt.Columns.Add(new DataColumn("현재무게"));

                            var row = dt.NewRow();
                            row["순번"] = "N/A";
                            row["장비번호"] = "N/A";
                            row["현재무게"] = "N/A";
                            dt.Rows.Add(row);

                            var xml = BizActorRuleBase.CreateXMLStream(ds);
                            var bytesArray = System.Text.Encoding.UTF8.GetBytes(xml);

                            _mainWnd.CurrentInstruction.Raw.ACTVAL = _mainWnd.TableTypeName;
                            _mainWnd.CurrentInstruction.Raw.NOTE = bytesArray;

                            var result = await _mainWnd.Phase.RegistInstructionValue(_mainWnd.CurrentInstruction);

                            if (result != enumInstructionRegistErrorType.Ok)
                            {
                                throw new Exception(string.Format("값 등록 실패, ID={0}, 사유={1}", _mainWnd.CurrentInstruction.Raw.IRTGUID, result));
                            }

                            if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                            else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);

                            _mainWnd.Close();
                            //
                            CommandResults["NoRecordConfirmCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["NoRecordConfirmCommand"] = false;
                            _repeater.Start();
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["NoRecordConfirmCommand"] = true;

                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("NoRecordConfirmCommand") ?
                        CommandCanExecutes["NoRecordConfirmCommand"] : (CommandCanExecutes["NoRecordConfirmCommand"] = true);
                });
            }
        }

        public ICommand ClickCancelCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ClickCancelCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["ClickCancelCommand"] = false;
                            CommandCanExecutes["ClickCancelCommand"] = false;

                            ///
                            _mainWnd.Close();
                            ///

                            CommandResults["ClickCancelCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["ClickCancelCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ClickCancelCommand"] = true;

                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ClickCancelCommand") ?
                        CommandCanExecutes["ClickCancelCommand"] : (CommandCanExecutes["ClickCancelCommand"] = true);
                });
            }
        }

        public ICommand SetTareCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["SetTareCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["SetTareCommand"] = false;
                            CommandCanExecutes["SetTareCommand"] = false;

                            ///

                            _BR_PHR_REG_ScaleSetTare.INDATAs.Clear();
                            _BR_PHR_REG_ScaleSetTare.OUTDATAs.Clear();

                            _BR_PHR_REG_ScaleSetTare.INDATAs.Add(new BR_PHR_REG_ScaleSetTare.INDATA
                            {
                                ScaleID = realEqptID
                            });

                            if (await _BR_PHR_REG_ScaleSetTare.Execute() != false)
                            {
                                curWeighing = "0.000mg";
                                avgWeighing = "0.000mg";
                                _mainWnd.txtConnectionAlarm.Text = "Tare 성공";
                            }
                            else
                            {
                                _mainWnd.txtConnectionAlarm.Text = "Tare 실패";
                            }

                            ///

                            CommandResults["SetTareCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["SetTareCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["SetTareCommand"] = true;

                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("SetTareCommand") ?
                        CommandCanExecutes["SetTareCommand"] : (CommandCanExecutes["SetTareCommand"] = true);
                });
            }
        }

        public ICommand RecordCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["RecordCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["RecordCommand"] = false;
                            CommandCanExecutes["RecordCommand"] = false;

                            ///
                            if (_BR_PHR_SEL_CurrentWeight.OUTDATAs.Count == 0 || _BR_PHR_SEL_CurrentWeight.OUTDATAs[0].Weight == null) return;

                            filteredComponents.Add(new EACH_INDATA()
                            {
                                CHK = "N",
                                INX = inx++,
                                SCALEID = realEqptID,
                                SMPQTY = sampleCount,
                                CUR_WEIGHT = curWeighing,
                                AVG_WEIGHT = "",
                                CUR_WEIGHT_NUM = _BR_PHR_SEL_CurrentWeight.OUTDATAs[0].Weight,
                                UOM = _BR_PHR_SEL_CurrentWeight.OUTDATAs[0].UOM
                            });

                            calMaxMinWeight();
                            ///

                            CommandResults["RecordCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["RecordCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["RecordCommand"] = true;

                            if (filteredComponents.Count > 0)
                            {
                                isSaveEnable = true;
                                isEqptReadonly = true;
                            }
                            else isSaveEnable = false;

                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("RecordCommand") ?
                        CommandCanExecutes["RecordCommand"] : (CommandCanExecutes["RecordCommand"] = true);
                });
            }
        }

        public ICommand RowDeleteCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["RowDeleteCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["RowDeleteCommand"] = false;
                            CommandCanExecutes["RowDeleteCommand"] = false;

                            ///

                            var elements = (from data in filteredComponents
                                            where data.CHK == "N"
                                            select data).ToList();

                            filteredComponents.Clear();
                            inx = 1;
                            foreach (var data in elements)
                            {
                                data.INX = inx++;
                                filteredComponents.Add(data);
                            }

                            if(filteredComponents.Count > 0)
                                calMaxMinWeight();
                            ///
                            CommandResults["RowDeleteCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["RowDeleteCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["RowDeleteCommand"] = true;

                            if (filteredComponents.Count == 0)
                            {
                                isSaveEnable = false;
                                isEqptReadonly = false;
                            }

                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("RowDeleteCommand") ?
                        CommandCanExecutes["RowDeleteCommand"] : (CommandCanExecutes["RowDeleteCommand"] = true);
                });
            }
        }

        async void _repeater_Tick(object sender, EventArgs e)
        {
            try
            {
                _repeater.Stop();

                _BR_PHR_SEL_CurrentWeight.INDATAs.Clear();
                _BR_PHR_SEL_CurrentWeight.OUTDATAs.Clear();
                _BR_PHR_SEL_CurrentWeight.INDATAs.Add(new BR_PHR_SEL_CurrentWeight.INDATA()
                {
                    ScaleID = realEqptID
                });

                if (await _BR_PHR_SEL_CurrentWeight.Execute(exceptionHandle: LGCNS.iPharmMES.Common.Common.enumBizRuleXceptionHandleType.FailEvent) == true)
                    curWeighing = calWeightdigit(_BR_PHR_SEL_CurrentWeight.OUTDATAs[0].Weight, _BR_PHR_SEL_CurrentWeight.OUTDATAs[0].UOM);
                
            }
            catch (Exception ex)
            {
                _BR_PHR_SEL_CurrentWeight.INDATAs.Clear();
                _BR_PHR_SEL_CurrentWeight.OUTDATAs.Clear();
                _repeater.Start();
            }
            _repeater.Start();
        }
        #endregion

        //public void calAvgWeight()
        //{
        //    //측정한 값이 존재
        //    if (_BR_PHR_SEL_CurrentWeight.OUTDATAs.Count > 0)
        //    {
        //        double? convertedWeight = null;
        //        double? temp = null;

        //        if (_BR_PHR_SEL_CurrentWeight.OUTDATAs[0].UOM == "kg")
        //        {
        //            convertedWeight = _BR_PHR_SEL_CurrentWeight.OUTDATAs[0].Weight * 1000000;
        //        }
        //        else if (_BR_PHR_SEL_CurrentWeight.OUTDATAs[0].UOM == "g")
        //        {
        //            convertedWeight = _BR_PHR_SEL_CurrentWeight.OUTDATAs[0].Weight * 1000;
        //        }
        //        else if (_BR_PHR_SEL_CurrentWeight.OUTDATAs[0].UOM == "mg")
        //        {
        //            convertedWeight = _BR_PHR_SEL_CurrentWeight.OUTDATAs[0].Weight;
        //        }

        //        temp = convertedWeight / Double.Parse(sampleCount);

        //        avgWeighing = string.Format("{0:#,##0.0}{1}", temp, "mg");
        //    }
        //}

        public string calWeightdigit(decimal? weight, string uom)
        {
            string strWeight = "";

            if (uom == "kg")
            {
                strWeight = string.Format("{0:#.0}{1}", weight * 1000000, "mg");
            }
            else if (uom == "g")
            {
                strWeight = string.Format("{0:#.0}{1}", weight * 1000, "mg");
            }
            else
            {
                strWeight = string.Format("{0:#.0}{1}", weight, "mg");
            }
            return strWeight;
        }

        public void calMaxMinWeight()
        {
            var MAXidx = _filteredComponents.Max(x => x.CUR_WEIGHT_NUM);
            var MINidx = _filteredComponents.Min(x => x.CUR_WEIGHT_NUM);
            var MAXitem = _filteredComponents.First(x => x.CUR_WEIGHT_NUM == MAXidx);
            var MINitem = _filteredComponents.First(x => x.CUR_WEIGHT_NUM == MINidx);
            var AVGitem = _filteredComponents.Average(x => x.CUR_WEIGHT_NUM);

            MAXValue = calWeightdigit(MAXitem.CUR_WEIGHT_NUM, MAXitem.UOM);
            MINValue = calWeightdigit(MINitem.CUR_WEIGHT_NUM, MINitem.UOM);
            AVGValue = calWeightdigit(AVGitem, MINitem.UOM);
        }

        public 개별질량측정ViewModel()
        {
            _BR_BRS_CHK_Equipment_Is_Scale = new BR_BRS_CHK_Equipment_Is_Scale();
            _BR_PHR_SEL_CurrentWeight = new BR_PHR_SEL_CurrentWeight();
            _BR_BRS_REG_IPC_EACH_WEIGHT_MULTI = new BR_BRS_REG_IPC_EACH_WEIGHT_MULTI();
            _BR_PHR_REG_ScaleSetTare = new BR_PHR_REG_ScaleSetTare();
            _filteredComponents = new ObservableCollection<EACH_INDATA>();

            isBtnEnable = false;
            isNumericEnable = false;

            _btnName = "기록";
            sampleCount = "1";
            maxValue = 2000000000;

            string interval_str = ShopFloorUI.App.Current.Resources["GetWeightInterval"].ToString();
            if (int.TryParse(interval_str, out _repeaterInterval) == false)
                _repeaterInterval = 2000;

            _repeater.Interval = new TimeSpan(0, 0, 0, 0, _repeaterInterval);
            _repeater.Tick += _repeater_Tick;
        }

        public partial class EACH_INDATA
        {
            private string _CHK;
            public string CHK
            {
                get { return _CHK; }
                set { _CHK = value; }
            }

            private string _SCALEID;
            public string SCALEID
            {
                get { return _SCALEID; }
                set { _SCALEID = value; }
            }

            private string _SMPQTY;
            public string SMPQTY
            {
                get { return _SMPQTY; }
                set { _SMPQTY = value; }
            }

            private string _CUR_WEIGHT;
            public string CUR_WEIGHT
            {
                get { return _CUR_WEIGHT; }
                set { _CUR_WEIGHT = value; }
            }

            private string _AVG_WEIGHT;
            public string AVG_WEIGHT
            {
                get { return _AVG_WEIGHT; }
                set { _AVG_WEIGHT = value; }
            }

            private int _INX;
            public int INX
            {
                get { return _INX; }
                set { _INX = value; }
            }

            private string _UOM;
            public string UOM
            {
                get { return _UOM; }
                set { _UOM = value; }
            }

            private decimal? _CUR_WEIGHT_NUM;
            public decimal? CUR_WEIGHT_NUM
            {
                get { return _CUR_WEIGHT_NUM; }
                set { _CUR_WEIGHT_NUM = value;}
            }

            public EACH_INDATA() { }
        }
    }
}
