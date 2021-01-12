using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FileView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string attachID = Request["ATTACHGUID"];

        //if (attachID != null)
        //{
        //    BizActorRemoteCall call = new BizActorRemoteCall();

        //    var row = call.GetAttachFile(attachID);

        //    if (row != null)
        //    {
        //        byte[] fileStream = row["HEADERINFO"] as byte[];
        //        string fileName = row["FILENAME"] as string;

        //        string mimeType = "";

        //        Response.Buffer = true;
        //        Response.Clear();
        //        Response.ContentType = mimeType;
        //        Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode(fileName));
        //        Response.BinaryWrite(fileStream);
        //        Response.Flush();
        //        Response.End();
        //    }
        //}

        //Response.End();

        string querystring;
        try { querystring = Request.Url.ToString().Split(new char[] { '?' })[1]; }
        catch { querystring = "no querystring"; }
        //metaRefresh.Content = string.Format("0; URL=/PDFService/pdfViewer/web/viewer.html?" + (querystring.Equals("no querystring") ? string.Empty : querystring));

        Response.Redirect("/PDFService/pdfViewer/web/viewer.html?" + (querystring.Equals("no querystring") ? string.Empty : querystring));
    }

    public class BizActorRemoteCall
    {
        public DataRow GetAttachFile(string attachID)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            var bizActorIp = ConfigurationManager.AppSettings["BizActorServerName"];
            var bizActorPort = ConfigurationManager.AppSettings["BizActorPort"];
            var bizActorSysId = ConfigurationManager.AppSettings["BizActorSysID"];

            var remoteCall = new ServerAgent.RemoteCall(bizActorIp, "TCP_BUFFERED", bizActorPort, "SERVICE", bizActorSysId);

            using (DataSet ds = new DataSet())
            {
                ds.Tables.Add("INDATA");
                ds.Tables["INDATA"].Columns.Add("ATTACHGUID");

                DataRow row = ds.Tables["INDATA"].NewRow();
                row["ATTACHGUID"] = attachID;
                ds.Tables["INDATA"].Rows.Add(row);

                var dsResult = remoteCall.ExecuteService("BR_PHR_SEL_DocumentAttachmentFile", ds, "INDATA", "OUTDATA");

                if(dsResult.Tables.Count > 0 && dsResult.Tables["OUTDATA"].Rows.Count > 0)
                {
                    return dsResult.Tables["OUTDATA"].Rows[0];
                }
            }

            return null;
        }
    }
}