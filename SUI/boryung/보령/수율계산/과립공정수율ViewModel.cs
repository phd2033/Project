﻿using LGCNS.iPharmMES.Common;
using Order;
using System;
using System.Windows.Input;
using System.Linq;
using ShopFloorUI;
using System.Text;
using C1.Silverlight.Data;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using C1.Silverlight.Imaging;

namespace 보령
{
    public class 과립공정수율ViewModel : ViewModelBase
    {
        #region Properties

        과립공정수율 _mainWndXml;
        private int PRECISION;


        ShopFloorCustomWindow _mainWnd;
        public ShopFloorCustomWindow MainWnd
        {
            get { return _mainWnd; }
            set { _mainWnd = value; }
        }

        BR_BRS_SEL_Yield_Calculation_Granule _BR_BRS_SEL_Yield_Calculation_Granule;
        public BR_BRS_SEL_Yield_Calculation_Granule BR_BRS_SEL_Yield_Calculation_Granule
        {
            get { return _BR_BRS_SEL_Yield_Calculation_Granule; }
            set { _BR_BRS_SEL_Yield_Calculation_Granule = value; }
        }

        private string _result_IN;
        public string Result_IN
        {
            get { return _result_IN; }
            set
            {
                _result_IN = value;
                NotifyPropertyChanged();
            }
        }

        private string _result_OUT;
        public string Result_OUT
        {
            get { return _result_OUT; }
            set
            {
                _result_OUT = value;
                NotifyPropertyChanged();
            }
        }

        private decimal? _result_SUM;
        public decimal? Result_SUM
        {
            get { return _result_SUM; }
            set
            {
                _result_SUM = value;
                NotifyPropertyChanged();
            }
        }

        private string _LossQty;
        public string LossQty
        {
            get { return _LossQty; }
            set
            {
                _LossQty = value;
                NotifyPropertyChanged();
            }
        }
      


        #endregion


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

                            if (arg != null)
                            {
                                MainWnd = arg as ShopFloorCustomWindow;

                                PRECISION = MainWnd.CurrentInstruction.Raw.PRECISION.HasValue ? MainWnd.CurrentInstruction.Raw.PRECISION.Value : 1;

                                _BR_BRS_SEL_Yield_Calculation_Granule.INDATAs.Clear();
                                _BR_BRS_SEL_Yield_Calculation_Granule.OUTDATAs.Clear();
                                _BR_BRS_SEL_Yield_Calculation_Granule.INDATAs.Add(new BR_BRS_SEL_Yield_Calculation_Granule.INDATA()
                                {
                                    MTRLID = _mainWnd.CurrentOrder.MaterialID,
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID
                                });

                                await _BR_BRS_SEL_Yield_Calculation_Granule.Execute();


                                if (_BR_BRS_SEL_Yield_Calculation_Granule.OUTDATAs.Count > 0)
                                {
                                    //과립공정수율 = (과립반제품/(분쇄반제품+과립액원료)) * 100
                                    Result_IN = _BR_BRS_SEL_Yield_Calculation_Granule.OUTDATAs[0].IN_VALUE;
                                    Result_OUT = _BR_BRS_SEL_Yield_Calculation_Granule.OUTDATAs[0].OUT_VALUE;
                                    Result_SUM = _BR_BRS_SEL_Yield_Calculation_Granule.OUTDATAs[0].SUM.HasValue ? MathExt.Round(_BR_BRS_SEL_Yield_Calculation_Granule.OUTDATAs[0].SUM.Value, PRECISION, MidpointRounding.AwayFromZero) : 0m;
                                    LossQty = _BR_BRS_SEL_Yield_Calculation_Granule.OUTDATAs[0].LOSSQTY;
                                }
                            }


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

                            ///
                            if (arg != null)
                            {
                                _mainWndXml = arg as 과립공정수율;

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

                                var outputValues = InstructionModel.GetResultReceiver(_mainWnd.CurrentInstruction, _mainWnd.Instructions);

                                authHelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "OM_ProductionOrder_Yield");

                                if (await authHelper.ClickAsync(
                                    Common.enumCertificationType.Function,
                                    Common.enumAccessType.Create,
                                    "과립 공정 수율 기록",
                                    "과립 공정 수율 기록",
                                    false,
                                    "OM_ProductionOrder_Yield",
                                    "", null, null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }

                                Brush background = _mainWndXml.PrintArea.Background;
                                _mainWndXml.PrintArea.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xD6, 0xD4, 0xD4));
                                _mainWndXml.PrintArea.BorderThickness = new System.Windows.Thickness(1);
                                _mainWndXml.PrintArea.Background = new SolidColorBrush(Colors.White);


                                _mainWnd.CurrentInstruction.Raw.ACTVAL = Result_SUM.ToString();
                                _mainWnd.CurrentInstruction.Raw.NOTE = imageToByteArray();

                                var result = await _mainWnd.Phase.RegistInstructionValue(_mainWnd.CurrentInstruction, false, false, true);
                                if (result != enumInstructionRegistErrorType.Ok)
                                {
                                    throw new Exception(string.Format("값 등록 실패, ID={0}, 사유={1}", _mainWnd.CurrentInstruction.Raw.IRTGUID, result));
                                }

                                foreach (var item in outputValues)
                                {
                                    item.Raw.ACTVAL = _mainWnd.CurrentInstruction.Raw.ACTVAL;

                                    result = await _mainWnd.Phase.RegistInstructionValue(item);
                                    if (result != enumInstructionRegistErrorType.Ok)
                                    {
                                        throw new Exception(string.Format("값 등록 실패, ID={0}, 사유={1}", item.Raw.IRTGUID, result));
                                    }
                                }

                                if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                                else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);
                            }
                            ///

                            CommandResults["ConfirmCommand"] = true;
                        }
                        catch (Exception ex)
                        {
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


        public byte[] imageToByteArray()
        {
            try
            {

                C1Bitmap bitmap = new C1Bitmap(new WriteableBitmap(_mainWndXml.PrintArea, null));
                System.IO.Stream stream = bitmap.GetStream(ImageFormat.Png, true);

                int len = (int)stream.Seek(0, SeekOrigin.End);

                byte[] datas = new byte[len];

                stream.Seek(0, SeekOrigin.Begin);

                stream.Read(datas, 0, datas.Length);

                return datas;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public 과립공정수율ViewModel()
        {
            _BR_BRS_SEL_Yield_Calculation_Granule = new BR_BRS_SEL_Yield_Calculation_Granule();
            //  Is_EnableOKBtn = false;
        }



    }
}


