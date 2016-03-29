<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.master"
    CodeBehind="RecoveriesProposal.aspx.cs" Inherits="SAHL.Web.Views.Recoveries.RecoveriesProposal" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 103%;" valign="top">
                <br />
                <SAHL:DXGridView ID="gvRecoveriesProposal" PostBackType="None" runat="server" KeyFieldName="RecoveriesProposalKey"
                    AutoGenerateColumns="false" Width="100%" OnHtmlDataCellPrepared="gvRecoveriesProposal_HtmlDataCellPrepared">
                    <Settings ShowGroupPanel="false" ShowTitlePanel="true" />
                    <SettingsText Title="Recoveries Proposal Details" EmptyDataRow=" " />
                    <SettingsBehavior AllowGroup="false" />
                    <SettingsPager PageSize="15" />
                    <Columns>
                        <SAHL:DXGridViewFormattedTextColumn FieldName="ShortfallAmount" Caption="Shortfall Amount"
                            VisibleIndex="0">
                        </SAHL:DXGridViewFormattedTextColumn>
                        <SAHL:DXGridViewFormattedTextColumn FieldName="RepaymentAmount" Caption="Repayment Amount"
                            VisibleIndex="1">
                        </SAHL:DXGridViewFormattedTextColumn>
                        <SAHL:DXGridViewFormattedTextColumn FieldName="StartDate" Caption="Start Date" VisibleIndex="2">
                        </SAHL:DXGridViewFormattedTextColumn>
                        <SAHL:DXGridViewFormattedTextColumn FieldName="" Caption="Status"
                            VisibleIndex="3">
                        </SAHL:DXGridViewFormattedTextColumn>
                        <SAHL:DXGridViewFormattedTextColumn FieldName="" Caption="User" VisibleIndex="4">
                        </SAHL:DXGridViewFormattedTextColumn>
                        <SAHL:DXGridViewFormattedTextColumn FieldName="" Caption="AOD"
                            VisibleIndex="5">
                        </SAHL:DXGridViewFormattedTextColumn>
                    </Columns>
                </SAHL:DXGridView>
                <br />
                <br />
                <asp:Panel ID="pnlAdd" runat="server" Style="width: 99%">
                    <table border="0" class="tableStandard">
                        <tr>
                            <td>
                                <SAHL:SAHLLabel ID="lblShortfallAmount" runat="server" Text="Shortfall Amount" />
                            </td>
                            <td>
                                <SAHL:SAHLCurrencyBox ID="txtShortfallAmount" runat="server" TextAlign="Right"></SAHL:SAHLCurrencyBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <SAHL:SAHLLabel ID="lblRepaymentAmount" runat="server" Text="Repayment Amount" />
                            </td>
                            <td>
                                <SAHL:SAHLCurrencyBox ID="txtRepaymentAmount" runat="server" TextAlign="Right"></SAHL:SAHLCurrencyBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <SAHL:SAHLLabel ID="lblStartDate" runat="server" Text="Start Date" />
                            </td>
                            <td>
                                <SAHL:SAHLDateBox ID="dteStartDate" runat="server"></SAHL:SAHLDateBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkAOD" Text="Acknowledgement of Debt" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click"
                    CausesValidation="false" />
                <SAHL:SAHLButton ID="AddButton" runat="server" Text="Add" AccessKey="A" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
