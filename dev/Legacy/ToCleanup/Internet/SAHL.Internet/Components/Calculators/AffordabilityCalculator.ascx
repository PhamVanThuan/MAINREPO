<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/Components/Calculators/AffordabilityCalculator.ascx.cs" Inherits="SAHL.Internet.Components.Calculators.AffordabilityCalculator" %>
<%@ Register TagPrefix="sahl" TagName="ApplicationForm" Src="ApplicationForm.ascx" %>
<asp:Panel ID="pnlAffordabilityCalculator" runat="server" DefaultButton="bttnCalculate">
    <p>Fill in your Income and Loan details to find out what value of Home you can afford to buy. This Affordability Calculator will allow you to apply for a Home Loan with SA Home Loans.</p>
    <div class="left-pane">
        <asp:Panel ID="pnlCalculatorOffline" Visible="false" runat="server">
            <p>Our affordability calculator is currently undergoing maintenance, we apologise for the inconvenience. Please try again shortly.</p>
        </asp:Panel>
        <asp:Panel ID="pnlCalculatorOnline" runat="server">
            <h2>What can I spend?</h2>
            <asp:Panel ID="pnlIncomeDetails" CssClass="income-details" GroupingText="Income" runat="server">
                <div class="row salary-one">
                    <asp:Label CssClass="title" AssociatedControlID="txtSalaryOne" Text="Monthly income before tax" runat="server" /><!--
                 --><asp:Label CssClass="currency-symbol" AssociatedControlID="txtSalaryOne" Text="R" runat="server" /><!--
                 --><asp:TextBox ID="txtSalaryOne" Width="60px" Text="0" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
                 --><asp:CustomValidator ID="validatorSalary" ControlToValidate="txtSalaryOne" ClientValidationFunction="$.affordabilityCalculator.validateJointIncome" Text="*" ErrorMessage="To afford a bond, your joint monthly income must exceed R 6,000." ValidationGroup="<%#ValidationGroup %>" runat="server" />
                </div>
                <div class="row salary-two">
                    <asp:Label CssClass="title" AssociatedControlID="txtSalaryTwo" Text="2nd monthly income before tax" runat="server" /><!--
                 --><asp:Label CssClass="currency-symbol" AssociatedControlID="txtSalaryTwo" Text="R" runat="server" /><!--
                 --><asp:TextBox ID="txtSalaryTwo" Width="60px" Text="0" ValidationGroup="<%#ValidationGroup %>" runat="server" />
                </div>
                <div class="row sale-profit">
                    <asp:Label CssClass="title" AssociatedControlID="txtProfitFromSale" Text="Profit from sale of existing home" runat="server" /><!--
                 --><asp:Label CssClass="currency-symbol" AssociatedControlID="txtProfitFromSale" Text="R" runat="server" /><!--
                 --><asp:TextBox ID="txtProfitFromSale" Width="60px" Text="0" ValidationGroup="<%#ValidationGroup %>" runat="server" />
                </div>
                <div class="row other-contributions">
                    <asp:Label CssClass="title" AssociatedControlID="txtOtherContribution" Text="Other contributions" runat="server" /><!--
                 --><asp:Label CssClass="currency-symbol" AssociatedControlID="txtOtherContribution" Text="R" runat="server" /><!--
                 --><asp:TextBox ID="txtOtherContribution" Width="60px" Text="0" ValidationGroup="<%#ValidationGroup %>" runat="server" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlLoanDetails" CssClass="loan-details" GroupingText="Loan Details" runat="server">
                <asp:Label ID="lblStatus" CssClass="status" runat="server" Text="How much can you afford?" />
                <div class="row monthly-installment">
                    <asp:Label CssClass="title" AssociatedControlID="txtMonthlyInstalment" Text="Monthly instalment" runat="server" /><!--
                 --><asp:Label CssClass="currency-symbol" AssociatedControlID="txtMonthlyInstalment" Text="R" runat="server" /><!--
                 --><asp:TextBox ID="txtMonthlyInstalment" Width="60px" Text="0" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
                 --><asp:CustomValidator ID="validatorMonthlyInstallment" ControlToValidate="txtMonthlyInstalment" ClientValidationFunction="$.affordabilityCalculator.validateMonthlyInstallment" Text="*" ErrorMessage="Your selected instalment is equal to 0, or greater than what you can afford." ValidationGroup="<%#ValidationGroup %>" runat="server" />
                </div>
                <div class="row loan-term">
                    <asp:Label CssClass="title" AssociatedControlID="txtLoanTerm" Text="Term of Loan (years)" runat="server" /><!--
                 --><asp:TextBox ID="txtLoanTerm" Width="60px" Text="20" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
                 --><asp:RangeValidator Type="Integer" MinimumValue="1" MaximumValue="20" ID="validatorLoanTerm" ControlToValidate="txtLoanTerm" ErrorMessage="Please select between 1 and 20 years" Text="*" Display="Dynamic" CssClass="fieldError" ValidationGroup="<%#ValidationGroup %>" runat="server" />
                </div>
                <div class="row interest-rate">
                    <asp:Label CssClass="title" AssociatedControlID="txtInterestRate" Text="Interest Rate (%)" runat="server" /><!--
                 --><asp:TextBox ID="txtInterestRate" Width="60px" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
                 --><asp:RangeValidator Type="Double" MinimumValue="1" MaximumValue="99" ID="cvPurchasePrice" ControlToValidate="txtInterestRate" ErrorMessage="The interest rate must be at least 1% and less than 100%" Text="*" Display="Dynamic" CssClass="fieldError" ValidationGroup="<%#ValidationGroup %>" runat="server" />
                </div>
            </asp:Panel>
            <asp:Button ID="bttnCalculate" CssClass="calculate-button" OnClick="Calculate" Text="Calculate what you can afford" ValidationGroup="<%#ValidationGroup %>" runat="server" />
        </asp:Panel>
    </div><div class="right-pane">
        <asp:ValidationSummary HeaderText="To calculate your affordability we need" ValidationGroup="<%#ValidationGroup %>" runat="server" />
        <asp:Panel ID="pnlFiller" runat="server">
            <div class="testimonial">
                <h3 class="title">What our clients say</h3>
                <div class="content">
                    <p class="text"></p>
                    <span class="author"></span>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlResults" runat="server" Visible="false">
            <h2>The Loan Value</h2>
            <asp:Label ID="lblMessage" runat="server" />
            <div class="row loan-amount">
                <span class="title">Loan amount</span><asp:Label ID="lblLoanAmount" CssClass="value" runat="server" />
            </div>
            <div class="row available-savings">
                <span class="title">Available savings <span class="subTitle">(after paying costs)</span></span><asp:Label ID="lblAvailableSavings" CssClass="value" runat="server" />
            </div>
            <div class="row home-value">
                <span class="title">Home Value <span class="subTitle">(approximation)</span></span><asp:Label ID="lblApproximateHomeValue" CssClass="value" runat="server" />
            </div>
            <div class="row loan-to-value-ratio">
                <span>Loan to Value Ratio of <asp:Label ID="lblLTVRatio" runat="server" Text="81%" /> <span class="subTitle">(loan amount as a % of the property value)</span></span>
            </div>
            <div class="row repayment-to-income-ratio">
                <span>Repayment to Income Ratio of <asp:Label ID="lblPTIRatio" runat="server" Text="23%" /> <span class="subTitle">(monthly instalment as a % of your income)</span></span>
            </div>
            <asp:Button CssClass="reset-button" Text="Reset" OnClick="Reset" runat="server" ValidationGroup="<%#ValidationGroup %>" />
            <asp:Button CssClass="apply-button" Text="Apply now" OnClick="Apply" runat="server" ValidationGroup="<%#ValidationGroup %>" />
        </asp:Panel>
    </div>
</asp:Panel>
<sahl:ApplicationForm ID="applicationAffordability" runat="server" Visible="false" ValidationGroup="<%#ValidationGroup %>ApplicationForm" />