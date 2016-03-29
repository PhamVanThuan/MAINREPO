<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.QuarterlyLoanStatement" Title="Quarterly Loan Statement"
    Codebehind="QuarterlyLoanStatement.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 99%;" valign="top">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 30%" class="TitleText">
                            Origination Source
                        </td>
                        <td style="width: 50%">
                            <SAHL:SAHLDropDownList ID="ddlOriginationSource" runat="server" Style="width: 100%"
                                PleaseSelectItem="true">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td style="width: 20%">
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Reset Configuration
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="ddlResetConfiguration" runat="server" Style="width: 100%"
                                PleaseSelectItem="true">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Statement Months
                        </td>
                        <td>
                            <SAHL:SAHLTextBox ID="txtStatementMonths" runat="server" Style="width: 10%" Text="3"
                                DisplayInputType="Number"></SAHL:SAHLTextBox>
                        </td>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="Samples" runat="server" GroupingText="Samples Extract" Style="width: 95%">
                    <table border="0" style="width: 95%">
                        <tr>
                            <td style="width: 85%">
                                <SAHL:SAHLLabel ID="label1" runat="server" Text="Please select the sample accounts to you want to extract"></SAHL:SAHLLabel>
                            </td>
                            <td style="width: 15%">
                                <SAHL:SAHLLabel ID="label2" runat="server" Text="Sample Size"></SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="SampleType1" runat="server" Text="Accounts closed in the last 3 months" />
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="SampleType1Count" runat="server" Text="5" DisplayInputType="Number"
                                    Style="width: 35px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="SampleType2" runat="server" Text="Vanilla Variable Loans" />
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="SampleType2Count" runat="server" Text="5" DisplayInputType="Number"
                                    Style="width: 35px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="SampleType3" runat="server" Text="Variable opting into VariFix in the last 3 months" />
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="SampleType3Count" runat="server" Text="5" DisplayInputType="Number"
                                    Style="width: 35px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="SampleType4" runat="server" Text="VariFix Account Closed in the last 3 months" />
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="SampleType4Count" runat="server" Text="5" DisplayInputType="Number"
                                    Style="width: 35px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="SampleType5" runat="server" Text="Open Super Low Accounts" />
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="SampleType5Count" runat="server" Text="5" DisplayInputType="Number"
                                    Style="width: 35px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="SampleType6" runat="server" Text="Interest Only" />
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="SampleType6Count" runat="server" Text="5" DisplayInputType="Number"
                                    Style="width: 35px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="SampleType7" runat="server" Text="Variable Loan with a Subsidy" />
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="SampleType7Count" runat="server" Text="5" DisplayInputType="Number"
                                    Style="width: 35px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="SampleType8" runat="server" Text="Loans moving SPV" />
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="SampleType8Count" runat="server" Text="5" DisplayInputType="Number"
                                    Style="width: 35px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="SampleType9" runat="server" Text="Loans with Regent Life" />
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="SampleType9Count" runat="server" Text="5" DisplayInputType="Number"
                                    Style="width: 35px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="SampleType10" runat="server" Text="Cap Clients" />
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="SampleType10Count" runat="server" Text="5" DisplayInputType="Number"
                                    Style="width: 35px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <table cellpadding="0" cellspacing="0" width="95%">
                    <tr>
                        <td colspan="2" class="TitleText">
                            Please provide the username(s) of the people that should receive notification of
                            batch completion.
                        </td>
                    </tr>
                    <tr>
                        <td width="260">
                            <SAHL:SAHLTextBox runat="server" Rows="4" ID="MailAddress" Mandatory="true" Height="90px"
                                Width="260px" TextMode="MultiLine"></SAHL:SAHLTextBox>
                        </td>
                        <td valign="top">
                            &nbsp
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Run Batch" AccessKey="S"
                    Visible="true" OnClick="SubmitButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
