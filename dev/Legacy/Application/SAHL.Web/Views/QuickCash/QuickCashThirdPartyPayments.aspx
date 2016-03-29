<%@ Page Language="C#"  MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" CodeBehind="QuickCashThirdPartyPayments.aspx.cs" Inherits="SAHL.Web.Views.QuickCash.QuickCashThirdPartyPayments" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">
    function calculatePortionToClient()
    {
        var availableAmount;
        var requestedAmount;
        availableAmount = SAHLCurrencyBox_getValue('<%=txtAvailableAmount.ClientID%>').replaceAll(',', '');
        requestedAmount = SAHLCurrencyBox_getValue('<%=txtAmount.ClientID%>').replaceAll(',', '');
        var amt = parseFloat(availableAmount)-parseFloat(requestedAmount);
        if (amt < 0)
            amt = 0;
        amt = formatNumber(amt, 2, ',', '.'); 
        SAHLCurrencyBox_setValue('<%=txtPortionToClient.ClientID%>', amt);
    }
</script> 
<div id="divMain">
 <SAHL:SAHLGridView ID="grdQuickCashThirdPartyPayments" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" width="100%" GridWidth="100%"   HeaderCaption="Quick Cash Third Party Payment Details"
        NullDataSetMessage=""
        EmptyDataSetMessage="There are no Quick Cash Third Party Payments."
        OnRowDataBound="grdQuickCashThirdPartyPayments_RowDataBound" ShowFooter="true" OnSelectedIndexChanged="grdQuickCashThirdPartyPayments_SelectedIndexChanged">
        <RowStyle CssClass="TableRowA" />
</SAHL:SAHLGridView>
<br />
<asp:Panel ID="pnlMain" runat="server" Width="100%">
<div class="row" style="width:99%">
    <div class="cellInput" style="width:5%"></div>
    <SAHL:SAHLLabel runat="server" ID="lblAmtRequested" Font-Bold="true" Width="160px">Total Payment Amount</SAHL:SAHLLabel>
    <SAHL:SAHLCurrencyBox runat="server" id="txtAmtRequested" Width="100px" AllowNegative="false" ReadOnly="true"></SAHL:SAHLCurrencyBox>
    <SAHL:SAHLLabel runat="server" ID="lblAvailableAmount" Font-Bold="true" Width="180px">Available Payment Amount</SAHL:SAHLLabel>
    <SAHL:SAHLCurrencyBox runat="server" id="txtAvailableAmount" Width="100px" AllowNegative="false" ReadOnly="true"></SAHL:SAHLCurrencyBox>
     <SAHL:SAHLLabel runat="server" ID="lblPortionToClient" Font-Bold="true"  Width="120px">&nbsp;&nbsp;Portion to client</SAHL:SAHLLabel>
    <SAHL:SAHLCurrencyBox runat="server" id="txtPortionToClient" Width="100px" AllowNegative="false" ReadOnly="true"></SAHL:SAHLCurrencyBox>
</div>
<br />
<br />
<asp:Panel ID="panelPayments" runat="server" GroupingText="Payment Details" Font-Bold="true">
<div class="row" style="width:99%">
    <div class="cellInput" style="width:20%"></div>
    <asp:RadioButton  ID="rdCDI" runat="server" Text="CDI Payment" TextAlign="Right" Width="20%" Checked="true" Font-Bold="false"/>
    <asp:RadioButton  ID="rdOtherPayment" runat="server" Text="Other Payment" Enabled="false" TextAlign="Right" Width="20%" Font-Bold="false"/>
</div>
<br />
<br />
<div class="row" style="width:99%"> 
     <div class="cellInput TitleText" style="width:20%">Expense Type</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLDropDownList ID="ddlExpenseType" runat="server" style="width:100%;"></SAHL:SAHLDropDownList>                
    </div> 
     <div class="cellInput" style="width:10%"></div>
     <div class="cellInput TitleText" style="width:15%">Account Type</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLDropDownList ID="ddlAccountType" runat="server" Enabled="false" style="width:100%;"></SAHL:SAHLDropDownList>                
    </div> 
</div>
<div class="row" style="width:99%">    
     <div class="cellInput TitleText" style="width:20%">Expense Name</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLTextBox ID="txtExpenseName" runat="server" style="width:100%;"></SAHL:SAHLTextBox>                
    </div> 
     <div class="cellInput" style="width:10%"></div>
     <div class="cellInput TitleText" style="width:15%">Bank</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLDropDownList ID="ddlBank" runat="server" Enabled="false" style="width:100%;"></SAHL:SAHLDropDownList>                
    </div> 

</div>
<div class="row" style="width:99%">    
     <div class="cellInput TitleText" style="width:20%">Expense Account Number</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLTextBox ID="txtExpenseAccountNumber" runat="server" style="width:100%;"></SAHL:SAHLTextBox>                
    </div> 
     <div class="cellInput" style="width:10%"></div>
     <div class="cellInput TitleText" style="width:15%">Branch</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLDropDownList ID="ddlBranch" runat="server" Enabled="false" style="width:100%;"></SAHL:SAHLDropDownList>                
    </div> 
</div>
<div class="row" style="width:99%">    
     <div class="cellInput TitleText" style="width:20%">Amount</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLCurrencyBox ID="txtAmount" runat="server" style="width:100%;">0</SAHL:SAHLCurrencyBox>                
    </div> 
     <div class="cellInput" style="width:10%"></div>
     <div class="cellInput TitleText" style="width:15%">Account Name</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLTextBox ID="txtAccountName" runat="server" Enabled="false" style="width:100%;"></SAHL:SAHLTextBox>                
    </div> 
</div>
<div class="row" style="width:99%">    
     <div class="cellInput TitleText" style="width:20%">Third Party Name</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLTextBox ID="txtThirdPartyName" runat="server" Enabled="false" style="width:100%;"></SAHL:SAHLTextBox>                
    </div> 
     <div class="cellInput" style="width:10%"></div>
     <div class="cellInput TitleText" style="width:15%">Account Number</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLTextBox ID="txtAccountNumber" runat="server" Enabled="false" style="width:100%;"></SAHL:SAHLTextBox>                
    </div> 
</div>
<div class="row" style="width:99%">    
     <div class="cellInput TitleText" style="width:20%">Third CDI Number</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLTextBox ID="txtCDINumber" runat="server" Enabled="false" style="width:100%;"></SAHL:SAHLTextBox>                
    </div> 
</div>   
<div class="row" style="width:99%">    
     <div class="cellInput TitleText" style="width:20%">Third CDI Reference</div>
     <div class="cellInput" style="width:150px;">
        <SAHL:SAHLTextBox ID="txtThirdPartyReference" runat="server" Enabled="false" style="width:100%;"></SAHL:SAHLTextBox>                
    </div> 
</div> 
</asp:Panel>
</asp:Panel>
<div class="buttonBar" style="width:99%">
<SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" CausesValidation="false" />
<SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" CausesValidation="false" />
</div>
</div>
</asp:Content>