<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CBOMenu2.aspx.cs" Inherits="SAHL.Web.Views.Administration.CBOMenu3"
    MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table id="tblMaint" runat="server" width="790">
        <tr>
            <td colspan="2">
                <SAHL:SAHLLabel ID="lbl" runat="server">Select a Parent Node</SAHL:SAHLLabel>
                <asp:Panel runat="server" ScrollBars="Vertical" ID="pnl" Width="100%" Height="300px"
                    Style="overflow-y: scroll">
                    <SAHL:SAHLTreeView runat="server" ID="tv" OnNodeSelected="tv_NodeSelected" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
        <td width="20%"><SAHL:SAHLLabel ID="lbl2" runat="server">CBOParent</SAHL:SAHLLabel></td>
        <td><SAHL:SAHLTextBox ID="txtParent" runat="server"></SAHL:SAHLTextBox></td>
        </tr>
        <tr>
            <td colspan="2"><SAHL:SAHLButton runat="server" ID="btnNext" Text="Finish" OnClick="btnNext_Click" /></td>
        </tr>
    </table>
</asp:Content>
