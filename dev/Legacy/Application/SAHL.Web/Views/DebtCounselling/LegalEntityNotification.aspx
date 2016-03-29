<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="LegalEntityNotification.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.LegalEntityNotification" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

    </script>
    <asp:Panel ID="pnlUpdate" runat="server" Visible="true" Width="99%">
        <SAHL:SAHLGridView ID="grdLegalEntities" runat="server" AutoGenerateColumns="false"
            FixedHeader="false" EnableViewState="false" GridWidth="100%" Width="100%" HeaderCaption="Update Legal Entity Notification"
            PostBackType="None" NullDataSetMessage="There are no Legal Entity records."
            EmptyDataSetMessage="There are no Legal Entity records." OnRowDataBound="grdLegalEntities_RowDataBound">
            <HeaderStyle CssClass="TableHeaderB" />
            <RowStyle CssClass="TableRowA" />
        </SAHL:SAHLGridView>
    </asp:Panel>
    <table style="width: 100%">
        <tr>
            <td align="right">
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Update" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
