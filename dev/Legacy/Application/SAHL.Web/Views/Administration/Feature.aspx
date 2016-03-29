<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Feature.aspx.cs" Inherits="SAHL.Web.Views.Administration.Feature"
    MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
<table width="790" runat="server" id="tblFeatureList" visible=true>
<tr>
<td><SAHL:SAHLLabel ID="lbl2" runat="server">Please Select a feature.</SAHL:SAHLLabel>
<SAHL:SAHLDropDownList ID="ddlFeature" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFeature_SelectedIndexChanged"></SAHL:SAHLDropDownList></td>
</tr>
</table><br>
    <table width="790" runat="server" id="tblMaint" visible="true">
    <tr>
    <td colspan="2"><SAHL:SAHLLabel runat="server" ID="lbl1">Select Parent Feature</SAHL:SAHLLabel></td>
    </tr>
        <tr>
            <td colspan="2">
                <asp:Panel runat="server" ScrollBars="Vertical" ID="pnl" Width="100%" Height="300px" style="overflow-y:scroll">
                <SAHL:SAHLTreeView runat="server" ID="tv" OnNodeSelected="tv_NodeSelected" />
                </asp:Panel>
            </td>
        </tr>
    <tr>
            <td width="20%" align="left">
            <input type="hidden" runat="server" id="hdKey" value="0"/>
                <SAHL:SAHLLabel ID="SAHLLabel1" runat="server">Feature</SAHL:SAHLLabel></td>
            <td align="left">
                <SAHL:SAHLTextBox ID="txtFeature" runat="server" Width="485px"></SAHL:SAHLTextBox>
                <SAHL:SAHLRequiredFieldValidator ID="rvfFeature" runat="server" ControlToValidate="txtFeature" />
            </td>
        </tr>
        <tr>
            <td width="20%" align="left">
                <SAHL:SAHLLabel ID="SAHLLabel2" runat="server">Long Description</SAHL:SAHLLabel></td>
            <td align="left">
                <SAHL:SAHLTextBox ID="txtLong" runat="server" Width="485px"></SAHL:SAHLTextBox>
                <SAHL:SAHLRequiredFieldValidator ID="rfvLong" runat="server" ControlToValidate="txtLong" />
            </td>
        </tr>
        <tr>
            <td width="20%" align="left">
                <SAHL:SAHLLabel ID="SAHLLabel3" runat="server">Parent Feature</SAHL:SAHLLabel></td>
            <td align="left">
                <SAHL:SAHLTextBox ID="txtParent" runat="server" ReadOnly="True" Width="485px"></SAHL:SAHLTextBox>
                <input type="hidden" runat="server" id="hdParent" value="0"/>
                <SAHL:SAHLButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear Parent" />
                </td>
                
        </tr>
        <tr>
        <td width="20%" align="left">
            <SAHL:SAHLLabel ID="SAHLLabel4" runat="server" CssClass="LabelText">Sequence</SAHL:SAHLLabel></td>
        <td align=left>
            <SAHL:SAHLTextBox ID="txtSeq" runat="server" Width="38px">1</SAHL:SAHLTextBox>
            <SAHL:SAHLRequiredFieldValidator ID="SAHLRequiredFieldValidator1" runat="server" ControlToValidate="txtFeature" /></td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <SAHL:SAHLButton ID="btn" runat="server" Text="Save" ButtonSize="Size4" OnClick="btn_Click" /></td>
        </tr>
    </table>
</asp:Content>
