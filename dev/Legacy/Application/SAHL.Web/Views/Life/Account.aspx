<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" Inherits="SAHL.Web.Views.Life.Account"
    Title="Account" Codebehind="Account.aspx.cs" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <table  style="width: 100%"  class="tableStandard">
            <tr>
                <td align="center" style="text-align: left">
                    <cc1:LegalEntityGrid ID="LegalEntityGrid" runat="server" ColumnIDPassportVisible="True" GridHeight="">
                    </cc1:LegalEntityGrid>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Panel ID="pnlLoanDetails" runat="server" Width="100%" GroupingText="Loan Summary">
                        <table border="0" width="100%">
                            <tr>
                                <td class="TitleText" style="width: 176px">
                                    Loan Number</td>
                                <td style="width: 250px">
                                    <SAHL:SAHLLabel ID="lblLoanNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td class="TitleText" style="width: 150px">
                                </td>
                                <td style="width: 150px">
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText" style="width: 176px;">
                                    Product</td>
                                <td style="width: 250px;">
                                    <SAHL:SAHLLabel ID="lblProduct" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                                <td class="TitleText" style="width: 150px;">
                                    Purpose
                                </td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblPurpose" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    Loan Open Date
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblOpenDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                                <td class="TitleText">
                                    Close Date
                                </td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblCloseDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    Account Status
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                                <td class="TitleText">
                                    Pipeline Status
                                </td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblPipelineStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    Initial Term
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblInitialTerm" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td class="TitleText">
                                    Remaining Term
                                </td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblRemainingTerm" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    Total Bond Amount</td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblBondAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                                <td class="TitleText">
                                    Instalment
                                </td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblInstallment" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    Current Balance</td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblCurrentBalance" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td class="TitleText">
                                    Arrear Balance
                                </td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblArrearBalance" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    Property Valuation</td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblPropertyValuation" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td class="TitleText">
                                    Discounted
                                    Link Rate
                                </td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblLinkRate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    Market Rate
                                Description</td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblMarketRateDescription" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td class="TitleText">
                                    Effective Rate
                                </td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblEffectiveRate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    Market Rate
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblMarketRate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td class="TitleText">
                                    Debit Order Day&nbsp;
                                </td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblDebitOrderDay" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    Next Reset Date
                                </td>
                                <td>
                                    <SAHL:SAHLLabel ID="lblNextResetDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td class="TitleText">
                                </td>
                                <td style="width: 150px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
