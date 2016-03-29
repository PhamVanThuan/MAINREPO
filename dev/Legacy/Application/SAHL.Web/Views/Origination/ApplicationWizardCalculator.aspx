<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="ApplicationWizardCalculator.aspx.cs" Inherits="SAHL.Web.Views.Origination.ApplicationWizardCalculator" Title="Untitled Page" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<script language="javascript" type="text/javascript">
function setupLoanPurpose(resetValues)
{
    //debugger;
    var ddlPurpose = document.getElementById('<%=ddlPurpose.ClientID%>');
    var purposeType = ddlPurpose.value;


    var rMarketValue = document.getElementById('<%=rowMarketValue.ClientID%>');
    var rCurrentLoan = document.getElementById('<%=rowCurrentLoan.ClientID%>');
    var rCashOut = document.getElementById('<%=rowCashOut.ClientID%>');
    var rCapitaliseFees = document.getElementById('<%=rowCapitaliseFees.ClientID%>');
    var rPurchasePrice = document.getElementById('<%=rowPurchasePrice.ClientID%>');
    var rCashDeposit = document.getElementById('<%=rowCashDeposit.ClientID%>');
    var rCashRequired = document.getElementById('<%=rowCashRequired.ClientID%>');
    var ddlNeedsIdentification = document.getElementById('<%=ddlNeedsIdentification.ClientID%>');
    
    rMarketValue.style.display = "none";
    rCurrentLoan.style.display = "none";
    rCashOut.style.display = "none";
    rCapitaliseFees.style.display = "none";
    rPurchasePrice.style.display = "none";
    rCashDeposit.style.display = "none";
    rCashRequired.style.display = "none";   

  
    switch(purposeType)
    {
        case "2": //Switch loan
            rMarketValue.style.display = "inline";
            rCurrentLoan.style.display = "inline";
            rCashOut.style.display = "inline";
            if(ddlNeedsIdentification.value != "-select-")
            {
                rCapitaliseFees.style.display = "inline";
                //chkCapitaliseFees.checked = true;
            }
            break;
        case "3": //New purchase
            rPurchasePrice.style.display = "inline";   
            rCashDeposit.style.display = "inline";
            rCapitaliseFees.style.display = "none";
            break;
        case "4": //Refinance
            rMarketValue.style.display = "inline";
            if(ddlNeedsIdentification.value != "-select-")
            {
                rCapitaliseFees.style.display = "inline";
            }
             rCashRequired.style.display = "inline";           
           break;
        default:
            break;
    }
    
    SetCreateApplication(false, true);
}

function SetupProductDisplay()
{
  var rProduct = document.getElementById('<%=rowProduct.ClientID%>');
  var rCapitaliseFees = document.getElementById('<%=rowCapitaliseFees.ClientID%>');
  var ddlNeedsIdentification = document.getElementById('<%=ddlNeedsIdentification.ClientID%>');
  var ddlProduct = document.getElementById('<%=ddlProduct.ClientID%>');
    
  var needsSelected = ddlNeedsIdentification.value;              
   
  if(needsSelected == "-select-")
  {  
     ddlProduct.value = "-select-";   
     rProduct.style.display = "none";      
     rCapitaliseFees.style.display = "none"; 
  }
  else
  {   
     rProduct.style.display = "inline";    
     setupLoanPurpose(false);
     setupProduct(false);       
  }    
    
}

