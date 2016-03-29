<%
  sDatabase =Session("SQLDatabase") 
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_UpdClient = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Update Client",Session("UserName"))
  i_ClientMemo = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Client Memo",Session("UserName"))
  i_ClientDependants = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Client Dependants",Session("UserName"))


  sClientNbr =Request.Form.Item("LastClientNbr")
  sLastClientNbr = Request.Cookies("LastClientNumber")
  sLastSurname  = Request.Form.Item("LastClientSurname")


'Response.Write "last is " & sClientNbr
 if sClientNbr <>  "" and sClientNbr > 0 then
   'Response.Write "cookie written -> " & sClientNbr
    sLastClientNbr = sClientNbr
    Response.cookies("LastClientNbr")=sClientNbr
    Response.cookies("LastClientNbr").Expires = "01/01/2010"
 
 else
    sLastClientNbr  = Request.Cookies("LastClientNbr")
    sClientNbr = sLastClientNbr
   'Response.Write "cookie read is " & sLastClientNbr
 end if
 
if sLastSurname <> "" then
  Response.Cookies("LastClientSurname") = sLastSurname
  Response.Cookies("LastClientSurname").Expires = "01/01/2010"
  'Response.Write "cookie written is " & sLastSurname
else
  Response.Cookies("LastClientSurname") = sLastSurname
  Response.Cookies("LastClientSurname").Expires = "01/01/2010"
  'Response.Write "cookie written is " & sLastSurname
end if

sLoanNbr =Request.Form.Item("LastLoanNbr")
if sLoanNbr <>  "" and sLoanNbr > 0 then
	' Response.Write "cookie written -> " & sLoanNbr
	 sLastLoanNbr = sLoanNbr
	 Response.cookies("LastLoanNumber")=sLoanNbr
	 Response.cookies("LastLoanNumber").Expires = "01/01/2010"
end if 

%>
<head>
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->
<!--#include file="server.asp"-->

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>
<meta name="VI60_defaultClientScript" content="VBScript">
<meta name="VI60_DTCScriptingPlatform" "Server (ASP)">
<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<script ID="clientEventHandlersVBS" LANGUAGE="vbscript">
<!--


Dim v_BookMark
Dim i_EmployeeType
Dim i_SAHLEmployeeNumber
Dim i_EmployeeTeamNumber

Dim s_CurrentClientNbr

Dim b_loading 

Dim i_CurrentClientNbr
Dim b_AllDataLoaded
dim gi_LEGALENTITY 
dim  gi_JOINTOWNER 

if rs_open <> true then

  
   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"
    
	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   return
    end if

     
   
	set conn = createobject("ADODB.Connection")
	set rs_Client  = createobject("ADODB.Recordset")
	set rs_Province = createobject("ADODB.Recordset")
	set rs_LoanDetail  = createobject("ADODB.Recordset")
	set rs_CreditCard = createobject("ADODB.Recordset")
	set rs_Insurance = createobject("ADODB.Recordset")
	set rs_Insurance1 = createobject("ADODB.Recordset")
	set rs_Insurance2 = createobject("ADODB.Recordset")

	'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [ManageClient.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	conn.Open sDSN
	rs_open = false
	rs_Client_open = false 
	rs_CreditCard_open = false
	
end if

Sub SetAccessLightsServer
     
     
    sRes1 = "<%=i_UpdClient%>"
    if sRes1 = "Allowed" then
       window.pic_UpdateClient.src = "images/MLSAllowed.bmp"
       window.pic_UpdateClient.title = "1"
    else
       window.pic_UpdateClient.src = "images/MLSDenied.bmp"
       window.pic_UpdateClient.title = "0"
	end if 
	
	 sRes1 = "<%=i_ClientMemo%>"
    if sRes1 = "Allowed" then
       window.pic_ClientMemo.src = "images/MLSAllowed.bmp"
       window.pic_ClientMemo.title = "1"
    else
       window.pic_ClientMemo.src = "images/MLSDenied.bmp"
       window.pic_ClientMemo.title = "0"
	end if
	
	 sRes1 = "<%=i_ClientDependants%>"
    if sRes1 = "Allowed" then
       window.pic_ClientDependants.src = "images/MLSAllowed.bmp"
       window.pic_ClientDependants.title = "1"
    else
       window.pic_ClientDependants.src = "images/MLSDenied.bmp"
       window.pic_ClientDependants.title = "0"
	end if
	  
		
end Sub


Sub SetAccessLights

    sUserName = "<%= Session("UserID")%>"

	set x = CreateObject("MLSSecurity.FunctionClass")
	
	sRes1 = x.CheckFunctionAccess("DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>",window.btn_UpdateClient.title,Trim(sUserName))
     
    if sRes1 = "Allowed" then
          window.pic_UpdateClient.src = "images/MLSAllowed.bmp"
          window.pic_UpdateClient.title = "1"
	end if
	
    sRes1 = x.CheckFunctionAccess("DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>",window.btn_ClientMemo.title,Trim(sUserName))
     
    if sRes1 = "Allowed" then
          window.pic_ClientMemo.src = "images/MLSAllowed.bmp"
          window.pic_ClientMemo.title = "1"
	end if   
	
	 sRes1 = x.CheckFunctionAccess("DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>",window.btn_ClientDependants.title,Trim(sUserName))
     
    if sRes1 = "Allowed" then
          window.pic_ClientDependants.src = "images/MLSAllowed.bmp"
          window.pic_ClientDependants.title = "1"
	end if   
	
	
  
	
End Sub 

