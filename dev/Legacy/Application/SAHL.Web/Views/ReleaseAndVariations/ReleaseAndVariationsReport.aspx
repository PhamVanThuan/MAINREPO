<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.ReleaseAndVariations.Report"
    Title="Release And Variations Conditions" Codebehind="ReleaseAndVariationsReport.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content id="Content1" ContentPlaceHolderID="Main" runat="server">
    <table style="width: 95%">
        <tr>
            <td align="right" colspan="2">
                <asp:Panel id="Panel1" runat="server" GroupingText="Release & Variations Report" Height="100%" width="99%">
                </asp:Panel>
            </td>
            </tr>
    </table>
    <table style="width: 95%">
            <tr>
                <td align="right" colspan="2">
                    </td>
            </tr>
        <tr>
            <td align="right" colspan="2">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" ButtonSize="Size4" CssClass="BtnNormal4"
                        OnClick="btnCancel_Click" Text="Done!" Visible="False" Width="147px" /></td>
        </tr>
    </table>
</asp:Content>
