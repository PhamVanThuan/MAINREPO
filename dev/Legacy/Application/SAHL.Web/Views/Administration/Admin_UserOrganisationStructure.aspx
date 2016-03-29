<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Administration.Admin_UserOrganisationStructure" Title="User Organisation Structure Adminstration"
    Codebehind="Admin_UserOrganisationStructure.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="uosContent" runat="server" ContentPlaceHolderID="Main">

    <script language="javascript" type="text/javascript">
    
        function doSearch(btnSearchId)
        {
            if (window.event.keyCode == 13)
            {
                window.event.returnValue = false;
                document.getElementById(btnSearchId).click();
            }
        }
        
    </script>
    <div>
    <br />
        <asp:Panel ID="panelADUserSearchHeader" runat="server" Width="99%" Height="100%" CssClass="headerPanel">
            <table>
                <tr>
                    <td valign="middle">
                        <asp:Label ID="label1" runat="server" Text="Search" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="panelADUserSearch" runat="server" Width="100%">
            <br />
            <table id="tblADUserSearch" runat="server" border="0" style="width: 100%; height: 100%">
                <tr style="width: 100%; height: 100%">
                    <td align="left" style="width: 15%;">
                        <SAHL:SAHLLabel ID="lblADUserName" runat="server" CssClass="LabelText" TextAlign="left">ADUser Name</SAHL:SAHLLabel>
                    </td>
                    <td align="left" style="width: 60%;">
                        <SAHL:SAHLTextBox ID="txtADUserName" runat="server" CssClass="SAHLTextBox" TextAlign="Left"> </SAHL:SAHLTextBox>
                    </td>
                    <td align="left" style="width: 25%;">
                        <SAHL:SAHLButton ID="btnADUserSearch" runat="server" Text="Search" OnClick="btnADUserSearch_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="panelADUserSearchResultsHeader" runat="server" Width="99%" Height="100%" CssClass="headerPanel">
            <table>
                <tr>
                    <td valign="middle">
                        <asp:Label ID="label2" runat="server" Text="Results" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="panelADUserSearchResults" runat="server" Width="100%">
        <br />
            <table id="tblADUserSearchResults" runat="server" visible="true" style="width: 100%;
                height: 100%">
                <tr style="width: 100%; height: 100%">
                    <td>
                        <SAHL:SAHLLabel ID="lblHeading" runat="server" Font-Bold="true" CssClass="LabelText"
                            TextAlign="left"></SAHL:SAHLLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <SAHL:DXGridView ID="ADUserResultsGrid" runat="server" AutoGenerateColumns="False" Width="100%"
                            ClientInstanceName="gridADUser" Settings-ShowTitlePanel="true" SettingsText-Title="ADUser Search Results"
                            EnableViewState="false">
                            
                        </SAHL:DXGridView>
                    </td>
                </tr>
            </table>
            <br />
            <table id="tblADUserResultsGridBtn" runat="server" visible="false" style="width: 100%;
                height: 100%">
                <tr>
                    <td align="left" style="width: 75%;">
                    </td>
                    <td align="left" style="width: 25%;">
                        <SAHL:SAHLButton ID="btnViewADUserHist" runat="server" Text="View History" OnClick="btnViewADUserHist_Click" />
                        <SAHL:SAHLButton ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                        <SAHL:SAHLButton ID="btnRemove" runat="server" Text="Remove" OnClick="btnRemove_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="panelOrgStructHeader" runat="server" Width="99%" Height="100%" CssClass="headerPanel" Visible="false">
            <table>
                <tr>
                    <td valign="middle">
                        <asp:Label ID="label3" runat="server" Text="Organisation Structure" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="panelOrgStruct" runat="server" Width="100%" Visible="false" Font-Bold="true">
        <br />
            <table id="tblCompanyList" runat="server" visible="false" style="width: 100%; height: 100%">
                <tr style="width: 100%; height: 100%">
                    <td align="left" style="width: 15%;">
                        <SAHL:SAHLLabel ID="ldlCompany" runat="server" CssClass="LabelText" TextAlign="left">Company</SAHL:SAHLLabel>
                    </td>
                    <td align="left" style="width: 50%;">
                        <SAHL:SAHLDropDownList ID="ddlCompany" PleaseSelectItem="false" runat="server" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"
                            AutoPostBack="True">
                        </SAHL:SAHLDropDownList>
                    </td>
                    <td align="left" style="width: 35%;">
                    </td>
                </tr>
            </table>
            <br />
            <table id="tblOrgStruct" runat="server" visible="false" style="width: 100%; height: 100%">
                <tr style="width: 100%; height: 100%">
                    <td style="width: 100%; height: 100%">
                        <asp:Panel runat="server" ScrollBars="Vertical" ID="pnl" Width="100%" Height="390px"
                            Style="overflow: scroll" Font-Bold="false">
                            <SAHL:SAHLTreeView ID="tvOrgStruct" runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <table id="tblUserSummaryGrid" runat="server" visible="false" style="width: 100%;
            height: 100%">
            <tr style="width: 100%; height: 100%">
                <td>
                    <SAHL:DXGridView ID="UserSummaryGrid" runat="server" AutoGenerateColumns="False" Width="100%"
                        PostBackType="None" ClientInstanceName="grid" Settings-ShowTitlePanel="true"
                        SettingsText-Title="User Designation Summary Grid">
                        <SettingsEditing Mode="Inline" />
                        <ClientSideEvents RowClick="function(s, e) {s.StartEditRow(e.visibleIndex);}" />
                    </SAHL:DXGridView>
                </td>
            </tr>
        </table>
        <br />
        <table id="tblButton" runat="server" style="width: 100%; height: 100%">
            <tr style="width: 100%; height: 100%">
                <td style="width: 75%;">
                </td>
                <td style="width: 25%;">
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Visible="false" Text="Submit" OnClick="btnSubmit_Click" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Visible="false" Text="Cancel" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
