<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.Notification" Codebehind="Notification.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    &nbsp;
    <div style="padding-left:10px;padding-top:10px;" id="divOuter" runat="server">
        <h5><asp:Label ID="lblTitleText" runat="server" Text="Notification" ></asp:Label></h5>
        <hr size="1" />
        <div style="padding-top:5px;">
            <asp:Label ID="lblNotification" runat="server"></asp:Label>
        </div>
    </div>  
    
</asp:Content>