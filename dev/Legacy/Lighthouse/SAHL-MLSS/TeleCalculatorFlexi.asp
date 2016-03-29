<%@ Language=VBScript %>

<%

Option Explicit

'//----------------------------------------------------------------------------------------------------------------------------------
'//      Database
'//----------------------------------------------------------------------------------------------------------------------------------

' Variable Declarations

dim objConn
dim iSPVNumber
dim sSPVNumber

If Request.QueryString("Source") = "eWorkFlexi" Then

    Session("SQLDatabase") = DB_SERVER
    Session("UserID") = DB_EWORK_USERID
    Session("DSN") = "eWorks_launch"

    if Request.QueryString("LinkRate") = "0.0275" or Request.QueryString("LinkRate") = "0.031" then
		iSPVNumber = 17
        sSPVNumber = "16,17,18"
    else
		iSPVNumber = 16
        sSPVNumber = "16,17,18"
    end if
else
    iSPVNumber = 16
    sSPVNumber = 16
End If

'------------------------------------------------------------------------------
sub getConnection()

'// Creates connection to the database
'// objConn

    dim sDSN
    ' Connection string
    sDSN = "Provider=SQLOLEDB.1; Application Name='MLS System Version1 [TeleCalculatorFlexi.asp_ss]';Data Source=" & Session("SQLDatabase") & ";uid=" & Session("UserID")

    Set objConn = Server.CreateObject("ADODB.Connection")

    objConn.Open sDSN

end sub

'------------------------------------------------------------------------------------------------------------------------------

sub populateDDL(sSQL)

'// Populates drop down list.
'// first field in recordset will be the option value and second field will be the text
'// The select tags must be added in the html code
'// EG: Call populateDDL("Select PurposeNumber, PurposeDescription From Purpose (nolock)")

    Dim rs
    dim iWidth

    call getConnection

    iWidth = 150
    set rs = Server.CreateObject("ADODB.Recordset")

    rs.CursorLocation=3

    rs.Open sSQL, objConn,3

    if not rs.EOF then

        while not rs.EOF
            Response.Write("<option value=" & rs.Fields(0).Value & ">" & rs.Fields(1).Value & "</option>")
            rs.MoveNext
        wend

    end if

    rs.Close
    objConn.Close

    set objConn = nothing
    set rs = nothing

end sub

%>

<HTML>
<HEAD>
<!--#include file="server.asp"-->

<title>Flexi-Fix Proportion Calculator</title>
<META name="VI60_DefaultClientScript" Content="VBScript">

<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css"/>

<script src="TeleCalcFunctions.inc"></script>

<script language=vbscript>

Function GetNothing()
    Set GetNothing = Nothing
End Function

</script>

<script language=javascript>
<!--#include file="TeleCalcFunctions.inc"-->

// ---------------------- GLOBAL VARIABLES -------------------------------------------------------------------------------------------------

var fMinLoanAmount = 0;     // Minimum total loan amount ... not used at the moment (overridden by fMinFFTotalAmount)
var fMinFixedAmount = 0;    // Minimum fixed loan amount
var fMinFFTotalAmount = 0;  // Minimum total flexi fixed loan amount

var cmMaxPTIMarginSalaried = 0;     // Increase in PTI above the max PTI for the percentage fixed portion: 20 percent increase allowed : Salaried Employment Type : Stored as value below 1
var cmMaxPTIMarginSelfEmployed = 0;     // Increase in PTI above the max PTI for the percentage fixed portion: 20 percent increase allowed  : Self Employed Employment Type : Stored as value below 1
var cmMaxPTIMarginSubsidised = 0;     // Increase in PTI above the max PTI for the percentage fixed portion: 20 percent increase allowed  : Subsidised Employment Type : Stored as value below 1
var cmMaxLoanAmount = 0; // Determined in credit matrix
var cmPTIType = 0;      // Retrieve min or max PTI credit matrix. 0: min PTI 1: Max PTI
var cmMaxPropertyValue = 0;
var cmMinPropertyValue = 0;

var cmrsCreditMatrixOpen = false;

var fVariableLoanRate = 0;  //eWork specific: Loan rate for the variable portion is passed in from the loan table record

var loanPurpose = 0;    // Tracks currently selected loan purpose
var pSource = "";   // Form that called the calculator

var validated = false   // Used with validation function

var rsCreditMatrix = new ActiveXObject("ADODB.Recordset");  // Hold the credit matrix details

var eFolderId = 0;  // Used when updating eWorks database. Calculator launched from eWorks help desk.

var ref_PTI;    // Used in credit matrix function also for max loan amount and qualify
var ref_LTV;    // Used in credit matrix function

var calcFontWeight = "bold";    // Answer fields
var calcFontColour = "royalblue";   // Answer fields

var sessDSN = "DSN=<%= Session("DSN")%>";

var iVarifixVersion = 2;

    if(sessDSN == "DSN=") {
        alert("Your login has expired, please log in again.");
	    window.opener.location.href = "Default.asp";
    }

// ---------------------- END GLOBAL VARIABLES -------------------------------------------------------------------------------------------------

// ---------------------- EVENT FUNCTIONS --------------------------------------------------------------------------------------------------

function window_onload() {

    getRates();

    // Set to monthly instalments
    radio_monthly_instalment.checked = true

    // Monthly instalment default calculator
    show_monthly_instalment();  // If this is after set_params the readonly does not work out.

    // Retrieves parameters from the query string
    set_Params();

    // Retrieves the credit matrix recordset and stored locally in a disconnected recordset
    // moved from above show_monthly_instalment so that the cmPTIType param can get populated before retrieving the recordset
    getCreditMatrixRS();

    // Retrieves min loan amount (fMinFFTotalAmount) and min fixed amount (fMinFixedAmount)
    // calc_defaults_callback() calls the validate(1) and calculate functions.
    calc_defaults();  // must be last due to calc_defaults_callback()

    // testing code
    //ddlLinkRate.disabled = true;

}

function set_Params() {

    var pEmploymentType = "<%=Request.QueryString("EmploymentType")%>";
    var pMarketValue = "<%=Request.QueryString("MarketValue")%>";
    var pPurchasePrice = "<%=Request.QueryString("PurchasePrice")%>";
    var pLoanAmount = "<%=Request.QueryString("LoanAmount")%>";
    var pCashDeposit = "<%=Request.QueryString("CashDeposit")%>";
    var pCashRequired = "<%=Request.QueryString("CashRequired")%>";
    var pTerm = "<%=Request.QueryString("Term")%>";
    var pLinkRate = "<%=Request.QueryString("LinkRate")%>";
    var pFixedPercent = "<%=Request.QueryString("FixedPercent")%>";
    var pVariablePercent = "<%=Request.QueryString("VariablePercent")%>";
    var pName = "<%=Request.QueryString("Name")%>";
    var pNumber = "<%=Request.QueryString("Number")%>";
    var pIncome = "<%=Request.QueryString("Income")%>";
    iVarifixVersion =  "<%=Request.QueryString("VarifixVersion")%>";
    fVariableLoanRate = "<%=Request.QueryString("VariableLoanRate")%>"

    loanPurpose = "<%=Request.QueryString("Purpose")%>";       // Global variable
    pSource = "<%=Request.QueryString("Source")%>"; // Global variable

    // Common to all loan purposes
    txtName.value= pName;
    txtNumber.value = pNumber;
    ddlLoanPurpose.value = loanPurpose;
    ddlEmploymentType.value = pEmploymentType;
    txtTerm.value = pTerm;
    txtFixedPercent.value = formatNum2(pFixedPercent);
    txtVariablePercent.value = formatNum2(pVariablePercent);
    ddlLinkRate.value = pLinkRate;
    txtTotalAmount.value = pLoanAmount;
    txtPurchasePrice.value = (pPurchasePrice == 0) ? "" : pPurchasePrice;
    txtIncome.value = (pIncome == 0)? "" : pIncome ;
    txtPropertyValue.value = (pMarketValue == 0) ? "" : pMarketValue;

    if (pCashDeposit == "") pCashDeposit = 0;
    if (pCashRequired == "") pCashRequired = 0;

    // Loan purpose specific (Deposit and Cash Required)
    if (pCashDeposit > 0 && pCashRequired > 0) {

        txtDeposit.value = pCashDeposit - pCashRequired;

    }
    else if (parseFloat(pCashDeposit) > 0 && parseFloat(pCashRequired) == 0) {

        txtDeposit.value = pCashDeposit;

    }
    else if (parseFloat(pCashDeposit) == 0 && parseFloat(pCashRequired) > 0) {

        txtDeposit.value = pCashRequired;

    }

    update_amounts("export");

    calc_loanAmounts("fixed");

    set_loanPurposeDetails(ddlLoanPurpose.value);

    calc_rates();

 // Fields on the calculator are disabled depending on where the calculator was launched
 // Cosmetic only

    switch (pSource) {

        case "ManageProspect":

            td_number.innerHTML = "Prospect Number";
            cmd_hide.innerHTML = "Update";

            radio_loan_amount.disabled = true
            radio_extra_payments.disabled = true
            radio_maximum_loan.disabled = true
            radio_bond_costs.disabled = true
            radio_minimum_deposit.disabled = true

            toggle_readOnly_purchasePrice(true);
            toggle_readOnly_deposit(true);
            toggle_readOnly_loanAmount(true);

            break;

        case "ManagePreProspect":

            toggle_readOnly_purchasePrice(true);
            toggle_readOnly_deposit(true);
            td_number.innerHTML = "Preprospect Number";
            cmd_hide.innerHTML = "Update";

            radio_loan_amount.disabled = true
            radio_extra_payments.disabled = true
            radio_maximum_loan.disabled = true
            radio_bond_costs.disabled = true
            radio_minimum_deposit.disabled = true

            toggle_readOnly_loanAmount(true)

            break;

        case "TeleLeadManage":
            td_number.innerHTML = "TeleNumber";

            // Only used from TeleLeadManage
            cmPTIType = "<%=Request.QueryString("PTIType")%>";

            if (cmPTIType==1)
                chkHigherPTI.checked = true

            break;

        case "eWorkFlexi":
            cmd_hide.innerHTML = "Update";
            radio_loan_amount.disabled = true;
            radio_extra_payments.disabled = true;
            radio_maximum_loan.disabled = true;
            radio_bond_costs.disabled = true;
            radio_minimum_deposit.disabled = true;

            toggle_readOnly_purchasePrice(true);
            toggle_readOnly_deposit(true);
            toggle_readOnly_loanAmountSplits(true);
            toggle_readOnly_income(true);
            toggle_readOnly_propertyValue(true);
            toggle_readOnly_term(true);
            ddlLoanPurpose.disabled = true;
            ddlEmploymentType.disabled = true;
            ddlFixedMarketRateType.disabled = true;
            ddlVariableMarketRateType.disabled = true;
            toggle_readOnly_linkRate(true);

             //ddlLinkRate.disabled = true;
            //ddlLinkRate.readOnly = true;

            eFolderId = "<%=Request.QueryString("folderID")%>"

            break;

    }

}

function set_loanPurposeDetails(val) {

    if (val == 3) {
        // New Purchase
        td_pp.innerHTML = "&nbsp;Purchase Price (R)"

        td_dep.innerHTML = "&nbsp;Deposit (R)"

    }
    else if (val == 2) {
        // Switch
        td_pp.innerHTML = "&nbsp;Existing Loan (R)"

        td_dep.innerHTML = "&nbsp;Cash Required (R)"
    }
    else if (val == 4) {
        // Refinance
        td_pp.innerHTML = "&nbsp;Existing Loan (R)"

        td_dep.innerHTML = "&nbsp;Cash Required (R)"
    }
}

// --------------- PURCHASE PRICE ----------------

function txtPurchasePrice_onkeyup() {

    if (validate_PositiveNumber(txtPurchasePrice.value,"Purchase Price") == true) {

        update_amounts("purchase");

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }
    }
}

// --------------- DEPOSIT ----------------

