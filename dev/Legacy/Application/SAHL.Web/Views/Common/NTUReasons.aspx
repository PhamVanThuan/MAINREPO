<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.NTUReasons" Title="Untitled Page" EnableEventValidation="false" Codebehind="NTUReasons.aspx.cs"%>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <asp:Panel ID="pnlHistory" runat="server" Width="100%" GroupingText="Application Reason History">
        <br />
        &nbsp;<SAHL:SAHLGridView ID="grdReasons" runat="server" FixedHeader="False" GridHeight=""
                GridWidth="100%" Invisible="False" SelectFirstRow="True" TotalsFooter="True"
                Width="100%" AutoGenerateColumns="False">
            </SAHL:SAHLGridView>
        <br />
    </asp:Panel>
    &nbsp;&nbsp;<br />
    &nbsp;
    <br />
    &nbsp;<br />
    &nbsp;&nbsp;
</asp:Content>

