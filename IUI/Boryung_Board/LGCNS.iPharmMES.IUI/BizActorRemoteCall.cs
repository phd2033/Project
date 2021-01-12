using ServerAgent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace LGCNS.iPharmMES.IUI
{
    //public class BizActorRemoteCall
    //{
        //public Dictionary<string, string> GetSystemOptions(string optionType)
        //{
        //    Dictionary<string, string> options = new Dictionary<string, string>();

        //    var bizActorIp = ConfigurationManager.AppSettings["BizActorServerName"];
        //    var bizActorPort = ConfigurationManager.AppSettings["BizActorPort"];
        //    var bizActorSysId = ConfigurationManager.AppSettings["BizActorSysID"];

        //    var remoteCall = new RemoteCall(bizActorIp, "TCP_BUFFERED", bizActorPort, "SERVICE", bizActorSysId);

        //    using (DataSet ds = new DataSet())
        //    {
        //        ds.Tables.Add("INDATA");
        //        ds.Tables["INDATA"].Columns.Add("OPTIONTYPE");
        //        ds.Tables["INDATA"].Columns.Add("ISUSE");

        //        DataRow row = ds.Tables["INDATA"].NewRow();
        //        row["OPTIONTYPE"] = optionType;
        //        row["ISUSE"] = "Y";
        //        ds.Tables["INDATA"].Rows.Add(row);

        //        var dsResult = remoteCall.ExecuteService("BR_PHR_SEL_System_Option_OPTIONTYPE", ds, "INDATA", "OUTDATA");

        //        foreach (DataRow valueRow in dsResult.Tables["OUTDATA"].Rows)
        //        {
        //            options.Add(valueRow["OPTIONITEM"] as string, valueRow["OPTIONVALUE"] as string);
        //        }
        //    }

        //    return options;
        //}
    //}
}