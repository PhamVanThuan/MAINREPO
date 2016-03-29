<%@ Control Language="C#" CodeFile="ManageLoanBreakdown.ascx.cs" Inherits="ManageLoanBreakdown_ascx" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table width="100%">
    <tr>
        <td class="ManagementPanelTitle">
            Breakdown</td>
        <td align="right">
            &nbsp;</td>
    </tr>
</table>
<asp:Repeater ID="Repeater1" Runat="server" DataSourceID="oLoanBreakdown">
 <HeaderTemplate>
             <table border=0 cellpadding=2 cellspacing=0 align=center width=380 class=Normal>
                <tr><td align=right>
         
                <table border=0 width=100% class=normal>
                <tr>
                <td  class="TableRowSeperator" align=right nowrap="nowrap" visible="false"><b>Loan Type:</b></td>
              </tr>
             <tr>
                <td  class="TableRowSeperator" align=right nowrap="nowrap"><b>Current Balance:</b></td>
              </tr>
              
             <tr>
                <td  class="TableRowSeperator" align=right nowrap="nowrap"><b>Initial Balance:</b></td>
             </tr>
             <tr>
                <td  class="TableRowSeperator" align=right nowrap="nowrap"><b>Arrear Balance:</b></td>
             </tr>
             <tr>
                <td  class="TableRowSeperator" align=right nowrap="nowrap"><b>Initial Term:</b></td>
             </tr>
             <tr>
                <td  class="TableRowSeperator" align=right nowrap="nowrap"><b>Remaining Term:</b></td>
             </tr>
             <tr>
                <td  class="TableRowSeperator" align=right nowrap="nowrap"><b>Installment:</b></td>
             </tr>
             </table>
         
             </td>
             
          </HeaderTemplate>
          
          <ItemTemplate>
          <td>
             <table border=0 width=100% class=Normal>
             <tr>
                <td  class="TableRowSeperator" align=right><b><%# DataBinder.Eval(Container.DataItem, "description").ToString() %></b></td>
              </tr>
             <tr>
                <td  class="TableRowSeperator" align=right><%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "CurrentBalance")).ToString(Constants.CURRENCY_FORMAT) %></td>
              </tr>
              
             <tr>
                <td  class="TableRowSeperator" align=right><%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "InitialBalance")).ToString(Constants.CURRENCY_FORMAT) %></td>
             </tr>
               <tr>
                <td  class="TableRowSeperator" align=right><%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "ArrearBalance")).ToString(Constants.CURRENCY_FORMAT) %></td>
             </tr>
             <tr>
                <td  class="TableRowSeperator" align=right><%# DataBinder.Eval(Container.DataItem, "InitialInstallments") %> months</td>
             </tr>
             <tr>
                <td  class="TableRowSeperator" align=right><%# DataBinder.Eval(Container.DataItem, "RemainingInstallments") %> months</td>
             </tr>
             <tr>
                <td  class="TableRowSeperator" align=right><%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "Payment")).ToString(Constants.CURRENCY_FORMAT)%></td>
             </tr>
             
             </table>
             
             </td>
          </ItemTemplate>
          
           <FooterTemplate>
           
           </td></tr>
             </table>
          </FooterTemplate>


</asp:Repeater>
<table width="100%">
    <tr>
        <td class="ManagementPanelTitle">
            </td>
        <td align="right">
            &nbsp;</td>
    </tr>
</table>
<asp:SqlDataSource ID="oLoanBreakdown" SelectCommand="select currentbalance,&#13;&#10;description,&#10; ArrearBalance,&#10;InitialBalance,&#10;InitialInstallments,&#10;RemainingInstallments,&#10;Payment&#10;from mortgageloan&#10;inner join financialService on financialService.financialservicekey = mortgageloan.financialservicekey&#13;&#10;inner join financialservicetype on financialservicetype.financialservicetypekey = financialservice.financialservicetypekey&#10;where financialService.accountkey = @accountkey" ProviderName="System.Data.SqlClient"
    Runat="server">
    <SelectParameters>
        <asp:QueryStringParameter Name="accountkey" QueryStringField="param0"></asp:QueryStringParameter>
    </SelectParameters>
</asp:SqlDataSource>
<!--<img height="0" src="" width="450" />-->
