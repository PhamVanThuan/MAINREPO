<%

 Function GetAccess
 
  dim i_UserGroup
  i_UserGroup = "xxx"
  
  sDatabase =Session("SQLDatabase") 
  sUid = Session("UserID")
  
  Session("UG") = ""

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_UserGroup = 0 
  i_UserGroup = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"User Group",Session("UserName"))
  Session("UG") = i_UserGroup
  i_Functions = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Functions",Session("UserName"))
   Session("F") = i_Functions
  i_SAHLEmployees = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"SAHL Employees",Session("UserName"))
   Session("SE") = i_SAHLEmployees
  i_GroupAccess = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Group Access",Session("UserName"))
   Session("GA") = i_GroupAccess
  i_EndOfPeriod = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"End Of Period",Session("UserName"))
   Session("EOP") = i_EndOfPeriod
  i_Exports = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Exports",Session("UserName"))
   Session("E") = i_Exports
  i_Imports = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Imports",Session("UserName"))
   Session("I") = i_Imports
   
   i_AssetTxfer = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Asset Transfer",Session("UserName"))
   Session("AT") = i_AssetTxfer
  i_Reports = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"System Reports",Session("UserName"))
   Session("RS") = i_Reports
  GetAccess = "done"
  
   set oSecurity = nothing
  
End Function

%>
 
<html>
<head>
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>

<OBJECT id=oPictures style="LEFT: 0px; TOP: 0px" classid="clsid:715A0A4D-7EB2-44DD-8515-3DF2316F14EA"  declare VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="5054">
	<PARAM NAME="_ExtentY" VALUE="1138"></OBJECT>
	
<meta name="VI60_DTCScriptingPlatform" content="Server (ASP)">
<meta name="VI60_defaultClientScript" content="VBScript">
<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<script ID="clientEventHandlersVBS" LANGUAGE="vbscript">
<!--

Dim conn
Dim v_BookMark
Dim Flag 
Dim b_updinProgress
 
Dim sStyle

Dim rs_open
Dim rs1_open 
Dim rs2_open 
	
if rs_open <> true then

  
   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"
    
	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   window.close
    end if

	set conn = createobject("ADODB.Connection")
	set rs  = createobject("ADODB.Recordset")
	set rs1  = createobject("ADODB.Recordset")
	set rs2  = createobject("ADODB.Recordset")
	set sStyle  = createobject("TrueOleDBGrid60.Style")
	set rs_topLevelFunctions  = createobject("ADODB.Recordset")
 
	'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [GroupAccess.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	conn.Open sDSN
	rs_open = false
	
end if

