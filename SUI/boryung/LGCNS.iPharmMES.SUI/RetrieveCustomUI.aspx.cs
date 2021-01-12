using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RetrieveCustomUI : System.Web.UI.Page
{
    string _selectedCustomName;
    string _version;
    protected void Page_Load(object sender, EventArgs e)
    {
        _selectedCustomName = Request.QueryString["SelectedCustom"];               
    }

    public string BuildInitParams()
    {
        string folerPath = AppDomain.CurrentDomain.BaseDirectory + @"\ClientBin\";
        string[] files = Directory.GetFiles(folerPath);

        StringBuilder sb = new StringBuilder();
        foreach (string filename in files)
        {
            if (filename.ToLower().EndsWith(".dll"))
            {
                if (sb.Length > 0)
                {
                    sb.Append("|");
                }
                sb.Append(filename.Replace(folerPath, ""));
            }
        }
        
        return string.Format("Files={0}, SelectedCustomName={1}, Version={2}", sb.ToString(), _selectedCustomName, ConfigurationManager.AppSettings["Version"]); 
    }
}
