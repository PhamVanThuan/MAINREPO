<%
  
  sDatabase =Session("SQLDatabase") 
  sUid = Session("UserID")
  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")

  i_LinkRate = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Link Rate",Session("UserName"))
  i_SPV = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"SPV Description",Session("UserName"))
  
  
  
%>
<html>
<head>
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>

<meta name="VI60_DefaultClientScript" Content="VBScript">

<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<script ID="clientEventHandlersVBS" LANGUAGE="vbscript">
<!--

Dim i_EmployeeType
Dim i_SAHLEmployeeNumber
Dim i_EmployeeTeamNumber
Dim gi_OriginationAdmin

Dim i_ProspectNumber
Dim s_ReturnPage
Dim b_Legal
Dim b_Loading
Dim b_Loading1
Dim b_Loading2
Dim b_Loading3
Dim i_CurrentBankRate
Dim i_CurrentRate
Dim i_InterimPeriod
Dim b_AllDataLoaded
Dim s_ProspectStage
Dim gi_LegalEntity
Dim gi_JointOwner
Dim gi_SwitchLoan
Dim gi_NewPurchase
Dim gi_Refinance
Dim gi_FurtherLoan
Dim i_CurrentLinkRate
Dim i_SubsidyFlag
Dim i_LoanPurpose

Dim b_CDone 


if rs_open <> true then

  
   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"
    
	if  sessDSN = "DSN=" then
	   window.location.href = "Default.asp"
	   'return
    end if
    sUserName = "<%= Session("UserID")%>"
    
   
	set conn = createobject("ADODB.Connection")
	set rs_SelectedProspect1  = createobject("ADODB.Recordset")
	set rs_Education = createobject("ADODB.Recordset")
	set rs_Population = createobject("ADODB.Recordset")
	set rs_Language = createobject("ADODB.Recordset")
	set rs_SAHLBranch= createobject("ADODB.Recordset")
	set rs_SPV = createobject("ADODB.Recordset")
	set rs_LinkRate = createobject("ADODB.Recordset")
	set rs_MarketRate = createobject("ADODB.Recordset")
	set rs_ACBBank = createobject("ADODB.Recordset")
	set rs_ACBBranch = createobject("ADODB.Recordset")
	set rs_ACBType = createobject("ADODB.Recordset")
	set rs_DebitOrderDay = createobject("ADODB.Recordset")
    set rstemp = createobject("ADODB.Recordset")
 
	'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL MLSS [takeondetails.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	conn.Open sDSN
	rs_open1 = false

	b_AllDataLoaded = false

	
end if

Sub GetTakeOnProspectData()
   
    if rs_open1 = true  then
       rs_SelectedProspect1.Close
       rs_open1 = false
	end if

    'msgbox i_LoanPurpose
    'msgbox gi_FurtherLoan
    if Cint(i_LoanPurpose) = gi_FurtherLoan then
        sSQL = "t_GetSelectedTakeOnFurtherLoan " & i_ProspectNumber 
   '     msgbox sSQL
    else
       sSQL = "t_GetSelectedTakeOnProspect " & i_ProspectNumber 
    end if
    
   ' msgbox sSQL
    rs_SelectedProspect1.CursorLocation = 3
	'rs_GridProspects.Open sSQL ,conn,adOpenStatic
	'rs_SelectedProspect1.CacheSize  =10
	
    'this.style.cursor = "hand"
    
	rs_SelectedProspect1.Open sSQL,conn,adOpenDynamic
	
	

	window.ProspectFirstNames.Text = rs_SelectedProspect1.Fields("ProspectFirstNames").Value
	window.ProspectSurname.Text = rs_SelectedProspect1.Fields("ProspectSurname").Value
	window.ProspectSalutation.Text = rs_SelectedProspect1.Fields("ProspectSalutation").Value
	'msgbox rs_SelectedProspect1.Fields("ProspectPropertyDescription1").Value
	window.ProspectPropertyDesc1.Text = rs_SelectedProspect1.Fields("ProspectPropertyDescription1").Value
    window.ProspectPropertyDesc2.Text = rs_SelectedProspect1.Fields("ProspectPropertyDescription2").Value
    window.ProspectPropertyDesc3.Text = rs_SelectedProspect1.Fields("ProspectPropertyDescription3").Value


    if Cint(i_LoanPurpose) = gi_FurtherLoan then
       window.ProspectACBAccountNumber.Text =  rs_SelectedProspect1.Fields("LoanACBAccountNumber").Value
    end if
	
   '*** Manually populate the Education rs
    rs_Education.Fields.Append "EducationNumber",19
    rs_Education.Fields.Append "EducationDetail",200,180
    rs_Education.Open

   rs_Education.AddNew 
   rs_Education.fields("EducationNumber").Value = 0 
   rs_Education.fields("EducationDetail").Value = "Matric"
   rs_Education.Update
   rs_Education.AddNew 
   rs_Education.fields("EducationNumber").Value = 1 
   rs_Education.fields("EducationDetail").Value = "University Degree"
   rs_Education.Update
   rs_Education.AddNew 
   rs_Education.fields("EducationNumber").Value = 2 
   rs_Education.fields("EducationDetail").Value = "Diploma"
   rs_Education.Update
   rs_Education.AddNew 
   rs_Education.fields("EducationNumber").Value = 3
   rs_Education.fields("EducationDetail").Value = "Other"
   rs_Education.Update
   rs_Education.MoveFirst
	
	
	TrueDBCombo_Education.RowSource = rs_Education
	TrueDBCombo_Education.ListField = rs_Education.Fields("EducationDetail").name
	TrueDBCombo_Education.BoundText = rs_Education.Fields("EducationNumber").Value
	TrueDBCombo_Education.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_Education.EvenRowStyle.BackColor = &HC0C0C0

    if Cint(i_LoanPurpose) = gi_FurtherLoan then
		TrueDBCombo_Education.BoundText = rs_SelectedProspect1.Fields("ClientTertiary").Value
    else
		TrueDBCombo_Education.BoundText = rs_Education.Fields("EducationNumber").Value
	end if
	TrueDBCombo_Education.Refresh

	sSQL = "SELECT * FROM POPULATIONGROUP"
	rs_Population.CursorLocation = 3
	rs_Population.Open sSQL ,conn,adOpenStatic
	TrueDBCombo_Population.RowSource = rs_Population
	TrueDBCombo_Population.ListField = rs_Population.Fields("PopulationGroupDescription").name
	TrueDBCombo_Population.BoundText = rs_Population.Fields("PopulationGroupNumber").Value
	TrueDBCombo_Population.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_Population.EvenRowStyle.BackColor = &HC0C0C0
    
    if Cint(i_LoanPurpose) = gi_FurtherLoan then
		TrueDBCombo_Population.BoundText = rs_SelectedProspect1.Fields("PopulationGroupNumber").Value
    else	
		TrueDBCombo_Population.BoundText = rs_Population.Fields("PopulationGroupNumber").Value
	end if
    TrueDBCombo_Population.Refresh
	
	
	sSQL = "SELECT * FROM LANGUAGE"
	rs_Language.CursorLocation = 3
	rs_Language.Open sSQL ,conn,adOpenStatic
	TrueDBCombo_Language.RowSource = rs_Language
	TrueDBCombo_Language.ListField = rs_Language.Fields("LanguageDescription").name
	TrueDBCombo_Language.BoundText = rs_Language.Fields("LanguageNumber").Value
	TrueDBCombo_Language.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_Language.EvenRowStyle.BackColor = &HC0C0C0

    if Cint(i_LoanPurpose) = gi_FurtherLoan then
		TrueDBCombo_Language.BoundText = rs_SelectedProspect1.Fields("LanguageNumber").Value
    else	
		TrueDBCombo_Language.BoundText = rs_Language.Fields("LanguageNumber").Value
	end if
	TrueDBCombo_Language.Refresh
	
	sSQL = "SELECT SAHLBranchNumber,SAHLBranchName FROM SAHLBRANCH"
	rs_SAHLBranch.CursorLocation = 3
	rs_SAHLBranch.Open sSQL ,conn,adOpenStatic
	TrueDBCombo_SAHLBranch.RowSource = rs_SAHLBranch
	TrueDBCombo_SAHLBranch.ListField = rs_SAHLBranch.Fields("SAHLBranchName").name
	TrueDBCombo_SAHLBranch.BoundText = rs_SAHLBranch.Fields("SAHLBranchNumber").Value
	TrueDBCombo_SAHLBranch.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_SAHLBranch.EvenRowStyle.BackColor = &HC0C0C0

    if Cint(i_LoanPurpose) = gi_FurtherLoan then
		TrueDBCombo_SAHLBranch.BoundText = rs_SelectedProspect1.Fields("SAHLBranchNumber").Value
    else	
		TrueDBCombo_SAHLBranch.BoundText = rs_SAHLBranch.Fields("SAHLBranchNumber").Value
	end if
	TrueDBCombo_SAHLBranch.Refresh
	
	sSQL = "SELECT SPVNumber,SPVDescription FROM SPV WHERE SPVLocked = 0"
	rs_SPV.CursorLocation = 3
	rs_SPV.Open sSQL ,conn,adOpenStatic
	TrueDBCombo_SPV.RowSource = rs_SPV
	TrueDBCombo_SPV.ListField = rs_SPV.Fields("SPVDescription").name
	TrueDBCombo_SPV.BoundText = rs_SelectedProspect1.Fields("SPVNumber").Value
	TrueDBCombo_SPV.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_SPV.EvenRowStyle.BackColor = &HC0C0C0

    if Cint(i_LoanPurpose) = gi_FurtherLoan then
		TrueDBCombo_SPV.BoundText = rs_SelectedProspect1.Fields("SPVNumber").Value
    else
		TrueDBCombo_SPV.BoundText = rs_SelectedProspect1.Fields("SPVNumber").Value
    end if
	TrueDBCombo_SPV.Refresh
	
	
    sSQL = "SELECT * FROM LinkRate"
	rs_LinkRate.CursorLocation = 3
	rs_LinkRate.Open sSQL ,conn,adOpenStatic
	TrueDBCombo_LinkRate.RowSource = rs_LinkRate
	TrueDBCombo_LinkRate.ListField = rs_LinkRate.Fields("LinkRateDescription").name
	TrueDBCombo_LinkRate.BoundText = rs_LinkRate.Fields("LinkRate").Value
	TrueDBCombo_LinkRate.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_LinkRate.EvenRowStyle.BackColor = &HC0C0C0
    rs_LinkRate.Find "LinkRate = " & i_CurrentLinkRate/100.00
	TrueDBCombo_LinkRate.BoundText = rs_LinkRate.Fields("LinkRate").Value
	TrueDBCombo_LinkRate.Refresh
	
	sSQL = "SELECT * FROM MarketRateType"
	rs_MarketRate.CursorLocation = 3
	rs_MarketRate.Open sSQL ,conn,adOpenStatic
	TrueDBCombo_MarketRateType.RowSource = rs_MarketRate
	TrueDBCombo_MarketRateType.ListField = rs_MarketRate.Fields("MarketRateTypeDescription").name
	TrueDBCombo_MarketRateType.BoundText = rs_MarketRate.Fields("MarketRateTypeNumber").Value
	TrueDBCombo_MarketRateType.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_MarketRateType.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBCombo_MarketRateType.BoundText = rs_MarketRate.Fields("MarketRateTypeNumber").Value
	TrueDBCombo_MarketRateType.Refresh
	
	sSQL = "SELECT * FROM ACBBANK"
	rs_ACBBank.CursorLocation = 3
	rs_ACBBank.Open sSQL ,conn,adOpenStatic
	TrueDBCombo_ACBBank.RowSource = rs_ACBBank
	TrueDBCombo_ACBBank.ListField = rs_ACBBank.Fields("ACBBankDescription").name
	TrueDBCombo_ACBBank.BoundText = rs_ACBBank.Fields("ACBBankCode").Value
	TrueDBCombo_ACBBank.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_ACBBank.EvenRowStyle.BackColor = &HC0C0C0

    if Cint(i_LoanPurpose) = gi_FurtherLoan then
		TrueDBCombo_ACBBank.BoundText = rs_SelectedProspect1.Fields("ACBBankCode").Value
    else
		TrueDBCombo_ACBBank.BoundText = rs_ACBBank.Fields("ACBBankCode").Value
	end if
	TrueDBCombo_ACBBank.Refresh
	
	sSQL = "SELECT ACBBranchCode,ACBBranchDescription,ACBBankCode FROM ACBBRANCH WHERE ACBBankCode = " &  rs_ACBBank.Fields("ACBBankCode")
	rs_ACBBranch.CursorLocation = 3
	rs_ACBBranch.Open sSQL ,conn,adOpenStatic
	'msgbox rs_ACBBranch.RecordCount
	TrueDBCombo_ACBBranch.RowSource = rs_ACBBranch
	TrueDBCombo_ACBBranch.ListField = rs_ACBBranch.Fields("ACBBranchDescription").name
	TrueDBCombo_ACBBranch.BoundText = rs_ACBBranch.Fields("ACBBranchCode").Value
	TrueDBCombo_ACBBranch.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_ACBBranch.EvenRowStyle.BackColor = &HC0C0C0

    if Cint(i_LoanPurpose) = gi_FurtherLoan then
		TrueDBCombo_ACBBranch.BoundText = rs_SelectedProspect1.Fields("ACBBranchCode").Value
    else
		TrueDBCombo_ACBBranch.BoundText = rs_ACBBranch.Fields("ACBBranchCode").Value
	end if
	TrueDBCombo_ACBBranch.Refresh
	
	
	sSQL = "SELECT * FROM ACBTYPE"
	rs_ACBType.CursorLocation = 3
	rs_ACBType.Open sSQL ,conn,adOpenStatic
	'msgbox rs_ACBBranch.RecordCount
	TrueDBCombo_AccountType.RowSource = rs_ACBType
	TrueDBCombo_AccountType.ListField = rs_ACBType.Fields("ACBTypeDescription").name
	TrueDBCombo_AccountType.BoundText = rs_ACBType.Fields("ACBTypeNumber").Value
	TrueDBCombo_AccountType.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_AccountType.EvenRowStyle.BackColor = &HC0C0C0

    if Cint(i_LoanPurpose) = gi_FurtherLoan then
		TrueDBCombo_AccountType.BoundText = rs_SelectedProspect1.Fields("ACBTypeNumber").Value
    else
		TrueDBCombo_AccountType.BoundText = rs_ACBType.Fields("ACBTypeNumber").Value
	end if
	TrueDBCombo_AccountType.Refresh

    
     '*** Manually populate the Education rs
    rs_DebitOrderDay.Fields.Append "DONumber",19
    rs_DebitOrderDay.Fields.Append "DODetail",200,180
    rs_DebitOrderDay.Open

   rs_DebitOrderDay.AddNew 
   rs_DebitOrderDay.fields("DONumber").Value = 1 
   rs_DebitOrderDay.fields("DODetail").Value = " 1"
   rs_DebitOrderDay.Update
   rs_DebitOrderDay.AddNew 
   rs_DebitOrderDay.fields("DONumber").Value = 7 
   rs_DebitOrderDay.fields("DODetail").Value = " 7"
   rs_DebitOrderDay.Update
   rs_DebitOrderDay.AddNew 
   rs_DebitOrderDay.fields("DONumber").Value = 17 
   rs_DebitOrderDay.fields("DODetail").Value = "17"
   rs_DebitOrderDay.Update
   rs_DebitOrderDay.AddNew 
   rs_DebitOrderDay.fields("DONumber").Value = 25
   rs_DebitOrderDay.fields("DODetail").Value = "25"
   rs_DebitOrderDay.Update
   rs_DebitOrderDay.AddNew 
   rs_DebitOrderDay.fields("DONumber").Value = 28
   rs_DebitOrderDay.fields("DODetail").Value = "28"
   rs_DebitOrderDay.Update
   rs_DebitOrderDay.MoveFirst
	
	
	TrueDBCombo_DebitOrderDay.RowSource = rs_DebitOrderDay
	TrueDBCombo_DebitOrderDay.ListField = rs_DebitOrderDay.Fields("DODetail").name
	TrueDBCombo_DebitOrderDay.BoundText = rs_DebitOrderDay.Fields("DONumber").Value
	TrueDBCombo_DebitOrderDay.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_DebitOrderDay.EvenRowStyle.BackColor = &HC0C0C0

    if Cint(i_LoanPurpose) = gi_FurtherLoan then
		TrueDBCombo_DebitOrderDay.BoundText = rs_SelectedProspect1.Fields("LoanDebitOrderDay").Value
    else	
		TrueDBCombo_DebitOrderDay.BoundText = rs_DebitOrderDay.Fields("DONumber").Value
	end if
	TrueDBCombo_DebitOrderDay.Refresh
    
    
    i_SubsidyFlag = 0
    
	if Cint(rs_SelectedProspect1.Fields("PurposeNumber").Value) <> gi_FurtherLoan then
    
		sSQL = "t_GetProspectSubsidyCount " & i_ProspectNumber 
		rstemp.CursorLocation = 3
		rstemp.Open sSQL,conn,adOpenDynamic
    
    
    
		if rstemp.Fields(0) > 0 then
		  i_SubsidyFlag = 1
		  window.ProspectPaypoint.Enabled = true
		  window.ProspectPayNotch.Enabled = true
		  window.ProspectSalaryNumber.Enabled = true
		  window.ProspectRank.Enabled = true
		end if
    
		rstemp.Close
		
		window.ProspectTaxNumber.focus()
    else
        DisableControls
    end if
    
     
     
End Sub

Sub btn_Cancel_onclick
window.parent.frames("RegistrationPanel").location.href = "TakeOn.asp"
End Sub

