<%
  sDatabase =Session("SQLDatabase") 
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_AddEmployee = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Add Employee",Session("UserName"))
  i_UpdEmployee = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Update Employee",Session("UserName"))
  i_DelEmployee = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Delete Employee",Session("UserName"))
 %>
 
<html>
<head>
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>
<meta name="VI60_defaultClientScript" content="VBScript">
<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<script ID="clientEventHandlersVBS" LANGUAGE="vbscript">
<!--


Dim v_BookMark
Dim i_CurrentTeamNbr

if rs_open <> true then

  
   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"
    
	if  sessDSN = "DSN=" then
	   window.location.href = "Default.asp"
	   return
    end if

	set conn = createobject("ADODB.Connection")
	set rs  = createobject("ADODB.Recordset")
	set rs1  = createobject("ADODB.Recordset")
	set rs2  = createobject("ADODB.Recordset")
	set rs3  = createobject("ADODB.Recordset")
	set rs4  = createobject("ADODB.Recordset")
	set rs_Status  = createobject("ADODB.Recordset")
	set rs_temp = createobject("ADODB.Recordset")
'	rs.MaxRecords = 20000
 '   rs.CacheSize = 10
 
	'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [sahlemployee.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	conn.Open sDSN
	rs_open = false
	
end if

Sub window_onload
 
    SetAccessLightsServer
    ClearFields
    DisableFields()
	ShowSAHLEmployeeTable()
	 
	SetActionButtons
	
End Sub

Sub ShowSAHLEmployeeTable()
   
   document.body.style.cursor = "wait"

    if rs_open = true  then
       rs.Close
	end if

    sSQL = "SELECT * FROM SAHLEmployee (nolock)"
  
    rs.CursorLocation = 3
	'rs.Open sSQL ,conn,adOpenStatic
	rs.CacheSize  =10
	
    'this.style.cursor = "hand"
    
	rs.Open sSQL,conn,adOpenDynamic
	
	TrueDBGrid.DataSource = rs

  
	'Reconfigure the Grid..
    'Remove all columns
    Dim I 
	For I = 0 to TrueDBGrid.Columns.Count - 1
		TrueDBGrid.Columns.Remove(0)
	Next
   
    'Create then necessary columns...
	set tmpCol =  TrueDBGrid.Columns.Add(0)
	tmpCol.Width = 60
	tmpCol.Caption = "Number"
	tmpCol.DataField = rs.Fields("SAHLEmployeeNumber").name
	tmpCol.Visible = True
	set tmpCol =  TrueDBGrid.Columns.Add(1)
	tmpCol.Caption = "Employee UserID"
	tmpCol.Width =140
	tmpCol.DataField = rs.Fields("SAHLEmployeeName").name 
	tmpCol.Visible = True
	set tmpCol =  TrueDBGrid.Columns.Add(2)
	tmpCol.Caption = "Employee Name"
	tmpCol.Width = 160
	tmpCol.DataField = rs.Fields("SAHLEmployeeFullName").name 
	tmpCol.Visible = True
	set tmpCol =  TrueDBGrid.Columns.Add(3)
	tmpCol.Caption = "Employee Team"
    tmpCol.Width = 110
	tmpCol.DataField = rs.Fields("EmployeeTeamNumber").name 
	tmpCol.Visible = True
	set tmpCol =  TrueDBGrid.Columns.Add(4)
	tmpCol.Caption = "Employee Type (View)"
	tmpCol.Width = 160
	tmpCol.DataField = rs.Fields("EmployeeTypeNumber").name 
	tmpCol.Visible = True
	set tmpCol =  TrueDBGrid.Columns.Add(5)
	tmpCol.Caption = "User Group"
	tmpCol.DataField = rs.Fields("UserGroupNumber").name 
	tmpCol.Visible = True
	
	
	
	'Set the colors....
	TrueDBGrid.OddRowStyle.BackColor = &HC0FFFF
	TrueDBGrid.EvenRowStyle.BackColor = &HC0C0C0
	
	
	TrueDBGrid.HoldFields
	
	 call TDBOLeGridColumnTranslate(TrueDBGrid,3 ,"EmployeeTeam", "EmployeeTeamNumber", "EmployeeTeamDescription" )
	 call TDBOLeGridColumnTranslate(TrueDBGrid,4 ,"EmployeeType", "EmployeeTypeNumber", "EmployeeTypeDescription" )
	 call TDBOLeGridColumnTranslate(TrueDBGrid,5 ,"UserGroup", "UserGroupNumber", "UserGroupDescription" )
	
	
	
     rs_open = true  
     
       'SAHL Employee Status Flag Combo
 
    rs_Status.Fields.Append "StatusNumber",19
    rs_Status.Fields.Append "StatusDescription",200,180
    rs_Status.Open

	rs_Status.AddNew 
	rs_Status.fields("StatusNumber").Value = 0 
	rs_Status.fields("StatusDescription").Value = "InActive"
	rs_Status.Update
	rs_Status.AddNew 
	rs_Status.fields("StatusNumber").Value = 1 
	rs_Status.fields("StatusDescription").Value = "Active"
	rs_Status.Update
   
	rs_Status.MoveFirst
	
	set DataCombo4.RowSource = rs_Status
	DataCombo4.ListField = rs_Status.Fields("StatusDescription").name
	DataCombo4.BoundColumn =  rs_Status.Fields("StatusNumber").name
	DataCombo4.BoundText = rs_Status.Fields("StatusNumber").Value
	DataCombo4.Refresh
	if rs.RecordCount > 0 then
 		DataCombo4.BoundText = rs.Fields("SAHLEmployeeStatusFlag").Value
    end if 
	DataCombo4.Refresh

     
     
    'USER GROUP COMBO
	sSQL = "SELECT UserGroupNumber,UserGroupDescription FROM USERGROUP (nolock)"
	rs1.CursorLocation = 3
	rs1.Open sSQL ,conn,adOpenStatic

	set DataCombo.RowSource = rs1
	DataCombo.ListField = rs1.Fields("UserGroupDescription").name
	DataCombo.BoundColumn = rs1.Fields("UserGroupNumber").name
    if rs.RecordCount > 0 then
 		DataCombo.BoundText = rs.Fields("UserGroupNumber").Value
    end if 
    DataCombo.Refresh

    'Employee Type COMBO
	sSQL = "SELECT * FROM EMPLOYEETYPE (nolock)"
	rs2.CursorLocation = 3
	rs2.Open sSQL ,conn,adOpenStatic
		
	set DataCombo1.RowSource = rs2
	DataCombo1.ListField = rs2.Fields("EmployeeTypeDescription").name
	DataCombo1.BoundColumn = rs2.Fields("EmployeeTypeNumber").name
    if rs.RecordCount > 0 then
 		DataCombo1.BoundText = rs.Fields("EmployeeTypeNumber").Value
    end if 
    DataCombo1.Refresh

	'Employee Team  COMBO
	sSQL = "SELECT * FROM EMPLOYEETEAM (nolock)"
	rs3.CursorLocation = 3
	rs3.Open sSQL ,conn,adOpenStatic
	
	set DataCombo2.RowSource = rs3
	DataCombo2.ListField = rs3.Fields("EmployeeTeamDescription").name
	DataCombo2.BoundColumn = rs3.Fields("EmployeeTeamNumber").name
    if rs.RecordCount > 0 then
 		DataCombo2.BoundText = rs.Fields("EmployeeTeamNumber").Value
    end if 
    DataCombo2.Refresh
	
    'SAHL Branch  COMBO
	sSQL = "SELECT * FROM SAHLBRANCH (nolock)"
	rs4.CursorLocation = 3
	rs4.Open sSQL ,conn,adOpenStatic
	
	set DataCombo3.RowSource = rs4
	DataCombo3.ListField = rs4.Fields("SAHLBranchName").name
	DataCombo3.BoundColumn = rs4.Fields("SAHLBranchNumber").name
    if rs.RecordCount > 0 then
 		DataCombo3.BoundText = rs.Fields("SAHLBranchNumber").Value
    end if 
    DataCombo3.Refresh
    
    rs.MoveFirst
		 
	EnableAllControls
    document.body.style.cursor = "default"


