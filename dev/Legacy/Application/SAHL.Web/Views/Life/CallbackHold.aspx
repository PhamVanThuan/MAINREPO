<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Life.CallbackHold" Title="Callback Hold" Codebehind="CallbackHold.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <asp:Panel ID="pnlAssuredLivesGrid" runat="server" Width="100%">
            <table id="tAssuredLivesGrid" width="100%" class="tableStandard">
                <tr>
                    <td>
                        <SAHL:SAHLLabel ID="Label1" runat="server" Text="CALLBACK HOLD" Font-Bold="True"
                            Font-Names="Arial" Font-Size="Medium" Font-Underline="True" CssClass="LabelText"></SAHL:SAHLLabel>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        This client has a callback set and will remain in hold mode until the callback time
                        is reached.
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <table border="0" width="100%">
            <tr>
                <td align="left">
                    <SAHL:SAHLGridView ID="gridCallBack" runat="server" AutoGenerateColumns="false" EmptyDataSetMessage="No callbacks have been set on this offer."
                        EnableViewState="false" FixedHeader="false" GridHeight="350px" GridWidth="100%"
                        HeaderCaption="Callback History" NullDataSetMessage="" Width="100%" OnRowDataBound="gridCallBack_RowDataBound">
                        <HeaderStyle CssClass="TableHeaderB" />
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
