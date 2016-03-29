<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="ReportingServices.aspx.cs" Inherits="SAHL.Web.Views.Reports.ReportingServices" EnableViewState="true" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <asp:Panel ID="Panel1" runat="server" onmousemove="masterCancelBeforeUnload();">
    </asp:Panel>
    <br />
    <br />
    <br />
    <table width="100%">
        <tr>
            <td></td>
            <td style="width: 33%"></td>
            <td align="right">
                <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" /></td>
            <td align="right"></td>
        </tr>
    </table>
</asp:Content>
