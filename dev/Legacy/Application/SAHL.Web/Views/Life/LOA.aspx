<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="LOA.aspx.cs" Inherits="SAHL.Web.Views.Life.LOA" Title="LOA" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <table style="width: 100%" class="tableStandard">
            <tr>
                <td align="center" colspan="2">
                    <SAHL:SAHLLabel ID="Label1" runat="server" Text="LOA" Font-Bold="True" Font-Names="Arial"
                        Font-Size="Medium" Font-Underline="True" CssClass="LabelText"></SAHL:SAHLLabel>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Panel runat="server" ID="panel1" Width="99%" BorderStyle="Solid" BorderColor="Silver"
                        BorderWidth="1" HorizontalAlign="Left">
                        <table>
                            <tr>
                                <td align="left" style="width: 30px">
                                </td>
                                <td align="left" style="width: 370px">
                                    The total bond amount to be registered is<br />
                                </td>
                                <td style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtRegistrationAmount" runat="server" Width="204px" DisplayInputType="Currency"
                                        ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px">
                                </td>
                                <td style="width: 35px">
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20px">
                                </td>
                                <td align="left" style="width: 370px">
                                    The exact loan amount requested will be<br />
                                </td>
                                <td style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtLoanAgreementAmount" runat="server" Width="204px" DisplayInputType="Currency"
                                        ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px">
                                </td>
                                <td style="width: 35px">
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td style="width: 60px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20px">
                                </td>
                                <td align="left" style="width: 370px">
                                    The cash portion you have requested is (if applicable)<br />
                                </td>
                                <td style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtCashPortion" runat="server" Width="204px" DisplayInputType="Currency"
                                        ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px" align="center">
                                </td>
                                <td style="width: 35px">
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td style="width: 60px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20px">
                                </td>
                                <td align="left" style="width: 370px">
                                    Interim Interest<br />
                                </td>
                                <td style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtInterimInterest" runat="server" Width="204px" DisplayInputType="Currency"
                                        ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px">
                                </td>
                                <td align="center" style="width: 35px">
                                    Var</td>
                                <td style="width: 15px">
                                </td>
                                <td align="left">
                                    Fixed</td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20px">
                                </td>
                                <td align="left" style="width: 370px">
                                    Just to confirm that currently, your monthly instalment will be<br />
                                </td>
                                <td style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtMonthlyInstalment" runat="server" Width="204px" DisplayInputType="Currency"
                                        ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px" align="center">
                                    @</td>
                                <td align="center" style="width: 35px">
                                    <SAHL:SAHLTextBox ID="txtVariable" runat="server" Width="40px" DisplayInputType="Currency"
                                        ReadOnly="True"></SAHL:SAHLTextBox>%</td>
                                <td align="center" style="width: 15px">
                                    &amp;</td>
                                <td align="left">
                                    <SAHL:SAHLTextBox ID="txtFixed" runat="server" Width="40px" DisplayInputType="Currency"
                                        ReadOnly="True"></SAHL:SAHLTextBox>%</td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20px">
                                </td>
                                <td align="left" style="width: 370px">
                                    HOC Sum Assured
                                    <br />
                                </td>
                                <td style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtHOCSumAssured" runat="server" Width="204px" DisplayInputType="Currency"
                                        ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px" align="center">
                                </td>
                                <td style="width: 35px">
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td style="width: 60px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20px">
                                </td>
                                <td align="left" style="width: 370px">
                                    Your monthly HOC Insurance Premium<br />
                                </td>
                                <td style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtHOCPremium" runat="server" Width="204px" DisplayInputType="Currency"
                                        ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px">
                                </td>
                                <td style="width: 35px">
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20px">
                                </td>
                                <td align="left" style="width: 370px">
                                    Total monthly instalment (including the insurance premiums)<br />
                                </td>
                                <td style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtTotalMonthlyInstalment" runat="server" Width="204px" DisplayInputType="Currency"
                                        ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px">
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblMonthlyServiceFee" runat="server" Text="Label"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20px">
                                </td>
                                <td align="left" style="width: 370px">
                                    Over a bond term (remaining) of
                                    <br />
                                </td>
                                <td style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtBondTerm" runat="server" Width="204px" DisplayInputType="Number"
                                        ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px">
                                </td>
                                <td style="width: 35px">
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20px">
                                </td>
                                <td align="left" style="width: 370px">
                                    <asp:Label ID="Label2" runat="server" Text="Is this a VariFix Loan?"></asp:Label></td>
                                <td valign="top" align="left" style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtVariFix" runat="server" Width="20px" ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px">
                                </td>
                                <td style="width: 35px">
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20px">
                                </td>
                                <td align="left" style="width: 370px">
                                    <asp:Label ID="Label5" runat="server" Text="Interest Only ?"></asp:Label></td>
                                <td valign="top" align="left" style="width: 210px">
                                    <SAHL:SAHLTextBox ID="txtInterestOnly" runat="server" Width="20px" ReadOnly="True"></SAHL:SAHLTextBox></td>
                                <td style="width: 15px">
                                </td>
                                <td style="width: 35px">
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr class="backgroundLight">
                <td align="left" width="60%" style="padding-right: 1px; padding-left: 1px; padding-bottom: 1px;
                    padding-top: 1px">
                    <asp:Label ID="Label3" runat="server" Text="Attorney Name : " CssClass="TitleText"></asp:Label>
                    <asp:Label ID="lblAttorneyName" runat="server" Text="-" CssClass="LabelText"></asp:Label>
                </td>
                <td align="right" width="40%" style="padding-right: 1px; padding-left: 1px; padding-bottom: 1px;
                    padding-top: 1px">
                    <asp:Label ID="Label4" runat="server" Text="Loan Consultant : " CssClass="TitleText"></asp:Label>
                    <asp:Label ID="lblLoanConsultant" runat="server" Text="-" CssClass="LabelText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <SAHL:SAHLGridView ID="LoanConditionsGrid" runat="server" AutoGenerateColumns="false"
                        EmptyDataSetMessage="There are no conditions on this Loan." EmptyDataText="There are no conditions on this Loan."
                        EnableViewState="false" FixedHeader="false" GridWidth="100%" HeaderCaption="Loan Conditions"
                        NullDataSetMessage="" Width="100%" GridHeight="60px" ShowHeader="False" OnRowDataBound="LoanConditionsGrid_RowDataBound">
                        <HeaderStyle CssClass="TableHeaderB" />
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
            <tr class="backgroundLight">
                <td align="center" colspan="2">
                    <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" Width="100%" CssClass="TitleText">Please describe any discrepancies in the text box below. The loan dept will be sent an email with the details.</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <SAHL:SAHLTextBox ID="txtMailBody" runat="server" Rows="2" TextMode="MultiLine" Width="100%"></SAHL:SAHLTextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    &nbsp;<SAHL:SAHLButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" SecurityTag="LifeUpdateAccessWorkflow"/></td>
            </tr>
        </table>
    </div>
</asp:Content>
