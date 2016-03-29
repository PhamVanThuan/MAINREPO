<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Administration.Views_BatchProgress" Title="Batch Progress"
    Codebehind="BatchProgress.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 99%;" valign="top">
                <table style="width: 100%" class="tableStandard">
                    <tr>
                        <td style="width: 176px;">
                            <SAHL:SAHLLabel ID="BatchTypeListTitle" runat="server" Text="Batch Type"></SAHL:SAHLLabel>
                        </td>
                        <td style="width: 300px;">
                            <SAHL:SAHLDropDownList ID="BatchTypeList" runat="server" Enabled="true" Style="width: 100%;"
                                PleaseSelectItem="false" AutoPostBack="True" OnSelectedIndexChanged="BatchTypeList_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td style="width: 294px;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <SAHL:SAHLGridView ID="BatchGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                                EnableViewState="false" GridHeight="100px" GridWidth="100%" Width="100%" HeaderCaption="Batch Records"
                                PostBackType="SingleClick" SelectFirstRow="true" NullDataSetMessage="" EmptyDataSetMessage="There are no Batch records."
                                OnRowDataBound="BatchGrid_RowDataBound" OnSelectedIndexChanged="BatchGrid_SelectedIndexChanged">
                                <HeaderStyle CssClass="TableHeaderB" />
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 176px;">
                            <SAHL:SAHLLabel ID="DescriptionTitle" runat="server" Text="Description"></SAHL:SAHLLabel>
                        </td>
                        <td style="width: 243px;">
                            <SAHL:SAHLLabel ID="Description" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td style="width: 15px;">
                            &nbsp;
                        </td>
                        <td style="width: 150px;">
                            &nbsp;
                        </td>
                        <td style="width: 186px;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="EffectiveDateTitle" runat="server" 
                                Text="Effective Date"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="EffectiveDate" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="StartedTitle" runat="server" Text="Started"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="Started" runat="server"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="BatchStatusTitle" runat="server"  Text="Batch Status"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="BatchStatus" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="FinishedTitle" runat="server" Text="Finished"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="Finished" runat="server"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="FileNameTitle" runat="server" Text="File Name"></SAHL:SAHLLabel>
                        </td>
                        <td colspan="4">
                            <SAHL:SAHLLabel ID="FileName" runat="server"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="MessageTypeListTitle" runat="server" 
                                Text="Message Type"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="MessageTypeList" runat="server" Style="width: 100%;" PleaseSelectItem="false"
                                AutoPostBack="True" OnSelectedIndexChanged="MessageTypeList_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <SAHL:SAHLGridView ID="MessageGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                                EnableViewState="false" GridHeight="100px" GridWidth="100%" Width="100%" HeaderCaption="Messages"
                                NullDataSetMessage="Select a Message Type to display the associated messages."
                                OnRowDataBound="MessageGrid_RowDataBound" EmptyDataSetMessage="There are no messages.">
                                <HeaderStyle CssClass="TableHeaderB" />
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="RefreshButton" runat="server" Text="Refresh" AccessKey="R" OnClick="RefreshButton_Click"
                    Visible="true" />
            </td>
        </tr>
    </table>
</asp:Content>
