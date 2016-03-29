<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.FutureDatedTransactions" Title="Untitled Page"
    CodeBehind="FutureDatedTransactions.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script type="text/javascript" language="javascript">
        function confirmAccountReference() {
            var accountNo = document.getElementById('<%= hidAccountNo.ClientID %>').value;
            var reference = document.getElementById('<%= txtReferenceUpdate.ClientID %>').value;

            if (reference.trim() != accountNo) {
                return confirm('The reference field is not the same as the account number (' + accountNo + ').\n\nAre you sure you want to continue?');
            }
            return true;
        }
    </script>
    <table style="width: 98%; height: 100%" runat="server">
        <tr style="width: 100%; height: 100%" runat="server">
            <td style="width: 100%; height: 100%" runat="server">
                <SAHL:SAHLGridView ID="RecordsGrid" runat="server" AutoGenerateColumns="False" FixedHeader="false"
                    EnableViewState="false" GridWidth="100%" GridHeight="200px" SelectFirstRow="True"
                    HeaderCaption="Manual Debit Orders" PostBackType="SingleClick" NullDataSetMessage="No manual debit orders found."
                    EmptyDataSetMessage="No manual debit orders found." OnSelectedIndexChanged="RecordsGrid_SelectedIndexChanged"
                    OnRowDataBound="RecordsGrid_RowDataBound" Width="99%">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
    </table>
    <table border="0" id="DisplayData" runat="server" class="tableStandard" width="99%">
        <tr runat="server">
            <td style="width: 15%" runat="server">&nbsp;
            </td>
            <td style="width: 80%" runat="server">
            </td>
            <td style="width: 5%" runat="server">
            </td>
        </tr>
        <tr id="ArrearBalanceRow" runat="server" visible="false">
            <td class="TitleText">
                Arrear Balance
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblArrearBalance" runat="server">-</SAHL:SAHLLabel>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="width: 160px;">
                Bank
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblBank" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                <SAHL:SAHLDropDownList ID="ddlBankUpdate" runat="server" Mandatory="true" Width="620px">
                </SAHL:SAHLDropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Effective Date
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblEffectiveDate" runat="server">-</SAHL:SAHLLabel>
                <SAHL:SAHLDateBox ID="dateEffectiveDateUpdate" runat="server" Mandatory="true" />
            </td>
            <td>
                <SAHL:SAHLTextBox ID="ValEffectiveDateUpdateControl" runat="server" Style="display: none;"
                    Width="100%"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Amount
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblAmount" runat="server">-</SAHL:SAHLLabel>
                <SAHL:SAHLCurrencyBox ID="txtAmountUpdate" runat="server" Mandatory="true" Text="0.00" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Reference
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblReference" runat="server">-</SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtReferenceUpdate" runat="server" Mandatory="true"></SAHL:SAHLTextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr runat="server" id="trNoOfPayments">
            <td class="TitleText">
                No. of Payments
            </td>
            <td>
                <SAHL:SAHLTextBox ID="txtNoOfPayments" DisplayInputType="Number" runat="server" Mandatory="true" Width="40px"></SAHL:SAHLTextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr style="width: 99%">
            <td class="TitleText" valign="top">
                Note
            </td>
            <td style="width: 99%">
                <asp:Panel ID="NotePanel" runat="server" Style="height: 40px; width: 99%; overflow-y: scroll;
                    border: 1px solid #E5E5E5;">
                    <SAHL:SAHLLabel ID="lblNote" runat="server">-</SAHL:SAHLLabel>
                </asp:Panel>
                <SAHL:SAHLTextBox ID="txtNoteUpdate" runat="server" TextMode="MultiLine" Rows="3"
                    Width="99%" EnableViewState="False" onkeyup="limitTextLength(this, 250, 'divNotesCount')"></SAHL:SAHLTextBox>
                <div id="divNotesCount" style="text-align: right;">
                </div>
            </td>
            <td valign="top">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" id="tdRequestedBy">
                Requested By
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblRequestedBy" runat="server">-</SAHL:SAHLLabel>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" id="tdProcessedBy">
                Processed By
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblProcessedBy" runat="server">-</SAHL:SAHLLabel>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="tblGridPrv" runat="server" visible="false" style="width: 98%; height: 100%">
        <tr style="width: 100%; height: 100%" runat="server">
            <td style="width: 100%; height: 100%" runat="server">
                <SAHL:SAHLGridView ID="RecordsGridPrv" runat="server" AutoGenerateColumns="False"
                    FixedHeader="false" EnableViewState="false" GridWidth="100%" GridHeight="200px"
                    HeaderCaption="Previous Manual Debit Orders" NullDataSetMessage="No previous manual debit orders found."
                    EmptyDataSetMessage="No previous manual debit orders found." Width="99%" OnRowDataBound="RecordsGridPrv_RowDataBound">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
    </table>
    <br />
    <div class="buttonBar" style="width: 99%; margin-top: 5px;">
        <table>
            <tr>
                <td style="width: 95%" align="right" nowrap="nowrap">
                    <SAHL:SAHLButton ID="btnAdd" runat="server" Text=" Add " AccessKey="A" OnClick="btnAdd_Click"
                        Visible="false" OnClientClick="return confirmAccountReference();" />
                    <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" AccessKey="U" OnClick="btnUpdate_Click"
                        Visible="false" OnClientClick="return confirmAccountReference();" />
                    <SAHL:SAHLButton ID="btnDelete" runat="server" Text="Delete" AccessKey="D" OnClick="btnDelete_Click"
                        Visible="false" OnClientClick="return confirm('Are you sure you want to delete the selected item?')" />
                    <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                        CausesValidation="False" />
                </td>
                <td style="width: 5%">
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hidAccountNo" runat="server" Value="" />
</asp:Content>
