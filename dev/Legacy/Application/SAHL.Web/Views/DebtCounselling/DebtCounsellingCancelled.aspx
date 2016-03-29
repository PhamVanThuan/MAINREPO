<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.master"
    CodeBehind="DebtCounsellingCancelled.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.DebtCounsellingCancelled" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <br />
    &nbsp;
    <br />
    <asp:Panel ID="pnlUpdateReasons" runat="server" Width="100%">
        <table>
            <tr>
                <td>
                    <SAHL:SAHLLabel ID="lblReason" runat="server" CssClass="LabelText" Font-Bold="True"
                        Width="140px">Cancellation Reason</SAHL:SAHLLabel>
                </td>
                <td>
                    <SAHL:SAHLDropDownList ID="ddlReason" runat="server">
                    </SAHL:SAHLDropDownList>
                </td>
            </tr>
        </table>
        <br />
        <div style="text-align: left">
            <table width="100%" class="tableStandard">
                <tr>
                    <td>
                        <SAHL:SAHLGridView ID="gvLinkedAccounts" runat="server" AutoGenerateColumns="False"
                            FixedHeader="False" GridHeight="150" GridWidth="100%" Width="100%" HeaderCaption="Linked Accounts"
                            PostBackType="NoneWithClientSelect" EmptyDataSetMessage="No linked accounts exist for this debt counselling case."
                            OnRowDataBound="gvLinkedAccounts_RowDataBound" Invisible="False" SelectFirstRow="True"
                            EmptyDataText="No linked accounts exist for this debt counselling case.">
                            <RowStyle CssClass="TableRowA" />
                        </SAHL:SAHLGridView>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSubmit" runat="server" Width="100%">
        <br />
        <table id="tblConfirm" width="100%">
            <tr>
                <td align="right">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" EnableViewState="False" />
                    <SAHL:SAHLButton ID="btnConfirm" runat="server" Text="Submit" CausesValidation="False" EnableViewState="False" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    &nbsp;<asp:HiddenField ID="hiddenComments" runat="server" />
    <asp:HiddenField ID="hiddenSelection" runat="server" />
    <asp:HiddenField ID="HiddenInd" runat="server" Value="0" />
    <asp:HiddenField ID="HiddenSelection2" runat="server" />
</asp:Content>
