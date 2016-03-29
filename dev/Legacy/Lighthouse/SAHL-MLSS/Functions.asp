<%
  sDatabase =Session("SQLDatabase") 
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_AddFunction = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Add Function",Session("UserName"))
  i_UpdFunction = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Update Function",Session("UserName"))
  i_DelFunction = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Delete Function",Session("UserName"))
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
dim v_BookMark
dim rs_open
dim rs_open1
Dim i_tmp1

Dim b_Loaded 



if rs_open <> true then
  
     'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"
    
	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   return
    end if
    
	set conn = createobject("ADODB.Connection")
	set rs  = createobject("ADODB.Recordset")
	set rs1 = createobject("ADODB.Recordset")


	'"DSN=SAHLS01;uid=sa"	 

	'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [Functions.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"

	conn.Open sDSN
	rs_open = false

end if

Sub GetFunctionRecords

     if rs_open = true  then
	   rs.Close
	end if
	
    sSQL = "y_GetFunctionTree"

    rs.CursorLocation = 3
	rs.Open sSQL ,conn,adOpenStatic
		

End Sub

Sub window_onload
     
   
    
    window.Sibling.status = true
    window.Child.status = false
    b_Loaded = false
    'SetAccessLights
    SetAccessLightsServer
    call GetFunctionRecords()
    window.focus
   
    FunctionTree.Refresh
	PopulateTree
	
	'myImageList.ListImages.Add  1 ,,window.Image1
	'myImageList.ListImages.Add  2 ,,window.Image2.src
	Functiontree.Nodes(2).Expanded  = false
	window.focus
	DisableFields

	if rs.RecordCount > 0 then
	  rs.MoveFirst
	  window.FunctionShortName.Text = rs.Fields("FunctionShortName").Value
	  window.FunctionDescription.Text = rs.Fields("FunctionDescription").Value
	  window.FunctionTree.focus()
	end if

	'test
End Sub

Function PopulateTree
Dim i 
Dim sCurParent 
Dim sPrevParent 
Dim sKey
Dim iKey
Dim iTotRecords 

Dim bParent 
Dim nodx


'On Error GoTo Error1:
i=0
bParent = false
  
  '  call GetFunctionRecords()

 

  iTotRecords = rs.RecordCount
   rs.MoveFirst

 
   
    For i = 1 To iTotRecords Step 1
         If i > iTotRecords Then Exit For
            
        If rs.Fields("FunctionParent") = -1 and bParent = false Then
              bParent = true
             sKey = "'" & CInt(rs.Fields("FunctionNumber")) & "'"             
             sRootKey = sKey
             Set nodX = FunctionTree.Nodes.Add(, , sKey, rs.Fields("FunctionShortName"))
              nodX.EnsureVisible
              sCurParent = "'" & CInt(rs.Fields("FunctionNumber")) & "'"
         ElseIf ("'" & CStr(rs.Fields("FunctionParent")) & "'") <> sCurParent Then
             sPrevParent = sCurParent
             sCurParent = "'" & CInt(rs.Fields("FunctionParent")) & "'"
         Else
            '  sPrevParent = sCurParent
           '  sCurParent = "'" & CInt(rs.Fields("FunctionParent")) & "'"
             '  iCurrentParent = rs.Fields("FunctionParent")
         End If
 
        If rs.Fields("FunctionParent") <> -1 and bParent =true Then
             If sPrevParent = "" Then
                  sKey = "'" & CInt(rs.Fields("FunctionNumber")) & "'"
                  Set nodX = FunctionTree.Nodes.Add(sCurParent, 4, sKey, rs.Fields("FunctionShortName"))
             ElseIf ("'" & CInt(rs.Fields("FunctionNumber")) & "'") <> sPrevKey then
                  
                  sKey = "'" & CInt(rs.Fields("FunctionNumber")) & "'"
                   sPrevKey =  sKey
                 Set nodX = FunctionTree.Nodes.Add(sCurParent, 4, sKey, rs.Fields("FunctionShortName"))
             ElseIf ("'" & CInt(rs.Fields("FunctionNumber")) & "'") <> sPrevKey then
                  
                  sKey = "'" & CInt(rs.Fields("FunctionNumber")) & "'"
                   sPrevKey =  sKey
             End If
        End If

         rs.MoveNext
       
     
    Next 
    
    FunctionTree.Refresh
    bNodYSet = False
    nodX.EnsureVisible
   
