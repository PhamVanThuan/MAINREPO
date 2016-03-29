<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="DisabilityClaimDetails.aspx.cs" Inherits="SAHL.Web.Views.Life.DisabilityClaimDetails" Title="Disability Claim Details" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: center">
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
                    <td align="left" class="TitleText">Claim Status</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblClaimStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp</td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Date of Diagnosis</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblDateOfDiagnosis" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                        <SAHL:SAHLDateBox ID="dtDateOfDiagnosis" runat="server" TabIndex="1" CssClass="mandatory" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Nature of the Disability</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblDisabilityType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                        <SAHL:SAHLDropDownList ID="ddlDisabilityType" runat="server" CssClass="mandatory" TabIndex="2"></SAHL:SAHLDropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Additional Details of the Disability</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblOtherDisabilityComments" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                        <SAHL:SAHLTextBox ID="txtOtherDisabilityComments" runat="server" Width="100%" TabIndex="3" TextMode="MultiLine" Rows="3" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp</td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Occupation</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblClaimantOccupation" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                        <SAHL:SAHLTextBox ID="txtClaimantOccupation" runat="server" TabIndex="4" CssClass="mandatory" Width="100%"/>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Last Date Worked</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblLastDateWorked" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                        <SAHL:SAHLDateBox ID="dtLastDateWorked" runat="server" TabIndex="5" CssClass="mandatory" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Expected Return to Work Date</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblExpectedReturnToWorkDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                        <SAHL:SAHLDateBox ID="dtExpectedReturnToWorkDate" runat="server" TabIndex="6"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlPaymentDetails" runat="server" GroupingText="Payment Details" Width="100%" Visible="false">
            <table style="width: 100%;" class="tableStandard">
                <tr>
                    <td align="left" class="TitleText" style="width: 25%">Claim Approved Date</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblPaymentApprovedDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Approved By</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblApprovedBy" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Instalments Authorised</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblNumberOfPayments" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Payment Start Date</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblPaymentStartDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Payment End Date</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblPaymentEndDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlButtons" runat="server" Width="100%">
            <table width="100%" class="tableStandard">
                <tr>
                    <td align="right">
                        <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
