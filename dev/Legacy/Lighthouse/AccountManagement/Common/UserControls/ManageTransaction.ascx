<%@ Control Language="C#" CodeFile="ManageTransaction.ascx.cs" Inherits="ManageTransaction_ascx" %>
<%@ Register TagPrefix="uc1" TagName="ManageLoan" Src="ManageLoan.ascx" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlOptions" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%" class="Normal">
        <tr>
            <td align="left" class="ManagementPanel" valign="top">
                <table class="Normal">
                    <tr>
                        <td class="ManagementPanelTitle" colspan="2">
                            Transaction Filtering Criteria:
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;<b>Transaction Types:</b>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTransGroupTypes" runat="server" AutoPostBack="True" DataMember="DefaultView"
                                DataSourceID="oTransactionGroups" DataTextField="description" DataValueField="transactiongroupkey">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Financial Service Types</b>:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddllFinancialServiceTypes" runat="server" AutoPostBack="True"
                                DataMember="DefaultView" DataSourceID="oFinServiceTypes" DataTextField="description"
                                DataValueField="financialservicetypekey">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<table style="width: 100%" cellspacing="0" cellpadding="0">
    <tr>
        <td class="ManagementPanel" colspan="3" align="center">
            <asp:Panel ID="pnlTransactionGrid" runat="server" ScrollBars="Auto" Width="100%"
                Height="350px" align="center">
                &nbsp;&nbsp;&nbsp;
                <asp:GridView BORDERCOLORLIGHT="black" BORDERCOLORDARK="black" ID="gvTransactions"
                    runat="server" AutoGenerateColumns="False" DataKeyNames="Number,TransactionTypeNumber"
                    DataSourceID="oTransactions" OnRowDataBound="gvTransactions_RowDataBound" CellPadding="2"
                    EnableViewState="False" CssClass="Normal" OnRowEditing="gvTransactions_RowEditing"
                    ShowFooter="True">
                    <Columns>
                        <asp:ButtonField ButtonType="Image" CommandName="Edit" ImageUrl="~/Common/Images/node.gif"
                            Text="Button" />
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Common/Images/rollback.gif"
                            SelectText="Rollback Transaction" ShowCancelButton="False" ShowSelectButton="True">
                            <ItemStyle Wrap="False" />
                        </asp:CommandField>
                        <asp:BoundField DataField="Number" HeaderText="Number" InsertVisible="False" ReadOnly="True"
                            SortExpression="Number" FooterText="Number">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Service Type" HeaderText="Service Type" SortExpression="Service Type"
                            FooterText="Service Type">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TransactionTypeNumber" HeaderText="TransactionTypeNumber"
                            ReadOnly="True" SortExpression="TransactionTypeNumber" Visible="False" FooterText="TransactionTypeNumber">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AllowRollback" HeaderText="AllowRollback" ReadOnly="True"
                            SortExpression="AllowRollback" ShowHeader="False" Visible="False" FooterText="AllowRollback">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TransactionTypeHTMLColour" HeaderText="TransactionTypeHTMLColour"
                            SortExpression="TransactionTypeHTMLColour" Visible="False" FooterText="TransactionTypeHTMLColour" />
                        <asp:BoundField DataField="Transaction Type" HeaderText="Transaction Type" SortExpression="Transaction Type"
                            FooterText="Transaction Type">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Reference" HeaderText="Reference" ReadOnly="True" SortExpression="Reference"
                            FooterText="Reference">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Changed By" HeaderText="Changed By" SortExpression="Changed By"
                            FooterText="Changed By">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Insert Date" HeaderText="Insert Date" ReadOnly="True"
                            SortExpression="Insert Date" FooterText="Insert Date">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Effective Date" HeaderText="Effective Date" ReadOnly="True"
                            SortExpression="Effective Date" FooterText="Effective Date">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Rate" HeaderText="Rate" ReadOnly="True" SortExpression="Rate"
                            DataFormatString="{0:p}" FooterText="Rate">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Amount" HeaderText="Amount" ReadOnly="True" SortExpression="Amount"
                            DataFormatString="{0:R #,###,###,###,##0.00}" FooterText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Balance" HeaderText="Balance" ReadOnly="True" SortExpression="Balance"
                            DataFormatString="{0:R #,###,###,###,##0.00}" FooterText="Balance">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Account Balance" DataFormatString="{0:R #,###,###,###,##0.00}"
                            FooterText="Account Balance" HeaderText="Account Balance" SortExpression="Account Balance">
                            <HeaderStyle Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Service Type" FooterText="Service Type" HeaderText="Service Type"
                            SortExpression="Service Type">
                            <HeaderStyle Wrap="False" />
                        </asp:BoundField>
                    </Columns>
                    <RowStyle CssClass="TransactionGridRowStyle" />
                    <HeaderStyle CssClass="TransactionGridHeaderStyle" Wrap="False" />
                    <AlternatingRowStyle CssClass="TransactionGridAlternatingRowStyle" />
                    <SelectedRowStyle BackColor="Yellow" />
                    <FooterStyle CssClass="TransactionGridHeaderStyle" Font-Bold="True" />
                </asp:GridView>
            </asp:Panel>
        </td>
    </tr>
</table>
<asp:SqlDataSource ID="oTransactions" runat="server" ProviderName="System.Data.SqlClient"
    SelectCommand="pLoanGetTransactions" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:QueryStringParameter Name="accountKey" DefaultValue="0" QueryStringField="param0"
            Type="Int32"></asp:QueryStringParameter>
        <asp:ControlParameter Name="transactionTypeGroup" DefaultValue="0" Type="Int32" ControlID="ddlTransGroupTypes"
            PropertyName="SelectedValue"></asp:ControlParameter>
        <asp:ControlParameter Name="financialServiceType" DefaultValue="0" Type="Int32" ControlID="ddllFinancialServiceTypes"
            PropertyName="SelectedValue"></asp:ControlParameter>
        <asp:QueryStringParameter DefaultValue="N" Name="returnArrearsTransactions" QueryStringField="param1"
            Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="oTransactionGroups" runat="server" ProviderName="System.Data.SqlClient"
    SelectCommand="select TransactionGroupKey,Description from [2am].fin.TransactionGroup
where TransactionGroupKey in (1,2)
union
select 0,'All'
"></asp:SqlDataSource>
<asp:SqlDataSource ID="oFinServiceTypes" runat="server" SelectCommand="select 	distinct fst.FinancialServiceTypeKey,
	fst.Description
from [2am].dbo.FinancialService fs (nolock)
inner join [2am].dbo.FinancialServiceType fst (nolock) on fst.FinancialServiceTypeKey = fs.FinancialServiceTypeKey
inner join [2am].fin.MortgageLoan ml (nolock) on ml.FinancialServiceKey = fs.FinancialServiceKey
where AccountKey = @accountkey
	and ( fs.CloseDate is null or fst.FinancialServiceTypeKey = 2)
union
select 0,'All'
" ProviderName="System.Data.SqlClient">
    <SelectParameters>
        <asp:QueryStringParameter Name="accountkey" DefaultValue="0" QueryStringField="param0">
        </asp:QueryStringParameter>
    </SelectParameters>
</asp:SqlDataSource>