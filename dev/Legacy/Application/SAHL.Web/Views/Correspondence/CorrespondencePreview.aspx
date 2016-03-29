<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="CorrespondencePreview.aspx.cs" Inherits="SAHL.Web.Views.Correspondence.CorrespondencePreview"
    Title="Untitled Page" EnableViewState="true" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="width: 100%; height: 100%">
        <table id="tblReports" runat="server" width="100%">
            <tr style="height:90%">
                <td>
                    <asp:PlaceHolder ID="pnTabContainer" runat="server" />
               </td>
            </tr>
            <tr style="height:10%">
                <td align="center">
                    <br />
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Done" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
