<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.Views_RateChange" Title="Rate Change" Codebehind="RateChange.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="100%" class="TableStandard">
        <tr>
            <td align="left" style="height: 99%;" valign="top">
    <SAHL:SAHLGridView ID="RatesGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
                    EnableViewState="false" GridHeight="400px" GridWidth="100%" Width="100%" HeaderCaption="Financial Services"
                    NullDataSetMessage="" EmptyDataSetMessage="There are no Financial Services."
        OnRowDataBound="RatesGrid_RowDataBound" >
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    <br />
    <table border="0" id="TermTable1" runat="server">
        <tr>
            <td style="width:166px; height: 26px;" class="TitleText">
                New Remaining Term
            </td>
            <td style="height: 26px">
                            <SAHL:SAHLTextBox ID="NewRemainingTerm" runat="server" CausesValidation="True" DisplayInputType="Number"
                                FormatDecimalPlace="0" EnableTheming="True" Mandatory="True"></SAHL:SAHLTextBox>&nbsp;
            </td>
            <td style="width: 7px; height: 26px;">
                &nbsp;</td>
        </tr>
    </table>
                <asp:Panel ID="pnlPTI" Width="30%" runat="server" Visible="false">
                    <table border="0" runat="server">
                        <tr id="CurrentPTIRow" runat="server">
                            <td style="width: 201px;" class="TitleText">
                                Current PTI
                            </td>
                            <td style="width: 156px;">
                                <SAHL:SAHLLabel ID="CurrentPTI" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr id="NewPTIRow" runat="server">
                            <td style="width: 201px;" class="TitleText">
                                New PTI
                            </td>
                            <td style="width: 156px;">
                                <SAHL:SAHLLabel ID="NewPTI" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
    <table border="0" id="TermTable2" runat="server">
        <tr>
            <td>
                <asp:Panel ID="TermVariable" runat="server" GroupingText="Variable Loan">
                <table border="0">
                    <tr>
                        <td style="width:201px;" class="TitleText">
                            New Instalment
                        </td>
                        <td style="width:156px;">
                            <SAHL:SAHLLabel ID="TermNewInstallmentVariable" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                                    <tr id="AmortisatisionRow1" runat="server">
                        <td style="width:201px;" class="TitleText">
                            Amortisation Instalment
                        </td>
                        <td style="width:156px;">
                            <SAHL:SAHLLabel ID="AmortisationInstallmentVariable" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>                    
                </table>
                </asp:Panel>
            </td>
            <td style="width: 140px">
                <asp:Panel ID="TermFixed" runat="server" GroupingText="Fixed Loan">
                <table border="0">
                    <tr>
                        <td style="width:201px;" class="TitleText">
                            New Instalment
                        </td>
                        <td style="width:156px;">
                            <SAHL:SAHLLabel ID="TermNewInstallmentFixed" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                                    <tr id="AmortisatisionRow2" runat="server">
                        <td style="width:201px;" class="TitleText">
                            Amortisation Instalment
                        </td>
                        <td style="width:156px;">
                            <SAHL:SAHLLabel ID="AmortisationInstallmentFixed" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table border="0" id="RateTable1" runat="server">
        <tr>
            <td style="width:166px;" class="TitleText">
                New Link Rate
            </td>
            <td style="width:211px;">
                            <SAHL:SAHLDropDownList ID="NewLinkRate" runat="server" Style="width: 100%;" PleaseSelectItem="false"
                                AutoPostBack="true" OnSelectedIndexChanged="NewLinkRate_SelectedIndexChanged"
                                CausesValidation="True">
                </SAHL:SAHLDropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <table border="0" id="RateTable2" runat="server">
        <tr>
            <td>
                <asp:Panel ID="RateVariable" runat="server" GroupingText="Variable Loan">
                <table border="0">
                    <tr>
                        <td style="width:201px;" class="TitleText">
                            Market Rate
                        </td>
                        <td style="width:156px;">
                            <SAHL:SAHLLabel ID="RateMarketRateVariable" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Discount
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="RateDiscountVariable" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            New Interest Rate
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="RateNewIntRateVariable" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            New Instalment
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="RateNewInstallmentVariable" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                                    <tr id="RateAmortisationRow1" runat="server">
                        <td  class="TitleText">
                            Amortisation Instalment
                        </td>
                        <td >
                            <SAHL:SAHLLabel ID="RateAmortisationInstallmentVariable" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
            </td>
            <td style="width: 140px">
                <asp:Panel ID="RateFixed" runat="server" GroupingText="Fixed Loan">
                <table border="0">
                    <tr>
                        <td style="width:201px;" class="TitleText">
                            Market Rate
                        </td>
                        <td style="width:156px;">
                            <SAHL:SAHLLabel ID="RateMarketRateFixed" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Discount
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="RateDiscountFixed" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            New Interest Rate
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="RateNewIntRateFixed" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            New Installment
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="RateNewInstallmentFixed" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                                    <tr id="RateAmortisationRow2" runat="server">
                        <td  class="TitleText">
                            Amortisation Instalment
                        </td>
                        <td >
                            <SAHL:SAHLLabel ID="RateAmortisationInstallmentFixed" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table border="0" id="TermCommentsPanel" runat="server">
        <tr>
            <td style="width:100px">
                            <asp:Panel ID="TermCommentPanel" runat="server" GroupingText="Comments" Wrap="False"
                                Width="373px">
                <table border="0" style="width: 155px">
                    <tr>
                            <td style="width:37px; height: 67px;">
                                            <SAHL:SAHLTextBox ID="TermMemoComments" runat="server" CausesValidation="True" EnableTheming="True"
                                                FormatDecimalPlace="0" Height="44px" Width="350px" TextMode="MultiLine" OnTextChanged="TermMemoComments_TextChanged"
                                                Mandatory="True"></SAHL:SAHLTextBox></td>
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
                <SAHL:SAHLTextBox ID="ProceedWith60" runat="server" Style="display: none;"></SAHL:SAHLTextBox>
                <SAHL:SAHLButton ID="CalcNewTerm" runat="server" Text="Calculate" OnClick="CalcNewTerm_Click" />
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" ButtonSize="Size5"
                    AccessKey="S" OnClick="SubmitButton_Click" />
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                    CausesValidation="False" />&nbsp;
                <SAHL:SAHLTextBox ID="SubmitControl" runat="server" Style="display: none;"></SAHL:SAHLTextBox>
            </td>
        </tr>
    </table>
</asp:Content>
