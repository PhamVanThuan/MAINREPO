<%@ Control Language="C#" CodeFile="ManageLoan.ascx.cs" Inherits="Loan_ascx" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table width="100%">
    <tr>
        <td class="ManagementPanelTitle">
            Summary
        </td>
        <td align="right">
            &nbsp;
        </td>
    </tr>
</table>
<asp:Repeater ID="Repeater1" runat="server" DataSourceID="oLoanSummary" EnableViewState="False">
    <ItemTemplate>
        <table border="0" align="center" cellpadding="2" cellspacing="0" width="750px" class="Normal">
            <tr>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Loan Number:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<%#  DataBinder.Eval(Container.DataItem, "AccountKey").ToString()%>
                </td>
                <td nowrap="nowrap" class="TableRowSeperator">
                    &nbsp;
                </td>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Client Name:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "clientname").ToString()%>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Current Balance:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "currentbalance")).ToString(Constants.CURRENCY_FORMAT)%>
                </td>
                <td nowrap="nowrap" class="TableRowSeperator">
                    &nbsp;
                </td>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Installment:</b>
                </td>
                <%if (bIsInterestOnly)
                  { %>
                <td onmouseover="ShowPopup('Amortising Instalment:<%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "UnadjustedInstalment")).ToString(Constants.CURRENCY_FORMAT)%>',25,250)"
                    class="TableRowSeperator">
                    &nbsp;<% =Convert.ToDouble(sInstallment).ToString(Constants.CURRENCY_FORMAT)%>
                </td>
                <%}
                  else
                  { %>
                <td class="TableRowSeperator">
                    &nbsp;<% =Convert.ToDouble(sInstallment).ToString(Constants.CURRENCY_FORMAT)%>
                </td>
                <%} %>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Arrear Balance:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "arrearbalance")).ToString(Constants.CURRENCY_FORMAT) %>
                </td>
                <td nowrap="nowrap" class="TableRowSeperator">
                    &nbsp;
                </td>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Total Bond Amount:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "loantotalbondamount")).ToString(Constants.CURRENCY_FORMAT) %>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Initial Balance:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "initialbalance")).ToString(Constants.CURRENCY_FORMAT) %>
                </td>
                <td nowrap="nowrap" class="TableRowSeperator">
                    &nbsp;
                </td>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Loan Agreement Amount:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "loanagreementamount")).ToString(Constants.CURRENCY_FORMAT) %>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Loan Open Date:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "loanopendate") %>
                </td>
                <td nowrap="nowrap" class="TableRowSeperator">
                    &nbsp;
                </td>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>SPV Description:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<%# DataBinder.Eval(Container.DataItem, "SPVDescription") %>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Product Elected:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<% =Product %>
                </td>
                <td nowrap="nowrap" class="TableRowSeperator">
                    &nbsp;
                </td>
                <td nowrap="nowrap" align="right" class="TableRowSeperator">
                    <b>Next Reset Date:</b>
                </td>
                <td class="TableRowSeperator">
                    &nbsp;<% =ResetDate %>
                </td>
            </tr>
        </table>
    </ItemTemplate>
</asp:Repeater>
<asp:SqlDataSource ID="oLoanSummary" runat="server" SelectCommand="select fs.accountkey,
sum(cur_bal.currentbalance) currentbalance,
sum(arr_bal.ArrearBalance) arrearbalance,
sum(lb.InitialBalance) initialbalance,
sum(fs.Payment) payment,
spv.[Description] as SPVDescription,
client.clientfirstnames + ' ' + client.clientsurname clientname,
isnull(loan.LoanTotalBondAmount,0) as LoanTotalBondAmount,
isnull(loan.LoanAgreementAmount,0) as LoanAgreementAmount,
Convert(varchar(10),loan.loanopendate,103) LoanOpenDate,
dbo.fLoanCalcInstalment(sum(cur_bal.currentbalance),avg(lb.InterestRate),12,avg(lb.RemainingInstalments)) UnadjustedInstalment,
IsInterestOnly = convert(bit,isNull((select 1 from [2am].fin.FinancialAdjustment fa (nolock)
									join [2am].dbo.FinancialService fins (nolock) on fins.FinancialServiceKey = fa.FinancialServiceKey
									and fins.AccountKey = @accountkey
									and fa.FinancialAdjustmentTypeKey = 6) ,0))
from [2am].fin.MortgageLoan ml (nolock)
join [2am].dbo.FinancialService fs (nolock) on fs.financialservicekey = ml.financialservicekey
join [2am].dbo.Account a (nolock) ON  fs.AccountKey = a.AccountKey
join [2am].spv.SPV (nolock) on spv.SPVKey = a.SPVKey
join [2am].fin.LoanBalance lb (nolock) on lb.FinancialServiceKey = fs.FinancialServiceKey
join [2am].dbo.vMortgageLoanCurrentBalance cur_bal on cur_bal.accountkey = a.accountkey
join [2am].dbo.vMortgageLoanArrearBalance arr_bal on arr_bal.accountkey = a.accountkey
join sahldb..Loan loan (nolock) on fs.accountkey = loan.loannumber
 join sahldb..Client client (nolock) on loan.clientnumber = client.clientnumber
where fs.accountkey = @accountkey
AND convert(datetime, convert(varchar(10),isnull(a.CloseDate, getdate()+1), 103), 103) = convert(datetime, convert(varchar(10),isnull(fs.CloseDate, getdate()+1), 103), 103)
group by fs.accountkey,spv.Description, clientfirstnames + ' ' + clientsurname, LoanTotalBondAmount, LoanAgreementAmount, loan.loanopendate
" EnableViewState="False">
    <SelectParameters>
        <asp:QueryStringParameter Name="accountkey" DefaultValue="0" QueryStringField="param0">
        </asp:QueryStringParameter>
    </SelectParameters>
</asp:SqlDataSource>