Sub SetSystemAccessLightsServer
     
    x= "<%=GetAccess()%>"
    'msgbox x
    sRes1 = "<%=Session("UG")%>"
    'msgbox sRes1
     if sRes1 = "Allowed" then
          parent.SystemOptions.pic_UserGroup.src = "images/accessallowedorange1.bmp"
          parent.SystemOptions.pic_UserGroup.title = "1"
    else
          parent.SystemOptions.pic_UserGroup.src = "images/accessdeniedorange1.bmp"
          parent.SystemOptions.pic_UserGroup.title = "0"
	end if   
	
	 sRes1 = "<%=Session("F")%>"
     if sRes1 = "Allowed" then
          parent.SystemOptions.pic_Functions.src ="images/accessallowedorange1.bmp"
          parent.SystemOptions.pic_Functions.title = "1"
    else
          parent.SystemOptions.pic_Functions.src = "images/accessdeniedorange1.bmp"
          parent.SystemOptions.pic_Functions.title = "0"
	end if 
	
	sRes1 = "<%=Session("SE")%>"
    if sRes1 = "Allowed" then
          parent.SystemOptions.pic_SAHLEmployee.src = "images/accessallowedorange1.bmp"
          parent.SystemOptions.pic_SAHLEmployee.title = "1"
    else
          parent.SystemOptions.pic_SAHLEmployee.src = "images/accessdeniedorange1.bmp"
          parent.SystemOptions.pic_SAHLEmployee.title = "0"
	end if   
	
	 sRes1 = "<%=Session("GA")%>"
	if sRes1 = "Allowed" then
          parent.SystemOptions.pic_GroupAccess.src = "images/accessallowedorange1.bmp"
          parent.SystemOptions.pic_GroupAccess.title = "1"
    else
          parent.SystemOptions.pic_GroupAccess.src = "images/accessdeniedorange1.bmp"
          parent.SystemOptions.pic_GroupAccess.title = "0"    
	end if   
	 
	 sRes1 = "<%=Session("EOP")%>"  
    if sRes1 = "Allowed" then
          parent.SystemOptions.pic_EndOfPEriod.src = "images/accessallowedorange1.bmp"
          parent.SystemOptions.pic_EndOfPEriod.title = "1"
    else
          parent.SystemOptions.pic_EndOfPEriod.src = "images/accessdeniedorange1.bmp"
          parent.SystemOptions.pic_EndOfPEriod.title = "0" 
	end if 
	
	 sRes1 = "<%=Session("E")%>"	
	if sRes1 = "Allowed" then
          parent.SystemOptions.pic_Exports.src = "images/accessallowedorange1.bmp"
          parent.SystemOptions.pic_Exports.title = "1"
    else
          parent.SystemOptions.pic_Exports.src ="images/accessdeniedorange1.bmp"
          parent.SystemOptions.pic_Exports.title = "0" 
	end if   
	
	sRes1 = "<%=Session("I")%>"
	if sRes1 = "Allowed" then
          parent.SystemOptions.pic_Imports.src = "images/accessallowedorange1.bmp"
          parent.SystemOptions.pic_Imports.title = "1"
    else
          parent.SystemOptions.pic_Imports.src = "images/accessdeniedorange1.bmp"
          parent.SystemOptions.pic_Imports.title = "0" 
	end if 
	
	sRes1 = "<%=Session("AT")%>"
	if sRes1 = "Allowed" then
          parent.SystemOptions.pic_AssetTransfer.src = "images/accessallowedorange1.bmp"
          parent.SystemOptions.pic_AssetTransfer.title = "1"
    else
          parent.SystemOptions.pic_AssetTransfer.src = "images/accessdeniedorange1.bmp"
          parent.SystemOptions.pic_AssetTransfer.title = "0" 
	end if 
	
	sRes1 = "<%=Session("RS")%>"
    if sRes1 = "Allowed" then
          parent.SystemOptions.pic_Reports.src = "images/accessallowedorange1.bmp"
          parent.SystemOptions.pic_Reports.title = "1"
    else
          parent.SystemOptions.pic_Reports.src = "images/accessdeniedorange1.bmp"
          parent.SystemOptions.pic_Reports.title = "0" 
	end if   
end Sub

Sub window_onload
	rs_open = false
    rs1_open = false
    rs2_open = false
    Flag = False
    b_updinProgress = True
    
    ' RSM: Sort out the grid ...
	ShowGroupAccessTable()

End Sub

