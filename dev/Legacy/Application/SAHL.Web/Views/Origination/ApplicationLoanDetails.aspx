<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="ApplicationLoanDetails.aspx.cs" Inherits="SAHL.Web.Views.Origination.ApplicationLoanDetails" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script language="javascript" type="text/javascript">
        function disableSubmit() {
            SAHLButton_setEnabled('<%=btnUpdate.ClientID%>', false);
        }

        function enableUpdateButton(enabled) {
            SAHLButton_setEnabled('<%=btnUpdate.ClientID%>', enabled);
        }

        function discountChange(chkDiscountedLinkRate) {
            disableSubmit();
            hideDiscount(chkDiscountedLinkRate.checked); //hide discount JS is inserted by LoanDetails control
            hideDiscountEHL(chkDiscountedLinkRate.checked);
        }

    </script>
    &nbsp;
    <br />
    <table width="100%" class="tableStandard">
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" GroupingText="Application Details" Width="100%">
                    <table width="100%" class="tableStandard">
                        <tr>
                            <td style="width: 33%">
                            </td>
                            <td style="width: 67%">
                            </td>
                        </tr>
                        <tr runat="server" id="trIsExceptionCreditCriteria" visible="false">
                            <td class="TitleText" style="width: 33%">
                                <font style="color: Red">Application marginally out of criteria</font>
                            </td>
                            <td style="width: 67%">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table width="100%" class="tableStandard">
                                    <tr>
                                        <td class="TitleText">
                                            Product
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblProduct" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                                runat="server" ID="ddlProduct" Width="134px" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </SAHL:SAHLDropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            SPV Name
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblSPVName" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                            <SAHL:SAHLDropDownList runat="server" ID="ddlSPV" Width="134px">
                                            </SAHL:SAHLDropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Application Type
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblApplicationType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Category
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblCategory" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                            <%--<SAHL:SAHLDropDownList runat="server" ID="ddlCategory" Width="134px">
                                </SAHL:SAHLDropDownList>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Minimum Bond
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblMinimumBond" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Bond to Register
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblBondToRegister" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                            <SAHL:SAHLCurrencyBox runat="server" ID="txtBondtoRegister" DisplayInputType="Currency"
                                                onkeyup="disableSubmit()" CssClass="mandatory"></SAHL:SAHLCurrencyBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            <SAHL:SAHLLabel ID="lblPropertValuation" runat="server" CssClass="TitleText">Est. Property Value</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblPropertyValue" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                            <SAHL:SAHLTextBox ID="txtPropertyValue" runat="server" DisplayInputType="Number"
                                                Width="130px" onkeyup="disableSubmit()" CssClass="mandatory"></SAHL:SAHLTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Term
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblTerm" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                            <SAHL:SAHLTextBox ID="txtTerm" runat="server" DisplayInputType="Number" Width="130px"
                                                onkeyup="disableSubmit()" MaxLength="3" CssClass="mandatory"></SAHL:SAHLTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Household Income
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblHouseHoldIncome" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Occupancy Type
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblOccupancyType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table width="100%" class="tableStandard">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlSwitch" runat="server" Width="100%">
                                                <table width="100%" class="tableStandard">
                                                    <tr>
                                                        <td class="TitleText">
                                                            Existing Loan
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLCurrencyBox ID="txtSwitchExistingLoan" runat="server" DisplayInputType="Currency"
                                                                onkeyup="disableSubmit()" CssClass="mandatory"></SAHL:SAHLCurrencyBox>
                                                            <SAHL:SAHLLabel ID="lblSwitchExistingLoan" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TitleText">
                                                            + Interim Interest
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblSwitchInterimInterest" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TitleText">
                                                            + Cash Out
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLCurrencyBox ID="txtSwitchCashOut" runat="server" DisplayInputType="Currency"
                                                                onkeyup="disableSubmit()"></SAHL:SAHLCurrencyBox>
                                                            <SAHL:SAHLLabel ID="lblSwitchCashOut" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TitleText">
                                                            + Fees
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblSwitchFees" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TitleText">
                                                            = Total Loan Required
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblSwitchTotalLoanRequired" runat="server" CssClass="LabelText"
                                                                TextAlign="Left">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlNewPurchase" runat="server" Width="100%">
                                                <table width="100%" class="tableStandard">
                                                    <tr>
                                                        <td class="TitleText">
                                                            Purchase Price
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLCurrencyBox ID="txtNewPurchasePurchasePrice" runat="server" DisplayInputType="Currency"
                                                                onkeyup="disableSubmit()" CssClass="mandatory"></SAHL:SAHLCurrencyBox>
                                                            <SAHL:SAHLLabel ID="lblNewPurchasePurchasePrice" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TitleText">
                                                            - Cash Deposit
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLCurrencyBox ID="txtNewPurchaseCashDeposit" runat="server" DisplayInputType="Currency"
                                                                onkeyup="disableSubmit()"></SAHL:SAHLCurrencyBox>
                                                            <SAHL:SAHLLabel ID="lblNewPurchaseCashDeposit" runat="server" CssClass="LabelText"
                                                                TextAlign="Left">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TitleText">
                                                            + Fees
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblNewPurchaseFees" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TitleText">
                                                            = Total Loan Required
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblTotalLoanRequired" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlRefinance" runat="server" Width="100%">
                                                <table width="100%" class="tableStandard">
                                                    <tr>
                                                        <td class="TitleText">
                                                            - Cash Out
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLCurrencyBox ID="txtRefinanceCashOut" runat="server" DisplayInputType="Currency"
                                                                onkeyup="disableSubmit()" CssClass="mandatory"></SAHL:SAHLCurrencyBox>
                                                            <SAHL:SAHLLabel ID="lblRefinanceCashOut" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TitleText">
                                                            + Fees
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblRefinanceFees" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TitleText">
                                                            = Total Loan Required
                                                        </td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblRefinanceTotalLoanRequired" runat="server" CssClass="LabelText"
                                                                TextAlign="Left">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <br />
                                            <asp:Panel ID="pnlLTVPTI" runat="server" Width="100%">
                                                <table width="100%" class="tableStandard">
                                                    <tr>
                                                        <td class="TitleText">
                                                            <SAHL:SAHLLabel ID="lblLTV" runat="server" CssClass="LabelText" Font-Bold="True">-</SAHL:SAHLLabel>
                                                        </td>
                                                        <td class="TitleText">
                                                            <SAHL:SAHLLabel ID="lblPTI" runat="server" CssClass="LabelText" Font-Bold="True">-</SAHL:SAHLLabel>
                                                        </td>
                                                        <td class="TitleText">
                                                            <SAHL:SAHLLabel ID="lblPTIVF" runat="server" CssClass="LabelText" Font-Bold="True"
                                                                Visible="false">-</SAHL:SAHLLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td valign="top">
                                            <table width="100%" class="tableStandard">
                                                <tr runat="server" id="trCancellationFee">
                                                    <td class="TitleText">
                                                        Cancellation Fee
                                                    </td>
                                                    <td>
                                                        <SAHL:SAHLLabel ID="lblCancellationFee" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                                        <SAHL:SAHLCurrencyBox ID="txtCancellationFee" runat="server" DisplayInputType="Currency"
                                                            onkeyup="disableSubmit()"></SAHL:SAHLCurrencyBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TitleText">
                                                        Initiation Fee
                                                    </td>
                                                    <td>
                                                        <SAHL:SAHLLabel ID="lblInitiationFee" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TitleText">
                                                        Registration Fee
                                                    </td>
                                                    <td>
                                                        <SAHL:SAHLLabel ID="lblRegistrationFee" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkCapitaliseFees" runat="server" Text="Capitalise Fees" OnCheckedChanged="chkCapitaliseFees_CheckedChanged"
                                                            AutoPostBack="True" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkOverrideFees" runat="server" Text="Override Fees" OnCheckedChanged="chkOverrideFees_CheckedChanged"
                                                            AutoPostBack="True" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkQuickPayFee" runat="server" Text="Quick Pay Loan" OnCheckedChanged="chkQuickPayFee_CheckedChanged"
                                                            AutoPostBack="True" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkCapitaliseInitiationFees" runat="server" Text="Capitalise Initiation Fees" OnCheckedChanged="chkCapitaliseInitiationFees_CheckedChanged"
                                                            AutoPostBack="True" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <SAHL:SAHLLabel runat="server" ID="lblRateAdjustments" Font-Bold="true" Text="Rate Adjustment">
                                                        </SAHL:SAHLLabel>
                                                    </td>
                                                    <td>
                                                        <SAHL:SAHLDropDownList runat="server" ID="ddlRateAdjustmentElements" CssClass="mandatory">
                                                        </SAHL:SAHLDropDownList>
                                                        <SAHL:SAHLLabel runat="server" ID="lblRateAdjustmentsDisplay"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlApplicationOptions" runat="server" Width="100%" GroupingText="Application Options">
                                                <table width="100%" class="tableStandard">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkQuickCash" runat="server" Text="Quick Cash" OnCheckedChanged="chkQuickCash_CheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkHOC" runat="server" Text="HOC" OnCheckedChanged="chkHOC_CheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkStaffLoan" runat="server" Text="Staff Loan" OnCheckedChanged="chkStaffLoan_CheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkLife" runat="server" Text="Life" OnCheckedChanged="chkLife_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkCAP" runat="server" Text="CAP" OnCheckedChanged="chkCAP_CheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkInterestOnly" runat="server" Text="Interest Only" OnCheckedChanged="chkInterestOnly_CheckedChanged"
                                                                AutoPostBack="True" />
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chkDiscountedLinkRate" runat="server" Text="Discounted Link Rate"
                                                                onclick="discountChange(this)" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" class="tableStandard">
                    <tr>
                        <td style="width: 60%">
                            <cc1:VarifixDetailsPanel ID="VarifixDetails" runat="server" Width="100%" OnOnCalculateMaximumFixedPercentage="VarifixDetails_OnCalculateMaximumFixedPercentage">
                            </cc1:VarifixDetailsPanel>
                            <cc1:LoanDetailsPanel ID="LoanDetails" runat="server" Width="100%" GroupingText="Rate/Instalment details">
                            </cc1:LoanDetailsPanel>
                            <cc1:EdgeDetailsPanel ID="EHLDetails" runat="server" Width="100%" GroupingText="Rate/Instalment details">
                            </cc1:EdgeDetailsPanel>
                        </td>
                        <td valign="top" style="width: 50%">
                            <cc1:SuperLoInfoPanel ID="SuperLoInfo" runat="server" Width="100%">
                            </cc1:SuperLoInfoPanel>
                            <cc1:QuickCashPanel ID="QuickCashDetails" runat="server" Width="100%">
                            </cc1:QuickCashPanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right">
                <SAHL:SAHLButton ID="btnRecalc" runat="server" Text="Recalculate" OnClick="btnRecalc_Click"
                    CssClass="SAHLButton5" ButtonSize="Size4" />
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                    ButtonSize="Size4" />&nbsp;
                <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                    CssClass="SAHLButton5" ButtonSize="Size4" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
