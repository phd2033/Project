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
            _SplitRslt = new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATACollection();
            _BR_PHR_SEL_System_Printer = new BR_PHR_SEL_System_Printer();
            _BR_BRS_REG_MaterialSubLot_INV_Split.OnExecuteCompleted += new DelegateExecuteCompleted(BR_BRS_REG_MaterialSubLot_INV_Split_OnExecuteCompleted);
        }
        #region VIEW
        private string _srcVesselId;
        public string srcVesselId
        {
            get { return _srcVesselId; }
            set
            {
                _srcVesselId = value;
                OnPropertyChanged("srcVesselId");
            }
        }
        private decimal _srcGross;
        public string srcGross
        {
            get { return _srcGross.ToString("N0"); }
            set
            {
                _srcGross = decimal.Parse(value);
                OnPropertyChanged("srcGross");
                OnPropertyChanged("srcTotal");
            }
        }
        private decimal _srcTare;
        public string srcTare
        {
            get { return _srcTare.ToString("N0"); }
            set
            {
                _srcTare = decimal.Parse(value);
                OnPropertyChanged("srcTare");
                OnPropertyChanged("srcTotal");
            }
        }
        public string srcTotal
        {
            get { return (_srcGross + _srcTare).ToString("N0"); }
        }

        private string _splitVesselId;
        public string splitVesselId
        {
            get { return _splitVesselId; }
            set
            {
                _splitVesselId = value;
                OnPropertyChanged("splitVesselId");
            }
        }
        private decimal _splitGross;
        public string splitGross
        {
            get { return _splitGross.ToString("N0"); }
            set
            {
                _splitGross = decimal.Parse(value);
                setSourceRemained();
                OnPropertyChanged("splitGross");
                OnPropertyChanged("splitTotal");
            }
        }
        private decimal _splitTare;
        public string splitTare
        {
            get { return _splitTare.ToString("N0"); }
            set
            {
                _splitTare = decimal.Parse(value);
                setSourceRemained();
                OnPropertyChanged("splitTare");
                OnPropertyChanged("splitTotal");
            }
        }
        public string splitTotal
        {
            get { return (_splitGross + _splitTare).ToString("N0"); }           
        }
        #endregion
        private BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA _selectedSourceWIP;
        public BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA selectedSourceWIP
        {
            get { return _selectedSourceWIP; }
            set
            {
                _selectedSourceWIP = value;
                _SourceWIP = _selectedSourceWIP;
                srcVesselId = _SourceWIP.VESSELID;
                srcGross = _SourceWIP.MSUBLOTQTY.ToString();
                srcTare = _SourceWIP.TAREWEIGHT.ToString();
                _SplitWIP = new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA { VESSELID = "", MSUBLOTQTY = 0, TAREWEIGHT = 0 };
                OnPropertyChanged("selectedSourceWIP");
            }
        }
        private BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA _SourceWIP;
        private BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA _SplitWIP;
        private BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATACollection _SplitRslt;
        public BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATACollection SplitRslt
        {
            get { return _SplitRslt; }
            set
            {
                _SplitRslt = value;
                OnPropertyChanged("SplitRslt");
            }
        }
        private BR_PHR_SEL_System_Printer.OUTDATA _selectedPrint;
        private BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATACollection _rsltWIPs;
        public BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATACollection rsltWIPs
        {
            get { return _rsltWIPs; }
            set
            {
                _rsltWIPs = value;
                OnPropertyChanged("rsltWIPs");
            }
        }

        #endregion
        #region 1.BizRule
        /// <summary>
        /// 포장반제품 조회
        /// </summary>
        private BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT = new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT();
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
        private BR_PHR_SEL_System_Printer _BR_PHR_SEL_System_Printer = new BR_PHR_SEL_System_Printer();
        /// <summary>
        /// 라벨 출력
        /// </summary>
        private BR_PHR_SEL_PRINT_LabelImage _BR_PHR_SEL_PRINT_LabelImage = new BR_PHR_SEL_PRINT_LabelImage();        
        /// <summary>
        /// 반제품 분할
        /// </summary>
        private BR_BRS_REG_MaterialSubLot_INV_Split _BR_BRS_REG_MaterialSubLot_INV_Split = new BR_BRS_REG_MaterialSubLot_INV_Split();

        private DataTable _CurrentWIPList;
        public DataTable CurrentWIPList
        {
            get { return _CurrentWIPList; }
            set
            {
                _CurrentWIPList = value;
                OnPropertyChanged("CurrentWIPList");
            }
        }
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
                                ROOMID = AuthRepositoryViewModel.Instance.RoomID                                 
                            });

                            if(await _BR_PHR_SEL_System_Printer.Execute() && _BR_PHR_SEL_System_Printer.OUTDATAs.Count > 0)                            
                                _selectedPrint = _BR_PHR_SEL_System_Printer.OUTDATAs[0];                            

                            // 사용가능한 포장 반제품 조회
                            _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.INDATAs.Clear();
                            _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATAs.Clear();
                            _BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.INDATAs.Add(new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.INDATA
                            {
                                POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID
                            });

                            await BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.Execute();
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
                            // 분할 내용 확인
                            if (string.IsNullOrWhiteSpace(srcVesselId) || string.IsNullOrWhiteSpace(srcGross) || string.IsNullOrWhiteSpace(srcTare)
                                || string.IsNullOrWhiteSpace(splitVesselId) || string.IsNullOrWhiteSpace(splitGross) || string.IsNullOrWhiteSpace(splitTare))
                                throw new Exception("분할 정보에 없는 값이 있습니다.");

                            // MM_MaterialLot_SplitMerge(Lot 분할 및 합침) 권한 확인
                                if (AuthRepositoryViewModel.HaveFunctionCodeAuthority("MM_MaterialLot_SplitMerge", Common.enumAccessType.Create))
                            {
                                // 전자서명
                                var authHelper = new iPharmAuthCommandHelper();
                                authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "MM_MaterialLot_SplitMerge");
                                if (await authHelper.ClickAsync(
                                    Common.enumCertificationType.Role,
                                    Common.enumAccessType.Create,
                                    "반제품 분할",
                                    "반제품 분할",
                                    false,
                                    "MM_MaterialLot_SplitMerge",
                                    "",
                                    null, null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }

                                // 반제품 분할
                                _BR_BRS_REG_MaterialSubLot_INV_Split.INDATAs.Clear();
                                _BR_BRS_REG_MaterialSubLot_INV_Split.INDATA_POs.Clear();

                                _BR_BRS_REG_MaterialSubLot_INV_Split.INDATA_POs.Add(new BR_BRS_REG_MaterialSubLot_INV_Split.INDATA_PO
                                {
                                    POID = _selectedSourceWIP.POID,
                                    OPSGGUID = _selectedSourceWIP.OPSGGUID,
                                    OUTPUTGUID = _selectedSourceWIP.OUTPUTGUID,
                                    USERID = AuthRepositoryViewModel.GetUserIDByFunctionCode("MM_MaterialLot_SplitMerge")
                                });
                                // Source WIP
                                _BR_BRS_REG_MaterialSubLot_INV_Split.INDATAs.Add(new BR_BRS_REG_MaterialSubLot_INV_Split.INDATA
                                {
                                    MSUBLOTID = _SourceWIP.MSUBLOTID,
                                    VESSELID = _SourceWIP.VESSELID,
                                    MSUBLOTQTY = _SourceWIP.MSUBLOTQTY,
                                    TAREWEIGHT = _SourceWIP.TAREWEIGHT
                                });
                                // Split WIP
                                _BR_BRS_REG_MaterialSubLot_INV_Split.INDATAs.Add(new BR_BRS_REG_MaterialSubLot_INV_Split.INDATA
                                {
                                    MSUBLOTID = _SplitWIP.MSUBLOTID,
                                    VESSELID = _SplitWIP.VESSELID,
                                    MSUBLOTQTY = _SplitWIP.MSUBLOTQTY,
                                    TAREWEIGHT = _SplitWIP.TAREWEIGHT
                                });

                                //await _BR_BRS_REG_MaterialSubLot_INV_Split.Execute();
                            }
                            else
                                OnMessage("해당 기능을 수행하기 위한 권한이 없습니다.");

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

                            if (!string.IsNullOrWhiteSpace(_selectedPrint.PRINTERNAME))
                            {

                                _BR_PHR_SEL_PRINT_LabelImage.INDATAs.Clear();
                                _BR_PHR_SEL_PRINT_LabelImage.OUTDATAs.Clear();
                                _BR_PHR_SEL_PRINT_LabelImage.INDATAs.Add(new BR_PHR_SEL_PRINT_LabelImage.INDATA
                                {
                                    ReportPath = "/Reports/Label/LABEL_RELEASE",
                                    PrintName = _selectedPrint.PRINTERNAME,
                                    USERID = AuthRepositoryViewModel.Instance.LoginedUserID
                                });

                                _BR_PHR_SEL_PRINT_LabelImage.Parameterss.Add(new BR_PHR_SEL_PRINT_LabelImage.Parameters
                                {
                                    ParamName = "MSUBLOTBCD",
                                    ParamValue = ""
                                });
                                _BR_PHR_SEL_PRINT_LabelImage.Parameterss.Add(new BR_PHR_SEL_PRINT_LabelImage.Parameters
                                {
                                    ParamName = "OPERATOR",
                                    ParamValue = AuthRepositoryViewModel.Instance.LoginedUserID
                                });

                                //await _BR_PHR_SEL_PRINT_LabelImage.Execute();
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

                            foreach (var item in SplitRslt)
                            {
                                DataRow row = dt.NewRow();
                                row["용기번호"] = item.VESSELID ?? "N/A";
                                row["내용물무게"] = item.MSUBLOTQTY.HasValue ? item.MSUBLOTQTY.Value.ToString("N0") : "N/A";
                                row["용기무게"] = item.TAREWEIGHT.HasValue ? item.TAREWEIGHT.Value.ToString("N0") : "N/A";
                                row["총무게"] = (item.MSUBLOTQTY.Value + item.TAREWEIGHT.Value).ToString("N0") ?? "N/A";
                                dt.Rows.Add(row);
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
        public void setSourceRemained()
        {         
            _srcGross = selectedSourceWIP.MSUBLOTQTY.Value - _splitGross;            
            OnPropertyChanged("srcGross");
            OnPropertyChanged("srcTotal");
            OnPropertyChanged("splitGross");
        }
        private async void BR_BRS_REG_MaterialSubLot_INV_Split_OnExecuteCompleted(string ruleName)
        {
            if(_BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATAs.Count > 0)
            {
                if(await OnMessageAsync("라벨을 출력하시겠습니까?"))
                {

                }

                // 수행 결과 GRID로 이동
                foreach (var item in _BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATAs)
                {
                    SplitRslt.Add(new BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA
                    {
                        MSUBLOTID = item.MSUBLOTID,
                        MSUBLOTQTY = item.MSUBLOTQTY,
                        TAREWEIGHT = item.TAREWEIGHT,
                        VESSELID = item.VESSELID,
                        MSUBLOTBCD = item.MSUBLOTBCD

                    });
                } 
            }
        }
        #endregion
    }
}
