<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="Blank.aspx.cs" Inherits="SAHL.Web.Views.Common.Blank" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <br />
    <br />
    <br />
    <br />
    <SAHL:SAHLLabel runat=server ID="lblText" Text=""></SAHL:SAHLLabel>
    <br />
    <br />
    <br />
</asp:Content>
