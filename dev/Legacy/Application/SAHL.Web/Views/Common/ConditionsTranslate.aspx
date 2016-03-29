<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.ConditionsTranslate" Title="Conditions Translate" Codebehind="ConditionsTranslate.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
    
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <table width="100%" >
        <tr>
            <td align="left">
                <asp:Panel ID="Panel1" runat="server" GroupingText="Translate Conditions" Width="100%" ScrollBars="Vertical">
                    <asp:ListBox ID="listGenericConditions" runat="server" Rows="8" Width="100%" Height="153px"></asp:ListBox></asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="left">
            <asp:Panel ID="Panel2" runat="server" GroupingText="Original condition for translating" Width="100%">
                <SAHL:SAHLTextBox ID="txtDisplay" runat="server" ReadOnly="True" Rows="3" TextMode="MultiLine"
                    Width="100%" Height="67px" BackColor="#FFFFC0"></SAHL:SAHLTextBox></asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="Condition - Afrikaans" Width="100%">
            <SAHL:SAHLTextBox ID="txtTranslate" runat="server" Rows="10" TextMode="MultiLine" Width="100%" Height="70px" ReadOnly="True" BackColor="#FFFFC0" />
            </asp:Panel>
                &nbsp; &nbsp;
                <asp:HiddenField ID="txtConditionToEdit" runat="server" EnableViewState="False" /><asp:HiddenField ID="txtConditionsKey" runat="server" EnableViewState="False" />
            </td>
        </tr>
           <tr>
            <td align="right">
                <SAHL:SAHLButton ID="btnEdit" runat="server" ButtonSize="Size4" CssClass="BtnNormal4 " Text="Edit Translation" Width="150px" OnClick="btnEdit_Click"/>
                <SAHL:SAHLButton ID="btnUpdate" runat="server" ButtonSize="Size4" CssClass="BtnNormal4 " Text="Save & Exit" Width="150px" OnClick="btnUpdate_Click"/>
                <SAHL:SAHLButton ID="btnCancel" runat="server" ButtonSize="Size4" CssClass="BtnNormal4" Text="Cancel" Width="147px" OnClick="btnCancel_Click"/>
           </td>
        </tr>    
        <tr>
            <td align="right" style="text-align: left">
                <asp:Label ID="lblErrorMessage" runat="server" Font-Bold="True" ForeColor="Red" Text="You have included conditions within your condition set that contain [Tokens] that you need to edit before saving. These conditions are highlighted in Silver in the Selected Condition Set."
                    Visible="False" Width="100%"></asp:Label></td>
        </tr>
             </table>
</asp:Content>