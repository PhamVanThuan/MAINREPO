<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" MasterPageFile="~/MasterPages/Blank.Master"
	Inherits="SAHL.Web.Views.DebtCounselling.Payment" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="sahlControls" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
	<div class="TableHeaderB">Payment Received Details</div>
	<asp:Panel ID="paymentUpdatePanel" runat="server" Visible="true" Width="99%">
		<table runat="server" class="tableStandard" width="100%">
			<tr id="trPmtRecDt" runat="server">
				<td style="width: 30%" runat="server" align="left">
					<SAHL:SAHLLabel runat="server" Text="Payment Received Date" Font-Bold="True" />
				</td>
				<td style="width: 70%" runat="server" align="left">
					<SAHL:SAHLDateBox ID="dtePaymentReceivedDate" runat="server"></SAHL:SAHLDateBox>
					<SAHL:SAHLLabel ID="lblPaymentReceivedDate" runat="server" />
				</td>
			</tr>
			<tr id="trPmtRecAmt" runat="server">
				<td>
					<SAHL:SAHLLabel runat="server" Text="Payment Received Amount" Font-Bold="True" />
				</td>
				<td>
					<SAHL:SAHLCurrencyBox ID="txtPaymentReceivedAmount" runat="server" TextAlign="Right"></SAHL:SAHLCurrencyBox>
					<SAHL:SAHLLabel ID="lblPaymentReceivedAmount" runat="server" />
				</td>
			</tr>
			<tr id="trTermReviewDt" runat="server">
				<td>
					<SAHL:SAHLLabel ID="SAHLLabel1" runat="server" Text="Term Review Date" Font-Bold="True" />
				</td>
				<td>
					<SAHL:SAHLDateBox ID="dteTermReviewDate" runat="server"></SAHL:SAHLDateBox>
					<SAHL:SAHLLabel ID="lblTermReviewDate" runat="server" />
				</td>
			</tr>
			<tr id="trInstExpDt" runat="server">
				<td>
					<SAHL:SAHLLabel runat="server" Text="Reset Instalment Expectancy Date" Font-Bold="True" />
				</td>
				<td>
					<SAHL:SAHLLabel ID="lblResetInstallmentExpectancyDate" runat="server" />
				</td>
			</tr>
			<tr id="trPDA" runat="server">
				<td>
					<SAHL:SAHLLabel runat="server" Text="Payment Distribution Agent" Font-Bold="True" />
				</td>
				<td>
					<SAHL:SAHLLabel ID="lblPaymentDistributionAgent" runat="server" />
				</td>
			</tr>
		</table>
	</asp:Panel>
	<table runat="server" id="tblButton" style="width: 100%">
		<tr>
			<td align="right">
				<SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click">
				</SAHL:SAHLButton>
				<SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
				</SAHL:SAHLButton>
			</td>
		</tr>
	</table>
</asp:Content>
