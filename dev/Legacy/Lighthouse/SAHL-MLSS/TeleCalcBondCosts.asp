<%@ Language=VBScript %>



<% 
dim objConn
    
   
'------------------------------------------------------------------------------
sub getConnection()

'// Creates connection to the database
'// objConn
    
    dim sDSN   
    ' Connection string
    sDSN = "Provider=SQLOLEDB.1; Application Name='MLS System Version1 [TeleCalculatorFlexi.asp]';Data Source=" & Session("SQLDatabase") & ";uid=" & Session("UserID") 
    
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
<!--#include file="product.inc"-->
<!--#include file="creditmatrix.inc"-->

<title id=bc_title></title>

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" id="Microsoft_Licensed_Class_Manager_1_0" VIEWASTEXT 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>

<META name="VI60_DefaultClientScript" content=JavaScript>

<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">

<SCRIPT id=clientEventHandlersJS language=javascript>
<!--
<!--#include file="TeleCalcFunctions.inc"-->

var sLoanPurpose;
var isFlexi = false;
var SAHLFixedRate;
var sIntOnly  = "<%=Request.Querystring("IntOnly")%>";

// Product options
//	1 - Variable Loan
//	2 - VariFix Loan
//	5 - Super Rate

//var i_Product = "<%=Variable_Product_Key %>";	// Default to variable
var i_Product = "<%=New_Variable_Key %>" // Default to New Variable


var bShowVariable = true;
var d_DiscountRate = 0;


function AddDomElement(objDom, strName, strValue) {

	var objNode;
	    
	//Add new node
	objNode = objDom.createElement(strName);
	objNode.text = strValue;
	objDom.documentElement.appendChild(objNode);
}


function GetFees(dPurchaseAmount, dCashDeposit, dCashRequired, sLoanPurpose, sLoanTransferType,dEstimatedPropValue) {

	//alert(dLoanAmount + " " + sLoanPurpose + " " + sLoanTransferType);
	
 	if (sLoanPurpose == "SwitchAndSave") sLoanPurpose = "Switch";
	if (sLoanPurpose == "NewPurchase") sLoanPurpose = "NewHome";
	if (sLoanPurpose == "Refinance") sLoanPurpose = "Refinance";
		
	var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	xmlhttp.open("GET", "TeleCalcFees.asp", false);

	var objDOM  = new ActiveXObject("Microsoft.XMLDOM");		
	var objNode = objDOM.createElement("Loan");
	objDOM.appendChild(objNode);
		 
		 
	AddDomElement (objDOM, "PurchaseAmount", dPurchaseAmount);
	AddDomElement (objDOM, "CashDeposit", dCashDeposit);
	AddDomElement (objDOM, "CashRequired", dCashRequired);
	AddDomElement (objDOM, "LoanPurpose", sLoanPurpose);
	AddDomElement (objDOM, "LoanTransferType", sLoanTransferType);
	AddDomElement (objDOM, "MarketValue",dEstimatedPropValue );
				
	var xmlText = objDOM.xml;
			
	var xmldom = new ActiveXObject("Microsoft.XMLDOM");
	xmldom.loadXML(xmlText);
	xmlhttp.send(xmldom);
	
					
	var doc = xmldso.XMLDocument;
	var sStart = xmlhttp.responseText.indexOf("<root>")
	var sEnd = xmlhttp.responseText.indexOf("</root>")
	doc.loadXML(xmlhttp.responseText.substring( sStart,sEnd+8));

	var RegistrationFeeSAHL = xmldso.recordset.fields(0).value;     
	var RegistrationFeeBank = 2*xmldso.recordset.fields(0).value;     
	var CancellationFeeSAHL = xmldso.recordset.fields(1).value;
	var CancellationFeeBank = 0.0;
	var ValuationFeeSAHL = xmldso.recordset.fields(2).value;
	var ValuationFeeBank = xmldso.recordset.fields(2).value;    
	var InitiationFeeSAHL = xmldso.recordset.fields(3).value;
	var InitiationFeeBank = 0.0;
	var TransferFeeSAHL = xmldso.recordset.fields(4).value; 
	var TransferFeeBank = xmldso.recordset.fields(4).value;
	var InterimInterestFeeSAHL = xmldso.recordset.fields(5).value;    
	var InterimInterestFeeBank = xmldso.recordset.fields(5).value;    

	window.txtRegistrationFeeSAHL.value = NumberToString(RegistrationFeeSAHL,2);     
	window.txtRegistrationFeeBank.value = NumberToString(RegistrationFeeBank,2);     
	window.txtCancellationFeeSAHL.value = NumberToString(CancellationFeeSAHL,2);
	window.txtCancellationFeeBank.value = NumberToString(CancellationFeeBank,2);
	window.txtValuationFeeSAHL.value = NumberToString(ValuationFeeSAHL,2);
	window.txtValuationFeeBank.value = NumberToString(ValuationFeeBank,2);    
	window.txtInitiationFeeSAHL.value = NumberToString(InitiationFeeSAHL,2);
	window.txtInitiationFeeBank.value = NumberToString(InitiationFeeBank,2);
	window.txtTransferFeeSAHL.value = NumberToString(TransferFeeSAHL,2); 
	window.txtTransferFeeBank.value = NumberToString(TransferFeeBank,2);
	window.txtInterimInterestFeeSAHL.value = NumberToString(InterimInterestFeeSAHL,2);    
	window.txtInterimInterestFeeBank.value = NumberToString(InterimInterestFeeBank,2);    


 }

function GetRates() {

	var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	xmlhttp.open("GET", "TeleCalcRates.asp", false);

	var objDOM  = new ActiveXObject("Microsoft.XMLDOM");		
	var objNode = objDOM.createElement("Loan");
	objDOM.appendChild(objNode);
		 
	var xmlText = objDOM.xml;
			
	var xmldom = new ActiveXObject("Microsoft.XMLDOM");
	xmldom.loadXML(xmlText);
	xmlhttp.send(xmldom);
				
	var doc = xmldso1.XMLDocument;
	var sStart = xmlhttp.responseText.indexOf("<root>")
	var sEnd = xmlhttp.responseText.indexOf("</root>")
	doc.loadXML(xmlhttp.responseText.substring( sStart,sEnd+8));

	//window.inJIBARRate.value = NumberToString(xmldso1.recordset.fields(0).value,2);
	window.txtSAHLInterestRate.value = NumberToString(xmldso1.recordset.fields(1).value,2);
	window.txtRateBank.value = NumberToString(xmldso1.recordset.fields(2).value,2);
	SAHLFixedRate = NumberToString(xmldso1.recordset.fields(3).value,2);
	
 }

function trim(strText) { 
    // this will get rid of leading spaces 
    while (strText.substring(0,1) == ' ') 
        strText = strText.substring(1, strText.length);

    // this will get rid of trailing spaces 
    while (strText.substring(strText.length-1,strText.length) == ' ')
        strText = strText.substring(0, strText.length-1);

   return strText;
} 

function window_onload() {
  	
  	GetRates();
  	
 		//If Calculator opened from the New Lead
  	if (trim("<%=Request.QueryString("Principal")%>") != "") 
  	{
  	    
  	    //get common fields
 		ddlPurpose.selectedIndex = "<%=Request.QueryString("Purpose")%>"; 
 
		ddEmploymentType.selectedIndex  = "<%=Request.QueryString("EmploymentType")%>";

		txtTerm.value = trim("<%=Request.QueryString("Term")%>");
		
		//change to purpose visible fields
		window.ddlPurpose.onchange();

		//fill in the values
		if (ddlPurpose.selectedIndex == 0) //Switch
		{
			txtMarketValue.value = trim("<%=Request.QueryString("MarketValue")%>");
			txtCashRequired.value = trim("<%=Request.QueryString("CashRequired")%>");
			txtLoanAmount.value	= trim("<%=Request.QueryString("LoanAmount")%>");
			
		}
		else if (ddlPurpose.selectedIndex == 1) //New Purchase
		{
		    
			txtPurchasePrice.value = trim("<%=Request.QueryString("PurchasePrice")%>");
			txtCashDeposit.value = trim("<%=Request.QueryString("CashDeposit")%>");
			
//			alert(trim("<%=Request.QueryString("PurchasePrice")%>"));
			
		}
		else if (ddlPurpose.selectedIndex == 2) //Refinance
		{
			txtMarketValue.value = trim("<%=Request.QueryString("MarketValue")%>");
			txtCashRequired.value = trim("<%=Request.QueryString("CashRequired")%>");
		}
      

        if ("<%=Request.QueryString("isFlexi")%>" == 1) {

            show_flexi();            

            i_Product = "<%=VariFix_Product_Key %>";   // VariFix
            
            document.title = "VariFix Proportion Calculator";  
         

            window.fixed_percent.value = "<%=Request.QueryString("FixedPercent")%>";  

            isFlexi = true;

            // moved before next so can used fixed amount on calculated values screen.
            calc_amounts("fixed");

		    calculate_flexi_fix("percentage");  // Calculate() called from this proc	           

            disable_fields();

        }
        else {
			
			d_DiscountRate = "<%=Request.QueryString("DiscountRateSave")%>";

			if (parseFloat(d_DiscountRate) > 0 ) {
			
				i_Product = "<%=SuperRate_Product_Key %>"	// Super Rate
				i_eTerm = (trim("<%=Request.QueryString("eTerm")%>"));
				if (i_eTerm == 1) {
					window.cbExtTerm.checked = true;
					window.txtTerm.value = 276;
					window.txtTerm.disabled = true;
					window.txtExtTerm.value = "Yes";
					}
				else
					{
					window.cbExtTerm.checked = false;
					window.txtTerm.value = trim("<%=Request.QueryString("Term")%>");
					window.txtTerm.disabled = false;
					window.txtExtTerm.value = "No";	
					}
				show_super_rate()
				Calculate();
			}
			else {
			
		//	i_Product = "<%=Variable_Product_Key %>";   // Variable
			i_Product = "<%=New_Variable_Key %>"; // New Variable  
			                 
            isFlexi = false;
		    show_core();
		    Calculate();
		    }
		    
        }
        				
  	} 
  	else
  	{
			//If Calculator is called from elsewhere use defaults
			window.txtTerm.value = 240;
			window.ddEmploymentType.selectedIndex = 0 //Switch
			window.ddlPurpose.selectedIndex = 0 //Salaried

			window.ddlPurpose.onchange(); 
   }
   
   // Set the product combo
   ddlProduct.value = i_Product;
   
	if (sIntOnly == "1" ) {
		window.cbIntOnly.checked = true;
		Calculate();
	} else if (sIntOnly == "0" ) {
		window.cbIntOnly.checked = false;
		window.IntOnlyInst.style.display = "none";	
	} else {
   		window.IntOnly.style.display = "none";
		window.IntOnlyInst.style.display = "none";	
	}
   
}


function disable_fields() {

    ddlPurpose.disabled = true;
    ddEmploymentType.disabled = true;
    txtMarketValue.disabled = true;
    txtPurchasePrice.disabled = true;
    txtLoanAmount.disabled = true;
    txtCashDeposit.disabled = true;
    txtCashRequired.disabled = true;
    txtPrincipal.disabled = true;
    txtTerm.disabled = true;
    
}

function btnEnteredValues_onclick () {

		window.trEnteredValues.style.display = "inline";
		window.trFees.style.display = "none"; 
		window.trCalculations.style.display = "none";  
		
		window.table_heading.innerText = "Entered Values:"				
		btnEnteredValues.style.backgroundColor = "#FFA000";
		btnGoToFlexi.style.backgroundColor = "SeaGreen";
		btnFees.style.backgroundColor = "SeaGreen";
		btnCalculatedValues.style.backgroundColor = "SeaGreen";
}

function btnEnteredValues_onmouseover () {
		btnEnteredValues.style.color="yellow";
}

function btnEnteredValues_onmouseout () {
		btnEnteredValues.style.color="white";
}


function btnFees_onclick () {
		window.trEnteredValues.style.display = "none";
		window.trFees.style.display = "inline"; 
		window.trCalculations.style.display = "none";  
		
		window.table_heading.innerText = "Fees:"			
		window.btnEnteredValues.style.backgroundColor = "SeaGreen";
		window.btnFees.style.backgroundColor = "#FFA000";
		window.btnCalculatedValues.style.backgroundColor = "SeaGreen";

}

function btnFees_onmouseover () {
		btnFees.style.color="yellow";
}

function btnFees_onmouseout () {
		btnFees.style.color="white";
}

function btnCalculatedValues_onclick () {
		window.trEnteredValues.style.display = "none";
		window.trFees.style.display = "none"; 
		window.trCalculations.style.display = "inline";  

		window.table_heading.innerText = "Calculated Values:"
		window.btnEnteredValues.style.backgroundColor = "SeaGreen";
		window.btnFees.style.backgroundColor = "SeaGreen";
		window.btnCalculatedValues.style.backgroundColor = "#FFA000";
		
		if (isFlexi == true)
		    btnShowVariable.click();

}

function btnCalculatedValues_onmouseover () {
		btnCalculatedValues.style.color="yellow";
}

function btnCalculatedValues_onmouseout () {
		btnCalculatedValues.style.color="white";
}


function SwitchAndSave() {
	sLoanPurpose = "SwitchAndSave";

	//error line
	window.tdErrorLine.innerHTML = "<font color=#d2b48c>" + "_" + "</font>";
	
	window.btnEnteredValues.click();
	
	window.trPropertValue.style.display = "";
	window.trLoanAmount.style.display = "";
	window.trPurchasePrice.style.display = "none";
	window.trCashDeposit.style.display = "none";
	window.trCashRequired.style.display = "";  
	
	window.trFeesNewHome2.style.display = "none";  

	ClearDetails();
	
	window.txtMarketValue.focus();  
		

}

