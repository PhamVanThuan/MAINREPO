<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalRole.aspx.cs" MasterPageFile="~/MasterPages/Blank.Master"
    Inherits="SAHL.Web.Views.DebtCounselling.ExternalRole" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <table class="tableStandard" width="100%">
        <tr>
            <td class="TitleText" style="width: 20%">
                Litigation Attorney : 
            </td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblSelectedAttorney" runat="server">
                </SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Debt Counsellor : 
            </td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblSelectedDebtCounsellor" runat="server">
                </SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Payment Distribution Agency :
            </td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblSelectedPDA" runat="server">
                </SAHL:SAHLLabel>
            </td>
        </tr>
    </table>
</asp:Content>
