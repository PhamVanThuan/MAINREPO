<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.ManualDebitOrder" Title="Untitled Page" Codebehind="ManualDebitOrder.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
<table border="0" cellspacing="0" cellpadding="0" class="PageBlock" style="width: 956px"><tr><td align="left" style="height:99%; width: 98%;" valign="top">

    <SAHL:SAHLGridView ID="RecordsGrid" runat="server" AutoGenerateColumns="False" FixedHeader="false"
        EnableViewState="false"  GridWidth="99%"
        HeaderCaption="Manual Debit Orders" 
        NullDataSetMessage="No manual debit orders found."
        EmptyDataSetMessage="No manual debit orders found." 
        PostBackType = "SingleClick"
        OnSelectedIndexChanged="RecordsGrid_SelectedIndexChanged" 
        OnRowDataBound="RecordsGrid_RowDataBound" Height="22%">
        <HeaderStyle CssClass="TableHeaderB" />
    </SAHL:SAHLGridView>
    
    <br />    

    <table border="0" id="DisplayData" runat="server" width="100%" style="height:50%">
         <tr id="ArrearBalanceRow" runat="server">
            <td style="width:166px;" class="TitleText">
                Arrear Balance
            </td>
            <td style="width:750px;">
                <SAHL:SAHLLabel ID="lblArrearBalance" runat="server">-</SAHL:SAHLLabel>
            </td>
            <td style="width:15%;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width:15%;" class="TitleText">
                Bank
            </td>
            <td style="width:750px;">
                <SAHL:SAHLLabel ID="lblBank" runat="server" CssClass="LabelText" TextAlign="Left" Width="100%">-</SAHL:SAHLLabel>
                <SAHL:SAHLDropDownList ID="ddlBankUpdate" runat="server" Width="100%"></SAHL:SAHLDropDownList>
            </td>
            <td style="width:15%;">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="TitleText">
                Effective Date
            </td>
            <td style="width: 750px;">
                <SAHL:SAHLLabel ID="lblEffectiveDate" runat="server">-</SAHL:SAHLLabel>
                <SAHL:SAHLInputDate ID="dateEffectiveDateUpdate" runat="server" />
            </td>
            <td>
                <SAHL:SAHLTextBox ID="ValEffectiveDateUpdateControl" runat="server" style="display:none;" Width="100%"></SAHL:SAHLTextBox></td>
        </tr>
        <tr>
            <td class="TitleText">
                Amount
            </td>
            <td style="width: 750px">
                <SAHL:SAHLLabel ID="lblAmount" runat="server">-</SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtAmountUpdate" runat="server" Mandatory="true" DisplayInputType="Currency">0.00</SAHL:SAHLTextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="TitleText">
                Reference
            </td>
            <td style="width: 750px">
                <SAHL:SAHLLabel ID="lblReference" runat="server">-</SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtReferenceUpdate" runat="server" Width="100%"></SAHL:SAHLTextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="TitleText" valign="top">
                Note
            </td>
            <td style="width: 750px">
                <asp:Panel ID="NotePanel" runat="server" style="width:100%;height:40px;overflow-y:scroll;border:1px solid #E5E5E5;" Width="100%">
                    <SAHL:SAHLLabel ID="lblNote" runat="server" Width="100%">-</SAHL:SAHLLabel>
                </asp:Panel>
                <SAHL:SAHLTextBox ID="txtNoteUpdate" runat="server" TextMode="MultiLine" Width="102%" EnableViewState="False"></SAHL:SAHLTextBox>
            </td>
            <td valign="top">
                &nbsp;</td>
        </tr>
    </table>

</td></tr>
<tr id="ButtonRow" runat="server"><td align="right" style="width: 98%">
    <br />
    
    <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" CausesValidation="False" />
    <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />&nbsp;
    <SAHL:SAHLTextBox ID="SubmitValuation" runat="server" style="display:none;" Width="15%"></SAHL:SAHLTextBox>
    
</td></tr></table>
</asp:Content>
