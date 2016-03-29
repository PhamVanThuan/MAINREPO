<%@ Language=VBScript %>
<%
Option explicit
Response.Expires = 0
%>

<!--#include file="TeleXMLFuncs.asp"-->
<!--#include file="server.asp"-->
<%

GetFees

function GetDataConnection()
   dim oConn, strConn
   Set oConn = Server.CreateObject("ADODB.Connection")
   
   strConn = "Provider=SQLOLEDB;Data Source=" & DB_SERVER & "; Initial Catalog=" & LIGHTHOUSE_DB      
   strConn = strConn & ";User Id=" & Session("UserID")
       
   oConn.Open strConn
   set GetDataConnection = oConn
end function    
	
Sub GetFees
	Dim objDOM, strText, strSQL, cnnNW
	Dim strNbr,strDetails,strNbrType,strTmp
	Dim dPurchaseAmount, dCashDeposit, dCashRequired, sLoanPurpose, sLoanTransferType,dEstimatedPropValue
	
	Dim rsXML,strXML,DQ
	DQ = Chr(34)
	
	set rsXML = Server.CreateObject("ADODB.RecordSet")

	'Open Database connection
	Set cnnNW = GetDataConnection()
	
	'Create an XML DOM from the incoming XML
	set objDOM = Server.CreateObject("Microsoft.XMLDOM")
	objDOM.Load(Request)
	
	'Extract the values from the incoming XML
	dPurchaseAmount	= GetNodeValue(objDOM, "/Loan/PurchaseAmount")
	dCashDeposit		= GetNodeValue(objDOM, "/Loan/CashDeposit")
	dCashRequired		= GetNodeValue(objDOM, "/Loan/CashRequired")
	'dLoanAmount		= GetNodeValue(objDOM, "/Loan/LoanAmount")
	sLoanPurpose		= GetNodeValue(objDOM, "/Loan/LoanPurpose")
	sLoanTransferType	= GetNodeValue(objDOM, "/Loan/LoanTransferType")
    dEstimatedPropValue	= GetNodeValue(objDOM, "/Loan/MarketValue")
	
			
	'Build the Insert SQL Statement
   ' strSQL = "w_GetCalculatorFees " & Cdbl(dPurchaseAmount) & "," & Cdbl(dCashDeposit) & "," & Cdbl(dCashRequired) & ",'" & CStr(sLoanPurpose) & "','" & CStr(sLoanTransferType) & "'"
   
    strSQL = "w_GetCalculatorFees " & Cdbl(dPurchaseAmount) & "," & Cdbl(dCashDeposit) & "," & Cdbl(dCashRequired) & ",'" & CStr(sLoanPurpose) & "','" & CStr(sLoanTransferType) & "'," & Cdbl(dEstimatedPropValue) 
   ' strSQL = "w_GetCalculatorFees " & Cdbl(725000) & "," & Cdbl(15000) & "," & Cdbl(0) & ",'" & CStr("NewHome") & "','" & CStr("Natural") & "'," & Cdbl(900000) 
    
 	Set rsXML = cnnNW.Execute(strSQL)

   'Write the results to the client
	Response.Write Err.description

	strXML = "<root>"
	Do While Not rsXML.EOF
			strXML = strXML & "<Loan"
			strXML = strXML & " field1=" & DQ & rsXML("RegistrationFee") & DQ
			strXML = strXML & " field2=" & DQ & rsXML("CancelFee") & DQ
			strXML = strXML & " field3=" & DQ & rsXML("ValuationFee") & DQ
			strXML = strXML & " field4=" & DQ & rsXML("AdminFee") & DQ
			strXML = strXML & " field5=" & DQ & rsXML("TransferFee") & DQ
			strXML = strXML & " field6=" & DQ & rsXML("InterimInterestFee") & DQ
			strXML = strXML & "/>" & vbCRLF
			rsXML.MoveNext
	Loop 
	strXML = strXML & "</root>"
		
	Response.Write strip(strXML)
End Sub
%>
