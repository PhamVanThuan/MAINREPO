<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.DeclineReasonsHistory" Title="Decline Reasons History" Codebehind="DeclineReasonsHistory.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table class="tableStandard" width="100%">
<tr><td align="left" valign="top" >

    <table  id="InfoTable" class="tableStandard" runat="server" width="100%" >
        <tr>
            <td style="width: 100%">
                <asp:Panel ID="Panel1" runat="server" GroupingText="Application Revision History" Width="100%">
                    <SAHL:SAHLGridView ID="grdRevisionHistory" runat="server" AutoGenerateColumns="false"
                    EmptyDataSetMessage="There are no revisions" EnableViewState="false" FixedHeader="false"
                    GridHeight="120px" GridWidth="100%"
                    NullDataSetMessage="" Width="100%" OnSelectedIndexChanged="grdRevisionHistory_SelectedIndexChanged" PostBackType="SingleClick">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                </asp:Panel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
             <asp:Panel ID="panelOI" runat="server" GroupingText="Offer Information" width="100%">
                    <br />
                    <table width="100%">
                        <tr>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblHOCInsurer" runat="server" CssClass="LabelText" Font-Bold="True" Width="100%">Total Loan Required</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblTotalLoanRequired" runat="server" CssClass="LabelText"
                                    Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblHOCConstruction" runat="server" CssClass="LabelText" Font-Bold="True" Width="100%">LTV</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblLTV" runat="server" CssClass="LabelText"
                                    Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblHOCPolicyNumber" runat="server" CssClass="LabelText" Font-Bold="True"
                                    TextAlign="Left" Width="30%">Term</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblTerm" runat="server" CssClass="LabelText"
                                    Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td >
                                <SAHL:SAHLLabel ID="lblHOCSubsidence" runat="server" CssClass="LabelText" Font-Bold="True" Width="100%">PTI</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblPTI" runat="server" CssClass="LabelText"
                                    Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td>
                                <SAHL:SAHLLabel ID="lblHOCMonthlyPremium" runat="server" CssClass="LabelText" Font-Bold="True" Width="100%">Link Rate</SAHL:SAHLLabel></td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblLinkRate" runat="server" CssClass="LabelText"
                                    Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblHOCCeded" runat="server" CssClass="LabelText" Font-Bold="True" Width="100%">Total Instalment</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblTotalInstallment" runat="server" CssClass="LabelText"
                                    Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblHOCRoofType" runat="server" CssClass="LabelText" Font-Bold="True" Width="100%">Effective Rate</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblEffectiveRate" runat="server" CssClass="LabelText"
                                    Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" CssClass="LabelText" Font-Bold="True" Width="100%">Household Income</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblHouseHoldIncome" runat="server" CssClass="LabelText"
                                    Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText" Font-Bold="True" Width="100%">Bond to Register</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblBondToRegister" runat="server" CssClass="LabelText"
                                    Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="SAHLLabel3" runat="server" CssClass="LabelText" Font-Bold="True" Width="100%">Category</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblCategory" runat="server" CssClass="LabelText"
                                    Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td class="cellDisplay">
                            </td>
                            <td>
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="SAHLLabel4" runat="server" CssClass="LabelText" Font-Bold="True" Width="100%">SPV Name</SAHL:SAHLLabel></td>
                            <td>
                                <SAHL:SAHLLabel ID="lblSPVName" runat="server" CssClass="LabelText" Font-Bold="False" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
            </td>
        </tr>
        <tr>
            <td width="100%" >
                <br />
                <asp:Panel ID="Panel2" runat="server" GroupingText="Decline Reasons"
                    Width="100%">
                <SAHL:SAHLGridView ID="grdDeclineReasons" runat="server" AutoGenerateColumns="False"
                    EmptyDataSetMessage="There are no decline reasons for this revision." EnableViewState="false" FixedHeader="false" GridHeight="100px" GridWidth="100%" NullDataSetMessage="There are no decline reasons for this revision." Width="100%" EmptyDataText="There are no decline reasons for this revision." HorizontalAlign="Left" >
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                </asp:Panel>
            </td>
        </tr>
    </table>

</td>
</tr>
</table>
</asp:Content>