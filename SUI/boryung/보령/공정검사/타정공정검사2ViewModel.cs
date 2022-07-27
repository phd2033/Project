﻿using C1.Silverlight.Data;
using LGCNS.iPharmMES.Common;
using ShopFloorUI;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace 보령
{
    public class 타정공정검사2ViewModel : ViewModelBase
    {
        #region Property
        public 타정공정검사2ViewModel()
        {
            _BR_BRS_SEL_ProductionOrderIPCResult = new BR_BRS_SEL_ProductionOrderIPCResult();
            _BR_BRS_SEL_ProductionOrderIPCStandard = new BR_BRS_SEL_ProductionOrderIPCStandard();
            _IPCResults = new BR_BRS_SEL_ProductionOrderIPCResult.OUTDATACollection();
            _BR_BRS_REG_ProductionOrderTestResult = new BR_BRS_REG_ProductionOrderTestResult();
        }

        private 타정공정검사2 _mainWnd;
        private string IPC_TSID = "타정공정검사2";

        private IPCControlData _ShapeIPCData;
        public IPCControlData ShapeIPCData
        {
            get { return _ShapeIPCData; }
            set
            {
                _ShapeIPCData = value;
                OnPropertyChanged("ShapeIPCData");
            }
        }
        private IPCControlData _CrumblingIPCData;
        public IPCControlData CrumblingIPCData
        {
            get { return _CrumblingIPCData; }
            set
            {
                _CrumblingIPCData = value;
                OnPropertyChanged("CrumblingIPCData");
            }
        }
        private IPCControlData _FriabilityIPCData;
        public IPCControlData FriabilityIPCData
        {
            get { return _FriabilityIPCData; }
            set
            {
                _FriabilityIPCData = value;
                OnPropertyChanged("FriabilityIPCData");
            }
        }

        private BR_BRS_SEL_ProductionOrderIPCResult.OUTDATACollection _IPCResults;
        public BR_BRS_SEL_ProductionOrderIPCResult.OUTDATACollection IPCResults
        {
            get { return _IPCResults; }
            set
            {
                _IPCResults = value;
                OnPropertyChanged("IPCResults");
            }
        }
        
        #endregion
        #region BizRule
        private BR_BRS_SEL_ProductionOrderIPCResult _BR_BRS_SEL_ProductionOrderIPCResult;
        private BR_BRS_SEL_ProductionOrderIPCStandard _BR_BRS_SEL_ProductionOrderIPCStandard;
        private BR_BRS_REG_ProductionOrderTestResult _BR_BRS_REG_ProductionOrderTestResult;
        #endregion
        #region Command

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
                            IsBusy = true;

                            CommandResults["LoadedCommandAsync"] = false;
                            CommandCanExecutes["LoadedCommandAsync"] = false;

                            ///
                            if(arg != null && arg is 타정공정검사2)
                            {
                                _mainWnd = arg as 타정공정검사2;

                                // IPC 기준정보 조회
                                _BR_BRS_SEL_ProductionOrderIPCStandard.INDATAs.Clear();
                                _BR_BRS_SEL_ProductionOrderIPCStandard.OUTDATAs.Clear();
                                _BR_BRS_SEL_ProductionOrderIPCStandard.INDATAs.Add(new BR_BRS_SEL_ProductionOrderIPCStandard.INDATA
                                {
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                                    TSID = IPC_TSID
                                });

                                if(await _BR_BRS_SEL_ProductionOrderIPCStandard.Execute() && _BR_BRS_SEL_ProductionOrderIPCStandard.OUTDATAs.Count == 3)
                                {
                                    ShapeIPCData = IPCControlData.SetIPCControlData(_BR_BRS_SEL_ProductionOrderIPCStandard.OUTDATAs[0]);
                                    CrumblingIPCData = IPCControlData.SetIPCControlData(_BR_BRS_SEL_ProductionOrderIPCStandard.OUTDATAs[1]);
                                    FriabilityIPCData = IPCControlData.SetIPCControlData(_BR_BRS_SEL_ProductionOrderIPCStandard.OUTDATAs[2]);

                                    await GetIPCResult();
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

                            IsBusy = false;
                        }
                    }
                }, arg =>
               {
                   return CommandCanExecutes.ContainsKey("LoadedCommandAsync") ?
                       CommandCanExecutes["LoadedCommandAsync"] : (CommandCanExecutes["LoadedCommandAsync"] = true);
               });
            }
        }


        public ICommand RegisterIPCCommandAsync
        {
            get
            {
                return new AsyncCommandBase(async arg =>
                {
                    using (await AwaitableLocks["RegisterIPCCommandAsync"].EnterAsync())
                    {
                        try
                        {
                            IsBusy = true;

                            CommandResults["RegisterIPCCommandAsync"] = false;
                            CommandCanExecutes["RegisterIPCCommandAsync"] = false;

                            ///
                            if (_ShapeIPCData.DEVIATIONFLAG.HasValue && _CrumblingIPCData.DEVIATIONFLAG.HasValue && _FriabilityIPCData.DEVIATIONFLAG.HasValue)
                            {
                                _BR_BRS_REG_ProductionOrderTestResult.INDATA_SPECs.Clear();
                                _BR_BRS_REG_ProductionOrderTestResult.INDATA_ITEMs.Clear();
                                _BR_BRS_REG_ProductionOrderTestResult.INDATA_ETCs.Clear();

                                var authHelper = new iPharmAuthCommandHelper();
                                authHelper.InitializeAsync(Common.enumCertificationType.Role, Common.enumAccessType.Create, "OM_ProductionOrder_IPC");

                                if (await authHelper.ClickAsync(
                                    Common.enumCertificationType.Function,
                                    Common.enumAccessType.Create,
                                    string.Format("IPC 결과를 기록합니다."),
                                    string.Format("IPC 결과를 기록합니다."),
                                    false,
                                    "OM_ProductionOrder_IPC",
                                    "", null, null) == false)
                                {
                                    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
                                }

                                string confirmguid = AuthRepositoryViewModel.Instance.ConfirmedGuid;
                                DateTime curDttm = await AuthRepositoryViewModel.GetDBDateTimeNow();
                                string user = AuthRepositoryViewModel.GetUserIDByFunctionCode("OM_ProductionOrder_IPC");

                                // 시험명세 기록
                                _BR_BRS_REG_ProductionOrderTestResult.INDATA_SPECs.Add(new BR_BRS_REG_ProductionOrderTestResult.INDATA_SPEC
                                {
                                    POTSRGUID = Guid.NewGuid(),
                                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                                    OPTSGUID = new Guid(_ShapeIPCData.OPTSGUID),
                                    OPSGGUID = new Guid(_mainWnd.CurrentOrder.OrderProcessSegmentID),
                                    TESTSEQ = null,
                                    STRDTTM = curDttm,
                                    ENDDTTM = curDttm,
                                    EQPTID = null,
                                    INSUSER = user,
                                    INSDTTM = curDttm,
                                    EFCTTIMEIN = curDttm,
                                    EFCTTIMEOUT = curDttm,
                                    MSUBLOTID = null,
                                    REASON = null,
                                    ISUSE = "Y",
                                    ACTIVEYN = "Y",
                                    SMPQTY = _ShapeIPCData.SMPQTY,
                                    SMPQTYUOMID = _ShapeIPCData.SMPQTYUOMID,
                                });

                                // 전자서명 코멘트
                                _BR_BRS_REG_ProductionOrderTestResult.INDATA_ETCs.Add(new BR_BRS_REG_ProductionOrderTestResult.INDATA_ETC
                                {
                                    COMMENTTYPE = "CM001",
                                    COMMENT = AuthRepositoryViewModel.GetCommentByFunctionCode("OM_ProductionOrder_IPC"),
                                    TSTYPE = _ShapeIPCData.TSTYPE,
                                    LOCATIONID = AuthRepositoryViewModel.Instance.RoomID
                                });

                                // 시험상세결과 기록
                                _BR_BRS_REG_ProductionOrderTestResult.INDATA_ITEMs.Add(new BR_BRS_REG_ProductionOrderTestResult.INDATA_ITEM
                                {
                                    POTSRGUID = _BR_BRS_REG_ProductionOrderTestResult.INDATA_SPECs[0].POTSRGUID,
                                    OPTSIGUID = new Guid(_ShapeIPCData.OPTSIGUID),
                                    POTSIRGUID = Guid.NewGuid(),
                                    //2022.07.26 김호연 성상 등록할때 등록 데이터 변경
                                    //ACTVAL = _ShapeIPCData.ACTVAL.ToString(),
                                    ACTVAL = _ShapeIPCData.GetACTVAL.ToString(),
                                    INSUSER = user,
                                    INSDTTM = curDttm,
                                    EFCTTIMEIN = curDttm,
                                    EFCTTIMEOUT = curDttm,
                                    COMMENTGUID = !string.IsNullOrWhiteSpace(confirmguid) ? new Guid(confirmguid) : (Guid?)null,
                                    REASON = null,
                                    ISUSE = "Y",
                                    ACTIVEYN = "Y"
                                });

                                _BR_BRS_REG_ProductionOrderTestResult.INDATA_ITEMs.Add(new BR_BRS_REG_ProductionOrderTestResult.INDATA_ITEM
                                {
                                    POTSRGUID = _BR_BRS_REG_ProductionOrderTestResult.INDATA_SPECs[0].POTSRGUID,
                                    OPTSIGUID = new Guid(_CrumblingIPCData.OPTSIGUID),
                                    POTSIRGUID = Guid.NewGuid(),
                                    ACTVAL = _CrumblingIPCData.GetACTVAL,
                                    INSUSER = user,
                                    INSDTTM = curDttm,
                                    EFCTTIMEIN = curDttm,
                                    EFCTTIMEOUT = curDttm,
                                    COMMENTGUID = !string.IsNullOrWhiteSpace(confirmguid) ? new Guid(confirmguid) : (Guid?)null,
                                    REASON = null,
                                    ISUSE = "Y",
                                    ACTIVEYN = "Y"
                                });

                                Guid? guidPOTSRGUID = _BR_BRS_REG_ProductionOrderTestResult.INDATA_SPECs[0].POTSRGUID;
                                Guid? guidOPTSIGUID = new Guid(_FriabilityIPCData.OPTSIGUID);
                                Guid? guidPOTSIRGUID = Guid.NewGuid();
                                Guid? guidCOMMENTGUID = new Guid(confirmguid);

                                _BR_BRS_REG_ProductionOrderTestResult.INDATA_ITEMs.Add(new BR_BRS_REG_ProductionOrderTestResult.INDATA_ITEM
                                {
                                    POTSRGUID = guidPOTSRGUID,
                                    OPTSIGUID = guidOPTSIGUID,
                                    POTSIRGUID = guidPOTSIRGUID,
                                    ACTVAL = _FriabilityIPCData.GetACTVAL,
                                    INSUSER = user,
                                    INSDTTM = curDttm,
                                    EFCTTIMEIN = curDttm,
                                    EFCTTIMEOUT = curDttm,
                                    COMMENTGUID = guidCOMMENTGUID,
                                    REASON = null,
                                    ISUSE = "Y",
                                    ACTIVEYN = "Y"
                                });
                                foreach (var raw in _FriabilityIPCData.RawDatas)
                                {
                                    _BR_BRS_REG_ProductionOrderTestResult.INDATA_RAWs.Add(new BR_BRS_REG_ProductionOrderTestResult.INDATA_RAW
                                    {
                                        POTSIRAWGUID = Guid.NewGuid().ToString(),
                                        POTSIRGUID = guidPOTSIRGUID.ToString(),
                                        POTSIRVER = 1,
                                        COLLECTID = raw.COLLECTID,
                                        ACTVAL = raw.ACTVAL,
                                        INSUSER = user,
                                        INSDTTM = curDttm,
                                        COMMENTGUID = null,
                                        REASON = null,
                                        ISUSE = "Y",
                                    });
                                }

                                if (await _BR_BRS_REG_ProductionOrderTestResult.Execute())
                                {
                                    ShapeIPCData = IPCControlData.SetIPCControlData(_BR_BRS_SEL_ProductionOrderIPCStandard.OUTDATAs[0]);
                                    CrumblingIPCData = IPCControlData.SetIPCControlData(_BR_BRS_SEL_ProductionOrderIPCStandard.OUTDATAs[1]);
                                    FriabilityIPCData = IPCControlData.SetIPCControlData(_BR_BRS_SEL_ProductionOrderIPCStandard.OUTDATAs[2]);

                                    await GetIPCResult();
                                }
                                    
                            }
                            else
                                OnMessage("시험결과를 확인해주세요.");
                            
                            ///

                            CommandResults["RegisterIPCCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["RegisterIPCCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["RegisterIPCCommandAsync"] = true;

                            IsBusy = false;
                        }
                    }
                }, arg =>
               {
                   return CommandCanExecutes.ContainsKey("RegisterIPCCommandAsync") ?
                       CommandCanExecutes["RegisterIPCCommandAsync"] : (CommandCanExecutes["RegisterIPCCommandAsync"] = true);
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
                            IsBusy = true;

                            CommandResults["ConfirmCommandAsync"] = false;
                            CommandCanExecutes["ConfirmCommandAsync"] = false;

                            ///
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

                            var ds = new DataSet();
                            var dt = new DataTable("DATA");
                            ds.Tables.Add(dt);
                            dt.Columns.Add(new DataColumn("구분"));
                            dt.Columns.Add(new DataColumn("성상"));
                            dt.Columns.Add(new DataColumn("붕해"));
                            dt.Columns.Add(new DataColumn("마손도"));
                            
                            foreach (var item in _IPCResults)
                            {
                                var row = dt.NewRow();
                                row["구분"] = item.GUBUN ?? "";
                                row["성상"] = item.RSLT1 ?? "";
                                row["붕해"] = item.RSLT2 ?? "";
                                row["마손도"] = item.RSLT3 ?? "";
                                dt.Rows.Add(row);
                            }

                            if (dt.Rows.Count > 0)
                            {
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

                            CommandResults["ConfirmCommandAsync"] = true;
                        }
                        catch (Exception ex)
                        {
                            CommandResults["ConfirmCommandAsync"] = false;
                            OnException(ex.Message, ex);
                        }
                        finally
                        {
                            CommandCanExecutes["ConfirmCommandAsync"] = true;

                            IsBusy = false;
                        }
                    }
                }, arg =>
               {
                   return CommandCanExecutes.ContainsKey("ConfirmCommandAsync") ?
                       CommandCanExecutes["ConfirmCommandAsync"] : (CommandCanExecutes["ConfirmCommandAsync"] = true);
               });
            }
        }

        #endregion
        
        private async Task GetIPCResult()
        {
            try
            {
                _IPCResults.Clear();
                _BR_BRS_SEL_ProductionOrderIPCResult.INDATAs.Clear();
                _BR_BRS_SEL_ProductionOrderIPCResult.OUTDATAs.Clear();

                _BR_BRS_SEL_ProductionOrderIPCResult.INDATAs.Add(new BR_BRS_SEL_ProductionOrderIPCResult.INDATA
                {
                    POID = _mainWnd.CurrentOrder.ProductionOrderID,
                    OPSGGUID = _mainWnd.CurrentOrder.OrderProcessSegmentID,
                    TSID = IPC_TSID
                });

                if(await _BR_BRS_SEL_ProductionOrderIPCResult.Execute() && _BR_BRS_SEL_ProductionOrderIPCResult.OUTDATAs.Count > 0 )
                {
                    _IPCResults.Add(new BR_BRS_SEL_ProductionOrderIPCResult.OUTDATA
                    {
                        GUBUN = "기준",
                        RSLT1 = _ShapeIPCData.Standard,
                        RSLT2 = _CrumblingIPCData.Standard,
                        RSLT3 = _FriabilityIPCData.Standard
                    });

                    foreach (BR_BRS_SEL_ProductionOrderIPCResult.OUTDATA item in _BR_BRS_SEL_ProductionOrderIPCResult.OUTDATAs)
                    {
                        //2022.07.26 김호연 성상 등록할때 등록 데이터 변경으로 인해 주석
                        var rawData = _BR_BRS_SEL_ProductionOrderIPCResult.OUTDATA_RAWs.FirstOrDefault(
                            o => o.COLLECTID == "결과값설명" && (o.POTSIRGUID == item.RSLTID1 || o.POTSIRGUID == item.RSLTID2 || o.POTSIRGUID == item.RSLTID3));

                        if (rawData != null)
                        {
                            string actValue = string.IsNullOrEmpty(rawData.ACTVAL) ? item.RSLT1 : rawData.ACTVAL;

                            if (rawData.POTSIRGUID == item.RSLTID1) item.RSLT1 = string.IsNullOrEmpty(rawData.ACTVAL) ? item.RSLT1 : rawData.ACTVAL;
                            if (rawData.POTSIRGUID == item.RSLTID2) item.RSLT2 = string.IsNullOrEmpty(rawData.ACTVAL) ? item.RSLT2 : rawData.ACTVAL;
                            if (rawData.POTSIRGUID == item.RSLTID3) item.RSLT3 = string.IsNullOrEmpty(rawData.ACTVAL) ? item.RSLT3 : rawData.ACTVAL;
                        }

                        //item.RSLT1 = item.RSLT1 == "1" ? "적합" : "부적합";
                        _IPCResults.Add(item);
                    }

                    OnPropertyChanged("IPCResults");
                }
            }
            catch (Exception ex)
            {
                OnException(ex.Message, ex);
            }
        }

    }
}
