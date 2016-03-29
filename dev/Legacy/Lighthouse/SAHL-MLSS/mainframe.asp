
<%

'************************************************************************************
'*
'* Module : MainFrame.asp
'*
'*
'* Description  : Performs the NT security and SQL security checks
'*
'*
'************************************************************************************

Response.buffer = true

sServerName = trim(Request.Form("inServer"))
sDatabase   = trim(request.form("txtDatabaseName")) 
'sDatabase   = trim(Request.Form("DSN_Selected")) 

Session("DSN") = sServerName
Session("SQLDatabase") = sDatabase

sServer =Session("DSN")

sUid        = trim(request.form("Login"))
sPassword   = trim(request.form("Password"))
sUserName   = trim(request.form("Login"))

sCurrentClient = "XX"

Session("UserID") = sUid
Session("UserName") = sUserName

Session("Password") = sPassword
Session("ServerName") = sServerName
Session("SystemMessage1") = ""

Session("CurrentClientNumber") = "99999"

sDatabase = Session("SQLDatabase")
sUid = Session("UserID")

   if  sServer = "" then
	   Session("SystemMessage1")  = "STATUS : Invalid Domain Server Name.............!!"
	   response.redirect("default.asp")
    end if
 
    sUid = Session("UserID")
    if  sUid = "" then
        Session("SystemMessage1")  = "STATUS : Invalid User Name..........................!!"
	   response.redirect("default.asp")
	   
    end if
 
	sPass=  Session("Password")
    if  sPass = "" then
		Session("SystemMessage1")  = "STATUS : A Password is required................!!"

		response.redirect("default.asp")
    end if
    
    if sPass = "Unexpected Error :  1330" then
		Session("SystemMessage1")  = "STATUS : Your Password has expired. Please change your Password!!"

		response.redirect("default.asp")
    end if
 
    Session("CurrentClientNumber") = "0"
    
 sDSN_Selected = ""
 sDSN_Selected = trim(Request.Form("DSN_Selected"))
   
if sDSN_Selected <> "" then
   Response.cookies("LastDSN")=sDSN_Selected
   Response.cookies("LastDSN").Expires = "01/01/2010"
   Response.cookies("LastServer")=sServerName
   Response.cookies("LastServer").Expires = "01/01/2010"
end if    
        
SQLLogin


'************************************************************************************
'*
'* Function/Sub : SQLLogin
'*
'*
'* Description  : Checks to see if the userid is a valid username/password in the NT Domain
'*
'*
'************************************************************************************

Function NTLogin
	dim x

      set x = Server.CreateObject("MLSSecurity.NTLogonClass")
	  ' RSM: 2005/11/30
	  ' Win XP/2003	bug - We need to pass through the domain name to teh autehntication dll!
	  ' sRes1= x.LoginToNTDomain(trim(request.form("ServerName")),trim(request.form("Login")),trim(request.form("Password")))

	   sRes1= x.LoginToNTDomain(trim(sServerName),trim(request.form("Login")),trim(request.form("Password")))

	   if sRes1 <> "Success" then
		    Session("NTError")  = sRes1
		    NTLogin = sRes1
		    Session("LoggedOn") = "Failure"
	   else
	       Session("NTError") = "Success"
	       Session("LoggedOn") = "Success"
	       NTLogin = sRes1
	       Session("LoggedOn") = "Success"
	   end if
	   
  
End Function

'************************************************************************************
'*
'* Function/Sub : SQLLogin
'*
'*
'* Description  : Checks to see if the userid is a valid username in the SQL database
'*
'*
'************************************************************************************


