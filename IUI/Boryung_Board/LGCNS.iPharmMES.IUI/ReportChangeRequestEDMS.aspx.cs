using Microsoft.Reporting.WebForms;
using ServerAgent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web.UI;

public partial class ReportChangeRequestEDMS : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
                if (string.IsNullOrWhiteSpace(ClientQueryString))
                {
                    Response.End();
                    return;
                }

                // Set the processing mode for the ReportViewer to Remote
                rptPopup.ProcessingMode = ProcessingMode.Remote;
                rptPopup.ShowParameterPrompts = false;
                rptPopup.ShowToolBar = false;
                rptPopup.InteractivityPostBackMode = InteractivityPostBackMode.SynchronousOnDrillthrough;

                ServerReport serverReport = rptPopup.ServerReport;

                var call = new LGCNS.iPharmMES.IUI.BizActorRemoteCall();

                var options = call.GetSystemOptions("OPTP006");

                string rptSvrUserName = options["SYS_RRT_SVR_USER"];
                string rptSvrPassword = options["SYS_RRT_SVR_PASSWORD"];
                string rptSvrServiceAddress = options["SYS_RRT_SVR_SVC_ADDR"];
                string rptSvrDomain = options["SYS_RRT_SVR_DOMAIN"];

                serverReport.ReportServerUrl = new Uri(rptSvrServiceAddress);

                serverReport.ReportServerCredentials = new ReportNetworkCredentials(rptSvrUserName, rptSvrPassword, rptSvrDomain);

                System.Collections.Generic.List<ReportParameter> param = new System.Collections.Generic.List<ReportParameter>();

                string filename = "/Order/Report_Main";

                foreach (var par in Server.UrlDecode(this.ClientQueryString).Split('&'))
                {
                    switch (par.Split('=').First())
                    {
                        case "REPORTPATH":
                            serverReport.ReportPath = par.Split('=').Last();
                            break;

                        case "FILENAME":
                            filename = par.Split('=').Last().Replace(' ', '_');
                            break;

                        default:
                            param.Add(new ReportParameter(par.Split('=').First(), string.IsNullOrWhiteSpace(par.Split('=').Last()) ? null : par.Split('=').Last(), true));
                            break;
                    }
                }

                rptPopup.ServerReport.SetParameters(param);
                rptPopup.ServerReport.Refresh();

                byte[] pdf_file = null;
                string format = "Word";
                string deviceinfo = "";
                string mimeType = "";
                string encoding = "";
                string extension = "doc";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;

                pdf_file = rptPopup.ServerReport.Render(format, deviceinfo, out mimeType, out encoding, out extension, out streams, out warnings);

                File.WriteAllBytes("c:\\mbr\\" + filename + "." + extension, pdf_file);

                hdnResult.Value = Server.UrlEncode("c:\\mbr\\" + filename + "." + extension);

                HiddenField1.Value = "Y";
            //    Response.End();
            //}
            //catch (Exception ex)
            //{
            //    Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert(" + ex.Message + ")</SCRIPT>");
            //    Response.End();
            //    //throw ex;
            //}
        }
    }
}

//public class ReportNetworkCredentials : IReportServerCredentials
//{
//    private string _userName;
//    private string _password;
//    private string _domain;

//    public ReportNetworkCredentials(string userName, string password, string domain)
//    {
//        _userName = userName;
//        _password = password;
//        _domain = domain;
//    }

//    public WindowsIdentity ImpersonationUser
//    {
//        get
//        {
//            // Use default identity.
//            return null;
//        }
//    }

//    public ICredentials NetworkCredentials
//    {
//        get
//        {
//            // Use default identity.
//            return new NetworkCredential(_userName, _password, _domain);
//        }
//    }

//    public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
//    {
//        // Do not use forms credentials to authenticate.
//        authCookie = null;
//        user = password = authority = null;
//        return false;
//    }
//}

//public class BizActorRemoteCall
//{
//    public Dictionary<string, string> GetSystemOptions(string optionType)
//    {
//        Dictionary<string, string> options = new Dictionary<string, string>();

//        var bizActorIp = ConfigurationManager.AppSettings["BizActorServerName"];
//        var bizActorPort = ConfigurationManager.AppSettings["BizActorPort"];
//        var bizActorSysId = ConfigurationManager.AppSettings["BizActorSysID"];

//        var remoteCall = new RemoteCall(bizActorIp, "TCP_BUFFERED", bizActorPort, "SERVICE", bizActorSysId);

//        using (DataSet ds = new DataSet())
//        {
//            ds.Tables.Add("INDATA");
//            ds.Tables["INDATA"].Columns.Add("OPTIONTYPE");
//            ds.Tables["INDATA"].Columns.Add("ISUSE");

//            DataRow row = ds.Tables["INDATA"].NewRow();
//            row["OPTIONTYPE"] = optionType;
//            row["ISUSE"] = "Y";
//            ds.Tables["INDATA"].Rows.Add(row);

//            var dsResult = remoteCall.ExecuteService("BR_PHR_SEL_System_Option_OPTIONTYPE", ds, "INDATA", "OUTDATA");

//            foreach (DataRow valueRow in dsResult.Tables["OUTDATA"].Rows)
//            {
//                options.Add(valueRow["OPTIONITEM"] as string, valueRow["OPTIONVALUE"] as string);
//            }
//        }

//        return options;
//    }
//}