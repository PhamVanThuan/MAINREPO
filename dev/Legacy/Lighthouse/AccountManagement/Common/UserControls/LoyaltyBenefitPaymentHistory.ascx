<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoyaltyBenefitPaymentHistory.ascx.cs"
    Inherits="LoyaltyBenefitPaymentHistory_ascx" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />

<table width="100%">
    <tr>
        <td class="ManagementPanelTitle" style="height: 21px">
            Loyalty Benefit Payment History</td>
        <td align="right" style="height: 21px">
            &nbsp;</td>
    </tr>
</table>

<asp:Panel ID="pnlTransactionGrid" runat="server" ScrollBars="Auto" Width="875px"
    Height="150px" align="center">
    <asp:GridView BORDERCOLORLIGHT="black" BORDERCOLORDARK="black" ID="gvTransactions"
        runat="server" AutoGenerateColumns="False" DataKeyNames="Number,TransactionTypeNumber" CellPadding="2"
        EnableViewState="False"
        CssClass="Normal" >
        <Columns>
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
        </Columns>
        
        <RowStyle CssClass="TransactionGridRowStyle" />
        <HeaderStyle CssClass="TransactionGridHeaderStyle" Wrap="False" />
        <AlternatingRowStyle CssClass="TransactionGridAlternatingRowStyle" />
        <SelectedRowStyle BackColor="Yellow" />
        <FooterStyle CssClass="TransactionGridHeaderStyle" Font-Bold="True" />
    </asp:GridView>
</asp:Panel>

<table width="100%">
    <tr>
        <td style="text-align: right;height:35px;vertical-align:bottom">
            <asp:HyperLink runat="server" ID="lExit" ToolTip="Exit" ImageUrl="~/common/images/exit.gif">Exit</asp:HyperLink> 
        </td>
        <td style="width:25px">&nbsp;
        </td>
    </tr>
</table>

