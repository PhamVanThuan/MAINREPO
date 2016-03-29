<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="LifeProductSwitch.aspx.cs" Inherits="SAHL.Web.Views.Life.LifeProductSwitch"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div runat="server" style="text-align: center">
        <br />
        <table style="width: 100%">
            <tr>
                <td align="center" style="width: 100%;">
                    <SAHL:SAHLLabel ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        Font-Underline="True" Text="Change Policy Type" CssClass="LabelText"></SAHL:SAHLLabel>
                    <br />
                    <br />
                </td>
            </tr>
            <tr runat="server" style="width: 100%" align="center">
                <td runat="server" align="center">
                    <SAHL:SAHLLabel ID="lblReason" runat="server" Text="Policy Type"></SAHL:SAHLLabel>
                    &nbsp;&nbsp;
                    <SAHL:SAHLDropDownList ID="ddlPolicyType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPolicyType_SelectedIndexChanged">
                    </SAHL:SAHLDropDownList>
                    <br />
                    <br />
                </td>
            </tr>
            <tr runat="server" style="width: 100%" align="center">
                <td runat="server" align="center" colspan="1">
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Visible="false" Text="Next" OnClick="btnSubmit_Click" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"/>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
