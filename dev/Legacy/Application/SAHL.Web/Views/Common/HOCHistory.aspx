<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Codebehind="HOCHistory.aspx.cs" Inherits="SAHL.Web.Views.Common.Views_HOCHistory" Title="HOCHistory"%>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table width="100%"  class="tableStandard">
<tr><td align="left">
    
    <SAHL:SAHLGridView ID="HOCHistoryGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="200px" GridWidth="100%"  Width="100%"
        HeaderCaption="HOC History"
        NullDataSetMessage=""
        EmptyDataSetMessage="There are no HOC History Records." 
        OnRowDataBound="HOCHistoryGrid_RowDataBound"
        OnSelectedIndexChanged="HOCHistoryGrid_SelectedIndexChanged" >
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    
    <br/>
    
    <SAHL:SAHLGridView ID="HOCHistoryDetailGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="200px" GridWidth="100%"  Width="100%"
        HeaderCaption="HOC History Details"
        NullDataSetMessage=""
        EmptyDataSetMessage="There are no HOC History Detail Records." 
        OnRowDataBound="HOCHistoryDetailGrid_RowDataBound">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    
</td></tr></table>
</asp:Content>