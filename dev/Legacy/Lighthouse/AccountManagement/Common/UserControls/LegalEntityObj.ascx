<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LegalEntityObj.ascx.cs" Inherits="LegalEntity" %>
<%@ Register Src="toolbar.ascx" TagName="toolbar" TagPrefix="uc1" %>
<link href="../../style.css" rel="stylesheet" type="text/css" />
<table width="100%">
    <tr>
        <td class="ManagementPanelTitle">
            Legal Entity Details:</td>
        <td align="right">
            <uc1:toolbar ID="ucToolBar" runat="server" />
            &nbsp;</td>
    </tr>
</table>
<asp:Panel ID="pnlDisplay" runat="server" Wrap="False">
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="oLegalEntity" EnableViewState="False">
        <FooterTemplate>
            </td></tr> </table>
        </FooterTemplate>
        <HeaderTemplate>
            <table align="left" border="0" cellpadding="2" cellspacing="0" width="380">
            </table>
                <tr>
                </tr>
                    <td align="left">
                        <table border="0" width="100%">
                            <tr>
                                <td align="left" class="TableRowSeperator" nowrap="nowrap">
                                    <b>Salutation:</b></td>
                            </tr>
                            <tr>
                                <td align="left" class="TableRowSeperator" nowrap="nowrap">
                                    <b>Initials:</b></td>
                            </tr>
                            <tr>
                                <td align="left" class="TableRowSeperator" nowrap="nowrap">
                                    <b>First Names:</b></td>
                            </tr>
                            <tr>
                                <td align="left" class="TableRowSeperator" nowrap="nowrap">
                                    <b>Surname:</b></td>
                            </tr>
                        </table>
                    </td>
        </HeaderTemplate>
        <ItemTemplate>
            <td>
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td class="TableRowSeperator" nowrap="nowrap">
                            <%# DataBinder.Eval(Container.DataItem, "Salutation") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableRowSeperator">
                            <%# DataBinder.Eval(Container.DataItem, "Initials") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableRowSeperator">
                            <%# DataBinder.Eval(Container.DataItem, "Firstnames") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableRowSeperator">
                            <%# DataBinder.Eval(Container.DataItem, "Surname") %>
                        </td>
                    </tr>
                </table>
            </td>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>
<asp:Panel ID="pnlEdit" runat="server">
    <table align="center" border="0" cellpadding="2" cellspacing="0" width="380">
        <tr>
            <td align="left" class="TableRowSeperator" nowrap="nowrap">
                <b>Salutation:</b></td>
            <td align="left" class="TableRowSeperator" nowrap="nowrap">
                <asp:TextBox ID="txtSalutation" runat="server" CssClass="style.css"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" class="TableRowSeperator" nowrap="nowrap">
                <b>First Names:</b></td>
            <td align="left" class="TableRowSeperator" nowrap="nowrap">
                <asp:TextBox ID="txtFirstNames" runat="server" CssClass="style.css"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" class="TableRowSeperator" nowrap="nowrap">
                <b>Initials:</b></td>
            <td align="left" class="TableRowSeperator" nowrap="nowrap">
                <asp:TextBox ID="txtInitials" runat="server" CssClass="style.css"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" class="TableRowSeperator" nowrap="nowrap">
                <b>Surname:</b></td>
            <td align="left" class="TableRowSeperator" nowrap="nowrap">
                <asp:TextBox ID="txtSurname" runat="server" CssClass="style.css"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:HiddenField ID="hfKey" runat="server" />
<asp:SqlDataSource ID="oLegalEntity" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    SelectCommand="select LegalEntityKey, Salutation, Firstnames, Initials,&#10;Surname from legalentity where legalentitykey = @LEKey" >
    <SelectParameters>
        <asp:ControlParameter ControlID="hfKey" Name="LEKey" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