function setupProduct(resetTerm)
{
    var ddlProduct = document.getElementById('<%=ddlProduct.ClientID%>');
    var ddlNeedsIdentification = document.getElementById('<%=ddlNeedsIdentification.ClientID%>');
    var productType = ddlProduct.value;
    
    var rowVarifix = document.getElementById('<%=rowVarifix.ClientID%>');
    var rowVariFixReset = document.getElementById('<%=rowVariFixReset.ClientID%>');
    var rCapitaliseFees = document.getElementById('<%=rowCapitaliseFees.ClientID%>')
    var rCashRequired = document.getElementById('<%=rowCashRequired.ClientID%>');
    var tbLoanTerm = document.getElementById('<%=tbLoanTerm.ClientID%>');

    switch(productType)
    {
        case "2": //VariFix
            rowVarifix.style.display = "inline";
            rowVariFixReset.style.display = "inline";
            if (resetTerm)
                tbLoanTerm.value = 240;
            tbLoanTerm.disabled = false;
            break;
        case "5": //SuperLo  
        case "9": //NewVariable             
             rowVarifix.style.display = "none";  
             rowVariFixReset.style.display = "none";
             if (resetTerm)
                tbLoanTerm.value = 240;
             tbLoanTerm.disabled = false;    
            break;
        case "11": //Edge
            rowVarifix.style.display = "none";
            rowVariFixReset.style.display = "none";
            tbLoanTerm.value = '<% =EdgeTerm %>';
            tbLoanTerm.disabled = true;
            break;
        default:
            break;
    }
    
    SetCreateApplication(false, true);
}
function updateVal(control)
{   
    var tbCashOut = document.getElementById('<%=tbCashOut.ClientID%>');
    var tbCashRequired = document.getElementById('<%=tbCashRequired.ClientID%>');
    var tbPurchasePrice = document.getElementById('<%=tbPurchasePrice.ClientID%>');
    var tbMarketValue = document.getElementById('<%=tbMarketValue.ClientID%>');

    var val = SAHLCurrencyBox_getValue(control.id);//valArr[0].concat(".", valDec).replace(/,/g, "").replace(/_/g, "");
    
    SetCreateApplication(false, true);
}

function SetCreateApplication(enabled, calcReset)
{   
    if (calcReset)
    {
        var tbValidCalc = document.getElementById('<%=tbValidCalc.ClientID%>');
        tbValidCalc.value = false.toString();
    }
    
    SAHLButton_setEnabled('<%=btnCalculate.ClientID%>', true);
    SAHLButton_setEnabled('<%=btnCreateApplication.ClientID%>', enabled);
}

function checkValMax(control, max)
{
    var val = Number(control.value);
    if (isNaN(val) || val > max)
    {
        alert('Maximum allowed value is ' + max.toString());
        control.value = max;
    }
}

function SetResetPeriod(chkReset)
{
    //For VF one or the other must be checked
    var chkVFReset6Month = document.getElementById('<%=chkVFReset6Month.ClientID%>');
    var chkVFReset5Year = document.getElementById('<%=chkVFReset5Year.ClientID%>');
    
    if (chkReset == chkVFReset6Month)
        chkVFReset5Year.checked = !chkReset.checked;
    else
        chkVFReset6Month.checked = !chkReset.checked;
}

function btnNextEnabled()
{
 
    
    var validLE = true;
 
    SetCreateApplication(validLE, false);
}

