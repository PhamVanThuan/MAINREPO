<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="WorkflowBatchReassign.aspx.cs" Inherits="SAHL.Web.Views.Common.WorkflowBatchReassign"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
	    function SelectRelated(object, debtCounsellingGroupClass) {
	        var checked = $(object).attr('checked');
	        $("." + debtCounsellingGroupClass + " input:checkbox").each(function () {
	                $(this).attr('checked', checked);
	        });
	    }
    </script>
    <div style="text-align: center">
        <table width="100%" class="tableStandard">
            <tr>
                <td align="left" colspan="2" style="height: 99%;" valign="top">
                    <table border="0">
                        <tr>
                            <td align="left" class="TitleText" style="width: 195px">
                            </td>
                            <td align="left" style="width: 409px">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="TitleText" style="width: 195px">
                                <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" CssClass="LabelText" Font-Bold="True">Role Type</SAHL:SAHLLabel>
                            </td>
                            <td align="left" style="width: 409px">
                                <SAHL:SAHLDropDownList ID="ddlRoleType" runat="server" CssClass="CboText" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlRoleType_SelectedIndexChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="TitleText" style="width: 195px">
                                <SAHL:SAHLLabel ID="lblSearchUser" runat="server" CssClass="LabelText" Font-Bold="True">Assigned User</SAHL:SAHLLabel>
                            </td>
                            <td align="left" style="width: 409px">
                                <SAHL:SAHLDropDownList ID="ddlSearchUser" runat="server" CssClass="CboText" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="TitleText" style="width: 195px">
                            </td>
                            <td align="right" style="width: 409px">
                                <SAHL:SAHLButton ID="btnSearch" runat="server" Text="Search" AccessKey="S" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                    <SAHL:SAHLLabel ID="lblSearchResults" runat="server" CssClass="LabelText" Font-Bold="True"
                        ForeColor="Red"></SAHL:SAHLLabel><br />
                    <SAHL:SAHLGridView ID="SearchGrid" runat="server" AutoGenerateColumns="False" FixedHeader="False"
                        GridHeight="350px" GridWidth="100%" Width="150%" ScrollX="True" HeaderCaption="Search Results"
                        PostBackType="NoneWithClientSelect" EmptyDataSetMessage="No Applications were found for the selected search criteria"
                        OnRowDataBound="SearchGrid_RowDataBound" Invisible="False" SelectFirstRow="True"
                        EmptyDataText="No Applications were found for the selected search criteria">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
            <tr id="ButtonRow" runat="server">
                <td align="left" class="TitleText">
                    <SAHL:SAHLLabel ID="lblReassignUser" runat="server" CssClass="LabelText">Reassign to User</SAHL:SAHLLabel>
                    <SAHL:SAHLDropDownList ID="ddlReassignUser" runat="server" CssClass="CboText">
                    </SAHL:SAHLDropDownList>
                </td>
                <td align="right">
                    &nbsp;<SAHL:SAHLButton ID="btnReassignLeads" runat="server" Text="Reassign" AccessKey="L"
                        OnClick="btnReassignLeads_Click" ButtonSize="Size5" Visible="False" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click"
                        CausesValidation="False" />&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
