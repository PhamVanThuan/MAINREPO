<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="ParentAccountSummary.aspx.cs" Inherits="SAHL.Web.Views.Common.ParentAccountSummary"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: left">
        <table width="100%" class="tableStandard">
            <tr>
                <td>
                    <SAHL:SAHLGridView ID="SummaryGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                        EnableViewState="false" GridHeight="115px" GridWidth="100%" Width="100%" HeaderCaption="Product Summary"
                        NullDataSetMessage="" EmptyDataSetMessage="There are no Products." OnRowDataBound="SummaryGrid_RowDataBound">
                        <HeaderStyle CssClass="TableHeaderB" />
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                    <br />
                    <table border="0" width="100%" class="tableStandard">
                        <tr>
                            <td style="width: 30%;" class="TitleText" align="left">
                                Total Loan Instalment
                            </td>
                            <td style="width: 70%;" align="left">
                                <SAHL:SAHLLabel ID="TotalLoanInstallment" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText" align="left">
                                Total Short Term Loan Instalment
                            </td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="TotalShortTermLoanInstallment" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText" align="left">
                                Total Premiums
                            </td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="TotalPremiums" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText" align="left">
                                Monthly Service Fee
                            </td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="ServiceFee" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText" align="left">
                                Total Amount Due
                            </td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="TotalAmountDue" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText" align="left">
                                Fixed Debit Order Amount
                            </td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="FixedDebitOrderAmount" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <asp:Panel runat="server" ID="SubsidyPanel">
                            <tr>
                                <td class="TitleText">
                                    Subsidy Stop Order Amount
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="SubsidyStopOrderAmount" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr id="ArrearsRow" runat="server">
                            <td class="TitleText" align="left">
                                Months in Arrears
                            </td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="MonthsInArrears" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Panel ID="pnlInterestOnly" GroupingText="Interest Only Product Information"
                        runat="server" Style="width: 99%;" HorizontalAlign="Left">
                        <table border="0" width="100%">
                            <tr>
                                <td style="width: 30%;" class="TitleText">
                                    <asp:Label runat="server" ID="lblMaturityOrExpiryDate" Text="Maturity Date"></asp:Label>
                                </td>
                                <td style="width: 70%;"> 
                                    <SAHL:SAHLLabel ID="lblMaturityDate" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    Amortising Instalment
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblAmortisingInstallment" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <SAHL:SAHLGridView ID="FinancialAdjustmentGrid" runat="server" AutoGenerateColumns="false"
                        FixedHeader="false" EnableViewState="false" GridHeight="173px" GridWidth="100%"
                        Width="100%" HeaderCaption="Financial Adjustments" NullDataSetMessage="" EmptyDataSetMessage="There are no Financial Adjustment entries."
                        OnRowDataBound="FinancialAdjustmentGrid_RowDataBound">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
