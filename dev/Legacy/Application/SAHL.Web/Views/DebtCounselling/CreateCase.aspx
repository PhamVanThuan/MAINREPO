<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
	CodeBehind="CreateCase.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.CreateCase" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
	<script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
	<script type="text/javascript" language="javascript">
		//Find Legal Entities
		function FindLegalEntities(textbox) {
			if ($(textbox).val().toString().length > 5) {
				$.ajax({
					type: "POST",
					data: "{'prefix':'" + $(textbox).val().toString() + "'}",
					dataType: "json",
					url: "../../Ajax/LegalEntity.asmx/GetLegalEntitiesByIDOrPassportNumber",
					contentType: "application/json; charset=utf-8",
					success: onSuccess
				});
			}
			else {
				$("#bodyQueryResults").find("tr").remove();
			}
		}

		//Results from GetLegalEntitiesByIDOrPassportNumber
		function onSuccess(jsonData) {
			if (jsonData.d.length < 3) {
				$("#thResultsHeader").hide();
				$("#bodyQueryResults").find("tr").remove();
				$("#bodyQueryResults").append("<tr><td style=\"font-weight:bold;\">No Results</td></tr>");
			}
			else {
				$("#thResultsHeader").show();
				$("#bodyQueryResults").find("tr").remove();
				var obj = $.parseJSON(jsonData.d);
				$.each(obj, function (index, data) {
					$("#bodyQueryResults").append("<tr onclick=\"__doPostBack('LegalEntitySearchPattern'," + data.Key + ");\" onmouseout=\"this.className=''\" onmouseover=\"this.className='HighlightLightGrey'\"><td><img src='../../Images/Search.png'>" + data.LegalName + "</td><td>" + data.IDNumber + "</td></tr>");
				});
			}
		}

		$(document).ready(function () {
			if ($("#<%= footRelatedResults.ClientID %> tr").length == 0) {
				$("#thResultsHeader").hide();
			}
			else {
				$("#thResultsHeader").show();
			}
		});

		function DeSelectFromAccount(key) {
			$("#" + "<%= trvAccounts.ClientID %> input:checkbox").each(function () {
				if ($(this).val().indexOf("-" + key) >= 0) {
					$(this).attr('checked', false);
				}
			});
		}

		function HighlightRelated(object, accountClass, legalEntityClass) {
			$("#" + "<%= trvPeople.ClientID %> ." + legalEntityClass + " input:checkbox").each(function () {
				if ($(this).attr('checked') == false) {
					$("." + accountClass).removeClass('Highlight');
					$("." + legalEntityClass).removeClass('Highlight');
				}
			});
			$("#" + "<%= trvPeople.ClientID %> ." + accountClass + " input:checkbox").each(function () {
				if ($(this).attr('checked') == false) {
					$("." + accountClass).removeClass('Highlight');
					$("." + legalEntityClass).removeClass('Highlight');
				}
			});
			$("#" + "<%= trvPeople.ClientID %> ." + legalEntityClass + " input:checkbox").each(function () {
				if ($(this).attr('checked') == true) {
					$("." + accountClass).addClass('Highlight');
					$("." + legalEntityClass).addClass('Highlight');
				}
			});
			$("#" + "<%= trvPeople.ClientID %> ." + accountClass + " input:checkbox").each(function () {
				if ($(this).attr('checked') == true) {
					$("." + accountClass).addClass('Highlight');
					$("." + legalEntityClass).addClass('Highlight');
				}
			});
		}
	</script>
	<div class="TableHeaderA">
		Create Debt Counselling</div>
	<table class="tableStandard">
		<tr>
			<td width="160px" class="TitleText">
				Debt Counsellor:
			</td>
			<td>
				<SAHL:SAHLLabel ID="lblDebtCounsellor" runat="server" MaxLength="20" Width="180px"></SAHL:SAHLLabel>
			</td>
		</tr>
		<tr>
			<td style="vertical-align: text-top;" class="TitleText">
				17.1 Date:
			</td>
			<td>
				<SAHL:SAHLDateBox ID="dte17pt1Date" runat="server" Mandatory="true" />
			</td>
		</tr>
        <tr>
			<td style="vertical-align: text-top;" class="TitleText">
				Reference Number:
			</td>
			<td>
                <SAHL:SAHLTextBox ID="txtReferenceNo" runat="server" Mandatory="false" />
			</td>
		</tr>
	</table>
	<div class="TableHeaderB">
		Search</div>
	<table class="tableStandard">
		<tr>
			<td width="160px" class="TitleText">
				ID/Passport Number:
			</td>
			<td>
				<SAHL:SAHLTextBox ID="txtPassportNumber" runat="server" MaxLength="20" Width="180px"
					onkeydown="FindLegalEntities(this)" onkeyup="FindLegalEntities(this)" onblur="FindLegalEntities(this)"
					onkeypress="FindLegalEntities(this)" TabIndex="2" />
			</td>
			<td>
				<img src='..\..\Images\Search.png' alt="Type in the search box">
			</td>
		</tr>
	</table>
	<div id="divResults" style="width: 100%; max-height: 150px; overflow: auto;">
		<table id="tblResults" width="99%">
			<colgroup align="left" valign="top" width="300px">
			</colgroup>
			<colgroup align="center" valign="top">
			</colgroup>
			<thead id="thResultsHeader" style="display: none">
				<tr>
					<td class="TableHeaderB">
						Legal Name
					</td>
					<td class="TableHeaderB">
						ID Number
					</td>
				</tr>
			</thead>
			<tbody id="bodyQueryResults">
			</tbody>
			<tfoot runat="server" id="footRelatedResults">
			</tfoot>
		</table>
	</div>
	<div id="divRelatedLegalEntities" runat="server" style="width: 550px;">
	</div>
	<table style="background-color: #f9f9f9; width: 99%;" class="tableStandard">
		<tr>
			<td align="left" valign="top" class="TableHeaderB" style="width: 350px;">
				People of Importance
			</td>
			<td align="left" valign="top" class="TableHeaderB">
				Accounts of Importance
			</td>
		</tr>
		<tr>
			<td style="vertical-align: text-top;">
				<SAHL:SAHLTreeView runat="server" ID="trvPeople" OnNodeSelected="OnRemovePersonClick" />
			</td>
			<td style="vertical-align: text-top;">
				<SAHL:SAHLTreeView runat="server" ID="trvAccounts" EnableViewState="false" CheckBoxesVisible="true"
					Style="max-height: 200px; overflow: auto;" />
			</td>
		</tr>
	</table>
	<br />
	<div style="float: right;">
		<SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Create Case(s)" OnClick="OnCreateCaseClick" />
		<SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="OnCancelClick" />
	</div>
</asp:Content>
