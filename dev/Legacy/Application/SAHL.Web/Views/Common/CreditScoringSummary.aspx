<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    CodeBehind="CreditScoringSummary.aspx.cs" Inherits="SAHL.Web.Views.Common.CreditScoringSummary" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <SAHL:SAHLGridView ID="ScoreGrid" runat="server" AutoGenerateColumns="False" FixedHeader="False"
        HeaderCaption="Credit Scoring Summary" Visible="True" TotalsFooter="True" Width="100%"
        EmptyDataText="There are no credit scores for this offer" GridHeight="320px"
        GridWidth="100%" OnSelectedIndexChanged="ScoreGrid_SelectedIndexChanged" OnRowDataBound="ScoreGrid_RowDataBound"
        EnableViewState="False">
        <RowStyle BorderStyle="Solid" BorderWidth="1px" />
    </SAHL:SAHLGridView>
    <div style="width: 100px">
    </div>
    <SAHL:SAHLGridView ID="ApplicantGrid" runat="server" AutoGenerateColumns="False"
        FixedHeader="False" HeaderCaption="Applicant Decisions" SelectFirstRow="True"
        TotalsFooter="True" Width="100%" GridHeight="90px" GridWidth="100%" OnRowDataBound="ApplicantGrid_RowDataBound"
        EnableViewState="False" GridLines="None" Visible="False">
    </SAHL:SAHLGridView>
    <br />
    <table id="tblCreditScoreValues" runat="server" visible="false" class="tableStandard" style="width: 100%">
        <tr style="width: 100%">
            <td style="width: 15%" class="titleText">
                Risk Matrix
            </td>
            <td style="width: 15%">
                <SAHL:SAHLLabel ID="lblRiskMatrix" runat="server"></SAHL:SAHLLabel>
            </td>
            <td style="width: 70%">
            </td>
        </tr>
        <tr>
            <td class="titleText">
                Risk Matrix Version
            </td>
            <td >
                <SAHL:SAHLLabel ID="lblRiskMatrixVersion" runat="server"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="titleText">
                SBC Score Card
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblScoreCard" runat="server"></SAHL:SAHLLabel>
            </td>
        </tr>
    </table>
    <br />
    <SAHL:SAHLGridView ID="RuleGrid" runat="server" AutoGenerateColumns="False" FixedHeader="False"
        HeaderCaption="Decision Reasons" SelectFirstRow="True" TotalsFooter="True" Width="100%"
        GridHeight="90px" GridWidth="100%" OnRowDataBound="RuleGrid_RowDataBound" EnableViewState="False"
        Visible="False">
    </SAHL:SAHLGridView>
</asp:Content>
