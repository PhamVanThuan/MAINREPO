<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title></title>
    <meta content="text/html" http-equiv="Content-Type">
    <meta name="VI60_DefaultClientScript" content="VBScript">
    <meta name="GENERATOR" content="MSHTML 5.50.4807.2300">
    <base target="main">
    <link href="SAHL-MLSS.css" type="text/css" rel="stylesheet">
</head>
<body class="Generic" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <p>
        <img style="z-index: 100; left: -132px; position: absolute; top: 0px" height="60"
            src="images/sq_grn_lt.jpg">
        <table cellspacing="0" cellpadding="0" border="0" id="Table1">
            <tr>
                <td>
                    <img id="IMG_REGISTRATION" style="z-index: 114; width: 17px; cursor: hand; position: absolute;
                        top: 2px; height: 17px" height="17" alt="" hspace="0" src="images/accessdenied17.bmp"
                        width="17" usemap="" border="0" name="IMG_REGISTRATION">
                    <input class="button-orange2" id="btn_Registration" title="Registration" style="width: 150px;
                        cursor: hand; background-repeat: no-repeat; height: 22px" type="button" align="middle"
                        value="Registration" name="btn_Registration">
                </td>
                <td>
                    <img id="IMG_CLIENTSERVICES" style="z-index: 112; width: 17px; cursor: hand; position: absolute;
                        top: 2px; height: 17px" height="17" alt="" hspace="0" src="images/accessdenied17.bmp"
                        width="17" usemap="" border="0" name="IMG_CLIENTSERVICES">
                    <input class="button-orange2" id="btn_ClientServices" title="Loans" style="width: 150px;
                        cursor: hand; background-repeat: no-repeat; height: 22px" type="button" align="middle"
                        value="Client Services" name="btn_ClientServices">
                </td>
                <td>
                    <img id="IMG_SECURITISATION" style="z-index: 116; width: 17px; cursor: hand; position: absolute;
                        top: 2px; height: 17px" height="17" alt="" hspace="0" src="images/accessdenied17.bmp"
                        width="17" usemap="" border="0" name="IMG_SECURITISATION">
                    <input class="button-orange2" id="btn_Securitisation" title="Securitisation" style="width: 150px;
                        cursor: hand; background-repeat: no-repeat; height: 22px" type="button" align="middle"
                        value="Securitisation" name="btn_Securitisation">
                </td>
                <td>
                    <img id="IMG_SYSTEM" style="z-index: 115; width: 17px; cursor: hand; position: absolute;
                        top: 1px; height: 17px" height="17" alt="" hspace="0" src="images/accessdenied17.bmp"
                        width="17" usemap="" border="0" name="IMG_SYSTEM">
                    <input class="button-orange2" id="btn_System" title="System" style="width: 150px;
                        cursor: hand; background-repeat: no-repeat; height: 22px" type="button" align="middle"
                        value="System" name="btn_System">
                </td>
        </table>
    </p>
    <%

'************************************************************************************
'*
'* Function/Sub : -
'*
'*
'* Description  : Server side script which invokes the system security module to
'*                determine the access rights to grant/deny to the user.
'*
'*
'************************************************************************************
 sDatabase = Session("SQLDatabase")
 sUid = Session("UserID")
  if Session("LoggedOn") = "Success" then
	set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
	i_Registration = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Registration",Session("UserName"))
	i_ClientServices = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Client Services",Session("UserName"))
	i_Securitisation = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Securitisation",Session("UserName"))
	i_System = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"System",Session("UserName"))
  End if
    %>
    <script id="clientEventHandlersVBS" language="vbscript">
<!--
'************************************************************************************
'*
'* Module : TopToolBar.asp
'*
'*
'* Description  : Provides access to main system functional areas.
'*
'*
'************************************************************************************
Dim sRes1
Dim dtLastAuditCheck
Dim i_Counter
Dim i_SAHLEmployeeNumber
Dim i_EmployeeTeamNumber
if rs_open <> true then
   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"
	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
    end if
    x = "=<%= Session("SQLDatabase")%>"
    if x <> "=" then
		set conn = createobject("ADODB.Connection")
		set rs_temp = createobject("ADODB.Recordset")
	end if
end if

