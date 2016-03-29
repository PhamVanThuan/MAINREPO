<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.AffordabilityAssessment.Summary" Title="Assessment Summary" CodeBehind="Summary.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" class="PageBlock" width="99%">
        <tr>
            <td align="left" style="height: 99%;" valign="top">

                <SAHL:SAHLGridView ID="AffordabilityAssessmentsGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                    EnableViewState="false" GridHeight="100px" GridWidth="100%" Width="100%"
                    HeaderCaption="Affordability Assessments"
                    NullDataSetMessage=""
                    EmptyDataSetMessage="There are no affordability assessments."
                    OnRowDataBound="AffordabilityAssessmentsGrid_RowDataBound">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
    </table>
</asp:Content>