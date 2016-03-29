<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="DisabilityClaimCreate.aspx.cs" Inherits="SAHL.Web.Views.Life.DisabilityClaimCreate" Title="Disability Claim Create" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: center">
        <asp:Panel ID="pnlPolicyDetails" runat="server" GroupingText="Life Policy &amp; Loan Details" Width="100%">
            <table id="tPolicyDetails" border="0" style="width: 100%; vertical-align: top" class="tableStandard">
                <tr>
                    <td align="left" class="TitleText" style="width: 50%; vertical-align: top">
                        <table style="width: 100%;">
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">Policy Number</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel CssClass="LabelText" ID="lblPolicyNumber" runat="server">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">Policy Type</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblPolicyType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText" align="left" style="width: 50%">Policy Status</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblPolicyStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">Accepted Date</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblDateOfAcceptance" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">Commencement Date</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblDateOfCommencement" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">&nbsp;</td>
                                <td align="left" style="width: 50%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">Current Sum Assured</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblCurrentSumAssured" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="height: 28px">Reassured IPB Amount</td>
                                <td style="width: 50%; text-align: left; height: 28px;">
                                    <SAHL:SAHLLabel ID="lblReassuredIPBAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="left" class="TitleText" style="width: 50%; vertical-align: top">
                        <table>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">Loan Number</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblLoanNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">Loan Status</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblLoanStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%;">Loan Term</td>
                                <td align="left" style="width: 50%;">
                                    <SAHL:SAHLLabel ID="lblLoanTerm" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">Loan Amount</td>
                                <td align="left" style="width: 50%">
                                    <SAHL:SAHLLabel ID="lblLoanAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%" valign="top">Loan Debit Order Day</td>
                                <td align="left" style="width: 50%" valign="top">
                                    <SAHL:SAHLLabel ID="lblLoanDebitOrderDay" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" style="width: 50%">&nbsp;</td>
                                <td align="left" style="width: 50%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">Outstanding Bond Amount</td>
                                <td style="width: 50%; text-align: left">
                                    <SAHL:SAHLLabel ID="lblOutstandingBondAmount" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">Bond Instalment</td>
                                <td style="width: 50%; text-align: left;">
                                    <SAHL:SAHLLabel ID="lblBondInstalment" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <asp:Panel ID="pnlAssuredLivesGrid" runat="server" Width="100%">
            <table id="tAssuredLivesGrid" border="0" cellpadding="4" cellspacing="0" width="100%">
                <tr>
                    <td align="center" style="text-align: left">
                        <cc1:LegalEntityGrid ID="LegalEntityGrid" runat="server">
                        </cc1:LegalEntityGrid>
                        <br />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />

        <asp:Panel ID="pnlButtons" runat="server" Width="100%">
            <table width="100%" class="tableStandard">
                <tr>
                    <td style="width: 175px"></td>
                    <td></td>
                </tr>
                <tr id="ButtonRow" runat="server">
                    <td align="left" class="TitleText" style="width: 175px">
                        <SAHL:SAHLLabel ID="lblClaimant" runat="server" CssClass="LabelText">Select Claimant    </SAHL:SAHLLabel>
                    </td>
                    <td align="left" class="TitleText">

                        <SAHL:SAHLDropDownList ID="ddlClaimant" runat="server" CssClass="CboText">
                        </SAHL:SAHLDropDownList>
                    </td>
                    <td align="right">
                        <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Create Claim" AccessKey="S" OnClick="btnSubmit_Click" ButtonSize="Size5" Visible="True" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
    </div>
</asp:Content>