Sub GetClientData()
   
   document.body.style.cursor = "wait"
    i_switch = true
   
    
    if rs_open = true  then
       rs_open = false
	end if

    sSQL = "c_GetClientDetails " & i_CurrentClientNbr
    
    'msgbox sSQL
    rs_Client.CursorLocation = 3
	rs_Client.CacheSize  =10
	
    'this.style.cursor = "hand"
    
	rs_Client.Open sSQL,conn,adOpenDynamic
	
	
	rs_Client_open = true
	
	 sSQL = "SELECT * FROM PROVINCE"

	rs_Province.CursorLocation = 3
	rs_Province.Open sSQL ,conn,adOpenStatic
	set DataCombo_HomeProvince.RowSource = rs_Province
	DataCombo_HomeProvince.ListField = rs_Province.Fields("ProvinceName").name
	DataCombo_HomeProvince.BoundColumn = rs_Province.Fields("ProvinceName").name

	DataCombo_HomeProvince.BoundText = rs_Client.Fields("ClientHomeProvince").Value
	DataCombo_HomeProvince.Refresh
	
	
	sSQL = "SELECT ProspectSurvey.ProspectSurveyInsuranceNumber, ProspectSurveyInsurance.ProspectSurveyInsuranceDescription From ProspectSurvey inner join prospectsurveyinsurance on ProspectSurvey.ProspectSurveyInsuranceNumber = ProspectSurveyInsurance.ProspectSurveyInsuranceNumber inner join prospect on prospectsurvey.prospectnumber = prospect.prospectnumber where Prospect.Clientnumber = " & i_CurrentClientNbr 
	'msgbox sSQL
	rs_Insurance1.CursorLocation = 3
	rs_Insurance1.Open sSQL ,conn,adOpenStatic
	
	if rs_Insurance1.RecordCount > 0 then	
			
	sSQL = "SELECT * FROM ProspectSurveyInsurance where ProspectSurveyInsuranceNumber <> 2 and ProspectSurveyStatus = 1"
	''sSQL = "SELECT ProspectSurvey.ProspectSurveyInsuranceNumber,ProspectSurveyInsurance.ProspectSurveyInsuranceDescription FROM ProspectSurvey INNER JOIN ProspectSurveyInsurance ON ProspectSurveyInsurance.ProspectSurveyInsuranceNumber =ProspectSurveyInsurance.ProspectSurveyInsuranceNumber where ProspectNumber = " & i_ProspectNumber 
	''msgbox sSQL
	rs_Insurance2.CursorLocation = 3
	rs_Insurance2.Open sSQL ,conn,adOpenStatic	
	
	set DataCombo_Insurance.RowSource = rs_Insurance2
	DataCombo_Insurance.ListField = rs_Insurance2.Fields("ProspectSurveyInsuranceDescription").name
	DataCombo_Insurance.BoundColumn =  rs_Insurance2.Fields("ProspectSurveyInsuranceNumber").name
	DataCombo_Insurance.BoundText = rs_Insurance1.Fields("ProspectSurveyInsuranceNumber").Value
	DataCombo_Insurance.Refresh
	
	
	else
	
	sSQL = "SELECT * FROM ProspectSurveyInsurance where ProspectSurveyInsuranceNumber <> 2 and ProspectSurveyStatus = 1"
	''msgbox sSQL
	rs_Insurance.CursorLocation = 3
	rs_Insurance.Open sSQL ,conn,adOpenStatic
     
	set DataCombo_Insurance.RowSource = rs_Insurance
	DataCombo_Insurance.ListField = rs_Insurance.Fields("ProspectSurveyInsuranceDescription").name
	DataCombo_Insurance.BoundColumn =  rs_Insurance.Fields("ProspectSurveyInsuranceNumber").name
	DataCombo_Insurance.BoundText = rs_Insurance.Fields("ProspectSurveyInsuranceNumber").Value
	DataCombo_Insurance.Refresh

    end if

     document.body.style.cursor = "default"
     
End Sub


Sub window_onload

 
 'SetAccessLights

 
 SetAccessLightsServer
 b_AllDataLoaded = false
 i_CurrentClientNbr = 0
 i_LastClientNbr = 0
 i_CurrentClientNbr = "<%=sClientNbr%>"
 sLastNbr = "<%=sLastClientNbr%>"




 v = ""
 v = "<%=Request.QueryString("Number")%>"

 if  v <> "" then
	     
  i_CurrentClientNbr = v
  
 else
 
  i_CurrentClientNbr = window.parent.frames(0).CurrentClientNbr.value
 
 end if
    
if sLastNbr <> "" then
    
    i_LastClientNbr =  sLastNbr
    window.parent.frames(0).CurrentClientNbr.value = i_LastClientNbr
 end if

if i_CurrentClientNbr = "" then
   i_CurrentClientNbr = 0
end if

if  i_CurrentClientNbr = 0 and i_LastClientNbr = 0 then
    
	window.location.href = "FindClient.asp"
	window.close
	exit sub
end if

if i_CurrentClientNbr = 0 then
   i_CurrentClientNbr = i_LastClientNbr
end if

 'msgbox "CLIENT NBR >" & i_CurrentClientNbr & "<"

window.parent.frames(0).CurrentClientNbr.value = i_CurrentClientNbr

 gi_LEGALENTITY  = 4
 gi_JOINTOWNER  = 5

 window.ClientIntroductionDate.DropDown.Visible = 1
 window.ClientIntroductionDate.Spin.Visible = 1
 window.ClientBirthDate.DropDown.Visible = 1
 window.ClientBirthDate.Spin.Visible = 1

GetClientData

sSQL = "SELECT DetailTypeNumber from DETAIL WHERE DetailTypeNumber = 10 " _
		& "And LoanNumber not in (select LoanNumber From Detail Where DetailTypeNumber = 451) " _
		& "And LoanNumber = " & rs_Client.Fields("LoanNumber").Value
		
rs_LoanDetail.CursorLocation = 3
rs_LoanDetail.Open sSQL,conn,adOpenStatic
   
'Hide all buttons if the loan is closed and current balance is zero
if rs_LoanDetail.RecordCount > 0 then 
  if Cint(rs_LoanDetail.Fields("DetailTypeNumber").Value) = 10 then 
	   
     window.btn_AdditionalDetails.style.visibility = "hidden"
     window.btn_ClientDependants.style.visibility = "hidden"
     window.btn_ClientMemo.style.visibility = "hidden"
     window.btn_UpdateClient.style.visibility = "hidden"

	 window.pic_ClientDependants.style.visibility = "hidden"
	 window.pic_ClientMemo.style.visibility = "hidden"
	 window.pic_UpdateClient.style.visibility = "hidden"
     window.msg_locked.style.visibility = "visible"
     window.btn_CreditCard.style.visibility = "hidden"
	     
   end if
 end if
  
rs_LoanDetail.Close

window.parent.frames(0).CurrentLoanNbr.value = rs_Client.Fields("LoanNumber").Value

window.focus

End Sub


Sub window_onfocus