Sub ShowGroupAccessTable()


	' RSM: Setup some defaults ...
	

     '**************************************************
     
    if rs_open = true  then
	   rs_topLevelFunctions.Close
	end if
    DataCombo_TopLevelFunctions.focus
    sSQL = "select FunctionNumber, FunctionShortName from [function] (nolock) where FunctionParent = 161 or FunctionNumber  = 161 order by FunctionSequence"
	
	
	rs_topLevelFunctions.CursorLocation = 3
	rs_topLevelFunctions.Open sSQL ,conn,adOpenStatic
	rs_topLevelFunctions.MoveFirst

	set DataCombo_TopLevelFunctions.RowSource = rs_topLevelFunctions
	DataCombo_TopLevelFunctions.ListField = rs_topLevelFunctions.Fields("FunctionShortName").name
	DataCombo_TopLevelFunctions.BoundColumn =  rs_topLevelFunctions.Fields("FunctionNumber").name
	DataCombo_TopLevelFunctions.BoundText =  rs_topLevelFunctions.Fields("FunctionNumber").value
	DataCombo_TopLevelFunctions.Refresh


     '********* GROUPS *******************************
    
    if rs_open = true  then
	   rs.Close
	end if
	window.status = "Please Wait... Fetching User Groups....."
    sSQL = "y_GetSortedUserGroup"

    rs.CursorLocation = 3
	rs.Open sSQL ,conn,adOpenStatic   
	
	rs_open  =true
	  
	'Reconfigure the Grid..

    Dim I 
    ' Create an additional split:
	Dim S
	Dim C
	Dim Cols

	Set S = TrueDBGrid.Splits.Add(0) 

	' Hide all columns in the leftmost split, Splits(0),
	' except for columns 0 and 1
	     
	Set Cols = TrueDBGrid.Splits.Item(0).Columns
	For Each C In Cols
		C.Visible = False
	Next

	Cols(0).Visible = True
	Cols(1).Visible = false

    Cols(0).Caption = "xxxxx"
    Cols(0).DataField = rs.Fields(0).name' 
    Cols(0).Width = 140
    
    
	' Configure Splits(0) to display exactly two columns, 
	' and disable resizing
	TrueDBGrid.Splits.Item(0).SizeMode = 2 ' dbgNumberOfColumns
	TrueDBGrid.Splits.Item(0).Size = 1
	TrueDBGrid.Splits.Item(0).AllowSizing = False
	TrueDBGrid.Splits.Item(0).HeadingStyle.WrapText = True
	
	
		
	' Usually, if you fix columns 0 and 1 from scrolling 
	' in a split, you will want to make them invisible in 
	' other splits:
	Set Cols = TrueDBGrid.Splits.Item(1).Columns
	Cols(0).Visible = False
	Cols(1).Visible = False
	

	'Create then necessary columns...
	
    rs.MoveFirst
    for I= 0 to rs.RecordCount  - 1 step 1
   
		'Create then necessary columns...
		set tmpCol =  TrueDBGrid.Splits.Item(1).Columns.Add(i)
		tmpCol.Width = 160
		tmpCol.Caption = rs.Fields("UserGroupShortName").Value
		'tmpCol.DataField = rs.Fields(0).name
		tmpCol.Alignment = 2
		tmpCol.ValueItems.Translate = true
		tmpCol.ValueItems.CycleOnClick = True
		
	    set Itm = CreateObject("TrueOleDBGrid60.ValueItem")
		Itm.Value = 0
		
		' RSM: Buggy; commented out
		' Now getting the images from the ocx file
        ' Itm.DisplayValue =  aPictureBox4.Image
		' Itm.DisplayValue = "Denied"
		
        set Itm.DisplayValue = oPictures.GetImageStream("DENIED")

		tmpCol.ValueItems.Add(Itm)
        
		Itm.Value = 1

		' RSM: Buggy; commented out 
		' Itm.DisplayValue = aPictureBox3.Image  ' "Granted"
		 Itm.DisplayValue = "Granted"
        set Itm.DisplayValue = oPictures.GetImageStream("GRANTED")
		tmpCol.ValueItems.Add(Itm)
        
		tmpCol.WrapText = True
		tmpCol.Visible = True
		rs.MoveNext
	
	next 
	
	TrueDBGrid.RowHeight = 34
	
	FetchGroupAccessRights
	
End Sub


Sub FetchGroupAccessRights	

	
	'********** FUNCTIONS ****************************************
	window.status = "Please Wait... Fetching Functions....."
	 if rs1_open = true  then
	   rs1.Close
	end if
	
  '  sSQL = "y_GetFunctionTree" 
  
	sSQL = "y_GetFunctionTreePerArea " & window.DataCombo_TopLevelFunctions.BoundText

    rs1.CursorLocation = 3
	rs1.Open sSQL ,conn,adOpenStatic

  rs1_open = true

   Dim i_Rows
   Dim i_Cols
   
   i_Rows = rs1.RecordCount 
   i_cols   = rs.RecordCount
   

   'Set Up arrays


   
