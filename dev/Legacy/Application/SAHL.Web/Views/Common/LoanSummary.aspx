<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.LoanSummary" Title="Loan Summary" Codebehind="LoanSummary.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div style="text-align: left">
        <table width="100%" class="tableStandard">
            <tr class="rowStandard">
                <td style="width: 176px;" class="TitleText">
                    Account Number
                </td>
                <td style="width: 181px;">
                    <SAHL:SAHLLabel ID="lblAccountNumber" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td style="width: 176px;" class="TitleText">
                    Account Status
                </td>
                <td style="width: 181px;">
                    <SAHL:SAHLLabel ID="lblAccountStatus" runat="server">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr class="rowStandard">
                <td class="TitleText">
                    Open Date
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblOpenDate" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td class="TitleText">
                    Product
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblProduct" runat="server">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr class="rowStandard">
                <td class="TitleText">
                    Total Bond Amount
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblTotalBondAmount" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td class="TitleText">
                    Latest Valuation
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblLatestProperyValuation" runat="server">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr class="rowStandard">
                <td class="TitleText">
                    Loan Agreement
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblLoanAgreementAmount" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td class="TitleText">
                    Valuation Date
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblValuationDate" runat="server">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr class="rowStandard">
                <td class="TitleText">
                    Committed Loan Value (CLV)
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblCLV" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td class="TitleText">
                    Close Date
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblCloseDate" runat="server">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr class="rowStandard">
                <td class="TitleText">
                    SPV Description
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblSPVDescription" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td class="TitleText">
                    Title Deed on File
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lbHaveTitleDeed" runat="server">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr class="rowStandard">
                <td class="TitleText">
                    Household Income
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblHouseholdIncome" runat="server">-</SAHL:SAHLLabel>
                </td>
                 <td class="TitleText">
                    Naedo Compliant
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblNaedoCompliant" runat="server">-</SAHL:SAHLLabel>
                </td>
            </tr>
            
            <tr>
                <td class="TitleText">
                    Debit Order Day
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblDebitOrderDay" runat="server">-</SAHL:SAHLLabel>
                </td>
                                
                <td class="TitleText">
                    GEPF Funded
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblGEPFFunded" runat="server">-</SAHL:SAHLLabel>
                </td>  
            </tr>
            
            <tr class="rowStandard">
                <td class="TitleText">
                    LTV
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblLTV" runat="server">-</SAHL:SAHLLabel>
                </td>
                
                <td colspan="2" rowspan="6" valign="top">
                    <asp:Panel ID="AccruedInterestPanel" GroupingText="Accrued Interest" runat="server"
                        Style="width: 340px;">
                        <table border="0" width="100%">
                            <tr class="rowStandard">
                                <td style="width: 150px;" class="TitleText">
                                    Accrued Interest
                                </td>
                                <td class="TitleText">
                                    Variable
                                </td>
                                <td id="FixedCell1" runat="server" class="TitleText">
                                    Fixed
                                </td>
                            </tr>
                            <tr class="rowStandard">
                                <td class="TitleText">
                                    Current to Date
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblCurrentToDateVariable" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td id="FixedCell2" runat="server">
                                    <SAHL:SAHLLabel ID="lblCurrentToDateFixed" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr class="rowStandard">
                                <td class="TitleText">
                                    Total for Month
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblCurrentToMonthVariable" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td id="FixedCell3" runat="server">
                                    <SAHL:SAHLLabel ID="lblCurrentToMonthFixed" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr class="rowStandard">
                                <td class="TitleText">
                                    Previous Month
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblPreviousMonthVariable" runat="server">-</SAHL:SAHLLabel>
                                </td>
                                <td id="FixedCell4" runat="server">
                                    <SAHL:SAHLLabel ID="lblPreviousMonthFixed" runat="server">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>         
            </tr>
 
            <tr id="rowPTI" runat="server" class="rowStandard">
                <td class="TitleText">
                    PTI
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblPTI" runat="server">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr id="rowMaturityDate" runat="server" class="rowStandard">
                <td class="TitleText">
                     <SAHL:SAHLLabel ID="lblMaturityDateTitle" Font-Bold="true" runat="server">Maturity Date</SAHL:SAHLLabel>
                </td>
                <td>
                    <SAHL:SAHLLabel ID="lblMaturityDate" runat="server">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr runat="server" class="rowStandard">
                <td class="TitleText">
                    Non-Performing
                </td>
                <td>
                    <asp:CheckBox ID="NonPerformingCheck" runat="server" Text="" />
                </td>
            </tr>

            <tr id="rowManualLifePolicyPayment" runat="server" class="rowStandard">
                <td class="TitleText">
                     <SAHL:SAHLLabel runat="server" ForeColor="Red" Font-Bold="true">Manual Life Policy Payment</SAHL:SAHLLabel>
                </td>
                <td>
                    <SAHL:SAHLLabel runat="server"  ForeColor="Red" >Yes</SAHL:SAHLLabel>
                </td>
            </tr>

            <tr class="rowStandard"></tr>
            <tr class="rowStandard">
                <td colspan="4">
                    <SAHL:SAHLGridView ID="LoansGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                        EnableViewState="false" GridHeight="100px" GridWidth="100%" Width="100%" HeaderCaption="Loans"
                        NullDataSetMessage="" EmptyDataSetMessage="There are no Loans." OnRowDataBound="LoansGrid_RowDataBound"
                        ShowFooter="true">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
            <tr class="rowStandard">
                <td colspan="4">
                    <SAHL:SAHLGridView ID="ShortTermLoansGrid" runat="server" AutoGenerateColumns="false"
                        FixedHeader="false" EnableViewState="false" GridHeight="80px" GridWidth="100%"
                        Width="100%" HeaderCaption="Short Term Loans" NullDataSetMessage="" EmptyDataSetMessage="There are no Short Term Loans."
                        OnRowDataBound="ShortTermLoansGrid_RowDataBound" ShowFooter="true">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