if b_AllDataLoaded = false then
    'msgbox rs_Client.Fields("ClientNumber").Value
	window.ClientNumber.Value = rs_Client.Fields("ClientNumber").Value
	window.ClientIntroductionDate.Value = rs_Client.Fields("ClientIntroductionDate").Value
	window.ClientSalutation.Text = rs_Client.Fields("ClientSalutation").Value
	window.ClientFirstNames.Text = rs_Client.Fields("ClientFirstNames").Value
	window.ClientSurname.Text = rs_Client.Fields("ClientSurname").Value
	window.ClientIDNumber.Text = rs_Client.Fields("ClientIDNumber").Value
	window.ClientInitials.Text = rs_Client.Fields("ClientInitials").Value
	window.ClientBirthDate.Value = rs_Client.Fields("ClientBirthDate").Value
	window.ClientInitials.Text = rs_Client.Fields("ClientInitials").Value
	window.ClientSpouseIDNumber.Text = rs_Client.Fields("ClientSpouseIDNumber").Value
	window.ClientSpouseFirstNames.Text = rs_Client.Fields("ClientSpouseFirstNames").Value
	window.ClientSpouseInitials.Text = rs_Client.Fields("ClientSpouseInitials").Value
	window.ClientHomePhoneCode.Text = rs_Client.Fields("ClientHomePhoneCode").Value
	window.ClientHomePhoneNumber.Text = rs_Client.Fields("ClientHomePhoneNumber").Value
	window.ClientWorkPhoneCode.Text = rs_Client.Fields("ClientWorkPhoneCode").Value
    window.ClientWorkPhoneNumber.Text = rs_Client.Fields("ClientWorkPhoneNumber").Value
	window.ClientFAXCode.Text = rs_Client.Fields("ClientFAXCode").Value
    window.ClientFAXNumber.Text = rs_Client.Fields("ClientFAXNumber").Value
	window.ClientCellular.Text = rs_Client.Fields("ClientCellularPhoneNumber").Value
    window.ClientEMail.Text = rs_Client.Fields("ClientEMailAddress").Value


	window.ClientBuildingNumber.Text = rs_Client.Fields("ClientHomeBuildingNumber").Value
	window.ClientBuildingName.Text = rs_Client.Fields("ClientHomeBuildingName").Value
	window.ClientStreetNumber.Text = rs_Client.Fields("ClientHomeStreetNumber").Value
	window.ClientStreetName.Text = rs_Client.Fields("ClientHomeStreetName").Value
	window.ClientSuburb.Text = rs_Client.Fields("ClientHomeSuburb").Value
	window.ClientCity.Text = rs_Client.Fields("ClientHomeCity").Value
	window.ClientProvince.Text = rs_Client.Fields("ClientHomeProvince").Value
	window.ClientCountry.Text = rs_Client.Fields("ClientHomeCountry").Value
	window.ClientPostalCode.Text = rs_Client.Fields("ClientHomePostalCode").Value
	
    rs_Province.Find "ProvinceName = '" & rs_Client.Fields("ClientHomeProvince").Value & "'"
	DataCombo_HomeProvince.BoundText = rs_Client.Fields("ClientHomeProvince").Value
	
	window.ClientBoxNumber.Text = rs_Client.Fields("ClientBoxNumber").Value
	window.ClientPostOffice.Text = rs_Client.Fields("ClientPostOffice").Value
	window.ClientPhysicalPostalCode.Text = rs_Client.Fields("ClientPostalCode").Value
	

	if Cint(rs_Client.Fields("SexNumber").Value) = gi_JOINTOWNER then
	   window.lbl_spousefirstnames.innerText = "Joint Names"
	   window.lbl_SpouseInitials.innerText = "Joint Initials"
	elseif Cint(rs_Client.Fields("SexNumber").Value) = gi_LEGALENTITY then
	   window.lbl_firstnames.style.visibility = "hidden"
	   window.ClientFirstNames.style.visibility = "hidden"
	   window.lbl_Salutation.style.visibility = "hidden"
	   window.ClientSalutation.style.visibility = "hidden"
	   window.lbl_spousefirstnames.style.visibility = "hidden"
	   window.ClientSpouseFirstNames.style.visibility = "hidden"
	   window.lbl_SpouseInitials.style.visibility = "hidden"
	   window.ClientSpouseInitials.style.visibility = "hidden"
	   window.lbl_Surname.innerText = "Legal Entity Name"
	   window.lbl_idnumber.innerText = "Legal Entity Number"
	   window.lbl_initials.style.visibility = "hidden"
	   window.ClientInitials.style.visibility = "hidden"
	   window.lbl_spouseidnumber.style.visibility = "hidden"
	   window.ClientSpouseIDNumber.style.visibility = "hidden"
	end if
	
	s_Source = "<%=Request.QueryString("Source")%>"
    if s_Source = "OutboundClientDetail.asp" then
		window.btn_CancelClientAction.style.visibility = "visible"
		DisableAllControls		
	End if	
	
	if s_Source = "OutboundClientDocScript.asp" then
		window.btn_CancelClientAction.style.visibility = "visible"
		DisableAllControls	
	End if	

    window.focus
	b_AllDataLoaded = true

end if
End Sub

Sub EnableFields
	'window.ClientNumber.Enabled = True
	'window.ClientIntroductionDate.Enabled = True
	window.ClientSalutation.Enabled = True
	window.ClientFirstNames.Enabled = True
	window.ClientSurname.Enabled = True
	window.ClientIDNumber.Enabled = True
	window.ClientInitials.Enabled = True
	window.ClientBirthDate.Enabled = True
	window.ClientInitials.Enabled = True
	window.ClientSpouseIDNumber.Enabled = True
	window.ClientSpouseFirstNames.Enabled = True
	window.ClientSpouseInitials.Enabled = true
	window.ClientHomePhoneCode.Enabled = True
	window.ClientHomePhoneNumber.Enabled = True
	window.ClientWorkPhoneCode.Enabled = True
    window.ClientWorkPhoneNumber.Enabled = True
	window.ClientFAXCode.Enabled = True
    window.ClientFAXNumber.Enabled = True
	window.ClientCellular.Enabled = True
    window.ClientEMail.Enabled = True


	window.ClientBuildingNumber.Enabled = True
	window.ClientBuildingName.Enabled = True
	window.ClientStreetNumber.Enabled = True
	window.ClientStreetName.Enabled = True
	window.ClientSuburb.Enabled = True
	window.ClientCity.Enabled = True
	window.DataCombo_HomeProvince.Enabled = True
	window.ClientCountry.Enabled = True
	window.ClientPostalCode.Enabled = True
	
	window.ClientBoxNumber.Enabled = True
	window.ClientPostOffice.Enabled = True
	window.ClientPhysicalPostalCode.Enabled = True
	window.Datacombo_Insurance.enabled = true

End Sub

Sub DisableFields
	window.ClientNumber.Enabled = False
	window.ClientIntroductionDate.Enabled = False
	window.ClientSalutation.Enabled = False
	window.ClientFirstNames.Enabled = False
	window.ClientSurname.Enabled = False
	window.ClientIDNumber.Enabled = False
	window.ClientInitials.Enabled = False
	window.ClientBirthDate.Enabled = False
	window.ClientInitials.Enabled = False
	window.ClientSpouseIDNumber.Enabled = False
	window.ClientSpouseFirstNames.Enabled = False
	window.ClientSpouseInitials.Enabled = False
	window.ClientHomePhoneCode.Enabled = False
	window.ClientHomePhoneNumber.Enabled = False
	window.ClientWorkPhoneCode.Enabled = False
    window.ClientWorkPhoneNumber.Enabled = False
	window.ClientFAXCode.Enabled = False
    window.ClientFAXNumber.Enabled = False
	window.ClientCellular.Enabled = False
    window.ClientEMail.Enabled = False


	window.ClientBuildingNumber.Enabled = False
	window.ClientBuildingName.Enabled = False
	window.ClientStreetNumber.Enabled = False
	window.ClientStreetName.Enabled = False
	window.ClientSuburb.Enabled = False
	window.ClientCity.Enabled = False
	window.DataCombo_HomeProvince.Enabled = False
	window.ClientCountry.Enabled = False
	window.ClientPostalCode.Enabled = False
	
	window.ClientBoxNumber.Enabled = False
	window.ClientPostOffice.Enabled = False
	window.ClientPhysicalPostalCode.Enabled = False
	window.Datacombo_Insurance.enabled = false

