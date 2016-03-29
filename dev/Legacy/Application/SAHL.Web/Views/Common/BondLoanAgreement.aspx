<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="BondLoanAgreement.aspx.cs" Inherits="SAHL.Web.Views.Common.BondLoanAgreement"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" valign="top">
                <SAHL:SAHLGridView ID="BondGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                    EnableViewState="false" GridHeight="150px" GridWidth="100%" Width="100%" HeaderCaption="Bonds"
                    NullDataSetMessage="There are no Bonds registered." EmptyDataSetMessage="There are no Bonds registered."
                    OnRowDataBound="BondGrid_RowDataBound" OnSelectedIndexChanged="BondGrid_SelectedIndexChanged">
                </SAHL:SAHLGridView>
                <table id="BondDetailRow" runat="server" class="tableStandard">
                   <tr>
                        <td class="TitleText"><br />

                            Deeds Office
                        </td>
                        <td colspan="4"><br />
                            <SAHL:SAHLDropDownList ID="DeedsOfficeUpdate" runat="server" Style="width: 100%;" PleaseSelectItem="false" OnSelectedIndexChanged="DeedsOfficeUpdate_SelectedIndexChanged" AutoPostBack="True">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td><br />
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Attorney
                        </td>
                        <td colspan="4">
                            <SAHL:SAHLDropDownList ID="AttorneyUpdate" runat="server" Style="width: 100%;" PleaseSelectItem="false">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 176px;" class="TitleText">
                            Bond Registration Number
                        </td>
                        <td style="width: 186px;">
                            <SAHL:SAHLTextBox ID="BondRegNumberUpdate" runat="server" MaxLength="15" Style="width: 98%;"></SAHL:SAHLTextBox>
                        </td>
                        <td style="width: 15px;">
                        </td>
                        <td style="width: 176px;" class="TitleText">
                            Bond Registration Amount
                        </td>
                        <td style="width: 186px;">
                            <SAHL:SAHLTextBox ID="BondRegAmountUpdate" runat="server" DisplayInputType="Currency"
                                Style="width: 86%;" MaxLength="12"></SAHL:SAHLTextBox>
                        </td>
                        <td style="width: 15px;">
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Bond Registration Date<br />
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="BondRegDate" runat="server">-</SAHL:SAHLLabel><br />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="TitleText">
                            Loan Agreement Amount<br />
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="LoanAgreeAmount" runat="server">-</SAHL:SAHLLabel><br />
                        </td>
                        <td>
                            <br />
                        </td>
                    </tr>
                </table>
                <SAHL:SAHLGridView ID="LoanAgreeGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="150px" GridWidth="100%"
                    Width="100%" HeaderCaption="Loan Agreements" NullDataSetMessage="There are no Loan Agreements."
                    EmptyDataSetMessage="There are no Loan Agreements.">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
                <table id="AddRow" runat="server" border="0">
                    <tr>
                        <td style="width: 176px;" class="TitleText">
                            Loan Agreement Date
                        </td>
                        <td style="width: 186px;">
                            <SAHL:SAHLDateBox ID="LoanAgreeDateAdd" runat="server" />
                        </td>
                        <td style="width: 15px;">
                        </td>
                        <td style="width: 176px;" class="TitleText" align="center">
                            Loan Agreement Amount
                        </td>
                        <td style="width: 186px;">
                            <SAHL:SAHLTextBox ID="LoanAgreeAmountAdd" runat="server" DisplayInputType="Currency"
                                Style="width: 100%;" MaxLength="12">0.00</SAHL:SAHLTextBox>
                        </td>
                        <td style="width: 15px;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                    CausesValidation="False" />
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
