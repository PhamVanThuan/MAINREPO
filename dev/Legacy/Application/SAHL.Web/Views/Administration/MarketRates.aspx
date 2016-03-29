<%@ Page Language="C#" AutoEventWireup="true" Codebehind="MarketRates.aspx.cs" Inherits="SAHL.Web.Views.Administration.MarketRates"
    MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 103%;" valign="top">
                <br />
                <SAHL:SAHLGridView ID="MarketRateGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="200px" GridWidth="100%"
                    Width="100%" HeaderCaption="Market Rates" NullDataSetMessage="There are no Market Rates Defined."
                    EmptyDataSetMessage="There are no Market Rates Defined." PostBackType="SingleClick"
                    OnSelectedIndexChanged="MarketRateGrid_SelectedIndexChanged" SelectFirstRow="true">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
                <asp:Panel ID="UpdatePanel" runat="server" GroupingText="Market Rate" Style="width: 99%">
                    <table border="0" class="tableStandard">
                        <tr>
                            <td style="width: 30%">
                                <SAHL:SAHLLabel ID="MarketRateKeyTitle" runat="server" Text="Rate Key"></SAHL:SAHLLabel>
                            </td>
                            <td style="width: 50%">
                                <SAHL:SAHLLabel ID="lblMarketRateKey" runat="server" Text=""></SAHL:SAHLLabel>
                            </td>
                            <td style="width: 20%">
                                &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%">
                                <SAHL:SAHLLabel ID="MarketRateDescriptionTitle" runat="server" Text="Description"></SAHL:SAHLLabel>
                            </td>
                            <td style="width: 50%">
                                <SAHL:SAHLLabel ID="lblMarketRateDescription" runat="server" Text=""></SAHL:SAHLLabel>
                            </td>
                            <td style="width: 20%">
                                &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%">
                                <SAHL:SAHLLabel ID="MarketRateValueTitle" runat="server" Text="Value"></SAHL:SAHLLabel>
                            </td>
                            <td style="width: 50%">
                                <SAHL:SAHLTextBox ID="txtMarketRateValue" runat="server" Text="" DisplayInputType="CurrencyUnLimitedDecimals"></SAHL:SAHLTextBox>
                                <SAHL:SAHLLabel ID="lblMarketRateValue" runat="server" Text=""></SAHL:SAHLLabel>
                            </td>
                            <td style="width: 20%">
                                <SAHL:SAHLRequiredFieldValidator ID="ValtxtMarketRateValue" runat="server" ControlToValidate="txtMarketRateValue"
                                    ErrorMessage="Please enter a new market rate value" InitialValue=""></SAHL:SAHLRequiredFieldValidator>
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
                <SAHL:SAHLCustomValidator ID="ValSubmitValuation" runat="server" ControlToValidate="SubmitValuation"
                    ErrorMessage="The current active debit order may not be deleted" />
                <SAHL:SAHLTextBox ID="SubmitValuation" runat="server" Style="display: none;"></SAHL:SAHLTextBox>
            </td>
        </tr>
    </table>
</asp:Content>
