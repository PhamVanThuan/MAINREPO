<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LegalEntitySearch.ascx.cs" Inherits="LegalEntitySearch" %>
<%@ Register Src="LegalEntityList.ascx" TagName="LegalEntityList" TagPrefix="uc1" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table>
    <tr>
        <td class="ManagementPanelTitle">
            <asp:Label ID="lblLEType" runat="server" CssClass="style.css" Text="Choose a type of Legal Entity to search for:"></asp:Label>
        </td>
        <td class="ManagementPanelTitle">
            <asp:DropDownList ID="ddType" runat="server" AutoPostBack="True" CssClass="style.css"
                DataSourceID="oTypes" DataTextField="description" DataValueField="LegalEntityTypeKey"
                OnSelectedIndexChanged="ddLEType_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
</table>
<asp:SqlDataSource ID="oTypes" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    SelectCommand="SELECT LegalEntityTypeKey, Description FROM dbo.LegalEntityType&#10;WHERE Description <> 'Unknown'">
</asp:SqlDataSource>
<asp:Panel ID="pnlNatural" runat="server">
    <table>
        <tr>
            <td class="ManagementPanelTitle">
                <asp:Label ID="Label4" runat="server" CssClass="style.css" Text="Natural Person's details"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" CssClass="style.css" Text="Surname:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSurname" runat="server" CssClass="style.css"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" CssClass="style.css" Text="ID Number:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtIDNumber" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" CssClass="style.css" Text="Passport Number:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtPassportNum" runat="server" CssClass="style.css"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlCompany" runat="server">
    <table>
        <tr>
            <td class="ManagementPanelTitle">
                <asp:Label ID="lblCompany" runat="server" CssClass="style.css" Text="Company's details"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" CssClass="style.css" Text="Trading Name:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTradingName" runat="server" CssClass="style.css"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" CssClass="style.css" Text="Registration Number:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRegName" runat="server" CssClass="style.css"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" CssClass="style.css" Text="Surname:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRegNumber" runat="server" CssClass="style.css"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Panel>
<table width="100%">
    <tr>
        <td colspan="2">
        </td>
        <td align="right" class="ManagementPanelTitle">
            <asp:Button ID="cmdSearch" runat="server" CssClass="style.css" OnClick="cmdSearch_Click"
                Text="Search" />
        </td>
    </tr>
</table>
<uc1:LegalEntityList id="ucLEList" runat="server">
</uc1:LegalEntityList>
