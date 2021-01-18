using C1.Silverlight.Data;
using LGCNS.iPharmMES.Common;
using ShopFloorUI;
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

namespace 보령
{
    public class 포장공정반제품분할ViewModel : ViewModelBase
    {
        #region 0.Property
        private 포장공정반제품분할 _mainWnd;
        public 포장공정반제품분할ViewModel()
        {
            _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT = new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT();
            _BR_PHR_SEL_System_Printer = new BR_PHR_SEL_System_Printer();
            _BR_PHR_SEL_PRINT_LabelImage = new BR_PHR_SEL_PRINT_LabelImage();
            _BR_BRS_REG_MaterialSubLot_INV_Split = new BR_BRS_REG_MaterialSubLot_INV_Split();
            _BR_BRS_SEL_VESSEL_Info = new BR_BRS_SEL_VESSEL_Info();
        }
        // 프린터
        private BR_PHR_SEL_System_Printer.OUTDATA _selectedPrint;
        public string curPrintName
        {
            get
            {
                if (_selectedPrint != null)
                    return _selectedPrint.PRINTERNAME;
                else
                    return "N/A";
            }           
        }
        // 분할 대상의 용기번호
        private string _SRC_VesselID;
        public string SRC_VesselID
        {
            get { return _SRC_VesselID; }
            set
            {
                _SRC_VesselID = value;
                OnPropertyChanged("SRC_VesselID");
            }
        }
        // 빈용기 용기번호
        private string _EMPTY_VesselID;
        public string EMPTY_VesselID
        {
            get { return _EMPTY_VesselID; }
            set
            {
                _EMPTY_VesselID = value;
                OnPropertyChanged("EMPTY_VesselID");
            }
        }
        private decimal _EMPTY_GROSS;
        public decimal EMPTY_GROSS
        {
            get { return _EMPTY_GROSS; }
            set
            {
                _EMPTY_GROSS = value;
                try
                {
                    setSplitMaterialSubLot();
                }
                catch (Exception ex)
                {
                    OnException(ex.Message, ex);
                }
                OnPropertyChanged("EMPTY_GROSS");
            }
        }
        private decimal _EMPTY_TARE;
        public decimal EMPTY_TARE
        {
            get { return _EMPTY_TARE; }
            set
            {
                _EMPTY_TARE = value;
                try
                {
                    setSplitMaterialSubLot();
                }
                catch (Exception ex)
                {
                    OnException(ex.Message, ex);
                }
                OnPropertyChanged("EMPTY_TARE");
            }
        }
        private string _PRECISION_FORMAT;
        public string PRECISION_FORMAT
        {
            get { return _PRECISION_FORMAT; }
            set
            {
                _PRECISION_FORMAT = value;
                OnPropertyChanged("PRECISION_FORMAT");
            }
        }
        private BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA _SourceMaterialSubLot;
        public BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA SourceMaterialSubLot
        {
            get { return _SourceMaterialSubLot; }
            set
            {
                _SourceMaterialSubLot = value;
                OnPropertyChanged("SourceMaterialSubLot");
            }
        }
        private BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA _SplitMaterialSubLot_A;
        public BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA SplitMaterialSubLot_A
        {
            get { return _SplitMaterialSubLot_A; }
            set
            {
                _SplitMaterialSubLot_A = value;
                OnPropertyChanged("SplitMaterialSubLot_A");
            }
        }
        private BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA _SplitMaterialSubLot_B;
        public BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA SplitMaterialSubLot_B
        {
            get { return _SplitMaterialSubLot_B; }
            set
            {
                _SplitMaterialSubLot_B = value;
                OnPropertyChanged("SplitMaterialSubLot_B");
            }
        }