bTreeShown = True
End Function



Sub FunctionTree_NodeClick(ByVal Node)

	rs.MoveFirst

	sKey   = Replace(Node.Key,"'","",1,10,1)

	rs.Find "FunctionNumber = " & sKey

	window.FunctionShortName.Text = trim(rs.Fields("FunctionShortName").Value)
	window.FunctionDescription.Text = trim(rs.Fields("FunctionDescription").Value)


End Sub

Sub DisableFields

	window.FunctionShortName.Enabled = false
	window.FunctionDescription.Enabled = false

End Sub

Sub EnableFields

	window.FunctionShortName.Enabled = true
	window.FunctionDescription.Enabled = true

End Sub

Sub ClearFields

	window.FunctionShortName.Text = ""
	window.FunctionDescription.Text =  ""

End Sub


Function DisableControls(s_Action)
  
   window.FunctionTree.Enabled = false
     
  if s_Action =  "Add" then
     btn_AddFunction.disabled = false
	 btn_UpdFunction.disabled = true
     btn_DelFunction.disabled = true
     window.btn_UpdFunction.style.visibility = "hidden"
     window.btn_DelFunction.style.visibility = "hidden"
     window.pic_UpdateFunction.style.visibility = "hidden"
     window.pic_DelFunction.style.visibility = "hidden"
     
  Elseif s_Action = "Update" then
     btn_UpdFunction.disabled = false
	 btn_AddFunction.disabled = true
     btn_DelFunction.disabled = true   
     window.btn_DelFunction.style.visibility = "hidden"
     window.btn_AddFunction.style.visibility = "hidden"
     window.pic_AddFunction.style.visibility = "hidden"
     window.pic_DelFunction.style.visibility = "hidden"
      
  Elseif s_Action = "Delete" then
     btn_DelFunction.disabled = false
	 btn_AddFunction.disabled = true
     btn_UpdFunction.disabled = true    
     window.btn_UpdFunction.style.visibility = "hidden"
     window.btn_AddFunction.style.visibility = "hidden"
     window.pic_UpdateFunction.style.visibility = "hidden"
     window.pic_AddFunction.style.visibility = "hidden"
     
  end if
     
End Function

Function EnableControls()
  
     window.FunctionTree.Enabled = true
     btn_AddFunction.disabled = false
     btn_UpdFunction.disabled = false
     btn_DelFunction.disabled = false
     
     window.btn_AddFunction.style.visibility = "visible"
     window.btn_UpdFunction.style.visibility = "visible"
     window.btn_DelFunction.style.visibility = "visible"
     window.pic_AddFunction.style.visibility = "visible"
     window.pic_UpdateFunction.style.visibility = "visible"
     window.pic_DelFunction.style.visibility = "visible"
     
End Function

Sub SetActionButtons

  if rs.RecordCount > 0 then
     window.btn_UpdFunction.disabled = false
     window.btn_DelFunction.disabled = false
  else
     window.btn_UpdFunction.disabled = true
     window.btn_DelFunction.disabled = true
  end if

End sub

