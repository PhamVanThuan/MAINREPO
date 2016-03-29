<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.Views_ReOpenAccount" Title="ReOpen Account" Codebehind="ReOpenAccount.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table border="0" cellspacing="0" cellpadding="0" class="tableStandard">
<tr><td align="left" style="height:99%;" valign="middle">

    <table class="PageTableBorder">
        <tr>
            <td align="center" class="TitleText">
                This action will cause the selected account to be re-opened. 
            </td>
        </tr>  
        <tr>    
            <td align="center" class="TitleText">
                Are you sure you wish to re-open the account ?
            </td>
         </tr> 
    </table>
    
</td></tr>

<tr id="ButtonRow" runat="server"><td align="right"> 
    <SAHL:SAHLButton ID="NoButton" runat="server" Text="No" AccessKey="N" OnClick="NoButton_Click" />
    <SAHL:SAHLButton ID="YesButton" runat="server" Text="Yes" AccessKey="Y" OnClick="YesButton_Click" />
</td></tr></table>
</asp:Content>