End sub
Sub TrueDBGrid_RowColChange(LastRow, LastCol)

	window.SAHLEmployeeNumber.Value =  rs.Fields("SAHLEmployeeNumber").Value
	window.SAHLEmployeeName.Text =  rs.Fields("SAHLEmployeeName").Value
	window.SAHLEmployeeFullName.Text =  rs.Fields("SAHLEmployeeFullName").Value
 	window.SAHLEmployeeCode.Value =  rs.Fields("SAHLEmployeeCode").Value

    
    DataCombo.BoundText = rs.Fields("UserGroupNumber").Value
    DataCombo1.BoundText = rs.Fields("EmployeeTypeNumber").Value
    DataCombo2.BoundText = rs.Fields("EmployeeTeamNumber").Value
	DataCombo3.BoundText = rs.Fields("SAHLBranchNumber").Value
	DataCombo4.BoundText = rs.Fields("SAHLEmployeeStatusFlag").Value
	
End Sub

Sub SetAccessLightsServer
     
     
    sRes1 = "<%=i_AddEmployee%>"
    if sRes1 = "Allowed" then
       window.pic_AddEmployee.src = "images/MLSAllowed.bmp"
       window.pic_AddEmployee.title = "1"
    else
       window.pic_AddEmployee.src = "images/MLSDenied.bmp"
       window.pic_AddEmployee.title = "0"
	end if 
	
	 sRes1 = "<%=i_UpdEmployee%>"
    if sRes1 = "Allowed" then
       window.pic_UpdateEmployee.src = "images/MLSAllowed.bmp"
       window.pic_UpdateEmployee.title = "1"
    else
       window.pic_UpdateEmployee.src = "images/MLSDenied.bmp"
       window.pic_UpdateEmployee.title = "0"
	end if
	
	 sRes1 = "<%=i_DelEmployee%>"
    if sRes1 = "Allowed" then
       window.pic_DelEmployee.src = "images/MLSAllowed.bmp"
       window.pic_DelEmployee.title = "1"
    else
       window.pic_DelEmployee.src = "images/MLSDenied.bmp"
       window.pic_DelEmployee.title = "0"
	end if

	  
		
