<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.ApplicationHistory" Codebehind="ApplicationHistory.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">
// <!CDATA[

function TABLE1_onclick() {

}

// ]]>
</script>

    <SAHL:SAHLGridView ID="gridHistory" runat="server" AutoGenerateColumns="False"
            FixedHeader="False" HeaderCaption="Application Revision History" Invisible="False"
            SelectFirstRow="True" TotalsFooter="True" Width="100%" EmptyDataText="There are no offers for this offer status." GridHeight="100px" GridWidth="100%" OnSelectedIndexChanged="gridHistory_SelectedIndexChanged">
        </SAHL:SAHLGridView>
    <table style="width: 801px; height: 240px;" id="TABLE1" onclick="return TABLE1_onclick()">
        <tr>
            <td valign="top" style="height: 95px; width: 356px;">
                <asp:Panel ID="panelOfferInformationNew" runat="server" GroupingText="Offer Information"
                    Height="372px" Width="410px" HorizontalAlign="Left" Visible="False">
                    <table style="width: 360px; height: 97px;">
                    <tr>
                        <td style="width: 172px">
                        </td>
                        <td style="width: 117px">
                                <SAHL:SAHLLabel ID="lblCurrent" runat="server" CssClass="LabelText">Current</SAHL:SAHLLabel></td>
                        <td style="width: 47px">
                                <SAHL:SAHLLabel ID="lblInitial" runat="server" CssClass="LabelText">Initial</SAHL:SAHLLabel></td>
                    </tr>
                    <tr id="rowPurchasePrice" runat="server" visible="false">
                        <td style="width: 172px; height: 13px;">
                                <SAHL:SAHLLabel ID="lblPurchasePrice" runat="server" CssClass="LabelText" TextAlign="Left" Visible="False">Effective Purchase Price</SAHL:SAHLLabel></td>
                        <td style="width: 117px; height: 13px;">
                            <SAHL:SAHLTextBox ID="txtPurchasePriceCurrent" runat="server" Width="97px" Visible="False" BorderStyle="None"></SAHL:SAHLTextBox>
                        </td>
                        <td style="height: 13px; width: 47px;">
                            <SAHL:SAHLTextBox ID="txtPurchasePriceInitial" runat="server" Width="97px" Visible="False" BorderStyle="None"></SAHL:SAHLTextBox></td>
                    </tr>
                    <tr id="rowCashDeposit" runat="server" visible="false">
                        <td style="height: 7px; width: 172px;">
                                <SAHL:SAHLLabel ID="lblCashDeposit" runat="server" CssClass="LabelText" Visible="False">Cash Deposit</SAHL:SAHLLabel></td>
                        <td style="height: 7px; width: 117px;">
                            <SAHL:SAHLTextBox ID="txtCashDepositCurrent" runat="server" Width="97px" Visible="False" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        <td style="height: 7px; width: 47px;">
                            <SAHL:SAHLTextBox ID="txtCashDepositInitial" runat="server" Width="97px" Visible="False" BorderStyle="None"></SAHL:SAHLTextBox></td>
                    </tr>
                        <tr id="rowExistingLoan" runat="server" visible="false">
                            <td style="width: 172px; height: 8px;">
                                <SAHL:SAHLLabel ID="lblExistingLoan" runat="server" CssClass="LabelText" TextAlign="Left" Visible="False">Existing Loan</SAHL:SAHLLabel></td>
                            <td style="width: 117px; height: 8px;">
                                <SAHL:SAHLTextBox ID="txtExistingLoanCurrent" runat="server" Width="97px" Visible="False" BorderStyle="None"></SAHL:SAHLTextBox></td>
                            <td style="height: 8px; width: 47px;">
                                <SAHL:SAHLTextBox ID="txtExistingLoanInitial" runat="server" Width="97px" Visible="False" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowCashOut" runat="server" visible="false">
                            <td style="width: 172px; height: 1px;">
                                <SAHL:SAHLLabel ID="lblCashOut" runat="server" CssClass="LabelText" Visible="False">Cash Out</SAHL:SAHLLabel></td>
                            <td style="width: 117px; height: 1px;">
                                <SAHL:SAHLTextBox ID="txtCashOutCurrent" runat="server" Width="97px" Visible="False" BorderStyle="None"></SAHL:SAHLTextBox></td>
                            <td style="height: 1px; width: 47px;">
                                <SAHL:SAHLTextBox ID="txtCashOutInitial" runat="server" Width="97px" Visible="False" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowInterimInterest" visible="false" runat="server">
                            <td style="width: 172px;">
                                <SAHL:SAHLLabel ID="lblInterimInterest" runat="server" CssClass="LabelText"
                                    Width="163px" Visible="False">Interim Interest</SAHL:SAHLLabel></td>
                            <td style="width: 117px;">
                                <SAHL:SAHLTextBox ID="txtInterimInterestCurrent" runat="server" Width="97px" Visible="False" BorderStyle="None"></SAHL:SAHLTextBox></td>
                            <td style="width: 47px">
                                <SAHL:SAHLTextBox ID="txtInterimInterestInitial" runat="server" Width="97px" Visible="False" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowFees" runat ="server" visible="false">
                            <td style="width: 172px; height: 2px">
                                <SAHL:SAHLLabel ID="lblFees" runat="server" CssClass="LabelText" TextAlign="Left">Fees</SAHL:SAHLLabel></td>
                            <td style="width: 117px; height: 2px">
                                <SAHL:SAHLTextBox ID="txtFeesCurrent" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                            <td style="height: 2px; width: 47px;">
                                <SAHL:SAHLTextBox ID="txtFeesInitial" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        </tr>
                    <tr>
                        <td style="width: 172px">
                                <SAHL:SAHLLabel ID="lblTotalLoanRequired" runat="server" CssClass="LabelText"
                                    Width="163px" TextAlign="Left">Loan Agreement Amount</SAHL:SAHLLabel></td>
                        <td style="width: 117px">
                            <SAHL:SAHLTextBox ID="txtTotalLoanRequiredCurrent" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        <td style="width: 47px">
                            <SAHL:SAHLTextBox ID="txtTotalLoanRequiredInitial" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 172px; height: 2px;">
                                <SAHL:SAHLLabel ID="lblTerm" runat="server" CssClass="LabelText" TextAlign="Left">Term</SAHL:SAHLLabel></td>
                        <td style="width: 117px; height: 2px;">
                            <SAHL:SAHLTextBox ID="txtTermCurrent" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        <td style="height: 2px; width: 47px;">
                            <SAHL:SAHLTextBox ID="txtTermInitial" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                    </tr>
                        <tr>
                            <td style="width: 172px; height: 2px">
                                <SAHL:SAHLLabel ID="lblLinkRate" runat="server" CssClass="LabelText" TextAlign="Left">Link Rate</SAHL:SAHLLabel></td>
                            <td style="width: 117px; height: 2px">
                            <SAHL:SAHLTextBox ID="txtLinkRateCurrent" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                            <td style="width: 47px; height: 2px">
                            <SAHL:SAHLTextBox ID="txtLinkRateInitial" runat="server" Width="97px" BorderStyle="None" OnTextChanged="txtLinkRateInitial_TextChanged"></SAHL:SAHLTextBox></td>
                        </tr>
                    <tr>
                        <td style="width: 172px; height: 26px;">
                            <SAHL:SAHLLabel ID="lblDiscountedLinkRate" runat="server" CssClass="LabelText" Width="155px">Discounted Link Rate</SAHL:SAHLLabel></td>
                        <td style="width: 117px; height: 26px;">
                            <SAHL:SAHLTextBox ID="txtDiscountedLinkRateCurrent" runat="server" BorderStyle="None"
                                Width="97px"></SAHL:SAHLTextBox></td>
                        <td style="width: 47px; height: 26px;">
                            <SAHL:SAHLTextBox ID="txtDiscountedLinkRateInitial" runat="server" BorderStyle="None"
                                Width="97px"></SAHL:SAHLTextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 172px;">
                                <SAHL:SAHLLabel ID="lblEffectiveRate" runat="server" CssClass="LabelText" TextAlign="Left">Effective Rate</SAHL:SAHLLabel></td>
                        <td style="width: 117px;">
                                <SAHL:SAHLTextBox ID="txtEffectiveRateCurrentNew" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        <td style="width: 47px">
                                <SAHL:SAHLTextBox ID="txtEffectiveRateInitial" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 172px">
                                <SAHL:SAHLLabel ID="lblBondToRegister" runat="server" CssClass="LabelText" TextAlign="Left">Bond to Register</SAHL:SAHLLabel></td>
                        <td style="width: 117px">
                                <SAHL:SAHLTextBox ID="txtBondToRegisterCurrent" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        <td style="width: 47px">
                                <SAHL:SAHLTextBox ID="txtBondToRegisterInitial" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 172px;">
                                <SAHL:SAHLLabel ID="lblEstPropertyValue" runat="server" CssClass="LabelText" TextAlign="Left">Property Value</SAHL:SAHLLabel></td>
                        <td style="width: 117px;">
                                <SAHL:SAHLTextBox ID="txtEstPropertyValueCurrent" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        <td style="width: 47px">
                                <SAHL:SAHLTextBox ID="txtEstPropertyValueInitial" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 172px">
                                <SAHL:SAHLLabel ID="lblHouseholdIncome" runat="server" CssClass="LabelText" TextAlign="Left">Household Income</SAHL:SAHLLabel></td>
                        <td style="width: 117px">
                                <SAHL:SAHLTextBox ID="txtHouseholdIncomeCurrent" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        <td style="width: 47px">
                                <SAHL:SAHLTextBox ID="txtHouseholdIncomeInitial" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 172px;">
                                <SAHL:SAHLLabel ID="lblCategory" runat="server" CssClass="LabelText" TextAlign="Left">Category</SAHL:SAHLLabel></td>
                        <td style="width: 117px;">
                                <SAHL:SAHLTextBox ID="txtCategoryCurrent" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                        <td style="width: 47px">
                                <SAHL:SAHLTextBox ID="txtCategoryInitial" runat="server" Width="97px" BorderStyle="None"></SAHL:SAHLTextBox></td>
                    </tr>
                    <tr>
                       <td style="width: 172px">
                            <SAHL:SAHLLabel ID="lblSPVName" runat="server" CssClass="LabelText" TextAlign="Left">SPV Name</SAHL:SAHLLabel></td>
                    </tr>
                    <tr>
                       <td style="width: 172px" />
                       <td style="width: 117px" valign="bottom">
                            <SAHL:SAHLTextBox ID="txtSPVNameCurrent" runat="server" Style="overflow:hidden" Width="97px" BorderStyle="None" Height="44px" Rows="5" TextMode="MultiLine"></SAHL:SAHLTextBox></td>
                        <td valign="bottom" style="width: 47px">
                            <SAHL:SAHLTextBox ID="txtSPVNameInitial" runat="server" Style="overflow:hidden" Width="97px" BorderStyle="None" Height="42px" Rows="2" TextMode="MultiLine"></SAHL:SAHLTextBox></td>
                    </tr>                    
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;
            </td>
            <td style="width: 621px; height: 95px" valign="top">
            </td>
            <td style="width: 700px; height: 95px" valign="top" align="right">
                <asp:Panel ID="panelInstalmentDetails" runat="server" GroupingText="Instalment Details"
                    Height="372px" Width="450px" Visible="False">
                    <table style="width: 424px; height: 1px;">
                        <tr>
                            <td style="width: 165px">
                            </td>
                            <td style="width: 94px">
                                <SAHL:SAHLLabel ID="lblInstalmentDetailsCurrent" runat="server" CssClass="LabelText">Current</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblInstalmentDetailsInitial" runat="server" CssClass="LabelText">Initial</SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td style="width: 165px" align="left">
                                &nbsp;<SAHL:SAHLLabel ID="lblLTV" runat="server" CssClass="LabelText" TextAlign="Left">LTV</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtLTVCurrent" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox>&nbsp;</td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtLTVInitial" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="height: 21px; width: 165px;" align="left">
                                &nbsp;<SAHL:SAHLLabel ID="lblPTI" runat="server" CssClass="LabelText" TextAlign="Left">PTI</SAHL:SAHLLabel></td>
                            <td style="height: 21px; width: 94px;">
                                <SAHL:SAHLTextBox ID="txtPTICurrent" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="height: 21px">
                                <SAHL:SAHLTextBox ID="txtPTIInitial" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowMonthlyInstalment" runat="server">
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblMonthlyInstalment" runat="server" CssClass="LabelText" Width="137px">Monthly Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtMonthlyInstalmentCurrent" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtMonthlyInstalmentInitial" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr runat="server" id="rowAmortisingInstalment" visible="false">
                            <td style="width: 165px; height: 0px;" align="left">
                                <SAHL:SAHLLabel ID="lblAmortisingInstalment" runat="server" CssClass="LabelText" TextAlign="Left" Visible="False">Amortising Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px; height: 26px;">
                                <SAHL:SAHLTextBox ID="txtAmortisingInstalmentCurrent" runat="server" BorderStyle="None"
                                    Width="97px" Visible="False"></SAHL:SAHLTextBox></td>
                            <td style="height: 26px">
                                <SAHL:SAHLTextBox ID="txtAmortisingInstalmentInitial" runat="server" BorderStyle="None"
                                    Width="97px" Visible="False"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr runat="server" visible="false" id="rowInterestOnly">
                            <td style="width: 165px; height: 0px;" align="left">
                                <SAHL:SAHLLabel ID="lblInterestOnlyInstalment" runat="server" CssClass="LabelText" Visible="False" Width="180px">Interest Only Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtInterestOnlyInstalmentCurrent" runat="server" BorderStyle="None"
                                    Visible="False" Width="97px"></SAHL:SAHLTextBox></td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtInterestOnlyInstalmentInitial" runat="server" BorderStyle="None"
                                    Visible="False" Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr runat="server" visible="false" id="rowPrepaymentThreshhold">
                            <td style="width: 165px">
                                <SAHL:SAHLLabel ID="lblPrePaymentThreshhold" runat="server" CssClass="LabelText" Width="230px">Prepayment Threshhold/Annum</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtPrepaymentThreshholdCurrent" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtPrepaymentThreshholdInitial" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panelInstalmentDetailsVarifix" runat="server" GroupingText="Instalment Details"
                    Height="372px" Width="430px" Visible="False">
                    <table style="width: 407px; height: 1px;">
                        <tr>
                            <td style="width: 165px">
                            </td>
                            <td style="width: 94px">
                                <SAHL:SAHLLabel ID="lblCurrentVarifix" runat="server" CssClass="LabelText" TextAlign="Left">Current</SAHL:SAHLLabel></td>
                            <td style="width: 116px">
                                <SAHL:SAHLLabel ID="lblInitialVarifix" runat="server" CssClass="LabelText" TextAlign="Left">Initial</SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblPTIVarifix" runat="server" CssClass="LabelText">PTI</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtPTICurrentVarifix" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox>
                            </td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtPTIInitialVarifix" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="height: 21px; width: 165px;" align="left">
                                <SAHL:SAHLLabel ID="lblLTVVarifix" runat="server" CssClass="LabelText">LTV</SAHL:SAHLLabel></td>
                            <td style="height: 21px; width: 94px;">
                                <SAHL:SAHLTextBox ID="txtLTVCurrentVarifix" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="height: 21px; width: 116px;">
                                <SAHL:SAHLTextBox ID="txtLTVInitialVarifix" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblFixedInstalmentVarifix" runat="server" CssClass="LabelText"
                                    TextAlign="Left">Fixed Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtFixedInstalmentCurrentVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtFixedInstalmentInitialVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 165px; height: 26px;" align="left">
                                <SAHL:SAHLLabel ID="lblVariableInstalmentVarifix" runat="server" CssClass="LabelText"
                                    Width="139px">Variable Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px; height: 26px;">
                                <SAHL:SAHLTextBox ID="txtVariableInstalmentCurrentVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="height: 26px; width: 116px;">
                                <SAHL:SAHLTextBox ID="txtVariableInstalmentInitialVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowMonthlyInstalmentVarifix" runat="server" visible="false">
                            <td style="width: 165px; height: 26px" align="left">
                                <SAHL:SAHLLabel ID="lblMonthlyInstalmentVarifix" runat="server" CssClass="LabelText" Width="139px" Visible="False">Monthly Instalment</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 94px; height: 26px">
                                <SAHL:SAHLTextBox ID="txtMonthlyInstalmentCurrentVarifix" runat="server" BorderStyle="None"
                                    Width="97px" Visible="False"></SAHL:SAHLTextBox></td>
                            <td style="height: 26px; width: 116px;">
                                <SAHL:SAHLTextBox ID="txtMonthlyInstalmentInitialVarifix" runat="server" BorderStyle="None"
                                    Width="97px" Visible="False"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowInterestOnlyInstalmentVarifix" visible="false" runat="server">
                            <td style="width: 165px; height: 26px" align="left">
                                <SAHL:SAHLLabel ID="lblInterestOnlyInstalmentVarifix" runat="server" CssClass="LabelText"
                                    Width="184px" Visible="False">Interest Only Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px; height: 26px">
                                <SAHL:SAHLTextBox ID="txtInterestOnlyInstalmentCurrentVarifix" runat="server" BorderStyle="None"
                                    Width="97px" Visible="False"></SAHL:SAHLTextBox></td>
                            <td style="height: 26px; width: 116px;">
                                <SAHL:SAHLTextBox ID="txtInterestOnlyInstalmentInitialVarifix" runat="server" BorderStyle="None"
                                    Width="97px" Visible="False"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowTotalInstalmentVarifix" visible="false" runat="server">
                            <td style="width: 165px; height: 26px;" align="left">
                                <SAHL:SAHLLabel ID="lblTotalInstalmentVarifix" runat="server" CssClass="LabelText">Total Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px; height: 26px;">
                                <SAHL:SAHLTextBox ID="txtTotalInstalmentCurrentVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="height: 26px; width: 116px;">
                                <SAHL:SAHLTextBox ID="txtTotalInstalmentInitialVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblVarifixFixedPercentage" runat="server" CssClass="LabelText">VariFix Fixed %</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtVarifixFixedPercentageCurrentVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtVarifixFixedPercentageInitialVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblVarifixVariablePercentage" runat="server" CssClass="LabelText">VariFix Variable %</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtVarifixVariablePercentageCurrentVarifix" runat="server"
                                    BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtVarifixVariablePercentageInitialVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblFixedPortionVarifix" runat="server" CssClass="LabelText">Fixed Portion</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtFixedPortionCurrentVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtFixedPortionInitialVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblVariablePortionVarifix" runat="server" CssClass="LabelText">Variable Portion</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtVariablePortionCurrentVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtVariablePortionInitialVarifix" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblPrepaymentThresholdVarifix" runat="server" CssClass="LabelText"
                                    TextAlign="Left" Width="225px">Prepayment Threshold/Annum</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtPrepaymentThresholdVarifixCurrent" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtPrepaymentThresholdVarifixInitial" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                    </table>
                </asp:Panel>
                
                <asp:Panel ID="panelInstalmentDetailsEdge" runat="server" GroupingText="Instalment Details"
                    Height="372px" Width="450px" Visible="False">
                    <table style="width: 424px; height: 1px;">
                                            <tr>
                            <td style="width: 165px">
                            </td>
                            <td style="width: 94px">
                                <SAHL:SAHLLabel ID="lblInstalmentDetailsCurrentEHL" runat="server" CssClass="LabelText">Current</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblInstalmentDetailsInitialEHL" runat="server" CssClass="LabelText">Initial</SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td style="width: 165px" align="left">
                                &nbsp;<SAHL:SAHLLabel ID="lblLTVEHL" runat="server" CssClass="LabelText" TextAlign="Left">LTV</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtLTVCurrentEHL" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox>&nbsp;</td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtLTVInitialEHL" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="height: 21px; width: 165px;" align="left">
                                &nbsp;<SAHL:SAHLLabel ID="lblPTIEHL" runat="server" CssClass="LabelText" TextAlign="Left">PTI</SAHL:SAHLLabel></td>
                            <td style="height: 21px; width: 94px;">
                                <SAHL:SAHLTextBox ID="txtPTICurrentEHL" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="height: 21px">
                                <SAHL:SAHLTextBox ID="txtPTIInitialEHL" runat="server" BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowVarifixEHLFixedInstalment" visible="false" runat="server">
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText"
                                    TextAlign="Left">Fixed Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtFixedInstalmentCurrentVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtFixedInstalmentInitialVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowVarifixEHLVarInstalment" visible="false" runat="server">
                            <td style="width: 165px; height: 26px;" align="left">
                                <SAHL:SAHLLabel ID="lblVariableInstalmentVarifixEHL" runat="server" CssClass="LabelText"
                                    Width="139px">Variable Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px; height: 26px;">
                                <SAHL:SAHLTextBox ID="txtVariableInstalmentCurrentVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="height: 26px; width: 116px;">
                                <SAHL:SAHLTextBox ID="txtVariableInstalmentInitialVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 165px; height: 0px;" align="left">
                                <SAHL:SAHLLabel ID="lblInterestOnlyInstalmentEHL" runat="server" CssClass="LabelText" TextAlign="Left" >Interest Only Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px; height: 26px;">
                                <SAHL:SAHLTextBox ID="txtInterestOnlyInstalmentCurrentEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="height: 26px">
                                <SAHL:SAHLTextBox ID="txtInterestOnlyInstalmentInitialEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 165px; height: 0px;" align="left">
                                <SAHL:SAHLLabel ID="lblAmortisingInstalmentEHL" runat="server" CssClass="LabelText" TextAlign="Left" >Amortising Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px; height: 26px;">
                                <SAHL:SAHLTextBox ID="txtAmortisingInstalmentCurrentEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="height: 26px">
                                <SAHL:SAHLTextBox ID="txtAmortisingInstalmentInitialEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 165px; height: 0px;" align="left">
                                <SAHL:SAHLLabel ID="lblFullTermEHL" runat="server" CssClass="LabelText" TextAlign="Left"> Full Term Instalment</SAHL:SAHLLabel></td>
                            <td style="width: 94px; height: 26px;">
                                <SAHL:SAHLTextBox ID="txtullTermCurrentEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="height: 26px">
                                <SAHL:SAHLTextBox ID="txtullTermInitialEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowVarifixFixedPercentageEHL" visible="false" runat="server">
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblVarifixFixedPercentageEHL" runat="server" CssClass="LabelText">VariFix Fixed %</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtVarifixFixedPercentageCurrentVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtVarifixFixedPercentageInitialVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowVarifixVariablePercentageEHL" visible="false" runat="server">
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblVarifixVariablePercentageEHL" runat="server" CssClass="LabelText">VariFix Variable %</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtVarifixVariablePercentageCurrentVarifixEHL" runat="server"
                                    BorderStyle="None" Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtVarifixVariablePercentageInitialVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowFixedPortionVarifixEHL" visible="false" runat="server">
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblFixedPortionVarifixEHL" runat="server" CssClass="LabelText">Fixed Portion</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtFixedPortionCurrentVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtFixedPortionInitialVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr id="rowVariablePortionVarifixEHL" visible="false" runat="server">
                            <td style="width: 165px" align="left">
                                <SAHL:SAHLLabel ID="lblVariablePortionVarifixEHL" runat="server" CssClass="LabelText">Variable Portion</SAHL:SAHLLabel></td>
                            <td style="width: 94px">
                                <SAHL:SAHLTextBox ID="txtVariablePortionCurrentVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                            <td style="width: 116px">
                                <SAHL:SAHLTextBox ID="txtVariablePortionInitialVarifixEHL" runat="server" BorderStyle="None"
                                    Width="97px"></SAHL:SAHLTextBox></td>
                        </tr>                                                                                                
                    </table>
                    </asp:Panel>
                &nbsp;
                <SAHL:SAHLButton ID="btnGo" runat="server" Text="Update" OnClick="btnGo_Click" /></td>
        </tr>        
    </table>
</asp:Content>
