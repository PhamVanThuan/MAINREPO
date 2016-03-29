<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="ExternalRoleUpdate.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.ExternalRoleUpdate" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
	<script language="javascript" type="text/javascript">
		$(document).ready(function () {
			$("#<%= btnUpdate.ClientID %>").attr("disabled", true);

			$("input:checkbox").click(function () {
				if ($("input:checked").length > 0) {
					$("#<%= btnUpdate.ClientID %>").attr("disabled", false);
				}
				else {
					$("#<%= btnUpdate.ClientID %>").attr("disabled", true);
				}
			});
		});
	</script>
    <asp:Panel ID="pnlUpdate" runat="server" Visible="true" Width="99%">
        <SAHL:SAHLGridView ID="grdDebtCounsel" runat="server" AutoGenerateColumns="false"
            FixedHeader="false" EnableViewState="false" GridWidth="100%" Width="100%" HeaderCaption="Update Role"
            PostBackType="None" NullDataSetMessage="There are no Debt Counselling records."
            EmptyDataSetMessage="There are no Debt Counselling records." OnRowDataBound="grdDebtCounsel_OnRowDataBound">
            <HeaderStyle CssClass="TableHeaderB" />
            <RowStyle CssClass="TableRowA" />
        </SAHL:SAHLGridView>
    </asp:Panel>
    <table style="width: 100%">
        <tr>
            <td align="right">
                <SAHL:SAHLButton ID="btnSelect" runat="server" Text="Select" OnClick="btnSelect_Click"></SAHL:SAHLButton>
                <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" Enabled="false"></SAHL:SAHLButton>
            </td>
        </tr>
    </table>
</asp:Content>
