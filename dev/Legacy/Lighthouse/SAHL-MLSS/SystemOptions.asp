<%
  sDatabase =Session("SQLDatabase")
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_UserGroups = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"User Groups",Session("UserName"))
  i_Functions = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Functions",Session("UserName"))
  i_SystemUsers = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"System Users",Session("UserName"))
  i_GroupAccess = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Group Access",Session("UserName"))
  i_Exports = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Exports",Session("UserName"))
  i_AssetTxfer = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Asset Transfer",Session("UserName"))

%>
<html>
<head>
    <!--#include virtual="/SAHL-MLSS/miscutils.inc"-->
    <meta name="VI60_DTCScriptingPlatform" content="Client (IE 4.0 DHTML)">
    <meta name="VI60_defaultClientScript" content="VBScript">
    <meta name="GENERATOR" content="Microsoft Visual Studio 6.0">
    <title></title>
    <script id="clientEventHandlersVBS" language="vbscript">
<!--
dim sess1

'**************************************************************
'*
'* User Group Tables
'*
'**************************************************************

Sub btn_UserGroup_onclick
    if pic_UserGroup.title = "0" then
      window.status = "Access denied to User Group function....."
      exit sub
    end if

    window.parent.frames("SystemPanel").location.href = "UserGroup.asp"
    SetSelectedButton 1
End Sub

Sub btn_UserGroup_onmouseover
	if btn_UserGroup.style.color="lime" then exit sub
	btn_UserGroup.style.color="yellow"

End Sub

Sub btn_UserGroup_onmouseout
	if btn_UserGroup.style.color="lime" then exit sub
	btn_UserGroup.style.color="white"
End Sub

Sub pic_UserGroup_onclick
	window.btn_UserGroup.click
End Sub

'**************************************************************
'*
'* Function Table
'*
'**************************************************************

Sub btn_Functions_onclick
    if pic_Functions.title = "0" then
      window.status = "Access denied to Functions....."
      exit sub
    end if

    window.parent.frames("SystemPanel").location.href = "Functions.asp"
    SetSelectedButton 2

End Sub

Sub btn_Functions_onmouseover
	if btn_Functions.style.color="lime" then exit sub
	btn_Functions.style.color="yellow"

End Sub

Sub btn_Functions_onmouseout
	if btn_Functions.style.color="lime" then exit sub
	btn_Functions.style.color="white"
End Sub

Sub pic_Functions_onclick
	window.btn_Functions.click
End Sub

'**************************************************************
'*
'* SAHL Employee
'*
'**************************************************************

Sub btn_SAHLEmployee_onclick
    if window.pic_SAHLEmployee.title = "0" then
      window.status = "Access denied to SAHL Employees...."
      exit sub
    end if

    window.parent.frames("SystemPanel").location.href = "SAHLEmployee.asp"
    SetSelectedButton 3

End Sub

Sub btn_SAHLEmployee_onmouseover
	if btn_SAHLEmployee.style.color="lime" then exit sub
	btn_SAHLEmployee.style.color="yellow"

End Sub

Sub btn_SAHLEmployee_onmouseout
	if btn_SAHLEmployee.style.color="lime" then exit sub
	btn_SAHLEmployee.style.color="white"
End Sub

Sub pic_SAHLEmployee_onclick
	window.btn_SAHLEmployee.click
End Sub

'**************************************************************
'*
'* Group Access
'*
'**************************************************************

Sub btn_Access_onclick

    if window.pic_GroupAccess.title = "0" then
      window.status = "Access denied to Group Acess..."
      exit sub
    end if

    window.parent.frames("SystemPanel").location.href = "GroupAccess.asp"
    SetSelectedButton 4
End Sub

Sub btn_Access_onmouseover
	if btn_Access.style.color="lime" then exit sub
	btn_Access.style.color="yellow"

End Sub

Sub btn_Access_onmouseout
	if btn_Access.style.color="lime" then exit sub
	btn_Access.style.color="white"
End Sub

Sub pic_GroupAccess_onclick
	window.btn_Access.click
End Sub

'**************************************************************
'*
'* Asset Transfer
'*
'**************************************************************

Sub btn_AssetTxfer_onclick

   if pic_AssetTransfer.title = "0" then
      window.status = "Access denied to Asset Transfer...."
      exit sub
    end if

    window.parent.frames("SystemPanel").location.href = "http://<%=domain%>/base/plugins/miscellaneous/assettransfer/assettransfer.aspx?Param0=&Param1=&Param2=" & <%=Session("UserID")%> '"AssetTransfer.asp"

    SetSelectedButton 5

End Sub

Sub btn_AssetTxfer_onmouseover
	if btn_AssetTxfer.style.color="lime" then exit sub
	btn_AssetTxfer.style.color="yellow"