function NewPurchase() {
	sLoanPurpose = "NewPurchase";

	window.btnEnteredValues.click();
	
	window.tdErrorLine.innerHTML = "<font color=#d2b48c>" + "_" + "</font>";
	
	window.btnEnteredValues.click();
	window.trPropertValue.style.display = "none";
	window.trPurchasePrice.style.display = "";  
	window.trLoanAmount.style.display = "none";
	window.trPurchasePrice.style.display = "";  
	window.trCashDeposit.style.display = "";
	window.trCashRequired.style.display = "none";  

 	ClearDetails();

	window.txtPurchasePrice.focus();  
		
}

function Refinance() {
	sLoanPurpose = "Refinance";
		
	window.tdErrorLine.innerHTML = "<font color=#d2b48c>" + "_" + "</font>";

	window.btnEnteredValues.click();

	window.trPropertValue.style.display = "";
	window.trLoanAmount.style.display = "none";
	window.trPurchasePrice.style.display = "none";  
	window.trCashDeposit.style.display = "none";
	window.trCashRequired.style.display = "";  
	
	window.trFeesNewHome2.style.display = "none";  
	
	ClearDetails();
	
	window.txtMarketValue.focus();  

	
}

function ClearDetails () {
	window.tdQualified.innerHTML  = "<font color=#ff0000 size=3><strong>NO</strong></font>";
	
	window.txtMarketValue.value = "";
	window.txtLoanAmount.value = "";
	window.txtPurchasePrice.value = "";
	window.txtCashRequired.value = "";
	window.txtCashDeposit.value = "";     
	
	//window.inInterestRate.value = "";
	window.txtTerm.value  = 240;
	//window.ddEmploymentType.selectedIndex = 0;
	window.txtPrincipal.value = "";
	
	window.txtCancellationFeeSAHL.value = "";
	window.txtCancellationFeeBank.value = "";
	window.txtRegistrationFeeSAHL.value = "";
	window.txtRegistrationFeeBank.value = "";
	window.txtInitiationFeeSAHL.value = "";   
	window.txtInitiationFeeBank.value = "";
	window.txtValuationFeeSAHL.value = "";
	window.txtValuationFeeBank.value = "";
	window.txtTransferFeeSAHL.value = "";
	window.txtTransferFeeBank.value = "";   
	window.txtInterimInterestFeeSAHL.value = "";   
	window.txtInterimInterestFeeBank.value  = "";
	window.ddTransferType.selectedIndex = 0;
	   window.cbTotalFees.checked = false;
   window.cbTotalFees.disabled = true;   

	if (sLoanPurpose == "NewPurchase")  {
			window.cbTotalFees.checked = true;
			window.cbTotalFees.disabled = false; 
   }  
	else  {
			window.cbTotalFees.checked = true;
			window.cbTotalFees.disabled = false;   
		}
	
	
	window.txtTotalFeesSAHL.value = ""; 
	window.txtTotalFeesBank.value = ""; 
	window.txtTotalLoanSAHL.value = ""; 
	window.txtTotalLoanBank.value = "";   
	
	window.txtRateSAHL.value = "";
	window.txtIncomeSAHL.value = "";
	window.txtInstalmentSAHL.value = "";
	window.txtLTVSAHL.value = ""; 
	window.txtPTISAHL.value = "";
	window.txtSAHLRateCategory.value = "";
	window.txtSAHLRateCategory_fixed.value = "";
	window.txtSavingsTerm.value = "";   

	window.txtInstalmentBank.value = ""; 	

}

function ClearEnteredFields() {
	window.txtMarketValue.value = ""; 
	window.txtLoanAmount.value = "";
	window.txtPurchasePrice.value = "";
	window.txtCashRequired.value = "";
	window.txtCashDeposit.value = "";
	//window.inInterestRate.value = "";
	window.txtTerm.value = "240";
	//window.ddEmploymentType.selectedIndex = 0;
}

function ClearFees() {
	window.txtCancellationFeeSAHL.value = "";
	window.txtCancellationFeeBank.value = "";
	window.txtRegistrationFeeSAHL.value = "";
	window.txtRegistrationFeeBank.value = "";
	window.txtInitiationFeeSAHL.value = "";   
	window.txtInitiationFeeBank.value = "";
	window.txtValuationFeeSAHL.value = "";
	window.txtValuationFeeBank.value = "";
	window.txtTransferFeeSAHL.value = "";
	window.txtTransferFeeBank.value = "";   
	window.txtInterimInterestFeeSAHL.value = "";   
	window.txtInterimInterestFeeBank.value  = "";
	window.ddTransferType.selectedIndex = 0;
	
//	if (sLoanPurpose == "NewPurchase")
//		window.cbTotalFees.checked = false; 
//	else 
//		window.cbTotalFees.checked = true;  
	
	window.txtTotalFeesSAHL.value = ""; 
	window.txtTotalFeesBank.value = ""; 
	window.txtTotalLoanSAHL.value = ""; 
	window.txtTotalLoanBank.value = "";        
}

function ClearCalcFields() {
	
		//window.txtRateSAHL.value = ""
		window.txtIncomeSAHL.value = "";
		window.txtInstalmentSAHL.value = "";
		window.txtLTVSAHL.value = ""; 
		window.txtPTISAHL.value = "";
		window.txtSAHLRateCategory.value = "";
		window.txtSAHLRateCategory_fixed.value = "";
		window.txtSavingsTerm.value = "";   

		//'window.txtRateBank.value = ""
		window.txtIncomeBank.value = "";
		window.txtInstalmentBank.value = "";
		window.txtLTVBank.value = ""; 
		window.txtPTIBank.value = "";
		
		window.txtFixedPortion.value = "";
		window.txtVariablePortion.value = "";
		window.txtInstalmentSAHL_fixed.value = "";						
		window.txtInstalmentSAHL_variable.value = "";
		window.window.entered_variable_amount.value = "";
		window.window.entered_fixed_amount.value = "";	
		
		window.txtIntOnlyInst.value = "";
		
			
  }


function DisableQualify() {
	
	if (window.txtMarketValue.value == "") dPropertyValue = 0.0; else dPropertyValue = parseFloat(window.txtMarketValue.value);
	if (window.txtMarketValue.value == "") dEstimatedPropValue = 0.0; else dEstimatedPropValue = parseFloat(window.txtMarketValue.value);
	if (window.txtLoanAmount.value == "") dLoanAmount = 0.0; else dLoanAmount = parseFloat(window.txtLoanAmount.value);
	if (window.txtPurchasePrice.value == "") dPurchasePrice = 0.0; else dPurchasePrice = parseFloat(window.txtPurchasePrice.value);
	if (window.txtCashRequired.value == "") dCashRequired = 0.0; else dCashRequired = parseFloat(window.txtCashRequired.value);
	if (window.txtCashDeposit.value == "") dCashDeposit = 0.0; else dCashDeposit = parseFloat(window.txtCashDeposit.value);
	if (window.txtTerm.value == "") iTerm = 240; else iTerm = parseInt(window.txtTerm.value);
	
	var dPrincipal =  dLoanAmount + dPurchasePrice + dCashRequired - dCashDeposit;
	window.txtPrincipal.value = NumberToString(dPrincipal,0);

	window.tdQualified.innerHTML  = "<font color=#ff0000><strong>NO</strong></font>";
  
  ClearFees();
 	ClearCalcFields();
}


