<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="PreCheck.aspx.cs" Inherits="SAHL.Web.Views.FurtherLending.PreCheck" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script language="javascript" type="text/javascript">
    </script>

    <table border="0" width="100%" class="tableStandard">
        <tr>
            <td>
                <table>
                    <tr>
                        <td class="TitleText" align="left">Product
                        </td>
                        <td align="left">
                            <SAHL:SAHLLabel ID="lblProduct" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText" align="left">Payment Method&nbsp;&nbsp;&nbsp;
                        </td>
                        <td align="left">
                            <SAHL:SAHLLabel ID="lblPaymentMethod" runat="server">-</SAHL:SAHLLabel>
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLGridView ID="gvDetail" runat="server" AutoGenerateColumns="False"
                    EmptyDataSetMessage="There are no further lending critical detail types loaded against this Account." EnableViewState="False"
                    FixedHeader="False" ShowHeader="False" NullDataSetMessage="There are no further lending critical detail types loaded against this Account." HeaderCaption="Detail Types"
                    Width="100%" EmptyDataText="There are no further lending critical detail types loaded against this Account." GridHeight="100px" GridWidth="100%" Invisible="False" SelectFirstRow="True" TotalsFooter="True">
                </SAHL:SAHLGridView>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLGridView ID="gvConditions" runat="server" AutoGenerateColumns="False"
                    EmptyDataSetMessage="There are no Conditions for this Account." EnableViewState="False"
                    FixedHeader="False" ShowHeader="False" NullDataSetMessage="There are no Conditions for this Account." HeaderCaption="Conditions"
                    Width="100%" EmptyDataText="There are no Conditions for this Account." GridHeight="100px" GridWidth="100%" Invisible="False" SelectFirstRow="True" TotalsFooter="True">
                </SAHL:SAHLGridView>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <%--<SAHL:SAHLGridView ID="FinancialAdjustmentGrid" runat="server" AutoGenerateColumns="false"
                        FixedHeader="false" EnableViewState="false" GridHeight="100px" GridWidth="100%" HeaderCaption="Financial Adjustments"
                        Width="100%" NullDataSetMessage="" EmptyDataSetMessage="There are no Financial Adjustment entries.">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>--%>
                <SAHL:SAHLGridView ID="FinancialAdjustmentGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="150px" GridWidth="100%"
                    Width="100%" HeaderCaption="Financial Adjustments" NullDataSetMessage="" EmptyDataSetMessage="There are no Financial Adjustment entries."
                    OnRowDataBound="FinancialAdjustmentGrid_RowDataBound">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
    </table>

    <table style="width: 98%" runat="server" id="btnbar" class="tableStandard">
        <tr>
            <td style="height: 4px">&nbsp;</td>
        </tr>
        <tr>
            <td align="right" style="width: 25%"></td>
            <td align="right" nowrap="nowrap" style="width: 25%">
                <SAHL:SAHLButton ID="btnCancel" runat="server" ButtonSize="Size5" Visible="true" OnClick="btnCancel_Click" CausesValidation="false" Text="Cancel" />&nbsp;
            </td>
            <td align="left" nowrap="nowrap">
                <SAHL:SAHLButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next ->" Width="100px" CssClass="BtnNormal4" Enabled="False" SecurityTag="FLCalculatorCreateApplication" />
            </td>
        </tr>
    </table>
</asp:Content>