end Sub



Sub SetActionButtons

 
  if rs.RecordCount > 0 then
     window.btn_UpdEmployee.disabled = false
     window.btn_DelEmployee.disabled = false
  else
     window.btn_UpdEmployee.disabled = true
     window.btn_DelEmployee.disabled = true
  end if

end sub


Sub DisableFields

	window.SAHLEmployeeNumber.Enabled = false
	window.SAHLEmployeeName.Enabled = false
	window.SAHLEmployeeFullName.Enabled = false
	window.SAHLEmployeeCode.Enabled = false
	window.DataCombo.Enabled = false
	window.DataCombo1.Enabled = false
	window.DataCombo2.Enabled = false
	window.DataCombo3.Enabled = false
	window.DataCombo4.Enabled = false
	

End Sub

Function EnableAllControls()
     TrueDBGrid.Enabled = true
     TrueDBGrid.style.cursor = "hand"
     btn_AddEmployee.disabled = false
     btn_UpdEmployee.disabled = false
     btn_DelEmployee.disabled = false
     
     btn_AddEmployee.style.cursor = "hand"
     btn_UpdEmployee.style.cursor = "hand"
     btn_DelEmployee.style.cursor = "hand"
     
     
End function

Sub btn_AddEmployee_onclick

 if window.pic_AddEmployee.title = "0" then
      window.status = "Access denied to " & window.btn_AddEmployee.title
      exit sub
    end if

  v_BookMark = TrueDBGrid.Bookmark

  if btn_AddEmployee.value = "Add" then

  
   'Disable other controls...
	DisableControls("Add")

	'Enable Fields
	EnableFields("Add")
	btn_CanceEmployeeAction.style.visibility = "visible"
		

	'Set Focus 
    'Set the button caption..
	btn_AddEmployee.value = "Commit"  

	ClearFields
	window.SAHLEmployeeNumber.style.visibility = "hidden"
	window.SAHLEmployeeName.HighlightText = true
	window.SAHLEmployeeName.focus()

 
	
  elseif btn_AddEmployee.value = "Commit" then


  	call AddSAHLEmployeeRecord (window.SAHLEmployeeName.Text, _ 
  	                          window.SAHLEmployeeFullName.Text, _
  	                          window.SAHLEmployeeCode.Value, _
  	                          window.DataCombo.BoundText, _
  	                          window.DataCombo1.BoundText, _
  	                          window.DataCombo2.BoundText, _
  	                          window.DataCombo3.BoundText)
  	                      	
  	'Clean up...

	btn_AddEmployee.value = "Add"
	btn_CanceEmployeeAction.style.visibility = "hidden"
    window.SAHLEmployeeNumber.style.visibility = "visible"
	EnableControls
	DisableFields()

	SetActionButtons
	
	End if
	
End Sub

Function DisableControls(s_Action)
  
   TrueDBGrid.Enabled = false
     
  if s_Action =  "Add" then
	 btn_UpdEmployee.disabled = true
     btn_DelEmployee.disabled = true
     window.btn_UpdEmployee.style.visibility = "hidden"
     window.btn_DelEmployee.style.visibility = "hidden"
     window.pic_UpdateEmployee.style.visibility = "hidden"
     window.pic_DelEmployee.style.visibility = "hidden"
  Elseif s_Action = "Update" then
	 btn_AddEmployee.disabled = true
     btn_DelEmployee.disabled = true     
     window.btn_DelEmployee.style.visibility = "hidden"
     window.btn_AddEmployee.style.visibility = "hidden"
     window.pic_AddEmployee.style.visibility = "hidden"
     window.pic_DelEmployee.style.visibility = "hidden"
     
  Elseif s_Action = "Delete" then
	 btn_AddEmployee.disabled = true
     btn_UpdEmployee.disabled = true  
     window.btn_UpdEmployee.style.visibility = "hidden"
     window.btn_AddEmployee.style.visibility = "hidden"
     window.pic_UpdateEmployee.style.visibility = "hidden"
     window.pic_AddEmployee.style.visibility = "hidden"
         
  end if
     
  
 
End Function

Function EnableFields(s_Action)

	window.SAHLEmployeeNumber.Enabled = True
	window.SAHLEmployeeName.Enabled = True
	window.SAHLEmployeeFullName.Enabled = True
	window.SAHLEmployeeCode.Enabled = True
	window.DataCombo.Enabled = True
	window.DataCombo1.Enabled = True
	window.DataCombo2.Enabled = True
	window.DataCombo3.Enabled = True
	window.DataCombo4.Enabled = True
  

