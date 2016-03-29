<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Codebehind="X2NonUserState.aspx.cs" Inherits="SAHL.Web.Views.Common.X2NonUserState" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div style="height: 20px">
    </div>
    <table style="width: 100%">
        <tr>
            <td align="center">
                <SAHL:SAHLLabel ID="Label1" runat="server" Text=""></SAHL:SAHLLabel>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="center">
                <SAHL:SAHLButton ID="btnRefresh" runat="server" Text="Refresh" />
            </td>
        </tr>
    </table>
</asp:Content>
