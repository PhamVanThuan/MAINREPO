<%@ Page Language="C#" AutoEventWireup="true" Codebehind="OrganisationStructure.aspx.cs"
    Inherits="SAHL.Web.Views.Administration.OrganisationStructure" MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="790">
        <tr>
            <td colspan="2">
            <asp:Panel runat="server" ScrollBars="Vertical" ID="pnl" Width="100%" Height="300px" style="overflow-y:scroll">
                <SAHL:SAHLTreeView runat="server" ID="tv" OnNodeSelected="tv_NodeSelected" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table width="790" runat="server" id="tblMaint" visible="false">
        <tr>
            <td width="20%" align="left"><SAHL:SAHLLabel runat="server" id="lbl">Description</SAHL:SAHLLabel><input type="hidden" id="hdKey" runat="server"/>
            <input type="hidden" id="hdParentKey" runat="server"/>
            </td>
            <td width="80%">
            <SAHL:SAHLTextBox runat="server" ID="txtDesc"></SAHL:SAHLTextBox>
            <SAHL:SAHLRequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtDesc" ErrorMessage="*" />
            </td>
        </tr>

        <tr>
            <td width="20%" align="left">
                <SAHL:SAHLLabel ID="SAHLLabel2" runat="server">Organisation Type</SAHL:SAHLLabel></td>
            <td width="80%">
            <SAHL:SAHLDropDownList runat="server" ID="ddlOSType"></SAHL:SAHLDropDownList>
            </td>
        </tr>
        <tr>
            <td width="20%" align="left">
                <SAHL:SAHLLabel ID="SAHLLabel3" runat="server">Status</SAHL:SAHLLabel></td>
            <td width="80%">
            <SAHL:SAHLDropDownList runat="server" ID="ddlGeneralStatus"></SAHL:SAHLDropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
            <SAHL:SAHLButton runat="server" ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
