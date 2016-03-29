<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="PDFReportViewer.aspx.cs" Inherits="SAHL.Web.Views.Reports.PDFReportViewer"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table class="tableStandard" width="100%">
        <tr>
            <td>
                <SAHL:SAHLPdfViewer ID="pdfViewer" runat="server" BorderStyle="Solid" BorderWidth="1px"
                    Height="440px" Width="99.9%" BorderColor="Black" />
            </td>
        </tr>
        
        <tr id="ButtonRow" runat="server">
            <td align="center">
                <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="CancelButton_Click" Text="Cancel" />
            </td>
        </tr>
    </table>
</asp:Content>
