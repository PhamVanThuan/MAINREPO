<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CBOMenu1.aspx.cs" Inherits="SAHL.Web.Views.Administration.CBOMenu2"
    MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table id="tblMaint" runat="server" width="100%">
        <tr>
            <td align="center" style="width: 50%">
    <SAHL:SAHLLabel ID="lbl" runat="server" CssClass="LabelText" Font-Bold="True" Font-Underline="True" TextAlign="0">Select a Feature</SAHL:SAHLLabel></td>
            <td align="center" colspan="1">
            </td>
        </tr>
    <tr>
    <td>
    <asp:Panel runat="server" ScrollBars="Vertical" ID="pnl" Width="100%" Height="300px" style="overflow-y:scroll">
                <SAHL:SAHLTreeView runat="server" ID="tv" OnNodeSelected="tv_NodeSelected" />
                </asp:Panel></td>
        <td colspan="1">
        </td>
    </tr>
        <tr>
            <td width="20%">
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="right"><SAHL:SAHLButton runat="server" ID="btnNext" Text="Next" OnClick="btnNext_Click" /></td>
        </tr>
    </table>
</asp:Content>