function Calculate() {
	if (ValidateData() == false) 
		return;

	var dPropertyValue;
	var dEstimatedPropValue;
	var dLoanAmount;
	var dPurchasePrice;
	var dCashRequired;
	var dCashDeposit;
	var dSAHLRate;
	var iTerm;

	if (window.txtMarketValue.value == "") dPropertyValue = 0.0; else dPropertyValue = parseFloat(window.txtMarketValue.value);
	if (window.txtMarketValue.value == "") dEstimatedPropValue = 0.0; else dEstimatedPropValue = parseFloat(window.txtMarketValue.value);
	if (window.txtLoanAmount.value == "") dLoanAmount = 0.0; else dLoanAmount = parseFloat(window.txtLoanAmount.value);
	if (window.txtPurchasePrice.value == "") dPurchasePrice = 0.0; else dPurchasePrice = parseFloat(window.txtPurchasePrice.value);
	if (window.txtCashRequired.value == "") dCashRequired = 0.0; else dCashRequired = parseFloat(window.txtCashRequired.value);
	if (window.txtCashDeposit.value == "") dCashDeposit = 0.0; else dCashDeposit = parseFloat(window.txtCashDeposit.value);
	if (window.txtTerm.value == "") iTerm = 240; else iTerm = parseInt(window.txtTerm.value);
	
	var dPrincipal =  dLoanAmount + dPurchasePrice + dCashRequired - dCashDeposit;
	window.txtPrincipal.value = NumberToString(dPrincipal,0);
	
	var dTotalLoanAmount = dPrincipal + parseFloat(window.txtRegistrationFeeSAHL.value) + parseFloat(window.txtCancellationFeeSAHL.value) +
							parseFloat(window.txtValuationFeeSAHL.value) + parseFloat(window.txtInitiationFeeSAHL.value) +
							parseFloat(window.txtTransferFeeSAHL.value) + parseFloat(window.txtInterimInterestFeeSAHL.value);
	
	
	if (window.cbIntOnly.checked != true) {
	
		if ( (dPrincipal < 140000) || (dPrincipal > 5000000) ) {
				DisableQualify();
				window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>Please keep Total Loan Amount to between R140,000 and  R5,000,000</strong></font></div>";
				return;
		}
		
		if (i_Product == "<%=SuperRate_Product_Key %>") {
			if (dTotalLoanAmount < 250000) {
				DisableQualify();
				window.td_LoyaltyBonus.innerHTML = "";
				window.td_IndicativeInstalment.innerHTML = "";	                    
				window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>Please keep Total Loan Amount >= R250 000 for Super Rate</strong></font></div>";
				return;
			}
		}
	} else {
		if (cbTotalFees.checked == true) {

			if ( (dTotalLoanAmount < <%=INTEREST_ONLY_MIN%>) || (dTotalLoanAmount > <%=INTEREST_ONLY_MAX%>) ) {
				DisableQualify();
				window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>Please keep Total Loan Amount to between R<%=formatNumber(INTEREST_ONLY_MIN,0)%> and  R<%=formatNumber(INTEREST_ONLY_MAX,0)%> for interest only loans.</strong></font></div>";
				return;
			}

		} else if ( (dPrincipal < <%=INTEREST_ONLY_MIN%>) || (dPrincipal > <%=INTEREST_ONLY_MAX%>) ) {

				DisableQualify();
				window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>Please keep Total Loan Amount to between R<%=formatNumber(INTEREST_ONLY_MIN,0)%> and  R<%=formatNumber(INTEREST_ONLY_MAX,0)%> for interest only loans.</strong></font></div>";
				return;

		}
	}	
		
	if (dPrincipal == 0.0)	{
			DisableQualify();
			window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>Please keep Total Loan Amount great than 0</strong></font></div>";
			return;
	}
	var dPurchaseAmount;
	if (dLoanAmount == 0.0) 
		dPurchaseAmount = dPurchasePrice;
	else 
		dPurchaseAmount = dLoanAmount;

	GetFees(dPurchaseAmount, dCashDeposit, dCashRequired, sLoanPurpose, window.ddTransferType.value,dEstimatedPropValue);

	var dRegistrationFeeSAHL;     
	var dRegistrationFeeBank;     
	var dCancellationFeeSAHL;
	var dCancellationFeeBank;
	var dValuationFeeSAHL;
	var dValuationFeeBank;    
	var dInitiationFeeSAHL;
	var dInitiationFeeBank;
	var dTransferFeeSAHL; 
	var dTransferFeeBank;
	var dInterimInterestFeeSAHL;    
	var dInterimInterestFeeBank;    
	var dSubTotalFeesSAHL;
	var dSubTotalFeesBank;
	var dTotalFeesSAHL;
	var dTotalFeesBank;


	dRegistrationFeeSAHL = parseFloat(window.txtRegistrationFeeSAHL.value);
	dRegistrationFeeBank = parseFloat(window.txtRegistrationFeeBank.value);
	dCancellationFeeSAHL = parseFloat(window.txtCancellationFeeSAHL.value);
	dCancellationFeeBank = parseFloat(window.txtCancellationFeeBank.value);
	dValuationFeeSAHL = parseFloat(window.txtValuationFeeSAHL.value);
	dValuationFeeBank = parseFloat(window.txtValuationFeeBank.value);
	dInitiationFeeSAHL = parseFloat(window.txtInitiationFeeSAHL.value);
	dInitiationFeeBank = parseFloat(window.txtInitiationFeeBank.value);

	dSubTotalFeesSAHL = dRegistrationFeeSAHL+dCancellationFeeSAHL+dValuationFeeSAHL+dInitiationFeeSAHL;
	dSubTotalFeesBank = dRegistrationFeeBank+dCancellationFeeBank+dValuationFeeBank+dInitiationFeeBank;
	window.txtSubTotalFeesSAHL.value =  NumberToString(dSubTotalFeesSAHL,2); 
	window.txtSubTotalFeesBank.value =  NumberToString(dSubTotalFeesBank,2);

	if (window.cbTotalFees.checked == false ) 
	{
		dRegistrationFeeSAHL = 0; dRegistrationFeeBank = 0;
		dCancellationFeeSAHL = 0; dCancellationFeeBank = 0;
		dValuationFeeSAHL = 0; dValuationFeeBank = 0;
		dInitiationFeeSAHL = 0; dInitiationFeeBank = 0;
	}
	
	////////////////////////////////
	dTransferFeeSAHL = parseFloat(window.txtTransferFeeSAHL.value);
	dTransferFeeBank = parseFloat(window.txtTransferFeeBank.value);
	
	dInterimInterestFeeSAHL = parseFloat(window.txtInterimInterestFeeSAHL.value);
	dInterimInterestFeeBank = parseFloat(window.txtInterimInterestFeeBank.value);

	//Include Interim Interest	
	dTotalFeesSAHL = dRegistrationFeeSAHL+dCancellationFeeSAHL+dValuationFeeSAHL+dInitiationFeeSAHL+dInterimInterestFeeSAHL;
	dTotalFeesBank = dRegistrationFeeBank+dCancellationFeeBank+dValuationFeeBank+dInitiationFeeBank+dInterimInterestFeeBank;
	window.txtTotalFeesSAHL.value =  NumberToString(dTotalFeesSAHL,2); 
	window.txtTotalFeesBank.value =  NumberToString(dTotalFeesBank,2);
	
	if (sLoanPurpose == "NewPurchase") {
		var dNewHomeFees = parseFloat(window.txtRegistrationFeeSAHL.value) + parseFloat(window.txtCancellationFeeSAHL.value) +
							parseFloat(window.txtValuationFeeSAHL.value) + parseFloat(window.txtInitiationFeeSAHL.value) +
							parseFloat(window.txtTransferFeeSAHL.value) + parseFloat(window.txtInterimInterestFeeSAHL.value);
		
		window.txtFeesNewHome.value  =  NumberToString(dNewHomeFees,2); 
		
		if (window.ddEmploymentType.value == "Salaried") {
		//	window.cbTotalFees.checked = true;	
			if (window.txtPurchasePrice.value < 1999999) {
				if (window.txtCashDeposit.value < dSubTotalFeesSAHL)	{
				DisableQualify();
				window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>With 100% Loan - Cash Deposit can not be less than "  + dSubTotalFeesSAHL + " with fees capitalised</strong></font></div>";
			   	return;
			   }	
			}
		  } 
	}	
	var dTotalLoanSAHL = dPrincipal + dTotalFeesSAHL;
	var dTotalLoanBank = dPrincipal + dTotalFeesBank;
	window.txtTotalLoanSAHL.value = NumberToString(dTotalLoanSAHL,2); 
	window.txtTotalLoanBank.value = NumberToString(dTotalLoanBank,2); 


	var dLTVSAHL, dLTVBank; 
	if (sLoanPurpose == "NewPurchase") {
		if (dPurchasePrice == 0.0)	return;
		dLTVSAHL = dTotalLoanSAHL/dPurchasePrice*100;
		dLTVBank = dTotalLoanBank/dPurchasePrice*100;
		}
	else {
		if (dPropertyValue == 0.0)	return;
		dLTVSAHL = dTotalLoanSAHL/dPropertyValue*100;
		dLTVBank = dTotalLoanBank/dPropertyValue*100;
	}
	
	window.txtLTVSAHL.value = NumberToString(dLTVSAHL,2);
	window.txtLTVBank.value = NumberToString(dLTVBank,2);   
	
	var sEmploymentType = window.ddEmploymentType.value;
	
	//alert(sLoanPurpose + " " + dTotalLoanSAHL + " " + sEmploymentType + " " + dLTVSAHL);
	
	var sPTIRateCategory = LTVPTIMatrixGetPTI(sLoanPurpose, dTotalLoanSAHL, sEmploymentType, NumberToString(dLTVSAHL,2));

	if (dLTVSAHL > 80 && window.cbIntOnly.checked) {
		window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>LTV of " + NumberToString(dLTVSAHL,2) + "% may not exceed 80% for interest only loans.</strong></font></div>";
	}

	var dPTI1=0;
	var sRateCategory1="";
	var iInterestPeriods = 12;
	var iType = 0;
	var dInterestRateSAHL;
	var dSAHLInterestRate = parseFloat(window.txtSAHLInterestRate.value);

	if (sPTIRateCategory == "0") {
	
		DisableQualify();		
	
		window.tdQualified.innerHTML  = "<font color=#ff0000 size=3><strong>NO</strong></font>";
		window.text1.value = sPTIRateCategory;
		
		return;
	}
	else if (sPTIRateCategory.length >= 3)
	{
		window.tdQualified.innerHTML = "<font color=#008000 size=3><strong>YES</strong></font>";
		
		dPTI1 = parseInt(sPTIRateCategory.substring(0,2));
		window.txtPTISAHL.value =  NumberToString(dPTI1,2);
		
		sRateCategory1	= sPTIRateCategory.substring(2,3);
		if (sRateCategory1 == "1") {
			dInterestRateSAHL = dSAHLInterestRate;
			//window.txtSAHLRateCategory.value = "2.10"; 
			//window.txtSAHLRateCategory_fixed.value = "2.10"; 
			window.txtSAHLRateCategory.value = "1.90"; 
			window.txtSAHLRateCategory_fixed.value = "1.90"; 			
		}
		if (sRateCategory1 == "2") {
			dInterestRateSAHL = dSAHLInterestRate + 0.3;
			//window.txtSAHLRateCategory.value = "2.30"; 
			//window.txtSAHLRateCategory_fixed.value = "2.30"; 
			window.txtSAHLRateCategory.value = "2.20"; 
			window.txtSAHLRateCategory_fixed.value = "2.00"; 
		}
		if (sRateCategory1 == "3") {
			//dInterestRateSAHL = dSAHLInterestRate + 0.60;
			dInterestRateSAHL = dSAHLInterestRate + 0.50;
			//window.txtSAHLRateCategory.value = "2.70"; 
			//window.txtSAHLRateCategory_fixed.value = "2.70";
			window.txtSAHLRateCategory.value = "2.40"; 
			window.txtSAHLRateCategory_fixed.value = "2.20";
		}
		
			if (sRateCategory1 == "4") {
			//dInterestRateSAHL = dSAHLInterestRate+0.60;
			dInterestRateSAHL = dSAHLInterestRate+0.50;
			//window.txtSAHLRateCategory.value = "2.7";
			//window.txtSAHLRateCategory_fixed.value = "2.70";
			window.txtSAHLRateCategory.value = "2.40"; 
			window.txtSAHLRateCategory_fixed.value = "2.20"; 
		}
		
		// Super Rate 
		window.txtSAHLRateCategory_super.value = NumberToString(parseFloat(window.txtSAHLRateCategory.value) - d_DiscountRate*100,2);
		
		window.text1.value = (sPTIRateCategory + " " + dPTI1 + " " +sRateCategory1); 

	    var SAHLFixedRateLinked = parseFloat(SAHLFixedRate) + parseFloat(window.txtSAHLRateCategory.value);
    	
	    // VariFix: Fixed portion interest rate from DB	
	    window.InterestRateSAHL_fixed.value = NumberToString(SAHLFixedRateLinked,2);

        // Variable: VariFix too.
        window.InterestRateSAHL_var.value  = NumberToString(dInterestRateSAHL,2);
		
		// Super Rate:
		window.txtRateSAHL_super.value  = NumberToString(dInterestRateSAHL - d_DiscountRate*100,2);
		
		//alert(dPrincipal + " " + dInterestRateSAHL + " " +  iInterestPeriods + " " + iTerm);
		
		// VariFix: variables used to calculate fixed and variable instalment amounts
		var dInterestRateSAHL_fixed = window.InterestRateSAHL_fixed.value;		
		var dFixedPortionLoanAmount = window.txtFixedPortion.value;
		var dVariablePortionLoanAmount = window.txtVariablePortion.value;
		
		// Variable: Calculate total instalments
		var dInstalmentSAHL = CalculateInstallment(dTotalLoanSAHL, dInterestRateSAHL/100, iInterestPeriods, iTerm, iType);	

		// VariFix: Calculate fixed portion of the instalment
		if (dFixedPortionLoanAmount > 0) {
			var dInstalmentSAHL_fixed = CalculateInstallment(dFixedPortionLoanAmount, dInterestRateSAHL_fixed/100, iInterestPeriods, iTerm, iType);	
		}
		else {
			var dInstalmentSAHL_fixed = 0;
		}
		
		// VariFix: Calculate variable portion of the instalment
		if (dVariablePortionLoanAmount > 0) {
			var dInstalmentSAHL_variable = CalculateInstallment(dVariablePortionLoanAmount, dInterestRateSAHL/100, iInterestPeriods, iTerm, iType);	
		}
		else {
			var dInstalmentSAHL_variable = 0;
		}	
		
		var dInterestRateSAHL_super = dInterestRateSAHL - d_DiscountRate*100
		
		// Super Rate: 
		var dInstalmentSAHL_super = CalculateInstallment(dTotalLoanSAHL, dInterestRateSAHL_super/100, iInterestPeriods, iTerm, iType);	
		
		var dIncomeSAHL = dInstalmentSAHL *100/dPTI1;
		if (dIncomeSAHL < 7000) dIncomeSAHL = 7000;
		
		window.txtRateSAHL.value = NumberToString(dInterestRateSAHL,2); 
		window.txtIncomeSAHL.value = NumberToString(dIncomeSAHL,2); 
		
		// Calculate Total Month Instalment
        switch (i_Product) {
        
            case "<%=Variable_Product_Key %>": // Variable
            
                window.txtInstalmentSAHL.value = NumberToString(dInstalmentSAHL,2);
                
                break;
            
             case  "<%=New_Variable_Key %>": // New Variable
            
                window.txtInstalmentSAHL.value = NumberToString(dInstalmentSAHL,2);
                
                break;
                    
                
            case "<%=VariFix_Product_Key %>": // VariFix
            
		        if (bShowVariable == false) {
        		
			        window.txtInstalmentSAHL_fixed.value = NumberToString(dInstalmentSAHL_fixed,2);
			        window.txtInstalmentSAHL_variable.value = NumberToString(dInstalmentSAHL_variable,2);
			        window.txtInstalmentSAHL.value = NumberToString((parseFloat(dInstalmentSAHL_fixed) + parseFloat(dInstalmentSAHL_variable)),2);
		        }
		        else if (bShowVariable == true){
        		
			        window.txtInstalmentSAHL.value = NumberToString(dInstalmentSAHL,2);
        			
		        }
		            
                break;
            
            case "<%=SuperRate_Product_Key %>": // Super Rate
            
                window.txtInstalmentSAHL_super.value = NumberToString(dInstalmentSAHL_super,2);
                window.txtInstalmentSAHL.value = NumberToString(dInstalmentSAHL,2);
                
                update_super_details();
                
                break;
               
        }
        
		// Calculate and display the interest inly installment if required.
		if (window.cbIntOnly.checked) {
			var dIntOnlyInstallment;
			dIntOnlyInstallment = CalculateIntOnlyInstallment(dTotalLoanSAHL,dInterestRateSAHL/100)
			window.txtIntOnlyInst.value = NumberToString(dInstalmentSAHL,2);
			window.txtInstalmentSAHL.value = NumberToString(dIntOnlyInstallment,2);
		} 
		
		// ** PTI
		// Bank
		window.txtPTIBank.value = NumberToString(25.0,2);
		
		// Super Rate
		window.txtPTISAHL_super.value = NumberToString(dInstalmentSAHL_super/dIncomeSAHL*100,2)
		
		dInterestRateBank = parseFloat(window.txtRateBank.value );
		var dInstalmentBank = CalculateInstallment(dTotalLoanBank, dInterestRateBank/100, iInterestPeriods, iTerm, iType);	
		var dIncomeBank = dInstalmentBank *100/dPTI1;
		
		window.txtIncomeBank.value = NumberToString(dIncomeBank,2); 
		window.txtInstalmentBank.value = NumberToString(dInstalmentBank,2);
		
		//Saving
		var dLifeInterestSAHL = CalculateLifeInterest(dTotalLoanSAHL, dInterestRateSAHL/100, iInterestPeriods, iTerm, iType);
		var dLifeInterestBank = CalculateLifeInterest(dTotalLoanBank, dInterestRateBank/100, iInterestPeriods, iTerm, iType);
		window.txtSavingsTerm.value = NumberToString(dLifeInterestBank-dLifeInterestSAHL,2); 

	}
	
	
	
}

function update_super_details() {

    window.td_LoyaltyBonus.innerHTML = getSuperRateBonus(parseFloat(window.txtTotalLoanSAHL.value), parseFloat(txtRateSAHL.value)/100, d_DiscountRate, txtTerm.value );
    
    window.td_IndicativeInstalment.innerHTML = getSuperRateInstalments(parseFloat(window.txtTotalLoanSAHL.value), parseFloat(txtRateSAHL.value)/100, d_DiscountRate, txtTerm.value );	                    

}

