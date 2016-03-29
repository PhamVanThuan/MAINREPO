<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="AttorneyInvoice.aspx.cs" Inherits="SAHL.Web.Views.Administration.AttorneyInvoice" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        //Disable Add Button, hide grid on LoanNumber Change
        function resetDisplay() {
            $("#<%= btnAdd.ClientID %>").attr("disabled", true);
            $("#<%= pnlInvoiceDetail.ClientID %>").hide();
            $("#<%= btnDelete.ClientID %>").attr("disabled", true);
        }
        function confirmDelete() {

            var selRow = <%= grdInvoice.ClientID %>.lastMultiSelectIndex;
            if (selRow == -1) {
                alert('Please select the row you want to remove.')
                return false;
            }

            var msg = 'Are you sure you want to delete this invoice?';
            if (confirm(msg))
                return true;

            return false;
        }

        function setTotalAmount() {

            //get the values of the Amount and Vat currency boxes
            var amount = parseFloat(SAHLCurrencyBox_getValue('<%= currAmount.ClientID %>'));
            var vat = parseFloat(SAHLCurrencyBox_getValue('<%= currVatAmount.ClientID %>'));

            var amount = formatNumber(amount,2,'','.');
            var vat = formatNumber(vat,2,'','.');

            if (isNaN(amount))
                amount = 0;           
            if (isNaN(vat))
                vat = 0;  

            var total = formatNumber(parseFloat(amount) + parseFloat(vat),2,'','.') + '';

            SAHLCurrencyBox_setValue("<%= currTotalAmount.ClientID %>", total);
        }
    </script>
    <asp:Panel ID="pnlAttorneySelect" runat="server" Width="98%" Visible="false">
        <table class="tableStandard" width="100%">
            <tr>
                <td class="TitleText" style="width:120px">
                    Account Number:
                </td>
                <td class="LabelText" style="width: 245px">
                    <SAHL:SAHLTextBox ID="txtAccountKey" runat="server" MaxLength="7" Width="180px" OnChange="resetDisplay();"
                        onKeyPress="resetDisplay();" Mandatory="true"/>
                    <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acAccount" TargetControlID="txtAccountKey"
                        OnItemSelected="OnAccountItemSelected" MinCharacters="1">
                    </SAHL:SAHLAutoComplete>
                </td>
                <td style="width: 9px">
                    &nbsp;
                </td>
                <td class="TitleText" style="width: 120px">
                    Attorney:
                </td>
                <td colspan="3">
                    <SAHL:SAHLDropDownList ID="ddlAttorney" runat="server" PleaseSelectItem="true" Mandatory="true"/>
                </td>
            </tr>
            <tr>
                <td class="TitleText">
                    Invoice Number:
                </td>
                <td class="LabelText">
                    <SAHL:SAHLTextBox runat="server" ID="txtInvNumber" MaxLength="20" Mandatory="true" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td class="TitleText">
                    Amount (ex VAT):
                </td>
                <td class="LabelText" style="width: 100px">
                    <SAHL:SAHLCurrencyBox ID="currAmount" runat="server" Mandatory="true" onkeyup="setTotalAmount();"/>
                </td>
                <td class="LabelText" style="width: 44px">
                    &nbsp;</td>
                <td class="LabelText" style="width: 97px">
                    &nbsp;</td>
                                    <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="TitleText">
                    Invoice Date:
                </td>
                <td class="LabelText">
                    <SAHL:SAHLDateBox runat="server" ID="dteInvoiceDate" Mandatory="true"/>
                </td>
                <td>
                </td>
                <td class="TitleText">
                    VAT Amount:
                </td>
                <td class="LabelText">
                    <SAHL:SAHLCurrencyBox ID="currVatAmount" runat="server" onkeyup="setTotalAmount();"/>
                    </td>
                <td class="TitleText" style="text-align:right;vertical-align:middle">
                    Total:</td>
                <td class="LabelText"">
                    <SAHL:SAHLCurrencyBox ID="currTotalAmount" runat="server" ReadOnly="true"/>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="TitleText">
                    Comments:
                </td>
                <td class="LabelText" colspan="6">
                    <SAHL:SAHLTextBox runat="server" ID="txtComments" MaxLength="800" Width="100%" TextMode="MultiLine"
                        Rows="5" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td class="LabelText" colspan="6" align="right">
                    <SAHL:SAHLButton ID="btnAdd" runat="server" Enabled="false" 
                        OnClick="btnAdd_Click" Text="Add" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" 
                        Text="Cancel" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlInvoiceDetail" runat="server" Width="98%" Visible="false">
        <table class="tableStandard" width="100%">
            <tr>
                <td class="TitleText">
                    <SAHL:DXGridView runat="server" ID="grdInvoice" AutoGenerateColumns="False" Width="100%"
                        SettingsBehavior-AllowSelectSingleRowOnly="true" Settings-ShowFooter="true" settings
                        PostBackType="NoneWithClientSelect" Settings-ShowTitlePanel="true" SettingsText-Title="Non Capitalised Invoice Detail"
                        Settings-ShowGroupPanel="true" Settings-ShowVerticalScrollBar="true" SettingsBehavior-AllowDragDrop="true">
                    </SAHL:DXGridView>
                </td>
            </tr>
            <tr>
                <td class="TitleText" align="right">
                    <SAHL:SAHLButton runat="server" ID="btnDelete" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirmDelete();" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
