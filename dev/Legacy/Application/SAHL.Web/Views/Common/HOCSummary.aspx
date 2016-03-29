<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Codebehind="HOCSummary.aspx.cs" Inherits="SAHL.Web.Views.Common.HOCSummary" Title="HOCSummary"%>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table width="100%" class="tableStandard">
<tr><td align="left">

    <table border="0">
        <tr>
            <td class="TitleText" style="width:176px;">
                Account Number
            </td>
            <td style="width:186px;">
                <SAHL:SAHLLabel ID="lblAccountNumber" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Product
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblProduct" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Status
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblStatus" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Open Date
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblOpenDate" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Close Date
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblCloseDate" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Total HOC Sum Insured
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblTotalHOCSumInsured" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="height: 21px">
                Total Premium
            </td>
            <td style="height: 21px">
                <SAHL:SAHLLabel ID="lblTotalPremium" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
    </table>

    <br />
    
    <SAHL:SAHLGridView ID="HOCGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="200px" GridWidth="100%"  Width="100%"
        HeaderCaption="HOC Financial Services" OnRowDataBound="HOCGrid_RowDataBound"
        NullDataSetMessage=""
        EmptyDataSetMessage="There are no Services.">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>

</td></tr></table>
</asp:Content>
