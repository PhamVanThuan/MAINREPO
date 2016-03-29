<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="LifeNTU.aspx.cs" Inherits="SAHL.Web.Views.Life.LifeNTU" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <table style="width: 100%" class="tableStandard">
            <tr>
                <td align="center" colspan="1">
                    <SAHL:SAHLLabel ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        Font-Underline="True" Text="NTU POLICY" CssClass="LabelText"></SAHL:SAHLLabel>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <SAHL:SAHLLabel ID="lblReason" runat="server" Text="NTU Reason"></SAHL:SAHLLabel>
                    <SAHL:SAHLDropDownList ID="ddlNTUReason" runat="server">
                    </SAHL:SAHLDropDownList>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="1">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" colspan="1">
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                        OnClientClick="return confirm('Are you sure you want to NTU this Policy ?')" SecurityTag="LifeUpdateAccessWorkflow"/>
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                        CausesValidation="False" /></td>
            </tr>
        </table>
    </div>
</asp:Content>
