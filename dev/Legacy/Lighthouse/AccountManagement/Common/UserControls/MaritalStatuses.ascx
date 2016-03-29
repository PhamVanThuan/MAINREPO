<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MaritalStatuses.ascx.cs" Inherits="MaritalStatuses" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table>
    <tr>
        <td class="ManagementPanelTitle">
            <asp:Label ID="lblRoles" runat="server" CssClass="style.css" Text="Select legal entity's marital status:"></asp:Label>
        </td>
        <td class="ManagementPanelTitle">
            &nbsp;<asp:DropDownList ID="ddMaritalStatuses" runat="server" DataSourceID="oMaritalStatus"
                DataTextField="description" DataValueField="MaritalStatusKey">
            </asp:DropDownList></td>
    </tr>
</table>
<asp:SqlDataSource ID="oMaritalStatus" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    SelectCommand="SELECT Description, MaritalStatusKey FROM dbo.MaritalStatus"></asp:SqlDataSource>
