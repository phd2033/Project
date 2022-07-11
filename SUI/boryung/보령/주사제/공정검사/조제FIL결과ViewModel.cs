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
using C1.Silverlight.Data;
using ShopFloorUI;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using C1.Silverlight.Imaging;
using System.Windows.Media.Imaging;
using System.IO;

namespace 보령
{
    public class 조제FIL결과ViewModel : ViewModelBase
    {
        private 조제FIL결과 _mainWnd;

        #region [Property]
        private string _curLotNo;
        public string curLotNo
        {
            get { return _curLotNo; }
            set
            {
                _curLotNo = value;
                OnPropertyChanged("curLotNo");
            }
        }

        private decimal _curUnderVal;
        public decimal curUnderVal
        {
            get { return _curUnderVal; }
            set
            {
                _curUnderVal = value;
                OnPropertyChanged("curUnderVal");
            }
        }
        private decimal _curFilCount;
        public decimal curFilCount
        {
            get { return _curFilCount; }
            set
            {
                _curFilCount = value;
                OnPropertyChanged("curFilCount");
            }
        }
        private decimal _curUpperVal;
        public decimal curUpperVal
        {
            get { return _curUpperVal; }
            set
            {
                _curUpperVal = value;
                OnPropertyChanged("curUpperVal");
            }
        }
        private string _curResult;
        public string curResult
        {
            get { return _curResult; }
            set
            {
                _curResult = value;
                OnPropertyChanged("curResult");
            }
        }

        private bool _isFilCountEnable;
        public bool isFilCountEnable
        {
            get { return _isFilCountEnable; }
            set
            {
                _isFilCountEnable = value;
                OnPropertyChanged("isFilCountEnable");
            }
        }

        private bool _isConfirmEnable;
        public bool isConfirmEnable
        {
            get { return _isConfirmEnable; }
            set
            {
                _isConfirmEnable = value;
                OnPropertyChanged("isConfirmEnable");
            }
        }

        private bool _isSaveEnable;
        public bool isSaveEnable
        {
            get { return _isSaveEnable; }
            set
            {
                _isSaveEnable = value;
                OnPropertyChanged("isSaveEnable");
            }
        }

        private bool _isLotNoEnable;
        public bool isLotNoEnable
        {
            get { return _isLotNoEnable; }
            set
            {
                _isLotNoEnable = value;
                OnPropertyChanged("isLotNoEnable");
            }
        }
        #endregion

        #region [Constructor]
        public 조제FIL결과ViewModel()
        {
            _BR_BRS_REG_ProductionOrderCustomValue = new 보령.BR_BRS_REG_ProductionOrderCustomValue();
            _BR_BRS_SEL_ProductionOrderCustomValue = new 보령.BR_BRS_SEL_ProductionOrderCustomValue();
        }
        #endregion

        #region [BizRule]
        private BR_BRS_REG_ProductionOrderCustomValue _BR_BRS_REG_ProductionOrderCustomValue;
        public BR_BRS_REG_ProductionOrderCustomValue BR_BRS_REG_ProductionOrderCustomValue
        {
            get { return _BR_BRS_REG_ProductionOrderCustomValue; }
            set
            {
                _BR_BRS_REG_ProductionOrderCustomValue = value;
                OnPropertyChanged("BR_BRS_REG_ProductionOrderCustomValue");
            }
        }

        private BR_BRS_SEL_ProductionOrderCustomValue _BR_BRS_SEL_ProductionOrderCustomValue;
        public BR_BRS_SEL_ProductionOrderCustomValue BR_BRS_SEL_ProductionOrderCustomValue
        {
            get { return _BR_BRS_SEL_ProductionOrderCustomValue; }
            set
            {
                _BR_BRS_SEL_ProductionOrderCustomValue = value;
                OnPropertyChanged("BR_BRS_SEL_ProductionOrderCustomValue");
            }
        }
        #endregion

