<%@ Control Language="C#" CodeFile="ManageBankAccount.ascx.cs" Inherits="BankAccount_ascx" %>
<%@ Register TagPrefix="uc1" TagName="toolbar" Src="toolbar.ascx" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table width="100%">
    <tr>
        <td  class="ManagementPanelTitle">Bank Account</td>
        <td align="right">
            <uc1:toolbar ID="ucToolbar" Runat="server" /></td>
    </tr>
</table>
<asp:Panel ID="pnlDisplay" Runat="server">
  
<asp:Repeater ID="oBankAccountRepeater" Runat="server" DataSourceID="oBankAccount">
<ItemTemplate>
 
<table border=0 cellpadding=2 cellspacing=0 align=center class=Normal>
        <tr>
            <td width=145 nowrap="noWrap" align="right" class="TableRowSeperator">
                <b>Account Type:</b></td>
            <td width=225 class="TableRowSeperator">
                <%# DataBinder.Eval(Container.DataItem, "acbtypedescription") %>
            </td>
        </tr>
        <tr>
            <td nowrap="noWrap" align="right" class="TableRowSeperator">
                <b>Bank:</b></td>
            <td class="TableRowSeperator">
                <%# DataBinder.Eval(Container.DataItem, "acbbankdescription") %>
            </td>
        </tr>
        <tr>
            <td nowrap="noWrap" align="right" class="TableRowSeperator">
                <b>Branch:</b></td>
            <td class="TableRowSeperator">
                <%# DataBinder.Eval(Container.DataItem, "acbbranchdescription") %>
            </td>
        </tr>
        <tr>
            <td nowrap="noWrap" align="right" class="TableRowSeperator">
                <b>Account Number:</td>
            <td class="TableRowSeperator">
                <%# DataBinder.Eval(Container.DataItem, "accountnumber") %>
            </td>
        </tr>
    </table>
 
</ItemTemplate>
</asp:Repeater></asp:Panel>
<asp:Panel ID="pnlEdit" Runat="server">

<table border=0 cellpadding=2 cellspacing=0 align=center class=Normal>
        <tr>
            <td width=145 align="right" class="TableRowSeperator">
                <b> Account Type:</b></td>
            <td width=225 class="TableRowSeperator">
                <asp:DropDownList ID="ddAccountType" Runat="server" DataMember="DefaultView" DataValueField="ACBTypeNumber" DataTextField="ACBTypeDescription" DataSourceID="oACBType">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" class="TableRowSeperator">
                <b> Bank:</b></td>
            <td class="TableRowSeperator">
                <asp:DropDownList ID="ddBank" Runat="server" DataMember="DefaultView" DataValueField="ACBBankCode" DataTextField="ACBBankDescription" DataSourceID="oACBBank" AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" class="TableRowSeperator">
                <b> Branch:</b></td>
            <td class="TableRowSeperator">
                <asp:DropDownList  ID="ddBranch" Runat="server" DataMember="DefaultView" DataValueField="ACBBranchCode" DataTextField="ACBBranchDescription" DataSourceID="oACBBranch">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" class="TableRowSeperator">
                <b> Account Number:</b></td>
            <td class="TableRowSeperator">
                <asp:TextBox ID="tbAccountNumber" Runat="server" CausesValidation="True"></asp:TextBox><asp:RangeValidator
                    ID="rvAccountNumber" runat="server" ControlToValidate="tbAccountNumber" CssClass="ErrorMsg"
                    Display="Dynamic" EnableClientScript="False" ErrorMessage="<< Invalid" MaximumValue="99999999999999999999"
                    MinimumValue="0" Type="Double"></asp:RangeValidator><asp:RequiredFieldValidator ID="rfvAccountNumber"
                        runat="server" ControlToValidate="tbAccountNumber" CssClass="ErrorMsg" Display="Dynamic"
                        EnableClientScript="False" ErrorMessage="<< Required"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    &nbsp;
    <input id="tbBankAccKey" type="hidden" runat="server" /></asp:Panel>
<asp:SqlDataSource ID="oBankAccount" Runat="server" SelectCommand="select &#13;&#10;top 1 ba.bankaccountkey,&#13;&#10;&#9;act.acbtypenumber, &#13;&#10;&#9;act.acbtypedescription, &#13;&#10;&#9;ba.accountnumber, &#13;&#10;&#9;acbr.acbbranchcode, &#13;&#10;&#9;acbr.acbbranchcode + ' - ' + acbr.acbbranchdescription acbbranchdescription,&#13;&#10;&#9;acb.acbbankcode,&#13;&#10;    acb.acbbankdescription&#13;&#10;from bankaccount ba (nolock)&#13;&#10;inner join acbbranch acbr (nolock) on acbr.ACBBranchCode = ba.acbbranchcode&#13;&#10;inner join acbbank acb (nolock) on acb.acbbankcode = acbr.acbbankcode&#13;&#10;inner join acbtype act (nolock) on act.acbtypenumber = ba.acbtypenumber&#13;&#10;inner join FinancialServiceBankAccount fsba (nolock) on fsba.bankaccountkey = ba.bankaccountkey &#13;&#10;inner join financialservice fs (nolock) on fs.FinancialServiceKey = fsba.FinancialServiceKey&#13;&#10;where fs.accountkey = @accountkey"
    ProviderName="System.Data.SqlClient" UpdateCommand="update bankaccount&#10;set acbbranchcode = @branchCode,&#10;accountnumber = @accnum,&#10;acbtypenumber = @type&#10;where bankaccountkey = @bankAccKey" >
    <UpdateParameters>
        <asp:ControlParameter Name="branchCode" ControlID="ddBranch" PropertyName="SelectedValue"></asp:ControlParameter>
        <asp:ControlParameter Name="type" ControlID="ddAccountType" PropertyName="SelectedValue"></asp:ControlParameter>
        <asp:ControlParameter Name="accnum" ControlID="tbAccountNumber" PropertyName="Text"></asp:ControlParameter>
        <asp:ControlParameter Name="bankAccKey" ControlID="tbBankAccKey" PropertyName="Value"></asp:ControlParameter>
    </UpdateParameters>
    <SelectParameters>
        <asp:QueryStringParameter Name="accountkey" DefaultValue="0" QueryStringField="param0"></asp:QueryStringParameter>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="oACBType" 
    SelectCommand="select ACBTypeNumber, ACBTypeDescription from ACBType" Runat="server" DataSourceMode="DataReader">
</asp:SqlDataSource>
<asp:SqlDataSource ID="oACBBank" 
    SelectCommand="select ACBBankCode, ACBBankDescription from acbbank" Runat="server">
</asp:SqlDataSource>
<asp:SqlDataSource ID="oACBBranch" 
    SelectCommand="select ACBBranchCode, ACBBranchCode + ' - ' + Left(ACBBranchDescription,20) ACBBranchDescription,ACBBankCode&#10;from ACBBranch where activeindicator = 0 "
    Runat="server" FilterExpression="ACBbankCode = {0}">
    <FilterParameters>
        <asp:ControlParameter Name="bank" ControlID="ddBank" PropertyName="SelectedValue"></asp:ControlParameter>
    </FilterParameters>
</asp:SqlDataSource>
