<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="InternetLeadUsers.aspx.cs" Inherits="SAHL.Web.Views.Administration.InternetLeadUsers"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <asp:Panel ID="pnlAttorneyDetails" runat="server" Width="100%" Height="400px" GroupingText="Manage Internet Lead Users"
        Visible="true" >
        <table class="tableStandard" width="100%" height="400">
            <tr>
                <td align="left" valign="top" style="height: 296px">
                    <table id="InfoTable" runat="server" class="tableStandard" width="100%" height="100%">
                        <tr>
                            <td style="width: 40%;" class="TitleText">
                                Active Users</td>
                            <td style="width: 20%;" class="TitleText">
                            </td>
                            <td style="width: 40%;" class="TitleText">
                                Inactive Users</td>
                        </tr>
                        <tr>
                            <td style="width: 40%; height: 400px;" class="TitleText">
                                <SAHL:SAHLGridView ID="lstActiveUsers" runat="server" AutoGenerateColumns="False"
                                    EmptyDataSetMessage="There are no Active Users." EnableViewState="false" FixedHeader="false"
                                     GridWidth="100%" NullDataSetMessage="" PostBackType="SingleAndDoubleClick"
                                    Width="99%"  GridHeight="400px" OnSelectedIndexChanged="lstActiveUsers_SelectedIndexChanged" Height="12%" PageSize="99">
                                    <RowStyle CssClass="TableRowA" />
                                </SAHL:SAHLGridView>
                            </td>
                            <td  align="center" style="width: 20%; text-align: center; height: 100%;" class="TitleText">
                                <SAHL:SAHLButton ID="btnAdd" runat="server" Text="<  Activate" OnClick="btnAdd_Click" />
                                <br />
                                <SAHL:SAHLButton ID="btnRemove" runat="server" Text="Deactivate >" OnClick="btnRemove_Click" /></td>
                            <td style="width: 40%; height: 400px;" class="TitleText">
                                <SAHL:SAHLGridView ID="lstInactiveUsers" runat="server" AutoGenerateColumns="False"
                                    EmptyDataSetMessage="There are no Inactive Users." EnableViewState="false" FixedHeader="false"
                                     GridWidth="100%"   GridHeight="400px" NullDataSetMessage="" PostBackType="SingleAndDoubleClick"
                                    Width="99%" OnSelectedIndexChanged="lstInactiveUsers_SelectedIndexChanged" Height="12%">
                                    <RowStyle CssClass="TableRowA" />
                                </SAHL:SAHLGridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>
    <div class="buttonBar" style="width: 99%; text-align: right;">
        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" CausesValidation="false" OnClick="btnCancel_Click" />&nbsp;
        <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Save Changes" AccessKey="S" OnClick="btnUpdate_Click" />
            </div>
</asp:Content>
