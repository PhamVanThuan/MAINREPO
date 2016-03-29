<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="True" Codebehind="OlapViewer.aspx.cs" Inherits="SAHL.Web.Views.Reports.OlapViewer"  EnableViewState="true"%>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    &nbsp;&nbsp;
    <asp:Panel runat="server" Width="99%" ID="pnlMain" > 
    <div id="divReportView" runat="server">
    </div>
    </asp:Panel>
    <br />
    <div class="buttonBar" style="width:99%;margin-top:5px;">
            <SAHL:SAHLButton ID="btnBack" runat="server" OnClick="btnCancel_Click" Text="Back" /></td>
    </div>
</asp:Content>

