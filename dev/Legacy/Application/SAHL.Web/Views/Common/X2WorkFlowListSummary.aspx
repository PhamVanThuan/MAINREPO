<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.X2WorkFlowListSummary" Title="CBO Page" Codebehind="X2WorkFlowListSummary.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
        <asp:Label ID="Label1" runat="server" Text="Craigs Workflow SuperSearch Please Enter InstanceID"></asp:Label><asp:TextBox
        ID="txtIID" runat="server"></asp:TextBox><asp:Button ID="btnGo"
            runat="server" Text="Add Instance To CBO" OnClick="btnGo_Click" />
</asp:Content>