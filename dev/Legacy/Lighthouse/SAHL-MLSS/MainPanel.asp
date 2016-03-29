<%if Session("UserID")="" then response.Redirect "default.asp"%>
<html>
<head>
<meta name="VI60_defaultClientScript" content="VBScript">
<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<title></title>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
</head>
<body leftMargin="0" topMargin="0" rightMargin="0"  bottomMargin="0"  class=Generic>

<p>&nbsp;</p>
<%
set conn = createobject("ADODB.Connection")
sDSN = "Provider=MSDASQL;Application Name=SAHL-MLSS[Main.asp] ;Data Source=" & Session("SQLDatabase") & ";uid=" & Session("UserID")
conn.Open sDSN
set rs = createobject("ADODB.Recordset")
rs.CursorLocation = 3

sub rsOpen(sql)
	on error resume next
	rs.close
	err.Clear
	rs.Open sql,conn,adOpenStatic
	if err then
		response.Write "<p>" & err.Description & "</P>"
		response.Write "<p>" & sql & "</P>"
		response.End
	end if
end sub
function FormatRand(byref amount)
	FormatRand="R " & FormatNumber(amount, 2, true,true,true)
end function
	
sSQL = "SELECT COUNT(TeleProspectNumber) ,  SUM(ISNULL(TeleProspectLoan , 0))"
sSQL = sSQL & " FROM TeleProspect (nolock) WHERE "
call rsOpen(sSQL & "TeleProspectStatusNumber=2 and TeleProspectValidationFlag=0")
%>

<!-- TELECENTER --->
<TABLE border=0 cellPadding=1 cellSpacing=1 class=Generic dataSrc="" 
style="BORDER-RIGHT: 0px groove; BORDER-TOP: 0px groove; Z-INDEX: 101; LEFT: 11px; BORDER-LEFT: 0px groove; WIDTH: 479px; BORDER-BOTTOM: 0px groove; POSITION: absolute; TOP: 2px; HEIGHT: 228px" 
width=400 ID="Table3">
  
  <TR>
    <TD align=middle class=Header1 colSpan=3 noWrap style="HEIGHT: 20px" >TeleCentre</TD></TR>
  <TR>
    <TH align=left>Status</TH>
    <TH align=right>Count</TH>
    <TH align=right>Value</TH></TR>
  <TR>
    <TD noWrap>Declined Leads</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "TeleProspectStatusNumber=3 and TeleProspectValidationFlag=0")
%>
  <TR>
    <TD noWrap>NTU Leads</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "TeleProspectStatusNumber=2 and TeleProspectValidationFlag>=1")
%>
  <TR>
    <TD noWrap>Declined TeleProspects</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD>
    </TR>
<%
call rsOpen(sSQL & "TeleProspectStatusNumber=3 and TeleProspectValidationFlag>=1")
%>
    <TR>
    <TD noWrap>NTU TeleProspects</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD>
    </TR>
 
  <TR>
    <TD noWrap height=15 style="HEIGHT: 15px"></TD>
    <TD align=right noWrap></TD>
    <TD align=right noWrap></TD>
    </TR>
<%
call rsOpen(sSQL & "TeleProspectStatusNumber=4 and TeleProspectValidationFlag=0")
totCount=rs.Fields(0).Value
totValue=rs.Fields(1).Value
%>
<TR>
    <TD noWrap>Leads</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "TeleProspectStatusNumber=5 and TeleProspectValidationFlag>=1")
totCount=totCount+rs.Fields(0).Value
totValue=totValue+rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>TeleProspects</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "TeleProspectStatusNumber=6 and TeleProspectValidationFlag>=1")
totCount=totCount+rs.Fields(0).Value
totValue=totValue+rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Transferred to Origination</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
  <TR>
    <TD align=right style="BORDER-BOTTOM: black 0px double">Total</TD>
    <TD align=right noWrap 
    style="BORDER-TOP: black 2px double; BORDER-BOTTOM: black 4px double"><%=formatNumber(totCount,0, true,true,true)%>
</TD>
    <TD align=right noWrap 
    style="BORDER-TOP: black 2px double; BORDER-BOTTOM: black 4px double"><%=FormatRand(totValue)%>
</TD></TR></TABLE>
<%response.Flush%>

<!--  ORIGINATION -->
<TABLE datasrc="" border=0 cellPadding=1 cellSpacing=1 
style="BORDER-RIGHT: 0px groove; BORDER-TOP: 0px groove; Z-INDEX: 101; LEFT: 10px; BORDER-LEFT: 0px  groove; WIDTH: 479px; BORDER-BOTTOM: 0px groove; POSITION: absolute; TOP: 270px; HEIGHT:    
218px" width=400 class=Generic ID="Table1">
  <TR>
    <TD align=middle colSpan=3 noWrap class=Header1 style="HEIGHT: 20px">Origination</TD></TR>
  <TR>
    <TH align=left>Stage</TH>
    <TH align=right>Count</TH>
    <TH align=right>Value</TH></TR>
  <TR>
    <TD noWrap> New Prospects</TD>
