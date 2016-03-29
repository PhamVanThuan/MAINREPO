<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.ConditionsEdit" Title="Conditions Update" Codebehind="ConditionsEdit.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
    
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <table width="100%">
        <tr>
            <td align="left" colspan="2" style="height: 31px">
                &nbsp; &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2">
            <asp:Panel ID="Panel2" runat="server" GroupingText="Loan Condition - Text Update" Width="100%">
                    <SAHL:SAHLTextBox ID="txtNotes" runat="server" Rows="8" TextMode="MultiLine" Width="100%" BackColor="#FFFFC0"></SAHL:SAHLTextBox></asp:Panel><asp:Panel ID="pnlTranslation" runat="server" GroupingText="Loan Condition - Translation - Afrikaans" Width="100%" Visible="False">
                        <SAHL:SAHLTextBox ID="txtTranslation" runat="server" BackColor="#FFFFC0" Rows="8"
                            TextMode="MultiLine" Visible="False" Width="100%"></SAHL:SAHLTextBox></asp:Panel>
            
            </td>
        </tr>
         <tr>
            <td align="left" colspan="2">
            <asp:Panel ID="textPanel" runat="server" Width="100%" Visible="false">
                <SAHL:SAHLLabel ID="txtMessageLabel" runat="server" CssClass="LabelText">Please Edit the [Placeholder] values below:</SAHL:SAHLLabel></asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <asp:PlaceHolder ID="PlaceHolder1"  runat="server"></asp:PlaceHolder>
                &nbsp; &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2">
                <asp:HiddenField ID="txtTokenIDs" runat="server" EnableViewState="False" />
                <asp:HiddenField ID="txtTokenValues" runat="server" EnableViewState="False" />
                <SAHL:SAHLButton ID="btnRestoreString" runat="server" ButtonSize="Size6" Text="Restore Condition" Width="150px" CssClass="BtnNormal6" Visible="False" OnClick="btnRestoreString_Click"/>
                <SAHL:SAHLButton ID="btnAdd" runat="server" ButtonSize="Size6" Text="Add" Width="150px" CssClass="BtnNormal6" Visible="False" OnClick="btnAdd_Click"/>
                <SAHL:SAHLButton ID="btnUpdate" runat="server" ButtonSize="Size6" Text="Update" Width="150px" CssClass="BtnNormal6" Visible="False" OnClick="btnUpdate_Click" />
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" Width="150px" ButtonSize="Size6" CssClass="BtnNormal6" Visible="False" OnClick="btnCancel_Click"/>
                </td>
        </tr>
    </table>
</asp:Content>