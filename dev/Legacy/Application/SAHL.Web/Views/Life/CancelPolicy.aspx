<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Life.CancelPolicy" Title="Cancel Policy" Codebehind="CancelPolicy.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div style="text-align: center">
        <table style="width: 100%" class="tableStandard">
            <tr>
                <td align="center" colspan="2">
                    <SAHL:SAHLLabel ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        Font-Underline="True" Text="CANCEL POLICY" CssClass="LabelText"></SAHL:SAHLLabel><br />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 200px" class="TitleText">
                    Days since Accepted Date</td>
                <td align="left" style="width: 75%" class="cellDisplay">
                    <SAHL:SAHLLabel ID="lblDays" runat="server" CssClass="LabelText"></SAHL:SAHLLabel>
                </td>
            </tr>
            <tr>
                <td align="left" class="TitleText">
                    Policy has commenced
                </td>
                <td align="left" class="cellDisplay">
                    <SAHL:SAHLLabel ID="lblCommenced" runat="server" CssClass="LabelText"></SAHL:SAHLLabel>
                </td>
            </tr>
            <tr>
                <td align="left" class="TitleText">
                    Life is Condition of Loan
                </td>
                <td align="left" class="cellDisplay">
                    <SAHL:SAHLLabel ID="lblCondition" runat="server" CssClass="LabelText"></SAHL:SAHLLabel>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="cellDisplay">
                </td>
            </tr>
            <tr>
                <td align="left" class="TitleText">
                    Cancellation Letter Received
                </td>
                <td align="left" class="cellDisplay">
                    <asp:CheckBox ID="chkLetter" runat="server" Text="" TextAlign="Right" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2" class="cellDisplay">
                </td>
            </tr>
            <tr>
                <td align="left" class="TitleText">
                    Cancellation Type
                </td>
                <td align="left" class="cellDisplay">
                    <asp:RadioButtonList ID="rblType" runat="server" BorderStyle="Solid" BorderWidth="1px"
                        Height="112px">
                    </asp:RadioButtonList>
                </td>
            </tr>
                        <tr>
                <td colspan="2" class="cellDisplay">
                </td>
            </tr>
            <tr>
                <td align="left" class="TitleText">
                    Cancellation Reason
                </td>
                <td align="left" class="cellDisplay">
                    <SAHL:SAHLDropDownList ID="ddlReason" runat="server">
                    </SAHL:SAHLDropDownList>
                </td>
            </tr>
            <tr>
                <td align="left">
                </td>
                <td align="left" class="cellDisplay">
                    <asp:Label ID="lblError" ForeColor="Red" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                </td>
                <td align="left">
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click">
                    </SAHL:SAHLButton>
                    <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
