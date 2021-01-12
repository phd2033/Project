using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LGLSFileCopy;
using System.IO;
using System.Diagnostics;

public partial class fileTrans : System.Web.UI.Page
{
    static string _eventLog = "iPharmMES SUI";
    static string _eventSource = "iPharmMES/SUI";

    protected void Page_Load(object sender, EventArgs e)
    {
        string isRemove = Request["isRemove"];
        string filename = Request["filename"];

        string transtype = Request["transtype"];
        string ip = Request["transip"];
        string uId = Request["uid"];
        string pwd = Request["pwd"];
        string downpath = Request["downpath"];
        string extpath = Request["extpath"];

        if (isRemove != null && downpath != null && filename != null)
        {
            var manager = new LGLSFileCopy.TransferManager();

            var result =
                manager.FileDelete(Path.Combine(HttpUtility.UrlDecode(downpath), HttpUtility.UrlDecode(filename)));
                //manager.FileDelete(Path.Combine("\\\\LGLS-OS-MESDB\\DataFile", HttpUtility.UrlDecode(downpath), HttpUtility.UrlDecode(filename)));

            hdnFinishYn.Value = "Y";
            hdnOperationType.Value = "REMOVE";
            hdnResult.Value = result;
        }
        else if (transtype != null)
        {
            LGLSFileCopy.TransferType myType;

            if (transtype == "0") myType = TransferType.Web;
            else if (transtype == "1") myType = TransferType.Ftp;
            else if (transtype == "2") myType = TransferType.File;
            else myType = TransferType.LocalFile;

            var trans = new TransferManager();
            string retList = trans.GetFileList(myType, ip, uId, pwd,
                Path.Combine(HttpUtility.UrlDecode(downpath)), extpath);
                //Path.Combine("\\\\LGLS-OS-MESDB\\DataFile", HttpUtility.UrlDecode(downpath)), extpath);

            hdnFinishYn.Value = "Y";
            hdnOperationType.Value = "GET";
            hdnResult.Value = retList;

            //string LOG = "Recipe Service - iPharmMES";
            //string LOG_SRC = "iRcpSvcSource";

            //var eventLogger = new EventLog();

            //if (!EventLog.SourceExists(LOG_SRC))
            //{
            //    EventLog.CreateEventSource(LOG_SRC, LOG);
            //}

            //if (EventLog.SourceExists(LOG_SRC))
            //{
            //    var eventLog = new EventLog(_eventLog);
            //    eventLog.Source = LOG_SRC;
            //    eventLog.Log = LOG;

            //    var message =
            //        string.Format("transtype - {0}, transip - {1}, uid - {2}, upwd - {3}, downpath - {4}, extpath - {5}",
            //        transtype, ip, uId, pwd, Path.Combine(Server.MapPath("./EquipmentFiles"), downpath), extpath);

            //    eventLog.WriteEntry("File transfer log " + _eventSource + "\r\n\r\n" + message, EventLogEntryType.Information);
            //}
        }
    }
}