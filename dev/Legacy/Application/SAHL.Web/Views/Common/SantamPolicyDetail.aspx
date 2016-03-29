<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="True"
    Codebehind="SantamPolicyDetail.aspx.cs" Inherits="SAHL.Web.Views.Common.SantamPolicyDetail" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div style="text-align: left">
        <table width="100%" class="tableStandard">
            <tr>
                <td align="center" class="TableHeaderA" colspan="4">
                    Santam Policy Detail</td>
            </tr>
        </table>
        <asp:Panel ID="pnlPolicyDisplay" runat="server">
            <br />
            <table width="100%" class="tableStandard">
                <tr class="rowStandard">
                    <td style="width: 20%;" class="TitleText">
                        Loan Number
                    </td>
                    <td style="width: 30%;" class="TitleText">
                        <asp:Label ID="lblLoanNumber" runat="server"></asp:Label>
                    </td>
                    <td style="width: 20%;" class="TitleText">
                        Policy Reference
                    </td>
                    <td style="width: 30%;" class="TitleText">
                        <asp:Label ID="lblPolicyReference" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="rowStandard">
                    <td style="width: 20%;" class="TitleText">
                        Quote Reference
                    </td>
                    <td class="TitleText">
                        <asp:Label ID="lblQuoteReference" runat="server"></asp:Label>
                    </td>
                    <td style="width: 20%;" class="TitleText">
                        Monthly Premium
                    </td>
                    <td class="TitleText">
                        <asp:Label ID="lblMonthlyPremium" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="rowStandard">
                    <td style="width: 20%;" class="TitleText">
                        Policy Status
                    </td>
                    <td class="TitleText">
                        <asp:Label ID="lblPolicyStatus" runat="server"></asp:Label>
                    </td>
                    <td style="width: 20%;" class="TitleText">
                        Payment Debit Order Date
                    </td>
                    <td class="TitleText">
                        <asp:Label ID="lblDebitOrderDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="rowStandard">
                    <td style="width: 20%;" class="TitleText">
                        Product Provider
                    </td>
                    <td class="TitleText">
                        <asp:Label ID="lblProductProvider" Text="Santam - Multiplex product" runat="server"></asp:Label>
                    </td>
                    <td style="width: 20%;" class="TitleText">
                        Open Date
                    </td>
                    <td class="TitleText">
                        <asp:Label ID="lblOpenDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="rowStandard">
                    <td style="width: 20%;" class="TitleText">
                    </td>
                    <td>
                    </td>
                    <td style="width: 20%;" class="TitleText">
                        Close Date
                    </td>
                    <td class="TitleText">
                        <asp:Label ID="lblCloseDate" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlPolicyNone" runat="server">
            <table width="100%" class="tableStandard">
                <tr>
                    <td align="center" class="TitleText">
                        No Santam Policy Details Exist</td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
