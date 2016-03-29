<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ApplicationForm.ascx.cs"  Inherits="SAHL.Internet.Components.Calculators.ApplicationForm" %>
<%@ Register TagPrefix="sahl" TagName="ApplicationSummary" Src="ApplicationSummary.ascx" %>
<asp:Panel ID="pnlApplicationForm" runat="server">
    <p>In submitting this application to SA Home Loans you consent to our normal credit vetting process, including the use of information that we may request from recognised credit bureau services.</p>
    <div class="left-pane">
        <asp:Panel ID="pnlDetails" CssClass="details" runat="server">
            <h2>Details of the Loan</h2>
            <asp:Panel ID="pnlReferenceNumber" CssClass="row reference-number" Visible="false" runat="server">
                <span class="title">Reference number</span><asp:Label ID="lblReferenceNumber" CssClass="value" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlExtra" runat="server">
                <div class="row loan-period">
                    <span class="title">Loan period (months)</span><asp:Label ID="lblLoanPeriod" CssClass="value" runat="server" />
                </div>
                <div class="row total-loan-amount">
                    <span class="title">Total loan amount</span><asp:Label ID="lblTotalLoan" CssClass="value" runat="server" />
                </div>
                <div class="row monthly-income">
                    <span class="title">My monthly income</span><asp:Label ID="lblMonthlyIncome" CssClass="value" runat="server" />
                </div>
                <asp:Panel ID="pnlCapitaliseFees" CssClass="row capitalise-fees" Visible="false" runat="server">
                    <span class="title">Capitalise fees</span><asp:Label ID="lblCapitaliseFees" CssClass="value" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlInterestOnly" CssClass="row interest-only" Visible="false" runat="server">
                    <span class="title">Interest only </span><asp:Label ID="lblInterestOnly" CssClass="value" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlFixedPortionElected" CssClass="row fixed-portion-elected" Visible="false" runat="server">
                    <span class="title">Fixed portion elected</span><asp:Label ID="lblIsFixedPortion" CssClass="value" runat="server" />
                </asp:Panel>
                <div class="row variable-interest-rate">
                    <span class="title">Variable interest rate</span><asp:Label ID="lblInterestRate" CssClass="value" runat="server" />
                </div>
                <asp:Panel ID="pnlFixedPortion" CssClass="row fixed-portion" Visible="false" runat="server">
                    <span class="title">Fixed portion</span><asp:Label ID="lblFixedPortionElected" CssClass="value" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlFixedPortionRate" CssClass="row fixed-portion-rate" Visible="false" runat="server">
                    <span class="title">Fixed interest rate</span><asp:Label ID="lblFixedInterestRate" CssClass="value" runat="server" />
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </div><div class="right-pane">
        <asp:Panel ID="pnlAlreadyApplied" runat="server" Visible="false">
            <p>Thank you for your optimism. We have already received an application from you, and we will be contacting you very shortly. If you need to contact us, please call us on the phone number above, and quote your reference number, displayed to the left of this message. If any of your details are incorrect, we can adjust them when we make contact.</p>
        </asp:Panel>
        <asp:Panel ID="pnlApplicationDetails" DefaultButton="bttnApply" GroupingText="Application Form Details" runat="server">
            <div class="row first-names">
                <asp:Label AssociatedControlID="txtFirstNames" CssClass="title" Text="First names" runat="server" /><!--
             --><asp:TextBox ID="txtFirstNames" Width="100px" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:RequiredFieldValidator ControlToValidate="txtFirstNames" ErrorMessage="Please fill in your first names." Text="*" ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </div>
            <div class="row surname">
                <asp:Label AssociatedControlID="txtSurname" CssClass="title" Text="Surname" runat="server" /><!--
             --><asp:TextBox ID="txtSurname" Width="100px" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:RequiredFieldValidator ControlToValidate="txtSurname" ErrorMessage="Please fill in your surname." Text="*" ValidationGroup="<%#ValidationGroup %>" runat="server" />    
            </div>
            <div class="row contact-number">
                <asp:Label AssociatedControlID="phCode" CssClass="title" Text="Contact number" runat="server" /><!--
             --><asp:TextBox ID="phCode" Width="30px" MaxLength="3" ValidationGroup="<%#ValidationGroup %>" runat="server" />-<asp:TextBox ID="phNumber" Width="60px" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:RequiredFieldValidator ID="valPhonecode" ControlToValidate="phCode" ErrorMessage="Please fill in your dialing code." Text="*" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:RequiredFieldValidator ID="valPhoneNumber" ControlToValidate="phNumber" ErrorMessage="Please fill in your phone number." Text="*" ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </div>
            <div class="row number-of-applicants">
                <asp:Label AssociatedControlID="tbNumApplicants" CssClass="title" Text="Number of applicants" runat="server" /><!--
             --><asp:TextBox ID="tbNumApplicants" Width="100px" Text="1" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:RequiredFieldValidator ControlToValidate="tbNumApplicants" ErrorMessage="Please fill in the number of applicants." Text="*" ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </div>
            <div class="row email-address">
                <asp:Label AssociatedControlID="txtEmailAddress" CssClass="title" Text="E-mail address" runat="server" /><!--
             --><asp:TextBox ID="txtEmailAddress" Width="100px" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
             --><asp:RegularExpressionValidator ControlToValidate="txtEmailAddress" ValidationExpression="\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b" Text="*" ErrorMessage="Please enter a valid email address." ValidationGroup="<%#ValidationGroup %>" runat="server" />
            </div>
            <asp:Button ID="bttnApply" Text="Submit Application" CssClass="apply-button" OnCommand="ApplicationFormCommand" ValidationGroup="<%#ValidationGroup %>" runat="server" />
        </asp:Panel>
        <asp:ValidationSummary ID="valApplicationSummary" HeaderText="Please provide the following information" ValidationGroup="<%#ValidationGroup %>" runat="server" />
    </div>
</asp:Panel>
<sahl:ApplicationSummary ID="summaryApplication" Visible="false" runat="server" />