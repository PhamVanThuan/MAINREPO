<html>
<head>
    <!--#include virtual="/SAHL-MLSS/miscutils.inc"-->
    <!--#include file="server.asp"-->
    <object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" viewastext id="Microsoft_Licensed_Class_Manager_1_0" 1>
        <param name="LPKPath" value="APEX.lpk">
    </object>
    <meta name="VI60_DTCScriptingPlatform" content="Server (ASP)">
    <meta name="VI60_defaultClientScript" content="VBScript">
    <meta name="GENERATOR" content="Microsoft Visual Studio 6.0">
    <title></title>
    <%
  sDatabase =Session("SQLDatabase")
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_Reports = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Client Services Reports",Session("UserName"))

sLoanNbr =Request.Form.Item("LastLoanNbr")

    %>
    <script id="clientEventHandlersVBS" language="vbscript">

<!--
dim sess1

'**************************************************************
'*
'*Reports
'*
'**************************************************************

Sub btn_Reports_onclick
 if window.pic_Reports.title = "0" then
      window.status = "Access denied to Reports..."
     ' exit sub
    end if

    window.parent.frames(1).location.href = "ClientServicesReports.asp"
    SetSelectedButton 1

End Sub

Sub  btn_Reports_onmouseout
if  btn_Reports.style.color="lime" then exit sub
	 btn_Reports.style.color="white"

End Sub

Sub  btn_Reports_onmouseover
if   btn_Reports.style.color="lime" then exit sub
	 btn_Reports.style.color="yellow"
End Sub

Sub pic_Reports_onclick
window.btn_Reports.click
End Sub

-->
    </script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<base target="main">
<body leftmargin="0" rightmargin="0" topmargin="0" bottommargin="0" class="Generic">
    <p>
        <input id="btn_Reports" name="btn_Reports" style="cursor: hand; height: 39px; left: 2px;
            position: absolute; top: 48px; width: 110px; z-index: 112" title="Loan Reports"
            type="button" height="39" width="140" class="button2" value="Reports">
        <img alt="" border="0" height="17" hspace="0" id="pic_Reports" src="images/accessdeniedgreen17.bmp"
            style="height: 17px; left: 4px; position: absolute; top: 58px; width: 17px; z-index: 122"
            usemap="" width="17">
    </p>
    <p>
        <input id="CurrentLoanNbr" name="CurrentLoanNbr" value="<%=Request.Querystring("qsLoanNumber") %>"
            style="height: 22px; left: 9px; position: absolute; top: 827px; visibility: hidden;
            width: 69px; z-index: 126">
        <input id="CurrentClientNbr" name="CurrentClientNbr" value="<%=Request.Querystring("qsClientNumber") %>"
            style="height: 22px; left: 10px; position: absolute; top: 849px; visibility: hidden;
            width: 67px; z-index: 127">
        <input id="LastClientSurname" name="text1" style="left: 91px; position: absolute;
            top: 844px; z-index: 130">
    </p>
    <br>
    <input width="200" type="hidden" name="DB" value="<%=Request.Querystring("qsDB") %>" />
</body>
</html>
<script language="vbscript">

Sub window_onload

	SetAccessLightsServer

End Sub

Sub SetAccessLightsServer

    window.btn_Reports.style.backgroundcolor = "SeaGreen"

    sRes1 = "<%=i_Reports%>"
    if sRes1 = "Allowed" then
          window.pic_Reports.src = "images/accessallowedgreen17.bmp"
          window.pic_Reports.title = "1"
    else
          window.pic_Reports.src = "images/accessdeniedgreen17.bmp"
          window.pic_Reports.title = "0"
	end if

End Sub

'**************************************************************
'*
'* SetNormalButtons
'*
'**************************************************************
Sub SetNormalButtons

    window.btn_Reports.style.color="white"

End Sub

'**************************************************************
'*
'* SetSelectedButton
'*
'**************************************************************

Sub SetSelectedButton(iButton)

    SetAccessLightsServer

	Select Case iButton
	     Case 1
                 window.btn_Reports.style.backgroundcolor = "SteelBlue"
	             window.pic_Reports.src = "images/accessallowedsteelblue17.bmp"

	End Select

End Sub
</script>