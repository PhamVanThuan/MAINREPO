<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Administration.Views_BulkTransactionBatch" Title="Bulk Transaction Batch"
    Codebehind="BulkTransactionBatch.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 99%;" valign="top">
                <table style="width: 100%">
                    <tr id="BatchStatusRow" runat="server">
                        <td>
                            <SAHL:SAHLLabel ID="BatchStatusTitle" runat="server" Text="Batch Status"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="BatchStatus" runat="server" PleaseSelectItem="false" Style="width: 100%;"
                                AutoPostBack="True" OnSelectedIndexChanged="BatchStatus_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                        </td>
                    </tr>
                    <tr id="BatchNumberRow" runat="server">
                        <td>
                            <SAHL:SAHLLabel ID="BatchNumberTitle" runat="server" Text="Batch Number"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="BatchNumber" runat="server" PleaseSelectItem="false" Style="width: 100%;"
                                AutoPostBack="True" OnSelectedIndexChanged="BatchNumber_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                        </td>
                    </tr>
                    <tr id="SubsidyTypeRow" runat="server">
                        <td>
                            <SAHL:SAHLLabel ID="SubsidyTypeTitle" runat="server" Text="Subsidy Type"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="SubsidyType" runat="server" PleaseSelectItem="false" Style="width: 100%;"
                                AutoPostBack="True" OnSelectedIndexChanged="SubsidyType_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                        </td>
                    </tr>
                    <tr id="ParentSubsidyProviderRow" runat="server">
                        <td>
                            <SAHL:SAHLLabel ID="ParentSubsidyProviderTitle" runat="server" 
                                Text="Parent Subsidy Provider"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="ParentSubsidyProvider" runat="server" PleaseSelectItem="false"
                                Style="width: 100%;" AutoPostBack="True" DataTextField="Object" DataValueField="Key"
                                OnSelectedIndexChanged="ParentSubsidyProvider_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 180px;">
                            <SAHL:SAHLLabel ID="SubsidyProviderTitle" runat="server" 
                                Text="Subsidy Provider"></SAHL:SAHLLabel>
                        </td>
                        <td style="width: 430px;">
                            <SAHL:SAHLLabel ID="SubsidyProviderDisplay" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="TransactionTypeTitle" runat="server"
                                Text="Transaction Type"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="TransactionType" runat="server" PleaseSelectItem="false"
                                Style="width: 100%;" AutoPostBack="True" OnSelectedIndexChanged="TransactionType_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                            <SAHL:SAHLLabel ID="TransactionTypeDisplay" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px">
                            <SAHL:SAHLLabel ID="EffectiveDateTitle" runat="server"
                                Text="Effective Date"></SAHL:SAHLLabel>
                        </td>
                        <td style="height: 26px">
                            <SAHL:SAHLDateBox ID="dtEffectiveDate" runat="server" />
                            <SAHL:SAHLLabel ID="EffectiveDateDisplay" runat="server">-</SAHL:SAHLLabel>
                        </td>
                        <td style="height: 26px">
                        </td>
                        <td style="height: 26px">
                            <SAHL:SAHLTextBox ID="hiddenBox" runat="server" Style="display: none;"></SAHL:SAHLTextBox>
                        </td>
                    </tr>
                    <tr id="TransactionStatusRow" runat="server">
                        <td>
                            <SAHL:SAHLLabel ID="TransactionStatusTitle" runat="server" 
                                Text="Transaction Status"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="TransactionStatus" runat="server" PleaseSelectItem="false"
                                Style="width: 100%;" AutoPostBack="True" OnSelectedIndexChanged="TransactionStatus_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <SAHL:SAHLGridView ID="BulkGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                                EnableViewState="false" GridHeight="150px" GridWidth="100%" Width="100%" TotalsFooter="true"
                                ShowFooter="true" HeaderCaption="Bulk Transactions" NullDataSetMessage="Select a Batch number to display its Transactions."
                                EmptyDataSetMessage="There are no transactions." OnRowDataBound="BulkGrid_RowDataBound"
                                OnSelectedIndexChanged="BulkGrid_SelectedIndexChanged" SelectFirstRow="False">
                                <HeaderStyle CssClass="TableHeaderB" />
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <SAHL:SAHLLabel ID="RowCount" runat="server">Total Count: 0</SAHL:SAHLLabel>
                                    </td>
                                    <td align="right">
                                        <SAHL:SAHLLabel ID="PageIndicator" runat="server"></SAHL:SAHLLabel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table id="OtherPanel" runat="server" style="width: 100%">
                    <tr>
                        <td style="width: 385px;">
                            <asp:Panel ID="BulkTranGroup" runat="server" GroupingText="Bulk Transaction">
                                <table>
                                    <tr>
                                        <td style="width: 176px;">
                                            <SAHL:SAHLLabel ID="TransactionAmountTitle" runat="server" Text="Transaction Amount"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 186px;">
                                            <SAHL:SAHLCurrencyBox ID="currTransactionAmount" runat="server" Enabled="false" DisplayInputType="Currency"
                                                MaxLength="12" Style="width: 98%;" Amount="0"></SAHL:SAHLCurrencyBox>
                                        </td>
                                        <td style="width: 15px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <SAHL:SAHLButton ID="RemoveButton" runat="server" Text="Remove" CausesValidation="false"
                                                AccessKey="R" Enabled="false" OnClick="RemoveButton_Click" />
                                            <SAHL:SAHLButton ID="UpdateButton" runat="server" Text="Update" CausesValidation="false"
                                                AccessKey="U" Enabled="false" OnClick="UpdateButton_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td style="width: 385px;">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="FindButton" GroupingText="Find Account in Grid">
                                <table>
                                    <tr>
                                        <td style="width: 176px;">
                                            <SAHL:SAHLLabel ID="AccountNumberTitle" runat="server" Text="Account Number"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 201px;">
                                            <SAHL:SAHLTextBox ID="AccountNumber" runat="server" DisplayInputType="Number" Enabled="true"
                                                Columns="15" MaxLength="9"></SAHL:SAHLTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <SAHL:SAHLButton ID="FindButton" runat="server" Text="Find" AccessKey="F" Enabled="true"
                                                CausesValidation="False" OnClick="FindButton_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLCustomValidator ID="ValBatch" runat="server" ControlToValidate="ValBatchControl" />
                <SAHL:SAHLTextBox ID="ValBatchControl" runat="server" Style="display: none;">0</SAHL:SAHLTextBox>
                <SAHL:SAHLButton ID="HiddenButton" runat="server" CausesValidation="false" Style="display: none;"
                    OnClick="HiddenButton_Click" />
                <SAHL:SAHLButton ID="NoQueryButton" runat="server" CausesValidation="false" Style="display: none;" />
                <SAHL:SAHLButton ID="PreviousButton" runat="server" Text="Previous" AccessKey="P"
                    CausesValidation="false" OnClick="PreviousButton_Click" />
                <SAHL:SAHLButton ID="NextButton" runat="server" Text="Next" AccessKey="N" CausesValidation="false"
                    OnClick="NextButton_Click" />
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" CausesValidation="false"
                    OnClick="CancelButton_Click" />
                <SAHL:SAHLButton ID="SaveButton" runat="server" Text="Save Batch" AccessKey="S" ButtonSize="Size4"
                    OnClick="SaveButton_Click" />
                <SAHL:SAHLButton ID="PostButton" runat="server" Text="Post Batch" AccessKey="P" ButtonSize="Size4"
                    OnClick="PostButton_Click" />
                <SAHL:SAHLButton ID="DeleteButton" runat="server" Text="Delete Batch" AccessKey="D"
                    ButtonSize="Size4" OnClick="DeleteButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
