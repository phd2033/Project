<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fileTrans.aspx.cs" Inherits="fileTrans" %>
<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fileTrans.aspx.cs" Inherits="fileTrans" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
<script type="text/javascript">
    function checkFinish() {
        if (document.getElementById("hdnFinishYn").value == "Y") {
            if (document.getElementById("hdnOperationType").value == "REMOVE") {
                var ret = document.getElementById("hdnResult").value;
                document.getElementById("hdnFinishYn").value = "N";
                parent.endFileRemove(ret);
            }
            else {
                var ret = document.getElementById("hdnResult").value;
                document.getElementById("hdnFinishYn").value = "N";
                parent.endFileTrans(ret);
            }
        }
    }
</script>
</head>
<body onload="checkFinish();">
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hdnFinishYn" runat="server" Value="N" />
        <asp:HiddenField ID="hdnOperationType" runat="server" Value="" />
        <asp:HiddenField ID="hdnResult" runat="server" Value="" />
    </div>
    </form>
</body>
</html>