<%
sSQL = "SELECT 	ISNULL(COUNT(ProspectNumber) , 0) ,ISNULL(SUM(ProspectLoan) , 0.00)"
sSQL = sSQL & " FROM PROSPECT (nolock) WHERE "
call rsOpen(sSQL & "CONVERT(char(12),ProspectInsertDate,101)  = CONVERT(char(12),GETDATE(),101)")
totCount=rs.Fields(0).Value
totValue=rs.Fields(1).Value
%>
    <TD align=right noWrap ><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "ProspectStage=1 AND ProspectCreditCheckStatus=1")
totCount=totCount+rs.Fields(0).Value
totValue=totValue+rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Credit Check</TD>
    <TD align=right noWrap ><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "ProspectStage=2 AND ProspectDeedOfficeStatus=1")
totCount=totCount+rs.Fields(0).Value
totValue=totValue+rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Deeds Office</TD>
    <TD align=right noWrap ><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "ProspectStage=3 AND ProspectAgentStatus=1")
totCount=totCount+rs.Fields(0).Value
totValue=totValue+rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Consultant</TD>
    <TD align=right noWrap ><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "ProspectStage=4 AND ProspectValuationStatus=1")
totCount=totCount+rs.Fields(0).Value
totValue=totValue+rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Valuation</TD>
    <TD align=right noWrap ><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "ProspectStage=5 AND ProspectGrantedStatus=1")
totCount=totCount+rs.Fields(0).Value
totValue=totValue+rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Final Approval</TD>
    <TD align=right noWrap ><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
  <TR>
    <TD align=right style="BORDER-BOTTOM: black 0px double" 
     >Total</TD>
    <TD align=right noWrap style="BORDER-TOP: black 2px double; BORDER-BOTTOM: black 4px double">
    <%=formatNumber(totCount,0, true,true,true)%>
</TD>
    <TD align=right noWrap style="BORDER-TOP: black 2px double; BORDER-BOTTOM: black 4px double">
    <%=FormatRand(totValue)%>
</TD>
    </TR>
  </TABLE>
<%response.Flush%>
  
<!-- REGISTRATION -->
<TABLE datasrc="" border=0 cellPadding=1 cellSpacing=1 
style="BORDER-RIGHT: 0px groove; BORDER-TOP: 0px groove; Z-INDEX: 102; LEFT: 498px; BORDER-LEFT: 0px  groove; WIDTH: 497px; BORDER-BOTTOM: 0px groove; POSITION: absolute; TOP: 2px; HEIGHT:    
194px" width=350 class=Generic id=tbl_Registration>
  <TR>
    <TD align=center colSpan=3 noWrap class=Header1 style="HEIGHT: 22px">Registration</TD>
     </TR>
  <TR>
    <TH align=left>Queues</TH>
    <TH align=right>Count</TH>
    <TH align=right>Value</TH></TR>
  <TR>
    <TD noWrap>Take On</TD>
<%
sSQL = "SELECT	ISNULL(COUNT(ProspectNumber) , 0) , ISNULL(SUM (ProspectLoan) , 0.00)"
ssql = ssql & " FROM PROSPECT (nolock) WHERE ProspectStage=6 AND ProspectClientTakeOnStatus=1"
call rsOpen(sSQL)
totCount=rs.Fields(0).Value
totValue=rs.Fields(1).Value
%>
    <TD align=right noWrap ><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
  <TR>
<%
response.Flush
sSQL = "SELECT  DISTINCT ISNULL(COUNT(ProspectNumber) , 0) ,ISNULL(SUM (ProspectLoan) , 0.00) "
ssql = ssql & " FROM Prospect (nolock)"
ssql = ssql & " INNER JOIN Detail (nolock) on Prospect.LoanNumber = Detail.LoanNumber"
ssql = ssql & " INNER JOIN Regmail (nolock) on PROSPECT.LoanNumber = REGMAIL.LoanNumber AND"
ssql = ssql & " PROSPECT.PurposeNumber = REGMAIL.PurposeNumber"
ssql = ssql & " WHERE DETAIL.DetailTypeNumber "
call rsOpen(sSQL & "IN(1, 2,7,8)")
totCount=rs.Fields(0).Value
totValue=rs.Fields(1).Value
%>
    <TD noWrap>Instruct Attorney (Not Sent)</TD>
    <TD align=right noWrap ><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "=3")
totCount=totCount + rs.Fields(0).Value
totValue=totValue + rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Instructions Sent (Awaiting Reply)</TD>
    <TD align=right noWrap ><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "IN(4,5)")
totCount=totCount + rs.Fields(0).Value
totValue=totValue + rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Replies Received</TD>
    <TD align=right noWrap ><%=rs.Fields(0).Value%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
call rsOpen(sSQL & "=9")
totCount=totCount + rs.Fields(0).Value
totValue=totValue + rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Lodgement</TD>
    <TD align=right noWrap ><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap ><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<TR><TD align=right>Total</TD><TD  align=right
    style="BORDER-TOP: black 2px solid; BORDER-BOTTOM-COLOR: black; BORDER-BOTTOM-STYLE: double" 
    ><%=formatNumber(totCount,0, true,true,true)%>
