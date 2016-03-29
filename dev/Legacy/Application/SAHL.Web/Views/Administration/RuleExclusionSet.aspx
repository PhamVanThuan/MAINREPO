<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RuleExclusionSet.aspx.cs" Inherits="SAHL.Web.Views.Administration.RuleExclusionSet" MasterPageFile="~/MasterPages/Blank.master"%>
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
            <td colspan="3">
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
    <table id="tblMaintRuleSet" runat="server" visible="false" width="790">
        <tr><td colspan="2"><input runat="server" type="hidden" id="txtMaintRuleSetKey" value=""/></td></tr>
        <tr>
            <td width="30%" align="left"><SAHL:SAHLLabel runat="server" ID="label1" CssClass="LabelText" TextAlign="left">RuleExclusionSet Name</SAHL:SAHLLabel>
            </td>
            <td><SAHL:SAHLTextBox runat="Server" ID="txtMaintRuleSetName" Width="577px"></SAHL:SAHLTextBox>
            <SAHL:SAHLRequiredFieldValidator ID="SAHLRequiredFieldValidator1" runat="server" ControlToValidate="txtMaintRuleSetName" ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td width="30%" align="left">
                <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" CssClass="LabelText" TextAlign="left">RuleExclusionSet Description</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtMaintRuleSetDesc" runat="server" Width="578px"></SAHL:SAHLTextBox>
                <SAHL:SAHLRequiredFieldValidator ID="SAHLRequiredFieldValidator2" runat="server" ControlToValidate="txtMaintRuleSetDesc" ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
            <SAHL:SAHLButton runat="server" Text="Submit" ID="btnMaint" OnClick="BtnSubmitRuleSetClick" />
            </td>
        </tr>
    </table>
    </asp:Content>