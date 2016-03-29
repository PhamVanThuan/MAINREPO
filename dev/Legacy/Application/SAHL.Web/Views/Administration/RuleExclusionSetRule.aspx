<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RuleExclusionSetRule.aspx.cs" Inherits="SAHL.Web.Views.Administration.RuleExclusionSetRule" MasterPageFile="~/MasterPages/Blank.master"%>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="790" runat="server" id="tblSearch" visible="true">
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText" Font-Bold="True"
                    Font-Names="Arial" Font-Size="Medium" Font-Underline="True" Text="RuleExclusionSet Name"></SAHL:SAHLLabel></td>
            <td colspan="2">
                <SAHL:SAHLTextBox runat="server" ID="txtRuleName" Mandatory="True" Width="550px"></SAHL:SAHLTextBox>
                &nbsp;<SAHL:SAHLButton runat="server" Text="Search" ID="btnSearch" OnClick="btnSearch_OnClick" />&nbsp;
                </td>
        </tr>
    </table>
    <table runat="server" id="tblRuleSets" visible="false" width="790">
        <tr>
            <td colspan="3" style="height: 126px">
                <SAHL:SAHLGridView ID="RuleSetGrid" runat="server" AutoGenerateColumns="false" EmptyDataSetMessage="There are no Rules Defined."
                    EnableViewState="false" FixedHeader="false" GridHeight="80px" GridWidth="100%"
                    HeaderCaption="Rule" NullDataSetMessage="" Width="790px" EmptyDataText="There are no Rules Defined."
                    OnGridDoubleClick="RulesGrid_GridDoubleClick" PostBackType="DoubleClick">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
    </table>
    
    <table runat="server" id="tblMapping" width="790" visible="false">
    <tr>
    <td width="45%">
        &nbsp;<asp:ListBox ID="liNotInRuleSet" runat="server" Height="316px"></asp:ListBox></td>
    <td abbr="10%" valign="middle" align="center">
        <SAHL:SAHLButton runat="Server" ID="btnAdd" Text=" >> " OnClick="btnAdd_Click"  Visible="false"/><br />
        <asp:CheckBox runat="Server" ID="chkEnforce" Checked="true" Text="Enforce" Visible="false"/>
        <asp:CheckBox runat="server" ID="chkDIsable" Checked="true" Text="Disable" Visible="false"/>
        <br /><SAHL:SAHLButton runat="Server" ID="btnREmove" Text=" << " OnClick="btnREmove_Click"  Visible="false"/>
    </td>
    <td width="45%">
        <asp:ListBox ID="liInRuleSet" runat="server" Height="316px"></asp:ListBox></td>
    </tr>
    <tr>
    <td><SAHL:SAHLButton runat="Server" Text="Submit" value="Save Changes" id="btnSubmit" OnClick="btnSubmit_Click" Visible="false"/></td>
    </tr>
    </table>
    </asp:Content>
