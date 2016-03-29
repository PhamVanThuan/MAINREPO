<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="TransactionAccessMaintenance.aspx.cs" Inherits="SAHL.Web.Views.Common.TransactionAccessMaintenance"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table class="tableStandard" width="100%">
        <tr>    
            <td align="left" style="height: 99%;" valign="top">
                <table class="tableStandard" width="100%">
                    <tr runat="server" id="trGroup">
                        <td class="TitleText" style="width: 190px;">
                            Transaction Access Group
                        </td>
                        <td valign="top" style="width: 300px;">
                            <SAHL:SAHLTextBox ID="tbGroup" runat="server" Style="width: 100%;"></SAHL:SAHLTextBox>
                            <SAHL:SAHLDropDownList ID="ddlGroup" runat="server" Style="width: 100%;" PleaseSelectItem="false"
                                AutoPostBack="true">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                            <SAHL:SAHLRequiredFieldValidator ID="ValTxnAccessGroup" runat="server" ControlToValidate="tbGroup"
                                ErrorMessage="Please enter a Transaction Access Group" InitialValue="" />
                        </td>
                    </tr>
                    <tr runat="server" id="trUser">
                        <td class="TitleText" style="width: 190px;">
                            Transaction Access User
                        </td>
                        <td valign="top" style="width: 300px;">
                            <SAHL:SAHLDropDownList ID="ddlAdUser" runat="server" Style="width: 100%;" PleaseSelectItem="false"
                                AutoPostBack="true">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <table class="tableStandard" width="100%">
                    <tr>
                        <td id="TranHeader" class="TableHeaderA" style="width: 770px;" align="center">
                            Transaction Types
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Panel ID="TranTreePanel" runat="server" Width="100%" Height="400px" Style="border: 1px solid #C0C0C0;
                                overflow-y: scroll">
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trButton" runat="server">
            <td align="right" style="padding-top: 10px;">
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click"
                    CausesValidation="False" />
                <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" AccessKey="S" OnClick="btnSubmit_Click"
                    CausesValidation="False" />
            </td>
        </tr>
    </table>
</asp:Content>
