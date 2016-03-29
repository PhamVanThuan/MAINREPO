<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
	CodeBehind="PersonalLoanApplicationSummary.aspx.cs" Inherits="SAHL.Web.Views.PersonalLoan.PersonalLoanApplicationSummary" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
	<div class="TableHeaderB">Application Details</div>
	<table class="tableStandard">
		<tr>
			<td class="titleText" style="width:300px;">
				Application Number
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblApplicationNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText" style="width:300px;">
				Account Number
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblAccountNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText">
				Product Type
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblApplicationType" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText">
				Loan Amount
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblLoanAmount" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText">
				Loan Term
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblLoanTerm" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText">
				Initiation Fee
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblInitiationFee" Format="GridCurrency" runat="server" CssClass="LabelText"
					TextAlign="Left">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText">
				Interest Rate
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblInterestRate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText">
				Link Rate
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblLinkRate" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText">
				Personal Loan Consultant
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblCurrentPersonalLoansConsultant" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
			</td>
		</tr>
	</table>
	<br />
	<div class="TableHeaderB">Instalment Breakdown</div>
	<table class="tableStandard">
		<tr>
			<td class="titleText" style="width:300px;">
				Monthly Personal Loan Instalment
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblInstallment" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText">
				Credit Life Premium
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblCreditLifePremium" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText">
				Monthly Service Fee
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblMonthlyServiceFee" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td class="titleText">
				Total Instalment
			</td>
			<td class="cellDisplay">
				<SAHL:SAHLLabel ID="lblTotalInstalment" runat="server" CssClass="LabelText" Font-Bold="true">-</SAHL:SAHLLabel>
			</td>
		</tr>
	</table>
    <table class="tableStandard" width="100%">
        <tr>
            <td align="right">
                <SAHL:SAHLButton ID="btnTransitionHistory" runat="server" Text="History" OnClick="btnTransitionHistory_Click" />&nbsp;
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
