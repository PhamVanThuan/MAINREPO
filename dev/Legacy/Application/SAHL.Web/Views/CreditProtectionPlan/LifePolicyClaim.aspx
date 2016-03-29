<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
	Inherits="SAHL.Web.Views.CreditProtectionPlan.LifePolicyClaim" Title="Credit Life Policy Claim Summary"
	CodeBehind="LifePolicyClaim.aspx.cs"%>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:HiddenField ID="hiLifePolicyClaimKey" runat="server" EnableViewState="False" />
    <div class="TableHeaderB">Credit Life Policy Claim Details</div>
	<table class="tableStandard">
        <tr>
        <td align="left" style="width:600px;" valign="top">
        <SAHL:SAHLGridView ID="LifePolicyClaimGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
            EnableViewState="false" GridHeight="120px" GridWidth="100%" Width="100%" SelectFirstRow="true"
            HeaderCaption="Life Policy Claim Details" NullDataSetMessage="" EmptyDataSetMessage="There are no Life Policy Claim Details." 
            OnSelectedIndexChanged="LifePolicyClaimGrid_SelectedIndexChanged" OnRowDataBound="LifePolicyClaimGrid_RowDataBound">
            <HeaderStyle CssClass="TableHeaderB" />
            <RowStyle CssClass="TableRowA" />   
        </SAHL:SAHLGridView>
        <br />
        <table class="tableStandard">
            <tr>
                <td class="TitleText">Claim Type</td>
                <td>
                    <SAHL:SAHLLabel ID="lblClaimType" runat="server" Visible="false">-</SAHL:SAHLLabel>
                    <SAHL:SAHLDropDownList ID="ddlClaimType" runat="server" Mandatory="True" Visible="false"></SAHL:SAHLDropDownList>
                </td>
            </tr>
            <tr>
                <td class="TitleText">Claim Status</td>
                <td>
                    <SAHL:SAHLLabel ID="lblClaimStatus" runat="server" Visible="false">-</SAHL:SAHLLabel>
                    <SAHL:SAHLDropDownList ID="ddlClaimStatus" runat="server" Mandatory="True" Visible="false"></SAHL:SAHLDropDownList>
                </td>
            </tr>
            <tr>
                <td class="TitleText">Claim Date</td>
                <td>
                    <SAHL:SAHLLabel ID="lblClaimStatusDate" runat="server" Visible="false">-</SAHL:SAHLLabel>
                    <SAHL:SAHLDateBox ID="dtClaimStatusDate" runat="server" Mandatory="true" Visible="false"/>
                </td>
            </tr>
        </table>

        </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right" style="width: 99%">
                <SAHL:SAHLButton runat="server" ID="SubmitButton" Text="Save" OnClick="btnSubmit_Click" />
                <SAHL:SAHLButton runat="server" ID="CancelButton" Text="Cancel" OnClick="btnCancel_Click" />
            </td>
        <td>&nbsp;</td>
        </tr>
	</table>
</asp:Content>