Sub SetAccessLightsServer
     
     
    sRes1 = "<%=i_AddFunction%>"
    if sRes1 = "Allowed" then
       window.pic_AddFunction.src = "images/MLSAllowed.bmp"
       window.pic_AddFunction.title = "1"
    else
       window.pic_AddFunction.src = "images/MLSDenied.bmp"
       window.pic_AddFunction.title = "0"
	end if 
	
	 sRes1 = "<%=i_UpdFunction%>"
    if sRes1 = "Allowed" then
       window.pic_UpdateFunction.src = "images/MLSAllowed.bmp"
       window.pic_UpdateFunction.title = "1"
    else
       window.pic_UpdateFunction.src = "images/MLSDenied.bmp"
       window.pic_UpdateFunction.title = "0"
	end if
	
	 sRes1 = "<%=i_DelFunction%>"
    if sRes1 = "Allowed" then
       window.pic_DelFunction.src = "images/MLSAllowed.bmp"
       window.pic_DelFunction.title = "1"
    else
       window.pic_DelFunction.src = "images/MLSDenied.bmp"
       window.pic_DelFunction.title = "0"
	end if

	  
		
end Sub

Sub SetAccessLights

    sUserName = "<%= Session("UserID")%>"

	set x = CreateObject("MLSSecurity.FunctionClass")
    sRes1 = x.CheckFunctionAccess("DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>",window.btn_AddFunction.title,Trim(sUserName))
     
    if sRes1 = "Allowed" then
          window.pic_AddFunction.src = "images/MLSAllowed.bmp"
          window.pic_AddFunction.title = "1"
	end if   
	
	sRes1 = x.CheckFunctionAccess("DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>",window.btn_UpdFunction.title,Trim(sUserName))
     
    if sRes1 = "Allowed" then
          window.pic_UpdateFunction.src = "images/MLSAllowed.bmp"
          window.pic_UpdateFunction.title = "1"
	end if  
    

    sRes1 = x.CheckFunctionAccess("DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>",window.btn_DelFunction.title,Trim(sUserName))

    if sRes1 = "Allowed" then
          window.pic_DelFunction.src = "images/MLSAllowed.bmp"
          window.pic_DelFunction.title = "1"
	end if  
	

End Sub 

Sub btn_CancelFunctionAction_onclick
   	
   	ClearFields

   	EnableControls

   	if rs.RecordCount > 0 then

		rs.Requery

		rs.Bookmark = v_BookMark
		window.FunctionShortName.Text = rs.Fields("FunctionShortName").Value
		window.FunctionDescription.Text = rs.Fields("FunctionDescription").Value

	end if

	btn_AddFunction.value = "Add"
	btn_UpdFunction.value = "Update"
	btn_DelFunction.value = "Delete"
	btn_CancelFunctionAction.style.visibility = "hidden"
	OptionTable.style.visibility = "visible"
  
  
	EnableControls
	DisableFields
    window.FunctionTree.focus()
  
End Sub

Sub btn_AddFunction_onclick

  if window.pic_AddFunction.title = "0" then
      window.status = "Access denied to " & window.btn_AddFunction.title
      exit sub
    end if

  v_BookMark = rs.Bookmark

  if btn_AddFunction.value = "Add" then

    ClearFields
     'Disable other controls...
	DisableControls("Add")
	'Enable Fields
	EnableFields
	btn_CancelFunctionAction.style.visibility = "visible"


	'Set Focus 
    'Set the button caption..
	btn_AddFunction.value = "Commit"  
	  

	window.FunctionShortName.HighlightText = true
	window.FunctionShortName.focus()
	
  
  elseif btn_AddFunction.value = "Commit" then

    call AddFunctionRecord(trim(window.FunctionShortName.Text),trim(window.FunctionDescription.text))
	'Clean up...

	btn_AddFunction.value = "Add"
	btn_CancelFunctionAction.style.visibility = "hidden"

	EnableControls

	DisableFields()

	SetActionButtons
	 FunctionTree.focus()
    Set nodX = FunctionTree.SelectedItem
    nodX.Selected = True
    FunctionTree_NodeClick nodX
    
	End if
End Sub

Function AddFunctionRecord(sName,sDesc)
dim i_tmp

    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")
   'Cannot use OLE DB Provider as it appears that it does not return a recordset
    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS [Functions.asp 2];uid=<%= Session("UserID")%>"
