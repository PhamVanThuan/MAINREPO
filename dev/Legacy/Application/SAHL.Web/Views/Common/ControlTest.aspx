<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="ControlTest.aspx.cs" Inherits="SAHL.Web.Views.Common.ControlTest"
    Title="Untitled Page" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register assembly="DevExpress.Web.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSplitter" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    1<script type="text/javascript">
/*<![CDATA[*/

// TreeList handlers
function tree_StartDragNode(s, e) {
	var rootTarget = document.getElementById("rootTarget");
	e.targets.push(rootTarget);
}
function tree_EndDragNode(s, e) {
	if(e.targetElement.id == "rootTarget") {
		e.cancel = true;
		tree.MoveNode(e.nodeKey, "");
	}
}
/*]]>*/
</script><div style="float: left">
        <table width="100%">
            <tr>
                <td>
                    <SAHL:DXTreeList ID="tlOrgStructure" runat="server" AutoGenerateColumns="False" Height="150px"
                        Width="200px" Caption="SAHL DX TreeList" OnProcessDragNode="ProcessDragNode" ClientInstanceName="tree">
                        <Columns>
                            <SAHL:DXTreeListDataColumn Caption="Description" FieldName="OSDescription" VisibleIndex="0">
                            </SAHL:DXTreeListDataColumn>
                            <SAHL:DXTreeListDataColumn Caption="Type" FieldName="OSTypeDescription" VisibleIndex="1">
                            </SAHL:DXTreeListDataColumn>
                            <SAHL:DXTreeListDataColumn Caption="DisplayName" FieldName="DisplayName" VisibleIndex="2">
                            </SAHL:DXTreeListDataColumn>
                        </Columns>
                        <SettingsBehavior AllowFocusedNode="True" AutoExpandAllNodes="True" />
                        <SettingsEditing AllowNodeDragDrop="True" />
                   </SAHL:DXTreeList>
                    <dx:ASPxSplitter ID="ASPxSplitter1" runat="server" Orientation="Vertical" 
                        SeparatorSize="1px" ShowCollapseBackwardButton="True" 
                        ShowCollapseForwardButton="True">
                        <Panes>
                            <dx:SplitterPane>
                            </dx:SplitterPane>
                            <dx:SplitterPane>
                                <ContentCollection>
                                    <dx:SplitterContentControl runat="server">
                                    </dx:SplitterContentControl>
                                </ContentCollection>
                            </dx:SplitterPane>
                        </Panes>
                    </dx:ASPxSplitter>
                </td>
                <td>
                    <dx:ASPxTreeList ID="ASPxTreeList1" runat="server" AutoGenerateColumns="False" Height="150px"
                        Width="200px" Caption="Dev Express TreeList" OnProcessDragNode="ProcessDragNode" ClientInstanceName="tree">
                        <Columns>
                            <SAHL:DXTreeListDataColumn Caption="Description" FieldName="OSDescription" VisibleIndex="0">
                            </SAHL:DXTreeListDataColumn>
                            <SAHL:DXTreeListDataColumn Caption="Type" FieldName="OSTypeDescription" VisibleIndex="1">
                            </SAHL:DXTreeListDataColumn>
                            <SAHL:DXTreeListDataColumn Caption="DisplayName" FieldName="DisplayName" VisibleIndex="2">
                            </SAHL:DXTreeListDataColumn>
                        </Columns>
                        <SettingsBehavior AllowFocusedNode="True" AutoExpandAllNodes="True" />
                        <SettingsEditing AllowNodeDragDrop="True" />
                    </dx:ASPxTreeList>
                </td>
            </tr>
            <tr id="ButtonRow" runat="server">
                <td align="center" colspan="2">
                    <SAHL:SAHLButton ID="btnRemove" runat="server" Text="Remove" OnClick="btnRemove_Click"/>
                    <asp:CheckBox ID="chkDevExpressDragHandled" runat="server" Text="TreeList Drag Handled"
                        TextAlign="Left" />
                    <asp:CheckBox ID="chkThowError" runat="server" Text="Throw Error"
                        TextAlign="Left" /></td>
            </tr>
            <tr>
                <td>
                    <SAHL:DXGridView ID="gvSAHL" runat="server" PostBackType="NoneWithClientSelect" AutoGenerateColumns="False"  SettingsBehavior-AllowGroup="True" Settings-ShowGroupPanel="True" Visible="True">
                        <Settings ShowTitlePanel="True" ShowGroupPanel="True" />
                        <SettingsText Title="SAHL DX Grid" />
                        <Styles>
                            <AlternatingRow Enabled="True">
                            </AlternatingRow>
                        </Styles>
                        <Border BorderWidth="2px" />
                    </SAHL:DXGridView>
                </td>
                <td>
                    <dx:ASPxGridView ID="gvDevExpress" runat="server" AutoGenerateColumns="True" SettingsBehavior-AllowGroup="True" Settings-ShowGroupPanel="True" Visible="True">
                        <SettingsText Title="Dev Express Grid" />
                        <Settings ShowTitlePanel="True" ShowGroupPanel="True" />
                    </dx:ASPxGridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
