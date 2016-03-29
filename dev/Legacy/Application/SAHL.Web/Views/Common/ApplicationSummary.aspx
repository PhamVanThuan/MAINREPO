<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="ApplicationSummary.aspx.cs" Inherits="SAHL.Web.Views.Common.ApplicationSummary"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <SAHL:SAHLGridView ID="gridSummary" runat="server" GridHeight="90px" Width="150%"
        GridWidth="100%" AutoGenerateColumns="false" FixedHeader="False" OnSelectedIndexChanged="gridSummary_SelectedIndexChanged"
        Height="44px" HorizontalAlign="Left" ScrollX="True">
        <RowStyle CssClass="TableRowA" />
        <HeaderStyle Wrap="False" />
        <EditRowStyle Wrap="True" />
    </SAHL:SAHLGridView>
    <table class="tableStandard" width="100%">
        <tr>
            <td colspan="2">
                <ajaxToolkit:Accordion ID="accApplicationSummary" runat="server" SelectedIndex="-1"
                    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                    ContentCssClass="" FadeTransitions="false" FramesPerSecond="40" TransitionDuration="250"
                    AutoSize="None" SuppressHeaderPostbacks="true">
                    <Panes>
                        <ajaxToolkit:AccordionPane ID="apApplicationDetails" runat="server">
                            <Header>
                                <a href="">Application Details</a>
                            </Header>
                            <Content>
                                <br />
                                <table runat="server" style="width: 100%" id="Table1">
                                    <tr>
                                        <td class="titleText" style="width: 278px">
                                            Application Number</td>
                                        <td class="cellDisplay" style="width: 218px">
                                            <SAHL:SAHLLabel ID="lblApplicationNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText" style="width: 219px">
                                            LTV</td>
                                        <td class="cellDisplay" style="width: 218px">
                                            <SAHL:SAHLLabel ID="lblLTV" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Applicant Type</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblApplicantType" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            PTI</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblPTI" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Application Type</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblApplicationType" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            SPV</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblSPV" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Employment Type</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblEmploymentType" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Occupancy Type</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblOccupancyType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Household Income</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblHouseholdIncome" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Current Branch Consultant</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblCurrentConsultant" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Purchase Price/Existing Loan</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblPurchasePrice" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Originating Branch Consultant</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblOriginatingConsultant" runat="server" CssClass="LabelText"
                                                TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Cash Deposit/Cash Out</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblCashDeposit" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Branch</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblBranch" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Total Fees</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblFeesTotal" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            New Business Processor</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblNewBusinessProcessor" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Loan Amount</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblLoanAmount" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Associated Branch Admin</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblAssocBranchAdmin" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Valuation Amount</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblValuationAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Conveyancing Attorney</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblConveyancingAttorney" runat="server" CssClass="LabelText"
                                                TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Link Rate</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblLinkRate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Transferring Attorney</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblTransferringAttorney" runat="server" CssClass="LabelText"
                                                TextAlign="Left">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Discount</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblDiscount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Current Commissionable Consultant
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblCurrentCommissionableConsultant" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Discounted Link Rate
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblDiscountedRate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
										<td class="titleText">
                                            <SAHL:SAHLLabel ID="lblRateAdjustment" runat="server" Font-Bold="true" CssClass="LabelText">Rate Adjustment</SAHL:SAHLLabel>
                                        </td>
                                        <td class="titleText">
                                            <SAHL:SAHLLabel ID="lblRateAdjustmentValue" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>                                   
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Pricing Adjustment</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblPricingAdjustment" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                       	<td class="titleText">
                                            <SAHL:SAHLLabel ID="lblCapitecBranchLabel" runat="server" Font-Bold="true" CssClass="LabelText">Capitec Branch</SAHL:SAHLLabel>
                                            <SAHL:SAHLLabel ID="lblComcorpVendorLabel" runat="server" Font-Bold="true" CssClass="LabelText">Comcorp Vendor</SAHL:SAHLLabel>
                                        </td>
                                        <td class="titleText">
                                            <SAHL:SAHLLabel ID="lblCapitecBranch" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                            <SAHL:SAHLLabel ID="lblComcorpVendor" runat="server" CssClass="LabelText"></SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Agency</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblAgency" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                       	<td class="titleText">
                                            <SAHL:SAHLLabel ID="lblCapitecConsultantLabel" runat="server" Font-Bold="true" CssClass="LabelText">Capitec Consultant</SAHL:SAHLLabel>
                                        </td>
                                        <td class="titleText">
                                            <SAHL:SAHLLabel ID="lblCapitecConsultant" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">Quick Pay Loan</td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblQuickPayLoan" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                             <SAHL:SAHLLabel ID="lblIsGEPFLabel" runat="server" Font-Bold="true" CssClass="LabelText">GEPF Funded</SAHL:SAHLLabel>
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblIsGEPF" runat="server" CssClass="LabelText">Yes</SAHL:SAHLLabel></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            <SAHL:SAHLLabel ID="lblStopOrderDiscountEligibilityLabel" runat="server" Font-Bold="true" CssClass="LabelText">Eligible for Stop Order Discount</SAHL:SAHLLabel>
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblStopOrderDiscountEligibility" runat="server" CssClass="LabelText">Yes</SAHL:SAHLLabel></td>
                                        <td></td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apCredit" runat="server" Visible="false">
                            <Header>
                                <a href="">Credit Details</a>
                            </Header>
                            <Content>
                                <br />
                                <table class="tableStandard">
                                    <tr>
                                        <td class="titleText" style="width: 278px">
                                            Credit Decision
                                        </td>
                                        <td class="cellDisplay" style="width: 218px">
                                            <SAHL:SAHLLabel ID="lblCreditDecision" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText" style="width: 230px">
                                        </td>
                                        <td class="cellDisplay" style="width: 200px">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Credit User
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblCreditUser" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                        </td>
                                        <td class="cellDisplay">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trComment" visible="false">
                                        <td class="titleText" colspan="6">
                                            Comments (hover over description to see more detail)
                                            <cc1:MemoGrid ID="gridCreditComments" runat="server">
                                            </cc1:MemoGrid>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trDeclineReason" visible="false">
                                        <td class="titleText" colspan="6">
                                            Decline Reasons
                                            <cc1:ReasonsGrid ID="gridCreditDeclineReasons" runat="server">
                                            </cc1:ReasonsGrid>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apNTU" runat="server" Visible="false">
                            <Header>
                                <a href="">NTU Details</a>
                            </Header>
                            <Content>
                                <table class="tableStandard" width="100%">
                                    <tr>
                                        <td>
                                            NTU Reasons
                                            <cc1:ReasonsGrid ID="gridNTUReasons" runat="server">
                                            </cc1:ReasonsGrid>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apAdminDecline" runat="server" Visible="false">
                            <Header>
                                <a href="">Decline Details</a>
                            </Header>
                            <Content>
                                <table class="tableStandard" width="100%">
                                    <tr>
                                        <td>
                                            Decline Reasons
                                            <cc1:ReasonsGrid ID="grdAdminDeclineReasons" runat="server">
                                            </cc1:ReasonsGrid>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apQuickCash" runat="server" Visible="false">
                            <Header>
                                <a href="">Quick Cash Application Details</a>
                            </Header>
                            <Content>
                                <br />
                                <SAHL:SAHLLabel ID="lblNoQuickCash" runat="server" CssClass="LabelText" Visible="False">No Quick Cash has been approved.<br /></SAHL:SAHLLabel>
                                <table id="tblQuickCash" runat="server" style="width: 100%">
                                    <tr>
                                        <td class="titleText" style="width: 278px">
                                            Credit Approved Amount
                                        </td>
                                        <td class="cellDisplay" style="width: 218px">
                                            <SAHL:SAHLLabel ID="lblCreditApprovedAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText" style="width: 230px">
                                        </td>
                                        <td class="cellDisplay" style="width: 200px">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Credit Upfront Approved Amount
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblUpfrontCreditApprovedAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                        </td>
                                        <td class="cellDisplay">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trQCDecline" visible="false">
                                        <td class="titleText" colspan="6">
                                            Quick Cash has been declined with the following reasons:
                                            <cc1:ReasonsGrid ID="gridQCDeclineReasons" runat="server">
                                            </cc1:ReasonsGrid>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apHOC" runat="server" Visible="false">
                            <Header>
                                <a href="">HOC Policy Details</a>
                            </Header>
                            <Content>
                                <br />
                                <SAHL:SAHLLabel ID="lblNoHOC" runat="server" CssClass="LabelText" Visible="False">No related HOC Policy.<br /></SAHL:SAHLLabel>
                                <table id="tblHOC" runat="server" style="width: 100%">
                                    <tr>
                                        <td class="titleText" style="width: 278px">
                                            HOC Insurer
                                        </td>
                                        <td class="cellDisplay" style="width: 218px">
                                            <SAHL:SAHLLabel ID="lblHOCInsurer" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText" style="width: 219px">
                                            HOC Subsidence
                                        </td>
                                        <td class="cellDisplay" style="width: 218px">
                                            <SAHL:SAHLLabel ID="lblHOCSubsidence" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            HOC Policy Number
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblHOCPolicyNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            HOC Construction
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblHOCConstruction" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            HOC Monthly Premium
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblHOCMonthlyPremium" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Ceded
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblHOCCeded" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            HOC Roof Type
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblHOCRoofType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                        </td>
                                        <td class="cellDisplay">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apLife" runat="server" Visible="false">
                            <Header>
                                <a href="">Life Policy Details</a>
                            </Header>
                            <Content>
                                <br />
                                <SAHL:SAHLLabel ID="lblNoLifePolicy" runat="server" CssClass="LabelText" Visible="False">No related Life Policy.<br /></SAHL:SAHLLabel>
                                <table id="tblLifePolicy" runat="server" style="width: 100%">
                                    <tr>
                                        <td class="titleText" style="width: 278px">
                                            Policy Number
                                        </td>
                                        <td class="cellDisplay" style="width: 218px">
                                            <SAHL:SAHLLabel ID="lblLifePolicyNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText" style="width: 219px">
                                            Date Last Updated
                                        </td>
                                        <td class="cellDisplay" style="width: 218px">
                                            <SAHL:SAHLLabel ID="lblLifeDateLastUpdated" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Monthly Premium
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblLifeMonthlyPremium" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Consultant
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblLifeConsultant" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Yearly Premium
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblLifeYearlyPremium" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Insurer
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblLifeInsurer" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Sum Assured
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblLifeSumAssured" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Broker
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblBroker" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                        </td>
                                        <td class="cellDisplay">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apEstateAgent" runat="server" Visible="false">
                            <Header>
                                <a href="">Estate Agent Details</a>
                            </Header>
                            <Content>
                                <br />
                                <SAHL:SAHLLabel ID="lblNoEstateAgent" runat="server" CssClass="LabelText" Visible="False">No Estate Agent Details.<br /></SAHL:SAHLLabel>
                                <table id="tblEstateDetails" runat="server" style="width: 100%">
                                    <tr>
                                        <td class="titleText" style="width: 278px">
                                            Agency Name
                                        </td>
                                        <td class="cellDisplay" style="width: 218px">
                                            <SAHL:SAHLLabel ID="lblAgencyName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Branch Name
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblBranchName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Principal Name
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblPrincipalName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Principal Phone
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblPrincipalPhone" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Agents Name
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblAgentsName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            Agents Phone
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblAgentsPhone" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                        </td>
                                        <td class="cellDisplay">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                    </Panes>
                </ajaxToolkit:Accordion>
            </td>
        </tr>
        <tr>
            <td align="left">
                <div class="cell" id="lblTip" runat="server" style="padding: 2px;">
                    <strong>Tip:</strong> Click the blue arrow above to expand/collapse more information.</div>
            </td>
            <td align="right">
                <SAHL:SAHLButton ID="btnTransitionHistory" runat="server" Text="History" OnClick="btnTransitionHistory_Click" />&nbsp;
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
