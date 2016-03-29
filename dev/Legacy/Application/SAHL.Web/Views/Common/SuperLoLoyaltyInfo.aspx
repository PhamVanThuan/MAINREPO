<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.SuperLoLoyaltyInfo" Title="CBO Page" CodeBehind="SuperLoLoyaltyInfo.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" style="width: 100%;">
        <tr>
            <td align="left" valign="top">
                <asp:Panel ID="LoyaltyBenefitPanel" GroupingText="Loyalty Benefit" runat="server">
                    <table border="0" width="100%">
                        <tr>
                            <td style="width: 200px;" class="TitleText">
                                Accumulated To Date
                            </td>
                            <td style="width: 170px;">
                                <SAHL:SAHLLabel ID="AccumulatedToDate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" class="TitleText">
                                Next Payment Date
                            </td>
                            <td style="width: 170px;">
                                <SAHL:SAHLLabel ID="NextPaymentDate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;" class="TitleText">
                                Accumulated Month To Date
                            </td>
                            <td style="width: 170px;">
                                <SAHL:SAHLLabel ID="AccumulatedMonthToDate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PrepaymentThresholds" GroupingText="Prepayment Thresholds" runat="server"
                    Style="width: 100%;">
                    <table border="0" width="100%">
                        <tr>
                            <td style="width: 200px;" class="TitleText" align="left">
                            </td>
                            <td style="width: 170px;" class="TitleText" align="left">
                                Annual
                            </td>
                            <td style="width: 170px;" class="TitleText" align="left">
                                Cumulative
                            </td>
                            <td align="left" class="TitleText" style="width: 170px">
                                Pre-Payment Allowed
                            </td>
                            <td style="width: 170px;" align="left" class="TitleText">
                                Over Payment
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;" class="TitleText" align="left">
                                Year 1
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="Year1Annual" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="Year1Cumulative" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td align="left" style="width: 170px">
                                <SAHL:SAHLLabel ID="lblPrePaymentY1" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="lblOverPaymentY1" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;" class="TitleText" align="left">
                                Year 2
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="Year2Annual" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="Year2Cumulative" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td align="left" style="width: 170px">
                                <SAHL:SAHLLabel ID="lblPrePaymentY2" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="lblOverPaymentY2" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;" class="TitleText" align="left">
                                Year 3
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="Year3Annual" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="Year3Cumulative" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td align="left" style="width: 170px">
                                <SAHL:SAHLLabel ID="lblPrePaymentY3" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="lblOverPaymentY3" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;" class="TitleText" align="left">
                                Year 4
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="Year4Annual" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="Year4Cumulative" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td align="left" style="width: 170px">
                                <SAHL:SAHLLabel ID="lblPrePaymentY4" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="lblOverPaymentY4" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;" class="TitleText" align="left">
                                Year 5
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="Year5Annual" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="Year5Cumulative" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td align="left" style="width: 170px">
                                <SAHL:SAHLLabel ID="lblPrePaymentY5" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 170px;" align="left">
                                <SAHL:SAHLLabel ID="lblOverPaymentY5" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlThresholdManagement" GroupingText="Threshhold Management" runat="server" Style="width: 100%;">
                    <table border="0" width="100%">
                        <tr>
                            <td style="width: 200px;" class="TitleText">
                                Exclude From Opt Out
                            </td>
                            <td style="width: 170px;">
                                <SAHL:SAHLCheckbox ID="chbExclude" Enabled="true" SecurityDisplayType="Disable" SecurityTag="SuperLoLoyaltyInfoEdit"
                                    runat="server" />
                                Exclusion End Date
                            </td>
                            <td style="width: 170px;">
                                <SAHL:SAHLDateBox ID="dtExclusionDate" Enabled="true" SecurityDisplayType="Disable"
                                    SecurityTag="SuperLoLoyaltyInfoEdit" runat="server" Mandatory="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;" class="TitleText">
                                Exclusion reason
                            </td>
                            <td style="width: 170px;" colspan="2">
                                <SAHL:SAHLTextBox ID="txtExcludeReason" Enabled="true" SecurityDisplayType="Disable"
                                    SecurityTag="SuperLoLoyaltyInfoEdit" runat="server" Width="300px" />
                            </td>
                            <td>
                                <SAHL:SAHLButton ID="btnUpdateThreshold" Enabled="true" SecurityDisplayType="Hide"
                                    SecurityTag="SuperLoLoyaltyInfoEdit" runat="server" Text="Update" AccessKey="C"
                                    OnClick="btnUpdateThreshold_Click" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <SAHL:SAHLGridView ID="LoyaltyPaymentGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="200px" GridWidth="100%"
                    Width="100%" HeaderCaption="Loyalty Benefit Payment History" ScrollX="true" NullDataSetMessage="There are no Loyalty Benefit Payment entries"
                    EmptyDataSetMessage="There are no Loyalty Benefit Payment entries" OnRowDataBound="LoyaltyPaymentGrid_RowDataBound">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
        </table>
        <table id="tblCreateSpace" runat="server" visible="false" class="tableStandard" style="width: 100%;">
        <tr>
        <td>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
        </td>
        </tr>
        </table>
        <table class="tableStandard" style="width: 100%;">
        <tr id="ButtonRow" runat="server" style="width:100%">
            <td align="right" >
                <table>
                    <tr>
                        <td>
                            <SAHL:SAHLButton ID="SuperLoOptOut" runat="server" Text="Opt Out" AccessKey="O" OnClick="SuperLoOptOutButton_Click"
                                CausesValidation="false" />
                        </td>
                        <td align="right">
                            <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                                CausesValidation="false" />
                        </td>
                        <td style="width:30%">

                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
