<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" 
    Inherits="SAHL.Web.Views.PersonalLoan.UnsecuredLoanSummary" Title="Unsecured Lending Loan Summary"
    CodeBehind="UnsecuredLoanSummary.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div id="summaryHeading" runat="server" class="TableHeaderB"></div>
    <table class="tableStandard">
        <tr>
            <td>
                <table>
                    <tr>
                        <td class="titleText" style="width: 250px;">Account Number
                        </td>
                        <td class="titleText" style="width: 140px;">
                            <SAHL:SAHLLabel runat="server" ID="lblAccountNumber"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText">Income
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblIncome"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText">Loan Amount
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblLoanAmount"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText">Debit Order Day
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblDebitOrderDay"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText">Account Status
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblAccountStatus"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText">Open Date
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblOpenDate"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText">Settlement Date
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblSettlementDate"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td class="titleText">Month to Date Interest
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblInterestMonthToDate"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText">Total Interest for Month
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblInterestForMonth"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText" style="width: 180px;">Current Balance
                        </td>
                        <td class="titleText" style="width: 90px;">
                            <SAHL:SAHLLabel runat="server" ID="lblCurrentBalance"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText">Arrear Balance
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblArrearBalance"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText">Remaining Term
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblRemainingTerm"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="titleText">Interest Rate
                        </td>
                        <td>
                            <SAHL:SAHLLabel runat="server" ID="lblInterestRate"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div class="TableHeaderB">
        Instalment Breakdown
    </div>
    <table class="tableStandard">
        <tr>
            <td class="titleText" style="width: 250px;">Monthly Personal Loan Instalment
            </td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblPersonalLoanInstalment" runat="server" CssClass="LabelText"
                    TextAlign="Left">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="titleText">Credit Life Premium
            </td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblCreditLifePremium" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="titleText">Monthly Service Fee
            </td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblMonthlyServiceFee" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="titleText">Total Instalment
            </td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblTotalInstalment" runat="server" CssClass="LabelText" Font-Bold="true">-</SAHL:SAHLLabel>
            </td>
        </tr>
    </table>
    <br />
    <div id="pendingLifePolicyClaimHeading" runat="server" class="TableHeaderB" style="color: red"></div>
</asp:Content>
