<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="LoanCalculator.aspx.cs" Inherits="SAHL.Web.Views.Common.LoanCalculator"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <script type="text/javascript">
    function NoChange()
    {
//        debugger;
        window.event.returnValue=true;
        window.event.keyCode = 0;
        return;
    } 
    
    function SameText(elem)
    {
//        debugger;
        var o = document.getElementById("<%= txtRemainingTermF.ClientID %>");

        if ( o != null )
        {
            o.value = elem.value;
        }
    }
    
    function AddVariable()
    {
//        debugger;
        var linkRate = document.getElementById("<%= cboLnkRate.ClientID %>").options[document.getElementById("<%= cboLnkRate.ClientID %>").selectedIndex].text;
        var baseRate = document.getElementById("<%= txtMarketRateV.ClientID %>").value;
        
        linkRate = linkRate.toString().replace('%','');
        linkRate = parseFloat(linkRate);
        baseRate = baseRate.toString().replace('%','');
        baseRate = parseFloat(baseRate);
        
        var total = linkRate + baseRate;
        
        document.getElementById("<%= txtMarketRateV.ClientID %>").value = baseRate.toFixed(2) + ' %';
        document.getElementById("<%= txtInterestRateV.ClientID %>").innerText = total.toFixed(2) + ' %'; 
    }
    
    function AddFixed()
    {
//        debugger;
        var linkRate = document.getElementById("<%= txtLinkRateF.ClientID %>").innerText;//options[document.getElementById("<%= cboLnkRate.ClientID %>").selectedIndex].text;
        var baseRate = document.getElementById("<%= txtMarketRateF.ClientID %>").value;
        
        linkRate = linkRate.toString().replace('%','');
        linkRate = parseFloat(linkRate);
        baseRate = baseRate.toString().replace('%','');
        baseRate = parseFloat(baseRate);
        
        var total = linkRate + baseRate;
        
        document.getElementById("<%= txtMarketRateF.ClientID %>").value = baseRate.toFixed(2) + ' %';
        document.getElementById("<%= txtInterestRateF.ClientID %>").innerText = total.toFixed(2) + ' %'; 
    }
    </script>

    <table class="tableStandard" width="100%">
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <SAHL:SAHLGridView ID="grdMortgageLoan" runat="server" AutoGenerateColumns="false"
                                EmptyDataSetMessage="There are no Services." EnableViewState="false" FixedHeader="false"
                                GridHeight="100px" GridWidth="100%" HeaderCaption="Loans" NullDataSetMessage="no data"
                                ShowFooter="true" OnRowDataBound="grdMortgageLoan_OnRowDataBound">
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="vertical-align:middle">
                                        Value to Calculate:
                                    </td>
                                    <td style="width:20px">
                                    </td>
                                    <td>
                                        <SAHL:SAHLDropDownList ID="cboValueToCalculate" runat="server" CssClass="CboText"
                                            PleaseSelectItem="false" AutoPostBack="True" OnSelectedIndexChanged="cboValueToCalculate_SelectedIndexChanged">
                                        </SAHL:SAHLDropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td style="width:50%">
                                        <asp:Panel ID="pnlVariableLoan" GroupingText="Variable Loan" runat="server">
                                            <table id="VariableLoanTable">
                                                <tr>
                                                    <td>
                                                        Remaining Term:
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLTextBox ID="txtRemainingTermV" runat="server" DisplayInputType="Number"></SAHL:SAHLTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Link Rate: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLDropDownList ID="cboLnkRate" runat="server" CssClass="CboText" PleaseSelectItem="False"
                                                            AutoPostBack="True" OnSelectedIndexChanged="cboLnkRate_SelectedIndexChanged">
                                                        </SAHL:SAHLDropDownList>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="rowDiscountV" visible="false">
                                                    <td>
                                                        Rate Discount: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="lblDiscountV" runat="server">-</SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Market Rate: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLTextBox ID="txtMarketRateV" runat="server" OnChange="AddVariable();" DisplayInputType="Number"
                                                            MaxLength="10"></SAHL:SAHLTextBox>
                                                    </td>
                                                </tr>
                                                <tr>    
                                                    <td>
                                                        Interest Rate: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="txtInterestRateV" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Current Balance: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLCurrencyBox ID="txtCurrentBalanceV" runat="server" ReadOnly="True"></SAHL:SAHLCurrencyBox>
                                                    </td>
                                                </tr>
                                                <tr id="LoanSplitRow" runat="server">
                                                    <td>
                                                        Loan Split Percentage: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="txtLoanSplitV" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Instalment - Capital: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="txtInstallmentCapitalV" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Instalment - Interest: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="txtInstallmentInterestV" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Instalment - Total: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLCurrencyBox ID="txtInstallmentTotalV" runat="server" ReadOnly="True"></SAHL:SAHLCurrencyBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Total Interest: 
                                                    </td>
                                                    <td class="TitleText cellDisplay">
                                                        <SAHL:SAHLLabel ID="txtTotalInterestV" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr id="AmortisatisionRow1" runat="server">
                                                    <td>
                                                        Amortisation Instalment: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="AmortisationInstallmentVariable" runat="server">-</SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlFixedLoan" GroupingText="Fixed Loan" runat="server">
                                            <table id="FixedLoanTable">
                                                <tr>
                                                    <td>
                                                        Remaining Term: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLTextBox ID="txtRemainingTermF" runat="server" ReadOnly="true"></SAHL:SAHLTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Link Rate: 
                                                    </td>
                                                    <td class="cellDisplay" style="height:19px">
                                                        <SAHL:SAHLLabel ID="txtLinkRateF" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="rowDiscountF" visible="false">
                                                    <td>
                                                        Rate Discount: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="lblDiscountF" runat="server">-</SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Market Rate: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLTextBox ID="txtMarketRateF" runat="server" OnChange="AddFixed();" DisplayInputType="Number"
                                                            MaxLength="10"></SAHL:SAHLTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Interest Rate: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="txtInterestRateF" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Current Balance: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLCurrencyBox ID="txtCurrentBalanceF" runat="server" ReadOnly="True"></SAHL:SAHLCurrencyBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Loan Split Percentage: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="txtLoanSplitF" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Instalment - Capital: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="txtInstallmentCapitalF" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Instalment - Interest: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="txtInstallmentInterestF" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Instalment - Total: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLCurrencyBox ID="txtInstallmentTotalF" runat="server" ReadOnly="True"></SAHL:SAHLCurrencyBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Total Interest: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="txtTotalInterestF" runat="server"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr id="AmortisatisionRow2" runat="server">
                                                    <td>
                                                        Amortisation Instalment: 
                                                    </td>
                                                    <td class="cellDisplay">
                                                        <SAHL:SAHLLabel ID="AmortisationInstallmentFixed" runat="server">-</SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="Amortisation" runat="server" Text="Amortisation Schedule" AccessKey="A"
                    ButtonSize="Size6" Enabled="False" OnClick="Amortisation_Click" />
                <SAHL:SAHLButton ID="LastRateChange" runat="server" Text="Current Rate Change" AccessKey="L"
                    ButtonSize="Size6" Enabled="True" OnClick="LastRateChange_Click" />
                <SAHL:SAHLButton ID="Reset" runat="server" Text="Reset" AccessKey="R" CausesValidation="False"
                    UseSubmitBehavior="false" OnClick="Reset_Click" />
                <SAHL:SAHLButton ID="Calculate" runat="server" Text="Calculate" AccessKey="C" OnClick="Calculate_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
