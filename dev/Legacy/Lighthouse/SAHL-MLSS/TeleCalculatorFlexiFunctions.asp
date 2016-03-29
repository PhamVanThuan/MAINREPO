<%@ Language=VBScript %>
<%
Option Explicit

' Variable Declarations
dim objConn, CalculatorType

Dim PTI, LTV, Category, Rate, MinLoanAmount, MinFixedAmount, sTest, MinFFTotalAmount
Dim MaxPTIMarginSalaried, MaxPTIMarginSelfEmployed, MaxPTIMarginSubsidised
        
Dim bnQualify

CalculatorType = request.QueryString("CalcType")

Select Case CalculatorType
    
    Case "CreditMatrix"
        Call RetrieveCreditMatrixDetails
        
    Case "Defaults"
        Call RetrieveDefaults
        
    
End Select 
  

sub RetrieveCreditMatrixDetails()

    Dim rs, sSQL, iEmploymentType, iLoanPurpose, fMinIncome, fLoanAmount, fPropertyValue, fLTVPercentage, fPTIPercentage 

    
    If Request.QueryString("EmploymentType") = "" Then
        iEmploymentType = "Null"
    Else
        iEmploymentType = Request.QueryString("EmploymentType")
    End If
    
    If Request.QueryString("LoanPurpose") = "" Then
        iLoanPurpose = "Null"
    Else
        iLoanPurpose = Request.QueryString("LoanPurpose")
    End If
    
    If Request.QueryString("MinIncome") = "" Then
        fMinIncome = "Null"
    Else
        fMinIncome = Request.QueryString("MinIncome")
    End If
        
    If Request.QueryString("LoanAmount") = ""  Then
        fLoanAmount = "Null"
    Else
        fLoanAmount = Request.QueryString("LoanAmount")
    End If
    
    If Request.QueryString("PropertyValue") = "" Then
        fPropertyValue = "Null"
    Else
        fPropertyValue = Request.QueryString("PropertyValue")
    End If
          
    If Request.QueryString("LTVPercentage") = "" Then
        fLTVPercentage = "Null"
    Else
        fLTVPercentage = Request.QueryString("LTVPercentage")
    End If
    
    If Request.QueryString("PTIPercentage") = "" Then
        fPTIPercentage = "Null"
    Else
        fPTIPercentage = Request.QueryString("PTIPercentage")
    End If
        
    call getConnection    
   
    set rs = Server.CreateObject("ADODB.Recordset")
    
    rs.CursorLocation=3
    
    sSQL = "c_GetCreditMatrixDetails " _
                & iEmploymentType & ", " _
                & iLoanPurpose & ", " _
                & fMinIncome & ", " _
                & fLoanAmount & ", " _
                & fPropertyValue & ", " _
                & fLTVPercentage & ", " _
                & fPTIPercentage
                
    
       
    sTest = sSQL
    
    rs.Open sSQL, objConn,3
    
    if not rs.EOF then     
        bnQualify = 1
        Category = rs.Fields(0).Value           
        Rate = rs.Fields(1).Value            
        LTV = rs.Fields(2).Value           
        PTI = rs.Fields(3).Value   
    else
        bnQualify = 0         
    end if
        
    rs.Close
    set rs = nothing
    
    objConn.Close    
    set objConn = nothing
    
  
end sub

'----------------------------------------------------------------------------------------------------------------------------------------------------------------

sub RetrieveDefaults()

    dim rs, sSQL
    
    call getConnection    
   
    set rs = Server.CreateObject("ADODB.Recordset")
    
    rs.CursorLocation=3
    
    sSQL = "tlc_GetCalculatorDefaults"                
    
    rs.Open sSQL, objConn,3
    
    if not rs.EOF then     
          
        MinLoanAmount = rs.Fields(0).Value
        MinFixedAmount = rs.Fields(1).Value
        MinFFTotalAmount = rs.Fields(2).Value
        MaxPTIMarginSalaried = rs.Fields(3).Value
        MaxPTIMarginSelfEmployed = rs.Fields(4).Value
        MaxPTIMarginSubsidised = rs.Fields(5).Value
    end if
    
    rs.Close
    
    set rs = nothing

end sub

'----------------------------------------------------------------------------------------------------------------------------------------------------------------
sub getConnection()

'// Creates connection to the database
'// objConn

dim sessDSN
dim sDSN

    ' Make sure user has logged on properly...if not redirect to login page...   	
    sessDSN= "DSN=" & Session("DSN")      
    
     
	if sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"     ' Login page	   	   
    end if
    
    ' Connection string    
    sDSN = "Provider=SQLOLEDB.1; Application Name='MLS System Version1 [TeleCalculatorFlexiFunctions.asp]';Data Source=" & Session("SQLDatabase") & ";uid=" & Session("UserID") 
    
        
    Set objConn = Server.CreateObject("ADODB.Connection")
     
    objConn.Open sDSN        

end sub

%>
<html>
<body>
<script language=javascript>

    switch("<%=CalculatorType%>") {
    
        case "CreditMatrix":
            
            // Save variables
            window.parent.fActual_PTI = "<%=PTI%>"; 
            window.parent.fActual_LTV = "<%=LTV%>";
            window.parent.fRate = "<%=Rate%>"; 
            window.parent.iCategory = "<%=Category%>"; 
                                                
            // Display
            window.parent.td_LTV.innerHTML = "<%=LTV%>";
            window.parent.td_PTI.innerHTML = "<%=PTI%>";  
            
            if ("<%=bnQualify%>" == 1) 
                window.parent.td_qualify.innerHTML = "Yes";
            else 
                window.parent.td_qualify.innerHTML = "No";
                
            //window.parent.td_error.innerHTML = "<%=sTest%>"                      
            break;
            
        case "Defaults":     
                             
            window.parent.fMinLoanAmount = "<%=MinLoanAmount%>"; 
            window.parent.fMinFixedAmount = "<%=MinFixedAmount%>";
            window.parent.fMinFFTotalAmount = "<%=MinFFTotalAmount%>";
            window.parent.cmMaxPTIMarginSelfEmployed = "<%=MaxPTIMarginSelfEmployed%>";
            window.parent.cmMaxPTIMarginSalaried = "<%=MaxPTIMarginSalaried%>";
            window.parent.cmMaxPTIMarginSubsidised = "<%=MaxPTIMarginSubsidised%>"; 
            window.parent.calc_defaults_callback();
            
            break;
           
            
    }
  

 
</script>
</body>
</html>
