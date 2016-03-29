<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="LeadReassign.aspx.cs" Inherits="SAHL.Web.Views.Life.LeadReassign"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: center">
        <asp:Panel ID="pnlReassign" runat="server" Width="100%">
            <table width="100%" class="tableStandard">
                <tr>
                    <td align="left" colspan="2" style="text-align: left">
                        <asp:Label ID="lblHeading1" runat="server" Text="Search for Applications and reassign them to the selected Consultant."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2" style="text-align: left">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left" colspan="2" style="height: 99%;" valign="top">
                        <table border="0">
                            <tr>
                                <td align="left" class="TitleText" style="width: 195px">
                                </td>
                                <td align="left" style="width: 409px"  colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText" >
                                    <SAHL:SAHLLabel ID="lblAccountNumber" runat="server" CssClass="LabelText" Font-Bold="True">Account Number</SAHL:SAHLLabel></td>
                                <td align="left"  colspan="2">
                                    <SAHL:SAHLTextBox ID="txtAccountNumber" runat="server" Style="width: 200px;" MaxLength="7"
                                        DisplayInputType="Number"></SAHL:SAHLTextBox></td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" CssClass="LabelText" Font-Bold="True">Application Status</SAHL:SAHLLabel></td>
                                <td align="left"  colspan="2">
                                    <SAHL:SAHLDropDownList ID="ddlApplicationStatus" runat="server" CssClass="CboText"
                                        PleaseSelectItem="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText" Font-Bold="True">Consultant</SAHL:SAHLLabel></td>
                                <td align="left" colspan="2">
                                    <SAHL:SAHLDropDownList ID="ddlSearchConsultant" runat="server" CssClass="CboText"
                                        PleaseSelectItem="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TitleText">
                                    <SAHL:SAHLLabel ID="lblClientName" runat="server" CssClass="LabelText" Font-Bold="True">Client Name</SAHL:SAHLLabel></td>
                                <td align="left" colspan="2">
                                    <SAHL:SAHLTextBox ID="txtClientName" runat="server" Style="width: 400px;" MaxLength="50"></SAHL:SAHLTextBox>
                                </td>
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
                                <td align="right">
                                    <SAHL:SAHLButton ID="btnSearch" runat="server" Text="Search" AccessKey="S" OnClick="btnSearch_Click" />
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
                            GridHeight="250px" GridWidth="100%" Width="100%" HeaderCaption="Search Results"
                            PostBackType="NoneWithClientSelect" EmptyDataSetMessage="No Applications were found for the selected search criteria"
                            OnRowDataBound="SearchGrid_RowDataBound" Invisible="False" SelectFirstRow="True"
                            EmptyDataText="No Applications were found for the selected search criteria">
                            <RowStyle CssClass="TableRowA" />
                        </SAHL:SAHLGridView>
                    </td>
                </tr>
                <tr id="ButtonRow" runat="server">
                    <td align="left" class="TitleText">
                        <SAHL:SAHLLabel ID="lblConsultant" runat="server" CssClass="LabelText">Reassign Lead(s) to Consultant</SAHL:SAHLLabel>
                        <SAHL:SAHLDropDownList ID="ddlConsultant" runat="server" CssClass="CboText">
                        </SAHL:SAHLDropDownList>&nbsp;
                    </td>
                    <td align="right">
                        &nbsp; &nbsp;<SAHL:SAHLButton ID="btnReassignLeads" runat="server" Text="Reassign Lead(s)"
                            AccessKey="L" OnClick="btnReassignLeads_Click" ButtonSize="Size5" Visible="False" />
                        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click"
                            CausesValidation="False" />&nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlConfirmation" runat="server" Width="100%" DefaultButton="btnConfirm"
            HorizontalAlign="Center">
            <div style="height: 20px">
            </div>
            <table style="width: 100%">
                <tr>
                    <td align="center">
                        <SAHL:SAHLLabel ID="lblConfirmationMessage1" runat="server" Text="" CssClass="TitleText"></SAHL:SAHLLabel>
                        <br />
                        <br />
                        <SAHL:SAHLLabel ID="lblConfirmationMessage2" runat="server" Text=""></SAHL:SAHLLabel>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <SAHL:SAHLButton ID="btnConfirm" runat="server" Text="Return" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <br />
                        <br />
                        If you dont press 'return', you will be redirected back to the reassign screen in<input
                            id="txtSecsRemaining" readonly="readonly" style="width: 18px; text-align: center;"
                            type="text" />seconds.
                    </td>
                </tr>
            </table>

            <script language="javascript" type="text/javascript">
                function NavigateBack()
                {
                    document.getElementById('<%=btnConfirm.ClientID %>').click();
                }
                setInterval("NavigateBack()",5000)
                
                /* script to show time remaining until next refresh */
                var seconds=4
                document.getElementById("txtSecsRemaining").value='5'
                function ShowTimeRemaining()
                {
                    document.getElementById("txtSecsRemaining").value=seconds
                    seconds-=1
                }

                setInterval("ShowTimeRemaining()",1000)
            </script>

        </asp:Panel>
    </div>
</asp:Content>
