<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Codebehind="Policy.aspx.cs" Inherits="SAHL.Web.Views.Life.Policy" Title="Policy" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" Visible="false" />
    <div style="text-align: center; vertical-align: top">
        <asp:Panel ID="pnlPolicyDetails" runat="server" GroupingText="Policy Details" Width="100%">
            <table id="tPolicyDetails" border="0" style="width: 100%; vertical-align: top" class="tableStandard">

                <tr runat="server" id="rowManualLifePolicyPayment">
                    <td align="left" class="TitleText" style="width: 50%; vertical-align:top">
                        <table style="width: 100%" >
                            <tr>
                                <td align="left" style="width: 50%" class="TitleText">
                                    <SAHL:SAHLLabel runat="server" ForeColor="Red" Font-Bold="true">Manual Life Policy Payment</SAHL:SAHLLabel>
                                </td>
                                <td align="left" style="text-align: left">
                                    <SAHL:SAHLLabel runat="server"  ForeColor="Red" >Yes</SAHL:SAHLLabel>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%; vertical-align:top">
                        &nbsp;
                    </td>
                </tr>

                <tr>
                    <td align="left" class="TitleText" style="width: 50%; vertical-align:top">
                        <table style="width: 100%;">
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">
                                    Policy Type</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblPolicyType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">
                                    Policy Number</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel CssClass="LabelText" ID="lblPolicyNumber" runat="server">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText" align="left" style="width: 50%">
                                    Policy Status</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblPolicyStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                    <SAHL:SAHLLabel ID="lblPolicyStatusKey" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText" align="left" style="width: 50%">
                                    Application Status</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblApplicationStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText" align="left" style="width: 50%">
                                    Consultant</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel CssClass="LabelText" ID="lblConsultant" runat="server">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText" align="left" style="width: 50%">
                                    Accepted Date</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel CssClass="LabelText" ID="lblDateOfAcceptance" runat="server">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">
                                    Commencement Date</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel CssClass="LabelText" ID="lblDateOfCommencement" runat="server">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">
                                    Cancellation Date</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel CssClass="LabelText" ID="lblCancellationDate" runat="server">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">
                                </td>
                                <td align="left" style="width: 50%">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">
                                    Loan Number</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblLoanNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">
                                    Loan Status</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblLoanStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">
                                    Loan Term</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblLoanTerm" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">
                                    Loan Amount</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblLoanAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">
                                    Loan Pipeline Status</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblPipelineStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%" valign="top">
                                    Loan Debit Order Day</td>
                                <td align="left" style="width: 50%" valign="top">
                                    <SAHL:SAHLLabel ID="lblLoanDebitOrderDay" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%" valign="top">
                                    Life Condition of Loan</td>
                                <td align="left" style="width: 50%" valign="top">
                                    <SAHL:SAHLLabel ID="lblLifeCondition" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%" valign="top">
                                </td>
                                <td align="left" style="width: 50%" valign="top">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%" valign="top">
                                    Contact Name</td>
                                <td align="left" style="width: 50%" valign="top">
                                    <SAHL:SAHLLabel ID="lblPolicyHolder" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                        ID="ddlPolicyHolder" runat="server" CausesValidation="True" OnSelectedIndexChanged="ddlPolicyHolder_SelectedIndexChanged"
                                        AutoPostBack="True" PleaseSelectItem="False">
                                    </SAHL:SAHLDropDownList></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%" valign="top">
                                    <SAHL:SAHLLabel ID="lblAlternateContactText" runat="server" CssClass="LabelText" Font-Bold="True">Alternate Contact No</SAHL:SAHLLabel>
                                </td>
                                <td align="left" style="width: 50%" valign="top">
                                    <SAHL:SAHLLabel ID="lblAlternateContact" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                        </table>
                    </td>
                    <td align="left" class="TitleText" style="width: 50%; vertical-align:top">
                        <table>
                            <tr id="trClaimStatus" runat="server" class="backgroundError">
                                <td align="left" class="TitleText">
                                    Claim Status</td>
                                <td style="width: 50%; text-align: left">
                                    <SAHL:SAHLLabel ID="lblClaimStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr id="trClaimStatusDate" runat="server" class="backgroundError">
                                <td align="left" class="TitleText">
                                    Claim Status Date</td>
                                <td style="width: 50%; text-align: left">
                                    <SAHL:SAHLLabel ID="lblClaimStatusDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    Initial Sum Assured</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblInitialSumAssured" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText" align="left">
                                    Current Sum Assured</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblCurrentSumAssured" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr id="trRetainedDBSumAssured" runat="server" class="backgroundLight">
                                <td class="TitleText" align="left">
                                    Retained DB Sum Assured</td>
                                <td style="width: 50%; text-align: left">
                                    <SAHL:SAHLLabel ID="lblRetainedDBSumAssured" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr id="trReassuredDBAmount" runat="server" class="backgroundLight">
                                <td align="left" class="TitleText">
                                    Reassured DB Amount</td>
                                <td style="width: 50%; text-align: left">
                                    <SAHL:SAHLLabel ID="lblReassuredDBAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr id="trRetainedIPBSumAssured" runat="server" class="backgroundLight">
                                <td class="TitleText" align="left">
                                    Retained IPB Sum Assured</td>
                                <td style="width: 50%; text-align: left">
                                    <SAHL:SAHLLabel ID="lblRetainedIPBSumAssured" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr id="trReassuredIPBAmount" runat="server" class="backgroundLight">
                                <td align="left" class="TitleText">
                                    Reassured IPB Amount</td>
                                <td style="width: 50%; text-align: left">
                                    <SAHL:SAHLLabel ID="lblReassuredIPBAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr id="trSpacer1" runat="server">
                                <td colspan="2" style="height: 10px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    Death Benefit Premium</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblDeathBenefitPremium" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td id="benefitType" runat="server" align="left" class="TitleText">
                                    IP Benefit Premium</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblIPBenefitPremium" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    Annual Premium&nbsp;</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblAnnualPremium" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    Monthly Instalment</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblMonthlyInstalment" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    Bond Instalment</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblBondInstallment" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    HOC Instalment</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblHOCInstallment" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                             <tr>
                                <td align="left" class="TitleText">
                                    Monthly Service Fee</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblMonthlyServiceFee" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    Total Instalment</td>
                                <td style="text-align: left">
                                    <SAHL:SAHLLabel CssClass="LabelText" ID="lblTotalAmountDue" runat="server">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr id="trSpacer2" runat="server">
                                <td colspan="2" style="height: 10px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" style="text-align: left">
                                    The premiums are based on your current loan balance and age at next birthday</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlAssuredLivesGrid" runat="server" Width="100%">
            <table id="tAssuredLivesGrid" border="0" cellpadding="4" cellspacing="0" width="100%">
                <tr>
                    <td align="center" style="text-align: left">
                        <cc1:LegalEntityGrid ID="LegalEntityGrid" runat="server">
                        </cc1:LegalEntityGrid>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <SAHL:SAHLButton ID="btnAdd" runat="server" Text="Add Life" OnClick="btnAdd_Click"
                            Visible="False" ButtonSize="Size4" SecurityTag="LifeUpdateAccessWorkflow"></SAHL:SAHLButton>
                        <SAHL:SAHLButton ID="btnRemove" runat="server" Text="Remove Life" OnClick="btnRemove_Click"
                            Visible="False" ButtonSize="Size4" SecurityTag="LifeUpdateAccessWorkflow"></SAHL:SAHLButton>
                        <SAHL:SAHLButton ID="btnRecalculatePremiums" runat="server" Text="Recalculate Premiums" Visible="False"
                                OnClick="btnRecalculatePremiums_Click" ButtonSize="Size6" SecurityTag="LifeUpdateAccessWorkflow"></SAHL:SAHLButton>
                        <SAHL:SAHLButton ID="btnPremiumQuote" runat="server" Text="Premium Calculator" Visible="False"
                            OnClick="btnPremiumQuote_Click" ButtonSize="Size5" SecurityTag="LifeUpdateAccessWorkflow"></SAHL:SAHLButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlPlanButtons" runat="server" Width="100%">
            <table id="tPlanButtons" width="100%">
                <tr>
                    <td align="center" style="height: 26px">
                        <SAHL:SAHLButton ID="btnAccept" runat="server" Text="Accept Quote" OnClick="btnAccept_Click" SecurityTag="LifeUpdateAccessWorkflow"/>
                        <SAHL:SAHLButton ID="btnDecline" runat="server" Text="Decline Quote" OnClick="btnDecline_Click" SecurityTag="LifeUpdateAccessWorkflow" />
                        <SAHL:SAHLButton ID="btnQuote" runat="server" Text="Quote Required" ButtonSize="Size4"
                            OnClick="btnQuote_Click" SecurityTag="LifeUpdateAccessWorkflow"/>
                        <SAHL:SAHLButton ID="btnConsidering" runat="server" Text="Considering" OnClick="btnConsidering_Click" SecurityTag="LifeUpdateAccessWorkflow"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
