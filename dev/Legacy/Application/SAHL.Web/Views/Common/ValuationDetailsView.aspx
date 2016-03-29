<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="ValuationDetailsView.aspx.cs" Inherits="SAHL.Web.Views.Common.ValuationDetailsView"
    Title="Valuation Details View" EnableViewState="False" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <asp:Panel ID="pnlProperty" runat="server" GroupingText="Property Address" Width="99%">
        <div style="text-align: left">
            <table border="0" style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <SAHL:SAHLGridView ID="grdProperty" runat="server" AutoGenerateColumns="False" EmptyDataSetMessage="No addresses found"
                            EnableViewState="false" FixedHeader="false" GridHeight="50px" GridWidth="100%"
                            NullDataSetMessage="No addresses found" Width="99%" EmptyDataText="No addresses found">
                            <RowStyle CssClass="TableRowA" />
                        </SAHL:SAHLGridView>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlValuations" runat="server" GroupingText="Valuations" Width="99%"
        Visible="False">
        <SAHL:SAHLGridView ID="grdValuations" runat="server" AutoGenerateColumns="False"
            EmptyDataSetMessage="There are no Valuations." EnableViewState="false" FixedHeader="false"
            GridHeight="150px" GridWidth="100%" NullDataSetMessage="There are no Valuations." Width="99%" PostBackType="SingleAndDoubleClick"
            OnSelectedIndexChanged="grdValuations_SelectedIndexChanged" EmptyDataText="There are no Valuations.">
            <RowStyle CssClass="TableRowA" />
        </SAHL:SAHLGridView>
    </asp:Panel>
    <asp:Panel ID="pnlInspectionContactDetails" runat="server" GroupingText="Inspection Contact Details"
        Width="99%">
        <table style="width: 760px">
            <tr>
                <td style="height: 21px">
                    <SAHL:SAHLLabel ID="Label1" runat="server" Text="Contact 1"></SAHL:SAHLLabel></td>
                <td colspan="5" style="height: 21px">
                    <SAHL:SAHLTextBox ID="txtContact1Name" runat="server" Width="388px"></SAHL:SAHLTextBox></td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <SAHL:SAHLLabel ID="Label2" runat="server" Text="Phone"></SAHL:SAHLLabel></td>
                <td style="width: 280px">
                    <SAHL:SAHLTextBox ID="txtContact1Phone" runat="server" Width="120px"></SAHL:SAHLTextBox></td>
                <td style="width: 280px">
                    <SAHL:SAHLLabel ID="Label6" runat="server" Text="Work Phone" Width="92px"></SAHL:SAHLLabel></td>
                <td style="width: 280px">
                    <SAHL:SAHLTextBox ID="txtContact1WorkPhone" runat="server" Width="120px"></SAHL:SAHLTextBox></td>
                <td style="width: 280px">
                    <SAHL:SAHLLabel ID="Label5" runat="server" Text="Mobile Phone" Width="106px"></SAHL:SAHLLabel></td>
                <td style="width: 280px">
                    <SAHL:SAHLTextBox ID="txtContact1MobilePhone" runat="server" Width="120px"></SAHL:SAHLTextBox></td>
            </tr>
            <tr>
                <td style="height: 21px">
                    <SAHL:SAHLLabel ID="Label3" runat="server" Text="Contact 2"></SAHL:SAHLLabel></td>
                <td colspan="5" style="height: 21px">
                    <SAHL:SAHLTextBox ID="txtContact2Name" runat="server" Width="388px"></SAHL:SAHLTextBox></td>
            </tr>
            <tr>
                <td>
                    <SAHL:SAHLLabel ID="Label4" runat="server" Text="Phone"></SAHL:SAHLLabel></td>
                <td>
                    <SAHL:SAHLTextBox ID="txtContact2Phone" runat="server" Width="120px"></SAHL:SAHLTextBox></td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <table width="99%">
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="center" valign="bottom" style="text-align: left">
                <SAHL:SAHLLabel ID="lblErrorMessage" runat="server" Font-Bold="True" ForeColor="Red"
                    Visible="False" Width="100%"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td align="center" style="text-align: right" valign="bottom">
                <SAHL:SAHLButton ID="btnProperty" runat="server" Text="Select Property" Visible="False"
                    Width="153px" OnClick="btnProperty_Click" ButtonSize="Size6" />
                <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Request AVM" Visible="False"
                    Width="153px" OnClick="btnSubmit_Click" ButtonSize="Size6" />
                <SAHL:SAHLButton ID="btnViewDetail" runat="server" Text="View Detail" Visible="False"
                    Width="153px" OnClick="btnViewDetail_Click" />
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" Visible="False" Width="153px"
                    OnClick="btnCancel_Click" /></td>
        </tr>
    </table>
    <br />
    <br />
</asp:Content>
