<%

  sDatabase =Session("SQLDatabase")
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_LoanTransaction = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Financial Transaction",Session("UserName"))

  sLoanNbr =Request.Form.Item("LastLoanNbr")
  sLastLoanNbr = Request.Cookies("LastLoanNumber")

 'Response.Write "last is " & sLoanNbr

if sLoanNbr <>  "" and sLoanNbr > 0 then
 sLastLoanNbr = sLoanNbr
 Response.cookies("LastLoanNumber")=sLoanNbr
 Response.cookies("LastLoanNumber").Expires = "01/01/2010"
else

  sLastLoanNbr  = Request.Cookies("LastLoanNumber")
  sLoanNbr = sLastLoanNbr
end if

%>
<html>
<head>
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->
<!--#include virtual="/SAHL-MLSS/database.inc"-->

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>

<meta name="VI60_DefaultClientScript" Content="VBScript">

<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<script ID="clientEventHandlersVBS" LANGUAGE="vbscript">
<!--
Dim s_ReturnPage
Dim s_ReturnPanel
Dim i_LoanNumber

dim i_CurrentLoanNbr

Dim v_BookMark
Dim i_EmployeeType
Dim i_SAHLEmployeeNumber
Dim i_EmployeeTeamNumber
Dim s_CurrentLoanNbr
Dim b_loading
Dim b_AllDataLoaded

Dim s_date1
Dim s_date2

