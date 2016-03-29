<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="ProductSummary.aspx.cs" Inherits="SAHL.Web.Views.Common.ProductSummary"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: left">
        <table width="100%" class="tableStandard">
            <tr>
                <td>
                    <sahl:sahlgridview id="SummaryGrid" runat="server" autogeneratecolumns="false" fixedheader="false"
                        enableviewstate="false" gridheight="200px" gridwidth="100%" width="100%" headercaption="Product Summary"
                        nulldatasetmessage="" emptydatasetmessage="There are no Products." onrowdatabound="SummaryGrid_RowDataBound">
                        <HeaderStyle CssClass="TableHeaderB" />
                        <RowStyle CssClass="TableRowA" />
                    </sahl:sahlgridview>
                    <br />
                    <sahl:sahlgridview id="FinancialAdjustmentGrid" runat="server" autogeneratecolumns="false"
                        fixedheader="false" enableviewstate="false" gridheight="150px" gridwidth="100%"
                        width="100%" headercaption="Financial Adjustments" nulldatasetmessage="" emptydatasetmessage="There are no Financial Adjustment entries."
                        onrowdatabound="FinancialAdjustmentGrid_RowDataBound">
                        <RowStyle CssClass="TableRowA" />
                    </sahl:sahlgridview>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>