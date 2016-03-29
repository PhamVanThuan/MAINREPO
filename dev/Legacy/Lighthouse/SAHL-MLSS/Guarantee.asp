<%
  sDatabase =Session("SQLDatabase") 
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_AddGuarantee = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Add Guarantee",Session("UserName"))
  i_UpdGuarantee = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Update Guarantee",Session("UserName"))
  i_DelGuarantee = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Delete Guarantee",Session("UserName"))



%>
<HTML>
<HEAD>
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>
<META name="VI60_DefaultClientScript" Content="VBScript">

<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<SCRIPT ID=clientEventHandlersVBS LANGUAGE=vbscript>
<!--

Dim v_BookMark
Dim v_BookMark1
Dim b_Grid1Configured
Dim b_Loading
Dim i_NewNbr


if rs_open <> true then

  
   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"
    
	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   window.close
    end if
  
    x = "=<%= Session("SQLDatabase")%>"

    if x <> "=" then
		set conn = createobject("ADODB.Connection")
		set rs_GridLoans  = createobject("ADODB.Recordset")
		set rs_Guarantee  = createobject("ADODB.Recordset")
		set rs_status = createobject("ADODB.Recordset")
		set rs_temp = createobject("ADODB.Recordset")
 
		'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
		sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [Guarantee.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"

		conn.Open sDSN
		rs_open = false
		b_loading = true
	end if
	
end if

Sub window_onload
    b_Loading = true
    b_Grid1Configured = false
    i_NewNbr = -1
    window.GuaranteeIssueDate.DropDown.Visible = 1
    window.GuaranteeIssueDate.Spin.Visible = 1
    window.GuaranteeCancelDate.DropDown.Visible = 1
    window.GuaranteeCancelDate.Spin.Visible = 1
    SetAccessLightsServer
    GetLoans
    ConfigureLoanGrid
    
'	GetGuaranteeDetails
'	ConfigureGuaranteeGrid
	
End Sub


Sub SetAccessLightsServer
     
     
    sRes1 = "<%=i_AddGuarantee%>"
    if sRes1 = "Allowed" then
       window.pic_AddGuarantee.src = "images/MLSAllowed.bmp"
       window.pic_AddGuarantee.title = "1"
    else
       window.pic_AddGuarantee.src = "images/MLSDenied.bmp"
       window.pic_AddGuarantee.title = "0"
	end if 
	
	 sRes1 = "<%=i_UpdGuarantee%>"
    if sRes1 = "Allowed" then
       window.pic_UpdateGuarantee.src = "images/MLSAllowed.bmp"
       window.pic_UpdateGuarantee.title = "1"
    else
       window.pic_UpdateGuarantee.src = "images/MLSDenied.bmp"
       window.pic_UpdateGuarantee.title = "0"
	end if
	
	 sRes1 = "<%=i_DelGuarantee%>"
    if sRes1 = "Allowed" then
       window.pic_DeleteGuarantee.src = "images/MLSAllowed.bmp"
       window.pic_DeleteGuarantee.title = "1"
    else
       window.pic_DeleteGuarantee.src = "images/MLSDenied.bmp"
       window.pic_DeleteGuarantee.title = "0"
	end if
	  
		
end Sub


Sub GetLoans()
   
   document.body.style.cursor = "wait"
 
   
    
    if rs_open1 = true  then
       rs_GridLoans.Close
       rs_open1 = false
	end if

     sSQL = "t_GetGuaranteeLoans" 
     
    rs_GridLoans.CursorLocation =   3

	rs_GridLoans.Open sSQL,conn,1,3,1

	TrueDBGrid.DataSource = rs_GridLoans
	
	rs_open1 = true

    
    document.body.style.cursor = "default"
     
End Sub

Sub ConfigureLoanGrid
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
	tmpCol.Width = 100
	tmpCol.Caption = "Loan Number"
	tmpCol.DataField = rs_GridLoans.Fields("LoanNumber").name
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid.Columns.Add(1)
	tmpCol.Caption = "Client Number"
	tmpCol.Width = 100
	tmpCol.Alignment = 1
	tmpCol.DataField = rs_GridLoans.Fields("ClientNumber").name 
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid.Columns.Add(2)
	tmpCol.Caption = "Client Name"
	tmpCol.Width =340
	tmpCol.DataField = rs_GridLoans.Fields("ClientName").name 
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid.Columns.Add(3)
	tmpCol.Caption = "Guarantees Total"
	tmpCol.Width = 120
	tmpCol.Alignment = 1
	tmpCol.DataField = rs_GridLoans.Fields("GuaranteeTotal").name 
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid.Columns.Add(4)
	tmpCol.Caption = "Status"
	'tmpCol.Width = 300
	tmpCol.DataField = rs_GridLoans.Fields("DetailTypeNumber").name 
	tmpCol.Visible = True
	
	tmpCol.ValueItems.Translate = true

    call TDBOLeGridColumnTranslate(TrueDBGrid,4 ,"DetailType", "DetailTypeNumber", "DetailTypeDescription" )
    
    
     FormatStyle.ForeColor = &H00000000& 	
	FormatStyle.BackColor =  &H00F8A969& 'Blue
	
	TrueDBGrid.Columns.Item(4).ValueItems.Translate = true
	TrueDBGrid.Columns.Item(4).AddRegexCellStyle 0,FormatStyle, "^Scheduled"

    
    FormatStyle.ForeColor = &H00000000& 	
	FormatStyle.BackColor =  &H00BBFFFF& 'Yellow
	
	TrueDBGrid.Columns.Item(4).ValueItems.Translate = true
	TrueDBGrid.Columns.Item(4).AddRegexCellStyle 0,FormatStyle, "^Instruction Sent"
	
	FormatStyle.ForeColor = &H00FFFFFF& 	
	FormatStyle.BackColor =  &H001F804F& 'Green
	
	TrueDBGrid.Columns.Item(4).AddRegexCellStyle 0,FormatStyle, "^Reply Received"  
	TrueDBGrid.Columns.Item(4).AddRegexCellStyle 0,FormatStyle, "^Registration Received"
	
	FormatStyle.ForeColor = &H00000000& 	
	FormatStyle.BackColor =  &H00FFFF9F& 'Cyan

	TrueDBGrid.Columns.Item(4).AddRegexCellStyle 0,FormatStyle, "^Lodged"
    FormatStyle.ForeColor = &H00FFFFFF& 	
	FormatStyle.BackColor = &H004F4AFF& 'Red
	
	TrueDBGrid.Columns.Item(4).AddRegexCellStyle 0,FormatStyle, "^Instruction Not Sent"

	
	'Set the colors_Guarantee....
	TrueDBGrid.OddRowStyle.BackColor = &HC0FFFF
	TrueDBGrid.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBGrid.HoldFields
	
	
    document.body.style.cursor = "default"
    
    


End sub



Sub GetGuaranteeDetails(i_LoanNbr)
   
   document.body.style.cursor = "wait"
 
   
    
    if rs_open = true  then
       rs_Guarantee.Close
       rs_open = false
	end if

     sSQL = "t_GetGuaranteeRecords " & i_LoanNbr
     
    rs_Guarantee.CursorLocation = 3
    
	rs_Guarantee.Open sSQL,conn,adOpenStatic
	
	TrueDBGrid1.DataSource = rs_Guarantee
	
	rs_open = true

    
    if b_Grid1Configured = false then
      ConfigureGuaranteeGrid
    end if
    
    if rs_Guarantee.RecordCount > 0 then
		if CLng(i_NewNbr) > -1 then
		    rs_Guarantee.MoveFirst
		    
			rs_Guarantee.Find "GuaranteeNumber >= " & CStr(i_NewNbr)
			i_NewNbr  = -1 
		end if
    end if
    
    EnableControls
    
    document.body.style.cursor = "default"
     
End Sub


Sub ConfigureGuaranteeGrid
	'Reconfigure the Grid..
    'Remove all columns
    Dim I 
    
    document.body.style.cursor = "hand"
    
   set Itm = CreateObject("TrueOleDBGrid60.ValueItem")
   set FormatStyle = CreateObject("TrueOleDBGrid60.Style")
    
	For I = 0 to TrueDBGrid1.Columns.Count - 1
		TrueDBGrid1.Columns.Remove(0)
	Next
   

    'Create then necessary columns...
	set tmpCol =  TrueDBGrid1.Columns.Add(0)
	tmpCol.Width = 70
	tmpCol.Caption = "Number"
	tmpCol.DataField = rs_Guarantee.Fields("GuaranteeNumber").name
	tmpCol.Visible = True
	
	
	set tmpCol =  TrueDBGrid1.Columns.Add(1)
	tmpCol.Caption = "Limited Amount"
	tmpCol.Width =160
	tmpCol.Alignment = 1
	tmpCol.DataField = rs_Guarantee.Fields("GuaranteeLimitedAmount").name 
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid1.Columns.Add(2)
	tmpCol.Caption = "Issue Date"
	tmpCol.Width = 170
	tmpCol.Alignment = 2
	tmpCol.DataField = rs_Guarantee.Fields("GuaranteeIssueDate").name 
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid1.Columns.Add(3)
	tmpCol.Caption = "Status"
	tmpCol.Width = 300
	tmpCol.DataField = rs_Guarantee.Fields("GuaranteeStatusNumber").name 
	tmpCol.Visible = True
	
	tmpCol.ValueItems.Translate = true

    Itm.Value = 0
    Itm.DisplayValue = "Not Issued"
    tmpCol.ValueItems.Add(Itm)
	Itm.Value = 1
    Itm.DisplayValue = "Issued"
    tmpCol.ValueItems.Add(Itm)
	Itm.Value = 2
    Itm.DisplayValue = "Payed Away on disbursement"
    tmpCol.ValueItems.Add(Itm)
	Itm.Value = 3
    Itm.DisplayValue = "Cancelled for Other Reasons"
    tmpCol.ValueItems.Add(Itm)
    
	
	FormatStyle.BackColor =  &H0000C000& 'vbGreen
	TrueDBGrid1.Columns.Item(3).ValueItems.Translate = true
	TrueDBGrid1.Columns.Item(3).AddRegexCellStyle 0,FormatStyle, "^Payed Away on disbursement"
	FormatStyle.ForeColor =  &HFFFFFF& '
	FormatStyle.BackColor =  &H000000FF& 'vbRed
	TrueDBGrid1.Columns.Item(3).AddRegexCellStyle 0,FormatStyle, "^Not Issued"
	FormatStyle.ForeColor =  &HFFFFFF& 
	FormatStyle.BackColor =  &H00C0C000& 'vbCyan
	TrueDBGrid1.Columns.Item(3).AddRegexCellStyle 0,FormatStyle, "^Issued"
	FormatStyle.ForeColor =  &HFFFFFF& 
    FormatStyle.BackColor =  &H00FF00FF& 'vbMagenta
    TrueDBGrid1.Columns.Item(3).AddRegexCellStyle 0,FormatStyle, "^Cancelled for Other Reasons"
	
	set tmpCol =  TrueDBGrid1.Columns.Add(4)
	tmpCol.Caption = "Cancelled Date"
	'tmpCol.Width = 150
	tmpCol.Alignment = 2
	tmpCol.DataField = rs_Guarantee.Fields("GuaranteeCancelledDate").name 
	tmpCol.Visible = True
	
	
	
	'Set the colors_Guarantee....
	TrueDBGrid1.OddRowStyle.BackColor = &HC0FFFF
	TrueDBGrid1.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBGrid1.HoldFields
	
	'*** Manually populate the Guarantee status
    rs_Status.Fields.Append "GuaranteeStatusNumber",19
    rs_Status.Fields.Append "StatusDetail",200,180
    rs_Status.Open

    rs_Status.AddNew 
    rs_Status.fields("GuaranteeStatusNumber").Value = 0 
    rs_Status.fields("StatusDetail").Value = "Not Issued"
    rs_Status.Update
    rs_Status.AddNew 
    rs_Status.fields("GuaranteeStatusNumber").Value = 1 
    rs_Status.fields("StatusDetail").Value = "Issued"
    rs_Status.Update
    rs_Status.AddNew 
    rs_Status.fields("GuaranteeStatusNumber").Value = 2 
    rs_Status.fields("StatusDetail").Value = "Payed Away on disbursement"
    rs_Status.Update
    rs_Status.AddNew 
    rs_Status.fields("GuaranteeStatusNumber").Value = 3
    rs_Status.fields("StatusDetail").Value = "Cancelled for Other Reasons"
    rs_Status.Update
   
	
	set DataCombo.RowSource = rs_Status
	DataCombo.ListField = rs_Status.Fields("StatusDetail").name
	DataCombo.BoundColumn = rs_Status.Fields("GuaranteeStatusNumber").name
	if rs_Guarantee.RecordCount > 0 then
		DataCombo.BoundText = rs_Guarantee.Fields("GuaranteeStatusNumber").Value
	else
	   	DataCombo.BoundText = rs_Status.Fields("GuaranteeStatusNumber").Value
	end if
	
	DataCombo.Refresh
	

    
	
    document.body.style.cursor = "default"
    
    b_Grid1Configured = true


End sub

sub DisableControls(s_Action)

 if s_Action = "Add" then
   window.btn_UpdateGuarantee.disabled = true
   window.btn_DeleteGuarantee.disabled = true
   window.btn_UpdateGuarantee.style.visibility = "hidden"
   window.btn_DeleteGuarantee.style.visibility = "hidden"
   window.pic_UpdateGuarantee.style.visibility = "hidden"
   window.pic_DeleteGuarantee.style.visibility = "hidden"
   
 elseif s_Action = "Update" then
   window.btn_AddGuarantee.disabled = true
   window.btn_DeleteGuarantee.disabled = true
   window.btn_AddGuarantee.style.visibility = "hidden"
   window.btn_DeleteGuarantee.style.visibility = "hidden"
   window.pic_AddGuarantee.style.visibility = "hidden"
   window.pic_DeleteGuarantee.style.visibility = "hidden"
   
 elseif s_Action = "Delete" then
   window.btn_UpdateGuarantee.disabled = true
   window.btn_AddGuarantee.disabled = true
   window.btn_UpdateGuarantee.style.visibility = "hidden"
   window.btn_AddGuarantee.style.visibility = "hidden"
   window.pic_UpdateGuarantee.style.visibility = "hidden"
   window.pic_AddGuarantee.style.visibility = "hidden"
   
 end if
   window.TrueDBGrid1.Enabled = false
 
   
end sub


sub EnableControls
   
   window.btn_AddGuarantee.disabled = false
   window.btn_UpdateGuarantee.disabled = true
   window.btn_DeleteGuarantee.disabled = true
   
   if rs_Guarantee.RecordCount > 0 then
      window.btn_UpdateGuarantee.disabled = false
      window.btn_DeleteGuarantee.disabled = false
   end if
   window.TrueDBGrid1.Enabled = true
     
   window.btn_AddGuarantee.style.visibility = "visible"  
   window.btn_UpdateGuarantee.style.visibility = "visible"
   window.btn_DeleteGuarantee.style.visibility = "visible"
   window.pic_AddGuarantee.style.visibility = "visible"
   window.pic_UpdateGuarantee.style.visibility = "visible"
   window.pic_DeleteGuarantee.style.visibility = "visible"
     
   if rs_Guarantee.RecordCount <= 0 then
   	
   end if
    
end sub

Sub ClearFields
      
    window.GuaranteeNumber.Text = 0
	window.GuaranteeLimitedAmount.Value = 0.00
	window.GuaranteeIssueDate.Value = Date()
	window.GuaranteeCancelDate.Text = "__/__/____"
	window.DataCombo.BoundText = 1
   
end sub
Sub EnableFields
   
	window.GuaranteeLimitedAmount.Enabled = true
	window.GuaranteeIssueDate.Enabled = true
	window.GuaranteeCancelDate.Enabled = true
	window.DataCombo.Enabled = true

End Sub

Sub DisableFields
   
	window.GuaranteeLimitedAmount.Enabled = false
	window.GuaranteeIssueDate.Enabled = false
	window.GuaranteeCancelDate.Enabled = false
	window.DataCombo.Enabled = false

End Sub

Sub btn_AddGuarantee_onclick
 
  if window.pic_AddGuarantee.title = "0" then
      window.status = "Access denied to " & window.btn_AddGuarantee.title
      exit sub
    end if
   v_BookMark =  TrueDBGrid.Bookmark
   v_BookMark1 = window.TrueDBGrid1.Bookmark

  if btn_AddGuarantee.value = "Add Guarantee" then
     b_AllDataLoaded = true
     
     window.TrueDBGrid.Enabled = false
     window.TrueDBGrid1.Enabled = false

     DisableControls("Add")
     ClearFields
	
	'Enable Fields
	 EnableFields
	 window.btn_Cancel.style.visibility = "visible"
	

	'Set Focus 
    'Set the button caption..
	 btn_AddGuarantee.value = "Commit"  
	 window.GuaranteeLimitedAmount.focus()
  
  elseif btn_AddGuarantee.value = "Commit" then

    if ValidateFields = -1 then
       exit sub
     end if
  	call MantainGuaranteeRecord("Add")
	'Clean up...
     TrueDBGrid.Bookmark = v_BookMark
	 btn_AddGuarantee.value = "Add Guarantee"
	 window.btn_Cancel.style.visibility = "hidden"
	 EnableControls
	 DisableFields
     
	 rs_Guarantee.Requery
	 if rs_Guarantee.RecordCount > 0 then
		'window.TrueDBGrid1.Bookmark =  v_BookMark1
		rs_Guarantee.MoveFirst
		
		rs_Guarantee.Find "GuaranteeNumber = " & CStr(i_NewNbr)
		
	 end if
	 
	 
	 window.TrueDBGrid.Enabled = true
     window.TrueDBGrid1.Enabled = true

   End if
	
End Sub

Sub TrueDBGrid1_RowColChange(LastRow, LastCol)

 if b_Loading = true then exit sub
  
  if window.GuaranteeNumber.Value > 0 then
     window.GuaranteeNumber.style.visibility = "visible"
     window.lbl_GuaranteeNumber.style.visibility = "visible"
     
  else
     window.GuaranteeNumber.style.visibility = "hidden"
     window.lbl_GuaranteeNumber.style.visibility = "hidden"
  end if
  
  
  if rs_Guarantee.RecordCount > 0 then
    
    if not rs_Guarantee.BOF and not rs_Guarantee.EOF then
       window.TrueDBGrid1.SelBookmarks.Add window.TrueDBGrid1.Bookmark
		window.GuaranteeNumber.Value = rs_Guarantee.Fields("GuaranteeNumber").Value
		window.GuaranteeLimitedAmount.Value = rs_Guarantee.Fields("GuaranteeLimitedAmount").Value
		window.GuaranteeIssueDate.value = rs_Guarantee.Fields("GuaranteeIssueDate").Value
		'if rs_Guarantee.Fields("GuaranteeIssueDate").Value = "__/__/____" then
		'   window.GuaranteeIssueDate.Text = rs_Guarantee.Fields("GuaranteeIssueDate").Value
		'else
		'   window.GuaranteeCancelDate.Value = rs_Guarantee.Fields("GuaranteeIssueDate").Value
		'end if
		DataCombo.BoundText = rs_Guarantee.Fields("GuaranteeStatusNumber").Value
		if rs_Guarantee.Fields("GuaranteeCancelledDate").Value = "__/__/____" then
		   window.GuaranteeCancelDate.Text = rs_Guarantee.Fields("GuaranteeCancelledDate").Value
		else
		   window.GuaranteeCancelDate.Value = rs_Guarantee.Fields("GuaranteeCancelledDate").Value
		end if
	end if
  else
	window.GuaranteeNumber.Value = 0
	window.GuaranteeLimitedAmount.Value = 0.00
	window.GuaranteeIssueDate.Text = "__/__/____"
	DataCombo.BoundText = 0
	window.GuaranteeCancelDate.Text = "__/__/____"
  end if
  
End Sub

Sub btn_Cancel_onclick
    
    btn_AddGuarantee.value = "Add Guarantee"
    btn_UpdateGuarantee.value = "Update Guarantee"
	btn_DeleteGuarantee.value = "Delete Guarantee"
	btn_Cancel.style.visibility = "hidden"
	EnableControls
	DisableFields()
	rs_Guarantee.Requery

	if rs_Guarantee.RecordCount > 0 then
		window.TrueDBGrid1.Bookmark =  v_BookMark1
	end if
	window.TrueDBGrid.Enabled = true
    window.TrueDBGrid1.Enabled = true
	
    b_AllDataLoaded = true
End Sub


Function ValidateFields
ValidateFields = -1

if window.GuaranteeLimitedAmount.Value < 1 then 
   msgbox "Guarantee Limited Amount cannot be less than or equal to zero",,"Guarantee Error"
   window.GuaranteeLimitedAmount.focus
   exit function
End if

if window.GuaranteeIssueDate.Text = "__/__/____"  then 
   msgbox "Guarantee Issue Date must be filled in...!!",,"Guarantee Error"
   window.GuaranteeIssueDate.focus
   exit function
End if

if  window.GuaranteeLimitedAmount.Value > 0 and Cint(window.DataCombo.BoundText) = 0 then 
   msgbox "Guarantee Status cannot be 'Not Issued' when Limited Amount is greater than zero..",,"Guarantee Error"
   window.DataCombo.focus
   exit function
End if

if  window.GuaranteeIssueDate.Text <> "__/__/____"  and Cint(window.DataCombo.BoundText) = 0 then 
   msgbox "Guarantee Status cannot be 'Not Issued' when the Issue Date has been captured..",,"Guarantee Error"
   window.DataCombo.focus
   exit function
End if

if  window.GuaranteeCancelDate.Text = "__/__/____"  and Cint(window.DataCombo.BoundText) > 1 then 
   msgbox "A Guarantee Cancel Date is required when its status is 'Payed Away' or 'Canceled'",,"Guarantee Error"
   window.GuaranteeCancelDate.focus
   exit function
End if


ValidateFields = 0 
End function

Function MantainGuaranteeRecord(s_Action)
	
Dim i_res


MantainGuaranteeRecord = -1

    document.body.style.cursor = "hand"
    
  
    i_res = 0     
    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")
    set rs_temp1 = createobject("ADODB.Recordset")
    'Cannot use OLE DB Provider as it appears that it does not return a recordset
    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS [Guarantee.asp 2];uid=<%= Session("UserID")%>"
'    sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [Guarantees.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	
	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc

    if s_Action = "Add" then
		sSQL = "t_AddGuaranteeRecord"  
	elseif s_Action = "Update" then
		sSQL = "t_UpdGuaranteeRecord" 
		
    elseif s_Action = "Delete" then
		sSQL = "t_DelGuaranteeRecord" 
	end if


	com.CommandText = sSQL
	
'	msgbox sSQL
	if s_Action = "Update" or s_Action = "Delete" then 
	    set prm = com.CreateParameter ( "GuaranteeNumber",19,1,,window.GuaranteeNumber.Value) 'AdVarchar , adParamInput
	    com.Parameters.Append prm
	end if
  
    if s_Action = "Add" or s_Action = "Update" then 
	
	    
	    set prm = com.CreateParameter ( "LoanNumber",19,1,,window.LoanNumber.Value) 'AdVarchar , adParamInput
	    com.Parameters.Append prm
	    
	    set prm = com.CreateParameter ( "GuaranteeLimitedAmount",5,1,,window.GuaranteeLimitedAmount.value) 'AdVarchar , adParamInput
	    com.Parameters.Append prm
 
       'if DetermineWorksStationDateFormat = "A" then 'American
	    'All Ok format must be 'mm/dd/yyyyy' - database format
	   '    set prm = com.CreateParameter ("GuaranteeIssueDate",200,1,10,window.GuaranteeIssueDate.text) 'AdVarchar , adParamInput
	'	elseif DetermineWorksStationDateFormat = "S" then 'SA or UK
	'	    s = window.GuaranteeIssueDate.text
	'	    s_date = Mid(s,4,2) & "/" & Mid(s,1,2) & "/" & Mid(s,7,4)
	'	    set prm = com.CreateParameter ( "GuaranteeIssueDate",200,1,10,s_date) 'AdVarchar , adParamInput
	'	else
	'	   	Msgbox  "Your Workstation's Date Format is not currently supported in this System... " & chr(13) & chr(10) & "Changed your Regional Date setting to dd/mm/yyyy or Contact IT"
	'	   	exit function
	'	end if
        s = window.GuaranteeIssueDate.text
		s_date = Mid(s,4,2) & "/" & Mid(s,1,2) & "/" & Mid(s,7,4)
		set prm = com.CreateParameter ( "GuaranteeIssueDate",200,1,10,s_date) 'AdVarchar , adParamInput
	    com.Parameters.Append prm
     
	    set prm = com.CreateParameter ( "GuaranteeStatusNumber",19,1,,Cint(window.DataCombo.BoundText)) 'AdVarchar , adParamInput
	    com.Parameters.Append prm
	    
	    if window.GuaranteeCancelDate.text = "__/__/____" then
	      set prm = com.CreateParameter ("GuaranteeCancelledDate",200,1,10,NULL) 'AdVarchar , adParamInput
	    else  
	    
'	      if DetermineWorksStationDateFormat = "A" then 'American
'			'All Ok format must be 'mm/dd/yyyyy' - database format
'			  set prm = com.CreateParameter ( "GuaranteeCancelDate",200,1,10,window.GuaranteeCancelDate.text) 'AdVarchar , adParamInput
'		  elseif DetermineWorksStationDateFormat = "S" then 'SA or UK
'		     s = window.GuaranteeCancelDate.text
'		     msgbox s
'		     s_date1 = Mid(s,4,2) & "/" & Mid(s,1,2) & "/" & Mid(s,7,4)
'		     set prm = com.CreateParameter ("GuaranteeCancelDate",200,1,10,s_date1) 'AdVarchar , adParamInput
'		  else
'		     Msgbox  "Your Workstation's Date Format is not currently supported in this System... " & chr(13) & chr(10) & "Changed your Regional Date setting to dd/mm/yyyy or Contact IT"
'		     exit function
'		  end if
		  s = window.GuaranteeCancelDate.text
		  s_date1 = Mid(s,4,2) & "/" & Mid(s,1,2) & "/" & Mid(s,7,4)
		  set prm = com.CreateParameter ("GuaranteeCancelDate",200,1,10,s_date1) 'AdVarchar , adParamInput
	
	    end if
	    
	    com.Parameters.Append prm
	
    end if

    set rs_temp = com.Execute 
    
    if s_Action = "Update" or s_Action = "Add" then        
       i_NewNbr = rs_temp.Fields(0).value
    end if
    
    
    com.CommandType = 1
    com.CommandText = "select sum(GuaranteeLimitedAmount) from GUARANTEE (nolock) where LoanNumber = " & window.LoanNumber.Value
    set rs_temp = com.Execute

    rs_GridLoans.Requery
    window.TrueDBGrid.DataSource = rs_GridLoans
    window.TrueDBGrid.ReBind
   
    document.body.style.cursor = "default"
    MantainGuaranteeRecord = 0
   
  
End Function

Sub TDBOLeGridColumnTranslate(ByRef TDBGrid_TDBGrid, ByVal i_Column , ByVal s_LookupTable, ByVal s_LookupTableKey, ByVal s_LookupTableColumn )
    'TDBGrid - the grid as a TBDGrid that you want to do the translation in
    'i_Column - the column number that you want to translate
    's_LookupTable - the table name that you want to look up
    's_LookupTableKey - the primary key name to the lookup table
    's_LookupTableColumn - the column name you want to translate to
    dim color
    dim forecolor
    forecolor = &H00FFFFFF&
    
    ' set Itm = CreateObject("TrueOleDBGrid60.ValueItem")
     set FormatStyle = CreateObject("TrueOleDBGrid60.Style")

    set rs_Lookup  = createobject("ADODB.Recordset")
    
   ' set Itm = CreateObject("TrueOleDBGrid60.ValueItem")
    
    set tmpCol =  TDBGrid_TDBGrid.Columns.item(i_Column)

    tmpcol.ValueItems.Translate = true
    
    sSQL = "select " & s_LookupTableKey & "," & s_LookupTableColumn & " from " & s_LookupTable
   'msgbox sSQL
    rs_Lookup.CursorLocation = 3
	rs_Lookup.Open sSQL ,conn,adOpenStatic
    
    Do Until rs_Lookup.EOF
        
        set Itm = CreateObject("TrueOleDBGrid60.ValueItem")
       
      '  set tmpColFormat =  TDBGrid_TDBGrid.Columns.item(i_Column)
        
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
                'FormatStyle.ForeColor = forecolor
                'TDBGrid_TDBGrid.Columns.Item(i_Column).AddRegexCellStyle 0,FormatStyle, rs_Lookup.Fields(1)
				' tmpCol.AddRegexCellStyle 0,FormatStyle, rs_Lookup.Fields(1)


        End Select

        'set TempCol = TDBGrid_TDBGrid.Columns.item(i_Column)

		TDBGrid_TDBGrid.Columns.Item(i_Column).ValueItems.Add(Itm)
        rs_Lookup.MoveNext
        
    Loop
    rs_Lookup.Close

End Sub

Sub TrueDBGrid_RowColChange(LastRow, LastCol)

if rs_GridLoans.RecordCount > 0 then

   i_LoanNbr = rs_GridLoans.Fields("LoanNumber").Value
   window.LoanNumber.Value = i_LoanNbr
   GetGuaranteeDetails(i_LoanNbr)
  
    b_Loading = false
	
end if

End Sub

Function DetermineWorksStationDateFormat()

  'd = formatdatetime("31/12/2000",2)
  d = window.GuaranteeIssueDate.Text
  i = instr(1,Cstr(d),Cstr(month(d)),1)
  
  if i = 1 then
     DetermineWorksStationDateFormat = "A"
     
  elseif i = 4 then
      DetermineWorksStationDateFormat = "S"
      
  else
      DetermineWorksStationDateFormat = "U"
  end if
  
 End function

Sub btn_UpdateGuarantee_onclick
 if window.pic_UpdateGuarantee.title = "0" then
      window.status = "Access denied to " & window.btn_UpdateGuarantee.title
      exit sub
    end if
   v_BookMark =  TrueDBGrid.Bookmark
   v_BookMark1 = window.TrueDBGrid1.Bookmark

  if btn_UpdateGuarantee.value = "Update Guarantee" then
     b_AllDataLoaded = true
     
     window.TrueDBGrid.Enabled = false
     window.TrueDBGrid1.Enabled = false

     DisableControls("Update")

	
	'Enable Fields
	 EnableFields
	 window.btn_Cancel.style.visibility = "visible"
	

	'Set Focus 
    'Set the button caption..
	 btn_UpdateGuarantee.value = "Commit"  
	 window.GuaranteeLimitedAmount.focus()
  
  elseif btn_UpdateGuarantee.value = "Commit" then

    if ValidateFields = -1 then
       exit sub
     end if
  	call MantainGuaranteeRecord("Update")
	'Clean up...
     TrueDBGrid.Bookmark = v_BookMark
	 btn_UpdateGuarantee.value = "Update Guarantee"
	 window.btn_Cancel.style.visibility = "hidden"
	 EnableControls
	 DisableFields()
     
	 rs_Guarantee.Requery
	 
	 
	 window.TrueDBGrid.Enabled = true
     window.TrueDBGrid1.Enabled = true
     
     TrueDBGrid.Bookmark = v_BookMark 
     window.TrueDBGrid1.Bookmark =   v_BookMark1 

   End if
End Sub

Sub btn_DeleteGuarantee_onclick
 if window.pic_DeleteGuarantee.title = "0" then
      window.status = "Access denied to " & window.btn_DeleteGuarantee.title
      exit sub
    end if
   
 


  if btn_DeleteGuarantee.value = "Delete Guarantee" then
     b_AllDataLoaded = true
     v_BookMark = TrueDBGrid.Bookmark
     rs_Guarantee.MovePrevious
     
     if rs_Guarantee.BOF then
        rs_Guarantee.MoveFirst
        i_NewNbr = rs_Guarantee.Fields(0).value
        
     else
        i_NewNbr = rs_Guarantee.Fields(0).value
         v_BookMark1= rs_Guarantee.Bookmark
         rs_Guarantee.MoveNext
     end if
     
     window.TrueDBGrid.Enabled = false
     window.TrueDBGrid1.Enabled = false

     DisableControls("Delete")

	
	'Enable Fields

	 window.btn_Cancel.style.visibility = "visible"
	

	'Set Focus 
    'Set the button caption..
	 btn_DeleteGuarantee.value = "Commit"  

  
  elseif btn_DeleteGuarantee.value = "Commit" then


  	call MantainGuaranteeRecord("Delete")
	'Clean up...
     
	 btn_DeleteGuarantee.value = "Delete Guarantee"
	 window.btn_Cancel.style.visibility = "hidden"
	 EnableControls
     
	 rs_Guarantee.Requery
	 
	 
	 window.TrueDBGrid.Enabled = true
     window.TrueDBGrid1.Enabled = true
 
     
    
     if rs_Guarantee.RecordCount > 0 then
		window.TrueDBGrid1.Bookmark =   v_BookMark1 
     end if
      TrueDBGrid.Bookmark = v_BookMark

   End if
   
End Sub

Sub TrueDBGrid_HeadClick(ColIndex)

	set tmpCol =  window.TrueDBGrid.Columns.item(ColIndex)
	s =  tmpCol.DataField
	rs_GridLoans.Sort =  s
	
End Sub

Sub btn_FindLoan_onclick
  if rs_GridLoans.RecordCount > 0 then
     rs_GridLoans.MoveFirst
     rs_GridLoans.Find "LoanNumber >= " & window.FindLoanNumber.Value
     if rs_GridLoans.EOF = True then
		rs_GridLoans.MoveLast
     end if
  end if
End Sub

Sub FindLoanNumber_KeyDown(KeyCode, Shift)
if  KeyCode = 13 then
    window.btn_FindLoan.click
end if
End Sub


-->
</SCRIPT>
</HEAD>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">

<BODY bottomMargin=0 leftMargin=0 rightMargin=0 topMargin=0 class=Generic>
<TABLE border=0 cellPadding=1 cellSpacing=1 
style="HEIGHT: 196px; LEFT: 19px; POSITION: absolute; TOP: 329px; WIDTH: 688px; Z-INDEX: 100" width     
="100%" class=Table1>
  
  <TR>
    <TD align=right id=lbl_GuaranteeNumber>Guarantee Number</TD>
    <TD>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=25 
      id=GuaranteeNumber style="HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 95px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2514"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="######0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR>
  <TR>
    <TD align=right>Loan Number</TD>
    <TD>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=25 
      id=LoanNumber style="HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 124px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3281"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="######0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR>
  <TR>
    <TD align=right>Limited Amount</TD>
    <TD>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 
      id=GuaranteeLimitedAmount 
      style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 155px" tabIndex=1><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="4101"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="##,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="#######0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999.99"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="0"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR>
  <TR>
    <TD align=right>Issue Date</TD>
    <TD>
      <OBJECT classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830 
      id=GuaranteeIssueDate 
      style="HEIGHT: 26px; LEFT: 1px; TOP: -1px; WIDTH: 125px" tabIndex=2><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3307"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="1"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="1"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="dd/mm/yyyy"><PARAM NAME="HighlightText" VALUE="2"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</TD></TR>
  <TR>
    <TD align=right>Status</TD>
    <TD>
      <OBJECT classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 height=26 
      id=DataCombo style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 346px" 
      tabIndex=3 width=346><PARAM NAME="_ExtentX" VALUE="9155"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="0"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="0"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <TR>
    <TD align=right>Cancellation Date</TD>
    <TD>
      <OBJECT classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830 
      id=GuaranteeCancelDate 
      style="HEIGHT: 26px; LEFT: 1px; TOP: -1px; WIDTH: 125px" tabIndex=4><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3307"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="1"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="1"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="dd/mm/yyyy"><PARAM NAME="HighlightText" VALUE="2"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</TD></TR>
  <TR>
    <TD></TD>
    <TD></TD></TR></TABLE><INPUT id=btn_AddGuarantee name=btn_UpdateProspect style="CURSOR: hand; HEIGHT: 55px; LEFT: 726px; PADDING-TOP: 15px; POSITION: absolute; TOP: 329px; VERTICAL-ALIGN: sub; WIDTH: 136px; Z-INDEX:          102" title ="Add Guarantee" type =button value="Add Guarantee" class=button3><IMG 
alt="" border=0 height=23 hspace  
 
            
=0 id=pic_AddGuarantee src="images/MLSDenied.bmp" style="HEIGHT: 23px; LEFT: 781px; POSITION: absolute; TOP: 332px; WIDTH: 19px; Z-INDEX: 108" title=0 useMap="" width=19 >
<OBJECT classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABgAAAAEAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAcAAAABAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAIAAAAAQAAAAAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACQAAAAEAAACAAAAAAAAAAAQAAADAwMAACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAoAAAABAAAAgAAAAAAAAAAEAAAA///GAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAALAAAAAQAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADAAAAAIAAAAAAAAAAAAAABQAAAAGhMgA////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA0AAAADAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAOAAAABQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADwAAAAcAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABAAAAAGAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAARAAAACAAAAAAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEgAAAAkAAAAAAAAAAAAAAAQAAADAwMAACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABMAAAAKAAAAAAAAAAAAAAAEAAAA///GAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAUAAAABAAAAAAAAAAAAAAAEQAAAABsNgD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFQAAAAwAAAAAAAAAAAAAABQAAAAGhMgA////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABYAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAXAAAADwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGAAAAAsAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABkAAAAMAAAAAAAAAAAAAAAUAAAABoTIAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAaAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGwAAAA8AAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABwAAAALAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHgAAAB0AAADAAgEAAAIAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB8AAAAdAAAAwAABAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAgAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIQAAAB4AAAAAAQAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACIAAAAdAAAAwAAAAAAAAAAEAAAACAAAgAUAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAjAAAAHQAAAIAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAJAAAAB0AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAAAAAA//////////8BAAAAAAAAAAAAAAABAAAA/v///wQAAAAAAAAAAAAAAAUAAAD9////AgAAAAAAAAAAAAAAAgAAAOr///8DAAAAAAAAAAAAAAADAAAA/P///wUAAAAAAAAAAAAAAP/////7////BgAAAAAAAAAAAAAABAAAAPr///8HAAAAAAAAAAAAAAD/////8f///wgAAAAAAAAAAAAAAAYAAADv////CQAAAAAAAAAAAAAABwAAAO7///8KAAAAAAAAAAAAAAAIAAAA+f///wsAAAABAAAAAAAAAP/////r////FAAAAAEAAAAAAAAA//////j///8MAAAAAQAAAAAAAAD/////6f///w0AAAABAAAAAAAAAP/////3////DgAAAAEAAAAAAAAA//////b///8QAAAAAQAAAAAAAAD/////9f///w8AAAABAAAAAAAAAP/////w////EQAAAAEAAAAAAAAA/////+3///8SAAAAAQAAAAAAAAD/////7P///xMAAAABAAAAAAAAAP/////0////GAAAAAEAAAABAAAA//////P///8VAAAAAQAAAAEAAAD/////6P///xYAAAABAAAAAQAAAP/////y////FwAAAAEAAAABAAAA//////T///8cAAAAAQAAAAIAAAD/////8////xkAAAABAAAAAgAAAP/////o////GgAAAAEAAAACAAAA//////L///8bAAAAAQAAAAIAAAD/////CAAAAE5vcm1hbAAFoCxkBTAsZAXAK2QF8CpkBXApZAXQKGQFHQAAAEhlYWRpbmcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHgAAAEZvb3RpbmcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHwAAAFNlbGVjdGVkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIAAAAENhcHRpb24AAAAAAAAAAAAjAAAAAAAAADEAAAAQAAAAIQAAAEhpZ2hsaWdodFJvdwAAAAAAAAAAAAAAAAAAAAAAAAAAIgAAAEV2ZW5Sb3cAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIwAAAE9kZFJvdwAFAAAAAAAAAAAAAAAAAAAAAAAAAAAAkaYFJAAAAAsAAAD//wAAAwAAAAAAAAALAAAAAAAAAAMAAAAAAAAACwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAALAAAA//8AAAMAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAD//wAACwAAAAAAAAADAAAAAQAAAB4AAAABAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAB4AAAAyAAAARHJhZyBhIGNvbHVtbiBoZWFkZXIgaGVyZSB0byBncm91cCBieSB0aGF0IGNvbHVtbgAAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAPeiBgADAAAAkNADAD0AAAAAAAAADAAAAFRydWVEQkdyaWQxAAIAAAAMAAAAQWxsb3dBZGROZXcALwAAAAwAAABBbGxvd0Fycm93cwABAAAADAAAAEFsbG93RGVsZXRlAAQAAAAMAAAAQWxsb3dVcGRhdGUAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQAI/v//DAAAAEJvcmRlclN0eWxlAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAnAAAAAsAAABEYXRhTWVtYmVyACUAAAAJAAAARGF0YU1vZGUAuwAAAAkAAABEYXRhVmlldwDEAAAAEgAAAERlYWRBcmVhQmFja0NvbG9yAAoAAAAMAAAARGVmQ29sV2lkdGgAUAAAAA0AAABFZGl0RHJvcERvd24AXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkADAAAAAPAAAARXhwb3NlQ2VsbE1vZGUAkQAAAAoAAABGb290TGluZXMAwgAAAA8AAABHcm91cEJ5Q2FwdGlvbgAMAAAACgAAAEhlYWRMaW5lcwCYAAAACwAAAEluc2VydE1vZGUAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAsQAAAAoAAABMYXlvdXRVUkwASgAAAA4AAABNYXJxdWVlVW5pcXVlAM4AAAAIAAAATWF4Um93cwCjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgCEAAAADAAAAE11bHRpU2VsZWN0AGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUAlwAAABEAAABQaWN0dXJlQWRkbmV3Um93AJUAAAASAAAAUGljdHVyZUN1cnJlbnRSb3cAswAAABEAAABQaWN0dXJlRm9vdGVyUm93ALIAAAARAAAAUGljdHVyZUhlYWRlclJvdwCWAAAAEwAAAFBpY3R1cmVNb2RpZmllZFJvdwCwAAAAEwAAAFBpY3R1cmVTdGFuZGFyZFJvdwC0AAAACwAAAFByaW50SW5mb3MADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAIwAAAAcAAABTcGxpdHMAMQAAABAAAABUYWJBY3Jvc3NTcGxpdHMAMgAAAAoAAABUYWJBY3Rpb24AmQAAABcAAABUcmFuc3BhcmVudFJvd1BpY3R1cmVzADMAAAAQAAAAV3JhcENlbGxQb2ludGVyANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA 
height=500 id=TrueDBGrid1 
style="HEIGHT: 121px; LEFT: 18px; POSITION: absolute; TOP: 204px; WIDTH: 865px; Z-INDEX: 109" 
width=871></OBJECT>
<IMG 
alt="" border=0 height=23 hspace  
 
            
=0 id=pic_UpdateGuarantee src="images/MLSDenied.bmp" style="HEIGHT: 23px; LEFT: 782px; POSITION: absolute; TOP: 390px; WIDTH: 19px; Z-INDEX: 105" title=0 useMap="" width=19 ><IMG alt="" border=0 height=23 hspace 
 
 
            
=0 id=pic_DeleteGuarantee src="images/MLSDenied.bmp" style="HEIGHT: 23px; LEFT: 783px; POSITION: absolute; TOP: 449px; WIDTH: 19px; Z-INDEX: 106" title=0 useMap="" width=19 ><INPUT id=btn_Cancel name=btn_UpdateProspect style="CURSOR: hand; HEIGHT: 55px; LEFT: 725px; POSITION: absolute; TOP: 503px; VERTICAL-ALIGN: sub; VISIBILITY: hidden; WIDTH: 136px; Z-INDEX: 101" title          =Cancel type=button value=Cancel class=button3><INPUT id=btn_DeleteGuarantee name=btn_UpdateProspect style="CURSOR: hand; HEIGHT: 55px; LEFT: 726px; PADDING-TOP: 15px; POSITION: absolute; TOP: 445px; VERTICAL-ALIGN: sub; WIDTH: 136px; Z-INDEX:          104" title ="Delete Guarantee" type =button value="Delete Guarantee" class=button3><INPUT id=btn_UpdateGuarantee name=btn_UpdateProspect style="CURSOR: hand; HEIGHT: 55px; LEFT: 726px; PADDING-TOP: 15px; POSITION: absolute; TOP: 387px; VERTICAL-ALIGN: sub; WIDTH: 136px; Z-INDEX:          103" title ="Update Guarantee" type =button value="Update Guarantee" class=button3>
<OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 id=FindLoanNumber 
style="HEIGHT: 25px; LEFT: 563px; POSITION: absolute; TOP: 336px; WIDTH: 119px; Z-INDEX: 111"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3149"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="#########0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
<OBJECT classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAGAAAAAQAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABwAAAAEAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAgAAAABAAAAAAAAAAAAAAAEAAAACAAAgAUAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAJAAAAAQAAAIAAAAAAAAAABAAAAMDAwAAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACgAAAAEAAACAAAAAAAAAAAQAAAD//8YACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAsAAAABAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAMAAAAAgAAAAAAAAAAAAAAFAAAAAaEyAD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADQAAAAMAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA4AAAAFAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAPAAAABwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEAAAAAYAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABEAAAAIAAAAAAAAAAAAAAAEAAAACAAAgAUAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAASAAAACQAAAAAAAAAAAAAABAAAAMDAwAAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEwAAAAoAAAAAAAAAAAAAAAQAAAD//8YACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABQAAAAEAAAAAAAAAAAAAAARAAAAAGw2AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAVAAAADAAAAAAAAAAAAAAAFAAAAAaEyAD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABcAAAAPAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAYAAAACwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGQAAAAwAAAAAAAAAAAAAABQAAAAGhMgA////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABoAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAbAAAADwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHAAAAAsAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAeAAAAHQAAAMACAQAAAgAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHwAAAB0AAADAAAEAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACAAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAhAAAAHgAAAAABAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIgAAAB0AAADAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACMAAAAdAAAAgAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAkAAAAHQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAAAAAD//////////wEAAAAAAAAAAAAAAAEAAAD+////BAAAAAAAAAAAAAAABQAAAP3///8CAAAAAAAAAAAAAAACAAAA6v///wMAAAAAAAAAAAAAAAMAAAD8////BQAAAAAAAAAAAAAA//////v///8GAAAAAAAAAAAAAAAEAAAA+v///wcAAAAAAAAAAAAAAP/////x////CAAAAAAAAAAAAAAABgAAAO////8JAAAAAAAAAAAAAAAHAAAA7v///woAAAAAAAAAAAAAAAgAAAD5////CwAAAAEAAAAAAAAA/////+v///8UAAAAAQAAAAAAAAD/////+P///wwAAAABAAAAAAAAAP/////p////DQAAAAEAAAAAAAAA//////f///8OAAAAAQAAAAAAAAD/////9v///xAAAAABAAAAAAAAAP/////1////DwAAAAEAAAAAAAAA//////D///8RAAAAAQAAAAAAAAD/////7f///xIAAAABAAAAAAAAAP/////s////EwAAAAEAAAAAAAAA//////T///8YAAAAAQAAAAEAAAD/////8////xUAAAABAAAAAQAAAP/////o////FgAAAAEAAAABAAAA//////L///8XAAAAAQAAAAEAAAD/////9P///xwAAAABAAAAAgAAAP/////z////GQAAAAEAAAACAAAA/////+j///8aAAAAAQAAAAIAAAD/////8v///xsAAAABAAAAAgAAAP////8IAAAATm9ybWFsAAULAAAAGAAAAAAAAAD/////kLVjBQAAAAAdAAAASGVhZGluZwAAAAAAAJGmBQAAdQTwkKYFAQAAAACwYwUeAAAARm9vdGluZwAAAAAAAAAAAAAAAAAAAAAAMQAAAPAAAAAfAAAAU2VsZWN0ZWQAAAAAUAFjBQAAAAD/////QAAAADEAAAAgAAAAQ2FwdGlvbgAxAAAAYAEAAMwlYQXMJWEF////AAaEyAAhAAAASGlnaGxpZ2h0Um93AAAAAOCQpgUgBGMFDQAAABoAAAAiAAAARXZlblJvdwASAACADwAAgAAAAAAAAAAAAAAAAACRpgUjAAAAT2RkUm93AAUPAAAAGwAAAAAAAAD/////IAAAAAAAAAAkAAAACwAAAP//AAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAALAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAsAAAD//wAAAwAAAAAAAAAEAAAAAAAAAAMAAADoAwAACwAAAP//AAALAAAAAAAAAAMAAAABAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAACAAAAHgAAADIAAABEcmFnIGEgY29sdW1uIGhlYWRlciBoZXJlIHRvIGdyb3VwIGJ5IHRoYXQgY29sdW1uAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAA96IGAAMAAACQ0AMAPQAAAAAAAAALAAAAVHJ1ZURCR3JpZAACAAAADAAAAEFsbG93QWRkTmV3AC8AAAAMAAAAQWxsb3dBcnJvd3MAAQAAAAwAAABBbGxvd0RlbGV0ZQAEAAAADAAAAEFsbG93VXBkYXRlAL0AAAAOAAAAQW5pbWF0ZVdpbmRvdwDAAAAAEwAAAEFuaW1hdGVXaW5kb3dDbG9zZQC+AAAAFwAAAEFuaW1hdGVXaW5kb3dEaXJlY3Rpb24AvwAAABIAAABBbmltYXRlV2luZG93VGltZQD4/f//CwAAAEFwcGVhcmFuY2UACP7//wwAAABCb3JkZXJTdHlsZQD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAJwAAAALAAAARGF0YU1lbWJlcgAlAAAACQAAAERhdGFNb2RlALsAAAAJAAAARGF0YVZpZXcAxAAAABIAAABEZWFkQXJlYUJhY2tDb2xvcgAKAAAADAAAAERlZkNvbFdpZHRoAFAAAAANAAAARWRpdERyb3BEb3duAF8AAAAKAAAARW1wdHlSb3dzAP79//8IAAAARW5hYmxlZAAwAAAADwAAAEV4cG9zZUNlbGxNb2RlAJEAAAAKAAAARm9vdExpbmVzAMIAAAAPAAAAR3JvdXBCeUNhcHRpb24ADAAAAAoAAABIZWFkTGluZXMAmAAAAAsAAABJbnNlcnRNb2RlAF0AAAAPAAAATGF5b3V0RmlsZU5hbWUAXAAAAAsAAABMYXlvdXROYW1lALEAAAAKAAAATGF5b3V0VVJMAEoAAAAOAAAATWFycXVlZVVuaXF1ZQDOAAAACAAAAE1heFJvd3MAowAAAAoAAABNb3VzZUljb24ApAAAAA0AAABNb3VzZVBvaW50ZXIAhAAAAAwAAABNdWx0aVNlbGVjdABhAAAADgAAAE11bHRpcGxlTGluZXMAnwAAAAwAAABPTEVEcmFnTW9kZQCgAAAADAAAAE9MRURyb3BNb2RlAJcAAAARAAAAUGljdHVyZUFkZG5ld1JvdwCVAAAAEgAAAFBpY3R1cmVDdXJyZW50Um93ALMAAAARAAAAUGljdHVyZUZvb3RlclJvdwCyAAAAEQAAAFBpY3R1cmVIZWFkZXJSb3cAlgAAABMAAABQaWN0dXJlTW9kaWZpZWRSb3cAsAAAABMAAABQaWN0dXJlU3RhbmRhcmRSb3cAtAAAAAsAAABQcmludEluZm9zAA8AAAAQAAAAUm93RGl2aWRlclN0eWxlACMAAAAHAAAAU3BsaXRzADEAAAAQAAAAVGFiQWNyb3NzU3BsaXRzADIAAAAKAAAAVGFiQWN0aW9uAJkAAAAXAAAAVHJhbnNwYXJlbnRSb3dQaWN0dXJlcwAzAAAAEAAAAFdyYXBDZWxsUG9pbnRlcgDTBwAACQAAAF9FeHRlbnRYANQHAAAJAAAAX0V4dGVudFkAAAIAAAwAAABfTGF5b3V0VHlwZQAQAAAACwAAAF9Sb3dIZWlnaHQAAQIAAAsAAABfU3R5bGVEZWZzAAQCAAAWAAAAX1dhc1BlcnNpc3RlZEFzUGl4ZWxzAA== 
height=500 id=TrueDBGrid 
style="HEIGHT: 193px; LEFT: 19px; POSITION: absolute; TOP: 4px; WIDTH: 865px; Z-INDEX: 112" 
width=871></OBJECT>
<INPUT id=btn_FindLoan name=btn_FindLoan style="CURSOR: hand; HEIGHT: 28px; LEFT: 448px; POSITION: absolute; TOP: 335px; WIDTH: 110px; Z-INDEX: 110" type          =button value="Find Loan" class=button2>

</BODY>
</HTML>