'************************************************************************************
'*
'* Function/Sub : SetAccessLightsServer
'*
'*
'* Description  : Set access rights to buttons(functions) based on values returned from
'*                the security module(MLSSecurity.dll) which runs on the server.
'*
'*
'************************************************************************************
Sub SetAccessLightsServer
     window.btn_Registration.style.backgroundColor= "#FFA000"
     window.btn_ClientServices.style.backgroundColor= "#FFA000"
     window.btn_Securitisation.style.backgroundColor= "#FFA000"
     window.btn_System.style.backgroundColor= "#FFA000"

   	 sRes1 = "<%=i_Registration%>"
   	 if sRes1 = "Allowed" then
           IMG_REGISTRATION.src = "images/accessallowed17.bmp"
           IMG_REGISTRATION.name = "1"
     else
          IMG_REGISTRATION.src = "images/accessdenied17.bmp"
          IMG_REGISTRATION.name = "0"
     end if
     sRes1 = "<%=i_ClientServices%>"
     if sRes1 = "Allowed" then
           IMG_CLIENTSERVICES.src = "images/accessallowed17.bmp"
           IMG_CLIENTSERVICES.name = "1"
     else
          IMG_CLIENTSERVICES.src = "images/accessdenied17.bmp"
          IMG_CLIENTSERVICES.name = "0"
     end if
   	 sRes1 = "<%=i_Securitisation%>"
   	 if sRes1 = "Allowed" then
           IMG_SECURITISATION.src = "images/accessallowed17.bmp"
           IMG_SECURITISATION.name = "1"
     else
          IMG_SECURITISATION.src = "images/accessdenied17.bmp"
          IMG_SECURITISATION.name = "0"
     end if
	 sRes1 = "<%=i_System%>"
	 if sRes1 = "Allowed" then
          IMG_SYSTEM.src = "images/accessallowed17.bmp"
          IMG_SYSTEM.name = "1"
     else
          IMG_SYSTEM.src = "images/accessdenied17.bmp"
          IMG_SYSTEM.name = "0"
	 end if
End sub

Sub window_onunload
    window.status = "Ready....."
End Sub

Sub window_onload
	SetAccessLightsServer
	sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	conn.Open sDSN
	sUserName = "<%= Session("UserName")%>"
	sSQL = "SELECT SAHLEmployeeNumber,EmployeeTeamNumber FROM SAHLEMPLOYEE (nolock) WHERE SAHLEmployeeName = '" & RTrim(sUserName) & "'"

	rs_temp.CursorLocation = 3
	rs_temp.Open sSQL,conn,adOpenDynamic
	if rs_temp.RecordCount > 0 then
		i_SAHLEmployeeNumber = rs_temp.Fields("SAHLEmployeeNumber").Value
		i_EmployeeTeamNumber = Cint(rs_temp.Fields("EmployeeTeamNumber").Value)
		rs_temp.Close
	else
		rs_temp.close
	end if
	conn.close
	i_Counter = 0
	window.focus
End Sub

Sub btn_Registration_onclick
	'* Description  : Registration functions
	window.status = ""
	if IMG_REGISTRATION.name = "0" then
		window.status = "Access denied to Registration...."
		exit sub
	end if
	SetSelectedButton(1)
	window.parent.frames("main").location.href = "Registration.htm"
	IMG_REGISTRATION.name = "1"
End Sub

Sub IMG_REGISTRATION_onclick
	window.btn_Registration.click
End Sub

Sub btn_ClientServices_onclick
	'* Description  : Client Service functions (Client and Mortgage Loan functions)
	window.status = ""
	if IMG_CLIENTSERVICES.name = "0" then
	   window.status = "Access denied to Client Services...."
	   exit sub
	end if
	SetSelectedButton(2)
	 window.parent.frames("main").location.href = "ClientServices.htm"
	IMG_CLIENTSERVICES.name = "1"
End Sub

Sub IMG_CLIENTSERVICES_onclick
	window.btn_ClientServices.click
End Sub

Sub btn_Securitisation_onclick
	'* Description  : Securitisation functions
	window.status = ""
	if  IMG_SECURITISATION.name = "0" then
	      window.status = "Access denied to Securitisation...."
	      exit sub
	end if
	SetSelectedButton(3)
	 window.parent.frames("main").location.href = "Securitisation.htm"
	IMG_SECURITISATION.name = "1"
End Sub

Sub IMG_SECURITISATION_onclick
	window.btn_Securitisation.click
End Sub

Sub btn_System_onclick
	'* Description  : System related functions (Rate Chnages,Month end , Function definitions)
	if IMG_SYSTEM.name = "0" then
	      window.status = "Access denied to System....."
	      exit sub
	end if
	SetSelectedButton(4)
	window.parent.frames("main").location.href = "System.htm"
	IMG_SYSTEM.name = "1"
End Sub
Sub IMG_SYSTEM_onclick
	window.btn_System.click
End Sub

