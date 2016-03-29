<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" CodeBehind="IncomeAndValuation.aspx.cs" Inherits="SAHL.Web.Views.FurtherLending.IncomeAndValuation" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    &nbsp;<table style="width: 100%" id="TABLE1">
        <tr>
            <td align="center" style="width: 42%">
            </td>
            <td align="center" colspan="1">
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;</td>
            <td align="left" colspan="1">
                <asp:CheckBox ID="chkValuation" runat="server" Text="New Valuation required  " /></td>
        </tr>
        <tr>
            <td align="center">
            </td>
            <td align="left" colspan="1">
                <asp:CheckBox ID="chkIncome" runat="server" Text="Income Verification required" /></td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;</td>
            <td align="center" colspan="1">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <SAHL:SAHLButton id="btnSubmit" runat="server" onclick="btnSubmit_Click" text="Submit" /></td>
        </tr>
    </table>
</asp:Content>