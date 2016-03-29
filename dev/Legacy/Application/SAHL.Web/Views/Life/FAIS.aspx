<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="FAIS.aspx.cs" Inherits="SAHL.Web.Views.Life.FAIS" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align:center">
        <SAHL:SAHLLabel ID="lblPageHeader" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
            Font-Underline="True" Text="FAIS Requirements" CssClass="LabelText">
        </SAHL:SAHLLabel>
        <br />
        <table id="tblTextStatements" width="100%"  class="tableStandard">
            <tr>
                <td align="center">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Table ID="tblText" runat="server" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Panel ID="pnlConfirm" runat="server" CssClass="backgroundLight">
                        <asp:RadioButtonList ID="rbnsConfirmation" runat="server" AutoPostBack="True" >
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:Panel>
                    <SAHL:SAHLTextBox ID="txtPhone" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="center">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <SAHL:SAHLButton ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" OnClientClick="return ValidateCheckBoxes()" SecurityTag="LifeUpdateAccessWorkflow" /></td>
            </tr>
        </table>
        <asp:HiddenField ID="AllChecked" runat="server" Value="false" />
        <asp:HiddenField ID="ConfirmMode" runat="server" Value="false" />
        <asp:HiddenField ID="ContactSpouse" runat="server" Value="none" />
    </div>
</asp:Content>