</script>

    <table style="width: 98%" class="tableStandard">
        <tr valign="top" style="height: 450px">
            <td style="width: 380px; vertical-align:top">
            
                <asp:Panel runat="server" ID="pnlLegalEntity" GroupingText="Applicant">
                <table>
                    <tr>
                        <td colspan="4" style="height: 5px"></td>
                    </tr>
                    <tr>
                        <td align="right">Marketing Source: </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblMarketingSource" runat="server" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                        <td style="width:10px;">&nbsp;</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right">Identity Number: </td>
                        <td>
                            &nbsp;<SAHL:SAHLLabel ID="lblIdentityNo" runat="server" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                        <td style="width:10px;">&nbsp;</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right">First Names: </td>
                        <td>
                            &nbsp;<SAHL:SAHLLabel ID="lblFirstNames" runat="server" CssClass="LabelText" TextAlign="Left" />
                        </td>
                        <td style="width:10px;">&nbsp;</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right">Surname: </td>
                        <td>
                            &nbsp;<SAHL:SAHLLabel ID="lblSurname" runat="server" CssClass="LabelText" TextAlign="Left" />
                        </td>
                        <td style="width:10px;">&nbsp;</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right">Contact Number: </td>
                        <td>
                            &nbsp;<SAHL:SAHLLabel runat="server" ID="lblContact" CssClass="LabelText" TextAlign="Left" />
                        </td>
                        <td style="width:10px;">&nbsp;</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right">Number of Applicants: </td>
                        <td><SAHL:SAHLLabel runat="server" ID="lblNumApplicants" CssClass="LabelText" TextAlign="Left" /></td>
                        <td style="width:10px;">&nbsp;</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right">Estate Agent Deal: </td>
                        <td>
                            &nbsp;<SAHL:SAHLCheckbox runat="server" ID="chkEstateAgentDeal" Enabled="false" />
                        </td>
                        <td style="width:10px;">&nbsp;</td>
                        <td></td>
                    </tr>
                </table>
                </asp:Panel>
            
                <asp:Panel ID="pnlInput" runat="server" GroupingText="Calculator" Width="380px">
                    <table>
                        <tr>
                            <td align="right">Loan Purpose:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td>
                                <SAHL:SAHLDropDownList ID="ddlPurpose" runat="server" OnChange="setupLoanPurpose(true);" CssClass="mandatory">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td></td>
                        </tr>
                        <tr id="rowPurchasePrice" runat="server">
                            <td align="right">Purchase Price:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td><SAHL:SAHLCurrencyBox ID="tbPurchasePrice" runat="server" Width="80px" CssClass="mandatory" /></td>
                            <td>
                                <img alt="" src="../../Images/help.gif" title="The sellers price for the home i.e. Excluding any costs of purchase." />
                            </td>
                        </tr>
                        <tr id="rowCashDeposit" runat="server">
                            <td align="right">Cash Deposit:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td>
                                <SAHL:SAHLCurrencyBox ID="tbCashDeposit" runat="server" Width="80px" OnChange="SetCreateApplication(false, true);" ></SAHL:SAHLCurrencyBox>
                            </td>
                            <td>
                                <img alt="" src="../../Images/help.gif" title="How much you will be able to deposit towards the purchase of your new home." />&nbsp;
                            </td>
                        </tr>
                        <tr id="rowMarketValue" runat="server">
                            <td align="right">Market Value of your home:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td><SAHL:SAHLCurrencyBox ID="tbMarketValue" runat="server" Width="80px" CssClass="mandatory" /></td>
                            <td>
                                <img alt="" src="../../Images/help.gif" title="The amount which you believe that your property would sell for on the open market." />&nbsp;
                            </td>
                        </tr>
                        <tr id="rowCurrentLoan" runat="server">
                            <td align="right">Current Loan Amount:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td><SAHL:SAHLCurrencyBox ID="tbCurrentLoan" runat="server" CssClass="mandatory" Width="80px" OnChange="SetCreateApplication(false, true);"></SAHL:SAHLCurrencyBox></td>
                            <td>
                                <img alt="" src="../../Images/help.gif" title="The amount you need to repay on your existing mortgage loan account with your current bank." />&nbsp;
                            </td>
                        </tr>
                        <tr id="rowCashOut" runat="server">
                            <td align="right">Cash Out:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td><SAHL:SAHLCurrencyBox ID="tbCashOut" runat="server" CssClass="mandatory" Width="80px" ></SAHL:SAHLCurrencyBox></td>
                            <td>
                                <img alt="" src="../../Images/help.gif" title="An additional amount you wish to borrow i.e. to take extra cash out of your home loan." />&nbsp;
                            </td>
                        </tr>
                        <tr id="rowCashRequired" runat="server">
                            <td align="right">Cash Required:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td>
                                <SAHL:SAHLCurrencyBox ID="tbCashRequired" runat="server" CssClass="mandatory" Width="80px" ></SAHL:SAHLCurrencyBox>
                            </td>
                            <td>
                                <img alt="" src="../../Images/help.gif" title="The amount you wish to borrow." />&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Employment Type:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td><SAHL:SAHLDropDownList ID="ddlEmploymentType" runat="server" CssClass="mandatory"  OnChange="SetCreateApplication(false, true);"></SAHL:SAHLDropDownList></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="right">Term of loan:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td><SAHL:SAHLTextBox ID="tbLoanTerm" runat="server" MaxLength="3" Width="30px" CssClass="mandatory" OnChange="SetCreateApplication(false, true);" DisplayInputType="Number" >240</SAHL:SAHLTextBox></td>
                            <td>
                                <img alt="" src="../../Images/help.gif" title="How many months you will take to pay back this loan (240 months = 20 years)" />&nbsp;
                            </td>
                        </tr>
                        <tr id="rowVarifix" runat="server">
                            <td align="right">Percentage to Fix:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td><SAHL:SAHLTextBox ID="tbFixPercentage" runat="server" MaxLength="3" Width="30px" CssClass="mandatory" OnChange="SetCreateApplication(false, true); checkValMax(this, 100);" DisplayInputType="Number" >100</SAHL:SAHLTextBox></td>
                            <td>
                                <img alt="" src="../../Images/help.gif" title="What percentage portion of the loan would they like to fix?" />&nbsp;
                            </td>
                        </tr>
                        <tr id="rowVariFixReset" runat="server">
                            <td align="right">Reset period:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td>
                            <asp:CheckBox ID="chkVFReset5Year" runat="server" OnClick="SetCreateApplication(false, true);SetResetPeriod(this);" Checked="true" Text="5 yrs" Enabled="false"/>
                            &nbsp;
                            <asp:CheckBox ID="chkVFReset6Month" runat="server" OnClick="SetCreateApplication(false, true);SetResetPeriod(this);" Text="6 mnths" Visible="false"/>
                            </td>
                            <td>
                                <img alt="" src="../../Images/help.gif" title="What percentage portion of the loan would they like to fix?" />&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Gross Monthly Household Income:</td>
                            <td style="width:10px;">&nbsp;</td>
                            <td><SAHL:SAHLCurrencyBox ID="tbHouseholdIncome" runat="server" CssClass="mandatory" Width="80px" OnChange="SetCreateApplication(false, true);"></SAHL:SAHLCurrencyBox></td>
                            <td>
                                <SAHL:SAHLTextBox runat="server" ID="tbCreditMatrixKey" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbCategoryKey" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbActiveMarketRate" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbLinkRate" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbMarginKey" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbTotalFees" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbCancellationFee" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbInitiationFee" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbRegistrationFee" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbInterimInterest" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbLTV" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbPTI" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbInstalmentTotal" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbInstalmentFix" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbValidCalc" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbLegalEntityKey" style="display:none" />
                                <SAHL:SAHLTextBox runat="server" ID="tbIsWizardMode" style="display:none" />
                                <img alt="" src="../../Images/help.gif" title="This is the total amount of income your household receives before taxation." />&nbsp;
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="right" style="height: 1px">
                                Needs Identification:</td>
                            <td style="height: 1px"></td>
                            <td style="height: 1px"><SAHL:SAHLDropDownList ID="ddlNeedsIdentification" runat="server" OnChange="SetupProductDisplay();" CssClass="mandatory" Width="191px">
                            </SAHL:SAHLDropDownList></td>
                            <td style="height: 1px"></td>
                        </tr>
                        <tr id="rowProduct" runat="server">
                            <td align="right">
                                Product:</td>
                            <td>
                            </td>
                            <td>
                                <SAHL:SAHLDropDownList ID="ddlProduct" runat="server" OnChange="setupProduct(true);" CssClass="mandatory" Width="140px"/></td>
                                 <td></td>
                        </tr>
                        <tr id="rowCapitaliseFees" runat="server">
                            <td align="right">
                                Capitalise Fees:</td>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkCapitaliseFees" runat="server" OnChange="SetCreateApplication(false,true);" Checked="True" /></td>
                            <td><img alt="" src="../../Images/help.gif" title="Ticking this will include the loan's fees in the total loan amount." /></td>                                
                        </tr>
                        <%--<tr id="rowInterestOnly" runat="server">
                            <td align="right">
                                Interest Only:</td>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkInterestOnly" runat="server" OnChange="SetCreateApplication(false, true);" /></td>
                            <td><img alt="" src="../../Images/help.gif" title="Ticking this will display monthly instalments for interest-only loans." /></td>
                        </tr>--%>
                    </table>
                </asp:Panel>
                
            </td>
            <td>
                <asp:Panel ID="pnlResults" runat="server" GroupingText="Results" Visible="false">
                    <table id="ResultsTab" class="Calculator" cellspacing="0" cellpadding="3" align="center" border="0">
                        <tbody>
                            <tr>
                                <td align="center">
                                        <SAHL:SAHLLabel ID="lblNotQualifyMsg" visible="false" runat="server" style="font-weight: bold" ForeColor="Orange">WARNING!!! The income specified is insufficient. This is a warning only</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="formheading" align="center" style="height: 11px">
                                    <SAHL:SAHLLabel ID="lblQualifyMsg" runat="server" CssClass="headline2" EnableViewState="False" Visible="false"
                                        Font-Names="Verdana" Font-Size="14pt" ForeColor="ForestGreen">The application provisionally qualifies</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="formheading" align="center">
                                    <p>
                                        Following is a detailed breakdown of the loan requirement.</p>
                                </td>
                            </tr>
                            <tr>
                                <td class="fontSmallerEx" align="center">
                                    Loan to Value(LTV) ratio is
                                    <SAHL:SAHLLabel ID="lblLTV" runat="server"></SAHL:SAHLLabel>
                                    <img alt="" title="Your loan amount expressed as a percentage of your property value."
                                        src="../../Images/help.gif" /></td>
                            </tr>
                            <tr id="rowPTIStandard" runat="server">
                                <td class="fontSmallerEx" align="center">
                                    Payment to Income(PTI) ratio is
                                    <SAHL:SAHLLabel ID="lblPTI" runat="server"></SAHL:SAHLLabel>
                                    <img alt="" title="Your loan instalment expressed as a percentage of your monthly household income."
                                        src="../../Images/help.gif" /></td>
                            </tr>
                            <tr id="rowPTIStdForFix" runat="server">
                                <td class="fontSmallerEx" align="center">
                                    Without fixing a portion of the loan the Payment to Income(PTI) ratio is
                                    <SAHL:SAHLLabel ID="lblVarPTI" runat="server"></SAHL:SAHLLabel>
                                    <img alt="" title="Your loan instalment expressed as a percentage of you monthly household income."
                                        src="../../Images/help.gif" /></td>
                            </tr>
                            <tr id="rowPTIFix" runat="server">
                                <td class="fontSmallerEx" align="center">
                                    <SAHL:SAHLLabel ID="lblFixPercent" runat="server"></SAHL:SAHLLabel> fixed have been specified,
                                    therefore the Payment to Income (PTI) ratio is
                                    <SAHL:SAHLLabel ID="lblFixPTI" runat="server"></SAHL:SAHLLabel>
                                    <img alt="" title="Your loan instalment, calculated based on your fixed election, expressed as a percentage of you monthly household income."
                                        src="../../Images/help.gif" /></td>
                            </tr>
                            <tr id="rowExtendedResults" runat="server">
                                <td class="fontSmallerEx" align="center">
                                    <table id="tblStandard" cellspacing="0" cellpadding="3" width="450" align="center" border="0" runat="server">
                                        <tbody>
                                            <tr>
                                                <td id="TD1" class="fontSmallerEx" valign="middle" align="right" width="50%" bgcolor="#ffbb00">
                                                </td>
                                                <td class="FormHeading" valign="middle" bgcolor="#ffbb00">
                                                    <SAHL:SAHLLabel ID="lblColumnHeader1" runat="server">SA Home Loans</SAHL:SAHLLabel>
                                                </td>
                                                <td bgcolor="#ffbb00" class="FormHeading" valign="middle" runat="server" id="tdInterestOnlyColumnHeader">
                                                    <SAHL:SAHLLabel ID="lblColumnHeader2" runat="server">Interest Only</SAHL:SAHLLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="right" width="225">
                                                    Total Loan Requirement:</td>
                                                <td valign="top">
                                                    <SAHL:SAHLLabel ID="lblSAHLTotLoan" runat="server" EnableViewState="False"></SAHL:SAHLLabel>
                                                    <SAHL:SAHLLabel ID="lblFeeInfoInd" runat="server" EnableViewState="False"
                                                        Font-Size="XX-Small"></SAHL:SAHLLabel></td>
                                                <td valign="top">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="right">
                                                    Interest Rate:</td>
                                                <td valign="middle">
                                                    <SAHL:SAHLLabel ID="lblSAHLIntRate" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td valign="top">
                                                </td>
                                            </tr>
                                            <tr runat="server" id="rowVarInstal">
                                                <td valign="middle" align="right">
                                                    Monthly Instalment:</td>
                                                <td valign="middle">
                                                    <SAHL:SAHLLabel ID="lblSAHLMonthlyInst" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td valign="middle">
                                                    <SAHL:SAHLLabel ID="lblIOSAHLMonthlyInst" runat="server" EnableViewState="False"></SAHL:SAHLLabel>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="rowEHLIOInstal">
                                                <td valign="middle" align="right">
                                                    Initial (IO) Instalment:</td>
                                                <td valign="middle">
                                                    <SAHL:SAHLLabel ID="lblIOEHLInst" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td valign="middle">
                                                    
                                                </td>
                                            </tr>
                                            <tr runat="server" id="rowEHLAMInstal">
                                                <td valign="middle" align="right">
                                                    Monthly AM Instalment:</td>
                                                <td valign="middle">
                                                    <SAHL:SAHLLabel ID="lblAMEHLInst" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td valign="middle">
                                                    
                                                </td>
                                            </tr>
                                            <tr runat="server" id="rowEHLAMInstalFull">
                                                <td valign="middle" align="right">
                                                    23yr Amortising Instalment:</td>
                                                <td valign="middle">
                                                    <SAHL:SAHLLabel ID="lblAMEHLInstFull" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td valign="middle">
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="right" style="height: 14px">
                                                    Interest Paid Over Term:</td>
                                                <td valign="middle" style="height: 14px">
                                                    <SAHL:SAHLLabel ID="lblSAHLIntOverTerm" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td valign="middle">
                                                    <SAHL:SAHLLabel ID="lblIOSAHLIntOverTerm" runat="server" EnableViewState="False"></SAHL:SAHLLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="center" colspan="4">
                                                    <SAHL:SAHLLabel ID="lblFeeInfo" runat="server" EnableViewState="False"
                                                        Font-Italic="True"></SAHL:SAHLLabel></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <table id="tblVariFix" cellspacing="0" cellpadding="3" width="450" align="center" border="0" runat="server">
                                        <tbody>
                                            <tr>
                                                <td style="width: 170px" id="Td2" class="fontSmallerEx" valign="middle" align="right"
                                                    width="170" bgcolor="#ffbb00">
                                                </td>
                                                <td style="width: 147px" class="FormHeading" valign="middle" nowrap="nowrap" bgcolor="#ffbb00">
                                                    Fixed Portion</td>
                                                <td style="width: 147px" class="FormHeading" valign="middle" nowrap="nowrap" bgcolor="#ffbb00">
                                                    Variable Portion</td>
                                                <td class="FormHeading" valign="middle" bgcolor="#ffbb00">
                                                    Total</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 170px" valign="middle" nowrap="nowrap" align="right"
                                                    width="170">
                                                    Loan Split:</td>
                                                <td style="width: 147px" valign="top" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblFixedPercent" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td style="width: 147px" valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblVariablePercent" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td valign="middle" nowrap="nowrap">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 170px" valign="middle" nowrap="nowrap" align="right"
                                                    width="170">
                                                    Total Loan Requirement:</td>
                                                <td style="width: 147px" valign="top" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblFixLoanAmount" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td style="width: 147px" valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblVarLoanAmount" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblTotalFixLoan" runat="server" EnableViewState="False"></SAHL:SAHLLabel>
                                                    <SAHL:SAHLLabel ID="lblFeeInfoIndFix" runat="server" EnableViewState="False"
                                                        Font-Size="XX-Small"></SAHL:SAHLLabel></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 170px" valign="middle" nowrap="nowrap" align="right">
                                                    Interest Rate:</td>
                                                <td style="width: 147px" valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblFixRate" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td style="width: 147px" valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblVarRate" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td valign="middle" nowrap="nowrap">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 170px" valign="middle" nowrap="nowrap" align="right">
                                                    Monthly Instalment:</td>
                                                <td style="width: 147px" valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblFixMonthlyInst" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td style="width: 147px" valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblVarMonthlyInst" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                                <td valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblTotFixMonthlyInst" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 170px" valign="middle" nowrap="nowrap" align="right">
                                                    Interest Paid Over Term:</td>
                                                <td style="width: 147px" valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblIntPaidTermFix" runat="server" EnableViewState="False"></SAHL:SAHLLabel>
                                                </td>
                                                <td style="width: 147px" valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblIntPaidTermVar" runat="server" EnableViewState="False"></SAHL:SAHLLabel>
                                                </td>
                                                <td valign="middle" nowrap="nowrap">
                                                    <SAHL:SAHLLabel ID="lblTotFixIntPaidTerm" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                            </tr>
                                            <tr>
                                                <td class="fontSmallerEx" valign="middle" align="center" colspan="4">
                                                    <SAHL:SAHLLabel ID="lblFeeInfoFix" runat="server" EnableViewState="False"
                                                        Font-Italic="True"></SAHL:SAHLLabel></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <table id="Table3" cellspacing="0" cellpadding="3" width="450"
                                        border="0">
                                        <tbody>
                                            <tr>
                                                <td class="FormHeading" valign="middle" align="center" bgcolor="#ffbb00" colspan="2">
                                                    Fees</td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="right" width="50%">
                                                    Cancellation Fee:</td>
                                                <td valign="middle" align="left">
                                                    <SAHL:SAHLLabel ID="lblCancellationFee" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="right" width="225">
                                                    Registration Fee:</td>
                                                <td valign="middle" align="left">
                                                    <SAHL:SAHLLabel ID="lblRegFee" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="right" width="225">
                                                    Initiation Fee:</td>
                                                <td valign="middle" align="left">
                                                    <SAHL:SAHLLabel ID="lblBondPrepFee" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="right" width="225">
                                                    Total Fees:</td>
                                                <td valign="middle" align="left">
                                                    <SAHL:SAHLLabel ID="lblTotalFees" runat="server" EnableViewState="False"></SAHL:SAHLLabel></td>
                                            </tr>
                                            <tr id="rowInterimInterest" runat="server">
                                                <td valign="middle" align="right" width="225">
                                                    Interim Interest Provision:</td>
                                                <td align="left">
                                                    <SAHL:SAHLLabel ID="lblInterimIntProv" runat="server" EnableViewState="False"></SAHL:SAHLLabel>
                                                    <img alt="" title="Provision for interest on your existing bank loan, over the 3 month notice period - which needs to be included in your bank settlement amount."
                                                        src="../../Images/help.gif" align="middle" /></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        
        <tr>
        <td >
            <SAHL:SAHLButton ID="btnBack" runat="server" ButtonSize="Size4" CausesValidation="false"
                CssClass="BtnNormal4" OnClick="btnBack_Click" Text="Back" Visible="False" /></td>
            <td colspan="2" align="right">
                &nbsp;<SAHL:SAHLButton ID="btnCalculate" runat="server" Text="Calculate" ButtonSize="Size4" CssClass="BtnNormal4" CausesValidation="false" OnClick="OnCalculate_Click"/>&nbsp;
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" ButtonSize="Size4" CssClass="BtnNormal4" CausesValidation="false" OnClick="OnCancel_Click" />&nbsp;
                <SAHL:SAHLButton ID="btnCreateApplication" runat="server" Text="Create Application" ButtonSize="Size4" CssClass="BtnNormal4" CausesValidation="false" OnClick="OnCreateApplication" Enabled="false"/>&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
