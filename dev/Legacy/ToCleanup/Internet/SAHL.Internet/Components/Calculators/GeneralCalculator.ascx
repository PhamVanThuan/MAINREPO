<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneralCalculator.ascx.cs" Inherits="SAHL.Internet.Components.Calculators.GeneralCalculator" %>
<%@ Register TagPrefix="sahl" TagName="ApplicationForm" Src="ApplicationForm.ascx" %>
<asp:Panel ID="pnlGeneralCalculator" runat="server" DefaultButton="bttnCalculate">
    <p><asp:Label ID="lblDescription" runat="server" /></p>
    <div class="left-pane">
        <asp:Panel ID="pnlCalculatorOffline" Visible="false" runat="server">
            <p>This calculator is currently undergoing maintenance, we apologise for the inconvenience. Please try again shortly.</p>
        </asp:Panel>
        <asp:Panel ID="pnlCalculatorOnline" runat="server">
            <h2>Calculator Details</h2>
            <asp:Panel ID="pnlPurchasePriceInput" CssClass="row purchase-price" runat="server">
                <asp:Label CssClass="title" AssociatedControlID="tbPurchasePrice" Text="Purchase price" runat="server" /><!--
             --><asp:Label CssClass="currency-symbol" AssociatedControlID="tbPurchasePrice" Text="R" runat="server" /><!--
             --><asp:TextBox ID="tbPurchasePrice" Width="60px" Text="0" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:RangeValidator Type="Integer" MinimumValue="170000" MaximumValue="5000000" ID="cvPurchasePrice" ControlToValidate="tbPurchasePrice" ErrorMessage="The purchase price must be between R 170,000 & R 5,000,000." Text="*" Display="Dynamic" ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlCashDepositInput" CssClass="row cash-deposit" runat="server">
                <asp:Label CssClass="title" AssociatedControlID="tbCashDeposit" Text="Cash deposit" runat="server" /><!--
             --><asp:Label CssClass="currency-symbol" AssociatedControlID="tbCashDeposit" Text="R" runat="server" /><!--
             --><asp:TextBox ID="tbCashDeposit" Width="60px" ValidationGroup="<%#ValidationGroup %>" Text="0" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlMarketValue" CssClass="row market-value" runat="server">
                <asp:Label CssClass="title" AssociatedControlID="tbMarketValue" Text="Value of your home" runat="server" /><!--
             --><asp:Label CssClass="currency-symbol" AssociatedControlID="tbMarketValue" Text="R" runat="server" /><!--
             --><asp:TextBox ID="tbMarketValue" runat="server" Width="60px" Text="0" ValidationGroup="<%#ValidationGroup %>" /><!--
             --><asp:CompareValidator Type="Integer" Operator="GreaterThan" ValueToCompare="170000" ID="cvMarketValue" ControlToValidate="tbMarketValue" ErrorMessage="The property value must be greater than R 170,000" Text="*" Display="Dynamic" ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlCurrentLoan" CssClass="row current-loan-amount" runat="server">
                <asp:Label CssClass="title" AssociatedControlID="tbCurrentLoan" Text="Current loan amount" runat="server" /><!--
             --><asp:Label CssClass="currency-symbol" AssociatedControlID="tbCurrentLoan" Text="R" runat="server" /><!--
             --><asp:TextBox ID="tbCurrentLoan" Width="60px" Text="0" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:RangeValidator Type="Integer" ID="cvCurrentLoanAmount" ControlToValidate="tbCurrentLoan" Text="*" Display="Dynamic" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:CompareValidator Type="Integer" Operator="GreaterThan" ValueToCompare="0" ID="cvLoanAmount" ControlToValidate="tbCurrentLoan" ErrorMessage="You must have a current loan to switch." Text="*" Display="Dynamic" ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlCashOut" CssClass="row cash-out" runat="server">
                <asp:Label CssClass="title" AssociatedControlID="tbCashOut" Text="Cash out" runat="server" /><!--
             --><asp:Label CssClass="currency-symbol" AssociatedControlID="tbCashOut" Text="R" runat="server" /><!--
             --><asp:TextBox ID="tbCashOut" Width="60px" Text="0" ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </asp:Panel>
            <div class="row employment-type">
                <span class="title">I am</span><!--
             --><div class="radio-group">
                    <asp:RadioButton ID="rbSalaried" Text="Salaried" Checked="true" runat="server" GroupName="Employment" ValidationGroup="<%#ValidationGroup %>" /><!--
                 --><asp:RadioButton ID="rbSelfEmployed" Text="Self Employed" runat="server" GroupName="Employment" ValidationGroup="<%#ValidationGroup %>" />
                </div>
            </div>
            <div class="row gross-monthly-income">
                <asp:Label CssClass="title" AssociatedControlID="tbHouseholdIncome" Text="Gross monthly income" runat="server" /><!--
             --><asp:Label CssClass="currency-symbol" AssociatedControlID="tbHouseholdIncome" Text="R" runat="server" /><!--
             --><asp:TextBox ID="tbHouseholdIncome" Width="60px" Text="0" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:CompareValidator EnableClientScript="true" Type="Integer" Operator="GreaterThanEqual" ValueToCompare="6000" ID="cvHouseholdIncome" ControlToValidate="tbHouseholdIncome" ErrorMessage="Your gross income must exceed R5,000" Text="*" Display="Dynamic" ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </div>
            <div class="row loan-type">
                <span class="title">I'm looking for a</span><!--
             --><div class="radio-group">
                    <asp:RadioButton ID="rbVariableRate" Text="Variable rate loan" Checked="true" runat="server" GroupName="LoanType" ValidationGroup="<%#ValidationGroup %>" /><!--
                 --><asp:RadioButton ID="rbFixedRate" Text="Fixed rate loan" runat="server" GroupName="LoanType" ValidationGroup="<%#ValidationGroup %>" />
                </div>
            </div>
            <div class="row loan-term">
                <asp:Label CssClass="title" AssociatedControlID="tbLoanTerm" Text="Term of loan (months)" runat="server" /><!--
             --><asp:TextBox ID="tbLoanTerm" MaxLength="3" Width="30px" Text="240" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:RangeValidator ControlToValidate="tbLoanTerm" MinimumValue="1" MaximumValue="360" ID="rvLoanTerm" Text="*" ErrorMessage="The loan term must be between 1 and 360 months." ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </div>
            <asp:Panel ID="pnlFixPercentage" CssClass="row percentage-to-fix" runat="server">
                <asp:Label CssClass="title" AssociatedControlID="tbFixPercentage" Text="Percentage to fix" runat="server" /><!--
             --><asp:TextBox ID="tbFixPercentage" runat="server" MaxLength="3" Width="30px" Text="100" ValidationGroup="<%#ValidationGroup %>" /><!--
             --><asp:CustomValidator ControlToValidate="tbFixPercentage" ClientValidationFunction="$.calculator.validateFixedPercentageValues" EnableClientScript="true" Text="*" ErrorMessage="The percentage you have fixed is too low. please increase it." ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </asp:Panel>
            <div class="row extra-options">
                <asp:CheckBox ID="chkCapitaliseFees" runat="server" Checked="true" Text="Capitalise fees?" ValidationGroup="<%#ValidationGroup %>" /><!--
             --><asp:CheckBox ID="chkInterestOnly" runat="server" Text="Interest only?" ValidationGroup="<%#ValidationGroup %>" />
            </div>
            <asp:Button ID="bttnCalculate" CssClass="calculate-button" Text="What Can I Afford?" OnCommand="Calculate" ValidationGroup="<%#ValidationGroup %>" runat="server" />
        </asp:Panel>
    </div><div class="right-pane">
        <asp:ValidationSummary runat="server" HeaderText="Qualifying notes" ValidationGroup="<%#ValidationGroup %>" />
        <asp:Panel ID="pnlFiller" runat="server">
            <div class="testimonial">
                <h3 class="title">What our clients say</h3>
                <div class="content">
                    <p class="text"></p>
                    <span class="author"></span>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlResults" Visible="false" runat="server">
            <h2>Loan Breakdown</h2>
            <asp:Label ID="lblNotQualifyMsg" Visible="true" runat="server" CssClass="orangetext" />
            <asp:Label ID="lblQualifyMsg" runat="server" Visible="false">The application provisionally qualifies</asp:Label>
            <div class="loan-to-value-ratio">
                Loan To Value (LTV) ratio of <asp:Label ID="lblLTV" runat="server" /> <span class="subTitle">(loan amount as a % of the property value)</span>
            </div>
            <asp:Panel ID="pnlSuperLoPti" CssClass="payment-to-income-ratio" runat="server">
                Payment To Income (PTI) ratio of <asp:Label ID="lblPTI" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlVarifixPti" CssClass="fix-percent" runat="server">
                <asp:Label ID="lblFixPercent" runat="server" /> fixed has been specified, therefore the Payment To Income (PTI) ratio is <asp:Label ID="lblFixPTI" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlTotalLoanRequirement" CssClass="row total-loan-requirement" runat="server">
                <span class="title">Total loan requirement</span><asp:Label ID="lblSAHLTotLoan" CssClass="value" runat="server" /><asp:Label ID="lblFeeInfoInd" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlInterestRate" CssClass="row interest-rate" runat="server">
                <span class="title">Interest rate</span><asp:Label ID="lblSAHLIntRate" CssClass="value" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlMonthlyInstallment" CssClass="row monthly-installment" runat="server">
                <span class="title">Monthly installment</span><asp:Label ID="lblSAHLMonthlyInst" CssClass="value" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlInterestPaidOverTerm" CssClass="row interest-over-term" runat="server">
                <span class="title">Interest paid over term</span><asp:Label ID="lblSAHLIntOverTerm" CssClass="value" runat="server" />
            </asp:Panel>
            <asp:Table ID="tblVarifix" CssClass="varifix" runat="server">
                <asp:TableHeaderRow runat="server" TableSection="TableHeader">
                    <asp:TableHeaderCell runat="server" />
                    <asp:TableHeaderCell runat="server">Fixed Portion</asp:TableHeaderCell>
                    <asp:TableHeaderCell runat="server">Variable Portion</asp:TableHeaderCell>
                    <asp:TableHeaderCell runat="server">Total</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Loan Split</asp:TableCell>
                    <asp:TableCell ID="lblFixedPercent" runat="server" />
                    <asp:TableCell ID="lblVariablePercent" runat="server" />
                    <asp:TableCell ID="SAHLLabel9" runat="server" />
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Total Loan</asp:TableCell>
                    <asp:TableCell ID="lblFixLoanAmount" runat="server" />
                    <asp:TableCell ID="lblVarLoanAmount" runat="server" />
                    <asp:TableCell ID="lblTotalFixLoan" runat="server" />
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Interest Rate</asp:TableCell>
                    <asp:TableCell ID="lblFixRate" runat="server" />
                    <asp:TableCell ID="lblVarRate" runat="server" />
                    <asp:TableCell runat="server" />
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Monthly Installment</asp:TableCell>
                    <asp:TableCell ID="lblFixMonthlyInst" runat="server" />
                    <asp:TableCell ID="lblVarMonthlyInst" runat="server" />
                    <asp:TableCell ID="lblTotFixMonthlyInst" runat="server" />
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Interest Over Term</asp:TableCell>
                    <asp:TableCell ID="lblIntPaidTermFix" runat="server" />
                    <asp:TableCell ID="lblIntPaidTermVar" runat="server" />
                    <asp:TableCell ID="lblTotFixIntPaidTerm" runat="server" />
                </asp:TableRow>
            </asp:Table>
            <asp:Label ID="lblFeeInfoFix" runat="server" />
            <h4>Fees</h4>
            <div class="row cancellation-fee">
                <span class="title">Cancellation fee</span><asp:Label ID="lblCancellationFee" CssClass="value" runat="server" />
            </div>
            <div class="row registration-fee">
                <span class="title">Registration fee</span><asp:Label ID="lblRegFee" CssClass="value" runat="server" />
            </div>
            <div class="row initiation-fee">
                <span class="title">Initiation fee</span><asp:Label ID="lblBondPrepFee" CssClass="value" runat="server" />
            </div>
            <div class="row total-fees">
                <span class="title">Total fees</span><asp:Label ID="lblTotalFees" CssClass="value" runat="server" />
            </div>
            <div class="row interim-interest-provision">
                <span class="title">Interim interest provision</span><asp:Label ID="lblInterimIntProv" CssClass="value" runat="server" />
            </div>
            <h4>Details Specified</h4>
            <asp:Panel ID="pnlPurchasePrice" CssClass="row purchase-price" Visible="false" runat="server">
                <span class="title">Purchase price</span><asp:Label ID="lblPurchasePrice" CssClass="value" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlCashDeposit" CssClass="row cash-deposit" runat="server" Visible="false">
                <span class="title">Cash deposit</span><asp:Label ID="lblCashDeposit" CssClass="value" runat="server" />
            </asp:Panel>
            <asp:Button CssClass="reset-button" Text="Reset" OnClick="Reset" runat="server" ValidationGroup="<%#ValidationGroup %>" />
            <asp:Button CssClass="apply-button" Text="Apply now" OnClick="Apply" runat="server" ValidationGroup="<%#ValidationGroup %>" />
        </asp:Panel>
    </div>
</asp:Panel>
<sahl:ApplicationForm ID="applicationCalculator" runat="server" Visible="false" ValidationGroup="<%#ValidationGroup %>ApplicationForm" />