function LTVPTIMatrixGetPTI(LoanPurpose, MaxLoan, EmploymentType, LTV) {
//alert(MaxLoan);	
	if (MaxLoan < 0)
		return "0";
	if (LTV > 100)
		return "0";	
	if (window.cbIntOnly.checked == true && LTV > <%=INTEREST_ONLY_LTV%>)
		return "0";	
		
// ************************************************************************************************************************ 
//	                    Salaried
// ************************************************************************************************************************ 	
	
	if (EmploymentType == "Salaried") {
		if (MaxLoan <= 1500000)
		{
			if (LoanPurpose == "NewPurchase") 
			{ // New
				window.cbLTVPTI.disabled = true;
				if (LTV <= 80) return "301";
				else if (LTV <= 90) return "302";
				else if (LTV <= 95) return "253";
				else if (MaxLoan >= 250000)
				{
				    if (LTV <= 100) return "254"; 						
				}    	 
			else return "0";
			} // New
			
			if (LoanPurpose == "SwitchAndSave" ) 
			{ 	// Switch
				window.cbLTVPTI.disabled = true;
				if (LTV <= 80) return "301";
				else if (LTV <= 90) return "302";
				else if (LTV <= 95) return "253";
				else return "0";
			} // Switch
			
			if (LoanPurpose == "Refinance") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false) 
				{
					if (LTV <= 75) return "251";
					else if (LTV <= 80) return "252";
					else if (LTV <= 85) return "253";
				}
				if (window.cbLTVPTI.checked == true) 
				{
					if (LTV <= 75) return "251";
					else if (LTV <= 80) return "252";
					else if (LTV <= 85) return "253";
				}	
			else return "0";
			} // Refinance

		} //MaxLoan <= 1500000
		
		if (MaxLoan <= 2500000){
			if (LoanPurpose == "NewPurchase") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false)
				{
					if (LTV <= 80) return "301";
					else if (LTV <= 85) return "302";
					else if (LTV <= 90) return "253";
					else if (MaxLoan >= 250000) 
					{
						if (LTV <= 95) 
						{
						window.cbLTVPTI.disabled=true;
						return "254";
						}
					}
				}
				if (window.cbLTVPTI.checked == true)
				{
						if (LTV <= 80) return "301";
						else if (LTV <= 85) return "302";
						else if (LTV <= 90) return "253";
						else if (LTV <= 95) return "254";
				}		
				else return "0";
		   }
			
			if (LoanPurpose == "SwitchAndSave") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false)
				{
					if (LTV <= 80) return "251";
					else if (LTV <= 85) return "252";
					else if (LTV <= 90) return "253";
				}
				if (window.cbLTVPTI.checked == true)
				{
						if (LTV <= 80) return "251";
						else if (LTV <= 85) return "252";
						else if (LTV <= 90) return "253";
				}		
				else return "0";
		    }
		   	
			if (LoanPurpose == "Refinance") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false)
				{
					if (LTV <= 70) return "251";
					else if (LTV <= 75) return "252";
					else if (LTV <= 80) return "253";
					//else if (LTV <= 85) return "204";
				}
				if (window.cbLTVPTI.checked == true)
				{
					if (LTV <= 70) return "251";
					else if (LTV <= 75) return "252";
					else if (LTV <= 80) return "253";
				}		
				else return "0";
			} // Refinance
		
		} //MaxLoan <= 2500000
		
		if (MaxLoan <= 5000000){
			if (LoanPurpose == "NewPurchase") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false) 
				{
				    if (LTV <= 75) return "251";
				    else if (LTV <= 80) return "252";
				    else if (LTV <= 85) return "253";
				}    
				if (window.cbLTVPTI.checked == true) 
				{
				    if (LTV <= 75) return "251";
				    else if (LTV <= 80) return "252";
				    else if (LTV <= 85) return "253";
				}	 
				else return "0";
			}
			
			if (LoanPurpose == "SwitchAndSave") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false) 
				{
				    if (LTV <= 75) return "251";
				    else if (LTV <= 80) return "252";
				    else if (LTV <= 85) return "253";
				}    
				if (window.cbLTVPTI.checked == true) 
				{
				    if (LTV <= 75) return "251";
				    else if (LTV <= 80) return "252";
				    else if (LTV <= 85) return "253";
				}	 
				else return "0";
			}

			if (LoanPurpose == "Refinance") {
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false) {
				    if (LTV <= 70) return "251";
				    else if (LTV <= 75) return "252";
				    //else if (LTV <= 75) return "203";
				}
				if (window.cbLTVPTI.checked == true) {
				    if (LTV <= 70) return "251";
				    else if (LTV <= 75) return "252";
				   // else if (LTV <= 70) return "253";
				}	
				else return "0";
			}
		
		}//MaxLoan <= 5000000
				
	} // Salaried
	
// ************************************************************************************************************************ 
//	                    SelfEmployed
// ************************************************************************************************************************ 	

if (EmploymentType == "SelfEmployed") {
	window.cbLTVPTI.disabled = true ;
	if (MaxLoan <= 1500000){
		if (LoanPurpose == "NewPurchase") 
		{
			if (LTV <= 80) return "251";
			else if (LTV <= 85) return "252";
			else if (LTV <= 90) return "203";
			else return "0";
		}
		if (LoanPurpose == "SwitchAndSave") 
		{
			if (LTV <= 80) return "251";
			else if (LTV <= 85) return "252";
			else if (LTV <= 90) return "203";
			else return "0";
		}

		if (LoanPurpose == "Refinance") 
		{
			if (LTV <= 70) return "251";
			else if (LTV <= 75) return "202";
			else if (LTV <= 80) return "203";
			else return "0";
		}
	}	// MaxLoan <= 1500000
	
	if (MaxLoan <= 2500000){ 
		if (LoanPurpose == "NewPurchase") 
		{
			if (LTV <= 80) return "251";
			else if (LTV <= 85) return "202";
			else if (MaxLoan >= 200000) 
				{
					if (LTV <= 90) return "203";
				}	
			else return "0";
		}
		
		if (LoanPurpose == "SwitchAndSave") 
		{
			if (LTV <= 75) return "251";
			else if (LTV <= 80) return "202";
			else if (MaxLoan >= 200000) 
				{
					if (LTV <= 85) return "203";
				}	
			else return "0";
		}

		if (LoanPurpose == "Refinance") 
		{
			if (LTV <= 65) return "251";
			else if (LTV <= 75) return "202";
			//else if (LTV <= 75) return "203";
			else return "0";
		}
	} // MaxLoan <= 2500000
	
	if (MaxLoan <= 5000000)
	{
		if (LoanPurpose == "NewPurchase") 
		{
			if (LTV <= 75) return "251";
			else if (LTV <= 80) return "202";
			else if (LTV <= 85) return "203";
			else return "0";
		}
		if (LoanPurpose == "SwitchAndSave") 
		{
			if (LTV <= 70) return "251";
			else if (LTV <= 75) return "202";
			else if (LTV <= 80) return "203";
			else return "0";
		}


		if (LoanPurpose == "Refinance") 
		{
			if (LTV <= 60) return "251";
			else if (LTV <= 70) return "202";
			//else if (LTV <= 70) return "203";
			else return "0";
		}
	} // MaxLoan <= 5000000
		
} // EmploymentType == "SelfEmployed"


// ************************************************************************************************************************ 
//	                    Subsidied
// ************************************************************************************************************************
	if (EmploymentType == "Subsidy") 
	{
		if (MaxLoan <= 1500000)
		{
			if (LoanPurpose == "NewPurchase") 
			{
				window.cbLTVPTI.disabled = true;
				if (LTV <= 80) return "301";
				else if (LTV <= 90) return "302";
				else if (LTV <= 95) return "253";
				else if (MaxLoan >= 250000)
				{
				    if (LTV <= 100) return "254"; 						
				}    	 
			else return "0";
			}
			if (LoanPurpose == "SwitchAndSave") 
			{
				window.cbLTVPTI.disabled = true;
				if (LTV <= 80) return "301";
				else if (LTV <= 90) return "252";
				else if (LTV <= 95) return "253";
				else return "0";
			}
			
			if (LoanPurpose == "Refinance") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false) 
				{
					if (LTV <= 75) return "251";
					else if (LTV <= 80) return "252";
					else if (LTV <= 85) return "253";
				}
				if (window.cbLTVPTI.checked == true) 
				{
					if (LTV <= 75) return "251";
					else if (LTV <= 80) return "252";
					else if (LTV <= 85) return "253";
				}	
			else return "0";
			}

		 } //MaxLoan <= 1500000
		
		if (MaxLoan <= 2500000)
		{
			if (LoanPurpose == "NewPurchase") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false)
				{
					if (LTV <= 80) return "301";
					else if (LTV <= 85) return "302";
					else if (LTV <= 90) return "253";
					else if (MaxLoan >= 250000) 
					{
						if (LTV <= 95) 
						{
						window.cbLTVPTI.disabled=true;
						return "254";
						}
					}
				}
				if (window.cbLTVPTI.checked == true)
				{
						if (LTV <= 80) return "301";
						else if (LTV <= 85) return "302";
						else if (LTV <= 90) return "253";
						else if (LTV <= 95) return "254";
				}		
				else return "0";
		    } // New
		   if (LoanPurpose == "SwitchAndSave") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false)
				{
					if (LTV <= 80) return "251";
					else if (LTV <= 85) return "252";
					else if (LTV <= 90) return "253";
				}
				if (window.cbLTVPTI.checked == true)
				{
						if (LTV <= 80) return "251";
						else if (LTV <= 85) return "252";
						else if (LTV <= 90) return "253";
				}		
				else return "0";
		    } // Switch

			if (LoanPurpose == "Refinance") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false)
				{
					if (LTV <= 70) return "251";
					else if (LTV <= 75) return "252";
					else if (LTV <= 80) return "253";
					//else if (LTV <= 85) return "204";
				}
				if (window.cbLTVPTI.checked == true)
				{
					if (LTV <= 70) return "251";
					else if (LTV <= 75) return "252";
					else if (LTV <= 80) return "253";
				}		
				else return "0";
			}
		
		} //MaxLoan <= 2500000
		
		if (MaxLoan <= 5000000)
		{
			if (LoanPurpose == "NewPurchase") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false) 
				{
				    if (LTV <= 75) return "251";
				    else if (LTV <= 80) return "252";
				    else if (LTV <= 85) return "253";
				}
				if (window.cbLTVPTI.checked == true) 
				{
				    if (LTV <= 75) return "251";
				    else if (LTV <= 80) return "252";
				    else if (LTV <= 85) return "253";
				}	 
				else return "0";
			} // New
			
			if (LoanPurpose == "SwitchAndSave") 
			{
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false) 
				{
				    if (LTV <= 75) return "251";
				    else if (LTV <= 80) return "252";
				    else if (LTV <= 85) return "253";
				}
				if (window.cbLTVPTI.checked == true) 
				{
				    if (LTV <= 75) return "251";
				    else if (LTV <= 80) return "252";
				    else if (LTV <= 85) return "253";
				}	 
				else return "0";
			} // Switch

			if (LoanPurpose == "Refinance") {
				window.cbLTVPTI.disabled = false;
				if (window.cbLTVPTI.checked == false) {
				    if (LTV <= 70) return "251";
				    else if (LTV <= 75) return "252";
				   // else if (LTV <= 75) return "203";
				}
				if (window.cbLTVPTI.checked == true) {
				    if (LTV <= 70) return "251";
				    else if (LTV <= 75) return "252";
				   // else if (LTV <= 70) return "253";
				}	
				else return "0";
			}
		
		}//MaxLoan <= 5000000		
		
	} // Subsidied		
	
		return "0";
}


function ValidateNoValues (num, field)
{
	if (num.length == 0 || num == 0 )
		return false;
	else
		return true;
	
}

function ValidateEmpty (num, field)
{
	if (num.length == 0)
	{
		window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>Please enter a value for "  + field.name + "</strong></font></div>";
		field.focus();
		return false;
	}
	else
		return true;
	
}

function ValidatePositiveNumber (num, field)
{
	var decimal = 0;
   for (var i = 0; i < num.length; i++)
   {
       ch = num.charAt(i)
       if (ch == '.')
			decimal++;
	   
       if (((ch < "0" || "9" < ch) && ch != '.') || decimal > 1)
       {
				window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>Please enter a valid positive number for "  + field.name + "</strong></font></div>";
				field.focus();
			return false;
	   }
   }
   return true;
}

function ValidateMaximum(num, field, amt)
{
	if (parseFloat(num) > amt)
    {
		window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>Please enter a value less than " + amt + " for " + field.name + "</strong></font></div>";
		field.focus();
		return false;
	}
	else
		return true;
}

function ValidateMinimum(num, field, amt)
{
	if (parseFloat(num) < amt)
    {
 		window.tdErrorLine.innerHTML = "<div align=center><font color=#FF0000><strong>Please enter a value greater than " + amt + " for " + field.name + "</strong></font></div>";
		field.focus();
		return false;
	}
	else
		return true;
}

