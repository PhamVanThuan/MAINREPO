<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.DisbursementHistory" Title="Disbursement History"
    Codebehind="DisbursementHistory.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 103%;" valign="top">
                <table border="0">
                    <tr>
                        <td style="width: 100px;" class="TitleText">
                            Filter For
                        </td>
                        <td style="width: 220px;">
                            <SAHL:SAHLDropDownList ID="FilterDropDown" runat="server" Style="width: 98%;" AutoPostBack="true">
                            </SAHL:SAHLDropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <SAHL:SAHLGridView ID="DisbursementGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="250px" GridWidth="100%"
                    Width="100%" HeaderCaption="Disbursements" NullDataSetMessage="There are no Disbursements."
                    EmptyDataSetMessage="There are no Disbursements." OnRowDataBound="DisbursementGrid_RowDataBound">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
                <table class="tableStandard" width="100%">
                    <tr>
                        <td style="width: 620px;" class="TitleText" align="right">
                            Total
                        </td>
                        <td style="width: 145px;" align="right">
                            <SAHL:SAHLLabel ID="TotalDisbursements" runat="server">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
