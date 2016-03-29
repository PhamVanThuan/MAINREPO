<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" CodeBehind="NonPerformingLoan.aspx.cs" Title="Non Performing Loan" Inherits="SAHL.Web.Views.Common.NonPerformingLoan" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
	<div style="text-align: left">
		<table width="100%" class="tableStandard">
			<tr>
				<td style="width: 100%;" class="TitleText">Please be aware that by ticking the checkbox below, you are suspending interest on this loan.
				</td>
			</tr>
		</table>
		<br />
		<br />
		<table style="width: 100%">
			<tr>
				<td style="width: 100%;" class="TitleText">If you are unsure please refer to Loss Control: Litigation before performing this action.
				</td>
			</tr>
		</table>
		<br />
		<br />
		<table style="width: 100%">
			<tbody>
				<tr>
					<td></td>
					<td align="center">
						<asp:Label ID="lblSuspendingInterestDetails" runat="server" Font-Bold="True" Font-Underline="True"
							Text="Suspending Interest Details"></asp:Label>
					</td>
					<td></td>
				</tr>
				<tr>
					<td style="height: 81px"></td>
					<td align="center" style="height: 81px">
						<table class="borderAll tableStandard" id="TABLE1" style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; width: 332px; border-bottom: black thin solid">
							<thead>
								<tr>
									<th></th>
									<th>Variable</th>
									<th runat="server" id="headerFixed" visible="false">Fixed</th>
								</tr>
							</thead>
							<tbody>
								<tr>
									<td align="left" style="width: 192px; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">
										<asp:Label ID="Label1" runat="server" Text="Suspended Interest to date" Width="168px"></asp:Label></td>
									<td align="right" style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; width: 170px; border-bottom: black thin solid">
										<asp:Label ID="lblSuspendedInterestAmt" runat="server" Style="text-align: right"
											Width="120px">0</asp:Label></td>
									<td runat="server" id="tdSuspendedInterestFixed" visible="false" align="right" style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; width: 170px; border-bottom: black thin solid">
										<asp:Label ID="lblSuspendedInterestFixedAmount" runat="server" Style="text-align: right"
											Width="120px">0</asp:Label></td>
								</tr>
								<tr>
									<td align="left" style="width: 192px; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">
										<asp:Label ID="Label2" runat="server" Text="Month to date Interest Amount" Width="194px"></asp:Label></td>
									<td align="right" style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; width: 170px; color: black; border-bottom: black thin solid">&nbsp;<asp:Label ID="lblMonthToDateInterestAmt" runat="server" Style="text-align: right"
										Width="81px">0</asp:Label></td>
									<td runat="server" id="tdMonthToDateInterestFixed" visible="false" align="right" style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; width: 170px; color: black; border-bottom: black thin solid">&nbsp;<asp:Label ID="lblMonthToDateInterestFixedAmt" runat="server" Style="text-align: right"
										Width="81px">0</asp:Label></td>
								</tr>
							</tbody>
						</table>
					</td>
					<td style="height: 81px"></td>
				</tr>
			</tbody>
		</table>
		<br />
		<br />
		<table class="tableStandard" style="width: 100%">
			<tr>
				<td style="width: 20%;" class="TitleText">Mark as Non-Performing
				</td>
				<td style="width: 30%;">
					<asp:CheckBox ID="NonPerformingCheck" runat="server" Text="" AutoPostBack="True" OnCheckedChanged="NonPerformingCheck_CheckedChanged" />
				</td>
				<td style="width: 50%;"></td>
			</tr>
		</table>
		<table width="100%" class="tableStandard">
			<tr class="rowStandard">
				<td align="center" style="width: 100%">
					<SAHL:SAHLButton ID="btnProceed" runat="server" Text="Proceed" AccessKey="P" OnClick="btnProceed_Click" />
					<SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click" />
				</td>
			</tr>
		</table>
	</div>
</asp:Content>
