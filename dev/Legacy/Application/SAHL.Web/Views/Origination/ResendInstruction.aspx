<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Origination.ResendInstruction" Title="ResendInstruction"
    Codebehind="ResendInstruction.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" class="tableStandard" width="100%">
        <tr>
            <td align="left">
                <table border="0" width="100%">
                    <tr>
                        <td>
                            <SAHL:SAHLGridView ID="grdInstructions" runat="server" AutoGenerateColumns="false"
                                FixedHeader="false" EnableViewState="false" Width="100%" GridWidth="100%" HeaderCaption="Instruction History"
                                PostBackType="None" NullDataSetMessage="" EmptyDataSetMessage="There is no Instruction History."
                                OnRowDataBound="grdInstruction_RowDataBound">
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table border="0">
                    <tr>
                        <td align="left" style="height: 99%;">
                            <asp:Panel ID="pnlLegalAttorney" runat="server" GroupingText="Select Attorney" Width="395px"
                                Height="80px" Font-Bold="true">
                                <SAHL:SAHLLabel ID="lblDeedsOffice" runat="server" Width="130px">Deeds Office</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlDeedsOffice" runat="server" Width="240px" OnSelectedIndexChanged="ddlDeedsOffice_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </SAHL:SAHLDropDownList>
                                <SAHL:SAHLLabel ID="lblAttorneyname" runat="server" Width="130px">Attorney Name</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlRegistrationAttorney" runat="server" Width="240px">
                                </SAHL:SAHLDropDownList>
                            </asp:Panel>
                        </td>
                        <td align="left" style="height: 99%; width: 340px;">
                            <asp:Panel ID="pnlPreferredAttorney" runat="server" GroupingText="Preferred Attorney (if any)"
                                Width="340px" Height="80px" Font-Bold="true">
                                <SAHL:SAHLLabel ID="lblPreferredAttorney" runat="server" CssClass="LabelText" Width="185px"> Preferred Attorney</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtPreferredAttorney" runat="server" Enabled="False" Width="240px"></SAHL:SAHLTextBox>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="btnUpdateButton" runat="server" AccessKey="U" OnClick="btnUpdateButton_Click"
                    Text="Update" />
                <SAHL:SAHLButton ID="btnSendInstruction" runat="server" AccessKey="S" Text="Send Instruction" OnClick="btnSendInstruction_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