End Function

Function ClearFields()

    window.SAHLEmployeeNumber.Value  = 0
	window.SAHLEmployeeName.Text  = ""
	window.SAHLEmployeeFullName.Text  = ""
	window.SAHLEmployeeCode.Value = 0
	window.DataCombo.Text = ""
	window.DataCombo1.Text = ""
	window.DataCombo2.Text = ""
	window.DataCombo3.Text = ""
	window.DataCombo4.Text= ""
	
End Function

Function EnableControls()
  
     TrueDBGrid.Enabled = true
     btn_AddEmployee.disabled = false
     
     if rs.RecordCount >  0 then
		btn_UpdEmployee.disabled = false
		btn_DelEmployee.disabled = false
     else
		btn_UpdEmployee.disabled = true
		btn_DelEmployee.disabled = true
     
     end if
     
     window.btn_AddEmployee.style.visibility = "visible"
     window.btn_UpdEmployee.style.visibility = "visible"
     window.btn_DelEmployee.style.visibility = "visible"
     window.pic_AddEmployee.style.visibility = "visible"
     window.pic_UpdateEmployee.style.visibility = "visible"
     window.pic_DelEmployee.style.visibility = "visible"
     
End Function

Sub SetActionButtons

 
  if rs.RecordCount > 0 then
     window.btn_UpdEmployee.disabled = false
     window.btn_DelEmployee.disabled = false
  else
     window.btn_UpdEmployee.disabled = true
     window.btn_DelEmployee.disabled = true
  end if

end sub
Sub btn_CanceEmployeeAction_onclick
   	
   	ClearFields
   	if rs.RecordCount > 0 then
		rs.Requery
		TrueDBGrid.Bookmark = v_BookMark
	end if

	btn_AddEmployee.value = "Add"
	btn_UpdEmployee.value = "Update"
	btn_DelEmployee.value = "Delete"
	btn_CanceEmployeeAction.style.visibility = "hidden"
	window.SAHLEmployeeNumber.style.visibility = "visible"

	EnableControls
	DisableFields

End Sub

Sub btn_UpdEmployee_onclick
  
   if window.pic_UpdateEmployee.title = "0" then
      window.status = "Access denied to " & window.btn_UpdEmployee.title
      exit sub
    end if
  v_BookMark = TrueDBGrid.Bookmark
  
  if btn_UpdEmployee.value = "Update" then
    i_CurrentTeamNbr = Cint(rs.Fields("EmployeeTeamNumber").Value)
   'Disable other controls...
	DisableControls("Update")

	'Enable Fields
	EnableFields("Update")
	btn_CanceEmployeeAction.style.visibility = "visible"
		
	'Set Focus
    'Set the button caption..
	btn_UpdEmployee.value = "Commit"  
	  

	window.SAHLEmployeeName.HighlightText = true
	window.SAHLEmployeeName.focus()
	
  elseif btn_UpdEmployee.value = "Commit" then
  
  cr = Chr(13) & chr(10)
  i_Resp = 0
  
'  GaryD: this functionality is no longer required
'    sUserName = "<%= Session("UserName")%>"
'	sSQL = "y_GetSAHLEmployeeProspectCount " & rs.Fields("SAHLEMployeeNumber").Value
'	'msgbox sSQL
'	rs_temp.CursorLocation = 3
'	rs_temp.Open sSQL,conn,adOpenDynamic
	
'    if rs_temp.Fields(0).Value > 0 then
      
'		if Cint(window.DataCombo2.BoundText) <> i_CurrentTeamNbr then
'   
'		   i_Resp = MsgBox("Do you want to move this Employee's Prospects to the Employee's new Team...?? " & cr & cr  & "Press YES to move the Employee's prospects to his/her new Team..." & cr & "Press NO to move them to the Unknown Employee...." & cr & "Press CANCEL to keep the Employee's current Team but update the other fields....", 3,"Employee Team Change")
'		   
'		end if
'		call UpdateSAHLEmployeeRecord ( _
'		                           rs.Fields("SAHLEMployeeNumber").Value, _
'		                           window.SAHLEmployeeName.Text, _ 
'  			                          window.SAHLEmployeeFullName.Text, _
'  			                          window.SAHLEmployeeCode.Value, _
'  			                          window.DataCombo.BoundText, _
'  			                          window.DataCombo1.BoundText, _
'  			                          window.DataCombo2.BoundText, _
'  			                          window.DataCombo3.BoundText, _
'  			                          Cint(i_Resp))
'  	else
  	   call UpdateSAHLEmployeeRecord ( _
		                           rs.Fields("SAHLEMployeeNumber").Value, _
		                           window.SAHLEmployeeName.Text, _ 
  			                          window.SAHLEmployeeFullName.Text, _
  			                          window.SAHLEmployeeCode.Value, _
  			                          window.DataCombo.BoundText, _
  			                          window.DataCombo1.BoundText, _
  			                          window.DataCombo2.BoundText, _
  			                          window.DataCombo3.BoundText, _
  			                          0)
