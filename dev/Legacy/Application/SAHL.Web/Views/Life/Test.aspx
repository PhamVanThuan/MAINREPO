<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="Test.aspx.cs" Inherits="SAHL.Web.Views.Life.Test" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div>
        <asp:Panel ID="pnlAssuredLivesGrid" runat="server" Width="100%">
            <table id="tAssuredLivesGrid" width="100%">
                <tr>
                    <td align="left" style="text-align: left">
                        <SAHL:SAHLGridView ID="LegalEntityGrid" runat="server" AutoGenerateColumns="false"
                            EmptyDataSetMessage="There are no applicants on this account." EmptyDataText="There are no applicants on this account."
                            EnableViewState="false" FixedHeader="false" GridHeight="95px" GridWidth="100%"
                            HeaderCaption="Applicants" NullDataSetMessage="" Width="100%" PostBackType="SingleClick"
                            OnSelectedIndexChanged="LegalEntityGrid_SelectedIndexChanged" OnRowDataBound="LegalEntityGrid_RowDataBound">
                            <HeaderStyle CssClass="TableHeaderB" />
                            <RowStyle CssClass="TableRowA" />
                        </SAHL:SAHLGridView>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="text-align: left">
                        <cc1:LegalEntityGrid ID="LegalEntityGridNew" runat="server" OnSelectedIndexChanged="LegalEntityGridNew_SelectedIndexChanged">
                        </cc1:LegalEntityGrid>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
