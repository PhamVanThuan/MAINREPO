<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.Master"
    CodeBehind="ChangeOfCircumstance.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.ChangeOfCircumstance" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div class="TableHeaderB">
        Change in circumstance</div>
    <br />
    <table width="100%" class="tableStandard">
        <tr>
            <td class="TitleText" width="80px">
                17.3 Date:
            </td>
            <td class="LabelText">
                <SAHL:SAHLDateBox runat="server" ID="dte17point3" />
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Comments:
            </td>
            <td class="LabelText">
                <SAHL:SAHLTextBox runat="server" ID="txtComments" TextMode="MultiLine" Width="98%" Height="64px" MaxLength="2000" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="right">
                <SAHL:SAHLButton runat="server" ID="btnSave" Text="Save" OnClick="btnSubmit_Click" />
                <SAHL:SAHLButton runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
