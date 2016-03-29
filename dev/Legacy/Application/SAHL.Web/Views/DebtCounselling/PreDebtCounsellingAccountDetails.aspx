<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SAHL.Master" AutoEventWireup="true"
	CodeBehind="PreDebtCounsellingAccountDetails.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.PreDebtCounsellingAccountDetails" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>
<asp:Content ID="cntSnapShot" ContentPlaceHolderID="Main" runat="server">
	<asp:Panel runat="server" ID="pnlEmptyDebtCounsellingSnapShot" Visible="false">
		<span class="titleText">There is no snapshot details as no accepted debt counselling
			proposal exists for this account.</span>
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlDebtCounsellingSnapShot" Visible="false">
		<div class="TableHeaderA" width="100%">
			Pre-Debt Counselling Account Snap Shot Details
		</div>
		<table width="30%" class="tableStandard">
			<tr>
				<td class="titleText" width="50%">
					Product :
				</td>
				<td>
					<asp:Label runat="server" ID="lblProduct"></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="titleText">
					Term :
				</td>
				<td>
					<asp:Label runat="server" ID="lblTerm"></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="titleText">
					HOC Premium :
				</td>
				<td>
					<asp:Label runat="server" ID="lblHOCPremium"></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="titleText">
					Life Premium :
				</td>
				<td>
					<asp:Label runat="server" ID="lblLifePremium"></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="titleText">
					Monthly Service Fee :
				</td>
				<td>
					<asp:Label runat="server" ID="lblMonthlyServiceFee"></asp:Label>
				</td>
			</tr>
		</table>
		<br />
		<h5 style="text-align:center;">
			Instalment / Rate Information</h5>
		<table class="tableStandard" width="100%">
			<thead>
				<tr>
					<th>
						Financial Service Type
					</th>
					<th>
						Instalment
					</th>
					<th>
						Link Rate
					</th>
					<th>
						Market Rate
					</th>
					<th>
						Effective Rate
					</th>
				</tr>
			</thead>
			<tr>
				<td>
					Variable
				</td>
				<td>
					<asp:Label runat="server" ID="lblVariableInstallment"></asp:Label>
				</td>
				<td>
					<asp:Label runat="server" ID="lblVariableLinkRate"></asp:Label>
				</td>
				<td>
					<asp:Label runat="server" ID="lblVariableMarketRate"></asp:Label>
				</td>
				<td>
					<asp:Label runat="server" ID="lblVariableInterestRate"></asp:Label>
				</td>
			</tr>
			<tr id="divFixed" runat="server">
				<td>
					Fixed
				</td>
				<td>
					<asp:Label runat="server" ID="lblFixedInstallment"></asp:Label>
				</td>
				<td>
					<asp:Label runat="server" ID="lblFixedLinkRate"></asp:Label>
				</td>
				<td>
					<asp:Label runat="server" ID="lblFixedMarketRate"></asp:Label>
				</td>
				<td>
					<asp:Label runat="server" ID="lblFixedInterestRate"></asp:Label>
				</td>
			</tr>
		</table>
		<div style="clear: both;">
		</div>
		<br />
		<br />
		<SAHL:DXGridView ID="gridFinancialAdjustments" runat="server" AutoGenerateColumns="False" FixedHeader="false"
			Width="100%" EnableViewState="false" PostBackType="None">
			<SettingsText Title="Financial Adjustments" />
			<Settings ShowTitlePanel="true" />
		</SAHL:DXGridView>
        <br />
        <div id="DebtCounsellingInfo" runat="server">
        <div id="DebtCounsellingCancelledHeading" class="TableHeaderA" width="100%" runat="server"></div>
        <table width="30%" class="tableStandard">
            <tr>
	            <td class="titleText">
		            Current Balance :
	            </td>
	            <td>
		            <asp:Label runat="server" ID="lblCurrentBalance"></asp:Label>
	            </td>
            </tr>
            <tr>
	            <td class="titleText" width="50%">
		            Term :
	            </td>
	            <td>
		            <asp:Label runat="server" ID="lblTermOptOutToday"></asp:Label>
	            </td>
            </tr>
            <tr>
	            <td class="titleText">
		            HOC Premium :
	            </td>
	            <td>
		            <asp:Label runat="server" ID="lblHOCPremiumOptOutToday"></asp:Label>
	            </td>
            </tr>
            <tr>
	            <td class="titleText">
		            Life Premium :
	            </td>
	            <td>
		            <asp:Label runat="server" ID="lblLifePremiumOptOutToday"></asp:Label>
	            </td>
            </tr>
            <tr>
	            <td class="titleText">
		            Monthly Service Fee :
	            </td>
	            <td>
		            <asp:Label runat="server" ID="lblMonthlyServiceFeeOptOutToday"></asp:Label>
	            </td>
            </tr>
        </table>
        <br />
        <h5 style="text-align:center;">Instalment / Rate Information</h5>
		<table class="tableStandard" width="100%">
			<thead>
				<tr>
					<th>
						Financial Service Type
					</th>
					<th>
						Instalment
					</th>
					<th>
						Link Rate
					</th>
					<th>
						Market Rate
					</th>
					<th>
						Effective Rate
					</th>
				</tr>
			</thead>
			<tr>
				<td>
					Variable
				</td>
				<td>
					<asp:Label runat="server" ID="lblVariableInstallmentOptOutToday"></asp:Label>
				</td>
				<td>
					<asp:Label runat="server" ID="lblVariableLinkRateOptOutToday"></asp:Label>
				</td>
				<td>
					<asp:Label runat="server" ID="lblVariableMarketRateOptOutToday"></asp:Label>
				</td>
				<td>
					<asp:Label runat="server" ID="lblVariableInterestRateOptOutToday"></asp:Label>
				</td>
			</tr>
		</table>
        </div>
	</asp:Panel>
</asp:Content>