        #region [Command]
        public ICommand LoadCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["LoadCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["LoadCommandAsync"] = false;
                            CommandCanExecutes["LoadCommandAsync"] = false;
                            IsBusy = true;

                            if(arg != null && arg is 조제FIL결과)
                            {
                                _mainWnd = arg as 조제FIL결과;

                                _BR_BRS_SEL_ProductionOrderCustomValue.INDATAs.Clear();
                                _BR_BRS_SEL_ProductionOrderCustomValue.OUTDATAs.Clear();
                                _BR_BRS_SEL_ProductionOrderCustomValue.INDATAs.Add(new BR_BRS_SEL_ProductionOrderCustomValue.INDATA
                                {
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGNAME = "조제",
                                    POCDID = "FIL_RESULT"
                                });

                                if (await _BR_BRS_SEL_ProductionOrderCustomValue.Execute())
                                {
                                    if (_BR_BRS_SEL_ProductionOrderCustomValue.OUTDATAs.Count > 0)
                                    {
                                        var outData = _BR_BRS_SEL_ProductionOrderCustomValue.OUTDATAs[0];
                                        curLotNo = outData.POCDVAL1;
                                        curUnderVal = Convert.ToInt32(outData.POCDVAL2);
                                        curFilCount = Convert.ToInt32(outData.POCDVAL3);
                                        curUpperVal = Convert.ToInt32(outData.POCDVAL4);
                                        curResult = outData.POCDVAL5;

                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(_mainWnd.CurrentInstruction.Raw.MINVAL))
                                        {
                                            curUnderVal = 0;
                                        }
                                        else
                                        {
                                            curUnderVal = Convert.ToDecimal(_mainWnd.CurrentInstruction.Raw.MINVAL);
                                        }

                                        if (string.IsNullOrEmpty(_mainWnd.CurrentInstruction.Raw.MAXVAL))
                                        {
                                            curUpperVal = 0;
                                        }
                                        else
                                        {
                                            curUpperVal = Convert.ToDecimal(_mainWnd.CurrentInstruction.Raw.MAXVAL);
                                        }
                                    }
                                }

                                if(_mainWnd.CurrentOrder.OrderProcessSegmentName.Equals("조제"))
                                {
                                    isLotNoEnable = true;
                                    isFilCountEnable = true;
                                    isSaveEnable = false;
                                    isConfirmEnable = false;
                                }
                                else
                                {
                                    isLotNoEnable = false;
                                    isFilCountEnable = false;
                                    isSaveEnable = true;
                                    isConfirmEnable = false;
                                }
                            }

                            IsBusy = false;
                            ///

