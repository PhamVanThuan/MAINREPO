<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.TradeManagement" Title="Trade Management" Codebehind="TradeManagement.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left"  valign="top">
                <table id="InfoTable" runat="server" class="tableStandard" width="100%">
                    <tr>
                        <td style="width: 10%;" class="TitleText">
                            Trade Type</td>
                        <td style="width: 150px">
                            <SAHL:SAHLDropDownList ID="ddlTradeType" runat="server" PleaseSelectItem="false"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlTradeType_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%;" class="TitleText">
                            Reset Date</td>
                        <td style="width: 300px">
                            <SAHL:SAHLDropDownList ID="ddlResetDate" runat="server" AutoPostBack="True"
                                EnableViewState="False" OnSelectedIndexChanged="ddlResetDate_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan='2'>
                            <SAHL:SAHLGridView ID="TradeGroupingGrid" runat="server" AutoGenerateColumns="false"
                                FixedHeader="false" EnableViewState="false" GridHeight="100px" GridWidth="100%"
                                Width="100%" HeaderCaption="Trade Groupings" NullDataSetMessage="" EmptyDataSetMessage="There are no Trade Groupings"
                                OnSelectedIndexChanged="TradeGroupingGrid_SelectedIndexChanged" PostBackType="SingleClick"
                                OnRowDataBound="TradeGroupingGrid_RowDataBound">
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan='2'>
                            <SAHL:SAHLGridView ID="TradeGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                                EnableViewState="false" GridHeight="100px" GridWidth="100%" Width="100%" HeaderCaption="Trades"
                                NullDataSetMessage="" EmptyDataSetMessage="There are no Trades." OnSelectedIndexChanged="TradeGrid_SelectedIndexChanged"
                                PostBackType="SingleAndDoubleClick" OnRowDataBound="TradeGrid_RowDataBound">
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan='2'>
                            <asp:Panel ID="AddUpdatePanel" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%;" class="TitleText">
                                            Company</td>
                                        <td style="width: 25%;">
                                            <SAHL:SAHLTextBox ID="TradeCompany" DisplayInputType="AlphaNumNoSpace" runat="server"
                                                MaxLength="50"></SAHL:SAHLTextBox></td>
                                        <td style="width: 15%;" class="TitleText">
                                            Premium</td>
                                        <td style="width: 25%;">
                                            <SAHL:SAHLTextBox ID="TradePremium" runat="server" DisplayInputType="CurrencyUnLimitedDecimals"></SAHL:SAHLTextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%;" class="TitleText">
                                            Trade Date</td>
                                        <td style="width: 25%;">
                                            <SAHL:SAHLDateBox ID="TradeDate" runat="server" /></td>
                                        <td style="width: 15%;" class="TitleText">
                                            Trade Balance</td>
                                        <td style="width: 25%;">
                                            <SAHL:SAHLTextBox ID="TradeBalance" runat="server" DisplayInputType="Currency"></SAHL:SAHLTextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%;" class="TitleText">
                                            Strike Rate (%)</td>
                                        <td style="width: 25%;">
                                            <SAHL:SAHLTextBox ID="TradeStrikeRate" runat="server" DisplayInputType="CurrencyUnLimitedDecimals"></SAHL:SAHLTextBox></td>
                                        <td style="width: 15%;" class="TitleText">
                                            Effective Date</td>
                                        <td style="width: 30%;">
                                            <SAHL:SAHLDateBox ID="TradeEffectiveDate" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%;" class="TitleText">
                                            Term</td>
                                        <td style="width: 25%;">
                                            <SAHL:SAHLTextBox ID="TradeTerm" runat="server" DisplayInputType="Number"></SAHL:SAHLTextBox></td>
                                        <td style="width: 15%;" class="TitleText">
                                            Closure Date</td>
                                        <td style="width: 30%;">
                                            <SAHL:SAHLLabel ID="TradeClosureDate" runat="server" /></td>
                                    </tr>
                                    <tr id="CapTypeRow" runat="server">
                                        <td style="width: 15%;" class="TitleText">
                                            Cap Type</td>
                                        <td style="width: 25%;">
                                            <SAHL:SAHLDropDownList ID="TradeCapType" runat="Server">
                                            </SAHL:SAHLDropDownList></td>
                                        <td colspan='3'>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </asp:Panel>
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
            </td>
        </tr>
    </table>
</asp:Content>
