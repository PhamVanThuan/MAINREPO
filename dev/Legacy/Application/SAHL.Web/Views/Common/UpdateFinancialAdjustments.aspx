<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.master"
    Codebehind="UpdateFinancialAdjustments.aspx.cs" Inherits="SAHL.Web.Views.Common.UpdateFinancialAdjustments" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div>
        <table id="tblUpateFinancialAdjustments" runat="server" visible="true" style="width: 100%;
            height: 100%">
            <tr style="width: 100%; height: 100%">
                <td>
                    <SAHL:DXGridView ID="gvUpdateFinancialAdjustments" runat="server" AutoGenerateColumns="False"
                        Width="100%" PostBackType="None" ClientInstanceName="grid" Settings-ShowTitlePanel="true"
                        SettingsText-Title="Financial Adjustments" Settings-ShowVerticalScrollBar="true" OnRowUpdating="FinancialAdjustmentsGridRowUpdating_RowUpdating">
                        <SettingsEditing Mode="Inline" />
                        <ClientSideEvents RowClick="function(s, e) {s.StartEditRow(e.visibleIndex);}" />
                    </SAHL:DXGridView>
                </td>
            </tr>
        </table>
        <br />
		<div class="info">
			<asp:Label runat="server" ID="lblInformation" style="color:Green; font-weight:bold" />
		</div>
        <table id="tblButton" runat="server" style="width: 100%; height: 100%">
            <tr style="width: 100%; height: 100%">
                <td style="width: 75%;">
                </td>
                <td style="width: 25%;" align="right">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Visible="true" Text="Cancel" OnClick="btnCancel_Click" />
                    <SAHL:SAHLButton ID="btnUpdate" runat="server" Visible="true" Text="Update" OnClick="btnUpdate_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