function ValidateData() {

		window.tdErrorLine.innerHTML = "<font color=#d2b48c>" + "_" + "</font>";
		window.tdQualified.innerHTML = "<font color=#d2b48c>" + "_" + "</font>"
	
	if (ValidateEmpty(txtTerm.value,txtTerm) == false) {DisableQualify(); return false;}
	if (ValidatePositiveNumber(txtTerm.value,txtTerm) == false)  {DisableQualify(); return false;}
	if (ValidateMaximum(txtTerm.value,txtTerm,480) == false) {DisableQualify(); return false;}
	if (ValidateMinimum(txtTerm.value,txtTerm,12) == false) {DisableQualify(); return false;}
	

	if (sLoanPurpose == "SwitchAndSave") {
		if (ValidateEmpty(txtMarketValue.value,txtMarketValue) == false) {DisableQualify(); return false;}
		if (ValidatePositiveNumber(txtMarketValue.value,txtMarketValue) == false) {DisableQualify(); return false;}
		if (ValidateMaximum(txtMarketValue.value,txtMarketValue,10000000) == false) {DisableQualify(); return false;}
		if (ValidateMinimum(txtMarketValue.value,txtMarketValue,170000) == false) {DisableQualify(); return false;}
				
		if (ValidateEmpty(txtLoanAmount.value,txtLoanAmount) == false) {DisableQualify(); return false;}
		if (ValidatePositiveNumber(txtLoanAmount.value,txtLoanAmount) == false)  {DisableQualify(); return false;}
		if (ValidateMaximum(txtLoanAmount.value,txtLoanAmount,5000000) == false)  {DisableQualify(); return false;}
		//if (ValidateMinimum(txtLoanAmount.value,txtLoanAmount,140000) == false) {DisableQualify(); return false;}
	
		if (parseFloat(txtLoanAmount.value) > parseFloat(txtMarketValue.value)) {
			alert("Please enter a Loan Amount that is less than Market Value");
			DisableQualify();
			return false;
		}
	}

	else if (sLoanPurpose == "NewPurchase") {
		if (ValidateEmpty(txtPurchasePrice.value,txtPurchasePrice) == false) {DisableQualify(); return false;}
		if (ValidatePositiveNumber(txtPurchasePrice.value,txtPurchasePrice) == false)  {DisableQualify(); return false;}
		if (ValidateMaximum(txtPurchasePrice.value,txtPurchasePrice,10000000) == false)  {DisableQualify(); return false;}
		if (ValidateMinimum(txtPurchasePrice.value,txtPurchasePrice,140000) == false) {DisableQualify(); return false;}
		if (i_Product == "<%=SuperRate_Product_Key %>") {
		if (ValidateMinimum(txtPurchasePrice.value,txtPurchasePrice,250000) == false) {DisableQualify(); return false;}
		}	

		if (ValidateEmpty(txtCashDeposit.value,txtCashDeposit) == false) {DisableQualify(); return false;}
		if (ValidatePositiveNumber(txtCashDeposit.value,txtCashDeposit) == false)  {DisableQualify(); return false;}
		if (ValidateMaximum(txtCashDeposit.value,txtCashDeposit,5000000) == false)  {DisableQualify(); return false;}
		if (ValidateMinimum(txtCashDeposit.value,txtCashDeposit,0) == false) {DisableQualify(); return false;}
	}
	
	else if (sLoanPurpose == "Refinance") {
		if (ValidateEmpty(txtMarketValue.value,txtMarketValue) == false) {DisableQualify(); return false;}
		if (ValidatePositiveNumber(txtMarketValue.value,txtMarketValue) == false) {DisableQualify(); return false;}
		if (ValidateMaximum(txtMarketValue.value,txtMarketValue,10000000) == false) {DisableQualify(); return false;}
		if (ValidateMinimum(txtMarketValue.value,txtMarketValue,170000) == false)  {DisableQualify(); return false;}
		
		if (ValidateEmpty(txtCashRequired.value,txtCashRequired) == false) {DisableQualify(); return false;}
		if (ValidatePositiveNumber(txtCashRequired.value,txtCashRequired) == false)  {DisableQualify(); return false;}
		
		if (ValidateMaximum(txtCashRequired.value,txtCashRequired,parseFloat(txtMarketValue.value)) == false)  {DisableQualify(); return false;}
		
		if (ValidateMinimum(txtCashRequired.value,txtCashRequired,140000) == false) {DisableQualify(); return false;}
		
		if (i_Product == "<%=SuperRate_Product_Key %>") {
		if (ValidateMinimum(txtCashRequired.value,txtCashRequired,250000) == false) {DisableQualify(); return false;}
		}	
		
		if (parseFloat(txtCashRequired.value) > parseFloat(txtMarketValue.value)) {
			alert("Please enter a Cash Required Amount that is less than Market Value");
			DisableQualify();
			return false;
		}
	}
	
	return true;

}

function ddlPurpose_onchange () {

	if (window.ddlPurpose.selectedIndex  == 0) 	 
		SwitchAndSave();
	else if  (window.ddlPurpose.selectedIndex  == 1) 
		NewPurchase();
	else if   (window.ddlPurpose.selectedIndex  == 2) 
		Refinance();
}

function txtMarketValue_onpropertychange() {
	
	//if (window.txtMarketValue.value == "" || window.txtMarketValue.value == 0 ) return;
	if (ValidateNoValues(txtMarketValue.value) == false ) return;
	if (ValidatePositiveNumber(txtMarketValue.value,txtMarketValue) == false) {DisableQualify(); return;}
	if (ValidateMaximum(txtMarketValue.value,txtMarketValue,10000000) == false)  {DisableQualify(); return;}
	if (ValidateMinimum(txtMarketValue.value,txtMarketValue,170000) == false) {DisableQualify(); return;}
	
	Calculate();
	calc_amounts("fixed");	
	calculate_flexi_fix("percentage");
}

function txtLoanAmount_onpropertychange() {
	if (ValidateNoValues(txtLoanAmount.value) == false ) return;
	if (ValidatePositiveNumber(txtLoanAmount.value,txtLoanAmount) == false)  {DisableQualify(); return;}
	if (ValidateMaximum(txtLoanAmount.value,txtLoanAmount,5000000) == false)  {DisableQualify(); return;}
	//if (ValidateMinimum(txtLoanAmount.value,txtLoanAmount,100000) == false) {DisableQualify(); return;}
	
	Calculate();
	calc_amounts("fixed");	
	calculate_flexi_fix("percentage");
}


function txtPurchasePrice_onpropertychange() {
	if (ValidateNoValues(txtPurchasePrice.value) == false ) return;
	if (ValidatePositiveNumber(txtPurchasePrice.value,txtPurchasePrice) == false)  {DisableQualify(); return;}
	if (ValidateMaximum(txtPurchasePrice.value,txtPurchasePrice,10000000) == false)  {DisableQualify(); return;}
	if (ValidateMinimum(txtPurchasePrice.value,txtPurchasePrice,140000) == false) {DisableQualify(); return;}
	
	Calculate();
	calc_amounts("fixed");	
	calculate_flexi_fix("percentage");
}

function txtCashDeposit_onpropertychange() {
	if (ValidateNoValues(txtCashDeposit.value) == false ) return;
	if (ValidatePositiveNumber(txtCashDeposit.value,txtCashDeposit) == false)  {DisableQualify(); return;}
	//if (ValidateMinimum(inCashDeposit.value,inCashDeposit,2000000) == false)  {DisableQualify(); return;}
	
	Calculate();
	calc_amounts("fixed");	
	calculate_flexi_fix("percentage");
}

function txtCashRequired_onkeypress() {
	if (ValidateNoValues(txtCashRequired.value) == false ) return;
	if (ValidatePositiveNumber(txtCashRequired.value,txtCashRequired) == false)  {DisableQualify(); return;}
	//if (ValidateMinimum(txtCashRequired.value,txtCashRequired,100000) == false)  {DisableQualify(); return;}
	
	Calculate();
	calc_amounts("fixed");	
	calculate_flexi_fix("percentage");
}

function txtTerm_onpropertychange() {
	if (ValidateNoValues(txtTerm.value) == false ) return;
	if (ValidatePositiveNumber(txtTerm.value,txtTerm) == false)  {DisableQualify(); return;}
	if (window.cbExtTerm.checked == false) {
		if (ValidateMaximum(txtTerm.value,txtTerm,240) == false) {DisableQualify(); return;}
	}	
	if (ValidateMinimum(txtTerm.value,txtTerm,12) == false) {DisableQualify(); return;}
	
	Calculate();
	calc_amounts("fixed");	
	calculate_flexi_fix("percentage");
}


function ddEmploymentType_onclick() {
		Calculate();
}


function cbTotalFees_onclick() {
	if (trim(window.txtSubTotalFeesSAHL.value) == "") 
		return;

	Calculate();
}

function cbLTVPTI_onclick() {
	Calculate();
}

function cbLTVPTI_onchange() {
	Calculate();
}

function ddTransferType_onclick() {
	if (trim(window.txtSubTotalFeesSAHL.value) == "") 
		return;

		Calculate();
}

function popup(url) {
		window.open(url,"","left=200,top=90,width=400,height=400,toolbar=no,menubar=no,scrollbars=yes,location=no,directories=no,status=no,resizable=yes,dependent=no");
}

function cbExtTerm_onclick() {
	if (window.cbExtTerm.checked == true)
		{
		txtTerm.value = 276;
		window.txtTerm.disabled = true;
		window.txtExtTerm.value = "Yes";
		}
	else
		{
		txtTerm.value = 240;	
		window.txtTerm.disabled = false;
		window.txtExtTerm.value = "No";
		}
		
}

function cbIntOnly_onclick() {
	if (window.cbIntOnly.checked)
		window.IntOnlyInst.style.display = "";
	else
		window.IntOnlyInst.style.display = "none";
	
	Calculate();
}

function getAge(dateString,dateType) {
/*
   function getAge
   parameters: dateString dateType
   returns: boolean

   dateString is a date passed as a string in the following
   formats:

   type 1 : 19970529
   type 2 : 970529
   type 3 : 29/05/1997
   type 4 : 29/05/97

   dateType is a numeric integer from 1 to 4, representing
   the type of dateString passed, as defined above.

   Returns string containing the age in years, months and days
   in the format yyy years mm months dd days.
   Returns empty string if dateType is not one of the expected
   values.
*/

    var now = new Date();
    var today = new Date(now.getYear(),now.getMonth(),now.getDate());

    var yearNow = now.getYear();
    var monthNow = now.getMonth();
    var dateNow = now.getDate();

    if (dateType == 1)
        var dob = new Date(dateString.substring(0,4),
                            dateString.substring(4,6)-1,
                            dateString.substring(6,8));
    else if (dateType == 2)
        var dob = new Date(dateString.substring(0,2),
                            dateString.substring(2,4)-1,
                            dateString.substring(4,6));
    else if (dateType == 3)
        var dob = new Date(dateString.substring(6,10),
                            dateString.substring(3,5)-1,
                            dateString.substring(0,2));
    else if (dateType == 4)
        var dob = new Date(dateString.substring(6,8),
                            dateString.substring(3,5)-1,
                            dateString.substring(0,2));
    else
        return '';

    var yearDob = dob.getFullYear();
    var monthDob = dob.getMonth();
    var dateDob = dob.getDate();

    yearAge = yearNow - yearDob;
   
    if (monthNow >= monthDob)
        var monthAge = monthNow - monthDob;
    else {
        yearAge--;
        var monthAge = 12 + monthNow -monthDob;
    }

    if (dateNow >= dateDob)
        var dateAge = dateNow - dateDob;
    else {
        monthAge--;
        var dateAge = 31 + dateNow - dateDob;

        if (monthAge < 0) {
            monthAge = 11;
            yearAge--; 
        }
    }

    return yearAge + ' years ' + monthAge + ' months ' + dateAge + ' days';
  
}

//------------------------------------------------------------------------------------------------------------------------------------------------
//          FLEXI
//------------------------------------------------------------------------------------------------------------------------------------------------

function show_flexi() {

		window.fixed_variable_total.style.display = "";
		window.fixed_variable_instalment.style.display = "";	
			
		window.qualifies_sahl_rate.style.display = "none";			
		window.tr_fixed_portion_interest_rate.style.display = "";		
		
		window.tr_fixed_variable_amount.style.visibility = "visible";
		window.tr_fixed_variable_percent.style.visibility = "visible";
		
		window.fixed_percent.style.visibility = "visible";
		window.variable_percent.style.visibility = "visible";
		
		window.tr_fixed_portion_interest_rate.style.display = "";
		
		window.td_fixed_1.style.display = "";		
		window.td_fixed_2.style.display = "";
		window.td_fixed_3.style.display = "";
		window.td_fixed_4.style.display = "";
		window.td_fixed_5.style.display = "";
		window.td_fixed_6.style.display = "";
		window.td_fixed_6a.style.display = "";
		window.td_fixed_7.style.display = "";
		window.td_fixed_8.style.display = "";
		window.td_fixed_9.style.display = "";
		window.td_fixed_10.style.display = "";		
		window.td_fixed_11.style.display = "";	
		
		window.div_flexi.style.display = "";
		window.div_core.style.display = "none";
		window.div_super.style.display = "none";
		
		window.tr_savings.style.display = "none";
		
		btnGoToFlexi.style.display = "";
		window.tr_market_rate_type.style.display = "";
		
		window.tr_super.style.visibility = "hidden";
		window.ExtTerm.style.visibility = "hidden";
		
}



function show_super_rate() {

		window.fixed_variable_total.style.display = "none";
		window.fixed_variable_instalment.style.display = "none";
		window.qualifies_sahl_rate.style.display = ""
		window.tr_fixed_portion_interest_rate.style.display = "none";
		
		window.tr_fixed_variable_amount.style.visibility = "hidden";
		window.tr_fixed_variable_percent.style.visibility = "hidden";
		
		window.fixed_percent.style.visibility = "hidden";
		window.variable_percent.style.visibility = "hidden";
		
		window.td_fixed_1.style.display = "none";
		window.td_fixed_2.style.display = "none";
		window.td_fixed_3.style.display = "none";
		window.td_fixed_4.style.display = "none";
		window.td_fixed_5.style.display = "none";
		window.td_fixed_6.style.display = "none";
		window.td_fixed_6a.style.display = "none";
		window.td_fixed_7.style.display = "none";
		window.td_fixed_8.style.display = "none";
		window.td_fixed_9.style.display = "none";
		window.td_fixed_10.style.display = "none";
		window.td_fixed_11.style.display = "none";
		
		window.div_flexi.style.display = "none";
		window.div_core.style.display = "none";
		window.div_super.style.display = "none";    // v2: super
		
		//window.tr_ltv.style.display = "";
		//window.tr_pti.style.display = "";
		
		//window.tbl_PTI.style.display = "none";
		window.tr_savings.style.display = "none";   // v2: super
		btnGoToFlexi.style.display = "none";
		window.tr_market_rate_type.style.display = "none";
		
		window.savings.innerHTML = "<strong>Loyalty Bonus</strong>"
		window.td_savings.style.display = "none";
		
		window.tr_super.style.visibility = "visible";
		window.ExtTerm.style.visibility = "visible";
				
}

