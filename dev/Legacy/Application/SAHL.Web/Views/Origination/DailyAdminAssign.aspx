<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Origination.DailyAdminAssign" Title="DailyAdminAssign"
    CodeBehind="DailyAdminAssign.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div>
        <br />
        <table id="tblRoleTypes" runat="server" visible="true" style="width: 100%; height: 100%">
            <tr style="width: 100%; height: 100%">
                <td width="10%">
                    <SAHL:SAHLLabel ID="lblRoleType" runat="server" CssClass="TitleText"
                        Font-Bold="true">Role Type</SAHL:SAHLLabel>
                    </td>
                        <td width="30%">
                            <SAHL:SAHLDropDownList ID="ddlRoleTypes" runat="server" Width="100%" OnSelectedIndexChanged="ddlRoleTypesSelectedIndexChanged"
                                AutoPostBack="true">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td width="60%">
                        </td>
            </tr>
        </table>
        <br />
        <table id="tblADUserStatusUpdate" runat="server" visible="true" style="width: 100%;
            height: 100%">
            <tr style="width: 100%; height: 100%">
                <td>
                    <SAHL:DXGridView ID="gvADUserStatusUpdate" runat="server" AutoGenerateColumns="False"
                        Width="100%" PostBackType="None" ClientInstanceName="grid" Settings-ShowTitlePanel="true"
                        SettingsText-Title="User Status Maintenance" OnRowUpdating="gvADUserStatusUpdate_RowUpdating">
                        <SettingsEditing Mode="Inline" />
                        <ClientSideEvents RowClick="function(s, e) {s.StartEditRow(e.visibleIndex);}" />
                    </SAHL:DXGridView>
                </td>
            </tr>
        </table>
        <br />
        <table id="ButtonRow" runat="server" visible="true" style="width: 100%; height: 100%">
            <tr style="width: 100%; height: 100%">
                <td align="right">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="Cancel_Click" />
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" AccessKey="U" OnClick="Submit_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
