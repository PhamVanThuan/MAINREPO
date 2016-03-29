<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Genders.ascx.cs" Inherits="Genders" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table>
    <tr>
        <td class="ManagementPanelTitle">
            <asp:Label ID="lblGender" runat="server" CssClass="style.css" Text="Select legal entity's employment type:"></asp:Label>
        </td>
        <td class="ManagementPanelTitle">
            <asp:DropDownList ID="ddGenderTypes" runat="server" DataSourceID="oGender" DataTextField="description"
                DataValueField="GenderKey">
            </asp:DropDownList>
        </td>
    </tr>
</table>
<asp:SqlDataSource ID="oGender" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    SelectCommand="SELECT Description, GenderKey FROM dbo.Gender"></asp:SqlDataSource>