function show_core() {

		window.fixed_variable_total.style.display = "none";
		window.fixed_variable_instalment.style.display = "none";
		window.qualifies_sahl_rate.style.display = ""
		window.tr_fixed_portion_interest_rate.style.display = "none";
		
		window.tr_fixed_variable_amount.style.visibility = "hidden";
		window.tr_fixed_variable_percent.style.visibility = "hidden";
		
		window.fixed_percent.style.visibility = "hidden";
		window.variable_percent.style.visibility = "hidden";
		
		window.td_fixed_1.style.display = "none";
		window.td_fixed_2.style.display = "none";
		window.td_fixed_3.style.display = "none";
		window.td_fixed_4.style.display = "none";
		window.td_fixed_5.style.display = "none";
		window.td_fixed_6.style.display = "none";
		window.td_fixed_6a.style.display = "none";
		window.td_fixed_7.style.display = "none";
		window.td_fixed_8.style.display = "none";
		window.td_fixed_9.style.display = "none";
		window.td_fixed_10.style.display = "none";
		window.td_fixed_11.style.display = "none";
		
		window.div_flexi.style.display = "none";
		window.div_core.style.display = "";
		window.div_super.style.display = "none";
		//window.tr_ltv.style.display = "";
		//window.tr_pti.style.display = "";
		
		//window.tbl_PTI.style.display = "none";
		window.tr_savings.style.display = "";
		btnGoToFlexi.style.display = "none";
		window.tr_market_rate_type.style.display = "none";
		
		window.tr_super.style.visibility = "hidden";
		window.ExtTerm.style.visibility = "hidden";
		
}

function btnShowVariable_onclick() {

    if (bShowVariable==true) {

		window.fixed_variable_total.style.display = "none";
		window.fixed_variable_instalment.style.display = "none";
		
		window.qualifies_sahl_rate.style.display = ""		
		window.tr_fixed_portion_interest_rate.style.display = "none";
			
		
		//window.tr_fixed_variable_amount.style.visibility = "hidden";
		//window.tr_fixed_variable_percent.style.visibility = "hidden";		
		
		window.tr_savings.style.display = "";
		window.tr_market_rate_type.style.display = "none";
		
		window.td_fixed_1.style.display = "none";
		window.td_fixed_2.style.display = "none";
		window.td_fixed_3.style.display = "none";
		window.td_fixed_4.style.display = "none";
		window.td_fixed_5.style.display = "none";
		window.td_fixed_6.style.display = "none";
		window.td_fixed_6a.style.display = "none";
		window.td_fixed_7.style.display = "none";
		window.td_fixed_8.style.display = "none";
		window.td_fixed_9.style.display = "none";
		window.td_fixed_10.style.display = "none";
		window.td_fixed_11.style.display = "none";
		
		window.td_titles.width = 486
		window.td_var_flexi.innerHTML = "VariFix";
		
		window.div_flexi.style.display = "";
		window.div_core.style.display = "none";
		
		btnGoToFlexi.style.display = "";   	
		
		if (parseFloat(txtTotalLoanSAHL.value) > 0)
		    Calculate();
		
		bShowVariable = false;
		
    }   
    else {
    
		window.fixed_variable_total.style.display = "";
		window.fixed_variable_instalment.style.display = "";				
		window.qualifies_sahl_rate.style.display = "none";			
					
		window.fixed_percent.style.visibility = "visible";
		window.variable_percent.style.visibility = "visible";
		
		window.tr_fixed_portion_interest_rate.style.display = "";
		window.tr_fixed_portion_interest_rate.style.display = "";
						
		//window.tr_fixed_variable_amount.style.visibility = "visible";
		//window.tr_fixed_variable_percent.style.visibility = "visible";
		
		window.tr_savings.style.display = "none";
		window.tr_market_rate_type.style.display = "";
		
		window.td_fixed_1.style.display = "";		
		window.td_fixed_2.style.display = "";
		window.td_fixed_3.style.display = "";
		window.td_fixed_4.style.display = "";
		window.td_fixed_5.style.display = "";
		window.td_fixed_6.style.display = "";
		window.td_fixed_6a.style.display = "";
		window.td_fixed_7.style.display = "";
		window.td_fixed_8.style.display = "";
		window.td_fixed_9.style.display = "";
		window.td_fixed_10.style.display = "";		
		window.td_fixed_11.style.display = "";	
		
		window.td_titles.width = 300
        window.td_var_flexi.innerHTML = "100 % Var";
        
		window.div_flexi.style.display = "";
		window.div_core.style.display = "none";	
		
		btnGoToFlexi.style.display = "";		
		
		
		if (parseFloat(txtTotalLoanSAHL.value) > 0)
            Calculate();
        
        bShowVariable = true;
        
    }

    
}




function variable_percent_onkeyup() {
	// Calculates the variable percent base on the value of the fixed percent

	if (window.variable_percent.value <=100){
				
		// Calculate variable percent
		window.fixed_percent.value = 100 - window.variable_percent.value;		
		
		calc_amounts("variable");
		
		calculate_flexi_fix("percentage");				
			
	}
	else {
		alert("Cannot have percentage greater than 100");
		window.variable_percent.value = 50;
		window.fixed_percent.value = 50;
		
		calc_amounts("variable");
		
		calculate_flexi_fix("percentage");
		
	}

}

function fixed_percent_onkeyup() {
	// Calculates the variable percent base on the value of the fixed percent

	if (window.fixed_percent.value <=100){		
		
		// Calculate variable percent
		window.variable_percent.value = 100 - window.fixed_percent.value;		
		
		calc_amounts("fixed");
		
		calculate_flexi_fix("percentage");				
			
	}
	else {
		alert("Cannot have percentage greater than 100");
				
		calc_amounts("fixed");
		
		calculate_flexi_fix("percentage");
		
	}
	
}

function calculate_flexi_fix(type) {
	// Calculates the relevant values for the flexi fix product
	// type = percentage --> Calculate fixed and variable portions 
	//						based on the total loan amount (txtTotalLoanSAHL)
	//						using percentages
	// type = amount --> Calculate fixed and variable portions 
	//						based on the Fixed / Variable Amount on entered values table
	
	
	if (window.fixed_percent.value != "") {
	    window.variable_percent.value = formatNum2(100 - parseFloat(window.fixed_percent.value));        
	}
	
	if (window.txtTotalLoanSAHL.value > 0) {
	
		if (type == "percentage") {
	                       	        		
			// Calculate fixed portion of the total loan amount
			window.txtFixedPortion.value = NumberToString(window.entered_fixed_amount.value,2); // NumberToString(Math.round(window.txtTotalLoanSAHL.value*window.fixed_percent.value/100*100)/100,2);			

			// Calculate variable portion of the total loan amount based on the fixed amount
			window.txtVariablePortion.value = NumberToString(window.txtTotalLoanSAHL.value - window.txtFixedPortion.value,2);
			
		}
		
		if (type == "amount") {
			
			var fixed_fees = NumberToString(Math.round(window.txtTotalFeesSAHL.value*window.fixed_percent.value/100*100)/100,2);
			
			window.txtFixedPortion.value = parseFloat(window.entered_fixed_amount.value) + parseFloat(fixed_fees);
			
			window.txtVariablePortion.value = parseFloat(window.entered_variable_amount.value) + parseFloat(window.txtTotalFeesSAHL.value) - parseFloat(fixed_fees);			
			
		}
		
		Calculate(); 
	}
	
}

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

function entered_fixed_amount_onkeyup() {
	// Calculates the variable portion of the principal amount based on the fixed portion
	
	if (window.txtPrincipal.value > 0) {
	
		if (parseFloat(window.entered_fixed_amount.value) <= parseFloat(window.txtPrincipal.value)){
		
			window.entered_variable_amount.value = window.txtPrincipal.value - window.entered_fixed_amount.value;		
			calc_percentages("fixed");
			calculate_flexi_fix("amount");
		}
		else {
					
			alert("Cannot enter an amount greater than the principal amount");
			window.entered_fixed_amount.value = window.txtPrincipal.value;
			window.entered_variable_amount.value = 0;
			calc_percentages("fixed");
			calculate_flexi_fix("amount");
			
		}		
			
	}		
	
}

function entered_variable_amount_onkeyup() {

	// Calculates the fixed portion of the principal amount based on the variable portion
	if (window.txtPrincipal.value > 0) {
		
		if (parseInt(window.entered_variable_amount.value) <= parseInt(window.txtPrincipal.value)) {
		
			window.entered_fixed_amount.value = window.txtPrincipal.value - window.entered_variable_amount.value;
			calc_percentages("variable");
			calculate_flexi_fix("amount");
		}
		else {
			
			alert("Cannot enter an amount greater than the principal amount");
			window.entered_variable_amount.value = window.txtPrincipal.value;
			window.entered_fixed_amount.value = 0;
			calc_percentages("variable");
			calculate_flexi_fix("amount");
		}
				
	}
	
}

function calc_percentages(type){
	// Calculates the fixed and variable percentages based on the amount entered
	// type = fixed --> Calculates fixed percent first
	// type = variable --> calculates variable percent first
	
	if (type == "fixed") {
	
		window.fixed_percent.value = Math.round(window.entered_fixed_amount.value / window.txtPrincipal.value*100);	
		window.variable_percent.value = 100 - window.fixed_percent.value;	
	}
	else if (type == "variable") {
		window.variable_percent.value = Math.round(window.entered_variable_amount.value / window.txtPrincipal.value*100);	
		window.fixed_percent.value = 100 - window.variable_percent.value;	
	}
	
	
}

function calc_amounts(type) {
	// Calculates the fixed and variable amounts based on the percentages entered
	// type = fixed --> Calculates fixed amount first
	// type = variable --> calculates variable amount first	
	
	if (type == "fixed") {
		
		window.entered_fixed_amount.value = NumberToString(Math.round(window.txtPrincipal.value * window.fixed_percent.value / 100 * 100) / 100,2);	
		window.entered_variable_amount.value = NumberToString((window.txtPrincipal.value - window.entered_fixed_amount.value),2);
		
	}
	else if (type == "variable") {
	
		window.entered_variable_amount.value = NumberToString(Math.round(window.txtPrincipal.value * window.variable_percent.value / 100 * 100) / 100,2);	
		window.entered_fixed_amount.value = NumberToString((window.txtPrincipal.value - window.entered_variable_amount.value),2);
	}
	
}

function btnGoToFlexi_onmouseover () {
		btnGoToFlexi.style.color="yellow";
}

function btnGoToFlexi_onmouseout () {
		btnGoToFlexi.style.color="white";
}

function btnGoToFlexi_onclick () {

/*
    var fFixedPercent = window.fixed_percent.value;
    var fVariablePercent = window.variable_percent.value;
       
    var sUrl = window.location.href + "&NewFixedPercent=" + fFixedPercent +
                                        "&NewVariablePercent=" + fVariablePercent;
		
		
    //window.location.href = sUrl;		
		alert(window.parent.);
		//alert(sUrl);						    

*/
		
        history.back(1);				
        
        					
		btnGoToFlexi.style.backgroundColor = "#FFA000";
		btnEnteredValues.style.backgroundColor = "SeaGreen";
		btnFees.style.backgroundColor = "SeaGreen";
		btnCalculatedValues.style.backgroundColor = "SeaGreen";
}

	

//-->
</SCRIPT>

<SCRIPT ID=clientEventHandlersVBS LANGUAGE=vbscript>
<!--

Sub dteBirthDate_Change

	if IsDate(window.dteBirthDate.Text) then
		window.tdAge.innerHTML = "<font size=3>" & getAge(window.dteBirthDate.Text ,3) & " </font>" 
	end if

End Sub

-->
</SCRIPT>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
  </HEAD>
<BODY class=Calculators LANGUAGE=javascript onload="return window_onload()">
<xml id=xmldso src="TeleCalcFees.asp"></xml>
<xml id=xmldso1 src="TeleCalcRates.asp"></xml>
<P>
<TABLE border=1 borderColor=red cellPadding=1 cellSpacing=1 
    style="Z-INDEX: 112; LEFT: 7px; WIDTH: 730px; POSITION: absolute; TOP: 400px; HEIGHT: 28px" width=700>
    <TR id=trErrorLine>
        <td id=tdErrorLine></td>
    </TR>
</TABLE>
<input class=button2 id=btnGoToFlexi name=goToFlexi 
    style="Z-INDEX: 109; LEFT: 23px; VISIBILITY: visible; WIDTH: 124px; CURSOR: hand; POSITION: absolute; TOP: 8px; HEIGHT: 29px" 
    tabIndex=20 title="goToFlexi" type=button value="Back to VariFix" LANGUAGE="javascript" 
    onclick="return btnGoToFlexi_onclick()" 
    onmouseover="return btnGoToFlexi_onmouseover()" 
    onmouseout="return btnGoToFlexi_onmouseout()">     
    
