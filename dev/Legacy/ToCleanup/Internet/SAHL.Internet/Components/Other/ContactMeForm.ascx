<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ContactMeForm.ascx.cs"  Inherits="SAHL.Internet.Components.Other.ContactMeForm" %>
<asp:LinkButton ID="bttnReset" CssClass="reset-link" style="display:none" OnClick="ResetClicked" runat="server">Reset Form</asp:LinkButton>
<asp:Panel ID="pnlContactMeForm" CssClass="call-me" runat="server">
    <h1>Call Me</h1>
    <p>Need Help? Fill in your contact details and one of our helpful consultants will contact you shortly.</p>
    <asp:Panel ID="pnlContactDetails" DefaultButton="bttnSend" GroupingText="Contact Details" CssClass="contact-details" runat="server">
        <div class="row first-names">
            <asp:Label AssociatedControlID="txtFirstNames" CssClass="title" Text="First names" runat="server" /><!--
            --><asp:TextBox ID="txtFirstNames" Width="150px" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
            --><asp:RequiredFieldValidator ControlToValidate="txtFirstNames" ErrorMessage="Please fill in your first names." Text="*" ValidationGroup="<%#ValidationGroup %>" runat="server" />
        </div>
        <div class="row surname">
            <asp:Label AssociatedControlID="txtSurname" CssClass="title" Text="Surname" runat="server" /><!--
            --><asp:TextBox ID="txtSurname" Width="150px" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
            --><asp:RequiredFieldValidator ControlToValidate="txtSurname" ErrorMessage="Please fill in your surname." Text="*" ValidationGroup="<%#ValidationGroup %>" runat="server" />    
        </div>
        <div class="row contact-number">
            <asp:Label AssociatedControlID="phCode" CssClass="title" Text="Contact number" runat="server" /><!--
            --><asp:TextBox ID="phCode" Width="30px" MaxLength="3" ValidationGroup="<%#ValidationGroup %>" runat="server" />-<asp:TextBox ID="phNumber" Width="110px" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
            --><asp:RequiredFieldValidator ID="valPhonecode" ControlToValidate="phCode" ErrorMessage="Please fill in your dialing code." Text="*" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
            --><asp:RequiredFieldValidator ID="valPhoneNumber" ControlToValidate="phNumber" ErrorMessage="Please fill in your phone number." Text="*" ValidationGroup="<%#ValidationGroup %>" runat="server" />
        </div>
        <div class="row email-address">
            <asp:Label AssociatedControlID="txtEmailAddress" CssClass="title" Text="E-mail address" runat="server" /><!--
            --><asp:TextBox ID="txtEmailAddress" Width="150px" ValidationGroup="<%#ValidationGroup %>" runat="server" /><!--
            --><asp:RegularExpressionValidator ControlToValidate="txtEmailAddress" ValidationExpression="\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b" Text="*" ErrorMessage="Please enter a valid email address." ValidationGroup="<%#ValidationGroup %>" runat="server" />
        </div>
        <div class="row existing-user">
            <asp:CheckBox ID="chkExistingCustomer" runat="server" Text="I am an existing customer." ValidationGroup="<%#ValidationGroup %>" />
        </div>
        <asp:Button ID="bttnSend" Text="Submit Details" CssClass="apply-button" OnCommand="SendRequest" ValidationGroup="<%#ValidationGroup %>" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlError" runat="server" Visible="false"><p>An error occurred while trying to send your request, please try again.</p></asp:Panel>
    <asp:ValidationSummary ID="valContactMeSummary" HeaderText="Please provide the following information" ValidationGroup="<%#ValidationGroup %>" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlContactMeMessage" CssClass="call-me-message" runat="server" Visible="false">
    <h1>Thank You</h1>
    <p>Your contact request has been successfully submitted, thank you for choosing SA Home Loans! One of our helpful consultants will contact you shortly.</p>
</asp:Panel>

