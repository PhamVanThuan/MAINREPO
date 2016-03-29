<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="LeadCreate.aspx.cs" Inherits="SAHL.Web.Views.Life.LeadCreate" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: center">
        <table width="100%" class="tableStandard">
            <tr>
                <td align="left" colspan="2" style="text-align: left">
                    Search for Loans to create new leads for the selected Consultant. Only loans for which you can create leads will be selectable
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2" style="height: 99%;" valign="top">
                    <table border="0">
                        <tr>
                            <td align="left" class="TitleText" style="width: 195px">
                            </td>
                            <td align="left" style="width: 409px" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="TitleText">
                                <SAHL:SAHLLabel ID="lblAccountNumber" runat="server" CssClass="LabelText" Font-Bold="True">Loan Number</SAHL:SAHLLabel></td>
                            <td align="left"  colspan="2">
                                <SAHL:SAHLTextBox ID="txtAccountNumber" runat="server" Style="width: 200px;" MaxLength="7"
                                    DisplayInputType="Number"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td align="left" class="TitleText">
                                <SAHL:SAHLLabel ID="lblSurname" runat="server" CssClass="LabelText" Font-Bold="True">Surname</SAHL:SAHLLabel></td>
                            <td align="left"  colspan="2">
                                <SAHL:SAHLTextBox ID="txtSurname" runat="server" Style="width: 400px;" MaxLength="50"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="TitleText">
                                <SAHL:SAHLLabel ID="lblFirstNames" runat="server" CssClass="LabelText" Font-Bold="True">First Names</SAHL:SAHLLabel></td>
                            <td align="left"  colspan="2">
                                <SAHL:SAHLTextBox ID="txtFirstnames" runat="server" MaxLength="50" Style="width: 400px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <b>Maximum records returned</b>
                            </td>
                            <td style="width: 50px">
                                <SAHL:SAHLDropDownList ID="selMaxResults" runat="server">
                                    <asp:ListItem Value="50" Selected="True">50</asp:ListItem>
                                    <asp:ListItem Value="100">100</asp:ListItem>
                                    <asp:ListItem Value="150">150</asp:ListItem>
                                    <asp:ListItem Value="200">200</asp:ListItem>
                                    <asp:ListItem Value="250">250</asp:ListItem>
                                    <asp:ListItem Value="300">300</asp:ListItem>
                                    <asp:ListItem Value="350">350</asp:ListItem>
                                    <asp:ListItem Value="400">400</asp:ListItem>
                                    <asp:ListItem Value="450">450</asp:ListItem>
                                    <asp:ListItem Value="500">500</asp:ListItem>
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td align="right" style="height: 26px">
                                <SAHL:SAHLButton ID="btnSearch" runat="server" Text="Search" AccessKey="S" OnClick="btnSearch_Click" />&nbsp;
                                <SAHL:SAHLButton ID="btnClear" runat="server" Text="Clear" AccessKey="S" CausesValidation="False"
                                    OnClientClick="ClearScreen();return false" UseSubmitBehavior="False" />&nbsp;
                            </td>
                        </tr>
                    </table>
                    <div id="divMaxResultsError" runat="server" class="row" visible="false" style="padding-bottom: 5px;">
                        <div class="cell">
                            <img src="../../images/flag_red.png" width="16" height="16" alt="" /></div>
                        <strong>Note: </strong>The query returned more than the supported limit of
                        <asp:Label ID="lblMaxCount" runat="server" />
                        records. Please refine your search criteria.
                    </div>
                    <SAHL:SAHLGridView ID="SearchGrid" runat="server" AutoGenerateColumns="False" FixedHeader="False"
                        GridHeight="290px" GridWidth="100%" Width="120%" HeaderCaption="Search Results"
                        PostBackType="NoneWithClientSelect" EmptyDataSetMessage="No Loans were found for the selected search criteria"
                        OnRowDataBound="SearchGrid_RowDataBound" Invisible="False" SelectFirstRow="True"
                        EmptyDataText="No Loans were found for the selected search criteria" ScrollX="True">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
            <tr id="ButtonRow" runat="server">
                <td align="left" class="TitleText">
                    <SAHL:SAHLLabel ID="lblConsultant" runat="server" CssClass="LabelText">Create Lead(s) for Consultant</SAHL:SAHLLabel>
                    <SAHL:SAHLDropDownList ID="ddlConsultant" runat="server" CssClass="CboText">
                    </SAHL:SAHLDropDownList>&nbsp;
                </td>
                <td align="right">
                    &nbsp;
                    &nbsp;<SAHL:SAHLButton ID="btnCreateLeads" runat="server" Text="Create Lead(s)" AccessKey="L"
                        OnClick="btnCreateLeads_Click" ButtonSize="Size5" Visible="False" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click"
                        CausesValidation="False" />&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
