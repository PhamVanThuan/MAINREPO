<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="Proposal.aspx.cs" Inherits="SAHL.Web.Views.Migrate.Proposal" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        // **** Date Edit **** //
        function setItemEndPeriod() {
            //get the periods between the dates
            var periods = monthDiff(getJSDateFromString($("#<%=dateStartDate.ClientID %>").val()), getJSDateFromString($("#<%=dateEndDate.ClientID %>").val()));
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

            var d = getDate(getJSDateFromString($("#<%= dateStartDate.ClientID %>").val()), Number($("#<%= txtEndPeriod.ClientID %>").val()) - Number($("#<%= txtStartPeriod.ClientID %>").val()) + 1);
            //For the SAHL date control, each parameter needs to be the correct length
            var dd = d.getDate();
            if (dd < 10)
                dd = "0" + dd;
            //JScript Month is a 0 bound array, so + 1
            var mm = Number(d.getMonth() + 1);
            if (mm < 10)
                mm = "0" + mm;

            if (!isNaN(dd) && !isNaN(mm) && !isNaN(d.getFullYear()))
                $("#<%= dateEndDate.ClientID %>").val(dd + '/' + mm + '/' + d.getFullYear());
        }
        // **** Period Edit **** //
    </script>

    <table class="tableStandard">
        <tr>
            <td>
                <SAHL:SAHLLabel runat="server" ID="lblAccountNumber" Font-Bold="true" Text="Account Number : "></SAHL:SAHLLabel>
                <SAHL:SAHLLabel ID="lblAccountKey" runat="server" />
            </td>
        </tr>
    </table>
    <div class="TableHeaderA">Proposal</div>
    <br />
    <table class="tableStandard">
        <thead>
            <tr>
                <td style="width: 150px;">
                </td>
                <td style="width: 150px;">
                </td>
                <td style="width: 150px;">
                </td>
                <td style="width: 150px;">
                </td>
            </tr>
        </thead>
        <tr>
            <td>
                <SAHL:SAHLLabel runat="server" ID="lblProposalDate" Text="Proposal Date"></SAHL:SAHLLabel>
            </td>
            <td>
                <SAHL:SAHLLabel runat="server" Text="Proposal Status"></SAHL:SAHLLabel>
            </td>
            <td>
                <SAHL:SAHLLabel runat="server" Text="Accepted Proposal"></SAHL:SAHLLabel>
            </td>
            <td>
                <SAHL:SAHLLabel runat="server" Text="Review Date"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLDateBox runat="server" ID="dateProposalDate" CssClass="mandatory "></SAHL:SAHLDateBox>
            </td>
            <td>
                <SAHL:SAHLDropDownList runat="server" ID="cmbProposalStatus" CssClass="mandatory ">
                </SAHL:SAHLDropDownList>
            </td>
            <td>
                <SAHL:SAHLDropDownList runat="server" ID="cmbAcceptedProposal" CssClass="mandatory ">
                </SAHL:SAHLDropDownList>
            </td>
            <td>
                <SAHL:SAHLDateBox runat="server" ID="dateReview" />
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLLabel runat="server" Text="HOC Inclusive"></SAHL:SAHLLabel>
            </td>
            <td>
                <SAHL:SAHLLabel runat="server" Text="Life Inclusive"></SAHL:SAHLLabel>
            </td>
            <td>
                <SAHL:SAHLLabel runat="server" Text="Approval Date"></SAHL:SAHLLabel>
            </td>
            <td>
                <SAHL:SAHLLabel runat="server" Text="Approval User"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLDropDownList runat="server" ID="cmbHOCInclusive" CssClass="mandatory" OnSelectedIndexChanged="OnHOCOrLifeChanged"
                    AutoPostBack="true">
                </SAHL:SAHLDropDownList>
            </td>
            <td>
                <SAHL:SAHLDropDownList runat="server" ID="cmbLifeInclusive" CssClass="mandatory"
                    OnSelectedIndexChanged="OnHOCOrLifeChanged" AutoPostBack="true">
                </SAHL:SAHLDropDownList>
            </td>
            <td>
                <SAHL:SAHLDateBox runat="server" ID="dateApprovalDate" />
            </td>
            <td>
                <SAHL:SAHLDropDownList runat="server" ID="cmbApprovalUsers">
                </SAHL:SAHLDropDownList>
            </td>
        </tr>
    </table>
    <div runat="server" style="width:100%">
        <SAHL:DXGridView ID="gridProposalItems" runat="server" AutoGenerateColumns="False"
            Settings-VerticalScrollableHeight="155" Width="100%" PostBackType="NoneWithClientSelect"
            ClientInstanceName="grid" Settings-ShowTitlePanel="true" SettingsText-Title="Proposal Details"
            Settings-ShowGroupPanel="false" Settings-ShowVerticalScrollBar="true" Settings-ShowHorizontalScrollBar="true">
        </SAHL:DXGridView>
    </div>
    <div class="TableHeaderA">Proposal Items</div>
    <table class="tableStandard" width="98%">
        <tr>
            <td style="width: 140px">
                <SAHL:SAHLPanel runat="server" ID="pnlDates" GroupingText="Dates">
                    <table class="tableStandard">
                        <tr>
                            <td class="LabelText">
                                Start
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                <SAHL:SAHLDateBox runat="server" ID="dateStartDate" CssClass="mandatory" onblur="setItemEndPeriod(); setItemEndDate()" onchange="setItemEndPeriod(); setItemEndDate()" />
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                End
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                <SAHL:SAHLDateBox runat="server" ID="dateEndDate" CssClass="mandatory" onblur="setItemEndPeriod()" onchange="setItemEndPeriod()" />
                            </td>
                        </tr>
                    </table>
                </SAHL:SAHLPanel>
            </td>
            <td style="width: 80px">
                <SAHL:SAHLPanel runat="server" ID="pnlPeriod" GroupingText="Periods">
                    <table class="tableStandard">
                        <tr>
                            <td class="LabelText">
                                Start
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                <SAHL:SAHLTextBox runat="server" ID="txtStartPeriod" CssClass="mandatory" DisplayInputType="Number" enabled="false" onblur="setItemEndPeriod(); setItemEndDate()"
                                    MaxLength="3" Width="30" />
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                End
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                <SAHL:SAHLTextBox runat="server" ID="txtEndPeriod" CssClass="mandatory" DisplayInputType="Number"
                                    MaxLength="3" Width="30" onblur="setItemEndDate()" />
                            </td>
                        </tr>
                    </table>
                </SAHL:SAHLPanel>
            </td>
            <td>
                <SAHL:SAHLPanel runat="server" ID="pnlDetail" GroupingText="Detail">
                    <table class="tableStandard">
                        <tr>
                            <td class="LabelText" style="width: 150px">
                                Market Rate
                            </td>
                            <td class="LabelText" style="width: 150px">
                                Amount
                            </td>
                            <td class="LabelText">
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                <SAHL:SAHLDropDownList runat="server" ID="cmbMarketRate" />
                            </td>
                            <td class="LabelText">
                                <SAHL:SAHLCurrencyBox runat="server" ID="txtAmount" CssClass="mandatory" />
                            </td>
                            <td class="LabelText">
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                Interest Rate %
                            </td>
                            <td class="LabelText">
                                Additional Amount
                            </td>
                            <td class="LabelText">
                                Annual Escalation %
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                <SAHL:SAHLCurrencyBox runat="server" ID="txtInterestRate" CssClass="mandatory" />
                            </td>
                            <td class="LabelText">
                                <SAHL:SAHLCurrencyBox runat="server" ID="txtAdditionalAmount" />
                            </td>
                            <td class="LabelText">
                                <SAHL:SAHLCurrencyBox runat="server" ID="txtAnnualEscalation" CssClass="mandatory"
                                    Text="0" />
                            </td>
                        </tr>
                    </table>
                </SAHL:SAHLPanel>
            </td>
        </tr>
    </table>
    <div id="divButtons" style="text-align: right;">
        <SAHL:SAHLButton runat="server" ID="btnBack" Text="<--" OnClick="OnBackClick" />
        <SAHL:SAHLButton runat="server" ID="btnAdd" Text="Add Proposal Item" OnClick="OnAddProposalItemClick" />
        <SAHL:SAHLButton runat="server" ID="btnRemove" Visible="true" Text="Remove Proposal Item"
            OnClick="OnRemoveProposalItemClick" CausesValidation="False" />
        <SAHL:SAHLButton runat="server" ID="btnFinish" Text="Finish" OnClick="OnFinishClick" />
    </div>
</asp:Content>
