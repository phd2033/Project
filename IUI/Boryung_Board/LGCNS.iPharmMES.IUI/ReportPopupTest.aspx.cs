using Microsoft.Reporting.WebForms;
using ServerAgent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web.UI;

namespace LGCNS.iPharmMES.IUI
{
    public partial class ReportPopupTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {                
                // Set the processing mode for the ReportViewer to Remote
                rptPopup.ProcessingMode = ProcessingMode.Remote;
                rptPopup.ShowParameterPrompts = false;
                rptPopup.ShowToolBar = false;
                rptPopup.InteractivityPostBackMode = InteractivityPostBackMode.SynchronousOnDrillthrough;                

                ServerReport serverReport = rptPopup.ServerReport;

                var call = new LGCNS.iPharmMES.IUI.BizActorRemoteCall();

                var options = call.GetSystemOptions("OPTP006");

                string rptSvrUserName = options["SYS_RRT_TSTSVR_USER"];
                string rptSvrPassword = options["SYS_RRT_TSTSVR_PASSWORD"];
                string rptSvrServiceAddress = options["SYS_RRT_TSTSVR_SVC_ADDR"];

                serverReport.ReportServerUrl = new Uri(rptSvrServiceAddress);

                serverReport.ReportServerCredentials = new ReportNetworkCredentials(rptSvrUserName, rptSvrPassword, ".");

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
                            filename = par.Split('=').Last().Replace(' ','_');
                            break;

                        default:
                            param.Add(new ReportParameter(par.Split('=').First(), string.IsNullOrWhiteSpace(par.Split('=').Last()) ? null : par.Split('=').Last(), true));
                            break;
                    }
                }

                rptPopup.ServerReport.SetParameters(param);
                rptPopup.ServerReport.Refresh();

                /*                 
                // Create the sales order number report parameter
                ReportParameter salesOrderNumber = new ReportParameter();
                salesOrderNumber.Name = "SalesOrderNumber";
                salesOrderNumber.Values.Add("SO43661");

                // Set the report parameters for the report
                rptPopup.ServerReport.SetParameters(
                    new ReportParameter[] { salesOrderNumber });
                */ 
/*
                System.Collections.Generic.List<ReportParameter> paramList = new System.Collections.Generic.List<ReportParameter>();
                ReportParameterInfoCollection pInfo = default(ReportParameterInfoCollection);
                pInfo = ReportViewer1.ServerReport.GetParameters();
                //if you have report parameters - add them here 
                paramList.Add(new ReportParameter("PARAM1-EXAMPLE", "1", true));
                paramList.Add(new ReportParameter("PARAM1-EXAMPLE", "2", true));
                ReportViewer1.ServerReport.SetParameters(paramList);
                // Process and render the report 

*/
                //output as PDF 


                byte[] pdf_file = null;
                string format = "PDF";
                string deviceinfo = "";
                string mimeType = "";
                string encoding = "";
                string extension = "pdf";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;

                pdf_file = rptPopup.ServerReport.Render(format, deviceinfo, out mimeType, out encoding, out extension, out streams, out warnings);

                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode(filename) + ".pdf");
                Response.BinaryWrite(pdf_file);
                Response.Flush();
                Response.End();
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
}