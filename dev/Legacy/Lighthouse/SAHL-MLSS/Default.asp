<%
  
  'Get the last DSN used....
  sLastDSN  = Request.Cookies("LastDSN")
  Session("LastDSN") = sLastDSN
  sLastServer  = Request.Cookies("LastServer")
  Session("LastServer") = sLastServer
  
  ServerDate= date()
%>
<html><head>
<!--#include virtual="/SAHL-MLSS/server.asp"-->
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->
<!--#include virtual="/SAHL-MLSS/dateutils.inc"-->
<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>
<object classid="clsid:CC1B3E64-97B9-44E6-BB82-F20F73B2F325" VIEWASTEXT codebase="MLSClientUtils.cab" id="MLSClientUtils">
</object>
<meta name="VI60_DTCScriptingPlatform" content="Server (ASP)">
<meta name="VI60_defaultClientScript" content="VBScript">
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<meta name="GENERATOR" content="Microsoft FrontPage (Internet Studio Edition) 2.0">
<title>SA Home Loans Mortgage Loan Securitisation System </title>
<script ID="clientEventHandlersVBS" LANGUAGE="vbscript">

Dim rs_DSN

'Size the Browser..... No windowstate = maximized so get max height and size...
with window.screen 
    window.resizeTo .availWidth, .availHeight 
end with

window.moveTo 0,0 

'************************************************************************************
'*
'* Function/Sub : window_onload
'*
'*
'* Description  : Downloads OCX controls and installs them if required. 
'*                Fills the Data Set List with ODBC entries from the client 
'*                machine. Gets the Workstations WSID by calling the MLSClientUtils
'*                DLL.
'*
'*
'************************************************************************************


function fadeIn() 

	window.MainBody.style.filter="blendTrans(duration=2)"
	window.form1.style.filter="blendTrans(duration=2)"
	window.hr_1.style.filter="blendTrans(duration=2)"
	window.tbl_Msg.style.filter="blendTrans(duration=2)"
	'window.form1.btn_Login.filter="blendTrans(duration=2)"

    if MainBody.filters.blendTrans.status <> 2 then
    
   
		MainBody.filters.blendTrans.apply()
		'window.form1.filters.blendTrans.apply()
		'window.hr_1.filters.blendTrans.apply()
		'window.tbl_Msg.filters.blendTrans.apply()

       
        MainBody.style.visibility="visible"
        window.form1.style.visibility="visible"
		window.hr_1.style.visibility="visible"
		window.tbl_Msg.style.visibility="visible"
	'	window.form1.btn_Login.visibility="visible"
        
        MainBody.filters.blendTrans.play()
        window.form1.filters.blendTrans.play()
		window.hr_1.filters.blendTrans.play()
		window.tbl_Msg.filters.blendTrans.play()
	'	window.form1.btn_Login.filters.blendTrans.play()
        
    end if

end function    

Sub window_onload
	
	' RSM: 2006/02/06 - remove error-handling to debug errors!
	' on error resume next
	
   
	'  window.MainBody.style.visibility = "hidden"
	' window.form1.style.visibility = "hidden"
    
	 
	' RSM: 2006/02/06	-	The control's invisible, so we can't set focus to it!
	' window.form1.txt_Date.focus
	 
	ServerDate = "<%=ServerDate%>"
	window.form1.txt_Date.value = date() 'FormatDateTime(ServerDate,2)
	'msgbox  date()
	 
	'msgbox "<%=CheckDateFormat%>"	,, "Server side Date Format"
	' msgbox CheckDateFormat,,"Client Side Date Format"
    'fadeIn
    
    window.status = date()
    
    if CheckDateFormat <> "S" then
		
		MsgBox "Your Local Regional Dat Setting is incorrect..." & vbCRLF & vbCRLF & "It should be dd/mm/yyyy" & vbCRLF & "Please correct or Contact IT" & vbCRLF & vbCRLF & " The System cannot continue ..." ,, "SAHL-MLSS : System Config Error"
		' RSM: We need to remove this line
		' window.parent.location.href = "http://sahlnet/SAHL-MLSS/default.asp"
		window.parent.location.href = "default.asp"
		window.parent.top.close
		exit sub
     End If
     
   '  if "<%=CheckDateFormat%>" <> "A" then
		
	'	MsgBox "Your Server Regional Date Setting is incorrect..." & vbCRLF & vbCRLF & "It should be mm/dd/yyyy" & vbCRLF & "Please Contact IT" & vbCRLF & vbCRLF & " The System cannot continue..." ,, "SAHL-MLSS : Server System Config Error"
	'	window.parent.location.href = "http://sahlnet/SAHL-MLSS/default.asp"
	'	'window.parent.top.close
	'	exit sub
     'End If
     
    
    'window.form1.btn_Login.visibility="visible"
    
    'Set the Combo drop down height
    
    
    ' RSM: 2005/12/06 - Removed - not giving the user too many options anymore
    ' window.form1.TrueDBCombo_DataSet.DropDownHeight = 130
    
  '  window.TDBNumber1.Value = 1
   
   
    'Display any messages..
    sMsg = ""
    sMsg= "<%= Session("SystemMessage1")%>"
	window.Message.innerText = sMsg
	
	window.form1.btn_Login.disabled = false
	
	'Populate the ODBC DSN List

	FillDSNList
	
	'Get the workstations ID
	set oNTUtil = createobject("MLSClientUtils.NTUtilityClass")
	s = oNTUtil.GetNTWSLoginName()
	
	window.form1.txtLogin.value = s
    window.form1.txtPWD.focus()
    
    exit sub
