<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.master"
	CodeBehind="ProposalDetails.aspx.cs" Inherits=" SAHL.Web.Views.DebtCounselling.ProposalDetails" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="sahlControls" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
	TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v10.2" Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
	<script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
	<script src="../../Scripts/Plugins/Lightbox/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
	<link href="../../Scripts/Plugins/Lightbox/jquery.fancybox-1.3.4.css" rel="stylesheet"
		type="text/css" />
	<script type="text/javascript">
		function OnGridRowSelected(s, e) {
			if (!readonly())
				grid.GetSelectedFieldValues('Key;StartDate;EndDate;StartPeriod;EndPeriod;Amount;MarketRate;InterestRate;InstalmentPercent;AnnualEscalation;AdditionalAmount', OnGetSelectedFieldValues);
		}

		function readonly() {
			return ($("#<%=txtReadOnly.ClientID %>").val() == 'True');
		}

		function OnGetSelectedFieldValues(values) {

			if (values == null || values[0] == null || readonly())
				return;

			//Need to set the key
			$("#<%= txtSaveKey.ClientID %>").val(values[0][0]);
			$("#<%= dteStartDate.ClientID %>").val(getDateStringFromJSDate(values[0][1]));
			$("#<%= dteEndDate.ClientID %>").val(getDateStringFromJSDate(values[0][2]));
			$("#<%= txtStartPeriod.ClientID %>").val(values[0][3]);
			$("#<%= txtEndPeriod.ClientID %>").val(values[0][4]);

			//disable common inputs
			$("#<%= dteEndDate.ClientID %>").attr('disabled', 'disabled');
			$("#<%= txtEndPeriod.ClientID %>").attr('disabled', 'disabled');

			//hide the add button
			$("#<%= btnAdd.ClientID %>").attr('style', 'display: none');
			//show the update button
			$("#<%= btnSave.ClientID %>").attr('style', 'display: inline');

			SAHLCurrencyBox_setValue("<%= txtInterestRate.ClientID %>", getRateForInput(values[0][7]));

			var instAmnt = get2Decimal(roundVal(values[0][5]).toString());

			if ($("#<%= ProposalType.ClientID %>").val() == "CounterProposal") {
				$("#<%= txtInstalmentPercentage.ClientID %>").val(getRateForInput(values[0][8]));
				SAHLCurrencyBox_setValue("<%= txtInstalPercentDisplay.ClientID %>", get2Decimal(roundVal(getRateForInput(values[0][8])).toString()));
				SAHLCurrencyBox_setValue("<%= txtInstalment.ClientID %>", instAmnt);
				SAHLCurrencyBox_setValue("<%= txtAnnualEscalation.ClientID %>", getRateForInput(values[0][9]));
			}
			else {
				//disable inputs
				$("#<%= ddlMarketRate.ClientID %>").attr('disabled', 'disabled');

				SAHLCurrencyBox_setValue("<%= txtAmount.ClientID %>", instAmnt);
				SAHLCurrencyBox_setValue("<%= txtAdditionalAmount.ClientID %>", values[0][10].toString());
			}
		}

		function getDateStringFromJSDate(d) {
			if (d == null)
				return "";

			//For the SAHL date control, each parameter needs to be the correct length
			var dd = d.getDate();
			if (dd < 10)
				dd = "0" + dd;
			//JScript Month is a 0 bound array, so + 1
			var mm = Number(d.getMonth() + 1);
			if (mm < 10)
				mm = "0" + mm;

			if (!isNaN(dd) && !isNaN(mm) && !isNaN(d.getFullYear()))
				return dd + '/' + mm + '/' + d.getFullYear();
		}

		function getRateForInput(val) {
			var str = (val * 100).toString();

			if (str.indexOf('.') > -1)
				str += '0';

			return str;
		}

		$(window).resize(function () {
			$('#pageDiv').width($(window).width() - 285);
			$("#<%= accProposal.ClientID %>").width($(window).width() - 285);
			$("#<%= gridDiv.ClientID %>").width($(window).width() - 285);
		});

		$(document).ready(function () {
			$('#pageDiv').width($(window).width() - 285);
			$("#<%= accProposal.ClientID %>").width($(window).width() - 285);
			$("#<%= gridDiv.ClientID %>").width($(window).width() - 285);

			$("#proposalChartImage").width("180");
			$("#proposalChartImage").height("120");
			$("#proposalChartImage").attr("src", $("#<%= proposalGraph.ClientID %>").attr("src"));

			$("#inlineProposalGraph").click(function (s, e) {
				SetActiveGraph('ProposalGraph');
			});

			$("#inlineProposalGraph").fancybox({
				'hideOnContentClick': true
			});

			$("#secondaryProposalChartImage").width("180");
			$("#secondaryProposalChartImage").height("120");
			$("#secondaryProposalChartImage").attr("src", $("#<%= secondaryProposalGraph.ClientID %>").attr("src"));

			$("#inlineSecondaryProposalGraph").click(function (s, e) {
				SetActiveGraph('SecondaryProposalGraph');
			});

			$("#inlineSecondaryProposalGraph").fancybox({
				'hideOnContentClick': true
			});

			//Life and HOC might have changed and therefore the instalment must be recalculated
			CalculateInstalment('percent');
		});

		window.onload = function () {
			if (typeof _aspxAttachEventToDocument !== "undefined") {
				_aspxAttachEventToDocument("mousemove", OnMouseMove);
			}
		}

		function CalculateRelativeX(x, clickedElement) {
			var left = _aspxGetAbsoluteX(clickedElement);
			return Math.abs(x - left);
		}
		function CalculateRelativeY(y, clickedElement) {
			var top = _aspxGetAbsoluteY(clickedElement);
			return Math.abs(y - top);
		}
		function GetValueString(value) {
			if (!(value instanceof Date))
				return value.toString();
			var minutes = value.getMinutes();
			return (value.getUTCMonth() + 1) + "/" + value.getUTCDate() + " " + value.getUTCHours() + ":" + Math.round(minutes / 10).toString() + (minutes % 10).toString();
		}

		var graphName;

		function SetActiveGraph(name) {
			graphName = name;
		}

		function OnMouseMove(evt) {
			if (graphName == 'ProposalGraph') {
				if (typeof (window['proposalChart']) == 'undefined')
					return;

				var srcElement = _aspxGetEventSource(evt);

				if (proposalChart.GetMainDOMElement() != srcElement)
					return;
				var x = CalculateRelativeX(_aspxGetEventX(evt), srcElement);
				var y = CalculateRelativeY(_aspxGetEventY(evt), srcElement);
				var hitInfo = proposalChart.HitTest(x, y);

				var text = "";
				var seriesFound = false;
				for (var i = 0; i < hitInfo.length; i++)
					if (hitInfo[i].object instanceof ASPxClientSeries) {
						text += hitInfo[i].object.name;
						seriesFound = true;
						break;
					}
				for (var i = 0; i < hitInfo.length; i++)
					if (hitInfo[i].additionalObject instanceof ASPxClientSeriesPoint) {
						text += "<br>Term: " + GetValueString(hitInfo[i].additionalObject.argument);
						if (!hitInfo[i].additionalObject.isEmpty)
							text += "<br>Value: " + roundVal(hitInfo[i].additionalObject.values[0]);
						break;
					}
				var panel = document.getElementById("proposalChartPanel");
				if (text.length > 0) {
					panel.innerHTML = "<span style=\"white-space:nowrap\">" + text + "</span>";
					$("#proposalChartPanel").css({ "left": (x + 20) + "px", "top": (y + 20) + "px" });
					$("#proposalChartPanel").show();
				}
				else {
					panel.innerHTML = "";
					$("#proposalChartPanel").hide();
				}
			}
			else if (graphName == 'SecondaryProposalGraph') {
				if (typeof (window['secondaryProposalChart']) == 'undefined')
					return;

				var srcElement = _aspxGetEventSource(evt);

				if (secondaryProposalChart.GetMainDOMElement() != srcElement)
					return;
				var x = CalculateRelativeX(_aspxGetEventX(evt), srcElement);
				var y = CalculateRelativeY(_aspxGetEventY(evt), srcElement);
				var hitInfo = secondaryProposalChart.HitTest(x, y);

				var text = "";
				var seriesFound = false;
				for (var i = 0; i < hitInfo.length; i++)
					if (hitInfo[i].object instanceof ASPxClientSeries) {
						text += hitInfo[i].object.name;
						seriesFound = true;
						break;
					}
				for (var i = 0; i < hitInfo.length; i++)
					if (hitInfo[i].additionalObject instanceof ASPxClientSeriesPoint) {
						text += "<br>Term: " + GetValueString(hitInfo[i].additionalObject.argument);
						if (!hitInfo[i].additionalObject.isEmpty)
							text += "<br>Value: " + roundVal(hitInfo[i].additionalObject.values[0]);
						break;
					}
				var panel = document.getElementById("secondaryProposalChartPanel");
				if (text.length > 0) {
					panel.innerHTML = "<span style=\"white-space:nowrap\">" + text + "</span>";
					$("#secondaryProposalChartPanel").css({ "left": (x + 20) + "px", "top": (y + 20) + "px" });
					$("#secondaryProposalChartPanel").show();
				}
				else {
					panel.innerHTML = "";
					$("#secondaryProposalChartPanel").hide();
				}
			}
		}

		function roundVal(val) {
			var dec = 2;
			var result = Math.round(val * Math.pow(10, dec)) / Math.pow(10, dec);
			return result;
		}

		function setPercentAndCalc() {
			$("#<%= txtInstalmentPercentage.ClientID %>").val(SAHLCurrencyBox_getValue('<%= txtInstalPercentDisplay.ClientID %>'));
			CalculateInstalment('percent');
		}

		function CalculateInstalment(from) {
			// This calculation should only be performed for Counter Proposal            
			if (readonly() || $("#<%= ProposalType.ClientID %>").val() != "CounterProposal") {
				return;
			}
			var interestRate = SAHLCurrencyBox_getValue('<%= txtInterestRate.ClientID %>');
			var currentBalance = $("#<%= hdnCurrentBalance.ClientID %>").val();
			var remainingTerm = $("#<%= hdnRemainingTerm.ClientID %>").val();

			var hocMonthlyPremium = parseFloat($("#<%= hdnHOCIncl.ClientID %>").val());
			var lifeMonthlyPremium = parseFloat($("#<%= hdnLifeIncl.ClientID %>").val());
			var adminFee = parseFloat($("#<%= hdnAdminFee.ClientID %>").val());

			var monthlyInterestRate = (interestRate / 100.0) / 12.0;

			var instalmentPercentage = $("#<%= txtInstalmentPercentage.ClientID %>").val();
			if (from == 'percent' && (isNaN(instalmentPercentage) || instalmentPercentage == 0)) {
				return;
			}

			var amount = SAHLCurrencyBox_getValue('<%= txtInstalment.ClientID %>');
			if (from == 'amount' && (isNaN(amount) || amount == 0)) {
				return;
			}

			//alert('monthlyInterestRate: ' + monthlyInterestRate + ', remainingTerm: ' + remainingTerm + ', currentBalance: ' + currentBalance);

			var instalment = (monthlyInterestRate + (monthlyInterestRate / (Math.pow((1 + monthlyInterestRate), remainingTerm) - 1))) * currentBalance;

			//Check if the Calculation didn't result to a Null/NaN
			if (isNaN(instalment))
				return;

			//if HOC Inclusive
			if ($("#<%= ddlHOCIncl.ClientID %>").val() == "1")
				instalment += hocMonthlyPremium
			//if Life Inclusive
			if ($("#<%= ddlLifeIncl.ClientID %>").val() == "1")
				instalment += lifeMonthlyPremium;

			if ($("#<%= chkServiceFee.ClientID %>").is(':checked'))
				instalment += adminFee;

			//Instalment amount from percentage
			if (from == 'percent') {
				var instalmentAfterInstalmentPercentage = ((instalmentPercentage / 100.0) * instalment);
				SAHLCurrencyBox_setValue('<%= txtInstalment.ClientID %>', instalmentAfterInstalmentPercentage.toString());
			}
			else {
				//percentage from instalment amount
				var percentageOfInstalment = ((amount / instalment) * 100.0);
				$("#<%= txtInstalmentPercentage.ClientID %>").val(percentageOfInstalment);
				SAHLCurrencyBox_setValue('<%= txtInstalPercentDisplay.ClientID %>', get2Decimal(roundVal(percentageOfInstalment.toString()).toString()));
			}
		}

		function get2Decimal(value) {
			var ptIndex = value.indexOf('.');

			var decVal = '';
			var percVal = value;

			if (ptIndex > -1) {
				percVal = value.substring(0, ptIndex);
				decVal = value.substring(ptIndex + 1);
			}

			if (decVal.length == 0)
				decVal = '00';
			else if (decVal.length == 1)
				decVal = decVal + '0';
			else if (decVal.length > 2)
				decVal = decVal.substring(0, 2);

			return percVal + '.' + decVal
		}
	</script>
	<script type="text/javascript" language="javascript">


		// **** Date Edit **** //
		function setItemEndPeriod() {
			//get the periods between the dates
			var periods = monthDiff(getJSDateFromString($("#<%=dteStartDate.ClientID %>").val()), getJSDateFromString($("#<%=dteEndDate.ClientID %>").val()));
			//add to the Start Periods
			periods += Number($("#<%=txtStartPeriod.ClientID %>").val());
			if (!isNaN(periods))
				$("#<%=txtEndPeriod.ClientID %>").val(periods);
		}

		function monthDiff(d1, d2) {
			var months;
			months = (d2.getFullYear() - d1.getFullYear()) * 12;
			months -= d1.getMonth() + 1;
			months += d2.getMonth();

			//if the start date is the 1st, add that as a month
			if (d1.getDate() == "1")
				months += 1;

			return months;
		}
		// **** Date Edit **** //


		// **** Period Edit **** //
		function getDate(date, period) {
			//get the future date
			var d = new Date(date.setMonth(date.getMonth() + Number(period)));
			//minus 1 day
			return new Date(d.setDate(d.getDate() - 1));
		}

		function getJSDateFromString(string) {
			var d = string.toString().split("/");
			//JScript Month is a 0 bound array, so -1
			return new Date(d[2], d[1] - 1, d[0]);
		}

		function setItemEndDate() {
			if (Number($("#<%= txtEndPeriod.ClientID %>").val()) < 1)
				return;

			var d = getDate(getJSDateFromString($("#<%= dteStartDate.ClientID %>").val()), Number($("#<%= txtEndPeriod.ClientID %>").val()) - Number($("#<%= txtStartPeriod.ClientID %>").val()) + 1);
			//For the SAHL date control, each parameter needs to be the correct length
			var dd = d.getDate();
			if (dd < 10)
				dd = "0" + dd;
			//JScript Month is a 0 bound array, so + 1
			var mm = Number(d.getMonth() + 1);
			if (mm < 10)
				mm = "0" + mm;

			if (!isNaN(dd) && !isNaN(mm) && !isNaN(d.getFullYear()))
				$("#<%= dteEndDate.ClientID %>").val(dd + '/' + mm + '/' + d.getFullYear());
		}
		// **** Period Edit **** //
	</script>
	<ajaxToolkit:TabContainer runat="server" ID="ProposalTab">
		<ajaxToolkit:TabPanel runat="server" ID="ProposalInformation" HeaderText="Proposal information">
			<ContentTemplate>
				<div id="pageDiv">
					<asp:HiddenField runat="server" ID="ProposalType" />
					<asp:Panel runat="server" ID="pnlPropInfo" GroupingText="Proposal Information">
						<table id="tblProposalHeader" runat="server" class="tableStandard" style="width:100%;">
							<tr>
								<%--HOC--%>
								<td style="width:80px;"><SAHL:SAHLLabel ID="lblHOC" runat="server" Text="HOC" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblHOCIncl" runat="server"></SAHL:SAHLLabel><asp:HiddenField runat="server" ID="hdnHOCIncl" /><SAHL:SAHLDropDownList ID="ddlHOCIncl" runat="server" AutoPostBack="False" OnSelectedIndexChanged="ddlHOCIncl_SelectedIndexChanged"></SAHL:SAHLDropDownList></td>
								<td><SAHL:SAHLLabel ID="lblLife" runat="server" Text="Life" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><asp:HiddenField runat="server" ID="hdnLifeIncl" /><SAHL:SAHLLabel ID="lblLifeIncl" runat="server"></SAHL:SAHLLabel><SAHL:SAHLDropDownList ID="ddlLifeIncl" runat="server" AutoPostBack="False" OnSelectedIndexChanged="ddlLifeIncl_SelectedIndexChanged"></SAHL:SAHLDropDownList></td>
								<td><SAHL:SAHLLabel ID="SAHLLabel1" runat="server" Text="Service Fee" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><asp:HiddenField ID="hdnAdminFee" runat="server" /><SAHL:SAHLCheckbox ID="chkServiceFee" style="vertical-align:middle;" runat="server"></SAHL:SAHLCheckbox></td>
							</tr>
							<tr>
								<td><SAHL:SAHLLabel ID="lblAccountHOCPremium" runat="server" Text="Premium" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblProposalHOC" runat="server"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblAccountLifePremium" runat="server" Text="Premium" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblProposalLife" runat="server"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblAccountMonthlyServiceFee" runat="server" Text="Amount" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblProposalFees" runat="server"></SAHL:SAHLLabel></td>
							</tr>
							<tr>
								<td><SAHL:SAHLLabel ID="lblAccountOpeningBalance" runat="server" Text="Opening Balance" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblOpeningBalance" runat="server"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblAccountClosingBalance" runat="server" Text="Closing Balance" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblClosingBalance" runat="server"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblStatusDesc" runat="server" Text="Status" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblProposalStatus" runat="server"></SAHL:SAHLLabel></td>
							</tr>

							<tr id="trCounterProposal" runat="server" visible="false">
								<td><SAHL:SAHLLabel ID="lblCurrentRemainingTermDesc" runat="server" Text="Current Remaining Term" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><asp:HiddenField runat="server" ID="hdnRemainingTerm" /><SAHL:SAHLLabel ID="lblCurrentRemainingTerm" runat="server"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblProposalRemainingTermDesc" runat="server" Text="Proposal Remaining Term" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblProposalRemainingTerm" runat="server"></SAHL:SAHLLabel></td>
								<td><SAHL:SAHLLabel ID="lblReviewDate" runat="server" Text="Review Date" Font-Bold="True"></SAHL:SAHLLabel></td>
								<td>
									<%--<SAHL:SAHLLabel ID="lblReviewDateVal" runat="server" Text="Review Date"></SAHL:SAHLLabel>--%>
									<SAHL:SAHLDateBox ID="dteReviewDate" runat="server"></SAHL:SAHLDateBox>
								</td>
							</tr>
						</table>
					</asp:Panel>
					<ajaxToolkit:Accordion ID="accProposal" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
						HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="" FadeTransitions="false"
						FramesPerSecond="40" TransitionDuration="250" AutoSize="None" SuppressHeaderPostbacks="true">
						<Panes>
							<ajaxToolkit:AccordionPane ID="apReason" runat="server" Enabled="true">
								<Header>
									<a href="">Counter Proposal Notes</a>
								</Header>
								<Content>
									<table class="tableStandard" width="100%">
										<tr>
											<td class="TitleText">Description:
											</td>
											<td>
												<SAHL:SAHLTextBox runat="server" ID="txtCounterReason" Rows="9" Columns="80" TextMode="MultiLine"
													class="TitleText" />
											</td>
										</tr>
									</table>
								</Content>
							</ajaxToolkit:AccordionPane>
							<ajaxToolkit:AccordionPane ID="apAccountSummary" runat="server" Enabled="true">
								<Header>
									<a href="">Account Summary</a>
								</Header>
								<Content>
									<table class="tableStandard" width="100%">
										<tr>
											<td class="TitleText">SPV:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblSPV" Text="Main Street 65 (Pty) Ltd - 18th" />
											</td>
											<td class="TitleText">Debit Order Day:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblDODay" Text="99" />
											</td>
										</tr>
										<tr>
											<td class="TitleText">Current Balance:
											</td>
											<td class="LabelText">
												<asp:HiddenField runat="server" ID="hdnCurrentBalance" />
												<asp:Label runat="server" ID="lblCurrentBalance" Text="R 0 000 000.00" />
											</td>
											<td class="TitleText">Instalment Amount:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblInstalmentAmount" Text="R 00 000.00" />
											</td>
										</tr>
										<tr>
											<td class="TitleText">Arrear Balance:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblArrearBalance" Text="R 00 000.00" />
											</td>
											<td class="TitleText">Months in Arrear:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblMonthsInArrear" Text="3" />
											</td>
										</tr>
										<tr>
											<td class="TitleText">Loan Agreement Amount:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblLAA" Text="R 0 000 000.00" />
											</td>
											<td class="TitleText">Total Instalment:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblTotalInstalment" Text="R 00 000.00" />
											</td>
										</tr>
										<tr>
											<td class="TitleText">Total Bond Amount:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblTotalBondsRegistered" Text="R 0 000 000.00" />
											</td>
											<td class="TitleText">Fixed DO Amount:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblFixedDOAmount" Text="R 00 000.00" />
											</td>
										</tr>
										<tr>
											<td class="TitleText">Loan Open Date:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblAccountOpenDate" Text="01/01/2010" />
											</td>
											<td class="TitleText">Loan To Value:
											</td>
											<td class="LabelText">
												<asp:Label runat="server" ID="lblLTV" Text="95%" />
											</td>
										</tr>
									</table>
								</Content>
							</ajaxToolkit:AccordionPane>
							<ajaxToolkit:AccordionPane ID="apDetailGrid" runat="server" Enabled="true">
								<Header>
									<a href="">Details</a>
								</Header>
								<Content>
									<table id="tblProposalGrid" class="tableStandard">
										<tr>
											<td>
												<div runat="server" id="gridDiv">
													<SAHL:DXGridView ID="gvProposalItems" runat="server" AutoGenerateColumns="False"
														ClientInstanceName="grid" Settings-ShowTitlePanel="true" SettingsText-Title="Proposal Details"
														Settings-ShowGroupPanel="false" Settings-ShowHorizontalScrollBar="false" ClientSideEvents-SelectionChanged="function(s, e) { OnGridRowSelected(s, e); }"
														SettingsBehavior-AllowSelectSingleRowOnly="true" SettingsBehavior-AllowSelectByRowClick="true"
														SettingsBehavior-ColumnResizeMode="Control">
													</SAHL:DXGridView>
												</div>
											</td>
										</tr>
									</table>
								</Content>
							</ajaxToolkit:AccordionPane>
						</Panes>
					</ajaxToolkit:Accordion>
					<table class="tableStandard" width="100%">
						<tr>
							<td>
								<table id="tblProposalDetails" runat="server" class="tableStandard">
									<tr>
										<td style="width: 200px">
											<SAHL:SAHLLabel ID="lblStartDate" runat="server" Text="Start Date" />
										</td>
										<td style="width: 200px">
											<SAHL:SAHLDateBox ID="dteStartDate" runat="server" onblur="setItemEndPeriod(); setItemEndDate()"
												onchange="setItemEndPeriod(); setItemEndDate()"></SAHL:SAHLDateBox>
										</td>
									</tr>
									<tr>
										<td style="width: 200px">Start Period
										</td>
										<td style="width: 200px">
											<SAHL:SAHLTextBox runat="server" ID="txtStartPeriod" CssClass="mandatory" DisplayInputType="Number"
												Enabled="false" MaxLength="3" Width="30" />
										</td>
									</tr>
									<tr>
										<td>
											<SAHL:SAHLLabel ID="lblEndDate" runat="server" Text="End Date" />
										</td>
										<td>
											<SAHL:SAHLDateBox ID="dteEndDate" runat="server" onblur="setItemEndPeriod()" onchange="setItemEndPeriod()"></SAHL:SAHLDateBox>
										</td>
									</tr>
									<tr>
										<td style="width: 200px">End Period
										</td>
										<td style="width: 200px">
											<SAHL:SAHLTextBox runat="server" ID="txtEndPeriod" CssClass="mandatory" DisplayInputType="Number"
												MaxLength="3" Width="30" onblur="setItemEndDate()" />
										</td>
									</tr>
									<tr runat="server" id="MarketRateRow">
										<td>
											<SAHL:SAHLLabel ID="lblMarketRate" runat="server" Text="Market Rate" />
										</td>
										<td>
											<SAHL:SAHLDropDownList ID="ddlMarketRate" runat="server">
											</SAHL:SAHLDropDownList>
										</td>
									</tr>
									<tr>
										<td>
											<SAHL:SAHLLabel ID="lblInterestRate" runat="server" Text="Interest Rate" />
										</td>
										<td>
											<SAHL:SAHLCurrencyBox ID="txtInterestRate" runat="server" TextAlign="Right"></SAHL:SAHLCurrencyBox>
											<asp:HiddenField runat="server" ID="hiddenInterestRate" />
										</td>
									</tr>
									<tr runat="server" id="AmountRow">
										<td>
											<SAHL:SAHLLabel ID="lblAmount" runat="server" Text="Amount" />
										</td>
										<td>
											<SAHL:SAHLCurrencyBox ID="txtAmount" runat="server" TextAlign="Right"></SAHL:SAHLCurrencyBox>
										</td>
									</tr>
									<tr runat="server" id="AdditionalAmountRow">
										<td>
											<SAHL:SAHLLabel ID="lblAdditionalAmount" runat="server" Text="Additional Amount" />
										</td>
										<td>
											<SAHL:SAHLCurrencyBox ID="txtAdditionalAmount" runat="server" TextAlign="Right"></SAHL:SAHLCurrencyBox>
										</td>
									</tr>
									<tr runat="server" id="InstalmentPercentageRow">
										<td>
											<SAHL:SAHLLabel ID="lblInstalmentPercentage" runat="server" Text="Instalment %" />
										</td>
										<td>
											<SAHL:SAHLCurrencyBox ID="txtInstalPercentDisplay" runat="server" TextAlign="Right"></SAHL:SAHLCurrencyBox>
											<asp:TextBox ID="txtInstalmentPercentage" runat="server" TextAlign="Right" Style="display: none"></asp:TextBox>
										</td>
									</tr>
									<tr runat="server" id="InstalmentRow">
										<td>
											<SAHL:SAHLLabel ID="lblInstalment" runat="server" Text="Instalment" />
										</td>
										<td>
											<SAHL:SAHLCurrencyBox ID="txtInstalment" runat="server" TextAlign="Right"></SAHL:SAHLCurrencyBox>
										</td>
									</tr>
									<tr runat="server" id="AnnualEscalationRow">
										<td>
											<SAHL:SAHLLabel ID="lblAnnualEscalation" runat="server" Text="Annual Escalation %" />
										</td>
										<td>
											<SAHL:SAHLCurrencyBox ID="txtAnnualEscalation" runat="server" TextAlign="Right"></SAHL:SAHLCurrencyBox>
										</td>
									</tr>
								</table>
							</td>
							<td>
								<div runat="server" id="proposalGraphContainer">
									<SAHL:SAHLLabel runat="server" ID="lblProposalGraph"></SAHL:SAHLLabel>
									<br />
									<div style="display: none">
										<div id="proposalChartData" style="width: 900px; height: 600px;">
											<div id="proposalChartPanel" style="border: 1px solid black; background-color: white; position: absolute; height: 40px;">
											</div>
											<sahlControls:ProposalGraph ID="proposalGraph" Width="880" Height="560" runat="server">
											</sahlControls:ProposalGraph>
										</div>
									</div>
									<a id="inlineProposalGraph" href="#proposalChartData">
										<img id="proposalChartImage" />
									</a>
								</div>
							</td>
							<td>
								<div runat="server" id="secondaryProposalChartContainer">
									<SAHL:SAHLLabel runat="server" ID="lblSecondaryProposalGraph"></SAHL:SAHLLabel>
									<br />
									<div style="display: none">
										<div id="secondaryProposalChartData" style="width: 900px; height: 600px;">
											<div id="secondaryProposalChartPanel" style="border: 1px solid black; background-color: white; position: absolute; height: 40px;">
											</div>
											<sahlControls:ProposalGraph ID="secondaryProposalGraph" Width="880" Height="560"
												runat="server">
											</sahlControls:ProposalGraph>
										</div>
									</div>
									<a id="inlineSecondaryProposalGraph" href="#secondaryProposalChartData">
										<img id="secondaryProposalChartImage" />
									</a>
								</div>
							</td>
						</tr>
					</table>
					<table id="tblButton" runat="server" class="tableStandard" width="100%">
						<tr>
							<td align="right">
								<SAHL:SAHLButton ID="btnAdd" runat="server" Visible="true" Text="Add" OnClick="btnAdd_Click"
									CausesValidation="False" />
								<SAHL:SAHLButton ID="btnSave" runat="server" Visible="true" Text="Save" OnClick="btnSave_Click"
									CausesValidation="false" />
								<SAHL:SAHLButton ID="btnRemove" runat="server" Visible="true" Text="Remove" OnClick="btnRemove_Click"
									CausesValidation="False" />
								<SAHL:SAHLButton ID="btnCancel" runat="server" Visible="true" Text="Done" OnClick="btnCancel_Click"
									CausesValidation="False" />
								<SAHL:SAHLTextBox ID="txtSaveKey" runat="server" Style="display: none" />
								<SAHL:SAHLTextBox ID="txtReadOnly" runat="server" Style="display: none" />
							</td>
						</tr>
					</table>
				</div>
			</ContentTemplate>
		</ajaxToolkit:TabPanel>
		<ajaxToolkit:TabPanel runat="server" ID="AmortisationSchedule" HeaderText="Amortisation Schedule">
			<ContentTemplate>
				<div style="height: 380px; overflow: scroll;">
					<sahlControls:AmortisationSchedule runat="server" ID="pnlAmortisationSchedule"></sahlControls:AmortisationSchedule>
				</div>
				<table id="Table1" runat="server" class="tableStandard" width="100%">
					<tr>
						<td align="right">
							<SAHL:SAHLButton ID="btnViewAmortisationSchedule" runat="server" OnClick="OnSaveToPDFClick" Text="Save to PDF" CausesValidation="False" Visible="true" OnClientClick="masterCancelBeforeUnload()" />
						</td>
					</tr>
				</table>
			</ContentTemplate>
		</ajaxToolkit:TabPanel>
	</ajaxToolkit:TabContainer>
</asp:Content>
