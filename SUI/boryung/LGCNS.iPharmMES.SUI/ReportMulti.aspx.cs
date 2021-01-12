using Microsoft.Reporting.WebForms;
//using ServerAgent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Web.UI;
using LGCNS.iPharmMES.SUI;

public partial class ReportMulti : System.Web.UI.Page
{
    List<byte[]> _pages = new List<byte[]>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (string.IsNullOrEmpty(ClientQueryString))
            {
                Response.End();
                return;
            }

            rptPopup.ProcessingMode = ProcessingMode.Remote;
            rptPopup.ShowParameterPrompts = false;
            rptPopup.ShowToolBar = false;
            rptPopup.InteractivityPostBackMode = InteractivityPostBackMode.SynchronousOnDrillthrough;
            ServerReport serverReport = rptPopup.ServerReport;

            var call = new BizActorRemoteCall();

            var options = call.GetSystemOptions("OPTP006");

            string rptSvrUserName = options["SYS_RRT_SVR_USER"];
            string rptSvrPassword = options["SYS_RRT_SVR_PASSWORD"];
            string rptSvrServiceAddress = options["SYS_RRT_SVR_SVC_ADDR"];

            //serverReport.ReportServerUrl = new Uri(rptSvrServiceAddress);
            //serverReport.ReportServerCredentials = new ReportNetworkCredentials(rptSvrUserName, rptSvrPassword, ".");

            string printerName = string.Empty;
            string copies = "1";

            var items = Server.UrlDecode(this.ClientQueryString).Split(new string[] { ",," }, StringSplitOptions.RemoveEmptyEntries);
            int n = 0;
            foreach (var item in items)
            {
                n++;
                System.Collections.Generic.List<ReportParameter> param = new System.Collections.Generic.List<ReportParameter>();

                string filename = "/Weighing/DispensingLabelDemo";
                string pageOrientation = "Portrait";
                string pageWidth = "7";
                string pageHeight = "10";

                //foreach (var par in Server.UrlDecode(this.ClientQueryString).Split('&'))
                foreach (var par in item.Split('&'))
                {
                    switch (par.Split('=').First())
                    {
                        case "REPORT_PATH":
                            filename = par.Split('=').Last();
                            break;

                        case "PRINTER_NAME":
                            printerName = par.Split('=').Last();
                            break;
                        case "COPIES":
                            copies = par.Split('=').Last();
                            break;
                        case "ORIENTATION":
                            pageOrientation = par.Split('=').Last();
                            break;
                        case "PAGEWIDTH":
                            pageWidth = par.Split('=').Last();
                            break;
                        case "PAGEHEIGHT":
                            pageHeight = par.Split('=').Last();
                            break;
                        default:
                            param.Add(new ReportParameter(par.Split('=').First(), string.IsNullOrWhiteSpace(par.Split('=').Last()) ? null : par.Split('=').Last(), true));
                            break;
                    }
                }

                serverReport.ReportServerUrl = new Uri(rptSvrServiceAddress);
                serverReport.ReportServerCredentials = new ReportNetworkCredentials(rptSvrUserName, rptSvrPassword, ".");
                serverReport.ReportPath = filename;

                serverReport.SetParameters(param);
                serverReport.Refresh();

                // PDF 렌더링하고 Print 하는 부분
                string deviceInfo =
                        @"<DeviceInfo>" +
                        "<OutputFormat>EMF</OutputFormat>" +
                        "<DpiX>200</DpiX>" +
                        "<DpiY>200</DpiY>" +
                        "<PrintDpiX>200</PrintDpiX>" +
                        "<PrintDpiY>200</PrintDpiY>" +
                        "<Orientation>" + pageOrientation + "</Orientation>" +
                        "<PageWidth>" + pageWidth + "cm</PageWidth>" +
                        "<PageHeight>" + pageHeight + "cm</PageHeight>" +
                    //                    "<MarginTop>0.25in</MarginTop>" +
                    //                    "<MarginLeft>0.15in</MarginLeft>" +
                    //                    "<MarginRight>-0.15in</MarginRight>" +
                    //                    "<MarginBottom>0.25in</MarginBottom>" +
                        "<StartPage>" + "{0}" + "</StartPage>" +
                        "</DeviceInfo>";

                byte[] pdf_file = null;
                string format = "Image";
                string mimeType = "";
                string encoding = "";
                string extension = "jpg";
                string[] streams = null;
                List<byte[]> eachpages = new List<byte[]>();

                Microsoft.Reporting.WebForms.Warning[] warnings = null;

                // PDF 렌더링 (서버의 리포팅 서비스로부터)
                pdf_file = rptPopup.ServerReport.Render(format, string.Format(deviceInfo, 0), out mimeType, out encoding, out extension, out streams, out warnings);
                eachpages.Add(pdf_file);
                if (n == 1)
                {
                    _pages.Add(pdf_file);
                }

                byte[] data = null;
                foreach (var pageName in streams)
                {
                    data = rptPopup.ServerReport.RenderStream("Image", pageName, string.Format(deviceInfo, eachpages.Count), out mimeType, out encoding);

                    eachpages.Add(pdf_file);
                    if (n == 1)
                    {
                        _pages.Add(data);
                    }
                }
            }


