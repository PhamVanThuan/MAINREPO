<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.AffordabilityAssessment.Delete" Title="Delete Assessment" CodeBehind="Delete.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" class="PageBlock" width="99%">
        <tr>
            <td>
                <SAHL:SAHLGridView ID="UnconfirmedAssessmentGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                    EnableViewState="false" GridHeight="280px" GridWidth="100%" Width="100%"
                    PostBackType="NoneWithClientSelect"
                    HeaderCaption="Unconfirmed Assessments"
                    NullDataSetMessage=""
                    EmptyDataSetMessage="There are no unconfirmed assessments."
                    OnRowDataBound="AssessmentGrid_RowDataBound">
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
                <SAHL:SAHLButton ID="DeleteAssessmentButton" runat="server" Text="Delete Assessment" AccessKey="C" OnClick="DeleteAssessmentButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>