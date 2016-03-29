<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ContextMenu.aspx.cs" Inherits="SAHL.Web.Views.Administration.ContextMenu"
    MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
<table>
<tr><td align="center">Context Menu Maintenance</td></tr>
</table>
    <table runat="server" width="790" id="tblContextMenu" visible="true">
        <tr>
            <td>
                <asp:Panel runat="server" ScrollBars="Vertical" ID="pnl" Width="100%" Height="300px"
                    Style="overflow-y: scroll">
                    <SAHL:SAHLTreeView runat="server" ID="tv" OnNodeSelected="tv_NodeSelected" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table width="790" runat="server" id="tblMaint">
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel4" runat="server" CssClass="LabelText">Context Key</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtKey" runat="server"></SAHL:SAHLTextBox><SAHL:SAHLRequiredFieldValidator
                    ID="SAHLRequiredFieldValidator1" runat="server" ControlToValidate="txtKey" />
            </td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="lbl" runat="server" CssClass="LabelText">Description</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtDesc" runat="server"></SAHL:SAHLTextBox><SAHL:SAHLRequiredFieldValidator
                    ID="rfvDesc" runat="server" ControlToValidate="txtDesc" />
            </td>
        </tr>
        <tr>
            <td width="20%" align="left">
                <SAHL:SAHLLabel ID="lblParent" runat="server" Text="Parent"></SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtParent" runat="server" ReadOnly="true"></SAHL:SAHLTextBox>
                &nbsp;<SAHL:SAHLButton ID="btnShowParent" runat="server" Text="Show Parent" OnClick="btnShowParent_Click"
                    CausesValidation="False" Width="234px" /></td>
        </tr>
        <tr runat="server" visible="false" id="trParentHeader">
            <td colspan="2">
                Select Parent (do not select if this Context menu does not have a parent)</td>
        </tr>
        <tr runat="server" visible="false" id="trParentTree">
            <td colspan="2">
                <asp:Panel runat="server" ScrollBars="Vertical" ID="Panel1" Width="100%" Height="300px"
                    Style="overflow-y: scroll">
                    <SAHL:SAHLTreeView runat="server" ID="tvParent" OnNodeSelected="tv_ParentNodeSelected" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText">URL</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtURL" runat="server"></SAHL:SAHLTextBox>
                <SAHL:SAHLRequiredFieldValidator
                    ID="SAHLRequiredFieldValidator3" runat="server" ControlToValidate="txtURL" /></td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" CssClass="LabelText">Sequence</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtSequence" runat="server"></SAHL:SAHLTextBox>
                <SAHL:SAHLRequiredFieldValidator
                    ID="SAHLRequiredFieldValidator2" runat="server" ControlToValidate="txtSequence" /></td>
        </tr>
        <tr>
            <td width="20%" align="left">
                <SAHL:SAHLLabel ID="SAHLLabel3" runat="server" Text="Feature"></SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtFeature" runat="server" ReadOnly="true"></SAHL:SAHLTextBox>
                &nbsp;<SAHL:SAHLButton ID="btnFeature" runat="server" Text="Show Feature" OnClick="btnShowFeature"
                    CausesValidation="False" Width="240px" />
                    <SAHL:SAHLRequiredFieldValidator
                    ID="rfvFeature" runat="server" ControlToValidate="txtFeature" /></td>
        </tr>
        <tr>
        <td colspan="2" align="left">
        <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" Width="240px" />
        </td>
        </tr>
        <tr runat="server" visible="false" id="trFeatureHead">
            <td colspan="2">
                Select Feature</td>
        </tr>
        <tr runat="server" visible="false" id="trFeatureTree">
            <td colspan="2">
                <asp:Panel runat="server" ScrollBars="Vertical" ID="Panel2" Width="100%" Height="300px"
                    Style="overflow-y: scroll">
                    <SAHL:SAHLTreeView runat="server" ID="tvFeature" OnNodeSelected="tv_FeatureNodeSelected" />
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
