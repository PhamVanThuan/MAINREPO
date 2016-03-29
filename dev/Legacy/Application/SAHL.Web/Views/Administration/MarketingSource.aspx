<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.master"
    Codebehind="MarketingSource.aspx.cs" Inherits="SAHL.Web.Views.Administration.MarketingSource" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 103%;" valign="top">
                <br />
                <SAHL:SAHLGridView ID="gvMarketingSources" runat="server" AutoGenerateColumns="false"
                    EmptyDataSetMessage="There are no Marketing Sources." EnableViewState="false"
                    FixedHeader="false" GridHeight="200px" GridWidth="100%" HeaderCaption="Marketing Source"
                    Width="100%" NullDataSetMessage="" OnRowDataBound="gvMarketingSources_RowDataBound"
                    OnSelectedIndexChanged="gvMarketingSources_SelectedIndexChanged" PostBackType="SingleClick">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
                <br />
                <asp:Panel ID="UpdatePanel" runat="server" Style="width: 99%">
                    <table border="0" class="tableStandard">
                        <tr>
                            <td style="width: 30%">
                                <SAHL:SAHLLabel ID="MarketingSourceDescriptionTitle" runat="server" Text="Description"></SAHL:SAHLLabel>
                            </td>
                            <td style="width: 50%">
                                <SAHL:SAHLTextBox ID="txtMarketingSourceDescription" Width="200px" runat="server"
                                    MaxLength="200" Mandatory="true" CssClass="mandatory"></SAHL:SAHLTextBox>
                            </td>
                            <td style="width: 20%">
                                &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%">
                                <SAHL:SAHLLabel ID="MarketingSourceStatusTitle" runat="server" Text="Status"></SAHL:SAHLLabel>
                            </td>
                            <td style="width: 50%">
                                <SAHL:SAHLDropDownList ID="ddlMarketingSourceStatus" runat="server" Style="width: 120px;">
                                </SAHL:SAHLDropDownList>
                                <SAHL:SAHLLabel ID="lblMarketingSourceStatus" runat="server" Text=""></SAHL:SAHLLabel>
                                <SAHL:SAHLRequiredFieldValidator ID="valStatus" runat="server" ControlToValidate="ddlMarketingSourceStatus"
                                    ErrorMessage="Please select a Status" InitialValue="-select-" />
                            </td>
                            <td style="width: 20%">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="Cancel_Click"
                    CausesValidation="false" />
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Update" AccessKey="S" OnClick="Submit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
