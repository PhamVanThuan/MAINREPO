<%
 sDatabase =Session("SQLDatabase")
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_CATS = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"CATS",Session("UserName"))
  i_Disbursements = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Disbursements",Session("UserName"))
  i_Guarantees = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Guarantees",Session("UserName"))
  i_Reports = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Registration Reports",Session("UserName"))

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
'* CATS
'*
'**************************************************************

Sub btn_CATS_onclick
  window.status = "Ready..."
  if window.pic_CATS.title = "0" then
      window.status = "Access denied to " & btn_CATS.title
      exit sub
    end if

   SetSelectedButton 2

   window.parent.frames("RegistrationPanel").location.href = "RegistrationCATS.asp"

End Sub

Sub btn_CATS_onmouseover
	if btn_CATS.style.color="lime" then exit sub
	btn_CATS.style.color="yellow"

End Sub

Sub btn_CATS_onmouseout
	if btn_CATS.style.color="lime" then exit sub
	btn_CATS.style.color="white"
End Sub

'**************************************************************
'*
'* Guarantees
'*
'**************************************************************
Sub btn_Guarantees_onclick

 window.status = "Ready..."
  if window.pic_Guarantees.title = "0" then
      window.status = "Access denied to Guarantees....."
      exit sub
    end if

   SetSelectedButton 3

   window.parent.frames("RegistrationPanel").location.href = "Guarantee.asp"

End Sub

Sub btn_Guarantees_onmouseout
	if window.btn_Guarantees.style.color="lime" then exit sub
	btn_Guarantees.style.color="white"

End Sub

Sub pic_Guarantees_onclick
    window.btn_Guarantees.click
End Sub
'**************************************************************
'*
'* Disbursments
'*
'**************************************************************

Sub btn_Disbursement_onclick
  window.status = "Ready..."
  if window.pic_Disbursment.title = "0" then
      window.status = "Access denied to Disbursemensts....."
      exit sub
    end if

   SetSelectedButton 1

   window.parent.frames("RegistrationPanel").location.href = "Disbursements.asp"

End Sub

Sub btn_Disbursement_onmouseover
	if btn_Disbursement.style.color="lime" then exit sub
	window.btn_Disbursement.style.color="yellow"

End Sub

Sub btn_Disbursement_onmouseout
	if btn_Disbursement.style.color="lime" then exit sub
	btn_Disbursement.style.color="white"
End Sub

-->
    </script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<base target="main">
<body leftmargin="0" rightmargin="0" topmargin="0" bottommargin="0" background=""
    class="Generic">
    <p>
        <input id="btn_Disbursement" name="btn_Disbursement" style="cursor: hand; height: 39px;
            left: 2px; padding-left: 18px; position: absolute; top: 48px; width: 110px; z-index: 106"
            title="Disbursement" type="button" height="39" width="140" class="button2" value="Disbursements">
        <img alt="" border="0" height="17" hspace="0" id="pic_Disbursment" src="images/accessdeniedgreen17.bmp"
            style="height: 17px; left: 4px; position: absolute; top: 58px; width: 17px; z-index: 116">
    </p>
    <p>
        <input id="btn_CATS" name="btn_CATS" style="cursor: hand; height: 39px; left: 2px;
            text-align: left; padding-left: 18px; position: absolute; top: 91px; width: 110px;
            z-index: 115" title="CATS" type="button" height="39" width="140" class="button2"
            value="CATS">
        <img alt="" border="0" height="17" hspace="0" id="pic_CATS" src="images/accessdeniedgreen17.bmp"
            style="height: 17px; left: 4px; position: absolute; top: 101px; width: 17px;
            z-index: 116">
    </p>
        <p>
        <input id="btn_Guarantees" name="btn_Guarantees" style="cursor: hand; height: 39px; left: 2px;
            text-align: left; padding-left: 18px; position: absolute; top: 134px; width: 110px;
            z-index: 115" title="Guarantees" type="button" height="39" width="140" class="button2"
            value="Guarantees">
        <img alt="" border="0" height="17" hspace="0" id="pic_Guarantees" src="images/accessdeniedgreen17.bmp"
            style="height: 17px; left: 4px; position: absolute; top: 144px; width: 17px;
            z-index: 116">
    </p>
    <p>
        <input id="CurrentLoanNbr" name="CurrentLoanNbr" style="height: 22px; left: 9px;
            position: absolute; top: 827px; visibility: hidden; width: 69px; z-index: 119">
        <input id="CurrentClientNbr" name="CurrentClientNbr" style="height: 22px; left: 10px;
            position: absolute; top: 849px; visibility: hidden; width: 67px; z-index: 120">
    </p>
</body>
</html>
<script language="vbscript">

Sub window_onload
	SetAccessLightsServer
End Sub

Sub SetAccessLightsServer

    window.btn_Disbursement.style.backgroundColor= "SeaGreen"

	sRes1 = "<%=i_Disbursements%>"
	if sRes1 = "Allowed" then
          window.pic_Disbursment.src = "images/accessallowedgreen17.bmp"
          window.pic_Disbursment.title = "1"
    else
          window.pic_Disbursment.src = "images/accessdeniedgreen17.bmp"
          window.pic_Disbursment.title = "0"
	end if

    sRes1 = "<%=i_CATS%>"

	if sRes1 = "Allowed" then
          window.pic_CATS.src = "images/accessallowedgreen17.bmp"
          window.pic_CATS.title = "1"
    else
          window.pic_CATS.src = "images/accessdeniedgreen17.bmp"
          window.pic_CATS.title = "0"
	end if

    sRes1 = "<%=i_Guarantees%>"
	
    if sRes1 = "Allowed" then
          window.pic_Guarantees.src = "images/accessallowedgreen17.bmp"
          window.pic_Guarantees.title = "1"
    else
          window.pic_Guarantees.src = "images/accessdeniedgreen17.bmp"
          window.pic_Guarantees.title = "0"
	end if  

End Sub

'**************************************************************
'*
'* SetNormalButtons
'*
'**************************************************************
Sub SetNormalButtons

    btn_Disbursement.style.color="white"
    btn_CATS.style.color="white"
    btn_Guarantees.style.color="white"

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
	      	    window.btn_Disbursement.style.backGroundColor = "SteelBlue"
	            window.pic_Disbursment.src = "images/accessallowedgreen17.bmp"
         Case 2
	      	    window.btn_CATS.style.backGroundColor = "SteelBlue"
	            window.pic_CATS.src = "images/accessallowedgreen17.bmp"
    	 Case 3
		        window.btn_Guarantees.style.backGroundColor = "SteelBlue"
	            window.pic_Guarantees.src = "images/accessallowedgreen17.bmp"
	End Select

End Sub

</script>