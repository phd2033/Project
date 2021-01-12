<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportPopup.aspx.cs" Inherits="LGCNS.iPharmMES.IUI.ReportPopup" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <script type="text/javascript">
    <!--
        //Disable right mouse click Script
        ///////////////////////////////////
        function clickIE4() {
            if (event.button == 2) {
                return false;
            }
        }

        function clickNS4(e) {
            if (document.layers || document.getElementById && !document.all) {
                if (e.which == 2 || e.which == 3) {
                    return false;
                }
            }
        }

        if (document.layers) {
            document.captureEvents(Event.MOUSEDOWN);
            document.onmousedown = clickNS4;
        }
        else if (document.all && !document.getElementById) {
            document.onmousedown = clickIE4;
        }

        document.oncontextmenu = new Function("return false")

        // --> 
    </script>    

</head>

<body>
    <form id="form1" runat="server">
    <div>
    
        <rsweb:ReportViewer ID="rptPopup" runat="server" Font-Names="Tahoma" Font-Size="10pt" Height="100%" Width="100%">
        </rsweb:ReportViewer>
    
    </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </form>
</body>
</html>
