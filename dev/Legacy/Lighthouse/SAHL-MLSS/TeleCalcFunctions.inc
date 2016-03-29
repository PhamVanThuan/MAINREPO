function RoundNumber(number,X) { 
// rounds number to X decimal places, defaults to 2 
	X = (!X ? 2 : X); 
	return Math.floor(number*Math.pow(10,X))/Math.pow(10,X); 
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

function CalculateInstallment(d_Loan, d_PeriodRate, i_InterestPeriods, i_Term, i_Type) {
    //d_Loan  = loan amount
    //d_PeriodRate = interest rate for period as percentage
    //i_term = number of periods over which loan is to be repaid
    //i_Type = (0 = installment at end of period 1  = installment at beginning of period)

	if ((d_Loan <= 0) || (d_Loan == null)) {
		alert ("Loan cannot be less or equal to zero  ... Check Values....!!!");
		return 0;
	}

	if ((d_PeriodRate <= 0) || (d_PeriodRate == null)) {
		alert( "Rate cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((i_InterestPeriods <= 0) || (i_InterestPeriods == null)) {
		alert( "Interest Periods cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((i_Term <= 0) || (i_Term == null)) {
		alert( "Term cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}
	
	if ((i_Type > 1) || (i_Type == null)) {
		alert( "Type must be 0 or 1 ... Check Values....!!!");
		return 0;
	}

    var CalculateInstallment = (d_PeriodRate / i_InterestPeriods) * (d_Loan * Math.pow((1 + d_PeriodRate / i_InterestPeriods), i_Term)) / ((1 + d_PeriodRate / i_InterestPeriods * i_Type) * (1 - Math.pow((1 + d_PeriodRate / i_InterestPeriods), i_Term))) * -1;

    if (CalculateInstallment < 0)
           CalculateInstallment = 0;
    
    return RoundNumber(CalculateInstallment,2);
}


/*
 *
 *  Calculates an interest only installment.
 *
 *  W van Rooyen.
 *
 */
function CalculateIntOnlyInstallment(d_Loan, d_PeriodRate) {

	if ((d_Loan <= 0) || (d_Loan == null)) {
		alert ("Loan cannot be less or equal to zero  ... Check Values....!!!");
		return 0;
	}

	if ((d_PeriodRate <= 0) || (d_PeriodRate == null)) {
		alert( "Rate cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	// (Daily interest * 93 day quarter) / 3 months
	var IntOnlyInstallment = ((d_Loan*d_PeriodRate/365)*93)/3;

	return RoundNumber(IntOnlyInstallment ,2);
}


function CalculateTerm(d_Loan, d_PeriodRate, i_InterestPeriods, d_Installment, i_Type) {
   //d_Loan = loan amount
    //d_PeriodRate = interest rate for the period as a percentage
    //i_InterestPeriods = the number of interest periods per year
    //d_Installment = the installment amount
    //i_Type = (0 = Installment at end of period 1 = Installment at beginning of period)
 	
 	if ((d_Loan <= 0) || (d_Loan == null)) {
		alert ("Loan cannot be less or equal to zero  ... Check Values....!!!");
		return 0;
	}

	if ((d_PeriodRate <= 0) || (d_PeriodRate == null)) {
		alert( "Rate cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((i_InterestPeriods <= 0) || (i_InterestPeriods == null)) {
		alert( "Interest Periods cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((d_Installment <= 0) || (d_Installment == null)) {
		alert( "Installment cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}
	
	if ((i_Type > 1) || (i_Type == null)) {
		alert( "Type must be 0 or 1 ... Check Values....!!!");
		return 0;
	}
  
    //alert (d_Installment + " " + d_PeriodRate + " "+ i_InterestPeriods + " " + d_Loan);
   var d_Temp = -d_Installment * (1 + d_PeriodRate / i_InterestPeriods * i_Type) / (d_Loan * d_PeriodRate / i_InterestPeriods);
    
   if (d_Temp > -1) return 0;
   
   //CalculateTerm = Log(d_Temp / (1 + d_Temp)) / Log(1 + d_PeriodRate / i_InterestPeriods)
   var CalculateTerm = Math.log (d_Temp / (1 + d_Temp)) / Math.log (1 + d_PeriodRate / i_InterestPeriods);

	if (isNaN(CalculateTerm)) 
		CalculateTerm =  0;
		
  
  return RoundNumber(CalculateTerm,2);
}



function CalculateLoanRemainingBalance(d_Loan, d_PeriodRate, i_InterestPeriods, i_Term, i_CurrentPeriod, i_Type) {
    //d_Loan = loan amount
    //d_PeriodRate = interest rate for the period as a percentage
    //i_InterestPeriods = the number of interest periods per year
    //i_Term = the number of interest periods over which the loan is to be repaid
   //i_CurrentPeriod = the current interest period
    //i_Type = (0 = Installment at end of period 1 = Installment at beginning of period)
    
   if  ((i_Term <= 0) || (i_Term == null) ||(d_PeriodRate <= 0) || (d_Loan <= 0) (i_Term <= 0)) 
		return 0;
    
   var CalculateLoanRemainingBalance =  d_Loan * (((1 + d_PeriodRate / i_InterestPeriods) ^ i_Term) - ((1 + d_PeriodRate / i_InterestPeriods) ^ i_CurrentPeriod)) / (((1 + d_PeriodRate / i_InterestPeriods) ^ i_Term) - 1);
   
   return RoundNumber(CalculateLoanRemainingBalance,2);
}


function CalculatePrincipalPayment(d_Loan, d_PeriodRate, i_InterestPeriods, i_Term, i_CurrentPeriod, i_Type) {
    //d_Loan = loan amount
    //d_PeriodRate = interest rate for the period as a percentage
    //d_PeriodRate = the number of interest periods per year
    //i_Term = the number of interest periods over which the loan is to be repaid
    //i_CurrentPeriod = the current interest period
    //i_Type = (0 = Installment at end of period 1 = Installment at beginning of period)
    
    if  ((i_Term <= 0) || (i_Term == null) || (d_PeriodRate <= 0) || (d_Loan <= 0) || (i_CurrentPeriod <= 0) )
            return 0;
 
    var CalculatePrincipalPayment = d_Loan * (d_PeriodRate / i_InterestPeriods * ((1 + d_PeriodRate / i_InterestPeriods) ^ (i_CurrentPeriod - 1))) / (((1 + d_PeriodRate / i_InterestPeriods) ^ i_Term) - 1);
    
    return RoundNumber(CalculatePrincipalPayment,2);
}



function CalculateLifeInterest(d_Loan, d_PeriodRate, i_InterestPeriods, i_Term, i_Type) {
    //d_Loan = loan amount
    //d_PeriodRate = interest rate for the period as a percentage
    //i_InterestPeriods = the number of interest periods per year
    //i_Term = the number of interest periods over which the loan is to be repaid
    //i_Type = (0 = Installment at end of period 1 = Installment at beginning of period)
   

   if ((d_Loan <= 0) || (d_Loan == null)) {
		alert ("Loan cannot be less or equal to zero  ... Check Values....!!!");
		return 0;
	}

	if ((d_PeriodRate <= 0) || (d_PeriodRate == null)) {
		alert( "Rate cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((i_InterestPeriods <= 0) || (i_InterestPeriods == null)) {
		alert( "Interest Periods cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((i_Term <= 0) || (i_Term == null)) {
		alert( "Term cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}
	
	if ((i_Type > 1) || (i_Type == null)) {
		alert( "Type must be 0 or 1 ... Check Values....!!!");
		return 0;
	}
 
    var i_CurrentPeriod = 1;
    var d_Interest = 0;
    var d_totalinterest = 0;

    while ( i_CurrentPeriod <= i_Term) {
        d_Interest = CalculateInterestPayment(d_Loan, d_PeriodRate, i_InterestPeriods, i_Term, i_CurrentPeriod, i_Type);
       d_totalinterest = d_totalinterest + d_Interest;
        i_CurrentPeriod = i_CurrentPeriod + 1;
    }
     
    return RoundNumber(d_totalinterest,2);
    
}

function CalculateInterestPayment(d_Loan, d_PeriodRate, i_InterestPeriods, i_Term, i_CurrentPeriod, i_Type) {
    //d_Loan = loan amount
    //d_PeriodRate = interest rate for the period as a percentage
    //d_PeriodRate = the number of interest periods per year
    //i_Term = the number of interest periods over which the loan is to be repaid
    //i_CurrentPeriod = the current interest period
    //i_Type = (0 = Installment at end of period 1 = Installment at beginning of period)
    
   var CalculateInterestPayment =  d_Loan * d_PeriodRate / i_InterestPeriods * (( Math.pow ((1 + d_PeriodRate / i_InterestPeriods),i_Term)) - ( Math.pow ((1 + d_PeriodRate / i_InterestPeriods),(i_CurrentPeriod - 1)) )) / (( Math.pow ((1 + d_PeriodRate / i_InterestPeriods), i_Term)) - 1);

    //ClaireY - 22/05/2005 - Amended calculation to NOT round everytime as this is inconsistent with the Loan Calculator
   //return RoundNumber(CalculateInterestPayment,2);
	return(CalculateInterestPayment);
}


function CalculateLoan(d_Installment, d_PeriodRate, i_InterestPeriods, i_Term, i_Type) {
    //d_Loan  = loan amount
    //d_PeriodRate = interest rate for period as percentage
    //i_term = number of periods over which loan is to be repaid
    //i_Type = (0 = installment at end of period 1  = installment at beginning of period)
    
  if ((d_Installment <= 0) || (d_Installment == null)) {
		alert ("Loan cannot be less or equal to zero  ... Check Values....!!!");
		return 0;
	}

	if ((d_PeriodRate <= 0) || (d_PeriodRate == null)) {
		alert( "Rate cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((i_InterestPeriods <= 0) || (i_InterestPeriods == null)) {
		alert( "Interest Periods cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((i_Term <= 0) || (i_Term == null)) {
		alert( "Term cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}
	
	if ((i_Type > 1) || (i_Type == null)) {
		alert( "Type must be 0 or 1 ... Check Values....!!!");
		return 0;
	}

    var CalculateLoan = ((d_Installment * (((1 + d_PeriodRate / i_InterestPeriods * i_Type) * (1 - Math.pow ((1 + d_PeriodRate / i_InterestPeriods), i_Term))) * -1)) / ((d_PeriodRate / i_InterestPeriods)) / Math.pow ((1 + d_PeriodRate / i_InterestPeriods), i_Term));

    return RoundNumber(CalculateLoan,2);

}

function  PresentValueFactor (d_PeriodRate, i_InterestPeriods, i_Term, i_Type ) {
   //d_Loan = loan amount
    //d_PeriodRate = interest rate for the period as a percentage
    //i_InterestPeriods = the number of interest periods per year
    //d_Installment = the installment amount
    //i_Type = (0 = Installment at end of period 1 = Installment at beginning of period)

	
	if ((d_PeriodRate <= 0) || (d_PeriodRate == null)) {
		alert( "Rate cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((i_InterestPeriods <= 0) || (i_InterestPeriods == null)) {
		alert( "Interest Periods cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((i_Term <= 0) || (i_Term == null)) {
		alert( "Term cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}
	
	if ((i_Type > 1) || (i_Type == null)) {
		alert( "Type must be 0 or 1 ... Check Values....!!!");
		return 0;
	}

	var NPV = 1 / Math.pow((1 + (d_PeriodRate / i_InterestPeriods)), i_Term);
	
	return  RoundNumber(NPV,2);


}

function CalculateNPV(d_Loan1, d_Loan2, d_PeriodRate1, d_PeriodRate2, d_InflationRate, i_InterestPeriods, i_Term, i_Type) {
    //d_Loan = loan amount
    //d_PeriodRate = interest rate for the period as a percentage
    //i_InterestPeriods = the number of interest periods per year
    //i_Term = the number of interest periods over which the loan is to be repaid
    //i_Type = (0 = Installment at end of period 1 = Installment at beginning of period)
   
   if ((d_Loan1 <= 0) || (d_Loan1 == null)) {
		alert ("Loan cannot be less or equal to zero  ... Check Values....!!!");
		return 0;
	}
   if ((d_Loan2 <= 0) || (d_Loan2 == null)) {
		alert ("Loan cannot be less or equal to zero  ... Check Values....!!!");
		return 0;
	}

	if ((d_PeriodRate1 <= 0) || (d_PeriodRate1 == null)) {
		alert( "Rate cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((d_PeriodRate2 <= 0) || (d_PeriodRate2 == null)) {
		alert( "Rate cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((d_InflationRate <= 0) || (d_InflationRate == null)) {
		alert( "Inflation Rate cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}


	if ((i_InterestPeriods <= 0) || (i_InterestPeriods == null)) {
		alert( "Interest Periods cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}

	if ((i_Term <= 0) || (i_Term == null)) {
		alert( "Term cannot be less or equal to zero ... Check Values....!!!");
		return 0;
	}
	
	if ((i_Type > 1) || (i_Type == null)) {
		alert( "Type must be 0 or 1 ... Check Values....!!!");
		return 0;
	}
 
    var i_CurrentPeriod = 1;
    var d_Interest = 0;
    var d_InterestBank = 0;
	  var d_TotalInterest = 0;
		var d_TotalInterestBank = 0;

		var d_PFV = 0;
		var d_TotalPFV = 0;
		var d_diff = 0;
    
    
    while ( i_CurrentPeriod <= i_Term) {
 
        d_Interest = CalculateInterestPayment(d_Loan1, d_PeriodRate1, i_InterestPeriods, i_Term, i_CurrentPeriod, i_Type);
        d_TotalInterest = d_TotalInterest + d_Interest;
         
        d_InterestBank = CalculateInterestPayment(d_Loan2, d_PeriodRate2, i_InterestPeriods, i_Term, i_CurrentPeriod, i_Type);
        d_TotalInterestBank = d_TotalInterestBank + d_InterestBank;
     
        d_diff = d_InterestBank - d_Interest;
        
        d_PFV = PresentValueFactor(d_InflationRate, i_InterestPeriods, i_CurrentPeriod, i_Type);
        //d_TotalPFV = d_TotalPFV + (d_diff/Math.pow( (1 + (0.1/12)),i_CurrentPeriod))
        
        d_TotalPFV = d_TotalPFV + d_diff * d_PFV;
        
        i_CurrentPeriod = i_CurrentPeriod + 1;
    }
     
    return RoundNumber(d_TotalPFV, 2);
    
}

