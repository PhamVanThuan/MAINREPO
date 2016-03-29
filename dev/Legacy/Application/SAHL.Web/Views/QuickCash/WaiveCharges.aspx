<%@ Page Language="C#" MasterPageFile="~/MasterPages/SAHL.Master" AutoEventWireup="true" CodeBehind="WaiveCharges.aspx.cs" Inherits="SAHL.Web.Views.QuickCash.WaiveCharges" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    &nbsp;<SAHL:SAHLGridView ID="gridQuickCash" runat="server" Height="133px" Width="696px">
    </SAHL:SAHLGridView>
    <br />
    <br />
    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="697px">
        <table style="width: 699px">
            <tr>
                <td align="right" style="width: 233px; height: 26px">
                </td>
                <td align="right" style="width: 88px; height: 26px;">
                    <SAHL:SAHLLabel ID="lblInitiationFee" runat="server" CssClass="LabelText" TextAlign="Left">Initiation Fee : </SAHL:SAHLLabel></td>
                <td style="width: 1px; height: 26px;">
                </td>
                <td style="width: 159px; height: 26px">
                <SAHL:SAHLCurrencyBox id="txtInitiationFee" runat="server" Width="169px" />
                </td>
                <td style="height: 26px">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="panelReasons" runat="server" Height="66px" Width="697px">
        &nbsp;<table style="width: 700px">
            <tr>
                <td style="width: 662px; height: 30px">
                </td>
                <td style="width: 33px; height: 30px">
                    <SAHL:SAHLTextBox ID="txtWaiveChargesReason" runat="server" Height="74px" Width="510px"></SAHL:SAHLTextBox></td>
                <td style="height: 30px">
                </td>
            </tr>
            <tr>
                <td style="width: 662px; height: 21px">
                </td>
                <td style="width: 33px; height: 21px">
                </td>
                <td style="height: 21px">
                </td>
            </tr>
            <tr>
                <td style="width: 662px; height: 21px">
                </td>
                <td style="width: 33px; height: 21px">
                    <SAHL:SAHLTextBox ID="txtManagerComments" runat="server" Height="74px" Width="511px"></SAHL:SAHLTextBox></td>
                <td style="height: 21px">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div style="width: 422px; height: 48px">
        <br />
        <SAHL:SAHLLabel ID="SAHLLabel1" runat="server"></SAHL:SAHLLabel></div>
    <asp:Panel ID="Panel2" runat="server" Height="50px" Width="125px">
        <table style="width: 702px">
            <tr>
                <td style="width: 598px">
                </td>
                <td style="width: 26px">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" AccessKey="C" CausesValidation="false"
                        Text="Cancel" OnClick="btnCancel_Click" /></td>
                <td style="width: 40px">
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" AccessKey="S" OnClick="SubmitButton_Click"
                        Text="Submit" /></td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
</asp:Content>