        private bool _CanSplitHLAB;
        /// <summary>
        /// ture : 분할버튼 활성화, false : 분할버튼 비활성화
        /// </summary>
        public bool CanSplitHLAB
        {
            get { return _CanSplitHLAB; }
            set
            {
                _CanSplitHLAB = value;
                OnPropertyChanged("CanSplitHLAB");
            }
        }        
        #endregion
        #region 1.BizRule
        /// <summary>
        /// 포장반제품 조회
        /// </summary>
        private BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT;
        public BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT
        {
            get { return _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT; }
            set
            {
                _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT = value;
                OnPropertyChanged("BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT");
            }
        }
        /// <summary>
        /// 프린터 조회
        /// </summary>
        private BR_PHR_SEL_System_Printer _BR_PHR_SEL_System_Printer;
        /// <summary>
        /// 라벨 출력
        /// </summary>
        private BR_PHR_SEL_PRINT_LabelImage _BR_PHR_SEL_PRINT_LabelImage;        
        /// <summary>
        /// 반제품 분할
        /// </summary>
        private BR_BRS_REG_MaterialSubLot_INV_Split _BR_BRS_REG_MaterialSubLot_INV_Split;
        /// <summary>
        /// 스캔한 용기 확인
        /// </summary>
        private BR_BRS_SEL_VESSEL_Info _BR_BRS_SEL_VESSEL_Info;
        #endregion
        #region 2.Command
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
                            if (arg == null || !(arg is 포장공정반제품분할))
                                return;

                            _mainWnd = arg as 포장공정반제품분할;

                            // 프린터 조회
                            _BR_PHR_SEL_System_Printer.INDATAs.Clear();
                            _BR_PHR_SEL_System_Printer.OUTDATAs.Clear();
                            _BR_PHR_SEL_System_Printer.INDATAs.Add(new BR_PHR_SEL_System_Printer.INDATA
                            {
                                LANGID = AuthRepositoryViewModel.Instance.LangID,
                                ROOMID = AuthRepositoryViewModel.Instance.RoomID,
                                IPADDRESS = Common.ClientIP
                            });

                            if (await _BR_PHR_SEL_System_Printer.Execute() && _BR_PHR_SEL_System_Printer.OUTDATAs.Count > 0)
                            {
                                if(_BR_PHR_SEL_System_Printer.OUTDATAs.Count == 1)
                                    _selectedPrint = _BR_PHR_SEL_System_Printer.OUTDATAs[0];
                            }

                            OnPropertyChanged("curPrintName");

                            GetavailableWIPList();

                            CanSplitHLAB = false;
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