Sub window_onload
   window.tbl_Message.style.visibility  = "hidden"
   window.lbl_Message.style.visibility  = "hidden"
   SetAccessLightsServer
   EnableFields
      
   b_AllDataLoaded = false
   i_SubsidyFlag = 0
   gi_FurtherLoan = 5

   sSQL = "SELECT MarketRateTypeRate FROM MARKETRATETYPE WHERE MarketRateTypeNumber = 1"
   rstemp.CursorLocation = 3
   rstemp.Open sSQL,conn,adOpenDynamic
   
   i_CurrentRate = rstemp.Fields("MarketRateTypeRate").Value

   rsTemp.Close
   
   sSQL = "SELECT ControlNumeric FROM CONTROL WHERE ControlNumber = 1"
   rstemp.CursorLocation = 3
   rstemp.Open sSQL,conn,adOpenDynamic

   i_CurrentLinkRate = rstemp.Fields("ControlNumeric").Value

   i_CurrentRate = i_CurrentRate + (i_CurrentLinkRate/100.00)

   rsTemp.Close

 i_ProspectNumber = "<%=Request.QueryString("Number")%>"
 i_LoanPurpose = "<%=Request.QueryString("Purpose")%>"
  'msgbox i_LoanPurpose
 GetTakeOnProspectData
 
  'window.ProspectPropertyDesc1.Text = rs_SelectedProspect1.Fields("ProspectPropertyDescription1").Value
  'window.ProspectPropertyDesc2.Text = rs_SelectedProspect1.Fields("ProspectPropertyDescription2").Value
  'window.ProspectPropertyDesc3.Text = rs_SelectedProspect1.Fields("ProspectPropertyDescription3").Value
 
    window.TrueDBCombo_ACBBank.style.visibility  = "hidden"
	window.TrueDBCombo_ACBBank.style.visibility  = "visible"
    window.TrueDBCombo_ACBBranch.style.visibility  = "hidden"
	window.TrueDBCombo_ACBBranch.style.visibility  = "visible"
	window.focus
    
End Sub


Sub window_onfocus

if b_AllDataLoaded = true then exit sub	
'msgbox rs_SelectedProspect1.Fields("ProspectPropertyDescription1").Value
window.ProspectPropertyDesc1.Text = rs_SelectedProspect1.Fields("ProspectPropertyDescription1").Value
window.ProspectPropertyDesc2.Text = rs_SelectedProspect1.Fields("ProspectPropertyDescription2").Value
window.ProspectPropertyDesc3.Text = rs_SelectedProspect1.Fields("ProspectPropertyDescription3").Value
b_AllDataLoaded = true

End Sub

Sub TrueDBCombo_ACBBank_ItemChange

    rs_ACBBranch.Close
	sSQL = "SELECT ACBBranchCode,ACBBranchDescription,ACBBankCode FROM ACBBRANCH WHERE ACBBankCode = " & window.TrueDBCombo_ACBBank.BoundText & " ORDER BY ACBBranchDescription"
	rs_ACBBranch.CursorLocation = 3
	rs_ACBBranch.Open sSQL ,conn,adOpenStatic
	'msgbox rs_ACBBranch.RecordCount
	TrueDBCombo_ACBBranch.RowSource = rs_ACBBranch
	TrueDBCombo_ACBBranch.ListField = rs_ACBBranch.Fields("ACBBranchDescription").name
	TrueDBCombo_ACBBranch.BoundText = rs_ACBBranch.Fields("ACBBranchCode").Value
	TrueDBCombo_ACBBranch.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_ACBBranch.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBCombo_ACBBranch.BoundText = rs_ACBBranch.Fields("ACBBranchCode").Value
	TrueDBCombo_ACBBranch.Refresh
	 
	


End Sub


Function CreateTakeOnRecords()

Dim i_res


CreateTakeOnRecords = -1

    document.body.style.cursor = "hand"
    
  
    i_res = 0     
    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")
    set rs_temp1 = createobject("ADODB.Recordset")
    'Cannot use OLE DB Provider as it appears that it does not return a recordset
    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL MLSS [takeondetails.asp];uid=<%= Session("UserID")%>"