End Sub

Sub btn_UpdateClient_onclick
 
 if window.pic_UpdateClient.title = "0" then
      window.status = "Access denied to " & window.btn_UpdateClient.title
      exit sub
    end if

  if btn_UpdateClient.value = "Update Client" then

     DisableAllControls
 
	'Enable Fields
	EnableFields
	window.btn_CancelClientAction.style.visibility = "visible"
	

	'Set Focus 
    'Set the button caption..
	btn_UpdateClient.value = "Commit"  
	if Cint(rs_Client.Fields("SexNumber").Value) <> gi_LEGALENTITY then
		window.ClientSalutation.HighlightText = true
		window.ClientSalutation.focus()
	else
		window.ClientSurname.HighlightText = true
		window.ClientSurname.focus()
	end if
	
  
  elseif btn_UpdateClient.value = "Commit" then

	if ValidateFields() = 0 then
  		call UpdateClientRecord()
  	else
  		exit sub
  	end if
	'Clean up...

	btn_UpdateClient.value = "Update Client"
	btn_CancelClientAction.style.visibility = "hidden"
	
	
	s_Source = "<%=Request.QueryString("Source")%>"
	if s_Source = "OutboundClientDetail.asp" then
		window.btn_CancelClientAction.value = "Exit"
		window.btn_CancelClientAction.style.visibility = "visible"
		DisableAllControls	
	End if	
	
	if s_Source = "OutboundClientDocScript.asp" then
		window.btn_CancelClientAction.value = "Exit"
		window.btn_CancelClientAction.style.visibility = "visible"
		DisableAllControls	
	End if	
		
	DisableFields()
	EnableAllControls

	End if
	

End Sub

Function ValidateFields()

ValidateFields = -1

	if trim(window.ClientSpouseIDNumber.Text) = "0" then
		msgbox "A Valid Spouse ID Number must be captured else use an empty field",,"Manage Client"
		window.ClientSpouseIDNumber.Enabled =true
		window.ClientSpouseIDNumber.focus
		exit function
	
	end if
	
	if Window.Datacombo_Insurance.text = "" then
		MsgBox "Insurance request needs to be captured"
		window.Datacombo_Insurance.enabled = true
		window.Datacombo_Insurance.focus()
		exit function
	end if
	
	if Window.Datacombo_Insurance.text = "null" then
		MsgBox "Insurance request needs to be captured"
		window.Datacombo_Insurance.enabled = true
		window.Datacombo_Insurance.focus()
		exit function
	end if
	
ValidateFields = 0

End Function

Function ValidateBarclayCardFields()
ValidateBarclayCardFields = -1
if rs_Client.Fields("SexNumber").Value = "5"  then 
		MsgBox "Cannot offer a SAHL Gold Card to a Joint Ownership client",vbOKOnly, "SAHL/BarclayCard GoldCard"
		ValidateBarclayCardFields = -1
		exit function
	end if

	if (rs_Client.Fields("LegalStatusNumber").Value = "1" or rs_Client.Fields("LegalStatusNumber").Value = "5" or rs_Client.Fields("LegalStatusNumber").Value = "6" or rs_Client.Fields("LegalStatusNumber").Value = "7" or rs_Client.Fields("LegalStatusNumber").Value = "9" ) then
		MsgBox "Cannot offer a SAHL Gold Card to a " & rs_Client.Fields("LegalStatusDescription").Value & " client",vbOKOnly, "SAHL/BarclayCard GoldCard"
		ValidateBarclayCardFields = -1
		exit function
	end if

if (rs_Client.Fields("LegalStatusNumber").Value = "3" or rs_Client.Fields("LegalStatusNumber").Value = "4" ) then
	If Cint(InStr(rs_Client.Fields("ClientFirstNames").Value,"&")) > 0 then
		MsgBox "Cannot offer a SAHL Gold Card to a " & rs_Client.Fields("LegalStatusDescription").Value & " client",vbOKOnly, "SAHL/BarclayCard GoldCard"
		ValidateBarclayCardFields = -1
		exit function
	end if
end if

if rs_Client.Fields("LegalStatusNumber").Value = "8" then
	if Cint(InStr(rs_Client.Fields("ClientFirstNames").Value,"&")) > 0 then
		MsgBox "Cannot offer a SAHL Gold Card to a Divorced client with a partner",vbOKOnly, "SAHL/BarclayCard GoldCard"
		ValidateBarclayCardFields = -1
		exit function
	end if 
end if

if rs_Client.Fields("LoanArrearBalance").Value > "0" then
	MsgBox "Cannot offer a SAHL Gold Card to a client whose account is in arrears",vbOKOnly, "SAHL/BarclayCard GoldCard"
	ValidateBarclayCardFields = -1
	exit function
end if
ValidateBarclayCardFields = 0

End Function

Sub DisableAllControls
      
	window.btn_AdditionalDetails.disabled = true
	window.btn_ClientMemo.disabled = true
	window.btn_ClientDependants.disabled = true
   
	window.btn_AdditionalDetails.style.visibility = "hidden"
	window.btn_ClientDependants.style.visibility = "hidden"
	window.btn_ClientMemo.style.visibility = "hidden"
	window.pic_ClientDependants.style.visibility = "hidden"
	window.pic_ClientMemo.style.visibility = "hidden"

End sub

Sub EnableAllControls
      
    window.btn_AdditionalDetails.disabled = false
    window.btn_ClientMemo.disabled = false
    window.btn_ClientDependants.disabled = false

	window.btn_AdditionalDetails.style.visibility = "visible"
	window.btn_ClientDependants.style.visibility = "visible"
	window.btn_ClientMemo.style.visibility = "visible"
	window.pic_ClientDependants.style.visibility = "visible"
	window.pic_ClientMemo.style.visibility = "visible"

End sub


Function UpdateClientRecord()
	
Dim i_res