End Sub

'**************************************************************
'*
'* Function/Sub : FillDSNList
'*
'*
'* Description  : Fills the Dropdown Combo with ODBC connections
'*                defined on the client workstation. Note : A DSN
'*                with the same name must be defined on the Server
'*                machine on which IIS is located. e.g. A DSN of 
'*                SAHLS01 must also be set up on the IIS Server as 
'*                it is the server that will do the actual ODBC 
'*                connection call.
'*       
'*
'**************************************************************

Sub FillDSNList

    'Call the SQLDSNClass function in the MLSClientUtils DLL
    'This DLL should have been installed  during the first time
    'load of the system on a client machine...

    set rs = createobject("ADODB.Recordset")

    rs.CursorLocation = 3
    set rs_DSN = createobject("ADODB.Recordset")
    set oDSN = createobject("MLSClientUtils.SQLDSNClass")
    set rs  = oDSN.SQLDataSetSource

    rs.MoveFirst
  
    '*** Manually populate the rs
     rs_DSN.CursorLocation = 3
     rs_DSN.CursorType = 2
     rs_DSN.Fields.Append "DataSetNumber",19
     rs_DSN.Fields.Append "DataSetName",200,180
     rs_DSN.Open
    
     for i= 0 to rs.RecordCount-1 step 1
        rs_DSN.AddNew 
	 	rs_DSN.fields("DataSetNumber").Value = i 
	 	rs_DSN.fields("DataSetName").Value =  rs.Fields(0).Value
	 	rs_DSN.Update
        rs.MoveNext
     next

     rs_DSN.MoveFirst
       
    'To Ensure that this combo box installs properly 
    'it must be included in the Licence Manager file (APEX.lpk)
    'ALSO if the rowsource property is not set at run-time ensure that
    'MDAC Verison 2.1 is installed....

     rs_DSN.MoveFirst
    
    'Configure the Data Combo Box...

    ' RSM: 2005/12/06 - Removed - not giving the user too many options anymore
   	 'window.form1.TrueDBCombo_DataSet.RowSource = rs_DSN
     'window.form1.TrueDBCombo_DataSet.BoundText = rs_DSN.Fields("DataSetNumber").Value
     'window.form1.TrueDBCombo_DataSet.BoundColumn = rs_DSN.Fields("DataSetNumber").name
     'window.form1.TrueDBCombo_DataSet.ListField = rs_DSN.Fields("DataSetName").name
     'window.form1.TrueDBCombo_DataSet.OddRowStyle.BackColor = &HC0FFFF
	 'window.form1.TrueDBCombo_DataSet.EvenRowStyle.BackColor = &HC0C0C0
	 'window.form1.TrueDBCombo_DataSet.BoundText = rs_DSN.Fields("DataSetNumber").Value

	' RSM: 2005/12/06: Get the dsn teh user's allowed to connect to
	' window.form1.TrueDBCombo_DataSet.Enabled = false

	 'Get the last Server used and set the Server value ...
    

	' RSM: 2005/12/06: Get the dsn teh user's allowed to connect to	
	's = "<%=Session("LastServer")%>"
	s = "<%=NETWORK_DOMAIN%>"

    if trim(s) <> "" then
		form1.inServer.value = s
    end if
    
    'Get the last DSN used on the client machine and then position to last DSN
    'used...
     s = "<%=Session("LastDSN")%>"
     
	' RSM: 2005/12/06: Get the dsn teh user's allowed to connect to
	' Had to override the 'last dsn' rule
    s = "<%=DB_SERVER%>"
	
	window.form1.txtDatabaseName.value = s
	
    if trim(s) <> "" then
		rs_DSN.MoveFirst
        rs_DSN.Find "DataSetName = '" & s & "'"
        if rs_DSN.EOF = True then
           rs_DSN.MoveFirst
        end if
        ' window.form1.TrueDBCombo_DataSet.BoundText = rs_DSN.Fields("DataSetNumber").Value
        ' window.form1.TrueDBCombo_DataSet.Refresh
     else
        rs_DSN.MoveFirst
     end if
   
