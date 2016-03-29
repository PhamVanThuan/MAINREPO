<%@ Page Language="C#" AutoEventWireup="true" Codebehind="MapUsersToOrgStructure.aspx.cs"
    Inherits="SAHL.Web.Views.Administration.MapUsersToOrgStructure" MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="790">
        <tr>
            <td colspan="2">
                <asp:Panel runat="server" ScrollBars="Vertical" ID="pnl" Width="100%" Height="300px"
                    Style="overflow-y: scroll">
                    <SAHL:SAHLTreeView runat="server" ID="tv" OnNodeSelected="tv_NodeSelected" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table width="790" runat="server" id="tblMaint" visible="false">
        <tr>
            <td width="45%"><SAHL:SAHLLabel ID="lbl1" runat="server" >Users not in group</SAHL:SAHLLabel><br />
                <asp:ListBox ID="liNotIn" runat="server" Height="316px"></asp:ListBox></td>
            <td abbr="10%" valign="middle" align="center">
                <SAHL:SAHLButton runat="Server" ID="btnAdd" Text=" >> " OnClick="btnAdd_Click" Visible="false" /><br />
                <br />
                <SAHL:SAHLButton runat="Server" ID="btnREmove" Text=" << " OnClick="btnREmove_Click"
                    Visible="false" />
            </td>
            <td width="45%"><SAHL:SAHLLabel ID="SAHLLabel1" runat="server" >Users not in group</SAHL:SAHLLabel><br />
                <asp:ListBox ID="liIn" runat="server" Height="316px"></asp:ListBox></td>
        </tr>
    </table>
</asp:Content>
