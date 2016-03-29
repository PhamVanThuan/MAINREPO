<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.LoanFinancialServiceSummary" Title="Untitled Page"
    Codebehind="LoanFinancialServiceSummary.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: left">
        <table width="100%" class="tableStandard">
            <tr>
                <td align="left" style="height: 99%;" valign="top">
                    <table border="0">
                        <tr class="rowStandard">
                            <td style="width: 176px;" class="TitleText">
                                Service
                            </td>
                            <td style="width: 201px;">
                                <SAHL:SAHLLabel ID="Service" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 176px;" class="TitleText">
                                Purpose
                            </td>
                            <td style="width: 201px;">
                                <SAHL:SAHLLabel ID="Purpose" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td class="TitleText">
                                Open Date
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="OpenDate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td class="TitleText">
                                Close Date
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="CloseDate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td class="TitleText">
                                Status
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="Status" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td class="TitleText">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td class="TitleText">
                                Initial Term
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="InitialTerm" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td class="TitleText">
                                Remaining Term
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="RemainingTerm" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td class="TitleText">
                                Total Term
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblTotalTerm" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td></td><td></td>
                        </tr>
                        <tr class="rowStandard">
                            <td class="TitleText">
                                Initial Balance
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="InitialBalance" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td class="TitleText">
                                Market Rate Description
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="MarketRateDescription" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td class="TitleText">
                                Current Balance
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="CurrentBalance" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td class="TitleText">
                                Market Rate
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="MarketRate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td class="TitleText">
                                Arrear Balance
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="ArrearBalance" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td class="TitleText">
                                Link Rate
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="LinkRate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td class="TitleText">
                                Instalment
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="Installment" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td class="TitleText">
                                Effective Rate
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="EffectiveRate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td class="TitleText">
                                Next Reset Date
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="NextResetDate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td class="TitleText" id="lblAmortisingInstallment" runat="server">
                                Amortisation Instalment
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="AmortisingInstallment" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
<%--                        <tr class="rowStandard">
                            <td class="TitleText">
                                PreApproved Amount
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblPreApproved" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td class="TitleText">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>--%>
                    </table>
                    <asp:Panel ID="AccruedInterestPanel" GroupingText="Interest" runat="server" Style="width: 370px;">
                        <table border="0" width="100%">
                            <tr class="rowStandard">
                                <td style="width: 65%;" class="TitleText">
                                    Interest: Current to Date
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="IntCurrentToDate" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr class="rowStandard">
                                <td class="TitleText">
                                    Interest: Total for Month
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="IntCurrentToMonth" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr class="rowStandard">
                                <td class="TitleText">
                                    Interest: Previous Month
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="IntPreviousMonth" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <SAHL:SAHLGridView ID="FinancialAdjustmentGrid" runat="server" AutoGenerateColumns="false"
                        FixedHeader="false" EnableViewState="false" GridHeight="150px" GridWidth="100%"
                        Width="100%" HeaderCaption="Financial Adjustments" NullDataSetMessage="" EmptyDataSetMessage="There are no Financial Adjustment entries."
                        OnRowDataBound="FinancialAdjustmentGrid_RowDataBound">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
            <tr id="ButtonRow" runat="server">
                <td align="right">
                    <SAHL:SAHLButton ID="LoyaltyButton" runat="server" Text="Loyalty Benefit" AccessKey="C"
                        OnClick="LoyaltyButton_Click" CausesValidation="false" ButtonSize="Size4" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