End Sub


'*******************************************************************************
'*
'* Function/Sub : window_onfocus
'*
'*
'* Description  : When focus is set to default.asp and one or more frames are 
'*                a Session time out has occured...therefore return the user back
'*                to the main  logon screen where must logon again. Note the Session
'*                timeout value is set in globa.asa.
'*       
'*
'*******************************************************************************
Sub window_onfocus
    
if window.top.frames.length > 0 then    
    window.top.document.location.replace "default.asp"
end if

End Sub

Sub img_Login_onclick
window.form1.btn_Login.click
End Sub

Sub img_Login_onkeydown
window.form1.btn_Login.click
End Sub

Sub txtPWD_onkeydown

if window.event.keyCode = 13 then ' enter key pressed
   window.form1.btn_Login.click
end if

End Sub

</script>


</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<body leftMargin="300" scroll="no" topMargin="100" bgProperties="fixed" class=Generic id=MainBody>
<font color="#000080"><em>&nbsp;
<hr style="LEFT: 260px; VISIBILITY: hidden; WIDTH: 300px; TOP: 122px; HEIGHT: 2px" width=300 id=hr_1>
</em>&nbsp;&nbsp;<font color="#000080"><em></em></font>&nbsp;&nbsp;
   </font>

<form action="MainFrame.asp" method="post" id="form1" name="form1" >
<font color="#000080"><em>
</em>
   </font>
    <table border="0" style="LEFT: 340px; WIDTH: 400px; TOP: 212px; HEIGHT: 1px" height   ="1" background="" width="400" class=Table>
        <tr>
            <th align="right">
      <p style="VISIBILITY: visible">Domain 
Name&nbsp;  </p> </th>
            <td><input maxlength="256" name="inServer" id="inServer" value="SAHL" style="WIDTH: 143px; HEIGHT: 25px" height       ="25" tabIndex="1" align="textTop" class=Generic width="143" 
      readOnly></td>
        </tr>
        <tr height=44 style="HEIGHT: 44px">
            <th align="right">Database Name&nbsp; 
        </th>
            <td><FONT color=#000080><EM><INPUT class=Generic id=txtDatabaseName 
      style="WIDTH: 143px; HEIGHT: 25px" tabIndex =3 maxLength=256 
      name=txtDatabaseName height="25" readOnly 
     >
</EM></FONT>
</td>
        </tr>
        <tr>
            <th align="right">User ID</th>
            <td><input maxlength="256" name="Login" height="25" style="WIDTH: 143px; HEIGHT: 25px" id   
      ="txtLogin" tabIndex="3" class=Generic></td>
        </tr>
        <tr>
            <th align="right">Password</th>
            <td><input type="password" maxlength="256" name="Password" id="txtPWD" height="0" style="LEFT: 1px;  WIDTH: 143px; CURSOR: text; TOP:    
        
      10px; 
      HEIGHT: 25px" 
      tabIndex="4" class=Generic  width="143"><INPUT id=txt_Date disabled 
name=text1 style="VISIBILITY: hidden; 
     WIDTH: 110px; HEIGHT: 22px" size=15 
     ></td>
        </tr></table>


    <p>&nbsp;</p>
<P>&nbsp;</P>
<P>&nbsp;
</P>
<P>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <input type="submit" value="Login" style="Z-INDEX: 104; LEFT: 428px; VISIBILITY: visible; WIDTH: 136px; POSITION: absolute; TOP: 358px; HEIGHT: 54px" id="btn_Login" tabindex="-1" src="" class=button2>&nbsp; 
<font color="#000080"><em>

</em>
   </font></P>
<p></p></form>


<p> </p>
<p><font color="#000080"><em>











<table border="1" cellPadding="1" cellSpacing="1" style="Z-INDEX: 101; 
LEFT: 310px; VISIBILITY: visible; WIDTH: 391px; POSITION: absolute; TOP: 507px; HEIGHT: 93px" width  ="75%" id=tbl_Msg class="">


  <tr>
    <td id="Message" title ="" valign="center" align="middle">&nbsp;
      
</td></tr></table></p></EM></FONT>

</body>
</html>
