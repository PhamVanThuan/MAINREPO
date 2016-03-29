<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PopulationGroups.ascx.cs" Inherits="PopulationGroups" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table>
    <tr>
        <td class="ManagementPanelTitle">
            <asp:Label ID="lblRoles" runat="server" CssClass="style.css" Text="Select legal entity's population group:"></asp:Label>
        </td>
        <td class="ManagementPanelTitle">
            <asp:DropDownList ID="ddPopulationGroups" runat="server" DataSourceID="oPopulationGrp"
                DataTextField="description" DataValueField="PopulationGroupKey">
            </asp:DropDownList>
        </td>
    </tr>
</table>
<asp:SqlDataSource ID="oPopulationGrp" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    SelectCommand="SELECT Description, PopulationGroupKey FROM dbo.PopulationGroup">
</asp:SqlDataSource>
