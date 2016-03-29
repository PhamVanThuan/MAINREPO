<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.LastRateChange" Title="Untitled Page" Codebehind="LastRateChange.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    
<table border="0" cellspacing="0" cellpadding="0" class="PageBlock"><tr>
    <td align="left" style="height:99%; width: 680px;" valign="top" >
    
    <table width="100%">
        <tr>
            <td>
                <SAHL:SAHLGridView ID="grdLastRateChange" runat="server" AutoGenerateColumns="False" EmptyDataSetMessage="There are no Services."
                    EnableViewState="false" FixedHeader="false" GridHeight="100px" GridWidth="100%"
                    HeaderCaption="Current Rate Change" ShowFooter="true" NullDataSetMessage="no data" Width="770px" OnRowDataBound="grdLastRateChange_RowDataBound">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                
            </td>        
        </tr>
        </table>
    </td>
    </tr>
    <SAHL:SAHLLabel ID="SAHLLabel1" runat="server"></SAHL:SAHLLabel><tr id="ButtonRow" runat="server"><td align="right" style="height: 24px;">
        <br />
    <SAHL:SAHLButton ID="Cancel" runat="server" Text="Cancel" AccessKey="C" CausesValidation="False" UseSubmitBehavior="false" OnClick="Cancel_Click" />
</td></tr></table>

</asp:Content>

