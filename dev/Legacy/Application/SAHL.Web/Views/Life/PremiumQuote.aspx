<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="PremiumQuote.aspx.cs" Inherits="SAHL.Web.Views.Life.PremiumQuote"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <table class="tableStandard" width="100%">
            <tr>
                <td style="text-align: center">
                    <asp:Panel ID="pnlPolicyDetails" runat="server" GroupingText="Premiums and Instalments"
                        Width="100%" HorizontalAlign="Center">
                        <table id="tPolicyDetails" class="tableStandard" style="width: 100%">
                            <tr id="trPolicyType" visible="false" runat="server">
                                <td colspan="4" align="left" >
                                    <b>Policy Type</b>  
                                    <SAHL:SAHLDropDownList ID="ddlPolicyType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPolicyType_SelectedIndexChanged">
                                    </SAHL:SAHLDropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="cellDisplay" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 175px">
                                    Current Sum Assured</td>
                                <td align="left" class="TitleText" style="width: 139px">
                                    <SAHL:SAHLLabel ID="lblCurrentSumAssured" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td align="left" class="TitleText" style="width: 175px">
                                    Monthly Instalment</td>
                                <td align="left" class="TitleText" style="width: 139px">
                                    <SAHL:SAHLLabel ID="lblMonthlyInstalment" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    Death Benefit Premium</td>
                                <td style="width: 162px; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblDeathBenefitPremium" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td align="left" class="TitleText" style="width: 175px">
                                    Bond Instalment</td>
                                <td align="left" class="TitleText" style="width: 139px">
                                    <SAHL:SAHLLabel ID="lblBondInstallment" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td id = "benefitType" runat="server" class="TitleText" align="left">
                                    IP Benefit Premium</td>
                                <td style="width: 162px; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblIPBenefitPremium" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td align="left" class="TitleText" style="width: 175px">
                                    HOC Instalment</td>
                                <td align="left" class="TitleText" style="width: 139px">
                                    <SAHL:SAHLLabel ID="lblHOCInstallment" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText" align="left">
                                    <strong>Annual Premium&nbsp;</strong></td>
                                <td style="width: 162px; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblAnnualPremium" runat="server" CssClass="LabelText" Font-Bold="True"
                                        BorderStyle="Solid" BorderWidth="1px">-</SAHL:SAHLLabel></td>
                                <td align="left" class="TitleText" style="width: 175px">
                                    Monthly Service Fee</td>
                                <td align="left" class="TitleText" style="width: 139px">
                                    <SAHL:SAHLLabel ID="lblMonthlyServiceFee" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                </td>
                                <td style="width: 162px; text-align: left">
                                </td>
                                <td align="left" class="TitleText" style="width: 175px">
                                    <strong>Total Instalment</strong></td>
                                <td align="left" class="TitleText" style="width: 139px">
                                    <SAHL:SAHLLabel CssClass="LabelText" ID="lblTotalAmountDue" runat="server" Font-Bold="True"
                                        BorderStyle="Solid" BorderWidth="1px">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText" align="left">
                                </td>
                                <td style="width: 162px; text-align: left;">
                                </td>
                                <td align="left" class="TitleText" style="width: 175px">
                                </td>
                                <td align="left" class="TitleText" style="width: 139px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4" style="text-align: center">
                                    The premiums are based on your current loan balance and age at next birthday</td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4" style="text-align: center">
                                    Only active Legal Entities will be included in the premium calculations.</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <SAHL:SAHLButton ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click"
                                        Visible="True" ButtonSize="Size4" ToolTip="Press to Calculate Premiums and Instalments"
                                        CausesValidation="False"></SAHL:SAHLButton></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlAssuredLivesGrid" runat="server" Width="100%" GroupingText="Assured Lives">
                        <table id="tAssuredLivesGrid" border="0" cellpadding="4" cellspacing="0" width="100%">
                            <tr>
                                <td align="center">
                                    <SAHL:SAHLGridView ID="LegalEntityGrid" runat="server" HeaderCaption="Assured Lives" AutoGenerateColumns="false"
                                        EmptyDataSetMessage="There are no Assured Lives on this Policy." EmptyDataText="There are no Assured Lives on this Policy."
                                        EnableViewState="false" FixedHeader="false" GridWidth="100%" 
                                        NullDataSetMessage="" OnRowDataBound="LegalEntityGrid_RowDataBound" Width="100%" GridHeight="140px">
                                        <HeaderStyle CssClass="TableHeaderB" />
                                        <RowStyle CssClass="TableRowA" />
                                    </SAHL:SAHLGridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlAddLives" runat="server" Width="100%" GroupingText="Add Assured Lives">
                        <table id="tAddLives" border="0" cellpadding="4" cellspacing="0" width="100%">
                            <tr style="text-align: left; vertical-align: middle">
                                <td>
                                    Name
                                </td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtName" runat="server" Width="300px"></SAHL:SAHLTextBox>
                                </td>
                                <td>
                                    Date Of Birth
                                </td>
                                <td>
                                    <SAHL:SAHLDateBox ID="dteDOB" runat="server" />
                                </td>
                                <td>
                                    <SAHL:SAHLButton ID="btnAdd" runat="server" ButtonSize="Size2" OnClick="btnAddLife_Click"
                                        Text="Add" Visible="True" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlButtons" runat="server" Width="100%">
                        <table id="tButtons" border="0" cellpadding="4" cellspacing="0" width="100%">
                            <tr>
                                <td style="text-align: center">
                                    &nbsp;<SAHL:SAHLButton ID="btnReturn" runat="server" Text="Cancel" OnClick="btnReturn_Click"
                                        Visible="True" ButtonSize="Size4" CausesValidation="False"></SAHL:SAHLButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