Function SQLLogin


  sDatabase = Session("SQLDatabase")
  sUid = Session("UserID")
        
  set oSecurity = Server.CreateObject("MLSSecurity.SQLServerLogonClass")
  sRes1 = oSecurity.LoginToSQLServer("DSN=" & sDatabase & ";uid=" & sUid,Session("UserName"))



  if sRes1 <> "Success" then
    Session("SystemMessage1")  = "STATUS : " & sRes1
    Session("SQLError")= sRes1
     Session("SQLLoginMsg") = sRes1
     Session("LoggedOn") = "Failure"
     
   else
      Session("LoggedOn") = "Success"
     
     set conn = server.createobject("ADODB.Connection")
	 sProvider = "DSN="
	 'sProvider = "Provider=SQLOLEDB.1;Data Source="
	 sUserName = Session("UserName")
	 sDSN  = sProvider & sDatabase & ";uid=" & sUserName 
	 conn.Open sDSN
     
	 sSQL = "SELECT SAHLEmployeeFullName,UserGroupShortName FROM SAHLEMPLOYEE (nolock),USERGROUP (nolock) WHERE  SAHLEMployee.UserGroupNumber = USERGROUP.UserGroupNumber AND SAHLEmployeeName = '" & RTrim(sUserName) & "'" 
	' msgbox sSQL
	 set rs_temp = server.createobject("ADODB.Recordset")
	 rs_temp.CursorLocation = 3
	 rs_temp.Open sSQL,conn,adOpenDynamic
     
     Session("UserGroupName")  =  rs_temp.fields(1) 
      Session("UserFullName")  =  rs_temp.fields(0) 
     
     
     Session("SQLLoginMsg") = "Welcome " & rs_temp.fields(0) 'trim(request.form("Login"))
     rs_temp.close
     Session("LoggedOn") = "Success"
     Session("SQLError")= "Success"
   end if 
   
  ' Response.Write sRes1
       
End Function

Function SetNTErrorMessage
  Session("SystemMessage1")= "STATUS : " & Session("NTError") 
  if Session("NTError")  <> "Success" then
     Response.Clear
     Response.Redirect("default.asp")
    ' Response.end
  end if
End Function

Function SetSQLErrorMessage
  Session("SystemMessage1")= "STATUS : " & Session("SQLError") 
  if Session("SQLError")  <> "Success" then
     Response.Clear
     Response.Redirect("default.asp")
     
     'Response.End
  end if
End Function

Function SetSuccessMessage
 Session("SystemMessage1")= "Success"
End Function

Function GetUserFullName
End Function


%>

<html>

<head>
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->


<meta name="VI60_DTCScriptingPlatform" content="Server (ASP)">
<meta name="VI60_defaultClientScript" content="VBScript">
<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">

<title>Mortgage Loans Securitisation System</title>
<script ID="clientEventHandlersVBS" LANGUAGE="vbscript">
<!--


'************************************************************************************
'*
'* Function/Sub : window_onload
'*
'*
'* Description  : Informs the user of a failed or successful logon.
'*
'*
'************************************************************************************

 
Sub window_onload


  s = "<%=NTLogin%>"

  if s = "Success" then
     
     t = "<%=Session("SQLLoginMsg")%>"
     
     if Mid(t,1,7) = "Welcome" then
        <%SetSuccessMessage%>
     '   MsgBox t,,"Mortgage Loan Securitisation - Logon Message" 
         window.document.title = " ** SAHL Mortgage Loan Securitisation System **     [Database : "  & "<%=Session("SQLDatabase")%>" &  "]"    
         window.status = "[ User Name : " & "<%=Session("UserFullName")%>" & " ]       [ User Group : " & "<%=Session("UserGroupName")%>" & " ]" 
         
     End if
     
     if Mid(t,1,7) <> "Welcome" then
         <%SetSQLErrorMessage%>
        exit sub
     End if
     
  else
        'MsgBox s
        <%SetNTErrorMessage%>
        'window.navigate("default.asp")
        exit sub
 
  end if
   
End Sub


Sub window_onfocus

if trim(window.parent.name) <> "" then    
    window.top.document.location.replace "default.asp"
end if

End Sub

Sub window_onbeforeunload
 
'This causes problems when Session Times out ....need to investgate..
' x = "<%= Session("Status1")%>"
' msgbox x
 'if trim(x) <> "" then    
'  window.event.returnValue = ""
 'end if

End Sub



-->
</script>
</head>

<frameset rows="47,*" frameborder="0" framespacing="0">
   
    <frameset cols="140, *">
    
          <frame src="crest.htm" name="crest" marginwidth="0" marginheight="0" noresize scrolling="no">     

          <frame src="TopToolBar.asp" name="toptoolbar" marginwidth="0" marginheight="0" scrolling="no" >
          
    </frameset> 
    
          <frame src="Main.asp" marginwidth="0" marginheight="0" name="main" scrolling="no" noresize>     
      
 </frameset>
 
<p><noframes>
<p><strong>Sorry for the inconvenience, but...</strong>
<p>This web site depends on frames and it appears your browser does not support 
them.  You can download the latest version of Internet Explorer at Microsoft's 
<a HREF="http://www.microsoft.com/ie">Internet Explorer web site</a>.
</noframes>


</html>