</TD><TD  align=right
    style="BORDER-TOP: black 2px solid; BORDER-LEFT-WIDTH: 2px; BORDER-BOTTOM: black double; BORDER-RIGHT-WIDTH: 2px" 
    ><%=FormatRand(totValue)%>
</TD></TR>
  </TABLE>
<%response.Flush%>

<!-- MORTGAGE LOANS -->
<TABLE border=0 cellPadding=1 cellSpacing=1 dataSrc="" 
style="Z-INDEX: 103; LEFT: 497px; WIDTH: 504px; POSITION: absolute; TOP: 270px; HEIGHT:                 
250px" width=400 class=Generic id="Table2" height=200>
  <TR>
    <TD align=middle colSpan=3 noWrap class=Header1 style="HEIGHT: 20px">Mortgage Loans</TD></TR>
  <TR>
    <TH></TH>
    <TH align=right>Count</TH>
    <TH align=right>Value</TH></TR>
<%
sSQL = "SELECT ISNULL(COUNT(RegMailNumber)  , 0 ) , ISNULL(SUM(RegMailGuaranteeAmount)  , 0.00 )"
ssql = ssql & " FROM REGMAIL (nolock) WHERE"
ssql = ssql & " (RegMailBondDate > GETDATE() - 1 AND RegMailBondDate < GETDATE() +1 and RegMailGuaranteeAmount > 0)"
ssql = ssql & " OR (RegMailBondDate > GETDATE() - 1 AND RegMailBondDate < GETDATE() +1 and RegMailCashRequired > 0 AND "
ssql = ssql & "   PurposeNumber = 5 and DetailTypeNumber =6 )"
call rsOpen(sSQL)
%>
  <TR>
    <TD noWrap>Today's Disbursements</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
  <TR>
    <TD noWrap>&nbsp;</TD>
    <TD align=right noWrap></TD>
    <TD align=right noWrap></TD></TR>
    <TR>
    <TD noWrap height=1 style="HEIGHT: 1px"></TD>
    <TD align=right noWrap height=1 style="HEIGHT: 1px" 
   ></TD>
    <TD align=right noWrap height=1 style="HEIGHT: 1px" 
   ></TD></TR>
<%
sSQL = "SELECT ISNULL( COUNT( LoanNumber ) , 0 ) , ISNULL( SUM( LoanCurrentBalance) , 0.00 )"
ssql = ssql & " FROM LOAN (nolock) WHERE LoanNumber in "
ssql = ssql & " (Select LoanNumber  from vw_openLoans (nolock))"
call rsOpen(sSQL)
%>
   <TR>
    <TD noWrap>Open Loan Accounts (Current Balance)</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
    <TR>
    <TD noWrap height=1 style="HEIGHT: 1px">&nbsp;</TD>
    <TD align=right noWrap height=1 style="HEIGHT: 1px" 
   ></TD>
    <TD align=right noWrap height=1 style="HEIGHT: 1px" 
   ></TD></TR>
    
  <TR>
    <TD noWrap height=1 style="HEIGHT: 1px">&nbsp;</TD>
    <TD align=right noWrap height=1 style="HEIGHT: 1px" 
   ></TD>
    <TD align=right noWrap height=1 style="HEIGHT: 1px" 
   ></TD></TR>
<%
sSQL = "SELECT ISNULL( COUNT( LoanNumber ) , 0 ) , ISNULL( SUM( LoanInitialBalance) , 0.00 )"
ssql = ssql & " FROM LOAN (nolock) WHERE LoanNumber in "
ssql = ssql & " (select loannumber from Bond (nolock))"
call rsOpen(sSQL)
totCount= rs.Fields(0).Value
totValue= rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Opened Loan Accounts (Initial Balance)</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
<%
sSQL = "SELECT ISNULL( COUNT( LoanNumber ) , 0 ) , ISNULL( SUM( LoanInitialBalance) , 0.00 )"
ssql = ssql & " FROM LOAN (nolock) WHERE LoanNumber in "
ssql = ssql & " (select loannumber from vw_ClosedLoans (nolock))"
call rsOpen(sSQL)
totCount=totCount - rs.Fields(0).Value
totValue=totValue - rs.Fields(1).Value
%>
  <TR>
    <TD noWrap>Closed Loan Accounts (Initial Balance)</TD>
    <TD align=right noWrap><%=formatNumber(rs.Fields(0).Value,0, true,true,true)%>
</TD>
    <TD align=right noWrap><%=FormatRand(rs.Fields(1).Value)%>
</TD></TR>
  <TR>
    <TD align=right style="BORDER-BOTTOM: black 0px double">Total</TD>
    <TD align=right noWrap 
    style="BORDER-TOP: black 2px double; BORDER-BOTTOM: black 4px double"><%=formatNumber(totCount,0, true,true,true)%>
</TD>
    <TD align=right noWrap 
    style="BORDER-TOP: black 2px double; BORDER-BOTTOM: black 4px double"><%=FormatRand(totValue)%>
</TD></TR></TABLE>
</body>

</html>
