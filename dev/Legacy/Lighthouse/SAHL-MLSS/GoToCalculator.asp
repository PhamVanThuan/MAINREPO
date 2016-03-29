<%@  language="VBScript" %>
<%
Option explicit
Response.Expires = 0
%>
<!--#include file="TeleXMLFuncs.asp"-->
<%

Dim sLoanNumber, sFolderID, sUserID
Dim sDebug
Dim fFixedPC, fVariablePC
Dim iCreditMatrixNumber

sDebug = "Y"

Response.Write "<P>Beginning page...<P>"

Call Init
Call RedirectToCalc

function GetDataConnection()
   dim oConn, strConn

   Set oConn = Server.CreateObject("ADODB.Connection")

   strConn = "Provider=SQLOLEDB;Data Source=" & DB_SERVER & "; Initial Catalog=" & LIGHTHOUSE_DB

   strConn = strConn & ";User Id=" & DB_EWORK_USERID
   oConn.Open strConn
   set GetDataConnection = oConn
end function

Sub Init

	Response.Write "<P>Init...<P>"

	Dim oCon, oRS
	Dim sSQL, sql

	On Error Resume Next

	sSQL = "Provider=SQLOLEDB;Data Source=" & DB_SERVER & ";Initial Catalog=" & DB_EWORK & ";User Id=" & DB_EWORK_USERID & ";"

	sFolderID = 0
	sFolderID = Request.QueryString("folderID")

	Set oCon = Server.CreateObject("ADODB.Connection")
	oCon.ConnectionString = sSQL
	oCon.Open

	if err.number <> 0 then
		Response.Write "<P>2. Err.Number = " & err.number & vbCrLf & "Err.Description = " & err.Description & ".</P>"
		Response.End
	end if

	Set oRS = Server.CreateObject("ADODB.Recordset")
	sql = "SELECT hd.LoanNumber, isnull(hd.FlexiFixedPC, 0) as FlexiFixedPC, isnull(hd.FlexiVariablePC, 100) As FlexiVariablePC, hd.UserToDo, ml.MortgageLoanPurposeKey as CreditMatrixNumber FROM HelpDesk hd  (nolock) "
	sql = sql & " join [2am].dbo.FinancialService fs (nolock) on hd.LoanNumber = fs.AccountKey and fs.AccountStatusKey = 1 and fs.FinancialServiceTypeKey = 1"
    sql = sql & " join [2am].fin.MortgageLoan ml (nolock) on fs.FinancialServiceKey = ml.FinancialServiceKey "
	sql = sql & " WHERE hd.eFolderID = '" & sFolderID & "'"

	oRS.Open sql, oCon

	if err.number <> 0 then
		Response.Write "<P>2. Err.Number = " & err.number & vbCrLf & "Err.Description = " & err.Description & ".</P>"
		Response.End
	end if

	if not oRS is nothing then
		if not oRS.EOF then
			sLoanNumber = oRS.Fields("LoanNumber")
			fFixedPC = oRS.Fields("FlexiFixedPC")
			fVariablePC = oRS.Fields("FlexiVariablePC")
			sUserID = oRS.Fields("UserToDo")
			iCreditMatrixNumber = oRS.Fields("CreditMatrixNumber")
		else
			sLoanNumber = ""
			sUserID = DB_EWORK_USERID
			iCreditMatrixNumber = 16
		end if
	else
		sLoanNumber = ""
		sUserID = DB_EWORK_USERID
		iCreditMatrixNumber = 16
	end if

	if sUserID = "" then sUserID = DB_EWORK_USERID

	Response.Write "<P>LoanNumber = " & sLoanNumber & ".</P>"

End Sub

