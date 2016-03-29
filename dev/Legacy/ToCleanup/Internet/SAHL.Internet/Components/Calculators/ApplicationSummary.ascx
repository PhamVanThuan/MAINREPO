<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ApplicationSummary.ascx.cs" Inherits="SAHL.Internet.Components.Calculators.ApplicationSummary" %>
<asp:Panel ID="pnlApplicationSummary" runat="server">
    <p>Your application has been successfully submitted, thank you for choosing SA Home Loans! One of our helpful consultants will contact you shortly. Please print a copy of this page for your records.</p>
    <h2>Details of the Loan</h2>
    <div class="row reference-number">
        <span class="title">Your reference number</span><asp:Label ID="lblSummaryReferenceNumber" CssClass="value" runat="server" />
    </div>
    <div class="row loan-period">
        <span class="title">Loan period (months)</span><asp:Label ID="lblLoanPeriod" CssClass="value" runat="server" />
    </div>
    <div class="row total-loan-amount">
        <span class="title">Total loan amount</span><asp:Label ID="lblTotalLoan" CssClass="value" runat="server" />
    </div>
    <div class="row monthly-income">
        <span class="title">My monthly income</span><asp:Label ID="lblMonthlyIncome" CssClass="value" runat="server" />
    </div>
    <asp:Panel ID="pnlCapitaliseFees" CssClass="row capitalise-fees" runat="server" Visible="false">
        <span class="title">Capitalise fees</span><asp:Label ID="lblCapitaliseFees" CssClass="value" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlInterestOnly" CssClass="row interest-only" runat="server" Visible="false">
        <span class="title">Interest only</span><asp:Label ID="lblInterestOnly" CssClass="value" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlFixedPortionElected" CssClass="row fixed-portion-elected" runat="server" Visible="false">
        <span class="title">Fixed portion elected</span><asp:Label ID="lblIsFixedPortion" CssClass="value" runat="server" />
    </asp:Panel>
    <div class="row variable-interest-rate">
        <span class="title">Variable interest rate</span><asp:Label ID="lblInterestRate" CssClass="value" runat="server" />
    </div>
    <asp:Panel ID="pnlFixedPortion" CssClass="row fixed-portion" runat="server" Visible="false">
        <span class="title">Fixed portion elected</span><asp:Label ID="lblFixedPortionElected" CssClass="value" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlFixedPortionRate" CssClass="row fixed-portion-rate" runat="server" Visible="false">
        <span class="title">Fixed interest rate</span><asp:Label ID="lblFixedInterestRate" CssClass="value" runat="server" />
    </asp:Panel>
</asp:Panel>