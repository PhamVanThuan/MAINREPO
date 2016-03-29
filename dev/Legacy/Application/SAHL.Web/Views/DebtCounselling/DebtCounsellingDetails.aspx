<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.Master"
    CodeBehind="DebtCounsellingDetails.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.DebtCounsellingDetails" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: center">
        <table width="100%" class="tableStandard">
            <tr>
                <td align="center" class="TableHeaderA" colspan="4">
                    Update Debt Counselling Details
                </td>
            </tr>
        </table>
        <table border="0" class="tableStandard">
            <tr>
                <td>
                    <br />
                     <SAHL:SAHLLabel Font-Bold="true" ID="ReferenceNoTitle" runat="server" Text="Reference Number:"></SAHL:SAHLLabel>
                </td>
                <td>
                    <br />
                    <SAHL:SAHLTextBox ID="txtReferenceNo" runat="server"></SAHL:SAHLTextBox>
                </td>
            </tr>
            <tr id="ButtonRow" runat="server">
                <td></td>
                <td align="right">
                    <br />
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit"
                        CausesValidation="False" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