<input class=button2 id=btnEnteredValues name=btnEnteredValues style="Z-INDEX: 109; LEFT: 152px; VISIBILITY: visible; WIDTH: 124px; CURSOR: hand; POSITION: absolute; TOP: 8px; HEIGHT: 29px" tabIndex=20 title="Entered Values" type=button value="Entered Values" 
    LANGUAGE="javascript" onclick="return btnEnteredValues_onclick()" onmouseover="return btnEnteredValues_onmouseover()" onmouseout="return btnEnteredValues_onmouseout()"> 
<input class=button2 id=btnFees name=btnFees style="Z-INDEX: 110; LEFT: 281px; VISIBILITY: visible; WIDTH: 124px; CURSOR: hand; POSITION: absolute; TOP: 8px; HEIGHT: 29px" tabIndex=21 title=Fees type=button value=Fees 
    LANGUAGE="javascript" onclick="return btnFees_onclick()" onmouseover="return btnFees_onmouseover()" onmouseout="return btnFees_onmouseout()"> 
<input class=button2 id=btnCalculatedValues name=btnCalculatedValues style="Z-INDEX: 111; LEFT: 410px; VISIBILITY: visible; WIDTH: 124px; CURSOR: hand; POSITION: absolute; TOP: 8px; HEIGHT: 29px" tabIndex=41 title="Calculated Values" type=button value="Calculated Values" 
    LANGUAGE="javascript" onclick="return btnCalculatedValues_onclick()" onmouseover="return btnCalculatedValues_onmouseover()" onmouseout="return btnCalculatedValues_onmouseout()"> 

<table align=center border=0 cellPadding=0 cellSpacing=0 class=Table1 id=tblMain 
	style="Z-INDEX: 101; LEFT: 7px; WIDTH: 730px; POSITION: absolute; TOP: 39px" title="Bond Costs">
  <TBODY>
  <tr>
	<td class=Header2 colSpan=4 height=16 id=table_heading ><STRONG>Enter Values:</STRONG></td>	
  </tr>  
  <tr id=trEnteredValues>
    <td id=x width="46%" valign=top>
		<table border=0 class=table3 id=tblEnterValues>
			<TBODY>	
			    <tr>
			        <td align=right height=16 width="40%">Product&nbsp;</td>
					<td height=16 width=250>
						<SELECT id="ddlProduct" disabled language=javascript name="ff" style="WIDTH: 121px; HEIGHT: 22px"> 							
						    <%Call populateDDL("Select ProductKey, Description From [2am]..Product (nolock) Where OriginateYN = 'Y'")%>
						</SELECT>
					</td>
			    </tr>			
				<tr>
				    <td align=right height=16 width="40%">Loan Purpose&nbsp;</td>
					<td height=16 width=250>
						<SELECT id=ddlPurpose language=javascript name=ddlPurpose onchange="return ddlPurpose_onchange()" style="WIDTH: 121px; HEIGHT: 22px"> 							
						    <OPTION value=2 selected>Switch</OPTION>
							<OPTION value=3>New Purchase</OPTION>							
							<OPTION value=4>Refinance</OPTION>
						</SELECT>
					</td>			
				</tr>
				<tr id=trEmploymentType>
					<td align=right height=23>Employment Type</td>
					<td height=23 width=250>
						<SELECT id=ddEmploymentType name=ddEmploymentType language=javascript 
								onclick="return ddEmploymentType_onclick()" 
								style="WIDTH: 120px; HEIGHT: 23px" tabIndex=28> 
							<OPTION value=Salaried selected>Salaried</OPTION> 
							<OPTION value=SelfEmployed>Self Employed</OPTION>
							<OPTION value="Subsidy">Subsidy</OPTION>
						</SELECT>
					</td>					
				</tr>        
				<tr id=trPropertValue>
					<td align=right height=16>Market Value</td>
					<td height=16 width=250>
						<input           
							id=txtMarketValue name="Market Value"       
							LANGUAGE="javascript" size  =14 
							style="WIDTH: 120px; HEIGHT: 22px" onpropertychange="return txtMarketValue_onpropertychange()" tabIndex=21 width="120" height="22">(R) 
					</td>			
				</tr>        
				<tr id=trPurchasePrice>
					<td align=right height=16>Purchase Price</td>
					<td height=16 width=250><input class="" id=txtPurchasePrice name="Purchase Price" 
						LANGUAGE="javascript" size=14 style="WIDTH: 120px; HEIGHT: 22px" 
						onpropertychange="return txtPurchasePrice_onpropertychange()" 
					tabIndex=22 height="22">(R)</td>        
				</tr>
				    
				<tr id=trLoanAmount>
					<td align=right height=16>Loan Amount</td>
					<td height=16 width=250>
						<input class=""	id=txtLoanAmount name="Loan Amount" LANGUAGE="javascript" 
							style="WIDTH:120px; HEIGHT:22px" onpropertychange="return txtLoanAmount_onpropertychange()" 
							tabIndex=23 height="22">(R)
					</td>
				</tr>        
				<tr id=trCashDeposit>
					<td align=right height=16>Cash Deposit</td>
					<td height=16 width=250><input class="" 
						id=txtCashDeposit    
						name="Cash Deposit" LANGUAGE 
						="javascript" style   ="WIDTH: 
						120px; HEIGHT: 
						22px" onpropertychange="return txtCashDeposit_onpropertychange()" 
						tabIndex=24 height="22">(R)</td>
				</tr>
				<tr id=trCashRequired>
					<td align=right height=16>Cash  Required</td>
					<td height=16 width=250><input class="" 
						id=txtCashRequired  name="Cash Required" 
						LANGUAGE="javascript" style  
						="WIDTH: 120px"  
					onpropertychange ="return txtCashRequired_onkeypress()" 
						tabIndex=25 width="120">(R)</td>
				</tr>
				<tr>
					<td align=right height=16 >Principal</td>
					<td height=16 width=250><input id=txtPrincipal 
						name=Principal readOnly size=14 style="WIDTH: 120px; HEIGHT: 22px" 
						tabIndex=216 height="22">&nbsp;(R)</td>			
				</tr>        
				<tr>
					<td align=right height=16 >Term of Loan</td>
					<td height=16 width=250>
					    <input id=txtTerm name="Term" LANGUAGE="javascript" size=14 
						    style="WIDTH: 120px; HEIGHT: 22px" 
						    onpropertychange = "return txtTerm_onpropertychange()" tabIndex=27>(months)</td>			
				</tr>        
				<tr>
					<td align=right height=16>&nbsp;</td>
					<td height=16 width=250>&nbsp;</td>          
				<tr>
					<td align=right height=16 width=300>Birthday</td>
					<td height=16 width=250>
            <OBJECT id=dteBirthDate 
            style="LEFT: 1px; WIDTH: 120px; TOP: 1px; HEIGHT: 23px" tabIndex=30 
            height=23 classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
	<PARAM NAME="_ExtentY" VALUE="609">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="0">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="CursorPosition" VALUE="0">
	<PARAM NAME="DataProperty" VALUE="0">
	<PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="FirstMonth" VALUE="4">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="dd/mm/yyyy">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="IMEMode" VALUE="3">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxDate" VALUE="2958465">
	<PARAM NAME="MinDate" VALUE="2">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="PromptChar" VALUE="_">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ShowLiterals" VALUE="0">
	<PARAM NAME="TabAction" VALUE="0">
	<PARAM NAME="Text" VALUE="__/__/____">
	<PARAM NAME="ValidateMode" VALUE="0">
	<PARAM NAME="ValueVT" VALUE="1179649">
	<PARAM NAME="Value" VALUE="2.63417926253582E-308">
	<PARAM NAME="CenturyMode" VALUE="0">
	</OBJECT>
					</td>        
				</tr> 
			</TBODY>
		</table>
	</td>
        <td valign=top> 
		    
      <table border=0 width="100%">
        <tr> 
          <td id=td_Higher_PTI height=26 align=right> Higher PTI</td>
          <td align=center> 
            <input language=javascript id=cbLTVPTI onclick="return cbLTVPTI_onclick()" 
					        tabIndex=30 type=checkbox name=cbLTVPTI>
          </td>
        </tr>
        <tr id="ExtTerm"> 
          <td height="26" align=right>Ext. Term</td>
          <td align=center> 
            <input language=javascript id="cbExtTerm" onclick="return cbExtTerm_onclick()" 
					        tabIndex=30 type=checkbox name=cbExtTerm>
          </td>
        </tr>
        <tr id="IntOnly">
          <td height="26" align=right>Interest Only</td>
          <td align=center>
            <input type="checkbox" name="cbIntOnly" id="cbIntOnly" onclick="return cbIntOnly_onclick()" tabIndex=30>
          </td>
        </tr>
        <tr id=tr_fixed_variable_percent> 
          <td height="26">Fixed / Variable Percent</td>
          <td align=center> 
            <input disabled class="TextBox1" id="fixed_percent" language="javascript" name="Fixed Percent" onkeyup="return fixed_percent_onkeyup()"
						    size="14" style="FONT-WEIGHT:normal; BORDER-LEFT-COLOR:black; BORDER-BOTTOM-COLOR:black; WIDTH:81px; BORDER-TOP-COLOR:black; TEXT-ALIGN:right; BORDER-RIGHT-COLOR:black"
						    tabIndex="32">
            <input disabled class="TextBox1" id="variable_percent" language="javascript" name="Variable Percent" 
						    size="14" style="FONT-WEIGHT:normal; BORDER-LEFT-COLOR:black; BORDER-BOTTOM-COLOR:black; WIDTH:81px; BORDER-TOP-COLOR:black; TEXT-ALIGN:right; BORDER-RIGHT-COLOR:black"
						    tabIndex="33">
          </td>
        </tr>
        <tr id=tr_fixed_variable_amount> 
          <td height="26">Fixed / Variable Amount</td>
          <td align=center> 
            <input disabled class="TextBox1" id=entered_fixed_amount language="javascript" name="Fixed Amount" onkeyup="return entered_fixed_amount_onkeyup()"
						    size="14" style="FONT-WEIGHT:normal; BORDER-LEFT-COLOR:black; BORDER-BOTTOM-COLOR:black; WIDTH:81px; BORDER-TOP-COLOR:black; TEXT-ALIGN:right; BORDER-RIGHT-COLOR:black"
						    tabIndex="32">
            <input disabled class="TextBox1" id=entered_variable_amount language="javascript" name="Variable Amount" onkeyup="return entered_variable_amount_onkeyup()"
						    size="14" style="FONT-WEIGHT:normal; BORDER-LEFT-COLOR:black; BORDER-BOTTOM-COLOR:black; WIDTH:81px; BORDER-TOP-COLOR:black; TEXT-ALIGN:right; BORDER-RIGHT-COLOR:black"
						    tabIndex="33">
          </td>
        </tr>
        <tr> 
          <td height=27>&nbsp;</td>
          <td>&nbsp;</td>
        </tr>
        <tr> 
          <td height=27 width="50%"> 
            <input id=text1 name=text1 readOnly style="VISIBILITY: hidden; WIDTH: 76px; HEIGHT: 22px" height="22">
            <input class="" id=txtSAHLInterestRate name=inSAHLInterestRate style="VISIBILITY: hidden; WIDTH: 77px; HEIGHT: 22px" 
							    height="22">
          </td>
          <td>&nbsp;</td>
        </tr>
        <tr> 
          <td height=20>&nbsp;
            <INPUT id=txtExtTerm 
            style="VISIBILITY: hidden; WIDTH: 76px; HEIGHT: 22px" readOnly 
            name=text1 height="22">
          </td>
          <td>&nbsp;</td>
        </tr>
        <tr> 
          <td height=27 id=tdAge colspan=2>&nbsp;</td>
        </tr>
      </table>
	    </td>	
    </tr>	
    <tr id=trFees>
        <td colspan=2 height=16 width=730>
            <table border=0 class=table3 >
                <TBODY>
                    <tr id=trFeesHeader>
                        <td height=16 width=250><b><STRONG>Capitalise Fees</STRONG></b>
                            <input CHECKED id=cbTotalFees lang="javascript" name=cbTotalFees 
                                onclick="return cbTotalFees_onclick()" tabindex=41 type=checkbox></td>
                        <td align=center height=16><b>SAHL</b></td>
                        <td align=center height=16><b>Bank</b></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr id=trCancellationFee>
                        <td align=right height=16 width=250 >Cancellation Fee</td>
                        <td height=16 width=150>
                            <input id=txtCancellationFeeSAHL name=inCancellationFeeSAHL readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                        <td height=16 width=250>
                            <input id=txtCancellationFeeBank name=inCancellationFeeBank readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr id=trRegistrationFee>
                        <td align=right height=16 width=250 >Registration Fee</td>
                        <td height=16 width=150>
                            <input id=txtRegistrationFeeSAHL name=inRegistrationFeeSAHL readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                        <td height=16 width=250>
                            <input id=txtRegistrationFeeBank name=inRegistrationFeeBank readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td></tr>
                    <tr id=trInitiationFee>
                        <td align=right height=16 width=250 >Initiation Fee</td>
                        <td height=16 width=150>
                            <input id=txtInitiationFeeSAHL name=inInitiationFeeSAHL readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                        <td height=16 width=250>
                            <input id=txtInitiationFeeBank name=inInitiationFeeBank readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                    </tr>
                    <tr id=trValuationFee>
                        <td align=right height=16 width=250 >Valuation Fee</td>
                        <td height=16 width=150>
                            <input id=txtValuationFeeSAHL name=inValuationFeeSAHL readOnly size=15 
                                tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                        <td height=16 width=250>
                            <input id=txtValuationFeeBank name=inValuationFeeBank readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                    </tr>
                    <tr>
                        <td align=right height=16 width=250><STRONG>Sub Total</STRONG></td>
                        <td height=16 width=150>
                            <input id=txtSubTotalFeesSAHL name=txtSubTotalFeesSAHL readOnly tabIndex=0 style="FONT-WEIGHT: bold; WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                        <td height=16 width=250>
                            <input id=txtSubTotalFeesBank name=txtSubTotalFeesBank readOnly tabIndex=0 style="FONT-WEIGHT: bold; WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td></tr>
                    <tr id=trTransferFee>
                        <td align=right height=16 width=250 >Transfer Fee 
                            <select class=CalcInputText id=ddTransferType language=javascript name=ddTransferType 
                                    onchange="return ddTransferType_onclick()" style="WIDTH: 73px; HEIGHT: 22px" tabIndex=42> 
                                <option selected value=Natural>Natural</option>
                                <option value=Legal>Legal</option>
                            </select> 
                        </td>
                        <td height=16 width=150>
                            <input id=txtTransferFeeSAHL name=inTransferFeeSAHL readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                        <td height=16 width=250><input id=txtTransferFeeBank name=inTransferFeeBank readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                    </tr>
                    <tr id=trInterimInterestFee>
                        <td align=right height=16 width=250 >Interim Interest Provision</td>
                        <td height=16 width=150>
                            <input id=txtInterimInterestFeeSAHL name=inInterimInterestFeeSAHL readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                        <td height=16 width=250>
                            <input id=txtInterimInterestFeeBank name=inInterimInterestFeeBank readOnly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
                    </tr>
        <tr id="trTotalFees">
          <td align=right height=16 width=250><b>Total Capitalised Fees</b></td>
          <td height=16><input class="" 
            id=txtTotalFeesSAHL name="Total Fees SAHL" readonly tabIndex=0 
            style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
          <td height=16><input class="" 
            id=txtTotalFeesBank name="Total Fees Bank" readonly tabIndex=0 style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td></tr>
        <tr id=trFeesNewHome2>
          <td align=right height=16 width=250>Total Fees(including Transfer Fees)</td>
          <td height=16 width=150><input class="" 
            id=txtFeesNewHome name=FeesNewHome readOnly tabindex=0 
            style="WIDTH: 123px; HEIGHT: 22px">&nbsp;(R)</td>
          <td height=16 width=250>&nbsp;</td>
          </td>
        </tr>
    </TBODY>
