<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.AffordabilityAssessment.History" Title="Assessment History" CodeBehind="History.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" class="PageBlock" width="99%">
        <tr>
            <td>
                <SAHL:SAHLGridView ID="AffordabilityAssessmentsGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                    EnableViewState="false" GridHeight="280px" GridWidth="100%" Width="100%"
                    PostBackType="NoneWithClientSelect"
                    OnRowDataBound="AffordabilityAssessmentsGrid_RowDataBound">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
        <tr>
            <td style="height: 5px"></td>
        </tr>
        <tr id="ButtonRow" runat="server" visible="false">
            <td align="right">
                <SAHL:SAHLButton ID="ViewAssessmentButton" runat="server" Text="View Assessment" AccessKey="C" OnClick="ViewAssessmentButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>