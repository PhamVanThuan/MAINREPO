<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.GroupExposure" Title="Group Exposure" CodeBehind="GroupExposure.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="100%" class="tableStandard">
        <tr class="LabelHeader" align="left">
            <td>
                <SAHL:SAHLLabel ID="lblLegalEntity" Text="Legal Entities playing a role in Account"
                    runat="server" CssClass="LabelText" Font-Bold="True"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <SAHL:SAHLGridView ID="LegalEntityGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="140px" GridWidth="100%"
                    Width="100%" HeaderCaption="" NullDataSetMessage="No Legal Entities." EmptyDataSetMessage="No Legal Entities."
                    OnRowDataBound="LegalEntityGrid_RowDataBound" PostBackType="SingleClick" SelectedIndex="0"
                    OnSelectedIndexChanged="LegalEntityGrid_SelectedIndexChanged">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
            </td>
        </tr>
        <tr class="LabelHeader" align="left">
            <td>
                <SAHL:SAHLLabel ID="lblGroupExposure" Text="Group Exposure" runat="server" CssClass="LabelText"
                    Font-Bold="True"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <SAHL:SAHLGridView ID="GroupExposureGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="180px" GridWidth="100%"
                    Width="150%" HeaderCaption="" NullDataSetMessage="No Group Exposure Details."
                    EmptyDataSetMessage="No Group Exposure Details."
                    OnSelectedIndexChanged="GroupExposureGrid_SelectedIndexChanged"
                    PostBackType="SingleClick"
                    ScrollX="True">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
            </td>
        </tr>
        <tr class="LabelHeader" align="left">
            <td>
                <SAHL:SAHLLabel ID="lblDebtCounsellingHeading" Text="Debt Counselling Messages" runat="server" CssClass="LabelText"
                    Font-Bold="True"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr align="center" valign="top">
            <td>
                <SAHL:SAHLGridView ID="DebtCounsellingGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="50px" GridWidth="100%"
                    Width="100%" HeaderCaption="" NullDataSetMessage="No Debt Counselling Messages." EmptyDataSetMessage="No Debt Counselling Messages."
                    OnRowDataBound="DebtCounsellingGrid_RowDataBound"
                    PostBackType="None">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
            </td>
        </tr>
    </table>
</asp:Content>