'  	End If
 
'   rs_temp.Close
	''Clean up...
	btn_UpdEmployee.value = "Update"
	btn_CanceEmployeeAction.style.visibility = "hidden"
	EnableControls
	DisableFields
    
    TrueDBGrid.Bookmark = v_BookMark
  end if
  
End Sub

Sub pic_DelEmployee_onclick

 if window.pic_DelEmployee.title = "0" then
      window.status = "Access denied to " & window.btn_DelEmployee.title
      exit sub
    end if
  v_BookMark = TrueDBGrid.Bookmark
  
  if btn_DelEmployee.value = "Delete" then
    
    'Disable other controls...
	DisableControls("Delete")
	
	btn_CanceEmployeeAction.style.visibility = "visible"
    btn_DelEmployee.value = "Commit" 
	
  elseif btn_DelEmployee.value = "Commit" then
  

    'call DelControlFileRecord(ControlNumber.Text)
	
	'Clean up...
	btn_DelEmployee.value = "Delete"
	btn_CanceEmployeeAction.style.visibility = "hidden"
	EnableControls
	DisableFields
	  
    if rs.RecordCount > 0 then
      TrueDBGrid.Bookmark = v_BookMark
    else
       SetActionButtons
       ClearFields
    end if

  end if
  
End Sub

Function AddSAHLEmployeeRecord(s_Name,s_FullName,i_Code,i_UserGroup,i_Type,i_Team,i_Branch)
Dim conn
    

    set conn = createobject("ADODB.Connection")
    'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [sahlemployee.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    conn.CursorLocation = 1
    
	conn.Open sDSN	

  
	conn.Execute "y_AddSAHLEmployeeRecord '" & s_Name &  "','" & s_FullName &  "'," & i_Code &  "," & i_UserGroup &  "," & i_Type &  "," & i_Team &  "," & i_Branch & "," & CInt(DataCombo4.BoundText)
    conn.close
   
    rs.Requery 
    rs.MoveLast
     
End Function

Function UpdateSAHLEmployeeRecord(i_Number,s_Name,s_FullName,i_Code,i_UserGroup,i_Type,i_Team,i_Branch,i_Resp)
Dim conn
    
    set conn = createobject("ADODB.Connection")
    'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [sahlemployee.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    conn.CursorLocation = 1
    
	conn.Open sDSN	
	conn.Execute "y_UpdSAHLEmployeeRecord " & i_Number & ",'" & s_Name &  "','" & s_FullName &  "'," & i_Code &  "," & i_UserGroup &  "," & i_Type &  "," & i_Team &  "," & i_Branch & "," & i_Resp & "," & CInt(DataCombo4.BoundText)
    conn.close
   
    rs.Requery 

End Function

Function DeleteSAHLEmployeeRecord(i_Number)
Dim conn
    
    set conn = createobject("ADODB.Connection")
    'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [sahlemployee.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    conn.CursorLocation = 1
    
	conn.Open sDSN	

	conn.Execute "y_DelSAHLEmployeeRecord " & i_Number 
    conn.close
   
    rs.Requery 

End Function

Sub btn_DelEmployee_onclick
 if window.pic_DelEmployee.title = "0" then
      window.status = "Access denied to " & window.btn_DelEmployee.title
      exit sub
    end if
  v_BookMark = TrueDBGrid.Bookmark
  
  if btn_DelEmployee.value = "Delete" then
    
    'Disable other controls...
	DisableControls("Delete")
	
	btn_CanceEmployeeAction.style.visibility = "visible"
    btn_DelEmployee.value = "Commit" 
	
  elseif btn_DelEmployee.value = "Commit" then
  

    call DeleteSAHLEmployeeRecord(rs.Fields("SAHLEmployeeNumber"))
	
	'Clean up...
	btn_DelEmployee.value = "Delete"
	btn_CanceEmployeeAction.style.visibility = "hidden"
	EnableControls
	DisableFields
	  
    if rs.RecordCount > 0 then
      TrueDBGrid.Bookmark = v_BookMark
    else
       SetActionButtons
       ClearFields
    end if

  end if
End Sub