'********** ACCESS *********************
' DataArray.Clear
  DArray.ReDim 1, i_Rows, 0, i_Cols
  KArray.ReDim 1, i_Rows, 0, i_Cols
  KArray2.ReDim 1, i_Rows  +3, 0, i_Cols
  
 ' msgbox i_Rows
  'msgbox i_Cols
  
   rs.MoveFirst
   Dim K 
   K=1
  for I= 0 to rs.RecordCount  - 1 step 1
   
   
     S = rs.Fields("UserGroupNumber").Value
     KArray.Value(1,K) = S
   
     rs.MoveNext
	 K = K + 1 
	 
  next 
  
  
  K = 1
  for I= 0 to rs1.RecordCount  - 1 step 1
   
   
     S = rs1.Fields("FunctionNumber").Value
     KArray.Value(K,0) = S
     S = rs1.Fields("FunctionShortName").Value
     DArray.Value(K,0) = S
   
     rs1.MoveNext
	 K = K + 1 
	 
  next 
  
  K = 1

  rs.MoveFirst
    for I= 0 to rs.RecordCount  - 1 step 1
   
   
     S = rs.Fields("UserGroupNumber").Value

 
     KArray.Value(1,K) = S
   
     rs.MoveNext
	 K = K + 1 
	 
  next 
  
  
    rs.MoveFirst
    K=1
    for I= 0 to rs.RecordCount  - 1 step 1
   
   
     S = rs.Fields("UserGroupNumber").Value

     on error resume next

     KArray2.Value(K,0) = S
   
     rs.MoveNext
	 K = K + 1 
	 
	next 
  
   KArray2.ReDim 1, K, 0, i_Cols
  
   if rs2_open = true  then
	   rs2.Close
	end if
	window.status = "Please Wait... Fetching Access data....."
    'sSQL = "y_GetAccessValues"
     sSQL = "y_GetAccessValuesPerArea " & window.DataCombo_TopLevelFunctions.BoundText

    rs2.CursorLocation = 3
	rs2.Open sSQL ,conn,adOpenStatic
  

	rs2_open = true
    'Initialize the entire data array to zero's
    window.status = "Please Wait... Initializing....."
    
    For X = 1 to i_Cols Step 1
      
		For Y = 1 to i_Rows Step 1
		     DArray.Value(Y,X) = 0   
        Next
        
    Next
    
    For X = 1 to i_Cols Step 1
      
		For Y = 1 to i_Rows Step 1
		     DArray.Value(Y,X) = 0   
        Next
        
    Next
   
    Set Cols = TrueDBGrid.Splits.Item(1).Columns
	Cols(0).Visible = False
	'Cols(1).Visible = False
	
    set tmpCol =  TrueDBGrid.Splits.Item(0).Columns.item(0)
    tmpCol.ValueItems.Translate = false
	tmpCol.ValueItems.CycleOnClick = false
    
    Set Cols = TrueDBGrid.Splits.Item(0).Columns
    Cols(0).Visible = True
    
	for I = 1 to rs.RecordCount  step 1
		Set Cols = TrueDBGrid.Splits.Item(0).Columns
		Cols(I).Visible = False
		
	next 
	
	
	'************* POPULATE ACCESS VALUES *******************************************
    Dim last
    last = 1
    window.ProgressBar1.Min  = 0
    window.ProgressBar1.Max = rs2.RecordCount - 1 
    window.ProgressBar1.Value = 0
	rs2.MoveFirst
	Set Cols = TrueDBGrid.Splits.Item(1).Columns

	For B = 0 to rs2.RecordCount - 1 Step 1
	     
	     iFunc  = rs2.Fields("FunctionNumber").Value
	     iGroup  = rs2.Fields("UserGroupNumber").Value
	   '  sFunc  = rs2.Fields("FunctionShortName").Value
	   '  sGroup = rs2.Fields("UserGroupShortName").Value
	    ' iAccess= rs2.Fields("GroupFunctionAccess").Value
	     

	 '    window.status = "Please Wait.... Fetching " & sFunc & " access rights for " & sGroup 
	     
	     window.ProgressBar1.Value = B
	    
	     
	     L = KArray2.Find(1,0,CStr(iGroup),2,1,9)
	     xOff = L - 1
	     	     
	     K = KArray.Find(1,0,CStr(iFunc),2,1,9)
	     yOff = K 
	     
	     if yOff > 0 then
			DArray.Value(yOff,xOff) = 1
	     end if
	     
	     rs2.MoveNext
	   
	     
	Next

	'**********************************************************
   
    Set TrueDBGrid.Array = DArray
    
    ' Force the grid to fetch data
    ' RSM: Uncommented this out to force it to get teh data from the DB
	TrueDBGrid.DataMode = 4

    TrueDBGrid.Splits.Item(1).HeadingStyle.wordwrap = true
    TrueDBGrid.HoldFields
    TrueDBGrid.ReBind
  
    window.ProgressBar1.style.visibility = "hidden"
  
     window.status = "Done....."
