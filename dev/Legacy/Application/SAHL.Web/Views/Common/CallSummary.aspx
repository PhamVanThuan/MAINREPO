<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.CallSummary" Title="Call Summary" Codebehind="CallSummary.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td valign="top">
                <table class="tableStandard" width="100%">
                    <tr>
                        <td colspan="4">
                            <SAHL:SAHLGridView ID="CallSummaryGrid" runat="server" AutoGenerateColumns="false"
                                FixedHeader="false" EnableViewState="false" GridHeight="180px" GridWidth="100%"
                                Width="100%" HeaderCaption="Call Summary" NullDataSetMessage="" EmptyDataSetMessage="There are no Call's."
                                PostBackType="SingleClick" OnSelectedIndexChanged="CallSummaryGrid_SelectedIndexChanged"
                                OnRowDataBound="CallSummaryGrid_RowDataBound">
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="AddUpdatePanel" runat="server" GroupingText="Call Query" Width="100%">
                                <table border="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 12%;">
                                        </td>
                                        <td style="width: 50%;">
                                        </td>
                                        <td style="width: 15%;">
                                        </td>
                                        <td style="width: 25%;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TitleText">
                                            Description
                                        </td>
                                        <td colspan="3" align="left">
                                            <SAHL:SAHLTextBox ID="txtShortDescription" runat="server" MaxLength="80" Width="96%" CssClass="mandatory"></SAHL:SAHLTextBox>
                                            <SAHL:SAHLLabel ID="lblShortDescription" runat="server"></SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TitleText">
                                            Category
                                        </td>
                                        <td align="left">
                                            <SAHL:SAHLDropDownList ID="ddlCategory" runat="server" AutoPostBack="False" PleaseSelectItem="true" CssClass="mandatory">
                                            </SAHL:SAHLDropDownList>
                                            <SAHL:SAHLLabel ID="lblCategory" runat="server"></SAHL:SAHLLabel>
                                        </td>
                                        <td align="left" class="TitleText" runat="server" id="StatusTitle">
                                            Status
                                        </td>
                                        <td align="left">
                                            <SAHL:SAHLDropDownList ID="ddlStatus" runat="server" AutoPostBack="False" PleaseSelectItem="true" CssClass="mandatory">
                                            </SAHL:SAHLDropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TitleText" runat="server" id="ExpiryDateTitle">
                                            Expiry Date
                                        </td>
                                        <td align="left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <SAHL:SAHLDateBox ID="datExpiryDate" Style="width: 100px" runat="server" CssClass="mandatory"></SAHL:SAHLDateBox></td>
                                                    <td>
                                                        <SAHL:SAHLTextBox ID="txtExpiryDate" Style="display: none; width: 2px" Width="2px"
                                                            runat="server"></SAHL:SAHLTextBox></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="left" class="TitleText" runat="server" id="ReminderDateTitle">
                                            Reminder Date
                                        </td>
                                        <td align="left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <SAHL:SAHLDateBox ID="datReminderDate" Style="width: 100px" runat="server" CssClass="mandatory"></SAHL:SAHLDateBox></td>
                                                    <td>
                                                        <SAHL:SAHLTextBox ID="txtReminderDate" Style="display: none; width: 2px" Width="2px"
                                                            runat="server"></SAHL:SAHLTextBox></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TitleText">
                                            Query Type
                                        </td>
                                        <td align="left">
                                            <SAHL:SAHLDropDownList ID="ddlQueryType" runat="server" AutoPostBack="True" PleaseSelectItem="true" CssClass="mandatory">
                                            </SAHL:SAHLDropDownList>
                                            <SAHL:SAHLLabel ID="lblQueryType" runat="server"></SAHL:SAHLLabel>
                                        </td>
                                        <td align="left" class="TitleText" id="AccountNumberTitle" runat="server">
                                            Account Number
                                        </td>
                                        <td align="left">
                                            <SAHL:SAHLDropDownList ID="ddlAccountNumber" runat="server" AutoPostBack="False" PleaseSelectItem="true" CssClass="mandatory">
                                            </SAHL:SAHLDropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="TitleText">
                                            Detailed Description
                                        </td>
                                        <td colspan="3" align="left">
                                            <SAHL:SAHLLabel ID="lblDetailDescription" runat="server" Style="width: 100%; height: 100%;"></SAHL:SAHLLabel>
                                            <SAHL:SAHLTextBox ID="txtDetailDescription" runat="server" MaxLength="7000" TextMode="MultiLine"
                                                Width="96%" Height="80px" Font-Names="Verdana" Font-Size="8pt" CssClass="mandatory"></SAHL:SAHLTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="right">
                                            <SAHL:SAHLButton ID="btnAddOrUpdate" runat="server" CausesValidation="true" Text="Submit"
                                                AccessKey="S" OnClick="btnAddOrUpdate_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlConsultant" runat="server" GroupingText="Route To Consultant" Width="100%" Visible="false">
                                <table border="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 12%;">
                                        </td>
                                        <td style="width: 50%;">
                                        </td>
                                        <td style="width: 15%;">
                                        </td>
                                        <td style="width: 25%;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TitleText">
                                            Consultant
                                        </td>
                                        <td align="left">
                                            <SAHL:SAHLDropDownList ID="ddlConsultant" runat="server">
                                            </SAHL:SAHLDropDownList>
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
            <td align="right" valign="bottom">
                <br />
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" CausesValidation="false"
                    OnClick="btnCancel_Click" />
                <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" AccessKey="S" CausesValidation="false"
                    OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
