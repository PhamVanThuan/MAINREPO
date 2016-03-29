<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="SalariedWithDeductionPricingComparison.aspx.cs" Inherits="SAHL.Web.Views.SalariedWithDeduction.SalariedWithDeductionPricingComparison"
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
                                <td style="width:20%; border: 1px solid black; padding-left:5px" class="TitleText">
                                    Contractual Rate
                                </td>
                                <td style="width:20%; border: 1px solid black; padding-left:5px" class="TitleText">
                                    Discounted Rate
                                </td>
                                <td style="width:20%"></td>
                            </tr>
                            <tr>
                                <td style="text-align: left; border: 1px solid black; padding-left:5px" class="TitleText">Total Loan Requirement</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblTotalLoanRequirement_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                 <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblTotalLoanRequirement_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">LTV Ratio</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblLTV_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblLTV_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">PTI Ratio</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblPTI_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblPTI_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Market Rate</td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblMarketRate_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblMarketRate_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Link Rate</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblLinkRate_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblLinkRate_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Discount on Rate</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblDiscountOnRate_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblDiscountOnRate_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Pricing Adjustment</td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblPricingAdjustment_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblPricingAdjustment_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Effective Rate</td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblEffectiveRate_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblEffectiveRate_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;border: 1px solid black; padding-left:5px" class="TitleText">Monthly Instalment</td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblInstalment_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                               <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblInstalment_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; border: 1px solid black; padding-left:5px" class="TitleText">Interest Paid over Term</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblInterest_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblInterest_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; border: 1px solid black; padding-left:5px" class="TitleText">Registration Fee</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblRegistrationFee_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblRegistrationFee_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; border: 1px solid black; padding-left:5px" class="TitleText">Initiation Fee</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblInitiationFee_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblInitiationFee_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; border: 1px solid black; padding-left:5px" class="TitleText">Total Fees</td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblTotalFees_Current" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td style="border: 1px solid black; padding-left:5px">
                                    <SAHL:SAHLLabel ID="lblTotalFees_New" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                        </table>
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
                </td>
                <td></td>
            </tr>
        </table>
    </div>
</asp:Content>