'    sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL MLSS [takeondetails.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"

	
	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc

	sSQL = "t_CreateTakeOnRecords"  

	

	com.CommandText = sSQL
	
	set prm = com.CreateParameter ( "ProspectNumber",19,1,,rs_SelectedProspect1.fields("ProspectNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "SAHLBranchNumber",19,1,,CInt(window.TrueDBCombo_SAHLBranch.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm
		
	set prm = com.CreateParameter ( "PopulationGroupNumber",19,1,,CInt(window.TrueDBCombo_Population.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "SexNumber",19,1,,rs_SelectedProspect1.Fields("SexNumber").Value) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "LanguageNumber",19,1,,CInt(window.TrueDBCombo_Language.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm
 
	set prm = com.CreateParameter ( "LegalStatusNumber",19,1,,rs_SelectedProspect1.Fields("LegalStatusNumber").Value) ' AdUnsigned Int
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ClientSurname",200,1,50,rs_SelectedProspect1.Fields("ProspectSurname").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ClientFirstNames",200,1,50,rs_SelectedProspect1.Fields("ProspectFirstNames").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		
	
	s_Initials = GetInitials(rs_SelectedProspect1.Fields("ProspectFirstNames").Value)
	
	set prm = com.CreateParameter ( "ClientInitials",200,1,50,s_Initials) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	   set prm = com.CreateParameter ( "ClientSalutation",200,1,15,rs_SelectedProspect1.Fields("ProspectSalutation").Value) 'AdVarchar , adParamInput
	   com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ClientTertiary",19,1,,CInt(window.TrueDBCombo_Education.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ClientIDNumber",200,1,50,rs_SelectedProspect1.Fields("ProspectIDNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		
	
	
	if Cint(rs_SelectedProspect1.Fields("SexNumber").Value) = 4 then   'Legal so no birthdate
	   dt_ClientBirthDate = Date()
	   s_date = Mid(dt_ClientBirthDate, 4, 2) & "/" & Mid(dt_ClientBirthDate, 1, 2) & "/" & Mid(dt_ClientBirthDate, 7, 4)
	else
		dt_ClientBirthDate = GetBirthDateFromIDNumber(rs_SelectedProspect1.Fields("ProspectIDNumber").Value)
		s_date = Mid(dt_ClientBirthDate, 4, 2) & "/" & Mid(dt_ClientBirthDate, 1, 2) & "/" & Mid(dt_ClientBirthDate, 7, 4)
		if s_date = "X1" or s_date = "/X1/" then
			set prm = com.CreateParameter ( "ClientBirthDate",135,1,10,"01/01/1900")
		else
		' msgbox s_date
			if DetermineWorksStationDateFormat = "A" then 'American
			    set prm = com.CreateParameter ( "ClientBirthDate",135,1,10,s_date) 'AdVarchar , adParamInput
			elseif DetermineWorksStationDateFormat = "S" then 'SA or UK	
			    set prm = com.CreateParameter ( "ClientBirthDate",135,1,10,s_date) 'AdVarchar , adParamInput
		 else
		    	Msgbox  "Your Workstation's Date Format is not currently supported in this System... " & chr(13) & chr(10) & "Changed your Regional Date setting to dd/mm/yyyy or Contact IT"
		    	exit function
			end if
		end if
    end if

'    set prm = com.CreateParameter ( "ClientBirthDate",129,1,10,s_date) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ClientTaxNumber",200,1,50,window.ProspectTaxNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	
	set prm = com.CreateParameter ( "ClientEmployer",200,1,50,rs_SelectedProspect1.Fields("ProspectCurrentEmployer").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

    if rs_SelectedProspect1.Fields("ProspectCurrentStartDate").Value = "__/__/____" then
        set prm = com.CreateParameter ( "ClientEmployerStartDate",135,1,10,"01/01/1900") 'AdVarchar , adParamInput
    else
		'set prm = com.CreateParameter ( "ClientEmployerStartDate",129,1,10,rs_SelectedProspect1.Fields("ProspectCurrentStartDate").Value) 'AdVarchar , adParamInput
		
		if DetermineWorksStationDateFormat = "A" then 'American
		    s_date1 = rs_SelectedProspect1.Fields("ProspectCurrentStartDate").Value
			set prm = com.CreateParameter ( "ClientEmployerStartDate",135,1,10,s_date1) 'AdVarchar , adParamInput
		elseif DetermineWorksStationDateFormat = "S" then 'SA or UK	
		    s_date1 = Mid(rs_SelectedProspect1.Fields("ProspectCurrentStartDate").Value,4,2) & "/" & Mid(rs_SelectedProspect1.Fields("ProspectCurrentStartDate").Value,1,2) & "/" & Mid(rs_SelectedProspect1.Fields("ProspectCurrentStartDate").Value,7,4)
			set prm = com.CreateParameter ( "ClientEmployerStartDate",135,1,10,s_date1) 'AdVarchar , adParamInput
		else
       		Msgbox  "Your Workstation's Date Format is not currently supported in this System... " & chr(13) & chr(10) & "Change your Regional Date setting to dd/mm/yyyy or Contact IT"
       		exit function
		end if
		
	end if
	'msgbox s_date
	com.Parameters.Append prm	
	

	set prm = com.CreateParameter ( "ClientIncome",5,1,,rs_SelectedProspect1.Fields("ProspectIncome").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

    set prm = com.CreateParameter ( "ClientSpouseFirstNames",200,1,50,rs_SelectedProspect1.Fields("ProspectSpouseFirstNames").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	s_Initials = GetInitials(rs_SelectedProspect1.Fields("ProspectSpouseFirstNames").Value)
	
	set prm = com.CreateParameter ( "ClientSpouseInitials",200,1,50,s_Initials) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	    set prm = com.CreateParameter ( "ClientSpouseIDNumber",200,1,50,rs_SelectedProspect1.Fields("ProspectSpouseIDNumber").Value) 'AdVarchar , adParamInput
	    com.Parameters.Append prm		

	set prm = com.CreateParameter ( "ClientSpouseIncome",5,1,,rs_SelectedProspect1.Fields("ProspectSpouseIncome").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ClientCellularPhoneNumber",200,1,50,rs_SelectedProspect1.Fields("ProspectCellularTelephone").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	set prm = com.CreateParameter ( "ClientHomePhoneCode",200,1,10,rs_SelectedProspect1.Fields("ProspectHomePhoneCode").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	set prm = com.CreateParameter ( "ClientHomePhoneNumber",200,1,25,rs_SelectedProspect1.Fields("ProspectHomePhoneNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	set prm = com.CreateParameter ( "ClientWorkPhoneCode",200,1,10,rs_SelectedProspect1.Fields("ProspectWorkPhoneCode").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	set prm = com.CreateParameter ( "ClientWorkPhoneNumber",200,1,25,rs_SelectedProspect1.Fields("ProspectWorkPhoneNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	set prm = com.CreateParameter ( "ClientEMailAddress",200,1,50,rs_SelectedProspect1.Fields("ProspectEMailAddress").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	set prm = com.CreateParameter ( "ClientBoxNumber",200,1,50,rs_SelectedProspect1.Fields("ProspectBoxNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	set prm = com.CreateParameter ( "ClientPostOffice",200,1,50,rs_SelectedProspect1.Fields("ProspectPostOffice").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		
	
	    set prm = com.CreateParameter ( "ClientPostalCode",200,1,50,rs_SelectedProspect1.Fields("ProspectPostalCode").Value) 'AdVarchar , adParamInput
	    com.Parameters.Append prm		
	
	set prm = com.CreateParameter ( "ClientHomeBuildingNumber",200,1,10,rs_SelectedProspect1.Fields("ProspectHomeBuildingNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		
	
	set prm = com.CreateParameter ( "ClientHomeBuildingName",200,1,50,rs_SelectedProspect1.Fields("ProspectHomeBuildingName").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ClientHomeStreetNumber",200,1,10,rs_SelectedProspect1.Fields("ProspectHomeStreetNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		
	
	set prm = com.CreateParameter ( "ClientHomeStreetName",200,1,50,rs_SelectedProspect1.Fields("ProspectHomeStreetName").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ClientHomeSuburb",200,1,50,rs_SelectedProspect1.Fields("ProspectHomeSuburb").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ClientHomeCity",200,1,50,rs_SelectedProspect1.Fields("ProspectHomeCity").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ClientHomeProvince",200,1,50,rs_SelectedProspect1.Fields("ProspectHomeProvince").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ClientHomeCountry",200,1,50,rs_SelectedProspect1.Fields("ProspectHomeCountry").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ClientHomePostalCode",200,1,50,rs_SelectedProspect1.Fields("ProspectHomePostalCode").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

    
	    set prm = com.CreateParameter ( "PropertyTypeNumber",19,1,,rs_SelectedProspect1.Fields("PropertyTypeNumber").Value) ' AdUnsigned Int
	    com.Parameters.Append prm

	set prm = com.CreateParameter ( "AreaClassificationNumber",19,1,,rs_SelectedProspect1.Fields("AreaClassificationNumber").Value) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TitleTypeNumber",19,1,,rs_SelectedProspect1.Fields("TitleTypeNumber").Value) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "PropertyDescription1",200,1,50,window.ProspectPropertyDesc1.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "PropertyDescription2",200,1,50,window.ProspectPropertyDesc2.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "PropertyDescription3",200,1,50,window.ProspectPropertyDesc3.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "PropertyOwnerOccupied",19,1,,rs_SelectedProspect1.Fields("ProspectOwnerOccupied").Value) ' AdUnsigned Int
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "PropertyBuildingNumber",200,1,10,rs_SelectedProspect1.Fields("ProspectPropertyBuildingNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		
	
	set prm = com.CreateParameter ( "PropertyBuildingName",200,1,50,rs_SelectedProspect1.Fields("ProspectPropertyBuildingName").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "PropertyStreetNumber",200,1,10,rs_SelectedProspect1.Fields("ProspectPropertyStreetNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		
	
	    set prm = com.CreateParameter ( "PropertyStreetName",200,1,50,rs_SelectedProspect1.Fields("ProspectPropertyStreetName").Value) 'AdVarchar , adParamInput
	    com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "PropertySuburb",200,1,50,rs_SelectedProspect1.Fields("ProspectPropertySuburb").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "PropertyCity",200,1,50,rs_SelectedProspect1.Fields("ProspectPropertyCity").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "PropertyProvince",200,1,50,rs_SelectedProspect1.Fields("ProspectPropertyProvince").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "PropertyCountry",200,1,50,rs_SelectedProspect1.Fields("ProspectPropertyCountry").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "PropertyPhysicalPostalCode",200,1,50,rs_SelectedProspect1.Fields("ProspectPropertyPostalCode").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
  
	set prm = com.CreateParameter ( "SPVNumber",19,1,,CInt(window.TrueDBCombo_SPV.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "MarketRateTypeNumber",19,1,,CInt(window.TrueDBCombo_MarketRateType.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "LinkRate",5,1,,CDbl(window.TrueDBCombo_LinkRate.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ACBBranchCode",200,1,10,window.TrueDBCombo_ACBBranch.BoundText) ' AdUnsigned Int
	com.Parameters.Append prm

    i_Installment = CalculateInstallment(rs_SelectedProspect1.fields("ProspectLoan").Value,i_CurrentRate,12,rs_SelectedProspect1.Fields("ProspectTermRequired").Value,0)
    
	    set prm = com.CreateParameter ( "LoanInstallmentAmount",5,1,,i_Installment) ' AdUnsigned Int
	    com.Parameters.Append prm

	set prm = com.CreateParameter ( "LoanRate",5,1,,i_CurrentRate*100.00) ' AdUnsigned Int
	com.Parameters.Append prm
    
	set prm = com.CreateParameter ( "LoanDebitOrderDay",19,1,,Cint(window.TrueDBCombo_DebitOrderDay.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "LoanFixedDebitOrderAmt",5,1,,window.ProspectDebitOrderAmt.Value) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ACBTypeNumber",19,1,,CInt(window.TrueDBCombo_AccountType.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "LoanACBAccountNumber",200,1,50,CSTR(window.ProspectACBAccountNumber.Text)) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

    set prm = com.CreateParameter ( "LoanSuretorName",200,1,50,rs_SelectedProspect1.Fields("ProspectSuretorName").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

    set prm = com.CreateParameter ( "LoanSuretorIDNumber",200,1,20,rs_SelectedProspect1.Fields("ProspectSuretorIDNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	set prm = com.CreateParameter ( "ValuatorNumber",19,1,,rs_SelectedProspect1.Fields("ValuatorNumber").Value) ' AdUnsigned Int
	com.Parameters.Append prm

    'set prm = com.CreateParameter ( "ValuationDate ",135,1,,rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value) 'AdVarchar , adParamInput
    
    
    if DetermineWorksStationDateFormat = "A" then 'American
        s_date3 = rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value
		set prm = com.CreateParameter ( "ProspectPropertyValuationDate",135,1,10,rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value) 'AdVarchar , adParamInput
	elseif DetermineWorksStationDateFormat = "S" then 'SA or UK	
	    s_date3 = Mid(rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value,4,2) & "/" & Mid(rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value,1,2) & "/" & Mid(rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value,7,4)
		set prm = com.CreateParameter ( "ProspectPropertyValuationDate",135,1,10,s_date3) 'AdVarchar , adParamInput
	else
       	Msgbox  "Your Workstation's Date Format is not currently supported in this System... " & chr(13) & chr(10) & "Change your Regional Date setting to dd/mm/yyyy or Contact IT"
       	exit function
	end if
	'msgbox s_date
	com.Parameters.Append prm	

	    set prm = com.CreateParameter ( "Valuation",5,1,,rs_SelectedProspect1.Fields("ProspectPropertyValuation").Value) ' AdUnsigned Int
	    com.Parameters.Append prm

	set prm = com.CreateParameter ( "ValuationHOCValue",5,1,,rs_SelectedProspect1.Fields("ProspectPropertyHOCValuation").Value) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ValuationMunicipal",5,1,,0.00) ' AdUnsigned Int
	com.Parameters.Append prm

    sUserName = "<%= Session("UserID")%>"
    
    set prm = com.CreateParameter ( "ValuationUserID",200,1,20,sUserName) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	set prm = com.CreateParameter ( "Thatched",19,1,,rs_SelectedProspect1.Fields("ProspectPropertyThatch").Value) ' AdUnsigned Int
	com.Parameters.Append prm
		
	set prm = com.CreateParameter ( "FAXCode",200,1,10,rs_SelectedProspect1.Fields("ProspectFAXCode").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		

	set prm = com.CreateParameter ( "FAXNumber",200,1,15,rs_SelectedProspect1.Fields("ProspectFAXNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		
 
    set prm = com.CreateParameter ( "PurposeNumber",19,1,,rs_SelectedProspect1.Fields("PurposeNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm		


    if i_SubsidyFlag = 1 then   'Subsidy Client
       	set prm = com.CreateParameter ( "SubsidyFlag",19,1,,1) 'AdVarchar , adParamInput
	    com.Parameters.Append prm	
	    
	    set prm = com.CreateParameter ( "PerSalNumber",200,1,12,window.ProspectSalaryNumber.Text) 'AdVarchar , adParamInput
	    com.Parameters.Append prm	
	    
	    set prm = com.CreateParameter ( "SubPayPoint",200,1,12,window.ProspectPaypoint.Text) 'AdVarchar , adParamInput
	    com.Parameters.Append prm	

        set prm = com.CreateParameter ( "SubNotch",19,1,,window.ProspectPayNotch.Value) 'AdVarchar , adParamInput
	    com.Parameters.Append prm
	    
	    set prm = com.CreateParameter ( "SubRank",200,1,50,window.ProspectRank.Text) 'AdVarchar , adParamInput
	    com.Parameters.Append prm
	    
    end if
    

    set rs_temp = com.Execute 
    
  
    
    MsgBox("Loan Number :  " & rs_temp.Fields(0).Value & " and " & Chr(13) & Chr(10) & "Client Number : " & rs_temp(1).value & Chr(13) & Chr(10) & Chr(13) & Chr(10) &  " have been created for " & Chr(13) & Chr(10) &   Chr(13) & Chr(10) & rs_SelectedProspect1.Fields("ProspectSalutation").Value & " " & rs_SelectedProspect1.Fields("ProspectFirstNames").Value & " "  & rs_SelectedProspect1.Fields("ProspectSurname").Value )

    document.body.style.cursor = "default"

    CreateTakeOnRecords = 0
       
    
End Function


Function CreateFurtherLoanTakeOnRecords()

Dim i_res


    CreateFurtherLoanTakeOnRecords = -1

    if rs_SelectedProspect1.Fields("LoanNumber").Value  < 1235888 then
       MsgBox "ERROR : This Further Loan Prospect has an Invalid Loan Number"
       exit function
    end if
    
    document.body.style.cursor = "hand"
    
  
    i_res = 0     
    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")
    set rs_temp1 = createobject("ADODB.Recordset")
    'Cannot use OLE DB Provider as it appears that it does not return a recordset
    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL MLSS [takeondetails.asp];uid=<%= Session("UserID")%>"
'    sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL MLSS [takeondetails.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"

	
	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc

	sSQL = "t_CreateFurtherLoanRecords"  

	

	com.CommandText = sSQL
	
	set prm = com.CreateParameter ( "ProspectNumber",19,1,,rs_SelectedProspect1.fields("ProspectNumber").Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "LoanNumber",19,1,,rs_SelectedProspect1.Fields("LoanNumber").Value) ' AdUnsigned Int
	com.Parameters.Append prm
		
    set prm = com.CreateParameter ( "ValuatorNumber",19,1,,rs_SelectedProspect1.Fields("ValuatorNumber").Value) ' AdUnsigned Int
	com.Parameters.Append prm

    'set prm = com.CreateParameter ( "ValuationDate ",135,1,,rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value) 'AdVarchar , adParamInput
     if DetermineWorksStationDateFormat = "A" then 'American
        s_date4 = rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value
		set prm = com.CreateParameter ( "ValuationDate",135,1,10,rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value) 'AdVarchar , adParamInput
	elseif DetermineWorksStationDateFormat = "S" then 'SA or UK	
	    s_date4 = Mid(rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value,4,2) & "/" & Mid(rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value,1,2) & "/" & Mid(rs_SelectedProspect1.Fields("ProspectPropertyValuationDate").Value,7,4)
		set prm = com.CreateParameter ( "ValuationDate",135,1,10,s_date4) 'AdVarchar , adParamInput
	else
       	Msgbox  "Your Workstation's Date Format is not currently supported in this System... " & chr(13) & chr(10) & "Change your Regional Date setting to dd/mm/yyyy or Contact IT"
       	exit function
	end if
	com.Parameters.Append prm	
		
    set prm = com.CreateParameter ( "Valuation",5,1,,rs_SelectedProspect1.Fields("ProspectPropertyValuation").Value) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ValuationHOCValue",5,1,,rs_SelectedProspect1.Fields("ProspectPropertyHOCValuation").Value) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ValuationMunicipal",5,1,,0.00) ' AdUnsigned Int
	com.Parameters.Append prm

    sUserName = "<%= Session("UserID")%>"
    
    set prm = com.CreateParameter ( "ValuationUserID",200,1,20,sUserName) 'AdVarchar , adParamInput
	com.Parameters.Append prm
			
	
     set rs_temp = com.Execute 
    
    
     i = MsgBox("Further Loan Entries for Loan Number :  " & rs_temp.Fields(0).Value & "" & Chr(13) & Chr(10) & "Client Number : " & rs_temp(1).value & Chr(13) & Chr(10) & Chr(13) & Chr(10) &  " were created for " & Chr(13) & Chr(10) &   Chr(13) & Chr(10) & rs_SelectedProspect1.Fields("ProspectSalutation").Value & " " & rs_SelectedProspect1.Fields("ProspectFirstNames").Value & " "  & rs_SelectedProspect1.Fields("ProspectSurname").Value,,"Take On Results" )

     document.body.style.cursor = "default"

     CreateFurtherLoanTakeOnRecords = 0
       
    
End Function



Function GetInitials(ByVal s_FirstNames) 
    Dim i_Pos 
    Dim s_Initials 
    
    If s_FirstNames = "" Then
        GetInitials = ""
        Exit Function
    End If
    
     If Left(s_FirstNames, 1) <> "" Then
        s_Initials = Left(s_FirstNames, 1)
    End If
   
    i_Pos = 2
    Do Until i_Pos > 50
        If Mid(s_FirstNames, i_Pos, 1) <> "" Then
            If Mid(s_FirstNames, i_Pos - 1, 1) = " " Then
                s_Initials = s_Initials & Mid(s_FirstNames, i_Pos, 1)
            End If
        End If
        i_Pos = i_Pos + 1
    Loop
    GetInitials = UCase(s_Initials)
End Function

Function GetBirthDateFromIDNumber(ByVal s_IDNumber) 
' Converts a 13 char ID string to an 8 char string format dd/mm/yyyy
'   Errors
'       X1 - ID input string must = 13 chars
'       X2 - All 13 chars of input must be numeric
'       X3 - First 6 chars of input don't convert to a valid date
    Dim s_date 
    
    If Len(s_IDNumber) <> 13 Then
        s_date = "X1"
    Else
        If IsNumeric(s_IDNumber) = False Then
            s_date = "X2"
        Else
            s_date = Mid(s_IDNumber, 5, 2) & "/" & Mid(s_IDNumber, 3, 2) & "/19" & Left(s_IDNumber, 2)
            If IsDate(s_date) = False Then
                s_date = "X3"
            Else
                If Left(s_IDNumber, 2) < 10 Then
                    Mid(s_date, 7, 2) = 20
                End If
            End If
        End If
    End If
    GetBirthDateFromIDNumber = s_date
End Function

Function CalculateInstallment(ByVal d_Loan, ByVal d_PeriodRate, ByVal i_InterestPeriods , ByVal i_Term , ByVal i_Type) 
    'd_Loan  = loan amount
    'd_PeriodRate = interest rate for period as percentage
    'i_term = number of periods over which loan is to be repaid
    'i_Type = (0 = installment at end of period 1  = installment at beginning of period)

If i_Term = 0  Then
   MsgBox "Application Error : Term  is zero ... Check Values....!!!"
   CalculateInstallment = 0
   Exit Function
End If

If  d_PeriodRate <= 0.00 Then
   MsgBox "Application Error : Rate is less tha or equal zero ... Check Values....!!!"
   CalculateInstallment = 0
   Exit Function
End If

If d_Loan <= 0 Then 
   MsgBox "Application Error : Loan Required cannot be zero  ... Check Loa Values....!!!"    
   Exit Function
End If

If i_Term > 360 Then 
   MsgBox "Application Error : Term exceeds permitted range ... Check Term Value....!!!"    
   Exit Function
End  If

	CalculateInstallment = Round((d_PeriodRate / i_InterestPeriods) * (d_Loan * (1 + d_PeriodRate / i_InterestPeriods) ^ i_Term) / ((1 + d_PeriodRate / i_InterestPeriods * i_Type) * (1 - (1 + d_PeriodRate / i_InterestPeriods) ^ i_Term)) * -1, 2)

End Function

Sub btn_CreateTakeOnRecords_onclick

if window.btn_CreateTakeOnRecords.value = "Exit" then
    window.parent.frames("RegistrationPanel").location.href = "TakeOn.asp"
    exit sub
end if

    if ValidateFields = 0 then
    if Cint(i_LoanPurpose) = gi_FurtherLoan then
		i_Resp = MsgBox("Are you sure you want to Create the Further Loan Records for " & rs_SelectedProspect1.Fields("ProspectSalutation").Value & " " & rs_SelectedProspect1.Fields("ProspectFirstNames").Value & " "  & rs_SelectedProspect1.Fields("ProspectSurname").Value  , 4)     
    else
		i_Resp = MsgBox("Are you sure you want to Create the Loan,Client,Property, and Valuation Records for " & rs_SelectedProspect1.Fields("ProspectSalutation").Value & " " & rs_SelectedProspect1.Fields("ProspectFirstNames").Value & " "  & rs_SelectedProspect1.Fields("ProspectSurname").Value  , 4)
	end if
	if i_Resp= 7 then       
	    window.tbl_Message.style.visibility  = "hidden"
		window.lbl_Message.style.visibility  = "hidden"
		window.lbl_Message.innerText = ""
		window.lbl_Message.style.background = "red"
		exit sub
	else
	    if ValidateFields = 0 then
	       
	        DisableControls
	        
	        if Cint(rs_SelectedProspect1.Fields("PurposeNumber").Value) = gi_FurtherLoan then
	            if CreateFurtherLoanTakeOnRecords() = 0 then
	               window.lbl_Message.innerText = "Further Loan Take On Successfull....!!!"
				   window.lbl_Message.style.background = "green"
				   window.lbl_Message.style.visibility  = "visible"
                   window.tbl_Message.style.visibility  = "visible"
				   DisableControls
			       window.btn_Cancel.style.visibility = "hidden"
			       window.btn_CreateTakeOnRecords.value = "Exit"			   
				end if
	        else
				if CreateTakeOnRecords() = 0 then
				   DisableControls
				   window.lbl_Message.style.visibility  = "visible"
				   window.lbl_Message.innerText = "Take On Successfull....!!!"
				   window.lbl_Message.style.background = "green"
                   window.tbl_Message.style.visibility  = "visible"
                   window.btn_Cancel.style.visibility = "hidden"
			       window.btn_CreateTakeOnRecords.value = "Exit"			   
				end if
			end if
		end if
    end if 
	End if	


End Sub

Sub EnableFields

 if window.pic_UpdateMarketRate.title = "1" then
    window.TrueDBCombo_MarketRate.Enabled = true
 end if
    
 if window.pic_UpdateLinkRate.title = "1" then
    window.TrueDBCombo_LinkRate.Enabled = true
end if

End Sub

Sub DisableControls

window.TrueDBCombo_ACBBank.Enabled = false
window.TrueDBCombo_ACBBranch.Enabled = false
window.TrueDBCombo_AccountType.Enabled = false
window.TrueDBCombo_DebitOrderDay.Enabled = false
window.TrueDBCombo_Education.Enabled = false
window.TrueDBCombo_Language.Enabled = false
window.TrueDBCombo_LinkRate.Enabled = false
window.TrueDBCombo_MarketRateType.Enabled = false
window.TrueDBCombo_Population.Enabled = false
window.TrueDBCombo_SAHLBranch.Enabled = false
window.TrueDBCombo_SPV.Enabled = false

window.ProspectACBAccountNumber.Enabled  = false
window.ProspectDebitOrderAmt.Enabled  = false
window.ProspectPayNotch.Enabled  = false
window.ProspectPaypoint.Enabled  = false
window.ProspectPropertyDesc1.Enabled  = false
window.ProspectPropertyDesc2.Enabled  = false
window.ProspectPropertyDesc3.Enabled  = false
window.ProspectRank.Enabled  = false
window.ProspectSalaryNumber.Enabled  = false
window.ProspectTaxNumber.Enabled  = false


End sub

Function ValidateFields()



ValidateFields = -1
window.lbl_Message.innerText = ""
window.lbl_Message.style.visibility  = "hidden"
window.tbl_Message.style.visibility  = "hidden"
if window.TrueDBCombo_SPV.Text = "Unallocated Loans" then
   window.lbl_Message.style.background = "red"
   window.lbl_Message.innerText = "Invalid SPV....!!!"
   window.lbl_Message.style.visibility  = "visible"
   window.tbl_Message.style.visibility  = "visible"
   window.TrueDBCombo_SPV.Enabled   = true
   window.TrueDBCombo_SPV.focus()
   exit function
end if

if window.TrueDBCombo_AccountType.Text = "Unknown" then
   window.lbl_Message.innerText = "Account Type cannot be Unknown...!!!"   
   window.lbl_Message.style.background = "red"
   window.lbl_Message.style.visibility  = "visible"
   window.tbl_Message.style.visibility  = "visible"
   window.TrueDBCombo_AccountType.Enabled   = true
   window.TrueDBCombo_AccountType.focus()
   exit function
end if

if window.TrueDBCombo_AccountType.Text <> "Unknown" and Trim(window.ProspectACBAccountNumber.Text) = "" and TrueDBCombo_ACBBank.BoundText <> 0  then
   window.lbl_Message.innerText = "Account Number must be captured...!!!"
   window.lbl_Message.style.background = "red"
   window.lbl_Message.style.visibility  = "visible"
   window.tbl_Message.style.visibility  = "visible"
   window.ProspectACBAccountNumber.Enabled   = true
   window.ProspectACBAccountNumber.focus()
   exit function
end if

if window.TrueDBCombo_Education.Text = "Unknown" then
   window.lbl_Message.innerText = "Education cannot be Unknown...!!!"   
   window.lbl_Message.style.background = "red"
   window.lbl_Message.style.visibility  = "visible"
   window.tbl_Message.style.visibility  = "visible"
   window.TrueDBCombo_Education.Enabled   = true
   window.TrueDBCombo_Education.focus()
   exit function
end if

if window.TrueDBCombo_Population.Text = "Unknown" then
   window.lbl_Message.innerText = "Population Group cannot be Unknown...!!!"   
   window.lbl_Message.style.background = "red"
   window.lbl_Message.style.visibility  = "visible"
   window.tbl_Message.style.visibility  = "visible"
   window.TrueDBCombo_Population.Enabled   = true
   window.TrueDBCombo_Population.focus()
   exit function
end if

if window.TrueDBCombo_Language.Text = "Unknown" then
   window.lbl_Message.innerText = "Language cannot be Unknown...!!!"   
   window.lbl_Message.style.background = "red"
   window.lbl_Message.style.visibility  = "visible"
   window.tbl_Message.style.visibility  = "visible"
   window.TrueDBCombo_Language.Enabled   = true
   window.TrueDBCombo_Language.focus()
   exit function
end if

if window.TrueDBCombo_SAHLBranch.Text = "Unknown" then
   window.lbl_Message.innerText = "SAHL Branch cannot be Unknown...!!!"   
   window.lbl_Message.style.background = "red"
   window.lbl_Message.style.visibility  = "visible"
   window.tbl_Message.style.visibility  = "visible"
   window.TrueDBCombo_SAHLBranch.Enabled   = true
   window.TrueDBCombo_SAHLBranch.focus()
   exit function
end if

if Trim(window.ProspectPropertyDesc1.Text) = "" and Trim(window.ProspectPropertyDesc2.Text) = "" and Trim(window.ProspectPropertyDesc3.Text) = "" then
   window.lbl_Message.innerText = "A Property Description must be captured...!!!"
   window.lbl_Message.style.background = "red"
   window.lbl_Message.style.visibility  = "visible"
   window.tbl_Message.style.visibility  = "visible"
   window.ProspectPropertyDesc1.Enabled = true
   window.ProspectPropertyDesc1.Enabled = true
   window.ProspectPropertyDesc3.Enabled = true
   window.ProspectPropertyDesc1.focus()
   exit function
end if


if i_SubsidyFlag = 1 then 
   if Trim(window.ProspectSalaryNumber.Text) = ""  then
       window.lbl_Message.innerText = "A Salary Number must be captured...!!!"
	   window.lbl_Message.style.background = "red"
       window.tbl_Message.style.visibility  = "visible"
       window.lbl_Message.style.visibility  = "visible"
       window.ProspectSalaryNumber.Enabled = true
       window.ProspectSalaryNumber.focus()
       exit function
   end if
    if Trim(window.ProspectPaypoint.Text) = ""  then
       window.lbl_Message.innerText = "A Pay Point must be captured...!!!"
	   window.lbl_Message.style.background = "red"
	   window.lbl_Message.style.visibility  = "visible"
       window.tbl_Message.style.visibility  = "visible"
       window.ProspectPaypoint.Enabled = true
       window.ProspectPaypoint.focus()
       exit function
   end if
end if

ValidateFields = 0
window.tbl_Message.style.visibility  = "visible"
window.lbl_Message.style.visibility  = "visible"
window.lbl_Message.innerText = "Please Wait creating records....!!!"
window.lbl_Message.style.background = "blue"
End Function


Function DetermineWorksStationDateFormat()

  d = formatdatetime("31/12/2000",2)
  i = instr(1,Cstr(d),Cstr(month(d)),1)
  
  if i = 1 then
     DetermineWorksStationDateFormat = "A"
     
  elseif i = 4 then
      DetermineWorksStationDateFormat = "S"
      
  else
      DetermineWorksStationDateFormat = "U"
  end if
  
 End function

Sub SetAccessLightsServer
     
	 	
	sRes1 = "<%=i_MarketRate%>"
    if sRes1 = "Allowed" then
       window.pic_UpdateMarketRate.src = "images/pic_accessallowedblue.bmp"
       window.pic_UpdateMarketRate.title = "1"
    else
       window.pic_UpdateMarketRate.src = "images/pic_accessdeniedblue.bmp"
       window.pic_UpdateMarketRate.title = "0"
	end if
	
	sRes1 = "<%=i_LinkRate%>"
    if sRes1 = "Allowed" then
       window.pic_UpdateLinkRate.src = "images/pic_accessallowedblue.bmp"
       window.pic_UpdateLinkRate.title = "1"
    else
       window.pic_UpdateLinkRate.src = "images/pic_accessdeniedblue.bmp"
       window.pic_UpdateLinkRate.title = "0"
	end if
	

    
end Sub



Sub submit1_onclick
window.focus


End Sub

-->
</script>
</head>
<body background="images/sq_blue1_ltd.jpg" bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0">

<p>
<OBJECT classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAAcAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAIAAAAAQAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACQAAAAEAAAAAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAoAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAALAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADAAAAAIAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA0AAAADAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAOAAAABQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADwAAAAcAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABAAAAAGAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAARAAAACAAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEgAAAAkAAAAAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABMAAAAKAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAUAAAABAAAAAAAAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABYAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAXAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABkAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAaAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABwAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHgAAAB0AAADAAgEAAAIAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB8AAAAdAAAAwAABAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAgAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIQAAAB4AAAAAAQAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACIAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAjAAAAHQAAAIAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAJAAAAB0AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAAAAAA//////////8BAAAAAAAAAAAAAAABAAAA/v///wQAAAAAAAAAAAAAAAUAAAD9////AgAAAAAAAAAAAAAAAgAAAOr///8DAAAAAAAAAAAAAAADAAAA/P///wUAAAAAAAAAAAAAAP/////7////BgAAAAAAAAAAAAAABAAAAPr///8HAAAAAAAAAAAAAAD/////8f///wgAAAAAAAAAAAAAAAYAAADv////CQAAAAAAAAAAAAAABwAAAO7///8KAAAAAAAAAAAAAAAIAAAA+f///wsAAAABAAAAAAAAAP/////r////FAAAAAEAAAAAAAAA//////j///8MAAAAAQAAAAAAAAD/////6f///w0AAAABAAAAAAAAAP/////3////DgAAAAEAAAAAAAAA//////b///8QAAAAAQAAAAAAAAD/////9f///w8AAAABAAAAAAAAAP/////w////EQAAAAEAAAAAAAAA/////+3///8SAAAAAQAAAAAAAAD/////7P///xMAAAABAAAAAAAAAP/////0////GAAAAAEAAAABAAAA//////P///8VAAAAAQAAAAEAAAD/////6P///xYAAAABAAAAAQAAAP/////y////FwAAAAEAAAABAAAA//////T///8cAAAAAQAAAAIAAAD/////8////xkAAAABAAAAAgAAAP/////o////GgAAAAEAAAACAAAA//////L///8bAAAAAQAAAAIAAAD/////CAAAAE5vcm1hbAAFIQAAAOAAAACII7gFiCO4BQAAAAAAAgAAHQAAAEhlYWRpbmcAAAAAAAAAAAAAAAAAFQAAACEAAAAAAQAAHgAAAEZvb3RpbmcAkQAAADADAACwJLgFsCS4BQAAAAAAAwAAHwAAAFNlbGVjdGVkAMq5BYDKuQVgyrkFZUljb24A9v///xsAIAAAAENhcHRpb24AwMq5BTDMuQVAyrkFIMq5BQDKuQXgybkFIQAAAEhpZ2hsaWdodFJvdwDwuAUAALkFsPC4BQEAAAAAIhUHIgAAAEV2ZW5Sb3cAAAAAAAAAAAAAAAAAAAAAAOACAABBAAAAIwAAAE9kZFJvdwAAAAAAAAAIAAAAAAAAAAAAAAAAAAAov7gFJAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAP//AAADAAAAAAAAAB4AAAATAAAAQUNCQmFua0Rlc2NyaXB0aW9uAAAeAAAADAAAAEFDQkJhbmtDb2RlAAsAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAAAAAAACwAAAP//AAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAANAHAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAMgAAAAAAAAAUAAAAVHJ1ZURCQ29tYm9fQUNCQmFuawC9AAAADgAAAEFuaW1hdGVXaW5kb3cAwAAAABMAAABBbmltYXRlV2luZG93Q2xvc2UAvgAAABcAAABBbmltYXRlV2luZG93RGlyZWN0aW9uAL8AAAASAAAAQW5pbWF0ZVdpbmRvd1RpbWUA+P3//wsAAABBcHBlYXJhbmNlAIUAAAAPAAAAQXV0b0NvbXBsZXRpb24AggAAAA0AAABBdXRvRHJvcGRvd24ACP7//wwAAABCb3JkZXJTdHlsZQBzAAAADAAAAEJvdW5kQ29sdW1uAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAgQAAAAsAAABDb21ib1N0eWxlACUAAAAJAAAARGF0YU1vZGUACgAAAAwAAABEZWZDb2xXaWR0aADBAAAAEQAAAERyb3Bkb3duUG9zaXRpb24AiAAAAAkAAABFZGl0Rm9udABfAAAACgAAAEVtcHR5Um93cwD+/f//CAAAAEVuYWJsZWQAkQAAAAoAAABGb290TGluZXMADAAAAAoAAABIZWFkTGluZXMAZQAAAA8AAABJbnRlZ3JhbEhlaWdodABdAAAADwAAAExheW91dEZpbGVOYW1lAFwAAAALAAAATGF5b3V0TmFtZQCHAAAADAAAAExpbWl0VG9MaXN0AGMAAAAKAAAATGlzdEZpZWxkAMUAAAAHAAAATG9ja2VkALwAAAASAAAATWF0Y2hFbnRyeVRpbWVvdXQAowAAAAoAAABNb3VzZUljb24ApAAAAA0AAABNb3VzZVBvaW50ZXIAYQAAAA4AAABNdWx0aXBsZUxpbmVzAJ8AAAAMAAAAT0xFRHJhZ01vZGUAoAAAAAwAAABPTEVEcm9wTW9kZQAPAAAAEAAAAFJvd0RpdmlkZXJTdHlsZQCcAAAACgAAAFJvd01lbWJlcgCDAAAADAAAAFJvd1RyYWNraW5nACMAAAAHAAAAU3BsaXRzAAMCAAAQAAAAX0Ryb3Bkb3duSGVpZ2h0AAICAAAPAAAAX0Ryb3Bkb3duV2lkdGgA0wcAAAkAAABfRXh0ZW50WADUBwAACQAAAF9FeHRlbnRZAAACAAAMAAAAX0xheW91dFR5cGUAEAAAAAsAAABfUm93SGVpZ2h0AAECAAALAAAAX1N0eWxlRGVmcwAEAgAAFgAAAF9XYXNQZXJzaXN0ZWRBc1BpeGVscwA= 
id=TrueDBCombo_ACBBank 
style="BACKGROUND-IMAGE: url(images/sq_blue1_ltd.jpg); FONT-WEIGHT: bold; HEIGHT: 267px; LEFT: 158px; POSITION: absolute; TOP: 219px; WIDTH: 246px; Z-INDEX: -91" 
tabIndex=9 width=246></OBJECT>
<OBJECT classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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//////////8AAAAABwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAgAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAJAAAAAQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACgAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAsAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAMAAAAAgAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADQAAAAMAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA4AAAAFAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAPAAAABwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEAAAAAYAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABEAAAAIAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAASAAAACQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEwAAAAoAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABQAAAAEAAAAAAAAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAVAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABcAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAYAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABoAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAbAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAeAAAAHQAAAMACAQAAAgAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHwAAAB0AAADAAAEAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACAAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAhAAAAHgAAAAABAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIgAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACMAAAAdAAAAgAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAkAAAAHQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAAAAAD//////////wEAAAAAAAAAAAAAAAEAAAD+////BAAAAAAAAAAAAAAABQAAAP3///8CAAAAAAAAAAAAAAACAAAA6v///wMAAAAAAAAAAAAAAAMAAAD8////BQAAAAAAAAAAAAAA//////v///8GAAAAAAAAAAAAAAAEAAAA+v///wcAAAAAAAAAAAAAAP/////x////CAAAAAAAAAAAAAAABgAAAO////8JAAAAAAAAAAAAAAAHAAAA7v///woAAAAAAAAAAAAAAAgAAAD5////CwAAAAEAAAAAAAAA/////+v///8UAAAAAQAAAAAAAAD/////+P///wwAAAABAAAAAAAAAP/////p////DQAAAAEAAAAAAAAA//////f///8OAAAAAQAAAAAAAAD/////9v///xAAAAABAAAAAAAAAP/////1////DwAAAAEAAAAAAAAA//////D///8RAAAAAQAAAAAAAAD/////7f///xIAAAABAAAAAAAAAP/////s////EwAAAAEAAAAAAAAA//////T///8YAAAAAQAAAAEAAAD/////8////xUAAAABAAAAAQAAAP/////o////FgAAAAEAAAABAAAA//////L///8XAAAAAQAAAAEAAAD/////9P///xwAAAABAAAAAgAAAP/////z////GQAAAAEAAAACAAAA/////+j///8aAAAAAQAAAAIAAAD/////8v///xsAAAABAAAAAgAAAP////8IAAAATm9ybWFsAAAA67kFCgAAAAoAAAAAAAAAAAAAAJAiuQUdAAAASGVhZGluZwAACQAAUQAAACEIj/tkARsQhO0IACsuxxMeAAAARm9vdGluZwAxAAAAEAAAAAxBuQX8I7kF4AgAACEAAAAfAAAAU2VsZWN0ZWQAAAAAQAAAADQhuAU0IbgFQK65BRCuuQUgAAAAQ2FwdGlvbgAEAAAAeOrNBQACAABAWLkFIQAAAGAAAAAhAAAASGlnaGxpZ2h0Um93AAAAAGQhuAVkIbgFAQAAgHjqzQUiAAAARXZlblJvdwDoGAAAeOrNBQMCAAD/////IQAAAMAAAAAjAAAAT2RkUm93AACBAAAAAAEAAJQhuAWUIbgFAAAAAHjqzQUkAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAALAAAA//8AAAMAAAAAAAAAHgAAABUAAABBQ0JCcmFuY2hEZXNjcmlwdGlvbgAAAAAeAAAADgAAAEFDQkJyYW5jaENvZGUAAAALAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAAAAAAAAsAAAD//wAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADQBwAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAAAAAAALAAAAAAAAADIAAAAAAAAAFgAAAFRydWVEQkNvbWJvX0FDQkJyYW5jaAC9AAAADgAAAEFuaW1hdGVXaW5kb3cAwAAAABMAAABBbmltYXRlV2luZG93Q2xvc2UAvgAAABcAAABBbmltYXRlV2luZG93RGlyZWN0aW9uAL8AAAASAAAAQW5pbWF0ZVdpbmRvd1RpbWUA+P3//wsAAABBcHBlYXJhbmNlAIUAAAAPAAAAQXV0b0NvbXBsZXRpb24AggAAAA0AAABBdXRvRHJvcGRvd24ACP7//wwAAABCb3JkZXJTdHlsZQBzAAAADAAAAEJvdW5kQ29sdW1uAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAgQAAAAsAAABDb21ib1N0eWxlACUAAAAJAAAARGF0YU1vZGUACgAAAAwAAABEZWZDb2xXaWR0aADBAAAAEQAAAERyb3Bkb3duUG9zaXRpb24AiAAAAAkAAABFZGl0Rm9udABfAAAACgAAAEVtcHR5Um93cwD+/f//CAAAAEVuYWJsZWQAkQAAAAoAAABGb290TGluZXMADAAAAAoAAABIZWFkTGluZXMAZQAAAA8AAABJbnRlZ3JhbEhlaWdodABdAAAADwAAAExheW91dEZpbGVOYW1lAFwAAAALAAAATGF5b3V0TmFtZQCHAAAADAAAAExpbWl0VG9MaXN0AGMAAAAKAAAATGlzdEZpZWxkAMUAAAAHAAAATG9ja2VkALwAAAASAAAATWF0Y2hFbnRyeVRpbWVvdXQAowAAAAoAAABNb3VzZUljb24ApAAAAA0AAABNb3VzZVBvaW50ZXIAYQAAAA4AAABNdWx0aXBsZUxpbmVzAJ8AAAAMAAAAT0xFRHJhZ01vZGUAoAAAAAwAAABPTEVEcm9wTW9kZQAPAAAAEAAAAFJvd0RpdmlkZXJTdHlsZQCcAAAACgAAAFJvd01lbWJlcgCDAAAADAAAAFJvd1RyYWNraW5nACMAAAAHAAAAU3BsaXRzAAMCAAAQAAAAX0Ryb3Bkb3duSGVpZ2h0AAICAAAPAAAAX0Ryb3Bkb3duV2lkdGgA0wcAAAkAAABfRXh0ZW50WADUBwAACQAAAF9FeHRlbnRZAAACAAAMAAAAX0xheW91dFR5cGUAEAAAAAsAAABfUm93SGVpZ2h0AAECAAALAAAAX1N0eWxlRGVmcwAEAgAAFgAAAF9XYXNQZXJzaXN0ZWRBc1BpeGVscwA= 
id=TrueDBCombo_ACBBranch 
style="BACKGROUND-IMAGE: url(images/sq_blue1_ltd.jpg); FONT-WEIGHT: bold; HEIGHT: 239px; LEFT: 158px; POSITION: absolute; TOP: 247px; WIDTH: 246px; Z-INDEX: 100" 
tabIndex=10 width=246></OBJECT>
<table border="0" cellPadding="1" cellSpacing="1" style="FONT-FAMILY: fantasy; 
HEIGHT: 113px; WIDTH: 812px" width="75%">
  
  <tr>
    <td align="right" noWrap style="COLOR: white">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<IMG alt ="" height=96 src       ="images/sq_blue1_ltdc.gif" style="LEFT: 0px; POSITION: absolute; TOP: 0px; Z-INDEX: -88" width=96 >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      Salutation</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D 
      id=ProspectSalutation 
      style="FONT-WEIGHT: bold; HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 246px" 
      tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6509"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td>
    <td align="right" noWrap style="COLOR: white">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  Tertiary Education</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D id=Dummy 
      style="FONT-WEIGHT: bold; HEIGHT: 25px; LEFT: 4px; TOP: 1px; VISIBILITY: hidden; WIDTH: 243px" 
      tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6430"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap style="COLOR: white">First Names</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D 
      id=ProspectFirstNames 
      style="FONT-WEIGHT: bold; HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 244px" 
      tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6456"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td>
    <td align="right" noWrap style="COLOR: white">Population Group</td>
    <td noWrap></td></tr>
  <tr>
    <td align="right" noWrap style="COLOR: white">&nbsp;Surname</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D 
      id=ProspectSurname 
      style="FONT-WEIGHT: bold; HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 244px" 
      tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6456"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td>
    <td align="right" noWrap style="COLOR: white">Language</td>
    <td noWrap></td></tr>
  <tr>
    <td noWrap align="right" style="COLOR: white">Tax Number</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D 
      id=ProspectTaxNumber 
      style="FONT-WEIGHT: bold; HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 244px" 
      tabIndex=1><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6456"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td>
    <td align="right" noWrap style="COLOR: white">&nbsp; 
      SAHL Branch</td>
    <td noWrap></td></tr></table></p>
<OBJECT classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAP//////////AAAAAAcAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAIAAAAAQAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACQAAAAEAAAAAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAoAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAALAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADAAAAAIAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA0AAAADAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAOAAAABQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADwAAAAcAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABAAAAAGAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAARAAAACAAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEgAAAAkAAAAAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABMAAAAKAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAUAAAABAAAAAAAAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABYAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAXAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABkAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAaAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABwAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHgAAAB0AAADAAgEAAAIAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB8AAAAdAAAAwAABAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAgAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIQAAAB4AAAAAAQAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACIAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAjAAAAHQAAAIAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAJAAAAB0AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAAAAAA//////////8BAAAAAAAAAAAAAAABAAAA/v///wQAAAAAAAAAAAAAAAUAAAD9////AgAAAAAAAAAAAAAAAgAAAOr///8DAAAAAAAAAAAAAAADAAAA/P///wUAAAAAAAAAAAAAAP/////7////BgAAAAAAAAAAAAAABAAAAPr///8HAAAAAAAAAAAAAAD/////8f///wgAAAAAAAAAAAAAAAYAAADv////CQAAAAAAAAAAAAAABwAAAO7///8KAAAAAAAAAAAAAAAIAAAA+f///wsAAAABAAAAAAAAAP/////r////FAAAAAEAAAAAAAAA//////j///8MAAAAAQAAAAAAAAD/////6f///w0AAAABAAAAAAAAAP/////3////DgAAAAEAAAAAAAAA//////b///8QAAAAAQAAAAAAAAD/////9f///w8AAAABAAAAAAAAAP/////w////EQAAAAEAAAAAAAAA/////+3///8SAAAAAQAAAAAAAAD/////7P///xMAAAABAAAAAAAAAP/////0////GAAAAAEAAAABAAAA//////P///8VAAAAAQAAAAEAAAD/////6P///xYAAAABAAAAAQAAAP/////y////FwAAAAEAAAABAAAA//////T///8cAAAAAQAAAAIAAAD/////8////xkAAAABAAAAAgAAAP/////o////GgAAAAEAAAACAAAA//////L///8bAAAAAQAAAAIAAAD/////CAAAAE5vcm1hbAAFAAAAAGl4ZWxzAAAAAAAAADEAAAAwAAAAHQAAAEhlYWRpbmcAAAARAAAAAAAjAAAAAAAAADEAAAAQAAAAHgAAAEZvb3RpbmcAMQAAABABAAAfAAAAoCO4BTEAAAAQAAAAHwAAAFNlbGVjdGVkAAAAACUAAAAAAAAA/////yAAAAAAAAAAIAAAAENhcHRpb24AAAAAAIDELAYAALgFcMQsBgAAAAAAAAAAIQAAAEhpZ2hsaWdodFJvdwAAAAAAAAAAAAAAADEAAABwAAAAIgAAAEV2ZW5Sb3cAEKG5BfCguQXQoLkFsKC5BUAAAAAxAAAAIwAAAE9kZFJvdwAFMQAAAOAAAADcq7kFiCO4BQAAAAAAAAAAJAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAP//AAADAAAAAAAAAB4AAAAQAAAARWR1Y2F0aW9uRGV0YWlsAB4AAAAQAAAARWR1Y2F0aW9uTnVtYmVyAAsAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAAAAAAACwAAAP//AAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAANAHAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAMgAAAAAAAAAWAAAAVHJ1ZURCQ29tYm9fRWR1Y2F0aW9uAL0AAAAOAAAAQW5pbWF0ZVdpbmRvdwDAAAAAEwAAAEFuaW1hdGVXaW5kb3dDbG9zZQC+AAAAFwAAAEFuaW1hdGVXaW5kb3dEaXJlY3Rpb24AvwAAABIAAABBbmltYXRlV2luZG93VGltZQD4/f//CwAAAEFwcGVhcmFuY2UAhQAAAA8AAABBdXRvQ29tcGxldGlvbgCCAAAADQAAAEF1dG9Ecm9wZG93bgAI/v//DAAAAEJvcmRlclN0eWxlAHMAAAAMAAAAQm91bmRDb2x1bW4A+v3//wgAAABDYXB0aW9uAGAAAAAJAAAAQ2VsbFRpcHMAfgAAAA4AAABDZWxsVGlwc0RlbGF5AH0AAAAOAAAAQ2VsbFRpcHNXaWR0aACPAAAADgAAAENvbHVtbkZvb3RlcnMABwAAAA4AAABDb2x1bW5IZWFkZXJzAAgAAAAIAAAAQ29sdW1ucwCBAAAACwAAAENvbWJvU3R5bGUAJQAAAAkAAABEYXRhTW9kZQAKAAAADAAAAERlZkNvbFdpZHRoAMEAAAARAAAARHJvcGRvd25Qb3NpdGlvbgCIAAAACQAAAEVkaXRGb250AF8AAAAKAAAARW1wdHlSb3dzAP79//8IAAAARW5hYmxlZACRAAAACgAAAEZvb3RMaW5lcwAMAAAACgAAAEhlYWRMaW5lcwBlAAAADwAAAEludGVncmFsSGVpZ2h0AF0AAAAPAAAATGF5b3V0RmlsZU5hbWUAXAAAAAsAAABMYXlvdXROYW1lAIcAAAAMAAAATGltaXRUb0xpc3QAYwAAAAoAAABMaXN0RmllbGQAxQAAAAcAAABMb2NrZWQAvAAAABIAAABNYXRjaEVudHJ5VGltZW91dACjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgBhAAAADgAAAE11bHRpcGxlTGluZXMAnwAAAAwAAABPTEVEcmFnTW9kZQCgAAAADAAAAE9MRURyb3BNb2RlAA8AAAAQAAAAUm93RGl2aWRlclN0eWxlAJwAAAAKAAAAUm93TWVtYmVyAIMAAAAMAAAAUm93VHJhY2tpbmcAIwAAAAcAAABTcGxpdHMAAwIAABAAAABfRHJvcGRvd25IZWlnaHQAAgIAAA8AAABfRHJvcGRvd25XaWR0aADTBwAACQAAAF9FeHRlbnRYANQHAAAJAAAAX0V4dGVudFkAAAIAAAwAAABfTGF5b3V0VHlwZQAQAAAACwAAAF9Sb3dIZWlnaHQAAQIAAAsAAABfU3R5bGVEZWZzAAQCAAAWAAAAX1dhc1BlcnNpc3RlZEFzUGl4ZWxzAA== 
height=28 id=TrueDBCombo_Education 
style="BACKGROUND-IMAGE: url(images/sq_blue1_ltd.jpg); FONT-WEIGHT: bold; HEIGHT: 124px; LEFT: 567px; POSITION: absolute; TOP: 1px; WIDTH: 245px; Z-INDEX: -87" 
tabIndex=2></OBJECT>
<OBJECT classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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DQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAHAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwABQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB0AAABIZWFkaW5nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB4AAABGb290aW5nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB8AAABTZWxlY3RlZAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAAABDYXB0aW9uAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACEAAABIaWdobGlnaHRSb3cAAAAAAAAAAAAAAAAAAAAAAAAAACIAAABFdmVuUm93AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACMAAABPZGRSb3cAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACQAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAsAAAD//wAAAwAAAAAAAAAeAAAAGwAAAFBvcHVsYXRpb25Hcm91cERlc2NyaXB0aW9uAAAeAAAAFgAAAFBvcHVsYXRpb25Hcm91cE51bWJlcgAAAAsAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAAAAAAACwAAAP//AAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAANAHAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAMgAAAAAAAAAXAAAAVHJ1ZURCQ29tYm9fUG9wdWxhdGlvbgC9AAAADgAAAEFuaW1hdGVXaW5kb3cAwAAAABMAAABBbmltYXRlV2luZG93Q2xvc2UAvgAAABcAAABBbmltYXRlV2luZG93RGlyZWN0aW9uAL8AAAASAAAAQW5pbWF0ZVdpbmRvd1RpbWUA+P3//wsAAABBcHBlYXJhbmNlAIUAAAAPAAAAQXV0b0NvbXBsZXRpb24AggAAAA0AAABBdXRvRHJvcGRvd24ACP7//wwAAABCb3JkZXJTdHlsZQBzAAAADAAAAEJvdW5kQ29sdW1uAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAgQAAAAsAAABDb21ib1N0eWxlACUAAAAJAAAARGF0YU1vZGUACgAAAAwAAABEZWZDb2xXaWR0aADBAAAAEQAAAERyb3Bkb3duUG9zaXRpb24AiAAAAAkAAABFZGl0Rm9udABfAAAACgAAAEVtcHR5Um93cwD+/f//CAAAAEVuYWJsZWQAkQAAAAoAAABGb290TGluZXMADAAAAAoAAABIZWFkTGluZXMAZQAAAA8AAABJbnRlZ3JhbEhlaWdodABdAAAADwAAAExheW91dEZpbGVOYW1lAFwAAAALAAAATGF5b3V0TmFtZQCHAAAADAAAAExpbWl0VG9MaXN0AGMAAAAKAAAATGlzdEZpZWxkAMUAAAAHAAAATG9ja2VkALwAAAASAAAATWF0Y2hFbnRyeVRpbWVvdXQAowAAAAoAAABNb3VzZUljb24ApAAAAA0AAABNb3VzZVBvaW50ZXIAYQAAAA4AAABNdWx0aXBsZUxpbmVzAJ8AAAAMAAAAT0xFRHJhZ01vZGUAoAAAAAwAAABPTEVEcm9wTW9kZQAPAAAAEAAAAFJvd0RpdmlkZXJTdHlsZQCcAAAACgAAAFJvd01lbWJlcgCDAAAADAAAAFJvd1RyYWNraW5nACMAAAAHAAAAU3BsaXRzAAMCAAAQAAAAX0Ryb3Bkb3duSGVpZ2h0AAICAAAPAAAAX0Ryb3Bkb3duV2lkdGgA0wcAAAkAAABfRXh0ZW50WADUBwAACQAAAF9FeHRlbnRZAAACAAAMAAAAX0xheW91dFR5cGUAEAAAAAsAAABfUm93SGVpZ2h0AAECAAALAAAAX1N0eWxlRGVmcwAEAgAAFgAAAF9XYXNQZXJzaXN0ZWRBc1BpeGVscwA= 
height=28 id=TrueDBCombo_Population 
style="BACKGROUND-IMAGE: url(images/sq_blue1_ltd.jpg); FONT-WEIGHT: bold; HEIGHT: 157px; LEFT: 567px; POSITION: absolute; TOP: 29px; WIDTH: 245px; Z-INDEX: -97" 
tabIndex=3 width=245></OBJECT>
<OBJECT classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAA//////////8AAAAABwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAgAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAJAAAAAQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACgAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAsAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAMAAAAAgAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADQAAAAMAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA4AAAAFAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAPAAAABwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEAAAAAYAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABEAAAAIAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAASAAAACQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEwAAAAoAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABQAAAAEAAAAAAAAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAVAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABcAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAYAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABoAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAbAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAeAAAAHQAAAMACAQAAAgAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHwAAAB0AAADAAAEAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACAAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAhAAAAHgAAAAABAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIgAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACMAAAAdAAAAgAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAkAAAAHQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAAAAAD//////////wEAAAAAAAAAAAAAAAEAAAD+////BAAAAAAAAAAAAAAABQAAAP3///8CAAAAAAAAAAAAAAACAAAA6v///wMAAAAAAAAAAAAAAAMAAAD8////BQAAAAAAAAAAAAAA//////v///8GAAAAAAAAAAAAAAAEAAAA+v///wcAAAAAAAAAAAAAAP/////x////CAAAAAAAAAAAAAAABgAAAO////8JAAAAAAAAAAAAAAAHAAAA7v///woAAAAAAAAAAAAAAAgAAAD5////CwAAAAEAAAAAAAAA/////+v///8UAAAAAQAAAAAAAAD/////+P///wwAAAABAAAAAAAAAP/////p////DQAAAAEAAAAAAAAA//////f///8OAAAAAQAAAAAAAAD/////9v///xAAAAABAAAAAAAAAP/////1////DwAAAAEAAAAAAAAA//////D///8RAAAAAQAAAAAAAAD/////7f///xIAAAABAAAAAAAAAP/////s////EwAAAAEAAAAAAAAA//////T///8YAAAAAQAAAAEAAAD/////8////xUAAAABAAAAAQAAAP/////o////FgAAAAEAAAABAAAA//////L///8XAAAAAQAAAAEAAAD/////9P///xwAAAABAAAAAgAAAP/////z////GQAAAAEAAAACAAAA/////+j///8aAAAAAQAAAAIAAAD/////8v///xsAAAABAAAAAgAAAP////8IAAAATm9ybWFsAAXACgAAIQAAADQpuAU0KbgFAAAAACDkugUdAAAASGVhZGluZwB0Um93ADG7BXAxuwUAMbsFkDC7BSAwuwUeAAAARm9vdGluZwAAAAAACgAAAAIAAAAAAAAAMQAAAJAIAAAfAAAAU2VsZWN0ZWQAAAAAAAgAAFwZuwUkK7gFAAAAAAAAAAAgAAAAQ2FwdGlvbgAAAAAAAAAAAAAAAAACQAAAAAAAAAAAAAAhAAAASGlnaGxpZ2h0Um93AAAAACxwuwXMGLsFAQAAAABHFQciAAAARXZlblJvdwAAAAAAAAAAAAAAAAACQAAAAAAAAAAAAAAjAAAAT2RkUm93AAVBAAAA4AUAAFwZuwUkK7gFAAAAAAAAAAAkAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAALAAAA//8AAAMAAAAAAAAAHgAAABQAAABMYW5ndWFnZURlc2NyaXB0aW9uAB4AAAAPAAAATGFuZ3VhZ2VOdW1iZXIAAAsAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAAAAAAACwAAAP//AAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAANAHAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAMgAAAAAAAAAVAAAAVHJ1ZURCQ29tYm9fTGFuZ3VhZ2UAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQCFAAAADwAAAEF1dG9Db21wbGV0aW9uAIIAAAANAAAAQXV0b0Ryb3Bkb3duAAj+//8MAAAAQm9yZGVyU3R5bGUAcwAAAAwAAABCb3VuZENvbHVtbgD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAIEAAAALAAAAQ29tYm9TdHlsZQAlAAAACQAAAERhdGFNb2RlAAoAAAAMAAAARGVmQ29sV2lkdGgAwQAAABEAAABEcm9wZG93blBvc2l0aW9uAIgAAAAJAAAARWRpdEZvbnQAXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkAJEAAAAKAAAARm9vdExpbmVzAAwAAAAKAAAASGVhZExpbmVzAGUAAAAPAAAASW50ZWdyYWxIZWlnaHQAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAhwAAAAwAAABMaW1pdFRvTGlzdABjAAAACgAAAExpc3RGaWVsZADFAAAABwAAAExvY2tlZAC8AAAAEgAAAE1hdGNoRW50cnlUaW1lb3V0AKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAnAAAAAoAAABSb3dNZW1iZXIAgwAAAAwAAABSb3dUcmFja2luZwAjAAAABwAAAFNwbGl0cwADAgAAEAAAAF9Ecm9wZG93bkhlaWdodAACAgAADwAAAF9Ecm9wZG93bldpZHRoANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA 
height=28 id=TrueDBCombo_Language 
style="BACKGROUND-IMAGE: url(images/sq_blue1_ltd.jpg); HEIGHT: 103px; LEFT: 567px; POSITION: absolute; TOP: 57px; WIDTH: 245px; Z-INDEX: -99" 
tabIndex=4 width=245></OBJECT>
<input id="btn_Cancel" name="btn_Cancel" style="BACKGROUND-COLOR: #eb780f; BORDER-BOTTOM-STYLE: ridge; BORDER-LEFT-STYLE: ridge; BORDER-RIGHT-STYLE: ridge; BORDER-TOP-STYLE: ridge; COLOR: white; HEIGHT: 60px; LEFT: 184px; POSITION: absolute; TOP: 470px; WIDTH: 169px; Z-INDEX: 102" title="Cancel Take On" type="button" value="Cancel Take On" height="60">
<OBJECT classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,gzViDaLb0RG13wBgl2CJ0P7/AAAFAAIAgzViDaLb0RG13wBgl2CJ0AEAAAAhCI/7ZAEbEITtCAArLscTQAAAADwfAAAyAAAA0wcAAJgBAADUBwAAoAEAAAACAACoAQAAEAAAALABAAAEAgAAuAEAAAMCAADAAQAAAgIAAMgBAAAIAAAA0AEAACMAAAAEBQAA+P3//yALAAAI/v//KAsAAIEAAAAwCwAAhQAAADgLAACHAAAAQAsAAAcAAABICwAAjwAAAFALAAAlAAAAWAsAAAoAAABgCwAA/v3//2gLAAAMAAAAcAsAAJEAAAB4CwAADwAAAIALAAD6/f//iAsAAIgAAACUCwAAAQIAALwLAABcAAAAPBoAAF0AAABIGgAAYQAAAFQaAABfAAAAXBoAAGAAAABkGgAAYwAAAGwaAABzAAAAhBoAAGUAAACgGgAAfQAAAKgaAAB+AAAAsBoAAIIAAAC4GgAAgwAAAMAaAACcAAAAyBoAAKMAAADUGgAApAAAANwaAAC8AAAA5BoAAJ8AAADsGgAAoAAAAPQaAAC9AAAA/BoAAL4AAAAEGwAAvwAAAAwbAADAAAAAFBsAAMEAAAAcGwAAxQAAACQbAAAAAAAALBsAAAMAAABSGQAAAwAAAOsMAAACAAAABAAAAAMAAAABAACAAgAAAAAAAAADAAAAOwoAAAMAAAAAAAAASxAAAAIAAACWAQAA/v8AAAUAAgBxNWINotvREbXfAGCXYInQAQAAACEIj/tkARsQhO0IACsuxxNMAgAAZgEAAAkAAAACAgAAUAAAAAQCAABYAAAAGAAAAGAAAAAFAAAAZAAAADoAAACAAAAACAAAAIwAAAARAAAAqAAAAE4AAAC0AAAAAAAAALwAAAADAAAAAAAAAAIAAAAFAAAAAAAAAB4AAAARAAAAU0FITEJyYW5jaE51bWJlcgAAAAAeAAAAAQAAAAAAAAAeAAAAEQAAAFNBSExCcmFuY2hOdW1iZXIAAAAAHgAAAAEAAAAAAAAACwAAAAAAAAAJAAAAAAAAAAgAAABDb2x1bW4wAAUAAAAIAAAAQ2FwdGlvbgAIAAAACgAAAERhdGFGaWVsZAA6AAAACwAAAEZvb3RlclRleHQATgAAAAYAAABHcm91cAARAAAADQAAAE51bWJlckZvcm1hdAAYAAAACwAAAFZhbHVlSXRlbXMABAIAAA8AAABfTWF4Q29tYm9JdGVtcwACAgAADAAAAF9WbGlzdFN0eWxlAI4BAAD+/wAABQACAHE1Yg2i29ERtd8AYJdgidABAAAAIQiP+2QBGxCE7QgAKy7HE+YDAABeAQAACQAAAAICAABQAAAABAIAAFgAAAAYAAAAYAAAAAUAAABkAAAAOgAAAHwAAAAIAAAAiAAAABEAAACgAAAATgAAAKwAAAAAAAAAtAAAAAMAAAAAAAAAAgAAAAUAAAAAAAAAHgAAAA8AAABTQUhMQnJhbmNoTmFtZQAAHgAAAAEAAAAAAAAAHgAAAA8AAABTQUhMQnJhbmNoTmFtZQAAHgAAAAEAAAAAAAAACwAAAAAAAAAJAAAAAAAAAAgAAABDb2x1bW4xAAUAAAAIAAAAQ2FwdGlvbgAIAAAACgAAAERhdGFGaWVsZAA6AAAACwAAAEZvb3RlclRleHQATgAAAAYAAABHcm91cAARAAAADQAAAE51bWJlckZvcm1hdAAYAAAACwAAAFZhbHVlSXRlbXMABAIAAA8AAABfTWF4Q29tYm9JdGVtcwACAgAADAAAAF9WbGlzdFN0eWxlAEsQAAABAAAADQYAAP7/AAAFAAIAczViDaLb0RG13wBgl2CJ0AEAAAAhCI/7ZAEbEITtCAArLscTgAUAAN0FAAASAAAABgIAAJgAAAAgAAAAoAAAADoAAACoAAAAOwAAALAAAAADAAAAuAAAAAQAAADAAAAABwAAAMgAAAAGAAAA0AAAAA8AAADYAAAAEQAAAOAAAAADAgAA6AAAACkAAAAsBAAAKgAAADQEAAAvAAAAPAQAADIAAABEBAAAMwAAAEwEAAA1AAAAWAQAAAAAAABgBAAAAwAAAAAAAAALAAAA//8AAAsAAAD//wAACwAAAAAAAAACAAAAAQAAAAsAAAAAAAAAAwAAAAAAAAACAAAAAQAAAAsAAAAAAAAAAwAAAAQAAABBAAAAQAMAAEJpZ1JlZAECAgAAAAEAAAAZAAAABAAAABkFAADRDAAAAAAAAAQAAAABBQAAAQAAAAAAAAAEAAAAogUAAGcMAAAAAQAABAAAAP8EAACAgIAAAAAAAAQAAADuBAAAAAAAAAAAAAAEAAAABwUAAAAAAAAAAQAABAAAACUEAAAEAAAAAAAAAAQAAAArBAAAAAAAAAAAAAAEAAAA1AQAAAAAAAAAAgAABAAAAMgEAAAAAAAAAAAAAAQAAACEBAAAAAAAAAAAAAAEAAAAlAUAAAEAAAAAuLsFBAAAACMEAAABAAAAAAMAAAQAAADIBQAAAAAAAAAAAAAEAAAAwgUAAAAAAAAAAAAABAAAAOYFAAAAAAAAAAMAAAQAAADqBQAAAAAAAAAAAAAEAAAA+QUAAAEAAAAAAAAABAAAAMsFAAAAAAAAAAMAAAQAAACSBQAAAAAAAAAAAAAEAAAAsgUAAAAAAAAAAAAABAAAAL4FAAAAAAAAAAQAAAQAAAD7BQAAAAAAAAC3uwUEAAAA8wUAAAEAAAAAubsFBAAAAPUFAAABAAAAALm7BQIAAAAZAAAABAAAABkFAADRDAAAAAAAAAQAAAABBQAAAQAAAAAFAAAEAAAAogUAAGcMAAAABQAABAAAAP8EAACAgIAAAAAAAAQAAADuBAAAAAAAAACuuwUEAAAABwUAAAAAAAAArrsFBAAAACUEAAAEAAAAAJC7BQQAAAArBAAAAQAAAACRuwUEAAAA1AQAAAAAAAAAAAAABAAAAMgEAAAAAAAAAAAAAAQAAACEBAAAAAAAAAAAAAAEAAAAlAUAAAEAAAAAr7sFBAAAACMEAAACAAAAAAAAAAQAAADIBQAAAAAAAAAAAAAEAAAAwgUAAAAAAAAAAQAABAAAAOYFAAAAAAAAAAEAAAQAAADqBQAAAAAAAAABAAAEAAAA+QUAAAEAAAAAJ7sFBAAAAMsFAAAAAAAAAHO7BQQAAACSBQAAAAAAAAACAAAEAAAAsgUAAAAAAAAAAAAABAAAAL4FAAAAAAAAAAIAAAQAAAD7BQAAAAAAAACtuwUEAAAA8wUAAAEAAAAAtrsFBAAAAPUFAAABAAAAAK+7BQsAAAD//wAACwAAAAAAAAALAAAAAAAAAAsAAAD//wAAHgAAAAEAAAAAAAAAAwAAAAAAAAASAAAAAAAAAAcAAABTcGxpdDAAKgAAAA0AAABBbGxvd0NvbE1vdmUAKQAAAA8AAABBbGxvd0NvbFNlbGVjdAAPAAAADwAAAEFsbG93Um93U2l6aW5nAAQAAAAMAAAAQWxsb3dTaXppbmcAMgAAABQAAABBbHRlcm5hdGluZ1Jvd1N0eWxlADsAAAASAAAAQW5jaG9yUmlnaHRDb2x1bW4AMwAAAAgAAABDYXB0aW9uADUAAAANAAAARGl2aWRlclN0eWxlACAAAAASAAAARXh0ZW5kUmlnaHRDb2x1bW4ALwAAAA4AAABGZXRjaFJvd1N0eWxlADoAAAATAAAAUGFydGlhbFJpZ2h0Q29sdW1uABEAAAALAAAAU2Nyb2xsQmFycwADAAAADAAAAFNjcm9sbEdyb3VwAAYAAAAFAAAAU2l6ZQAHAAAACQAAAFNpemVNb2RlAAMCAAANAAAAX0NvbHVtblByb3BzAAYCAAALAAAAX1VzZXJGbGFncwAAAAADAAAAAQAAAAMAAAABAAAAAwAAAAIAAAALAAAAAAAAAAsAAAAAAAAACwAAAAAAAAALAAAAAAAAAAMAAAAAAAAABAAAAAAAAAALAAAA//8AAAQAAAAAAIA/BAAAAAAAgD8DAAAAAAAAAB4AAAABAAAAAAAAAEYAAAAgAAAAA1LjC5GPzhGd4wCqAEu4UQEAAAC8Atx8AQAFQXJpYWxBAAAAfA4AAFVTdHlsZQEFAAAAACUAAAAAAAAA//////8JAP8AAAAABAAAAAUAAIAIAACAsAQAAFRpbWVzIE5ldyBSb21hbgAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAAQAAAAAAAAA/CAD/AQAAAAQAAAAFAACACAAAgM8DAABBcmlhbAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAIAAAABAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAADAAAAAQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABAAAAAIAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAUAAAACAAAAwAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAGAAAAAQAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAgAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAJAAAAAQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACgAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAsAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAMAAAAAgAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADQAAAAMAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA4AAAAFAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAPAAAABwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEAAAAAYAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABEAAAAIAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAASAAAACQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEwAAAAoAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABQAAAAEAAAAAAAAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAVAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABcAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAYAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABoAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAbAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAeAAAAHQAAAMACAQAAAgAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHwAAAB0AAADAAAEAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACAAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAhAAAAHgAAAAABAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIgAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACMAAAAdAAAAgAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAkAAAAHQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAAAAAD//////////wEAAAAAAAAAAAAAAAEAAAD+////BAAAAAAAAAAAAAAABQAAAP3///8CAAAAAAAAAAAAAAACAAAA6v///wMAAAAAAAAAAAAAAAMAAAD8////BQAAAAAAAAAAAAAA//////v///8GAAAAAAAAAAAAAAAEAAAA+v///wcAAAAAAAAAAAAAAP/////x////CAAAAAAAAAAAAAAABgAAAO////8JAAAAAAAAAAAAAAAHAAAA7v///woAAAAAAAAAAAAAAAgAAAD5////CwAAAAEAAAAAAAAA/////+v///8UAAAAAQAAAAAAAAD/////+P///wwAAAABAAAAAAAAAP/////p////DQAAAAEAAAAAAAAA//////f///8OAAAAAQAAAAAAAAD/////9v///xAAAAABAAAAAAAAAP/////1////DwAAAAEAAAAAAAAA//////D///8RAAAAAQAAAAAAAAD/////7f///xIAAAABAAAAAAAAAP/////s////EwAAAAEAAAAAAAAA//////T///8YAAAAAQAAAAEAAAD/////8////xUAAAABAAAAAQAAAP/////o////FgAAAAEAAAABAAAA//////L///8XAAAAAQAAAAEAAAD/////9P///xwAAAABAAAAAgAAAP/////z////GQAAAAEAAAACAAAA/////+j///8aAAAAAQAAAAIAAAD/////8v///xsAAAABAAAAAgAAAP////8IAAAATm9ybWFsAAV0Um93ACe7BVAnuwXgJrsFcCa7BQAmuwUdAAAASGVhZGluZwAgfLsFsHu7BUB7uwVwersF4HO7BUBzuwUeAAAARm9vdGluZwAAAAAAcAAAAFwpuAVcKbgFQAAAADEAAAAfAAAAU2VsZWN0ZWQAAAAAAAAAAIIAAABQdLsFMQAAADACAAAgAAAAQ2FwdGlvbgAgZ3JvdXAgYnkgdGhhdCBjb2x1bW4AuAUhAAAASGlnaGxpZ2h0Um93AICzBSMAAACAEbMFMQAAABAAAAAiAAAARXZlblJvdwBgAAAAMQAAAPb///9lbnRYMQAAABAAAAAjAAAAT2RkUm93AAUxAAAAMAAAAB0AAAA8KbgFMQAAAEAAAAAkAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAALAAAA//8AAAMAAAAAAAAAHgAAAA8AAABTQUhMQnJhbmNoTmFtZQAAHgAAABEAAABTQUhMQnJhbmNoTnVtYmVyAAAAAAsAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAAAAAAACwAAAP//AAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAANAHAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAMgAAAAAAAAAXAAAAVHJ1ZURCQ29tYm9fU0FITEJyYW5jaAC9AAAADgAAAEFuaW1hdGVXaW5kb3cAwAAAABMAAABBbmltYXRlV2luZG93Q2xvc2UAvgAAABcAAABBbmltYXRlV2luZG93RGlyZWN0aW9uAL8AAAASAAAAQW5pbWF0ZVdpbmRvd1RpbWUA+P3//wsAAABBcHBlYXJhbmNlAIUAAAAPAAAAQXV0b0NvbXBsZXRpb24AggAAAA0AAABBdXRvRHJvcGRvd24ACP7//wwAAABCb3JkZXJTdHlsZQBzAAAADAAAAEJvdW5kQ29sdW1uAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAgQAAAAsAAABDb21ib1N0eWxlACUAAAAJAAAARGF0YU1vZGUACgAAAAwAAABEZWZDb2xXaWR0aADBAAAAEQAAAERyb3Bkb3duUG9zaXRpb24AiAAAAAkAAABFZGl0Rm9udABfAAAACgAAAEVtcHR5Um93cwD+/f//CAAAAEVuYWJsZWQAkQAAAAoAAABGb290TGluZXMADAAAAAoAAABIZWFkTGluZXMAZQAAAA8AAABJbnRlZ3JhbEhlaWdodABdAAAADwAAAExheW91dEZpbGVOYW1lAFwAAAALAAAATGF5b3V0TmFtZQCHAAAADAAAAExpbWl0VG9MaXN0AGMAAAAKAAAATGlzdEZpZWxkAMUAAAAHAAAATG9ja2VkALwAAAASAAAATWF0Y2hFbnRyeVRpbWVvdXQAowAAAAoAAABNb3VzZUljb24ApAAAAA0AAABNb3VzZVBvaW50ZXIAYQAAAA4AAABNdWx0aXBsZUxpbmVzAJ8AAAAMAAAAT0xFRHJhZ01vZGUAoAAAAAwAAABPTEVEcm9wTW9kZQAPAAAAEAAAAFJvd0RpdmlkZXJTdHlsZQCcAAAACgAAAFJvd01lbWJlcgCDAAAADAAAAFJvd1RyYWNraW5nACMAAAAHAAAAU3BsaXRzAAMCAAAQAAAAX0Ryb3Bkb3duSGVpZ2h0AAICAAAPAAAAX0Ryb3Bkb3duV2lkdGgA0wcAAAkAAABfRXh0ZW50WADUBwAACQAAAF9FeHRlbnRZAAACAAAMAAAAX0xheW91dFR5cGUAEAAAAAsAAABfUm93SGVpZ2h0AAECAAALAAAAX1N0eWxlRGVmcwAEAgAAFgAAAF9XYXNQZXJzaXN0ZWRBc1BpeGVscwA= 
height=28 id=TrueDBCombo_SAHLBranch 
style="BACKGROUND-IMAGE: url(images/sq_blue1_ltd.jpg); FONT-WEIGHT: bold; HEIGHT: 125px; LEFT: 567px; POSITION: absolute; TOP: 86px; WIDTH: 245px; Z-INDEX: -98" 
tabIndex=5 width=245></OBJECT>
<table border="0" cellPadding="1" cellSpacing="1" width="75%" style="FONT-FAMILY: fantasy" id="tbl_Financials">
  
  <tr>
    <td noWrap align="right" style="COLOR: white">SPV Name</td>
    <td noWrap></td>
    <td noWrap align="right" style="COLOR: white">&nbsp;&nbsp;&nbsp;&nbsp; Debit Order Amount</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 
      id=ProspectDebitOrderAmt 
      style="FONT-WEIGHT: bold; HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 186px" 
      tabIndex=13><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="4921"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0.00"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
  <tr>
    <td noWrap align="right" style="COLOR: white">Link Rate</td>
    <td noWrap></td>
    <td noWrap align="right" style="COLOR: white">Debit Order Day</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 
      id=ProspectDebitOrderDay 
      style="FONT-WEIGHT: bold; HEIGHT: 26px; LEFT: 1px; TOP: 1px; VISIBILITY: hidden; WIDTH: 52px" 
      tabIndex=13><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="1376"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0;;Null"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2012938241"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
  <tr>
    <td noWrap align="right" style="COLOR: white">Market Rate</td>
    <td noWrap></td>
    <td noWrap align="right" style="COLOR: white">Salary Number</td>
    <td noWrap>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id=ProspectSalaryNumber 
	style="FONT-WEIGHT: bold; HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 186px" tabIndex=15>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4921">
	<PARAM NAME="_ExtentY" VALUE="661">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td noWrap align="right" style="COLOR: white">ACB Bank</td>
    <td noWrap></td>
    <td noWrap align="right" style="COLOR: white">Pay Point</td>
    <td noWrap>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id=ProspectPaypoint style="FONT-WEIGHT: bold; HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 186px" 
	tabIndex=16>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4921">
	<PARAM NAME="_ExtentY" VALUE="661">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td noWrap align="right" style="COLOR: white">ACB Branch</td>
    <td noWrap></td>
    <td noWrap align="right" style="COLOR: white">Notch</td>
    <td noWrap>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" id=ProspectPayNotch style="FONT-WEIGHT: bold; HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 97px" 
	tabIndex=17>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="2566">
	<PARAM NAME="_ExtentY" VALUE="688">
	<PARAM NAME="AlignHorizontal" VALUE="1">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="0">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="ClearAction" VALUE="0">
	<PARAM NAME="DecimalPoint" VALUE=".">
	<PARAM NAME="DisplayFormat" VALUE="####0">
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
	<PARAM NAME="ValueVT" VALUE="2011758597">
	<PARAM NAME="Value" VALUE="0">
	<PARAM NAME="MaxValueVT" VALUE="5">
	<PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
  <tr>
    <td noWrap align="right" style="COLOR: white">ACB Account Number</td>
    <td noWrap>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id=ProspectACBAccountNumber 
	style="FONT-WEIGHT: bold; HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 244px" tabIndex=11>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="6456">
	<PARAM NAME="_ExtentY" VALUE="661">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="-1">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td>
    <td noWrap align="right" style="COLOR: white">Rank</td>
    <td noWrap>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id=ProspectRank style="FONT-WEIGHT: bold; HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 186px" 
	tabIndex=18>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4921">
	<PARAM NAME="_ExtentY" VALUE="661">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr></TR>
  <tr>
    <td noWrap align="right" style="COLOR: white">ACB Account Type</td>
    <td noWrap>
      
</td>
    <td noWrap align="right" style="COLOR: white"></td>
    <td noWrap>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id=Dummy style="FONT-WEIGHT: bold; HEIGHT: 25px; LEFT: 1px; TOP: 1px; VISIBILITY: hidden; WIDTH: 186px" 
	tabIndex=19>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4921">
	<PARAM NAME="_ExtentY" VALUE="661">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
      
</td></tr>

 
</table>
<OBJECT classid="clsid:0D623583-DBA2-11D1-B5DF-0060976089D0" data=data:application/x-oleobject;base64,gzViDaLb0RG13wBgl2CJ0P7/AAAFAAIAgzViDaLb0RG13wBgl2CJ0AEAAAAhCI/7ZAEbEITtCAArLscTQAAAAB0fAAAyAAAA0wcAAJgBAADUBwAAoAEAAAACAACoAQAAEAAAALABAAAEAgAAuAEAAAMCAADAAQAAAgIAAMgBAAAIAAAA0AEAACMAAAD0BAAA+P3//xALAAAI/v//GAsAAIEAAAAgCwAAhQAAACgLAACHAAAAMAsAAAcAAAA4CwAAjwAAAEALAAAlAAAASAsAAAoAAABQCwAA/v3//1gLAAAMAAAAYAsAAJEAAABoCwAADwAAAHALAAD6/f//eAsAAIgAAACECwAAAQIAAKwLAABcAAAALBoAAF0AAAA4GgAAYQAAAEQaAABfAAAATBoAAGAAAABUGgAAYwAAAFwaAABzAAAAdBoAAGUAAACIGgAAfQAAAJAaAAB+AAAAmBoAAIIAAACgGgAAgwAAAKgaAACcAAAAsBoAAKMAAAC8GgAApAAAAMQaAAC8AAAAzBoAAJ8AAADUGgAAoAAAANwaAAC9AAAA5BoAAL4AAADsGgAAvwAAAPQaAADAAAAA/BoAAMEAAAAEGwAAxQAAAAwbAAAAAAAAFBsAAAMAAABtGQAAAwAAAKMTAAACAAAABAAAAAMAAAABAACAAgAAAAAAAAADAAAA8xAAAAMAAAAAAAAASxAAAAIAAACGAQAA/v8AAAUAAgBxNWINotvREbXfAGCXYInQAQAAACEIj/tkARsQhO0IACsuxxNMAgAAVgEAAAkAAAACAgAAUAAAAAQCAABYAAAAGAAAAGAAAAAFAAAAZAAAADoAAAB4AAAACAAAAIQAAAARAAAAmAAAAE4AAACkAAAAAAAAAKwAAAADAAAAAAAAAAIAAAAFAAAAAAAAAB4AAAAKAAAAU1BWTnVtYmVyAAAAHgAAAAEAAAAAAAAAHgAAAAoAAABTUFZOdW1iZXIAAAAeAAAAAQAAAAAAAAALAAAAAAAAAAkAAAAAAAAACAAAAENvbHVtbjAABQAAAAgAAABDYXB0aW9uAAgAAAAKAAAARGF0YUZpZWxkADoAAAALAAAARm9vdGVyVGV4dABOAAAABgAAAEdyb3VwABEAAAANAAAATnVtYmVyRm9ybWF0ABgAAAALAAAAVmFsdWVJdGVtcwAEAgAADwAAAF9NYXhDb21ib0l0ZW1zAAICAAAMAAAAX1ZsaXN0U3R5bGUAjgEAAP7/AAAFAAIAcTViDaLb0RG13wBgl2CJ0AEAAAAhCI/7ZAEbEITtCAArLscT1gMAAF4BAAAJAAAAAgIAAFAAAAAEAgAAWAAAABgAAABgAAAABQAAAGQAAAA6AAAAfAAAAAgAAACIAAAAEQAAAKAAAABOAAAArAAAAAAAAAC0AAAAAwAAAAAAAAACAAAABQAAAAAAAAAeAAAADwAAAFNQVkRlc2NyaXB0aW9uAAAeAAAAAQAAAAAAAAAeAAAADwAAAFNQVkRlc2NyaXB0aW9uAAAeAAAAAQAAAAAAAAALAAAAAAAAAAkAAAAAAAAACAAAAENvbHVtbjEABQAAAAgAAABDYXB0aW9uAAgAAAAKAAAARGF0YUZpZWxkADoAAAALAAAARm9vdGVyVGV4dABOAAAABgAAAEdyb3VwABEAAAANAAAATnVtYmVyRm9ybWF0ABgAAAALAAAAVmFsdWVJdGVtcwAEAgAADwAAAF9NYXhDb21ib0l0ZW1zAAICAAAMAAAAX1ZsaXN0U3R5bGUASxAAAAEAAAANBgAA/v8AAAUAAgBzNWINotvREbXfAGCXYInQAQAAACEIj/tkARsQhO0IACsuxxNwBQAA3QUAABIAAAAGAgAAmAAAACAAAACgAAAAOgAAAKgAAAA7AAAAsAAAAAMAAAC4AAAABAAAAMAAAAAHAAAAyAAAAAYAAADQAAAADwAAANgAAAARAAAA4AAAAAMCAADoAAAAKQAAACwEAAAqAAAANAQAAC8AAAA8BAAAMgAAAEQEAAAzAAAATAQAADUAAABYBAAAAAAAAGAEAAADAAAAAAAAAAsAAAD//wAACwAAAP//AAALAAAAAAAAAAIAAAABAAAACwAAAAAAAAADAAAAAAAAAAIAAAABAAAACwAAAAAAAAADAAAABAAAAEEAAABAAwAAQmlnUmVkAQICAAAAAQAAABkAAAAEAAAAGQUAANEMAAAAAAAABAAAAAEFAAABAAAAAHs+AwQAAACiBQAAZwwAAAB4PgMEAAAA/wQAAICAgAAAAAAABAAAAO4EAAAAAAAAAAAAAAQAAAAHBQAAAAAAAAAAAAAEAAAAJQQAAAQAAAAA/xsABAAAACsEAAAAAAAAAAAAAAQAAADUBAAAAAAAAAAAAAAEAAAAyAQAAAAAAAAA////BAAAAIQEAAAAAAAAAJYfAAQAAACUBQAAAQAAAAB1PgMEAAAAIwQAAAEAAAAAlh8ABAAAAMgFAAAAAAAAAJYfAAQAAADCBQAAAAAAAACWHwAEAAAA5gUAAAAAAAAAlh8ABAAAAOoFAAAAAAAAAJYfAAQAAAD5BQAAAQAAAACWHwAEAAAAywUAAAAAAAAAlh8ABAAAAJIFAAAAAAAAAJYfAAQAAACyBQAAAAAAAACWHwAEAAAAvgUAAAAAAAAAlh8ABAAAAPsFAAAAAAAAAHQ+AwQAAADzBQAAAQAAAAB3PgMEAAAA9QUAAAEAAAAAdj4DAgAAABkAAAAEAAAAGQUAANEMAAAAAAAABAAAAAEFAAABAAAAAAAAAAQAAACiBQAAZwwAAAAAAAAEAAAA/wQAAICAgAAAAAAABAAAAO4EAAAAAAAAAAAAAAQAAAAHBQAAAAAAAAAAAAAEAAAAJQQAAAQAAAAAAAAABAAAACsEAAABAAAAAHE+AwQAAADUBAAAAAAAAAByPgMEAAAAyAQAAAAAAAAAdT4DBAAAAIQEAAAAAAAAAHQ+AwQAAACUBQAAAQAAAAByPgMEAAAAIwQAAAIAAAAADgAABAAAAMgFAAAAAAAAAA4AAAQAAADCBQAAAAAAAAAPAAAEAAAA5gUAAAAAAAAADwAABAAAAOoFAAAAAAAAAA8AAAQAAAD5BQAAAQAAAAAAAAAEAAAAywUAAAAAAAAAAAAABAAAAJIFAAAAAAAAAAAAAAQAAACyBQAAAAAAAAAAAAAEAAAAvgUAAAAAAAAAAAAABAAAAPsFAAAAAAAAAHA+AwQAAADzBQAAAQAAAABzPgMEAAAA9QUAAAEAAAAAcj4DCwAAAP//AAALAAAAAAAAAAsAAAAAAAAACwAAAP//AAAeAAAAAQAAAAAAAAADAAAAAAAAABIAAAAAAAAABwAAAFNwbGl0MAAqAAAADQAAAEFsbG93Q29sTW92ZQApAAAADwAAAEFsbG93Q29sU2VsZWN0AA8AAAAPAAAAQWxsb3dSb3dTaXppbmcABAAAAAwAAABBbGxvd1NpemluZwAyAAAAFAAAAEFsdGVybmF0aW5nUm93U3R5bGUAOwAAABIAAABBbmNob3JSaWdodENvbHVtbgAzAAAACAAAAENhcHRpb24ANQAAAA0AAABEaXZpZGVyU3R5bGUAIAAAABIAAABFeHRlbmRSaWdodENvbHVtbgAvAAAADgAAAEZldGNoUm93U3R5bGUAOgAAABMAAABQYXJ0aWFsUmlnaHRDb2x1bW4AEQAAAAsAAABTY3JvbGxCYXJzAAMAAAAMAAAAU2Nyb2xsR3JvdXAABgAAAAUAAABTaXplAAcAAAAJAAAAU2l6ZU1vZGUAAwIAAA0AAABfQ29sdW1uUHJvcHMABgIAAAsAAABfVXNlckZsYWdzAAAAAAMAAAABAAAAAwAAAAEAAAADAAAAAgAAAAsAAAAAAAAACwAAAAAAAAALAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAAEAAAAAAAAAAsAAAD//wAABAAAAAAAgD8EAAAAAACAPwMAAAAAAAAAHgAAAAEAAAAAAAAARgAAACAAAAADUuMLkY/OEZ3jAKoAS7hRAQAAALwC3HwBAAVBcmlhbEEAAAB8DgAAVVN0eWxlAQUAAAAAJQAAAAAAAAD//////wkA/wAAAAAEAAAABQAAgAgAAICwBAAAVGltZXMgTmV3IFJvbWFuAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAABAAAAAAAAAD8IAP8BAAAABAAAAAUAAIAIAACAzwMAAEFyaWFsAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAAgAAAAEAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAMAAAABAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAEAAAAAgAAAAAAAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABQAAAAIAAADAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAYAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAHAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB0AAABIZWFkaW5nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB4AAABGb290aW5nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB8AAABTZWxlY3RlZAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAAABDYXB0aW9uAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACEAAABIaWdobGlnaHRSb3cAAAAAAAAAAAAAAAAAAAAAAAAAACIAAABFdmVuUm93AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACMAAABPZGRSb3cAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACQAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAsAAAD//wAAAwAAAAAAAAAeAAAADwAAAFNQVkRlc2NyaXB0aW9uAAAeAAAACgAAAFNQVk51bWJlcgAAAAsAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAAAAAAACwAAAP//AAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAANAHAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAMgAAAAAAAAAQAAAAVHJ1ZURCQ29tYm9fU1BWAL0AAAAOAAAAQW5pbWF0ZVdpbmRvdwDAAAAAEwAAAEFuaW1hdGVXaW5kb3dDbG9zZQC+AAAAFwAAAEFuaW1hdGVXaW5kb3dEaXJlY3Rpb24AvwAAABIAAABBbmltYXRlV2luZG93VGltZQD4/f//CwAAAEFwcGVhcmFuY2UAhQAAAA8AAABBdXRvQ29tcGxldGlvbgCCAAAADQAAAEF1dG9Ecm9wZG93bgAI/v//DAAAAEJvcmRlclN0eWxlAHMAAAAMAAAAQm91bmRDb2x1bW4A+v3//wgAAABDYXB0aW9uAGAAAAAJAAAAQ2VsbFRpcHMAfgAAAA4AAABDZWxsVGlwc0RlbGF5AH0AAAAOAAAAQ2VsbFRpcHNXaWR0aACPAAAADgAAAENvbHVtbkZvb3RlcnMABwAAAA4AAABDb2x1bW5IZWFkZXJzAAgAAAAIAAAAQ29sdW1ucwCBAAAACwAAAENvbWJvU3R5bGUAJQAAAAkAAABEYXRhTW9kZQAKAAAADAAAAERlZkNvbFdpZHRoAMEAAAARAAAARHJvcGRvd25Qb3NpdGlvbgCIAAAACQAAAEVkaXRGb250AF8AAAAKAAAARW1wdHlSb3dzAP79//8IAAAARW5hYmxlZACRAAAACgAAAEZvb3RMaW5lcwAMAAAACgAAAEhlYWRMaW5lcwBlAAAADwAAAEludGVncmFsSGVpZ2h0AF0AAAAPAAAATGF5b3V0RmlsZU5hbWUAXAAAAAsAAABMYXlvdXROYW1lAIcAAAAMAAAATGltaXRUb0xpc3QAYwAAAAoAAABMaXN0RmllbGQAxQAAAAcAAABMb2NrZWQAvAAAABIAAABNYXRjaEVudHJ5VGltZW91dACjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgBhAAAADgAAAE11bHRpcGxlTGluZXMAnwAAAAwAAABPTEVEcmFnTW9kZQCgAAAADAAAAE9MRURyb3BNb2RlAA8AAAAQAAAAUm93RGl2aWRlclN0eWxlAJwAAAAKAAAAUm93TWVtYmVyAIMAAAAMAAAAUm93VHJhY2tpbmcAIwAAAAcAAABTcGxpdHMAAwIAABAAAABfRHJvcGRvd25IZWlnaHQAAgIAAA8AAABfRHJvcGRvd25XaWR0aADTBwAACQAAAF9FeHRlbnRYANQHAAAJAAAAX0V4dGVudFkAAAIAAAwAAABfTGF5b3V0VHlwZQAQAAAACwAAAF9Sb3dIZWlnaHQAAQIAAAsAAABfU3R5bGVEZWZzAAQCAAAWAAAAX1dhc1BlcnNpc3RlZEFzUGl4ZWxzAA== 
	id=TrueDBCombo_SPV style="BACKGROUND-IMAGE: url(images/sq_blue1_ltd.jpg); FONT-WEIGHT: bold; HEIGHT: 190px; LEFT: 158px; TOP: 134px; WIDTH: 246px; Z-INDEX: -94; POSITION: ABSOLUTE;" 
	tabIndex=6 width=246></OBJECT>
<OBJECT classid="clsid:0D623583-DBA2-11D1-B5DF-0060976089D0" data=data:application/x-oleobject;base64,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AAAAAP//////////AAAAAAcAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAIAAAAAQAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACQAAAAEAAAAAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAoAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAALAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADAAAAAIAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA0AAAADAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAOAAAABQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADwAAAAcAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABAAAAAGAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAARAAAACAAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEgAAAAkAAAAAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABMAAAAKAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAUAAAABAAAAAAAAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABYAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAXAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABkAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAaAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABwAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHgAAAB0AAADAAgEAAAIAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB8AAAAdAAAAwAABAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAgAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIQAAAB4AAAAAAQAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACIAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAjAAAAHQAAAIAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAJAAAAB0AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAAAAAA//////////8BAAAAAAAAAAAAAAABAAAA/v///wQAAAAAAAAAAAAAAAUAAAD9////AgAAAAAAAAAAAAAAAgAAAOr///8DAAAAAAAAAAAAAAADAAAA/P///wUAAAAAAAAAAAAAAP/////7////BgAAAAAAAAAAAAAABAAAAPr///8HAAAAAAAAAAAAAAD/////8f///wgAAAAAAAAAAAAAAAYAAADv////CQAAAAAAAAAAAAAABwAAAO7///8KAAAAAAAAAAAAAAAIAAAA+f///wsAAAABAAAAAAAAAP/////r////FAAAAAEAAAAAAAAA//////j///8MAAAAAQAAAAAAAAD/////6f///w0AAAABAAAAAAAAAP/////3////DgAAAAEAAAAAAAAA//////b///8QAAAAAQAAAAAAAAD/////9f///w8AAAABAAAAAAAAAP/////w////EQAAAAEAAAAAAAAA/////+3///8SAAAAAQAAAAAAAAD/////7P///xMAAAABAAAAAAAAAP/////0////GAAAAAEAAAABAAAA//////P///8VAAAAAQAAAAEAAAD/////6P///xYAAAABAAAAAQAAAP/////y////FwAAAAEAAAABAAAA//////T///8cAAAAAQAAAAIAAAD/////8////xkAAAABAAAAAgAAAP/////o////GgAAAAEAAAACAAAA//////L///8bAAAAAQAAAAIAAAD/////CAAAAE5vcm1hbAADAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHQAAAEhlYWRpbmcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHgAAAEZvb3RpbmcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHwAAAFNlbGVjdGVkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIAAAAENhcHRpb24AMQAAACAAAAAwJzsDHOQ9Aw4AAIANAACAIQAAAEhpZ2hsaWdodFJvdwAAAAD2////AAAAADEAAAAQAAAAIgAAAEV2ZW5Sb3cAAAAAAAAAAAAdAAAAAAAAADEAAABAAAAAIwAAAE9kZFJvdwD/YAAAAAAAAABMYXlvdXROYTEAAABwAgAAJAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAP//AAADAAAAAAAAAB4AAAAUAAAATGlua1JhdGVEZXNjcmlwdGlvbgAeAAAACQAAAExpbmtSYXRlAAAAAAsAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAAAAAAACwAAAP//AAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAANAHAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAMgAAAAAAAAAVAAAAVHJ1ZURCQ29tYm9fTGlua1JhdGUAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQCFAAAADwAAAEF1dG9Db21wbGV0aW9uAIIAAAANAAAAQXV0b0Ryb3Bkb3duAAj+//8MAAAAQm9yZGVyU3R5bGUAcwAAAAwAAABCb3VuZENvbHVtbgD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAIEAAAALAAAAQ29tYm9TdHlsZQAlAAAACQAAAERhdGFNb2RlAAoAAAAMAAAARGVmQ29sV2lkdGgAwQAAABEAAABEcm9wZG93blBvc2l0aW9uAIgAAAAJAAAARWRpdEZvbnQAXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkAJEAAAAKAAAARm9vdExpbmVzAAwAAAAKAAAASGVhZExpbmVzAGUAAAAPAAAASW50ZWdyYWxIZWlnaHQAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAhwAAAAwAAABMaW1pdFRvTGlzdABjAAAACgAAAExpc3RGaWVsZADFAAAABwAAAExvY2tlZAC8AAAAEgAAAE1hdGNoRW50cnlUaW1lb3V0AKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAnAAAAAoAAABSb3dNZW1iZXIAgwAAAAwAAABSb3dUcmFja2luZwAjAAAABwAAAFNwbGl0cwADAgAAEAAAAF9Ecm9wZG93bkhlaWdodAACAgAADwAAAF9Ecm9wZG93bldpZHRoANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA 
	id=TrueDBCombo_LinkRate style="BACKGROUND-IMAGE: url(images/sq_blue1_ltd.jpg); FONT-WEIGHT: bold; HEIGHT: 123px; LEFT: 158px; TOP: 162px; WIDTH: 246px; Z-INDEX: -92; POSITION: ABSOLUTE;" 
	tabIndex=7 width=246></OBJECT>
<OBJECT classid="clsid:0D623583-DBA2-11D1-B5DF-0060976089D0" data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAgAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAJAAAAAQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACgAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAsAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAMAAAAAgAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADQAAAAMAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA4AAAAFAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAPAAAABwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEAAAAAYAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABEAAAAIAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAASAAAACQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEwAAAAoAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABQAAAAEAAAAAAAAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAVAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABcAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAYAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABoAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAbAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAeAAAAHQAAAMACAQAAAgAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHwAAAB0AAADAAAEAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACAAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAhAAAAHgAAAAABAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIgAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACMAAAAdAAAAgAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAkAAAAHQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAAAAAD//////////wEAAAAAAAAAAAAAAAEAAAD+////BAAAAAAAAAAAAAAABQAAAP3///8CAAAAAAAAAAAAAAACAAAA6v///wMAAAAAAAAAAAAAAAMAAAD8////BQAAAAAAAAAAAAAA//////v///8GAAAAAAAAAAAAAAAEAAAA+v///wcAAAAAAAAAAAAAAP/////x////CAAAAAAAAAAAAAAABgAAAO////8JAAAAAAAAAAAAAAAHAAAA7v///woAAAAAAAAAAAAAAAgAAAD5////CwAAAAEAAAAAAAAA/////+v///8UAAAAAQAAAAAAAAD/////+P///wwAAAABAAAAAAAAAP/////p////DQAAAAEAAAAAAAAA//////f///8OAAAAAQAAAAAAAAD/////9v///xAAAAABAAAAAAAAAP/////1////DwAAAAEAAAAAAAAA//////D///8RAAAAAQAAAAAAAAD/////7f///xIAAAABAAAAAAAAAP/////s////EwAAAAEAAAAAAAAA//////T///8YAAAAAQAAAAEAAAD/////8////xUAAAABAAAAAQAAAP/////o////FgAAAAEAAAABAAAA//////L///8XAAAAAQAAAAEAAAD/////9P///xwAAAABAAAAAgAAAP/////z////GQAAAAEAAAACAAAA/////+j///8aAAAAAQAAAAIAAAD/////8v///xsAAAABAAAAAgAAAP////8IAAAATm9ybWFsAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAdAAAASGVhZGluZwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAeAAAARm9vdGluZwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAfAAAAU2VsZWN0ZWQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAQ2FwdGlvbgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAhAAAASGlnaGxpZ2h0Um93AAAAAAAAAAAAAAAAAAAAAAAAAAAiAAAARXZlblJvdwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAjAAAAT2RkUm93AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAkAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAALAAAA//8AAAMAAAAAAAAAHgAAABoAAABNYXJrZXRSYXRlVHlwZURlc2NyaXB0aW9uAAAAHgAAABUAAABNYXJrZXRSYXRlVHlwZU51bWJlcgAAAAALAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAAAAAAAAsAAAD//wAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADQBwAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAAAAAAALAAAAAAAAADIAAAAAAAAAGwAAAFRydWVEQkNvbWJvX01hcmtldFJhdGVUeXBlAL0AAAAOAAAAQW5pbWF0ZVdpbmRvdwDAAAAAEwAAAEFuaW1hdGVXaW5kb3dDbG9zZQC+AAAAFwAAAEFuaW1hdGVXaW5kb3dEaXJlY3Rpb24AvwAAABIAAABBbmltYXRlV2luZG93VGltZQD4/f//CwAAAEFwcGVhcmFuY2UAhQAAAA8AAABBdXRvQ29tcGxldGlvbgCCAAAADQAAAEF1dG9Ecm9wZG93bgAI/v//DAAAAEJvcmRlclN0eWxlAHMAAAAMAAAAQm91bmRDb2x1bW4A+v3//wgAAABDYXB0aW9uAGAAAAAJAAAAQ2VsbFRpcHMAfgAAAA4AAABDZWxsVGlwc0RlbGF5AH0AAAAOAAAAQ2VsbFRpcHNXaWR0aACPAAAADgAAAENvbHVtbkZvb3RlcnMABwAAAA4AAABDb2x1bW5IZWFkZXJzAAgAAAAIAAAAQ29sdW1ucwCBAAAACwAAAENvbWJvU3R5bGUAJQAAAAkAAABEYXRhTW9kZQAKAAAADAAAAERlZkNvbFdpZHRoAMEAAAARAAAARHJvcGRvd25Qb3NpdGlvbgCIAAAACQAAAEVkaXRGb250AF8AAAAKAAAARW1wdHlSb3dzAP79//8IAAAARW5hYmxlZACRAAAACgAAAEZvb3RMaW5lcwAMAAAACgAAAEhlYWRMaW5lcwBlAAAADwAAAEludGVncmFsSGVpZ2h0AF0AAAAPAAAATGF5b3V0RmlsZU5hbWUAXAAAAAsAAABMYXlvdXROYW1lAIcAAAAMAAAATGltaXRUb0xpc3QAYwAAAAoAAABMaXN0RmllbGQAxQAAAAcAAABMb2NrZWQAvAAAABIAAABNYXRjaEVudHJ5VGltZW91dACjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgBhAAAADgAAAE11bHRpcGxlTGluZXMAnwAAAAwAAABPTEVEcmFnTW9kZQCgAAAADAAAAE9MRURyb3BNb2RlAA8AAAAQAAAAUm93RGl2aWRlclN0eWxlAJwAAAAKAAAAUm93TWVtYmVyAIMAAAAMAAAAUm93VHJhY2tpbmcAIwAAAAcAAABTcGxpdHMAAwIAABAAAABfRHJvcGRvd25IZWlnaHQAAgIAAA8AAABfRHJvcGRvd25XaWR0aADTBwAACQAAAF9FeHRlbnRYANQHAAAJAAAAX0V4dGVudFkAAAIAAAwAAABfTGF5b3V0VHlwZQAQAAAACwAAAF9Sb3dIZWlnaHQAAQIAAAsAAABfU3R5bGVEZWZzAAQCAAAWAAAAX1dhc1BlcnNpc3RlZEFzUGl4ZWxzAA== 
	id=TrueDBCombo_MarketRateType style="BACKGROUND-IMAGE: url(images/sq_blue1_ltd.jpg); FONT-WEIGHT: bold; HEIGHT: 88px; LEFT: 158px; TOP: 191px; WIDTH: 246px; Z-INDEX: -93; POSITION: ABSOLUTE;" 
	tabIndex=8 width=246></OBJECT>
<br>
<OBJECT classid="clsid:0D623583-DBA2-11D1-B5DF-0060976089D0" data=data:application/x-oleobject;base64,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BwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAgAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAJAAAAAQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACgAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAsAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAMAAAAAgAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADQAAAAMAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA4AAAAFAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAPAAAABwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEAAAAAYAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABEAAAAIAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAASAAAACQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEwAAAAoAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABQAAAAEAAAAAAAAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAVAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABcAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAYAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABoAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAbAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAeAAAAHQAAAMACAQAAAgAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHwAAAB0AAADAAAEAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACAAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAhAAAAHgAAAAABAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIgAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACMAAAAdAAAAgAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAkAAAAHQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAAAAAD//////////wEAAAAAAAAAAAAAAAEAAAD+////BAAAAAAAAAAAAAAABQAAAP3///8CAAAAAAAAAAAAAAACAAAA6v///wMAAAAAAAAAAAAAAAMAAAD8////BQAAAAAAAAAAAAAA//////v///8GAAAAAAAAAAAAAAAEAAAA+v///wcAAAAAAAAAAAAAAP/////x////CAAAAAAAAAAAAAAABgAAAO////8JAAAAAAAAAAAAAAAHAAAA7v///woAAAAAAAAAAAAAAAgAAAD5////CwAAAAEAAAAAAAAA/////+v///8UAAAAAQAAAAAAAAD/////+P///wwAAAABAAAAAAAAAP/////p////DQAAAAEAAAAAAAAA//////f///8OAAAAAQAAAAAAAAD/////9v///xAAAAABAAAAAAAAAP/////1////DwAAAAEAAAAAAAAA//////D///8RAAAAAQAAAAAAAAD/////7f///xIAAAABAAAAAAAAAP/////s////EwAAAAEAAAAAAAAA//////T///8YAAAAAQAAAAEAAAD/////8////xUAAAABAAAAAQAAAP/////o////FgAAAAEAAAABAAAA//////L///8XAAAAAQAAAAEAAAD/////9P///xwAAAABAAAAAgAAAP/////z////GQAAAAEAAAACAAAA/////+j///8aAAAAAQAAAAIAAAD/////8v///xsAAAABAAAAAgAAAP////8IAAAATm9ybWFsAAN0Um93ACw+A4AsPgMQLD4DoCs+AzArPgMdAAAASGVhZGluZwBQPD4D4Ds+A3A7PgOgOj4DIDk+A4A4PgMeAAAARm9vdGluZwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAfAAAAU2VsZWN0ZWQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAQ2FwdGlvbgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAhAAAASGlnaGxpZ2h0Um93AAAAAAAAAAAAAAAAAAAAAAAAAAAiAAAARXZlblJvdwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAjAAAAT2RkUm93AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAkAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAALAAAA//8AAAMAAAAAAAAAHgAAAAkAAABET0RldGFpbAAAAAAeAAAACQAAAERPTnVtYmVyAAAAAAsAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAAAAAAACwAAAP//AAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAANAHAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAMgAAAAAAAAAaAAAAVHJ1ZURCQ29tYm9fRGViaXRPcmRlckRheQC9AAAADgAAAEFuaW1hdGVXaW5kb3cAwAAAABMAAABBbmltYXRlV2luZG93Q2xvc2UAvgAAABcAAABBbmltYXRlV2luZG93RGlyZWN0aW9uAL8AAAASAAAAQW5pbWF0ZVdpbmRvd1RpbWUA+P3//wsAAABBcHBlYXJhbmNlAIUAAAAPAAAAQXV0b0NvbXBsZXRpb24AggAAAA0AAABBdXRvRHJvcGRvd24ACP7//wwAAABCb3JkZXJTdHlsZQBzAAAADAAAAEJvdW5kQ29sdW1uAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAgQAAAAsAAABDb21ib1N0eWxlACUAAAAJAAAARGF0YU1vZGUACgAAAAwAAABEZWZDb2xXaWR0aADBAAAAEQAAAERyb3Bkb3duUG9zaXRpb24AiAAAAAkAAABFZGl0Rm9udABfAAAACgAAAEVtcHR5Um93cwD+/f//CAAAAEVuYWJsZWQAkQAAAAoAAABGb290TGluZXMADAAAAAoAAABIZWFkTGluZXMAZQAAAA8AAABJbnRlZ3JhbEhlaWdodABdAAAADwAAAExheW91dEZpbGVOYW1lAFwAAAALAAAATGF5b3V0TmFtZQCHAAAADAAAAExpbWl0VG9MaXN0AGMAAAAKAAAATGlzdEZpZWxkAMUAAAAHAAAATG9ja2VkALwAAAASAAAATWF0Y2hFbnRyeVRpbWVvdXQAowAAAAoAAABNb3VzZUljb24ApAAAAA0AAABNb3VzZVBvaW50ZXIAYQAAAA4AAABNdWx0aXBsZUxpbmVzAJ8AAAAMAAAAT0xFRHJhZ01vZGUAoAAAAAwAAABPTEVEcm9wTW9kZQAPAAAAEAAAAFJvd0RpdmlkZXJTdHlsZQCcAAAACgAAAFJvd01lbWJlcgCDAAAADAAAAFJvd1RyYWNraW5nACMAAAAHAAAAU3BsaXRzAAMCAAAQAAAAX0Ryb3Bkb3duSGVpZ2h0AAICAAAPAAAAX0Ryb3Bkb3duV2lkdGgA0wcAAAkAAABfRXh0ZW50WADUBwAACQAAAF9FeHRlbnRZAAACAAAMAAAAX0xheW91dFR5cGUAEAAAAAsAAABfUm93SGVpZ2h0AAECAAALAAAAX1N0eWxlRGVmcwAEAgAAFgAAAF9XYXNQZXJzaXN0ZWRBc1BpeGVscwA= 
	id=TrueDBCombo_DebitOrderDay style="BACKGROUND-IMAGE: url(images/sq_blue1_ltd.jpg); FONT-WEIGHT: bold; HEIGHT: 160px; LEFT: 567px; TOP: 162px; WIDTH: 99px; Z-INDEX: 104; POSITION: ABSOLUTE;" 
	tabIndex=14></OBJECT>
<table border="0" cellPadding="1" cellSpacing="1" style="FONT-FAMILY: fantasy; HEIGHT: 88px; WIDTH: 559px" width="75%">
  
  <tr>
    <td align="right" noWrap style="COLOR: white">&nbsp; Property Description</td>
    <td noWrap>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id=ProspectPropertyDesc1 
	style="FONT-WEIGHT: bold; HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 392px" tabIndex=20>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="10371">
	<PARAM NAME="_ExtentY" VALUE="688">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="-1">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td noWrap></td>
    <td noWrap>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id=ProspectPropertyDesc2 
	style="FONT-WEIGHT: bold; HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 392px" tabIndex=21>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="10371">
	<PARAM NAME="_ExtentY" VALUE="688">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="-1">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td noWrap></td>
    <td noWrap>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" id=ProspectPropertyDesc3 
	style="FONT-WEIGHT: bold; HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 392px" tabIndex=22>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="10371">
	<PARAM NAME="_ExtentY" VALUE="688">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="Enabled" VALUE="-1">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr></table>
<OBJECT classid="clsid:0D623583-DBA2-11D1-B5DF-0060976089D0" data=data:application/x-oleobject;base64,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//////////8AAAAABwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAgAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAJAAAAAQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACgAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAsAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAMAAAAAgAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADQAAAAMAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA4AAAAFAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAPAAAABwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEAAAAAYAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABEAAAAIAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAASAAAACQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEwAAAAoAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABQAAAAEAAAAAAAAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAVAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABcAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAYAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABoAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAbAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAeAAAAHQAAAMACAQAAAgAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHwAAAB0AAADAAAEAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACAAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAhAAAAHgAAAAABAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIgAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACMAAAAdAAAAgAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAkAAAAHQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAAAAAD//////////wEAAAAAAAAAAAAAAAEAAAD+////BAAAAAAAAAAAAAAABQAAAP3///8CAAAAAAAAAAAAAAACAAAA6v///wMAAAAAAAAAAAAAAAMAAAD8////BQAAAAAAAAAAAAAA//////v///8GAAAAAAAAAAAAAAAEAAAA+v///wcAAAAAAAAAAAAAAP/////x////CAAAAAAAAAAAAAAABgAAAO////8JAAAAAAAAAAAAAAAHAAAA7v///woAAAAAAAAAAAAAAAgAAAD5////CwAAAAEAAAAAAAAA/////+v///8UAAAAAQAAAAAAAAD/////+P///wwAAAABAAAAAAAAAP/////p////DQAAAAEAAAAAAAAA//////f///8OAAAAAQAAAAAAAAD/////9v///xAAAAABAAAAAAAAAP/////1////DwAAAAEAAAAAAAAA//////D///8RAAAAAQAAAAAAAAD/////7f///xIAAAABAAAAAAAAAP/////s////EwAAAAEAAAAAAAAA//////T///8YAAAAAQAAAAEAAAD/////8////xUAAAABAAAAAQAAAP/////o////FgAAAAEAAAABAAAA//////L///8XAAAAAQAAAAEAAAD/////9P///xwAAAABAAAAAgAAAP/////z////GQAAAAEAAAACAAAA/////+j///8aAAAAAQAAAAIAAAD/////8v///xsAAAABAAAAAgAAAP////8IAAAATm9ybWFsAAOEoyEAIAAAAAgAAAAwJzsDIQAAALAAAAAdAAAASGVhZGluZwAhAAAA8AAAAJgnOwOYJzsDhHySAxEAAAAeAAAARm9vdGluZwAAAAAAQ29sdW1uMQABAAAAIQAAAEABAAAfAAAAU2VsZWN0ZWQAAQAAIQAAAAAAOwMwJzsDaOsiAF9WbGkgAAAAQ2FwdGlvbgAxAAAAMAAAABzyPQM4JzsD+P///xkAAAAhAAAASGlnaGxpZ2h0Um93AAAAAAzaPQNQJzsDZAD2////GwAiAAAARXZlblJvdwAhAAAAkAAAAGgnOwNoJzsDeHQAAPT///8jAAAAT2RkUm93AAAYAAAAAAAAAAAAAAAAAAAAMQAAALAAAAAkAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAALAAAA//8AAAMAAAAAAAAAHgAAABMAAABBQ0JUeXBlRGVzY3JpcHRpb24AAB4AAAAOAAAAQUNCVHlwZU51bWJlcgAAAAsAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAAAAAAACwAAAP//AAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAANAHAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAMgAAAAAAAAAYAAAAVHJ1ZURCQ29tYm9fQWNjb3VudFR5cGUAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQCFAAAADwAAAEF1dG9Db21wbGV0aW9uAIIAAAANAAAAQXV0b0Ryb3Bkb3duAAj+//8MAAAAQm9yZGVyU3R5bGUAcwAAAAwAAABCb3VuZENvbHVtbgD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAIEAAAALAAAAQ29tYm9TdHlsZQAlAAAACQAAAERhdGFNb2RlAAoAAAAMAAAARGVmQ29sV2lkdGgAwQAAABEAAABEcm9wZG93blBvc2l0aW9uAIgAAAAJAAAARWRpdEZvbnQAXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkAJEAAAAKAAAARm9vdExpbmVzAAwAAAAKAAAASGVhZExpbmVzAGUAAAAPAAAASW50ZWdyYWxIZWlnaHQAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAhwAAAAwAAABMaW1pdFRvTGlzdABjAAAACgAAAExpc3RGaWVsZADFAAAABwAAAExvY2tlZAC8AAAAEgAAAE1hdGNoRW50cnlUaW1lb3V0AKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAnAAAAAoAAABSb3dNZW1iZXIAgwAAAAwAAABSb3dUcmFja2luZwAjAAAABwAAAFNwbGl0cwADAgAAEAAAAF9Ecm9wZG93bkhlaWdodAACAgAADwAAAF9Ecm9wZG93bldpZHRoANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA 
	id=TrueDBCombo_AccountType style="BACKGROUND-COLOR: #ff9b19; FONT-WEIGHT: bold; HEIGHT: 131px; LEFT: 158px; TOP: 305px; WIDTH: 245px; Z-INDEX: -89; POSITION: ABSOLUTE;" 
	tabIndex=12></OBJECT>
<input id="btn_CreateTakeOnRecords" name="btn_CreateTakeOnRecords" style="BACKGROUND-COLOR: #eb780f; BORDER-BOTTOM-STYLE: ridge; BORDER-LEFT-STYLE: ridge; BORDER-RIGHT-STYLE: ridge; BORDER-TOP-STYLE: ridge; COLOR: white; HEIGHT: 60px; LEFT: 521px; POSITION: absolute; TOP: 470px; WIDTH: 169px; Z-INDEX: 103" title="Create Take On Records" type="button" value="Create Take On Records" height="60"> 
<img alt ="" border="0" height="20" hspace="0" id="pic_UpdateMarketRate" name="pic_UpdateMarketRate" src="http://sahlnet/MLS-Version1/images/pic_accessdeniedblue.bmp" style           ="HEIGHT: 20px; LEFT: 40px; POSITION: absolute; TOP: 166px; WIDTH: 20px; Z-INDEX: 106" title="0" useMap="" width="20"><img alt ="" border="0" height="20" hspace="0" id="pic_UpdateLinkRate" src="http://sahlnet/MLS-Version1/images/pic_accessdeniedblue.bmp" style           ="HEIGHT: 20px; LEFT: 40px; POSITION: absolute; TOP: 194px; WIDTH: 20px; Z-INDEX: 107" title="0" useMap="" width="20"> 
<table border="1" cellPadding="1" cellSpacing="1" style="COLOR: white; FONT-FAMILY: fantasy; HEIGHT: 27px; LEFT: 2px; POSITION: absolute; TOP: 439px; VISIBILITY: hidden; 
WIDTH: 864px; Z-INDEX: 105" width="75%" id="tbl_Message">
  
  <tr>
    <td id="lbl_Message" noWrap style="BACKGROUND-COLOR: red; 
    COLOR: white; 
   FONT-FAMILY: fantasy; 
    VISIBILITY: visible" align="middle"></td></tr></table><INPUT id=submit1 name=submit1 style="LEFT: 0px; POSITION: absolute; TOP: 498px; VISIBILITY: hidden; Z-INDEX: 108" type=submit value=Submit>

</body>
</html>
