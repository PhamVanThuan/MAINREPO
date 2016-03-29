<%
  sDatabase =Session("SQLDatabase")
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_Reports = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Securitisation Reports",Session("UserName"))

%>
<html>
<head>
    <!--#include virtual="/SAHL-MLSS/miscutils.inc"-->
    <meta name="VI60_DTCScriptingPlatform" content="Server (ASP)">
    <meta name="VI60_defaultClientScript" content="VBScript">
    <meta name="GENERATOR" content="Microsoft Visual Studio 6.0">
    <title></title>
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

    window.parent.frames("SecuritisationPanel").location.href = "SecuritisationReports.asp"
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
            position: absolute; top: 48px; width: 110px; z-index: 112" title="Reports" type="button"
            height="39" width="140" class="button2" value="Reports">
        <img alt="" border="0" height="17" hspace="0" id="pic_Reports" src="images/accessdeniedgreen17.bmp"
            style="height: 17px; left: 4px; position: absolute; top: 58px; width: 17px; z-index: 122"
            usemap="" width="17">
    </p>
    <p>
        <input id="CurrentMBSNbr" name="text1" style="height: 22px; left: 5px; position: absolute;
            top: 826px; visibility: hidden; width: 90px; z-index: 124" value="0">
        <input id="CurrentSPVNbr" name="text1" style="height: 22px; left: 5px; position: absolute;
            top: 852px; visibility: hidden; width: 90px; z-index: 124" value="-1">
    </p>
</body>
</html>
<script language="vbscript">

Sub window_onload

	SetAccessLightsServer

End Sub

Sub SetAccessLightsServer

window.btn_Reports.style.backgroundColor= "SeaGreen"

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
	             window.btn_Reports.style.backGroundColor = "SteelBlue"
	             window.pic_Reports.src = "images/accessallowedgreen17.bmp"

	End Select

End Sub
</script>