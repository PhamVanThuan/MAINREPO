<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" CodeBehind="ApplicationAttributes.aspx.cs" Inherits="SAHL.Web.Views.Origination.ApplicationAttributes" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table border="0" cellspacing="0" cellpadding="0" class="tableStandard">    
<tr><td align="left" style="height:99%; width: 770px;" valign="top">    
<table border="0">
<tr>
    <td align="left" style="height:99%; width:400px;">     
        <asp:Panel ID="pnlAttributes" runat="server" GroupingText="Application Attributes" Width="400px" Font-Bold="true">
            <asp:CheckBoxList ID="chkAttributes" runat="server">
                </asp:CheckBoxList>
        </asp:Panel>      
     </td>
</tr> 
</table>
<table>
<tr style="height:5px">
    <td></td>
</tr>
    <tr>
        <td>
            <asp:Panel ID="pnlTransferringAttorney" runat="server" GroupingText="Transferring Attorney Details" Width="400px" Font-Bold="true">
            <SAHL:SAHLLabel ID="lblTransferringAttorneyLabel" runat="server" CssClass="TitleText">Transferring Attorney</SAHL:SAHLLabel>
            <SAHL:SAHLLabel ID="lblTransferringAttorney" runat="server" CssClass="TitleText">-</SAHL:SAHLLabel>
            <SAHL:SAHLTextBox ID="TxtTransferAttorney" runat="server" Width="200px"></SAHL:SAHLTextBox>
            <SAHL:SAHLAutoComplete ID="acTransferringAttorney" runat="server" TargetControlID="TxtTransferAttorney" ServiceMethod="SAHL.Web.AJAX.Employment.GetSubsidyProviders" AutoPostBack="true" />
          </asp:Panel>     
        </td>
        <td>
            <asp:Panel ID="pnlMarketingSource" runat="server" GroupingText="Application Marketing Source" Width="360px" Font-Bold="true">
            <SAHL:SAHLLabel ID="lblMarketingSourcelabel" runat="server" CssClass="TitleText">Marketing Source</SAHL:SAHLLabel>
            <SAHL:SAHLLabel ID="lblMarketingSource" runat="server" CssClass="TitleText">-</SAHL:SAHLLabel>
            <SAHL:SAHLDropDownList ID="ddlMarketingSource" runat="server" Width="200px"></SAHL:SAHLDropDownList>
          </asp:Panel>     
        </td>
    </tr>
</table>
</td></tr>
<tr id="ButtonRow" runat="server"><td align="right">
    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="Cancel_Click" />
    <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" AccessKey="U" OnClick="Update_Click"/>
</td></tr>
</table>
</asp:Content>

