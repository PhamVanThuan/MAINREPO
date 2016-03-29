<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.Master"
    CodeBehind="AssignCase.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.AssignCase" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div style="text-align: left">
        <table width="100%" class="tableStandard">
            <tr>
                <td align="center" class="TableHeaderA" colspan="4">
                    Assign Case
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlAssignCase" runat="server">
            <table width="100%" class="tableStandard">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMessage" Text="Message goes here" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="rowStandard">
                    <td class="TitleText">
                        User
                    </td>
                    <td class="TitleText">
                        <SAHL:SAHLDropDownList ID="ddlUser" runat="server">
                        </SAHL:SAHLDropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <br />
                        <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="OnCancelClick"
                            CausesValidation="false" />
                        <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="OnSubmitClick" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
