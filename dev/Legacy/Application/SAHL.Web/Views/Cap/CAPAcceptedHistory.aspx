<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Cap.CAPAcceptedHistory" Title="Accepted CAP History" Codebehind="CAPAcceptedHistory.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table border="0" cellspacing="0" cellpadding="0" class="PageBlock"><tr><td align="left" style="height:99%;" valign="top">
    
    <SAHL:SAHLGridView ID="HistoryGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="200px" GridWidth="100%" Width="100%"
        HeaderCaption="Accepted CAP History"
        NullDataSetMessage=""
        EmptyDataSetMessage="There is no History." 
        OnRowDataBound="HistoryGrid_RowDataBound">
        <HeaderStyle CssClass="TableHeaderB" />
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    
    <br />

    <table align="left">
        <tr id="CancelCapRow" runat="server" >
            <td class="TitleText" align="left" >
                Cancellation Reason
            </td>
            <td align="left" >
                <SAHL:SAHLDropDownList ID="ddlCancellationReason" runat="server" style="width:290px"></SAHL:SAHLDropDownList>
            </td>
        </tr>
    </table>


</td></tr>
<tr id="ButtonRow" runat="server"><td align="right">    
    <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" CausesValidation="false" />
    <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />
</td></tr></table>
</asp:Content>