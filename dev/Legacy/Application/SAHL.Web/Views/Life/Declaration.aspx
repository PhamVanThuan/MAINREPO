<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Life.Declaration" Title="Declaration" Codebehind="Declaration.aspx.cs" %>

<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <uc1:workflowheader id="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <sahl:sahllabel id="lblPageHeader" runat="server" font-bold="True" font-names="Arial"
            font-size="Medium" font-underline="True" text="DECLARATION" cssclass="LabelText">
        </sahl:sahllabel>
        <br />
        <table width="100%"  class="tableStandard">
            <tr>
                <td align="center" style="height: 20px; vertical-align: bottom">
                    In order to proceed, I need you to confirm your acceptance of the following points:&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Table ID="tblText" runat="server" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <sahl:sahlbutton id="btnNext" runat="server" text="Next" onclick="btnNext_Click" SecurityTag="LifeUpdateAccessWorkflow"/>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="HiddenField1" runat="server" Value="false" />
    </div>
</asp:Content>
