<%@ Page AutoEventWireup="true" Codebehind="AmortisationSchedule.aspx.cs" Inherits="SAHL.Web.Views.Common.AmortisationSchedule"
    Language="C#" MasterPageFile="~/MasterPages/Blank.Master" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table class="tableStandard" width="100%">
        <tr id="trValues1" runat="server" align="left">
            <td style="width: 20%;">
                Initial Balance:
            </td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblInitialBalance" runat="server"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr id="trValues2" runat="server" align="left">
            <td style="width: 20%;">
                Interest Rate:
            </td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblInterestRate" runat="server"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr id="trValues3" runat="server" align="left">
            <td style="width: 20%;">
                Term:
            </td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblTerm" runat="server"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <SAHL:DXGridView ID="grdVariableAmortisationSchedule" runat="server" AutoGenerateColumns="False"
                    EnableViewState="false" KeyFieldName="Number"  Visible="false"
                    Width="100%" PostBackType="None">
                    <SettingsText Title="Variable Portion" />
                    <Border BorderWidth="2px" />
                </SAHL:DXGridView>
                <SAHL:DXGridView ID="grdFixedAmortisationSchedule" runat="server" AutoGenerateColumns="False"
                    EnableViewState="false" KeyFieldName="Number"  Visible="false" Width="100%" PostBackType="None">
                    <SettingsText Title="Fixed Portion" />
                    <Border BorderWidth="2px" />
                </SAHL:DXGridView>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <SAHL:SAHLButton runat="server" ID="btnBack" OnClick="btnBack_Click" Text="Back" />
            </td>
        </tr>
    </table>
</asp:Content>
