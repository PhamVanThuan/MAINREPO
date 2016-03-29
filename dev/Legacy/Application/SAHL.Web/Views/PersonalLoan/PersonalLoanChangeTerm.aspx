<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="PersonalLoanChangeTerm.aspx.cs" Inherits="SAHL.Web.Views.PersonalLoan.PersonalLoanChangeTerm" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="100%" class="TableStandard">
        <tr>
            <td align="left" style="height: 99%;" valign="top">
                <SAHL:SAHLGridView ID="PersonalLoansGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                    EnableViewState="false" GridHeight="200px" GridWidth="100%" Width="100%" HeaderCaption="Personal Loan Details"
                    NullDataSetMessage="" EmptyDataSetMessage="There are no Personal Loans."
                    OnRowDataBound="PersonalLoans_RowDataBound">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
                <table border="0" id="TermTable1" runat="server">
                    <tr>
                        <td style="width: 166px; height: 26px;" class="TitleText">New Remaining Term
                        </td>
                        <td style="height: 26px">
                            <SAHL:SAHLTextBox ID="txtNewRemainingTerm" runat="server" CausesValidation="True" DisplayInputType="Number"
                                FormatDecimalPlace="0" EnableTheming="True" Mandatory="True" MaxLength="3"></SAHL:SAHLTextBox>&nbsp;
                        </td>
                        <td style="height: 26px;">
                            <SAHL:SAHLLabel ID="lblMaxTerm" runat="server"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                </table>
                <table class="tableStandard" border="0" id="TermTable2" runat="server">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlNewInstalment" runat="server" GroupingText="Instalment Breakdown">
                                <table border="0">
                                    <tr>
                                        <td class="TitleText" style="width: 200px;" >New Loan Instalment
                                        </td>
                                        <td class="cellDisplay" style="width: 150px;text-align:right">
                                            <SAHL:SAHLLabel ID="lblNewInstalment" runat="server" >-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trCreditLifePremium">
                                        <td class="TitleText">Credit Life Premium
                                        </td>
                                        <td class="cellDisplay" style="width: 150px;text-align:right">
                                            <SAHL:SAHLLabel ID="lblCreditLifePremium" runat="server">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trMonthlyServiceFee">
                                        <td class="TitleText" >Monthly Service Fee
                                        </td>
                                        <td class="cellDisplay" style="width: 150px;text-align:right">
                                            <SAHL:SAHLLabel ID="lblMonthlyServiceFee" runat="server">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trTotalInstalment">
                                        <td class="TitleText" >Total Instalment
                                        </td>
                                        <td class="cellDisplay" style="width: 150px;text-align:right">
                                            <SAHL:SAHLLabel ID="lblTotalInstalment" runat="server" Font-Bold="true">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>

                    </tr>
                </table>

                <table border="0" id="TermCommentsPanel" runat="server">
                    <tr>
                        <td style="width: 100px">
                            <asp:Panel ID="pnlComments" runat="server" GroupingText="Comments" Wrap="False"
                                Width="373px">
                                <table border="0" style="width: 155px">
                                    <tr>
                                        <td style="width: 37px; height: 67px;">
                                            <SAHL:SAHLTextBox ID="txtComments" runat="server" CausesValidation="True" EnableTheming="True"
                                                FormatDecimalPlace="0" Height="44px" Width="350px" TextMode="MultiLine" 
                                                Mandatory="True"></SAHL:SAHLTextBox></td>
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
                <SAHL:SAHLButton ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" />
                <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Process Term Change" ButtonSize="Size5"
                    AccessKey="T" OnClick="btnSubmit_Click" />
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click"
                    CausesValidation="False" />
            </td>
        </tr>
    </table>
</asp:Content>

