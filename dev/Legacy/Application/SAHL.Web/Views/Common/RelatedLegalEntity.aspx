<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="True"
    Inherits="SAHL.Web.Views.Common.RelatedLegalEntity" Title="Untitled Page" Codebehind="RelatedLegalEntity.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td>
                <SAHL:SAHLGridView ID="RelatedLEGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="290px" GridWidth="100%"
                    Width="100%" OnRowDataBound="RelatedLEGrid_RowDataBound" HeaderCaption="Related Legal Entity"
                    NullDataSetMessage="No related legal entities found." EmptyDataSetMessage="No related legal entities found."
                    PostBackType="DoubleClickWithClientSelect" OnGridDoubleClick="RelatedLEGrid_GridDoubleClick">
                    <HeaderStyle CssClass="TableHeaderB" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="RemoveButton" runat="server" AccessKey="A" Text="Remove" OnClick="RemoveButton_Click" />
                <SAHL:SAHLButton ID="AddToMenuButton" runat="server" AccessKey="A" Text="Add to Menu"
                    OnClick="AddToMenuButton_Click" />
                <SAHL:SAHLButton ID="CancelButton" runat="server" AccessKey="A" Text="Cancel" OnClick="CancelButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
