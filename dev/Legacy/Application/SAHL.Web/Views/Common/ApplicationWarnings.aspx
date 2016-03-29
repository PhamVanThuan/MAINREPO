<%@ Page Language="C#" MasterPageFile="~/MasterPages/SAHL.Master" AutoEventWireup="true"
    Codebehind="ApplicationWarnings.aspx.cs" Inherits="SAHL.Web.Views.Common.ApplicationWarnings"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div>
        <asp:Panel ID="panelLegalEntity" runat="server" GroupingText="Legal Entity" Width="100%">
            <br />
            <table id="tblLegalEntity" runat="server" class="tableStandard">
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="panelApplicationOffer" runat="server" GroupingText="Application/Offer"
            Width="100%">
            <br />
            <table id="tblApplicationOffer" runat="server" class="tableStandard">
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="panelCredit" runat="server" GroupingText="Credit Matrix Rules" Width="100%">
            <br />
            <table id="tblCredit" runat="server" class="tableStandard">
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlDetailTypes" runat="server" GroupingText="Further Lending Critical Detail Types"
            Width="100%" Visible="False">
            <br />
            <table id="tblDetailTypes" runat="server" class="tableStandard">
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
