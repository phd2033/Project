<%@ Page Language="C#" AutoEventWireup="true" Inherits="ReportChangeRequestEDMS" CodeFile="ReportChangeRequestEDMS.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <script type="text/javascript">
        function checkFinish() {
            if (document.getElementById("HiddenField1").value == "Y") {
                var ret = document.getElementById("hdnResult").value;
                document.getElementById("HiddenField1").value = "N";
                parent.endFileCreate(ret);
            }
        }
    </script>

</head>

<body onload="checkFinish();">
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="hdnFinishYn" runat="server" Value="N" />
            <asp:HiddenField ID="HiddenField1" runat="server" Value="N" />
            <asp:HiddenField ID="hdnResult" runat="server" Value="" />

            <rsweb:ReportViewer ID="rptPopup" runat="server" Font-Names="Tahoma" Font-Size="10pt" Height="100%" Width="100%">
            </rsweb:ReportViewer>

        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </form>
</body>
</html>