Sub RedirectToCalc

	Dim oCon, oRS, sSQL, sURL

	Dim sTerm, sLoanPurpose, sEmploymentType, sMarketValue, sPurchasePrice, sLoanAmount
	Dim sFixedPC, sVariablePC, sName, sNumber, sIncome, sLinkRate, sLoanRate

	Response.Write "<P>RedirectToCalc ...</P>"

	On Error Resume Next

	oCon = GetDataConnection()

	if err.number <> 0 then
		if sDebug = "Y" then
			Response.Write "<P>1. Err.Number = " & err.number & vbCrLf & "Err.Description = " & err.Description & ".</P>"
			Response.End
		end if
	end if

	Set oRS = Server.CreateObject("ADODB.Recordset")

	if err.number <> 0 then
		Response.Write "<P>2. Err.Number = " & err.number & vbCrLf & "Err.Description = " & err.Description & ".</P>"
		Response.End
	end if

    sSQL = "select top 1 MortgageLoanPurposeKey As LoanPurpose, "
    sSQL = sSQL & " case  "
    sSQL = sSQL & " when oivl.EmploymentTypeKey = 5 then 1 "
    sSQL = sSQL & " else isnull(oivl.EmploymentTypeKey,1) "
    sSQL = sSQL & " end as EmploymentType,   "
    sSQL = sSQL & " oivl.LoanAgreementAmount as ProspectLoan "
    sSQL = sSQL & " from [2am]..offer o (nolock) "
    sSQL = sSQL & " join [2am]..OfferMortgageLoan oml (nolock) on o.OfferKey = oml.OfferKey "
    sSQL = sSQL & " join [2am]..OfferInformation oi (nolock) on o.offerkey = oi.offerkey and oi.OfferInformationTypeKey = 3 "
    sSQL = sSQL & " join [2am]..offerinformationvariableloan oivl (nolock) on oi.offerinformationkey = oivl.offerinformationkey "
    sSQL = sSQL & " where o.AccountKey = " & sLoanNumber & " and o.offerStatusKey = 3 and o.OfferTypeKey in (6, 7, 8) "

	oRS.Open sSQL, oCon

	if err.number <> 0 then
		Response.Write "<P>3. Err.Number = " & err.number & vbCrLf & "Err.Description = " & err.Description & ".</P>"
		Response.Write "<P>sSQl = " & sSQL & ".</P>"
		Response.End
	end if

	if not oRS is nothing then
		if not oRS.EOF then
			sLoanPurpose = oRS.Fields("LoanPurpose")
			sEmploymentType = oRS.Fields("EmploymentType")
			sLoanAmount = oRS.Fields("ProspectLoan")
		end if
	end if

	if err.number <> 0 then
		Response.Write "<P>4. Err.Number = " & err.number & vbCrLf & "Err.Description = " & err.Description & ".</P>"
		Response.End
	end if

	oRS.Close

	if err.number <> 0 then
		Response.Write "<P>5. Err.Number = " & err.number & vbCrLf & "Err.Description = " & err.Description & ".</P>"
		Response.End
	end if

	sSQL = "SELECT Loan.PropertyLatestValuation As MarketValue, Loan.LoanInitialBalance As PurchasePrice, Loan.LoanCurrentBalance As LoanAmount, "
	sSQL = sSQL & "convert(numeric(4,4),Loan.LoanLinkRate) as LoanLinkRate, Loan.LoanRate, Client.ClientFirstNames + ' ' + Client.ClientSurname As [Name], "
	sSQL = sSQL & "Loan.LoanRemainingInstallments As Term, Client.ClientIncome + Client.ClientSpouseIncome As [Income] FROM "
	sSQL = sSQL & "[sahldb]..vw_AllLoans Loan (nolock) INNER JOIN [sahldb]..Client (nolock) ON Loan.ClientNumber = Client.ClientNumber WHERE "
	sSQL = sSQL & "Loan.LoanNumber = " & sLoanNumber

	oRS.Open sSQL, oCon

	if err.number <> 0 then
		Response.Write "<P>6. Err.Number = " & err.number & vbCrLf & "Err.Description = " & err.Description & ".</P>"
		Response.End
	end if

	'/** Get the balance of the parameter info
	if not oRS is nothing then
		if not oRS.EOF then
			sMarketValue = oRS.Fields("MarketValue")
			sPurchasePrice = oRS.Fields("PurchasePrice")
			if round(oRS.Fields("PurchasePrice"),2) <> 0 then
			    sLoanAmount = oRS.Fields("LoanAmount")
            end if
			sLinkRate = oRS.Fields("LoanLinkRate")
			sLoanRate = oRS.Fields("LoanRate")
			sName = oRS.Fields("Name")
			sIncome = oRS.Fields("Income")
			sTerm = oRS.Fields("Term")
		end if
	end if

	oRS.Close

	if err.number <> 0 then
		Response.Write "<P>7. Err.Number = " & err.number & vbCrLf & "Err.Description = " & err.Description & ".</P>"
		Response.End
	end if

	sName = replaceAmp(sName)

	'/** have all variables now redirect to calculator
	sURL = "TeleCalculatorFlexi.asp?Purpose=" & sLoanPurpose & "&EmploymentType=" & sEmploymentType & "&MarketValue=" & sMarketValue
	sURL = sURL & "&LoanAmount=" & sLoanAmount & "&CashDeposit=0&CashRequired=0&Term=" & sTerm
	sURL = sURL & "&LinkRate=" & sLinkRate & "&VariableLoanRate=" & sLoanRate & "&FixedPercent=" & fFixedPC & "&VariablePercent=" & fVariablePC & "&Source=eWorkFlexi&Name=" & sName
	sURL = sURL & "&Number=" & sLoanNumber & "&Income=" & sIncome & "&folderID=" & sFolderID & "&UserName=" & sUserID & "&CreditMatrixNumber=" & iCreditMatrixNumber

	Response.Write sURL
	Response.Redirect sURL

End Sub

Function replaceAmp(sVar)

    Dim sTemp, sRet
    Dim i
    sTemp = sVar

    for i = 1 to len(sVar)

        if left(sTemp, 1) <> "&" then
            sRet = sRet & left(sTemp, 1)
        else
            sRet = sRet & "and"
        end if
        sTemp = right(sTemp, len(sTemp) - 1)

    next

    replaceAmp = sRet

End Function

%>
<html>
<head>
    <!--#include file="server.asp"-->
</head>
</html>