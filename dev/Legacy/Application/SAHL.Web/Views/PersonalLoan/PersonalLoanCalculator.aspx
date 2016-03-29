<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="PersonalLoanCalculator.aspx.cs" Inherits="SAHL.Web.Views.PersonalLoan.PersonalLoanCalculator"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //Set button disabled
            $("#<%= btnCreateApplication.ClientID %>").attr("disabled", "disabled");

            $('input').bind("keydown", function (event) {
                // track enter key
                var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
                if (keycode == 13) { // keycode for enter key
                    // force the 'Enter Key' to implicitly click the Update button
                    $("#<%= btnCalculate.ClientID %>").click();
                    return false;
                } else {
                    return true;
                }
            });
        });

        function OnGridRowSelected(s, e) {
            $("#<%= btnCreateApplication.ClientID %>").removeAttr("disabled");

        }
    </script>
    <table width="99%" class="tableStandard">
        <tr valign="top">
            <td align="center">
                <asp:Panel ID="pnlInput" runat="server" GroupingText="Personal Loan Calculator">
                    <table align="center">
                        <tr>
                            <td align="right">
                                Loan Amount:
                            </td>
                            <td style="width: 10px;">
                                &nbsp;
                            </td>
                            <td>
                                <SAHL:SAHLCurrencyBox ID="tbLoanAmount" runat="server" CssClass="mandatory" Width="100px"></SAHL:SAHLCurrencyBox>
                            </td>
                            <td align="right" style="width: 30px;">
                                <img alt="" title="The amount of the loan." src="../../Images/help.gif" align="middle" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Loan Term:
                            </td>
                            <td style="width: 10px;">
                                &nbsp;
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="tbLoanTerm" runat="server" MaxLength="3" Width="50px" DisplayInputType="Number"></SAHL:SAHLTextBox>
                            </td>
                            <td align="right" style="width: 30px;">
                                <img alt="" title="Term of the Loan." src="../../Images/help.gif" align="middle" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Credit Life Policy:
                            </td>
                            <td style="width: 10px;">
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkLifePolicy" runat="server" Checked="true" />
                            </td>
                            <td align="right" style="width: 30px;">
                                <img alt="" title="Ticking this will include credit life premium in total instalment."
                                    src="../../Images/help.gif" align="middle" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Initiation Fee :
                            </td>
                            <td style="width: 10px;">
                                &nbsp;
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblInitiationFee" runat="server"></SAHL:SAHLLabel>
                            </td>
                            <td align="right" style="width: 30px;">
                                <img alt="" title="This is a fee that you will pay only once while actual processing of personal loan."
                                    src="../../Images/help.gif" align="middle" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Monthly Fee :
                            </td>
                            <td style="width: 10px;">
                                &nbsp;
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblMonthlyFee" runat="server"></SAHL:SAHLLabel>
                            </td>
                            <td align="right" style="width: 30px;">
                                <img alt="" title="This is a fee that you will pay every month." src="../../Images/help.gif"
                                    align="middle" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Credit Life Premium :
                            </td>
                            <td style="width: 10px;">
                                &nbsp;
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblLifeCreditpremium" runat="server"></SAHL:SAHLLabel>
                            </td>
                            <td align="right" style="width: 30px;">
                                <img alt="" title="Credit life premium is applicable if CreditLife Policy is ticked."
                                    src="../../Images/help.gif" align="middle" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <p align="center">
                        <SAHL:SAHLButton ID="btnCalculate" runat="server" Text="Calculate" ButtonSize="Size5"
                            CausesValidation="false" CssClass="BtnNormal4" OnClick="OnCalculateClick" /></p>
                    <br />
                    <table runat="server" id="pnlTermSummary" align="center">
                        <tr>
                            <td colspan="10">
                                <div style="max-height: 240px; overflow-y: auto">
                                    <SAHL:DXGridView ID="grdLoanOptions" runat="server" AutoGenerateColumns="false" EnableViewState="true"
                                        KeyFieldName="ID" PostBackType="NoneWithClientSelect" Width="601px" ClientSideEvents-SelectionChanged="function(s, e) { OnGridRowSelected(s, e); }">
                                        <SettingsBehavior AllowSelectSingleRowOnly="True" AllowSort="false" />
                                        <SettingsPager PageSize="200">
                                        </SettingsPager>
                                        <Styles>
                                            <AlternatingRow Enabled="True">
                                            </AlternatingRow>
                                        </Styles>
                                        <Border BorderWidth="2px" />
                                        <Columns>
                                            <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Amount"
                                                Format="GridCurrency" Caption="Loan Amount" VisibleIndex="0">
                                            </SAHL:DXGridViewFormattedTextColumn>
                                            <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Term"
                                                Caption="Term" SortIndex="0" SortOrder="Ascending" Format="GridString" VisibleIndex="1">
                                            </SAHL:DXGridViewFormattedTextColumn>
                                            <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Rate"
                                                Caption="Rate" Format="GridRate" VisibleIndex="2">
                                            </SAHL:DXGridViewFormattedTextColumn>
                                            <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="TotalInstalment"
                                                Format="GridCurrency" Caption="Total Instalment" VisibleIndex="3">
                                            </SAHL:DXGridViewFormattedTextColumn>
                                        </Columns>
                                    </SAHL:DXGridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="right">
                <SAHL:SAHLButton ID="btnCreateApplication" runat="server" Text="Create Application"
                    ButtonSize="Size5" CssClass="BtnNormal4" CausesValidation="false" SecurityTag="CalculatorCreateApplication"
                    OnClick="OnCreateApplicationClick" />&nbsp;
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" ButtonSize="Size5" CssClass="BtnNormal4"
                    CausesValidation="false" OnClick="OnCancelClick" />&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>