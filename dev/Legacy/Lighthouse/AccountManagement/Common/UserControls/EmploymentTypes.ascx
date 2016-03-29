<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmploymentTypes.ascx.cs" Inherits="EmploymentTypes" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
    <table>
        <tr>
            <td class="ManagementPanelTitle">
                <asp:Label ID="lblRoles" runat="server" CssClass="style.css" Text="Select legal entity's employment type:"></asp:Label>
            </td>
            <td class="ManagementPanelTitle">
                <asp:DropDownList ID="ddEmployTypes" runat="server" DataSourceID="oEmployType" DataTextField="description"
                    DataValueField="EmploymentTypeKey">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="oEmployType" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
        SelectCommand="SELECT Description, EmploymentTypeKey FROM dbo.EmploymentType">
    </asp:SqlDataSource>

