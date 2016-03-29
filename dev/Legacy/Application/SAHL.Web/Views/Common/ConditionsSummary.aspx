<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.ConditionsSummary" Title="Conditions Summary" Codebehind="ConditionsSummary.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
    
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <table width="100%" >
        <tr>
            <td align="left" style="height: 31px" >
                <asp:Panel ID="Panel1" runat="server" GroupingText="Current Loan Conditions" Width="100%">
                <asp:ListBox ID="listSelectedConditions" runat="server" Rows="8" Width="100%"/>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="left">
            <asp:Panel ID="Panel2" runat="server" GroupingText="Full Loan Condition Text" Width="100%">
            <SAHL:SAHLTextBox ID="txtDisplay" runat="server" Rows="10" TextMode="MultiLine" Width="100%" ReadOnly="True" BackColor="#FFFFC0" />
            </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>