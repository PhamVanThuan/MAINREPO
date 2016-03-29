<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Codebehind="ConvertStaffLoan.aspx.cs" Title="Convert Staff Loan" Inherits="SAHL.Web.Views.Common.ConvertStaffLoan" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div style="text-align: left">
        <table width="100%" class="tableStandard">
            <tr>
                <td>
                </td>
            </tr>
            <tr class="rowStandard">
                <td style="width: 20%; height: 21px;" class="TitleText">
                    Account Status
                </td>
                <td style="width: 30%; height: 21px;">
                    <SAHL:SAHLLabel ID="lblAccountStatus" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td style="width: 50%; height: 21px;">
                </td>
            </tr>
            <tr class="rowStandard">
                <td style="width: 20%; height: 21px;" class="TitleText">
                    Total Current Balance
                </td>
                <td style="width: 30%; height: 21px;">
                    <SAHL:SAHLLabel ID="lblCurrentBalance" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td style="width: 50%; height: 21px;">
                </td>
            </tr>
            <tr class="rowStandard">
                <td style="width: 20%; height: 21px;" class="TitleText">
                    Total Arrear Balance
                </td>
                <td style="width: 30%; height: 21px;">
                    <SAHL:SAHLLabel ID="lblArrearBalance" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td style="width: 50%; height: 21px;">
                </td>
            </tr>
            <tr class="rowStandard">
                <td style="width: 20%; height: 21px;" class="TitleText">
                    Total Remaining Installments
                </td>
                <td style="width: 30%; height: 21px;">
                    <SAHL:SAHLLabel ID="lblRemainingInstallments" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td style="width: 50%; height: 21px;">
                </td>
            </tr>
            <tr class="rowStandard">
                <td style="width: 20%; height: 21px;" class="TitleText">
                    Total Payment
                </td>
                <td style="width: 30%; height: 21px;">
                    <SAHL:SAHLLabel ID="lblPayment" runat="server">-</SAHL:SAHLLabel>
                </td>
                <td style="width: 50%; height: 21px;">
                </td>
            </tr>
        </table>
        <table>
            <tr>
            <td align="left" style="width: 40%;">
                <asp:Panel ID="pnlVariableML" runat="server" GroupingText="Variable Loan Rates">
                    <table border="0">
                        <tr class="rowStandard">
                            <td style="width: 50%; height: 21px;" class="TitleText">
                                Link Rate
                            </td>
                            <td style="width: 50%; height: 21px;">
                                <SAHL:SAHLLabel ID="lblvMLLinkRate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td style="width: 50%; height: 21px;" class="TitleText">
                                Effective Rate
                            </td>
                            <td style="width: 50%; height: 21px;">
                                <SAHL:SAHLLabel ID="lblvMLEffectiveRate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td style="width: 50%; height: 21px;" class="TitleText">
                                Market Rate
                            </td>
                            <td style="width: 50%; height: 21px;">
                                <SAHL:SAHLLabel ID="lblvMLMarketRate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td align="center" style="width: 20%;">
            </td>
            <td align="right" style="width: 40%;">
                <asp:Panel ID="pnlFixedML" runat="server" GroupingText="Fixed Loan Rates">
                    <table border="0">
                        <tr class="rowStandard">
                            <td style="width: 50%; height: 21px;" class="TitleText">
                                Link Rate
                            </td>
                            <td style="width: 50%; height: 21px;">
                                <SAHL:SAHLLabel ID="lblfMLLinkRate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td style="width: 50%; height: 21px;" class="TitleText">
                                Effective Rate
                            </td>
                            <td style="width: 50%; height: 21px;">
                                <SAHL:SAHLLabel ID="lblfMLEffectiveRate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr class="rowStandard">
                            <td style="width: 50%; height: 21px;" class="TitleText">
                                Market Rate
                            </td>
                            <td style="width: 50%; height: 21px;">
                                <SAHL:SAHLLabel ID="lblfMLMarketRate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            </tr>
        </table>
        <table width="100%" class="tableStandard">
            <tr class="rowStandard">
                <td align="right" style="width: 100%">
                    <SAHL:SAHLButton ID="btnConvert" runat="server" Text="Convert" AccessKey="C" OnClick="btnConvert_Click" />
                    <SAHL:SAHLButton ID="btnUnConvert" runat="server" Text="UnConvert" AccessKey="U" OnClick="btnUnConvert_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
