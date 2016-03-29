<%@ Page Language="C#" AutoEventWireup="true" Codebehind="FeatureGroup.aspx.cs" Inherits="SAHL.Web.Views.Administration.FeatureGroup"
    MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="790" runat="server" id="tblFeatureGroup" visible="true">
        <tr>
            <td>
                <SAHL:SAHLLabel ID="lbl2" runat="server">Please Select a FeatureGroup</SAHL:SAHLLabel>
                <SAHL:SAHLDropDownList ID="ddlFeatureGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFeature_SelectedIndexChanged">
                </SAHL:SAHLDropDownList></td>
        </tr>
    </table>
    <table runat="server" id="tblSummary" visible="false" width="790">
        <tr>
            <td>
            <asp:PlaceHolder runat="server" ID="pc"></asp:PlaceHolder>
            </td>
        </tr>
    </table>
    <table runat="server" id="tblTree" visible="false" width="790">
        <tr>
            <td>
            <asp:Panel runat="server" ScrollBars="Vertical" ID="pnl" Width="100%" Height="300px" style="overflow-y:scroll">
                <SAHL:SAHLTreeView runat="server" ID="tv" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
        <td align="left"><SAHL:SAHLButton runat="server" id="btnSubmit" Text="Save" OnClick="btnSubmit_Click"/></td>
        </tr>
    </table>
</asp:Content>
