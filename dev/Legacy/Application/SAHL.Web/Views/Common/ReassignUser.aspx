<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.ReassignUser" Title="Reassign User" Codebehind="ReassignUser.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div>
        <SAHL:SAHLGridView ID="UserRolesGrid" runat="server" Visible="false" AutoGenerateColumns="false"
            FixedHeader="false" EnableViewState="false" GridHeight="200px" GridWidth="100%"
            Width="100%" NullDataSetMessage="" EmptyDataSetMessage="No Records Found" PageSize="20">
            <RowStyle CssClass="TableRowA" />
        </SAHL:SAHLGridView>
        <table border="3" cellspacing="0" cellpadding="0" class="PageBlock" style="width: 100%;
            height: 100%;">
            <tr>
                <td valign="top">
                    <table border="0" style="width: 100%; height: 100%">
                        <tr style="width: 100%;">
                            <td style="width: 100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" style="width: 100%;">
                                            <SAHL:SAHLLabel ID="lblHeaderText" runat="server" CssClass="LabelText" TextAlign="Left"
                                                Font-Bold="true">Please select the user to whom the case should be assigned :</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table style="width: 100%; height: 100%">
                                    <tr id="trRoles" runat="server" visible="false">
                                        <td align="left" style="width: 10%;">
                                            <SAHL:SAHLLabel ID="lblAppRole" runat="server" CssClass="LabelText" TextAlign="left">User Role</SAHL:SAHLLabel>
                                        </td>
                                        <td align="left" style="width: 85%;">
                                            <SAHL:SAHLDropDownList ID="ddlRole" runat="server" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                                            </SAHL:SAHLDropDownList>
                                        </td>
                                        <td align="left" style="width: 5%;">
                                            <SAHL:SAHLRequiredFieldValidator ID="valAppRole" runat="server" ControlToValidate="ddlRole"
                                                ErrorMessage="Please select an Application Role" InitialValue="-select-" />
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                </table>
                                <br />
                                <table style="width: 100%; height: 100%">
                                    <tr id="trConsultants" runat="server" visible="true">
                                        <td align="left" style="width: 10%;">
                                            <SAHL:SAHLLabel ID="titleConsultant" runat="server" CssClass="LabelText" TextAlign="left">Consultant</SAHL:SAHLLabel>
                                        </td>
                                        <td align="left" style="width: 85%;">
                                            <SAHL:SAHLDropDownList ID="ddlConsultant" runat="server" OnSelectedIndexChanged="ddlConsultantSelectedIndexChanged">
                                            </SAHL:SAHLDropDownList>
                                        </td>
                                        <td align="left" style="width: 5%;">
                                            <SAHL:SAHLRequiredFieldValidator ID="valConsultant" runat="server" ControlToValidate="ddlConsultant"
                                                ErrorMessage="Please select a Consultant" InitialValue="-select-" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <table border="3" cellspacing="0" cellpadding="0" class="PageBlock" style="width: 100%;
            height: 100%;">
            <tr id="trComment" runat="server" visible="false">
                <td valign="top" style="width: 10%">
                    <SAHL:SAHLLabel ID="lblMemo" runat="server" CssClass="LabelText" TextAlign="left"
                        Width="100%">Comment:</SAHL:SAHLLabel>
                </td>
                <td valign="top" style="width: 85%">
                    <SAHL:SAHLTextBox ID="txtMemo" Style="width: 100%; height: 60px;" runat="server"
                        TextMode="MultiLine" Width="100%" EnableViewState="False" CssClass="mandatory"
                        Font-Names="Verdana" Font-Size="8pt"></SAHL:SAHLTextBox>
                </td>
                <td style="width: 5%">
                </td>
            </tr>
        </table>
        <br />
        <table border="3" cellspacing="0" cellpadding="0" class="PageBlock" style="width: 100%;
            height: 100%;">
            <tr id="trChkBx" runat="server" visible="false" style="width: 100%">
                <td valign="top" style="width: 20%">
                    <SAHL:SAHLLabel ID="chkLbl" runat="server" CssClass="LabelText" TextAlign="left"
                        Width="100%">ReAssign Branch Consultant Role:</SAHL:SAHLLabel>
                </td>
                <td valign="top" style="width: 75%">
                    <SAHL:SAHLCheckbox ID="chkReassignBC" runat="server" Checked="false" />
                </td>
                <td style="width: 5%">
                </td>
            </tr>
            <tr>
            </tr>
        </table>
        <br />
        <table border="3" cellspacing="0" cellpadding="0" class="PageBlock" style="width: 100%;
            height: 100%;">
            <tr id="ButtonRow" runat="server">
                <td>
                </td>
                <td align="right" valign="bottom">
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" AccessKey="S" OnClick="btnSubmit_Click" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" CausesValidation="false"
                        OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