'    sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [function.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"

	
	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc
	


	If FunctionTree.Nodes.Count <= 0 Then 'Create the root node...
	      ' Set Treeview control properties.
	      FunctionTree.LineStyle = 1  ' Linestyle 1   ' Add Node objects.
	     ' First node with 'Root' as text.
	      Set nodX = FunctionTree.Nodes.Add(, , "r", window.FunctionDescription.Text)
	      
	      com.CommandText = "y_AddFunctionRecord"
	      set prm = com.CreateParameter ( "FunctionShortName",200,1,40,sName) ' AdUnsigned Int
		  com.Parameters.Append prm
		  set prm = com.CreateParameter ( "FunctionShortDescription",200,1,80,sDesc) 'AdVarchar , adParamInput
		  com.Parameters.Append prm
	      set prm = com.CreateParameter ( "FunctionParent",19,1,,-1)
		  com.Parameters.Append prm
	      set prm = com.CreateParameter ( "FunctionSequence",19,1,,0)
		  com.Parameters.Append prm
		  set rs_temp = com.Execute 
		  
		   i_tmp = CInt(rs_temp.Fields(0).Value)
	 Else

	      Set nodX = FunctionTree.SelectedItem
	      Set nodY = nodX
	      bNodYSet = True

	      strNodeText = nodX.Text ' & vbLf
	      
	      If nodX.Root.Text = strNodeText And FunctionTree.Nodes.Count > 0 And  window.Child.status = false Then         'We are the  root..
	         Exit Function
	      Else  
	         If window.Child.status=true Then
	            sKey = nodX.Key
	         Else
	            sKey = nodX.Parent.Key
	         End If
	         iParent   = Cint(Replace(sKey,"'","",1,10,1))
	      End If
	      
	      sKey = nodX.Key
	            
		   iKey   = Cint(Replace(sKey,"'","",1,10,1))

		  'iSeq = GetFunctionSequence(iKey)
		 ' msgbox iSeq
		    iSeq =  rs.Fields("FunctionSequence")

			com.CommandText = "y_AddFunctionRecord"
			set prm = com.CreateParameter ( "FunctionShortName",200,1,40,sName) ' AdUnsigned Int
			com.Parameters.Append prm
			set prm = com.CreateParameter ( "FunctionShortDescription",200,1,80,sDesc) 'AdVarchar , adParamInput
			com.Parameters.Append prm
			set prm = com.CreateParameter ( "FunctionParent",19,1,,iParent)
			com.Parameters.Append prm
			set prm = com.CreateParameter ( "FunctionSequence",19,1,,iSeq)
			com.Parameters.Append prm
			set rs_temp = com.Execute 

	        If  CInt(rs_temp.Fields(0).Value) < 0 Then
	           window.FunctionShortName.Enabled = True
	           window.FunctionDescription.Enabled = True
	           Exit Function
	        End If
	        
	        i_tmp = CInt(rs_temp.Fields(0).Value)
	        
			sParent = "'" & CStr(iParent) & "'"
			sKey = "'" & CStr( rs_temp.Fields(0).Value) & "'"
			Dim iC
			Dim iP 
			If iKey > 0 Then
				Set nodX = FunctionTree.Nodes.Add(sParent, 4, sKey, window.FunctionDescription.Text)
				iC = nodX.Index
				iP = nodX.Parent.Index
			End If 
			Set nodY = nodX
			bNodYSet = True		           
			nodX.EnsureVisible
			FunctionTree.Nodes.Clear

			rs.Requery

			PopulateTree()
			'Reposition to Added node
			for i = 1 to FunctionTree.Nodes.Count step 1
				  ' msgbox i
				    set nodX = FunctionTree.Nodes(i)
				    sKey = nodX.Key
				     iKey   = Cint(Replace(sKey,"'","",1,10,1))
				     
				    if iKey = CInt(i_tmp) then
				    
				        nodX.Selected = true
				        
						exit for
				    end if
			next

	 End If
   
 


