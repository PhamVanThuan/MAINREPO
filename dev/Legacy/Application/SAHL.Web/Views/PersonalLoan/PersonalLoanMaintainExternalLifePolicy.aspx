<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" Title="Maintain Life Policy"
    CodeBehind="PersonalLoanMaintainExternalLifePolicy.aspx.cs"
    Inherits="SAHL.Web.Views.PersonalLoan.PersonalLoanMaintainExternalLifePolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <table class="tableStandard">
        <tr >
            <td class="titleText" style="width:300px;">Insurer</td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblInsurer" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLDropDownList ID="ddlInsurer" runat="server" Mandatory="True"></SAHL:SAHLDropDownList></td>
        </tr>
        <tr>
            <td class="titleText" style="width:300px;">Policy Number</td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblPolicyNumber" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtPolicyNumber" runat="server" Mandatory="True"></SAHL:SAHLTextBox></td>
        </tr>
        <tr>
            <td class="titleText" style="width:300px;">Commencement Date</td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblCommencementDate" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLDateBox ID="dtCommencementDate" runat="server" Mandatory="True"></SAHL:SAHLDateBox></td>
        </tr>
        <tr>
            <td class="titleText">Status</td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblStatus" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLDropDownList ID="ddlStatus" runat="server" Mandatory="True" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"></SAHL:SAHLDropDownList></td>
        </tr>
        <tr>
            <td class="titleText">Close Date</td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblClosedate" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLDateBox ID="dtClosedate" runat="server" Mandatory="True"></SAHL:SAHLDateBox></td>
        </tr>
        <tr>
            <td class="titleText">Sum Insured</td>
            <td class="cellDisplay">
                <SAHL:SAHLLabel ID="lblSumInsured" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtSumInsured" runat="server" Mandatory="True"></SAHL:SAHLTextBox></td>
        </tr>
        <tr>
            <td class="titleText">Policy Ceded</td>
            <td class="cellDisplay">
                <SAHL:SAHLCheckbox ID="chkPolicyCeded" runat="server" Mandatory="True"></SAHL:SAHLCheckbox></td>
        </tr>
        <tr>
            <td></td>
            <td align="right">
                <SAHL:SAHLButton runat="server" ID="btnSave" Text="Save" />
                <SAHL:SAHLButton runat="server" ID="btnCancel" Text="Cancel" />
            </td>
        </tr>
    </table>
</asp:Content>