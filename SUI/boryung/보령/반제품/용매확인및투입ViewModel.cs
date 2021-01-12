using C1.Silverlight.Data;
using Equipment;
using LGCNS.EZMES.Common;
using LGCNS.iPharmMES.Common;
using LGCNS.iPharmMES.Recipe.Common;
using ShopFloorUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Common = LGCNS.iPharmMES.Common.Common;
using System.Configuration;

namespace 보령
{
    public class 용매확인및투입ViewModel : ViewModelBase
    {
        #region [Property]
        public 용매확인및투입ViewModel()
        {
            _BR_BRS_SEL_Charging_Solvent = new BR_BRS_SEL_Charging_Solvent();
            _filteredComponents = new BR_BRS_SEL_Charging_Solvent.OUTDATACollection();
            _BR_BRS_REG_Dispense_Charging_Solvent = new BR_BRS_REG_Dispense_Charging_Solvent();
            _curSeletedItem = new BR_BRS_SEL_Charging_Solvent.OUTDATA();

            int interval = 2000;
            string interval_str = ShopFloorUI.App.Current.Resources["GetWeightInterval"].ToString();
            if (int.TryParse(interval_str, out interval) == false)
            {
                interval = 2000;
            }

            _DispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, interval);
            _DispatcherTimer.Tick += _DispatcherTimer_Tick;

        }
        enum enumScanType
        {
            Material,
            Scale
        };
        용매확인및투입 _mainWnd;
        public decimal? NumericScaleValue = 0;
        DispatcherTimer _DispatcherTimer = new DispatcherTimer(); // 저울값 타이머

        #region [기준정보]
        string _batchNo;
        public string BatchNo
        {
            get { return _batchNo; }
            set
            {
                _batchNo = value;
                NotifyPropertyChanged();
            }
        }

        string _orderNo;
        public string OrderNo
        {
            get { return _orderNo; }
            set
            {
                _orderNo = value;
                NotifyPropertyChanged();
            }
        }

        string _processSegmentID;
        public string ProcessSegmentID
        {
            get { return _processSegmentID; }
            set
            {
                _processSegmentID = value;
                NotifyPropertyChanged();
            }
        }

        string _bomID;
        public string BomID
        {
            get { return _bomID; }
            set
            {
                _bomID = value;
                NotifyPropertyChanged();
            }
        }