function txtDeposit_onkeyup() {

    if (validate_PositiveNumber(txtDeposit.value,"Deposit") == true) {

        update_amounts("deposit");

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

// --------------- PERCENT SPLIT ----------------

function txtFixedPercent_onkeyup() {

    if (validate_PositiveNumber(txtFixedPercent.value,"Fixed Percent")== true) {

        calc_loanAmounts("fixed");

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

function txtVariablePercent_onkeyup() {

    if (validate_PositiveNumber(txtVariablePercent.value,"Variable Percent")== true) {

        calc_loanAmounts("variable");

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

// --------------- LOAN AMOUNT ----------------
function txtFixedAmount_onkeyup() {

    if (txtFixedAmount.value == "") {

        txtVariableAmount.value = txtTotalAmount.value;

        reset_fields("monthly_instalment");

        return 0;
    }

    if (validate_PositiveNumber(txtFixedAmount.value,"Fixed Amount")== true) {

        if (parseFloat(txtFixedAmount.value) > parseFloat(txtTotalAmount.value)) {

            alert("The fixed portion cannot be greater than the total loan amount");
            txtFixedAmount.value = "";
            txtVariableAmount.value = txtTotalAmount.value

        }

        calc_LoanAmountDiff("fixed");

        calc_percentages("fixed");

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()
            reset_fields("monthly_instalment");
        }

    }

}

function txtVariableAmount_onkeyup() {

    if (txtVariableAmount.value == "") {

        txtFixedAmount.value = txtTotalAmount.value;

        reset_fields("monthly_instalment");

        return 0;

    }

    if (validate_PositiveNumber(txtVariableAmount.value,"Variable Amount")== true) {

        if (parseFloat(txtVariableAmount.value) > parseFloat(txtTotalAmount.value)) {

            alert("The variable portion cannot be greater than the total loan amount");
            txtVariableAmount.value = "";
            txtFixedAmount.value = txtTotalAmount.value
        }

        validate(0);

        if (validated == true) {

            // moved before next for bug fix: variable amount = total amount but there is still value in fixed instalment
            calc_LoanAmountDiff("variable");

            creditMatrixCalculate();

            calc_percentages("variable");

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

function txtTotalAmount_onkeyup() {

    if (validate_PositiveNumber(txtTotalAmount.value,"Total Amount")== true) {

         // jason: taken out to stop reset if pre populated figures that are present.
         // jason: want to keep the percent split
       // txtVariableAmount.value = txtTotalAmount.value;
       // txtFixedAmount.value = "";

        // Updates purchase price
        update_amounts("loan");

        // jason: Also removed for as above
        // Updates percentage splits
        //calc_percentages("variable");

        // jason: Also added for as above
        calc_loanAmounts("fixed");

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}
// --------------- TERM ----------------------

function txtTerm_onkeyup() {

    if (validate_PositiveNumber(txtTerm.value,"Term")== true) {

        //update_amounts("loan");

        if (parseInt(txtTerm.value) > 240){
            alert("Cannot have a term > 240 months");
            txtTerm.value = 240;
            return;
        }

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

           // jason: removed and added following for some bug with change term (not updating purchase price)
            //update_amounts("deposit");

            update_amounts("loan");

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}
// --------------- INSTALMENT ----------------

function txtFixedInstalment_onkeyup(){
// Calculates variable instalment amount base on difference between total instalment and entered amount
// TODO: add check for min instalment based on min loan amount

/* jason: removed to stop tab on monthly instalment clearing all values.
    if (txtFixedInstalment.value == "") {

        reset_fields("loan_amount");

        txtVariableInstalment.value = txtTotalInstalment.value;

    }
  */

    if (validate_PositiveNumber(txtFixedInstalment.value,"Fixed Instalment")== true) {

        if (parseFloat(txtFixedInstalment.value) > parseFloat(txtTotalInstalment.value)) {

            alert("The fixed instalment cannot be greater than the total instalment amount");
            txtFixedInstalment.value = "";
            txtVariableInstalment.value = txtTotalInstalment.value;
            reset_fields("loan_amount");
        }

        calc_InstalmentDiff("fixed");

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

function txtVariableInstalment_onkeyup() {
// Variable Instalment
// Calculates fixed instalment amount base on difference between total instalment and entered amount

    if (txtVariableInstalment.value == "") {

        reset_fields("loan_amount");

        txtFixedInstalment.value = txtTotalInstalment.value;

    }

    if (validate_PositiveNumber(txtVariableInstalment.value,"Variable Instalment")== true) {

        // Check for greater than total
        if (parseFloat(txtVariableInstalment.value) > parseFloat(txtTotalInstalment.value)) {

            alert("The variable instalment cannot be greater than the total instalment amount");
            txtVariableInstalment.value = "";
            txtFixedInstalment.value = txtTotalInstalment.value;

        }

        calc_InstalmentDiff("variable");

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

function txtTotalInstalment_onkeyup() {
// Total Instalment

    //reset_fields("loan_amount");

    if (validate_PositiveNumber(txtTotalInstalment.value,"Total Instalment") == true) {

        /* jason: removed to stop clearing of fields when tabbing around
        txtVariableInstalment.value = txtTotalInstalment.value;

        txtFixedInstalment.value = "";
        */

        // jason: added for same
        if (parseFloat(txtFixedInstalment.value) <= parseFloat(txtTotalInstalment.value)) {
            txtVariableInstalment.value = txtTotalInstalment.value - txtFixedInstalment.value
        }
        else {

            txtVariableInstalment.value = txtTotalInstalment.value;

            txtFixedInstalment.value = "";

        }

        validate(0);

        if (validated == true) {
        //x

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

// --------------- ADDITIONAL INSTALMENTS ----

function txtFixedAddInstalment_onkeyup() {

    if (validate_PositiveNumber(txtFixedAddInstalment.value,"Fixed Additional Instalment")== true) {

        // Check for greater than total
        if (parseFloat(txtFixedAddInstalment.value) > parseFloat(txtTotalAddInstalment.value)) {

            alert("The fixed additional instalment cannot be greater than the total additional instalment amount");
            txtFixedAddInstalment.value = "";
            txtVariableAddInstalment.value = txtTotalAddInstalment.value;

        }

        // Calc variable portion
        calc_AddInstalmentDiff("fixed");

        validate(0);

        if (validated == true) {
        //x

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

function txtVariableAddInstalment_onkeyup() {

    if (validate_PositiveNumber(txtVariableAddInstalment.value,"Variable Additional Instalment")== true) {

        // Check for greater than total
        if (parseFloat(txtVariableAddInstalment.value) > parseFloat(txtTotalAddInstalment.value)) {

            alert("The variable additional instalment cannot be greater than the total additional instalment amount");
            txtVariableAddInstalment.value = "";
            txtFixedAddInstalment.value = txtTotalAddInstalment.value;

        }

        calc_AddInstalmentDiff("variable");

        validate(0);

        if (validated == true) {
        //x

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

function txtTotalAddInstalment_onkeyup() {

    if (validate_PositiveNumber(txtTotalAddInstalment.value,"Total Additional Instalment")== true) {

        // Keep the fixed additional instalment amount, alter the variable

        if (parseFloat(txtTotalAddInstalment.value) >= parseFloat(txtFixedAddInstalment.value)) {

            txtVariableAddInstalment.value = formatNum2(txtTotalAddInstalment.value - txtFixedAddInstalment.value)

        }
        else {

            txtVariableAddInstalment.value = txtTotalAddInstalment.value;

        }

        calc_AddInstalmentDiff("variable");

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

// --------------- INCOME --------------------

function txtIncome_onkeyup() {

    if (validate_PositiveNumber(txtIncome.value,"Income")== true) {

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

function txtPropertyValue_onkeyup() {

    if (validate_PositiveNumber(txtPropertyValue.value,"Property Value")== true) {

        validate(0);

        if (validated == true) {

            creditMatrixCalculate()

        }
        else {

            clear_creditMatrixDetails()

        }

    }

}

// --------------- DROP DOWNS ----------------

function ddlLoanPurpose_change(){
//

    loanPurpose = ddlLoanPurpose.value;

    if (loanPurpose == 4 && radio_minimum_deposit.checked == true) {
        alert("You cannot calculate a minimum deposit on a refinance");
        ddlLoanPurpose.value = 3;
        return;
    }

    set_loanPurposeDetails(ddlLoanPurpose.value)

    updateCreditMatrix();

    if (loanPurpose == 4)
        txtPurchasePrice.value = "";

    update_amounts("deposit");

    validate(0);

    if (validated == true) {

        creditMatrixCalculate()

    }
    else {

        clear_creditMatrixDetails()

    }

}

function ddlEmploymentType_change() {

    updateCreditMatrix();

}

function ddlFixedMarketRateType_change() {

  calc_rates();

}

function ddlVariableMarketRateType_change() {

    calc_rates();

}

function ddlLinkRate_change() {

    if (ddlLinkRate.value == 0) {

        reset_fields("link_rate");

    }

    calc_rates();

    validate(0);

    if (validated == true) {

        creditMatrixCalculate();

        if (radio_minimum_deposit.checked == false) {

            updateCreditMatrix();

        }

    }

}

function cmd_close_click() {

    switch (pSource) {

        case "eWorkFlexi":

           window.close();

            break;

        default:
            window.close();

    }

}
function cmd_hide_click() {
// Hides or updates calling page depeding on the page that launched the calculator

    var sText = td_qualify.innerText

    switch (pSource) {

        case "ManageProspect":

            if (sText.substr(0,3) == "YES") update_opener_percent();

            window.close();

            break;

        case "TeleLeadManage":

            window.blur();
            break;

        case "ManagePreProspect":
            if (sText.substr(0,3) == "YES") update_opener_percent();
            window.close();
            break;

        case "eWorkFlexi":
            update_eWorks();
            window.close();
            break;

        default:
            window.blur();

    }

}

function chkHigherPTI_click() {
// Retrieves the credit matrix recordset depending on the higher pti option

    if (chkHigherPTI.checked == true)
         cmPTIType = 1;
    else
        cmPTIType = 0;

    if (cmrsCreditMatrixOpen == true) {
        rsCreditMatrix.Close;
    }

    getCreditMatrixRS();

    validate(0);

    if (validated == true) {

        creditMatrixCalculate()

    }
    else {

        clear_creditMatrixDetails()

    }

}

// ---------------------- END EVENT FUNCTIONS --------------------------------------------------------------------------------------------------

// ---------------------- READ ONLY FUNCTIONS --------------------------------------------------------------------------------------------------

function toggle_readOnly_all(enable) {

    // Loan Amount
    toggle_readOnly_loanAmount(enable);

    // % split
    toggle_readOnly_percentSplit(enable);

    // Term
    toggle_readOnly_term(enable);

    // Market rate type
    ddlFixedMarketRateType.readOnly = enable;
    ddlVariableMarketRateType.readOnly = enable;

    // Link rate
    ddlLinkRate.readOnly = enable;

    // Interest Rate
    toggle_readOnly_interestRate(enable);

    // Instalment
    toggle_readOnly_instalment(enable);

    // Additional instalment
    toggle_readOnly_addInstalment(enable);

    // Misc
    txtDeposit.readOnly = enable;
    txtIncome.readOnly = enable;
    txtPropertyValue.readOnly = enable;

    // Interest paid
    toggle_readOnly_interestPaid(enable);

}

function toggle_readOnly_deposit(enable) {
// Read Only: Deposit

    txtDeposit.readOnly = enable;

}

function toggle_readOnly_loanAmount(enable) {
// Read Only: Loan Amount

    txtFixedAmount.readOnly = enable;
    txtVariableAmount.readOnly = enable;
    txtTotalAmount.readOnly = enable;

}

function toggle_readOnly_loanAmountSplits(enable) {
// Read Only: Loan Amount Splits

   txtTotalAmount.readOnly = enable;

}

function toggle_readOnly_linkRate(enable) {
// Read Only: Link rate drop-down
    //ddlLinkRate.readOnly = enable;
    ddlLinkRate.disabled = enable;
}

function toggle_readOnly_percentSplit(enable) {
// Read Only: % Split

    txtFixedPercent.readOnly = enable;
    txtVariablePercent.readOnly = enable;

}

function toggle_readOnly_term(enable) {
// Read Only: Term

    txtTerm.readOnly = enable;

}

function toggle_readOnly_interestRate(enable) {
// Read Only: Interest Rate

    txtFixedRate.readOnly = enable;
    txtVariableRate.readOnly = enable;

}

function toggle_readOnly_instalment(enable) {
// Read Only: Instalment

    txtFixedInstalment.readOnly = enable;
    txtVariableInstalment.readOnly = enable;
    txtTotalInstalment.readOnly = enable;

}

function toggle_readOnly_addInstalment(enable) {
// Read Only: Additional Instalment

    txtFixedAddInstalment.readOnly = enable;
    txtVariableAddInstalment.readOnly = enable;
    txtTotalAddInstalment.readOnly = enable;
}

function toggle_readOnly_interestPaid(enable) {
// Read Only: Interest Paid

    txtFixedInterestPaid.readOnly = enable;
    txtVariableInterestPaid.readOnly = enable;
    txtTotalInterestPaid.readOnly = enable;

}

function toggle_readOnly_savings(enable) {
// Read Only: Savings

    txtFixedSavings.readOnly = enable;
    txtVariableSavings.readOnly = enable;
    txtTotalSavings.readOnly = enable;

}

function toggle_readOnly_newTerm(enable) {
// Read Only: New Term

    txtNewTerm.readOnly = enable;

}

function toggle_readOnly_purchasePrice(enable) {
// Read Only: Purchase Price

    txtPurchasePrice.readOnly = enable;

}

function toggle_readOnly_income(enable) {
// Read Only: Income

    txtIncome.readOnly = enable;

}

function toggle_readOnly_propertyValue(enable) {
// Read Only: Property Value

    txtPropertyValue.readOnly = enable;

}

function toggle_readOnly_monthlyInstalmentsCalc(enable) {

    // Reset readonly status
    toggle_readOnly_all(!enable);

   // Interest Rate
   toggle_readOnly_interestRate(enable);

   // Instalment
   toggle_readOnly_instalment(enable);

   // Interest Paid
   toggle_readOnly_interestPaid(enable);

}

function toggle_readOnly_loanAmountCalc(enable) {

    // Reset readonly status
    toggle_readOnly_all(!enable);

    // % Split
    toggle_readOnly_percentSplit(enable);

    // Loan Amount
    toggle_readOnly_loanAmount(enable);

    // Interest Rate
    toggle_readOnly_interestRate(enable);

    // Interest Paid
    toggle_readOnly_interestPaid(enable);

}

function toggle_readOnly_extraPaymentsCalc(enable) {

    // Reset readonly status
    toggle_readOnly_all(!enable);

   // Instalment
   toggle_readOnly_instalment(enable);

   // Interest Rate
   toggle_readOnly_interestRate(enable);

   // Interest Paid
   toggle_readOnly_interestPaid(enable);

   // Savings
   toggle_readOnly_savings(enable);

   // New Term
   toggle_readOnly_newTerm(enable);

}

function toggle_readOnly_maximumLoanCalc(enable) {

    // Reset readonly status
    toggle_readOnly_all(!enable);

    // % Split
    toggle_readOnly_percentSplit(enable);

    // Loan Amount
    toggle_readOnly_loanAmount(enable);

    // Interest Paid
    toggle_readOnly_interestPaid(enable);

   // Instalment
   toggle_readOnly_instalment(enable);

   // Interest Rate
   toggle_readOnly_interestRate(enable);

}

function toggle_readOnly_minimumDepositCalc(enable) {

    // Reset readonly status
    toggle_readOnly_all(!enable);

    // Loan Amount
    // toggle_readOnly_loanAmount(enable);
    txtTotalAmount.readOnly = enable;

    // Instalment
    toggle_readOnly_instalment(enable);

    // Interest Paid
    toggle_readOnly_interestPaid(enable);

    // Deposit
    toggle_readOnly_deposit(enable);

}

function toggle_readOnly_fixed(enable) {

    txtFixedPercent.readOnly = enable;
    txtFixedAmount.readOnly = enable;
    ddlFixedMarketRateType.readOnly = enable;
    txtFixedRate.readOnly = enable;
    txtFixedInstalment.readOnly = enable;
    txtFixedInterestPaid.readOnly = enable;

    if (enable == true) {
        var colour = "#DCDCDC";  //grey
        var foreColour = "#DCDCDC";
    }
    else {
        var colour = "white";
        var foreColour = "black";
    }

    // Background colour
    txtFixedPercent.style.backgroundColor = colour;
    txtFixedAmount.style.backgroundColor = colour;
    ddlFixedMarketRateType.style.backgroundColor = colour;
    txtFixedRate.style.backgroundColor = colour;
    txtFixedInstalment.style.backgroundColor = colour;
    txtFixedInterestPaid.style.backgroundColor = colour;

    // Font colour
    txtFixedPercent.style.color = foreColour;
    txtFixedAmount.style.color = foreColour;
    ddlFixedMarketRateType.style.color = foreColour;
    txtFixedRate.style.color = foreColour;
    txtFixedInstalment.style.color = foreColour;
    txtFixedInterestPaid.style.color = foreColour;

}

function toggle_readOnly_variable(enable) {

    txtVariablePercent.readOnly = enable;
    txtVariableAmount.readOnly = enable;
    ddlVariableMarketRateType.readOnly = enable;
    txtVariableRate.readOnly = enable;
    txtVariableInstalment.readOnly = enable;
    txtVariableInterestPaid.readOnly = enable;

    if (enable == true) {
        var colour = "#DCDCDC";  //grey
        var foreColour = "#DCDCDC";
    }
    else {
        var colour = "white";
        var foreColour = "black";
    }

    // Background colour
    txtVariablePercent.style.backgroundColor = colour;
    txtVariableAmount.style.backgroundColor = colour;
    ddlVariableMarketRateType.style.backgroundColor = colour;
    txtVariableRate.style.backgroundColor = colour;
    txtVariableInstalment.style.backgroundColor = colour;
    txtVariableInterestPaid.style.backgroundColor = colour;

    // Font colour
    txtVariablePercent.style.color = foreColour;
    txtVariableAmount.style.color = foreColour;
    ddlVariableMarketRateType.style.color = foreColour;
    txtVariableRate.style.color = foreColour;
    txtVariableInstalment.style.color = foreColour;
    txtVariableInterestPaid.style.color = foreColour;

}

// ---------------------- END READ ONLY FUNCTIONS --------------------------------------------------------------------------------------

// ---------------------- SHOW HIDE FUNCTIONS --------------------------------------------------------------------------------------------------

function show_monthly_instalment() {

// General
    window.td_detail_title.innerHTML = "<b>Monthly Instalment</b>";

// Read only
    toggle_readOnly_fixed(false);
    toggle_readOnly_variable(false);
    toggle_readOnly_monthlyInstalmentsCalc(true);

// Hide
    // Additional Instalment
    toggle_hide_add_instalment(true);

    // Savings
    toggle_hide_savings(true);

    // New Term
    toggle_hide_new_term(true);

    // InterestRate
    toggle_hide_interestRate(true);

// Data
    // Credit Matrix
    clear_creditMatrixDetails();

    // Reset
    reset_fields("monthly_instalment");

    validate(1);

// Tab Index
    set_tabIndex("monthly_instalment");
}

function show_loan_amount() {

// General
    window.td_detail_title.innerHTML = "<b>Loan Amount</b>"

// Read only
    toggle_readOnly_fixed(false);
    toggle_readOnly_variable(false);
    toggle_readOnly_loanAmountCalc(true);

// Hide
    // Additional Instalment
    toggle_hide_add_instalment(true);

    // Savings
    toggle_hide_savings(true);

    // New Term
    toggle_hide_new_term(true);

    // InterestRate
    toggle_hide_interestRate(true);

// Data
    // Credit Matrix
    clear_creditMatrixDetails();

    reset_fields("loan_amount");

    validate(1)

// Tab Index
    set_tabIndex("loan_amount");

}

function show_extra_payments() {

// General
    window.td_detail_title.innerHTML = "<b>Extra Payments</b>";

// Read only
    toggle_readOnly_fixed(false);
    toggle_readOnly_variable(false);
    toggle_readOnly_extraPaymentsCalc(true);

// Show
    // Additional Instalment
    toggle_hide_add_instalment(false);

    // Savings
    toggle_hide_savings(false);

    // New Term
    toggle_hide_new_term(false);

    // InterestRate
    toggle_hide_interestRate(true);

// Data
    // Credit Matrix
    clear_creditMatrixDetails();

    // Reset
    reset_fields("extra_payments");

    validate(1);

// Tab Index
    set_tabIndex("extra_payments");

}

function show_maximum_loan() {

// General
    window.td_detail_title.innerHTML = "<b>Maximum Loan</b>";

// Read only
    toggle_readOnly_maximumLoanCalc(true);
    toggle_readOnly_fixed(true);
    toggle_readOnly_variable(true);

// Hide
    // Additional Instalment
    toggle_hide_add_instalment(true);

    // Savings
    toggle_hide_savings(true);

    // InterestRate
    toggle_hide_interestRate(false);

    // New Term
    toggle_hide_new_term(true);

// Data
    // Credit Matrix
    clear_creditMatrixDetails();

    // Reset
    reset_fields("maximum_loan");

    validate(1);

    txtIncome_onkeyup()

// Tab Index
    set_tabIndex("maximum_loan");
}

function show_minimum_deposit() {

// General
    window.td_detail_title.innerHTML = "<b>Minimum Deposit</b>";

// Read only
    toggle_readOnly_minimumDepositCalc(true);
    toggle_readOnly_fixed(true);
    toggle_readOnly_variable(true);
    toggle_readOnly_income(true);
    toggle_readOnly_propertyValue(true);

// Hide

    // Additional Instalment
    toggle_hide_add_instalment(true);

    // InterestRate
    toggle_hide_interestRate(true);

// Data
    // Credit Matrix
    clear_creditMatrixDetails();

    // Reset
    reset_fields("minimum_deposit");

    if (txtPurchasePrice.value > 0)
        txtPurchasePrice_onkeyup();

    validate(1);

// Tab Index
    set_tabIndex("minimum_deposit");

}

function show_bond_costs() {

var pPurpose = (ddlLoanPurpose.selectedIndex == -1) ? 3 : ddlLoanPurpose.value;

var pPurchasePrice = parseFloat((txtPurchasePrice.value == "") ? 0 : txtPurchasePrice.value);
var pDeposit = parseFloat((txtDeposit.value == "") ? 0 : txtDeposit.value);
var pLoanAmount = parseFloat((txtTotalAmount.value == "") ? 0 : txtTotalAmount.value);

//alert("p:" + pPurpose + "pp: " + pPurchasePrice + "d: " + pDeposit  + "la: " + pLoanAmount);

    switch (pPurpose) {

        case "2":   // Switch
            pLoanAmount =  pPurchasePrice
            break;

        case "3":   // New Purchase

            if (pPurchasePrice == 0 && pLoanAmount > 0) {

                pPurchasePrice = pLoanAmount;

            }

            break;

        case "4":   // Refinance

            break;

    }

    // Required for current bond cost calculator that used selectedIndex instead of value
    pPurpose = pPurpose - 2

    window.location.href = "TeleCalcBondCosts.asp?Purpose=" + pPurpose
                                + "&EmploymentType=" + ddlEmploymentType.selectedIndex
                                + "&Term=" + txtTerm.value
                                + "&MarketValue=" + txtPropertyValue.value
                                + "&LoanAmount=" + pLoanAmount
                                + "&CashRequired=" + pDeposit
                                + "&CashDeposit=" + pDeposit
                                + "&PurchasePrice=" + pPurchasePrice
                                + "&FixedPercent=" + txtFixedPercent.value
                                + "&Principal=" + 0
                                + "&isFlexi=1";

}

// ---------------------- END SHOW HIDE FUNCTIONS --------------------------------------------------------------------------------------------------

// ---------------------- TOGGLE HIDE FUNCTIONS ------------------------------------------------------------------------------------------

function toggle_hide_savings(enable) {

    if (enable == true)
        var displayType = "none";
    else
        var displayType = "";

    window.tr_title_savings.style.display = displayType;
    window.tr_fixed_savings.style.display = displayType;
    window.tr_variable_savings.style.display = displayType;
    window.tr_total_savings.style.display = displayType;

}

function toggle_hide_new_term(enable) {

    if (enable == true)
        var displayType = "none";
    else
        var displayType = "";

    window.tr_title_new_term.style.display = displayType;
    window.tr_fixed_new_term.style.display = displayType;
    window.tr_variable_new_term.style.display = displayType;
    window.tr_total_new_term.style.display = displayType;

}

function toggle_hide_add_instalment(enable) {

    if (enable == true)
        var displayType = "none";
    else
        var displayType = "";

    window.tr_title_add_instalment.style.display = displayType;
    window.tr_fixed_add_instalment.style.display = displayType;
    window.tr_variable_add_instalment.style.display = displayType;
    window.tr_total_add_instalment.style.display = displayType;

}

function toggle_hide_interestRate(enable) {

    if (enable == true)
        var displayType = "hidden";
    else
        var displayType = "visible";

    txtInterestRate.style.visibility = displayType

}

// ---------------------- END TOGGLE HIDE FUNCTIONS -------------------------------------------------------------------------------------------------

// ---------------------- VALIDATION FUNCTIONS -----------------------------------------------------------------------------------------------

function validate_Empty (num, field)
{

	if (num.length == 0) {
		window.td_error.innerHTML = "Please enter a value for "  + field;
		return false;
	}
	else {
		window.td_error.innerHTML = "";
		return true;
	}
}

function validate_PositiveNumber (num, field)
{
	var decimal = 0;

   for (var i = 0; i < num.length; i++)
   {
       ch = num.charAt(i)
       if (ch == '.')
			decimal++;

       if (((ch < "0" || "9" < ch) && ch != '.') || decimal > 1)
       {
				window.td_error.innerHTML = "Please enter a valid positive number for "  + field;
			return false;
	   }
   }

	window.td_error.innerHTML = "";
   return true;
}

function validate_Maximum(num, field, amt)
{
	if (parseFloat(num) > amt) {
		window.td_error.innerHTML = "Please enter a value less than R" + amt + " for " + field;
		return false;
	}
	else {
		window.td_error.innerHTML = "";
		return true;
	}
}

function validate_MinimumLoan(num, field, amt)
{
    // Specific case requested
    if (field == "Fixed Loan" && parseFloat(num) == 0) {
        window.td_error.innerHTML = "";
		return true;
    }

	if (parseFloat(num) < amt) {
		window.td_error.innerHTML = "Please enter a value greater than R" + amt + " for " + field;
		return false;
	}
	else {
		window.td_error.innerHTML = "";
		return true;
	}
}

function validate_LinkRate() {

    if (ddlLinkRate.value == 0) {
		window.td_error.innerHTML = "Please select a valid link rate";
		return false;
	}
	else {
		window.td_error.innerHTML = "";
		return true;
	}

}

function validate(type) {

    // General validations
    // type: 0 ordinary validation during calculations
    // type: 1 initial validation when launching, affects the focus()

    validated = false;

    if (radio_monthly_instalment.checked == true) {

        // Total Loan Amount
        if (type == 1) txtTotalAmount.focus();
        if (validate_Empty(txtTotalAmount.value, "Total Loan Amount")== false) return 0;
        if (validate_MinimumLoan(txtTotalAmount.value,"Total Loan Amount", fMinFFTotalAmount)== false) return 0;

        // Fixed Loan Amount
        if (type == 1) txtFixedAmount.focus();
        if (validate_Empty(txtFixedAmount.value, "Fixed Loan Amount")== false) return 0;
        if (validate_MinimumLoan(txtFixedAmount.value,"Fixed Loan", fMinFixedAmount)== false) return 0;

        // Interest Rates
        if (pSource != "eWorkFlexi")
        {
        if (type == 1) ddlLinkRate.focus();
        if (validate_LinkRate() == false) return 0;
        }

        if (validate_Empty(txtFixedRate.value, "Fixed Rate")== false) return 0;
        if (validate_Empty(txtVariableRate.value, "Variable Rate")== false) return 0;

        // Term
        if (type == 1) txtTerm.focus();
        if (validate_Empty(txtTerm.value, "Term")== false) return 0;

        // Split
        if (validate_Empty(txtFixedPercent.value, "Fixed Percent")== false) return 0;

        if (type == 1) txtIncome.focus();

        validated = true;

    }
    else if (radio_loan_amount.checked==true) {

        // Instalment
        if (type == 1) txtTotalInstalment.focus()
        if (validate_Empty(txtTotalInstalment.value, "Total Instalment")== false) return 0;

        if (type == 1) txtFixedInstalment.focus()
        if (validate_Empty(txtFixedInstalment.value, "Fixed Instalment")== false) return 0;

        // Interest Rates
        if (type == 1) ddlLinkRate.focus()
        if (validate_LinkRate() == false) return 0;
        if (validate_Empty(txtFixedRate.value, "Fixed Rate")== false) return 0;
        if (validate_Empty(txtVariableRate.value, "Variable Rate")== false) return 0;

        // Term
        if (type == 1) txtTerm.focus()
        if (validate_Empty(txtTerm.value, "Term")== false) return 0;

        // Split
        if (type == 1) txtFixedPercent.focus()
        if (validate_Empty(txtFixedPercent.value, "Fixed Percent")== false) return 0;

        if (type == 1) txtTotalInstalment.focus()
        if (validate_Empty(txtVariablePercent.value, "Variable Percent")== false) return 0;

        if (type == 1) txtIncome.focus()
        validated = true;

    }
    else if (radio_extra_payments.checked==true) {

        // Total Loan Amount
        if (type == 1) txtTotalAmount.focus()
        if (validate_Empty(txtTotalAmount.value, "Total Loan Amount")== false) return 0;
        if (validate_MinimumLoan(txtTotalAmount.value,"Total Loan Amount", fMinFFTotalAmount)== false) return 0;

        // Fixed Loan Amount
        if (txtFixedAmount.value == 0) {

          txtFixedAmount.value = "";

        }

        if (type == 1) txtFixedAmount.focus()
        if (validate_Empty(txtFixedAmount.value, "Fixed Loan Amount")== false) return 0;
        if (validate_MinimumLoan(txtFixedAmount.value,"Fixed Loan", fMinFixedAmount)== false) return 0;

        // Interest Rates
        if (type == 1) ddlLinkRate.focus()
        if (validate_LinkRate() == false) return 0;
        if (validate_Empty(txtFixedRate.value, "Fixed Rate")== false) return 0;
        if (validate_Empty(txtVariableRate.value, "Variable Rate")== false) return 0;

        // Additional Instalment
        if (type == 1) txtTotalAddInstalment.focus()
        if (validate_Empty(txtTotalAddInstalment.value, "Total Additional Instalment")== false) return 0;

        if (type == 1) txtFixedAddInstalment.focus()
        if (validate_Empty(txtFixedAddInstalment.value, "Fixed Additional Instalment")== false) return 0;

        if (type == 1) txtVariableAddInstalment.focus()
        if (validate_Empty(txtVariableAddInstalment.value, "Variable Additional Instalment")== false) return 0;

        // Term
        if (type == 1) txtTerm.focus()
        if (validate_Empty(txtTerm.value, "Term")== false) return 0;

        // Split
        if (type == 1) txtFixedPercent.focus()
        if (validate_Empty(txtFixedPercent.value, "Fixed Percent")== false) return 0;

        if (type == 1) txtVariablePercent.focus()
        if (validate_Empty(txtVariablePercent.value, "Variable Percent")== false) return 0;

        // Variable Loan Amount
        if (type == 1) txtVariableAmount.focus()
        if (validate_Empty(txtVariableAmount.value, "Variable Loan Amount")== false) return 0;

        // Final
        if (type == 1) txtIncome.focus()

        validated = true;

    }
    else if (radio_maximum_loan.checked==true) {

        // Income
        if (type == 1) txtIncome.focus()
        if (validate_Empty(txtIncome.value, "Income")== false) {
            // The answer is cleared if the user back spaces to income amount.
            txtTotalAmount.value = "";
            return 0;

        }

        // Interest Rates
        if (type == 1) ddlLinkRate.focus()
        if (validate_LinkRate() == false) return 0;
        if (validate_Empty(txtInterestRate.value, "Interest Rate")== false) return 0;

        // Term
        if (type == 1) txtTerm.focus()
        if (validate_Empty(txtTerm.value, "Term")== false) return 0;

        validated = true;
    }
    else if (radio_minimum_deposit.checked == true) {

        // Purchase Price
        if (type == 1) txtPurchasePrice.focus()

        var sField = "Existing Loan"

        if (ddlLoanPurpose.value == 3) sField = "Purchase Price"

        if (validate_Empty(txtPurchasePrice.value, sField)== false) return 0;

        // Total Loan Amount
        if (type == 1) txtTotalAmount.focus()
        if (validate_Empty(txtTotalAmount.value, "Total Loan Amount")== false) return 0;
        if (validate_MinimumLoan(txtTotalAmount.value,"Total Loan Amount", fMinFFTotalAmount)== false) return 0;

        // Interest Rates
        if (type == 1) ddlLinkRate.focus()
        if (validate_LinkRate() == false) return 0;
        if (validate_Empty(txtFixedRate.value, "Fixed Rate")== false) return 0;
        if (validate_Empty(txtVariableRate.value, "Variable Rate")== false) return 0;

        validated = true;

    }

}

// -------------------------------- END VALIDATION FUNCTIONS ------------------------------------------------------------------------------------

// -------------------------------- CALC FUNCTIONS ---------------------------------------------------------------------------------------------------

function calculate() {

    if (radio_monthly_instalment.checked == true) {

        /*
            Order of calculation
                1 - Enter total amount
                2 - Enter fixed amount
                3 - Selected rate
                4 - Calculate fixed instalment
                5 - Calculate variable instalment
                6 - Sum instalments
                7 - Calculate interest paid
        */

        // 4
        if (txtFixedAmount.value == "" || txtFixedAmount.value == 0) {

            txtFixedInstalment.value = "";

        }
        else {

            // Calculate fixed instalment
            var fFixedInstalment = CalculateInstallment(txtFixedAmount.value,txtFixedRate.value/100,12,txtTerm.value,0);

            // Update fixed instalment field
            if (fFixedInstalment == 0) {

                txtFixedInstalment.value = "";
            }
            else {

                txtFixedInstalment.value = fFixedInstalment;

            }

        }

        // 5
        if (txtVariableAmount.value == "" || txtVariableAmount.value == 0) {

            txtVariableInstalment.value = "";

        }
        else {

            // Calculate the variable instalment amount
            var fVariableInstalment = formatNum2(CalculateInstallment(txtVariableAmount.value,txtVariableRate.value/100,12,txtTerm.value,0));

            // Update the variable instalment field
            if (fVariableInstalment == 0) {

                txtVariableInstalment.value = "";

            }
            else {

                txtVariableInstalment.value = fVariableInstalment;

            }

        }

        // 6
        sum_instalments();

        //alert("test");

        // 7
        calc_lifeInterest(txtTerm.value);

        txtTotalInstalment.style.fontWeight = calcFontWeight
        txtTotalInstalment.style.color = calcFontColour

    }
    else if (radio_loan_amount.checked == true) {

        /*
            Order of calculation
                1 - Enter total instalment
                2 - Enter either fixed or variable instalment
                3 - Calculate the other instalment (difference) (in keyup events)
                4 - Calculate fixed loan amount
                5 - Calculate variable loan amount
                6 - Sum variable and fixed loan amounts
                7 - Calculate fixed % split
                8 - Calculate variable % split
                9 - Calculate fixed interest paid
                10 - Calculate variable interest paid
                11 - Sum total interest paid

        */

        // 4
        if (txtFixedInstalment.value == 0 || txtFixedInstalment.value == "" ) {

            txtFixedAmount.value = "";

        }
        else {

            txtFixedAmount.value = formatNum2(CalculateLoan(txtFixedInstalment.value, txtFixedRate.value/100, 12, txtTerm.value,0));

        }

        // 5
        if (txtVariableInstalment.value == "" || txtVariableInstalment.value == 0) {

            txtVariableAmount.value = "";
        }
        else {

            txtVariableAmount.value = formatNum2(CalculateLoan(txtVariableInstalment.value, txtVariableRate.value/100, 12, txtTerm.value,0));

        }

        // 6
        sum_loanAmounts();

        // 7, 8
        calc_percentages("fixed");

        // 9, 10, 11
        calc_lifeInterest(txtTerm.value);

        txtTotalAmount.style.fontWeight = calcFontWeight
        txtTotalAmount.style.color = calcFontColour

    }
    else if (radio_extra_payments.checked == true) {

        /*
            Order of calculation
                1 - Enter total loan amount
                2 - Enter fixed loan amount
                3 - Calculate variable loan amount  (keyup event)
                4 - Calculate fixed % split         (keyup event)
                5 - Calculate variable % split      (keyup event)
                6 - Calculate instalments
                7 - Sum instalments
                8 - Enter total additional instalment
                9 - Enter fixed additional instalment
                10 - Calculate fixed interest paid
                11 - Calculate variable interest paid
                12 - Sum interest paid
                13 - Calculate new term
                14 - Calculate fixed saving
                15 - Calculate variable saving
                16 - Sum total saving

        */

        // 6

        txtFixedInstalment.value = formatNum2(CalculateInstallment(txtFixedAmount.value,txtFixedRate.value/100,12,txtTerm.value,0));

        txtVariableInstalment.value = formatNum2(CalculateInstallment(txtVariableAmount.value,txtVariableRate.value/100,12,txtTerm.value,0));

        // 7
        sum_instalments();

        // 13
        calc_new_term();

        // 10, 11, 12
        calc_lifeInterest(txtNewTerm.value);

        // 14, 15, 16
        calc_lifeInterestSavings(txtTerm.value,txtNewTerm.value)

        sum_savings()

        //
        txtNewTerm.style.fontWeight = calcFontWeight;
        txtNewTerm.style.color = calcFontColour;
        txtFixedSavings.style.fontWeight = calcFontWeight;
        txtFixedSavings.style.color = calcFontColour;
        txtVariableSavings.style.fontWeight = calcFontWeight;
        txtVariableSavings.style.color = calcFontColour;
        txtTotalSavings.style.fontWeight = calcFontWeight;
        txtTotalSavings.style.color = calcFontColour;

    }
    else if (radio_maximum_loan.checked == true) {

    /*
        Order of calculation
            1 - Check PTI is set
            2 - Enter income
            3 - Calculate instalment
            4 - Calculate loan amount
            5 - Set deposit = 0
            6 - Set purchase amount = loan amount
            7 - Calculate interest paid
            8 - Set Font
    */

        // 1
        if (td_PTI.innerHTML == "") {
            td_error.innerHTML = "Cannot calculate max loan due to invalid PTI"
            return 0;
        }

        // 3
        txtTotalInstalment.value = formatNum2(parseFloat(ref_PTI) * txtIncome.value/100);

        // 4
        txtTotalAmount.value = formatNum2(CalculateLoan(txtTotalInstalment.value, txtInterestRate.value/100, 12, txtTerm.value,0));

        // 6
        //txtPurchasePrice.value = txtTotalAmount.value;

        // 7
        calc_lifeInterest(txtTerm.value);

        // 8
        txtTotalAmount.style.fontWeight = calcFontWeight;
        txtTotalAmount.style.color = calcFontColour;

    }
    else if (radio_minimum_deposit.checked == true) {

    /*
        Order of calculation
            0 - Update credit matrix
            1 - Check LTV is set
            2 - Enter Purchase Price
            3 - Calculate Deposit
            4 - Calculate Loan Amounts
            5 - Calculate Instalments
            6 - Sum Instalments
            7 - Calculate Interest Paid (var,fix,tot)
    */
        // 0
        updateCreditMatrix();

        // 1
        if (td_LTV.innerHTML == "") {
            td_error.innerHTML = "Cannot calculate minimum deposit due to invalid LTV"
            return 0;
        }

        // 3
        var depPerc = 100 - parseFloat(td_LTV.innerHTML);

        txtDeposit.value = formatNum2(parseFloat(txtPurchasePrice.value) * depPerc / 100);

        // 4
        if (parseFloat(txtPurchasePrice.value) >= parseFloat(txtDeposit.value)) {

            txtTotalAmount.value = formatNum2(parseFloat(txtPurchasePrice.value) - parseFloat(txtDeposit.value));
            txtVariableAmount.value = formatNum2(txtTotalAmount.value);

        }

        // 5
        if (parseFloat(txtFixedAmount.value) > 0 ) {

            txtFixedInstalment.value = formatNum2(CalculateInstallment(txtFixedAmount.value,txtFixedRate.value/100,12,txtTerm.value,0));

        }
        else {

            txtFixedInstalment.value = 0;

        }

        if (parseFloat(txtVariableAmount.value) > 0) {

            txtVariableInstalment.value = formatNum2(CalculateInstallment(txtVariableAmount.value,txtVariableRate.value/100,12,txtTerm.value,0));

        }
        else {

            txtVariableInstalment.value = 0;

        }

        // 6
        sum_instalments();

        txtIncome.value = formatNum2(txtTotalInstalment.value /( parseFloat(ref_PTI)/100));
        txtPropertyValue.value = formatNum2(txtTotalAmount.value /( parseFloat(ref_LTV)/100));

        // 7
        calc_lifeInterest(txtTerm.value);

        // 8
        txtDeposit.style.fontWeight = calcFontWeight;
        txtDeposit.style.color = calcFontColour;

    }

}

function calc_defaults() {
// Retrieves min loan amount and min fixed amount

    envokeRS("TeleCalculatorFlexiFunctions.asp"
                       + "?CalcType=Defaults",0);

}

function calc_defaults_callback() {
// Called from TeleCalculatorFlexiFunctions.asp
// This code is run after the window_onload event

        validate(1);

        if (validated == true) {

            calculate();

            updateCreditMatrix();

        }
        else {

            clear_creditMatrixDetails()

        }

}

function calc_lifeInterest(iTerm) {

//function CalculateLifeInterest(d_Loan, d_PeriodRate, i_InterestPeriods, i_Term, i_Type) {
    //d_Loan = loan amount
    //d_PeriodRate = interest rate for the period as a percentage
    //i_InterestPeriods = the number of interest periods per year
    //i_Term = the number of interest periods over which the loan is to be repaid
    //i_Type = (0 = Installment at end of period 1 = Installment at beginning of period)

    if (iTerm == 0) return 0;

    // When max loan is selected
    if (radio_maximum_loan.checked == true) {

        txtTotalInterestPaid.value = CalculateLifeInterest(txtTotalAmount.value,txtInterestRate.value/100,12,iTerm,0);

        return 0;
    }

    if (parseFloat(txtFixedAmount.value) > 0) {

        txtFixedInterestPaid.value = formatNum2(CalculateLifeInterest(txtFixedAmount.value,txtFixedRate.value/100,12,iTerm,0));

    }
    else  {

        txtFixedInterestPaid.value = "";

    }

    if (parseFloat(txtVariableAmount.value) > 0) {

        txtVariableInterestPaid.value = formatNum2(CalculateLifeInterest(txtVariableAmount.value,txtVariableRate.value/100,12,iTerm,0));

    }
    else {

        txtVariableInterestPaid.value = "";

    }

    sum_interestPaid();

}

function calc_lifeInterestSavings(iOldTerm, iNewTerm) {

//function CalculateLifeInterest(d_Loan, d_PeriodRate, i_InterestPeriods, i_Term, i_Type) {
    //d_Loan = loan amount
    //d_PeriodRate = interest rate for the period as a percentage
    //i_InterestPeriods = the number of interest periods per year
    //i_Term = the number of interest periods over which the loan is to be repaid
    //i_Type = (0 = Installment at end of period 1 = Installment at beginning of period)

    var OldFixedInterestPaid = 0;
    var NewFixedInterestPaid = 0;
    var OldVariableInterestPaid = 0;
    var NewVariableInterestPaid = 0;

    if (iOldTerm == 0 || iNewTerm == 0) return 0;

    if (parseFloat(txtFixedAmount.value) > 0) {

        OldFixedInterestPaid = CalculateLifeInterest(txtFixedAmount.value, txtFixedRate.value/100 ,12 , iOldTerm,0);

        NewFixedInterestPaid = CalculateLifeInterest(txtFixedAmount.value, txtFixedRate.value/100, 12, iNewTerm, 0);

    }

    if (parseFloat(txtVariableAmount.value) > 0) {

        OldVariableInterestPaid = CalculateLifeInterest(txtVariableAmount.value,txtVariableRate.value/100,12,iOldTerm,0);

        NewVariableInterestPaid = CalculateLifeInterest(txtVariableAmount.value,txtVariableRate.value/100,12,iNewTerm,0);

    }

    // Calculate the difference
    var FixedSaving = OldFixedInterestPaid - NewFixedInterestPaid;
    var VariableSaving = OldVariableInterestPaid - NewVariableInterestPaid;

    // Update the fields
    txtFixedSavings.value = formatNum2(FixedSaving);
    txtVariableSavings.value = formatNum2(VariableSaving);

}

function calc_new_term() {
// Calculates the new term based on the increased instalments
    //CalculateTerm(d_Loan, d_PeriodRate, i_InterestPeriods, d_Installment, i_Type)
    //d_Loan = loan amount
    //d_PeriodRate = interest rate for the period as a percentage
    //i_InterestPeriods = the number of interest periods per year
    //d_Installment = the installment amount
    //i_Type = (0 = Installment at end of period 1 = Installment at beginning of period)

    var fixedTerm = 240;
    var variableTerm = 240;
    var totalFixedInstalment = 0;
    var totalVariableInstalment = 0;

    // Calculate the new total instalments
    totalFixedInstalment = parseFloat(txtFixedInstalment.value) + parseFloat(txtFixedAddInstalment.value);
    totalVariableInstalment = parseFloat(txtVariableInstalment.value) + parseFloat(txtVariableAddInstalment.value);

    // Calculate the 2 new terms
    fixedTerm = CalculateTerm(txtFixedAmount.value, txtFixedRate.value/100, 12, totalFixedInstalment,0);
    variableTerm = CalculateTerm(txtVariableAmount.value, txtVariableRate.value/100, 12, totalVariableInstalment,0);

    // Find the highest of the two
    if (fixedTerm >= variableTerm) {

        txtNewTerm.value = formatNum0(fixedTerm);

    }
    else {

        txtNewTerm.value = formatNum0(variableTerm);

    }

}

function calc_percentages(calc_type) {
// LOAN PERCENT SPLIT
// Calculates the percentages using the fixed amount and total
// Called from fixed and variable loan amount change events

//alert("calc_percentages: " + calc_type);

    if (txtTotalAmount.value > 0) {

        switch (calc_type) {

            case "fixed":

                if (parseFloat(txtFixedAmount.value) >= 0) {

                    var fPerc = parseFloat(txtFixedAmount.value) / parseFloat(txtTotalAmount.value) * 100;
                    var vPerc = 100 - formatNum2(fPerc);

                }
                else {

                    var fPerc = 0
                    var vPerc = 100

                }

                break;

            case "variable":

                if (parseFloat(txtVariableAmount.value) >= 0) {

                    var vPerc = parseFloat(txtVariableAmount.value) / parseFloat(txtTotalAmount.value) * 100;
                    var fPerc = 100 - formatNum2(vPerc);

                }
                else {

                    var fPerc = 100
                    var vPerc = 0

                }

        }

    }
    else {

        var fPerc = 0;
        var vPerc = 0;

    }

    txtFixedPercent.value = formatNum2(fPerc);

    txtVariablePercent.value = formatNum2(vPerc);

    txtTotalPercent.value = formatNum2(100);

}

function calc_loanAmounts(calc_type) {
// FIXED AND VARIABLE LOAN AMOUNTS
// Calculates the fixed and variable loan amounts based on the percentages
// Called from fixed and variable percent change events

    txtTotalPercent.value = formatNum2(100);

    // Update percentages
    switch (calc_type) {

        case 'fixed':

            if (txtFixedPercent.value <= 100) {
                //alert(formatNum2(txtFixedPercent.value));
                txtVariablePercent.value = formatNum2(100 - formatNum2(txtFixedPercent.value));

            }
            else {

                alert("Cannot have percent greater than 100");
                txtFixedPercent.value = "";
                txtVariablePercent.value = 100.00;

            }

            break;

        case 'variable':

            if (txtVariablePercent.value <= 100) {

                txtFixedPercent.value = formatNum2(100 - formatNum2(txtVariablePercent.value));

            }
            else {

                alert("Cannot have percent greater than 100");
                txtVariablePercent.value = "";
                txtFixedPercent.value = 100.00;

            }

            break;

        default:

            alert("error with calc_type in calc_loanamounts");

    }

    // Update the amounts
    // Can only calculate loan portions if total loan amount > 0
    if (txtTotalAmount.value > 0) {

        // calculate the fixed loan amount
        if (txtFixedPercent.value > 0) {

            var fAmt = parseFloat(txtTotalAmount.value) * parseFloat(txtFixedPercent.value) / 100;

        }
        else {

            var fAmt = 0;

        }

        // Use the total loan amount and the fixed amount
        var vAmt = parseFloat(txtTotalAmount.value) - formatNum2(fAmt);

        // Update the fixed amount
        if (fAmt == 0) {

            txtFixedAmount.value = 0;

        }
        else {

            txtFixedAmount.value = formatNum2(fAmt);

        }

        // Update the variable amount
        if (vAmt == 0) {

            txtVariableAmount.value = "";

        }
        else {

            txtVariableAmount.value = formatNum2(vAmt);

        }

    }
    else {

        txtFixedAmount.value = formatNum2(0);
        txtVariableAmount.value = formatNum2(0);

    }

}

function update_opener_percent() {
// Updates the percentages on the page that called the calculator
// Prospect and PreProspect called from same panel

    switch (window.parent.opener.name) {

        case "ProspectPanel":

            window.parent.opener.fixed_percent.value = txtFixedPercent.value;
            window.parent.opener.variable_percent.value = txtVariablePercent.value;

            break;

    }

}

function calc_rates() {
// FIXED AND VARIABLE INTEREST RATES

   if (ddlLinkRate.selectedIndex == -1) return 0;

    // Link rate
    var linkRate = ddlLinkRate.options(ddlLinkRate.selectedIndex).value;

    if (linkRate > 0) {

        if (iVarifixVersion == 1)
            ddlFixedMarketRateType.selectedIndex = 0;
        else
            ddlFixedMarketRateType.selectedIndex = 1;

        // Fixed portion interest rate
        var fixedMarketRate = ddlFixedMarketRateType.options(ddlFixedMarketRateType.selectedIndex).value;
        var fixedRate = parseFloat(fixedMarketRate) + parseFloat(linkRate);

        // Variable portion interest rate
        var variableMarketRate = ddlVariableMarketRateType.options(ddlVariableMarketRateType.selectedIndex).value;

        // Set the variable loan rate from the variable if it exists
        // eWorks specific modification

        if (parseFloat(fVariableLoanRate) > 0.0) {

            var variableRate = fVariableLoanRate;

        }
        else {

            var variableRate = parseFloat(variableMarketRate) + parseFloat(linkRate);

        }

        // Interest rate in total column for maximum loan is based on the variable portion
        var interestRate = variableRate

        // Display
        txtFixedRate.value = formatNum2(fixedRate*100);
        txtVariableRate.value = formatNum2(variableRate*100);
        txtInterestRate.value = formatNum2(interestRate*100);

    }
    else {

        txtFixedRate.value = formatNum2(0);
        txtVariableRate.value = formatNum2(0);

    }

}

function calc_LoanAmountDiff(calc_type) {
// Calculates the fixed or variable loan portion from the total loan amount

    switch(calc_type) {

        case "fixed":

            if (parseFloat(txtTotalAmount.value) > 0 && parseFloat(txtFixedAmount.value) <= parseFloat(txtTotalAmount.value)) {
                txtVariableAmount.value = formatNum2(parseFloat(txtTotalAmount.value) - parseFloat(txtFixedAmount.value));
            }

            break;

        case "variable":

            if (parseFloat(txtTotalAmount.value) > 0 && parseFloat(txtVariableAmount.value) <= parseFloat(txtTotalAmount.value)) {
                txtFixedAmount.value = formatNum2(parseFloat(txtTotalAmount.value) - parseFloat(txtVariableAmount.value));
            }

            break;

    }

}

function calc_InstalmentDiff(calc_type) {
// Calculates the fixed or variable instalment from the total instalment
// Only used when calculating loan amount

    if (radio_loan_amount.checked == true) {
        switch(calc_type) {

            case "fixed":

                if (txtTotalInstalment.value > 0 && parseFloat(txtFixedInstalment.value) <= parseFloat(txtTotalInstalment.value)) {
                    txtVariableInstalment.value = formatNum2(parseFloat(txtTotalInstalment.value) - parseFloat(txtFixedInstalment.value));
                }

                break;

            case "variable":

                if (txtTotalInstalment.value > 0 && parseFloat(txtVariableInstalment.value) <= parseFloat(txtTotalInstalment.value)) {
                    txtFixedInstalment.value = formatNum2(parseFloat(txtTotalInstalment.value) - parseFloat(txtVariableInstalment.value));
                }

                break;

        }
    }

}

function calc_AddInstalmentDiff(calc_type) {
// Calculates the fixed or variable instalment from the total instalment
// Only used when calculating loan amount

    if (radio_extra_payments.checked == true) {
        switch(calc_type) {

            case "fixed":

                if (txtTotalAddInstalment.value > 0 && parseFloat(txtFixedAddInstalment.value) <= parseFloat(txtTotalAddInstalment.value)) {
                    txtVariableAddInstalment.value = parseFloat(txtTotalAddInstalment.value) - parseFloat(txtFixedAddInstalment.value);
                }

                if (txtFixedAddInstalment.value == "") {

                    txtVariableAddInstalment.value = parseFloat(txtTotalAddInstalment.value)

                }

                break;

            case "variable":

                if (txtTotalAddInstalment.value > 0 && parseFloat(txtVariableAddInstalment.value) <= parseFloat(txtTotalAddInstalment.value)) {
                    txtFixedAddInstalment.value = parseFloat(txtTotalAddInstalment.value) - parseFloat(txtVariableAddInstalment.value);
                    if (txtFixedAddInstalment.value == 0) txtFixedAddInstalment.value = "";
                }

                if (txtVariableAddInstalment.value == "") {

                    txtFixedAddInstalment.value = parseFloat(txtTotalAddInstalment.value)

                }
                break;

        }

    }

}

// ---------------------- END CALC FUNCTIONS ---------------------------------------------------------------------------------------------------------

// ---------------------- SUM FUNCTIONS --------------------------------------------------------------------------------------------------------------

function sum_percentages() {
// PERCENTAGES

    var sumX = parseFloat(txtFixedPercent.value) + parseFloat(txtVariablePercent.value);

    txtTotalPercent.value = formatNum2(sumX);

}

function sum_loanAmounts() {
// LOAN AMOUNTS

    if (txtFixedAmount.value == "" ) {

        var fFixedAmount = 0;
    }
    else {

        var fFixedAmount = txtFixedAmount.value;

    }

    if (txtVariableAmount.value == "" ) {

        var fVariableAmount = 0;

    }
    else {

        var fVariableAmount = txtVariableAmount.value;

    }

    var sumX = parseFloat(fFixedAmount) + parseFloat(fVariableAmount);

    txtTotalAmount.value = formatNum2(sumX);

}

function sum_instalments() {
// INSTALMENTS

    var sumX = parseFloat((txtFixedInstalment.value == "") ? 0 : txtFixedInstalment.value) + parseFloat((txtVariableInstalment.value == "") ? 0 : txtVariableInstalment.value);

    txtTotalInstalment.value = formatNum2(sumX);
}

function sum_addInstalments() {
// ADDITIONAL INSTALMENTS

    var sumX = parseFloat((txtFixedAddInstalment.value == "") ? 0 : txtFixedAddInstalment.value) + parseFloat((txtVariableAddInstalment.value == "") ? 0 : txtVariableAddInstalment.value);

    txtTotalInstalment.value = formatNum2(sumX);
}

function sum_interestPaid() {
// INTEREST PAID

    if (txtFixedInterestPaid.value == "") {

        var fFixedInterestPaid = 0;

    }
    else {

        var fFixedInterestPaid = txtFixedInterestPaid.value;
    }

    if (txtVariableInterestPaid.value == "") {

        var fVariableInterestPaid = 0;
    }
    else {

        fVariableInterestPaid = txtVariableInterestPaid.value;

    }

    var sumX = parseFloat(fFixedInterestPaid) + parseFloat(fVariableInterestPaid);

    txtTotalInterestPaid.value = formatNum2(sumX);

}

function sum_savings() {
// SAVINGS

    var sumX = parseFloat(txtFixedSavings.value) + parseFloat(txtVariableSavings.value);

    txtTotalSavings.value = formatNum2(sumX);

}

// ---------------------- END SUM FUNCTIONS ---------------------------------------------------------------------------------------------------------

// ---------------------- GENERAL FUNCTIONS ---------------------------------------------------------------------------------------------------------

function update_amounts(amount_type) {
// Updates Purchase price, total loan amount or deposit depending on the selected calculator

    if (txtFixedAmount.value == "" ) {
        fFixedAmount = 0;
    }
    else {
        fFixedAmount = txtFixedAmount.value;
    }

    if (txtVariableAmount.value == "") {
        fVariableAmount = 0;
    }
    else {
        fVariableAmount = txtVariableAmount.value;
    }

    if (txtTotalAmount.value == "") {
        fTotalAmount = 0;
    }
    else {
        fTotalAmount = txtTotalAmount.value;
    }

    if (txtPurchasePrice.value == "") {
        fPurchasePrice = 0;
    }
    else {
        fPurchasePrice = txtPurchasePrice.value;
    }

    if (txtDeposit.value == "") {
        fDeposit = 0;
    }
    else {
        if (loanPurpose == 2 || loanPurpose == 4) {
            fDeposit = txtDeposit.value*-1;
        }
        else {
            fDeposit = txtDeposit.value;
        }
    }

    if (radio_monthly_instalment.checked == true) {
    // Update loan amount or purchase amount
        // jason: removed for term change not updating bug. clears instalment.
        //reset_fields("monthly_instalment");

        switch(amount_type) {

            case "deposit":

                // Loan amount
                if (parseFloat(fDeposit) <= parseFloat(fPurchasePrice)) {

                    txtTotalAmount.value = parseFloat(fPurchasePrice) - parseFloat(fDeposit);

                    // was removed because when tab from var amt to term both fixed and var == to total amount
                    // jason: removed: type in dep then var = total and fixed has value
                    //txtVariableAmount.value = parseFloat(fPurchasePrice) - parseFloat(fDeposit);

                    if (txtTotalAmount.value == 0) txtTotalAmount.value = "";
                    if (txtVariableAmount.value == 0) txtVariableAmount.value = "";

                    // jason: removed for the ddlpurpose event. re calc of loan amount depeding on loan purpose
                    //if (txtDeposit.value > 0) txtFixedAmount.value = "";

                    // jason: changed from variable. tabbing from pp to dep. monthly instalment.
                    //calc_percentages("fixed");
                    // jason: removed above added below: type in dep then var = total and fixed has value
                    calc_loanAmounts("fixed");

                }

                break;

            case "purchase":

                // Loan amount
                if (parseFloat(fDeposit) <= parseFloat(fPurchasePrice)) {

                    txtTotalAmount.value = parseFloat(fPurchasePrice) - parseFloat(fDeposit);

                    // jason: taken out to stop reset if pre populated figures that are present.
                    // jason: want to keep the percent split

                    // txtVariableAmount.value = parseFloat(fPurchasePrice) - parseFloat(fDeposit);

                    //txtFixedAmount.value = "";

                   // jason: put back in for same as above
                   calc_loanAmounts("fixed");

                    // jason: removed for same as above
                    //calc_percentages("variable");

                }
                else {

                    txtDeposit.value = "";
                    txtTotalAmount.value = txtPurchasePrice.value;

                }

                break;
            case "loan":

                // Purchase price
                if (parseFloat(fTotalAmount) > 0) {

                    txtPurchasePrice.value = parseFloat(fTotalAmount) + parseFloat(fDeposit);

                }

                break;

            case "export":
            //alert("pp: " + parseFloat(fPurchasePrice) + " ta: " + parseFloat(fTotalAmount) + " d: " + parseFloat(fDeposit));
                                                                                                // Removed parseFloat(fPurchasePrice) > 0 due to conflicting bugs between teleleadmanage and manageprospect
                if (parseFloat(fDeposit) <= parseFloat(fPurchasePrice) && parseFloat(fDeposit) != 0) {

                    txtTotalAmount.value = parseFloat(fPurchasePrice) - parseFloat(fDeposit);

                }
                else if (parseFloat(fTotalAmount) > 0 && parseFloat(fPurchasePrice) == 0 && parseFloat(fDeposit) == 0){

                    txtTotalAmount.value = parseFloat(fTotalAmount);

                }
                else {
                    // jason: added due to export of pp only then nothing goes into loan amount.
                    txtTotalAmount.value = parseFloat(fPurchasePrice)

                }

                if (txtTotalAmount.value == 0) txtTotalAmount.value = "";

        }
    }
    else if (radio_loan_amount.checked == true) {
        //alert(parseFloat(fPurchasePrice) + " " + parseFloat(fTotalAmount));
        switch(amount_type) {

            case "deposit":
                // Purchase price
                if (parseFloat(fTotalAmount) >= 0) {

                    if (ddlLoanPurpose.value == 4) {
                        txtDeposit.value = parseFloat(fTotalAmount)
                        txtPurchasePrice.value = 0;
                    }
                    else
                        txtPurchasePrice.value = parseFloat(fTotalAmount) + parseFloat(fDeposit);

                }
                break;

            case "purchase":

                if (parseFloat(fPurchasePrice) >= parseFloat(fTotalAmount)) {

                    txtDeposit.value = formatNum2(parseFloat(fPurchasePrice) - parseFloat(fTotalAmount));

                }

                break;

            case "loan":
                // Purchase price
                if (parseFloat(fTotalAmount) > 0) {

                    txtPurchasePrice.value = parseFloat(fTotalAmount) + parseFloat(fDeposit);

                }
                break;

        }
    }

    else if (radio_extra_payments.checked == true) {

        switch(amount_type) {

            case "deposit":

                //
                if (parseFloat(fDeposit) <= parseFloat(fPurchasePrice)) {

                    txtTotalAmount.value = parseFloat(fPurchasePrice) - parseFloat(fDeposit);

                    txtFixedAmount.value = formatNum2(parseFloat(txtTotalAmount.value)*txtFixedPercent.value/100);
                    txtVariableAmount.value = formatNum2(txtTotalAmount.value - txtFixedAmount.value);

                }

                break;
            case "purchase":
                // Loan amount
                if (parseFloat(fDeposit) <= parseFloat(fPurchasePrice)) {

                    txtTotalAmount.value = parseFloat(fPurchasePrice) - parseFloat(fDeposit);

                    calc_loanAmounts("fixed");

                }
                else {

                    txtDeposit.value = "";
                    txtTotalAmount.value = txtPurchasePrice.value;

                }
            ;
                break;
            case "loan":
                // Purchase price
                if (parseFloat(fTotalAmount) > 0) {

                    txtPurchasePrice.value = parseFloat(fTotalAmount) + parseFloat(fDeposit);

                }

            ;
                break;
        }
    }
    else if (radio_maximum_loan.checked == true) {

        switch(amount_type) {

            case "deposit":

                if (parseFloat(fTotalAmount) > 0 && parseFloat(fDeposit) > 0) {

                    txtPurchasePrice.value = parseFloat(fDeposit) + parseFloat(fTotalAmount);

                }
                else {

                    txtPurchasePrice.value = "";

                }

                break;

            case "purchase":

                if (parseFloat(fPurchasePrice) > parseFloat(fTotalAmount)) {

                    txtDeposit.value = parseFloat(fPurchasePrice) - parseFloat(fTotalAmount);
                }

                break;
            }

    }
    else if (radio_minimum_deposit.checked == true) {
//alert(parseFloat(fPurchasePrice) + " " + parseFloat(fTotalAmount));
        switch(amount_type) {

            case "deposit":
                if (parseFloat(fDeposit) >= 0) {

                    txtPurchasePrice.value = parseFloat(fDeposit) + parseFloat(fTotalAmount);

                }

                // jason: added for ddlpurpose change.
                if (txtPurchasePrice.value == 0)  txtPurchasePrice.value = "";

                break;

            case "purchase":
                if (parseFloat(fPurchasePrice) >= 0) {

                    txtTotalAmount.value = parseFloat(fPurchasePrice);

                }
                break;

        }

    }

}

function getRates() {

    // Create XMLHTTP object
	var xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");

	// Open connection to remote server ?
	xmlhttp.open("GET", "TeleCalcRates.asp", false);

	var objDOM  = new ActiveXObject("Microsoft.XMLDOM");
	var objNode = objDOM.createElement("Loan");
	objDOM.appendChild(objNode);

	var xmlText = objDOM.xml;

	var xmldom = new ActiveXObject("Microsoft.XMLDOM");
	xmldom.loadXML(xmlText);

	// Send request and get data
	xmlhttp.send(xmldom);

	var doc = xmldso1.XMLDocument;

	var sStart = xmlhttp.responseText.indexOf("<root>");
	var sEnd = xmlhttp.responseText.indexOf("</root>");

	doc.loadXML(xmlhttp.responseText.substring( sStart,sEnd+8));

	JIBARRate = NumberToString(xmldso1.recordset.fields(0).value,2);
    SAHLRate = NumberToString(xmldso1.recordset.fields(1).value,2);
    SAHLFixedRate = NumberToString(xmldso1.recordset.fields(3).value,2);

}

function envokeRS(sURL, frame_no) {

    frames(frame_no).location.href = sURL

}

function def_tabArray(PurchasePrice,Deposit,FixedAmount,VarAmt,TotAmt,Term,LinkRate,FixRate,VarRate,FixIns,VarIns,TotIns,FixAddIns,VarAddIns,TotAddIns,Inc,PropVal,FixSav,VarSav,TotSav,FixedPerc,VarPerc) {
// Basically used to line up values with fields ... easy reading ...

    return Array(PurchasePrice,Deposit,FixedAmount,VarAmt,TotAmt,Term,LinkRate,FixRate,VarRate,FixIns,VarIns,TotIns,FixAddIns,VarAddIns,TotAddIns,Inc,PropVal,FixSav,VarSav,TotSav,FixedPerc,VarPerc)

}

function set_tabIndex(calc_type) {
// Sets the tabIndex depending on the selected calculator
// jason: removed all focus events for new validate(1) idea.

    // Default
    var arrTabIndex = def_tabArray(1,2,4,7,3,5,6,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22);

    switch(calc_type) {

        case "monthly_instalment":

            if (parseFloat(txtTotalAmount.value) > 0) {
                txtFixedPercent.focus()
            }
            else
                txtTotalAmount.focus();

            var arrTabIndex = def_tabArray(1,2,4,17,3,18,6,9,10,11,12,13,14,15,16,7,8,18,20,21,5,22);

            break;

        case "loan_amount":

            var arrTabIndex = def_tabArray(7,6,9,10,11,8,3,12,13,2,14,1,15,16,17,4,5,18,19,20,21,22);

            break;

        case "extra_payments":

            var arrTabIndex = def_tabArray(11,8,2,10,1,9,4,12,13,14,15,16,6,17,5,7,8,18,21,20,3,22);

        /*  jason: new focus idea validate(1)
            if (txtTotalAmount.value == 0 || txtTotalAmount.value == "")
                txtTotalAmount.focus();
            else if (txtFixedAmount.value == 0 || txtFixedAmount.value == "")
                txtFixedAmount.focus();
            else if (txtTotalAddInstalment.value == 0 || txtTotalAddInstalment.value == "")
                txtTotalAddInstalment.focus()
            else
                txtTotalAmount.focus();
                */

            break;
        case "maximum_loan":

            var arrTabIndex = def_tabArray(5,4,17,7,8,6,2,9,10,11,12,13,14,15,16,1,3,18,19,20);

            break;
        case "minimum_deposit":

            var arrTabIndex = def_tabArray(1,16,4,8,17,8,3,9,21,10,11,12,13,14,15,6,7,18,19,20,5,22);

            break;

    }

    // Price
    txtPurchasePrice.tabIndex = arrTabIndex[0];
    txtDeposit.tabIndex = arrTabIndex[1];

    // Loan Amount
    txtFixedAmount.tabIndex = arrTabIndex[2];
    txtVariableAmount.tabIndex = arrTabIndex[3];
    txtTotalAmount.tabIndex = arrTabIndex[4];

    txtTerm.tabIndex = arrTabIndex[5];

    ddlLinkRate.tabIndex = arrTabIndex[6];

    // Rate
    txtFixedRate.tabIndex = arrTabIndex[7];
    txtVariableRate.tabIndex = arrTabIndex[8];

    // Instalment
    txtFixedInstalment.tabIndex = arrTabIndex[9];
    txtVariableInstalment.tabIndex = arrTabIndex[10];
    txtTotalInstalment.tabIndex = arrTabIndex[11];

    // Additional Instalment
    txtFixedAddInstalment.tabIndex = arrTabIndex[12];
    txtVariableAddInstalment.tabIndex = arrTabIndex[13];
    txtTotalAddInstalment.tabIndex = arrTabIndex[14];

    txtIncome.tabIndex = arrTabIndex[15];
    txtPropertyValue.tabIndex = arrTabIndex[16];

    // Savings
    txtFixedSavings.tabIndex = arrTabIndex[17];
    txtVariableSavings.tabIndex = arrTabIndex[18];
    txtTotalSavings.tabIndex = arrTabIndex[19];

    // % split
    txtFixedPercent.tabIndex = arrTabIndex[20];
    txtVariablePercent.tabIndex = arrTabIndex[21];

}

function reset_fields(calc_type) {
// Default values for fields

    switch(calc_type) {

        case "monthly_instalment":

            // Instalment
            txtFixedInstalment.value = "";
            txtVariableInstalment.value = "";
            txtTotalInstalment.value = "";

            // Interest Paid
            txtFixedInterestPaid.value = "";
            txtVariableInterestPaid.value = "";
            txtTotalInterestPaid.value = "";

            break;

        case "loan_amount":

            // Total col
            txtPurchasePrice.value = "";
            txtDeposit.value = "";

            // % Split
            txtFixedPercent.value = formatNum2(0);
            txtVariablePercent.value = formatNum2(100);
            txtTotalPercent.value = formatNum2(100);

            // Loan Amount
            txtFixedAmount.value = "";
            txtVariableAmount.value = "";
            txtTotalAmount.value = "";

            // Interest Paid
            txtFixedInterestPaid.value = "";
            txtVariableInterestPaid.value = "";
            txtTotalInterestPaid.value = "";

            break;

        case "extra_payments":

            // % Split
          //  txtFixedPercent.value = formatNum2(0);
           // txtVariablePercent.value = formatNum2(100);
           // txtTotalPercent.value = formatNum2(100);

            // Term
            txtTerm.value = 240;

            // Interest Rate
            txtFixedRate.value = "";
            txtVariableRate.value = "";

            // Instalment
            txtFixedInstalment.value = "";
            txtVariableInstalment.value = "";
            txtTotalInstalment.value = "";

            // Interest Paid
            txtFixedInterestPaid.value = "";
            txtVariableInterestPaid.value = "";
            txtTotalInterestPaid.value = "";

            // Savings
            txtFixedSavings.value = "";
            txtVariableSavings.value = "";
            txtTotalSavings.value = "";

            break;

        case "maximum_loan":

            txtPurchasePrice.value = "";
            txtDeposit.value = "";

            // % Split
           // txtFixedPercent.value = formatNum2(0);
           // txtVariablePercent.value = formatNum2(100);
           // txtTotalPercent.value = formatNum2(100);

            // Loan Amount
            txtFixedAmount.value = "";
            txtVariableAmount.value = "";
            txtTotalAmount.value = "";

            // Term
            txtTerm.value = 240;

            // Interest Rate
            txtFixedRate.value = "";
            txtVariableRate.value = "";

            // Instalment
            txtFixedInstalment.value = "";
            txtVariableInstalment.value = "";
            txtTotalInstalment.value = "";

            // Interest Paid
            txtFixedInterestPaid.value = "";
            txtVariableInterestPaid.value = "";
            txtTotalInterestPaid.value = "";

            // Savings
            txtFixedSavings.value = "";
            txtVariableSavings.value = "";
            txtTotalSavings.value = "";
            break;

        case "minimum_deposit":

            // % Split
            txtFixedPercent.value = formatNum2(0);
            txtVariablePercent.value = formatNum2(100);
            txtTotalPercent.value = formatNum2(100);

            // Loan Amount
            txtFixedAmount.value = "";
            txtVariableAmount.value = "";
            txtTotalAmount.value = "";

            // Term
            txtTerm.value = 240;

            // Interest Rate
            txtFixedRate.value = "";
            txtVariableRate.value = "";

            // Instalment
            txtFixedInstalment.value = "";
            txtVariableInstalment.value = "";
            txtTotalInstalment.value = "";

            // Misc
            txtDeposit.value = "";

            // Interest Paid
            txtFixedInterestPaid.value = "";
            txtVariableInterestPaid.value = "";
            txtTotalInterestPaid.value = "";

            // Savings
            txtFixedSavings.value = "";
            txtVariableSavings.value = "";
            txtTotalSavings.value = "";
            break;

        case "link_rate":

            // % Split
            txtFixedPercent.value = formatNum2(0);
            txtVariablePercent.value = formatNum2(100);
            txtTotalPercent.value = formatNum2(100);

            // Interest Rate
            txtFixedRate.value = "";
            txtVariableRate.value = "";

            // Instalment
            txtFixedInstalment.value = "";
            txtVariableInstalment.value = "";
            txtTotalInstalment.value = "";

            // Interest Paid
            txtFixedInterestPaid.value = "";
            txtVariableInterestPaid.value = "";
            txtTotalInterestPaid.value = "";

            break;

        case "all":

            // % Split
            txtFixedPercent.value = formatNum2(0);
            txtVariablePercent.value = formatNum2(100);
            txtTotalPercent.value = formatNum2(100);

            // Loan Amount
            txtFixedAmount.value = "";
            txtVariableAmount.value = "";
            txtTotalAmount.value = "";

            // Term
            txtTerm.value = 240;

            // Interest Rate
            txtFixedRate.value = "";
            txtVariableRate.value = "";

            // Instalment
            txtFixedInstalment.value = "";
            txtVariableInstalment.value = "";
            txtTotalInstalment.value = "";

            // Interest Paid
            txtFixedInterestPaid.value = "";
            txtVariableInterestPaid.value = "";
            txtTotalInterestPaid.value = "";

            // Savings
            txtFixedSavings.value = "";
            txtVariableSavings.value = "";
            txtTotalSavings.value = "";
            break;

    }

            td_info.innerHTML = "";
            td_PTI.innerHTML = "";
            td_LTV.innerHTML = "";
            td_PTI_Core.innerHTML = "";

            calc_rates();

            txtTotalInstalment.style.fontWeight = "normal"
            txtTotalInstalment.style.color = "black"
            txtTotalAmount.style.fontWeight = "normal"
            txtTotalAmount.style.color = "black"
            txtDeposit.style.fontWeight = "normal";
            txtDeposit.style.color = "black";
            txtNewTerm.style.fontWeight = "normal";
            txtNewTerm.style.color = "black";
            txtFixedSavings.style.fontWeight = "normal";
            txtFixedSavings.style.color = "black";
            txtVariableSavings.style.fontWeight = "normal";
            txtVariableSavings.style.color = "black";
            txtTotalSavings.style.fontWeight = "normal";
            txtTotalSavings.style.color = "black";

}

function update_eWorks() {
// Updates information relating to the helpdesk eWorks system.
// Setting variable and fixed percents.

    var com = new ActiveXObject("ADODB.Command");
	var con = new ActiveXObject("ADODB.Connection");
	var eworkFixPC = 0.0;

    sDSN = "Provider=SQLOLEDB;Data Source=<%=DB_SERVER%>; Initial Catalog=<%=DB_EWORK%>; User Id=<%=DB_EWORK_USERID%>"

    con.ConnectionString = sDSN;

    con.Open;

	com.ActiveConnection = con;

	com.CommandType = 1; //adCmdText

	eworkFixPC = formatNum2((txtFixedPercent.value == "") ? 0 : txtFixedPercent.value);

    var sSql = "Update HelpDesk Set FlexiFixedPC = " + formatNum2(txtFixedPercent.value)
                + ", FlexiVariablePC = " + formatNum2(txtVariablePercent.value)
                + ", LoanFixedInstallment = " + formatNum2((txtFixedInstalment.value == "") ? 0 : txtFixedInstalment.value)
                + ", LoanVariableInstallment = " + formatNum2((txtVariableInstalment.value == "") ? 0 : txtVariableInstalment.value)
                + ", FlexiFixedRate = " + formatNum2((txtFixedRate.value == "") ? 0 : txtFixedRate.value)
                + ", FlexiVariableRate = " + formatNum2((txtVariableRate.value == "") ? 0 : txtVariableRate.value)
        		+ ", UserConfirm = " + ((eworkFixPC == 0) ? "''" : "UserToDo")
                + " Where EFOLDERID = '" + eFolderId + "'";

	com.CommandText = sSql;

    com.Execute;

}

// ---------------------- END GENERAL FUNCTIONS --------------------------------------------------------------------------------------------

// --------------- CREDIT MATRIX FUNCTIONS -------------------------------------------------------------------------------------------------------------------------

function FindMaxFixed(loanAmount, fixedRate, variableRate, term, income, cmPTI){

    // cmPTI - Credit matrix PTI
    // Calculate instalment for 100 % fixed
    var fixedInstalment = CalculateInstallment(loanAmount, fixedRate, 12, term, 0);

    // Calculate instalment for 100% variable
    var varInstalment = CalculateInstallment(loanAmount, variableRate, 12, term, 0);

    // Calculate PTI for 100 % fixed
    var fPTI = formatNum2(fixedInstalment/income*100);

    // Calculate PTI for 100% vairable
    var vPTI = formatNum2(varInstalment/income*100);

    if (parseFloat(vPTI) > parseFloat(cmPTI)) {

        td_max_fixed_amount.innerHTML = "0";
        td_max_fixed_percent.innerHTML = "0";
        return 0;

    }

    switch (ddlEmploymentType.value) {

        case "1":     // Salaried
            var maxPTI = parseFloat(cmPTI) * (1 + parseFloat(cmMaxPTIMarginSalaried));
            break;

        case "2":     // Self Employed
            var maxPTI = parseFloat(cmPTI) * (1 + parseFloat(cmMaxPTIMarginSelfEmployed));
            break;

        case "3":     // Subsidised
            var maxPTI = parseFloat(cmPTI) * (1 + parseFloat(cmMaxPTIMarginSubsidised));
            break;

    }

    if (parseFloat(fPTI) <= parseFloat(maxPTI)) {
        // if PTI for 100% fixed is less than
        td_max_fixed_amount.innerHTML = loanAmount;
        td_max_fixed_percent.innerHTML = "100";

    }
    else {

        FindMaxRecursive(loanAmount, 0, loanAmount, loanAmount, fixedRate, variableRate, maxPTI, term, income);

    }

}

function FindMaxRecursive(varAmount, fixedAmount, total, interval, fixedRate, varRate, maxPTI, term, income){
// Finds the fixed instalment amount for which the combined fixed and variable PTI is equal to the max PTI

    var PTI;
    var fixedInstalment;
    var varInstalment;
    var fAmount;
    var vAmount;

    interval = interval / 2;

    if (fixedAmount <= 0) {

        fixedInstalment = 0;

    }
    else {

        fixedInstalment = CalculateInstallment(fixedAmount, fixedRate, 12, term, 0);

    }

    varInstalment = CalculateInstallment(varAmount, varRate, 12, term, 0);

    PTI = formatNum2(((parseFloat(fixedInstalment) + parseFloat(varInstalment)) / income)*100); // as percent 25.23 %

    // quit if equal
    if ((parseFloat(PTI) >= (parseFloat(maxPTI) - 0.001)) && (parseFloat(PTI) <= (parseFloat(maxPTI) + 0.001)))
    {

        td_max_fixed_amount.innerHTML = formatNum2(fixedAmount);
        td_max_fixed_percent.innerHTML = formatNum2(fixedAmount / total * 100);
        return 0;

    }

    if (parseFloat(PTI) < parseFloat(maxPTI)){

        fAmount = fixedAmount + interval;
        vAmount = total - fAmount;

    }
    else {

        fAmount = fixedAmount - interval;
        vAmount = total - fAmount;

    }

    FindMaxRecursive(vAmount, fAmount, total, interval, fixedRate, varRate, maxPTI, term, income);

}

function getCreditMatrixRS() {
// Populates a disconnected recordset with data from the credit matrix table
// rsCreditMatrix recordset is used for the credit matrix decisions

    var oConn = new ActiveXObject("ADODB.Connection");
    var oRS = new ActiveXObject("ADODB.Recordset");
    var jsNothing = GetNothing();

    var sDSN = "Provider=SQLOLEDB.1; Application Name='MLS System Version1 [TeleCalculatorFlexi.asp_cm]';Data Source=<%=Session("SQLDatabase")%>;uid=<%= Session("UserID")%>" ;

    oConn.Open(sDSN);

    iSPVNumber = "<%=iSPVNumber %>";

	cmPTIType = 1;
    var sSQL = "sahldb.dbo.c_GetCreditMatrixDetails " + cmPTIType + ", " + "<%=iSPVNumber %>";

    oRS.CursorLocation = 3;    // Client-side
    oRS.CursorType = 2;        // Static
    oRS.LockType = 1;          // ReadOnly

    // Retrieve the recordset
    oRS.Open(sSQL, oConn, 3);

    if (oRS.EOF) {

        alert("Could not retrieve credit matrix details");
        return 0;
    }

    rsCreditMatrix.CursorLocation = 3;
    rsCreditMatrix.CursorType = 2;
    rsCreditMatrix.LockType = 1;

    // Using vb script to disconnect recordset, could not do with null
    oRS.ActiveConnection = jsNothing;

    // Creates a copy of the recordset
    rsCreditMatrix = oRS.Clone();

    cmrsCreditMatrixOpen = true;

    oRS.Close;

    oConn.Close;

}

function clear_creditMatrixDetails() {
// Clears the PTI, LTV, Max Fixed, Qualifies

    update_qualify(false,"");
    td_PTI.innerHTML = "";
    td_LTV.innerHTML = "";
    td_max_fixed_amount.innerHTML = "";
    td_max_fixed_percent.innerHTML = "";
    td_info.innerHTML = "";

}

function update_PTI(var_pti, ff_pti) {
// Sets the display on the page

	if (ff_pti != "")
		td_PTI.innerHTML = formatNum2(ff_pti*100);

	if (var_pti != "")
		td_PTI_Core.innerHTML = formatNum2(var_pti*100);

}

function update_LTV(val) {
// Sets the display on the page

    td_LTV.innerHTML = formatNum2(val*100);

}

function error_creditMatrixRS() {
// Displays a critical error

    alert("Error with the credit matrix recordset. Contact administrator!");

}

function isLoanAmountValid() {
// Checks that the loan amount is within the bounds of allow loan values
// Returns bool

    var empType = ddlEmploymentType.value;
    var purpose = ddlLoanPurpose.value;
    var loanAmount = txtTotalAmount.value;
    var loanAmountOk = false;
    var loanInterval = 0;

    rsCreditMatrix.MoveFirst;

    if (!rsCreditMatrix.EOF) {

        do {
            // Check Employment Type, Loan Purpose
            if (rsCreditMatrix.Fields(0).Value == empType && rsCreditMatrix.Fields(1).Value == purpose) {

                // Check loan amount
                if (parseFloat(loanAmount) >= parseFloat(rsCreditMatrix.Fields(8).Value)) {
                    loanAmountOk = true;
                    break;
                }

            }

            rsCreditMatrix.MoveNext;

        } while (!rsCreditMatrix.EOF);

        // Check lower bound
        if (loanAmountOk == false) {
            // Below allow value
            return false;

        }

        // Lower bound ok. Find upper bound
        rsCreditMatrix.MoveFirst;

        if (!rsCreditMatrix.EOF) {

            do {
                // Check Employment Type, Loan Purpose
                if (rsCreditMatrix.Fields(0).Value == empType && rsCreditMatrix.Fields(1).Value == purpose) {

                    // Check loan amount
                    if (parseFloat(loanInterval) < parseFloat(rsCreditMatrix.Fields(2).Value)) {
                        // higher inteval
                        loanInterval = parseFloat(rsCreditMatrix.Fields(2).Value);

                    }

                }

                rsCreditMatrix.MoveNext;

            } while (!rsCreditMatrix.EOF);

        } // (!rsCreditMatrix.EOF)

        if (parseFloat(loanAmount) <= parseFloat(loanInterval)) {

            // Loan amount ok
            return true;

        }

        cmMaxLoanAmount = loanInterval;
        // Higher than allowed loan value
        return false;

    }

    error_creditMatrixRS();
    return false;

}

function isPropertyValueValid() {
// Checks if the property value is within the correct bounds
// Returns bool

    var empType = ddlEmploymentType.value;
    var purpose = ddlLoanPurpose.value;
    var propertyValue = txtPropertyValue.value;

    rsCreditMatrix.MoveFirst;

    if (!rsCreditMatrix.EOF) {

        do {

            if (rsCreditMatrix.Fields(0).Value == empType
                    && rsCreditMatrix.Fields(1).Value == purpose
                    && rsCreditMatrix.Fields(6).Value == ddlLinkRate.value) {

                // Check property value within bounds
                if (parseFloat(propertyValue) >= parseFloat(rsCreditMatrix.Fields(9).Value)) {

                    if (parseFloat(propertyValue) <= parseFloat(rsCreditMatrix.Fields(10).Value)) {
                        // Property value ok
                        return true;

                    }
                    else {

                        cmMaxPropertyValue = rsCreditMatrix.Fields(10).Value

                    }

                }
                else {

                    cmMinPropertyValue = rsCreditMatrix.Fields(9).Value;
                }

            }

            rsCreditMatrix.MoveNext;

        } while (!rsCreditMatrix.EOF);

            return false;
    }

    error_creditMatrixRS();
    return false;

}

function isIncomeValid() {
// Checks if the income is within the correct bounds
// Returns bool

    var empType = ddlEmploymentType.value;
    var purpose = ddlLoanPurpose.value;
    var linkRate = ddlLinkRate.value;
    var income = txtIncome.value;

    rsCreditMatrix.MoveFirst

    if (!rsCreditMatrix.EOF) {

        do {

            if (rsCreditMatrix.Fields(0).Value == empType
                    && rsCreditMatrix.Fields(1).Value == purpose
                    && rsCreditMatrix.Fields(6).Value == linkRate) {

                // Check income greater than min value
                if (parseFloat(income) >= parseFloat(rsCreditMatrix.Fields(7).Value)) {

                    // Income ok
                    return true;

                }

            }

            rsCreditMatrix.MoveNext;

        } while (!rsCreditMatrix.EOF);

        // Income not ok
        return false;

    }

    error_creditMatrixRS();
    return false;

}

function getLowestMaxLoan() {
// Finds lowest max loan value
// Returns the value

    var empType = ddlEmploymentType.value;
    var purpose = ddlLoanPurpose.value;
    var lowestValue = 100000000;

    rsCreditMatrix.MoveFirst;

    if (!rsCreditMatrix.EOF) {

        do {
            // Check Employment Type, Loan Purpose
            if (rsCreditMatrix.Fields(0).Value == empType && rsCreditMatrix.Fields(1).Value == purpose) {

                // Check loan amount
                if (parseFloat(lowestValue) > parseFloat(rsCreditMatrix.Fields(2).Value)) {
                    // higher inteval
                    lowestValue = parseFloat(rsCreditMatrix.Fields(2).Value);

                }

            }

            rsCreditMatrix.MoveNext;

        } while (!rsCreditMatrix.EOF);

        if (parseFloat(lowestValue) > 0 && parseFloat(lowestValue) < 100000000) {

            return lowestValue
        }

    } // (!rsCreditMatrix.EOF)

    error_creditMatrixRS();
    return false;
}

function getLowestCategory(loanAmount) {
// Finds lowest max loan value
// Returns link rate

    var empType = ddlEmploymentType.value;
    var purpose = ddlLoanPurpose.value;
    var lowestCat = 10;

    rsCreditMatrix.MoveFirst;

    if (!rsCreditMatrix.EOF) {

        do {
            // Check Employment Type, Loan Purpose, loan amount
            if (rsCreditMatrix.Fields(0).Value == empType
                && rsCreditMatrix.Fields(1).Value == purpose
                && rsCreditMatrix.Fields(2).Value == loanAmount) {

                // Check link rate
                if (parseFloat(lowestCat) > parseFloat(rsCreditMatrix.Fields(6).Value)) {
                    // higher inteval
                    lowestCat = parseFloat(rsCreditMatrix.Fields(6).Value);

                }

            }

            rsCreditMatrix.MoveNext;

        } while (!rsCreditMatrix.EOF);

        if (parseFloat(lowestCat) > 0 && parseFloat(lowestCat) < 10) {

            return lowestCat
        }

    } // (!rsCreditMatrix.EOF)

    error_creditMatrixRS();
    return false;
}

function getHighestCategory(loanAmount) {
// Finds lowest max loan value
// Returns link rate

    var empType = ddlEmploymentType.value;
    var purpose = ddlLoanPurpose.value;
    var lowestCat = 0;

    rsCreditMatrix.MoveFirst;

    if (!rsCreditMatrix.EOF) {

        do {
            // Check Employment Type, Loan Purpose, loan amount
            if (rsCreditMatrix.Fields(0).Value == empType
                && rsCreditMatrix.Fields(1).Value == purpose
                && rsCreditMatrix.Fields(2).Value == loanAmount) {

                // Check link rate
                if (parseFloat(lowestCat) < parseFloat(rsCreditMatrix.Fields(6).Value)) {
                    // higher inteval
                    lowestCat = parseFloat(rsCreditMatrix.Fields(6).Value);

                }

            }

            rsCreditMatrix.MoveNext;

        } while (!rsCreditMatrix.EOF);

        if (parseFloat(lowestCat) > 0 && parseFloat(lowestCat) < 10) {

            return lowestCat
        }

    } // (!rsCreditMatrix.EOF)

    error_creditMatrixRS();
    return false;
}

function getLoanInterval() {
// Finds the nearest loan amount interval greater than the loan amount
// Returns loan interval, 0 if error.

    var empType = ddlEmploymentType.value;
    var purpose = ddlLoanPurpose.value;
    var loanAmount = txtTotalAmount.value;
    var loanAmountInterval = 100000000;

    rsCreditMatrix.MoveFirst;

    // Find the loan interval
    if (!rsCreditMatrix.EOF) {

        // Finds the max loan amount
        do {

            if (rsCreditMatrix.Fields(0).Value == empType && rsCreditMatrix.Fields(1).Value == purpose) {

                if (parseFloat(loanAmount) <= parseFloat(rsCreditMatrix.Fields(2).Value)) {

                    if (rsCreditMatrix.Fields(2).Value <= loanAmountInterval) {
                        // Update loan interval
                        loanAmountInterval = rsCreditMatrix.Fields(2).Value

                    }

                }

            }

            rsCreditMatrix.MoveNext;

        } while (!rsCreditMatrix.EOF);

        if (parseFloat(loanAmountInterval) > 0) {

            // Found loan interval
            return loanAmountInterval

        }
    }
    else {
        error_creditMatrixRS();
        return 0;
    }

}

function update_qualify(yes, qualify_type) {
// Sets the qualify display

    if (yes == true) {

        td_qualify.innerHTML = "<font color=green><strong>YES: " + qualify_type + "</strong></font>";
        cmd_hide.disabled = false;
    }
    else if (yes == false) {

        td_qualify.innerHTML = "<font color=red><strong>NO</strong></font>";;
        if (pSource == "ManageProspect" || pSource == "ManagePreProspect" || pSource == "eWorkFlexi"){cmd_hide.disabled = true};
    }
    else {

       td_qualify.innerHTML = "Err"
       if (pSource == "ManageProspect" || pSource == "ManagePreProspect" || pSource == "eWorkFlexi"){cmd_hide.disabled = false}
    }

 }

function creditMatrixCalculate() {

    updateCreditMatrix();

    calculate();

   updateCreditMatrix();

   calculate();

}
function updateCreditMatrix() {

/*
    rsCreditMatrix:
    0 - EmploymentTypeNumber
    1 - LoanPurposeNumber
    2 - MaxLoanAmount
    3 - Category
    4 - LTV
    5 - PTI
    6 - Rate
    7 - MinIncomeAmount
    8 - MinLoanAmount
    9 - MinPropertyAmount
    10 - MaxPropertyAmount

 */

    var empType = ddlEmploymentType.value;
    var purpose = ddlLoanPurpose.value;
    var loanAmount = 0;
    var loanAmountInterval = 0;
    var loanAmountValid = false;
    var selectionOK = false;
    var qualify = true;
    var lowestCategory = 0;
    var highestCategory = 100;
    var qualify_type = "none";

    update_LTV(0);
    update_PTI(0,0);

    write_info("Reset LTV and PTI");

    update_qualify(false,"");

   // ********************************************************
   // ****** PHASE 1 OF CREDIT DECISION PROCESS **************
   // ********************************************************

    // ***** Check Employment Type and Loan Purpose *****

    rsCreditMatrix.MoveFirst;

    if (!rsCreditMatrix.EOF) {

        do {
            // Check Employment Type and Loan Purpose
            if (rsCreditMatrix.Fields(0).Value == empType && rsCreditMatrix.Fields(1).Value == purpose) {

                selectionOK = true;
                break;

            }

            rsCreditMatrix.MoveNext;

        } while (!rsCreditMatrix.EOF);

        // Check selection
        if (selectionOK == false) {

            // Incorrect selection
            write_info("Invalid employment type or loan purpose selection");
            return 0;

        }
    }
    else {

        // Credit matrix recordset error
        error_creditMatrixRS();
        return 0;

    }

    // ***** Is category selected ? *****

    if (ddlLinkRate.value == 0) {
        // Category not selected: Assume lowest category

        if (isLoanAmountValid()) {
            // Loan amount valid
            loanAmountValid = true;

            // Get loan amount interval
            loanAmount = getLoanInterval();

        }
        else {
            // Loan amount not valid
            loanAmountValid = false;

            // Get lowest max loan amount
            loanAmount = getLowestMaxLoan();

        }

        // Get the lowest category for the loanamount
        lowestCategory = getLowestCategory(loanAmount)

        // Set PTI and LTV based on lowest Category and loan amount (loan interval amount or lowest max loan amount)
        rsCreditMatrix.MoveFirst;

        if (!rsCreditMatrix.EOF) {

            do {

                if (rsCreditMatrix.Fields(0).Value == empType
                        && rsCreditMatrix.Fields(1).Value == purpose
                        && rsCreditMatrix.Fields(2).Value == loanAmount
                        && rsCreditMatrix.Fields(6).Value == lowestCategory) {

                    // LTV & PTI
                    update_LTV(rsCreditMatrix.Fields(4).Value);
                    update_PTI("",rsCreditMatrix.Fields(5).Value);

                    // Save values
                    ref_PTI = td_PTI.innerHTML;
                    ref_LTV = td_LTV.innerHTML;

                    if (loanAmountValid == false)
                        if (cmMaxLoanAmount > 0)
                            write_info("Invalid Loan amount. Maximum amount: R" + cmMaxLoanAmount + ". Assume lowest max loan and lowest category selected");
                        else
                            write_info("Loan amount invalid. Assume lowest max loan and lowest category selected");
                    else
                        write_info("Assume lowest category selected");

                    return 0;

                }

                rsCreditMatrix.MoveNext;

            } while (!rsCreditMatrix.EOF);

            alert("Could not set PTI and LTV for category 1 and loan amount");
            write_info("Could not set PTI and LTV for category 1 and loan amount");
            return 0;

        } // (!rsCreditMatrix.EOF)
        else {

            error_creditMatrixRS();
            return 0;

        }

    }   // (ddlLinkRate.value == 0) (Category not selected)

    // CATEGORY SELECTED

    if (ddlLinkRate.value != 0) {

        // ***** Is loan amount valid ? *****

        if (isLoanAmountValid()) {
            // Loan amount valid

            // Get loan amount interval
            loanAmount = getLoanInterval();

            // Get the lowest category for the loanamount (Returns link rate)
            lowestCategory = getLowestCategory(loanAmount)

            // Check that the selected link rate is ok for the entered loan amount
            if (ddlLinkRate.value < lowestCategory) {

                write_info("Invalid link rate / category selection for loan amount. Lowest category: " + lowestCategory*100 + "%");
                return 0;

            }

            // Get the highest category for the loan amount
            highestCategory = getHighestCategory(loanAmount)

            if (ddlLinkRate.value > highestCategory) {

                write_info("Invalid link rate / category selection for loan amount. Highest category: " + highestCategory*100 + "%");
                return 0;

            }

            // Update LTV and PTI according to the credit matrix
            rsCreditMatrix.MoveFirst

            if (!rsCreditMatrix.EOF) {

                do {

                    if (rsCreditMatrix.Fields(0).Value == empType
                            && rsCreditMatrix.Fields(1).Value == purpose
                            && rsCreditMatrix.Fields(2).Value == loanAmount
                            && rsCreditMatrix.Fields(6).Value == ddlLinkRate.value) {

                        // LTV & PTI from credit matrix
                        update_LTV(rsCreditMatrix.Fields(4).Value);
                        update_PTI("",rsCreditMatrix.Fields(5).Value);

                        // Save the values for quailfy decision
                        ref_PTI = td_PTI.innerHTML;
                        ref_LTV = td_LTV.innerHTML;

                        write_info("LTV & PTI OK on category selected");

                    }

                    rsCreditMatrix.MoveNext;

                } while (!rsCreditMatrix.EOF);

            }

        }
        else {
            // Loan amount not valid

            // Get lowest max loan amount
            loanAmount = getLowestMaxLoan();

            // Get the lowest category for the loanamount (Returns link rate)
            lowestCategory = getLowestCategory(loanAmount)

            // Set PTI and LTV based on Category 1 and loan amount
            rsCreditMatrix.MoveFirst;

            if (!rsCreditMatrix.EOF) {

                do {

                    if (rsCreditMatrix.Fields(0).Value == empType
                            && rsCreditMatrix.Fields(1).Value == purpose
                            && rsCreditMatrix.Fields(2).Value == loanAmount
                            && rsCreditMatrix.Fields(6).Value == lowestCategory) {

                        update_LTV(rsCreditMatrix.Fields(4).Value);
                        update_PTI("",rsCreditMatrix.Fields(5).Value);

                        // Save values
                        ref_PTI = td_PTI.innerHTML;
                        ref_LTV = td_LTV.innerHTML;

                        if (loanAmountValid == false)
                            write_info("Invalid Loan amount. Maximum amount: R" + cmMaxLoanAmount + ". Assuming lowest max loan and use selected category");
                        else
                            write_info("Loan amount invalid. Assume lowest max loan and use selected category");

                        return 0;

                    }

                    rsCreditMatrix.MoveNext;

                } while (!rsCreditMatrix.EOF);

                write_info("PTI & LTV info: Invalid loan amount");
                return 0;

            } // (!rsCreditMatrix.EOF)

            error_creditMatrixRS();
            return 0;

        } // (isLoanAmountValid())

    }   // (ddlLinkRate.value != 0)
    else {

    alert("Problem with link rate selection");
    write_info("Problem with link rate selection");
    return 0;

    }

    var sInfo = "PTI & LTV info:";
    var bExit = false;

   // ********************************************************
   // ****** PHASE 2 OF CREDIT DECISION PROCESS **************
   // ********************************************************

    if (radio_maximum_loan.checked == false) {
        // SPECIFIC CASE!!!

        // ***** Is instalment valid ? *****

        if (parseFloat(txtTotalInstalment.value) > 0) {

            // ***** Is income valid ? *****

            if (isIncomeValid()) {

                // ----- Update PTI -----
                var actual_PTI = formatNum2(parseFloat(txtTotalInstalment.value) / parseFloat(txtIncome.value)*100);

                // # Update FF PTI #
                update_PTI("",actual_PTI/100);

                // ***** Check PTI threshold *****

                // Calculate PTI using 100 % variable.
                // Some calculators do not have values for required fields. Use total instalment instead
                if (txtVariableRate.value > 0 && txtTotalAmount.value > 0) {

                    var instalment = formatNum2(CalculateInstallment(txtTotalAmount.value,txtVariableRate.value/100,12,txtTerm.value,0));

                }
                else {

                    var instalment = txtTotalInstalment.value;

                }

                // Calculate the 100% variable PTI.
                var var_qualify_PTI = formatNum2(parseFloat(instalment) / parseFloat(txtIncome.value) * 100);

                // # Update Var PTI #
	            update_PTI(var_qualify_PTI/100,"");

                // Check if 100% variable PTI is greater than ref.  Qualify on 100% variable first.
                if (parseFloat(var_qualify_PTI) > ref_PTI) {

                    sInfo = sInfo + " Var PTI:" + var_qualify_PTI + " ref: " + ref_PTI + ";"

                    qualify = false;

                }
                else {
                    // PTI valid

                    var fRate = txtFixedRate.value / 100;
                    var vRate = txtVariableRate.value / 100;

                    // ----- Update max fixed percent -----
                    if (txtTotalAmount.value > 0 && fRate > 0 && vRate > 0 && txtTerm.value > 0 && txtIncome.value > 0 && ref_PTI > 0) {

                        FindMaxFixed(txtTotalAmount.value, fRate, vRate, txtTerm.value, txtIncome.value, ref_PTI);

                    }

					// SS As per discussion with Antonie / Karina - tolerance to be removed as it was equating to 36%

                    switch (ddlEmploymentType.value) {

                        case "1":     // Salaried
                            var maxPTI = parseFloat(ref_PTI) * (1) // + parseFloat(cmMaxPTIMarginSalaried));
                            break;

                        case "2":     // Self Employed
                            var maxPTI = parseFloat(ref_PTI) * (1)// + parseFloat(cmMaxPTIMarginSelfEmployed));
                            break;

                        case "3":     // Subsidised
                            var maxPTI = parseFloat(ref_PTI) * (1)// + parseFloat(cmMaxPTIMarginSubsidised));
                            break;

                    }

                    //alert( "apti: " + actual_PTI + " ref_PTI:" + ref_PTI + " m:" + cmMaxPTIMargin);
                    // Is FF PTI above the reference PTI + the margin for fixed portion?
                    if (parseFloat(actual_PTI) > parseFloat(maxPTI)) {

                        sInfo = sInfo + " PTI:" + td_PTI.innerHTML + " ref: " + ref_PTI + ";"

                        qualify = false;

                    }
                    else {

                        if (parseFloat(actual_PTI) > parseFloat(ref_PTI))
                            qualify_type = "FF"
                        else
                            qualify_type = "Var"
                    }

                }

            }
            else {

                // Income invalid

                sInfo = sInfo + " Invalid income;";

                qualify = false;

            }

        }
        else {

            // Instalment invalid
            sInfo = sInfo + " Invalid instalment;";

            qualify = false;

        }
    } //(radio_maximum_loan.checked == false)

    // ***** Is property value valid ? *****

    if (isPropertyValueValid()) {

        // ----- Update LTV -----

        update_LTV(parseFloat(txtTotalAmount.value)/parseFloat(txtPropertyValue.value));

    }
    else {
        // Property value invalid

        if (parseFloat(cmMaxPropertyValue) > 0)
            sInfo = sInfo + " Property value must be less than: R" + cmMaxPropertyValue
        else
            if (parseFloat(cmMinPropertyValue) > 0)
                sInfo = sInfo + " Property value must be greater than: R" + cmMinPropertyValue

        cmMaxPropertyValue = 0;
        cmMinPropertyValue = 0;

        qualify = false;

    }

    // ***** Check LTV threshold *****

    if (parseFloat(td_LTV.innerHTML) >  ref_LTV) {

        // LTV invalid
        sInfo = sInfo + " LTV:" + td_LTV.innerHTML + " ref:" + ref_LTV;

        qualify = false;

    }

    // ***** DONE *****

    if (qualify == true) {

        // Qualifies
        write_info("LTV and PTI: Qualifies");
        update_qualify(true, qualify_type);

    }
    else {

        // Does not qualify
        write_info(sInfo);
        update_qualify(false,"");

    }

}

function write_info(msg) {
// Displays message

    td_info.innerHTML = msg

}

// --------------- END CREDIT MATRIX FUNCTIONS -------------------------------------------------------------------------------------------------------------

function formatNum0(number) {
// Returns a string formatted number with 0 decimal places

    return NumberToString(round_no(number,0),0);

}

function formatNum2(number) {
// Returns a string formatted number with 2 decimal places

    return NumberToString(round_no(number,2),2);

}

function round_no(number,X) {
// rounds number to X decimal places, defaults to 2

	X = (!X ? 2 : X);

	return (Math.round(number*100,X)/100);

}

function NumberToString(X, N) {
//rounds a signed number and outputs a string with trailing zeros

   var p = new Number("1e"+N);
   var S = new String(Math.round(X*p)/p);
   if (S.indexOf('e') == -1 ) {
		while ( (p = S.indexOf('.')) == -1 ) { S += '.' }
		while ( S.length <= p+N ) { S += '0' }
	}
	if (N == 0) {S = S.slice(0,-1);}
   return S;
}

//---------------------------------------------------------------------------------------------------------------------------------------------------
// TEMP
//---------------------------------------------------------------------------------------------------------------------------------------------------
/*
    if (!((window.event.keyCode > 47 && window.event.keyCode < 58) || (window.event.keyCode > 95 && window.event.keyCode < 106))) return 0;
    switch(calc_type) {

        case "monthly_instalment":

            break;

        case "loan_amount":

            break;

        case "extra_payments":

            break;
        case "maximum_loan":

            break;
        case "minimum_deposit":

            break;

        case "TeleCalcBondCosts":
            // TeleCalcBondCost specific adaption: When the user clicks back to flexi then the percentages are
            // passed back then the percentages are retained instead of being reset to the original percent
            // when the calculator was launched.

            var pNewFixedPercent = "<%=Request.QueryString("NewFixedPercent")%>";
            var pNewVariablePercent = "<%=Request.QueryString("NewVariablePercent")%>";

            txtFixedPercent.value = formatNum2(pNewFixedPercent);
            txtVariablePercent.value = formatNum2(pNewVariablePercent);

            break;

*/

</script>
</HEAD>
	<BODY class=Calculators language=javascript onload="return window_onload()">
		<XML id=xmldso1 src="TeleCalcRates.asp"></XML>
		<iframe id="FuncFrame" src="blank.html" style="width:0px; height:0px; border: 0px"></iframe>
		<iframe id="FuncFrame1" src="blank.html" style="width:0px; height:0px; border: 0px"></iframe>
    <table id=tbl_main width=810px border=0 height=560px class=Table1 cellpadding=0 cellspacing=0>
        <tr id=tr_main_prospect style="height: 60px">
            <td valign=top>
                <table align=center style="width:90%; height:100%" border=0 cellpadding=0 cellspacing=0>
                    <tr>
                        <td id=td_number style="height:20px; width:30%">Prospect / Loan Number</td>
                        <td>
                            <input id=txtNumber readonly type=text style="font-size:medium" />
                        </td>
                        <td style="width:100px">&nbsp;</td>
                        <td style=" font-weight:bold">Qualifies:</td>
                    </tr>
                    <tr>
                        <td>Client Name</td>
                        <td>
                            <input id=txtName readonly type=text style="width:400px; font-size:medium" />
                        </td>
                        <td style="width:100px">&nbsp;</td>
                        <td id=td_qualify></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id=tr_main_detail>
            <td valign=top>
                <table id=tbl_detail style="height:100%; width:100%" border=0 class=Table3 cellpadding=0 cellspacing=0>
                    <tr id=tr_detail_title class=Header1 align=center style="font-weight:bold">
                        <td id=td_detail_title colspan=5>Title</td>
                    </tr>
                    <tr id=tr_detail_headings style="height:20; font-weight:bold" class=Header2>
                        <td width=21%>&nbsp;</td>
                        <td align=center style="width: 20%">Fixed</td>
                        <td width=20% align=center>Variable</td>
                        <td width=20% align=center>Total</td>
                        <td width=19% align=center>&nbsp;</td>
                    </tr>
                    <tr>
                        <td id=td_detail_titles style="width: auto;" valign=top>
                            <table id=tblTitles style="width:100%; text-align:left" border=0 class=Table3 cellpadding=0 cellspacing=0>
                                <tr>
                                    <td style="height:26">&nbsp;Loan Purpose</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Employment Type</td>
                                </tr>
                                <tr id=tr_title_principal>
                                    <td id=td_pp style="height:26">&nbsp;Purchase Price (R)</td>
                                </tr>
                                <tr>
                                    <td id=td_dep style="height:26">&nbsp;Deposit (R)</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Loan Amount (R)</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Loan: % Split (%)</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Term (months)</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Market Rate Type</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Link Rate (%)</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Interest Rate (%)</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Instalment (R)</td>
                                </tr>
                                <tr id=tr_title_add_instalment>
                                    <td style="height:26">&nbsp;Additional Instalment (R)</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Income (R)</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Property Value (R)</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;Interest Paid (R)</td>
                                </tr>
                                <tr id=tr_title_savings>
                                    <td style="height:26">&nbsp;Savings (R)</td>
                                </tr>
                                <tr id=tr_title_new_term>
                                    <td style="height:26">&nbsp;New Term (months)</td>
                                </tr>
                            </table>
                       </td>
                       <td id=td_detail_fixed style="width: auto;" valign=top>
                            <table id=tblFixed style="width:100%; text-align:center" border=0 class=Table3 cellpadding=0 cellspacing=0>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr id=tr_fixed_principal>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtFixedAmount onkeyup="return txtFixedAmount_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtFixedPercent onkeyup="return txtFixedPercent_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <select id=ddlFixedMarketRateType style="width:150" language=javascript onchange="return ddlFixedMarketRateType_change()">
                                            <%Call populateDDL("Select Value, Description from sahldb.dbo.vw_2amMarketRate Where MarketRateKey in (3,4)")%>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input readonly type=text id=txtFixedRate style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtFixedInstalment onkeyup="return txtFixedInstalment_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr id=tr_fixed_add_instalment>
                                    <td style="height:26">
                                        <input type=text id=txtFixedAddInstalment onkeyup="return txtFixedAddInstalment_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtFixedInterestPaid style="width:150" />
                                    </td>
                                </tr>
                                <tr id=tr_fixed_savings>
                                    <td style="height:26">
                                        <input type=text id=txtFixedSavings style="width:150" />
                                    </td>
                                </tr>
                                <tr id=tr_fixed_new_term>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                            </table>
                       </td>
                       <td id=td_detail_variable style="width: auto;" valign=top>
                            <table id=tblVariable style="width:100%; text-align:center" border=0 cellpadding=0 cellspacing=0>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr id=tr_variable_principal>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26" >
                                        <input type=text id=txtVariableAmount onkeyup="return txtVariableAmount_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtVariablePercent onkeyup="return txtVariablePercent_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <select id=ddlVariableMarketRateType style="width:150" onchange="return ddlVariableMarketRateType_change()" language=javascript>
                                          <%Call populateDDL("Select value as MarketRateTypeRate, Description as MarketRateTypeDescription From [2am]..MarketRate Where MarketRateKey not in (3,4)")%>
                                        </select>
                                     </td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input readonly type=text id=txtVariableRate style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtVariableInstalment onkeyup="return txtVariableInstalment_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr id=tr_variable_add_instalment>
                                    <td style="height:26">
                                        <input type=text id=txtVariableAddInstalment onkeyup="return txtVariableAddInstalment_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtVariableInterestPaid style="width:150" />
                                    </td>
                                </tr>
                                <tr id=tr_variable_savings>
                                    <td style="height:26">
                                        <input type=text id=txtVariableSavings style="width:150" />
                                    </td>
                                </tr>
                                <tr id=tr_variable_new_term>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                            </table>
                       </td>
                       <td id=td_detail_total style="width: auto;" valign=top>
                            <table id=tblTotal style="width:100%; text-align:center" border=0 cellpadding=0 cellspacing=0>
                                <tr>
                                    <td style="height:26">
                                        <select id=ddlLoanPurpose style="width:150" onchange="return ddlLoanPurpose_change()" language=javascript>
                                            <%Call populateDDL("Select MortgageLoanPurposeKey as PurposeNumber, Description as PurposeDescription From [2am]..MortgageLoanPurpose Where MortgageLoanPurposeKey in (2,3,4)" )%>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <select id=ddlEmploymentType style="width:150" onchange="return ddlEmploymentType_change()" language=javascript>
                                            <%Call populateDDL("SELECT EmploymentTypeKey as EmploymentTypeNumber, Description as EmploymentTypeDescription From [2am]..EmploymentType (nolock)")%>
                                        </select>
                                    </td>
                                </tr>
                                <tr id=tr_principal>
                                    <td style="height:26">
                                        <input type=text id=txtPurchasePrice onkeyup="return txtPurchasePrice_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtDeposit onkeyup="return txtDeposit_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtTotalAmount onkeyup="return txtTotalAmount_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text readonly id=txtTotalPercent style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtTerm onkeyup="return txtTerm_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <select id=ddlLinkRate style="width:150" onchange="return ddlLinkRate_change()" language=javascript>
                                            <option value=0></option>
                                            <%
                                               '' Call populateDDL("Select Distinct Convert(Numeric(5,4),Rate/100.0),'JIBAR + ' + Convert(char(4),Convert(numeric(3,2),Rate)) +'%' From CreditMatrix (nolock) Where rate not in (1.8,2.1,3.1) and SPVNumber in (" & sSPVNumber & ")")
                                                If Request.QueryString("Source") = "eWorkFlexi" then
                                                    Call populateDDL("SELECT Distinct Convert(Numeric(5, 4),Rate/100.0),'JIBAR + ' + Convert(char(4),Convert(numeric(3,2),Rate)) + '%' From [sahldb].dbo.CreditMatrix (nolock)")
                                                else
                                                    Call populateDDL("Select Distinct Convert(Numeric(5,4),Rate/100.0),'JIBAR + ' + Convert(char(4),Convert(numeric(3,2),Rate)) +'%' From [sahldb].dbo.CreditMatrix (nolock) Where rate not in (1.7,1.8,2.1,2.3,2.7,3.1,2.0,2.29999999999999) and SPVNumber in (" & sSPVNumber & ")")
                                                end if
                                            %>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtInterestRate readonly style="width:150; visibility:hidden" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtTotalInstalment onkeyup="return txtTotalInstalment_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr id=tr_total_add_instalment>
                                    <td style="height:26">
                                        <input type=text id=txtTotalAddInstalment onkeyup="return txtTotalAddInstalment_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtIncome onkeyup="return txtIncome_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                        <input type=text id=txtPropertyValue onkeyup="return txtPropertyValue_onkeyup()" language=javascript style="width:150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:26">
                                       <input type=text id=txtTotalInterestPaid style="width:150" />
                                    </td>
                                </tr>
                                <tr id=tr_total_savings>
                                    <td style="height:26">
                                        <input type=text id=txtTotalSavings style="width:150" />
                                    </td style="height:26">
                                </tr>
                                <tr id=tr_total_new_term>
                                    <td style="height:26">
                                        <input type=text id=txtNewTerm style="width:150" />
                                    </td>
                                </tr>
                            </table>
                       </td>
                       <td id=td_detail_misc style="width: auto;" valign=top>
                            <table style="width:100%; height:435" border=0 cellpadding=0 cellspacing=0>
                                <tr style="height:15%">
                                    <td valign=top align=center>
                                        <table id=tbl_LTV style="width:70%; height:90%; " bordercolor=maroon border=1 cellpadding=0 cellspacing=0>
                                            <tr style="height:20%">
                                                <td align=center><b>LTV (%)</b></td>
                                            </tr>
                                            <tr>
                                                <td id=td_LTV align=center></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height:20%">
                                    <td valign=top align=center>
                                        <table id=Table1 style="width:70%; height:90%" bordercolor=maroon border=1 cellpadding=0 cellspacing=0>
                                            <tr style="height:20%">
                                                <td align=center title="Higher PTI Option Check Box" colspan=2><b>PTI (%)</b><input title="Higher PTI Option Check Box" type=checkbox id=chkHigherPTI onclick="return chkHigherPTI_click()" language=javascript /></td>
                                            </tr>
                                            <tr>
												<td>Var</td>
                                                <td id=td_PTI_Core align=center></td>
                                            </tr>
                                            <tr>
												<td width=10%>FF</td>
                                                <td id=td_PTI align=center></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height:20%">
                                    <td valign=top align=center>
                                        <table id=Table2 style="width:70%; height:90%" bordercolor=maroon border=1 cellpadding=0 cellspacing=0>
                                            <tr style="height:20%">
                                                <td align=center colspan=2><b>Max Fix</b></td>
                                            </tr>
                                            <tr>
                                                <td id=td1 align=center width=10%>R</td>
                                                <td id=td_max_fixed_amount align=center></td>
                                            </tr>
                                            <tr>
                                                <td id=td3 align=left width=10%>%</td>
                                                <td id=td_max_fixed_percent align=center></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height:40%">
                                    <td valign=top>
                                        <table style="width:100%;" border=0  class=Table3>
                                            <tr>
                                                <td colspan=2><b>I want to calculate:</b></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input id=radio_monthly_instalment type=radio name="calc" onclick="return show_monthly_instalment()" />
                                                </td>
                                                <td>Monthly Instalment</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input id=radio_loan_amount type=radio name="calc" onclick="return show_loan_amount()"/>
                                                </td>
                                                <td>Loan Amount</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input id=radio_extra_payments type=radio name="calc" onclick="return show_extra_payments()"/>
                                                </td>
                                                <td>Extra Payments</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input id=radio_maximum_loan type=radio name="calc" onclick="return show_maximum_loan()"/>
                                                </td>
                                                <td>Maximum Loan</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input id=radio_bond_costs type=radio name="calc" onclick="return show_bond_costs()"/>
                                                </td>
                                                <td>Bond Costs</td>
                                            </tr>
                                            <tr style="visibility:hidden">
                                                <td>
                                                    <input id=radio_minimum_deposit type=radio name="calc" onclick="return show_minimum_deposit()"/>
                                                </td>
                                                <td>Minimum Deposit</td>
                                            </tr>
                                            <tr>
                                                <td style="height:5px">&nbsp;</td>
                                                <td align=center><button id=cmd_close onclick="javascript: cmd_close_click()" type=button style="height:30; width:120; font-weight:bold">Close</button></td>
                                            </tr>

                                            <tr>
                                                <td style="height:5px">&nbsp;</td>
                                                <td align=center><button id=cmd_hide onclick="javascript: cmd_hide_click()" type=button style="height:30; width:120; font-weight:bold">Hide</button></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                       </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td id=td_error align=center valign=bottom style="color:Red; font-weight:bold;border-style:solid; border-width:thin; border-color:red"></td>
        </tr>
        <tr>
            <td id=td_info align=center valign=bottom style="color:Blue; font-weight:normal; border-style:solid; border-width:thin; border-color:Blue"></td>
        </tr>
    </table>

	</BODY>
</HTML>