<%@ Control Language="C#" CodeFile="ManageLoanDetail.ascx.cs" Inherits="ManageLoanDetail_ascx" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table width=100% class=Normal>
    <tr>
        <td class="ManagementPanelTitle">
            Loan Detail</td>
        <td align="right">
            <asp:ImageButton ID="ibManageDetail" runat="server" ImageUrl="~/Common/Images/edit.gif" />&nbsp;</td>
    </tr>
</table>
<div align=center>
<asp:Panel ID="pnlDisplay" Runat="server" ScrollBars=Auto Width="410px" Height="80px" Wrap="False" >
<asp:Repeater ID="rLoanDetail" Runat="server" DataSourceID="oLoanDetail">
<HeaderTemplate>
<table cellpadding=2 cellspacing=0 align=center width=90% class=Normal>
    <tr>
        <td class="TableRowSeperator"><b>Date</b>
        </td>
        <td class="TableRowSeperator" nowrap=nowrap><b>Description</b>
        </td>
        <td class="TableRowSeperator"><b>Amount</b>
        </td>
    </tr>
</HeaderTemplate>
<ItemTemplate>
    <tr>
        <td class="TableRowSeperator"><%# DataBinder.Eval(Container.DataItem, "detaildate") %>
        </td>
        <td class="TableRowSeperator" align=left><%# DataBinder.Eval(Container.DataItem, "detailtypedescription") %>
        </td>
        <td class="TableRowSeperator"><%# DataBinder.Eval(Container.DataItem, "detailamount") %>
        </td>
    </tr>
</ItemTemplate>
<FooterTemplate>
</table>
</FooterTemplate>
</asp:Repeater></asp:Panel><br />
<asp:SqlDataSource ID="oLoanDetail" Runat="server" SelectCommand="select convert(varchar(10),detail.detaildate,103) detaildate,&#10;&#9;   case when (detail.detailTypeNumber < 10 or detail.detailTypeNumber in (213,214,343,412,413)) then&#9;&#10;       detailtype.detailtypedescription + ' - ' + Attorney.AttorneyName&#10;&#9;   else detailtype.DetailTypeDescription end as detailtypedescription&#9;,&#10;       detailamount     &#10;from sahldb..detail detail (nolock)&#10;inner join sahldb..detailtype detailtype (nolock) on detailtype.detailtypenumber = detail.detailtypenumber&#13;&#10;inner join account (nolock) on account.accountkey = detail.loannumber &#10;left join SAHLDB..Regmail Regmail (nolock) on detail.LoanNumber = Regmail.LoanNumber&#10;left join SAHLDB..attorney Attorney (nolock) on Regmail.AttorneyNumber = Attorney.AttorneyNumber&#10;where detail.loannumber = @loannumber&#13;&#10;and ((detail.detailtypenumber <> 10 and account.accountstatuskey <> 2) &#13;&#10;or (account.accountstatuskey = 2)&#13;&#10;and (detail.DetailText not like '%conversion%' or detail.DetailTypeNumber <> 10)&#13;&#10;)&#13;&#10;">
    <SelectParameters>
        <asp:QueryStringParameter Name="loannumber" DefaultValue="0" QueryStringField="param0"></asp:QueryStringParameter>
    </SelectParameters>
</asp:SqlDataSource>
</div>
