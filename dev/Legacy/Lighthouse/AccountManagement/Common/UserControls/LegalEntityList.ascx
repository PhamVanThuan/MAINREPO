<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LegalEntityList.ascx.cs" Inherits="LegalEntityList" %>
<link href="../../style.css" rel="stylesheet" type="text/css" />
<table width="100%">
    <tr>
        <td class="ManagementPanelTitle">
            Legal Entities</td>
        <td align="right">
            &nbsp;</td>
    </tr>
</table>
<table>
    <tr>
        <td>
            <asp:GridView ID="gvLegalEntities" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                AutoGenerateSelectButton="True" CellPadding="2" CssClass="SelectedGridRow" OnSelectedIndexChanged="gvLegalEntities_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="LegalEntityKey" HeaderText="LegalEntityKey" Visible="False" />
                    <asp:BoundField DataField="AccountKey" HeaderText="AccountKey" Visible="False" />
                    <asp:BoundField DataField="RoleTypeKey" HeaderText="RoleTypeKey" Visible="False" />
                    <asp:BoundField DataField="RoleType" HeaderText="Role" />
                    <asp:BoundField DataField="Salutation" HeaderText="Salutation" />
                    <asp:BoundField DataField="Initials" HeaderText="Initials" />
                    <asp:BoundField DataField="Firstnames" HeaderText="First Names" />
                    <asp:BoundField DataField="Surname" HeaderText="Surname" />
                    <asp:BoundField DataField="TradingName" HeaderText="Trading Name" />
                    <asp:BoundField DataField="RegisteredName" HeaderText="Registered Name" />
                    <asp:BoundField DataField="LegalEntityType" HeaderText="Type" />
                </Columns>
                <SelectedRowStyle CssClass="SelectedRowStyle" />
            </asp:GridView>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hfAcctKey" runat="server" />
<asp:HiddenField ID="hfIDNum" runat="server" />
<asp:HiddenField ID="hfPassportNum" runat="server" />
<asp:HiddenField ID="hfSurname" runat="server" />
<asp:HiddenField ID="hfTradingName" runat="server" />
<asp:HiddenField ID="hfRegisteredName" runat="server" />
<asp:HiddenField ID="hfRegistrationNum" runat="server" />
<asp:HiddenField ID="hfType" runat="server" />
<asp:SqlDataSource ID="obyAccount" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    SelectCommand="select LE.LegalEntityKey, R.AccountKey, R.RoleTypeKey, &#10;ISNULL(RT.Description,'None') as RoleType,&#10;LE.Salutation, LE.Initials, LE.FirstNames as Firstnames, LE.Surname, &#10;LE.TradingName, LE.RegisteredName,&#10;LET.Description as LegalEntityType &#10;from LegalEntity LE &#10;LEFT OUTER JOIN Role R ON LE.LegalEntityKey = R.LegalEntityKey&#10;LEFT OUTER Join RoleType RT on RT.RoleTypeKey = R.RoleTypeKey&#10;Inner Join LegalEntityType LET on LET.LegalEntityTypeKey = LE.LegalEntityTypeKey&#10;where ISNULL(R.AccountKey,'') like @AcctKey&#10;">
    <SelectParameters>
        <asp:ControlParameter ControlID="hfAcctKey" DefaultValue="" Name="AcctKey" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="obySearch" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    SelectCommand="select LE.LegalEntityKey, &#10;LE.Salutation, LE.Initials, LE.FirstNames, LE.Surname, &#10;LE.TradingName, LE.RegisteredName,&#10;LET.Description as LegalEntityType,'' as AccountKey, '' as RoleTypeKey, 'none' as RoleType&#10;from LegalEntity LE &#10;Inner Join LegalEntityType LET on LET.LegalEntityTypeKey = LE.LegalEntityTypeKey&#10;where ISNULL(LE.IDNumber,'') like @ID &#10;AND ISNULL(LE.PassportNumber,'') like @PassportNum &#10;AND ISNULL(LE.Surname,'') like @Surname &#10;AND ISNULL(LE.TradingName,'') like @TradingName &#10;AND ISNULL(LE.RegisteredName,'') like @RegisteredName &#10;AND ISNULL(LE.RegistrationNumber,'') like @RegistrationNumber &#10;AND ISNULL(LE.LegalEntityTypeKey,'') Like @LEType&#10;">
    <SelectParameters>
        <asp:ControlParameter ControlID="hfIDNum" Name="ID" PropertyName="Value" />
        <asp:ControlParameter ControlID="hfPassportNum" Name="PassportNum" PropertyName="Value" />
        <asp:ControlParameter ControlID="hfSurname" Name="Surname" PropertyName="Value" />
        <asp:ControlParameter ControlID="hfTradingName" Name="TradingName" PropertyName="Value" />
        <asp:ControlParameter ControlID="hfRegisteredName" Name="RegisteredName" PropertyName="Value" />
        <asp:ControlParameter ControlID="hfRegistrationNum" Name="RegistrationNumber" PropertyName="Value" />
        <asp:ControlParameter ControlID="hfType" Name="LEType" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
<br />