Sub SetSelectedButton(iButton)
    SetAccessLightsServer
	Select Case iButton
	     Case 1
	             window.btn_Registration.style.backgroundColor= "SteelBlue"
	             window.IMG_REGISTRATION.src = "images/accessallowedsteelblue17.bmp"
	     Case 2
	             window.btn_ClientServices.style.backgroundColor= "SteelBlue"
	             window.IMG_CLIENTSERVICES.src = "images/accessallowedsteelblue17.bmp"
	     Case 3
	             window.btn_Securitisation.style.backgroundColor= "SteelBlue"
	             window.IMG_SECURITISATION.src = "images/accessallowedsteelblue17.bmp"
	     Case 4
	             window.btn_System.style.backgroundColor= "SteelBlue"
	             window.IMG_SYSTEM.src = "images/accessallowedsteelblue17.bmp"
	End Select

End Sub

Sub GetLastDateTimeCheck
	dtDate = Date()
	myDate = Year(dtDate) & "-" & Replace(formatnumber(Month(dtDate)/10,1,-1) ,".", "") & "-" & Replace(formatnumber(Day(dtDate)/10,1,-1) ,".", "")
	tTime   = Time()
	myTime = Replace(formatnumber(Hour(tTime)/10,1,-1) ,".", "") & ":"  & Replace(formatnumber(Minute(tTime)/10,1,-1) ,".", "") & ":" & Replace(formatnumber(Second(tTime)/10,1,-1) ,".", "")
	dtLastAuditCheck = myDate & " 00:00:00"   '& myTime
End Sub

Sub window_onfocus
	GetOriginationQueryUpdates
	GetClientServicesQueryUpdates
End Sub

Sub GetOriginationQueryUpdates
	if i_EmployeeTeamNumber = 3 or i_EmployeeTeamNumber = 4 or i_EmployeeTeamNumber = 5or i_EmployeeTeamNumber = 9 or i_EmployeeTeamNumber = 10 then
		on error resume next
		set rs_temp = createobject("ADODB.Recordset")
 		set conn = createobject("ADODB.Connection")
  		sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [TopToolBar.asp 2]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
		conn.Open sDSN
		sSQL = " hd_GetCurrentIssueMessages " & CSTR(i_EmployeeTeamNumber) & ",'"  & dtLastAuditCheck & "'"
  		rs_temp.CursorLocation = 3
		rs_temp.Open sSQL,conn,adOpenDynamic
		if  rs_temp.Fields(0).Value > 0 then
			i_Counter = i_Counter + 1
			if i_Counter = 1 then
				color = "Green"
			elseif i_Counter = 2 then
				color = "Blue"
			elseif i_Counter = 3 then
				color = "DarkCyan"
			elseif i_Counter = 4 then
				color = "Purple"
			elseif i_Counter = 5 then
				color = "Red"
				i_Counter = 0
			end if
			window.parent.frames("crest").Marquee0.style.backgroundcolor = color
			window.parent.frames("crest").Marquee0.innerText =  rs_temp.Fields(0).Value & " Queries require you attention...check todays queries on the Origination HelpDesk System and/or your e-Mail .."
			window.parent.frames("crest").Marquee0.style.visibility = "visible"
		else
			window.parent.frames("crest").Marquee0.style.visibility = "hidden"
			window.parent.frames("crest").Marquee0.innerText =  ""
		end if
		rs_temp.Close
		conn.close
	end if
End Sub

Sub GetClientServicesQueryUpdates
	if i_EmployeeTeamNumber = 11 then
		on error resume next
		set rs_temp = createobject("ADODB.Recordset")
 		set conn = createobject("ADODB.Connection")
  		sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [TopToolBar.asp 3]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
		conn.Open sDSN
		sSQL = " hdc_GetCurrentClientQueryMessages " & CSTR(i_EmployeeTeamNumber) & ",'"  & dtLastAuditCheck & "'"
  		rs_temp.CursorLocation = 3
		rs_temp.Open sSQL,conn,adOpenDynamic
		if  rs_temp.Fields(0).Value > 0 then
			i_Counter = i_Counter + 1
			if i_Counter = 1 then
				color = "Green"
			elseif i_Counter = 2 then
				color = "Blue"
			elseif i_Counter = 3 then
				color = "DarkCyan"
			elseif i_Counter = 4 then
				color = "Purple"
			elseif i_Counter = 5 then
				color = "Red"
				i_Counter = 0
			end if
			window.parent.frames("crest").Marquee0.style.backgroundcolor = color
			window.parent.frames("crest").Marquee0.innerText =  rs_temp.Fields(0).Value & " Queries require you attention...check todays queries on the Client Services HelpDesk System and/or your e-Mail .."
			window.parent.frames("crest").Marquee0.style.visibility = "visible"
		else
			window.parent.frames("crest").Marquee0.style.visibility = "hidden"
			window.parent.frames("crest").Marquee0.innerText =  ""
		end if
		rs_temp.Close
		conn.close
	end if
End Sub

-->
    </script>
</body>
</html>