End Function

Function GetFunctionSequence(i_Val)



    sSQL = "SELECT FunctionSequence FROM [Function] (nolock) WHERE FunctionNumber = "  & i_Val
	'msgbox sSQL
    rs1.CursorLocation = 3
	rs1.Open sSQL ,conn,adOpenStatic

	GetFunctionSequence = rs1.Fields(0)
	rs1.close

End Function

Sub Child_onclick

    window.Sibling.status = false
    window.Child.status = true

End Sub

Sub Sibling_onclick

    window.Sibling.status = true
    window.Child.status = false
End Sub

Sub btn_UpdFunction_onclick

 if window.pic_UpdateFunction.title = "0" then
      window.status = "Access denied to " & window.btn_UpdFunction.title
      exit sub
    end if

  v_BookMark = rs.Bookmark

  if btn_UpdFunction.value = "Update" then


    'Disable other controls...
	DisableControls("Update")

	'Enable Fields
	EnableFields
	btn_CancelFunctionAction.style.visibility = "visible"
	OptionTable.style.visibility = "hidden"
	
	'Set Focus 
    'Set the button caption..
	btn_UpdFunction.value = "Commit"  

	window.FunctionShortName.HighlightText = true
	window.FunctionShortName.focus()
	
  
  elseif btn_UpdFunction.value = "Commit" then

    i_tmp = rs.Fields("FunctionNumber").Value
        'msgbox trim(rs.Fields("FunctionNumber"))
        'msgbox trim(window.FunctionShortName.Text)
        'msgbox trim(window.FunctionDescription.text)
  	call UpdateFunctionRecord(trim(rs.Fields("FunctionNumber")),trim(window.FunctionShortName.Text),trim(window.FunctionDescription.text))
	'Clean up...

	btn_UpdFunction.value = "Update"
	btn_CancelFunctionAction.style.visibility = "hidden"
	window.OptionTable.style.visibility = "visible"
	EnableControls
	DisableFields()

	SetActionButtons
	
	FunctionTree.Nodes.Clear
	
	PopulateTree()
	
	'Reposition to updated node...........
	for i = 1 to FunctionTree.Nodes.Count step 1
	  ' msgbox i
	    set nodX = FunctionTree.Nodes(i)
	    sKey = nodX.Key
	     iKey   = Cint(Replace(sKey,"'","",1,10,1))

	    if iKey = CInt(i_tmp) then	    
	       nodX.Selected = true
			exit for
	    end if
	next

	FunctionTree.focus()
	
	rs.Bookmark = v_BookMark
	
	
	End if
End Sub

Sub btn_DelFunction_onclick


