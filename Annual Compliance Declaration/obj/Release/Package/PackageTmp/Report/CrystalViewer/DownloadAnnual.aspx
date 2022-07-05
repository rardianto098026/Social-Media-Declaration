<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadAnnual.aspx.cs" Inherits="Annual_Compliance_Declaration.Report.CrystalViewer.DownloadAnnual" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src='<%=ResolveUrl("~/crystalreportviewers13/js/crviewer/crv.js")%>' type="text/<a href="DownloadAnnual.aspx">DownloadAnnual.aspx</a>javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <CR:CrystalReportViewer ID="CrystalReport1" runat="server" OnInit="CrystalReport1_Init" OnNavigate="CrystalReport1_Navigate" AutoDataBind="true" />
        </div>
    </form>
</body>
</html>
