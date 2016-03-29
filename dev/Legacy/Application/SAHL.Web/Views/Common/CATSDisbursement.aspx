<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.CATSDisbursement" Title="CATS Disbursement" Codebehind="CATSDisbursement.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table class="tableStandard" width="100%">
<tr>
<td align="left" style="height:99%;" valign="top">
    <table border="0" id="tblDisplay" runat="server" width="99%">
        <tr>
            <td style="width:20%;" class="TitleText">
                Transaction Type
            </td>
            <td style="width:150px">
                <SAHL:SAHLLabel ID="lblTransactionType" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td style="width:20%;" class="TitleText">
                Amount
            </td>
            <td style="width:300px">
                <SAHL:SAHLLabel ID="lblDisbursementsTotal" runat="server">-</SAHL:SAHLLabel>        
            </td>
        </tr> 
    </table>
    
    <table border="0" id="tblAdd1" runat="server" width="99%">
        <tr>
            <td style="width:20%;" class="TitleText">
                Disbursement Type
            </td>
            <td style="width:30%">
                <SAHL:SAHLDropDownList ID="ddlDisbursementType" runat="server" PleaseSelectItem="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDisbursementType_OnSelectedIndexChanged"></SAHL:SAHLDropDownList>
                <SAHL:SAHLLabel ID="lblddlTransactionType" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td style="width:20%;" class="TitleText">
                Total Amount To Disburse
            </td>
            <td style="width:60%">
                <SAHL:SAHLCurrencyBox ID="txtTotalAmount" runat="server" Width="80px" MaxLength="15">0</SAHL:SAHLCurrencyBox>        
            </td>
        </tr> 
        <tr>
            <td style="width:20%;" class="TitleText">
                Disbursement Reference
            </td>
            <td style="width:60%">
                <SAHL:SAHLTextBox ID="txtDisbursementReference" runat="server" MaxLength="50" DisplayInputType="Normal" Width="80%" ToolTip="This is the reference that will be displayed on the loan transaction which will be posted when you select the Post button."></SAHL:SAHLTextBox>        
            </td>
        </tr>         
    </table>
    
    <table border="0" id="tblGrid" runat="server" width="99%">
     <tr>
        <td colspan='2'>        
            <SAHL:SAHLGridView ID="grdDisbursements" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
                EnableViewState="false" GridHeight="200px" GridWidth="100%" Width="100%"
                HeaderCaption="Disbursement Bank Account Details"
                NullDataSetMessage="There are no disbursement records."
                EmptyDataSetMessage="There are no disbursement records." OnRowDataBound="grdDisbursements_RowDataBound" 
                 >
                <RowStyle CssClass="TableRowA" />
            </SAHL:SAHLGridView>
        </td>
    </tr>
    </table>
    
    <table border="0" id="tblAdd2" runat="server" width="99%">
        <tr>
            <td style="width:20%;"  class="TitleText">
                Bank Details
            </td>
            <td style="width:80%;">
                <SAHL:SAHLDropDownList ID="ddlBankDetails" runat="server" PleaseSelectItem="true" Width="90%"></SAHL:SAHLDropDownList>
            </td>
        </tr>
        <tr>
            <td style="width:20%;"  class="TitleText">
                Amount
            </td>
            <td style="width:50%;">
                <SAHL:SAHLCurrencyBox ID="txtDisbursementAmount" runat="server" Width="80px" MaxLength="15">0</SAHL:SAHLCurrencyBox>        
            </td>
        </tr> 
        <tr><td align="right" colspan='2' style="height: 25px">
            <SAHL:SAHLButton ID="AddDisbursement" Text="Add" runat="server" OnClick="AddDisbursement_Click" CausesValidation="true"/>
            <SAHL:SAHLButton ID="DeleteDisbursement" Text="Delete" runat="server" OnClick="DeleteDisbursement_Click" />
        </td></tr>
    </table>    
    
    <table border="0" id="tblRollback" runat="server" width="99%">
     <tr>
        <td colspan='2'>        
            <SAHL:SAHLGridView ID="grdLoanTransactions" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
                EnableViewState="false" GridHeight="200px" GridWidth="100%" Width="100%"
                HeaderCaption="Loan Transactions"
                NullDataSetMessage="There are no loan transactions."
                EmptyDataSetMessage="There are no loan transactions." OnRowDataBound="grdLoanTransactions_RowDataBound"  PostBackType="SingleAndDoubleClick" OnSelectedIndexChanged="grdLoanTransactions_SelectedIndexChanged" 
                 >
                <RowStyle CssClass="TableRowA" />
            </SAHL:SAHLGridView>
        </td>
    </tr>
     <tr>
        <td colspan='2'>        
            <SAHL:SAHLGridView ID="grdDisbursementTransactions" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
                EnableViewState="false" GridHeight="200px" GridWidth="100%" Width="100%"
                HeaderCaption="Disbursement Transactions"
                NullDataSetMessage="There are no disbursement transactions."
                EmptyDataSetMessage="There are no disbursement transactions." OnRowDataBound="grdDisbursementTransactions_RowDataBound" 
                 >
                <RowStyle CssClass="TableRowA" />
            </SAHL:SAHLGridView>
        </td>
    </tr>    
    </table>    
</td></tr>
<tr id="ButtonRow" runat="server"><td align="right">
    <SAHL:SAHLButton ID="SaveButton" runat="server" Text="Save" AccessKey="C" OnClick="SaveButton_Click" />
    <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" CausesValidation="false" />
    <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" SecurityTag="CATDisbursementSubmit" SecurityDisplayType="Disable" />
</td></tr></table>
</asp:Content>