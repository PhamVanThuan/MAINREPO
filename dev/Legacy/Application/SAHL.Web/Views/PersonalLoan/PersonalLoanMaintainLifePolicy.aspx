<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" Title="Maintain Life Policy"
    CodeBehind="PersonalLoanMaintainLifePolicy.aspx.cs"
    Inherits="SAHL.Web.Views.PersonalLoan.PersonalLoanMaintainLifePolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <table width="100%" class="tableStandard">
        <tr class="rowStandard">
            <td class="TitleText">Insurer</td>
            <td class="TitleText">
                <SAHL:SAHLLabel ID="lblInsurer" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtInsurer" runat="server" Mandatory="True"></SAHL:SAHLTextBox></td>
        </tr>
        <tr class="rowStandard">
            <td class="TitleText">Policy Number</td>
            <td class="TitleText">
                <SAHL:SAHLLabel ID="lblPolicyNumber" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtPolicyNumber" runat="server" Mandatory="True"></SAHL:SAHLTextBox></td>
        </tr>
        <tr class="rowStandard">
            <td class="TitleText">Commencement Date</td>
            <td class="TitleText">
                <SAHL:SAHLLabel ID="lblCommencementDate" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLDateBox ID="dtCommencementDate" runat="server" Mandatory="True"></SAHL:SAHLDateBox></td>
        </tr>
        <tr class="rowStandard">
            <td class="TitleText">Status</td>
            <td class="TitleText">
                <SAHL:SAHLLabel ID="lblStatus" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLDropDownList ID="ddlStatus" runat="server" Mandatory="True"></SAHL:SAHLDropDownList></td>
        </tr>
        <tr class="rowStandard">
            <td class="TitleText">Close Date</td>
            <td class="TitleText">
                <SAHL:SAHLLabel ID="lblClosedate" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLDateBox ID="dtClosedate" runat="server" Mandatory="True"></SAHL:SAHLDateBox></td>
        </tr>
        <tr class="rowStandard">
            <td class="TitleText">Sum Insured</td>
            <td class="TitleText">
                <SAHL:SAHLLabel ID="lblSumInsured" runat="server" Visible="false">-</SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtSumInsured" runat="server" Mandatory="True"></SAHL:SAHLTextBox></td>
        </tr>
        <tr class="rowStandard">
            <td class="TitleText">Policy Ceded</td>
            <td class="TitleText">
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