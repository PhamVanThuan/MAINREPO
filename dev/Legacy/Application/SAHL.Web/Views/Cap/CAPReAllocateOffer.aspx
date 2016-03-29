<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Cap.CAPReAllocateOffer" Title="CAP ReAllocateOffer"
    Codebehind="CAPReAllocateOffer.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 99%;" valign="top">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 20%;" class="TitleText">
                            Allocated To
                        </td>
                        <td style="width: 40%;">
                            <SAHL:SAHLDropDownList ID="AllocatedToList" runat="server" PleaseSelectItem="true"
                                AutoPostBack="True" Style="width: 100%;">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td style="width: 40%;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="NTUReasonRow" runat="server" visible="false">
                        <td colspan="3">
                            <asp:CheckBox ID="SelectAll" runat="server" Text="Select All" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 140px">
                            <SAHL:SAHLGridView ID="AllocateGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                                EnableViewState="false" GridHeight="360px" GridWidth="100%" Width="100%" HeaderCaption="CAP Offers"
                                NullDataSetMessage="" EmptyDataSetMessage="There are no CAP Offers." OnRowDataBound="AllocateGrid_RowDataBound">
                                <HeaderStyle CssClass="TableHeaderB" />
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Re-Allocate To
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="ReAllocateToList" runat="server" PleaseSelectItem="true"
                                Style="width: 100%;">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                            <SAHL:SAHLRequiredFieldValidator ID="valReAllocate" runat="server" ControlToValidate="ReAllocateToList"
                                ErrorMessage="Please select a Broker" InitialValue="-select-" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="UpdateButton" runat="server" Text="Update" AccessKey="U" OnClick="UpdateButton_Click"
                    CausesValidation="true" />
            </td>
        </tr>
    </table>
</asp:Content>
