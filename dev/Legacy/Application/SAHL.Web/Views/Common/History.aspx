<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="Views_Life_History" Title="History" Codebehind="History.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
<!-- Need to ReAdd Workflow Header Controls for WorkFlow related screens -->
    <table border="0" cellspacing="0" cellpadding="0" class="PageBlock" align="left"
        width="790">
        <tr>
            <td>
                <SAHL:SAHLGridView ID="gridHistory" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                    EnableViewState="false" GridHeight="200px" GridWidth="100%" Width="790px" HeaderCaption="Policy History"
                    NullDataSetMessage="" EmptyDataSetMessage="There are no history entries for this policy."
                    PostBackType="NoneWithClientSelect" OnRowDataBound="gridHistory_RowDataBound">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLGridView ID="gridPremiumHistory" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="200px" GridWidth="100%"
                    Width="790px" HeaderCaption="Premium History" NullDataSetMessage="" EmptyDataSetMessage="There are no premium history entries for this policy."
                    PostBackType="NoneWithClientSelect" OnRowDataBound="gridPremiumHistory_RowDataBound">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="center">
                <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                    CausesValidation="False" />
            </td>
        </tr>
    </table>
</asp:Content>
