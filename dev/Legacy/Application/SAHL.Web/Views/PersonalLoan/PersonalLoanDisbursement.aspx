<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
	CodeBehind="PersonalLoanDisbursement.aspx.cs" Inherits="SAHL.Web.Views.PersonalLoan.PersonalLoanDisbursement" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
	<style type="text/css">
		.tblDisbursement td
		{
			font-weight: bold;
		}
	</style>
	<script type="text/javascript">
		function OnSubmitButtonClicked() {
			var button = document.getElementById('<%=btnConfirm.ClientID %>');
			if (button.value == 'Confirm') {
				if (confirm('Are you sure you want to disburse the funds for this application?'))
					event.returnValue = true;
				else
					event.returnValue = false;
			}
		}
	</script>
	<div class="TableHeaderB">
		Application Disbursement Details</div>
	<table class="tblDisbursement tableStandard">
		<tr>
			<td style="width:200px;">
				Amount to be disbursed
			</td>
			<td>
				<SAHL:SAHLLabel runat="server" ID="lblDisbursementAmount"></SAHL:SAHLLabel>
			</td>
		</tr>
	</table>
	<br />
	<div class="TableHeaderB">
		Bank Account Details
	</div>
	<table class="tblDisbursement tableStandard">
		<tr>
			<td style="width:200px;">
				Bank
			</td>
			<td>
				<SAHL:SAHLLabel runat="server" ID="lblBank"></SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td>
				Branch
			</td>
			<td>
				<SAHL:SAHLLabel runat="server" ID="lblBranch"></SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td>
				Account Type
			</td>
			<td>
				<SAHL:SAHLLabel runat="server" ID="lblAccountType"></SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td>
				Account Name
			</td>
			<td>
				<SAHL:SAHLLabel runat="server" ID="lblAccountName"></SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td>
				Account Number
			</td>
			<td>
				<SAHL:SAHLLabel runat="server" ID="lblAccountNumber"></SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td>
			</td>
			<td>
			</td>
			<td>
				<SAHL:SAHLButton ID="btnConfirm" runat="server" Text="Confirm" ButtonSize="Size5"
					CssClass="BtnNormal4" CausesValidation="false" OnClick="OnConfirmClick" OnClientClick = "OnSubmitButtonClicked()" />&nbsp;
				<SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" ButtonSize="Size5" CssClass="BtnNormal4"
					CausesValidation="false" OnClick="OnCancelClick" />&nbsp;
			</td>
		</tr>
	</table>
</asp:Content>
