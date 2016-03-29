<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.BankingDetailsSearch" Title="Banking Details Search"
    Codebehind="BankingDetailsSearch.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 99%;" valign="top">
                <SAHL:SAHLGridView ID="BankDetailsSearchGrid" runat="server" AutoGenerateColumns="False"
                    FixedHeader="false" EnableViewState="false" GridHeight="350px" GridWidth="100%"
                    Width="100%" HeaderCaption="Clients Related to this Account" NullDataSetMessage=""
                    EmptyDataSetMessage="There are no other related Clients." OnRowDataBound="BankDetailsSearchGrid_RowDataBound"
                    OnSelectedIndexChanged="BankDetailsSearchGrid_SelectedIndexChanged" PostBackType="NoneWithClientSelect">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right" style="height: 24px">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" />&nbsp;
                <SAHL:SAHLButton ID="UseButton" runat="server" Text="Use" AccessKey="S" OnClick="SelectButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
