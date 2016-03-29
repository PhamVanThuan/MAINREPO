<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ManageLoanStatus.ascx.cs" Inherits="ManageLoanStatus" %>
<asp:Panel ID="pnlStatus" runat="server" HorizontalAlign="Center" Width="900px">
    <table id="tStatusBar" runat="server" border="0" cellpadding="0" cellspacing="0"
        class="StatusPanel" width="100%">
        <tr>
            <td align="center">
<asp:Label ID="lStatusMsg" runat="server" CssClass="StatusPanelText" EnableViewState="False" Font-Bold="True"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
