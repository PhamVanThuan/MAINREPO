<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.WorkflowEmploymentTypeConfirmation" Codebehind="WorkflowEmploymentTypeConfirmation.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    &nbsp;
    <div style="padding-left:10px;padding-top:10px;" id="divOuter" runat="server">
        <h5><asp:Label ID="lblTitleText" runat="server"></asp:Label></h5>
        <hr size="1" />
        <div style="padding-top:5px;">
            <SAHL:SAHLLabel ID="EmplomentType" runat="server" CssClass="LabelText">Employment Type</SAHL:SAHLLabel>
            <SAHL:SAHLDropDownList ID="ddlSelectedEmploymentType" runat="server" />
        </div>
        <div align="right" style="padding-top:10px;">
        <SAHL:SAHLButton ID="btnConfirm" runat="server" Text="Confirm" Width="80px" />
        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" Width="80px" />
        </div>
    </div>
    
</asp:Content>