        public ICommand SearchWIPCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["SearchWIPCommand"].EnterAsync())
                    {
                        try
                        {

                            IsBusy = true;

                            CommandResults["SearchWIPCommand"] = false;
                            CommandCanExecutes["SearchWIPCommand"] = false;

                            ///
                            if (arg is string)
                            {
                                string ibcno = arg as string;
                                foreach (var item in _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATAs)
                                {
                                    if (item.VESSELID == ibcno)
                                    {
                                        SourceMaterialSubLot = item;

                                        _SplitMaterialSubLot_A = new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA();
                                        _SplitMaterialSubLot_A.POID = SourceMaterialSubLot.POID;
                                        _SplitMaterialSubLot_A.OPSGGUID = SourceMaterialSubLot.OPSGGUID;
                                        _SplitMaterialSubLot_A.OUTPUTGUID = SourceMaterialSubLot.OUTPUTGUID;
                                        _SplitMaterialSubLot_A.VESSELID = SourceMaterialSubLot.VESSELID;
                                        _SplitMaterialSubLot_A.MSUBLOTID = SourceMaterialSubLot.MSUBLOTID;
                                        _SplitMaterialSubLot_A.TARE.SetWeight(SourceMaterialSubLot.TARE.Value, SourceMaterialSubLot.TARE.Uom, SourceMaterialSubLot.TARE.Precision);
                                        _SplitMaterialSubLot_A.GROSS.SetWeight(SourceMaterialSubLot.GROSS.Value, SourceMaterialSubLot.GROSS.Uom, SourceMaterialSubLot.GROSS.Precision);
                                        _SplitMaterialSubLot_A.TOTAL.SetWeight(SourceMaterialSubLot.TOTAL.Value, SourceMaterialSubLot.TOTAL.Uom, SourceMaterialSubLot.TOTAL.Precision);

                                        _SplitMaterialSubLot_B = new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA();
                                        _SplitMaterialSubLot_B.POID = SourceMaterialSubLot.POID;
                                        _SplitMaterialSubLot_B.OPSGGUID = SourceMaterialSubLot.OPSGGUID;
                                        _SplitMaterialSubLot_B.OUTPUTGUID = SourceMaterialSubLot.OUTPUTGUID;
                                        _SplitMaterialSubLot_B.TARE.SetWeight(0, SourceMaterialSubLot.TARE.Uom, SourceMaterialSubLot.TARE.Precision);
                                        _SplitMaterialSubLot_B.GROSS.SetWeight(0, SourceMaterialSubLot.GROSS.Uom, SourceMaterialSubLot.GROSS.Precision);
                                        _SplitMaterialSubLot_B.TOTAL.SetWeight(0, SourceMaterialSubLot.TOTAL.Uom, SourceMaterialSubLot.TOTAL.Precision);

                                        OnPropertyChanged("SplitMaterialSubLot_A");
                                        OnPropertyChanged("SplitMaterialSubLot_B");

                                        CanSplitHLAB = true;
                                    }
                                }
                            }
                            ///

                            CommandResults["SearchWIPCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["SearchWIPCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["SearchWIPCommand"] = true;

                            IsBusy = false;
                        }
                    }
                }, arg =>
               {
                   return CommandCanExecutes.ContainsKey("SearchWIPCommand") ?
                       CommandCanExecutes["SearchWIPCommand"] : (CommandCanExecutes["SearchWIPCommand"] = true);
               });
            }
        }
        public ICommand CheckEMPTYVesselCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["CheckEMPTYVesselCommandAsync"].EnterAsync())
                    {
                        try
                        {

                            IsBusy = true;

                            CommandResults["CheckEMPTYVesselCommandAsync"] = false;
                            CommandCanExecutes["CheckEMPTYVesselCommandAsync"] = false;

                            ///
                            if (arg is string && _SplitMaterialSubLot_B != null)
                            {
                                string ibcno = arg as string;

                                _BR_BRS_SEL_VESSEL_Info.INDATAs.Clear();
                                _BR_BRS_SEL_VESSEL_Info.OUTDATAs.Clear();
                                _BR_BRS_SEL_VESSEL_Info.INDATAs.Add(new BR_BRS_SEL_VESSEL_Info.INDATA
                                {
                                    VESSELID = ibcno
                                });

                                if (await _BR_BRS_SEL_VESSEL_Info.Execute() && _BR_BRS_SEL_VESSEL_Info.OUTDATAs.Count == 1)
                                {
                                    EMPTY_TARE = _BR_BRS_SEL_VESSEL_Info.OUTDATAs[0].TAREWEIGHT.GetValueOrDefault();
                                    EMPTY_VesselID = ibcno;
                                }
                                else
                                {
                                    EMPTY_VesselID = "";
                                }
                            }
                            ///
                            CommandResults["CheckEMPTYVesselCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            EMPTY_VesselID = "";
                            CommandResults["CheckEMPTYVesselCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["CheckEMPTYVesselCommandAsync"] = true;

                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("SearchWIPCommand") ?
                        CommandCanExecutes["SearchWIPCommand"] : (CommandCanExecutes["SearchWIPCommand"] = true);
                });
            }
        }

        public ICommand SplictCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["SplictCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["SplictCommandAsync"] = false;
                            CommandCanExecutes["SplictCommandAsync"] = false;

                            ///

                            // 값이 있는지 확인
                            if (string.IsNullOrWhiteSpace(_SRC_VesselID) || string.IsNullOrWhiteSpace(_EMPTY_VesselID))
                                throw new Exception("용기번호가 없습니다.");
                            else if(_SplitMaterialSubLot_A.GROSS != null && _SplitMaterialSubLot_B.GROSS != null
                                        && (_SplitMaterialSubLot_A.GROSS.Value <= 0 || _SplitMaterialSubLot_B.GROSS.Value <= 0))
                                throw new Exception("내용물 무게가 없습니다.");
                            else if (_SplitMaterialSubLot_A.TARE != null && _SplitMaterialSubLot_B.TARE != null
                                        && (_SplitMaterialSubLot_A.TARE.Value <= 0 || _SplitMaterialSubLot_B.TARE.Value <= 0))
                                throw new Exception("용기 무게가 없습니다.");

                            // 전자서명 요청
                            var authHelper = new iPharmAuthCommandHelper();
                            authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Function,
                                Common.enumAccessType.Create,
                                string.Format("포장공정반제품분할"),
                                string.Format("포장공정반제품분할"),
                                false,
                                "OM_ProductionOrder_SUI",
                                "", null, null) == false)
                            {
                                throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                            }

                            // 반제품 분할
                            _BR_BRS_REG_MaterialSubLot_INV_Split.INDATAs.Clear();
                            _BR_BRS_REG_MaterialSubLot_INV_Split.INDATA_POs.Clear();

                            _BR_BRS_REG_MaterialSubLot_INV_Split.INDATA_POs.Add(new BR_BRS_REG_MaterialSubLot_INV_Split.INDATA_PO
                            {
                                POID = _SourceMaterialSubLot.POID,
                                OPSGGUID = _SourceMaterialSubLot.OPSGGUID,
                                OUTPUTGUID = _SourceMaterialSubLot.OUTPUTGUID,
                                USERID = AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_SUI")
                            });
                            // Source WIP
                            _BR_BRS_REG_MaterialSubLot_INV_Split.INDATAs.Add(new BR_BRS_REG_MaterialSubLot_INV_Split.INDATA
                            {
                                MSUBLOTID = _SplitMaterialSubLot_A.MSUBLOTID,
                                VESSELID = _SRC_VesselID,
                                MSUBLOTQTY = _SplitMaterialSubLot_A.GROSS.Value,
                                TAREWEIGHT = _SplitMaterialSubLot_A.TARE.Value
                            });
                            // Split WIP
                            _BR_BRS_REG_MaterialSubLot_INV_Split.INDATAs.Add(new BR_BRS_REG_MaterialSubLot_INV_Split.INDATA
                            {
                                MSUBLOTID = _SplitMaterialSubLot_B.MSUBLOTID,
                                VESSELID = _EMPTY_VesselID,
                                MSUBLOTQTY = _SplitMaterialSubLot_B.GROSS.Value,
                                TAREWEIGHT = _SplitMaterialSubLot_B.TARE.Value
                            });

                            if(await _BR_BRS_REG_MaterialSubLot_INV_Split.Execute())
                            {
                                GetavailableWIPList();
                                SourceMaterialSubLot = new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA();
                                SplitMaterialSubLot_A = new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA();
                                SplitMaterialSubLot_B = new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA();
                                SRC_VesselID = "";
                                EMPTY_VesselID = "";
                            }
                                

                            ///

                            CommandResults["SplictCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["SplictCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["SplictCommandAsync"] = true;
                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("SplictCommandAsync") ?
                        CommandCanExecutes["SplictCommandAsync"] : (CommandCanExecutes["SplictCommandAsync"] = true);
                });
            }
        }
        public ICommand LabelPrintCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["LabelPrintCommandAsync"].EnterAsync())
                    {
                        try
                        {

                            CommandResults["LabelPrintCommandAsync"] = false;
                            CommandCanExecutes["LabelPrintCommandAsync"] = false;

                            ///
                            IsBusy = true;

                            if (!string.IsNullOrWhiteSpace(curPrintName) && !curPrintName.Equals("N/A"))
                            {
                                foreach (var item in _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATAs)
                                {
                                    if (item.ISSELECTED)
                                    {
                                        _BR_PHR_SEL_PRINT_LabelImage.INDATAs.Clear();
                                        _BR_PHR_SEL_PRINT_LabelImage.OUTDATAs.Clear();
                                        _BR_PHR_SEL_PRINT_LabelImage.INDATAs.Add(new BR_PHR_SEL_PRINT_LabelImage.INDATA
                                        {
                                            ReportPath = "/Reports/Label/LABEL_INPROCESS_WIP",
                                            PrintName = _selectedPrint.PRINTERNAME,
                                            USERID = AuthRepositoryViewModel.Instance.LoginedUserID
                                        });

                                        _BR_PHR_SEL_PRINT_LabelImage.Parameterss.Add(new BR_PHR_SEL_PRINT_LabelImage.Parameters
                                        {
                                            ParamName = "VESSELID",
                                            ParamValue = item.VESSELID ?? ""
                                        });                                        

                                        await _BR_PHR_SEL_PRINT_LabelImage.Execute();
                                    }
                                }
                            }
                            else
                                throw new Exception("출력할 프린터정보가 없습니다.");

                            IsBusy = false;
                            ///

                            CommandResults["LabelPrintCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["LabelPrintCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["LabelPrintCommandAsync"] = true;
                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LabelPrintCommandAsync") ?
                        CommandCanExecutes["LabelPrintCommandAsync"] : (CommandCanExecutes["LabelPrintCommandAsync"] = true);
                });
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["SaveCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["SaveCommand"] = false;
                            CommandCanExecutes["SaveCommand"] = false;

                            ///
                            // 기록할 내용이 있는지 확인
                            int cnt = 0;
                            foreach (var item in _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATAs)
                            {
                                if (item.ISSELECTED)
                                    cnt++;
                            }
                            if (cnt == 0)
                                throw new Exception("기록할 내용이 없습니다.");

                            // 전자서명 요청
                            var authHelper = new iPharmAuthCommandHelper();
                            if (_mainWnd.CurrentInstruction.Raw.INSERTEDYN.Equals("Y") && _mainWnd.Phase.CurrentPhase.STATE.Equals("COMP")) // 값 수정
                            {

                                authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                                if (await authHelper.ClickAsync(
                                    Common.enumCertificationType.Function,
                                    Common.enumAccessType.Create,
                                    string.Format("기록값을 변경합니다."),
                                    string.Format("기록값 변경"),
                                    false,
                                    "OM_ProductionOrder_SUI",
                                    "", _mainWnd.CurrentInstruction.Raw.RECIPEISTGUID, null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }
                            }

                            authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Function,
                                Common.enumAccessType.Create,
                                string.Format("포장공정반제품분할"),
                                string.Format("포장공정반제품분할"),
                                false,
                                "OM_ProductionOrder_SUI",
                                "", null, null) == false)
                            {
                                throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                            }

                            // XML 변환
                            DataSet ds = new DataSet();
                            DataTable dt = new DataTable("DATA");
                            ds.Tables.Add(dt);

                            dt.Columns.Add(new DataColumn("용기번호"));
                            dt.Columns.Add(new DataColumn("내용물무게"));
                            dt.Columns.Add(new DataColumn("용기무게"));
                            dt.Columns.Add(new DataColumn("총무게"));

                            foreach (var item in _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATAs)
                            {
                                if(item.ISSELECTED)
                                {
                                    DataRow row = dt.NewRow();
                                    row["용기번호"] = item.VESSELID ?? "N/A";
                                    row["내용물무게"] = item.GROSS.WeightUOMString ?? "N/A";
                                    row["용기무게"] = item.TARE.WeightUOMString ?? "N/A";
                                    row["총무게"] = item.TOTAL.WeightUOMString ?? "N/A";
                                    dt.Rows.Add(row);
                                }
                            }

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
                            ///

                            CommandResults["SaveCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["SaveCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["SaveCommand"] = true;
                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("SaveCommand") ?
                        CommandCanExecutes["SaveCommand"] : (CommandCanExecutes["SaveCommand"] = true);
                });
            }
        }
        public ICommand NoSplitCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["NoSplitCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["NoSplitCommand"] = false;
                            CommandCanExecutes["NoSplitCommand"] = false;

                            ///
                            // 전자서명 요청
                            var authHelper = new iPharmAuthCommandHelper();
                            if (_mainWnd.CurrentInstruction.Raw.INSERTEDYN.Equals("Y") && _mainWnd.Phase.CurrentPhase.STATE.Equals("COMP")) // 값 수정
                            {

                                authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                                if (await authHelper.ClickAsync(
                                    Common.enumCertificationType.Function,
                                    Common.enumAccessType.Create,
                                    string.Format("기록값을 변경합니다."),
                                    string.Format("기록값 변경"),
                                    false,
                                    "OM_ProductionOrder_SUI",
                                    "", _mainWnd.CurrentInstruction.Raw.RECIPEISTGUID, null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }
                            }

                            authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Function,
                                Common.enumAccessType.Create,
                                string.Format("포장공정반제품분할"),
                                string.Format("포장공정반제품분할"),
                                false,
                                "OM_ProductionOrder_SUI",
                                "", null, null) == false)
                            {
                                throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                            }

                            // 지시문 결과 입력
                            _mainWnd.CurrentInstruction.Raw.ACTVAL = "반제품 분할 없음";
                            _mainWnd.CurrentInstruction.Raw.NOTE = null;
                            var result = await _mainWnd.Phase.RegistInstructionValue(_mainWnd.CurrentInstruction);

                            if (result != enumInstructionRegistErrorType.Ok)
                            {
                                throw new Exception(string.Format("값 등록 실패, ID={0}, 사유={1}", _mainWnd.CurrentInstruction.Raw.IRTGUID, result));
                            }

                            if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                            else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);
                            ///

                            CommandResults["NoSplitCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["NoSplitCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["NoSplitCommand"] = true;
                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("NoSplitCommand") ?
                        CommandCanExecutes["NoSplitCommand"] : (CommandCanExecutes["NoSplitCommand"] = true);
                });
            }
        }
        #endregion
        #region 3.Function
        private async void GetavailableWIPList()
        {
            try
            {
                _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.INDATAs.Clear();
                _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATAs.Clear();
                _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.INDATAs.Add(new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.INDATA
                {
                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID
                });

                if(await BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.Execute())
                {
                    int precision = 3;
                    decimal gross, tare;

                    foreach (var item in _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATAs)
                    {
                        precision = item.PRECISION.HasValue ? item.PRECISION.Value : 3;
                        tare = item.TAREWEIGHT.HasValue ? item.TAREWEIGHT.Value : -99999m;
                        gross = item.MSUBLOTQTY.HasValue ? item.MSUBLOTQTY.Value : -99999m;

                        item.TARE.SetWeight(tare, item.UOM, precision);
                        item.GROSS.SetWeight(gross, item.UOM, precision);
                        item.TOTAL.SetWeight(tare + gross, item.UOM, precision);
                    }

                    PRECISION_FORMAT = "F" + precision.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        private void setSplitMaterialSubLot()
        {
            try
            {
                decimal grosschk = _SourceMaterialSubLot.GROSS.Value - _EMPTY_GROSS;
                if (grosschk > 0)
                {
                    // B 내용 수정
                    _SplitMaterialSubLot_B.GROSS.Value = _EMPTY_GROSS;
                    _SplitMaterialSubLot_B.TARE.Value = _EMPTY_TARE;
                    _SplitMaterialSubLot_B.TOTAL.Value = _EMPTY_GROSS + _EMPTY_TARE;
                    
                    // A 내용 수정
                    _SplitMaterialSubLot_A.GROSS.Value = grosschk;
                    _SplitMaterialSubLot_A.TOTAL.Value = grosschk + _SplitMaterialSubLot_A.TARE.Value;

                    CanSplitHLAB = true;
                }
                else
                {
                    CanSplitHLAB = false;
                    throw new Exception("분할된 무게는 분할 전 수량 보다 많거나 같을 수 없습니다.");
                }

                OnPropertyChanged("SplitMaterialSubLot_A");
                OnPropertyChanged("SplitMaterialSubLot_B");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
