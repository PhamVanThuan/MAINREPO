<%
     sDatabase =Session("SQLDatabase")
      sUid = Session("UserID")
      Response.Expires = 0
      set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
      i_RefreshQueues = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Refresh Queues",Session("UserName"))
      i_CreateReadvances = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Create Readvances",Session("UserName"))
	  i_CreateDisbursements = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Create Disbursements",Session("UserName"))
	  i_GenerateDisbursements = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Generate Disbursements",Session("UserName"))

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
Dim  b_AllDataLoaded
Dim i_Purpose
dim v_BookMark
Dim b_open
Dim b_Configured

if rs_open <> true then

   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"

	if  sessDSN = "DSN=" or trim("<%= Session("UserID") %>") = "" then
	   window.top.location.href = "Default.asp"
	   window.close
    end if

    x = "=<%= Session("SQLDatabase")%>"

    if x <> "=" then
		set conn = createobject("ADODB.Connection")
		set rs_GridProspects  = createobject("ADODB.Recordset")
		set rs_DetailType = createobject("ADODB.Recordset")
		set rs_temp = createobject("ADODB.Recordset")
		set rs_ProspectConditions = createobject("ADODB.Recordset")

		sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [RegistrationCATS.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
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

	sRes1 = "<%=i_CreateDisbursements%>"
	if sRes1 = "Allowed" then
          window.pic_CreateDisbursements.src = "images/MLSAllowed.bmp"
          window.pic_CreateDisbursements.title = "1"
	end if

	sRes1 = "<%=i_GenerateDisbursements%>"
	if sRes1 = "Allowed" then
          window.pic_GenerateDisbursements.src = "images/MLSAllowed.bmp"
          window.pic_GenerateDisbursements.title = "1"
	end if

end Sub

Sub GetCATSRegistrations()

	   document.body.style.cursor = "wait"

		pnl_Message.bgColor  =&H000000FF&
		pnl_Message.innerHTML= "Please Wait............... Records Loading..!!"
		pnl_Message.style.visibility = "visible"

    i_switch = true

    if rs_open = true  then
       rs_GridProspects.Close
       rs_open = false
	end if

    b_loading = true

     i_RepliesReceived = 0
     i_Lodged = 0
     i_Disbursements = 0
     i_ReAdvances = 0
     i_ReAdvanceReversals  = 0
     i_UpForFees = 0

     if window.chk_RepliesReceived.checked = true	then
		i_RepliesReceived	= 5
	 end if

	 if window.chk_Lodged.checked = true then
		i_Lodged = 9
	 end if

	  if window.chk_UpForFees.checked = true then
		i_UpForFees = 343
	 end if

	 if window.chk_Disbursements.checked =  true then
		i_Disbursements = 6
	 end if

	 if window.chk_ReAdvances.checked =  true then
		i_ReAdvances = 140
		i_ReAdvanceReversals = 1140
	 end if

     sSQL = "r_GetCATSRegistrationsNew " &	 CSTR(i_RepliesReceived) & "," & CSTR(i_Lodged) & "," & CSTR(i_Disbursements) & "," & CSTR(i_ReAdvances) & ","   & CSTR(i_ReAdvanceReversals) & "," & CSTR(i_UpForFees)

    rs_GridProspects.CursorLocation = 3

	rs_GridProspects.Open sSQL,conn,adOpenDynamic
	conn.CommandTimeout = 5000
	TrueDBGrid.DataSource = rs_GridProspects

	rs_open = true
	b_loading = false

	if rs_GridProspects.RecordCount > 0 then
		window.btn_CreateReadvances.disabled = true
		window.btn_CreateDisbursements.disabled = false
		window.btn_GenerateDisbursements.disabled = false
	else
	    window.btn_CreateReadvances.disabled = true
		window.btn_CreateDisbursements.disabled = true
		window.btn_GenerateDisbursements.disabled = true

	end if

  	pnl_Message.bgColor  = &H00FF0000&
	pnl_Message.innerHTML= "Records Loaded.......!!"
	pnl_Message.style.visibility = "visible"

    document.body.style.cursor = "default"

End Sub

Sub ConfigureRegistrationsRecievedGrid
	'Reconfigure the Grid..
    'Remove all columns
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
	tmpCol.Width =260
	tmpCol.DataField = rs_GridProspects.Fields("FirstNames").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(2)
	tmpCol.Caption = "Surname"
	tmpCol.Width = 300
	tmpCol.DataField = rs_GridProspects.Fields("SurName").name
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(3)
	tmpCol.Caption = "Loan Status"
	tmpCol.DataField = rs_GridProspects.Fields("DetailTypeNumber").name
	tmpCol.Visible = True

   	call TDBOLeGridColumnTranslate(TrueDBGrid,3 ,"DetailType", "DetailTypeNumber", "DetailTypeDescription" )

    FormatStyle.ForeColor = &H00FFFFFF&
	FormatStyle.BackColor =  &H001F804F& 'vbGreen

	TrueDBGrid.Columns.Item(3).ValueItems.Translate = true

	TrueDBGrid.Columns.Item(3).AddRegexCellStyle 0,FormatStyle, "^Ready for CATS Disbursement"
	TrueDBGrid.Columns.Item(3).AddRegexCellStyle 0,FormatStyle, "^Reply Received"

	FormatStyle.ForeColor = &H00000000&
	FormatStyle.BackColor =  &H00FFFF9F& 'vbCyan

	TrueDBGrid.Columns.Item(3).AddRegexCellStyle 0,FormatStyle, "^Lodged"

	FormatStyle.ForeColor = &H00000000&
	FormatStyle.BackColor =  &H00F99F9F& '

	TrueDBGrid.Columns.Item(3).AddRegexCellStyle 0,FormatStyle, "^Replies Received"

	FormatStyle.ForeColor = &H00FFFFFF&
	FormatStyle.BackColor =  &H00333F9F& '

	TrueDBGrid.Columns.Item(3).AddRegexCellStyle 0,FormatStyle, "^Other Disbursement"

	FormatStyle.ForeColor = &H00FFFFFF&
	FormatStyle.BackColor =  &H00FF3F9F& '

	TrueDBGrid.Columns.Item(3).AddRegexCellStyle 0,FormatStyle, "^Up for Fees"

	'Set the colors_GridProspects....
	TrueDBGrid.OddRowStyle.BackColor = &HC0FFFF
	TrueDBGrid.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBGrid.HoldFields

    sSQL = "SELECT DetailTypeNumber,DetailTypeDescription FROM DETAILTYPE (nolock) WHERE DetailTypeNumber < 10 OR DetailTypeNumber = 343 UNION SELECT 90,'Other Disbursements' ORDER BY DetailTypeNumber"

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

'	EnableAllControls
    document.body.style.cursor = "default"

    b_open = true

End sub

Sub TDBOLeGridColumnTranslate(ByRef TDBGrid_TDBGrid, ByVal i_Column , ByVal s_LookupTable, ByVal s_LookupTableKey, ByVal s_LookupTableColumn )
    'TDBGrid - the grid as a TBDGrid that you want to do the translation in
    'i_Column - the column number that you want to translate
    's_LookupTable - the table name that you want to look up
    's_LookupTableKey - the primary key name to the lookup table
    's_LookupTableColumn - the column name you want to translate to
    dim color
    dim forecolor
    forecolor = &H00FFFFFF&

    set FormatStyle = CreateObject("TrueOleDBGrid60.Style")

    set rs_Lookup  = createobject("ADODB.Recordset")

    set tmpCol =  TDBGrid_TDBGrid.Columns.item(i_Column)

    tmpcol.ValueItems.Translate = true

    sSQL = "select " & s_LookupTableKey & "," & s_LookupTableColumn & " from " & s_LookupTable & " UNION SELECT 90,'Other Disbursements'"

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
                     if rs_Lookup.Fields(s_LookupTableColumn) = "Registration Received" then
						Itm.DisplayValue = "Ready for CATS Disbursement"
                     else
						Itm.DisplayValue = rs_Lookup.Fields(s_LookupTableColumn)
                     end if
                End If

        End Select

		TDBGrid_TDBGrid.Columns.Item(i_Column).ValueItems.Add(Itm)
        rs_Lookup.MoveNext

    Loop
    rs_Lookup.Close

End Sub

Sub window_onload

		pnl_Message.bgColor  =&H000000FF&
		pnl_Message.innerHTML= "Please Wait..... Records Loading..!!"
		pnl_Message.style.visibility = "visible"

		b_loading = True
		b_AllDataLoaded = false
		i_CurrentLoanNumber = 0

		b_open = false
		b_Configured = false

		window.InstructionDate.DropDown.Visible = 1
		window.InstructionDate.Spin.Visible = 1

		SetAccessLightsServer

		i =  "<%=Request.QueryString("Lodged")%>"

		if  i = "" then
			window.chk_Lodged.checked = false
		elseif  i = 0 then
			window.chk_Lodged.checked = false
		else
			window.chk_Lodged.checked = true
		end if

		i =  "<%=Request.QueryString("UpForFees")%>"

		if  i = "" then
			window.chk_UpForFees.checked = false
		elseif  i = 0 then
			window.chk_UpForFees.checked = false
		else
			window.chk_UpForFees.checked = true
		end if

		i =  "<%=Request.QueryString("RepliesReceived")%>"

		if  i = "" then
			window.chk_RepliesReceived.checked = false
		elseif  i = 0 then
			window.chk_RepliesReceived.checked = false
		else
			window.chk_RepliesReceived.checked = true
		end if

		GetCATSRegistrations

		ConfigureRegistrationsRecievedGrid

		i_CurrentLoan = 0

		v = ""
		v = "<%=Request.QueryString("Number")%>"

		if  v <> "" then
			i_CurrentLoan = v
		end if

		 if i_CurrentLoan <> 0 then
			if rs_GridProspects.RecordCount > 0 then
				rs_GridProspects.MoveFirst
				rs_GridProspects.Find "LoanNumber = " & i_CurrentLoan

			end if
		end if

		window.focus

  		pnl_Message.bgColor  = &H00FF0000&
		pnl_Message.innerHTML= "Records Loaded..!!"
		pnl_Message.style.visibility = "visible"

End Sub

Sub btn_RefreshQueue_onclick
		document.body.style.cursor = "wait"
		pnl_Message.bgColor  = &H00FF0000&
		pnl_Message.innerHTML= "Please Wait..... Records Loading..!!"
		pnl_Message.style.visibility = "visible"

		rs_GridProspects.Requery
		TrueDBGrid.DataSource = rs_GridProspects

		if rs_GridProspects.RecordCount> 0 then
			rs_GridProspects.MoveFirst
			rs_GridProspects.Find "LoanNumber >= " & CStr(i_CurrentLoanNumber)
		if rs_GridProspects.EOF = true then
			rs_GridProspects.MoveLast
		end if

		window.btn_CreateReadvances.disabled = true

		end if

		document.body.style.cursor = "default"
		pnl_Message.bgColor = &H00FF0000&
		pnl_Message.innerHTML= "Records Loaded..!!"
		pnl_Message.style.visibility = "visible"

End Sub

Sub TrueDBGrid_RowColChange(LastRow, LastCol)

if b_open = false then exit sub

if b_loading = false then

	if rs_GridProspects.RecordCount > 0 then
	    i_CurrentLoanNumber =  rs_GridProspects.Fields("LoanNumber").Value
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

b_loading = false

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

Sub FindLoanNumber_KeyDown(KeyCode, Shift)

	if  KeyCode = 13 then
		window.btn_FindLoan.click
	end if
End Sub

Sub window_onfocus

	 if b_AllDataLoaded = true then exit sub
	v = ""
	v = window.parent.frames("RegistrationOptions").CurrentLoanNbr.value
	if  v <> "" then
		i_CurrentLoanNumber = Clng(v)
	end if

	if i_CurrentLoanNumber = "" then i_CurrentLoanNumber = 0

	if i_CurrentLoanNumber > 1235887 then
		window.FindLoanNumber.Value = i_CurrentLoanNumber
		window.btn_FindLoan.click
	end if

	b_AllDataLoaded = true

End Sub

Sub btn_CreateReadvances_onclick
'		document.body.style.cursor = "wait"
'
'
'		pnl_Message.bgColor  = &H00FF0000&
'    pnl_Message.innerHTML= "Please Wait..... Running the CATS READVANCES Extract Procedure Procedure..!!"
'    pnl_Message.style.visibility = "visible"'

'    set com = createobject("ADODB.Command")
'    set prm = createobject("ADODB.Parameter")
'    set rs_temp = createobject("ADODB.Recordset")

'    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=MSAHL-MLSS [RegistrationCATS.asp 2]';uid=<%= Session("UserID")%>"
'
'		com.ActiveConnection = sDSN
'		com.CommandType = 4 'AdCmdStoredPRoc
'		com.CommandTimeout = 0
'
'		sSQL = "f_CreateCATSReadvances"
'
'    com.CommandText = sSQL
'
'		s_date = Mid(date, 4, 2) & "/" & Mid(date, 1, 2) & "/" & Mid(date, 7, 4)
'
'		set prm = com.CreateParameter ( "DisbursementActionDate",200,1,10,s_Date)
'		com.Parameters.Append prm
'
'		com.CommandTimeout = 0    'Ensure that command completes...indefinite timeout
'		set rs_temp = com.Execute
'
'		i_NumInserted =  rs_temp.Fields(0)
'	  i_NumUpdated  = rs_temp.Fields(1)
'	  i_NumDeleted = rs_temp.Fields(2)
'
'		if i_NumInserted = -1 then
'			pnl_Message.bgColor  = &H000000FF&
'			pnl_Message.innerHTML= "Transaction Aborted - error inserting or updateing Disbursement"
'			pnl_Message.style.visibility = "visible"
'
'		elseif i_NumInserted = -1 then
'			pnl_Message.bgColor  = &H000000FF&
'			pnl_Message.innerHTML= "Transaction Aborted - error deleteing from Disbursement"
'			pnl_Message.style.visibility = "visible"
'		else
'			pnl_Message.bgColor  = &H0000C000&
'			pnl_Message.innerHTML= "Number of Disbursement records: Inserted " & CStr(i_NumInserted) & ", Updated " & CStr(i_NumUpdated) & ", Deleted " & CStr(i_NumDeleted)
'			pnl_Message.style.visibility = "visible"
'		end if
'
'		close com
'		close prm
'		close rs_temp
'		set com = nothing
'		set prm = nothing
'		set rs_temp = nothing
'
'
'	  document.body.style.cursor = "default"
'
End Sub

Sub btn_GenerateDisbursements_onclick
   window.status = ""
	if window.pic_GenerateDisbursements.title <> "1" then
	  window.status = "Access denied to " & window.pic_GenerateDisbursements.title
      exit sub
	end if

	i_CurrentLoan = rs_GridProspects.Fields("LoanNumber").value
	i_Purpose = rs_GridProspects.Fields("PurposeNumber").value
	window.TrueDBGrid.Style.visibility = "hidden"

	window.tbl_Instructions.Style.visibility = "hidden"
	window.tbl_buttons.Style.visibility = "hidden"
	window.tbl_Statuses.Style.visibility = "hidden"

	window.btn_FindLoan.Style.visibility = "hidden"
	window.FindLoanNumber.Style.visibility = "hidden"
	window.pnl_Message.Style.visibility = "hidden"
	window.btn_CreateReadvances.Style.visibility = "hidden"
	window.pic_CreateReadvances.Style.visibility = "hidden"
	window.pic_RefreshQueues.Style.visibility = "hidden"

	window.img_Disk2.style.visibility = "hidden"

    window.parent.frames("RegistrationPanel").location.href = "DisbursementsGenerate.asp?Number= " & CStr(i_CurrentLoan) & "&Source=RegistrationCATS.asp&returnpage=RegistrationCATS.asp&purpose=" & CStr(i_Purpose) & "&RepliesReceived=" & Cint(window.chk_RepliesReceived.checked) & "&Lodged=" & Cint(window.chk_Lodged.checked) & "&Disbursements=" & Cint(window.chk_Disbursements.checked) & "&Readvances=" & Cint(window.chk_ReAdvances.checked)& "&UpForFees=" &  Cint(window.chk_UpForFees.checked)

End Sub

Sub btn_CreateDisbursements_onclick
   window.status = ""
	if window.pic_CreateDisbursements.title <> "1" then
	  window.status = "Access denied to " & window.pic_CreateDisbursements.title
      exit sub
	end if

     i_CurrentLoan = rs_GridProspects.Fields("LoanNumber").value
     i_Purpose = rs_GridProspects.Fields("PurposeNumber").value

	 window.parent.frames("RegistrationPanel").location.href = "DisbursementManage.asp?Number= " & CStr(i_CurrentLoan) & "&Source=RegistrationCATS.asp&returnpage=RegistrationCATS.asp&purpose=" & CStr(i_Purpose) & "&RepliesReceived=" & Cint(window.chk_RepliesReceived.checked) & "&Lodged=" & Cint(window.chk_Lodged.checked) & "&Disbursements=" & Cint(window.chk_Disbursements.checked) & "&Readvances=" & Cint(window.chk_ReAdvances.checked) & "&Status=" &  rs_GridProspects.Fields("DetailTypeNumber").Value & "&UpForFees=" &  Cint(window.chk_UpForFees.checked)
End Sub

Sub td_Lodged_onclick
  window.chk_Lodged.checked = not window.chk_Lodged.checked
  GetCATSRegistrations
End Sub

Sub td_UpForFees_onclick
  window.chk_UpForFees.checked = not window.chk_UpForFees.checked
  GetCATSRegistrations
End Sub

Sub td_ReAdvances_onclick
  window.chk_ReAdvances.checked = not window.chk_ReAdvances.checked
  GetCATSRegistrations
End Sub

Sub td_Disbursements_onclick
  window.chk_Disbursements.checked = not window.chk_Disbursements.checked
  GetCATSRegistrations
End Sub

Sub td_RepliesReceived_onclick
  window.chk_RepliesReceived.checked = not window.chk_RepliesReceived.checked
 GetCATSRegistrations
End Sub

Sub chk_Lodged_onclick
	GetCATSRegistrations
End Sub

Sub chk_UpForFees_onclick
	GetCATSRegistrations
End Sub

Sub chk_Disbursements_onclick
	GetCATSRegistrations
End Sub

Sub chk_ReAdvances_onclick
	GetCATSRegistrations

End Sub

Sub chk_RepliesReceived_onclick
	GetCATSRegistrations
End Sub

Sub TrueDBGrid_HeadClick(ColIndex)

    on error resume next
	set tmpCol =  window.TrueDBGrid.Columns.item(ColIndex)
	s =  tmpCol.DataField
    rs_GridProspects.Sort =  s

End Sub

'<OBJECT id=pnl_Message
'style="Z-INDEX: 101; LEFT: 19px; VISIBILITY: visible; WIDTH: 867px; POSITION: absolute; TOP: 433px; HEIGHT: 21px"
'codeBase=../../../../../../../../OCX/Threed32.ocx
'classid=clsid:0BA686B9-F7D3-101A-993E-0000C0EF6F5E VIEWASTEXT><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="22939"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="_StockProps" VALUE="15"><PARAM NAME="Caption" VALUE=""><PARAM NAME="ForeColor" VALUE="16777215"><PARAM NAME="BackColor" VALUE="12632256"><PARAM NAME="BevelWidth" VALUE="1"><PARAM NAME="BorderWidth" VALUE="3"><PARAM NAME="BevelOuter" VALUE="2"><PARAM NAME="BevelInner" VALUE="0"><PARAM NAME="RoundedCorners" VALUE="-1"><PARAM NAME="Outline" VALUE="0"><PARAM NAME="FloodType" VALUE="0"><PARAM NAME="FloodColor" VALUE="16711680"><PARAM NAME="FloodPercent" VALUE="0"><PARAM NAME="FloodShowPct" VALUE="-1"><PARAM NAME="ShadowColor" VALUE="0"><PARAM NAME="Font3D" VALUE="0"><PARAM NAME="Alignment" VALUE="7"><PARAM NAME="Autosize" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"></OBJECT>
' BORDER-BOTTOM: white 1px groove; BORDER-RIGHT: white 1px groove; BORDER-LEFT: white 1px groove; BORDER-TOP: white 1px groove;
-->
</script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">

<body bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" class="Generic">
<table border="1" cellPadding="0" cellSpacing="0" style="Z-INDEX: 101; color:black; font-weight:bold; border-color:black; LEFT: 19px; VISIBILITY: visible; WIDTH: 867px; POSITION: absolute; TOP: 432px; HEIGHT: 21px" >
	<tr><td id=pnl_Message></td></tr>
</table>
<p>
<OBJECT id=TrueDBGrid
style="Z-INDEX: 108; LEFT: 19px; WIDTH: 868px; POSITION: absolute; TOP: 3px; HEIGHT: 309px"
classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0
data=data:application/x-oleobject;base64,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FAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABgAAAAEAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAcAAAABAAAAAAAAAAAAAAAEAAAA96IGAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAIAAAAAQAAAAAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACQAAAAEAAACAAAAAAAAAAAQAAADAwMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAoAAAABAAAAgAAAAAAAAAAEAAAA///GAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAALAAAAAQAAAAAAAAAAAAAABAAAAPeiBgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADAAAAAIAAAAAAAAAAAAAABQAAAAGhMgA////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA0AAAADAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAOAAAABQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADwAAAAcAAAAAAAAAAAAAAAQAAAD3ogYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABAAAAAGAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAARAAAACAAAAAAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEgAAAAkAAAAAAAAAAAAAAAQAAADAwMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABMAAAAKAAAAAAAAAAAAAAAEAAAA///GAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAUAAAABAAAAAAAAAAAAAAAEQAAAABsNgD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFQAAAAwAAAAAAAAAAAAAABQAAAAGhMgA////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABYAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAXAAAADwAAAAAAAAAAAAAABAAAAPeiBgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGAAAAAsAAAAAAAAAAAAAAAQAAAD3ogYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABkAAAAMAAAAAAAAAAAAAAAUAAAABoTIAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAaAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGwAAAA8AAAAAAAAAAAAAAAQAAAD3ogYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABwAAAALAAAAAAAAAAAAAAAEAAAA96IGAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHgAAAB0AAADAAgEAAAIAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB8AAAAdAAAAwAABAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAgAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIQAAAB4AAAAAAQAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACIAAAAdAAAAwAAAAAAAAAAEAAAACAAAgAUAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAjAAAAHQAAAIAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAJAAAAB0AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAAAAAA//////////8BAAAAAAAAAAAAAAABAAAA/v///wQAAAAAAAAAAAAAAAUAAAD9////AgAAAAAAAAAAAAAAAgAAAOr///8DAAAAAAAAAAAAAAADAAAA/P///wUAAAAAAAAAAAAAAP/////7////BgAAAAAAAAAAAAAABAAAAPr///8HAAAAAAAAAAAAAAD/////8f///wgAAAAAAAAAAAAAAAYAAADv////CQAAAAAAAAAAAAAABwAAAO7///8KAAAAAAAAAAAAAAAIAAAA+f///wsAAAABAAAAAAAAAP/////r////FAAAAAEAAAAAAAAA//////j///8MAAAAAQAAAAAAAAD/////6f///w0AAAABAAAAAAAAAP/////3////DgAAAAEAAAAAAAAA//////b///8QAAAAAQAAAAAAAAD/////9f///w8AAAABAAAAAAAAAP/////w////EQAAAAEAAAAAAAAA/////+3///8SAAAAAQAAAAAAAAD/////7P///xMAAAABAAAAAAAAAP/////0////GAAAAAEAAAABAAAA//////P///8VAAAAAQAAAAEAAAD/////6P///xYAAAABAAAAAQAAAP/////y////FwAAAAEAAAABAAAA//////T///8cAAAAAQAAAAIAAAD/////8////xkAAAABAAAAAgAAAP/////o////GgAAAAEAAAACAAAA//////L///8bAAAAAQAAAAIAAAD/////CAAAAE5vcm1hbAAAAAAAAAAAAAAAAAAAAAAAAHAAAAAxAAAAHQAAAEhlYWRpbmcAAAAAAAAAAAAAAAAAAAAAAEAAAAAxAAAAHgAAAEZvb3RpbmcAMQAAAAAEAACMYH0DFCN8AwAAAAAA//8AHwAAAFNlbGVjdGVkAAAAADEAAABPZGRSb3cAAAAAAAAAAAAAIAAAAENhcHRpb24AQAAAADEAAAB4Y3wDEHt9Ax0AAAAkAAAAIQAAAEhpZ2hsaWdodFJvdwAAAAAAAAAAAAAAAAAAAACYY3wDIgAAAEV2ZW5Sb3cABAAAAAAAAAAIAAAAlHh9AwAAAAAAAAAAIwAAAE9kZFJvdwAAAAAAAAAAAAABAAAAAAAAAAAAAAAACAAAJAAAAAsAAAD//wAAAwAAAAAAAAALAAAAAAAAAAMAAAAAAAAACwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAALAAAA//8AAAMAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAD//wAACwAAAAAAAAADAAAAAQAAAB4AAAABAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAB4AAAAyAAAARHJhZyBhIGNvbHVtbiBoZWFkZXIgaGVyZSB0byBncm91cCBieSB0aGF0IGNvbHVtbgAAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAPeiBgADAAAAkNADAD0AAAAAAAAACwAAAFRydWVEQkdyaWQAAgAAAAwAAABBbGxvd0FkZE5ldwAvAAAADAAAAEFsbG93QXJyb3dzAAEAAAAMAAAAQWxsb3dEZWxldGUABAAAAAwAAABBbGxvd1VwZGF0ZQC9AAAADgAAAEFuaW1hdGVXaW5kb3cAwAAAABMAAABBbmltYXRlV2luZG93Q2xvc2UAvgAAABcAAABBbmltYXRlV2luZG93RGlyZWN0aW9uAL8AAAASAAAAQW5pbWF0ZVdpbmRvd1RpbWUA+P3//wsAAABBcHBlYXJhbmNlAAj+//8MAAAAQm9yZGVyU3R5bGUA+v3//wgAAABDYXB0aW9uAGAAAAAJAAAAQ2VsbFRpcHMAfgAAAA4AAABDZWxsVGlwc0RlbGF5AH0AAAAOAAAAQ2VsbFRpcHNXaWR0aACPAAAADgAAAENvbHVtbkZvb3RlcnMABwAAAA4AAABDb2x1bW5IZWFkZXJzAAgAAAAIAAAAQ29sdW1ucwCcAAAACwAAAERhdGFNZW1iZXIAJQAAAAkAAABEYXRhTW9kZQC7AAAACQAAAERhdGFWaWV3AMQAAAASAAAARGVhZEFyZWFCYWNrQ29sb3IACgAAAAwAAABEZWZDb2xXaWR0aABQAAAADQAAAEVkaXREcm9wRG93bgBfAAAACgAAAEVtcHR5Um93cwD+/f//CAAAAEVuYWJsZWQAMAAAAA8AAABFeHBvc2VDZWxsTW9kZQCRAAAACgAAAEZvb3RMaW5lcwDCAAAADwAAAEdyb3VwQnlDYXB0aW9uAAwAAAAKAAAASGVhZExpbmVzAJgAAAALAAAASW5zZXJ0TW9kZQBdAAAADwAAAExheW91dEZpbGVOYW1lAFwAAAALAAAATGF5b3V0TmFtZQCxAAAACgAAAExheW91dFVSTABKAAAADgAAAE1hcnF1ZWVVbmlxdWUAzgAAAAgAAABNYXhSb3dzAKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAIQAAAAMAAAATXVsdGlTZWxlY3QAYQAAAA4AAABNdWx0aXBsZUxpbmVzAJ8AAAAMAAAAT0xFRHJhZ01vZGUAoAAAAAwAAABPTEVEcm9wTW9kZQCXAAAAEQAAAFBpY3R1cmVBZGRuZXdSb3cAlQAAABIAAABQaWN0dXJlQ3VycmVudFJvdwCzAAAAEQAAAFBpY3R1cmVGb290ZXJSb3cAsgAAABEAAABQaWN0dXJlSGVhZGVyUm93AJYAAAATAAAAUGljdHVyZU1vZGlmaWVkUm93ALAAAAATAAAAUGljdHVyZVN0YW5kYXJkUm93ALQAAAALAAAAUHJpbnRJbmZvcwAPAAAAEAAAAFJvd0RpdmlkZXJTdHlsZQAjAAAABwAAAFNwbGl0cwAxAAAAEAAAAFRhYkFjcm9zc1NwbGl0cwAyAAAACgAAAFRhYkFjdGlvbgCZAAAAFwAAAFRyYW5zcGFyZW50Um93UGljdHVyZXMAMwAAABAAAABXcmFwQ2VsbFBvaW50ZXIA0wcAAAkAAABfRXh0ZW50WADUBwAACQAAAF9FeHRlbnRZAAACAAAMAAAAX0xheW91dFR5cGUAEAAAAAsAAABfUm93SGVpZ2h0AAECAAALAAAAX1N0eWxlRGVmcwAEAgAAFgAAAF9XYXNQZXJzaXN0ZWRBc1BpeGVscwA=
width=868 height=286></OBJECT>

<table border="0" cellPadding="1" cellSpacing="1"  style="Z-INDEX: 100; LEFT: 19px; WIDTH: 868px; POSITION: absolute; TOP: 348px; HEIGHT:  20px" width="868" class="Table1" id="tbl_Instructions">

  <tr>
    <td align="right" noWrap>&nbsp;
      Instruction&nbsp;&nbsp;Status</td>
    <td noWrap>
      <p>
      <OBJECT id=DataCombo_Status
      style="LEFT: 1px; WIDTH: 313px; TOP: 1px; HEIGHT: 26px" tabIndex=1
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=313 height=26><PARAM NAME="_ExtentX" VALUE="8281"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="0"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</p>
</td>
    <td align="right" noWrap> </td>
    <td noWrap>
</td></tr>
  <tr>
    <td align="right" style="VERTICAL-ALIGN: sub" vAlign="top">
       Instruction&nbsp;&nbsp;Date</td>
    <td noWrap><PARAM NAME="_Version" VALUE="65536">
      <OBJECT id=InstructionDate
      style="LEFT: 1px; WIDTH: 143px; TOP: 1px; HEIGHT: 25px" tabIndex=2
      classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3784"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="dd/mm/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT><PARAM NAME="_ExtentX"
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
      VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>

</td>
    <td align="right" noWrap> &nbsp; </td>
    <td noWrap><PARAM NAME="_Version" VALUE="65536"><PARAM
      NAME="_ExtentX" VALUE="17382"><PARAM NAME="_ExtentY" VALUE="3572"><PARAM
      NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode"
      VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM
      NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="0"><PARAM
      NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM
      NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM
      NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM
      NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM
      NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical"
      VALUE="0"><PARAM NAME="MultiLine" VALUE="-1"><PARAM NAME="ScrollBars"
      VALUE="0"><PARAM NAME="PasswordChar" VALUE><PARAM NAME="AllowSpace"
      VALUE="-1"><PARAM NAME="Format" VALUE><PARAM NAME="FormatMode"
      VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep"
      VALUE="0"><PARAM NAME="MaxLength" VALUE="765"><PARAM NAME="LengthAsByte"
      VALUE="0"><PARAM NAME="Text" VALUE><PARAM NAME="Furigana" VALUE="0"><PARAM
      NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM
      NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM
      NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode"
      VALUE="1"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode"
      VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>

</td></tr>
<tr>
    <td align="right" noWrap style="VISIBILITY: hidden">

      &nbsp;</td>
    <td noWrap>

</td>
    <td align="right" noWrap> </td>
    <td noWrap>
</td></tr>
</table>
</p>
<input id="btn_FindLoan" name="btn_FindLoan" style="Z-INDEX: 102; LEFT: 613px; WIDTH: 110px; CURSOR: hand; POSITION: absolute; TOP: 352px; HEIGHT: 28px" type="button" value="Find Loan" class="button2">
<IMG id =pic_RefreshQueues title=0 style="Z-INDEX: 105; LEFT: 124px; WIDTH: 19px; POSITION: absolute; TOP: 474px; HEIGHT: 23px" height   =23 alt ="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 >

<table WIDTH="868" BORDER="0" CELLSPACING="1" CELLPADDING="1" style="Z-INDEX: -100; LEFT: 20px; WIDTH: 868px; POSITION: absolute; TOP: 458px; HEIGHT: 73px" class="Table1" id="tbl_buttons">
	<tr>
		<td align="middle"><input id="btn_RefreshQueue" name="btn_RefreshQueue" style="LEFT: 31px; VERTICAL-ALIGN: sub; WIDTH: 210px; CURSOR: hand; PADDING-TOP: 25px; TOP: -84px; HEIGHT:  60px" title="Refresh Queues" type="button" value="Refresh Instruction Queue" height="60" class="button3" width="210">&nbsp;

</td>
	<td align="middle"><input disabled id="btn_CreateDisbursements" name="btn_CreateDisbursements" style="LEFT: 513px; VERTICAL-ALIGN: sub; WIDTH: 210px; CURSOR: hand; PADDING-TOP: 25px; TOP: 476px; HEIGHT: 61px" title="Manage CATS Disbursement Txns" type="button" value="Manage CATS Disbursement Txns" height="60" class="button3" width="210">
			<IMG id =pic_CreateDisbursements title=0 style="Z-INDEX: 107; LEFT: 320px; WIDTH: 19px; POSITION: absolute; TOP: 15px; HEIGHT: 23px" height   =23 alt ="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 >
	</td>
 <td align="middle"><input disabled id="btn_CreateReadvances" name="btn_CreateReadvances" style="LEFT: 162px; VISIBILITY: visible; VERTICAL-ALIGN: sub; WIDTH: 210px; CURSOR: hand; PADDING-TOP: 25px; TOP: 90px;  HEIGHT: 60px" title="Create Readvances" type="button" value="Create Readvances" height="60" class="button3" width="210">
 <IMG id =pic_CreateReadvances title=0 style="Z-INDEX: 108; LEFT: 530px; VISIBILITY: visible; WIDTH: 19px; POSITION: absolute; TOP: 15px; HEIGHT: 23px" height   =23 alt ="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 >

 </td>
    <td align="middle">&nbsp;

 </td>
		<td align="middle"><input id="btn_GenerateDisbursements" name="btn_GenerateDisbursements" style="LEFT: 685px; VERTICAL-ALIGN: sub; WIDTH: 200px; CURSOR: hand; PADDING-TOP: 25px; TOP: 476px; HEIGHT: 61px" title="Generate CATS Disbursements" type="button" value="Generate CATS Transaction file" height="60" class="button3" width="200">
		<IMG id =pic_GenerateDisbursements title=0 style="Z-INDEX: 101; LEFT: 751px; WIDTH: 19px; POSITION: absolute; TOP: 15px; HEIGHT: 23px" height   =23 alt ="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 >
</td>
	</tr>
</table>
<OBJECT id=FindLoanNumber
style="Z-INDEX: 103; LEFT: 744px; WIDTH: 119px; POSITION: absolute; TOP: 352px; HEIGHT: 25px"
classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3149"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="#########0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>

<table class="Table1" style="Z-INDEX: 109; LEFT: 19px; WIDTH: 868px; POSITION: absolute; TOP:
318px; HEIGHT: 28px" cellSpacing="1" cellPadding="1" width="868" border="0" id="tbl_Statuses">

  <tr>
    <td noWrap align="right" id="td_RepliesReceived">Replies Recieved</td>
    <td noWrap><input id="chk_RepliesReceived" type="checkbox" name="checkbox1" align="left"></td>
    <td noWrap align="right" id="td_Lodged">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Lodged</td>
    <td noWrap align="left"><input id="chk_Lodged" type="checkbox" name="checkbox2"></td>
    <td noWrap align="right" id="td_UpForFees" title="Disbursements Prepared or Carried out today">&nbsp;&nbsp;&nbsp; Up For Fees</td>
    <td noWrap align="left"><input id="chk_UpForFees" name="checkbox4" type="checkbox" value="on"></td>
    <td noWrap align="right" id="td_Disbursements">
      &nbsp;
      Disbursements</td>
    <td noWrap align="left"><input CHECKED id="chk_Disbursements" name="checkbox3" title="Disbursements Prepared or Carried out today" type="checkbox"></td>
    <td noWrap align="right" id="td_ReAdvances">
      &nbsp;Other
      Disbursements</td>
    <td noWrap align="left"><input id="chk_ReAdvances" type="checkbox" name="checkbox4" value="on" CHECKED></td>
    </tr></table>

<IMG id =img_Disk2 style="Z-INDEX: 110; LEFT: 626px; VISIBILITY: visible; WIDTH: 37px; POSITION: absolute; TOP: 484px; HEIGHT: 32px" height   =32 alt="" loop=0 src="images/downdisk2.gif" width=37 >

</body>
</html>