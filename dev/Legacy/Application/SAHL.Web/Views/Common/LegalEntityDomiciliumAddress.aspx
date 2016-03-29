<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.LegalEntityDomiciliumAddress" Title="CBO Page" CodeBehind="LegalEntityDomiciliumAddress.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>

    <table border="0" cellspacing="0" cellpadding="0" class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 100%; width: 100%;" valign="top">
                <table border="0" id="InfoTable" runat="server" width="100%">
                    <tr id="addressSelectionRow">
                        <td style="width: 100%">
                            <SAHL:DXGridView ID="gvAddresses" runat="server" AutoGenerateColumns="False" Width="100%" PostBackType="NoneWithClientSelect"
                                ClientInstanceName="gridAddressesDUser" Settings-ShowTitlePanel="true" SettingsText-Title="Select a Proposed Domicilium Address"
                                EnableViewState="false" OnHtmlDataCellPrepared="gvAddresses_HtmlDataCellPrepared">
                            </SAHL:DXGridView>
                        </td>
                    </tr>
                    <tr id="activeDomiciliumAddressRow">
                        <td>
                            <asp:Panel ID="pnlActiveDomiciliumAddressDetails" runat="server" GroupingText="Active Legal Entity Domicilium Address" Font-Bold="true">
                                <SAHL:SAHLTextBox ID="ActiveDomiciliumAddress" runat="server" CssClass="LabelText" Height="180px" Wrap="true"
                                    Width="180px" ReadOnly="True" TextMode="MultiLine" Style="overflow: auto" BorderStyle="None" Rows="12">-</SAHL:SAHLTextBox>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlDomiciliumAddressDetails" runat="server" GroupingText="Application Domicilium Address" Font-Bold="true">
                                <SAHL:SAHLTextBox ID="AddressLine" runat="server" CssClass="LabelText" Height="180px" Wrap="true"
                                    Width="180px" ReadOnly="True" TextMode="MultiLine" Style="overflow: auto" BorderStyle="None" Rows="12">-</SAHL:SAHLTextBox>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ViewButtons" runat="server">
            <td align="right" valign="bottom" style="width: 100%">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                    CausesValidation="false" />
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click"
                    CausesValidation="true" />&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>