End Sub

Sub btn_AssetTxfer_onmouseout
	if btn_AssetTxfer.style.color="lime" then exit sub
	btn_AssetTxfer.style.color="white"
End Sub

Sub pic_AssetTransfer_onclick
	window.btn_AssetTxfer.click
End Sub

'**************************************************************
'*
'* Exports
'*
'**************************************************************

Sub btn_Exports_onclick

    if pic_Exports.title = "0" then
      window.status = "Access denied to Exports...."
      exit sub
    end if

    window.parent.frames("SystemPanel").location.href = "Exports3.asp"
    SetSelectedButton 6
End Sub

Sub btn_Exports_onmouseover
	if btn_Exports.style.color="lime" then exit sub
	btn_Exports.style.color="yellow"

End Sub

Sub btn_Exports_onmouseout
	if btn_Exports.style.color="lime" then exit sub
	btn_Exports.style.color="white"
End Sub

Sub pic_Exports_onclick
	window.btn_Exports.click
End Sub

-->
    </script>
</head>
<!--#include file="server.asp"-->
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<base target="main">
<body leftmargin="0" rightmargin="15" topmargin="0" class="Generic">
    <p style="overflow: auto" title>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <input id="btn_AssetTxfer" name="btn_AssetTxfer" style="cursor: hand; height: 39px;
            left: 0px; position: absolute; top: 268px; width: 110px; z-index: 120" title="Asset Transfer"
            type="button" width="110" height="39" value=" Asset Transfer" class="button2">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <input id="btn_UserGroup" name="btn_UserGroup" style="cursor: hand; height: 39px;
            left: 2px; position: absolute; top: 36px; width: 110px; z-index: 102" title="User Group"
            type="button" height="39" width="110" value="  User Groups" class="button2">
        <img alt border="0" height="17" hspace="0" id="pic_UserGroup" name="pic_UserGroup"
            src="images/accessdeniedgreen17.bmp" style="background-image: url(images/LeftBar.gif);
            height: 17px; left: 4px; position: absolute; top: 46px; width: 17px; z-index: 132"
            title="0" usemap width="17">
        <input id="btn_Functions" name="btn_Functions" style="cursor: hand; height: 39px;
            left: 2px; position: absolute; top: 92px; width: 110px; z-index: 109" title="Functions"
            type="button" height="39" width="110" value="Functions " class="button2">
        <img alt border="0" height="17" hspace="0" id="pic_Functions" src="images/accessdeniedgreen17.bmp"
            style="height: 17px; left: 4px; position: absolute; top: 102px; width: 17px;
            z-index: 111" title="0" usemap width="17">
        <input id="btn_SAHLEmployee" name="btn_SAHLEmployee" style="cursor: hand; height: 39px;
            left: 2px; position: absolute; top: 151px; width: 110px; z-index: 110" title="SAHL Employees"
            type="button" height="39" width="110" value=" System Users" class="button2">
        <img alt border="0" height="17" hspace="0" id="pic_SAHLEmployee" src="images/accessdeniedgreen17.bmp"
            style="height: 17px; left: 4px; position: absolute; top: 161px; width: 17px;
            z-index: 113" title="0" usemap width="17">
        <input id="btn_Access" name="btn_Access" style="cursor: hand; height: 39px; left: 2px;
            position: absolute; top: 210px; width: 110px; z-index: 116" title="Group Access"
            type="button" height="39" width="110" value=" Group Access" class="button2">
        <img alt border="0" height="17" hspace="0" id="pic_GroupAccess" src="images/accessdeniedgreen17.bmp"
            style="height: 17px; left: 4px; position: absolute; top: 221px; width: 17px;
            z-index: 127" title="0" usemap width="17">
        &nbsp;&nbsp;
        <input id="btn_Exports" name="btn_Exports" style="cursor: hand; height: 39px; left: 2px;
            position: absolute; top: 328px; width: 110px; z-index: 122" title="Exports" type="button"
            height="39" width="110" value="Exports " class="button2">
        <img alt border="0" height="17" hspace="0" id="pic_Exports" src="images/accessdeniedgreen17.bmp"
            style="height: 17px; left: 4px; position: absolute; top: 339px; width: 17px;
            z-index: 129" title="0" usemap width="17">
        &nbsp;&nbsp;&nbsp;&nbsp;
        <img alt border="0" height="17" hspace="0" id="pic_AssetTransfer" src="images/accessdeniedgreen17.bmp"
            style="height: 17px; left: 4px; position: absolute; top: 279px; width: 17px;
            z-index: 130" title="0" usemap width="17">
    </p>
