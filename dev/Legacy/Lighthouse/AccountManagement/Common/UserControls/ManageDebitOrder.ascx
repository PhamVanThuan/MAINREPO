<%@ Control Language="C#" CodeFile="ManageDebitOrder.ascx.cs" Inherits="ManageDebitOrder_ascx" %>
<%@ Register TagPrefix="uc1" TagName="toolbar" Src="toolbar.ascx" %>
<title>DebitOrder</title>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table width="100%" >
    <tr>
        <td class="ManagementPanelTitle" style="height: 20px">
            Debit Order </td>
        <td align="right" style="height: 20px">
            <uc1:toolbar id="ucToolbar" runat="server"></uc1:toolbar>
            &nbsp;</td>
    </tr>
</table>
<asp:Panel ID="pnlEdit" Runat="server">
<table cellpadding=1 cellspacing=0 align=center class=Normal>
    <tr>
        <td width=100 align="right" class="TableRowSeperator"><b style="width: 100px">Fixed Amount:</b></td>
        <td class="TableRowSeperator">
            <asp:TextBox ID="tbDebitOrderAmount" Runat="server" CausesValidation="True" MaxLength="10" Width="70px" Height="20px"></asp:TextBox><asp:RangeValidator
                ID="rvAmount" runat="server" ControlToValidate="tbDebitOrderAmount" CssClass="ErrorMsg"
                Display="Dynamic" EnableClientScript="False" ErrorMessage="<< Invalid" MaximumValue="99999"
                MinimumValue="0" Type="Double" Height="16px"></asp:RangeValidator>
        </td>
        <td align="right" class="TableRowSeperator"> <b>Day:</b></td>
        <td class="TableRowSeperator">
            <asp:DropDownList ID="ddDebitOrderDay" Runat="server">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>17</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
            </asp:DropDownList>
        </td>
 </tr>
</table>
<table width="100%" >
    <tr>
        <td class="ManagementPanelTitle" style="height: 15px">Suretor Details</td>
    </tr>
</table>
<table cellpadding=1 cellspacing=0 align=center class=Normal>
<tr>
    <td width=100 align="right" class="TableRowSeperator" style="height: 20px"><b style="width: 120px">Suretor Name:</b></td>
     <td class="TableRowSeperator" style="width: 203px; height: 20px">
     <asp:TextBox ID="tbSuretorName" Runat="server" CausesValidation="True" MaxLength="50" Width="200px"></asp:TextBox>
    </td>
</tr>
<tr>
  <td width=100 align="right" class="TableRowSeperator" style="height: 25px"><b style="width: 100px">Suretor ID:</b></td>
    <td class="TableRowSeperator" style="width: 203px; height: 25px">
    <asp:TextBox ID="tbSuretorID" Runat="server" CausesValidation="True" MaxLength="20" Width="120px" Height="20px"></asp:TextBox>
  </td>
</tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlDisplay" Runat="server">


<asp:Repeater ID="Repeater1" Runat="server" DataSourceID="oDebitOrder">
<ItemTemplate>
<table cellpadding=1 cellspacing=0 align=center class=Normal>
    <tr>
        <td width=100 nowrap="nowrap" align="right" class="TableRowSeperator"><b>Fixed Amount:</td>
        <td width=100 class="TableRowSeperator">
         <%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "fixedpayment")).ToString(Constants.CURRENCY_FORMAT) %>   
        </td>
        <td nowrap="nowrap" align="right" class="TableRowSeperator"><b>Day:</b></td>
        <td class="TableRowSeperator">
           <%# DataBinder.Eval(Container.DataItem, "debitorderday") %>  
        </td>
    </tr>
</table>
</ItemTemplate>
</asp:Repeater> 
<table width="100%" style="height: 11px" >
    <tr>
        <td class="ManagementPanelTitle" style="height: 17px">Suretor Details</td>
    </tr>
</table>            
<asp:Repeater ID="Repeater2" Runat="server" DataSourceID="oDebitOrder">
<ItemTemplate>
<table cellpadding=1 cellspacing=0 align=center class=Normal>   
    <tr>
        <td width=220 nowrap="noWrap" align="right" class="TableRowSeperator"><b>Suretor Name: </td>
        <td width=220 nowrap="noWrap" class="TableRowSeperator">
         <%# DataBinder.Eval(Container.DataItem, "LoanSuretorname") %>   
        </td> 
    </tr>
    <tr>
        <td width=220 nowrap="noWrap" align="right" class="TableRowSeperator"><b>Suretor ID : </td>
        <td width=220 nowrap="noWrap" class="TableRowSeperator">
         <%# DataBinder.Eval(Container.DataItem, "LoanSuretorIDNumber") %>   
        </td> 
    </tr>
</table>
</ItemTemplate>
</asp:Repeater></asp:Panel>
<asp:SqlDataSource ID="oDebitOrder" Runat="server" SelectCommand="select top 1 fsb.debitorderday,a.fixedpayment,&#13;&#10;LoanSuretorName,LoanSuretorIDNumber&#13;&#10;from financialservice fs (nolock)&#13;&#10;inner join account a (nolock) on a.accountkey = fs.accountkey &#13;&#10;inner join FinancialServiceBankAccount fsb (nolock) on fsb.FinancialServiceKey = fs.FinancialServiceKey&#13;&#10;inner join SAHLDB..Loan loan (nolock) on Loan.LoanNumber = a.accountkey&#13;&#10;where fs.accountkey = @accountkey" UpdateCommand="update SAHLDB..Loan set LoanSuretorName = @LoanSuretorName,LoanSuretorIDNumber = @LoanSuretorIDNumber where LoanNumber = @accountkey;update account &#10;set fixedpayment = @fixedpayment&#10; where accountkey = @accountkey;&#10;update FinancialServiceBankAccount&#10;set debitorderday = @debitorderday&#10;where financialservicekey IN &#13;&#10;&#9;(select financialservicekey from financialservice where accountkey = @accountkey);">
    <UpdateParameters>
        <asp:ControlParameter Name="fixedpayment" ControlID="tbDebitOrderAmount" PropertyName="Text"></asp:ControlParameter>
        <asp:ControlParameter Name="debitorderday" ControlID="ddDebitOrderDay" PropertyName="SelectedValue"></asp:ControlParameter>
        <asp:ControlParameter Name="LoanSuretorName" ControlID="tbSuretorName" PropertyName="Text" />
        <asp:ControlParameter Name="LoanSuretorIDNumber" ControlID="tbSuretorID" PropertyName="Text" />
        <asp:QueryStringParameter Name="accountkey" DefaultValue="0" QueryStringField="param0"></asp:QueryStringParameter>
    </UpdateParameters>
    <SelectParameters>
        <asp:QueryStringParameter Name="accountkey" DefaultValue="0" QueryStringField="param0"></asp:QueryStringParameter>
    </SelectParameters>
</asp:SqlDataSource>
