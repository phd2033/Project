﻿using C1.Silverlight.Data;
using LGCNS.iPharmMES.Common;
using ShopFloorUI;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace 보령
{
    public class 혼합기레시피전송 : ShopFloorCustomClass
    {
        public override async Task<object> ExecuteAsync(object arg)
        {
            var bizRule = new BR_BRS_REG_Blender_Recipe_To_Eqpt();

            var inputValues = InstructionModel.GetParameterSender(this.CurrentInstruction, this.Instructions);
            var outputValues = InstructionModel.GetResultReceiver(this.CurrentInstruction, this.Instructions);

            //var noValueInstruction = inputValues.Where(o => o.Raw.INSERTEDYN == "N" && o.Raw.EQPTID == null).FirstOrDefault();
            //if (noValueInstruction != null) throw new Exception(string.Format("{0} 참조 지시문에 값이 없습니다.", noValueInstruction.Raw.IRTGUID));

            //if (inputValues.Count != 1) throw new Exception(string.Format("입력 파라미터 갯수가 일치하지 않습니다"));

            //string eqptid = string.Empty;

            //foreach (var item in inputValues)
            //{
            //    if (string.IsNullOrWhiteSpace(item.Raw.ACTVAL))
            //    {
            //        throw new Exception(string.Format("{0} 참조 지시문에 값이 없습니다.", item.Raw.IRTGUID));
            //    }

                //eqptid = eqptid + (!string.IsNullOrWhiteSpace(item.Raw.ACTVAL) ? item.Raw.ACTVAL : item.Raw.EQPTID) + ",";
            //}

            //// 전자서명 요청
            //var authHelper = new iPharmAuthCommandHelper();
            //authHelper.InitializeAsync(Common.enumCertificationType.Function, Common.enumAccessType.Create, "EM_EquipmentManagement_Action");

            //if (await authHelper.ClickAsync(
            //    Common.enumCertificationType.Function,
            //    Common.enumAccessType.Create,
            //    string.Format("[{0}] 설비 청소종료 로그북 생성", eqptid),
            //    string.Format("설비 청소종료 로그북 생성"),
            //    false,
            //    "EM_EquipmentManagement_Action",
            //    "", null, null) == false)
            //{
            //    throw new Exception(string.Format("서명이 완료되지 않았습니다."));
            //}


            bizRule.INDATAs.Add(new BR_BRS_REG_Blender_Recipe_To_Eqpt.INDATA()
            {
                POID = CurrentOrder.ProductionOrderID,
                OPSGGUID = CurrentOrder.OrderProcessSegmentID,
                VESSELID = inputValues.Count > 0 ? inputValues[0].Raw.ACTVAL : null,
                ROOMNO = AuthRepositoryViewModel.Instance.RoomID,
                USERID = AuthRepositoryViewModel.Instance.LoginedUserID,
                RECIPEID = inputValues.Count > 1 ? inputValues[1].Raw.TARGETVAL : null,
            });

            if (await bizRule.Execute() == false) throw bizRule.Exception;

            this.CurrentInstruction.Raw.ACTVAL = bizRule.OUTDATAs[0].RESULTFLAG;

            return outputValues;
        }
    }
}