if rs_open1 <> true then

   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"

	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   return
    end if

	set conn = createobject("ADODB.Connection")
	set rs_Loan  = createobject("ADODB.Recordset")
	set rs_LoanTxns  = createobject("ADODB.Recordset")
	set rs_TransactionType = createobject("ADODB.Recordset")
    set rs_temp = createobject("ADODB.Recordset")
    set rs_ControlFile = createobject("ADODB.Recordset")
    set rs_LoanDetail  = createobject("ADODB.Recordset")
    set rs_LoanDetail1  = createobject("ADODB.Recordset")

	sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [LoanTransactions.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	conn.Open sDSN

	b_AllDataLoaded = false

end if

Sub btn_Exit_onclick

    if s_ReturnPanel ="RegistrationPanel" then
       i_LoanNumber = 0
    end if
	window.parent.frames(s_ReturnPanel).location.href = s_ReturnPage & "?Number= " & CStr(i_LoanNumber)
End Sub

Sub window_onload

 SetAccessLightsServer

 tbl_TransactionInfo.focus
 window.TxnEffectiveDate.DropDown.Visible = 1
 window.TxnEffectiveDate.Spin.Visible = 1
 window.TxnEffectiveDate.Value = date()
 window.TxnAmount.Value = 0.00

 i_Nbr = "<%=Request.QueryString("Number")%>"

 s_Source = "<%=Request.QueryString("Source")%>"
 s_ReturnPage = s_Source
 s_Panel = "<%=Request.QueryString("Panel")%>"
 s_ReturnPanel = s_Panel

 i_LoanNumber = i_Nbr
 s_Purpose = "<%=Request.QueryString("purpose")%>"

 if s_Source = "Disbursements.asp" then
    window.btn_Exit.style.visibility = "visible"
    window.btn_Exit.disabled = false
    window.btn_LoanTransaction.disabled = true
    window.btn_LoanTransaction.style.visibility = "hidden"
    window.pic_FinancialTxn.style.visibility = "hidden"
    i_CurrentLoanNbr = i_LoanNumber
    GetPostDisbursementData()
    ConfigurePostDisbursementGrid

    i_CurrentLoanNbr = i_LoanNumber
 else
     window.btn_Exit.style.visibility = "hidden"
     window.btn_Exit.disabled = true
     window.btn_LoanTransaction.disabled = false
     window.btn_LoanTransaction.style.visibility = "visible"
	 b_AllDataLoaded = false
	 b_loading = true
	 i_CurrentLoanNbr = 0
	 i_LastLoanNbr = 0
	 i_CurrentLoanNbr = "<%=sLoanNbr%>"
	 sLastNbr = "<%=sLastLoanNbr%>"

	 v = ""
	 v = "<%=Request.QueryString("Number")%>"

	 if  v <> "" then

	  i_CurrentLoanNbr = v

	 else

	  i_CurrentLoanNbr = window.parent.frames(0).CurrentLoanNbr.value

	 end if

	 if sLastNbr <> "" then

	    i_LastLoanNbr =  sLastNbr
	    window.parent.frames(0).CurrentLoanNbr.value = i_LastLoanNbr
	 end if

    if i_CurrentLoanNbr = "" then i_CurrentLoanNbr = 0

	if  i_CurrentLoanNbr = 0 and i_LastLoanNbr = 0 then

		window.location.href = "FindLoan.asp"
		window.close
		exit sub
	end if

	if i_CurrentLoanNbr = 0 then
	   i_CurrentLoanNbr = i_LastLoanNbr
	end if

	'msgbox "LOAN NBR >" & i_CurrentLoanNbr & "<"

	window.parent.frames(0).CurrentLoanNbr.value = i_CurrentLoanNbr

    GetLoanTransactions()
    if rs_Loan.RecordCount < 1  then
		window.location.href = "FindLoan.asp"
		window.close
		exit sub
	end if
    ConfigureLoanTransactionGrid
    window.parent.frames(0).CurrentClientNbr.value = rs_Loan.Fields("ClientNumber").Value

 end if

  if s_Source = "LoanCalculator.asp" or s_Source = "Disbursements.asp" then
    window.radio_AllTransactions.click
  else
	window.radio_FinancialTxns.click
  end if

  sSQL = "SELECT DetailTypeNumber from DETAIL with (nolock) WHERE DetailTypeNumber = 10 AND LoanNumber = " & i_CurrentLoanNbr
  rs_LoanDetail.CursorLocation = 3
  rs_LoanDetail.Open sSQL,conn,adOpenStatic

   'Hide all buttons if the loan is closed and current balance is zero
	if rs_LoanDetail.RecordCount > 0 then
	  if Cint(rs_LoanDetail.Fields("DetailTypeNumber").Value) = 10 then
	     window.btn_LoanTransaction.style.visibility = "hidden"
		 window.pic_FinancialTxn.style.visibility = "hidden"
	     window.DataCombo_TxnType.Enabled = false
	     window.TxnEffectiveDate.Enabled = false
	     window.TxnAmount.Enabled = false
	     window.TxnReference.Enabled = false
	     window.msg_locked.style.visibility = "visible"

	   end if
	 end if

  rs_LoanDetail.Close

  if rs_LoanTxns.RecordCount > 0 then
	 rs_LoanTxns.MoveLast
  end if

window.TxnEffectiveDate.Value =date()
window.TxnAmount.Value = 0.00
window.TxnReference.Text = ""

 if  GetLoanBondRecord ( i_CurrentLoanNbr , "[LoanTransactions.asp]" ) < 1 then
	btn_LoanTransaction.style.visibility = "hidden"
	pic_FinancialTxn.style.visibility = "hidden"
 end if

End Sub

Function GetPostDisbursementData
  if rs_open1 = true  then
       rs_Loan.Close
       rs_open1 = false
	end if

    sSQL = "t_GetPostDisbursementDetails " & i_LoanNumber
    rs_Loan.CursorLocation = 3
	rs_Loan.Open sSQL,conn,adOpenDynamic

	sSQL = "t_GetPostDisbursementTxns " & i_LoanNumber
    rs_LoanTxns.CursorLocation = 3
	rs_LoanTxns.Open sSQL,conn,adOpenDynamic

    window.TrueDBGrid.DataSource = rs_LoanTxns

rs_open1 = true
End function

Function GetLoanTransactions
  if rs_open1 = true  then
       rs_Loan.Close
       rs_open1 = false
	end if

    sSQL = "c_GetLoanHeaderDetails " & i_CurrentLoanNbr
    rs_Loan.CursorLocation = 3
	rs_Loan.Open sSQL,conn,adOpenDynamic

	if rs_Loan.RecordCount < 1 then
		window.location.href = "FindLoan.asp"
		window.close
		exit function
	end if

	window.LoanNumber.Text = rs_Loan.Fields("LoanNumber").Value
	window.ClientNames.Text = rs_Loan.Fields("ClientNames").Value
	window.LoanOutStandingBal.value = rs_Loan.Fields("LoanCurrentBalance").Value
	window.LoanArrearBal.value = rs_Loan.Fields("LoanArrearBalance").Value
	window.LoanRate.value = rs_Loan.Fields("LoanRate").Value
	window.LoanOutstandingTerm.value = rs_Loan.Fields("LoanRemainingInstallments").Value
	window.LoanInstallment.value = rs_Loan.Fields("LoanInstallmentAmount").Value
	window.LoanOpenDate.value = rs_Loan.Fields("LoanOpenDate").Value

	sSQL = "c_GetLoanFinancialTransactions " & i_CurrentLoanNbr
    rs_LoanTxns.CursorLocation = 3
	rs_LoanTxns.Open sSQL,conn,adOpenDynamic

    window.TrueDBGrid.DataSource = rs_LoanTxns

rs_open1 = true
End function

Function ConfigurePostDisbursementGrid
 document.body.style.cursor = "hand"

	For I = 0 to TrueDBGrid.Columns.Count - 1
		TrueDBGrid.Columns.Remove(0)
	Next

   set Itm = CreateObject("TrueOleDBGrid60.ValueItem")
   set FormatStyle = CreateObject("TrueOleDBGrid60.Style")

    'Create then necessary columns...
	set tmpCol =  TrueDBGrid.Columns.Add(0)
	tmpCol.Width = 60
	tmpCol.Caption = "Number"
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionNumber").name
	tmpCol.Visible = True
	set tmpCol =  TrueDBGrid.Columns.Add(1)
	tmpCol.Caption = "Type"
	tmpCol.Width =150
	tmpCol.DataField = rs_LoanTxns.Fields("TransactionTypeNumber").name
	tmpCol.Visible = True

	call TDBOLeGridColumnTranslate(TrueDBGrid,1 ,"TransactionType", "TransactionTypeNumber", "TransactionTypeLoanDescription" )

    set tmpCol =  TrueDBGrid.Columns.Add(2)
	tmpCol.Caption = "Reference"
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionReference").name
	tmpCol.Width = 130
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(3)
	tmpCol.Caption = "Changed By"
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionUserID").name
	 tmpCol.Width = 80
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(4)
	tmpCol.Caption = "Insert Date"
	tmpCol.Width = 80
	tmpcol.alignment = 1
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionInsertDate").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(5)
	tmpCol.Caption = "Effective Date"
	tmpCol.Alignment = 3
    tmpCol.Width = 100
    tmpcol.alignment = 1
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionEffectiveDate").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(6)
	tmpCol.Caption = "Rate"
	tmpCol.Alignment = 3
    tmpCol.Width = 50
    tmpcol.alignment = 1
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionRate").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(7)
	tmpCol.Caption = "Amount"
	tmpCol.Alignment = 1
    tmpCol.Width = 90
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionAmount").name
	tmpCol.Visible = True

	'*********STATUS********
	set tmpCol =  TrueDBGrid.Columns.Add(8)
	tmpCol.Caption = "Balance"
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionNewBalance").name
	tmpCol.Width = 90
	tmpCol.Alignment = 1
	tmpCol.Visible = True

	'Set the colors_LoanTxns....
	TrueDBGrid.OddRowStyle.BackColor = &HC0FFFF
	TrueDBGrid.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBGrid.HoldFields

	document.body.style.cursor = "default"

End function

Function ConfigureLoanTransactionGrid
 document.body.style.cursor = "hand"

	For I = 0 to TrueDBGrid.Columns.Count - 1
		TrueDBGrid.Columns.Remove(0)
	Next

   set Itm = CreateObject("TrueOleDBGrid60.ValueItem")
   set FormatStyle = CreateObject("TrueOleDBGrid60.Style")

    'Create then necessary columns...
	set tmpCol =  TrueDBGrid.Columns.Add(0)
	tmpCol.Width = 60
	tmpCol.Caption = "Number"
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionNumber").name
	tmpCol.Visible = True
	set tmpCol =  TrueDBGrid.Columns.Add(1)
	tmpCol.Caption = "Type"
	tmpCol.Width =150
	tmpCol.DataField = rs_LoanTxns.Fields("TransactionTypeNumber").name
	tmpCol.Visible = True

	call TDBOLeGridColumnTranslate(TrueDBGrid,1 ,"TransactionType", "TransactionTypeNumber", "TransactionTypeLoanDescription" )

    set tmpCol =  TrueDBGrid.Columns.Add(2)
	tmpCol.Caption = "Reference"
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionReference").name
	tmpCol.Width = 130
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(3)
	tmpCol.Caption = "Changed By"
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionUserID").name
	 tmpCol.Width = 80
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(4)
	tmpCol.Caption = "Insert Date"
	tmpCol.Width = 80
	tmpcol.alignment = 1
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionInsertDate").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(5)
	tmpCol.Caption = "Effective Date"
	tmpCol.Alignment = 3
    tmpCol.Width = 100
    tmpcol.alignment = 1
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionEffectiveDate").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(6)
	tmpCol.Caption = "Rate"
	tmpCol.Alignment = 3
    tmpCol.Width = 50
    tmpcol.alignment = 1
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionRate").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(7)
	tmpCol.Caption = "Amount"
	tmpCol.Alignment = 1
    tmpCol.Width = 90
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionAmount").name
	tmpCol.Visible = True

	'*********STATUS********
	set tmpCol =  TrueDBGrid.Columns.Add(8)
	tmpCol.Caption = "Balance"
	tmpCol.DataField = rs_LoanTxns.Fields("LoanTransactionNewBalance").name
	tmpCol.Width = 90
	tmpCol.Alignment = 1
	tmpCol.Visible = True

    sSQL = "c_GetLoanScreenTxnTypes 0"
	rs_TransactionType.CursorLocation = 3
	rs_TransactionType.Open sSQL ,conn,adOpenStatic
	set DataCombo_TxnType.RowSource = rs_TransactionType
	DataCombo_TxnType.ListField = rs_TransactionType.Fields("TransactionTypeLoanDescription").name
	DataCombo_TxnType.BoundColumn = rs_TransactionType.Fields("TransactionTypeNumber").Name
	DataCombo_TxnType.BoundText = rs_TransactionType.Fields("TransactionTypeNumber").Value
	DataCombo_TxnType.Refresh

	'Set the colors_LoanTxns....
	TrueDBGrid.OddRowStyle.BackColor = &HC0FFFF
	TrueDBGrid.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBGrid.HoldFields

	document.body.style.cursor = "default"

End function

Sub TDBOLeGridColumnTranslate(ByRef TDBGrid_TDBGrid, ByVal i_Column , ByVal s_LookupTable, ByVal s_LookupTableKey, ByVal s_LookupTableColumn )
    'TDBGrid - the grid as a TBDGrid that you want to do the translation in
    'i_Column - the column number that you want to translate
    's_LookupTable - the table name that you want to look up
    's_LookupTableKey - the primary key name to the lookup table
    's_LookupTableColumn - the column name you want to translate to
    dim color
    dim forecolor
    color = &H03CB371& '&H00000000&
    forecolor = &H00FFFFFF&

    set FormatStyle = CreateObject("TrueOleDBGrid60.Style")

    set rs_Lookup  = createobject("ADODB.Recordset")

    set tmpCol =  TDBGrid_TDBGrid.Columns.item(i_Column)

    tmpcol.ValueItems.Translate = true

    sSQL = "select " & s_LookupTableKey & "," & s_LookupTableColumn & " from " & s_LookupTable
    rs_Lookup.CursorLocation = 3
	rs_Lookup.Open sSQL ,conn,adOpenStatic

    Do Until rs_Lookup.EOF

        set Itm = CreateObject("TrueOleDBGrid60.ValueItem")

        Select Case TDBGrid_TDBGrid.Columns.item(i_Column).NumberFormat
            Case "Percent"
                Itm.Value = Format(rs_Lookup.Fields(s_LookupTableKey), "##0.00%")
                Itm.DisplayValue = Format(rs_Lookup.Fields(s_LookupTableColumn), "##0.00%")
            Case Else
                Itm.Value = rs_Lookup.Fields(s_LookupTableKey)
                If IsNull(rs_Lookup.Fields(s_LookupTableColumn)) Then
                    Itm.DisplayValue = "Undefined"
                Else
                     Itm.DisplayValue = rs_Lookup.Fields(s_LookupTableColumn)
                End If
                FormatStyle.BackColor =  color 'vbGreen
                FormatStyle.ForeColor = forecolor
                TDBGrid_TDBGrid.Columns.Item(i_Column).AddRegexCellStyle 0,FormatStyle, rs_Lookup.Fields(1)
                color   = color + 2000

        End Select

		TDBGrid_TDBGrid.Columns.Item(i_Column).ValueItems.Add(Itm)
        rs_Lookup.MoveNext

    Loop
    rs_Lookup.Close

End Sub

Sub TrueDBGrid_RowColChange(LastRow, LastCol)

	if b_AllDataLoaded = false then

	window.LoanNumber.Text = rs_Loan.Fields("LoanNumber").Value
	window.ClientNames.Text = rs_Loan.Fields("ClientNames").Value
	window.LoanOutStandingBal.value = rs_Loan.Fields("LoanCurrentBalance").Value
	window.LoanArrearBal.value = rs_Loan.Fields("LoanArrearBalance").Value
	window.LoanRate.value = rs_Loan.Fields("LoanRate").Value
	window.LoanOutstandingTerm.value = rs_Loan.Fields("LoanRemainingInstallments").Value
	window.LoanInstallment.value = rs_Loan.Fields("LoanInstallmentAmount").Value
	window.LoanOpenDate.value = rs_Loan.Fields("LoanOpenDate").Value

    if rs_LoanTxns.RecordCount > 0 then

	 d_date1 = day(date()) - 1
     window.FromDate.Value = date() - d_date1
	 window.ToDate.Value = date()

	 sSQL = "f_CalcAccruedLoanInterest " & i_CurrentLoanNbr & ",'" & window.FromDate.Text & "','" & window.ToDate.Text & "'"

	 rs_temp.CursorLocation = 3
	 rs_temp.Open sSQL,conn,adOpenDynamic

	 window.AccruedToDate.Value = rs_temp.Fields(0).Value
	 rs_temp.close

     d_date1 = day(date()) - 1
     window.FromDate.Value = date() - d_date1
     window.ToDate.Value= dateadd("d",-(Day(Date()))+1,DateAdd("m",1,Date()))

	 sSQL = "f_CalcAccruedLoanInterest " & i_CurrentLoanNbr & ",'" & window.FromDate.Text & "','" & window.ToDate.Text & "'"

	 rs_temp.CursorLocation = 3
	 rs_temp.Open sSQL,conn,adOpenDynamic

	 window.AccuredToMonthEnd.Value = rs_temp.Fields(0).Value
	 rs_temp.close
	 window.FromDate.style.visibility = "hidden"
     window.ToDate.style.visibility = "hidden"

	 b_AllDataLoaded = true
    end if
  end if

End Sub

Sub SetAccessLightsServer

    sRes1 = "<%=i_LoanTransaction%>"
    if sRes1 = "Allowed" then
       window.pic_FinancialTxn.src = "images/MLSAllowed.bmp"
       window.pic_FinancialTxn.title = "1"
    else
       window.pic_FinancialTxn.src = "images/MLSDenied.bmp"
       window.pic_FinancialTxn.title = "0"
	end if

end Sub

Sub SetAccessLights

    sUserName = "<%= Session("UserID")%>"

	set x = CreateObject("MLSSecurity.FunctionClass")

	sRes1 = x.CheckFunctionAccess("DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>",window.btn_LoanTransaction.title,Trim(sUserName))

    if sRes1 = "Allowed" then
          window.pic_FinancialTxn.src = "images/MLSAllowed.bmp"
          window.pic_FinancialTxn.title = "1"
	end if

end sub

Sub chk_CorrectionType_onclick

if window.chk_CorrectionType.checked = true then

    rs_TransactionType.close
    sSQL = "c_GetLoanScreenTxnTypes 1"
	rs_TransactionType.CursorLocation = 3
	rs_TransactionType.Open sSQL ,conn,adOpenStatic
	set DataCombo_TxnType.RowSource = rs_TransactionType
	DataCombo_TxnType.ListField = rs_TransactionType.Fields("TransactionTypeLoanDescription").name
	DataCombo_TxnType.BoundColumn = rs_TransactionType.Fields("TransactionTypeNumber").name
	DataCombo_TxnType.BoundText = rs_TransactionType.Fields("TransactionTypeNumber").Value
	DataCombo_TxnType.Refresh
else

    rs_TransactionType.close
    sSQL = "c_GetLoanScreenTxnTypes 0"
	rs_TransactionType.CursorLocation = 3
	rs_TransactionType.Open sSQL ,conn,adOpenStatic
	set DataCombo_TxnType.RowSource = rs_TransactionType
	DataCombo_TxnType.ListField = rs_TransactionType.Fields("TransactionTypeLoanDescription").name
	DataCombo_TxnType.BoundColumn = rs_TransactionType.Fields("TransactionTypeNumber").name
	DataCombo_TxnType.BoundText = rs_TransactionType.Fields("TransactionTypeNumber").Value
	DataCombo_TxnType.Refresh

end if

End Sub

Sub radio_AllTransactions_onclick

    window.radio_FinancialTxns.checked = false
    window.radio_MemoTxns.checked = false

    radio_AllTransactions.checked = true
	GetSelectedTypeTransactions

End Sub

Sub GetSelectedTypeTransactions

   rs_LoanTxns.Close

    if radio_AllTransactions.checked = true then

	   sSQL = "c_GetLoanTransactions " & i_CurrentLoanNbr
	elseif radio_FinancialTxns.checked = true then

	    sSQL = "c_GetLoanFinancialTransactions " & i_CurrentLoanNbr
	elseif radio_MemoTxns.checked = true then

	    sSQL = "c_GetLoanMemoTransactions " & i_CurrentLoanNbr
	end if
    rs_LoanTxns.CursorLocation = 3
	rs_LoanTxns.Open sSQL,conn,adOpenDynamic
	rs_LoanTxns.Requery
	if rs_LoanTxns.RecordCount > 0 then
		rs_LoanTxns.Movelast
	end if
end sub

Sub radio_FinancialTxns_onclick

    window.radio_AllTransactions.checked = false
    window.radio_MemoTxns.checked = false

    radio_FinancialTxns.checked = true

	GetSelectedTypeTransactions

End Sub

Sub radio_MemoTxns_onclick

   window.radio_AllTransactions.checked = false
   window.radio_FinancialTxns.checked = false

   radio_MemoTxns.checked = true
   GetSelectedTypeTransactions

End Sub

Sub lbl_FinTxns_onclick
	window.radio_FinancialTxns.click
End Sub

Sub lbl_AllTxns_onclick
	window.radio_AllTransactions.click
End Sub

Sub lbl_MemoTxns_onclick
	window.radio_MemoTxns.click
End Sub

Sub btn_LoanTransaction_onclick

	if window.pic_FinancialTxn.title = "0" then
		window.status = "Access denied to " & window.btn_LoanTransaction.title
		exit sub
	end if

 if ValidateTransaction() = 0 then

	i_Resp = MsgBox("Are you sure you want to insert the transaction : " & window.DataCombo_TxnType.Text & " into Loan Number : " & window.LoanNumber.Text , 4)
    if i_Resp= 7 then
       exit sub
    else

    document.body.style.cursor = "hand"

	' Readvance - delete details type 456
	if Cint(window.DataCombo_TxnType.BoundText) = 140 then	' readvance
		RemoveFurtherLoanReadvanceDetailTypes
	end if

    i_res = 0
    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")
    'Cannot use OLE DB Provider as it appears that it does not return a recordset
    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS [LoanTransactions.asp 2];uid=<%= Session("UserID")%>"

	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc

	sSQL = "f_ProcessLoanFinancialTran"

	com.CommandText = sSQL

	set prm = com.CreateParameter ( "LoanNumber",19,1,,window.LoanNumber.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TramsactionTypeNumber",19,1,,Cint(window.DataCombo_TxnType.BoundText)) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	 s_date = Mid(window.TxnEffectiveDate.Text, 4, 2) & "/" & Mid(window.TxnEffectiveDate.Text, 1, 2) & "/" & Mid(window.TxnEffectiveDate.Text, 7, 4)

	set prm = com.CreateParameter ( "TransactionEffectiveDate",129,1,10,s_date) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TransactionAmount",5,1,,window.TxnAmount.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TransactionReference",200,1,80,window.TxnReference.text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	sUserName = "<%= Session("UserID")%>"
	set prm = com.CreateParameter ( "UserID",200,1,25,sUserName) 'AdVarchar , adParamInput
	com.Parameters.Append prm

    set rs_temp = com.Execute

    document.body.style.cursor = "default"

    Select Case rs_temp.Fields(0).Value
	     Case 1
	             MsgBox("Loan Number is Invalid...!!" )
                 Exit Sub
	     Case 2
	             MsgBox("Transaction Type is Invalid...!!" )
                 Exit Sub
	     Case 3
	             MsgBox("Transaction Effective Date is Invalid...!!" )
                 Exit Sub
	     Case 4
	             MsgBox("Transaction Amount is Invalid...!!" )
                 Exit Sub
	     Case 5
	             MsgBox("Transaction Reference Number is Invalid...!!" )
                 Exit Sub
         Case 6
	             MsgBox("System Integrity Error - Contact IT immediately...!!" )
                 Exit Sub

	End Select

	'GD: Update threshold values for a 140 and 1140
	if Cint(window.DataCombo_TxnType.BoundText) = 140 or Cint(window.DataCombo_TxnType.BoundText) = 1140 then	' readvance or correction
		document.body.style.cursor = "hand"
		UpdateThresholds
		document.body.style.cursor = "default"
	end if

	b_AllDataLoaded = false

	rs_temp.close
	rs_Loan.Requery
	rs_LoanTxns.Requery
	rs_LoanTxns.Movelast

    window.TxnEffectiveDate.Value =date()
    window.TxnAmount.Value = 0.00
	window.TxnReference.Text = ""
    end if

 end if

End Sub

sub UpdateThresholds
    Dim sSQl

	set connection = createobject("ADODB.Connection")
	if Cint(window.DataCombo_TxnType.BoundText) = 1140 then 'Correction, so negative value
	    sSQL = "Exec [2AM].dbo.pLoanUpdateThresholds " & window.LoanNumber.Value & ", 0, " & (0-window.TxnAmount.Value)
	else
	    sSQL = "Exec [2AM].dbo.pLoanUpdateThresholds " & window.LoanNumber.Value & ", 0, " & window.TxnAmount.Value
	end if

	connection.Open sDSN
	connection.Execute sSQL

end sub

Sub RemoveFurtherLoanReadvanceDetailTypes
	Dim sSQl

	set connection = createobject("ADODB.Connection")

	sSQL = "Delete from Detail where LoanNumber = " & window.LoanNumber.Value & " and detailtypenumber = 456"
	connection.Open sDSN
	connection.Execute sSQL

End Sub

'GaryD 14/06/2006 Added check for product election in progress
'   SuperLo added, other product checks can be added here
Function CheckNewProductElection(ByRef sMsg)

    CheckNewProductElection = false 'Assume failure

     sSQL = "Select * FROM EWorksupertracker WITH (NOLOCK) WHERE bactivefolder = -1  and SLoannumber = " & i_CurrentLoanNbr
    dim rs_SuperLoElected
    set rs_SuperLoElected = createobject("ADODB.Recordset")

    rs_SuperLoElected.CursorLocation = 3
	rs_SuperLoElected.Open sSQL,conn,adOpenStatic

	if not rs_SuperLoElected is nothing then
	    if not rs_SuperLoElected.BOF and not rs_SuperLoElected.EOF then
	        sMsg = "An application for Super Lo is currently in process for this loan." & vbcrlf & "The client must complete or cancel the Super Lo request before a re-advance can be requested." & vbcrlf & "Once the re-advance is complete the client can re-apply for Super Lo."
	    end if
	    rs_SuperLoElected.Close
	end if

	CheckNewProductElection = true

End Function

Function ValidateTransaction
ValidateTransaction = -1

if rs_open = true  then
       rs_open = false
	end if

if Cint(window.DataCombo_TxnType.BoundText) =  140 then  'Readvance....

    'GaryD 14/06/2006 Need to check if SuperLo has been elected
    '"Select SLoannumber from [e-work]..SuperTracker with (nolock) where bactivefolder = -1 and SLoannumber = " & i_CurrentLoanNbr
    dim sMsg

    if CheckNewProductElection(sMsg) then
        if len(sMsg) > 0 then
            msgbox sMsg, vbOKOnly, "Re-Advance not allowed."
            exit function
        end if
    end if

    ' Get the tolerance from the Control file...
     sSQL = "SELECT ControlNumeric FROM CONTROL with (nolock) WHERE ControlNumber = 40"

	 rs_controlfile.CursorLocation = 3
	 rs_controlfile.Open sSQL,conn,adOpenStatic

	 d_Tolerance = rs_controlfile.Fields(0).Value
	 d_Tolerance = 1 + d_Tolerance/100
	 rs_controlfile.close

    '24/07/2006 GaryD Further Advance can not be disbursed to Thek1, 2 or 3
    '19/12/2006 GaryD and now Thek 4
    If (CDBL(rs_Loan.Fields("LoanCurrentBalance").Value) + CDBL(window.TxnAmount.Value)) > CDBL(rs_Loan.Fields("LoanAgreementAmount").Value) then
        if CInt(rs_Loan.Fields("SPVNumber").Value) = 15 or CInt(rs_Loan.Fields("SPVNumber").Value) = 18 or CInt(rs_Loan.Fields("SPVNumber").Value) = 19 or CInt(rs_Loan.Fields("SPVNumber").Value) = 20 then
            Msgbox "This loan must change SPV before the advance can be paid.", , "Loan Transaction Validation"
            exit function
        end if
    end if

	If (CDBL(rs_Loan.Fields("LoanCurrentBalance").Value) + CDBL(window.TxnAmount.Value)) > CDBL(rs_Loan.Fields("LoanAgreementAmount").Value) * d_Tolerance then

       d_exceed = Round( CDBL(rs_Loan.Fields("LoanCurrentBalance").Value) + CDBL(window.TxnAmount.Value) - CDBL(rs_Loan.Fields("LoanAgreementAmount").Value) * d_Tolerance , 2)
	   Msgbox "The Loan's Current Balance plus this Transaction Amount would exceed " & vbCRLF & " the Loan Agreement Amount of R " & CStr( CDBL(rs_Loan.Fields("LoanAgreementAmount").Value) * d_Tolerance ) & " by R " & CStr(d_exceed) & vbCRLF & " [Tolerance : " & CSTR((d_Tolerance-1)*100)	& "% ]"  , , "Loan Transaction Validation"
	   window.TxnAmount.Value = 0.00
	   window.TxnAmount.focus
	   exit function
	End if
end if

	' Get the detailtypenumber from the detail...
if Cint(window.DataCombo_TxnType.BoundText) =  140 then
  sSQL = "SELECT DetailTypeNumber from DETAIL with (nolock) WHERE DetailTypeNumber = 100 AND LoanNumber = " & i_CurrentLoanNbr
  rs_LoanDetail1.CursorLocation = 3
  rs_LoanDetail1.Open sSQL,conn,adOpenStatic
  if rs_LoanDetail1.RecordCount > 0 then
	  if Cint(rs_LoanDetail1.Fields("DetailTypeNumber").Value) = 100 then
	   Msgbox "You cannot process this Transaction while a loan detail of Paid up with no HOC exists.!!"
	    exit Function
	   end if
	 end if
  rs_LoanDetail1.Close
end if

if trim(window.DataCombo_TxnType.Text) = "" then
   Msgbox "Transaction Type cannot be empty..!!"
   window.DataCombo_TxnType.focus
   exit Function
end if

if window.TxnEffectiveDate.text = "__/__/____" then
   Msgbox "Transaction Date cannot be empty..!!"
   window.TxnEffectiveDate.focus
   exit Function
end if

if window.TxnEffectiveDate.Value > date() then
   Msgbox "Transaction Effective Date cannot be greater than today..!!"
   window.TxnEffectiveDate.focus
   exit Function
end if

if IsNull(window.TxnAmount.Value)  then
   Msgbox "Transaction Amount cannot be less than or equal to zero..!!"
   window.TxnAmount.Value = 0.00
   window.TxnAmount.focus
   exit Function
end if

if window.TxnAmount.Value <= 0.00 then
   Msgbox "Transaction Amount cannot be less than or equal to zero..!!"
   window.TxnAmount.focus
   exit Function
end if

ValidateTransaction = 0

End Function

Sub lbl_CorrectionType_onclick
chk_CorrectionType.click
End Sub

Sub TxnAmount_Change
 if window.TxnAmount.Value < 0 then
     window.TxnAmount.Value = window.TxnAmount.Value * -1
 end if

End Sub

-->
</script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">

<body bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" class=Generic>

<p ><input id="btn_Exit" name="btn_Exit" style="CURSOR: hand; HEIGHT: 44px; LEFT: 725px; POSITION: absolute; TOP: 506px; VISIBILITY: hidden; WIDTH: 140px; Z-INDEX: 125" title           ="Exit" type="button" value="Exit" class=button2>
<table border="0" cellPadding="1" cellSpacing="1" style="FONT-SIZE: 13px; HEIGHT: 102px; LEFT: 25px; POSITION: absolute; TOP: 1px; WIDTH: 870px; Z-INDEX: 124" width="870" class=Table1>

  <tr>
    <td noWrap >Loan Number</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=21
      id=LoanNumber style="HEIGHT: 21px; LEFT: 1px; TOP: 1px; WIDTH: 99px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2619"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0;;Null"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2012741633"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td>
    <td noWrap >Loan Rate</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=21
      id=LoanRate style="HEIGHT: 21px; LEFT: 1px; TOP: 1px; WIDTH: 83px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2196"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#######0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0.00"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999.99"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
  <tr>
    <td noWrap >Client Name</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=21
      id=ClientNames style="HEIGHT: 21px; WIDTH: 431px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="11404"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td>
    <td noWrap >Outstanding Term</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=21
      id=LoanOutstandingTerm
      style="HEIGHT: 21px; LEFT: 1px; TOP: 1px; WIDTH: 84px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2223"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#######0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
  <tr>
    <td noWrap >Current Balance</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=21
      id=LoanOutStandingBal
      style="HEIGHT: 21px; LEFT: 1px; TOP: 1px; WIDTH: 143px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3784"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="###,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="###########0.00"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999999.99"><PARAM NAME="MinValue" VALUE="-99999999999.99"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td>
    <td noWrap >Installment</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=21
      id=LoanInstallment
style="HEIGHT: 21px; LEFT: 1px; TOP: 1px; WIDTH: 129px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3413"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#######0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0.00"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999.99"><PARAM NAME="MinValue" VALUE="-99999999.99"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
  <tr>
    <td noWrap >Arrear Balance</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=21
      id=LoanArrearBal style="HEIGHT: 21px; LEFT: 1px; TOP: 1px; WIDTH: 143px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3784"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#######0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0.00"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999.99"><PARAM NAME="MinValue" VALUE="-99999999.99"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td>
    <td noWrap >Loan Open Date</td>
    <td noWrap>
      <OBJECT classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830" height=21 id=LoanOpenDate
	style="HEIGHT: 21px; LEFT: 1px; TOP: 1px; WIDTH: 138px">
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3651">
	<PARAM NAME="_ExtentY" VALUE="556">
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
	<PARAM NAME="Enabled" VALUE="0">
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
	<PARAM NAME="MinDate" VALUE="-657434">
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
	<PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</td></tr></table>
<OBJECT classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0
data=data:application/x-oleobject;base64,0DRiDaLb0RG13wBgl2CJ0P7/AAAFAAIA0DRiDaLb0RG13wBgl2CJ0AEAAAAhCI/7ZAEbEITtCAArLscTQAAAAPsnAAA9AAAA0wcAAPABAADUBwAA+AEAAAACAAAAAgAAEAAAAAgCAAAEAgAAEAIAAAgAAAAYAgAAIwAAAMwHAAC0AAAAzA4AAAEAAACQEgAAAgAAAJgSAAAEAAAAoBIAAPj9//+oEgAACP7//7ASAAAHAAAAuBIAAI8AAADAEgAAJQAAAMgSAAAKAAAA0BIAAFAAAADYEgAA/v3//+ASAAAMAAAA6BIAAJEAAADwEgAASgAAAPgSAAAPAAAAABMAAPr9//8IEwAAAQIAABQTAAAvAAAAlCEAADAAAACcIQAAMQAAAKQhAAAyAAAArCEAADMAAAC0IQAAlQAAALwhAACWAAAAxCEAAJcAAADMIQAAsAAAANQhAACyAAAA3CEAALMAAADkIQAAowAAAOwhAACkAAAA9CEAAFwAAAD8IQAAXQAAAAgiAACxAAAAFCIAAGEAAAAgIgAAXwAAACgiAABgAAAAMCIAAH0AAAA4IgAAfgAAAEAiAACYAAAASCIAAJkAAABQIgAAhAAAAFgiAACcAAAAYCIAAJ8AAABsIgAAoAAAAHQiAAC7AAAAfCIAAMIAAACEIgAAvQAAAMAiAAC+AAAAyCIAAL8AAADQIgAAwAAAANgiAADEAAAA4CIAAM4AAADoIgAAAAAAAPAiAAADAAAA61kAAAMAAACrFwAAAgAAAAAAAAADAAAAAQAAgAIAAAAAAAAASxAAAAIAAADSAgAA/v8AAAUAAgDXNGINotvREbXfAGCXYInQAQAAACEIj/tkARsQhO0IACsuxxOUAgAAogIAABIAAAACAgAAmAAAAAQCAACgAAAAGAAAAKgAAAAFAAAArAAAADoAAAC4AAAACAAAAMQAAAAkAAAA0AAAAAkAAADYAAAAEQAAAOQAAAAsAAAA8AAAAC0AAAD8AAAARAAAAAQBAAAvAAAADAEAAEYAAAAYAQAAMQAAACQBAABMAAAALAEAAE4AAAA0AQAAAAAAADwBAAADAAAAAAAAAAIAAAAFAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAACwAAAAAAAAALAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAALAAAAAAAAABIAAAAAAAAACAAAAENvbHVtbjAAMQAAAA4AAABCdXR0b25QaWN0dXJlAAUAAAAIAAAAQ2FwdGlvbgBMAAAAEQAAAENvbnZlcnRFbXB0eUNlbGwACAAAAAoAAABEYXRhRmllbGQAJAAAAAoAAABEYXRhV2lkdGgACQAAAA0AAABEZWZhdWx0VmFsdWUALwAAAAkAAABEcm9wRG93bgAsAAAACQAAAEVkaXRNYXNrAEQAAAAOAAAARWRpdE1hc2tSaWdodAAtAAAADwAAAEVkaXRNYXNrVXBkYXRlAEYAAAAPAAAARXh0ZXJuYWxFZGl0b3IAOgAAAAsAAABGb290ZXJUZXh0AE4AAAAGAAAAR3JvdXAAEQAAAA0AAABOdW1iZXJGb3JtYXQAGAAAAAsAAABWYWx1ZUl0ZW1zAAQCAAAPAAAAX01heENvbWJvSXRlbXMAAgIAAAwAAABfVmxpc3RTdHlsZQDSAgAA/v8AAAUAAgDXNGINotvREbXfAGCXYInQAQAAACEIj/tkARsQhO0IACsuxxNqBQAAogIAABIAAAACAgAAmAAAAAQCAACgAAAAGAAAAKgAAAAFAAAArAAAADoAAAC4AAAACAAAAMQAAAAkAAAA0AAAAAkAAADYAAAAEQAAAOQAAAAsAAAA8AAAAC0AAAD8AAAARAAAAAQBAAAvAAAADAEAAEYAAAAYAQAAMQAAACQBAABMAAAALAEAAE4AAAA0AQAAAAAAADwBAAADAAAAAAAAAAIAAAAFAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAACwAAAAAAAAALAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAALAAAAAAAAABIAAAAAAAAACAAAAENvbHVtbjEAMQAAAA4AAABCdXR0b25QaWN0dXJlAAUAAAAIAAAAQ2FwdGlvbgBMAAAAEQAAAENvbnZlcnRFbXB0eUNlbGwACAAAAAoAAABEYXRhRmllbGQAJAAAAAoAAABEYXRhV2lkdGgACQAAAA0AAABEZWZhdWx0VmFsdWUALwAAAAkAAABEcm9wRG93bgAsAAAACQAAAEVkaXRNYXNrAEQAAAAOAAAARWRpdE1hc2tSaWdodAAtAAAADwAAAEVkaXRNYXNrVXBkYXRlAEYAAAAPAAAARXh0ZXJuYWxFZGl0b3IAOgAAAAsAAABGb290ZXJUZXh0AE4AAAAGAAAAR3JvdXAAEQAAAA0AAABOdW1iZXJGb3JtYXQAGAAAAAsAAABWYWx1ZUl0ZW1zAAQCAAAPAAAAX01heENvbWJvSXRlbXMAAgIAAAwAAABfVmxpc3RTdHlsZQBLEAAAAQAAAPEGAAD+/wAABQACANk0Yg2i29ERtd8AYJdgidABAAAAIQiP+2QBGxCE7QgAKy7HE0gIAADBBgAAGAAAAAYCAADIAAAAIAAAANAAAAA6AAAA2AAAADsAAADgAAAAAQAAAOgAAAADAAAA8AAAAB8AAAD4AAAABAAAAAABAAAFAAAACAEAAAcAAAAQAQAABgAAABgBAAAPAAAAIAEAABAAAAAoAQAABwIAADABAAARAAAAOAEAAAMCAABAAQAAKQAAAIQEAAAqAAAAjAQAACsAAACUBAAALwAAAJwEAAAyAAAApAQAADMAAACsBAAANQAAALgEAAAAAAAAwAQAAAMAAAAAAAAACwAAAP//AAALAAAA//8AAAsAAAAAAAAACwAAAAAAAAACAAAAAQAAAAMAAAADAAAACwAAAAAAAAALAAAA//8AAAMAAAAAAAAAAgAAAAEAAAALAAAA//8AAAsAAAAAAAAACwAAAAAAAAADAAAABAAAAEEAAABAAwAAQmlnUmVkAQICAAAAAQAAABkAAAAEAAAAGQUAANEMAAAAAgAABAAAAAEFAAABAAAAAAAAAAQAAACiBQAAZwwAAAAAAAAEAAAA/wQAAICAgAAAAAAABAAAAO4EAAAAAAAAAFVzZQQAAAAHBQAAAQAAAAAAAAAEAAAAJQQAAAQAAAAAAwAABAAAACsEAAABAAAAAP///wQAAADUBAAAAAAAAAAAAAAEAAAAyAQAAAAAAAAAAwAABAAAAIQEAAAAAAAAAGVsZQQAAACUBQAAAQAAAACsYA4EAAAAIwQAAAEAAAAArmAOBAAAAMgFAAAAAAAAAIhgDgQAAADCBQAAAAAAAAAAAAAEAAAA5gUAAAAAAAAAAPL/BAAAAOoFAAAAAAAAAIdgDgQAAAD5BQAAAQAAAACsYA4EAAAAywUAAAAAAAAArGAOBAAAAJIFAAAAAAAAAFVzZQQAAACyBQAAAAAAAAAAAAAEAAAAvgUAAAAAAAAAq2AOBAAAAPsFAAAAAAAAAKpgDgQAAADzBQAAAQAAAACtYA4EAAAA9QUAAAEAAAAArGAOAgAAABkAAAAEAAAAGQUAANEMAAAAAAAABAAAAAEFAAABAAAAANvREQQAAACiBQAAZwwAAAAAAAAEAAAA/wQAAICAgAAAAAAABAAAAO4EAAAAAAAAAAEAAAQAAAAHBQAAAQAAAAAAAAAEAAAAJQQAAAQAAAAAAAAABAAAACsEAAABAAAAAC7HEwQAAADUBAAAAAAAAACnYA4EAAAAyAQAAAAAAAAAqWAOBAAAAIQEAAAAAAAAAKlgDgQAAACUBQAAAQAAAACoYA4EAAAAIwQAAAIAAAAASBwABAAAAMgFAAAAAAAAAEgcAAQAAADCBQAAAAAAAABIHAAEAAAA5gUAAAAAAAAASBwABAAAAOoFAAAAAAAAAEgcAAQAAAD5BQAAAQAAAABIHAAEAAAAywUAAAAAAAAASBwABAAAAJIFAAAAAAAAAEgcAAQAAACyBQAAAAAAAACoYA4EAAAAvgUAAAAAAAAAp2AOBAAAAPsFAAAAAAAAAKZgDgQAAADzBQAAAQAAAACpYA4EAAAA9QUAAAEAAAAAqGAOCwAAAAAAAAALAAAAAAAAAAsAAAD//wAACwAAAAAAAAALAAAA//8AAB4AAAABAAAAAAAAAAMAAAAAAAAAGAAAAAAAAAAHAAAAU3BsaXQwACoAAAANAAAAQWxsb3dDb2xNb3ZlACkAAAAPAAAAQWxsb3dDb2xTZWxlY3QABQAAAAsAAABBbGxvd0ZvY3VzACsAAAAPAAAAQWxsb3dSb3dTZWxlY3QADwAAAA8AAABBbGxvd1Jvd1NpemluZwAEAAAADAAAAEFsbG93U2l6aW5nADIAAAAUAAAAQWx0ZXJuYXRpbmdSb3dTdHlsZQA7AAAAEgAAAEFuY2hvclJpZ2h0Q29sdW1uADMAAAAIAAAAQ2FwdGlvbgA1AAAADQAAAERpdmlkZXJTdHlsZQAgAAAAEgAAAEV4dGVuZFJpZ2h0Q29sdW1uAC8AAAAOAAAARmV0Y2hSb3dTdHlsZQABAAAABwAAAExvY2tlZAAfAAAADQAAAE1hcnF1ZWVTdHlsZQA6AAAAEwAAAFBhcnRpYWxSaWdodENvbHVtbgAQAAAAEAAAAFJlY29yZFNlbGVjdG9ycwARAAAACwAAAFNjcm9sbEJhcnMAAwAAAAwAAABTY3JvbGxHcm91cAAGAAAABQAAAFNpemUABwAAAAkAAABTaXplTW9kZQADAgAADQAAAF9Db2x1bW5Qcm9wcwAHAgAAFgAAAF9TYXZlZFJlY29yZFNlbGVjdG9ycwAGAgAACwAAAF9Vc2VyRmxhZ3MAAAAASxAAAAEAAAC1AwAA/v8AAAUAAgA2NWINotvREbXfAGCXYInQAQAAACEIj/tkARsQhO0IACsuxxNIDwAAhQMAABQAAAAHAgAAqAAAAAEAAACwAAAAAgAAAMwAAAADAAAA2AAAAAQAAADkAAAABQAAABwBAAAGAAAAVAEAAAcAAABcAQAACAAAAGQBAAAJAAAAbAEAAAoAAAB0AQAACwAAAHwBAAAMAAAAhAEAAA0AAACMAQAADgAAAJQBAAAPAAAAnAEAABAAAACoAQAAEQAAAMABAAAsAAAAyAEAAAAAAADQAQAAAwAAAAAAAAAeAAAAEQAAAERlZmF1bHRQcmludEluZm8AAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAARgAAAC8AAAADUuMLkY/OEZ3jAKoAS7hRAQAAAJAB3HwBABRNaWNyb3NvZnQgU2FucyBTZXJpZgBGAAAALwAAAANS4wuRj84RneMAqgBLuFEBAAAAkAHcfAEAFE1pY3Jvc29mdCBTYW5zIFNlcmlmAAsAAAAAAAAACwAAAP//AAALAAAAAAAAAAsAAAAAAAAACwAAAAAAAAALAAAAAAAAAAsAAAAAAAAAAgAAAAEAAAALAAAAAAAAAB4AAAABAAAAAAAAAB4AAAAOAAAAUGFnZSBccCBvZiBcUAAAAAsAAAAAAAAACwAAAAAAAAAUAAAAAAAAABEAAABEZWZhdWx0UHJpbnRJbmZvAA4AAAAIAAAAQ29sbGF0ZQAHAAAACAAAAERlZmF1bHQABgAAAAYAAABEcmFmdAABAAAABQAAAE5hbWUALAAAAAsAAABOb0NsaXBwaW5nAA0AAAAPAAAATnVtYmVyT2ZDb3BpZXMAAwAAAAsAAABQYWdlRm9vdGVyAAUAAAAPAAAAUGFnZUZvb3RlckZvbnQAAgAAAAsAAABQYWdlSGVhZGVyAAQAAAAPAAAAUGFnZUhlYWRlckZvbnQADwAAAA8AAABQcmV2aWV3Q2FwdGlvbgARAAAAEAAAAFByZXZpZXdNYXhpbWl6ZQAQAAAADgAAAFByZXZpZXdQYWdlT2YACwAAABQAAABSZXBlYXRDb2x1bW5Gb290ZXJzAAoAAAAUAAAAUmVwZWF0Q29sdW1uSGVhZGVycwAIAAAAEQAAAFJlcGVhdEdyaWRIZWFkZXIACQAAABMAAABSZXBlYXRTcGxpdEhlYWRlcnMADAAAABIAAABWYXJpYWJsZVJvd0hlaWdodAAHAgAADAAAAF9TdGF0ZUZsYWdzAAAAAAsAAAAAAAAACwAAAAAAAAALAAAAAAAAAAMAAAABAAAAAwAAAAEAAAALAAAA//8AAAsAAAAAAAAAAwAAAAAAAAAEAAAAAAAAAAsAAAD//wAACwAAAP//AAAEAAAAAACAPwQAAAAAAIA/CwAAAP//AAADAAAAAwAAAB4AAAABAAAAAAAAAEEAAAB8DgAAVVN0eWxlAQUAAAAAJQAAAAAAAAD//////wkA/wAAAAAEAAAABQAAgAgAAICwBAAAVGltZXMgTmV3IFJvbWFuAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAABAAAAAAAAAL8IAP8AAAAABAAAAPeiBgAIAACAzwMAAE1pY3Jvc29mdCBTYW5zIFNlcmlmAAAAAAAAAAAAAAAA//////////8AAAAAAgAAAAEAAADAAAAAAAAAABQAAAAyhssA////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAMAAAABAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAEAAAAAgAAAAAAAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABQAAAAIAAADAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAYAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAHAAAAAQAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAgAAAAAAAAAAEAAAAwMDAAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAIAAAAAAAAAABAAAAP//xgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAAMobLAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAwMDAAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAP//xgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAAMobLAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAADKGywD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwADiAAAAAxAAAAIFVgDuB1YA4hAAAAIAAAAB0AAABIZWFkaW5nAAEAAAAAAAAAAAAAAAAAAAAxAAAA4AAAAB4AAABGb290aW5nACEAAABQAAAAPCFfDjwhXw4QAQAAMQAAAB8AAABTZWxlY3RlZABqIgBfVXNlIwAAAGdzAADgAAAAMQAAACAAAABDYXB0aW9uAAAA9v///xsAAAAAAAAABwAAAAAAMQAAACEAAABIaWdobGlnaHRSb3cA////HQAAAAAAAAAGAgAAAHRvciIAAABFdmVuUm93AHJvcHMAAPL///8fAAAAAAAAAAMCAAAAACMAAABPZGRSb3cADmNvcmRTZWxlY3RvcnMA6v///ycAAAAAACQAAAALAAAA//8AAAMAAAAAAAAACwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAP//AAADAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAA//8AAAsAAAAAAAAAAwAAAAEAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAAeAAAAMgAAAERyYWcgYSBjb2x1bW4gaGVhZGVyIGhlcmUgdG8gZ3JvdXAgYnkgdGhhdCBjb2x1bW4AAAADAAAAAAAAAAMAAAAAAAAAAwAAAMgAAAADAAAAAAAAAAMAAAD3ogYAAwAAAJDQAwA9AAAAAAAAAAsAAABUcnVlREJHcmlkAAIAAAAMAAAAQWxsb3dBZGROZXcALwAAAAwAAABBbGxvd0Fycm93cwABAAAADAAAAEFsbG93RGVsZXRlAAQAAAAMAAAAQWxsb3dVcGRhdGUAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQAI/v//DAAAAEJvcmRlclN0eWxlAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAnAAAAAsAAABEYXRhTWVtYmVyACUAAAAJAAAARGF0YU1vZGUAuwAAAAkAAABEYXRhVmlldwDEAAAAEgAAAERlYWRBcmVhQmFja0NvbG9yAAoAAAAMAAAARGVmQ29sV2lkdGgAUAAAAA0AAABFZGl0RHJvcERvd24AXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkADAAAAAPAAAARXhwb3NlQ2VsbE1vZGUAkQAAAAoAAABGb290TGluZXMAwgAAAA8AAABHcm91cEJ5Q2FwdGlvbgAMAAAACgAAAEhlYWRMaW5lcwCYAAAACwAAAEluc2VydE1vZGUAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAsQAAAAoAAABMYXlvdXRVUkwASgAAAA4AAABNYXJxdWVlVW5pcXVlAM4AAAAIAAAATWF4Um93cwCjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgCEAAAADAAAAE11bHRpU2VsZWN0AGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUAlwAAABEAAABQaWN0dXJlQWRkbmV3Um93AJUAAAASAAAAUGljdHVyZUN1cnJlbnRSb3cAswAAABEAAABQaWN0dXJlRm9vdGVyUm93ALIAAAARAAAAUGljdHVyZUhlYWRlclJvdwCWAAAAEwAAAFBpY3R1cmVNb2RpZmllZFJvdwCwAAAAEwAAAFBpY3R1cmVTdGFuZGFyZFJvdwC0AAAACwAAAFByaW50SW5mb3MADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAIwAAAAcAAABTcGxpdHMAMQAAABAAAABUYWJBY3Jvc3NTcGxpdHMAMgAAAAoAAABUYWJBY3Rpb24AmQAAABcAAABUcmFuc3BhcmVudFJvd1BpY3R1cmVzADMAAAAQAAAAV3JhcENlbGxQb2ludGVyANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA
height=500 id=TrueDBGrid
style="BACKGROUND-IMAGE: url(images/background.gif); HEIGHT: 229px; LEFT: 25px; POSITION: absolute; TOP: 105px; WIDTH: 870px; Z-INDEX: 123"
width=870></OBJECT>

<table border="0" cellPadding="1" cellSpacing="1" style="FONT-SIZE:
13px;  HEIGHT: 153px; LEFT: 25px; POSITION: absolute; TOP: 335px; WIDTH: 870px; Z-INDEX: 122" width="870" id=tbl_TransactionInfo class=Table1>

  <tr>
    <td align="middle" colSpan="2" noWrap class=Header1
              >Loan Financial Transactions&nbsp;</td>
    <td noWrap></td>
    <td noWrap align="middle" colSpan="2" class=Header1
              >Accrued Interest
      &nbsp;&nbsp;</td></tr>
  <tr>
    <td noWrap >Transaction Type</td>
    <td noWrap height=27 style="HEIGHT: 27px">
      <OBJECT classid="clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10" height=26 id=DataCombo_TxnType
	style="HEIGHT: 22px; LEFT: 1px; TOP: 1px; WIDTH: 287px" tabIndex=1 width=306>
	<PARAM NAME="_ExtentX" VALUE="7594">
	<PARAM NAME="_ExtentY" VALUE="741">
	<PARAM NAME="_Version" VALUE="393216">
	<PARAM NAME="IntegralHeight" VALUE="-1">
	<PARAM NAME="Enabled" VALUE="-1">
	<PARAM NAME="Locked" VALUE="0">
	<PARAM NAME="MatchEntry" VALUE="0">
	<PARAM NAME="SmoothScroll" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Style" VALUE="0">
	<PARAM NAME="CachePages" VALUE="3">
	<PARAM NAME="CachePageSize" VALUE="50">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="BackColor" VALUE="-1">
	<PARAM NAME="ForeColor" VALUE="-1">
	<PARAM NAME="ListField" VALUE="">
	<PARAM NAME="BoundColumn" VALUE="">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="RightToLeft" VALUE="0">
	<PARAM NAME="DataMember" VALUE=""></OBJECT>
</td>
    <td noWrap style="CURSOR: hand" id
      ="lbl_CorrectionType"><input id="chk_CorrectionType" name="chk_CorrectionType" title="Correction Transaction Types" type="checkbox" style="CURSOR: hand">&nbsp;Correction Txn Types</td>
    <td noWrap align=right >Current
      To Date</td>
    <td noWrap>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" id=AccruedToDate style="HEIGHT: 22px; LEFT: 1px; TOP: 1px; WIDTH: 120px">
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="AlignHorizontal" VALUE="1">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="0">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="ClearAction" VALUE="0">
	<PARAM NAME="DecimalPoint" VALUE=".">
	<PARAM NAME="DisplayFormat" VALUE="#######0.00">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="######0.00">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxValue" VALUE="99999999.99">
	<PARAM NAME="MinValue" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="NegativeColor" VALUE="255">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="Separator" VALUE=",">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ValueVT" VALUE="2011693061">
	<PARAM NAME="Value" VALUE="0">
	<PARAM NAME="MaxValueVT" VALUE="5">
	<PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
  <tr>
    <td noWrap >Effective Date</td>
    <td noWrap>
      <OBJECT classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830" height=24 id=TxnEffectiveDate
	style="HEIGHT: 22px; LEFT: 1px; TOP: 1px; WIDTH: 138px" tabIndex=2>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3651">
	<PARAM NAME="_ExtentY" VALUE="582">
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
	<PARAM NAME="MinDate" VALUE="-657434">
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
	<PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</td>
    <td noWrap ><input id="btn_LoanTransaction" name="btn_LoanTransaction" style="CURSOR: hand; HEIGHT: 44px; LEFT: 99px; PADDING-TOP: 2px; POSITION: absolute; TOP: 154px; VERTICAL-ALIGN: middle; VISIBILITY: visible; WIDTH:            262px; Z-INDEX: 116" title  ="Financial Transaction" type ="button" value="Loan Transaction" class=button3>
      <OBJECT classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830" id=FromDate style="HEIGHT: 23px; LEFT: 670px; TOP: 583px; VISIBILITY: visible; WIDTH: 98px; Z-INDEX: 99; POSITION: ABSOLUTE;"
	tabIndex=0>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="2593">
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
	<PARAM NAME="DisplayFormat" VALUE="mm/dd/yyyy">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="FirstMonth" VALUE="4">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="mm/dd/yyyy">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="IMEMode" VALUE="3">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxDate" VALUE="2958465">
	<PARAM NAME="MinDate" VALUE="-657434">
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
	<PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
      <OBJECT classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830" id=ToDate style="HEIGHT: 23px; LEFT: 781px; TOP: 582px; VISIBILITY: visible; WIDTH: 93px; Z-INDEX: 98; POSITION: ABSOLUTE;"
	tabIndex=0>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="2461">
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
	<PARAM NAME="DisplayFormat" VALUE="mm/dd/yyyy">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="FirstMonth" VALUE="4">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="mm/dd/yyyy">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="IMEMode" VALUE="3">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxDate" VALUE="2958465">
	<PARAM NAME="MinDate" VALUE="-657434">
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
	<PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</td>
    <td noWrap align="right" >  </td>
    <td noWrap>
</td></tr>
  <tr>
    <td noWrap >Amount</td>
    <td noWrap>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" id=TxnAmount style="HEIGHT: 22px; LEFT: 1px; TOP: 1px; WIDTH: 143px"
	tabIndex=3>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3784">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="AlignHorizontal" VALUE="1">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="0">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="ClearAction" VALUE="0">
	<PARAM NAME="DecimalPoint" VALUE=".">
	<PARAM NAME="DisplayFormat" VALUE="#######0.00">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="######0.00">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxValue" VALUE="99999999.99">
	<PARAM NAME="MinValue" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="NegativeColor" VALUE="255">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="Separator" VALUE=",">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ValueVT" VALUE="2011693061">
	<PARAM NAME="Value" VALUE="0">
	<PARAM NAME="MaxValueVT" VALUE="5">
	<PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td>
    <td noWrap style="CURSOR: hand" id
      ="lbl_AllTxns"><input id="radio_AllTransactions" name="radio_AllTransactions" type="radio" style="CURSOR: hand">&nbsp;All Transactions</td>
    <td noWrap align=right >
      Current To Month End</td>
    <td noWrap>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" id=AccuredToMonthEnd style="HEIGHT: 22px; LEFT: 24px; TOP: 1px; WIDTH: 120px">
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="AlignHorizontal" VALUE="1">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="0">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="ClearAction" VALUE="0">
	<PARAM NAME="DecimalPoint" VALUE=".">
	<PARAM NAME="DisplayFormat" VALUE="#######0.00">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="######0.00">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxValue" VALUE="99999999.99">
	<PARAM NAME="MinValue" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="NegativeColor" VALUE="255">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="Separator" VALUE=",">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ValueVT" VALUE="2011693061">
	<PARAM NAME="Value" VALUE="0">
	<PARAM NAME="MaxValueVT" VALUE="5">
	<PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
  <tr>
    <td noWrap >Reference</td>
    <td noWrap>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id=TxnReference style="HEIGHT: 22px; LEFT: 1px; TOP: 1px; WIDTH: 264px"
	tabIndex=4>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="6985">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="-1">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="80">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td>
    <td noWrap style="CURSOR: hand" id
      ="lbl_FinTxns"><input id="radio_FinancialTxns" name="radio_FinancialTxns" type="radio" style="CURSOR: hand">&nbsp;Financial Transactions</td>
    <td noWrap >    &nbsp;</td>
    <td noWrap>
</td></tr>
  <tr>
    <td noWrap style="HEIGHT: 20px"></td>
    <td noWrap style="HEIGHT: 20px"></td>
    <td noWrap style="CURSOR: hand; HEIGHT:
      20px"
    id="lbl_MemoTxns"><input id="radio_MemoTxns" name="radio_MemoTxns" type="radio" style="CURSOR: hand; HEIGHT: 20px">&nbsp;Memo Transactions</td>
    <td noWrap style="HEIGHT: 20px"></td>
    <td noWrap style="HEIGHT: 20px"></td></tr></table>
<p></p>
<IMG alt ="" border=0 height=23 hspace                            =0 id=pic_FinancialTxn src="images/MLSDenied.bmp" style="CURSOR: hand; HEIGHT: 23px; LEFT: 338px; POSITION: absolute; TOP: 500px; WIDTH: 19px; Z-INDEX: 126" title=0 useMap="" width=19 >
<TABLE bgColor=red border=1 cellPadding=1 cellSpacing=1 id=msg_locked
style="LEFT: 120px; POSITION: absolute; TOP: 498px; VISIBILITY: hidden; Z-INDEX: 121"
width="75%" class=ErrorLine>

  <TR>
    <TD align=middle bgColor=red noWrap
    >THIS
      LOAN IS CLOSED AND LOCKED</TD></TR></TABLE>

</body>
</html>