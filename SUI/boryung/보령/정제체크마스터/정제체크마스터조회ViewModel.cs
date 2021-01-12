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
using System.Collections.Generic;

namespace 보령
{
    public class 정제체크마스터조회ViewModel : ViewModelBase
    {
        #region [Property]
        private string _EqptId;
        public string EqptId
        {
            get { return _EqptId; }
            set
            {
                _EqptId = value;
                NotifyPropertyChanged();
            }
        }

        private string _EqptName;
        public string EqptName
        {
            get { return _EqptName; }
            set
            {
                _EqptName = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _FromDatetime;
        public DateTime FromDatetime
        {
            get { return _FromDatetime; }
            set
            {
                _FromDatetime = value;
                OnPropertyChanged("FromDatetime");
            }
        }

        private DateTime _ToDatetime;
        public DateTime ToDatetime
        {
            get { return _ToDatetime; }
            set
            {
                _ToDatetime = value;
                OnPropertyChanged("ToDatetime");
            }
        }

        private bool _ISBUSY;
        public bool ISBUSY
        {
            get { return _ISBUSY; }
            set
            {
                _ISBUSY = value;
                OnPropertyChanged("ISBUSY");
            }
        }

        정제체크마스터조회 _mainWnd;
        #endregion

        #region [Bizrule]
        private BR_PHR_SEL_CODE _BR_PHR_SEL_CODE;
        public BR_PHR_SEL_CODE BR_PHR_SEL_CODE
        {
            get { return _BR_PHR_SEL_CODE; }
            set
            {
                _BR_PHR_SEL_CODE = value;
                NotifyPropertyChanged();
            }
        }

        private BR_BRS_GET_Selector_Check_Master _BR_BRS_GET_Selector_Check_Master;
        public BR_BRS_GET_Selector_Check_Master BR_BRS_GET_Selector_Check_Master
        {
            get { return _BR_BRS_GET_Selector_Check_Master; }
            set
            {
                _BR_BRS_GET_Selector_Check_Master = value;
                NotifyPropertyChanged();
            }
        }

        private BR_PHR_GET_HOUR_MINUTE_CBO _BR_PHR_GET_HOUR_MINUTE_CBO;
        public BR_PHR_GET_HOUR_MINUTE_CBO BR_PHR_GET_HOUR_MINUTE_CBO
        {
            get { return _BR_PHR_GET_HOUR_MINUTE_CBO; }
            set
            {
                _BR_PHR_GET_HOUR_MINUTE_CBO = value;
                NotifyPropertyChanged();
            }
        }

        private BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_HOUR _FromHour;
        public BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_HOUR FromHour
        {
            get { return _FromHour; }
            set
            {
                _FromHour = value;
                OnPropertyChanged("FromHour");
            }
        }

        private BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_MINUTE _FromMin;
        public BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_MINUTE FromMin
        {
            get { return _FromMin; }
            set
            {
                _FromMin = value;
                OnPropertyChanged("FromMin");
            }
        }

        private BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_HOUR _ToHour;
        public BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_HOUR ToHour
        {
            get { return _ToHour; }
            set
            {
                _ToHour = value;
                OnPropertyChanged("ToHour");
            }
        }

        private BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_MINUTE _ToMin;
        public BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_MINUTE ToMin
        {
            get { return _ToMin; }
            set
            {
                _ToMin = value;
                OnPropertyChanged("ToMin");
            }
        }

        private BR_BRS_REG_IPC_CHECKMASTER_MULTI _BR_BRS_REG_IPC_CHECKMASTER_MULTI;
        public BR_BRS_REG_IPC_CHECKMASTER_MULTI BR_BRS_REG_IPC_CHECKMASTER_MULTI
        {
            get { return _BR_BRS_REG_IPC_CHECKMASTER_MULTI; }
            set
            {
                _BR_BRS_REG_IPC_CHECKMASTER_MULTI = value;
                OnPropertyChanged("BR_BRS_REG_IPC_CHECKMASTER_MULTI");
            }
        }
        #endregion

        #region [Command]
        public ICommand LoadCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["LoadCommand"].EnterAsync())
                    {
                        try
                        {

                            CommandResults["LoadCommand"] = false;
                            CommandCanExecutes["LoadCommand"] = false;

                            ///
                            if (arg == null || !(arg is 정제체크마스터조회))
                                return;

                            _mainWnd = arg as 정제체크마스터조회;                            

                            await BR_PHR_GET_HOUR_MINUTE_CBO.Execute();

                            ToDatetime = await AuthRepositoryViewModel.GetDBDateTimeNow();
                            FromDatetime = ToDatetime.AddHours(-1);

                            DateSet(FromDatetime, ToDatetime);

                            EqptId = String.Empty;
                            _mainWnd.Search.IsEnabled = false;
                            _mainWnd.Record.IsEnabled = false;
                            ///

                            CommandResults["LoadCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["LoadCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["LoadCommand"] = true;

                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadCommand") ?
                        CommandCanExecutes["LoadCommand"] : (CommandCanExecutes["LoadCommand"] = true);
                });
            }
        }

