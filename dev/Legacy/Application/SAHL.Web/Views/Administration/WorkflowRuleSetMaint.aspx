<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowRuleSetMaint.aspx.cs" Inherits="SAHL.Web.Views.Administration.WorkflowRuleSetMaint" MasterPageFile="~/MasterPages/Blank.master" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
<table>
<tr><td width="20%"><SAHL:SAHLLabel ID="slbl1" runat=server Text="Workflow RuleSet"></SAHL:SAHLLabel></td><td>
<SAHL:SAHLDropDownList ID="ddlRuleSets" runat="server" OnSelectedIndexChanged="ddlRuleSets_SelectedIndexChanged" AutoPostBack="True"></SAHL:SAHLDropDownList>
    </td></tr>
    <tr><td valign="top">
        <asp:Label ID="Label1" runat="server" Text="Rule List"></asp:Label></td><td>
        <SAHL:SAHLCheckboxList ID="sclb" runat=server></SAHL:SAHLCheckboxList>
    <asp:CheckBoxList ID="clb" runat="server">
    </asp:CheckBoxList>
    
    </td></tr>
    <tr><td valign="top">
    <SAHL:SAHLButton ID=Submit runat=server OnClick="btnSubmit_Click" Text="Update" />
    </td></tr>
    </table>
</asp:Content>
