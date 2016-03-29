<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"  CodeBehind="PersonalLoanApplicationDeclaration.aspx.cs" Inherits="SAHL.Web.Views.PersonalLoan.PersonalLoanApplicationDeclaration" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" class="tableStandard">
        <tr>
            <td align="left" style="height: 99%; width: 80%;" valign="top">
                <asp:Panel ID="pnlDeclarations" runat="server" Font-Bold="true">
                    <asp:Table ID="tblDeclarations" runat="server" BackColor="White" Width="100%">
                    </asp:Table>
                </asp:Panel>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="left" style="width: 20%">
                <table style="width: 99%">
                    <tr>
                        <td style="width: 15%">
                        </td>
                        <td style="width: 30%">
                        </td>
                        <td style="width: 55%">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%" align="left" valign="bottom">
                            <SAHL:SAHLButton ID="btnBack" runat="server" ButtonSize="Size4" CausesValidation="false"
                                CssClass="BtnNormal4" OnClick="Back_Click" Text="Back" Visible="False" /></td>
                        <td style="width: 20%">
                        </td>
                        <td style="width: 65%" align="right" valign="bottom">
                            <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="Cancel_Click" />
                            <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" AccessKey="U" OnClick="Update_Click" /></td>
                    </tr>
                </table>
                <br />
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