i_CrLf =  Chr(13) & Chr(10) 

  if window.pic_DelFunction.title = "0" then
      window.status = "Access denied to " & window.btn_DelFunction.title
      exit sub
    end if

  v_BookMark = rs.Bookmark

  if btn_DelFunction.value = "Delete" then

    rs.MovePrevious
    if rs.BOF = true then
		rs.MoveFirst
    end if
    i_tmp1 = rs.Fields("FunctionNumber").Value 
    rs.MoveNext

     'Disable other controls...
	DisableControls("Delete")
	'Enable Fields

	btn_CancelFunctionAction.style.visibility = "visible"
	OptionTable.style.visibility = "hidden"


	'Set Focus 
    'Set the button caption..
	btn_DelFunction.value = "Commit"  

  
  elseif btn_DelFunction.value = "Commit" then
   
      

     set nodX = FunctionTree.SelectedItem
	 iChildren = nodX.Children
     if iChildren > 0 then
       i_Resp = MsgBox("Deleting " & Trim(rs.Fields("FunctionDescription").Value) & " will also delete its children..."  & i_CrLf & "Are you sure you want to do this...???? " , 4)
       if i_Resp= 7 then
         btn_CancelFunctionAction_onclick
         exit sub
       end if 
     end if
     
     i_Resp = MsgBox("Deleting " & Trim(rs.Fields("FunctionDescription").Value) & " will also delete all associated access rights..."  & i_CrLf & "Are you sure you want to do this...???? " , 4)
       if i_Resp= 7 then
         btn_CancelFunctionAction_onclick
         exit sub
       end if

    
     
    call DeleteFunctionRecord(rs.Fields("FunctionNumber").Value)
	'Clean up...

	btn_DelFunction.value = "Delete"
	btn_CancelFunctionAction.style.visibility = "hidden"
    window.OptionTable.style.visibility = "visible"
	EnableControls
   
	SetActionButtons

    FunctionTree.Nodes.Clear
	
	PopulateTree()
	
	'Reposition to previous node...........
	for i = 1 to FunctionTree.Nodes.Count step 1
	    set nodX = FunctionTree.Nodes(i)
	    sKey = nodX.Key
	    iKey   = Cint(Replace(sKey,"'","",1,10,1))
	    if iKey >= CInt(i_tmp1)  then      
	        nodX.Selected = true
	        window.FunctionTree.focus()
	       Set nodX = FunctionTree.SelectedItem
           nodX.Selected = True
            FunctionTree_NodeClick nodX
			exit for
	    end if
	next
		 
	if rs.RecordCount > 0 then
	  rs.MoveFirst
	  rs.Find "FunctionNumber >= " & Cint( i_tmp1)
	  window.FunctionShortName.Text = rs.Fields("FunctionShortName").Value
	  window.FunctionDescription.Text = rs.Fields("FunctionDescription").Value

	end if



  
	End if

End Sub

Function UpdateFunctionRecord(i_Number,sName,sDesc)
Dim conn
    

    set conn = createobject("ADODB.Connection")
    'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [Functions.asp 3]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    conn.CursorLocation = 1
    
	conn.Open sDSN	

	conn.Execute "y_UpdFunctionRecord " & i_Number & ",'" & sName & "','" & sDesc & "'"
    conn.close
   
    rs.Requery 
    
End Function


Function DeleteFunctionRecord(i_Number)
Dim conn
    

    set conn = createobject("ADODB.Connection")
    'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [Functions.asp 4]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    conn.CursorLocation = 1
    
	conn.Open sDSN	

	conn.Execute "y_DelFunctionRecord " & i_Number
    conn.close
   
    rs.Requery 
    
End Function

Sub lbl_Sibling_onclick
window.Sibling.click
End Sub

Sub lbl_Child_onclick
window.Child.click
End Sub

Sub window_onfocus

if b_Loaded = true then exit sub

if rs.RecordCount > 0 then
	  rs.MoveFirst
	  window.FunctionShortName.Text = rs.Fields("FunctionShortName").Value
	  window.FunctionDescription.Text = rs.Fields("FunctionDescription").Value
	  window.FunctionTree.focus()
end if
b_Loaded = true
End Sub

-->
</script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<body class=Generic>

<p>
<OBJECT id=FunctionTree 
style="Z-INDEX: 100; LEFT: 2px; WIDTH: 415px; POSITION: absolute; TOP: 22px; HEIGHT: 521px; BACKGROUND-COLOR: silver" 
 width=343 
