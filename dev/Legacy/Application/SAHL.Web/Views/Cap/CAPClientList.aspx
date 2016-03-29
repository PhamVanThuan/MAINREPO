<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Cap.CAPClientList" Title="CAP Client List" Codebehind="CAPClientList.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 99%;" valign="top">
                <table class="tableStandard" width="100%">
                    <tr>
                        <td style="width: 150px;" class="TitleText" align="left">
                            Reset Date
                        </td>
                        <td style="width: 336px;">
                            <SAHL:SAHLDropDownList ID="ddlResetDate" runat="server" PleaseSelectItem="false"
                                AutoPostBack="True" Style="width: 100%;">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td style="width: 284px;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText" align="left">
                            Offer Dates
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="ddlOfferDates" runat="server" PleaseSelectItem="true"
                                AutoPostBack="True" Mandatory="true" Style="width: 100%;">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <SAHL:SAHLGridView ID="CapListGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                                EnableViewState="false" GridHeight="100px" GridWidth="100%" Width="100%" HeaderCaption="CAP Type Configuration"
                                NullDataSetMessage="" EmptyDataSetMessage="There are no CAP Type Configuration records."
                                OnRowDataBound="CapListGrid_RowDataBound">
                                <HeaderStyle CssClass="TableHeaderB" />
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                    <tr id="ArrearRow" runat="server">
                        <td class="TitleText" align="left">
                            Loan Arrear Balance
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="ddlOperator" runat="server" PleaseSelectItem="false">
                            </SAHL:SAHLDropDownList>
                            R
                            <SAHL:SAHLTextBox ID="LoanArrearBalance" runat="server" Mandatory="true" DisplayInputType="Number" />
                            <span class="smaller">(Must be multiple of 500)</span>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="SPVRow" runat="server">
                        <td class="TitleText" align="left" valign="top">
                            SPV Number
                        </td>
                        <td valign="top" colspan="2">
                            <asp:Panel ID="SPVScroll" runat="server" Style="border: 1px solid #C0C0C0; width: 99%;
                                height: 200px; overflow-y: scroll;">
                                <SAHL:SAHLCheckboxList ID="SPVNumber" runat="server">
                                </SAHL:SAHLCheckboxList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr id="DateExcludeRow" runat="server">
                        <td valign="top" class="TitleText" style="padding-right: 5px;">
                            Exclude Clients with offers after
                        </td>
                        <td valign="top" colspan="2">
                            <SAHL:SAHLDateBox ID="DateExclude" runat="server" />
                        </td>
                    </tr>
                    <tr id="FileNameRow" runat="server" visible="false">
                        <td class="TitleText" align="left">
                            File Name
                        </td>
                        <td colspan="2">
                            <asp:FileUpload ID="FileName" runat="server" Style="width: 600px;" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="ExtractButton" runat="server" Text="Extract" AccessKey="E" Visible="false"
                    OnClick="ExtractButton_Click" />
                <SAHL:SAHLButton ID="ImportButton" runat="server" Text="Import" AccessKey="I" Visible="false"
                    OnClick="ImportButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
