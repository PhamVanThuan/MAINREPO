<SCRIPT ID=clientEventHandlersJS LANGUAGE=vbscript>
<!--
'---- CursorTypeEnum Values ----
Const adOpenForwardOnly = 0
Const adOpenKeyset = 1
Const adOpenDynamic = 2
Const adOpenStatic = 3

'---- CursorOptionEnum Values ----
Const adHoldRecords = &H00000100
Const adMovePrevious = &H00000200
Const adAddNew = &H01000400
Const adDelete = &H01000800
Const adUpdate = &H01008000
Const adBookmark = &H00002000
Const adApproxPosition = &H00004000
Const adUpdateBatch = &H00010000
Const adResync = &H00020000
Const adNotify = &H00040000
Const adFind = &H00080000
Const adSeek = &H00400000
Const adIndex = &H00800000

'---- LockTypeEnum Values ----
Const adLockReadOnly = 1
Const adLockPessimistic = 2
Const adLockOptimistic = 3
Const adLockBatchOptimistic = 4

'---- CursorLocationEnum Values ----
Const adUseServer = 2
Const adUseClient = 3

'---- PositionEnum Values ----
Const adPosUnknown = -1
Const adPosBOF = -2
Const adPosEOF = -3

'myModule is a global constant defined in the host page

'open a connection for all subsequent calls
Dim myConn
function OpenConn(moduleName)
	Dim constring
	on error resume next
	constring="Provider=MSDASQL;Application Name=SAHL-MLSS " & moduleName & ";Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"  
	set myConn = createobject("ADODB.Connection")
	myConn.Open constring
	if err then
		msgbox "Error:" & err.description & vbcrlf & constring
	end if
	set OpenConn = myConn
end function

Function GetConnectionString(sModule)
	'who on earth is using this?  it don't work!
	GetConnectionString = "Provider=MSDASQL;Application Name=SAHL-MLSS '& sModule & ';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"  
End function

Function OpenRecordset(sql, cursorLocation, cursorType)
	dim rs
	on error resume next
	set rs  = createobject("ADODB.Recordset")
	rs.CursorLocation = cursorLocation
	rs.Open sql, myConn, cursorType
	if err then
		msgbox "Error:" & err.description & vbcrlf & sql
		exit function
	end if
	set OpenRecordset = rs
end Function

Function ConnExecute(sql)
	dim rs, constring
	on error resume next
	myConn.execute sql
	if err then
		msgbox "Error:" & err.description & vbcrlf & sql
		exit function
	end if
	ConnExecute=myConn.RecordsAffected
End Function

Function GetLoanBondRecord(LoanNumber, sModule)
	GetLoanBondRecord = 0
	set conn = createobject("ADODB.Connection")
	sDSN = GetConnectionString(sModule)
	conn.Open sDSN
	set rs_Bond  = createobject("ADODB.Recordset")
	sSQL = "SELECT COUNT(BondNumber) FROM BOND WHERE LoanNumber = " & LoanNumber
	rs_Bond.CursorLocation = 3
	rs_Bond.Open sSQL,conn,adOpenStatic
	GetLoanBondRecord = rs_bond.Fields(0).Value
	rs_Bond.close
End Function

//-->
</SCRIPT>