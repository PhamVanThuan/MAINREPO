<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="DisabilityClaimHistoryDetails.aspx.cs" Inherits="SAHL.Web.Views.Life.DisabilityClaimHistoryDetails" Title="Disability Claim History Details" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: center">
        <asp:Panel ID="pnlDisabilityClaimsHistory" runat="server" Width="100%">
            <table id="tDisabilityClaimsHistory" border="0" cellpadding="4" cellspacing="0" width="100%">
                <tr>
                    <td align="center" style="text-align: left">
                        <SAHL:SAHLGridView ID="gridDisabilityClaims" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                            EnableViewState="false" GridHeight="100px" GridWidth="100%" Width="100%" PostBackType="SingleClick" SelectFirstRow="true"
                            HeaderCaption="Disability Claims" NullDataSetMessage="" EmptyDataSetMessage="There are no disability claim records."
                            OnSelectedIndexChanged="gridDisabilityClaims_OnSelectedIndexChanged" OnRowDataBound="gridDisabilityClaims_OnRowDataBound">
                            <RowStyle CssClass="TableRowA" />
                        </SAHL:SAHLGridView>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlDisabilityClaimDetails" runat="server" GroupingText="Disability Claim Details" Width="100%">
            <table style="width: 100%;" class="tableStandard">
                <tr>
                    <td align="left" class="TitleText" style="width: 25%">Claimant</td>
                    <td align="left" style="width: 25%">
                        <SAHL:SAHLLabel ID="lblClaimant" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                    <td style="width: 50%"></td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Date Claim Received</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblDateClaimReceived" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp</td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Nature of the Disability</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblDisabilityType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Date of Diagnosis</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblDateOfDiagnosis" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Additional Details of the Disability</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblOtherDisabilityComments" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp</td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Occupation</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblClaimantOccupation" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Last Date Worked</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblLastDateWorked" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Expected Return to Work Date</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblExpectedReturnToWorkDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Status</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblClaimStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlAcceptedClaim" runat="server" GroupingText="Approved" Width="100%" Visible="false">
            <table style="width: 100%;" class="tableStandard">
                <tr>
                    <td align="left" class="TitleText">Claim Decision Date</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblApprovedDecisionDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Approved By</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblApprovedBy" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText" style="width: 25%">Payment Start Date</td>
                    <td align="left" style="width: 25%">
                        <SAHL:SAHLLabel ID="lblPaymentStartDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                    <td style="width: 50%"></td>
                </tr>
                <tr>
                    <td align="left" class="TitleText" style="width: 25%">Instalments Authorised</td>
                    <td align="left" style="width: 25%">
                        <SAHL:SAHLLabel ID="lblNumberOfInstalmentsAuthorised" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                    <td style="width: 50%"></td>
                </tr>
                <tr>
                    <td align="left" class="TitleText" style="width: 25%">Payment End Date</td>
                    <td align="left" style="width: 25%">
                        <SAHL:SAHLLabel ID="lblPaymentEndDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                    <td style="width: 50%"></td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlDeclinedClaim" runat="server" GroupingText="Repudiated" Width="100%" Visible="false">
            <table style="width: 100%;" class="tableStandard">
                <tr>
                    <td align="left" class="TitleText" style="width: 25%">Claim Decision Date</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblRepudiatedDecisionDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText" style="width: 25%">Repudiated By</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblRepudiatedBy" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>              
            </table>
             <SAHL:SAHLGridView ID="gridRepudiatedReasons" runat="server" AutoGenerateColumns="false" 
                            EnableViewState="false" GridHeight="100%" GridWidth="100%" Width="100%" PostBackType="None" 
                            NullDataSetMessage="" EmptyDataSetMessage="There are no reason records."
                            Enabled="false" HeaderStyle-BackColor="#acacac" HeaderStyle-ForeColor="White"
                            HeaderCaption="text" HeaderVisible="false" HeaderStyle-HorizontalAlign="Left">
                            <RowStyle CssClass="TableRowA" />
            </SAHL:SAHLGridView>
        </asp:Panel>
        <asp:Panel ID="pnlTerminated" runat="server" GroupingText="Terminated" Width="100%" Visible="false">
            <table style="width: 100%;" class="tableStandard">
                <tr>
                    <td align="left" class="TitleText"  style="width: 25%">Claim Decision Date</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblTerminatedDecisionDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText"  style="width: 25%">Terminated By</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblTerminatedBy" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
            </table>
            <SAHL:SAHLGridView ID="gridTerminatedReasons" runat="server" AutoGenerateColumns="false" visible="false"
                            EnableViewState="false" GridHeight="100%" GridWidth="100%" Width="100%" PostBackType="None" 
                            NullDataSetMessage="" EmptyDataSetMessage=""
                            Enabled="false" HeaderStyle-BackColor="#acacac" HeaderStyle-ForeColor="White"
                            HeaderCaption="text" HeaderVisible="false" HeaderStyle-HorizontalAlign="Left">                    
            </SAHL:SAHLGridView>
        </asp:Panel>
        <asp:Panel ID="pnlSettled" runat="server" GroupingText="Settled" Width="100%" Visible="false">
            <table style="width: 100%;" class="tableStandard">
                <tr>
                    <td align="left" class="TitleText">Settled Date</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblSettledDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Settled By</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblSettledBy" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
