<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Cap.CapSalesConfiguration" Title="Cap Sales Configuration"
    CodeBehind="CapSalesConfiguration.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 103%;" valign="top">
                <table border="0">
                    <tr>
                        <td style="width: 200px;" class="TitleText">Cap Reset Configuration Date
                        </td>
                        <td style="width: 205px;">
                            <SAHL:SAHLDropDownList ID="CapResetConfigDate" runat="server" Style="width: 98%;"
                                AutoPostBack="true" PleaseSelectItem="false">
                            </SAHL:SAHLDropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <SAHL:SAHLGridView ID="CapSalesConfigGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="150px" GridWidth="100%"
                    Width="100%" HeaderCaption="Cap Sales Configuration" NullDataSetMessage="There are no Cap Sales Configurations."
                    EmptyDataSetMessage="There are no Cap Sales Configurations." OnRowDataBound="CapSalesConfigGrid_RowDataBound"
                    OnSelectedIndexChanged="CapSalesConfigGrid_SelectedIndexChanged">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
                <table border="0">
                    <tr>
                        <td valign="top">
                            <table border="0">
                                <tr>
                                    <td style="width: 205px;" class="TitleText">Cap Type
                                    </td>
                                    <td style="width: 165px;">
                                        <SAHL:SAHLLabel ID="CapTypeValue" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLDropDownList ID="CapTypeValueUpdate" runat="server" Style="width: 98%;"
                                            OnSelectedIndexChanged="CapTypeValue_SelectedIndexChanged" AutoPostBack="true"
                                            PleaseSelectItem="false">
                                        </SAHL:SAHLDropDownList>
                                    </td>
                                    <td style="width: 5px;">
                                        <SAHL:SAHLRequiredFieldValidator ID="ValCapTypeValueUpdate" runat="server" ControlToValidate="CapTypeValueUpdate"
                                            ErrorMessage="Please select a Cap Type" InitialValue="-select-" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 205px;" class="TitleText">Cap Base Rate
                                    </td>
                                    <td style="width: 165px;">
                                        <SAHL:SAHLLabel ID="CapBaseRate" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 205px;" class="TitleText">Offer Start Date
                                    </td>
                                    <td style="width: 165px;">
                                        <SAHL:SAHLLabel ID="OfferStartDate" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLDateBox ID="OfferStartDateUpdate" runat="server" />
                                    </td>
                                    <td style="width: 5px;">
                                        <SAHL:SAHLTextBox ID="OfferStartDateUpdateControl" runat="server" Style="display: none;"></SAHL:SAHLTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 205px;" class="TitleText">Offer End Date
                                    </td>
                                    <td style="width: 165px;">
                                        <SAHL:SAHLLabel ID="OfferEndDate" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLDateBox ID="OfferEndDateUpdate" runat="server" />
                                    </td>
                                    <td style="width: 5px;">
                                        <SAHL:SAHLTextBox ID="ValOfferEndDateUpdateControl" runat="server" Style="display: none;"></SAHL:SAHLTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 205px;" class="TitleText">Premium Fee (Cost per R1mil)
                                    </td>
                                    <td style="width: 165px;">
                                        <SAHL:SAHLLabel ID="PremiumFee" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLTextBox ID="PremiumFeeUpdate" runat="server" DisplayInputType="CurrencyUnLimitedDecimals"
                                            MaxLength="18" AutoPostBack="true"></SAHL:SAHLTextBox>
                                    </td>
                                    <td style="width: 5px;">&nbsp
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 205px;" class="TitleText">Admin Fee (Cost per R1mil)
                                    </td>
                                    <td style="width: 165px;">
                                        <SAHL:SAHLLabel ID="AdminFee" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLTextBox ID="AdminFeeUpdate" runat="server" DisplayInputType="CurrencyUnLimitedDecimals"
                                            MaxLength="18" AutoPostBack="true"></SAHL:SAHLTextBox>
                                    </td>
                                    <td style="width: 5px;">&nbsp
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table border="0">
                                <tr id="StatusRow" runat="server">
                                    <td style="width: 190px;" class="TitleText">Status
                                    </td>
                                    <td style="width: 140px;">
                                        <SAHL:SAHLLabel ID="Status" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLDropDownList ID="StatusUpdate" runat="server" Style="width: 98%;" PleaseSelectItem="true">
                                        </SAHL:SAHLDropDownList>
                                    </td>
                                    <td style="width: 5px;">&nbsp
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 190px;" class="TitleText">Cap Effective Date
                                    </td>
                                    <td style="width: 140px;">
                                        <SAHL:SAHLLabel ID="CapEffectiveDate" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 190px;" class="TitleText">Term
                                    </td>
                                    <td style="width: 140px;">
                                        <SAHL:SAHLLabel ID="Term" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLTextBox ID="TermUpdate" runat="server" DisplayInputType="Number" AutoPostBack="true"
                                            MaxLength="3"></SAHL:SAHLTextBox>
                                    </td>
                                    <td style="width: 5px;">&nbsp
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 190px;" class="TitleText">Cap Closure Date
                                    </td>
                                    <td style="width: 140px;">
                                        <SAHL:SAHLLabel ID="CapClosureDate" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 190px;" class="TitleText">Finance Rate (Strike Rate)
                                    </td>
                                    <td style="width: 140px;">
                                        <SAHL:SAHLLabel ID="FinanceRate" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLTextBox ID="FinanceRateUpdate" runat="server" DisplayInputType="CurrencyUnLimitedDecimals"
                                            MaxLength="18"></SAHL:SAHLTextBox>
                                    </td>
                                    <td style="width: 5px;">&nbsp
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 190px;" class="TitleText">Premium (Cost per R1mil)
                                    </td>
                                    <td style="width: 140px;">
                                        <SAHL:SAHLLabel ID="Premium" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                    <td style="width: 5px;">&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                    CausesValidation="false" />
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />
                <SAHL:SAHLCustomValidator ID="ValSubmitValuation" runat="server" ControlToValidate="SubmitValuation"
                    ErrorMessage="The current active debit order may not be deleted" />
                <SAHL:SAHLTextBox ID="SubmitValuation" runat="server" Style="display: none;"></SAHL:SAHLTextBox>
            </td>
        </tr>
    </table>
</asp:Content>