</table>
        <tr id=trCalculations>
	        <td colSpan=2 width="100%" valign=top align="right">
		        
      <table border=0 class=table3 id=tblCalculations>
        <tr> 
          <td id=td_titles height="16" width="300">&nbsp;</td>
          <td id=td_fixed_1 align=center height=16 width=150> <b>SAHL (Fixed)</b></td>
          <td align=center height=26 width=150><b>SAHL (Variable)</b></td>
          <td>&nbsp;</td>
        </tr>
        <tr> 
          <td align=right height=16><B>Total Loan</B></td>
          <td id=td_fixed_2 height=16>&nbsp;</td>
          <td height=16 nowrap> 
            <input class="TextBox2" id=txtTotalLoanSAHL name="Total Loan SAHL" 
						        readOnly size=14 tabIndex=0>
            (R)</td>
          <td>&nbsp;</td>
        <tr id=fixed_variable_total> 
          <td align=right><b>Fixed / Variable Portion </b>of Total Loan</td>
          <td id=td_fixed_3 height=16> 
            <input class="TextBox2" id=txtFixedPortion name=txtFixedPortion 
						        readOnly size=14 tabIndex=0>
            (R)</td>
          <td height=16 nowrap> 
            <input class="TextBox2" id=txtVariablePortion name=txtVariablePortion
						        readOnly size=14 tabIndex=0>
            (R)</td>
          <td>&nbsp;</td>
        </tr>
        <tr id=tr_market_rate_type> 
          <td align=right><b>Market Rate Type</b></td>
          <td> 
            <select id=ddlFixedMarketRateType style="WIDTH:145px" language=javascript onchange="return ddlFixedMarketRateType_change()">
              <%Call populateDDL("Select Value, Description from vw_2amMarketRate (nolock) Where MarketRateKey = 3")%>
            </select>
          </td>
          <td> 
            <select id=ddlVariableMarketRateType style="WIDTH:145px" onchange="return ddlVariableMarketRateType_change()" language=javascript>
              <%Call populateDDL("Select MarketRateTypeRate, MarketRateTypeDescription From MarketRateType (nolock)")%>
            </select>
          </td>
          <td>&nbsp;</td>
        </tr>
        <tr> 
          <td align=right height=16><b>Rate Category</b></td>
          <td id=td_fixed_6 height="16" align=center> 
            <input class="TextBox2" id="txtSAHLRateCategory_fixed" name="inSAHLRateCategory_fixed" readOnly size="14"									        
									        tabIndex="0">
            &nbsp;(%)</td>
          <td height=16 nowrap> 
            <input class="TextBox2" id=txtSAHLRateCategory name=inSAHLRateCategory 
						        readOnly size=14 tabIndex=0>
            (%)</td>
          <td>&nbsp;</td>
        </tr>
        <tr id=tr_fixed_portion_interest_rate> 
          <td id=td_rate align="right" height="16" width="500"><b>Interest Rate</b></td>
          <td id=td_fixed_6a height="16"  align=center> 
            <input class="TextBox2" id="InterestRateSAHL_fixed" name="InterestRateSAHL_fixed" 
						        tabIndex="0" readOnly size="14">
            &nbsp;(%)</td>
          <td height="16" align=center nowrap>
            <input class="TextBox2" id="InterestRateSAHL_var" name="InterestRateSAHL_var" readOnly size="14"									        
									        tabIndex="0">
            &nbsp;(%)</td>
          <td>&nbsp;</td>
        </tr>
        <tr id=qualifies_sahl_rate> 
          <td align=right height=16><b>Qualifies for a SAHL rate</b></td>
          <td id=td_fixed_7 height=16>&nbsp;</td>
          <td height=16 nowrap> 
            <input class="TextBox2" id=txtRateSAHL name=inSAHLRate 
						        readOnly size=14 tabIndex=0>
            (%)</td>
          <td>&nbsp;</td>
        </tr>
        <tr id=fixed_variable_instalment> 
          <td align=right><b>Fixed / Variable Portion </b>of Monthly Instalments</td>
          <td id=td_fixed_10 height=16> 
            <input class="TextBox2" id=txtInstalmentSAHL_fixed name=txtInstalmentSAHL_fixed 
						        readOnly size=14 tabIndex=0>
            (R)</td>
          <td height=16 nowrap> 
            <input class="TextBox2" id=txtInstalmentSAHL_variable name=txtInstalmentSAHL_variable
						        readOnly size=14 tabIndex=0>
            (R)</td>
          <td>&nbsp;</td>
        </tr>
        <tr> 
          <td align=right height=16><b>Total Monthly instalments</b></td>
          <td id=td_fixed_9 height=16>&nbsp;</td>
          <td height=16 nowrap> 
            <input class="TextBox2" id=txtInstalmentSAHL name=inInstalmentSAHL 
					        readOnly tabIndex=0 size="10">
            (R)</td>
          <td nowrap>
            <div id="IntOnlyInst"><b>Amortising</b> 
              <input id="txtIntOnlyInst" name="txtIntOnlyInst" type="text" readonly tabindex=0 size="10" />
              (R) </div>
          </td>
        </tr>
        <tr id=tr_savings> 
          <td align=right height=16 id="savings"><b>Savings over term of loan</b></td>
          <td id=td_fixed_11 height=16>&nbsp;</td>
          <td height=16 id="td_savings" nowrap> 
            <input class="TextBox2" id=txtSavingsTerm name=inSavingsTerm 
						        readOnly tabIndex=0>
            (R)</td>
          <td>&nbsp;</td>
        </tr>
        <tr> 
          <td align=right height=16><b>Income must be at least</b></td>
          <td id=td_fixed_8 height=16>&nbsp;</td>
          <td height=16 nowrap> 
            <input class="TextBox2" id=txtIncomeSAHL name=inIncomeSAHL 
						        readOnly size=14 tabIndex=0>
            (R)</td>
          <td>&nbsp;</td>
        </tr>
        <tr> 
          <td align=right height=16>Loan to Value <b>(LTV) </b>ratio</td>
          <td id=td_fixed_4 height=16>&nbsp;</td>
          <td height=16 nowrap> 
            <input class="TextBox2" id=txtLTVSAHL name=inLTV 
						        readOnly size=14 tabIndex=0>
            (%)</td>
          <td>&nbsp;</td>
        </tr>
        <tr> 
          <td align=right height=16 nowrap>Payment to Income <b>(PTI) </b>ratio</td>
          <td id=td_fixed_5 height=16>&nbsp;</td>
          <td height=16 nowrap> 
            <input class="TextBox2" id=txtPTISAHL name=inPTISAHL 
						        readOnly tabIndex=0>
            (%)</td>
          <td>&nbsp;</td>
        </tr>
      </table>
	        </td>
	        <td valign=top >		
		        <div id=div_super>
			        <table border=0>
				        <tr>
					        <td align="center" height="26" width="15%"><b><%=SuperRate_Product_Description%></b></td>
				        </tr>
				        <tr>
					        <td height="24"></td>
				        </tr>
				        <tr>
					        <td height="16">
						        <input class="TextBox2" id="txtSAHLRateCategory_super" name="Rate_super" readOnly tabIndex="0">(%)</td>
				        </tr>
				        <tr>
					        <td height="16">
						        <input class="TextBox2" id="txtRateSAHL_super" name="inPTIBank" readOnly tabIndex="0">(%)</td>
				        </tr>
				        <tr>
					        <td height="26"><input class="TextBox2" id="txtInstalmentSAHL_super" name="" readOnly tabIndex="0">(R)</td>
				        </tr>
				        <tr>
					        <td height="16">
						        <input class="TextBox2" id="txtSavingsTerm_super" name="" readOnly size="13" tabIndex="0">(R)</td>
				        </tr>
				        <tr>
					        <td height="20"></td>
				        </tr>
				        <tr>
					        <td height="20">&nbsp;</td>
				        </tr>
				        <tr>
					        <td height="16">
						        <input class="TextBox2" id="txtPTISAHL_super" name="" readOnly tabIndex="0">(%)</td>
				        </tr>
			        </table>
		        </div>	              
		        <div id=div_core>
			        <table border=0>
				        <tr>
					        <td align="center" height="26" width="15%"><b>Bank</b></td>
				        </tr>
				        <tr>
					        <td height="16"><input class="TextBox2" id="txtTotalLoanBank" name="Total Loan Bank" readOnly size="14"
							        tabIndex="0">(R)</td>
				        </tr>
				        <tr>
					        <td height="16">
						        <input class="TextBox2" id="txtBankRateCategory"  value="N/A" name="" readOnly tabIndex="0">(%)</td>
				        </tr>
				        <tr>
					        <td height="16">
						        <input class="TextBox2" id="txtRateBank" name="inPTIBank" readOnly tabIndex="0">(%)</td>
				        </tr>				        
				        <tr>
					        <td height="16">
						        <input class="TextBox2" id="txtInstalmentBank" name="inRateBank" readOnly size="13" tabIndex="0">(%)</td>
				        </tr>
				        <tr>
					        <td height="24">&nbsp;</td>
				        </tr>
				        <tr>
					        <td height="16">
						        <input class="TextBox2" id="txtIncomeBank" name="inInstalmentBank" readOnly tabIndex="0">(R)</td>
				        </tr>
				        <tr>
					        <td height="24"><input class="TextBox2" id="txtLTVBank" name="inLTVBank" readOnly tabIndex="0">(%)</td>
				        </tr>
				        <tr>
					        <td height="16">
						        <input class="TextBox2" id="txtPTIBank" name="inIncomeBank" readOnly tabIndex="0">(R)</td>
				        </tr>
			        </table>
		        </div>
	        </td>	        
            <td valign=top>   
                <div id=div_flexi>
                    <table border=0>                                    
                        <tr>
                            <td id=td_var_flexi valign=top align=center height=70>100 % Var</td>
                        </tr> 
                        <tr>
                            <td align=center>
                                <input id=btnShowVariable onclick="return btnShowVariable_onclick()" language=javascript type=button value="Go" >
                            </td>
                        </tr>      	
                        <tr style="VISIBILITY:hidden">
                            <td visible=false>VariFix</td>
                        </tr>				    
                    </table>        					    
                </div>
            </td>	
        </tr>
        <tr id="tr_super" visible="false">
            <td colspan=3>
                <table id="tbl_Super" border="1" borderColor="#4682b4" class="Table3" cellPadding="0" cellSpacing="0" width="100%">
                    <tr><td id="td_LoyaltyBonus"></td></tr>
                    <tr><td id="td_IndicativeInstalment"></td></tr>
                </table>
            </td>
        </tr></TBODY></TABLE>&nbsp;&nbsp;&nbsp;&nbsp; 

<TABLE border=1 borderColor=#4682b4 cellPadding=1 cellSpacing=1 id=tblQualifies style="Z-INDEX: 108; LEFT: 551px; FONT-FAMILY: serif; POSITION: absolute; TOP: 8px">
 <TR>
    <td>
    <FONT color=black face=Arial><STRONG>QUALIFIES?</STRONG></FONT>
    </td>
      <td id=tdQualified>&nbsp;</td>
   </TR>
   </TABLE></P>

</BODY>
</HTML>