classid=clsid:C74190B6-8589-11D1-B16A-00C0F0283628 VIEWASTEXT><PARAM NAME="_ExtentX" VALUE="10980"><PARAM NAME="_ExtentY" VALUE="13785"><PARAM NAME="_Version" VALUE="327682"><PARAM NAME="HideSelection" VALUE="0"><PARAM NAME="Indentation" VALUE="1005"><PARAM NAME="LabelEdit" VALUE="1"><PARAM NAME="LineStyle" VALUE="1"><PARAM NAME="PathSeparator" VALUE="\"><PARAM NAME="Sorted" VALUE="0"><PARAM NAME="Style" VALUE="7"><PARAM NAME="ImageList" VALUE="myImageList"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Enabled" VALUE="1"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
<table border="0" cellPadding="1" cellSpacing="1" style="Z-INDEX:  101; LEFT: 450px; WIDTH: 398px; POSITION: absolute; TOP: 
19px; HEIGHT: 93px" width   ="75%" class=Table1>
  
  <tr>
    <td align="right" noWrap>Short Name</td>
    <td noWrap>
      <OBJECT id=FunctionShortName 
      style="LEFT: 1px; WIDTH: 235px; TOP: 1px; HEIGHT: 26px" tabIndex=1 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6218"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap>Description</td>
    <td noWrap>
      <OBJECT id=FunctionDescription 
      style="LEFT: 1px; WIDTH: 316px; TOP: 1px; HEIGHT: 26px" tabIndex=2 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="8361"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr></table><input id="btn_AddFunction" name="btn_AddFunction" style="Z-INDEX: 105;  LEFT: 600px; WIDTH: 132px; CURSOR: hand; POSITION: absolute; TOP: 226px; HEIGHT: 48px" title  ="Add Function" type="button" value="Add" height="50" class=button3><input id="btn_UpdFunction" name="btn_UpdFunction" style="Z-INDEX: 110;  LEFT: 600px; WIDTH: 132px; CURSOR: hand; POSITION: absolute; TOP: 277px; HEIGHT: 48px" title  ="Update Function" type="button" value="Update" height="48" class=button3><input id="btn_DelFunction" name="btn_DelFunction" style="Z-INDEX: 112;  LEFT: 600px; WIDTH: 132px; CURSOR: hand; POSITION: absolute; TOP: 328px; HEIGHT: 48px" title  ="Delete Function" type="button" value="Delete" height="48" width="132" class=button3><input id="btn_CancelFunctionAction" name="btn_CancelFunctionAction" style="Z-INDEX: 103;  LEFT: 600px; VISIBILITY: hidden; WIDTH: 132px; CURSOR: hand; POSITION: absolute; TOP: 379px; HEIGHT: 48px" type  ="button" value="Cancel" height="48" width="132" class=button3><IMG id =pic_AddFunction title=0 style="Z-INDEX: 108;                          LEFT: 610px; WIDTH: 19px; POSITION: absolute; TOP: 236px; HEIGHT: 23px" height     =23 alt="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 ><IMG id =pic_UpdateFunction title=0 style="Z-INDEX: 113;                          LEFT: 610px; WIDTH: 19px; POSITION: absolute; TOP: 286px; HEIGHT: 23px" height     =23 alt="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 ><IMG id =pic_DelFunction title=0 style="Z-INDEX: 117;                          LEFT: 610px; WIDTH: 19px; POSITION: absolute; TOP: 337px; HEIGHT: 23px" height     =23 alt="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 >&nbsp; 
<table border="0" cellPadding="1" cellSpacing="1" style="Z-INDEX: 115;  LEFT: 605px; WIDTH: 120px; POSITION: absolute; TOP: 140px; 
HEIGHT: 47px" width  ="75%" id="OptionTable" background="" class=Table1>
  
  <tr>
    <td noWrap><input id="Sibling" name="radio1" type="radio" CHECKED></td>
    <td noWrap id="lbl_Sibling">Sibling&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td></tr>
  <tr>
    <td noWrap><input id="Child" name="radio2" type="radio" value="1"></td>
    <td noWrap id="lbl_Child">Child</td></tr></table><IMG 
id=Image1 style="Z-INDEX: 119; 
        
LEFT: 37px; POSITION: absolute; TOP: 716px" alt   ="" src="images/sahlsmall.ico" ><IMG id=Image2 style="Z-INDEX: 120; 
        
        LEFT: 93px; VISIBILITY: visible; POSITION: absolute; TOP: 728px" alt    ="" src="images/60-60-60.bmp" name=Image2 > 

</p>



</body>
</html>