UpdateClientRecord = -1

    document.body.style.cursor = "hand"
    
  
    i_res = 0     
    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")
    set rs_temp1 = createobject("ADODB.Recordset")
    'Cannot use OLE DB Provider as it appears that it does not return a recordset
    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS [ManageClient.asp 2];uid=<%= Session("UserID")%>"
  '  sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [manageclient.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	
	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc

	sSQL = "c_UpdClientDetails"  


	com.CommandText = sSQL
	
	set prm = com.CreateParameter ( "ClientNumber",19,1,,window.ClientNumber.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "Salutation",200,1,15,window.ClientSalutation.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "FirstNames",200,1,80,window.ClientFirstNames.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "Surname",200,1,80,window.ClientSurname.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "Initials",200,1,20,window.ClientInitials.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "IDNumber",200,1,25,window.ClientIDNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
     s_date = Mid(window.ClientBirthDate.Text, 4, 2) & "/" & Mid(window.ClientBirthDate.Text, 1, 2) & "/" & Mid(window.ClientBirthDate.Text, 7, 4)

	set prm = com.CreateParameter ( "BirthDate",129,1,10,s_date) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "SpouseFirstNames",200,1,80,window.ClientSpouseFirstNames.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "SpouseIDNumber",200,1,25,window.ClientSpouseIDNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "SpouseInitials",200,1,20,window.ClientSpouseInitials.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "HomePhoneCode",200,1,10,window.ClientHomePhoneCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "HomePhoneNbr",200,1,20,window.ClientHomePhoneNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "WorkWorkPhoneCode",200,1,15,window.ClientWorkPhoneCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "WorkWorkPhoneNbr",200,1,20,window.ClientWorkPhoneNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "WorkFAXCode",200,1,15,window.ClientFAXCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "WorkFAXNbr",200,1,20,window.ClientFAXNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "CellNbr",200,1,20,window.ClientCellular.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "EMail",200,1,50,window.ClientEMail.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "HomeBuildingNbr",200,1,10,window.ClientBuildingNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "HomeBuildingName",200,1,50,window.ClientBuildingName.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "HomeStreetNbr",200,1,10,window.ClientStreetNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "HomeStreetName",200,1,50,window.ClientStreetName.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "HomeSuburb",200,1,50,window.ClientSuburb.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "HomeCity",200,1,50,window.ClientCity.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
    
    
	set prm = com.CreateParameter ( "HomeProvince",200,1,50,window.ClientProvince.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "HomeCountry",200,1,50,window.ClientCountry.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "HomePostalCode",200,1,10,window.ClientPostalCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "HomeBoxNumber",200,1,20,window.ClientBoxNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "HomePostOffice",200,1,50,window.ClientPostOffice.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "PostalCode",200,1,10,window.ClientPhysicalPostalCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ProspectSurveyInsuranceNumber",19,1,,CInt(window.DataCombo_Insurance.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm


    set rs_temp = com.Execute 
    
    document.body.style.cursor = "default"




    UpdateClientRecord = 0
	
End Function

Sub btn_CancelClientAction_onclick

	s_Source = "<%=Request.QueryString("Source")%>"
	i_RequestNbr = "<%=Request.QueryString("RequestNbr")%>"
	i_LoanNbr = "<%=Request.QueryString("LoanNbr")%>"
	 i_CurrentClientNbr = rs_Client.Fields("ClientNumber").value
	
	 if s_Source = "OutboundClientDetail.asp" then
		window.parent.frames(1).Location.href = "OutboundClientDetail.asp?Number= " & CStr(i_CurrentClientNbr) & "&Source=ManageClient.asp"
    end if
    
    if s_Source = "OutboundClientDocScript.asp" then
		window.parent.frames(1).Location.href = "OutboundClientDocScript.asp?Number= " & CStr(i_CurrentClientNbr) & "&Source=ManageClient.asp"
    end if
    
    btn_UpdateClient.value = "Update Client"
	btn_CancelClientAction.style.visibility = "hidden"
	'EnableAllControls
	
	DisableFields()

	rs_Client.Requery
	b_AllDataLoaded = false
	
    EnableAllControls
    window.focus
    
End Sub

Sub btn_AdditionalDetails_onclick
     i_CurrentClientNbr = rs_Client.Fields("ClientNumber").value
	 window.parent.frames("ClientServicesPanel").location.href = "ClientAdditional.asp?Number= " & CStr(i_CurrentClientNbr) & "&Source=ManageClient.asp&Action=Update"
End Sub

Sub btn_ClientMemo_onclick
     
     i_CurrentClientNbr = rs_Client.Fields("ClientNumber").value
	 window.parent.frames("ClientServicesPanel").location.href = "ClientMemo.asp?Number= " & CStr(i_CurrentClientNbr) & "&Source=ManageClient.asp&Action=Update"

End Sub

Sub btn_ClientDependants_onclick
     
     i_CurrentClientNbr = rs_Client.Fields("ClientNumber").value
	 window.parent.frames("ClientServicesPanel").location.href = "ClientDependant.asp?Number= " & CStr(i_CurrentClientNbr) & "&Source=ManageClient.asp&Action=Update"

End Sub

Sub DataCombo_HomeProvince_Change
window.ClientProvince.Text  = window.DataCombo_HomeProvince.text


End Sub

Sub btn_CreditCard_onclick
	if ValidatebarclaycardFields() = 0 then
		i_Resp = MsgBox("Are you sure you wish to process a SAHL Gold Card Application for this client?", 4 , " SA Home Loans/BarclayCard Gold Card Application")
		if i_Resp = 6 then
			if rs_CreditCard_open = true then
				rs_CreditCard.Close
				rs_CreditCard_open = false
			end if
			sUserName = "<%= Session("UserName")%>"
			
			'Change made by ShomaS to include User ID however other changes not ready so commented out
			''''''''sSQL = "bc_AddUpdatebarclayCardRecord " & CStr(i_CurrentClientNbr) & ", 'Client','','" & RTrim(sUserName) & "'"
			
			
			sSQL = "bc_AddUpdatebarclayCardRecord " & CStr(i_CurrentClientNbr) & ", 'Client',''"
			rs_CreditCard.CursorLocation = 3
			rs_CreditCard.CacheSize  =10
			rs_CreditCard.Open sSQL,conn,adOpenDynamic
			rs_CreditCard_open = true
			document.clear()
								
			'document.write "<iframe frameborder=0 height=100% width=100% src='http://<%=domain%>/base/plugins/miscellaneous/barclaycard/BarclayCardApp.aspx?Mid=101&param0=" & rs_Client.Fields("LoanNumber").Value & "&param1=0&param2=" & replace(window.ClientFirstNames.text,"&"," and ") & " " & replace(window.ClientSurname.text, "&", " and ")& "' />" 's_ReturnPage & "?ProspectNumber= " & CStr(i_ProspectNumber ) & "&FirstNames=" & s_FirstNames &"&SurName=" & s_SurName
			 ''msgbox "<iframe frameborder=0 height=100% width=100% src='http://<%=domain%>/base/plugins/miscellaneous/barclaycard/BarclayCardApp.aspx?Mid=101&param0=0&param1=" & rs_Client.Fields("LoanNumber").Value & "&param2=" & replace(window.ClientFirstNames.text,"&"," and ") & " " & replace(window.ClientSurname.text, "&", " and ")& "' />" 
			
			
			document.write "<iframe frameborder=0 height=100% width=100% src='http://<%=domain%>/base/plugins/miscellaneous/barclaycard/BarclayCardApp.aspx?Mid=101&param0=0&param1=" & rs_Client.Fields("LoanNumber").Value & "&param2=" & replace(window.ClientFirstNames.text,"&"," and ") & " " & replace(window.ClientSurname.text, "&", " and ")& "' />" 						
			
			'sSQL = "Select * from vw_AllOpenLoans where ClientNumber=" & CStr(i_CurrentClientNbr)
			'rs_CreditCard.CursorLocation = 3
			'rs_CreditCard.CacheSize  =10
			'rs_CreditCard.Open sSQL,conn,adOpenDynamic
			'rs_CreditCard_open = true
			
			''msgbox "Loan Number: " &  rs_Client.Fields("LoanNumber").Value & "ClientName: " &  rs_Client.Fields("ClientName").Value & "ClientSurname: " &  rs_Client.Fields("ClientSurname").Value & "ACBBankDescription: " &  rs_Client.Fields("ACBBankDescription").Value	& "ACBBranchCode: " &  rs_Client.Fields("ACBBranchCode").Value & "ACBTypeDescription: " &  rs_Client.Fields("ACBTypeDescription").Value 	 
			
			
			
			
		document.location.reload()
			'window.parent.frames("ClientServicesPanel").location.href = "ClientDependant.asp?Number= " & CStr(i_CurrentClientNbr) & "&Source=ManageClient.asp&Action=Update"
		end if
	end if
end sub

-->
</script>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">

 </HEAD>

<body  bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" class=Generic>


<p><input id="btn_CancelClientAction" name="btn_CancelClientAction" style="Z-INDEX: 108; LEFT: 458px; VISIBILITY: hidden; WIDTH: 136px; CURSOR: hand; POSITION: absolute; TOP: 448px; HEIGHT: 55px" title            ="Cancel" type="button" value="Cancel" class=button3>
<table border="0" cellPadding="1" cellSpacing="1" style="Z-INDEX: 
99; LEFT: 
30px; WIDTH: 
859px; POSITION: 
absolute; TOP: 
0px; HEIGHT: 
498px" width ="75%" class=Table1>
  
  <tr>
    <td align="right" noWrap >Client Number</td>
    <td noWrap>
      <OBJECT id=ClientNumber 
      style="LEFT: 1px; WIDTH: 99px; TOP: 1px; HEIGHT: 22px" height=22 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="2619">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="AlignHorizontal" VALUE="1">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="0">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="ClearAction" VALUE="0">
	<PARAM NAME="DecimalPoint" VALUE=".">
	<PARAM NAME="DisplayFormat" VALUE="####0;;Null">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="######0">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxValue" VALUE="99999999">
	<PARAM NAME="MinValue" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="NegativeColor" VALUE="255">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="Separator" VALUE=",">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ValueVT" VALUE="2012741633">
	<PARAM NAME="Value" VALUE="0">
	<PARAM NAME="MaxValueVT" VALUE="5">
	<PARAM NAME="MinValueVT" VALUE="5">
	</OBJECT>
</td>
    <td noWrap></td>
    <td align="center" noWrap class=Header1              
      
    height=22 >Residential Address</td></tr>
  <tr>
    <td align="right" noWrap >Introduction Date</td>
    <td noWrap>
      <OBJECT id=ClientIntroductionDate 
      style="LEFT: 1px; WIDTH: 125px; TOP: -1px; HEIGHT: 22px" tabIndex=0 
      height=22 classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3307">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="1">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="CursorPosition" VALUE="0">
	<PARAM NAME="DataProperty" VALUE="0">
	<PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="FirstMonth" VALUE="1">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="dd/mm/yyyy">
	<PARAM NAME="HighlightText" VALUE="2">
	<PARAM NAME="IMEMode" VALUE="3">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxDate" VALUE="2958465">
	<PARAM NAME="MinDate" VALUE="-657434">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="PromptChar" VALUE="_">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="1">
	<PARAM NAME="ShowLiterals" VALUE="0">
	<PARAM NAME="TabAction" VALUE="0">
	<PARAM NAME="Text" VALUE="__/__/____">
	<PARAM NAME="ValidateMode" VALUE="0">
	<PARAM NAME="ValueVT" VALUE="1179649">
	<PARAM NAME="Value" VALUE="2.63417926253582E-308">
	<PARAM NAME="CenturyMode" VALUE="0">
	</OBJECT>
</td>
    <td noWrap height=22 ></td>
    <td noWrap height=22 >Number&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Name 
    </td></tr>
  <tr>
    <td align="right" noWrap id="lbl_Salutation" >Salutation</td>
    <td noWrap>
      <OBJECT id=ClientSalutation 
      style="LEFT: 1px; WIDTH: 226px; TOP: 1px; HEIGHT: 21px" tabIndex=1 
      height=21 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="5980">
	<PARAM NAME="_ExtentY" VALUE="556">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td align="right" noWrap >Building</td>
    <td noWrap>
      <OBJECT id=ClientBuildingNumber 
      style="LEFT: 1px; WIDTH: 59px; TOP: 1px; HEIGHT: 22px" tabIndex=18 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="1561">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
	
      <OBJECT id=ClientBuildingName 
      style="LEFT: 60px; WIDTH: 223px; TOP: 1px; HEIGHT: 22px" tabIndex=19 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="5900">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap id="lbl_firstnames" >First Names</td>
    <td noWrap>
      <OBJECT id=ClientFirstNames 
      style="LEFT: 1px; WIDTH: 280px; TOP: 1px; HEIGHT: 22px" tabIndex=2 
      height=22 width=280 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="7408">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td align="right" noWrap >Street</td>
    <td noWrap>
      <OBJECT id=ClientStreetNumber 
      style="LEFT: 1px; WIDTH: 59px; TOP: 1px; HEIGHT: 22px" tabIndex=20 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="1561">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
	
      <OBJECT id=ClientStreetName 
      style="LEFT: 60px; WIDTH: 223px; TOP: 1px; HEIGHT: 22px" tabIndex=21 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="5900">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap id="lbl_Surname" >Surname</td>
    <td noWrap>
      <OBJECT id=ClientSurname 
      style="LEFT: 1px; WIDTH: 280px; TOP: 1px; HEIGHT: 22px" tabIndex=2 
      height=22 width=280 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="7408">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td align="right" noWrap >Suburb</td>
    <td noWrap>
      <OBJECT id=ClientSuburb 
      style="LEFT: 1px; WIDTH: 216px; TOP: 1px; HEIGHT: 22px" tabIndex=22 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="5715">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap id="lbl_initials" >Initials</td>
    <td noWrap>
      <OBJECT id=ClientInitials 
      style="LEFT: 1px; WIDTH: 180px; TOP: 1px; HEIGHT: 22px" tabIndex=4 
      height=22 width=180 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4763">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td align="right" noWrap >City</td>
    <td noWrap>
      <OBJECT id=ClientCity 
      style="LEFT: 1px; WIDTH: 216px; TOP: 1px; HEIGHT: 22px" tabIndex=23 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="5715">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap id="lbl_idnumber" >ID Number</td>
    <td noWrap>
      <OBJECT id=ClientIDNumber 
      style="LEFT: 1px; WIDTH: 244px; TOP: 1px; HEIGHT: 22px" tabIndex=5 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="6456">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td align="right" noWrap >Province</td>
    <td noWrap>
      <OBJECT id=DataCombo_HomeProvince style="WIDTH: 216px; HEIGHT: 22px" 
      height=22 width=216 classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10>
	<PARAM NAME="_ExtentX" VALUE="5715">
	<PARAM NAME="_ExtentY" VALUE="688">
	<PARAM NAME="_Version" VALUE="393216">
	<PARAM NAME="IntegralHeight" VALUE="-1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="Locked" VALUE="0">
	<PARAM NAME="MatchEntry" VALUE="0">
	<PARAM NAME="SmoothScroll" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Style" VALUE="0">
	<PARAM NAME="CachePages" VALUE="3">
	<PARAM NAME="CachePageSize" VALUE="50">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="BackColor" VALUE="-1">
	<PARAM NAME="ForeColor" VALUE="-1">
	<PARAM NAME="ListField" VALUE="">
	<PARAM NAME="BoundColumn" VALUE="">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="RightToLeft" VALUE="0">
	<PARAM NAME="DataMember" VALUE="">
	</OBJECT>
	
      <OBJECT id=ClientProvince 
      style="Z-INDEX: 109; LEFT: 15px; VISIBILITY: visible; WIDTH: 238px; POSITION: absolute; TOP: 718px; HEIGHT: 24px" 
      height=24 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="6297">
	<PARAM NAME="_ExtentY" VALUE="635">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap >Birth Date</td>
    <td noWrap>
      <OBJECT id=ClientBirthDate 
      style="LEFT: 1px; WIDTH: 125px; TOP: -1px; HEIGHT: 22px" tabIndex=6 
      height=22 classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3307">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="1">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="CursorPosition" VALUE="0">
	<PARAM NAME="DataProperty" VALUE="0">
	<PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="FirstMonth" VALUE="1">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="dd/mm/yyyy">
	<PARAM NAME="HighlightText" VALUE="2">
	<PARAM NAME="IMEMode" VALUE="3">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxDate" VALUE="2958465">
	<PARAM NAME="MinDate" VALUE="-657434">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="PromptChar" VALUE="_">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="1">
	<PARAM NAME="ShowLiterals" VALUE="0">
	<PARAM NAME="TabAction" VALUE="0">
	<PARAM NAME="Text" VALUE="__/__/____">
	<PARAM NAME="ValidateMode" VALUE="0">
	<PARAM NAME="ValueVT" VALUE="1179649">
	<PARAM NAME="Value" VALUE="2.63417926253582E-308">
	<PARAM NAME="CenturyMode" VALUE="0">
	</OBJECT>
</td>
    <td align="right" noWrap >Country</td>
    <td noWrap>
      <OBJECT id=ClientCountry 
      style="LEFT: 1px; WIDTH: 216px; TOP: 1px; HEIGHT: 22px" tabIndex=25 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="5715">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap id="lbl_spousefirstnames" >Spouse First Names</td>
    <td noWrap>
      <OBJECT id=ClientSpouseFirstNames 
      style="LEFT: 1px; WIDTH: 280px; TOP: 1px; HEIGHT: 22px" tabIndex=2 
      height=22 width=280 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="7408">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td align="right" noWrap >&nbsp; Postal Code</td>
    <td noWrap>
      <OBJECT id=ClientPostalCode 
      style="LEFT: 1px; WIDTH: 94px; TOP: 1px; HEIGHT: 22px" tabIndex=26 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="2487">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap id="lbl_spouseidnumber" >Spouse ID Number</td>
    <td noWrap>
      <OBJECT id=ClientSpouseIDNumber 
      style="LEFT: 60px; WIDTH: 200px; TOP: 1px; HEIGHT: 22px" tabIndex=8 
      height=22 width=200 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="5292">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td noWrap></td>
    <td align="center" noWrap class=Header1                
   >Postal Address</td></tr>
  <tr>
    <td noWrap align="right" id="lbl_SpouseInitials" >Spouse Initials</td>
    <td noWrap>
      <OBJECT id=ClientSpouseInitials 
      style="LEFT: 5px; WIDTH: 180px; TOP: 1px; HEIGHT: 22px" tabIndex=9 
      height=22 width=180 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4763">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td align="right" noWrap >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      Box Number</td>
    <td noWrap>
      <OBJECT id=ClientBoxNumber 
      style="LEFT: 1px; WIDTH: 115px; TOP: 1px; HEIGHT: 22px" tabIndex=27 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3043">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td></tr>
  <tr>
    <td noWrap></td>
    <td align="center" noWrap > </td>
    <td align="right" noWrap >Post Office</td>
    <td noWrap>
      <OBJECT id=ClientPostOffice 
      style="LEFT: 1px; WIDTH: 240px; TOP: 1px; HEIGHT: 22px" tabIndex=28 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="6350">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td></tr>
<tr>
<td></td>
<td></td>
<td align="right" noWrap >Postal Code</td>
<td noWrap>
      <OBJECT id=ClientPhysicalPostalCode 
      style="LEFT: 1px; WIDTH: 94px; TOP: 1px; HEIGHT: 22px" tabIndex=29 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="2487">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
</tr>
  <tr>
    <td noWrap></td>
    <td noWrap align="center" class             
      
    =Header1 >Contact 
    Details</td>
    <td align="right" noWrap ></td>
    <td noWrap>
</td></tr>
  <tr>
    <td align="right" noWrap > </td>
    <td noWrap >
      Code&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Number
</td>
    <td noWrap>Insurance Request</td>
    <td noWrap>
      <OBJECT id=Datacombo_Insurance style="WIDTH: 216px; HEIGHT: 22px" 
      height=22 width=216 classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10>
	<PARAM NAME="_ExtentX" VALUE="5715">
	<PARAM NAME="_ExtentY" VALUE="688">
	<PARAM NAME="_Version" VALUE="393216">
	<PARAM NAME="IntegralHeight" VALUE="-1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="Locked" VALUE="0">
	<PARAM NAME="MatchEntry" VALUE="0">
	<PARAM NAME="SmoothScroll" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Style" VALUE="0">
	<PARAM NAME="CachePages" VALUE="3">
	<PARAM NAME="CachePageSize" VALUE="50">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="BackColor" VALUE="-1">
	<PARAM NAME="ForeColor" VALUE="-1">
	<PARAM NAME="ListField" VALUE="">
	<PARAM NAME="BoundColumn" VALUE="">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="RightToLeft" VALUE="0">
	<PARAM NAME="DataMember" VALUE="">
	</OBJECT></td></tr>
  <tr>
    <td align="right" noWrap >Home Phone</td>
    <td noWrap>
      <OBJECT id=ClientHomePhoneCode 
      style="LEFT: 1px; WIDTH: 59px; TOP: 1px; HEIGHT: 22px" tabIndex=10 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="1561">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
	
      <OBJECT id=ClientHomePhoneNumber 
      style="LEFT: 60px; WIDTH: 184px; TOP: 1px; HEIGHT: 22px" tabIndex=11 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4868">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td noWrap></td>
    <td noWrap></td></tr>
  <tr>
    <td align="right" noWrap height=24>Work 
    Phone</td>
    <td noWrap height=24>
      <OBJECT id=ClientWorkPhoneCode 
      style="LEFT: 1px; WIDTH: 59px; TOP: 1px; HEIGHT: 22px" tabIndex=12 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="1561">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
	
      <OBJECT id=ClientWorkPhoneNumber 
      style="LEFT: 60px; WIDTH: 184px; TOP: 1px; HEIGHT: 22px" tabIndex=13 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4868">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td noWrap height=24></td>
    <td noWrap height=24></td></tr>
  <tr>
    <td noWrap align="right" >FAX</td>
    <td noWrap>
      <OBJECT id=ClientFAXCode 
      style="LEFT: 1px; WIDTH: 59px; TOP: 1px; HEIGHT: 22px" tabIndex=14 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="1561">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
	
      <OBJECT id=ClientFAXNumber 
      style="LEFT: 60px; WIDTH: 184px; TOP: 1px; HEIGHT: 22px" tabIndex=15 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4868">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td noWrap></td>
    <td noWrap></td></tr>
  <tr>
    <td align="right" noWrap height=20>  </td>
    <td noWrap height=20>
</td>
    <td noWrap height=20></td>
    <td noWrap height=20></td></tr>
  <tr>
    <td align="right" noWrap >Cellular 
      Phone Number</td>
    <td noWrap>
      <OBJECT id=ClientCellular 
      style="LEFT: 60px; WIDTH: 184px; TOP: 1px; HEIGHT: 22px" tabIndex=16 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4868">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td noWrap></td>
    <td noWrap></td></tr>
  <tr>
    <td noWrap align="right" >E-Mail 
      Address</td>
    <td noWrap>
      <OBJECT id=ClientEMail title="" 
      style="LEFT: 1px; WIDTH: 244px; TOP: 1px; HEIGHT: 22px" tabIndex=17 
      height=22 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="6456">
	<PARAM NAME="_ExtentY" VALUE="582">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="MultiLine" VALUE="0">
	<PARAM NAME="ScrollBars" VALUE="0">
	<PARAM NAME="PasswordChar" VALUE="">
	<PARAM NAME="AllowSpace" VALUE="-1">
	<PARAM NAME="Format" VALUE="">
	<PARAM NAME="FormatMode" VALUE="1">
	<PARAM NAME="AutoConvert" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="MaxLength" VALUE="0">
	<PARAM NAME="LengthAsByte" VALUE="0">
	<PARAM NAME="Text" VALUE="">
	<PARAM NAME="Furigana" VALUE="0">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	</OBJECT>
</td>
    <td noWrap><input align="middle" id="btn_ClientDependants" name="btn_ClientDependants" style="Z-INDEX: 103; LEFT: 709px; VERTICAL-ALIGN: sub; WIDTH: 136px; CURSOR: hand; PADDING-TOP: 15px; POSITION: absolute; TOP: 390px; HEIGHT: 55px" title ="Dependants" type="button" value ="Client Dependants" tabIndex="32" class=button3></td>
    <td noWrap><INPUT align="middle" id=btn_CreditCard name="btn_CreditCard" style="Z-INDEX: 103; LEFT: 709px; VISIBILITY: visible; VERTICAL-ALIGN: sub; WIDTH: 136px; CURSOR: hand; PADDING-TOP: 15px; POSITION: absolute; TOP: 446px; HEIGHT: 55px" title="SAHL Gold Card" type="button" value="SAHL Gold Card" class=button3 tabIndex="33"></td></tr></table></p><input align="middle" id="btn_UpdateClient" name="btn_UpdateClient" style="Z-INDEX: 101; LEFT: 458px; VERTICAL-ALIGN: sub; WIDTH: 136px; CURSOR: hand; PADDING-TOP: 15px; POSITION: absolute; TOP: 392px; HEIGHT: 55px" title ="Update Client" type ="button" value="Update Client" class=button3><input align="middle" id="btn_AdditionalDetails" name="btn_AdditionalDetails" style="Z-INDEX: 102; LEFT: 601px; VERTICAL-ALIGN: sub; WIDTH: 136px; CURSOR: hand; POSITION: absolute; TOP: 448px; HEIGHT: 55px" title            ="Additional Details" type ="button" value="Additional Details" tabIndex="30" class=button2> 
<input align="middle" id="btn_ClientMemo" name="btn_ClientMemo" style="Z-INDEX: 125; LEFT: 599px; VERTICAL-ALIGN: sub; WIDTH: 136px; CURSOR: hand; PADDING-TOP: 15px; POSITION: absolute; TOP: 392px; HEIGHT: 55px" title ="Memo" type="button" value ="Client Memo" tabIndex="31" class=button3><IMG id =pic_UpdateClient title=0 style="Z-INDEX: 104;                            LEFT: 515px;                            WIDTH: 19px;                            CURSOR: hand;                            POSITION: absolute;                            TOP: 392px;                            HEIGHT: 23px" height     =23 alt="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 ><IMG id =pic_ClientMemo title=0 style="Z-INDEX: 126;                            LEFT: 655px;                            WIDTH: 19px;                            CURSOR: hand;                            POSITION: absolute;                            TOP: 392px;                            HEIGHT: 23px" height     =23 alt="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 ><IMG id =pic_ClientDependants title=0 style="Z-INDEX: 106;                            LEFT: 793px;                            WIDTH: 19px;                            CURSOR: hand;                            POSITION: absolute;                            TOP: 392px;                            HEIGHT: 23px" height     =23 alt="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 > 
<TABLE bgColor=red border=1 cellPadding=1 cellSpacing=1 id=msg_locked 
style="FONT-WEIGHT: bold; Z-INDEX: 124; LEFT: 110px; VISIBILITY: hidden; COLOR: white; POSITION: absolute; TOP: 528px" width="75%">
  
  <TR>
    <TD align=center bgColor=red noWrap >THIS CLIENT'S 
      LOAN IS CLOSED AND LOCKED</TD></TR></TABLE>


</BODY>