                            CommandResults["LoadCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            CommandResults["LoadCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["LoadCommandAsync"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("LoadCommandAsync") ?
                        CommandCanExecutes["LoadCommandAsync"] : (CommandCanExecutes["LoadCommandAsync"] = true);
                });
            }
        }

        public ICommand ConfirmCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["ConfirmCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["ConfirmCommandAsync"] = false;
                            CommandCanExecutes["ConfirmCommandAsync"] = false;
                            IsBusy = true;

                            if(string.IsNullOrEmpty(curLotNo))
                            {
                                isLotNoEnable = true;
                                throw new Exception(string.Format("FIL Lot No를 입력해 주세요."));
                            }

                            if (curUnderVal <= curFilCount && curUpperVal >= curFilCount)
                            {
                                curResult = "적합";
                            }
                            else
                            {
                                curResult = "부적합";
                            }

                            IsBusy = false;
                            ///

                            CommandResults["ConfirmCommandAsync"] = true;
                        }
                        catch (Exception ex)    
                        {
                            IsBusy = false;
                            CommandResults["ConfirmCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ConfirmCommandAsync"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("ConfirmCommandAsync") ?
                        CommandCanExecutes["ConfirmCommandAsync"] : (CommandCanExecutes["ConfirmCommandAsync"] = true);
                });
            }
        }

        public ICommand SaveCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["SaveCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            CommandResults["SaveCommandAsync"] = false;
                            CommandCanExecutes["SaveCommandAsync"] = false;

                            if(string.IsNullOrEmpty(curResult))
                            {
                                throw new Exception(string.Format("결과확인 후 기록 진행 가능합니다."));
                            }

                            // 전자서명(기록값 변경)
                            var authHelper = new iPharmAuthCommandHelper();

                            if (_mainWnd.CurrentInstruction.Raw.INSERTEDYN.Equals("Y") && _mainWnd.Phase.CurrentPhase.STATE.Equals("COMP"))
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

                            // 조회내용 기록
                            authHelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "OM_ProductionOrder_SUI");

                            if (await authHelper.ClickAsync(
                                Common.enumCertificationType.Function,
                                Common.enumAccessType.Create,
                                "조제FIL결과",
                                "조제FIL결과",
                                false,
                                "OM_ProductionOrder_SUI",
                                "", null, null) == false)
                            {
                                throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                            }
                            
                            // 조제에서만 기록 하도록 개발
                            if ("조제".Equals(_mainWnd.CurrentOrder.OrderProcessSegmentName))
                            {
                                _BR_BRS_REG_ProductionOrderCustomValue.INDATAs.Clear();

                                _BR_BRS_REG_ProductionOrderCustomValue.INDATAs.Add(new BR_BRS_REG_ProductionOrderCustomValue.INDATA
                                {
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                    OPSGNAME = _mainWnd.CurrentOrder.OrderProcessSegmentName,
                                    POCDID = "FIL_RESULT",
                                    POCDVAL1 = curLotNo,
                                    POCDVAL2 = curUnderVal.ToString(),
                                    POCDVAL3 = curFilCount.ToString(),
                                    POCDVAL4 = curUpperVal.ToString(),
                                    POCDVAL5 = curResult,
                                    USERID = AuthRepositoryViewModel.Instance.LoginedUserID
                                });

                                if (!await _BR_BRS_REG_ProductionOrderCustomValue.Execute()) return;
                            }
                            
                            var ds = new DataSet();
                            var dt = new DataTable("DATA");
                            ds.Tables.Add(dt);

                            dt.Columns.Add(new DataColumn("FilLotNo"));
                            dt.Columns.Add(new DataColumn("하한값"));
                            dt.Columns.Add(new DataColumn("Fil결과값"));
                            dt.Columns.Add(new DataColumn("상한값"));
                            dt.Columns.Add(new DataColumn("적부판단"));

                            DataRow row = dt.NewRow();
                            row["FilLotNo"] = curLotNo;
                            row["하한값"] = curUnderVal;
                            row["Fil결과값"] = curFilCount;
                            row["상한값"] = curUpperVal;
                            row["적부판단"] = curResult;

                            dt.Rows.Add(row);

                            // 조제공정에서만 기록 값 남도록
                            
                            var xml = BizActorRuleBase.CreateXMLStream(ds);
                            var bytesArray = System.Text.Encoding.UTF8.GetBytes(xml);

                            //_mainWnd.CurrentInstruction.Raw.ACTVAL = curFilCount.ToString();
                            //_mainWnd.CurrentInstruction.Raw.NOTE = imageToByteArray();
                            _mainWnd.CurrentInstruction.Raw.ACTVAL = _mainWnd.TableTypeName;
                            _mainWnd.CurrentInstruction.Raw.NOTE = bytesArray;
                            
                            var result = await _mainWnd.Phase.RegistInstructionValue(_mainWnd.CurrentInstruction);
                            if (result != enumInstructionRegistErrorType.Ok)
                            {
                                throw new Exception(string.Format("값 등록 실패, ID={0}, 사유={1}", _mainWnd.CurrentInstruction.Raw.IRTGUID, result));
                            }

                            // 조제에서만 하위 지시문에 기록값을 기록하고 적부 판단하도록 진행
                            if ("조제".Equals(_mainWnd.CurrentOrder.OrderProcessSegmentName))
                            {
                                var outputValues = InstructionModel.GetResultReceiver(_mainWnd.CurrentInstruction, _mainWnd.Instructions);
                                outputValues[0].Raw.ACTVAL = curFilCount.ToString();

                                foreach (var item in outputValues)
                                {
                                    result = await _mainWnd.Phase.RegistInstructionValue(item);
                                    if (result != enumInstructionRegistErrorType.Ok)
                                    {
                                        throw new Exception(string.Format("값 등록 실패, ID={0}, 사유={1}", item.Raw.IRTGUID, result));
                                    }
                                }
                            }

                            if (_mainWnd.Dispatcher.CheckAccess()) _mainWnd.DialogResult = true;
                            else _mainWnd.Dispatcher.BeginInvoke(() => _mainWnd.DialogResult = true);

                            CommandResults["SaveCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["SaveCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["SaveCommandAsync"] = true;
                        }
                    }
                }, arg =>
                {
                    return CommandCanExecutes.ContainsKey("SaveCommandAsync") ?
                        CommandCanExecutes["SaveCommandAsync"] : (CommandCanExecutes["SaveCommandAsync"] = true);
                });
            }
        }
        #endregion
        
        #region [User Define Function]
        public void isFILInput()
        {
            isConfirmEnable = true;
            isSaveEnable = false;
        }
        public void isConfirmInput()
        {
            isLotNoEnable = false;
            isFilCountEnable = false;
            isConfirmEnable = false;
            isSaveEnable = true;
        }
        #endregion
    }
}
