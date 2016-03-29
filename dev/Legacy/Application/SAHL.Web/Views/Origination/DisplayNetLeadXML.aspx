<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Origination.DisplayNetLeadXML" Title="Display NetLead XML" Codebehind="DisplayNetLeadXML.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" class="tableStandard" width="100%">
        <tr>
            <td align="left">
                <SAHL:SAHLLabel ID="lbNetLeadXML" runat="server" Width="100%" Height="100%" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <br />
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="btnRetry" runat="server" AccessKey="R" OnClick="btnRetryCreate_Click" Text="Retry" Enabled="false" />
            </td>
        </tr>
    </table>
</asp:Content>