End Sub


Sub TrueDBGrid_RowColChange(LastRow,LastCol)



 sStyle.ForeColor =  16777215
 sStyle.BackColor = 16711680
 
 TrueDBGrid.Columns.Item(0).AddCellStyle 1 , sStyle
	
End Sub

Sub TrueDBGrid_AfterColUpdate(ColIndex)


Dim row
Dim col

if ColIndex = 0  then exit sub
' Msgbox TrueDBGrid.Bookmark
document.body.style.cursor = "hand"
TrueDBGrid.Enabled = false

v_BookMark = window.TrueDBGrid.Bookmark
col =  ColIndex  + 1
row = TrueDBGrid.Bookmark  + 1
'msgbox row
'Msgbox  KArray.Value(row,0)

'msgbox KArray.Value(row-1,0)
'msgbox KArray.Value(1,col)
'msgbox TrueDBGrid.Columns.item(ColIndex).text
'msgbox DArray.Value(row-1,0)

'iFunc = KArray.Value(row-1,0)
iFunc = KArray.Value(row,0)
iGroup = KArray.Value(1,col)
'iAccess = Cint(DArray.Value(row,col-1))
 'msgbox ColIndex



 if TrueDBGrid.Columns.item(ColIndex).text ="<Bitmap>.0" then
'if TrueDBGrid.Columns.item(ColIndex).text ="Denied" then
	' RSM : 2005-0106: Altered this - needs to be one based
    ' DArray.Value(row-1,col-1) = 0
	 DArray.Value(row ,col) = 0
     call UpdateGroupFunctionRecord(iFunc,iGroup,0)
 elseif TrueDBGrid.Columns.item(ColIndex).text ="<Bitmap>.1" then
' elseif TrueDBGrid.Columns.item(ColIndex).text ="Granted" then
	' RSM : 2005-0106: Altered this - needs to be one based
    ' DArray.Value(row-1,col-1) = 1
     DArray.Value(row,col) = 1
     call UpdateGroupFunctionRecord(iFunc,iGroup,1)
 end if

'DArray.Value(row,col-1) = iAccess


'if iAccess = 0 then
'   call UpdateGroupFunctionRecord(iFunc,iGroup,1)
'   DArray.Value(row,col-1) = 1
 
'elseif iAccess = 1 then
'  Call UpdateGroupFunctionRecord(iFunc,iGroup,0)
'  DArray.Value(row,col-1) = 0
'else
'  msgbox "Error updating table...."
'end if



'set tmpCol =  TrueDBGrid.Splits.Item(1).Columns.item(ColIndex)
 'msgbox tmpCol.ValueItems.Item(0).Value
 'TrueDBGrid.ReBind

window.focus


'SetSystemAccessLights
'SetSystemAccessLightsServer
'parent.SystemOptions.document()


'window.location("SystemOptions").reload
'parent.SystemOptions.reload

'msgbox "PP"

'Refresh System Options panel in case their access has been changed...
'msgbox trim(DArray.Value(row-1,0))

