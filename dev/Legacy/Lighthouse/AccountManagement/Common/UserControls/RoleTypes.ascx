<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoleTypes.ascx.cs" Inherits="RoleTypes" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table>
    <tr>
        <td class="ManagementPanelTitle">
            <asp:Label ID="lblRoles" runat="server" CssClass="style.css" Text="Select legal entity's role in the account:"></asp:Label>
        </td>
        <td class="ManagementPanelTitle">
            &nbsp;<asp:DropDownList ID="ddRoles" runat="server" DataSourceID="oRoleTypes" DataTextField="description" DataValueField="RoleTypeKey">
            </asp:DropDownList></td>
    </tr>
</table>
<asp:SqlDataSource ID="oRoleTypes" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    SelectCommand="SELECT Description, RoleTypeKey FROM dbo.RoleType"></asp:SqlDataSource>
