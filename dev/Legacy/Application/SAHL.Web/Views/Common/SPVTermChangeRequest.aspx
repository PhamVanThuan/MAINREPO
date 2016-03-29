<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    CodeBehind="SPVTermChangeRequest.aspx.cs" Inherits="SAHL.Web.Views.Common.SPVTermChange"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script type="text/javascript" language="javascript">
        function confirmAccountReference() {
            var accountNo = document.getElementById('<%= hidAccountNo.ClientID %>').value;

            if (reference.trim() != accountNo) {
                return confirm('The reference field is not the same as the account number (' + accountNo + ').\n\nAre you sure you want to continue?');
            }
            return true;
        }
    </script>
    <br />
    <br />
    <table border="0" id="DisplayData" runat="server" class="tableStandard" width="99%">
        <tr id="Tr2" runat="server">
            <td id="Td2" style="width: 140px" runat="server">
            </td>
            <td id="Td3" style="width: 460px" runat="server">
            </td>
        </tr>
        <tr runat="server" visible="true">
            <td class="TitleText" style="width: 140px; height: 16px;">
                Loan Number
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblLoanNumber" runat="server" cssclass="LabelText">-</sahl:sahllabel>
            </td>
        </tr>
        <tr runat="server" visible="true">
            <td class="TitleText" style="width: 140px; height: 16px;">
                Loan Agreement
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblLoanAmount" runat="server" cssclass="LabelText">-</sahl:sahllabel>
            </td>
        </tr>
        <tr runat="server" visible="true">
            <td class="TitleText" style="width: 140px; height: 16px;">
                Outstanding Balance
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblOutstandingBalance" runat="server" cssclass="LabelText">-</sahl:sahllabel>
            </td>
        </tr>
        <tr id="ArrearBalanceRow" runat="server" visible="false">
            <td class="TitleText" style="width: 140px; height: 16px;">
                Current SPV
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblCurrentSPV" runat="server" cssclass="LabelText">-</sahl:sahllabel>
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td class="TitleText" style="width: 140px; height: 16px">
                New SPV
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblNewSPV" runat="server" cssclass="LabelText">-</sahl:sahllabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="width: 140px; height: 16px;">
                Initial Term&nbsp;
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblInitialTerm" runat="server" cssclass="LabelText">-</sahl:sahllabel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="width: 140px; height: 16px;">
                Current Term
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblCurrentTerm" runat="server" cssclass="LabelText">-</sahl:sahllabel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="width: 140px; height: 16px;">
                New Requested Term
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblNewTotalTerm" runat="server" cssclass="LabelText">-</sahl:sahllabel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="width: 140px; height: 16px;">
                New Instalment
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblNewInstallment" runat="server">-</sahl:sahllabel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="width: 140px; height: 16px;">
                LTV
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblLTV" runat="server">-</sahl:sahllabel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="width: 140px; height: 16px;">
                Current PTI
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblCurrentPTI" runat="server">-</sahl:sahllabel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="width: 140px; height: 16px;">
                New PTI
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblNewPTI" runat="server">-</sahl:sahllabel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" id="tdRequestedBy" style="width: 140px; height: 16px;">
                Requested By
            </td>
            <td style="height: 16px; width: 460px;">
                <sahl:sahllabel id="lblRequestedBy" runat="server">-</sahl:sahllabel>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:HiddenField ID="hidAccountNo" runat="server" Value="" />
</asp:Content>