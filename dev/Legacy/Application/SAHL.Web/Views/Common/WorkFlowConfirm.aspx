<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.WorkFlowConfirm" Codebehind="WorkFlowConfirm.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    &nbsp;
    <div style="padding-left:10px;padding-top:10px;" id="divOuter" runat="server">
        <h5><asp:Label ID="lblTitleText" runat="server"></asp:Label></h5>
        <hr size="1" />
        <div style="padding-top:5px;">
            Click yes to complete the action.
        </div>
        <div style="padding-top:10px;">
        <SAHL:SAHLButton ID="btnYes" runat="server" OnClick="btnYes_Click" Text="Yes" Width="80px" />
        <SAHL:SAHLButton ID="btnNo" runat="server" OnClick="btnNo_Click" Text="No" Width="80px" />
        </div>
    </div>
    
</asp:Content>