' RSM: The index is one-based! Code altered accordingly ...
'if trim(DArray.Value(row-1,0)) = "User Groups" or _
'   trim(DArray.Value(row-1,0)) = "Functions" or _
'   trim(DArray.Value(row-1,0)) = "System Users" or _
'   trim(DArray.Value(row-1,0)) = "Group Access" or _
'   trim(DArray.Value(row-1,0)) = "End of Period" or _
'   trim(DArray.Value(row-1,0)) = "Exports" or _
'   trim(DArray.Value(row-1,0)) = "Imports" or _
'   trim(DArray.Value(row-1,0)) = "Asset Transfer" or _
'   trim(DArray.Value(row-1,0)) = "System Reports" then
'   window.parent.SystemOptions.location.reload
'end if

if trim(DArray.Value(row,0)) = "User Groups" or _
   trim(DArray.Value(row,0)) = "Functions" or _
   trim(DArray.Value(row,0)) = "System Users" or _
   trim(DArray.Value(row,0)) = "Group Access" or _
   trim(DArray.Value(row,0)) = "End of Period" or _
   trim(DArray.Value(row,0)) = "Exports" or _
   trim(DArray.Value(row,0)) = "Imports" or _
   trim(DArray.Value(row,0)) = "Asset Transfer" or _
   trim(DArray.Value(row,0)) = "System Reports" then
   window.parent.SystemOptions.location.reload
end if

document.body.style.cursor = "default"
TrueDBGrid.Enabled = true
window.TrueDBGrid.focus

End Sub

Function UpdateGroupFunctionRecord (i_Func,i_Group,i_Access)

    

    'set conn = createobject("ADODB.Connection")
    'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
   ' sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [groupaccess.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
  '  conn.CursorLocation = 3
   ' conn.ConnectionTimeout = 20
'	conn.Open sDSN	
   
    'msgbox "y_UpdUserGroupAccess " & i_Func & "," & i_Group & "," & i_Access 
	conn.Execute "y_UpdUserGroupAccess " & i_Func & "," & i_Group & "," & i_Access 
    'conn.close
   '
     b_updinProgress = false

End Function


Sub window_onunload
window.status = "Ready...."
End Sub

Sub DataCombo_TopLevelFunctions_Change

    if rs1_open = true and rs2_open = true then
		'ShowGroupAccessTable
		FetchGroupAccessRights
	end if

End Sub

-->
</script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">


