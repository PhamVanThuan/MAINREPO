<%@ Import Namespace="SAHL.Internet.Components.Calculators" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SAHLCalculators.ascx.cs"
    Inherits="SAHL.Internet.Components.Calculators.SAHLCalculator" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<script type="text/javascript">

    function ShowHideErrorForm(displayIndex) {
        if (displayIndex == 1) {
            document.getElementById("errorContainerSpacer1").style.display = "";
            document.getElementById("errorContainerSpacer2").style.display = "";
            document.getElementById("errorContainer").style.display = "";

            if (document.getElementById("resultsSpacer1") != undefined) {
                document.getElementById("resultsSpacer1").style.display = "none";
                document.getElementById("resultsSpacer2").style.display = "none";
                document.getElementById("resultsSpacer3").style.display = "none";
                document.getElementById("resultsSpacer4").style.display = "none";
                document.getElementById("resultsSpacer5").style.display = "none";
                document.getElementById("resultsContainer").style.display = "none";
            }
        }
        else {
            document.getElementById("errorContainerSpacer1").style.display = "none";
            document.getElementById("errorContainerSpacer2").style.display = "none";
            document.getElementById("errorContainer").style.display = "none";
        }
    }


    //enable the button
    function enable(a) {
        document.getElementById(a).style.visibility = "visible";
    }

    //disable the calculator button
    function disable(a) {
        // alert(a);
        document.getElementById(a).style.visibility = "hidden";
    }

    function NumOnly(e) {
        var unicode = e.charCode ? e.charCode : e.keyCode;
        var evtobj = window.event ? event : e
        if (unicode != 8 && unicode != 9) //if the key isn't the backspace key  or tab key (which we should allow)
        {
            if (unicode < 48 || unicode > 57)     //if not a number
                return false;    //disable key press
            else
                return true; // enable keypress
        }
        else
            return true; // enable keypress
    }

