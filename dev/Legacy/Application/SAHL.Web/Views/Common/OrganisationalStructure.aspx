<%@ Page Language="C#" MasterPageFile="~/MasterPages/SAHL.Master" AutoEventWireup="true" CodeBehind="OrganisationalStructure.aspx.cs" Inherits="SAHL.Web.Views.Common.OrganisationalStructure" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table style="width: 427px; height: 371px">
        <tr>
            <td style="width: 223px">
            </td>
            <td style="width: 566px">
            </td>
            <td style="width: 180px">
            </td>
        </tr>
        <tr>
            <td style="width: 223px">
            </td>
            <td style="width: 566px">
                <asp:TreeView ID="treeOS" runat="server" Height="280px" Width="604px">
                </asp:TreeView>
            </td>
            <td style="width: 180px">
            </td>
        </tr>
        <tr>
            <td style="width: 223px">
            </td>
            <td style="width: 566px">
            </td>
            <td style="width: 180px">
            </td>
        </tr>
    </table>
</asp:Content>
