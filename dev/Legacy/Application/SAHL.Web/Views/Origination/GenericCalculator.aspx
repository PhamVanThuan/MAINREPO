<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="GenericCalculator.aspx.cs" Inherits="SAHL.Web.Views.Origination.GenericCalculator"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <script type="text/javascript">
        function NoChange()
        {
            window.event.returnValue=true;
            window.event.keyCode = 0;
            return;
        } 
        
        function SetInterestRate()
        {
//            debugger;
            // get the Link Rate and the Market Rate
            var linkRate = document.getElementById("<%= cboLnkRate.ClientID %>").options[document.getElementById("<%= cboLnkRate.ClientID %>").selectedIndex].text;
//            var baseRate = document.getElementById("<%= lblMarketRate.ClientID %>").value; // this is for a textbox
            var baseRate = document.getElementById("<%= lblMarketRate.ClientID %>").innerHTML; // this is for a label
            
            // cast the values to float types
            linkRate = linkRate.toString().replace('%','');
            linkRate = parseFloat(linkRate);
            baseRate = baseRate.toString().replace('%','');
            baseRate = parseFloat(baseRate);
            
            // add them together to get the Interest Rate
            var total = linkRate + baseRate;
            
            // set the Interest Rate
            document.getElementById("<%= lblInterestRate.ClientID %>").innerText = total.toFixed(2) + ' %'; 
        }
    </script>

    <table class="tableStandard" width="100%">
        <tr align="left">
            <td align="left"  valign="top">
                <br />
                <asp:Panel ID="pnlInfo" runat="server" GroupingText="Generic Loan Calculator" HorizontalAlign="Left" Width="380px">
                    <table id="InfoTable" class="tableStandard" width="100%">
                        <tr>
                            <td align="right" style="width: 150px;">
                                Company: </td>
                            <td style="width: 163px" class="cellDisplay">
                                <SAHL:SAHLDropDownList ID="cboCompany" runat="server" CssClass="CboText"
                                    PleaseSelectItem="False" AutoPostBack="True" OnSelectedIndexChanged="cboCompany_SelectedIndexChanged" >
                                </SAHL:SAHLDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 150px;">
                                Value to Calculate: </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLDropDownList ID="cboValueToCalculate" runat="server" CssClass="CboText"
                                     PleaseSelectItem="false" AutoPostBack="True" OnSelectedIndexChanged="ValueToCalculate_SelectedIndexChanged">
                                </SAHL:SAHLDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 150px;">
                                Remaining Term: </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLTextBox ID="txtRemainingTerm" runat="server" DisplayInputType="Number" MaxLength="3" Width="67px"></SAHL:SAHLTextBox>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 150px;">
                                Link Rate: </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLDropDownList ID="cboLnkRate" runat="server" CssClass="CboText"
                                    PleaseSelectItem="False">
                                </SAHL:SAHLDropDownList>
<%--                                <ajaxToolkit:CascadingDropDown ID="ccdLinkRates" runat="server"
                                    TargetControlID="cboLnkRate"
                                    ParentControlID="cboCompany"
                                    Category="LinkRates"
                                    PromptText=""
                                    PromptValue=""
                                    EmptyText="- Please select -"
                                    EmptyValue="-select-"                                  
                                    LoadingText="[Loading Link Rates...]"
                                    ServicePath="~/AJAX/LinkRates.asmx"
                                    ServiceMethod="GetLinkRatesByOriginationSource" ContextKey="" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 150px;">
                                Market Rate: </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblMarketRate" runat="server" Width="67px"></SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 150px;">
                                Interest Rate: </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblInterestRate" runat="server"></SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 150px;">
                                Current Balance: </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLCurrencyBox ID="txtCurrentBalance" runat="server"></SAHL:SAHLCurrencyBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 150px;">
                                Instalment - Capital: </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblInstallmentCapital" runat="server"></SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 150px;">
                                Instalment - Interest: </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblInstallmentInterest" runat="server"></SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 150px;">
                                Instalment - Total: </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLCurrencyBox ID="txtInstallmentTotal" runat="server" ></SAHL:SAHLCurrencyBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 150px;">
                                Total Interest: </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblTotalInterest" runat="server" EnableTheming="True"></SAHL:SAHLLabel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="Amortisation" runat="server" Text="Amortisation Schedule" AccessKey="A"
                    OnClick="AmortisationScheduleButton_Click" ButtonSize="Size6" Enabled="False" />
                <SAHL:SAHLButton ID="Reset" runat="server" Text="Reset" AccessKey="R" CausesValidation="False"
                    UseSubmitBehavior="false" OnClick="ResetButton_Click" />
                <SAHL:SAHLButton ID="Calculate" runat="server" Text="Calculate" AccessKey="C" OnClick="CalculateButton_Click" />&nbsp;&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