Sub TDBOLeGridColumnTranslate(ByRef TDBGrid_TDBGrid, ByVal i_Column , ByVal s_LookupTable, ByVal s_LookupTableKey, ByVal s_LookupTableColumn )
    'TDBGrid - the grid as a TBDGrid that you want to do the translation in
    'i_Column - the column number that you want to translate
    's_LookupTable - the table name that you want to look up
    's_LookupTableKey - the primary key name to the lookup table
    's_LookupTableColumn - the column name you want to translate to
    dim color
    dim forecolor
    color = &H000000FF& ' &H000CCFF&
    forecolor =  &H00FFFFFF&
    
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
                FormatStyle.BackColor =  color 'vbGreen
                FormatStyle.ForeColor = forecolor
                TDBGrid_TDBGrid.Columns.Item(i_Column).AddRegexCellStyle 0,FormatStyle, rs_Lookup.Fields(1)
               ' tmpCol.AddRegexCellStyle 0,FormatStyle, rs_Lookup.Fields(1)
                color   = color + 3400

        End Select

        'set TempCol = TDBGrid_TDBGrid.Columns.item(i_Column)

		TDBGrid_TDBGrid.Columns.Item(i_Column).ValueItems.Add(Itm)
        rs_Lookup.MoveNext
        
    Loop
    rs_Lookup.Close

End Sub

Sub TrueDBGrid_HeadClick(ColIndex)

	set tmpCol =  TrueDBGrid.Columns.item(ColIndex)
	s =  tmpCol.DataField
	rs.Sort =  s
End Sub

-->
</script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">

<body topMargin="0" bottomMargin="0" leftMargin="0" rightMargin="0" class=Generic>

