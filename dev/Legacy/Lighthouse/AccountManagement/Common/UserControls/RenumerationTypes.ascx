<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RenumerationTypes.ascx.cs" Inherits="RenumerationTypes" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table>
    <tr>
        <td class="ManagementPanelTitle" style="height: 24px">
            <asp:Label ID="lblRoles" runat="server" CssClass="style.css" Text="Select legal entity's renumeration type:"></asp:Label>
        </td>
        <td class="ManagementPanelTitle" style="height: 24px">
            <asp:DropDownList ID="ddRenumerationTypes" runat="server" DataSourceID="oRenumerationTypes"
                DataTextField="description" DataValueField="RenumerationTypeKey">
            </asp:DropDownList>
        </td>
    </tr>
</table>
<asp:SqlDataSource ID="oRenumerationTypes" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    SelectCommand="SELECT Description, RenumerationTypeKey FROM dbo.RenumerationType">
</asp:SqlDataSource>
