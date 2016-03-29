<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaselSummary.aspx.cs" Inherits="SAHL.Web.Views.Common.BaselSummary"
    MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="contentMain" ContentPlaceHolderID="Main" runat="server">
    <SAHL:DXGridView Width="100%" runat="server" ID="grdBehaviouralScores" NullDataSetMessage="There are no Basel II Summary Information for this Account."
        EmptyDataSetMessage="There are no Basel II Summary Information for this Account."
        GridHeight="150px" GridWidth="100%" OnHtmlRowPrepared="OnRowDataBound" HeaderCaption="Basel II Summary">
        <SettingsText Title="Basel II Summary" />
        <Settings ShowTitlePanel="True" />
    </SAHL:DXGridView>
</asp:Content>