<p><input id="btn_CanceEmployeeAction" name="btn_CanceEmployeeAction" style="CURSOR: hand;  HEIGHT: 60px; LEFT: 720px; POSITION: absolute; TOP: 476px; VISIBILITY: hidden; WIDTH: 124px; Z-INDEX: 109" type  ="button" value="Cancel" width="124" height="60" class=button3>
<OBJECT classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAGAAAAAQAAAMAAAAAAAAAABAAAAAAAgAD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABwAAAAEAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAgAAAABAAAAwAAAAAAAAAAEAAAAAACAAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAJAAAAAQAAAIAAAAAAAAAABAAAAMDAwAAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACgAAAAEAAACAAAAAAAAAAAQAAAD//8YACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAsAAAABAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAMAAAAAgAAAAAAAAAAAAAAFAAAAFKBugD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADQAAAAMAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA4AAAAFAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAPAAAABwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEAAAAAYAAAAAAAAAAAAAAAQAAAAAAIAA////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABEAAAAIAAAAAAAAAAAAAAAEAAAAAACAAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAASAAAACQAAAAAAAAAAAAAABAAAAMDAwAAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEwAAAAoAAAAAAAAAAAAAAAQAAAD//8YACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABQAAAAEAAAAAAAAAAAAAAARAAAAAFkAAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAVAAAADAAAAAAAAAAAAAAAFAAAAFKBugD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABcAAAAPAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAYAAAACwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGQAAAAwAAAAAAAAAAAAAABQAAABSgboA////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABoAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAbAAAADwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHAAAAAsAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAeAAAAHQAAAMACAQAAAgAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHwAAAB0AAADAAAEAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACAAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAhAAAAHgAAAAABAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIgAAAB0AAADAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACMAAAAdAAAAgAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAkAAAAHQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAAAAAD//////////wEAAAAAAAAAAAAAAAEAAAD+////BAAAAAAAAAAAAAAABQAAAP3///8CAAAAAAAAAAAAAAACAAAA6v///wMAAAAAAAAAAAAAAAMAAAD8////BQAAAAAAAAAAAAAA//////v///8GAAAAAAAAAAAAAAAEAAAA+v///wcAAAAAAAAAAAAAAP/////x////CAAAAAAAAAAAAAAABgAAAO////8JAAAAAAAAAAAAAAAHAAAA7v///woAAAAAAAAAAAAAAAgAAAD5////CwAAAAEAAAAAAAAA/////+v///8UAAAAAQAAAAAAAAD/////+P///wwAAAABAAAAAAAAAP/////p////DQAAAAEAAAAAAAAA//////f///8OAAAAAQAAAAAAAAD/////9v///xAAAAABAAAAAAAAAP/////1////DwAAAAEAAAAAAAAA//////D///8RAAAAAQAAAAAAAAD/////7f///xIAAAABAAAAAAAAAP/////s////EwAAAAEAAAAAAAAA//////T///8YAAAAAQAAAAEAAAD/////8////xUAAAABAAAAAQAAAP/////o////FgAAAAEAAAABAAAA//////L///8XAAAAAQAAAAEAAAD/////9P///xwAAAABAAAAAgAAAP/////z////GQAAAAEAAAACAAAA/////+j///8aAAAAAQAAAAIAAAD/////8v///xsAAAABAAAAAgAAAP////8IAAAATm9ybWFsAP8wAAAAAAAAALwhwAMUI78DMQAAACABAAAdAAAASGVhZGluZwBAAAAAMQAAALC9vwMAa8ADQAAAADEAAAAeAAAARm9vdGluZwAxAAAAkAEAANwhvwPcIb8DAAAAAAAAAAAfAAAAU2VsZWN0ZWQAAAAAMQAAALC9vwOgasADCAAAACsAAAAgAAAAQ2FwdGlvbgAAAAAAAAAAAAAAAAAAAAAAAAAAANC9vwMhAAAASGlnaGxpZ2h0Um93AGjAA+BiwAMgaGVywAMAAEEAAAAiAAAARXZlblJvdwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAjAAAAT2RkUm93AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAkAAAACwAAAP//AAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAALAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAsAAAD//wAAAwAAAAAAAAAEAAAAAAAAAAMAAADoAwAACwAAAP//AAALAAAAAAAAAAMAAAABAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAACAAAAHgAAADIAAABEcmFnIGEgY29sdW1uIGhlYWRlciBoZXJlIHRvIGdyb3VwIGJ5IHRoYXQgY29sdW1uAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAA96IGAAMAAACQ0AMAPQAAAAAAAAALAAAAVHJ1ZURCR3JpZAACAAAADAAAAEFsbG93QWRkTmV3AC8AAAAMAAAAQWxsb3dBcnJvd3MAAQAAAAwAAABBbGxvd0RlbGV0ZQAEAAAADAAAAEFsbG93VXBkYXRlAL0AAAAOAAAAQW5pbWF0ZVdpbmRvdwDAAAAAEwAAAEFuaW1hdGVXaW5kb3dDbG9zZQC+AAAAFwAAAEFuaW1hdGVXaW5kb3dEaXJlY3Rpb24AvwAAABIAAABBbmltYXRlV2luZG93VGltZQD4/f//CwAAAEFwcGVhcmFuY2UACP7//wwAAABCb3JkZXJTdHlsZQD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAJwAAAALAAAARGF0YU1lbWJlcgAlAAAACQAAAERhdGFNb2RlALsAAAAJAAAARGF0YVZpZXcAxAAAABIAAABEZWFkQXJlYUJhY2tDb2xvcgAKAAAADAAAAERlZkNvbFdpZHRoAFAAAAANAAAARWRpdERyb3BEb3duAF8AAAAKAAAARW1wdHlSb3dzAP79//8IAAAARW5hYmxlZAAwAAAADwAAAEV4cG9zZUNlbGxNb2RlAJEAAAAKAAAARm9vdExpbmVzAMIAAAAPAAAAR3JvdXBCeUNhcHRpb24ADAAAAAoAAABIZWFkTGluZXMAmAAAAAsAAABJbnNlcnRNb2RlAF0AAAAPAAAATGF5b3V0RmlsZU5hbWUAXAAAAAsAAABMYXlvdXROYW1lALEAAAAKAAAATGF5b3V0VVJMAEoAAAAOAAAATWFycXVlZVVuaXF1ZQDOAAAACAAAAE1heFJvd3MAowAAAAoAAABNb3VzZUljb24ApAAAAA0AAABNb3VzZVBvaW50ZXIAhAAAAAwAAABNdWx0aVNlbGVjdABhAAAADgAAAE11bHRpcGxlTGluZXMAnwAAAAwAAABPTEVEcmFnTW9kZQCgAAAADAAAAE9MRURyb3BNb2RlAJcAAAARAAAAUGljdHVyZUFkZG5ld1JvdwCVAAAAEgAAAFBpY3R1cmVDdXJyZW50Um93ALMAAAARAAAAUGljdHVyZUZvb3RlclJvdwCyAAAAEQAAAFBpY3R1cmVIZWFkZXJSb3cAlgAAABMAAABQaWN0dXJlTW9kaWZpZWRSb3cAsAAAABMAAABQaWN0dXJlU3RhbmRhcmRSb3cAtAAAAAsAAABQcmludEluZm9zAA8AAAAQAAAAUm93RGl2aWRlclN0eWxlACMAAAAHAAAAU3BsaXRzADEAAAAQAAAAVGFiQWNyb3NzU3BsaXRzADIAAAAKAAAAVGFiQWN0aW9uAJkAAAAXAAAAVHJhbnNwYXJlbnRSb3dQaWN0dXJlcwAzAAAAEAAAAFdyYXBDZWxsUG9pbnRlcgDTBwAACQAAAF9FeHRlbnRYANQHAAAJAAAAX0V4dGVudFkAAAIAAAwAAABfTGF5b3V0VHlwZQAQAAAACwAAAF9Sb3dIZWlnaHQAAQIAAAsAAABfU3R5bGVEZWZzAAQCAAAWAAAAX1dhc1BlcnNpc3RlZEFzUGl4ZWxzAA== 
height=500 id=TrueDBGrid 
style="HEIGHT: 333px; LEFT: 30px; POSITION: absolute; TOP: 0px; WIDTH: 825px; Z-INDEX: 100" 
width=871></OBJECT>

