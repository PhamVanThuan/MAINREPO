<%
  sDatabase =Session("SQLDatabase")
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_AddUserGroup = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Add User Group",Session("UserName"))
  i_UpdUserGroup = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Update User Group",Session("UserName"))
  i_DelUserGroup = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Delete User Group",Session("UserName"))
%>
<html>
<head>
    <!--#include virtual="/SAHL-MLSS/miscutils.inc"-->
    <object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
        <param name="LPKPath" value="APEX.lpk">
    </object>
    <meta name="VI60_defaultClientScript" content="VBScript">
    <meta name="GENERATOR" content="Microsoft Visual Studio 6.0">
    <script id="clientEventHandlersVBS" language="vbscript">
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
	   window.location.href = "Default.asp"
	   return
    end if

	set conn = createobject("ADODB.Connection")
	set rs  = createobject("ADODB.Recordset")
	set rs1 = createobject("ADODB.Recordset")

	sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [usergroup.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"

	conn.Open sDSN
	rs_open = false

end if

Sub GetUserGroupRecords

     if rs_open = true  then
	   rs.Close
	end if

    sSQL = "y_GetSortedUserGroup"

    rs.CursorLocation = 3
	rs.Open sSQL ,conn,adOpenStatic

End Sub

Sub window_onload
    b_Loaded = false
    window.Sibling.status = true
    window.Child.status = false

    SetAccessLightsServer

    call GetUserGroupRecords()

    UserGroupTree.Refresh
	PopulateTree
	DisableFields

	if rs.RecordCount > 0 then
	  rs.MoveFirst
	  window.UserGroupShortName.Text = rs.Fields("UserGroupShortName").Value
	  window.UserGroupDescription.Text = rs.Fields("UserGroupDescription").Value
	  window.UserGroupTree.focus()
	end if

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

    i=0
    bParent = false

    iTotRecords = rs.RecordCount
    rs.MoveFirst

    For i = 1 To iTotRecords Step 1
         If i > iTotRecords Then Exit For

        If rs.Fields("UserGroupParent") = -1 and bParent = false Then
              bParent = true
             sKey = "'" & CInt(rs.Fields("UserGroupNumber")) & "'"
             sRootKey = sKey
             Set nodX = UserGroupTree.Nodes.Add(, , sKey, rs.Fields("UserGroupDescription"))
              nodX.EnsureVisible
              sCurParent = "'" & CInt(rs.Fields("UserGroupNumber")) & "'"
         ElseIf ("'" & CStr(rs.Fields("UserGroupParent")) & "'") <> sCurParent Then
             sPrevParent = sCurParent
             sCurParent = "'" & CInt(rs.Fields("UserGroupParent")) & "'"
         End If

        If rs.Fields("UserGroupParent") <> -1 and bParent =true Then
             If sPrevParent = "" Then
                  sKey = "'" & CInt(rs.Fields("UserGroupNumber")) & "'"
                  Set nodX = UserGroupTree.Nodes.Add(sCurParent, 4, sKey, rs.Fields("UserGroupDescription"))
             Else
                  sKey = "'" & CInt(rs.Fields("UserGroupNumber")) & "'"
                 Set nodX = UserGroupTree.Nodes.Add(sCurParent, 4, sKey, rs.Fields("UserGroupDescription"))
             End If
        End If

         rs.MoveNext

    Next
    UserGroupTree.Refresh
    bNodYSet = False
    nodX.EnsureVisible

    bTreeShown = True
End Function

Sub UserGroupTree_NodeClick(ByVal Node)

	rs.MoveFirst

	sKey   = Replace(Node.Key,"'","",1,10,1)

	rs.Find "UserGroupNumber = " & sKey

	window.UserGroupShortName.Text = rs.Fields("UserGroupShortName").Value
	window.UserGroupDescription.Text = rs.Fields("UserGroupDescription").Value

End Sub

Sub DisableFields

	window.UserGroupShortName.Enabled = false
	window.UserGroupDescription.Enabled = false

End Sub

Sub EnableFields

	window.UserGroupShortName.Enabled = true
	window.UserGroupDescription.Enabled = true

End Sub

Sub ClearFields

	window.UserGroupShortName.Text = ""
	window.UserGroupDescription.Text =  ""

End Sub

Function DisableControls(s_Action)

   window.UserGroupTree.Enabled = false

  if s_Action =  "Add" then
     btn_AddUserGroup.disabled = false
	 btn_UpdUserGroup.disabled = true
     btn_DelUserGroup.disabled = true
     window.btn_UpdUserGroup.style.visibility = "hidden"
     window.btn_DelUserGroup.style.visibility = "hidden"
     window.pic_UpdateUserGroup.style.visibility = "hidden"
     window.pic_DelUserGroup.style.visibility = "hidden"

  Elseif s_Action = "Update" then
     btn_UpdUserGroup.disabled = false
	 btn_AddUserGroup.disabled = true
     btn_DelUserGroup.disabled = true
     window.btn_DelUserGroup.style.visibility = "hidden"
     window.btn_AddUserGroup.style.visibility = "hidden"
     window.pic_AddUserGroup.style.visibility = "hidden"
     window.pic_DelUserGroup.style.visibility = "hidden"

  Elseif s_Action = "Delete" then
     btn_DelUserGroup.disabled = false
	 btn_AddUserGroup.disabled = true
     btn_UpdUserGroup.disabled = true
     window.btn_UpdUserGroup.style.visibility = "hidden"
     window.btn_AddUserGroup.style.visibility = "hidden"
     window.pic_UpdateUserGroup.style.visibility = "hidden"
     window.pic_AddUserGroup.style.visibility = "hidden"

  end if

End Function

Function EnableControls()

     window.UserGroupTree.Enabled = true
     btn_AddUserGroup.disabled = false

     if rs.RecordCount > 0 then
		btn_UpdUserGroup.disabled = false
		btn_DelUserGroup.disabled = false
     else
		btn_UpdUserGroup.disabled = true
		btn_DelUserGroup.disabled = true
     end if

     window.btn_AddUserGroup.style.visibility = "visible"
     window.btn_UpdUserGroup.style.visibility = "visible"
     window.btn_DelUserGroup.style.visibility = "visible"
     window.pic_AddUserGroup.style.visibility = "visible"
     window.pic_UpdateUserGroup.style.visibility = "visible"
     window.pic_DelUserGroup.style.visibility = "visible"

End Function

Sub SetActionButtons

  if rs.RecordCount > 0 then
     window.btn_UpdUserGroup.disabled = false
     window.btn_DelUserGroup.disabled = false
  else
     window.btn_UpdUserGroup.disabled = true
     window.btn_DelUserGroup.disabled = true
  end if

End sub

Sub SetAccessLightsServer

    sRes1 = "<%=i_AddUserGroup%>"
    if sRes1 = "Allowed" then
       window.pic_AddUserGroup.src = "images/MLSAllowed.bmp"
       window.pic_AddUserGroup.title = "1"
    else
       window.pic_AddUserGroup.src = "images/MLSDenied.bmp"
       window.pic_AddUserGroup.title = "0"
	end if

	 sRes1 = "<%=i_UpdUserGroup%>"
    if sRes1 = "Allowed" then
       window.pic_UpdateUserGroup.src = "images/MLSAllowed.bmp"
       window.pic_UpdateUserGroup.title = "1"
    else
       window.pic_UpdateUserGroup.src = "images/MLSDenied.bmp"
       window.pic_UpdateUserGroup.title = "0"
	end if

	 sRes1 = "<%=i_DelUserGroup%>"
    if sRes1 = "Allowed" then
       window.pic_DelUserGroup.src = "images/MLSAllowed.bmp"
       window.pic_DelUserGroup.title = "1"
    else
       window.pic_DelUserGroup.src = "images/MLSDenied.bmp"
       window.pic_DelUserGroup.title = "0"
	end if

end Sub

Sub SetAccessLights

    sUserName = "<%= Session("UserID")%>"

	set x = CreateObject("MLSSecurity.FunctionClass")
    sRes1 = x.CheckFunctionAccess("DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>",window.btn_AddUserGroup.title,Trim(sUserName))

    if sRes1 = "Allowed" then
          window.pic_AddUserGroup.src = "images/MLSAllowed.bmp"
          window.pic_AddUserGroup.title = "1"
	end if

	sRes1 = x.CheckFunctionAccess("DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>",window.btn_UpdUserGroup.title,Trim(sUserName))

    if sRes1 = "Allowed" then
          window.pic_UpdateUserGroup.src = "images/MLSAllowed.bmp"
          window.pic_UpdateUserGroup.title = "1"
	end if

    sRes1 = x.CheckFunctionAccess("DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>",window.btn_DelUserGroup.title,Trim(sUserName))

    if sRes1 = "Allowed" then
          window.pic_DelUserGroup.src = "images/MLSAllowed.bmp"
          window.pic_DelUserGroup.title = "1"
	end if

End Sub

Sub btn_CancelUserGroupAction_onclick

   	ClearFields

   	EnableControls

   	if rs.RecordCount > 0 then

		rs.Requery

		rs.Bookmark = v_BookMark
		window.UserGroupShortName.Text = rs.Fields("UserGroupShortName").Value
		window.UserGroupDescription.Text = rs.Fields("UserGroupDescription").Value

	end if

	btn_AddUserGroup.value = "Add"
	btn_UpdUserGroup.value = "Update"
	btn_DelUserGroup.value = "Delete"
	btn_CancelUserGroupAction.style.visibility = "hidden"
	OptionTable.style.visibility = "visible"

	EnableControls
	DisableFields
    window.UserGroupTree.focus()

End Sub

Sub btn_AddUserGroup_onclick

  if window.pic_AddUserGroup.title = "0" then
      window.status = "Access denied to " & window.btn_AddUserGroup.title
      exit sub
    end if

  v_BookMark = rs.Bookmark

  if btn_AddUserGroup.value = "Add" then

    ClearFields
     'Disable other controls...
	DisableControls("Add")
	'Enable Fields
	EnableFields
	btn_CancelUserGroupAction.style.visibility = "visible"

	'Set Focus
    'Set the button caption..
	btn_AddUserGroup.value = "Commit"

	window.UserGroupShortName.HighlightText = true
	window.UserGroupShortName.focus()

  elseif btn_AddUserGroup.value = "Commit" then

    call AddUserGroupRecord(window.UserGroupShortName.Text,window.UserGroupDescription.text)
	'Clean up...

	btn_AddUserGroup.value = "Add"
	btn_CancelUserGroupAction.style.visibility = "hidden"

	EnableControls

	DisableFields()

	SetActionButtons
	 UserGroupTree.focus()
    Set nodX = UserGroupTree.SelectedItem
    nodX.Selected = True
    UserGroupTree_NodeClick nodX

	End if
End Sub

Function AddUserGroupRecord(sName,sDesc)
dim i_tmp

    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")
    'Cannot use OLE DB Provider as it appears that it does not return a recordset
    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=MLS System Version1 [usergroup.asp];uid=<%= Session("UserID")%>"
'    sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [usergroup.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"

	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc

	If UserGroupTree.Nodes.Count <= 0 Then 'Create the root node...
	      ' Set Treeview control properties.
	      UserGroupTree.LineStyle = 1  ' Linestyle 1   ' Add Node objects.
	     ' First node with 'Root' as text.
	      Set nodX = UserGroupTree.Nodes.Add(, , "r", window.UserGroupDescription.Text)

	      com.CommandText = "y_AddUserGroupRecord"
	      set prm = com.CreateParameter ( "UserGroupShortName",200,1,40,sName) ' AdUnsigned Int
		  com.Parameters.Append prm
		  set prm = com.CreateParameter ( "UserGroupShortDescription",200,1,80,sDesc) 'AdVarchar , adParamInput
		  com.Parameters.Append prm
	      set prm = com.CreateParameter ( "UserGroupParent",19,1,,-1)
		  com.Parameters.Append prm
	      set prm = com.CreateParameter ( "UserGroupSequence",19,1,,0)
		  com.Parameters.Append prm
		  set rs_temp = com.Execute

		   i_tmp = CInt(rs_temp.Fields(0).Value)
	 Else

	      Set nodX = UserGroupTree.SelectedItem
	      Set nodY = nodX
	      bNodYSet = True

	      strNodeText = nodX.Text ' & vbLf

	      If nodX.Root.Text = strNodeText And UserGroupTree.Nodes.Count > 0 And  window.Child.status = false Then         'We are the  root..
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

		    iSeq =  rs.Fields("UserGroupSequence")

			com.CommandText = "y_AddUserGroupRecord"
			set prm = com.CreateParameter ( "UserGroupShortName",200,1,40,sName) ' AdUnsigned Int
			com.Parameters.Append prm
			set prm = com.CreateParameter ( "UserGroupShortDescription",200,1,80,sDesc) 'AdVarchar , adParamInput
			com.Parameters.Append prm
			set prm = com.CreateParameter ( "UserGroupParent",19,1,,iParent)
			com.Parameters.Append prm
			set prm = com.CreateParameter ( "UserGroupSequence",19,1,,iSeq)
			com.Parameters.Append prm
			set rs_temp = com.Execute

	        If  CInt(rs_temp.Fields(0).Value) < 0 Then
	           window.UserGroupShortName.Enabled = True
	           window.UserGroupDescription.Enabled = True
	           Exit Function
	        End If

	        i_tmp = CInt(rs_temp.Fields(0).Value)

			sParent = "'" & CStr(iParent) & "'"
			sKey = "'" & CStr( rs_temp.Fields(0).Value) & "'"
			Dim iC
			Dim iP
			If iKey > 0 Then
				Set nodX = UserGroupTree.Nodes.Add(sParent, 4, sKey, window.UserGroupDescription.Text)
				iC = nodX.Index
				iP = nodX.Parent.Index
			End If
			Set nodY = nodX
			bNodYSet = True
			nodX.EnsureVisible
			UserGroupTree.Nodes.Clear

			rs.Requery

			PopulateTree()
			'Reposition to Added node
			for i = 1 to UserGroupTree.Nodes.Count step 1
				  ' msgbox i
				    set nodX = UserGroupTree.Nodes(i)
				    sKey = nodX.Key
				     iKey   = Cint(Replace(sKey,"'","",1,10,1))

				    if iKey = CInt(i_tmp) then

				        nodX.Selected = true

						exit for
				    end if
			next

	 End If

End Function

Function GetUserGroupSequence(i_Val)

    sSQL = "SELECT UserGroupSequence FROM USERGROUP (nolock) WHERE UserGroupNumber = "  & i_Val
	'msgbox sSQL
    rs1.CursorLocation = 3
	rs1.Open sSQL ,conn,adOpenStatic

	GetUserGroupSequence = rs1.Fields(0)
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

Sub btn_UpdUserGroup_onclick

 if window.pic_UpdateUserGroup.title = "0" then
      window.status = "Access denied to " & window.btn_UpdUserGroup.title
      exit sub
    end if

  v_BookMark = rs.Bookmark

  if btn_UpdUserGroup.value = "Update" then

    'Disable other controls...
	DisableControls("Update")

	'Enable Fields
	EnableFields
	btn_CancelUserGroupAction.style.visibility = "visible"
	OptionTable.style.visibility = "hidden"

	'Set Focus
    'Set the button caption..
	btn_UpdUserGroup.value = "Commit"

	window.UserGroupShortName.HighlightText = true
	window.UserGroupShortName.focus()

  elseif btn_UpdUserGroup.value = "Commit" then

    i_tmp = rs.Fields("UserGroupNumber").Value
  	call UpdateUserGroupRecord(rs.Fields("UserGroupNumber"),window.UserGroupShortName.Text,window.UserGroupDescription.text)
	'Clean up...

	btn_UpdUserGroup.value = "Update"
	btn_CancelUserGroupAction.style.visibility = "hidden"
	window.OptionTable.style.visibility = "visible"
	EnableControls
	DisableFields()

	SetActionButtons

	UserGroupTree.Nodes.Clear

	PopulateTree()

	'Reposition to updated node...........
	for i = 1 to UserGroupTree.Nodes.Count step 1
	  ' msgbox i
	    set nodX = UserGroupTree.Nodes(i)
	    sKey = nodX.Key
	     iKey   = Cint(Replace(sKey,"'","",1,10,1))

	    if iKey = CInt(i_tmp) then
	       nodX.Selected = true
			exit for
	    end if
	next

	UserGroupTree.focus()

	rs.Bookmark = v_BookMark

	End if
End Sub

Sub btn_DelUserGroup_onclick

i_CrLf =  Chr(13) & Chr(10)

  if window.pic_DelUserGroup.title = "0" then
      window.status = "Access denied to " & window.btn_DelUserGroup.title
      exit sub
    end if

  v_BookMark = rs.Bookmark

  if btn_DelUserGroup.value = "Delete" then

     rs.MovePrevious

     if rs.BOF = true then
		rs.MoveFirst
     end if
     i_tmp1 = rs.Fields("UserGroupNumber").Value

     rs.MoveNext

     'Disable other controls...
	 DisableControls("Delete")
	'Enable Fields

	btn_CancelUserGroupAction.style.visibility = "visible"
	OptionTable.style.visibility = "hidden"

	'Set Focus
    'Set the button caption..
	btn_DelUserGroup.value = "Commit"

  elseif btn_DelUserGroup.value = "Commit" then

     set nodX = UserGroupTree.SelectedItem
	 iChildren = nodX.Children
     if iChildren > 0 then
       i_Resp = MsgBox("Deleting " & trim(rs.Fields("UserGroupDescription").Value) & " will also delete its children..."  & i_CrLf & "Are you sure you want to do this...???? " , 4)
       if i_Resp= 7 then
         btn_CancelUserGroupAction_onclick
         exit sub
       end if
     end if

      i_Resp = MsgBox("Deleting " & trim(rs.Fields("UserGroupDescription").Value) & " will also delete all associated access rights..."  & i_CrLf & "Are you sure you want to do this...???? " , 4)
       if i_Resp= 7 then
         btn_CancelUserGroupAction_onclick
         exit sub
       end if

    call DeleteUserGroupRecord(rs.Fields("UserGroupNumber").Value)
	'Clean up...

	btn_DelUserGroup.value = "Delete"
	btn_CancelUserGroupAction.style.visibility = "hidden"
    window.OptionTable.style.visibility = "visible"
	EnableControls

	SetActionButtons

    UserGroupTree.Nodes.Clear

	PopulateTree()

	'Reposition to previous node...........
	for i = 1 to UserGroupTree.Nodes.Count step 1
	    set nodX = UserGroupTree.Nodes(i)
	    sKey = nodX.Key
	    iKey   = Cint(Replace(sKey,"'","",1,10,1))
	    if iKey >= CInt(i_tmp1)  then
	        nodX.Selected = true
	        window.UserGroupTree.focus()
	       Set nodX = UserGroupTree.SelectedItem
           nodX.Selected = True
            UserGroupTree_NodeClick nodX
			exit for
	    end if
	next

	if rs.RecordCount > 0 then
	  rs.MoveFirst
	  rs.Find "UserGroupNumber >= " & Cint( i_tmp1)
	  window.UserGroupShortName.Text = rs.Fields("UserGroupShortName").Value
	  window.UserGroupDescription.Text = rs.Fields("UserGroupDescription").Value

	end if

	End if

End Sub

Function UpdateUserGroupRecord(i_Number,sName,sDesc)
Dim conn

    set conn = createobject("ADODB.Connection")

    sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [usergroup.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    conn.CursorLocation = 1

	conn.Open sDSN

	conn.Execute "y_UpdUserGroupRecord " & i_Number & ",'" & sName & "','" & sDesc & "'"
    conn.close

    rs.Requery

End Function

Function DeleteUserGroupRecord(i_Number)
Dim conn

    set conn = createobject("ADODB.Connection")

    sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [usergroup.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    conn.CursorLocation = 1

	conn.Open sDSN

	conn.Execute "y_DelUserGroupRecord " & i_Number
    conn.close

    rs.Requery

End Function

Sub window_onfocus

if b_Loaded = false then
   if rs.RecordCount > 0 then
	  rs.MoveFirst
	  window.UserGroupShortName.Text = rs.Fields("UserGroupShortName").Value
	  window.UserGroupDescription.Text = rs.Fields("UserGroupDescription").Value
	  window.UserGroupTree.focus()
	end if
	b_Loaded = true
end if

End Sub

Sub lbl_Sibling_onclick
window.Sibling.click
End Sub

Sub lbl_Child_onclick
window.Child.click
End Sub

-->
    </script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<body class="Generic">
    <p>
        <table border="0" cellpadding="1" cellspacing="1" style="height: 93px; left: 397px;
            position: absolute; top: 19px; width: 344px; z-index: 101" width="75%" class="Table1">
            <tr>
                <td align="right" nowrap>
                    Short Name
                </td>
                <td nowrap>
                    <object classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id="UserGroupShortName"
                        style="height: 26px; left: 1px; top: 1px; width: 260px" tabindex="1">
                        <param name="_Version" value="65536">
                        <param name="_ExtentX" value="6879">
                        <param name="_ExtentY" value="688">
                        <param name="BackColor" value="-2147483643">
                        <param name="EditMode" value="0">
                        <param name="ForeColor" value="-2147483640">
                        <param name="ReadOnly" value="0">
                        <param name="ShowContextMenu" value="-1">
                        <param name="MarginLeft" value="1">
                        <param name="MarginRight" value="1">
                        <param name="MarginTop" value="1">
                        <param name="MarginBottom" value="1">
                        <param name="Enabled" value="-1">
                        <param name="MousePointer" value="0">
                        <param name="Appearance" value="1">
                        <param name="BorderStyle" value="1">
                        <param name="AlignHorizontal" value="0">
                        <param name="AlignVertical" value="0">
                        <param name="MultiLine" value="0">
                        <param name="ScrollBars" value="0">
                        <param name="PasswordChar" value="">
                        <param name="AllowSpace" value="-1">
                        <param name="Format" value="">
                        <param name="FormatMode" value="1">
                        <param name="AutoConvert" value="-1">
                        <param name="ErrorBeep" value="0">
                        <param name="MaxLength" value="0">
                        <param name="LengthAsByte" value="0">
                        <param name="Text" value="">
                        <param name="Furigana" value="0">
                        <param name="HighlightText" value="-1">
                        <param name="IMEMode" value="0">
                        <param name="IMEStatus" value="0">
                        <param name="DropWndWidth" value="0">
                        <param name="DropWndHeight" value="0">
                        <param name="ScrollBarMode" value="0">
                        <param name="MoveOnLRKey" value="0">
                        <param name="OLEDragMode" value="0">
                        <param name="OLEDropMode" value="0">
                    </object>
                </td>
            </tr>
            <tr>
                <td align="right" nowrap>
                    Description
                </td>
                <td nowrap>
                    <object classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id="UserGroupDescription"
                        style="height: 26px; left: 1px; top: 1px; width: 365px" tabindex="2">
                        <param name="_Version" value="65536">
                        <param name="_ExtentX" value="9657">
                        <param name="_ExtentY" value="688">
                        <param name="BackColor" value="-2147483643">
                        <param name="EditMode" value="0">
                        <param name="ForeColor" value="-2147483640">
                        <param name="ReadOnly" value="0">
                        <param name="ShowContextMenu" value="-1">
                        <param name="MarginLeft" value="1">
                        <param name="MarginRight" value="1">
                        <param name="MarginTop" value="1">
                        <param name="MarginBottom" value="1">
                        <param name="Enabled" value="-1">
                        <param name="MousePointer" value="0">
                        <param name="Appearance" value="1">
                        <param name="BorderStyle" value="1">
                        <param name="AlignHorizontal" value="0">
                        <param name="AlignVertical" value="0">
                        <param name="MultiLine" value="0">
                        <param name="ScrollBars" value="0">
                        <param name="PasswordChar" value="">
                        <param name="AllowSpace" value="-1">
                        <param name="Format" value="">
                        <param name="FormatMode" value="1">
                        <param name="AutoConvert" value="-1">
                        <param name="ErrorBeep" value="0">
                        <param name="MaxLength" value="0">
                        <param name="LengthAsByte" value="0">
                        <param name="Text" value="">
                        <param name="Furigana" value="0">
                        <param name="HighlightText" value="-1">
                        <param name="IMEMode" value="0">
                        <param name="IMEStatus" value="0">
                        <param name="DropWndWidth" value="0">
                        <param name="DropWndHeight" value="0">
                        <param name="ScrollBarMode" value="0">
                        <param name="MoveOnLRKey" value="0">
                        <param name="OLEDragMode" value="0">
                        <param name="OLEDropMode" value="0">
                    </object>
                </td>
            </tr>
        </table>
        <input id="btn_AddUserGroup" name="btn_AddUserGroup" style="cursor: hand; height: 48px;
            left: 565px; position: absolute; top: 226px; width: 132px; z-index: 105" title="Add User Group"
            type="button" value="Add" height="50" class="button3">
        <input id="btn_UpdUserGroup" name="btn_UpdUserGroup" style="cursor: hand; height: 48px;
            left: 565px; position: absolute; top: 277px; width: 132px; z-index: 110" title="Update User Group"
            type="button" value="Update" height="48" class="button3">
        <input id="btn_DelUserGroup" name="btn_DelUserGroup" style="cursor: hand; height: 48px;
            left: 565px; position: absolute; top: 328px; width: 132px; z-index: 112" title="Delete UserGroup"
            type="button" value="Delete" height="48" width="132" class="button3">
        <input id="btn_CancelUserGroupAction" name="btn_CancelUserGroupAction" style="cursor: hand;
            height: 48px; left: 565px; position: absolute; top: 379px; visibility: hidden;
            width: 132px; z-index: 103" type="button" value="Cancel" height="48" width="132"
            class="button3">
        <img height="23" id="pic_AddUserGroup" src="images/MLSDenied.bmp" style="height: 23px;
            left: 572px; position: absolute; top: 237px; width: 19px; z-index: 108" title="0"
            width="19">
        <img alt="" border="0" height="23" hspace="0" id="pic_UpdateUserGroup" src="images/MLSDenied.bmp"
            style="height: 23px; left: 572px; position: absolute; top: 287px; width: 19px;
            z-index: 113" title="0" usemap="" width="19">
        <img alt="" border="0" height="23" hspace="0" id="pic_DelUserGroup" src="images/MLSDenied.bmp"
            style="height: 23px; left: 572px; position: absolute; top: 339px; width: 19px;
            z-index: 117" title="0" usemap="" width="19">
        &nbsp;
        <table border="0" cellpadding="1" cellspacing="1" style="height: 51px; left: 566px;
            position: absolute; top: 140px; width: 133px; z-index: 115" width="75%" id="OptionTable"
            class="Table1">
            <tr>
                <td nowrap>
                    <input id="Sibling" name="radio1" type="radio" checked>
                </td>
                <td nowrap id="lbl_Sibling">
                    Sibling&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td nowrap>
                    <input id="Child" name="radio2" type="radio" value="1">
                </td>
                <td nowrap id="lbl_Child">
                    Child
                </td>
            </tr>
        </table>
        <object classid="clsid:C74190B6-8589-11D1-B16A-00C0F0283628" id="UserGroupTree" style="height: 536px;
            left: 30px; position: absolute; top: 5px; width: 357px; z-index: 118" VIEWASTEXT>
            <param name="_ExtentX" value="9446">
            <param name="_ExtentY" value="14182">
            <param name="_Version" value="327682">
            <param name="HideSelection" value="1">
            <param name="Indentation" value="1005">
            <param name="LabelEdit" value="0">
            <param name="LineStyle" value="0">
            <param name="PathSeparator" value="\">
            <param name="Sorted" value="0">
            <param name="Style" value="7">
            <param name="ImageList" value="myImageList">
            <param name="BorderStyle" value="1">
            <param name="Appearance" value="1">
            <param name="MousePointer" value="0">
            <param name="Enabled" value="1">
            <param name="OLEDragMode" value="0">
            <param name="OLEDropMode" value="0">
        </object>
    </p>
</body>
</html>