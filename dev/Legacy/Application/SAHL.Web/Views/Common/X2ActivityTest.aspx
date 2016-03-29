<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.X2ActivityTest" Title="CBO Page" Codebehind="X2ActivityTest.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    Activity:
    <asp:Label ID="lblActivity" runat="server"></asp:Label>
</asp:Content>