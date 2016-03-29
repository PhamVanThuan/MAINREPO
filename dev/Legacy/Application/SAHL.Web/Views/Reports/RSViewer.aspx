<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="RSViewer.aspx.cs" Inherits="SAHL.Web.Views.Reports.RSViewer" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table id="ParameterTable" runat="server" width="100%" class="tableStandard">
        <tr>
            <td align="left" style="height: 99%" valign="top">
                <table width="100%">
                    <tr id="GroupRow" runat="server">
                        <td style="width:100px">
                            <sahl:sahllabel id="lblReportGroup" runat="server" text="Report Group" CssClass="LabelText"></sahl:sahllabel>
                        </td>
                        <td>
                            <sahl:sahldropdownlist id="ddlReportGroup" runat="server" autopostback="True" onselectedindexchanged="ddlReportGroup_SelectedIndexChanged"></sahl:sahldropdownlist>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <sahl:sahlgridview id="tblReports" runat="server" AutoGenerateColumns="false" EmptyDataSetMessage="There are no available reports."
                                EnableViewState="false" fixedheader="false" GridHeight="175px" GridWidth="100%" Width="100%"
                                HeaderCaption="Available Reports" NullDataSetMessage="" OnRowDataBound="tblReports_RowDataBound"
                                OnSelectedIndexChanged="tblReports_SelectedIndexChanged" PostBackType="SingleAndDoubleClick"
                                SelectFirstRow="False" ><ROWSTYLE CssClass="TableRowA" /></sahl:sahlgridview>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" style="height: 99%" valign="top">
                        <br />
                            <asp:Panel ID="pnlParameters" runat="server" BorderColor="White" GroupingText="Report Parameters"
                                Width="99%" Visible="False">
                                <asp:Table ID="tblParameters" runat="server" BackColor="White" Width="100%">
                                </asp:Table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table id="ReportTable" runat="server" width="100%">
                                <tr id="SQLReportRow">
                                    <td>
                                        <iframe id="ReportViewerFrame" runat="server" frameborder="0" height="450" marginheight="0"
                                            marginwidth="0" scrolling="auto" src="" width="100%"></iframe>
                                    </td>
                                </tr>
                                <tr id="DataReportRow">
                                    <td>
                                        <sahl:sahlgridview id="grdDataReport" runat="server" autogeneratecolumns="false"
                                            emptydatasetmessage="There are no rows." enableviewstate="false" fixedheader="false"
                                            gridheight="400px" GridWidth="100%" Width="100%" HeaderCaption="Report" NullDataSetMessage=""
                                            onrowdatabound="grdDataReport_RowDataBound" scrollx="true" selectfirstrow="False"
                                            ><ROWSTYLE CssClass="TableRowA" /></sahl:sahlgridview>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right" colspan="3">
                <sahl:sahlbutton id="CancelButton" runat="server" onclick="CancelButton_Click" text="Cancel"></sahl:sahlbutton>
                <sahl:sahlbutton id="ViewReport" runat="server" onclick="ViewReport_Click" text="View"></sahl:sahlbutton>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
