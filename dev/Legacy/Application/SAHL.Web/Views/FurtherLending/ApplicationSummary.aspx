<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="ApplicationSummary.aspx.cs" Inherits="SAHL.Web.Views.FurtherLending.ApplicationSummary"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <SAHL:SAHLGridView ID="gridSummary" runat="server" GridHeight="90px" Width="150%"
        GridWidth="100%" AutoGenerateColumns="false" FixedHeader="False"
        Height="44px" HorizontalAlign="Left" ScrollX="True">
        <RowStyle CssClass="TableRowA" />
        <HeaderStyle Wrap="False" />
        <EditRowStyle Wrap="True" />
    </SAHL:SAHLGridView>
    <table class="tableStandard" width="100%">
        <tr>
            <td>
                <ajaxToolkit:Accordion ID="accApplicationSummary" runat="server" SelectedIndex="0"
                    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                    ContentCssClass="" FadeTransitions="false" FramesPerSecond="40" TransitionDuration="250"
                    AutoSize="None" SuppressHeaderPostbacks="true">
                    <Panes>
                        <ajaxToolkit:AccordionPane ID="ApplicationDetail" runat="server">
                            <Header>
                                <a href="">Further Lending Application Details</a>
                            </Header>
                            <Content>
                                <table class="tableStandard">
                                    <tr>
                                        <td>Name:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAccountLegalName" Text="-" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Account Number:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAccountNumber" Text="-" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Product:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAccountProduct" Text="-" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Bond Reg. Date:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbBondRegDate" Text="-" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Total Bond Reg. Amount:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbBondRegAmount" Text="-" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Requested Amounts</td>
                                        <td></td>
                                        <td>Fees Breakdown</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Readvance:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbReadvanceRequired" Text="-" /></td>
                                        <td>Registration Fee:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbRegistrationFee" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Further Advance:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbFurtherAdvanceRequired" Text="-" /></td>
                                        <td>Initiation Fee:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbInititationFee" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Further Loan:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbFurtherLoanRequired" Text="-" /></td>
                                        <td>Total:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbFeeTotal" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Bond to Register:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbBondToRegister" Text="-" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>Conveyancing Attorney:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbConveyancingAttorney" Text="-" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Transferring Attorney:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbTransferringAttorney" Text="-" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Created Date:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbApplicationCreatedDate" Text="-" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>Application Processor:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lblApplicationProcessor" Text="-" /></td>
                                        <td>Application Creator:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbApplicationCreator" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <tr>
                                            <td colspan="4">&nbsp;</td>
                                        </tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblStopOrderDiscountEligibilityLabel" Text="Eligible for Stop Order Discount:" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lblStopOrderDiscountEligibility" Text="Yes" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="CurrentCalculated" runat="server">
                            <Header>
                                <a href="">Current vs Calculated</a>
                            </Header>
                            <Content>
                                <table class="tableStandard">
                                    <tr>
                                        <td></td>
                                        <td runat="server" id="tdCurr1">Current</td>
                                        <td>Calculated</td>
                                        <td></td>
                                        <td 
                                            <asp:Label runat="server" ID="lbl20yrCalculated" Text="20yr Calculated" ForeColor="Red" Visible="false" /></td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>SPV:</td>
                                        <td runat="server" id="tdCurr2">
                                            <asp:Label runat="server" ID="lbSPV" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppSPV" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Remaining Term:</td>
                                        <td runat="server" id="tdCurr3">
                                            <asp:Label runat="server" ID="lbTerm" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppTerm" Text="-" /></td>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbl20yrTerm" Text="-" ForeColor="Red" Visible="false" /></td>
                                    </tr>
                                    <tr runat="server" id="trCurBalF1">
                                        <td>Current Balance</td>
                                        <td runat="server" id="tdCurr4"></td>
                                        <td></td>
                                    </tr>
                                    <tr runat="server" id="trCurBalF2">
                                        <td>Fixed:</td>
                                        <td runat="server" id="tdCurr5">
                                            <asp:Label runat="server" ID="lbFixedBalance" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppFixedBalance" Text="-" /></td>
                                    </tr>
                                    <tr runat="server" id="trCurBalF3">
                                        <td>Variable:</td>
                                        <td runat="server" id="tdCurr6">
                                            <asp:Label runat="server" ID="lbVariableBalance" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppVariableBalance" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lbCurrentBalanceDesc" Text="Current Balance:" /></td>
                                        <td runat="server" id="tdCurr7">
                                            <asp:Label runat="server" ID="lbCurrentBalance" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppCurrentBalance" Text="-" /></td>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbl20yrCurrentBalance" Text="-" ForeColor="Red"  Visible="false"/></td>
                                    </tr>
                                    <tr>
                                        <td>LTV:</td>
                                        <td runat="server" id="tdCurr8">
                                            <asp:Label runat="server" ID="lbLTV" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppLTV" Text="-" /></td>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbl20yrLTV" Text="-" ForeColor="Red"  Visible="false"/></td>
                                    </tr>
                                    <tr>
                                        <td>PTI:</td>
                                        <td runat="server" id="tdCurr9">
                                            <asp:Label runat="server" ID="lbPTI" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppPTI" Text="-" /></td>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbl20yrPTI" Text="-" ForeColor="Red"  Visible="false"/></td>
                                    </tr>
                                    <tr runat="server" id="trInstal1">
                                        <td>Instalments</td>
                                        <td runat="server" id="tdCurr10"></td>
                                        <td></td>
                                    </tr>
                                    <tr runat="server" id="trInstal2">
                                        <td>Fixed:</td>
                                        <td runat="server" id="tdCurr11">
                                            <asp:Label runat="server" ID="lbInstalmentFixed" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppInstalmentFixed" Text="-" /></td>
                                    </tr>
                                    <tr runat="server" id="trInstal3">
                                        <td>Variable:</td>
                                        <td runat="server" id="tdCurr12">
                                            <asp:Label runat="server" ID="lbInstalmentVariable" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppInstalmentVariable" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lbInstalDesc" Text="Instalment:" /></td>
                                        <td runat="server" id="tdCurr13">
                                            <asp:Label runat="server" ID="lbInstalmentTotal" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppInstalmentTotal" Text="-" /></td>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbl20yrInstalment" Text="-" ForeColor="Red"  Visible="false"/></td>
                                    </tr>
                                    <tr runat="server" id="trInstal4" visible="false">
                                        <td>
                                            <asp:Label runat="server" ID="lbAmortiseInstalDesc" Text="Amortising Instalment:" /></td>
                                        <td runat="server" id="tdCurr21">
                                            <asp:Label runat="server" ID="lbInstalmentTotalAM" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppInstalmentTotalAM" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Rates</td>
                                        <td runat="server" id="tdCurr14"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Market Rate:</td>
                                        <td runat="server" id="tdCurr15">
                                            <asp:Label runat="server" ID="lbMarketRate" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppMarketRate" Text="-" /></td>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbl20yrMarketRate" Text="-" ForeColor="Red"  Visible="false"/></td>
                                    </tr>
                                    <tr runat="server" id="trRateF1">
                                        <td>Market Rate (Fixed):</td>
                                        <td runat="server" id="tdCurr16">
                                            <asp:Label runat="server" ID="lbMarketRateFixed" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppMarketRateFixed" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Link Rate:</td>
                                        <td runat="server" id="tdCurr17">
                                            <asp:Label runat="server" ID="lbMargin" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppMargin" Text="-" /></td>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbl20yrLinkRate" Text="-" ForeColor="Red"  Visible="false"/></td>
                                    </tr>
                                    <tr>
                                        <td>Discount:</td>
                                        <td runat="server" id="tdCurr18">
                                            <asp:Label runat="server" ID="lbAccountDiscount" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppDiscount" Text="-" /></td>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbl20yrDiscount" Text="-" ForeColor="Red" /></td>
                                    </tr>
                                    <tr>
                                        <td>Pricing Adjustment:</td>
                                        <td runat="server" id="tdCurr22">
                                            <asp:Label runat="server" ID="lblPricingAdjustment" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lblAppPricingAdjustment" Text="-" /></td>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbl20yrPricingAdjustment" Text="-" ForeColor="Red"  Visible="false"/></td>

                                    </tr>
                                    <tr runat="server" id="trRateF2">
                                        <td>Effective Rate (Fixed):</td>
                                        <td runat="server" id="tdCurr19">
                                            <asp:Label runat="server" ID="lbEffectiveRateFixed" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppEffectiveRateFixed" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Effective Rate (Variable):</td>
                                        <td runat="server" id="tdCurr20">
                                            <asp:Label runat="server" ID="lbEffectiveRateVariable" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAppEffectiveRateVariable" Text="-" /></td>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbl20yrEffectiveRateVariable" Text="-" ForeColor="Red"  Visible="false"/></td>
                                    </tr>
                                </table>
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="ArrearIncomeValuation" runat="server">
                            <Header>
                                <a href="">Arrears, Income and Valuation Information</a>
                            </Header>
                            <Content>
                                <table class="tableStandard">
                                    <tr>
                                        <td style="width: 25%">Arrears Balance:</td>
                                        <td style="width: 25%">
                                            <asp:Label runat="server" ID="lbArrearsBalance" Text="-" /></td>
                                        <td style="width: 25%">
                                            <asp:Label runat="server" Visible="false" ID="lbLastReadvanceDateDesc" Text="Last Readvance Date (RR):" /></td>
                                        <td style="width: 25%">
                                            <asp:Label runat="server" Visible="false" ID="lbLastReadvanceDate" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Arrears Last 6 months:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbHasArrears" Text="-" /></td>
                                        <td>
                                            <asp:Label runat="server" Visible="false" ID="lbLastReadvanceAmountDesc" Text="Last Readvance Amount (RR):" /></td>
                                        <td>
                                            <asp:Label runat="server" Visible="false" ID="lbLastReadvanceAmount" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Employment Type:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbEmploymentType" Text="-" /></td>
                                        <td>Interest Accrued (RR):</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbAccruedInterest" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Household Income:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbHouseholdIncome" Text="-" /></td>
                                        <td>Quick Cash Approved (FL):</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbQuickCashApproved" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Latest Valuation Date:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbLatestValuationDate" Text="-" /></td>
                                        <td>Title Deed on File (FL):</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbHaveTitleDeed" Text="-" /></td>
                                    </tr>
                                    <tr>
                                        <td>Latest Valuation Amount:</td>
                                        <td>
                                            <asp:Label runat="server" ID="lbLatestValuationAmount" Text="-" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apCredit" runat="server" Visible="false">
                            <Header>
                                <a href="">Credit Details</a>
                            </Header>
                            <Content>
                                <table class="tableStandard" width="100%">
                                    <tr>
                                        <td>Credit Decision:</td>
                                        <td>
                                            <SAHL:SAHLLabel runat="server" ID="tbCreditDecision"></SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Credit User:</td>
                                        <td>
                                            <SAHL:SAHLLabel runat="server" ID="tbCreditUser"></SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trComment" visible="false">
                                        <td colspan="2">Comments: (hover over description to see more detail)
                                            <cc1:MemoGrid ID="gridCreditComments" runat="server">
                                            </cc1:MemoGrid>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trDeclineReason" visible="false">
                                        <td colspan="2">Decline Reasons:
                                            <cc1:ReasonsGrid ID="gridCreditDeclineReasons" runat="server">
                                            </cc1:ReasonsGrid>
                                        </td>
                                    </tr>

                                </table>
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apQuickCash" runat="server" Visible="false">
                            <Header>
                                <a href="">Quick Cash Details</a>
                            </Header>
                            <Content>
                                <table class="tableStandard" width="100%">
                                    <tr runat="server" id="trQCApproved1" visible="false">
                                        <td class="titleText" style="width: 278px">Credit Approved Amount
                                        </td>
                                        <td class="cellDisplay" style="width: 218px">
                                            <SAHL:SAHLLabel ID="lblCreditApprovedAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td></td>
                                        <td class="titleText" style="width: 230px"></td>
                                        <td class="cellDisplay" style="width: 200px"></td>
                                        <td></td>
                                    </tr>
                                    <tr runat="server" id="trQCApproved2" visible="false">
                                        <td class="titleText">Credit Upfront Approved Amount
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblUpfrontCreditApprovedAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td></td>
                                        <td class="titleText"></td>
                                        <td class="cellDisplay"></td>
                                        <td></td>
                                    </tr>
                                    <tr runat="server" id="trQCDecline" visible="false">
                                        <td class="titleText" colspan="6">Quick Cash has been declined with the following reasons:
                                            <cc1:ReasonsGrid ID="gridQCDeclineReasons" runat="server">
                                            </cc1:ReasonsGrid>
                                        </td>
                                    </tr>
                                </table>
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apNTU" runat="server" Visible="false">
                            <Header>
                                <a href="">NTU Details</a>
                            </Header>
                            <Content>
                                <table class="tableStandard" width="100%">
                                    <tr>
                                        <td>NTU Reasons
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
                                        <td>Decline Reasons
                                            <cc1:ReasonsGrid ID="grdAdminDeclineReasons" runat="server">
                                            </cc1:ReasonsGrid>
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
            <td align="right">
                <SAHL:SAHLButton ID="btnTransitionHistory" runat="server" Text="History" OnClick="btnTransitionHistory_Click" />&nbsp;
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
