using C1.Silverlight.Excel;
using LGCNS.iPharmMES.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Board
{
    public class TstRequestConfirmViewModel : ViewModelBase
    {
        #region ##### property ##### 
        private TstRequestConfirmViewModel  _mainWnd;


        private DateTime _PeriodSTDTTM;
        public DateTime PeriodSTDTTM
        {
            get { return _PeriodSTDTTM; }
            set
            {
                _PeriodSTDTTM = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _PeriodEDDTTM;
        public DateTime PeriodEDDTTM
        {
            get { return _PeriodEDDTTM; }
            set
            {
                _PeriodEDDTTM = value;
                NotifyPropertyChanged();
            }
        }

        #endregion ##### property #####

        #region [BizRule]

        // 시험의뢰 적합여부 확인
        private BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM _BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM;
        public BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM
        {
            get { return _BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM; }
            set
            {
                _BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM = value;
                OnPropertyChanged("BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM");
            }
        }
        #endregion

        public TstRequestConfirmViewModel()
        {
            _BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM = new BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM();
        }

        public ICommand LoadedCommandAsync
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
                            
                            _mainWnd = arg as TstRequestConfirmViewModel;

                            PeriodEDDTTM = await AuthRepositoryViewModel.GetDBDateTimeNow();
                            PeriodSTDTTM = PeriodEDDTTM.AddDays(-1);
                            
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

        public ICommand BtnSearchCommand
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["SearchCommand"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["SearchCommand"] = false;
                            CommandCanExecutes["SearchCommand"] = false;

                            _BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM.INDATAs.Clear();
                            _BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM.OUTDATAs.Clear();

                            _BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM.INDATAs.Add(new BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM.INDATA()
                            {
                                FROMDATE = PeriodSTDTTM,
                                TODATE = PeriodEDDTTM
                            });

                            await _BR_BRS_SEL_TST_REQUEST_NUMBER_CONFIRM.Execute();

                            CommandResults["SearchCommand"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["SearchCommand"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["SearchCommand"] = true;

                            IsBusy = false;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("SearchCommand") ?
                        CommandCanExecutes["SearchCommand"] : (CommandCanExecutes["SearchCommand"] = true);
                });
            }
        }
    }
}
