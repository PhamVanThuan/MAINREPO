<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Cap.CapOfferSales" Title="CAP Offer" Codebehind="CapOfferSales.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table id="CapPage" runat="server" width="100%" class="tableStandard">
        <tr>
            <td align="left" style="height: 99%;" valign="top">
                <table style="width: 100%">
                    <tr id="LENameRow" runat="server">
                        <td align="left" class="TitleText">
                            Legal Entity Name
                        </td>
                        <td colspan="4">
                            <SAHL:SAHLLabel ID="lblLegalEntityName" runat="server"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr id="AccountRow" runat="server" visible="false">
                        <td align="left" class="TitleText">
                            Account Number
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblAccountNumber" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TitleText">
                            Sales Consultant
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblSaleConsultant" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                        </td>
                        <td rowspan="5" colspan="2">
                            <SAHL:SAHLGridView ID="InstallmentGrid" runat="server" AutoGenerateColumns="false"
                                FixedHeader="false" EnableViewState="false" GridHeight="90px" GridWidth="100%"
                                Width="100%" HeaderCaption="Instalments" NullDataSetMessage="" EmptyDataSetMessage="There are no Instalments.">
                                <HeaderStyle CssClass="TableHeaderB" />
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TitleText">
                            Next Reset Date
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblNextResetDate" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TitleText">
                            CAP Effective Date
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblCapEffectiveDate" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TitleText">
                            Offer Start Date
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblOfferStartDate" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TitleText">
                            Offer End Date
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblOfferEndDate" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 176px;" align="left" class="TitleText">
                            Total Bond Amount
                        </td>
                        <td style="width: 186px;">
                            <SAHL:SAHLLabel ID="lblTotalBondAmount" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td style="width: 15px;">
                        </td>
                        <td style="width: 185px;" align="left" class="TitleText">
                            Loan Agreement Amount
                        </td>
                        <td style="width: 186px;">
                            <SAHL:SAHLLabel ID="lblLoanAgreementAmount" runat="server"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;" align="left" class="TitleText">
                            Latest Valuation
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblLatestValuation" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td></td>
                        <td align="left" class="TitleText">
                            Accrued Interest MTD
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblAccruedInterestMTD" runat="server"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TitleText">
                            Offer Status
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblOfferStatus" runat="server"></SAHL:SAHLLabel>
                            <SAHL:SAHLDropDownList ID="ddlOfferStatusList" runat="server" Visible="false">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                        </td>
                        <td align="left" class="TitleText">
                            Committed Loan Value (CLV)
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblCommittedLoanValue" runat="server"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr id="BalanceRow" runat="server" visible="false">
                        <td align="left" class="TitleText">
                            Balance to Cap
                        </td>
                        <td>
                            <SAHL:SAHLTextBox ID="lblBalanceToCap" runat="server" DisplayInputType="Currency"
                                MaxLength="12">0.00</SAHL:SAHLTextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:CheckBox ID="PromotionClient" runat="server" Text="Promotion Client" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TitleText">
                            Payment Option
                        </td>
                        <td colspan="4">
                            <SAHL:SAHLLabel ID="lblPaymentOption" runat="server"></SAHL:SAHLLabel>
                            <SAHL:SAHLDropDownList ID="ddlPaymentOption" runat="server">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TitleText">
                            Reason
                        </td>
                        <td colspan="4">
                            <SAHL:SAHLLabel ID="lblNTUReason" runat="server"></SAHL:SAHLLabel>
                            <SAHL:SAHLDropDownList ID="ddlNTUReason" runat="server">
                            </SAHL:SAHLDropDownList>
                        </td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td colspan="5">
                            <SAHL:SAHLGridView ID="CapOfferGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                                EnableViewState="false" GridHeight="200px" GridWidth="100%" Width="100%" PostBackType="NoneWithClientSelect"
                                HeaderCaption="CAP Offers" NullDataSetMessage="" EmptyDataSetMessage="There are no Offers."
                                OnRowDataBound="CapOfferGrid_RowDataBound">
                                <HeaderStyle CssClass="TableHeaderB" />
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" CausesValidation="false"
                    OnClick="CancelButton_Click" />
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" ButtonSize="Size6"
                    AccessKey="S" OnClick="SubmitButton_Click" />
            </td>
        </tr>
    </table>
    <table id="CapReport" runat="server" style="width: 100%;" visible="false">
        <tr>
            <td>
                <iframe runat="server" id="ReportViewerFrame" width="100%" height="470" src="" frameborder="0"
                    marginheight="0" marginwidth="0" scrolling="no" />
            </td>
        </tr>
    </table>
</asp:Content>
