<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="StageTransitionHistory.aspx.cs" Inherits="SAHL.Web.Views.Common.StageTransition_History"
    Title="StageTransition History" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Src="../Life/WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <table width="100%">
            <tr>
                <td>
                    <SAHL:DXGridView ID="gridHistory" runat="server" AutoGenerateColumns="False" Width="100%"
                        EnableViewState="false" KeyFieldName="StageTransitionKey" PostBackType="DoubleClick">
                        <SettingsText Title="Business Event History" />
                        <Settings ShowTitlePanel="True" />
                    </SAHL:DXGridView>
                </td>
            </tr>
            <tr id="ButtonRow" runat="server">
                <td align="center">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
