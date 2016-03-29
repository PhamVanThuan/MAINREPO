<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
	Inherits="SAHL.Web.Views.CreditProtectionPlan.CreditProtectionSummary" Title="Credit Life Policy Summary"
	CodeBehind="CreditProtectionSummary.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
	<div class="TableHeaderB">Credit Life Policy Details</div>
	<table class="tableStandard">
		<tr>
			<td class="TitleText" style="width:180px;">Policy Number</td>
			<td><SAHL:SAHLLabel runat="server" ID="lblPolicyNumber"></SAHL:SAHLLabel></td>
		</tr>
		<tr>
			<td class="TitleText">Open Date</td>
			<td><SAHL:SAHLLabel runat="server" ID="lblOpenDate"></SAHL:SAHLLabel></td>
		</tr>
		<tr>
			<td class="TitleText" style="width:180px;">Account Status</td>
			<td><SAHL:SAHLLabel runat="server" ID="lblAccountStatus"></SAHL:SAHLLabel></td>
		</tr>
		<tr>
			<td class="TitleText">Life Policy Premium</td>
			<td><SAHL:SAHLLabel runat="server" ID="lblLifePolicyPremium"></SAHL:SAHLLabel></td>
		</tr>
		<tr>
			<td class="TitleText">Sum Insured</td>
			<td><SAHL:SAHLLabel runat="server" ID="lblSumInsured"></SAHL:SAHLLabel></td>
		</tr>
        <tr>
        <td style="width:600px;" colspan="2">
        <br />
        <SAHL:SAHLGridView ID="LifePolicyClaimGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
            EnableViewState="false" GridHeight="120px" GridWidth="100%" Width="100%" PostBackType="None"
            HeaderCaption="Life Policy Claim Details" EmptyDataSetMessage="There are no Life Policy Claim Details." 
            OnRowDataBound="LifePolicyClaimGrid_RowDataBound">
            <HeaderStyle CssClass="TableHeaderB" />
            <RowStyle CssClass="TableRowA" />   
        </SAHL:SAHLGridView>
        </td>
        </tr>
	</table>
</asp:Content>
