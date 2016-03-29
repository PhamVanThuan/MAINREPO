<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
	CodeBehind="CreateCase.aspx.cs" Inherits="SAHL.Web.Views.Migrate.CreateCase" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
	<script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
	<script language="javascript" type="text/javascript">
		$(document).ready(function () {
			validateNextButtonForLegalEntities();
			$("input:checkbox").click(validateNextButtonForLegalEntities);
		});

		//Validate Next Button For Legal Entities
		function validateNextButtonForLegalEntities() {
			if ($("input:checkbox:visible").length == 0) {
				$("#<%= btnNext.ClientID %>").attr("disabled", false);
			}
			else {
				if ($("input:checked").length > 0) {
					$("#<%= btnNext.ClientID %>").attr("disabled", false);
				}
				else {
					$("#<%= btnNext.ClientID %>").attr("disabled", true);
				}
			}
		}

		function getnWorkingDaysFromDate(dteBox) {
		    SAHL.Web.AJAX.Account.GetnWorkingDaysFromDate(60, $(dteBox).val().toString(), getnWorkingDaysFromDate_callback);
		}

		function getnWorkingDaysFromDate_callback(result) {
		    $("#<%= dte60Days.ClientID %>").text(result);
		}
	</script>
	<table class="tableStandard" width="100%">
		<tr>
			<td>
				<SAHL:SAHLLabel runat="server" ID="lblAccountNumber" Font-Bold="true" Text="Account Number : "></SAHL:SAHLLabel>
				<SAHL:SAHLLabel ID="lblAccountKey" runat="server" Visible="false" />
				<SAHL:SAHLTextBox ID="txtAccountKey" runat="server" MaxLength="7" Width="180px" />
				<SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acAccount" TargetControlID="txtAccountKey"
					OnItemSelected="OnAccountItemSelected" MinCharacters="1">
				</SAHL:SAHLAutoComplete>
			</td>
		</tr>
		<tr runat="server" id="trLegalEntities" visible="false">
			<td>
				<table>
					<tr>
						<td>
							<SAHL:SAHLGridView ID="grdLegalEntities" runat="server" AutoGenerateColumns="false"
								FixedHeader="false" EnableViewState="false" GridWidth="100%" Width="98%" HeaderCaption="Account Legal Entities"
								PostBackType="None" NullDataSetMessage="There are no Legal Entities." EmptyDataSetMessage="There are no Legal Entities."
								OnRowDataBound="grdLegalEntities_OnRowDataBound">
								<HeaderStyle CssClass="TableHeaderB" />
								<RowStyle CssClass="TableRowA" />
							</SAHL:SAHLGridView>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr runat="server" id="trCaseDetails" visible="false">
			<td>
				<table>
					<tr>
						<td>
							Case Consultant:
						</td>
						<td>
							<SAHL:SAHLDropDownList runat="server" ID="ddlConsultant" PleaseSelectItem="true" />
						</td>
					</tr>
                    <tr>
						<td>
							17.1 Date:
						</td>
						<td>
							<SAHL:SAHLDateBox runat="server" ID="dte171" OnChange="getnWorkingDaysFromDate(this)" />
						</td>
					</tr>
					<tr>
						<td>
							Court Order Date:
						</td>
						<td>
							<SAHL:SAHLDateBox runat="server" ID="dteCourtOrder" />
						</td>
					</tr>
					<tr>
						<td>
							Termination Date:
						</td>
						<td>
							<SAHL:SAHLDateBox runat="server" ID="dteTermination" />
						</td>
					</tr>
					<tr>
						<td>
							60 Days Date:
						</td>
						<td>
							<SAHL:SAHLLabel runat="server" ID="dte60Days" />
						</td>
					</tr>
					<tr>
						<td>
							Payment Recieved Date:
						</td>
						<td>
							<SAHL:SAHLDateBox runat="server" ID="dtePaymentRecieved" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr runat="server" id="trButtons">
			<td align="right">
				<SAHL:SAHLButton runat="server" ID="btnBack" OnClick="btnBack_Click" Text="<--" Visible="false" />
				<SAHL:SAHLButton runat="server" ID="btnNext" OnClientClick="" OnClick="btnNext_Click"
					Text="-->" Visible="false" />
			</td>
		</tr>
	</table>
</asp:Content>