            var doc = new PrintReport();
            doc.PrinterSettings.PrinterName = printerName;
            doc.PrinterSettings.Copies = Convert.ToInt16(copies);
            doc.Pages = _pages;

            doc.Print();

            Response.End();
        }
    }

    public class PrintReport : PrintDocument
    {
        private int _currentPage = 0;
        private List<byte[]> _pages;

        public PrintReport()
        {
        }

        public List<byte[]> Pages
        {
            get { return _pages; }
            set { _pages = value; }
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
            _currentPage = 0;
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            Stream pageToPrint = new MemoryStream(this._pages[_currentPage]);
            pageToPrint.Position = 0;

            // Load each page into a Metafile to draw it.
            using (Metafile pageMetaFile = new Metafile(pageToPrint))
            {
                Rectangle adjustedRect = new Rectangle(
                        e.PageBounds.Left - (int)e.PageSettings.HardMarginX,
                        e.PageBounds.Top - (int)e.PageSettings.HardMarginY,
                        e.PageBounds.Width,
                        e.PageBounds.Height);

                // Draw a white background for the report
                e.Graphics.FillRectangle(Brushes.White, adjustedRect);

                // Draw the report content
                e.Graphics.DrawImage(pageMetaFile, adjustedRect);

                // Prepare for next page.  Make sure we haven't hit the end.
                _currentPage++;
                e.HasMorePages = _currentPage < this._pages.Count;
            }
        }

        protected override void OnQueryPageSettings(QueryPageSettingsEventArgs e)
        {
            e.PageSettings = (PageSettings)PrinterSettings.DefaultPageSettings.Clone();
        }
    }



    public class BizActorRemoteCall
    {
        public Dictionary<string, string> GetSystemOptions(string optionType)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            var bizActorIp = ConfigurationManager.AppSettings["BizActorServerName"];
            var bizActorPort = ConfigurationManager.AppSettings["BizActorPort"];
            var bizActorSysId = ConfigurationManager.AppSettings["BizActorSysID"];

            var remoteCall = new ServerAgent.RemoteCall(bizActorIp, "TCP_BUFFERED", bizActorPort, "SERVICE", bizActorSysId);

            using (DataSet ds = new DataSet())
            {
                ds.Tables.Add("INDATA");
                ds.Tables["INDATA"].Columns.Add("OPTIONTYPE");
                ds.Tables["INDATA"].Columns.Add("ISUSE");

                DataRow row = ds.Tables["INDATA"].NewRow();
                row["OPTIONTYPE"] = optionType;
                row["ISUSE"] = "Y";
                ds.Tables["INDATA"].Rows.Add(row);

                var dsResult = remoteCall.ExecuteService("BR_PHR_SEL_System_Option_OPTIONTYPE", ds, "INDATA", "OUTDATA");

                foreach (DataRow valueRow in dsResult.Tables["OUTDATA"].Rows)
                {
                    options.Add(valueRow["OPTIONITEM"] as string, valueRow["OPTIONVALUE"] as string);
                }
            }

            return options;
        }
    }
}



