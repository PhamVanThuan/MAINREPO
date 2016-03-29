<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Life.Benefit" Title="Benefit" Codebehind="Benefit.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" Font-Bold="True" Font-Names="Arial"
            Font-Size="Medium" Font-Underline="True" Text="PROTECTION BENEFITS" CssClass="LabelText"></SAHL:SAHLLabel>
        <br />
        <br />
        <table width="100%" class="tableStandard">
            <tr>
                <td align="center">
                    <asp:Label ID="lblIntro" runat="server" Text="Label" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" height="15">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Table ID="tblText1" runat="server" BorderColor="Silver" BorderStyle="Solid"
                        BorderWidth="1px">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" style="border: 1px solid silver">
                    <asp:Table ID="tblText2" runat="server">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <SAHL:SAHLButton ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" SecurityTag="LifeUpdateAccessWorkflow"/></td>
            </tr>
        </table>
        <asp:HiddenField ID="HiddenField1" runat="server" Value="false" />
    </div>
</asp:Content>