</script>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="left">
            <div class="inner-breadcrumb">
                <h4 class="inner-breadcrumb-heading">
                    <asp:Label ID="lblCalculatorName" runat="server" Text="Switch your Home Loan Calculator" /></h4>
                <a id="A1" runat="server" class="inner-breadcrumb-next">FOR ASSISTANCE DIAL 0860 103729</a>
            </div>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Image BorderColor="White" BorderWidth="5px" ID="stepimage" runat="server" AlternateText="Calculate - step one">
            </asp:Image>
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label ID="lblCalculatorDescriptionText" runat="server" Text="Fill in your details to find out what value of Home you can afford to buy. This Switch Loan Calculator will allow you to apply for a Home Loan with SA Home Loans." />
        </td>
    </tr>
    <tr>
        <td class="rowspacer">
        </td>
    </tr>
    <tr>
        <td valign="top">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top">
                        <table border="0" cellpadding="0" cellspacing="3" width="327px">
                            <tr>
                                <td colspan="2">
                                    <h2>
                                        Calculator Details</h2>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="rowspacer">
                                </td>
                            </tr>
                            <tr runat="server" id="rowPurchasePrice">
                                <td valign="top" align="right">
                                    &nbsp;Purchase Price:
                                </td>
                                <td valign="top" align="left" class="fieldError">
                                    <asp:TextBox ID="tbPurchasePrice" runat="server" Width="100px" Text="0"></asp:TextBox>&nbsp;<asp:RangeValidator
                                        runat="server" Type="Integer" MinimumValue="170000" MaximumValue="5000000" ID="cvPurchasePrice"
                                        ControlToValidate="tbPurchasePrice" ErrorMessage="The purchase price must be between R 170,000 & R 5,000,000."
                                        Text="*" Display="Dynamic" CssClass="fieldError" />
                                    <asp:RequiredFieldValidator ID="reqPurchasePrice" runat="server" ControlToValidate="tbPurchasePrice"
                                        Text="*" ErrorMessage="Purchase Price is required." Display="Dynamic" CssClass="fieldError"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="rowPurchasePricespacer" runat="server" class="rowspacer">
                                </td>
                            </tr>
                            <tr id="rowCashDeposit" runat="server">
                                <td valign="top" align="right">
                                    Cash deposit:
                                </td>
                                <td valign="top" align="left" class="fieldError">
                                    <asp:TextBox ID="tbCashDeposit" runat="server" Width="100px">0</asp:TextBox>
                                    <asp:CompareValidator runat="server" Type="Integer" Operator="NotEqual" ControlToCompare="tbPurchasePrice"
                                        ID="cmpCashDeposit" ControlToValidate="tbCashDeposit" ErrorMessage="Deposit cannot be equal the purchase price."
                                        Text="*" Display="Dynamic" CssClass="fieldError"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="rowCashDepositspacer" runat="server" class="rowspacer">
                                </td>
                            </tr>
                            <tr id="rowMarketValue" runat="server">
                                <td valign="top" align="right">
                                    Value of your home:
                                </td>
                                <td valign="top" align="left" class="fieldError">
                                    <asp:TextBox ID="tbMarketValue" runat="server" Width="100px">0</asp:TextBox>&nbsp;<asp:CompareValidator
                                        runat="server" Type="Integer" Operator="GreaterThan" ValueToCompare="170000"
                                        ID="cvMarketValue" ControlToValidate="tbMarketValue" ErrorMessage="The property value must be greater than R 170,000"
                                        Text="*" Display="Dynamic" CssClass="fieldError"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="requireHomeValueValidator" runat="server" ControlToValidate="tbMarketValue"
                                        Text="*" ErrorMessage="Value of your home required." Display="Dynamic" CssClass="fieldError"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="rowMarketValuespacer" runat="server" class="rowspacer">
                                </td>
                            </tr>
                            <tr id="rowCurrentLoan" runat="server">
                                <td valign="top" align="right">
                                    Current loan amount:
                                </td>
                                <td valign="top" align="left" class="fieldError">
                                    <asp:TextBox Visible="false" ID="hiddenTotalLoanAmount" runat="server" />
                                    <asp:TextBox ID="tbCurrentLoan" runat="server" Width="100px">0</asp:TextBox>&nbsp;<asp:RangeValidator
                                        runat="server" Type="Integer" Operator="GreaterThan" ValueToCompare="140000"
                                        ID="cvCurrentLoanAmount" ControlToValidate="hiddenTotalLoanAmount" ErrorMessage="Current Loan Amount"
                                        Text="*" Display="Dynamic" CssClass="fieldError"></asp:RangeValidator>
                                    <asp:CompareValidator runat="server" Type="Integer" Operator="GreaterThan" ValueToCompare="0"
                                        ID="cvLoanAmount" ControlToValidate="tbCurrentLoan" ErrorMessage="You must have a current loan to switch."
                                        Text="*" Display="Dynamic" CssClass="fieldError"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="requiredCurrentLoanAmount" runat="server" ControlToValidate="tbCurrentLoan"
                                        Text="*" ErrorMessage="Current Loan Amount required." Display="Dynamic" CssClass="fieldError"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="rowCurrentLoanSpacer" runat="server" class="rowspacer">
                                </td>
                            </tr>
                            <tr id="rowCashOut" runat="server">
                                <td valign="top" align="right">
                                    Cash out:
                                </td>
                                <td valign="top" align="left" class="fieldError">
                                    <asp:TextBox ID="tbCashOut" runat="server" Width="100px">0</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="rowspacer">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="redline">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="rowspacer">
                                </td>
                            </tr>
                            <tr align="left">
                                <td valign="top" colspan="2">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="top" style="width: 227px; height: 40px;" align="right">
                                                <strong>I am :</strong>
                                            </td>
                                            <td align="left" width="50%" style="height: 40px">
                                                <asp:RadioButton ID="rbSalaried" runat="server" GroupName="employmenttype" Text="Salaried"
                                                    Checked="True" Width="200px" /><br />
                                                <asp:RadioButton ID="rbSelfEmployed" runat="server" GroupName="employmenttype" Text="Self employed"
                                                    Width="200px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="rowspacer">
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right">
                                    Gross monthly income:
                                </td>
                                <td valign="top" align="left" class="fieldError">
                                    <asp:TextBox ID="tbHouseholdIncome" runat="server" Width="100px">0</asp:TextBox>&nbsp;<asp:CompareValidator
                                        runat="server" EnableClientScript="true" Type="Integer" Operator="GreaterThanEqual"
                                        ValueToCompare="6000" ID="cvHouseholdIncome" ControlToValidate="tbHouseholdIncome"
                                        ErrorMessage="Your gross income must exceed R5,000" Text="*" Display="Dynamic"
                                        CssClass="fieldError" />
                                    <asp:RequiredFieldValidator ID="reqHouseholdIncome" runat="server" ControlToValidate="tbHouseholdIncome"
                                        Text="*" ErrorMessage="Gross monthly income is required." Display="Dynamic" CssClass="fieldError"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="rowspacer">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="redline">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="rowspacer">
                                </td>
                            </tr>
                            <tr align="left">
                                <td valign="top" colspan="2">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="top" style="width: 227px; height: 40px;" align="right">
                                                <strong>I'm looking for a:</strong>
                                            </td>
                                            <td align="left" style="height: 40px">
                                                <asp:RadioButton Width="200px" ID="rbProduct3" runat="server" Checked="True" GroupName="products"
                                                    Text="Variable rate loan" /><br />
                                                <asp:RadioButton Width="200px" ID="rbProduct1" runat="server" GroupName="products"
                                                    Text="Fixed rate loan" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="rowspacer" />
                            </tr>
                            <tr>
                                <td valign="top" align="right">
                                    Term of loan (months):
                                </td>
                                <td valign="top" align="left" class="fieldError">
                                    <asp:TextBox ID="tbLoanTerm" runat="server" MaxLength="3" Width="30px" Text="240"></asp:TextBox>&nbsp;<asp:RangeValidator
                                        ControlToValidate="tbLoanTerm" MinimumValue="1" MaximumValue="360" ID="RangeValidator1"
                                        Text="*" runat="server" ErrorMessage="The loan term must be between 1 and 360 months." />
                                    <asp:RequiredFieldValidator ID="reqLoanTerm" runat="server" ControlToValidate="tbLoanTerm"
                                        Text="*" ErrorMessage="Term of Loan is required." Display="Dynamic" CssClass="fieldError"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="rowVarifixSpacer" class="rowspacer">
                                </td>
                            </tr>
                            <tr id="rowVarifix">
                                <td valign="top" align="right">
                                    Percentage to fix:
                                </td>
                                <td valign="top" align="left" class="fieldError">
                                    <asp:TextBox ID="tbFixPercentage" runat="server" MaxLength="3" Width="30px" Text="100"></asp:TextBox>&nbsp;<asp:CustomValidator
                                        ID="CustomValidator2" ControlToValidate="tbFixPercentage" ClientValidationFunction="CalculateFixedPercentageValues"
                                        EnableClientScript="true" Text="*" runat="server" ErrorMessage="The percentage you have fixed is too low. please increase it."></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="reqFixPercentage" runat="server" ControlToValidate="tbFixPercentage" Text="*" ErrorMessage="Percentage to fix is required." Display="Dynamic" CssClass="fieldError"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="rowspacer">
                                </td>
                            </tr>
                            <tr id="rowCheckOptions">
                                <td valign="top" colspan="2" align="right">
                                    <asp:CheckBox ID="chkCapitaliseFees" Visible="true" runat="server" Checked="True" /><asp:Label
                                        ID="lblCapitaliseFees" runat="server" Text="Capitalise fees?" />&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkInterestOnly" Visible="true" runat="server" /><asp:Label ID="lblInterestOnly"
                                        runat="server" Text="Interest only?" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="rowspacer">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="redline">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="rowspacer">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:ImageButton runat="server" ID="calcButton" ImageUrl="~/images/SAHomeLoans/Buttons/What-Can-I-Afford.jpg"
                                        OnCommand="CalculateAffordabilityCommand" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <%--ERROR HANLDER STARTS--%>
                    <td class="columnspacer" id="errorContainerSpacer1" style="display: none;">
                    </td>
                    <td class="columnspacer" id="errorContainerSpacer2" style="display: none;">
                    </td>
                    <td valign="top" align="right" id="errorContainer" style="display: none;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 346px; height: 100%;">
                            <tr>
                                <td class="rowspacer">
                                </td>
                            </tr>
                            <tr>
                                <td class="rowspacer">
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="center">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td class="errorContainerTop">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="errorContainerContent" align="left" valign="top">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td class="errorTitle">
                                                            Qualifying Notes:
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:ValidationSummary runat="server" ID="summary1" ShowSummary="true" CssClass="errorText"
                                                                ShowMessageBox="false" DisplayMode="BulletList" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="errorContainerBottom">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <%--ERROR HANLDER ENDS--%>
                    <asp:Panel runat="server" ID="pnlResults" Visible="false">
                        <td id="resultsSpacer1" class="columnspacer">
                        </td>
                        <td id="resultsSpacer2" class="columnspacer">
                        </td>
                        <td id="resultsSpacer3" class="dottedcolumn">
                        </td>
                        <td id="resultsSpacer4" class="columnspacer">
                        </td>
                        <td id="resultsSpacer5" class="columnspacer">
                        </td>
                        <td id="resultsContainer" valign="top">
                            <table id="Table4" cellspacing="0" cellpadding="3" border="0" width="100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblNotQualifyMsg" Visible="true" runat="server" CssClass="orangetext">Warning Message Here.</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblQualifyMsg" runat="server" Visible="false" Font-Names="Verdana"
                                            Font-Size="14pt" ForeColor="Black">The application provisionally qualifies</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h2>
                                            Breakdown of the Loan Requirement</h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="rowspacer">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        - Loan to Value (LTV) ratio is <strong class="orangetext">
                                            <asp:Label ID="lblLTV" runat="server"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr runat="server" id="SuperLoPTI">
                                    <td>
                                        - Payment to Income (PTI) ratio is <strong class="orangetext">
                                            <asp:Label ID="lblPTI" runat="server"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr runat="server" id="VarifixPTI">
                                    <td>
                                        - <strong class="orangetext">
                                            <asp:Label ID="lblFixPercent" runat="server">1sdfds</asp:Label></strong> fixed
                                        has been specified, therefore the Payment to Income (PTI) ratio is <strong class="orangetext">
                                            <asp:Label ID="lblFixPTI" runat="server"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr runat="server" id="VarifixRedline1">
                                    <td class="rowspacer">
                                    </td>
                                </tr>
                                <tr runat="server" id="VarifixRedline2">
                                    <td class="redline">
                                    </td>
                                </tr>
                                <tr runat="server" id="VarifixRedline3">
                                    <td class="rowspacer">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table cellspacing="0" cellpadding="3" width="100%" border="0">
                                            <tr>
                                                <td valign="top">
                                                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                                                        <tr runat="server" id="Tr1">
                                                            <td colspan="3">
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="Tr2">
                                                            <td align="left">
                                                                Total Loan Requirement:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblSAHLTotLoan" runat="server" EnableViewState="False"></asp:Label>
                                                                <asp:Label ID="lblFeeInfoInd" runat="server" EnableViewState="False" Font-Size="XX-Small"></asp:Label>
                                                            </td>
                                                            <td align="left" runat="server" id="tdLoanrequirement" visible="false">
                                                                <asp:Label ID="lblOtherBankLoanRequirement" runat="server" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="Tr3">
                                                            <td align="left">
                                                                Interest Rate:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblSAHLIntRate" runat="server" EnableViewState="False"></asp:Label>
                                                            </td>
                                                            <td align="left" runat="server" id="tdInterestRate" visible="false">
                                                                <asp:Label ID="lblOtherBankInterestRate" runat="server" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="Tr4">
                                                            <td align="left">
                                                                Monthly Instalment:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblSAHLMonthlyInst" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left" runat="server" id="tdMonthlyInstalment" visible="false">
                                                                <asp:Label ID="lblOtherBankMonthlyInstalment" runat="server" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="Tr5">
                                                            <td align="left">
                                                                Interest Paid Over Term:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblSAHLIntOverTerm" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left" runat="server" id="tdTermInterest" visible="false">
                                                                <asp:Label ID="lblOtherBankTerm" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="Tr6">
                                                            <td colspan="3" align="right">
                                                                <h5>
                                                                </h5>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="SuperLoRedline1">
                                                <td class="rowspacer">
                                                </td>
                                            </tr>
                                            <tr runat="server" id="SuperLoRedline2">
                                                <td class="redline">
                                                </td>
                                            </tr>
                                            <tr runat="server" id="SuperLoRedline3">
                                                <td class="rowspacer">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <table cellspacing="0" cellpadding="2" width="100%" border="0">
                                                        <tr runat="server" id="TrVF1">
                                                            <td>
                                                            </td>
                                                            <td align="left">
                                                                Fixed Portion
                                                            </td>
                                                            <td align="left">
                                                                Variable Portion
                                                            </td>
                                                            <td align="left">
                                                                Total
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="Tr7">
                                                            <td class="redline" colspan="4">
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="TrVF2">
                                                            <td align="left">
                                                                Loan Split:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblFixedPercent" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblVariablePercent" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="SAHLLabel9" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="TrVF3">
                                                            <td align="left">
                                                                Total Loan:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblFixLoanAmount" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblVarLoanAmount" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblTotalFixLoan" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="TrVF4">
                                                            <td align="left">
                                                                Interest Rate:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblFixRate" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblVarRate" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="TrVF5">
                                                            <td align="left">
                                                                Monthly Instalment:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblFixMonthlyInst" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblVarMonthlyInst" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblTotFixMonthlyInst" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="TrVF6">
                                                            <td align="left">
                                                                Interest Over Term:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblIntPaidTermFix" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblIntPaidTermVar" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblTotFixIntPaidTerm" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="4">
                                                                <h5>
                                                                    <asp:Label ID="lblFeeInfoFix" runat="server" Font-Italic="True">Feel Info Fix 8890</asp:Label></h5>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowspacer">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="redline">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowspacer">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <h2>
                                                                    Fees</h2>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Cancellation Fee:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblCancellationFee" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Registration Fee:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblRegFee" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Initiation Fee:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblBondPrepFee" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Total Fees:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblTotalFees" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Interim Interest Provision:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblInterimIntProv" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible="false" id="TRSpec1">
                                                            <td valign="top" align="left">
                                                                <h2>
                                                                    You specified the following details</h2>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible="false" id="TRSpec2">
                                                            <td align="left">
                                                                Purchase Price:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblPurchasePrice" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible="false" id="TRSpec3">
                                                            <td align="left">
                                                                Cash Deposit:
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblCashDeposit" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                                                        <tr>
                                                            <td colspan="2" align="right" valign="bottom">
                                                                <asp:ImageButton ImageUrl="~/images/SAHomeLoans/Buttons/Calc-Apply-Button.jpg" runat="server"
                                                                    OnCommand="ApplyCommand" ID="btnApply"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </asp:Panel>
                </tr>
            </table>
        </td>
    </tr>
</table>
