<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.FixedDebitOrderSummary" Title="Fixed Debit Order Summary" Codebehind="FixedDebitOrderSummary.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table width="100%"  class="TableStandard">
<tr><td align="left" style="height:99%;" valign="top">
    
    <SAHL:SAHLGridView ID="AccountsGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="200px" GridWidth="100%"  Width="100%"
        HeaderCaption="Related Accounts"
        NullDataSetMessage=""
        EmptyDataSetMessage="There are no Related Accounts." 
        OnRowDataBound="AccountsGrid_RowDataBound">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    
    <br />
    
    <table border="0" class="tableStandard">
        <tr>
            <td style="width:35%;" class="TitleText">
                Total Amount Due (Open Accounts)
            </td>
            <td style="width:65%;">
                <SAHL:SAHLLabel ID="TotalAmountDue" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Fixed Debit Order Amount
            </td>
            <td>
                <SAHL:SAHLLabel ID="FixedDebitOrderAmount" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Debit Order Day
            </td>
            <td>
                <SAHL:SAHLLabel ID="DebitOrderDay" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText" id="rowInterestOnly" runat="server" colspan = "2" >
            <asp:Panel id="pnlIntOnly" runat="server" GroupingText="Interest Only Product Information">
            <table border="0">
                <tr>
                    <td style="width:35%;" class="TitleText">
                        Amortising Instalment
                    </td>
                    <td style="width:65%;">
                        <SAHL:SAHLLabel ID="lblAmotisingInstallment" runat="server">-</SAHL:SAHLLabel>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            </td>
        </tr>
    </table>
    
    <br />
    
    <SAHL:SAHLGridView ID="FutureOrderGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="100px" GridWidth="100%"  Width="100%"
        HeaderCaption="Future Dated Fixed Debit Orders"
        NullDataSetMessage=""
        EmptyDataSetMessage="There are no Future Dated Fixed Debit Orders." 
        OnSelectedIndexChanged="FutureOrderGrid_SelectedIndexChanged"
        OnRowDataBound="BindFutureDatedDOGrid_RowDataBound">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    
    <br />
    
    <table id="AddRow" runat="server" border="0">
        <tr>
            <td style="width:176px;" class="TitleText">
                Fixed Debit Order Amount
            </td>
            <td style="width:186px;">
                <SAHL:SAHLTextBox ID="FixedDebitOrderAmountUpdate" runat="server" style="width:98%;" MaxLength="12" DisplayInputType="Currency">0.00</SAHL:SAHLTextBox>
            </td>
            <td style="width:15px;">
            </td>
            <td style="width:176px;" class="TitleText" align="center">
                Effective Date
            </td>
            <td style="width:186px;">
                <SAHL:SAHLDateBox ID="EffectiveDateUpdate" runat="server" />
            </td>
            <td style="width:15px;">
                <SAHL:SAHLTextBox ID="ValEffectiveDateUpdateControl" runat="server" style="display:none;"></SAHL:SAHLTextBox>
            </td>
        </tr>
    </table>

</td></tr>
<tr id="ButtonRow" runat="server"><td align="right">
    
    <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" />
    <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />
</td></tr></table>
</asp:Content>