        public ICommand EqptSearchCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["EqptSearchCommand"].EnterAsync())
                    {
                        try
                        {

                            CommandResults["EqptSearchCommand"] = false;
                            CommandCanExecutes["EqptSearchCommand"] = false;

                            EqptId = _mainWnd.txtEqptId.Text;
                            ///
                            if (String.IsNullOrWhiteSpace(EqptId)) return;

                            BR_PHR_SEL_CODE.INDATAs.Clear();
                            BR_PHR_SEL_CODE.OUTDATAs.Clear();
                            BR_PHR_SEL_CODE.INDATAs.Add(new BR_PHR_SEL_CODE.INDATA()
                            {
                                TYPE = "Equipment",
                                LANGID = "ko-KR",
                                CODE = EqptId
                            });

                            await BR_PHR_SEL_CODE.Execute();

                            if (BR_PHR_SEL_CODE.OUTDATAs.Count != 0)
                            {
                                EqptName = BR_PHR_SEL_CODE.OUTDATAs[0].NAME;
                                _mainWnd.Search.IsEnabled = true;
                            }
                            ///

                            CommandResults["EqptSearchCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["EqptSearchCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["EqptSearchCommand"] = true;

                            //isBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("EqptSearchCommand") ?
                        CommandCanExecutes["EqptSearchCommand"] : (CommandCanExecutes["EqptSearchCommand"] = true);
                });
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["SearchCommand"].EnterAsync())
                    {
                        try
                        {

                            CommandResults["SearchCommand"] = false;
                            CommandCanExecutes["SearchCommand"] = false;

                            ///
                            _mainWnd.BusyIndicator.IsBusy = true;

                            FromDatetime = ((DateTime)_mainWnd.From.DateTime).Date.AddMinutes((Int32.Parse(FromHour.HOUR) * 60) + Int32.Parse(FromMin.MINUTE));
                            ToDatetime = ((DateTime)_mainWnd.To.DateTime).Date.AddMinutes((Int32.Parse(ToHour.HOUR) * 60) + Int32.Parse(ToMin.MINUTE));

                            DateValidation();

                            BR_BRS_GET_Selector_Check_Master.INDATAs.Clear();
                            BR_BRS_GET_Selector_Check_Master.OUTDATAs.Clear();
                            BR_BRS_GET_Selector_Check_Master.INDATAs.Add(new BR_BRS_GET_Selector_Check_Master.INDATA
                            {
                                EQPTID = EqptId,
                                FROMDATETIME = FromDatetime.ToString("yyyy-MM-dd HH:mm:ss"),
                                TODATETIME = ToDatetime.ToString("yyyy-MM-dd HH:mm:ss")
                            });

                            await BR_BRS_GET_Selector_Check_Master.Execute();

                            if (BR_BRS_GET_Selector_Check_Master.OUTDATAs.Count != 0)
                            {
                                foreach (var item in BR_BRS_GET_Selector_Check_Master.OUTDATAs)
                                {
                                    if (item.FLAG.Equals("NG"))
                                    {
                                        _mainWnd.Record.IsEnabled = false;
                                        return;
                                    }
                                    _mainWnd.Record.IsEnabled = true;
                                }
                            }
                            ///

                            CommandResults["SearchCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["SearchCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            _mainWnd.BusyIndicator.IsBusy = false;
                            CommandCanExecutes["SearchCommand"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("SearchCommand") ?
                        CommandCanExecutes["SearchCommand"] : (CommandCanExecutes["SearchCommand"] = true);
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

                            CommandResults["RecordCommand"] = false;
                            CommandCanExecutes["RecordCommand"] = false;

                            ///
                            _mainWnd.BusyIndicator.IsBusy = true;

                            _BR_BRS_REG_IPC_CHECKMASTER_MULTI.INDATAs.Clear();

                            if (BR_BRS_GET_Selector_Check_Master.OUTDATAs.Count > 0)
                            {
                                var authHelper = new iPharmAuthCommandHelper(); // 전자서명 후 BR 실행
                                authHelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                                if (await authHelper.ClickAsync(
                                    Common.enumCertificationType.Function,
                                    Common.enumAccessType.Create,
                                    string.Format("정제체크마스터조회"),
                                    string.Format("정제체크마스터조회"),
                                    false,
                                    "OM_ProductionOrder_SUI",
                                    _mainWnd.CurrentOrderInfo.EquipmentID, _mainWnd.CurrentOrderInfo.RecipeID, null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }

                                // BR_BRS_REG_IPC_CHECKMASTER_MULTI BR Execute

                                foreach (var item in BR_BRS_GET_Selector_Check_Master.OUTDATAs)
                                {
                                    _BR_BRS_REG_IPC_CHECKMASTER_MULTI.INDATAs.Add(new BR_BRS_REG_IPC_CHECKMASTER_MULTI.INDATA
                                    {
                                        EQPTID = item.EQPTID != null ? item.EQPTID : "",
                                        POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                        OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                        SMPQTY = item.SMPQTY,
                                        USERID = AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_SUI"),
                                        STRTDTTM = item.STDTTM,
                                        LOCATIONID = AuthRepositoryViewModel.Instance.RoomID,
                                        AVG_WEIGHT = item.AVG_WEIGHT != null ? item.AVG_WEIGHT : "",
                                        AVG_THICKNESS = item.AVG_THICK != null ? item.AVG_THICK : "",
                                        AVG_HARDNESS = item.AVG_HARDNESS != null ? item.AVG_HARDNESS : "",
                                        AVG_DIAMETER = item.AVG_DIAMETER != null ? item.AVG_DIAMETER : ""
                                    });
                                }

                                if (await _BR_BRS_REG_IPC_CHECKMASTER_MULTI.Execute())
                                {

                                    DataSet ds = new DataSet();
                                    DataTable dt = new DataTable("DATA");
                                    ds.Tables.Add(dt);

                                    dt.Columns.Add(new DataColumn("장비번호"));
                                    dt.Columns.Add(new DataColumn("점검일시"));
                                    dt.Columns.Add(new DataColumn("평균질량"));
                                    dt.Columns.Add(new DataColumn("개별최소질량"));
                                    dt.Columns.Add(new DataColumn("개별최대질량"));
                                    dt.Columns.Add(new DataColumn("평균두께"));
                                    dt.Columns.Add(new DataColumn("최소두께"));
                                    dt.Columns.Add(new DataColumn("최대두께"));
                                    dt.Columns.Add(new DataColumn("평균경도"));
                                    dt.Columns.Add(new DataColumn("최소경도"));
                                    dt.Columns.Add(new DataColumn("최대경도"));
                                    dt.Columns.Add(new DataColumn("평균직경"));
                                    dt.Columns.Add(new DataColumn("최소직경"));
                                    dt.Columns.Add(new DataColumn("최대직경"));

                                    foreach (var rowdata in BR_BRS_GET_Selector_Check_Master.OUTDATAs)
                                    {
                                        var row = dt.NewRow();
                                        row["장비번호"] = rowdata.EQPTID;
                                        row["점검일시"] = rowdata.STDATETIME != null ? rowdata.STDATETIME : "";
                                        row["평균질량"] = rowdata.AVG_WEIGHT != null ? rowdata.AVG_WEIGHT : "";
                                        row["개별최소질량"] = rowdata.MIN_WEIGHT != null ? rowdata.MIN_WEIGHT : "";
                                        row["개별최대질량"] = rowdata.MAX_WEIGHT != null ? rowdata.MAX_WEIGHT : "";
                                        row["평균두께"] = rowdata.AVG_THICK != null ? rowdata.AVG_THICK : "";
                                        row["최소두께"] = rowdata.AVG_THICK != null ? rowdata.MIN_THICK : "";
                                        row["최대두께"] = rowdata.AVG_THICK != null ? rowdata.MAX_THICK : "";
                                        row["평균경도"] = rowdata.AVG_HARDNESS != null ? rowdata.AVG_HARDNESS : "";
                                        row["최소경도"] = rowdata.AVG_HARDNESS != null ? rowdata.MIN_HARDNESS : "";
                                        row["최대경도"] = rowdata.AVG_HARDNESS != null ? rowdata.MAX_HARDNESS : "";
                                        row["평균직경"] = rowdata.AVG_DIAMETER != null ? rowdata.AVG_DIAMETER : "";
                                        row["최소직경"] = rowdata.AVG_DIAMETER != null ? rowdata.MIN_DIAMETER : "";
                                        row["최대직경"] = rowdata.AVG_DIAMETER != null ? rowdata.MAX_DIAMETER : "";

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
                            }
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
                            _mainWnd.BusyIndicator.IsBusy = false;
                            CommandCanExecutes["RecordCommand"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("RecordCommand") ?
                        CommandCanExecutes["RecordCommand"] : (CommandCanExecutes["RecordCommand"] = true);
                });
            }
        }
        

        #endregion

        public 정제체크마스터조회ViewModel()
        {            
            _BR_PHR_SEL_CODE = new BR_PHR_SEL_CODE();
            _BR_BRS_GET_Selector_Check_Master = new BR_BRS_GET_Selector_Check_Master();
            _BR_PHR_GET_HOUR_MINUTE_CBO = new BR_PHR_GET_HOUR_MINUTE_CBO();
            _FromHour = new BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_HOUR();
            _FromMin = new BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_MINUTE();
            _ToHour = new BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_HOUR();
            _ToMin = new BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_MINUTE();
            _BR_BRS_REG_IPC_CHECKMASTER_MULTI = new BR_BRS_REG_IPC_CHECKMASTER_MULTI();
        }

        private void DateValidation()
        {
            int res = DateTime.Compare(FromDatetime, ToDatetime);

            if (res < 0)
                return;
            else if(res > 0)
            {
                DateTime temp = ToDatetime;
                ToDatetime = FromDatetime;
                FromDatetime = temp;

                _mainWnd.From.DateTime = FromDatetime;
                _mainWnd.To.DateTime = ToDatetime;

                DateSet(FromDatetime, ToDatetime);

                return;
            }
        }

        private void DateSet(DateTime From, DateTime To)
        {
            foreach (var item in BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_HOURs)
            {
                if (item.HOUR.Equals(FromDatetime.Hour.ToString("D2")))
                    _mainWnd.FromHour.SelectedItem = item;
                if (item.HOUR.Equals(ToDatetime.Hour.ToString("D2")))
                    _mainWnd.ToHour.SelectedItem = item;
            }
            foreach (var item in BR_PHR_GET_HOUR_MINUTE_CBO.OUTDATA_MINUTEs)
            {
                if (item.MINUTE.Equals(FromDatetime.Minute.ToString("D2")))
                    _mainWnd.FromMin.SelectedItem = item;
                if (item.MINUTE.Equals(ToDatetime.Minute.ToString("D2")))
                    _mainWnd.ToMin.SelectedItem = item;
            }

        }
    }
}
