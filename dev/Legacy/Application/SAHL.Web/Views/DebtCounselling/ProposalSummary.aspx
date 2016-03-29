<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
	CodeBehind="ProposalSummary.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.ProposalSummary"
	Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: center">
		<table width="100%" class="tableStandard">
			<tr>
				<td>
					<SAHL:DXGridView ID="gridProposalSummary" runat="server" AutoGenerateColumns="False"
						PostBackType="NoneWithClientSelect" Width="100%" KeyFieldName="Key">
						<SettingsText Title="Debt Counselling Proposals" EmptyDataRow="Debt Counselling Proposals" />
						<Settings ShowGroupPanel="True" />
						<Styles>
							<AlternatingRow Enabled="True">
							</AlternatingRow>
						</Styles>
						<Border BorderWidth="2px"></Border>
					</SAHL:DXGridView>
				</td>
			</tr>
			<tr id="ButtonRow" runat="server">
				<td align="center">
					<SAHL:SAHLButton ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" SecurityTag="ProposalSummaryAdd"
						CausesValidation="False" />
					<SAHL:SAHLButton ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" SecurityTag="ProposalSummaryUpdate"
						Text="Update" CausesValidation="False" />
					<SAHL:SAHLButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" SecurityTag="ProposalSummaryDelete"
						Text="Delete" CausesValidation="False" />
					<SAHL:SAHLButton ID="btnView" runat="server" OnClick="btnView_Click" Text="View"
						CausesValidation="False" />
					<SAHL:SAHLButton ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="Print" 
                        CausesValidation="False" SecurityTag="ProposalPrint"/>
					<SAHL:SAHLButton ID="btnCopyToDraft" runat="server" OnClick="btnCopyToDraft_Click"
						Text="Copy to Draft" SecurityTag="ProposalSummaryCopy" CausesValidation="False" />
					<SAHL:SAHLButton ID="btnCreateCounterProposal" runat="server" OnClick="btnCreateCounterProposal_Click"
						Text="Create Counter Proposal" SecurityTag="ProposalSummaryCopy" CausesValidation="False" />
					<SAHL:SAHLButton ID="btnSetActive" runat="server" OnClick="btnSetActive_Click" Text="Set Active"
						SecurityTag="ProposalSummaryActivate" CausesValidation="False" />
					<SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
						CausesValidation="False" />
					<SAHL:SAHLButton ID="btnReasons" runat="server" OnClick="OnReasonsClick" Text="Reasons" CausesValidation="False" Visible="false" />
					<SAHL:SAHLButton ID="btnViewAmortisationSchedule" runat="server" OnClick="OnViewAmortisationScheduleClick" Text="View Amortisation Schedule" CausesValidation="False" Visible="true" OnClientClick="masterCancelBeforeUnload()" />
				</td>
			</tr>
		</table>
	</div>
</asp:Content>