        string _chgSeq;
        public string ChgSeq
        {
            get { return _chgSeq; }
            set
            {
                _chgSeq = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region [자재정보]
        string _mtrlId;
        public string MtrlId
        {
            get { return _mtrlId; }
            set
            {
                _mtrlId = value;
                NotifyPropertyChanged();
            }
        }

        string _mtrlName;
        public string MtrlName
        {
            get { return _mtrlName; }
            set
            {
                _mtrlName = value;
                NotifyPropertyChanged();
            }
        }

        string _stdQty;
        public string StdQty
        {
            get { return _stdQty; }
            set
            {
                _stdQty = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        #region [무게측정]
        string _scaleId;
        public string ScaleId
        {
            get { return _scaleId; }
            set
            {
                _scaleId = value;
                NotifyPropertyChanged();
            }
        }

        string _scaleValue;
        public string ScaleValue
        {
            get { return _scaleValue; }
            set
            {
                _scaleValue = value;
                NotifyPropertyChanged();
            }
        }

        string _scaleUOM;
        public string ScaleUOM
        {
            get { return _scaleUOM; }
            set
            {
                _scaleUOM = value;
                NotifyPropertyChanged();
            }
        }

        decimal _OLDscaleValue = 0;
        public decimal OLDscaleValue
        {
            get { return _OLDscaleValue; }
            set
            {
                _OLDscaleValue = value;
                NotifyPropertyChanged();
            }
        }

        private string _UpperWeight;
        public string UpperWeight
        {
            get { return _UpperWeight; }
            set
            {
                _UpperWeight = value;
                OnPropertyChanged("UpperWeight");
            }
        }

        private string _LowerWeight;
        public string LowerWeight
        {
            get { return _LowerWeight; }
            set
            {
                _LowerWeight = value;
                OnPropertyChanged("LowerWeight");
            }
        }
        private bool _CANCHARGEFLAG; // 투입 버튼
        public bool CANCHARGEFLAG
        {
            get { return _CANCHARGEFLAG; }
            set
            {
                _CANCHARGEFLAG = value;
                NotifyPropertyChanged();
            }
        }
        private bool _CANRECORDFLAG; // 기록 버튼
        public bool CANRECORDFLAG
        {
            get { return _CANRECORDFLAG; }
            set
            {
                _CANRECORDFLAG = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        BR_BRS_SEL_Charging_Solvent.OUTDATACollection _filteredComponents; // 그리드 내용
        public BR_BRS_SEL_Charging_Solvent.OUTDATACollection FilteredComponents
        {
            get { return _filteredComponents; }
            set
            {
                _filteredComponents = value;
                NotifyPropertyChanged();
            }
        }
        private BR_BRS_SEL_Charging_Solvent.OUTDATA _curSeletedItem;
        public BR_BRS_SEL_Charging_Solvent.OUTDATA curSeletedItem
        {
            get { return _curSeletedItem; }
            set
            {
                _curSeletedItem = value;
                if (_mainWnd != null)
                    _mainWnd.dgProductionOutput.SelectedItem = value;
                ButtonControl();
                NotifyPropertyChanged();
            }
        }
        
        #endregion
        #region [BizRule]
        BR_BRS_SEL_Charging_Solvent _BR_BRS_SEL_Charging_Solvent; // 용매 조회
        public BR_BRS_SEL_Charging_Solvent BR_BRS_SEL_Charging_Solvent

        {
            get { return _BR_BRS_SEL_Charging_Solvent; }
            set
            {
                _BR_BRS_SEL_Charging_Solvent = value;
                NotifyPropertyChanged();
            }
        }
        private BR_BRS_REG_Dispense_Charging_Solvent _BR_BRS_REG_Dispense_Charging_Solvent; // 용매 소분 후 투입
        public BR_BRS_REG_Dispense_Charging_Solvent BR_BRS_REG_Dispense_Charging_Solvent
        {
            get { return _BR_BRS_REG_Dispense_Charging_Solvent; }
            set
            {
                _BR_BRS_REG_Dispense_Charging_Solvent = value;
                OnPropertyChanged("BR_BRS_REG_Dispense_Charging_Solvent");
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
                            IsBusy = true;

                            CommandResults["LoadedCommand"] = false;
                            CommandCanExecutes["LoadedCommand"] = false;

                            ///
                            if (arg != null && arg is 용매확인및투입) _mainWnd = arg as 용매확인및투입;

                            var instruction = _mainWnd.CurrentInstruction;
                            var phase = _mainWnd.Phase;

                            this.OrderNo = _mainWnd.CurrentOrder.OrderID;
                            this.BatchNo = _mainWnd.CurrentOrder.BatchNo;
                            this.ProcessSegmentID = _mainWnd.CurrentOrder.OrderProcessSegmentID;
                            this.BomID = _mainWnd.CurrentInstruction.Raw.BOMID;
                            this.ScaleId = _mainWnd.CurrentInstruction.Raw.EQPTID;
                            this.ChgSeq = _mainWnd.CurrentInstruction.Raw.EXPRESSION;

                            //// 테스트
                            //if (string.IsNullOrWhiteSpace(BomID))
                            //    throw new Exception(string.Format("해당 Instruction에 BOM ID가 설정되지 않았습니다."));

                            if (!string.IsNullOrWhiteSpace(ScaleId))
                                _DispatcherTimer.Start();

                            CANRECORDFLAG = false;
                            CANCHARGEFLAG = false;

                            _BR_BRS_SEL_Charging_Solvent.INDATAs.Add(new BR_BRS_SEL_Charging_Solvent.INDATA()
                            {
                                POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                CHGSEQ = ChgSeq,
                                ROOMNO = AuthRepositoryViewModel.Instance.RoomID,
                                MTRLID = BomID
                            });

                            if (await _BR_BRS_SEL_Charging_Solvent.Execute() == true)
                            {
                                //BOM 기준정보
                                if (_BR_BRS_SEL_Charging_Solvent.OUTDATA_BOMs.Count > 0)
                                {
                                    var item = _BR_BRS_SEL_Charging_Solvent.OUTDATA_BOMs.Where(o => o.MTRLID == BomID && o.CHGSEQ == ChgSeq).FirstOrDefault();

                                    MtrlId = item.MTRLID;
                                    MtrlName = item.MTRLNAME;
                                    StdQty = item.STDQTY + item.NOTATION;
                                }
                                // 자재목록
                                if (_BR_BRS_SEL_Charging_Solvent.OUTDATAs.Count > 0)
                                {
                                    foreach (var outdata in _BR_BRS_SEL_Charging_Solvent.OUTDATAs.Where(o => o.MTRLID == BomID && o.CHGSEQ == ChgSeq).ToList())
                                    {
                                        outdata.CHECK = "투입대기";
                                        _filteredComponents.Add(outdata);
                                    }
                                }
                                else
                                    throw new Exception(string.Format("조회된 결과가 없습니다."));

                                if (_filteredComponents.Count > 0)
                                {
                                    UpperWeight = string.Format("{0}{1}", _filteredComponents[0].UPPERQTY, _filteredComponents[0].UOMNAME);
                                    LowerWeight = string.Format("{0}{1}", _filteredComponents[0].LOWERQTY, _filteredComponents[0].UOMNAME);

                                    if (_filteredComponents.Where(o => o.ISBCDSCAN == "Y").ToList().Count > 0)
                                        ScanMtrlCommandAsync.Execute(null);
                                }
                                else
                                    throw new Exception(string.Format("대상 원료가 존재하지 않습니다."));
                            }

                            IsBusy = false;
                            ///

                            CommandResults["LoadedCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["LoadedCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["LoadedCommand"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadedCommand") ?
                        CommandCanExecutes["LoadedCommand"] : (CommandCanExecutes["LoadedCommand"] = true);
                });
            }
        }
        public ICommand ScanMtrlCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ScanMtrlCommand"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["ScanMtrlCommand"] = false;
                            CommandCanExecutes["ScanMtrlCommand"] = false;

                            ///
                            IsBusy = true;

                            var viewmodel = new 자재저울스캔팝업ViewModel(enumScanType.Material)
                            {
                                ParentVM = this,
                            };

                            var ScanPopup = new 자재저울스캔팝업()
                            {
                                DataContext = viewmodel
                            };

                            ScanPopup.Show();

                            IsBusy = false;
                            ///

                            CommandResults["ScanMtrlCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["ScanMtrlCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ScanMtrlCommand"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ScanMtrlCommand") ?
                        CommandCanExecutes["ScanMtrlCommand"] : (CommandCanExecutes["ScanMtrlCommand"] = true);
                });
            }
        }
        public ICommand ScanScaleCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ScanScaleCommand"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["ScanScaleCommand"] = false;
                            CommandCanExecutes["ScanScaleCommand"] = false;

                            ///
                            IsBusy = true;

                            var viewmodel = new 자재저울스캔팝업ViewModel(enumScanType.Scale)
                            {
                                ParentVM = this,
                            };

                            var ScanPopup = new 자재저울스캔팝업()
                            {
                                DataContext = viewmodel
                            };
                            ScanPopup.Closed += (s, e) =>
                            {
                                //저울타이머
                                if (!string.IsNullOrWhiteSpace(ScaleId))
                                    _DispatcherTimer.Start();
                                else
                                    _DispatcherTimer.Stop();
                            };
                            ScanPopup.Show();

                            IsBusy = false;
                            ///

                            CommandResults["ScanScaleCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["ScanScaleCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ScanScaleCommand"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ScanScaleCommand") ?
                        CommandCanExecutes["ScanScaleCommand"] : (CommandCanExecutes["ScanScaleCommand"] = true);
                });
            }
        }
        public ICommand ConfirmCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ConfirmCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["ConfirmCommand"] = false;
                            CommandCanExecutes["ConfirmCommand"] = false;

                            _DispatcherTimer.Stop();

                            var authHelper = new iPharmAuthCommandHelper();

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

                            ///////////////////////////반제품생성//////////////////////////////////////////////////////////////////////
                            authHelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "OM_ProductionOrder_Charging");

                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Function,
                                Common.enumAccessType.Create,
                                "원료(용매) 투입",
                                "원료(용매) 투입",
                                false,
                                "OM_ProductionOrder_Charging",
                                "", null, null) == false)
                            {
                                throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                            }

                            var bizrule = new BR_CUS_GET_SYSTIME();
                            await bizrule.Execute();

                            _BR_BRS_REG_Dispense_Charging_Solvent.INDATAs.Clear();

                            var bizruleData = FilteredComponents.OrderBy(x => x.TOTALQTY);
                            foreach (var item in bizruleData)
                            {
                                if (item.CHECK == "투입완료")
                                {
                                    _BR_BRS_REG_Dispense_Charging_Solvent.INDATAs.Add(new BR_BRS_REG_Dispense_Charging_Solvent.INDATA
                                    {
                                        LANGID = "ko-KR",
                                        MSUBLOTID = item.MSUBLOTID != null ? item.MSUBLOTID : "",
                                        MSUBLOTBCD = item.MSUBLOTBCD != null ? item.MSUBLOTBCD : "",
                                        INSDTTM = AuthRepositoryViewModel.GetDateTimeByFunctionCode("OM_ProductionOrder_Charging") == null ? DateTime.Now : AuthRepositoryViewModel.GetDateTimeByFunctionCode("OM_ProductionOrder_Charging"),
                                        INSUSER = AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_Charging") == null ? AuthRepositoryViewModel.Instance.LoginedUserID : AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_Charging"),
                                        DEPLETFLAG = "P",
                                        VESSELID = "",
                                        POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                        MSUBLOTTYPE = "DSP",
                                        COMPONENTGUID = item.COMPONENTGUID != null ? item.COMPONENTGUID : "",
                                        TARE = 0m,
                                        LOCATIONID = AuthRepositoryViewModel.Instance.RoomID,
                                        INVENTORYQTY = item.MSUBLOTQTY,
                                        DISPENSEQTY = item.REALQTY,
                                        ISDISPSTRT = "Y",
                                        ACTID = "Dispensing",
                                        CHECKINUSER = AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_Charging") == null ? AuthRepositoryViewModel.Instance.LoginedUserID : AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_Charging"),
                                        CHECKINDTTM = AuthRepositoryViewModel.GetDateTimeByFunctionCode("OM_ProductionOrder_Charging") == null ? DateTime.Now : AuthRepositoryViewModel.GetDateTimeByFunctionCode("OM_ProductionOrder_Charging"),
                                        WEIGHINGMETHOD = _BR_BRS_SEL_Charging_Solvent.OUTDATA_BOMs[0].WEIGHTYPECODE,
                                        UPPERVALUE = item.UPPERQTY,
                                        LOWERVALUE = item.LOWERQTY,
                                        LOTTYPE = string.Empty,
                                        OPSGGUID = Guid.Parse(_mainWnd.CurrentOrder.OrderProcessSegmentID),
                                        TAREWEIGHT = 0m,
                                        TAREUOMID = item.UOMNAME,
                                        REASON = "현장칭량",
                                        SCALEID = this.ScaleId,
                                        INSSIGNATURE = AuthRepositoryViewModel.Instance.ConfirmedGuid,
                                        DSPSTRTDTTM = (bizrule.OUTDATAs.Count > 0 && bizrule.OUTDATAs[0].SYSTIME.HasValue) ? bizrule.OUTDATAs[0].SYSTIME.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        DSPENDDTTM = (bizrule.OUTDATAs.Count > 0 && bizrule.OUTDATAs[0].SYSTIME.HasValue) ? bizrule.OUTDATAs[0].SYSTIME.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        WEIGHINGROOM = AuthRepositoryViewModel.Instance.RoomID
                                    });
                                }
                            }

                            if (await _BR_BRS_REG_Dispense_Charging_Solvent.Execute() == true)
                            {
                                var ds = new DataSet();
                                var dt = new DataTable("DATA");
                                ds.Tables.Add(dt);
                                dt.Columns.Add(new DataColumn("자재ID"));
                                dt.Columns.Add(new DataColumn("자재명"));
                                dt.Columns.Add(new DataColumn("원료배치번호"));
                                dt.Columns.Add(new DataColumn("바코드"));
                                dt.Columns.Add(new DataColumn("무게"));
                                dt.Columns.Add(new DataColumn("단위"));

                                foreach (var item in FilteredComponents)
                                {
                                    var row = dt.NewRow();
                                    row["자재ID"] = item.MTRLID != null ? item.MTRLID : "";
                                    row["자재명"] = item.MTRLNAME != null ? item.MTRLNAME : "";
                                    row["원료배치번호"] = item.MSUBLOTID != null ? item.MSUBLOTID : "";
                                    row["바코드"] = item.MSUBLOTBCD != null ? item.MSUBLOTBCD : "";
                                    row["무게"] = item.REALQTY.ToString("0.##0");
                                    row["단위"] = item.UOMNAME != null ? item.UOMNAME : "";
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

                                _DispatcherTimer.Stop();
                                if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                                else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);

                            }

                            CommandResults["ConfirmCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            _DispatcherTimer.Start();

                            CommandResults["ConfirmCommand"] = false;
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
        #region [Custom]
        async void _DispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                _DispatcherTimer.Stop();

                if (!string.IsNullOrWhiteSpace(ScaleId))
                {
                    BR_BRS_SEL_CurrentWeight currentweight = new BR_BRS_SEL_CurrentWeight();
                    currentweight.INDATAs.Add(new BR_BRS_SEL_CurrentWeight.INDATA()
                    {
                        ScaleID = ScaleId.ToUpper()
                    });

                    if (await currentweight.Execute(exceptionHandle: LGCNS.iPharmMES.Common.Common.enumBizRuleXceptionHandleType.FailEvent) == true)
                    {
                        NumericScaleValue = decimal.Parse(currentweight.OUTDATAs[0].Weight.ToString());
                        ScaleValue = string.Format("{0}{1}", currentweight.OUTDATAs[0].Weight.ToString(), currentweight.OUTDATAs[0].UOM);
                        ValidationScaleValue(ScaleValue);
                    }
                    else
                    {
                        _mainWnd.txtScaleValue.Background = new SolidColorBrush(Colors.Red);
                        ScaleValue = "저울 연결 끊김";
                    }

                    _DispatcherTimer.Start();
                }
            }
            catch (Exception ex)
            {
                _DispatcherTimer.Stop();
                OnException(ex.Message, ex);
            }
        }
        public void ScaleTextValidation(TextBox target)
        {
            try
            {
                if (ValidationScaleValue(target.Text.ToUpper()))
                    target.Background = new SolidColorBrush(Colors.Green);
                else
                    target.Background = new SolidColorBrush(Colors.Yellow);
            }
            catch (Exception ex)
            {
                OnException(ex.Message, ex);
            } 
        }
        public bool ValidationScaleValue(string target)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ScaleValue) && !target.Contains("저울"))
                {
                    double curweight = Convert.ToDouble(target.ToUpper().Replace("G", ""));
                    double numericUpper = Convert.ToDouble(UpperWeight.ToUpper().Replace("G", ""));
                    double numericLower = Convert.ToDouble(LowerWeight.ToUpper().Replace("G", ""));

                    if (curweight >= numericLower && curweight <= numericUpper)
                    {
                        if (FilteredComponents.Where(o => o.CHECK == "투입가능" || o.CHECK == "투입대기").Count() > 0)
                            CANRECORDFLAG = false;
                        else
                            CANRECORDFLAG = true;

                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                OnException(ex.Message, ex);
                return false;
            }
        }
        public void ChargingMTRL()
        {
            try
            {
                _DispatcherTimer.Stop();

                if (_curSeletedItem.CHECK == "투입가능" && NumericScaleValue.HasValue)
                {
                    if (NumericScaleValue.Value > OLDscaleValue)
                    {
                        decimal TOTALQTY = NumericScaleValue.Value;
                        decimal CHGQTY = NumericScaleValue.Value - OLDscaleValue;

                        _curSeletedItem.TOTALQTY = TOTALQTY;
                        _curSeletedItem.REALQTY = CHGQTY;
                        _curSeletedItem.CHECK = "투입완료";
                        ButtonControl();

                        OLDscaleValue = NumericScaleValue.Value;
                        _mainWnd.dgProductionOutput.Refresh();
                    }
                }
                _DispatcherTimer.Start();
            }
            catch (Exception ex)
            {
                _DispatcherTimer.Start();
                OnException(ex.Message, ex);
            }
        }
        public void ButtonControl()
        {
            try
            {
                if(curSeletedItem == null)
                {
                    CANCHARGEFLAG = false;
                    CANRECORDFLAG = false;
                    return;
                }
                // 투입버튼
                if (curSeletedItem.MSUBLOTQTY.Value > curSeletedItem.REALQTY)
                    CANCHARGEFLAG = true;
                else
                    CANCHARGEFLAG = false;
            }
            catch (Exception ex)
            {
                OnException(ex.Message, ex);
            }
        }
        #endregion
    }


}