</body>
</html>
<script language="vbscript">

Sub window_onload
	window.status = "Loading...."

	window.btn_Access.value = "   Group Access"
	window.btn_AssetTxfer.value = "      Asset Transfer"
	window.btn_Exports.value = "Exports        "
	window.btn_Functions.value = "Functions    "
	window.btn_SAHLEmployee.value = "   System Users"

	SetAccessLightsServer
	window.status = "Ready...."

End Sub

Sub SetAccessLightsServer

    window.btn_UserGroup.style.backgroundColor= "SeaGreen"
    window.btn_Functions.style.backgroundColor= "SeaGreen"
    window.btn_SAHLEmployee.style.backgroundColor= "SeaGreen"
    window.btn_Access.style.backgroundColor= "SeaGreen"
    window.btn_AssetTxfer.style.backgroundColor= "SeaGreen"
    window.btn_Exports.style.backgroundColor= "SeaGreen"

    sRes1 = "<%=i_UserGroups%>"
    if sRes1 = "Allowed" then
          window.pic_UserGroup.src = "images/accessallowedgreen17.bmp"
          window.pic_UserGroup.title = "1"
    else
          window.pic_UserGroup.src = "images/accessdeniedgreen17.bmp"
          window.pic_UserGroup.title = "0"
	end if

	sRes1 = "<%=i_Functions%>"
	if sRes1 = "Allowed" then
          window.pic_Functions.src = "images/accessallowedgreen17.bmp"
          window.pic_Functions.title = "1"
    else
           window.pic_Functions.src = "images/accessdeniedgreen17.bmp"
          window.pic_Functions.title = "0"
	end if

	sRes1 = "<%=i_SystemUsers%>"
	if sRes1 = "Allowed" then
          window.pic_SAHLEmployee.src = "images/accessallowedgreen17.bmp"
          window.pic_SAHLEmployee.title = "1"
    else sRes1 = "Allowed"
          window.pic_SAHLEmployee.src = "images/accessdeniedgreen17.bmp"
          window.pic_SAHLEmployee.title = "0"
	end if

	sRes1 = "<%=i_GroupAccess%>"
	 if sRes1 = "Allowed" then
          window.pic_GroupAccess.src = "images/accessallowedgreen17.bmp"
          window.pic_GroupAccess.title = "1"
    else
          window.pic_GroupAccess.src = "images/accessdeniedgreen17.bmp"
          window.pic_GroupAccess.title = "0"
	end if

	sRes1 = "<%=i_Exports%>"
	if sRes1 = "Allowed" then
          window.pic_Exports.src = "images/accessallowedgreen17.bmp"
          window.pic_Exports.title = "1"
    else
          window.pic_Exports.src = "images/accessdeniedgreen17.bmp"
          window.pic_Exports.title = "0"
	end if

	sRes1 = "<%=i_AssetTxfer%>"
	if sRes1 = "Allowed" then
          window.pic_AssetTransfer.src = "images/accessallowedgreen17.bmp"
          window.pic_AssetTransfer.title = "1"
    else
          window.pic_AssetTransfer.src = "images/accessdeniedgreen17.bmp"
          window.pic_AssetTransfer.title = "0"
	end if

End Sub

'**************************************************************
'*
'* SetNormalButtons
'*
'**************************************************************
Sub SetNormalButtons

window.btn_UserGroup.style.color="white"
window.btn_Functions.style.color="white"
window.btn_SAHLEmployee.style.color="white"
window.btn_Access.style.color="white"
window.btn_EndOfPeriod.style.color="white"
window.btn_Exports.style.color="white"

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
	      	    window.btn_UserGroup.style.backGroundColor = "SteelBlue"
	            window.pic_UserGroup.src = "images/accessallowedgreen17.bmp"
	     Case 2
	      	    window.btn_Functions.style.backGroundColor = "SteelBlue"
	            window.pic_Functions.src = "images/accessallowedgreen17.bmp"
	     Case 3
	      	    window.btn_SAHLEmployee.style.backGroundColor = "SteelBlue"
	            window.pic_SAHLEmployee.src = "images/accessallowedgreen17.bmp"
	     Case 4
	     	    window.btn_Access.style.backGroundColor = "SteelBlue"
	            window.pic_GroupAccess.src = "images/accessallowedgreen17.bmp"
	     Case 5
	     	    window.btn_AssetTxfer.style.backGroundColor = "SteelBlue"
	            window.pic_AssetTransfer.src = "images/accessallowedgreen17.bmp"
	     Case 6
	     	    window.btn_Exports.style.backGroundColor = "SteelBlue"
	            window.pic_Exports.src = "images/accessallowedgreen17.bmp"

	End Select

End Sub
</script>