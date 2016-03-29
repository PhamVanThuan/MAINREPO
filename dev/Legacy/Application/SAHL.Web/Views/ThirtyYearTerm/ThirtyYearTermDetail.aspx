<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="ThirtyYearTermDetail.aspx.cs" Inherits="SAHL.Web.Views.ThirtyYearTerm.ThirtyYearTermDetail"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: center">
        <br />
        <table class="tableStandard" width="100%" style="text-align: left">
            <tr>
                <td style="width: 5%;"></td>
                <td style="width: 50%;">
                    <asp:Panel ID="pnlDetail" runat="server">
                        <table width="100%">
                            <tr>
                                <td style="text-align: left; width: 40%;" class="TitleText"></td>
                                <td style="width:20%; border: 1px solid black; padding-left:5px" class="TitleText">Current
                                </td>
                                <td style="width:20%; border: 1px solid black; padding-left:5px" class="TitleText">
                                    30 Year Term
                                </td>
                                <td style="width:20%"></td>
                            </tr>
                            <tr>
                                <td style="text-align: left; border: 1px solid black; padding-left:5px" class="TitleText">Loan Agreement Amount</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblLoanAgreementAmt_20" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                 <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblLoanAgreementAmt_30" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Loan Term</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblTerm_20" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblTerm_30" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">LTV Ratio</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblLTV_20" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblLTV_30" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">PTI Ratio</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblPTI_20" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblPTI_30" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Market Rate</td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblMarketRate_20" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblMarketRate_30" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Link Rate</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblLinkRate_20" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblLinkRate_30" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Pricing for Risk Adjustment</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblPricingFoRiskAdj_20" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblPricingFoRiskAdj_30" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Effective Rate</td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblEffectiveRate_20" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblEffectiveRate_30" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Amortising Monthly Instalment</td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblInstalment_20" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblInstalment_30" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; border: 1px solid black; padding-left:5px" class="TitleText">Interest Paid over Term</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblInterest_20" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblInterest_30" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="pnlDecisionTreeMessages" runat="server" GroupingText="Application does not qualify for 30 Year Loan Term">
                        <asp:BulletedList ID="lstDecisionTreeMessages" runat="server"></asp:BulletedList>
                    </asp:Panel>

                </td>
                <td style="width: 35%;"></td>
            </tr>
            <tr>
                <td colspan="3">
                    <br />
                    <br />
                </td>
            </tr>

            <tr id="ButtonRow">
                <td colspan="3" align="right">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                        CausesValidation="False" />
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit"
                        CausesValidation="False" />

                </td>
                <td></td>
            </tr>
        </table>
    </div>
</asp:Content>
