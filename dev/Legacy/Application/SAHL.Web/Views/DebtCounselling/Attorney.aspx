<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.Master"
	CodeBehind="Attorney.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.Attorney" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="Main" runat="Server">
	<div class="TableHeaderA">
		Litigation Attorney
	</div>
	<div runat="server" id="divAttorneyView">
		<SAHL:SAHLLabel ID="lblSelectedAttorney" runat="server" Font-Bold="true">
		</SAHL:SAHLLabel>
	</div>
	<div runat="server" id="divAttorneyUpdate">
		<table class="tableStandard">
			<tr>
				<td>
					<SAHL:SAHLLabel ID="lblSelectAttorney" runat="server" Font-Bold="true" Text="Please Select an Attorney"></SAHL:SAHLLabel>
				</td>
			</tr>
			<tr>
				<td>
					<SAHL:SAHLDropDownList runat="server" ID="cmbAttornies">
					</SAHL:SAHLDropDownList>
				</td>
			</tr>
			<tr>
				<td>
					<SAHL:SAHLButton runat="server" Style="float: right;" ID="btnUpdate" Text="Update"
						OnClick="OnAttorneyUpdateClicked" />
				</td>
			</tr>
		</table>
	</div>
</asp:Content>
