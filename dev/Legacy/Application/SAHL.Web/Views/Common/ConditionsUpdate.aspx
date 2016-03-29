<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.ConditionsUpdate" Title="Conditions Update" Codebehind="ConditionsUpdate.aspx.cs" %>

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
                    <SAHL:SAHLTextBox ID="txtNotes" runat="server" Rows="8" TextMode="MultiLine" Width="100%"></SAHL:SAHLTextBox></asp:Panel>
            
            </td>
        </tr>
         <tr>
            <td align="left" colspan="2">
            <asp:Panel ID="textPanel" runat="server" Width="100%" Visible="false">
                <SAHL:SAHLLabel ID="txtMessageLabel" runat="server" CssClass="LabelText" TextAlign="Left">Please complete the fields below. You may edit the text above, but do not replace or remove the field [placeholders]. </SAHL:SAHLLabel></asp:Panel>
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
                <SAHL:SAHLButton ID="btnRestoreString" runat="server" ButtonSize="Size6" Text="Restore Condition" Width="150px" CssClass="BtnNormal6 " Visible="False" />
                <SAHL:SAHLButton ID="btnAdd" runat="server" ButtonSize="Size4" Text="Add" Width="150px" CssClass="BtnNormal4 " Visible="False" Enabled="False" /><SAHL:SAHLButton ID="btnUpdate" runat="server" ButtonSize="Size4" Text="Update" Width="150px" CssClass="BtnNormal4 " Visible="False" Enabled="False" /><SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" Width="147px" ButtonSize="Size4" CssClass="BtnNormal4" /></td>
        </tr>
    </table>
</asp:Content>