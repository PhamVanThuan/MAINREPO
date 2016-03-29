<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="PostTransaction.aspx.cs" Inherits="SAHL.Web.Views.Common.PostTransaction"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" class="PageBlock" style="width: 100%">
        <tr>
            <td style="width: 15%; height: 22px;" class="TitleText">
                Transaction Type
            </td>
            <td style="width: 50%; height: 22px;">
                <SAHL:SAHLDropDownList ID="ddlTransactionType" runat="server" OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged" AutoPostBack="true"   
                    Style="width: 100%;">
                </SAHL:SAHLDropDownList>
            </td>
            <td style="width: 40%; height: 22px;">
            </td>
        </tr>
        <tr runat="server" id="financialServiceRow" visible="false">
            <td style="width: 10%" class="TitleText">
                Financial Service
            </td>
            <td style="width: 50%">
                <SAHL:SAHLDropDownList ID="ddlFinancialService" runat="server" Style="width: 100%;">
                </SAHL:SAHLDropDownList>
            </td>
            <td style="width: 40%">
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Effective Date
            </td>
            <td>
                <SAHL:SAHLDateBox ID="dteEffectiveDate" runat="server"></SAHL:SAHLDateBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Amount
            </td>
            <td>
                <SAHL:SAHLCurrencyBox ID="curAmount" runat="server" DisplayInputType="Currency" MaxLength="12">0.00</SAHL:SAHLCurrencyBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Reference
            </td>
            <td>
                <SAHL:SAHLTextBox ID="txtReference" runat="server" MaxLength="50" Style="width: 98%;"></SAHL:SAHLTextBox>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr id="ButtonRow" runat="server">
            <td style="width: 10%">
            </td>
            <td style="width: 50%">
            </td>
            <td style="width: 40%" align="left">
                <SAHL:SAHLButton ID="PostButton" runat="server" Text="Post" AccessKey="P" OnClick="btnPost_Click" />
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click"
                    CausesValidation="False" />
            </td>
        </tr>
    </table>
</asp:Content>
