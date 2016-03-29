<%@ Control Language="C#" CodeFile="ManageLoanRates.ascx.cs" Inherits="LoanRates_ascx" %>
<%@ Register TagPrefix="uc1" TagName="toolbar" Src="toolbar.ascx" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table width="100%">
    <tr>
        <td class="ManagementPanelTitle">
            Rates</td>
        <td align="right">
            <uc1:toolbar ID="ucToolbar" runat="server" />
            &nbsp;</td>
    </tr>
</table>
<asp:Panel ID="pnlDisplay" runat="server">
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="oRateData">
        <HeaderTemplate>
            <table border="0" cellpadding="2" cellspacing="0" align="center" width="380" class="Normal">
                <tr>
                    <td align="right">
                        <b>Link Rate:</b></td>
                    <td>
                        <%Response.Write(getMargin());%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <table border="0" width="100%" class="Normal">
                            <tr>
                                <td class="TableRowSeperator" align="right" nowrap="nowrap">
                                    <b>Loan Type:</b></td>
                            </tr>
                            <tr>
                                <td class="TableRowSeperator" align="right" nowrap="nowrap">
                                    <b>Market Rate:</b></td>
                            </tr>
                            <tr>
                                <td class="TableRowSeperator" align="right" nowrap="nowrap">
                                    <b>Loan Rate:</b></td>
                            </tr>
                        </table>
                    </td>
        </HeaderTemplate>
        <ItemTemplate>
            <td>
                <table border="0" cellpadding="2" cellspacing="0" width="100%" class="Normal">
                    <tr>
                        <td class="TableRowSeperator" nowrap="nowrap">
                            <b>
                                <%# ((string)DataBinder.Eval(Container.DataItem, "description")).Replace("(rounded)","") %>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableRowSeperator" nowrap="nowrap">
                            <%# ((string)DataBinder.Eval(Container.DataItem, "MarketRateDesc")).Replace("(rounded)","") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableRowSeperator">
                            <%# Convert.ToDouble( DataBinder.Eval(Container.DataItem, "LoanRate")).ToString("P") %>
                        </td>
                    </tr>
                </table>
            </td>
        </ItemTemplate>
        <FooterTemplate>
            </td></tr> </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Panel>
<asp:Panel ID="pnlEdit" runat="server">
    <table cellspacing="0" cellpadding="2" align="center" width="380" class="Normal">
        <tr>
            <td align="right" class="TableRowSeperator" style="height: 25px">
                <b>Link Rate:</b></td>
            <td class="TableRowSeperator" style="height: 25px">
                <asp:DropDownList ID="ddMargin" runat="server" DataSourceID="oMargin" DataMember="DefaultView"
                    DataTextField="LinkRateDescription" DataValueField="RateConfigurationKey">
                </asp:DropDownList>
            </td>
            <td class="TableRowSeperator" style="height: 25px">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right" class="TableRowSeperator">
                &nbsp;</td>
            <td class="TableRowSeperator">
                <b>Variable</b>
                <asp:HiddenField ID="hfVarFinService" runat="server" />
            </td>
            <td class="TableRowSeperator">
                <b>Fixed</b><asp:HiddenField ID="hfFlexiFinService" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right" class="TableRowSeperator">
                <b>Market Rate:</b></td>
            <td class="TableRowSeperator">
                <asp:Label ID="lblMarketRateVar" runat="server"></asp:Label>
            </td>
            <td class="TableRowSeperator">
                <asp:Label ID="lblMarketRateFlexi" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="TableRowSeperator">
                <b>Loan Rate:</b></td>
            <td class="TableRowSeperator">
                <asp:Label ID="lblLoanRateVar" runat="server"></asp:Label>
            </td>
            <td class="TableRowSeperator">
                <asp:Label ID="lblLoanRateFlexi" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:SqlDataSource ID="oRateData" runat="server" ProviderName="System.Data.SqlClient"
    SelectCommand="&#10;select mortgageloan.financialservicekey,&#10;       financialservicetype.description,&#10;       marketrate.value 'MarketRate',&#10;       marketrate.description 'MarketRateDesc',&#10;       margin.value 'Margin',&#10;      &#13;&#10; convert (varchar(5),(margin.value + Discount) * 100 ) + '%'  as  'MarginDesc',&#10;       interestrate 'LoanRate',&#10;       mortgageloan.rateconfigurationkey&#10;from mortgageloan&#10;inner join rateconfiguration on rateconfiguration.rateconfigurationkey = mortgageloan.rateconfigurationkey&#10;inner join marketrate on marketrate.marketratekey = rateconfiguration.marketratekey&#10;inner join margin on margin.marginkey = rateconfiguration.marginkey&#10;inner join financialservice on financialservice.financialservicekey = mortgageloan.financialservicekey&#10;inner join financialservicetype on financialservicetype.financialservicetypekey = financialservice.financialservicetypekey&#10;where financialservice .accountkey = @accountkey&#13;&#10;-- and mortgageloan.closedate is null"
    UpdateCommand="update mortgageloan &#10;set rateconfigurationkey = @Margin &#10;where financialservicekey = @VarFinService;&#10;update mortgageloan &#10;set rateconfigurationkey = @Margin &#10;where financialservicekey = @FlexiFinService;"
    EnableViewState="False">
    <UpdateParameters>
        <asp:ControlParameter Name="Margin" ControlID="ddMargin" PropertyName="SelectedValue">
        </asp:ControlParameter>
        <asp:ControlParameter Name="VarFinService" ControlID="hfVarFinService" PropertyName="Value">
        </asp:ControlParameter>
        <asp:ControlParameter Name="FlexiFinService" ControlID="hfFlexiFinService" PropertyName="Value">
        </asp:ControlParameter>
    </UpdateParameters>
    <SelectParameters>
        <asp:QueryStringParameter Name="accountkey" DefaultValue="0" QueryStringField="param0">
        </asp:QueryStringParameter>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="oMargin" SelectCommandType="StoredProcedure" SelectCommand="c_GetLoanRatesPerSPVSourceProduct"
    runat="server">
    <SelectParameters>
        <asp:QueryStringParameter Name="LoanNumber" DefaultValue="0" QueryStringField="param0" />
        <asp:Parameter Name="DB" Type="string" DefaultValue="2AM" />
    </SelectParameters>
</asp:SqlDataSource>
&nbsp; 