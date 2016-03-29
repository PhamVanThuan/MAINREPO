<%
	Response.Expires = 0
  sDatabase =Session("SQLDatabase")
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_RefreshQueues = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Refresh Queues",Session("UserName"))
  i_UpdateInstruction = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Modify Instruction Status",Session("UserName"))
  i_GenerateTxns1 = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Generate Txns1",Session("UserName"))
  i_CreateGuarantees = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Create Guarantees",Session("UserName"))
  i_GenerateGuarantees = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Generate Guarantees",Session("UserName"))

%>

<html>
<head>
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>

<meta name="VI60_DefaultClientScript" Content="VBScript">

<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<script ID="clientEventHandlersVBS" LANGUAGE="vbscript">
<!--
Dim i_switch
Dim gi_LegalEntity
Dim gi_JointOwner
Dim i_EmployeeType
Dim i_SAHLEmployeeNumber
Dim i_EmployeeTeamNumber
Dim i_CurrentLoanNumber
Dim i_CurrentStage
Dim b_loading
Dim i_Purpose
dim v_BookMark
dim  b_AllDataLoaded
Dim b_open
Dim b_Configured
Dim b_RegistrationsRecieved
Dim b_Lodged
Dim b_UpForFees

if rs_open <> true then

   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"

	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   'return
	   window.close
    end if

	 x = "=<%= Session("SQLDatabase")%>"

    if x <> "=" then
		set conn = createobject("ADODB.Connection")
		set rs_GridProspects  = createobject("ADODB.Recordset")
		set rs_DetailType = createobject("ADODB.Recordset")
		set rs_AdditionalIntructons = createobject("ADODB.Recordset")
		set rs_temp = createobject("ADODB.Recordset")
		set rs_ProspectConditions = createobject("ADODB.Recordset")

		sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [Disbursements.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"

		conn.Open sDSN

		rs_open = false
		b_loading = true
	end if

end if

Sub SetAccessLightsServer

    sRes1 = "<%=i_RefreshQueues%>"
    if sRes1 = "Allowed" then
          window.pic_RefreshQueues.src = "images/MLSAllowed.bmp"
          window.pic_RefreshQueues.title = "1"
	end if

	sRes1 = "<%=i_UpdateInstruction%>"
	if sRes1 = "Allowed" then
          window.pic_ModifyInstructionStatus.src = "images/MLSAllowed.bmp"
          window.pic_ModifyInstructionStatus.title = "1"
	end if

	sRes1 = "<%=i_GenerateTxns1%>"
	if sRes1 = "Allowed" then
          window.pic_GenerateTxns.src = "images/MLSAllowed.bmp"
          window.pic_GenerateTxns.title = "1"
	end if

end Sub

Sub GetDisbursements()

   if window.chk_Lodged.checked = true and window.chk_UpForFees.checked = true and window.chk_RepliesReceived.checked  = true then
      i_DetailType = 0
   elseif window.chk_Lodged.checked = true and window.chk_UpForFees.checked = true  then
	  i_DetailType = 1
   elseif  window.chk_UpForFees.checked = true and window.chk_RepliesReceived.checked  = true then
	  i_DetailType = 2
  elseif window.chk_Lodged.checked = true  and window.chk_RepliesReceived.checked  = true then
	  i_DetailType = 3
   elseif window.chk_RepliesReceived.checked = true then
	  i_DetailType =  6
   elseif  window.chk_Lodged.checked  = true then
      i_DetailType = 9
   elseif  window.chk_UpForFees.checked  = true then
      i_DetailType = 343
   else
       i_DetailType = -1
   end if

   document.body.style.cursor = "wait"
    i_switch = true

    if rs_open = true  then
       rs_GridProspects.Close
       rs_open = false
	end if

     sSQL = "r_GetDisbursements "  & i_DetailType

    rs_GridProspects.CursorLocation = 3

	rs_GridProspects.Open sSQL,conn,adOpenDynamic

	TrueDBGrid.DataSource = rs_GridProspects

	rs_open = true

    document.body.style.cursor = "default"

    if rs_GridProspects.RecordCount> 0 then
		window.btn_ModifyInstructionStatus.disabled = false
		window.btn_GenerateTxns.disabled = false
    else
		window.btn_ModifyInstructionStatus.disabled = true
		window.btn_GenerateTxns.disabled = true
	end if

End Sub

Sub ConfigureDisbursementsGrid

    Dim I

    document.body.style.cursor = "hand"

   set Itm = CreateObject("TrueOleDBGrid60.ValueItem")
   set FormatStyle = CreateObject("TrueOleDBGrid60.Style")

	For I = 0 to TrueDBGrid.Columns.Count - 1
		TrueDBGrid.Columns.Remove(0)
	Next

    'Create then necessary columns...
	set tmpCol =  TrueDBGrid.Columns.Add(0)
	tmpCol.Width = 120
	tmpCol.Caption = "Loan Number"
	tmpCol.DataField = rs_GridProspects.Fields("LoanNumber").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(1)
	tmpCol.Caption = "First Names"
	tmpCol.Width =220
	tmpCol.DataField = rs_GridProspects.Fields("FirstNames").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(2)
	tmpCol.Caption = "Surname"
	tmpCol.Width = 220
	tmpCol.DataField = rs_GridProspects.Fields("SurName").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(3)
	tmpCol.Caption = "Loan Purpose"
	tmpCol.Width = 144
	tmpCol.DataField = rs_GridProspects.Fields("PurposeNumber1").name
	tmpCol.Visible = True

	call TDBOLeGridColumnTranslate(TrueDBGrid,3 ,"[2am]..MortgageLoanPurpose", "MortgageLoanPurposeKey", "Description" )

	set tmpCol =  TrueDBGrid.Columns.Add(4)
	tmpCol.Caption = "Loan Status"
	tmpCol.DataField = rs_GridProspects.Fields("DetailTypeNumber").name
	tmpCol.Visible = True

   	call TDBOLeGridColumnTranslate(TrueDBGrid,4 ,"DetailType", "DetailTypeNumber", "DetailTypeDescription" )

    set tmpCol =  TrueDBGrid.Columns.Add(5)
	tmpCol.Caption = "" ' Arrear Balance
	tmpCol.DataField = rs_GridProspects.Fields("LoanArrearBalance").name
	tmpCol.Visible = False

    FormatStyle.ForeColor = &H00000000&
	FormatStyle.BackColor =  &H00FFFF9F& 'vbCyan

	TrueDBGrid.Columns.Item(4).ValueItems.Translate = true

	TrueDBGrid.Columns.Item(4).AddRegexCellStyle 0,FormatStyle, "^Lodged"

	FormatStyle.ForeColor = &H00FFFFFF&
	FormatStyle.BackColor =  &H001F804F& 'vbGreen

	TrueDBGrid.Columns.Item(4).AddRegexCellStyle 0,FormatStyle, "^Registration Received"

	FormatStyle.ForeColor =  &H00000000&
	FormatStyle.BackColor =  &H001FFF4F& '

	TrueDBGrid.Columns.Item(4).AddRegexCellStyle 0,FormatStyle, "^Up for Fees"

	'Set the colors_GridProspects....
	TrueDBGrid.OddRowStyle.BackColor = &HC0FFFF
	TrueDBGrid.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBGrid.HoldFields

    sSQL = "SELECT * FROM DETAILTYPE (nolock) WHERE DetailTypeNumber < 10 OR DetailTypeNumber = 343"
	rs_DetailType.CursorLocation = 3
	rs_DetailType.Open sSQL ,conn,adOpenStatic

	set DataCombo_Status.RowSource = rs_DetailType
	DataCombo_Status.ListField = rs_DetailType.Fields("DetailTypeDescription").name
	DataCombo_Status.BoundColumn = rs_DetailType.Fields("DetailTypeNumber").Name
	if rs_GridProspects.RecordCount >  0 then
	   DataCombo_Status.BoundText = rs_GridProspects.Fields("DetailTypeNumber").Value
	else
	   DataCombo_Status.BoundText = rs_DetailType.Fields("DetailTypeNumber").Value
	end if
	DataCombo_Status.Refresh

    document.body.style.cursor = "default"

End sub

Sub TDBOLeGridColumnTranslate(ByRef TDBGrid_TDBGrid, ByVal i_Column , ByVal s_LookupTable, ByVal s_LookupTableKey, ByVal s_LookupTableColumn )
    'TDBGrid - the grid as a TBDGrid that you want to do the translation in
    'i_Column - the column number that you want to translate
    's_LookupTable - the table name that you want to look up
    's_LookupTableKey - the primary key name to the lookup table
    's_LookupTableColumn - the column name you want to translate to
    dim color
    dim forecolor
    color = &H03CBC71 '&H0000C000&
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
				color   = color + 2800

        End Select

		TDBGrid_TDBGrid.Columns.Item(i_Column).ValueItems.Add(Itm)
        rs_Lookup.MoveNext

    Loop
    rs_Lookup.Close

End Sub

Sub window_onload

 b_loading = True
 b_AllDataLoaded = false
 i_CurrentLoanNumber = 0
 b_open = false
 b_Configured = false

 window.btn_Cancel.style.visibility  = "hidden"
 window.InstructionDate.DropDown.Visible = 1
 window.InstructionDate.Spin.Visible = 1

 SetAccessLightsServer

 b_Lodged= false
 b_UpForFees = false
 b_RegistrationsRecieved = true

		i =  "<%=Request.QueryString("Lodged")%>"

		if  i = "" then
			window.chk_Lodged.checked = false
			 b_Lodged= false
		elseif  i = 0 then
			window.chk_Lodged.checked = false
			 b_Lodged= false
		else
			window.chk_Lodged.checked = true
			 b_Lodged= true
		end if

		i =  "<%=Request.QueryString("UpForFees")%>"

		if  i = "" then
			window.chk_UpForFees.checked = false
			 b_UpForFees= false
		elseif  i = 0 then
			window.chk_UpForFees.checked = false
			 b_UpForFees= false
		else
			window.chk_UpForFees.checked = true
			 b_UpForFees= true
		end if

		i =  "<%=Request.QueryString("RepliesReceived")%>"

		if  i = "" then
			window.chk_RepliesReceived.checked = false
			b_RegistrationsRecieved = false
		elseif  i = 0 then
			window.chk_RepliesReceived.checked = false
			b_RegistrationsRecieved = false
		else
			window.chk_RepliesReceived.checked = true
			b_RegistrationsRecieved = true
		end if

	GetDisbursements

	ConfigureDisbursementsGrid

	window.focus

End Sub

Sub window_onfocus

	if b_AllDataLoaded = true then exit sub

	i_CurrentLoan = 0
	v = ""
	v = "<%=Request.QueryString("Number")%>"

	if  v <> "" then
	i_CurrentLoan = v
	end if

 if rs_GridProspects.RecordCount> 0 then
    b_loading =FALSE
    window.btn_ModifyInstructionStatus.disabled = false
    window.btn_GenerateTxns.disabled = false

 end if
 b_AllDataLoaded = true

 if i_CurrentLoan <> 0 then
	 if rs_GridProspects.RecordCount > 0 then
		rs_GridProspects.MoveFirst
	    rs_GridProspects.Find "LoanNumber = " & i_CurrentLoan
	   end if
	end if

End Sub

Sub btn_RefreshQueue_onclick

i_CurrentLoanNumber = rs_GridProspects.Fields("LoanNumber").Value
rs_GridProspects.Requery
TrueDBGrid.DataSource = rs_GridProspects

if rs_GridProspects.RecordCount> 0 then
	rs_GridProspects.MoveFirst
	rs_GridProspects.Find "LoanNumber >= " & CStr(i_CurrentLoanNumber)
	if rs_GridProspects.EOF = true then
		rs_GridProspects.MoveLast
	end if

    window.btn_ModifyInstructionStatus.disabled = false
 end if

End Sub

Sub TrueDBGrid_RowColChange(LastRow, LastCol)

	if b_loading = false then

	if not rs_GridProspects.State = 0 then
		if rs_GridProspects.RecordCount > 0 then
		    i_CurrentLoanNumber = rs_GridProspects.Fields("LoanNumber").Value
		    window.InstructionDate.Text = rs_GridProspects.Fields("RegMailDateTime").Value
		    rs_DetailType.MoveFirst
		    rs_DetailType.Find "DetailTypeNumber = " & rs_GridProspects.Fields("DetailTypeNumber").Value
		    DataCombo_Status.BoundText = rs_DetailType.Fields("DetailTypeNumber").Value
		    DataCombo_Status.Refresh
		else
		    window.InstructionDate.Text = "__/__/____"
		    rs_DetailType.MoveFirst
		    rs_DetailType.Find "DetailTypeNumber = 6"
		    DataCombo_Status.BoundText = rs_DetailType.Fields("DetailTypeNumber").Value
		    DataCombo_Status.Refresh
		end if
	end if
	end if

	b_loading = false

End Sub

Sub btn_Cancel_onclick
Dim conn
        i_CurrentLoanNumber =  rs_GridProspects.Fields("LoanNumber").Value
        btn_ModifyInstructionStatus.value = "Modify Instruction Status"
        window.btn_Cancel.style.visibility  = "hidden"

		window.btn_RefreshQueue.disabled = false

		window.DataCombo_Status.Enabled = false
		window.InstructionDate.Enabled= false
		window.TrueDBGrid.Enabled = true
        window.btn_Cancel.style.visibility  = "hidden"
        rs_GridProspects.Requery

        if rs_GridProspects.RecordCount> 0 then
            window.btn_FindLoan.disabled = false
			rs_GridProspects.MoveFirst
			rs_GridProspects.Find "LoanNumber >= " & CStr(i_CurrentLoanNumber)
			if rs_GridProspects.EOF = true then
				rs_GridProspects.MoveLast
			end if
		end if

 window.btn_RefreshQueue.style.visibility  = "visible"
 window.pic_RefreshQueues.style.visibility  = "visible"
 window.btn_GenerateTxns.style.visibility  = "visible"
 window.pic_GenerateTxns.style.visibility  = "visible"

End Sub

Sub btn_ModifyInstructionStatus_onclick

Dim conn

 if btn_ModifyInstructionStatus.value = "Modify Instruction Status" then

     i_CurrentLoanNumber =  rs_GridProspects.Fields("LoanNumber").Value
     if window.InstructionDate.DisplayText = "__/__/____" then
				window.InstructionDate.Value = Date()
     end if
			window.btn_RefreshQueue.disabled = true
			window.TrueDBGrid.Enabled = false
			window.btn_FindLoan.disabled = true
			window.btn_RefreshQueue.style.visibility  = "hidden"
			window.pic_RefreshQueues.style.visibility  = "hidden"
			window.btn_GenerateTxns.style.visibility  = "hidden"
			window.pic_GenerateTxns.style.visibility  = "hidden"

			btn_ModifyInstructionStatus.value = "Commit"
			window.btn_Cancel.style.visibility  = "visible"
			window.DataCombo_Status.Enabled = true
			window.InstructionDate.Enabled= true
 else

	set conn = createobject("ADODB.Connection")

	sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [Disbursements.asp 2]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	conn.CursorLocation = 1
	conn.Open sDSN

    s_date = Mid(window.InstructionDate.Text, 4, 2) & "/" & Mid(window.InstructionDate.Text, 1, 2) & "/" & Mid(window.InstructionDate.Text, 7, 4)

 	if s_date = "__/__/____" then
	   conn.Execute "t_UpdInstructionStatus " & rs_GridProspects.Fields("LoanNumber").Value & "," & CInt(window.DataCombo_Status.BoundText) &  "," & NULL & ""
	else
	  conn.Execute "t_UpdInstructionStatus " & rs_GridProspects.Fields("LoanNumber").Value & "," & CInt(window.DataCombo_Status.BoundText) &  ",'" & s_date & "'"
	end if
	conn.close
	rs_GridProspects.Requery

    btn_ModifyInstructionStatus.value = "Modify Instruction Status"
    window.btn_RefreshQueue.disabled = false
    window.TrueDBGrid.Enabled = true
    window.DataCombo_Status.Enabled = false
		window.InstructionDate.Enabled= false
		window.btn_FindLoan.disabled = false
		window.btn_RefreshQueue.style.visibility  = "visible"
		window.pic_RefreshQueues.style.visibility  = "visible"
		window.btn_GenerateTxns.style.visibility  = "visible"
		window.pic_GenerateTxns.style.visibility  = "visible"

    window.btn_Cancel.style.visibility  = "hidden"

    if rs_GridProspects.RecordCount> 0 then
		rs_GridProspects.MoveFirst
		rs_GridProspects.Find "LoanNumber >= " & CStr(i_CurrentLoanNumber)
		if rs_GridProspects.EOF = true then
			rs_GridProspects.MoveLast
		end if
        window.btn_ModifyInstructionStatus.disabled = false
    else
       window.btn_ModifyInstructionStatus.disabled = true
    end if

 end if
End Sub

Sub btn_GenerateTxns_onclick
    window.status = ""
	if window.pic_GenerateTxns.title <> "1" then
	  window.status = "Access denied to " & window.btn_GenerateTxns.title
      exit sub
	end if

    if ChkDebtCounselling() = 1 then
		Msgbox "This Loan is under Debt Counselling and therefore can NOT be disbursed !!!"
		exit sub
	End if

   dLoanArrearBalance = rs_GridProspects.Fields("LoanArrearBalance").value

   i_CurrentLoan = rs_GridProspects.Fields("LoanNumber").value
   i_Purpose = rs_GridProspects.Fields("PurposeNumber").value

   if CDbl(dLoanArrearBalance) > 0 then
   	    bPrompted = true
	    i_Resp = msgbox ("Warning : This Loan : " & i_CurrentLoan & " is in arrears by R" & Cstr(dLoanArrearBalance) & " ! Do you wish to continue with Disbursement ? ",vbYesNo)
		    If i_Resp= 7 then
			    exit sub
		    End if
	end if

	window.parent.frames("RegistrationPanel").location.href = "DisbursementTxns.asp?Number= " & CStr(i_CurrentLoan) & "&Source=Disbursements.asp&purpose=" & CStr(i_Purpose) & "&RepliesReceived=" & Cint(window.chk_RepliesReceived.checked) & "&Lodged=" & Cint(window.chk_Lodged.checked) & "&UpForFees=" &  Cint(window.chk_UpForFees.checked)

End Sub

Sub btn_FindLoan_onclick

  if rs_GridProspects.RecordCount > 0 then
     rs_GridProspects.MoveFirst
     rs_GridProspects.Find "LoanNumber >= " & window.FindLoanNumber.Value
     if rs_GridProspects.EOF = True then
		rs_GridProspects.MoveLast
     end if
  end if

End Sub

Function ChkDebtCounselling ' Chk if Loan Under Debt Counselling

	ChkDebtCounselling = 0

	set rstemp3 = createobject("ADODB.Recordset")

	sSQL = "p_ChkDebtCounselling " & rs_GridProspects.Fields("LoanNumber").value

	rstemp3.CursorLocation = 3
	rstemp3.Open sSQL,conn,adOpenDynamic

	if cint(rstemp3.Fields("RetVal").Value) > 0 then
			ChkDebtCounselling = 1
	End if

End Function

Sub FindLoanNumber_KeyDown(KeyCode, Shift)
if  KeyCode = 13 then
    window.btn_FindLoan.click
end if
End Sub

Sub TrueDBGrid_HeadClick(ColIndex)

    on error resume next
	set tmpCol =  window.TrueDBGrid.Columns.item(ColIndex)
	s =  tmpCol.DataField
    rs_GridProspects.Sort =  s

End Sub

Sub chk_RepliesReceived_onclick

   	  GetDisbursements

End Sub

Sub chk_Lodged_onclick

	GetDisbursements

End Sub

Sub chk_UpForFees_onclick

	GetDisbursements

End Sub

-->
</script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">

<body bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" class="Generic">

<p>
<OBJECT id=TrueDBGrid style="Z-INDEX: 113; LEFT: 19px; WIDTH: 868px; TOP: 4px; HEIGHT: 346px; POSITION: ABSOLUTE;"
	height=346 width=871 data=data:application/x-oleobject;base64,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AAAAAAAAAAD//////////wAAAAAGAAAAAQAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAgAAAABAAAAAAAAAAAAAAAEAAAACAAAgAUAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAJAAAAAQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACgAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAsAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAMAAAAAgAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADQAAAAMAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA4AAAAFAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAPAAAABwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEAAAAAYAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABEAAAAIAAAAAAAAAAAAAAAEAAAACAAAgAUAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAASAAAACQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEwAAAAoAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABQAAAAEAAAAAAAAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAVAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABcAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAYAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABoAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAbAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAeAAAAHQAAAMACAQAAAgAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHwAAAB0AAADAAAEAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACAAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAhAAAAHgAAAAABAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIgAAAB0AAADAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACMAAAAdAAAAgAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAkAAAAHQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAAAAAD//////////wEAAAAAAAAAAAAAAAEAAAD+////BAAAAAAAAAAAAAAABQAAAP3///8CAAAAAAAAAAAAAAACAAAA6v///wMAAAAAAAAAAAAAAAMAAAD8////BQAAAAAAAAAAAAAA//////v///8GAAAAAAAAAAAAAAAEAAAA+v///wcAAAAAAAAAAAAAAP/////x////CAAAAAAAAAAAAAAABgAAAO////8JAAAAAAAAAAAAAAAHAAAA7v///woAAAAAAAAAAAAAAAgAAAD5////CwAAAAEAAAAAAAAA/////+v///8UAAAAAQAAAAAAAAD/////+P///wwAAAABAAAAAAAAAP/////p////DQAAAAEAAAAAAAAA//////f///8OAAAAAQAAAAAAAAD/////9v///xAAAAABAAAAAAAAAP/////1////DwAAAAEAAAAAAAAA//////D///8RAAAAAQAAAAAAAAD/////7f///xIAAAABAAAAAAAAAP/////s////EwAAAAEAAAAAAAAA//////T///8YAAAAAQAAAAEAAAD/////8////xUAAAABAAAAAQAAAP/////o////FgAAAAEAAAABAAAA//////L///8XAAAAAQAAAAEAAAD/////9P///xwAAAABAAAAAgAAAP/////z////GQAAAAEAAAACAAAA/////+j///8aAAAAAQAAAAIAAAD/////8v///xsAAAABAAAAAgAAAP////8IAAAATm9ybWFsAATAJs0EAwAAAAAAAAD//////////woAAAAdAAAASGVhZGluZwAgAGgAZQByAGUAIAB0AG8AIABnAHIAbwAeAAAARm9vdGluZwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAfAAAAU2VsZWN0ZWQAAAAAAAAAAAEAAAAAAAAAAAAAAAUAAAAgAAAAQ2FwdGlvbgDgDc0EcA3NBAANzQQwDM0EsArNBBAKzQQhAAAASGlnaGxpZ2h0Um93ACjNBGAozQTwJ80EgCfNBBAnzQQiAAAARXZlblJvdwAxAAAAsAYAANwgzQQ8LzQEsAXNBEAFzQQjAAAAT2RkUm93AP+wAAAAQQAAAAAAAAAAAAAAAAAAAAAAAAAkAAAACwAAAP//AAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAALAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAAEAAAAAAAAAAMAAADoAwAACwAAAP//AAALAAAAAAAAAAMAAAABAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAHgAAADIAAABEcmFnIGEgY29sdW1uIGhlYWRlciBoZXJlIHRvIGdyb3VwIGJ5IHRoYXQgY29sdW1uAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAA7OnYAAMAAACQ0AMAPQAAAAAAAAALAAAAVHJ1ZURCR3JpZAACAAAADAAAAEFsbG93QWRkTmV3AC8AAAAMAAAAQWxsb3dBcnJvd3MAAQAAAAwAAABBbGxvd0RlbGV0ZQAEAAAADAAAAEFsbG93VXBkYXRlAL0AAAAOAAAAQW5pbWF0ZVdpbmRvdwDAAAAAEwAAAEFuaW1hdGVXaW5kb3dDbG9zZQC+AAAAFwAAAEFuaW1hdGVXaW5kb3dEaXJlY3Rpb24AvwAAABIAAABBbmltYXRlV2luZG93VGltZQD4/f//CwAAAEFwcGVhcmFuY2UACP7//wwAAABCb3JkZXJTdHlsZQD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAJwAAAALAAAARGF0YU1lbWJlcgAlAAAACQAAAERhdGFNb2RlALsAAAAJAAAARGF0YVZpZXcAxAAAABIAAABEZWFkQXJlYUJhY2tDb2xvcgAKAAAADAAAAERlZkNvbFdpZHRoAFAAAAANAAAARWRpdERyb3BEb3duAF8AAAAKAAAARW1wdHlSb3dzAP79//8IAAAARW5hYmxlZAAwAAAADwAAAEV4cG9zZUNlbGxNb2RlAJEAAAAKAAAARm9vdExpbmVzAMIAAAAPAAAAR3JvdXBCeUNhcHRpb24ADAAAAAoAAABIZWFkTGluZXMAmAAAAAsAAABJbnNlcnRNb2RlAF0AAAAPAAAATGF5b3V0RmlsZU5hbWUAXAAAAAsAAABMYXlvdXROYW1lALEAAAAKAAAATGF5b3V0VVJMAEoAAAAOAAAATWFycXVlZVVuaXF1ZQDOAAAACAAAAE1heFJvd3MAowAAAAoAAABNb3VzZUljb24ApAAAAA0AAABNb3VzZVBvaW50ZXIAhAAAAAwAAABNdWx0aVNlbGVjdABhAAAADgAAAE11bHRpcGxlTGluZXMAnwAAAAwAAABPTEVEcmFnTW9kZQCgAAAADAAAAE9MRURyb3BNb2RlAJcAAAARAAAAUGljdHVyZUFkZG5ld1JvdwCVAAAAEgAAAFBpY3R1cmVDdXJyZW50Um93ALMAAAARAAAAUGljdHVyZUZvb3RlclJvdwCyAAAAEQAAAFBpY3R1cmVIZWFkZXJSb3cAlgAAABMAAABQaWN0dXJlTW9kaWZpZWRSb3cAsAAAABMAAABQaWN0dXJlU3RhbmRhcmRSb3cAtAAAAAsAAABQcmludEluZm9zAA8AAAAQAAAAUm93RGl2aWRlclN0eWxlACMAAAAHAAAAU3BsaXRzADEAAAAQAAAAVGFiQWNyb3NzU3BsaXRzADIAAAAKAAAAVGFiQWN0aW9uAJkAAAAXAAAAVHJhbnNwYXJlbnRSb3dQaWN0dXJlcwAzAAAAEAAAAFdyYXBDZWxsUG9pbnRlcgDTBwAACQAAAF9FeHRlbnRYANQHAAAJAAAAX0V4dGVudFkAAAIAAAwAAABfTGF5b3V0VHlwZQAQAAAACwAAAF9Sb3dIZWlnaHQAAQIAAAsAAABfU3R5bGVEZWZzAAQCAAAWAAAAX1dhc1BlcnNpc3RlZEFzUGl4ZWxzAA==
	classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0 VIEWASTEXT></OBJECT>
<input id="btn_RefreshQueue" name="btn_RefreshQueue" style="Z-INDEX: 101; LEFT: 19px; VERTICAL-ALIGN: sub; WIDTH: 169px; CURSOR: hand; PADDING-TOP: 12px; POSITION: absolute; TOP: 475px; HEIGHT:          60px" title="Refresh Queues" type="button" value="Refresh Instruction Queue" height="60" class="button3"><IMG id =pic_RefreshQueues title=0 style="Z-INDEX: 106; LEFT: 100px; WIDTH: 19px; POSITION: absolute; TOP: 480px; HEIGHT: 23px" height   =23 alt ="" hspace=0 src="images/MLSAllowed.bmp" width=19 useMap="" border=0 >&nbsp;
<table border="0" cellPadding="1" cellSpacing="1" style="Z-INDEX: 110; LEFT: 20px; WIDTH: 866px; POSITION: absolute; TOP: 387px; HEIGHT: 40px" width="75%" class="Table1">

  <tr>
    <td align="right" noWrap>&nbsp;
      Instruction&nbsp;&nbsp;Status</td>
    <td noWrap>
      <p>
      <OBJECT id=DataCombo_Status
      style="LEFT: 1px; WIDTH: 313px; TOP: 1px; HEIGHT: 26px" tabIndex=1
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=313 height=26><PARAM NAME="_ExtentX" VALUE="8281"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="0"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</p></td>
    <td align="right" noWrap></td>
    <td noWrap></td></tr>
  <tr>
    <td align="right" style="VERTICAL-ALIGN: sub" vAlign="top">Instruction&nbsp;&nbsp;Dated>
    <td noWrap><PARAM NAME="_Version" VALUE="65536">
      <OBJECT id=InstructionDate style="LEFT: 1px; WIDTH: 143px; TOP: 1px; HEIGHT: 25px"
	tabIndex=2 classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830">
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3784">
	<PARAM NAME="_ExtentY" VALUE="661">
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
<PARAM NAME="_ExtentX"
      VALUE="18943"><PARAM NAME="_ExtentY" VALUE="4075"><PARAM NAME="BackColor"
      VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM
      NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly"
      VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM
      NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM
      NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM
      NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM
      NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM
      NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical"
      VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars"
      VALUE="0"><PARAM NAME="PasswordChar" VALUE><PARAM NAME="AllowSpace"
      VALUE="-1"><PARAM NAME="Format" VALUE><PARAM NAME="FormatMode"
      VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep"
      VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte"
      VALUE="0"><PARAM NAME="Text" VALUE><PARAM NAME="Furigana" VALUE="0"><PARAM
      NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM
      NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM
      NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode"
      VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode"
      VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT></td>
    <td align="right" noWrap>&nbsp; </td>
    <td noWrap><PARAM NAME="_Version"
      VALUE="65536"><PARAM NAME="_ExtentX" VALUE="17382"><PARAM NAME="_ExtentY"
      VALUE="3572"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM
      NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor"
      VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM
      NAME="ShowContextMenu" VALUE="0"><PARAM NAME="MarginLeft" VALUE="1"><PARAM
      NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM
      NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM
      NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM
      NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal"
      VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine"
      VALUE="-1"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar"
      VALUE><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format"
      VALUE><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert"
      VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength"
      VALUE="765"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text"
      VALUE><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText"
      VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus"
      VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight"
      VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="1"><PARAM NAME="MoveOnLRKey"
      VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode"
      VALUE="0"></OBJECT></td></tr>
  <tr>
    <td align="right" noWrap>
    &nbsp;</td>
    <td noWrap>
      <p>
</p></td>
    <td align="right" noWrap></td>
    <td noWrap></td></tr></table>
</p>
<input id="btn_ModifyInstructionStatus" name="btn_ModifyInstructionStatus" style="Z-INDEX: 102; LEFT: 194px; VERTICAL-ALIGN: sub; WIDTH: 169px; CURSOR: hand; PADDING-TOP: 12px; POSITION: absolute; TOP: 475px; HEIGHT:          60px" title="Modify Instruction Status" type="button" value="Modify Instruction Status" height="60" disabled class="button3"><IMG id =pic_GenerateTxns title=0 style="Z-INDEX: 109; LEFT: 797px; WIDTH: 19px; POSITION: absolute; TOP: 480px; HEIGHT: 23px" height   =23 alt ="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 >
<input id="btn_Cancel" name="btn_Cancel" style="Z-INDEX: 105; LEFT: 368px; VISIBILITY: hidden; WIDTH: 169px; CURSOR: hand; POSITION: absolute; TOP: 475px; HEIGHT: 60px" title="Cancel" type="button" value="Cancel" height="60" class="button3">
<input id="btn_GenerateTxns" name="btn_GenerateTxns" style="Z-INDEX: 103; LEFT: 716px; VERTICAL-ALIGN: sub; WIDTH: 169px; CURSOR: hand; PADDING-TOP: 12px; POSITION: absolute; TOP: 475px; HEIGHT:          60px" title="Generate Txns" type="button" value="Generate Transactions" height="60" disabled class="button3"><IMG id =pic_ModifyInstructionStatus title=0 style="Z-INDEX: 107; LEFT: 278px; WIDTH: 19px; POSITION: absolute; TOP: 479px; HEIGHT: 23px" height   =23 alt ="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 > <input id="btn_FindLoan" name="btn_FindLoan" style="Z-INDEX: 111; LEFT: 650px; WIDTH: 110px; CURSOR: hand; POSITION: absolute; TOP: 391px; HEIGHT: 28px" type="button" value="Find Loan" class="button2">
<OBJECT id=FindLoanNumber
style="Z-INDEX: 112; LEFT: 764px; WIDTH: 119px; POSITION: absolute; TOP: 391px; HEIGHT: 25px"
classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3149"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="#########0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>

<table class="Table1" id="tbl_Statuses" style="Z-INDEX: 109; LEFT: 19px; WIDTH: 868px; POSITION: absolute; TOP: 354px; HEIGHT: 28px" cellSpacing="1" cellPadding="1" width="868" border="0">

  <tr>
    <td id="td_RepliesReceived" noWrap align="right">Registration
      Received</td>
    <td noWrap><input id="chk_RepliesReceived" type="checkbox" align="left" name="checkbox1" CHECKED></td>
    <td id="td_UpForFees" noWrap align="right">&nbsp;
      &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Up for Fees</td>
    <td noWrap align="left"><input id="chk_UpForFees" type="checkbox"></td>
    <td id="td_Disbursements" title="Disbursements Prepared or Carried out today" noWrap align="right"></td>
    <td noWrap align="left"></td>
    <td id="td_Lodged" noWrap align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Lodged<input id="chk_Lodged" type="checkbox"></td>
    <td noWrap align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </td></tr></table>

</body>
</html>