<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountDetails.ascx.cs" Inherits="AccountDetails" %>
<link href="../../style.css" rel="stylesheet" type="text/css" />
<table width="100%">
    <tr>
        <td class="ManagementPanelTitle">
            Account&nbsp;
        </td>
        <td align="right">
            &nbsp;</td>
    </tr>
</table>
<asp:Repeater ID="Repeater1" runat="server" DataSourceID="oAccount" EnableViewState="False">
    <ItemTemplate>
        <table align="center" border="0" cellpadding="2" cellspacing="0" width="750px">
            <tr>
                <td align="left" class="TableRowSeperator" nowrap="nowrap">
                    <b>Account Key:</b></td>
                <td class="TableRowSeperator">
                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "accountkey") %>
                </td>
                <td class="TableRowSeperator" nowrap="nowrap">
                </td>
                <td align="left" class="TableRowSeperator" nowrap="nowrap">
                    <b>Bank Account Key:</b></td>
                <td class="TableRowSeperator">
                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "BankAccountKey") %>
                </td>
            </tr>
            <tr>
                <td align="left" class="TableRowSeperator" nowrap="nowrap">
                    <b>Monthly Payment:</b></td>
                <td class="TableRowSeperator">
                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "Payment") %>
                </td>
                <td class="TableRowSeperator" nowrap="nowrap">
                </td>
                <td align="left" class="TableRowSeperator" nowrap="nowrap">
                    <b>Debit Order Day:</b></td>
                <td class="TableRowSeperator">
                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "DebitOrderDay") %>
                </td>
            </tr>
            <tr>
                <td align="left" class="TableRowSeperator" nowrap="nowrap">
                    <b>Financial Service Type:</b></td>
                <td class="TableRowSeperator">
                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "FinancialServiceType") %>
                </td>
                <td class="TableRowSeperator" nowrap="nowrap">
                    &nbsp;
                </td>
            </tr>
        </table>
    </ItemTemplate>
</asp:Repeater>
<asp:HiddenField ID="hfAcctKey" runat="server" EnableViewState="False" />
<asp:SqlDataSource ID="oAccount" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    SelectCommand="SELECT A.AccountKey, FS.BankAccountKey, 'R' + Cast(FS.Payment as varchar(10)) as Payment, FS.DebitOrderDay, FST.Description AS FinancialServiceType FROM dbo.Account AS A INNER JOIN dbo.FinancialService AS FS ON FS.AccountKey = A.AccountKey INNER JOIN dbo.FinancialServiceType AS FST ON FST.FinancialServiceTypeKey = FS.FinancialServiceTypeKey WHERE (A.AccountKey = @AcctKey)">
    <SelectParameters>
        <asp:ControlParameter ControlID="hfAcctKey" DefaultValue="" Name="AcctKey" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="oLegalEntities" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True">
</asp:SqlDataSource>
<asp:SqlDataSource ID="oFinancialService" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True">
</asp:SqlDataSource>