<body  bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" class=Generic>
<P>
<OBJECT id=TrueDBGrid 
style="Z-INDEX: 100; LEFT: 0px; WIDTH: 860px; POSITION: absolute; TOP: 34px; HEIGHT: 485px" 
codeBase="" height=485 width=860 
data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAYAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAHAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwABEAAAAAxAAAAKP06BEC8ZgUBAAAACAAAAB0AAABIZWFkaW5nAGEDAABgAAAASCM6BEgjOgQPAAAAFwAAAB4AAABGb290aW5nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB8AAABTZWxlY3RlZAAAAAAGAACAAQAAAAAAAAAAAAAAAAAAACAAAABDYXB0aW9uAPC9ZgWAvWYFEL1mBUC8ZgXAumYFILpmBSEAAABIaWdobGlnaHRSb3cA3mYFQN5mBdDdZgVg3WYF8NxmBSIAAABFdmVuUm93AAAAAAAAAAAAAAAAAAAAAAAAAAAAWAMAACMAAABPZGRSb3cABQoAAAAKAAAAAQAAAAK8+5vx7xoQhO0AqiQAAAALAAAA//8AAAMAAAAAAAAACwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAAAAAAADAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAA//8AAAsAAAAAAAAAAwAAAAEAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAAeAAAAMgAAAERyYWcgYSBjb2x1bW4gaGVhZGVyIGhlcmUgdG8gZ3JvdXAgYnkgdGhhdCBjb2x1bW4AAAADAAAAAAAAAAMAAAAAAAAAAwAAAMgAAAADAAAAAAAAAAMAAADs6dgAAwAAAJDQAwA9AAAAAAAAAAsAAABUcnVlREJHcmlkAAIAAAAMAAAAQWxsb3dBZGROZXcALwAAAAwAAABBbGxvd0Fycm93cwABAAAADAAAAEFsbG93RGVsZXRlAAQAAAAMAAAAQWxsb3dVcGRhdGUAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQAI/v//DAAAAEJvcmRlclN0eWxlAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAnAAAAAsAAABEYXRhTWVtYmVyACUAAAAJAAAARGF0YU1vZGUAuwAAAAkAAABEYXRhVmlldwDEAAAAEgAAAERlYWRBcmVhQmFja0NvbG9yAAoAAAAMAAAARGVmQ29sV2lkdGgAUAAAAA0AAABFZGl0RHJvcERvd24AXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkADAAAAAPAAAARXhwb3NlQ2VsbE1vZGUAkQAAAAoAAABGb290TGluZXMAwgAAAA8AAABHcm91cEJ5Q2FwdGlvbgAMAAAACgAAAEhlYWRMaW5lcwCYAAAACwAAAEluc2VydE1vZGUAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAsQAAAAoAAABMYXlvdXRVUkwASgAAAA4AAABNYXJxdWVlVW5pcXVlAM4AAAAIAAAATWF4Um93cwCjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgCEAAAADAAAAE11bHRpU2VsZWN0AGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUAlwAAABEAAABQaWN0dXJlQWRkbmV3Um93AJUAAAASAAAAUGljdHVyZUN1cnJlbnRSb3cAswAAABEAAABQaWN0dXJlRm9vdGVyUm93ALIAAAARAAAAUGljdHVyZUhlYWRlclJvdwCWAAAAEwAAAFBpY3R1cmVNb2RpZmllZFJvdwCwAAAAEwAAAFBpY3R1cmVTdGFuZGFyZFJvdwC0AAAACwAAAFByaW50SW5mb3MADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAIwAAAAcAAABTcGxpdHMAMQAAABAAAABUYWJBY3Jvc3NTcGxpdHMAMgAAAAoAAABUYWJBY3Rpb24AmQAAABcAAABUcmFuc3BhcmVudFJvd1BpY3R1cmVzADMAAAAQAAAAV3JhcENlbGxQb2ludGVyANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA 
classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0 ></OBJECT>
<OBJECT id=ProgressBar1 
style="Z-INDEX: 101; LEFT: 4px; WIDTH: 825px; POSITION: absolute; TOP: 520px; HEIGHT: 26px" 
codeBase=OCX/mscomctl.ocx classid=clsid:0713E8D2-850A-101B-AFC0-4210102A8DA7><PARAM NAME="_ExtentX" VALUE="21828"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="327682"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Enabled" VALUE="1"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="Min" VALUE="0"><PARAM NAME="Max" VALUE="100"></OBJECT>


</P>
<OBJECT id=DArray 
style="Z-INDEX: 104; LEFT: 16px; POSITION: absolute; TOP: 577px" width=1 
classid=clsid:0D62353B-DBA2-11D1-B5DF-0060976089D0></OBJECT>
<OBJECT id=KArray 
style="Z-INDEX: 105; LEFT: 35px; POSITION: absolute; TOP: 578px" 
classid=clsid:0D62353B-DBA2-11D1-B5DF-0060976089D0></OBJECT>
<OBJECT id=KArray2 
style="Z-INDEX: 106; LEFT: 58px; POSITION: absolute; TOP: 578px" 
classid=clsid:0D62353B-DBA2-11D1-B5DF-0060976089D0></OBJECT>
<TABLE class=Table1 id=tbl_DayFilter 
style="FONT-SIZE: 12px; Z-INDEX: 147; LEFT: 3px; WIDTH: 101px; FONT-FAMILY: MS Sans Serif; POSITION: absolute; TOP: 1px; HEIGHT: 28px" 
cellSpacing=1 cellPadding=1 width=101 border=1>
  
  <TR>
    <TD noWrap>
      <P>&nbsp;
      <OBJECT id=DataCombo_TopLevelFunctions 
      style="LEFT: 1px; WIDTH: 216px; TOP: 5px; HEIGHT: 21px" tabIndex=1 
      height=21 width=216 classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 
      name=DataCombo_TopLevelFunctions><PARAM NAME="_ExtentX" VALUE="5715"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="0"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</P></TD></TR></TABLE>

</body>
</html>
