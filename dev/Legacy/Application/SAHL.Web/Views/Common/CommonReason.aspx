<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.CommonReason" Title="Untitled Page" EnableEventValidation="false"
    CodeBehind="CommonReason.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:Panel ID="pnlHistory" runat="server" Width="100%" GroupingText="Credit Decline History"
        Visible="False">
        <br />
        &nbsp;<SAHL:SAHLGridView ID="gridHistory" runat="server" FixedHeader="False" GridHeight=""
            GridWidth="100%" Invisible="False" SelectFirstRow="True" TotalsFooter="True"
            Width="100%" AutoGenerateColumns="False" OnRowDataBound="gridHistory_RowDataBound">
        </SAHL:SAHLGridView>
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlReasons" runat="server" GroupingText="Credit Decline Reasons" Width="100%"
        Visible="False">
        <br />
        <asp:ListBox ID="lstCreditDeclineReasons" runat="server" Height="200px" Width="50%"
            onchange="lstSummarySelectedReasonIndexChanged()"></asp:ListBox>
    </asp:Panel>
    <br />
    &nbsp;
    <br />
    <asp:Panel ID="pnlUpdateReasons" runat="server" GroupingText="Update Reasons" Width="100%">
        <table style="width: 99%">
            <tr>
                <td align="left" style="width: 45%">
                    <SAHL:SAHLLabel ID="lblReasonType" runat="server" CssClass="LabelText" Font-Bold="True"
                        Width="107px">Reason Type</SAHL:SAHLLabel>
                </td>
                <td style="width: 10%">
                </td>
                <td style="width: 45%">
                </td>
                <td style="width: 11px">
                </td>
            </tr>
            <tr>
                <td>
                    <select id="cbxReasonType" runat="server" style="width: 280px" onchange="cbxReasonSelectedIndexChanged()">
                        <option selected="selected"></option>
                    </select>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td style="width: 11px">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td style="width: 11px">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <SAHL:SAHLLabel ID="SAHLLabel1" runat="server">Available Reasons</SAHL:SAHLLabel>
                </td>
                <td>
                </td>
                <td align="center">
                    <SAHL:SAHLLabel ID="lblSelected" runat="server" CssClass="LabelText">Selected Reasons</SAHL:SAHLLabel>
                </td>
                <td align="center" style="width: 11px">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListBox ID="lstAvailableReasons" runat="server" Height="192px" Width="100%"
                        onclick="lstAvailableReasonIndexChanged()"></asp:ListBox>
                </td>
                <td valign="middle">
                    <div style="width: 70%; vertical-align: middle; margin-left: auto; margin-right: auto;">
                        <br />
                        <SAHL:SAHLButton ID="btnAdd" Width="90%" runat="server" Text=">>" CausesValidation="False"
                            EnableViewState="False" OnClientClick="addItem();return false" UseSubmitBehavior="False" /><br />
                        <br />
                        <SAHL:SAHLButton ID="btnRemove" Width="90%" runat="server" OnClientClick="removeItem();return false"
                            Text="<<" UseSubmitBehavior="False" /><br />
                    </div>
                </td>
                <td>
                    <asp:ListBox ID="lstSelectedReasons" runat="server" Height="192px" Width="100%" onclick="lstSelectedReasonIndexChanged()">
                    </asp:ListBox>
                </td>
                <td style="width: 11px">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td style="width: 11px">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <SAHL:SAHLLabel ID="lblComment" runat="server" CssClass="LabelText" Font-Bold="True"
                        Width="107px" Enabled="False" TextAlign="Left">Comment</SAHL:SAHLLabel>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td style="width: 11px">
                </td>
            </tr>
            <tr>
                <td colspan="3" rowspan="4">
                    <div id="divReason" style="width: 100%; height: 64px">
                        <SAHL:SAHLTextBox ID="txtComment" runat="server" Height="64px" TextMode="MultiLine"
                            Width="99%" ReadOnly="True"></SAHL:SAHLTextBox></div>
                </td>
                <td colspan="1" rowspan="4" style="width: 11px">
                </td>
            </tr>
        </table>
        &nbsp;
    </asp:Panel>
    <asp:Panel ID="pnlMemo" runat="server" GroupingText="Memo" Width="100%" HorizontalAlign="Left">
        &nbsp;<SAHL:SAHLTextBox ID="txtMemo" runat="server" Height="64px" TextMode="MultiLine"
            Width="99%"></SAHL:SAHLTextBox></asp:Panel>
    <asp:Panel ID="pnlSubmit" runat="server" Width="100%">
        <br />
        <table id="tblConfirm" width="100%">
            <tr>
                <td align="right">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                        EnableViewState="False" OnClick="btnCancel_Click" />
                    <SAHL:SAHLButton ID="btnConfirm" runat="server" Text="Submit" OnClientClick="btnConfirmClick()"
                        CausesValidation="False" EnableViewState="False" OnClick="btnConfirm_Click" />
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
