<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.DocumentChecklist" Title="Document Checklist"
    Codebehind="DocumentChecklist.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td style="width: 100%; height: 450px">
                <SAHL:SAHLGridView ID="DocCheckListGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
                EnableViewState="false" GridHeight="99%" GridWidth="99%" Width="100%"
                HeaderCaption="Documents Required"
                NullDataSetMessage=""
                EmptyDataSetMessage="No applicable Documents found." OnRowDataBound="DocCheckListGrid_RowDataBound">
                <HeaderStyle CssClass="TableHeaderB" />
                <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                    CausesValidation="false" />
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
