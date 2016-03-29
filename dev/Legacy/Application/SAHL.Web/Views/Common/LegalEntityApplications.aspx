<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="LegalEntityApplications.aspx.cs" Inherits="SAHL.Web.Views.Common.LegalEntityApplications"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table class="tableStandard" width="100%">
        <tr>
            <td>
                <SAHL:SAHLGridView ID="ApplicationsGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                    EnableViewState="false" GridHeight="300px" GridWidth="100%" Width="100%" HeaderCaption="Legal Entity Applications"
                    NullDataSetMessage="" EmptyDataSetMessage="There are no Applications." OnRowDataBound="ApplicationsGrid_RowDataBound" OnGridDoubleClick="ApplicationsGrid_GridDoubleClick" PostBackType="DoubleClickWithClientSelect">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
        <tr>
        <td align="right">
            <SAHL:SAHLButton ID="btnShowDetails" runat="server" Text="Show Details" OnClick="btnShowDetails_Click" />
        </td>
        </tr>
    </table>
</asp:Content>