<input id="btn_AddEmployee" name="btn_AddEmployee" style="CURSOR: hand;  HEIGHT: 61px; LEFT: 50px; POSITION: absolute; TOP: 476px; VERTICAL-ALIGN: sub; WIDTH: 124px; Z-INDEX: 103" title  ="Add Employee" type="button" value="Add" class=button3>
<input id="btn_UpdEmployee" name="btn_UpdEmployee" style="CURSOR: hand;  HEIGHT: 60px; LEFT: 280px; POSITION: absolute; TOP: 476px; VERTICAL-ALIGN: sub; WIDTH: 124px; Z-INDEX: 104" title  ="Update Employee" type="button" value="Update" height="60" width="124" class=button3>
<input id="btn_DelEmployee" name="btn_DelEmployee" style="CURSOR: hand;  HEIGHT: 60px; LEFT: 500px; POSITION: absolute; TOP: 475px; VERTICAL-ALIGN: sub; WIDTH: 124px; Z-INDEX: 105" title  ="Delete Employee" type="button" value="Delete" height="60" width="124" class=button3>
<IMG height=23 id=pic_AddEmployee src="images/MLSDenied.bmp" style="HEIGHT: 23px; LEFT: 101px; POSITION: absolute; TOP: 480px; WIDTH: 19px; Z-INDEX: 106" title=0 width=19>
<IMG alt="" border=0 height=23 hspace=0 id           =pic_UpdateEmployee src="images/MLSDenied.bmp" style="HEIGHT: 23px; LEFT: 335px; POSITION: absolute; TOP: 479px; WIDTH: 19px; Z-INDEX: 107" title=0 useMap="" width=19 > 
<table border="0" cellPadding="1" cellSpacing="1" style="HEIGHT: 121px;  LEFT: 30px; POSITION: absolute; TOP: 333px; WIDTH: 825px; Z-INDEX: 101" width="75%" class=Table1>
  
  <tr>
    <td noWrap>
      <p align="right" style="VERTICAL-ALIGN: top">Employee Number</p></td>
    <td noWrap>
      <p>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 
      id=SAHLEmployeeNumber 
      style="HEIGHT: 25px; LEFT: 0px; TOP: 0px; WIDTH: 80px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2117"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0;;Null"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="####0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999"><PARAM NAME="MinValue" VALUE="-99999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</p></td>
    <td align="right" noWrap>User Group</td>
    <td noWrap>
      <p>
      <OBJECT classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 height=26 
      id=DataCombo style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 325px" 
      tabIndex=4 width=325><PARAM NAME="_ExtentX" VALUE="8599"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="0"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</p></td></tr>
  <tr>
    <td noWrap>
      <p align="right" style="VERTICAL-ALIGN: middle">User ID</p></td>
    <td noWrap>
      <p>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D 
      id=SAHLEmployeeName 
      style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 247px" tabIndex=1><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6535"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</p></td>
    <td align="right" noWrap>Employee Type</td>
    <td noWrap>
      <p>
      <OBJECT classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 height=26 
      id=DataCombo1 style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 325px" 
      tabIndex=5 width=325><PARAM NAME="_ExtentX" VALUE="8599"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="0"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</p></td></tr>
  <tr>
    <td noWrap>
      <p align="right" style="VERTICAL-ALIGN: bottom">Employee&nbsp;Name </p></td>
    <td noWrap>
      <p>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D 
      id=SAHLEmployeeFullName 
      style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 247px" tabIndex=2><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6535"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</p></td>
    <td align="right" noWrap>Employee Team</td>
    <td noWrap>
      <p>
      <OBJECT classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 height=26 
      id=DataCombo2 style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 325px" 
      tabIndex=6 width=325><PARAM NAME="_ExtentX" VALUE="8599"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="0"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</p></td></tr>
  <tr>
    <td align="right" noWrap>Code</td>
    <td noWrap>
      <P>
      <OBJECT classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 height=26 
      id=DataCombo4 style="HEIGHT: 26px; WIDTH: 247px" tabIndex=3 width=247><PARAM NAME="_ExtentX" VALUE="6535"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="0"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</P></td>
    <td align="right" noWrap>SAHL Branch</td>
    <td noWrap>
      <p>
      <OBJECT classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 height=26 
      id=DataCombo3 style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 325px" 
      tabIndex=7 width=325><PARAM NAME="_ExtentX" VALUE="8599"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="0"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</p></td></tr></table></p><IMG 
alt="" border=0 height=23 hspace=0 id=pic_DelEmployee 
src="images/MLSDenied.bmp" 
style="HEIGHT: 23px; LEFT: 550px; POSITION: absolute; TOP: 479px; WIDTH: 19px; Z-INDEX: 108" 
title=0 useMap="" width=19>
<OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 id=SAHLEmployeeCode 
style="HEIGHT: 25px; LEFT: 180px; POSITION: absolute; TOP: 533px; VISIBILITY: hidden; WIDTH: 80px; Z-INDEX: 110" 
tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2117"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0;;Null"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="####0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999"><PARAM NAME="MinValue" VALUE="-99999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</body>
</html>
