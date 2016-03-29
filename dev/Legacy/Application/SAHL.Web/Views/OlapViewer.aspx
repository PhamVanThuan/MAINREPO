<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="True" Codebehind="OlapViewer.aspx.cs" Inherits="SAHL.Web.Views.Reports.OlapViewer"  EnableViewState="true"%>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    &nbsp;&nbsp;
    <iframe runat="server" id="ReportViewerFrame" width="100%" height="560" marginheight="0" marginwidth="0" />
    &nbsp;
    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="100%">
    <table width="90%">
        <tr>
            <td align="right" style="height: 26px">
            </td>
            <td align="right" style="height: 26px">
            <SAHL:SAHLButton ID="btnBack" runat="server" OnClick="btnCancel_Click" Text="Back" /></td>
        </tr>
    </table>    
    </asp:Panel>
</asp:Content>

