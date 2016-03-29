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
      
   strConn = "Provider=SQLOLEDB;Application Name='TeleCalcRates.asp' ;Data Source=" & DB_SERVER & "; Initial Catalog=" & LIGHTHOUSE_DB
      
   strConn = strConn & ";User Id=" & Session("UserID")
   
   oConn.Open strConn
   
   set GetDataConnection = oConn
   
end function    
	
Sub GetFees
	Dim objDOM, strText, strSQL, cnnNW
	Dim strNbr,strDetails,strNbrType,strTmp
	Dim dLoanAmount, iLoanPurpose
	
	Dim rsXML,strXML,DQ
	DQ = Chr(34)
	
	set rsXML = Server.CreateObject("ADODB.RecordSet")

	'Open Database connection
	Set cnnNW = GetDataConnection()
	
	'Create an XML DOM from the incoming XML
	set objDOM = Server.CreateObject("Microsoft.XMLDOM")
	objDOM.Load(Request)
	
	'Build the Insert SQL Statement
   strSQL = "tlc_GetCalculatorRates"
 
 	Set rsXML = cnnNW.Execute(strSQL)

   'Write the results to the client
	Response.Write Err.description

	strXML = "<root>"
	Do While Not rsXML.EOF
			strXML = strXML & "<Loan"
			strXML = strXML & " field1=" & DQ & rsXML("JIBARRate") & DQ
			strXML = strXML & " field2=" & DQ & rsXML("SAHLRate") & DQ
			strXML = strXML & " field3=" & DQ & rsXML("BankRate") & DQ	
			strXML = strXML & " field4=" & DQ & rsXML("SAHLFixedRate") & DQ		
			strXML = strXML & "/>" & vbCRLF
			rsXML.MoveNext
	Loop 
	strXML = strXML & "</root>"
		
	Response.Write strip(strXML)
End Sub
%>
