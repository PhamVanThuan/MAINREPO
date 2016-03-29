<%

    Response.Clear

  sLoanNbr =Request.Form.Item("LastLoanNbr")
  sLastLoanNbr = Request.Cookies("LastLoanNumber")
  sLastSurname  = Request.Form.Item("LastClientSurname")
 'Response.Write "last is " & sLastSurname
   
' Response.Write "cookie written -> " & sLoanNbr
 sLastLoanNbr = sLoanNbr
 Response.cookies("LastLoanNumber")=sLoanNbr
 Response.cookies("LastLoanNumber").Expires = "01/01/2010"
 


if sLastSurname <> "" then
  Response.Cookies("LastClientSurname") = sLastSurname
  Response.Cookies("LastClientSurname").Expires = "01/01/2010"
  'Response.Write "cookie written is " & sLastSurname

end if

 sClientNbr =Request.Form.Item("LastClientNbr")

   'Response.Write "cookie written -> " & sClientNbr
    Response.cookies("LastClientNbr")=sClientNbr
    Response.cookies("LastClientNbr").Expires = "01/01/2010"
    


%>
<html>

<head>
<!--#include virtual="/SAHL-MLSS/stringutils.inc"-->
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->
<!--#include virtual="/SAHL-MLSS/database.inc"-->

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>
<meta name="VI60_DefaultClientScript" Content="VBScript">

<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<script ID="clientEventHandlersVBS" LANGUAGE="vbscript">
<!--

Dim i_EmployeeType
Dim i_UserGroup

Dim i_SAHLEmployeeNumber
Dim i_EmployeeTeamNumber
Dim gi_OriginationAdmin

Dim i_ProspectNumber
Dim i_LoanNumber

Dim s_ReturnPage
Dim b_Legal
Dim b_Loading
Dim b_Loading1
Dim b_Loading2
Dim b_Loading3
Dim i_CurrentBankRate
Dim i_CurrentLoanRate
Dim i_CurrentMarketRate
Dim i_CurrentLinkRate
Dim i_InterimPeriod
Dim b_AllDataLoaded
Dim s_ProspectStage
Dim gi_LegalEntity
Dim gi_JointOwner
Dim gi_SwitchLoan
Dim gi_NewPurchase
Dim gi_Refinance
Dim gi_FurtherLoan
Dim i_Object
Dim b_FeeCalced
Dim d_FeeLastCalced


Dim b_CDone 

Dim s_Action

Dim b_Forward 
Dim b_LoanBtn

Dim s_FirstNames
Dim s_SurName
Dim rs_AdminOpen
Dim b_Title
Dim rs_commentopen

if rs_open <> true then

  
   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"
    
	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   'return
    end if
    sUserName = "<%= Session("UserID")%>"
    
    x = "=<%= Session("SQLDatabase")%>"

    if x <> "=" then
		set conn = createobject("ADODB.Connection")
		set rs_SelectedProspect1  = createobject("ADODB.Recordset")
		set rs_SelectedProspect2  = createobject("ADODB.Recordset")
		set rs_SelectedProspect3  = createobject("ADODB.Recordset")
		set rs_SelectedProspect4  = createobject("ADODB.Recordset")
		set rs_SelectedProspect5  = createobject("ADODB.Recordset")
		set rs_Sex = createobject("ADODB.Recordset")
		set rs_Legal = createobject("ADODB.Recordset")
		set rs_purpose = createobject("ADODB.Recordset")
		set rs_Title = createobject("ADODB.Recordset")
		set rs_Property = createobject("ADODB.Recordset")
		set rs_Area = createobject("ADODB.Recordset")
		set rs_Thatch = createobject("ADODB.Recordset")
		set rs_Owner = createobject("ADODB.Recordset")
		set rs_Province = createobject("ADODB.Recordset")
		set rs_Province1 = createobject("ADODB.Recordset")
		set rstFees = createobject("ADODB.Recordset")
		set rs_SPV = createobject("ADODB.Recordset")
		set rs_Team = createobject("ADODB.Recordset")
		set rs_Admin = createobject("ADODB.Recordset")
		set rs_Source = createobject("ADODB.Recordset")
		set rs_Broker= createobject("ADODB.Recordset")
		set rs_Telesales = createobject("ADODB.Recordset")
		set rs_MarketRateType = createobject("ADODB.Recordset")
		set rs_LinkRate = createobject("ADODB.Recordset")
		set rs_EmploymentType = createobject("ADODB.Recordset")

		set rstemp = createobject("ADODB.Recordset")
		
		set rscomment = createobject("ADODB.Recordset")
 
		'sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [manageprospect.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
		sDSN = GetConnectionString("[ManageProspect.asp 1]")
		conn.Open sDSN
		rs_open1 = false
		rs_open2 = false
		rs_open3 = false
		rs_open4 = false
		rs_open5 = false
		b_AllDataLoaded = false
	end if
		 
end if

Sub GetProspectBasicData()
   

    i_switch = true
   
  
    
    if rs_open1 = true  then
       rs_SelectedProspect1.Close
       rs_open1 = false
	end if

    if s_Action = "Add Further Loan" then
       sSQL = "p_GetFurtherLoanProspectBasicDetails " & i_ProspectNumber
    else
		sSQL = "p_GetProspectBasicDetails " & i_ProspectNumber 
    end if
    
    
   ' msgbox sSQL
     rs_SelectedProspect1.CursorLocation = 3
	'rs_GridProspects.Open sSQL ,conn,adOpenStatic
	'rs_SelectedProspect1.CacheSize  =10
	
    'this.style.cursor = "hand"
    
	rs_SelectedProspect1.Open sSQL,conn,adOpenStatic
	
	window.btn_TeleCentreComment.style.visibility = "hidden"
	if  CInt(rs_SelectedProspect1.Fields("ProspectSourceNumber").Value)  = 2 then
	   window.btn_TeleCentreComment.style.visibility = "visible"
	end if
	
	
	window.ProspectNumber.Text = rs_SelectedProspect1.Fields("ProspectNumber").Value

	window.ProspectFirstNames.Text = rs_SelectedProspect1.Fields("ProspectFirstNames").Value
	window.ProspectSurname.Text = rs_SelectedProspect1.Fields("ProspectSurname").Value

	window.ProspectSalutation.Text = rs_SelectedProspect1.Fields("ProspectSalutation").Value
	window.ProspectIDNumber.Text = rs_SelectedProspect1.Fields("ProspectIDNumber").Value
	window.ProspectIncome.Value = rs_SelectedProspect1.Fields("ProspectIncome").Value
	window.ProspectSpouseIncome.Value = rs_SelectedProspect1.Fields("ProspectSpouseIncome").Value
	window.ProspectSpouseFirstNames.Text = rs_SelectedProspect1.Fields("ProspectSpouseFirstNames").Value
	window.ProspectSpouseIDNumber.Text = rs_SelectedProspect1.Fields("ProspectSpouseIDNumber").Value
	
	
	sSQL = "SELECT * FROM SEX"
	rs_Sex.CursorLocation = 3
	rs_Sex.Open sSQL ,conn,adOpenStatic

	set DataCombo_Gender.RowSource = rs_Sex
	DataCombo_Gender.ListField = rs_Sex.Fields("SexDescription").name
	DataCombo_Gender.BoundColumn =  rs_Sex.Fields("SexNumber").name
	DataCombo_Gender.BoundText = rs_SelectedProspect1.Fields("SexNumber").Value
	DataCombo_Gender.Refresh
	
	
	if CInt(rs_SelectedProspect1.Fields("SexNumber").Value) = gi_LegalEntity then
	   window.lbl_firstnames.style.visibility = "hidden"
	   window.ProspectFirstNames.style.visibility = "hidden"
	   window.lbl_surname.innerText = "Legal Entity"
	   window.lbl_salutation.style.visibility = "hidden"
	   window.ProspectSalutation.style.visibility = "hidden"
     elseif Cint(rs_SelectedProspect1.Fields("SexNumber").Value) = gi_JointOwner then
       window.lbl_firstNames.style.visibility = "visible"
	   window.ProspectFirstNames.style.visibility = "visible"
	   window.lbl_surname.innerText = "Joint Owner"
	   window.lbl_salutation.style.visibility = "visible"
	   window.ProspectSalutation.style.visibility = "visible"

	 end if
	
	
	sSQL = "SELECT * FROM LEGALSTATUS"
	
	if s_Action = "Copy"  then
	    if Cint(DataCombo_Gender.BoundText) = gi_LegalEntity then 'Legal
      
			sSQL = "SELECT * FROM LEGALSTATUS WHERE LegalStatusNumber >=5 AND LegalStatusNumber <=7"
	
		elseif CInt(DataCombo_Gender.BoundText) = 1 then 'Unknown
           
    		sSQL = "SELECT * FROM LEGALSTATUS WHERE LegalStatusNumber =1"
			
		 else
            
			sSQL = "SELECT * FROM LEGALSTATUS WHERE LegalStatusNumber >=2 AND LegalStatusNumber <=4 OR LegalStatusNumber = 8 OR LegalStatusNumber = 9"
			
		end if
	end if
	rs_Legal.CursorLocation = 3
	rs_Legal.Open sSQL ,conn,adOpenStatic
	
	set DataCombo_Legal.RowSource = rs_Legal
	DataCombo_Legal.ListField = rs_Legal.Fields("LegalStatusDescription").name
	DataCombo_Legal.BoundColumn =  rs_Legal.Fields("LegalStatusNumber").name
	DataCombo_Legal.BoundText = rs_SelectedProspect1.Fields("LegalStatusNumber").Value
	DataCombo_Legal.Refresh
	
   	sSQL = "SELECT * FROM EMPLOYMENTTYPE" 
   	
	rs_EmploymentType.CursorLocation = 3
	rs_EmploymentType.Open sSQL ,conn,adOpenStatic
	
	set window.DataCombo_EmploymentType.RowSource = rs_EmploymentType
	DataCombo_EmploymentType.ListField = rs_EmploymentType.Fields("EmploymentTypeDescription").name
	DataCombo_EmploymentType.BoundColumn =  rs_EmploymentType.Fields("EmploymentTypeNumber").name
	DataCombo_EmploymentType.BoundText = rs_SelectedProspect1.Fields("EmploymentTypeNumber").Value
	DataCombo_EmploymentType.Refresh

	
	b_Legal =true
End Sub

Sub GetProspectLoanData()		

    
    if rs_open2 = true  then
       rs_SelectedProspect2.Close
       rs_open2 = false
	end if
     b_AllDataLoaded =false
    if s_Action = "Add Further Loan"  then 
		sSQL = "p_GetFurtherLoanProspectLoanDetails " & i_ProspectNumber
    else
		sSQL = "p_GetProspectLoanDetails " & i_ProspectNumber
    end if 
    rs_SelectedProspect2.CursorLocation = 3
	rs_SelectedProspect2.Open sSQL,conn,adOpenStatic

	 b_AllDataLoaded =false
	
	
	window.ProspectExistingLoan.Value = rs_SelectedProspect2.Fields("ProspectExistingLoan").Value


    if s_Action = "Add" then
   		sSQL = "SELECT * FROM PURPOSE WHERE PurposeNumber <> " & gi_FurtherLoan 
   	else
   	    sSQL = "SELECT * FROM PURPOSE"
   	end if
	rs_Purpose.CursorLocation = 3
	rs_Purpose.Open sSQL ,conn,adOpenStatic
	
	set DataCombo_Purpose.RowSource = rs_Purpose
	DataCombo_Purpose.ListField = rs_Purpose.Fields("PurposeDescription").name
	DataCombo_Purpose.BoundColumn =  rs_Purpose.Fields("PurposeNumber").name
	DataCombo_Purpose.BoundText = rs_SelectedProspect2.Fields("PurposeNumber").Value
	DataCombo_Purpose.Refresh
	
		
  	
	if s_Action = "Add" then
   		sSQL = "SELECT * FROM MARKETRATETYPE" 'WHERE MarketRateTypeNumber = 1"  ' Hard Coded to only list 1 at this stage 
   	else
   	    sSQL = "SELECT * FROM MARKETRATETYPE" 'WHERE MarketRateTypeNumber = 1"  ' as is unlikely we will use another market rate type 
   	end if
	rs_MarketRateType.CursorLocation = 3
	rs_MarketRateType.Open sSQL ,conn,adOpenStatic
	
	set DataCombo_MarketRateType.RowSource = rs_MarketRateType
	DataCombo_MarketRateType.ListField = rs_MarketRateType.Fields("MarketRateTypeDescription").name
	DataCombo_MarketRateType.BoundColumn =  rs_MarketRateType.Fields("MarketRateTypeNumber").name
	DataCombo_MarketRateType.BoundText = rs_SelectedProspect2.Fields("MarketRateTypeNumber").Value
	DataCombo_MarketRateType.Refresh
	
	i_CurrentMarketRate = rs_MarketRateType.Fields("MarketRateTypeRate").Value

	if s_Action = "Add" then
   		sSQL = "SELECT * FROM LINKRATE" 
   	else
   	    sSQL = "SELECT * FROM LINKRATE" 
   	end if
	rs_LinkRate.CursorLocation = 3
	rs_LinkRate.Open sSQL ,conn,adOpenStatic
	
	set DataCombo_LinkRate.RowSource = rs_LinkRate
	DataCombo_LinkRate.ListField = rs_LinkRate.Fields("LinkRateDescription").name
	DataCombo_LinkRate.BoundColumn =  rs_LinkRate.Fields("LinkRate").name
	DataCombo_LinkRate.BoundText = rs_SelectedProspect2.Fields("LinkRate").Value
	DataCombo_LinkRate.Refresh
	

	i_CurrentLinkRate = rs_SelectedProspect2.Fields("LinkRate").Value

	i_CurrentLoanRate = i_CurrentMarketRate + i_CurrentLinkRate

	
	if Cint(rs_SelectedProspect2.Fields("PurposeNumber").Value) = gi_FurtherLoan then
	   window.DataCombo_Purpose.Enabled = false
	   window.lbl_Term.innerText = "Term (Months)"
	end if
	
	
    if s_Action = "Add Further Loan" or CInt(window.DataCombo_Purpose.BoundText) =  gi_FurtherLoan then 
		window.ProspectTermRequired.Value = rs_SelectedProspect2.Fields("ProspectTermRequired").Value 
    else
		window.ProspectTermRequired.Value = rs_SelectedProspect2.Fields("ProspectTermRequired").Value / 12
	end if
	window.ProspectEstimatedPropertyValue.Value = rs_SelectedProspect2.Fields("ProspectEstimatedPropertyValue").Value
	
	
    window.ProspectBondRequired.Value = rs_SelectedProspect2.Fields("ProspectBondRequired").Value

	
	window.ProspectFactor.Value = rs_SelectedProspect2.Fields("ProspectFactor").Value
	window.ProspectCashRequired.Value = rs_SelectedProspect2.Fields("ProspectCashRequired").Value
    window.ProspectCashDeposit.Value = rs_SelectedProspect2.Fields("ProspectCashDeposit").Value
	window.ProspectLoanRequired.Value = rs_SelectedProspect2.Fields("ProspectLoan").Value
	window.ProspectBondToRegister.Value = rs_SelectedProspect2.Fields("ProspectBond").Value

	
	window.ProspectCancelFees.Value = rs_SelectedProspect2.Fields("ProspectCancelFee").Value
    window.ProspectTransferFees.Value = rs_SelectedProspect2.Fields("ProspectTransferFee").Value
    window.ProspectRegistrationFees.Value = rs_SelectedProspect2.Fields("ProspectRegistrationFee").Value
	window.ProspectSAHLFees.Value = rs_SelectedProspect2.Fields("ProspectAdminFee").Value
	window.ProspectValuationFee.Value = rs_SelectedProspect2.Fields("ProspectValuationFee").Value

    window.chk_factor.checked =  rs_SelectedProspect2.Fields("ProspectFactorOverRide").Value
    window.chk_Fees.checked =  rs_SelectedProspect2.Fields("ProspectFeesOverRide").Value

   window.ProspectTotalFees.Value = window.ProspectTransferFees.Value + window.ProspectCancelFees.Value + window.ProspectRegistrationFees.Value + window.ProspectValuationFee.Value + window.ProspectSAHLFees.Value

     
End Sub

Sub GetProspectPropertyData()		
    
    
    if rs_open3 = true  then
       rs_SelectedProspect3.Close
       rs_open3 = false
	end if
    
    if d then
		sSQL = "p_GetFurtherLoanProspectPropertyDetails " & i_ProspectNumber 
    else
		sSQL = "p_GetProspectPropertyDetails " & i_ProspectNumber 
    end if
    rs_SelectedProspect3.CursorLocation = 3
	rs_SelectedProspect3.Open sSQL,conn,adOpenDynamic
	
    
    window.ProspectPropertyBuildingNumber.Text = rs_SelectedProspect3.Fields("ProspectPropertyBuildingNumber").Value
    window.ProspectPropertyBuildingName.Text = rs_SelectedProspect3.Fields("ProspectPropertyBuildingName").Value
    window.ProspectPropertyStreetNumber.Text = rs_SelectedProspect3.Fields("ProspectPropertyStreetNumber").Value
    window.ProspectPropertyStreetName.Text = rs_SelectedProspect3.Fields("ProspectPropertyStreetName").Value
	
	window.ProspectPropertySuburb.Text = rs_SelectedProspect3.Fields("ProspectPropertySuburb").Value
    window.ProspectPropertyCity.Text = rs_SelectedProspect3.Fields("ProspectPropertyCity").Value
    window.ProspectPropertyProvince.Text = rs_SelectedProspect3.Fields("ProspectPropertyProvince").Value
    window.ProspectPropertyCountry.Text = rs_SelectedProspect3.Fields("ProspectPropertyCountry").Value
    window.ProspectPropertyPostalCode.Text = rs_SelectedProspect3.Fields("ProspectPropertyPostalCode").Value


    sSQL = "SELECT * FROM PROVINCE"
	rs_Province.CursorLocation = 3
	rs_Province.Open sSQL ,conn,adOpenStatic

	set DataCombo_PropertyProvince.RowSource = rs_Province
	DataCombo_PropertyProvince.ListField = rs_Province.Fields("ProvinceName").name
	DataCombo_PropertyProvince.BoundColumn =  rs_Province.Fields("ProvinceName").name
	DataCombo_PropertyProvince.BoundText = rs_SelectedProspect3.Fields("ProspectPropertyProvince").Value
	DataCombo_PropertyProvince.Refresh
     
    sSQL = "SELECT * FROM PROPERTYTYPE"
	rs_Property.CursorLocation = 3
	rs_Property.Open sSQL ,conn,adOpenStatic

	set DataCombo_Property.RowSource = rs_Property
	DataCombo_Property.ListField = rs_Property.Fields("PropertyTypeDescription").name
	DataCombo_Property.BoundColumn =  rs_Property.Fields("PropertyTypeNumber").name
	DataCombo_Property.BoundText = rs_SelectedProspect3.Fields("PropertyTypeNumber").Value
	DataCombo_Property.Refresh
	
	
	
   if Cint(DataCombo_Property.BoundText) = 1 then 'Unknown
	        if b_Title = true then
	         rs_Title.close
	       end if
			sSQL = "SELECT * FROM TITLETYPE WHERE TitleTypeNumber = 1 OR TitleTypeNumber = 6"
			rs_Title.CursorLocation = 3
			rs_Title.Open sSQL ,conn,adOpenStatic
			b_Title= true
	elseif CInt(DataCombo_Property.BoundText) = 2 then 'House
	        if b_Title = true then
	          rs_Title.close
	        end if
			sSQL = "SELECT * FROM TITLETYPE WHERE TitleTypeNumber = 1 OR TitleTypeNumber = 2 OR TitleTypeNumber = 6"
			rs_Title.CursorLocation = 3
			rs_Title.Open sSQL ,conn,adOpenStatic
			b_Title= true
	elseif CInt(DataCombo_Property.BoundText) > 2 and  CInt(DataCombo_Property.BoundText) <= 7 then 'House
	        if b_Title = true then
	         rs_Title.close
	        end if
			sSQL = "SELECT * FROM TITLETYPE WHERE TitleTypeNumber = 1 OR (TitleTypeNumber >= 2 AND TitleTypeNumber <=7)"
			rs_Title.CursorLocation = 3
			rs_Title.Open sSQL ,conn,adOpenStatic
	        b_Title= true
	end if
	
	set DataCombo_Title.RowSource = rs_Title
	DataCombo_Title.ListField = rs_Title.Fields("TitleTypeDescription").name
	DataCombo_Title.BoundColumn =  rs_Title.Fields("TitleTypeNumber").name
	DataCombo_Title.BoundText = rs_SelectedProspect3.Fields("TitleTypeNumber").Value
	DataCombo_Title.Refresh
    b_Title = True
    

     sSQL = "SELECT * FROM AREACLASSIFICATION"
	rs_Area.CursorLocation = 3
	rs_Area.Open sSQL ,conn,adOpenStatic

    set DataCombo_Area.RowSource = rs_Area
	DataCombo_Area.ListField = rs_Area.Fields("AreaClassificationDescription").name
	DataCombo_Area.BoundColumn =  rs_Area.Fields("AreaClassificationNumber").name
	DataCombo_Area.BoundText = rs_SelectedProspect3.Fields("AreaClassificationNumber").Value
	DataCombo_Area.Refresh

     '*** Manually populate the prospect thatch
    rs_Thatch.Fields.Append "ThatchNumber",19
    rs_Thatch.Fields.Append "ThatchDetail",200,180
    rs_Thatch.Open

	rs_Thatch.AddNew 
	rs_Thatch.fields("ThatchNumber").Value = 0 
	rs_Thatch.fields("ThatchDetail").Value = "No"
	rs_Thatch.Update
	rs_Thatch.AddNew 
	rs_Thatch.fields("ThatchNumber").Value = 1 
	rs_Thatch.fields("ThatchDetail").Value = "Yes"
	rs_Thatch.Update
	rs_Thatch.MoveFirst

	set DataCombo_Thatch.RowSource = rs_Thatch
	DataCombo_Thatch.ListField = rs_Thatch.Fields("ThatchDetail").name
	DataCombo_Thatch.BoundColumn =  rs_Thatch.Fields("ThatchNumber").name
	DataCombo_Thatch.BoundText = rs_SelectedProspect3.Fields("ProspectPropertyThatch").Value
	DataCombo_Thatch.Refresh

 
 
     '*** Manually populate the owner occupired
    rs_Owner.Fields.Append "OwnerNumber",19
    rs_Owner.Fields.Append "OwnerDetail",200,180
    rs_Owner.Open

	rs_Owner.AddNew 
	rs_Owner.fields("OwnerNumber").Value = 0 
	rs_Owner.fields("OwnerDetail").Value = "No"
	rs_Owner.Update
	rs_Owner.AddNew 
	rs_Owner.fields("OwnerNumber").Value = 1 
	rs_Owner.fields("OwnerDetail").Value = "Yes"
	rs_Owner.Update
	rs_Owner.MoveFirst

	set DataCombo_Owner.RowSource = rs_Owner
	DataCombo_Owner.ListField = rs_Owner.Fields("OwnerDetail").name
	DataCombo_Owner.BoundColumn =  rs_Owner.Fields("OwnerNumber").name
	DataCombo_Owner.BoundText = rs_SelectedProspect3.Fields("ProspectOwnerOccupied").Value
	DataCombo_Owner.Refresh

	
    window.ProspectContactName.Text = rs_SelectedProspect3.Fields("ProspectContactName").Value
    window.ProspectContactNumber.Text = rs_SelectedProspect3.Fields("ProspectContactNumber").Value
    
End Sub

Sub GetProspectContactData()		
    

   if rs_open4 = true  then
       rs_SelectedProspect4.Close
       rs_open4 = false
	end if

    if s_Action = "Add Further Loan" then
         sSQL = "p_GetFurtherLoanProspectContactDetails " & i_ProspectNumber 
    else
		 sSQL = "p_GetProspectContactDetails " & i_ProspectNumber 
    end if
    rs_SelectedProspect4.CursorLocation = 3
	rs_SelectedProspect4.Open sSQL,conn,adOpenDynamic
	
   sSQL = "SELECT * FROM PROVINCE"
	rs_Province1.CursorLocation = 3
	rs_Province1.Open sSQL ,conn,adOpenStatic

	set DataCombo_ContactProvince.RowSource = rs_Province1
	DataCombo_ContactProvince.ListField = rs_Province1.Fields("ProvinceName").name
	DataCombo_ContactProvince.BoundColumn = rs_Province1.Fields("ProvinceName").name
	
	DataCombo_ContactProvince.BoundText = rs_SelectedProspect4.Fields("ProspectHomeProvince").Value
	DataCombo_ContactProvince.Refresh

    
    window.ProspectHomeBuildingNumber.Text = rs_SelectedProspect4.Fields("ProspectHomeBuildingNumber").Value
    window.ProspectHomeBuildingName.Text = rs_SelectedProspect4.Fields("ProspectHomeBuildingName").Value
    window.ProspectHomeStreetNumber.Text = rs_SelectedProspect4.Fields("ProspectHomeStreetNumber").Value
    window.ProspectHomeStreetName.Text = rs_SelectedProspect4.Fields("ProspectHomeStreetName").Value
	
	window.ProspectHomeSuburb.Text = rs_SelectedProspect4.Fields("ProspectHomeSuburb").Value
    window.ProspectHomeCity.Text = rs_SelectedProspect4.Fields("ProspectHomeCity").Value
    window.ProspectHomeProvince.Text = rs_SelectedProspect4.Fields("ProspectHomeProvince").Value
    window.ProspectHomeCountry.Text = rs_SelectedProspect4.Fields("ProspectHomeCountry").Value
    window.ProspectHomePostalCode.Text = rs_SelectedProspect4.Fields("ProspectHomePostalCode").Value

    window.ProspectHomePhoneCode.Text = rs_SelectedProspect4.Fields("ProspectHomePhoneCode").Value
    window.ProspectHomePhoneNumber.Text = rs_SelectedProspect4.Fields("ProspectHomePhoneNumber").Value
    window.ProspectWorkPhoneCode.Text = rs_SelectedProspect4.Fields("ProspectWorkPhoneCode").Value
    window.ProspectWorkPhoneNumber.Text = rs_SelectedProspect4.Fields("ProspectWorkPhoneNumber").Value
    window.ProspectFaxPhoneCode.Text = rs_SelectedProspect4.Fields("ProspectFaxCode").Value
    window.ProspectFaxPhoneNumber.Text = rs_SelectedProspect4.Fields("ProspectFaxNumber").Value
    window.ProspectCellPhoneNumber.Text = rs_SelectedProspect4.Fields("ProspectCellularTelephone").Value
    window.ProspectEMailAddress.Text = rs_SelectedProspect4.Fields("ProspectEMailAddress").Value
    window.ProspectBoxNumber.Text = rs_SelectedProspect4.Fields("ProspectBoxNumber").Value
    window.ProspectPostOffice.Text = rs_SelectedProspect4.Fields("ProspectPostOffice").Value
    window.ProspectPostalCode.Text = rs_SelectedProspect4.Fields("ProspectPostalCode").Value

End Sub

Sub GetProspectSAHLAdminData()		

    if rs_open5 = true  then
       rs_SelectedProspect5.Close
       rs_open5 = false
	end if

    sSQL = "p_GetProspectSAHLAdminDetails " & i_ProspectNumber 
    rs_SelectedProspect5.CursorLocation = 3
	rs_SelectedProspect5.Open sSQL,conn,adOpenDynamic

    window.ProspectSuretorName.Text = rs_SelectedProspect5.Fields("ProspectSuretorName").Value
    window.ProspectSuretorIDNumber.Text = rs_SelectedProspect5.Fields("ProspectSuretorIDNumber").Value
    window.ProspectTransferAttorney.Text = rs_SelectedProspect5.Fields("ProspectTransferAttorney").Value
    if rs_SelectedProspect5.Fields("ProspectCurrentStartDate").Value <> Null then
		window.ProspectCurrentStartDate.Value= rs_SelectedProspect5.Fields("ProspectCurrentStartDate").Value
    end if
    window.ProspectCurrentEmployer.Text = rs_SelectedProspect5.Fields("ProspectCurrentEmployer").Value
    if  rs_SelectedProspect5.Fields("ProspectPreviousStartDate").Value <> Null then 
		window.ProspectPreviousStartDate.Value = rs_SelectedProspect5.Fields("ProspectPreviousStartDate").Value
    end if
    window.ProspectPreviousEmployer.Text = rs_SelectedProspect5.Fields("ProspectPreviousEmployer").Value
    
    sSQL = "SELECT SPVNumber,SPVDescription FROM SPV WHERE SPVLocked = 0 or SPVNumber = " & rs_SelectedProspect5.Fields("SPVNumber").Value
	rs_SPV.CursorLocation = 3
	rs_SPV.Open sSQL ,conn,adOpenStatic
	

	set DataCombo_SPV.RowSource = rs_SPV
	DataCombo_SPV.ListField = rs_SPV.Fields("SPVDescription").name
	DataCombo_SPV.BoundColumn =  rs_SPV.Fields("SPVNumber").name
	DataCombo_SPV.BoundText = rs_SelectedProspect5.Fields("SPVNumber").Value
	DataCombo_SPV.Refresh

	
	
	sSQL = "SELECT * FROM EMPLOYEETEAM WHERE EmployeeTeamNumber NOT IN (6,8)"
	rs_Team.CursorLocation = 3
	rs_Team.Open sSQL ,conn,adOpenStatic
	

	set DataCombo_EmployeeTeam.RowSource = rs_Team
	DataCombo_EmployeeTeam.ListField = rs_Team.Fields("EmployeeTeamDescription").name
	DataCombo_EmployeeTeam.BoundColumn =  rs_Team.Fields("EmployeeTeamNumber").name
	DataCombo_EmployeeTeam.BoundText = rs_SelectedProspect5.Fields("EmployeeTeamNumber").Value
	DataCombo_EmployeeTeam.Refresh
	
	if rs_AdminOpen = true then
	   rs_Admin.Close
	end if
 
	'sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEMPLOYEE WHERE EmployeeTeamNumber = " & rs_SelectedProspect5.Fields("EmployeeTeamNumber").Value
     sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEMPLOYEE WHERE EmployeeTeamNumber = " & rs_SelectedProspect5.Fields("EmployeeTeamNumber").Value & " AND (UserGroupNumber = 5 or UserGroupNumber = 20 or UserGroupNumber = 4) AND (SAHLEmployeeStatusFlag <> 0 OR (SAHLEmployeeStatusFlag = 0  AND SAHLEmployeeNumber = " & rs_SelectedProspect5.Fields("SAHLEmployeeNumber1").Value & " )) ORDER BY SAHLEmployeeName"

	rs_Admin.CursorLocation = 3
	rs_Admin.Open sSQL ,conn,adOpenStatic
		
	set DataCombo_AdminEmployee.RowSource = rs_Admin
	DataCombo_AdminEmployee.ListField = rs_Admin.Fields("SAHLEmployeeName").name
	DataCombo_AdminEmployee.BoundColumn =  rs_Admin.Fields("SAHLEmployeeNumber").name
	DataCombo_AdminEmployee.BoundText = rs_SelectedProspect5.Fields("SAHLEmployeeNumber1").Value
	DataCombo_AdminEmployee.Refresh
	rs_Admin.MoveFirst
	rs_Admin.Find "SAHLEmployeeNumber = " & rs_SelectedProspect5.Fields("SAHLEmployeeNumber1").Value
	DataCombo_AdminEmployee.BoundText =  rs_SelectedProspect5.Fields("SAHLEmployeeNumber1").Value
        rs_AdminOpen = true
    
	'sSQL = "SELECT BrokerNumber,BrokerName FROM BROKER ORDER BY BrokerName"
	sSQL = "SELECT BrokerNumber,BrokerName FROM BROKER WHERE BrokerStatusNumber = 0 OR BrokerNumber = " & rs_SelectedProspect5.Fields("BrokerNumber").Value & " ORDER BY BrokerName"
	rs_Broker.CursorLocation = 3
	rs_Broker.Open sSQL ,conn,adOpenStatic
		
	set DataCombo_Broker.RowSource = rs_Broker
	DataCombo_Broker.ListField = rs_Broker.Fields("BrokerName").name
	DataCombo_Broker.BoundColumn =  rs_Broker.Fields("BrokerNumber").name
	DataCombo_Broker.BoundText = rs_SelectedProspect5.Fields("BrokerNumber").Value
	DataCombo_Broker.Refresh
	
	sSQL = "SELECT * FROM PROSPECTSOURCE"
	rs_Source.CursorLocation = 3
	rs_Source.Open sSQL ,conn,adOpenStatic
	
	set DataCombo_Source.RowSource = rs_Source
	DataCombo_Source.ListField = rs_Source.Fields("ProspectSourceDescription").name
	DataCombo_Source.BoundColumn =  rs_Source.Fields("ProspectSourceNumber").name
	DataCombo_Source.BoundText = rs_SelectedProspect5.Fields("ProspectSourceNumber").Value
	DataCombo_Source.Refresh
	   
	sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEMPLOYEE WHERE UserGroupNumber = 6 AND (SAHLEmployeeStatusFlag <> 0 OR (SAHLEmployeeStatusFlag = 0  AND SAHLEmployeeNumber = " & rs_SelectedProspect5.Fields("SAHLEmployeeNumber").Value & " )) "
	
	' AND SAHLEmployeeNumber = 4" 'Only select Unknown TeleConsultant (24/06/2001) - removed 04/07/2001 for TeleAdmin

	rs_Telesales.CursorLocation = 3
	rs_Telesales.Open sSQL ,conn,adOpenStatic
     
	set DataCombo_TeleSales.RowSource = rs_Telesales
	DataCombo_TeleSales.ListField = rs_Telesales.Fields("SAHLEmployeeName").name
	DataCombo_TeleSales.BoundColumn =  rs_Telesales.Fields("SAHLEmployeeNumber").name
	DataCombo_TeleSales.BoundText = rs_SelectedProspect5.Fields("SAHLEmployeeNumber").Value
	DataCombo_TeleSales.Refresh

     
	rs_open = true
   
  
     
End Sub


Sub GetFurtherLoanSAHLAdminData()		

    if rs_open5 = true  then
       rs_SelectedProspect5.Close
       rs_open5 = false
	end if

    sSQL = "p_GetFurtherLoanProspectSAHLAdminDetails " & i_ProspectNumber 
    rs_SelectedProspect5.CursorLocation = 3
	rs_SelectedProspect5.Open sSQL,conn,adOpenDynamic

    window.ProspectSuretorName.Text = rs_SelectedProspect5.Fields("ProspectSuretorName").Value
    window.ProspectSuretorIDNumber.Text = rs_SelectedProspect5.Fields("ProspectSuretorIDNumber").Value
    window.ProspectTransferAttorney.Text = rs_SelectedProspect5.Fields("ProspectTransferAttorney").Value
  
   ' if rs_SelectedProspect5.Fields("ProspectCurrentStartDate").Value <> Null then
      'msgbox rs_SelectedProspect5.Fields("ProspectCurrentStartDate").Value
		window.ProspectCurrentStartDate.Value= rs_SelectedProspect5.Fields("ProspectCurrentStartDate").Value
    'end if
    window.ProspectCurrentEmployer.Text = rs_SelectedProspect5.Fields("ProspectCurrentEmployer").Value
    'if  rs_SelectedProspect5.Fields("ProspectPreviousStartDate").Value <> Null then 
		window.ProspectPreviousStartDate.Value = rs_SelectedProspect5.Fields("ProspectPreviousStartDate").Value
    'end if
    window.ProspectPreviousEmployer.Text = rs_SelectedProspect5.Fields("ProspectPreviousEmployer").Value
    
    sSQL = "SELECT SPVNumber,SPVDescription FROM SPV WHERE SPVLocked = 0 or SPVNumber = " & rs_SelectedProspect5.Fields("SPVNumber").Value
	rs_SPV.CursorLocation = 3
	rs_SPV.Open sSQL ,conn,adOpenStatic
	

	set DataCombo_SPV.RowSource = rs_SPV
	DataCombo_SPV.ListField = rs_SPV.Fields("SPVDescription").name
	DataCombo_SPV.BoundColumn =  rs_SPV.Fields("SPVNumber").name
	DataCombo_SPV.BoundText = rs_SelectedProspect5.Fields("SPVNumber").Value
	DataCombo_SPV.Refresh

	
	
	 sSQL = "SELECT * FROM EMPLOYEETEAM WHERE EmployeeTeamNumber NOT IN (6,8)"
	rs_Team.CursorLocation = 3
	rs_Team.Open sSQL ,conn,adOpenStatic

	set DataCombo_EmployeeTeam.RowSource = rs_Team
	DataCombo_EmployeeTeam.ListField = rs_Team.Fields("EmployeeTeamDescription").name
	DataCombo_EmployeeTeam.BoundColumn =  rs_Team.Fields("EmployeeTeamNumber").name
	DataCombo_EmployeeTeam.BoundText = rs_Team.Fields("EmployeeTeamNumber").Value
	DataCombo_EmployeeTeam.Refresh
	
	rs_Team.MoveFirst
	rs_Team.Find "EmployeeTeamNumber = " & CSTR(i_EmployeeTeamNumber)
	window.DataCombo_EmployeeTeam.BoundText = i_EmployeeTeamNumber
    'DataCombo_EmployeeTeam.Refresh
	

        if rs_AdminOpen = true then
	   rs_Admin.Close
	end if

	sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEMPLOYEE WHERE EmployeeTeamNumber = " & CStr(i_EmployeeTeamNumber)  & " AND (UserGroupNumber = 5 or UserGroupNumber = 20 or UserGroupNumber = 4) AND SAHLEmployeeStatusFlag <> 0"
	
	rs_Admin.CursorLocation = 3
	rs_Admin.Open sSQL ,conn,adOpenStatic
        rs_AdminOpen = true
    
	set DataCombo_AdminEmployee.RowSource = rs_Admin
	DataCombo_AdminEmployee.ListField = rs_Admin.Fields("SAHLEmployeeName").name
	DataCombo_AdminEmployee.BoundColumn =  rs_Admin.Fields("SAHLEmployeeNumber").name
	DataCombo_AdminEmployee.BoundText = rs_Admin.Fields("SAHLEmployeeNumber").Value
	DataCombo_AdminEmployee.Refresh

	rs_Admin.MoveFirst
    rs_Admin.Find "SAHLEmployeeNumber = " & CStr(i_SAHLEmployeeNumber)
    window.DataCombo_AdminEmployee.BoundText = i_SAHLEmployeeNumber
    
    
	'sSQL = "SELECT BrokerNumber,BrokerName FROM BROKER ORDER BY BrokerName"
	sSQL = "SELECT BrokerNumber,BrokerName FROM BROKER WHERE BrokerStatusNumber = 0 OR BrokerNumber = " & rs_SelectedProspect5.Fields("BrokerNumber").Value & " ORDER BY BrokerName"
	rs_Broker.CursorLocation = 3
	rs_Broker.Open sSQL ,conn,adOpenStatic
		
	set DataCombo_Broker.RowSource = rs_Broker
	DataCombo_Broker.ListField = rs_Broker.Fields("BrokerName").name
	DataCombo_Broker.BoundColumn =  rs_Broker.Fields("BrokerNumber").name
	DataCombo_Broker.BoundText = rs_SelectedProspect5.Fields("BrokerNumber").Value
	DataCombo_Broker.Refresh
	
	sSQL = "SELECT * FROM PROSPECTSOURCE"
	rs_Source.CursorLocation = 3
	rs_Source.Open sSQL ,conn,adOpenStatic
	
	set DataCombo_Source.RowSource = rs_Source
	DataCombo_Source.ListField = rs_Source.Fields("ProspectSourceDescription").name
	DataCombo_Source.BoundColumn =  rs_Source.Fields("ProspectSourceNumber").name
	DataCombo_Source.BoundText = rs_SelectedProspect5.Fields("ProspectSourceNumber").Value
	DataCombo_Source.Refresh
	   
	sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEMPLOYEE WHERE UserGroupNumber = 6 AND (SAHLEmployeeStatusFlag <> 0 OR (SAHLEmployeeStatusFlag = 0  AND SAHLEmployeeNumber = " & rs_SelectedProspect5.Fields("SAHLEmployeeNumber").Value & " )) " ' AND SAHLEmployeeNumber = 4" 'Only select Unknown TeleConsultant (24/06/2001) - removed 04/07/2001 for TeleAdmin
	rs_Telesales.CursorLocation = 3
	rs_Telesales.Open sSQL ,conn,adOpenStatic
     
	set DataCombo_TeleSales.RowSource = rs_Telesales
	DataCombo_TeleSales.ListField = rs_Telesales.Fields("SAHLEmployeeName").name
	DataCombo_TeleSales.BoundColumn =  rs_Telesales.Fields("SAHLEmployeeNumber").name
	DataCombo_TeleSales.BoundText = rs_SelectedProspect5.Fields("SAHLEmployeeNumber").Value
	DataCombo_TeleSales.Refresh

     
	rs_open = true
   
  
     
End Sub

Sub btn_Exit_onclick

'msgbox s_ReturnPage
'msgbox s_ProspectStage
if s_ReturnPage = "FindProspect.asp" then
    if RTrim(s_FirstNames) <> "" or RTrim(s_SurName) <> "" then
      ' i_ProspectNumber = 0 
    end if
	window.parent.frames(1).location.href = s_ReturnPage & "?ProspectNumber= " & CStr(i_ProspectNumber ) & "&FirstNames=" & s_FirstNames &"&SurName=" & s_SurName
else   
	window.parent.frames(1).location.href = s_ReturnPage & "?Number= " & CStr(i_ProspectNumber ) & "&stage=" & s_ProspectStage
end if

End Sub

Function GetIEVersion()
 Dim strName, strVersion

strVersion = navigator.appVersion

i= 0
i = instr(1,lcase(strVersion),"5.0",1)
GetIEVersion = 0
if i > 0 then

   GetIEVersion = 5
   exit function
end if

i=0
i = instr(1,lcase(strVersion),"4.0",1)
if i > 0 then

   GetIEVersion = 4
   exit function
end if


End Function

Sub window_onload
rs_AdminOpen = false
x = "=<%= Session("SQLDatabase")%>"
    
if x = "=" then 
   exit sub
end if


if GetIEVersion = 4 then

	window.scroll 0 ,600
	window.scroll 0 ,1200
	window.scroll 0 ,0

end if


b_Forward = true
b_LoanBtn =false
b_FeeCalced = false
rs_commentopen = false

d_FeeLastCalced = 0.00

window.ProspectCurrentStartDate.DropDown.Visible = 1
window.ProspectCurrentStartDate.Spin.Visible = 1
window.ProspectPreviousStartDate.DropDown.Visible = 1
window.ProspectPreviousStartDate.Spin.Visible = 1


document.body.style.cursor = "hand"
b_CDone = false
    sUserName = "<%= Session("UserName")%>"
	sSQL = "SELECT SAHLEmployeeNumber,EmployeeTypeNumber,EmployeeTeamNumber,UserGroupNumber FROM SAHLEMPLOYEE WHERE SAHLEmployeeName = '" & RTrim(sUserName) & "'" 
	' msgbox sSQL
	rstemp.CursorLocation = 3
	rstemp.Open sSQL,conn,adOpenDynamic
	
	if rstemp.RecordCount > 0 then
		i_EmployeeType = rstemp.Fields("EmployeeTypeNumber").Value
		i_UserGroup = rstemp.Fields("UserGroupNumber").Value
		i_SAHLEmployeeNumber = rstemp.Fields("SAHLEmployeeNumber").Value
		i_EmployeeTeamNumber = rstemp.Fields("EmployeeTeamNumber").Value
		rstemp.Close
	else
	    MsgBox "You are not an authorised user in the Database....."
	    window.close 
	    exit sub
	end if


 
   sSQL = "SELECT ControlNumeric FROM CONTROL WHERE ControlNumber = 3"
   rstemp.CursorLocation = 3
   rstemp.Open sSQL,conn,adOpenDynamic
   
   i_CurrentBankRate = rstemp.Fields("ControlNumeric").Value/100

   rsTemp.Close
   
   sSQL = "SELECT ControlNumeric FROM CONTROL WHERE ControlNumber = 4"
   rstemp.CursorLocation = 3
   rstemp.Open sSQL,conn,adOpenDynamic
   
   i_InterimPeriod = rstemp.Fields("ControlNumeric").Value
   
   rsTemp.Close
   
  ' sSQL = "SELECT MarketRateTypeRate FROM MARKETRATETYPE WHERE MarketRateTypeNumber = 1"
  ' rstemp.CursorLocation = 3
  ' rstemp.Open sSQL,conn,adOpenDynamic
   
  ' i_CurrentLoanRate = rstemp.Fields("MarketRateTypeRate").Value

  ' rsTemp.Close
   
  ' sSQL = "SELECT ControlNumeric FROM CONTROL WHERE ControlNumber = 1"
  ' rstemp.CursorLocation = 3
  ' rstemp.Open sSQL,conn,adOpenDynamic

  ' i_CurrentLoanRate = i_CurrentLoanRate + (rstemp.Fields("ControlNumeric").Value/100.00)

  ' rsTemp.Close

   sSQL = "SELECT * FROM FEES ORDER BY FEERANGE"
   rstFees.CursorLocation = 3
   rstFees.Open sSQL,conn,adOpenDynamic


gi_OriginationAdmin = 2
gi_SwitchLoan = 2
gi_NewPurchase = 3
gi_Refinance = 4
gi_FurtherLoan = 5

 gi_LegalEntity = 4
 gi_JointOwner = 5
 b_Legal = False
 b_Title = False
 b_Loading =True
 b_Loading1 =True
 b_Loading2 = True
 b_Loading3 = true

 i_Nbr = "<%=Request.QueryString("Number")%>"
 'msgbox i_Nbr
 
 s_Source = "<%=Request.QueryString("Source")%>"
' msgbox s_Source
 '
 s_ReturnPage = s_Source
 
 i_ProspectNumber = i_Nbr

  s_ProspectStage = "<%=Request.QueryString("stage")%>"
  
  
  s_Action = "<%=Request.QueryString("Action")%>"
  
  
  s_LoanNumber = "<%=Request.QueryString("LoanNumber")%>"
  
  if s_LoanNumber <> "" then
	i_LoanNumber = CLng(s_LoanNumber)
  else
     i_LoanNumber = 0
  end if

'   msgbox i_LoanNumber
   
 document.body.style.cursor = "wait"
 
 if s_Action = "Add"  then  'Add Prospect
    window.focus
    'window.scroll 0,1000
   ' window.scroll 0,600
   ' SetUpAddProspectData
'	window.btn_Exit.value = "Cancel Add"
    'window.btn_Mode.value = "Commit Add"
	'EnableFields
	
	'window.DataCombo_Gender.focus()
	

 elseif s_Action = "Update" or s_Action = "View" then 
    s_FirstNames = "<%=Request.QueryString("FirstNames")%>"
    s_SurName = "<%=Request.QueryString("SurName")%>"
    window.focus
    'window.scroll 0,1000
    'window.scroll 0,600
   
	'GetProspectBasicData

	'GetProspectLoanData
	
	'GetProspectPropertyData

	'GetProspectContactData

	'GetProspectSAHLAdminData
	
	'window.btn_Exit.value = "Cancel Update"
	'window.btn_Mode.value = "Commit Update"
	'EnableFields
	'window.DataCombo_Gender.focus()
     ' window.scroll 0,0
  elseif s_Action = "Copy"  then 
    window.focus
    'window.scroll 0,1000
    'window.scroll 0,600
	'GetProspectBasicData
	'GetProspectLoanData
	'GetProspectPropertyData
	'ClearPropertyFields
	'GetProspectContactData
	'GetProspectSAHLAdminData
	'window.btn_Exit.value = "Cancel Copy"
	'window.btn_Mode.value = "Commit Copy"
	'EnableFields
	'window.DataCombo_Gender.focus()
	
 end if
 
 window.lbl_BasicMessage.innerText = "Basic Details"
 window.lbl_BasicMessage.style.backgroundcolor = ""
 window.btn_Mode.disabled = false
 
 if  s_Action <> "View" then
	window.btn_Mode.style.visibility  = "visible"
	window.pic_Mode.style.visibility  = "visible"
 end if
 document.body.style.cursor = "default"
 

 'window.focus


End Sub

Sub DataCombo_Gender_Change

'To Prevent sub activation  during window load...
if  b_Loading = true then 
   b_Loading = False
   exit sub
end if


     if Cint(DataCombo_Gender.BoundText) = gi_LegalEntity then 'Legal
            if b_Legal = true then
             rs_Legal.close
           end if
			sSQL = "SELECT * FROM LEGALSTATUS WHERE LegalStatusNumber >=5 AND LegalStatusNumber <=7"
			rs_Legal.CursorLocation = 3
			rs_Legal.Open sSQL ,conn,adOpenStatic
			b_Legal= true
    elseif CInt(DataCombo_Gender.BoundText) = 1 then 'Unknown
            if b_Legal = true then
              rs_Legal.close
            end if
    		sSQL = "SELECT * FROM LEGALSTATUS WHERE LegalStatusNumber =1"
			rs_Legal.CursorLocation = 3
			rs_Legal.Open sSQL ,conn,adOpenStatic
			b_Legal= true
    else
            if b_Legal = true then
             rs_Legal.close
            end if
			sSQL = "SELECT * FROM LEGALSTATUS WHERE LegalStatusNumber >=2 AND LegalStatusNumber <=4 OR LegalStatusNumber = 8 OR LegalStatusNumber = 9"
			rs_Legal.CursorLocation = 3
			rs_Legal.Open sSQL ,conn,adOpenStatic
            b_Legal= true
	end if
	 rs_legal.MoveFirst
	set DataCombo_Legal.RowSource = rs_Legal
	DataCombo_Legal.Refresh
	DataCombo_Legal.BoundText  = rs_legal.Fields("LegalStatusNumber").Value

    if Cint(window.DataCombo_Gender.BoundText) = gi_LegalEntity then
	  window.lbl_firstnames.style.visibility = "hidden"
	  window.ProspectFirstNames.style.visibility = "hidden"
	  window.lbl_surname.innerText = "Legal Entity"
	  window.lbl_salutation.style.visibility = "hidden"
	  window.ProspectSalutation.style.visibility = "hidden"
	  window.ProspectSalutation.Text = ""
	  window.ProspectFirstNames.Text = ""
	  window.ProspectSpouseIDNumber.Text = ""
	  window.ProspectSpouseIncome = 0
	  
	elseif CInt(window.DataCombo_Gender.BoundText) =  gi_JointOwner then
	   window.lbl_firstNames.style.visibility = "visible"
	   window.ProspectFirstNames.style.visibility = "visible"
	   window.lbl_surname.innerText = "Joint Owner"
	   window.lbl_salutation.style.visibility = "visible"
	   window.ProspectSalutation.style.visibility = "visible"
	else
	  window.lbl_firstNames.style.visibility = "visible"
	  window.ProspectFirstNames.style.visibility = "visible"
	  window.lbl_surname.innerText = "Surname"
	  window.lbl_salutation.style.visibility = "visible"
	  window.ProspectSalutation.style.visibility = "visible"
	end if
	
    if chk_factor.checked = false then
		CalculateFees
	end if

End Sub
'---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
'*
'*  Function Name         : CalculateFees
'*
'*  Description              :  Calculates fees applicable to the current prospect's loan values,sex,legal status etc..
'*
'*   Parameters             :  None
'*
'*  Return                     : none
'*
'---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Sub CalculateFees()
    Dim X 
    Dim Y 
    Dim z 
 
 
 if b_AllDataLoaded =false or b_Loading1 = true then exit sub

 
    if window.ProspectBondRequired.Value > 10000000 then
       msgbox "Invalid Bond Value... Bond cannot exceed R 2,000,000.00"
       exit sub
    end if
    
    if window.ProspectExistingLoan.Value > 10000000 then
       msgbox "Invalid Existing Loan Value... Existing Loan cannot exceed R R 10,000,000.00"
       exit sub
    end if
    
    if window.ProspectFactor.Value > 10000000 then
       msgbox "Invalid Interim Interest Value... Interim Interest cannot exceed R 10,000,000.00"
       exit sub
    end if
    
    if window.ProspectCashRequired.Value > 10000000 then
       msgbox "Invalid Cash Required... Cash Required cannot exceed R 10,000,000.00"
       exit sub
    end if
      
    if window.ProspectCashDeposit.Value > 10000000 then
       msgbox "Invalid Cash Depost... Cash Deposit has been limited to R 10,000,000.00"
       exit sub
    end if
      
    if window.ProspectCancelFees.Value > 10000000 then
       msgbox "Invalid Cancel Fee... Cancel Fee cannot exceed R 10,000,000.00"
       exit sub
    end if
    
    if window.ProspectTransferFees.Value > 10000000 then
       msgbox "Invalid Transfer Fee... Transfer Fee cannot exceed R 10,000,000.00"
       exit sub
    end if
      
      
     'Record set has already been poplulated during form load event...
  '   rstfees.MoveLast
     rstfees.MoveFirst

    'Calculate the Transfer Fees....
    window.ProspectTransferFees.Value = 0.00
    window.ProspectCancelFees.Value = 0.00

    'With rstfees
          If CInt(window.DataCombo_Purpose.BoundText) = 3 Then 'NewPurchase
             
                rstfees.Find "FeeRange >= " & window.ProspectExistingLoan.Value

                If CInt(window.DataCombo_Gender.BoundText) = 0 Or CInt(window.DataCombo_Gender.BoundText) = 1 Or CInt(window.DataCombo_Gender.BoundText)  = 2 Or CInt(window.DataCombo_Gender.BoundText)  = 3 Or CInt(window.DataCombo_Gender.BoundText)  = 5 Then 'Natural Person

                    window.ProspectTransferFees.Value =  0 'rstfees.Fields("FeeNaturalTransferDuty").Value + rstfees.fields("FeeNaturalConveyancing").Value + rstFees.Fields("FeeNaturalVat").Value

               Else 'Legal Person

                     window.ProspectTransferFees.Value = 0 ' rstfees.Fields("FeeLegalTransferDuty").Value + rstfees.fields("FeeLegalConveyancing").Value + rstFees.Fields("FeeLegalVat").Value
                     ' msgbox rstfees.fields("FeeLegalTransferDuty").Value + rstfees.fields("FeeLegalConveyancing").Value + rstFees.Fields("FeeLegalVat").Value
               end if
          ElseIf CInt(window.DataCombo_Purpose.BoundText) = 2 Then 'Switch Loan

                rstfees.Find "FeeRange >=" & window.ProspectExistingLoan.Value
                window.ProspectCancelFees.Value = rstFees.Fields("FeeCancelDuty").Value + rstfees.Fields("FeeCancelConveyancing").Value + rstFees.Fields("FeeCancelVAT").Value
            end if
     'End With
    

    ' Calculate the Total Loan to include the Transfer Fee or Cancel fee
    X = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectCashRequired.Value - window.ProspectCashDeposit.Value + window.ProspectTransferFees.Value + window.ProspectCancelFees.Value
    
    'Get the Admin and Valuation Fee.
    
     rstfees.MoveFirst
     rstfees.Find "FeeRange >= " & X

  '  TDBCmb_FeesRange.Text = rstfees!FeeRange
    window.ProspectSAHLFees.value = rstfees.Fields("FeeAdmin").Value
     If  CInt(window.DataCombo_Purpose.BoundText) <> 5 Then 'Further Loan
        window.ProspectValuationFee.Value = rstfees.Fields("FeeValuation").Value
    Else
        window.ProspectValuationFee.Value = 0.00
    end if

	if GetSAHLStaffDetailRecords() = 1 then
    
		window.ProspectValuationFee.Value = 0.00
		window.ProspectSAHLFees.value  = 0.00       
		
    end if

    'Add Admin and Valuation Fees..
    X = X + window.ProspectSAHLFees.Value + window.ProspectValuationFee.Value

    'Get the Registration Fees....
    
'   rstfees.MoveLast
   rstfees.MoveFirst

    If window.ProspectBondRequired.Value < X Then

         rstfees.Find "FeeRange >= " & X

        'TDBCmb_FeesRange.Text = rstfees!FeeRange
        z = rstfees.Fields("FeeRange").Value

      
		window.ProspectRegistrationFees.Value = rstFees.Fields("FeeBondStamps").Value + rstfees.Fields("FeeBondConveyancing").Value + rstfees.Fields("FeeBondVAT").Value

    Else

         rstfees.Find "FeeRange >= " & window.ProspectBondRequired.Value
         
         z = rstfees.Fields("FeeRange").Value

		 window.ProspectRegistrationFees.Value = rstfees.Fields("FeeBondStamps").Value + rstfees.Fields("FeeBondConveyancing").Value + rstfees.Fields("FeeBondVAT").Value
     
        'TDBCmb_FeesRange.Text = rstfees!FeeRange
        
         z = rstfees.Fields("FeeRange").Value

    end if

    window.ProspectLoanRequired.Value = X + window.ProspectRegistrationFees.Value
    
    'window.ProspectBondToRegister.Value = RoundUp(window.ProspectLoanRequired.Value)

   'If the bond to register amount is greate than the calculated amt then get Registration fees for the greater amt..
  ' rstfees.MoveLast
   rstfees.MoveFirst
    If window.ProspectBondToRegister.Value > z Then
        rstfees.Find "FeeRange > " & z + 1
  '      TDBCmb_FeesRange.Text = rstfees!FeeRange
        window.ProspectRegistrationFees.Value = rstfees.Fields("FeeBondStamps").Value + rstfees.Fields("FeeBondConveyancing").Value + rstFees.Fields("FeeBondVAT").Value
        window.ProspectLoanRequired.Value = X + window.ProspectRegistrationFees.Value
        window.ProspectBondToRegister.Value = RoundUp(window.ProspectLoanRequired.Value)
    end if

    window.ProspectTotalFees.Value = window.ProspectTransferFees.Value + window.ProspectCancelFees.Value + window.ProspectRegistrationFees.Value + window.ProspectValuationFee.Value + window.ProspectSAHLFees.Value

End Sub

Function RoundUp(d_InValue ) 

Dim l_Value 
Dim d_Value 
Dim l_Loan 
    
     RoundUp = 0.00
      
    ' l_Value = d_InValue / 1000

     'd_Value = Round(l_Value+0.5,0) * 1000.00

      'RoundUp = d_Value
        
     l_Value = Round(d_InValue / 1000,0)
     d_Value = l_Value * 1000
     if d_InValue - d_Value = 0 then
        RoundUp = d_Value
     elseif d_InValue - d_Value < 0 then
        RoundUp = l_Value * 1000
     else
          l_Loan = l_Value + 1
          RoundUp = l_Loan * 1000
     end if

End Function

Sub DataCombo_Purpose_Change

    'To Prevent sub activation  during window load...

	if  b_Loading1 = true then 
	   b_Loading1 = False
	'   exit sub
	end if	

	if CInt(window.DataCombo_Purpose.BoundText) <> 2 then
	    window.ProspectFactor.Value = 0.00
	elseif CInt(window.DataCombo_Purpose.BoundText) = 2 AND window.chk_factor.checked = false then
	     window.ProspectFactor.Value =  (window.ProspectExistingLoan.Value * i_CurrentBankRate)/52.00 * i_InterimPeriod
	elseif CInt(window.DataCombo_Purpose.BoundText) = 2 AND window.chk_factor.checked = true AND window.ProspectExistingLoan.Value > 0.00  then
	     'window.ProspectFactor.Value =  (window.ProspectExistingLoan.Value * i_CurrentBankRate)/52.00 * i_InterimPeriod
	else
	     window.ProspectFactor.Value = 0
	end if

	 if chk_factor.checked = false then
		CalculateFees
	end if
	
	if Cint(window.DataCombo_Purpose.BoundText) = gi_NewPurchase  then
	  window.lbl_ExistingLoan.style.visibility = "visible"
	  window.ProspectExistingLoan.style.visibility = "visible"
	  window.lbl_ExistingLoan.innerText = "Purchase Price"
	  window.lbl_factor.style.visibility = "hidden"
	  window.ProspectFactor.Value = 0.00
	  window.ProspectFactor.style.visibility = "hidden"
	  window.lbl_overridefactor.style.visibility = "hidden"
	  window.chk_factor.style.visibility = "hidden"
	elseif Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	  window.lbl_ExistingLoan.style.visibility = "hidden"
	  
	  window.ProspectCashDeposit.Value = 0
	  window.ProspectExistingLoan.Value = 0
	  window.ProspectExistingLoan.style.visibility = "hidden"
	  
	   window.ProspectFactor.Value = 0.00
	  window.lbl_factor.style.visibility = "hidden"
	 
	  window.ProspectFactor.style.visibility = "hidden"
	  window.lbl_overridefactor.style.visibility = "hidden"
	  window.chk_factor.style.visibility = "hidden"
	else
	  window.lbl_ExistingLoan.style.visibility = "visible"
	  window.lbl_ExistingLoan.innerText = "Existing Loan"
	  window.ProspectExistingLoan.style.visibility = "visible"
	  window.lbl_factor.style.visibility = "visible"
	  window.ProspectFactor.style.visibility = "visible"
	  window.lbl_overridefactor.style.visibility = "visible"
	  window.chk_factor.style.visibility = "visible"
    end if
	
End Sub

Sub ProspectBondRequired_Change
    'To Prevent sub activation  during window load...
		if  b_Loading1 = true then 
		   b_Loading1 = False
		   exit sub
		end if	

    if window.ProspectBondRequired.Value > 10000000 then
        window.ProspectBondRequired.Value = 0.00
        window.lbl_LoanMessage.innerText = "The Bond value is outside SAHL Fee Range..Please check and correct"
        window.lbl_LoanMessage.style.backgroundcolor = "Red"
        window.ProspectBondRequired.focus()
        exit sub
    End If

    window.lbl_LoanMessage.innerText = "Loan Details"
    window.lbl_LoanMessage.style.backgroundcolor = ""

	if chk_factor.checked = false then
		CalculateFees
	end if
	
	if window.ProspectBondRequired.Value > window.ProspectBondToRegister.Value then
	   window.ProspectBondToRegister.Value  = window.ProspectBondRequired.Value
	end if
	

	
End Sub

Sub ProspectCancelFees_Change

if b_AllDataLoaded =false or b_Loading1 = true then exit sub


 window.ProspectTotalFees.Value = window.ProspectCancelFees.Value + window.ProspectTransferFees.Value + window.ProspectRegistrationFees.Value + window.ProspectSAHLFees.Value + window.ProspectValuationFee.Value
   
 if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	window.ProspectLoanRequired.Value =  window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 else
	window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 end if
End Sub

Sub ProspectTransferFees_Change

if b_AllDataLoaded =false or b_Loading1 = true then exit sub


 window.ProspectTotalFees.Value = window.ProspectCancelFees.Value + window.ProspectTransferFees.Value + window.ProspectRegistrationFees.Value + window.ProspectSAHLFees.Value + window.ProspectValuationFee.Value
	
 if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	window.ProspectLoanRequired.Value =  window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 else
	window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 end if
End Sub

Sub ProspectRegistrationFees_Change

if b_AllDataLoaded =false or b_Loading1 = true then exit sub

	window.ProspectTotalFees.Value = window.ProspectCancelFees.Value + window.ProspectTransferFees.Value + window.ProspectRegistrationFees.Value + window.ProspectSAHLFees.Value + window.ProspectValuationFee.Value

	 if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
		window.ProspectLoanRequired.Value =  window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

	else
		window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

	end if
End Sub

Sub ProspectSAHLFees_Change
if b_AllDataLoaded =false or b_Loading1 = true then exit sub


window.ProspectTotalFees.Value = window.ProspectCancelFees.Value + window.ProspectTransferFees.Value + window.ProspectRegistrationFees.Value + window.ProspectSAHLFees.Value + window.ProspectValuationFee.Value
if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	window.ProspectLoanRequired.Value =  window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 else
	window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 end if
End Sub

Sub ProspectValuationFee_Change

if b_AllDataLoaded =false or b_Loading1 = true then exit sub


 window.ProspectTotalFees.Value = window.ProspectCancelFees.Value + window.ProspectTransferFees.Value + window.ProspectRegistrationFees.Value + window.ProspectSAHLFees.Value + window.ProspectValuationFee.Value
 if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	window.ProspectLoanRequired.Value =  window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 else
	window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 end if
 
 End Sub

Sub chk_factor_onclick


if chk_factor.checked = true then
   window.ProspectFactor.Enabled = true
else
   window.ProspectFactor.Enabled = false
end if

if CInt(window.DataCombo_Purpose.BoundText) <> 2 then
    window.ProspectFactor.Value = 0.00
elseif CInt(window.DataCombo_Purpose.BoundText) = 2 AND window.chk_factor.checked = false then
     window.ProspectFactor.Value =  (window.ProspectExistingLoan.Value * i_CurrentBankRate)/52.00 * i_InterimPeriod
elseif CInt(window.DataCombo_Purpose.BoundText) = 2 AND window.chk_factor.checked = true AND window.ProspectExistingLoan.Value > 0.00  then
     'window.ProspectFactor.Value =  (window.ProspectExistingLoan.Value * i_CurrentBankRate)/52.00 * i_InterimPeriod
else
     window.ProspectFactor.Value = 0
end if

End Sub

Sub chk_Fees_onclick

if chk_Fees.checked = true then

   if Cint(rs_SelectedProspect2.Fields("PurposeNumber").Value) = gi_FurtherLoan then
		window.ProspectValuationFee.Enabled = true
   end if
   window.ProspectCancelFees.Enabled = true
   window.ProspectTransferFees.Enabled = true
   
else
   window.ProspectCancelFees.Enabled = false
   window.ProspectTransferFees.Enabled = false
   window.ProspectValuationFee.Enabled = false
end if

End Sub

Sub ProspectExistingLoan_Change
 
if b_AllDataLoaded =false or b_Loading1 = true then exit sub

if CInt(window.DataCombo_Purpose.BoundText) <> 2 then
    window.ProspectFactor.Value = 0.00
elseif CInt(window.DataCombo_Purpose.BoundText) = 2 AND window.chk_factor.checked = false then
     window.ProspectFactor.Value =  (window.ProspectExistingLoan.Value * i_CurrentBankRate)/52.00 * i_InterimPeriod
elseif CInt(window.DataCombo_Purpose.BoundText) = 2 AND window.chk_factor.checked = true AND window.ProspectExistingLoan.Value > 0.00  then
     'window.ProspectFactor.Value =  (window.ProspectExistingLoan.Value * i_CurrentBankRate)/52.00 * i_InterimPeriod
else
     window.ProspectFactor.Value = 0
end if
 
 if chk_factor.checked = false then
		CalculateFees
 end if
 
 if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	window.ProspectLoanRequired.Value =  window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 else
	window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 end if
 
' window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit
	
End Sub

Sub ProspectFactor_Change

if b_AllDataLoaded =false or b_Loading1 = true then exit sub

 if chk_factor.checked = false then
		CalculateFees
 end if
 
 if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	window.ProspectLoanRequired.Value =  window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 else
	window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 end if
 
End Sub

Sub ProspectCashRequired_Change

if b_AllDataLoaded =false or b_Loading1 = true then exit sub

 if chk_factor.checked = false then
		CalculateFees
 end if
 
 window.ProspectCashRequired.Value = abs(window.ProspectCashRequired.Value )
 
 if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	window.ProspectLoanRequired.Value =  window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 else
	window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 end if
 
 

 
End Sub

Sub ProspectCashDeposit_Change

if b_AllDataLoaded =false or b_Loading1 = true then exit sub


 if chk_factor.checked = false then
    if b_FeeCalced = false then
		CalculateFees
		d_FeeLastCalced = 0.00
	else
	    b_FeeCalced = false
    end if
 end if
 
 window.ProspectCashDeposit.Value = abs(window.ProspectCashDeposit.Value )

 if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	window.ProspectLoanRequired.Value =  window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 else
	window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 end if
 
 
End Sub

Sub ProspectTotalFees_Change

if b_AllDataLoaded =false or b_Loading1 = true then exit sub

 
 if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	window.ProspectLoanRequired.Value =  window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired.Value  - window.ProspectCashDeposit.Value

 else
	window.ProspectLoanRequired.Value = window.ProspectExistingLoan.Value + window.ProspectFactor.Value + window.ProspectTotalFees + window.ProspectCashRequired - window.ProspectCashDeposit

 end if
End Sub

Sub ProspectLoanRequired_Change

  if b_AllDataLoaded =false or b_Loading1 = true then exit sub

	window.ProspectBondToRegister.Value = RoundUp(window.ProspectLoanRequired.Value)
End Sub

Sub chk_Foreign_onclick

	if chk_Foreign.checked = true then
        
        window.ProspectHomeProvince.Style.width = 241
        window.ProspectHomeProvince.Style.height = 27
        window.DataCombo_ContactProvince.Style.width = 0
        window.DataCombo_ContactProvince.Style.height = 0
        window.ProspectHomeProvince.Enabled = true
        window.DataCombo_ContactProvince.enabled = false
        
        window.ProspectHomeProvince.Style.visibility = "visible"
	    window.ProspectHomeCountry.Text = ""
		window.ProspectHomeProvince.focus
		
	elseif chk_Foreign.checked = false then
		
		window.ProspectHomeProvince.Style.width = 0
        window.ProspectHomeProvince.Style.height = 0
        window.ProspectHomeProvince.Enabled = false
		window.DataCombo_ContactProvince.Style.width = 241
        window.DataCombo_ContactProvince.Style.height = 27
        window.DataCombo_ContactProvince.enabled = true

		
		window.ProspectHomeProvince.Style.visibility = "hidden"
	 	window.ProspectHomeCountry.Text = "Republic of South Africa"
        window.DataCombo_ContactProvince.focus
        
	end if

	
	
End Sub

Sub DataCombo_EmployeeTeam_Change
 'To Prevent sub activation  during window load...
	if  b_Loading2 = true then 
	   b_Loading2 = False
	   exit sub
	end if	
	if rs_AdminOpen = true then
	   'rs_Admin.Close
	end if
    rs_Admin.Close
    
    if s_Action = "Update" then
		sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEmployee WHERE EmployeeTeamNumber = " & window.DataCombo_EmployeeTeam.BoundText  & " AND (UserGroupNumber = 5 or UserGroupNumber = 20 or UserGroupNumber = 4) AND (SAHLEmployeeStatusFlag <> 0 OR (SAHLEmployeeStatusFlag = 0  AND SAHLEmployeeNumber = " & rs_SelectedProspect5.Fields("SAHLEmployeeNumber1").Value & " )) ORDER BY SAHLEmployeeName"
    else
''		sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEmployee WHERE EmployeeTeamNumber = " & window.DataCombo_EmployeeTeam.BoundText  & " AND (EmployeeTypeNumber = 2 OR EmployeeTypeNumber = 3 OR EmployeeTypeNumber = 17) AND SAHLEmployeeStatusFlag <> 0"
		sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEmployee WHERE EmployeeTeamNumber = " & window.DataCombo_EmployeeTeam.BoundText  & " AND (UserGroupNumber = 5 or UserGroupNumber = 20 or UserGroupNumber = 4) AND SAHLEmployeeStatusFlag <> 0 ORDER BY SAHLEmployeeName"
	end if
  
	rs_Admin.CursorLocation = 3
	rs_Admin.Open sSQL ,conn,adOpenStatic
        if rs_Admin.RecordCount <> 0 then
           rs_Admin.MoveFirst
	   set DataCombo_AdminEmployee.RowSource = rs_Admin
	   DataCombo_AdminEmployee.Refresh
	   DataCombo_AdminEmployee.BoundText  = rs_Admin.Fields("SAHLEmployeeNumber").Value
	   'rs_Admin.Close
        else
           DataCombo_AdminEmployee.text = ""
           set DataCombo_AdminEmployee.RowSource = rs_Admin
	   DataCombo_AdminEmployee.Refresh
	   
        end if
	rs_AdminOpen = true
End Sub



Sub SetUpAddProspectData()
 
    i_switch = true
   
   sSQL = "SELECT ControlNumeric FROM CONTROL WHERE ControlNumber = 3"
   rstemp.CursorLocation = 3
   rstemp.Open sSQL,conn,adOpenDynamic
   
   i_CurrentBankRate = rstemp.Fields("ControlNumeric").Value/100
   
   rsTemp.Close
   
   sSQL = "SELECT ControlNumeric FROM CONTROL WHERE ControlNumber = 4"
   rstemp.CursorLocation = 3
   rstemp.Open sSQL,conn,adOpenDynamic
   
   i_InterimPeriod = rstemp.Fields("ControlNumeric").Value
   
   rsTemp.Close

   rstFees.Close
   sSQL = "SELECT * FROM FEES ORDER BY FEERANGE"
   rstFees.CursorLocation = 3
   rstFees.Open sSQL,conn,adOpenDynamic
   
   
   
    sSQL = "SELECT * FROM SEX"
	rs_Sex.CursorLocation = 3
	rs_Sex.Open sSQL ,conn,adOpenStatic

	rs_Sex.MoveFirst

    set DataCombo_Gender.RowSource = rs_Sex
	DataCombo_Gender.ListField = rs_Sex.Fields("SexDescription").name
	DataCombo_Gender.BoundColumn =  rs_Sex.Fields("SexNumber").name
	DataCombo_Gender.BoundText = rs_Sex.Fields("SexNumber").Value
	DataCombo_Gender.Refresh
	
    sSQL = "SELECT * FROM LEGALSTATUS WHERE LegalStatusNumber =1"
	rs_Legal.CursorLocation = 3
	rs_Legal.Open sSQL ,conn,adOpenStatic

	rs_Legal.MoveFirst
	
	set DataCombo_Legal.RowSource = rs_Legal
	DataCombo_Legal.ListField = rs_Legal.Fields("LegalStatusDescription").name
	DataCombo_Legal.BoundColumn =  rs_Legal.Fields("LegalStatusNumber").name
	DataCombo_Legal.BoundText = rs_Legal.Fields("LegalStatusNumber").Value
	DataCombo_Legal.Refresh
	
	b_Legal =true
	
	sSQL = "SELECT * FROM EMPLOYMENTTYPE" 
   	
	rs_EmploymentType.CursorLocation = 3
	rs_EmploymentType.Open sSQL ,conn,adOpenStatic
	
	set window.DataCombo_EmploymentType.RowSource = rs_EmploymentType
	DataCombo_EmploymentType.ListField = rs_EmploymentType.Fields("EmploymentTypeDescription").name
	DataCombo_EmploymentType.BoundColumn =  rs_EmploymentType.Fields("EmploymentTypeNumber").name
	DataCombo_EmploymentType.BoundText = rs_EmploymentType.Fields("EmploymentTypeNumber").Value
	DataCombo_EmploymentType.Refresh


   
     if s_Action = "Add" then
   		sSQL = "SELECT * FROM PURPOSE WHERE PurposeNumber <> " & gi_FurtherLoan 
   	else
   	    sSQL = "SELECT * FROM PURPOSE"
   	end if
	rs_Purpose.CursorLocation = 3
	rs_Purpose.Open sSQL ,conn,adOpenStatic
	

	set DataCombo_Purpose.RowSource = rs_Purpose
	DataCombo_Purpose.ListField = rs_Purpose.Fields("PurposeDescription").name
	DataCombo_Purpose.BoundColumn =  rs_Purpose.Fields("PurposeNumber").name
	DataCombo_Purpose.BoundText = rs_Purpose.Fields("PurposeNumber").Value
	DataCombo_Purpose.Refresh
	
	
	
	if s_Action = "Add" then
   		sSQL = "SELECT * FROM MARKETRATETYPE WHERE MarketRateTypeNumber = 1"  ' Hard Coded to only list 1 at this stage 
   	else
   	    sSQL = "SELECT * FROM MARKETRATETYPE WHERE MarketRateTypeNumber = 1"  ' as is unlikely we will use another market rate type 
   	end if
	rs_MarketRateType.CursorLocation = 3
	rs_MarketRateType.Open sSQL ,conn,adOpenStatic
	
	set DataCombo_MarketRateType.RowSource = rs_MarketRateType
	DataCombo_MarketRateType.ListField = rs_MarketRateType.Fields("MarketRateTypeDescription").name
	DataCombo_MarketRateType.BoundColumn =  rs_MarketRateType.Fields("MarketRateTypeNumber").name
	DataCombo_MarketRateType.BoundText = rs_MarketRateType.Fields("MarketRateTypeNumber").Value
	DataCombo_MarketRateType.Refresh
	
	i_CurrentMarketRate = rs_MarketRateType.Fields("MarketRateTypeRate").Value
	
	if s_Action = "Add" then
   		sSQL = "SELECT * FROM LINKRATE" 
   	else
   	    sSQL = "SELECT * FROM LINKRATE" 
   	end if
	rs_LinkRate.CursorLocation = 3
	rs_LinkRate.Open sSQL ,conn,adOpenStatic
	
	rs_LinkRate.Find "LinkRate = 0.021"
	set DataCombo_LinkRate.RowSource = rs_LinkRate
	DataCombo_LinkRate.ListField = rs_LinkRate.Fields("LinkRateDescription").name
	DataCombo_LinkRate.BoundColumn =  rs_LinkRate.Fields("LinkRate").name
	DataCombo_LinkRate.BoundText = rs_LinkRate.Fields("LinkRate").Value
	DataCombo_LinkRate.Refresh
	
	i_CurrentLinkRate = rs_LinkRate.Fields("LinkRate").Value
	
	i_CurrentLoanRate = i_CurrentMarketRate + i_CurrentLinkRate
	
	sSQL = "SELECT * FROM PROVINCE"
	rs_Province1.CursorLocation = 3
	rs_Province1.Open sSQL ,conn,adOpenStatic
    rs_Province1.MoveFirst
    
   ' msgbox rs_Province1.RecordCount
	set DataCombo_ContactProvince.RowSource = rs_Province1
	DataCombo_ContactProvince.ListField = rs_Province1.Fields("ProvinceName").name
	DataCombo_ContactProvince.BoundColumn =  rs_Province1.Fields("ProvinceName").name
	DataCombo_ContactProvince.BoundText = rs_Province1.Fields("ProvinceName").Value

	DataCombo_ContactProvince.Refresh


	sSQL = "SELECT * FROM PROVINCE"
	rs_Province.CursorLocation = 3
	rs_Province.Open sSQL ,conn,adOpenStatic
	
	set DataCombo_PropertyProvince.RowSource = rs_Province
	DataCombo_PropertyProvince.ListField = rs_Province.Fields("ProvinceName").name
	DataCombo_PropertyProvince.BoundColumn =  rs_Province.Fields("ProvinceName").name
	DataCombo_PropertyProvince.BoundText = rs_Province.Fields("ProvinceName").Value
	DataCombo_PropertyProvince.Refresh
	

    sSQL = "SELECT * FROM PROPERTYTYPE"
	rs_Property.CursorLocation = 3
	rs_Property.Open sSQL ,conn,adOpenStatic

	set DataCombo_Property.RowSource = rs_Property
	DataCombo_Property.ListField = rs_Property.Fields("PropertyTypeDescription").name
	DataCombo_Property.BoundColumn =  rs_Property.Fields("PropertyTypeNumber").name
	DataCombo_Property.BoundText = rs_Property.Fields("PropertyTypeNumber").Value
	DataCombo_Property.Refresh
	
	
	  if Cint(DataCombo_Property.BoundText) = 1 then 'Unknown
	        if b_Title = true then
	         rs_Title.close
	       end if
			sSQL = "SELECT * FROM TITLETYPE WHERE TitleTypeNumber = 1 OR TitleTypeNumber = 6"
			rs_Title.CursorLocation = 3
			rs_Title.Open sSQL ,conn,adOpenStatic
			b_Title= true
	elseif CInt(DataCombo_Property.BoundText) = 2 then 'House
	        if b_Title = true then
	          rs_Title.close
	        end if
			sSQL = "SELECT * FROM TITLETYPE WHERE TitleTypeNumber = 1 OR TitleTypeNumber = 2 OR TitleTypeNumber = 6"
			rs_Title.CursorLocation = 3
			rs_Title.Open sSQL ,conn,adOpenStatic
			b_Title= true
	elseif CInt(DataCombo_Property.BoundText) > 2 and  CInt(DataCombo_Property.BoundText) <= 7 then 'House
	        if b_Title = true then
	         rs_Title.close
	        end if
			sSQL = "SELECT * FROM TITLETYPE WHERE TitleTypeNumber = 1 OR (TitleTypeNumber >= 2 AND TitleTypeNumber <=7)"
			rs_Title.CursorLocation = 3
			rs_Title.Open sSQL ,conn,adOpenStatic
	        b_Title= true
	end if
	
	set DataCombo_Title.RowSource = rs_Title
	DataCombo_Title.ListField = rs_Title.Fields("TitleTypeDescription").name
	DataCombo_Title.BoundColumn =  rs_Title.Fields("TitleTypeNumber").name
	DataCombo_Title.BoundText = rs_Title.Fields("TitleTypeNumber").Value
	DataCombo_Title.Refresh
    b_Title = True


     sSQL = "SELECT * FROM AREACLASSIFICATION"
	rs_Area.CursorLocation = 3
	rs_Area.Open sSQL ,conn,adOpenStatic

	set DataCombo_Area.RowSource = rs_Area
	DataCombo_Area.ListField = rs_Area.Fields("AreaClassificationDescription").name
	DataCombo_Area.BoundColumn =  rs_Area.Fields("AreaClassificationNumber").name
	DataCombo_Area.BoundText = rs_Area.Fields("AreaClassificationNumber").Value
	DataCombo_Area.Refresh

     '*** Manually populate the prospect thatch
    rs_Thatch.Fields.Append "ThatchNumber",19
    rs_Thatch.Fields.Append "ThatchDetail",200,180
    rs_Thatch.Open

	rs_Thatch.AddNew 
	rs_Thatch.fields("ThatchNumber").Value = 0 
	rs_Thatch.fields("ThatchDetail").Value = "No"
	rs_Thatch.Update
	rs_Thatch.AddNew 
	rs_Thatch.fields("ThatchNumber").Value = 1 
	rs_Thatch.fields("ThatchDetail").Value = "Yes"
	rs_Thatch.Update
    rs_Thatch.MoveFirst

	set DataCombo_Thatch.RowSource = rs_Thatch
	DataCombo_Thatch.ListField = rs_Thatch.Fields("ThatchDetail").name
	DataCombo_Thatch.BoundColumn =  rs_Thatch.Fields("ThatchNumber").name
	DataCombo_Thatch.BoundText = rs_Thatch.Fields("ThatchNumber").Value
	DataCombo_Thatch.Refresh
 
 
     '*** Manually populate the owner occupired
    rs_Owner.Fields.Append "OwnerNumber",19
    rs_Owner.Fields.Append "OwnerDetail",200,180
    rs_Owner.Open

	rs_Owner.AddNew 
	rs_Owner.fields("OwnerNumber").Value = 0 
	rs_Owner.fields("OwnerDetail").Value = "No"
	rs_Owner.Update
	rs_Owner.AddNew 
	rs_Owner.fields("OwnerNumber").Value = 1 
	rs_Owner.fields("OwnerDetail").Value = "Yes"
	rs_Owner.Update
	rs_Owner.MoveFirst

	set DataCombo_Owner.RowSource = rs_Owner
	DataCombo_Owner.ListField = rs_Owner.Fields("OwnerDetail").name
	DataCombo_Owner.BoundColumn =  rs_Owner.Fields("OwnerNumber").name
	DataCombo_Owner.BoundText = rs_Owner.Fields("OwnerNumber").Value
	DataCombo_Owner.Refresh
	
	
	sSQL = "SELECT SPVNumber,SPVDescription FROM SPV where SPVLocked =0"
	rs_SPV.CursorLocation = 3
	rs_SPV.Open sSQL ,conn,adOpenStatic
	rs_SPV.MoveFirst
		
		
    

	set DataCombo_SPV.RowSource = rs_SPV
	DataCombo_SPV.ListField = rs_SPV.Fields("SPVDescription").name
	DataCombo_SPV.BoundColumn =  rs_SPV.Fields("SPVNumber").name
	DataCombo_SPV.BoundText = rs_SPV.Fields("SPVNumber").Value'
	DataCombo_SPV.Refresh

	
	sSQL = "SELECT * FROM EMPLOYEETEAM WHERE EmployeeTeamNumber NOT IN (6,8)"
	rs_Team.CursorLocation = 3
	rs_Team.Open sSQL ,conn,adOpenStatic

	set DataCombo_EmployeeTeam.RowSource = rs_Team
	DataCombo_EmployeeTeam.ListField = rs_Team.Fields("EmployeeTeamDescription").name
	DataCombo_EmployeeTeam.BoundColumn =  rs_Team.Fields("EmployeeTeamNumber").name
	DataCombo_EmployeeTeam.BoundText = rs_Team.Fields("EmployeeTeamNumber").Value
	DataCombo_EmployeeTeam.Refresh
	
	rs_Team.MoveFirst
	'rs_Team.Find "EmployeeTeamNumber = " & CSTR(i_EmployeeTeamNumber)
	'window.DataCombo_EmployeeTeam.BoundText = i_EmployeeTeamNumber
    'DataCombo_EmployeeTeam.Refresh
	
	
	'ADDED Status Flag filtering on 05/12/2001
	
	if rs_AdminOpen = true then
	   rs_Admin.Close
	end if
	
	'sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEMPLOYEE WHERE EmployeeTeamNumber = " & CStr(i_EmployeeTeamNumber)  & " AND (UserGroupNumber = 5 or UserGroupNumber = 20 or UserGroupNumber = 4) AND SAHLEmployeeStatusFlag <> 0 ORDER BY SAHLEmployeeName"
'	sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEMPLOYEE WHERE EmployeeTeamNumber = " & CStr(i_EmployeeTeamNumber)  & " AND (UserGroupNumber = 5 or UserGroupNumber = 20 or UserGroupNumber = 4) AND SAHLEmployeeStatusFlag <> 0 ORDER BY SAHLEmployeeName"
		sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEmployee WHERE EmployeeTeamNumber = " & window.DataCombo_EmployeeTeam.BoundText  & " AND (UserGroupNumber = 5 or UserGroupNumber = 20 or UserGroupNumber = 4) AND SAHLEmployeeStatusFlag <> 0 ORDER BY SAHLEmployeeName"

	rs_Admin.CursorLocation = 3
	rs_Admin.Open sSQL ,conn,adOpenStatic
    rs_AdminOpen = true
    
    
	'if s_Action = "Add" then
	'	rs_Admin.MoveFirst
	'	rs_Admin.Find "SAHLEmployeeNumber = 3"
	'end if
    
    
	set DataCombo_AdminEmployee.RowSource = rs_Admin
	DataCombo_AdminEmployee.ListField = rs_Admin.Fields("SAHLEmployeeName").name
	DataCombo_AdminEmployee.BoundColumn =  rs_Admin.Fields("SAHLEmployeeNumber").name
	DataCombo_AdminEmployee.BoundText = rs_Admin.Fields("SAHLEmployeeNumber").Value
	DataCombo_AdminEmployee.Refresh

	if s_Action = "Add" then
		rs_Admin.MoveFirst
		'rs_Admin.Find "SAHLEmployeeNumber = 3"
		
	else
		rs_Admin.MoveFirst
		rs_Admin.Find "SAHLEmployeeNumber = " & CStr(i_SAHLEmployeeNumber)
		window.DataCombo_AdminEmployee.BoundText = i_SAHLEmployeeNumber
	end if
    
    sSQL = "SELECT BrokerNumber,BrokerName FROM BROKER WHERE BrokerStatusNumber = 0 ORDER BY BrokerName"
    
	rs_Broker.CursorLocation = 3
	rs_Broker.Open sSQL ,conn,adOpenStatic
	rs_Broker.MoveFirst
    rs_Broker.Find "BrokerNumber = 1"
	set DataCombo_Broker.RowSource = rs_Broker
	DataCombo_Broker.ListField = rs_Broker.Fields("BrokerName").name
	DataCombo_Broker.BoundColumn =  rs_Broker.Fields("BrokerNumber").name
	DataCombo_Broker.BoundText = rs_Broker.Fields("BrokerNumber").Value
	DataCombo_Broker.Refresh
	
	sSQL = "SELECT * FROM PROSPECTSOURCE"
	rs_Source.CursorLocation = 3
	rs_Source.Open sSQL ,conn,adOpenStatic

	set DataCombo_Source.RowSource = rs_Source
	DataCombo_Source.ListField = rs_Source.Fields("ProspectSourceDescription").name
	DataCombo_Source.BoundColumn =  rs_Source.Fields("ProspectSourceNumber").name
	DataCombo_Source.BoundText = rs_Source.Fields("ProspectSourceNumber").Value
	DataCombo_Source.Refresh

	'ADDED Status Flag filtering on 05/12/2001

   ' if CInt(i_EmployeeType) <> 4 then
		sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEMPLOYEE WHERE UserGroupNumber = 6 AND SAHLEmployeeNumber = 4 AND SAHLEmployeeStatusFlag <> 0" 'Only select Unknown TeleConsultant (24/06/2001) - removed 04/07/2001 for TeleAdmin 
	'else
'	    sSQL = "SELECT SAHLEmployeeNumber,SAHLEmployeeName FROM SAHLEMPLOYEE WHERE UserGroupNumber = 6 AND SAHLEmployeeStatusFlag <> 0" ' AND SAHLEmployeeNumber = 4" 
'	end if 
	rs_Telesales.CursorLocation = 3
	rs_Telesales.Open sSQL ,conn,adOpenStatic

	set DataCombo_TeleSales.RowSource = rs_Telesales
	DataCombo_TeleSales.ListField = rs_Telesales.Fields("SAHLEmployeeName").name
	DataCombo_TeleSales.BoundColumn =  rs_Telesales.Fields("SAHLEmployeeNumber").name
	DataCombo_TeleSales.BoundText = rs_Telesales.Fields("SAHLEmployeeNumber").Value
	DataCombo_TeleSales.Refresh
	
    rs_Telesales.MoveFirst
    rs_Telesales.Find "SAHLEmployeeNumber = " & CStr(i_SAHLEmployeeNumber)
    window.DataCombo_TeleSales.BoundText = i_SAHLEmployeeNumber

    'if CInt(i_EmployeeType) = 4 then
   '    rs_Admin.MoveFirst
	'   rs_Admin.Find "SAHLEmployeeNumber = 12"
	'   window.DataCombo_AdminEmployee.BoundText = 12
	'   rs_Telesales.MoveFirst
	'   rs_Telesales.Find "SAHLEmployeeNumber = " & i_SAHLEmployeeNumber
 	'   window.DataCombo_TeleSales.BoundText = i_SAHLEmployeeNumber
	'else
	   rs_Telesales.MoveFirst
	   rs_Telesales.Find "SAHLEmployeeNumber = 4" 
 	   window.DataCombo_TeleSales.BoundText = 4
   ' end if
   
   
	
 End Sub



Sub EnableFields
    
    window.ProspectFirstNames.Enabled = True
	window.ProspectSurname.Enabled = True
	window.ProspectSalutation.Enabled = True
	window.ProspectIDNumber.Enabled = True
	window.ProspectIncome.Enabled = True
	window.ProspectSpouseIncome.Enabled = True
	window.ProspectSpouseFirstNames.Enabled = True
	window.ProspectSpouseIDNumber.Enabled = True
	window.DataCombo_Gender.Enabled = True
	DataCombo_Legal.Enabled = True
	window.DataCombo_EmploymentType.Enabled = True
	
	if s_Action <> "Add Further Loan" and CInt(window.DataCombo_Purpose.BoundText) <>  gi_FurtherLoan then 
		window.ProspectTermRequired.Enabled = True
    end if
	window.ProspectEstimatedPropertyValue.Enabled = True
    window.ProspectBondRequired.Enabled = True
	window.ProspectExistingLoan.Enabled = True
	window.ProspectCashRequired.Enabled = True
    window.ProspectCashDeposit.Enabled = True
	'window.ProspectLoanRequired.Enabled = True
	


    window.ProspectPropertyBuildingNumber.Enabled = True
    window.ProspectPropertyBuildingName.Enabled = True    
    window.ProspectPropertyStreetNumber.Enabled = True
    window.ProspectPropertyStreetName.Enabled = True
	
	window.ProspectPropertySuburb.Enabled = True
    window.ProspectPropertyCity.Enabled = True
    window.ProspectPropertyProvince.Enabled = True
    window.ProspectPropertyCountry.Enabled = True
    window.ProspectPropertyPostalCode.Enabled = true
    DataCombo_PropertyProvince.Enabled = True
	
 	DataCombo_Title.Enabled = True
	DataCombo_Property.Enabled = True
	DataCombo_Area.Enabled = True
	DataCombo_Thatch.Enabled = True
   
	DataCombo_Owner.Enabled = True
		
    window.ProspectContactName.Enabled = True
    window.ProspectContactNumber.Enabled = True
    
    DataCombo_ContactProvince.Enabled = True
	window.ProspectHomeBuildingNumber.Enabled = True
    window.ProspectHomeBuildingName.Enabled = True
    window.ProspectHomeStreetNumber.Enabled = True
    window.ProspectHomeStreetName.Enabled = True
	
	window.ProspectHomeSuburb.Enabled = True
    window.ProspectHomeCity.Enabled = True
    window.ProspectHomeProvince.Enabled = True
    window.ProspectHomeCountry.Enabled = True
    window.ProspectHomePostalCode.Enabled = True

    window.ProspectHomePhoneCode.Enabled = True
    window.ProspectHomePhoneNumber.Enabled = True
    window.ProspectWorkPhoneCode.Enabled = True
    window.ProspectWorkPhoneNumber.Enabled = True
    window.ProspectFaxPhoneCode.Enabled = True
    window.ProspectFaxPhoneNumber.Enabled = True
    window.ProspectCellPhoneNumber.Enabled = True
    window.ProspectEMailAddress.Enabled = True
    window.ProspectBoxNumber.Enabled = True
    window.ProspectPostOffice.Enabled = True
    window.ProspectPostalCode.Enabled = True

    window.ProspectSuretorName.Enabled = True
    window.ProspectSuretorIDNumber.Enabled = True
    window.ProspectTransferAttorney.Enabled = True
    window.ProspectCurrentStartDate.Enabled = True
    window.ProspectCurrentEmployer.Enabled = True
    window.ProspectPreviousStartDate.Enabled = True
    window.ProspectPreviousEmployer.Enabled = True
    
    DataCombo_SPV.Enabled = True
   ' if Cint(i_EmployeeType) <> gi_OriginationAdmin then
		DataCombo_EmployeeTeam.Enabled = True
		DataCombo_AdminEmployee.Enabled = True
	'end if
	DataCombo_Source.Enabled = True
	DataCombo_Broker.Enabled = True
	DataCombo_TeleSales.Enabled = True
	
	if s_Action <> "Add"  then 
		if Cint(rs_SelectedProspect2.Fields("PurposeNumber").Value) = gi_FurtherLoan then
		  ' window.DataCombo_Purpose.Enabled = false
		   window.ProspectExistingLoan.style.visibility = "hidden"
		   window.ProspectFactor.style.visibility = "hidden"
		   window.ProspectCashDeposit.style.visibility = "hidden"
		   window.chk_factor.style.visibility = "hidden"
		   
		   window.lbl_ExistingLoan.style.visibility = "hidden"
		   window.lbl_factor.style.visibility = "hidden"
		   window.lbl_CashDeposit.style.visibility = "hidden"
		   window.lbl_overridefactor.style.visibility = "hidden"
		   
		   
		   ' Changed on 03/10/2001 re : Bruce Pillay - Further Loans
		    'window.ProspectValuationFee.Enabled = true
			window.DataCombo_LinkRate.Enabled = true
			window.DataCombo_SPV.Enabled = true
		    
		else
		   window.DataCombo_Purpose.Enabled = True
		   window.DataCombo_LinkRate.Enabled = True
		end if
	else 
	    window.DataCombo_Purpose.Enabled = True
	    window.DataCombo_LinkRate.Enabled = True
	    window.DataCombo_SPV.Enabled = true
	end if
    
	
	
	
End sub


Sub SetUpFurtherLoanFields
       
    window.ProspectTermRequired.Enabled = false
    window.lbl_Term.innerText = "Term (Months)"
	window.ProspectEstimatedPropertyValue.Enabled = false
	window.lbl_EstimatedPropertyValue.innerText = "Last Prop. Valuation"
    window.ProspectBondRequired.Enabled = True
	window.ProspectExistingLoan.Enabled = false
	window.ProspectCashRequired.Enabled = True
    window.ProspectCashDeposit.Enabled = false
    window.chk_foreign.disabled = false
    window.btn_UsePropertyAddress.disabled = false
    
   ' window.ProspectValuationFee.Enabled = true
    window.DataCombo_LinkRate.Enabled = true
    window.DataCombo_SPV.Enabled = true
	'window.ProspectLoanRequired.Enabled = True
	
	window.ProspectExistingLoan.style.visibility = "hidden"
	window.ProspectFactor.style.visibility = "hidden"
	window.ProspectCashDeposit.style.visibility = "hidden"
	window.chk_factor.style.visibility = "hidden"
	
	
	window.lbl_ExistingLoan.style.visibility = "hidden"
	window.lbl_factor.style.visibility = "hidden"
	window.lbl_CashDeposit.style.visibility = "hidden"
	window.lbl_overridefactor.style.visibility = "hidden"
	
	 window.ProspectPropertyBuildingNumber.Enabled = True
    window.ProspectPropertyBuildingName.Enabled = True    
    window.ProspectPropertyStreetNumber.Enabled = True
    window.ProspectPropertyStreetName.Enabled = True
	
	window.ProspectPropertySuburb.Enabled = True
    window.ProspectPropertyCity.Enabled = True
    window.ProspectPropertyProvince.Enabled = True
    window.ProspectPropertyCountry.Enabled = True
    window.ProspectPropertyPostalCode.Enabled = true
    DataCombo_PropertyProvince.Enabled = True
	
 	DataCombo_Title.Enabled = True
	DataCombo_Property.Enabled = True
	DataCombo_Area.Enabled = True
	DataCombo_Thatch.Enabled = True
   
	DataCombo_Owner.Enabled = True
	DataCombo_EmploymentType.Enabled = True
		
    window.ProspectContactName.Enabled = True
    window.ProspectContactNumber.Enabled = True
    
    DataCombo_ContactProvince.Enabled = True
	window.ProspectHomeBuildingNumber.Enabled = True
    window.ProspectHomeBuildingName.Enabled = True
    window.ProspectHomeStreetNumber.Enabled = True
    window.ProspectHomeStreetName.Enabled = True
	
	window.ProspectHomeSuburb.Enabled = True
    window.ProspectHomeCity.Enabled = True
    window.ProspectHomeProvince.Enabled = True
    window.ProspectHomeCountry.Enabled = True
    window.ProspectHomePostalCode.Enabled = True

    window.ProspectHomePhoneCode.Enabled = True
    window.ProspectHomePhoneNumber.Enabled = True
    window.ProspectWorkPhoneCode.Enabled = True
    window.ProspectWorkPhoneNumber.Enabled = True
    window.ProspectFaxPhoneCode.Enabled = True
    window.ProspectFaxPhoneNumber.Enabled = True
    window.ProspectCellPhoneNumber.Enabled = True
    window.ProspectEMailAddress.Enabled = True
    window.ProspectBoxNumber.Enabled = True
    window.ProspectPostOffice.Enabled = True
    window.ProspectPostalCode.Enabled = True
	
    'if Cint(i_EmployeeType) <> gi_OriginationAdmin then
		DataCombo_EmployeeTeam.Enabled = True
		DataCombo_AdminEmployee.Enabled = True
'	end if
    ' SPV cannot be changed from the original laon
    
    

	
End sub

Sub DisableFields
    
    window.ProspectFirstNames.Enabled = False
	window.ProspectSurname.Enabled = False
	window.ProspectSalutation.Enabled = False
	window.ProspectIDNumber.Enabled = False
	window.ProspectIncome.Enabled = False
	window.ProspectSpouseIncome.Enabled = False
	window.ProspectSpouseFirstNames.Enabled = False
	window.ProspectSpouseIDNumber.Enabled = False
	DataCombo_Gender.Enabled = False
	DataCombo_Legal.Enabled = False
	window.DataCombo_EmploymentType.Enabled = False
	
	DataCombo_Purpose.Enabled = False
	window.DataCombo_LinkRate.Enabled = False

    window.ProspectTermRequired.Enabled = False
	window.ProspectEstimatedPropertyValue.Enabled = False
    window.ProspectBondRequired.Enabled = False
	window.ProspectExistingLoan.Enabled = False
	window.ProspectCashRequired.Enabled = False
    window.ProspectCashDeposit.Enabled = False
	window.ProspectLoanRequired.Enabled = False
	


    window.ProspectPropertyBuildingNumber.Enabled = False
    window.ProspectPropertyBuildingName.Enabled = False    
    window.ProspectPropertyStreetNumber.Enabled = False
    window.ProspectPropertyStreetName.Enabled = False
	
	window.ProspectPropertySuburb.Enabled = False
    window.ProspectPropertyCity.Enabled = False
    window.ProspectPropertyProvince.Enabled = False
    window.ProspectPropertyCountry.Enabled = False
    window.ProspectPropertyPostalCode.Enabled = False
    DataCombo_PropertyProvince.Enabled = False
	
 	DataCombo_Title.Enabled = False
	DataCombo_Property.Enabled = False
	DataCombo_Area.Enabled = False
	DataCombo_Thatch.Enabled = False
   
	DataCombo_Owner.Enabled = False

    window.ProspectContactName.Enabled = False
    window.ProspectContactNumber.Enabled = False
    
    DataCombo_ContactProvince.Enabled = False
	window.ProspectHomeBuildingNumber.Enabled = False
    window.ProspectHomeBuildingName.Enabled = False
    window.ProspectHomeStreetNumber.Enabled = False
    window.ProspectHomeStreetName.Enabled = False
	
	window.ProspectHomeSuburb.Enabled = False
    window.ProspectHomeCity.Enabled = False
    window.ProspectHomeProvince.Enabled = False
    window.ProspectHomeCountry.Enabled = False
    window.ProspectHomePostalCode.Enabled = False

    window.ProspectHomePhoneCode.Enabled = False
    window.ProspectHomePhoneNumber.Enabled = False
    window.ProspectWorkPhoneCode.Enabled = False
    window.ProspectWorkPhoneNumber.Enabled = False
    window.ProspectFaxPhoneCode.Enabled = False
    window.ProspectFaxPhoneNumber.Enabled = False
    window.ProspectCellPhoneNumber.Enabled = False
    window.ProspectEMailAddress.Enabled = False
    window.ProspectBoxNumber.Enabled = False
    window.ProspectPostOffice.Enabled = False
    window.ProspectPostalCode.Enabled = False

    window.ProspectSuretorName.Enabled = False
    window.ProspectSuretorIDNumber.Enabled = False
    window.ProspectTransferAttorney.Enabled = False
    window.ProspectCurrentStartDate.Enabled = False
    window.ProspectCurrentEmployer.Enabled = False
    window.ProspectPreviousStartDate.Enabled = False
    window.ProspectPreviousEmployer.Enabled = False
    
    DataCombo_SPV.Enabled = False
	DataCombo_EmployeeTeam.Enabled = False
	DataCombo_AdminEmployee.Enabled = False
	DataCombo_Source.Enabled = False
	DataCombo_Broker.Enabled = False
	DataCombo_TeleSales.Enabled = False
	
End Sub





Function ValidateFields()

ValidateFields = -1



'Check for possible duplicate
	set conn = createobject("ADODB.Connection")
	set rs_temp1 = createobject("ADODB.Recordset")
	sDSN = GetConnectionString("[ManageProspect.asp 2]")
	conn.CursorLocation = 1
	conn.Open sDSN	

	sSQL = "p_CheckForDuplicateProspects " &  window.ProspectNumber.Value & ",'" & window.ProspectIDNumber.Text & "','" &  _
			window.ProspectPropertyStreetNumber.Text & "','" & window.ProspectPropertyStreetName.Text & "','" &  _
			window.ProspectPropertyBuildingNumber.Text & "','" & window.ProspectPropertyBuildingName.Text & "','" &  _
			window.ProspectPropertySuburb.Text & "','" & window.ProspectPropertyCity.Text & "'"

	rs_Temp1.CursorLocation = 3
	
	
	'msgbox sSQL

	rs_Temp1.Open sSQL ,conn,adOpenStatic

	if rs_Temp1.RecordCount > 0 then
	   
	   
	  i_Resp = MsgBox("WARNING : A prospect already exists with this ID Number and Property Details.." & vbCRLF & vbCRLF & "Prospect Number : " & rs_temp1.Fields(0).Value & vbCRLF & "First Name : " & rs_temp1.fields(1).value & vbCRLF & "Surname : " & rs_temp1.fields(2).value & vbCRLF & vbCRLF & "Do you want to continue.....??" , 4 , "                                   ******* WARNING *******")
      if i_Resp= 7 then 
         rs_Temp1.close  
        exit function
	  end if
	 end if
	 rs_Temp1.close




window.lbl_BasicMessage.innerText = "Basic Details"
window.lbl_BasicMessage.style.backgroundcolor = ""
window.lbl_LoanMessage.innerText = "Loan Details"
window.lbl_LoanMessage.style.backgroundcolor = ""
window.lbl_PropertyMessage.innerText = "Property Details"
window.lbl_PropertyMessage.style.backgroundcolor = ""
window.lbl_ContactMessage.innerText = "Contact Details"
window.lbl_ContactMessage.style.backgroundcolor = ""
window.lbl_SAHLAdminMessage.innerText = "SAHL Administration Details"
window.lbl_SAHLAdminMessage.style.backgroundcolor = ""

if window.DataCombo_Gender.Text = "Unknown" then
   window.lbl_BasicMessage.innerText = "Gender cannot be Unknown...!!"
   window.lbl_BasicMessage.style.backgroundcolor = "Red"
   window.DataCombo_Gender.Enabled = true
   window.DataCombo_Gender.focus()
   ValidateFields = -1
   exit function
end if

if window.DataCombo_Legal.Text = "Unknown" then
   window.lbl_BasicMessage.innerText = "Legal Status cannot be Unknown...!!"
   window.lbl_BasicMessage.style.backgroundcolor = "Red"
   window.DataCombo_Gender.Enabled = true
   window.DataCombo_Gender.focus()
   ValidateFields = -1
   exit function
end if

if Cint(window.DataCombo_Gender.BoundText) <> gi_LegalEntity then
	if RTrim(window.ProspectFirstNames.Text) = "" then
	   window.lbl_BasicMessage.innerText = "First Names is a required field...!!"
	   window.lbl_BasicMessage.style.backgroundcolor = "Red"
	   window.ProspectFirstNames.Enabled = true
	   window.ProspectFirstNames.focus()
	    ValidateFields = -1
		exit function
	end if
end if

if RTrim(window.ProspectSurname.Text) = "" then
   window.lbl_BasicMessage.innerText = "Surname is a required field...!!"
   window.lbl_BasicMessage.style.backgroundcolor = "Red"
   window.ProspectSurname.Enabled = true
   window.ProspectSurname.focus()
   ValidateFields = -1
   exit function
end if

if Cint(window.DataCombo_Gender.BoundText) <> gi_LegalEntity then
	if RTrim(window.ProspectSalutation.Text) = "" then
	   window.lbl_BasicMessage.innerText = "Salutation is a required field...!!"
	   window.lbl_BasicMessage.style.backgroundcolor = "Red"
	   window.ProspectSalutation.Enabled = true
	   window.ProspectSalutation.focus()
	   ValidateFields = -1
	   exit function
	end if
end if

if RTrim(window.ProspectIDNumber.Text) = "" then
   window.lbl_BasicMessage.innerText = "ID Number is a required field...!!"
   window.lbl_BasicMessage.style.backgroundcolor = "Red"
   window.ProspectIDNumber.Enabled = true
   window.ProspectIDNumber.focus()
   ValidateFields = -1
   exit function
end if

if window.ProspectIncome.Value <= 0.00 then
  if window.ProspectSpouseIncome.Value <= 0.00 then
	window.lbl_BasicMessage.innerText = "Income is a required field...!!"
	window.lbl_BasicMessage.style.backgroundcolor = "Red"
	window.ProspectIncome.Enabled = true
	window.ProspectIncome.focus()
	ValidateFields = -1
	exit function
   End if
end if

'if ANC and male or female then spouse info no complusory
if Cint(window.DataCombo_Legal.BoundText) <> 4 and (Cint(window.DataCombo_Gender.BoundText) <> 2 or Cint(window.DataCombo_Gender.BoundText) <> 3) then
	if Cint(window.DataCombo_Legal.BoundText) = 3 or Cint(window.DataCombo_Legal.BoundText)= 4  then 'Married
		if RTrim(window.ProspectSpouseFirstNames.Text) = "" then
	   		window.lbl_BasicMessage.innerText = "Spouse first names and ID Number are required when legal status is married...!!"
	   		window.lbl_BasicMessage.style.backgroundColor = "Red"
	   		window.ProspectSpouseFirstNames.Enabled = true
	   		window.ProspectSpouseFirstNames.focus()
	   		ValidateFields = -1
	   		exit function
		end if
		if RTrim(window.ProspectSpouseIDNumber.Text) = "" then
	   		window.lbl_BasicMessage.innerText = "Spouse ID Number is required when legal status is married...!!"
	   		window.lbl_BasicMessage.style.backgroundColor = "Red"
	   		window.ProspectSpouseIDNumber.Enabled = true
	   		window.ProspectSpouseIDNumber.focus()
	   		ValidateFields = -1
	   		exit function
		end if
	end if
end if

if window.DataCombo_EmploymentType.Text = "Unknown" then
   window.lbl_BasicMessage.innerText = "Employment Type cannot be Unknown...!!"
   window.lbl_BasicMessage.style.backgroundcolor = "Red"
   window.DataCombo_EmploymentType.Enabled = true
   window.DataCombo_EmploymentType.focus()
   ValidateFields = -1
   exit function
end if

if window.DataCombo_Purpose.Text = "Unknown" then
   window.btn_LoanDetails.click
 
   window.lbl_LoanMessage.innerText = "Loan Purpose cannot be Unknown...!!"
   window.lbl_LoanMessage.style.backgroundColor = "Red"
   window.DataCombo_Purpose.Enabled = true
   window.DataCombo_Purpose.focus()
   ValidateFields = -1
   exit function
end if

if window.ProspectTermRequired.Value <= 0 then
   window.btn_LoanDetails.click
   window.lbl_LoanMessage.innerText = "Term is a required field...!!"
   window.lbl_LoanMessage.style.backgroundColor = "Red"
   window.ProspectTermRequired.Enabled = true
   window.ProspectTermRequired.focus()
   ValidateFields = -1
   exit function
end if

if s_Action = "Add Further Loan" or CInt(window.DataCombo_Purpose.BoundText) =  gi_FurtherLoan then 
	if window.ProspectTermRequired.Value > 240 then
	   window.btn_LoanDetails.click
	   window.lbl_LoanMessage.innerText = "Term cannot exceed 240 months...!!"
	   window.lbl_LoanMessage.style.backgroundcolor = "Red"
	   window.ProspectTermRequired.Enabled = true
	   window.ProspectTermRequired.focus()
	   ValidateFields = -1
	   exit function
	end if
else
	if window.ProspectTermRequired.Value > 20 then
	   window.btn_LoanDetails.click
	   window.lbl_LoanMessage.innerText = "Term cannot exceed 20 years...!!"
	   window.lbl_LoanMessage.style.backgroundcolor = "Red"
	   window.ProspectTermRequired.Enabled = true
	   window.ProspectTermRequired.focus()
	   ValidateFields = -1
	   exit function
	end if
end if

if window.ProspectEstimatedPropertyValue.Value <= 0 then
   window.btn_LoanDetails.click
   window.lbl_LoanMessage.innerText = "Estimated Property Value is a required field...!!"
   window.lbl_LoanMessage.style.backgroundcolor = "Red"
   window.ProspectEstimatedPropertyValue.Enabled = true
   window.ProspectEstimatedPropertyValue.focus()
   ValidateFields = -1
   exit function
end if

if window.ProspectEstimatedPropertyValue.Value <= 10000 then
   window.btn_LoanDetails.click
   window.lbl_LoanMessage.innerText = "THE PROPERTY ESTIMATED VALUE....not some thumbsuck value...!!"
   window.lbl_LoanMessage.style.backgroundcolor = "Red"
   window.ProspectEstimatedPropertyValue.Enabled = true
   window.ProspectEstimatedPropertyValue.focus()
   ValidateFields = -1
   exit function
end if

if Cint(window.DataCombo_Purpose.BoundText) = gi_SwitchLoan  then
	if window.ProspectExistingLoan.Value <= 0 then
	   window.btn_LoanDetails.click
	   window.lbl_LoanMessage.innerText = "Existing Loan is a required field...!!"
	   window.lbl_LoanMessage.style.backgroundcolor = "Red"
	   window.ProspectExistingLoan.Enabled = true
	   window.ProspectExistingLoan.focus()
	   ValidateFields = -1
	   exit function
	end if
end if

if Cint(window.DataCombo_Purpose.BoundText) = gi_NewPurchase then
	if window.ProspectExistingLoan.Value <= 0 then
	   window.btn_LoanDetails.click
	   window.lbl_LoanMessage.innerText = "Purchase Price is a required field...!!"
	   window.lbl_LoanMessage.style.backgroundcolor = "Red"
	   window.ProspectExistingLoan.Enabled = true
	   window.ProspectExistingLoan.focus()
	   ValidateFields = -1
	   exit function
	end if
end if

if Cint(window.DataCombo_Purpose.BoundText) = gi_Refinance or Cint(window.DataCombo_Purpose.BoundText) = gi_FurtherLoan then
	if window.ProspectCashRequired.Value <= 0 then
	   window.btn_LoanDetails.click
	   window.lbl_LoanMessage.innerText = "Cash Required is a required field...!!"
	   window.lbl_LoanMessage.style.backgroundcolor = "Red"
	   window.ProspectCashRequired.Enabled = true
	   window.ProspectCashRequired.focus()
	   ValidateFields = -1
	   exit function
	end if
end if

if RTrim(window.ProspectPropertyBuildingNumber.Text) = "" then
	if RTrim(window.ProspectPropertyStreetNumber.Text) = "" then
	   window.btn_PropertyDetails.click
	   window.lbl_PropertyMessage.innerText = "Property Street Number is a required field...!!"
	   window.lbl_PropertyMessage.style.backgroundcolor = "Red"
	   window.ProspectPropertyStreetNumber.Enabled = true
	   window.ProspectPropertyStreetNumber.focus()
	   ValidateFields = -1
	   exit function
	end if
	
end if


if RTrim(window.ProspectPropertyBuildingName.Text) = "" then
	if RTrim(window.ProspectPropertyStreetName.Text) = "" then
	   window.btn_PropertyDetails.click
	   window.lbl_PropertyMessage.innerText = "Property Street Name is a required field...!!"
	   window.lbl_PropertyMessage.style.backgroundcolor = "Red"
	   window.ProspectPropertyStreetName.Enabled = true
	   window.ProspectPropertyStreetName.focus()
	   ValidateFields = -1
	   exit function
	end if
end if

if RTrim(window.ProspectPropertyCity.Text) = "" then
   window.btn_PropertyDetails.click
   window.lbl_PropertyMessage.innerText = "Property City is a required field...!!"
   window.lbl_PropertyMessage.style.backgroundcolor = "Red"
   window.ProspectPropertyCity.Enabled = true
   window.ProspectPropertyCity.focus()
   ValidateFields = -1
   exit function
end if

if RTrim(window.ProspectPropertyPostalCode.Text) = "" then
   window.btn_PropertyDetails.click
   window.lbl_PropertyMessage.innerText = "Property Postal Code is a required field...!!"
   window.lbl_PropertyMessage.style.backgroundcolor = "Red"
   window.ProspectPropertyPostalCode.Enabled = true
   window.ProspectPropertyPostalCode.focus()
   ValidateFields = -1
   exit function
end if

if window.DataCombo_Title.Text = "Unknown" then
   window.btn_PropertyDetails.click
   window.lbl_PropertyMessage.innerText = "Title Type cannot be Unknown...!!"
   window.lbl_PropertyMessage.style.backgroundcolor = "Red"
   window.DataCombo_Title.Enabled = true
   window.DataCombo_Title.focus()
   ValidateFields = -1
   exit function
end if

if window.DataCombo_Property.Text = "Unknown" then
   window.btn_PropertyDetails.click
   window.lbl_PropertyMessage.innerText = "Property Type cannot be Unknown...!!"
   window.lbl_PropertyMessage.style.backgroundcolor = "Red"
   window.DataCombo_Property.Enabled = true
   window.DataCombo_Property.focus()
   ValidateFields = -1
   exit function
end if

if CInt(window.DataCombo_Gender.BoundText) =  gi_LegalEntity then
	if RTrim(window.ProspectSuretorName.Text) = "" then
	   window.btn_SAHLDetails.click
	   window.lbl_SAHLAdminMessage.innerText = "Suretor Name is a required field for Joint Ownership...!!"
	   window.lbl_SAHLAdminMessage.style.backgroundcolor = "Red"
	   window.ProspectSuretorName.Enabled = true
	   window.ProspectSuretorName.focus()
	   ValidateFields = -1
	   exit function
	end if
	if RTrim(window.ProspectSuretorIDNumber.Text) = "" then
	   window.btn_SAHLDetails.click
	   window.lbl_SAHLAdminMessage.innerText = "Suretor ID is a required field for Joint Ownership...!!"
	   window.lbl_SAHLAdminMessage.style.backgroundcolor = "Red"
	   window.ProspectSuretorIDNumber.Enabled = true
	   window.ProspectSuretorIDNumber.focus()
	   ValidateFields = -1
	   exit function
	end if
end if


if Trim(window.DataCombo_EmployeeTeam.text) =  "" or Trim(window.DataCombo_EmployeeTeam.text) =  "Unknown" then
 
     window.btn_SAHLDetails.click
     window.lbl_SAHLAdminMessage.innerText = "Employee Team cannot be unknown or empty...!!"
      window.lbl_SAHLAdminMessage.style.backgroundcolor = "Red"
	   window.DataCombo_EmployeeTeam.Enabled = true
	   window.DataCombo_EmployeeTeam.focus()
	   ValidateFields = -1
	   exit function

end if


if Trim(window.DataCombo_AdminEmployee.text) =  "" or Trim(window.DataCombo_AdminEmployee.text) =  "Unknown" then
 
     window.btn_SAHLDetails.click
     window.lbl_SAHLAdminMessage.innerText = "Admin Employee cannot be unknown or empty...!!"
      window.lbl_SAHLAdminMessage.style.backgroundcolor = "Red"
	   window.DataCombo_AdminEmployee.Enabled = true
	   window.DataCombo_AdminEmployee.focus()
	   ValidateFields = -1
	   exit function

end if


if Trim(window.DataCombo_TeleSales.text) =  ""  then
 
     window.btn_SAHLDetails.click
     window.lbl_SAHLAdminMessage.innerText = "Telesales Employee cannot be empty...!!"
      window.lbl_SAHLAdminMessage.style.backgroundcolor = "Red"
	   window.DataCombo_TeleSales.Enabled = true
	   window.DataCombo_TeleSales.focus()
	   ValidateFields = -1
	   exit function

end if



ValidateFields = 0

End Function

Sub btn_Mode_onclick
 
 if btn_Mode.value = "Commit Add" or _
    btn_Mode.value = "Commit Copy" then
   
	if ValidateFields() = 0 then
	    if MaintainProspectRecord("Add") = 0 then
		   btn_Mode.value = "Commit Update"
		   btn_Exit.value = "Exit"
		  ' s_ProspectStage = 0   'return to unselected prospects....
	    End if
	end if
 elseif btn_Mode.value = "Add Further Loan" then
   
	if ValidateFields() = 0 then
	    if MaintainProspectRecord("Add Further Loan") = 0 then
		   btn_Mode.value = "Commit Update"
		   btn_Exit.value = "Exit"
		  ' s_ProspectStage = 0   'return to unselected prospects....
	    End if
	end if
 elseif btn_Mode.value = "Commit Update"  then   
	if ValidateFields() = 0 then
	    if MaintainProspectRecord("Update") = 0 then
		   btn_Mode.value = "Commit Update"
		   btn_Exit.value = "Exit"
		   
		  if CInt(window.DataCombo_Purpose.BoundText) <>  gi_FurtherLoan then
	    
			'   msgbox window.ProspectTermRequired.Value 
				i_Installment = CalculateInstallment(window.ProspectLoanRequired.Value,i_CurrentLoanRate,12,window.ProspectTermRequired.Value*12 ,0) 
				i_income= window.ProspectIncome.Value + window.ProspectSpouseIncome.Value

				window.lbl_BasicMessage.innerText =  "Basic Details (Installment - R " & CSTR(i_Installment) & " / PTI - " & CSTR(Round( (i_Installment/i_income) *100 ,2 )) &  " %)"

				window.lbl_LoanMessage.innerText = "Loan Details (LTV - " & CSTR(round(window.ProspectLoanRequired.Value/window.ProspectEstimatedPropertyValue * 100,2)) & ")"
			end if

	    End if
	end if
 end if
End Sub

Function MaintainProspectRecord(s_Action)

Dim i_res


MaintainProspectRecord = -1

    document.body.style.cursor = "hand"
    
    i_CurrentLoanRate = i_CurrentMarketRate + i_CurrentLinkRate
    
'msgbox window.ProspectLoanRequired.Value
'msgbox i_CurrentLoanRate
  
    if s_Action = "Add Further Loan" or CInt(window.DataCombo_Purpose.BoundText) =  gi_FurtherLoan then 
        i_InstallmentToIncome = CalculateInstallment(window.ProspectLoanRequired.Value,i_CurrentLoanRate,12,window.ProspectTermRequired.Value ,0) / (window.ProspectIncome + window.ProspectSpouseIncome)
    else
		i_InstallmentToIncome = CalculateInstallment(window.ProspectLoanRequired.Value,i_CurrentLoanRate,12,window.ProspectTermRequired.Value * 12,0) / (window.ProspectIncome + window.ProspectSpouseIncome)
	end if
    i_res = 0     
    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")
    set rs_temp1 = createobject("ADODB.Recordset")
    
    'Cannot use OLE DB Provider as it appears that it does not return a recordset

    'sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=MLS System Version1 [manageprospect.asp];uid=<%= Session("UserID")%>"
    sDSN = GetConnectionString("[ManageProspect.asp 3]")
	
	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc
	
	if s_Action = "Add" or s_Action = "Add Further Loan" then
		sSQL = "p_AddProspectRecord"  
	elseif s_Action = "Update"  then
		sSQL = "p_UpdProspectRecord"  	
	end if
	
	com.CommandText = sSQL
	
	set prm = com.CreateParameter ( "SexNumber",19,1,,CInt(window.DataCombo_Gender.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "LegalStatusNumber",19,1,,CInt(window.DataCombo_Legal.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectFirstNames",200,1,50,window.ProspectFirstNames.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ProspectSurname",200,1,50,window.ProspectSurname.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ProspectSalutation",200,1,15,window.ProspectSalutation.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ProspectIDNumber",200,1,20,window.ProspectIDNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ProspectIncome",5,1,,window.ProspectIncome.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ProspectSpouseIncome",5,1,,window.ProspectSpouseIncome.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "ProspectSpouseFirstNames",200,1,50,window.ProspectSpouseFirstNames.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ProspectSpouseIDNumber",200,1,20,window.ProspectSpouseIDNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "EmploymentTypeNumber",19,1,,CInt(window.DataCombo_EmploymentType.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm
	
'    msgbox CInt(window.DataCombo_Purpose.BoundText)
	set prm = com.CreateParameter ( "PurposeNumber",19,1,,CInt(window.DataCombo_Purpose.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

    if s_Action = "Add Further Loan" or CInt(window.DataCombo_Purpose.BoundText) =  gi_FurtherLoan then 
		set prm = com.CreateParameter ( "ProspectTermRequired",19,1,,window.ProspectTermRequired.Value) 'AdVarchar , adParamInput
	else
		set prm = com.CreateParameter ( "ProspectTermRequired",19,1,,window.ProspectTermRequired.Value * 12) 'AdVarchar , adParamInput
	end if
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectEstimatedPropertyValue",5,1,,window.ProspectEstimatedPropertyValue.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectBondRequired",5,1,,window.ProspectBondRequired.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectExistingLoan",5,1,,window.ProspectExistingLoan.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectFactor",5,1,,window.ProspectFactor.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectCashRequired",5,1,,window.ProspectCashRequired.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectCashDeposit",5,1,,window.ProspectCashDeposit.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectLoan",5,1,,window.ProspectLoanRequired.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectBond",5,1,,window.ProspectBondToRegister.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectCancelFee",5,1,,window.ProspectCancelFees.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectTransferFee",5,1,,window.ProspectTransferFees.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectRegistrationFee",5,1,,window.ProspectRegistrationFees.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectAdminFee",5,1,,window.ProspectSAHLFees.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm
 
    set prm = com.CreateParameter ( "ProspectValuationFee",5,1,,window.ProspectValuationFee.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectPropertyBuildingNumber",200,1,10,window.ProspectPropertyBuildingNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ProspectPropertyBuildingName",200,1,50,window.ProspectPropertyBuildingName.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ProspectPropertyStreetNumber",200,1,10,window.ProspectPropertyStreetNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ProspectPropertyStreetName",200,1,50,window.ProspectPropertyStreetName.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ProspectPropertySuburb",200,1,50,window.ProspectPropertySuburb.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ProspectPropertyCity",200,1,50,window.ProspectPropertyCity.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectPropertyProvince",200,1,50,window.DataCombo_PropertyProvince.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectPropertyCountry",200,1,50,window.ProspectPropertyCountry.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectPropertyPostalCode",200,1,10,window.ProspectPropertyPostalCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "TitleTypeNumber",19,1,,CInt(window.DataCombo_Title.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "PropertyTypeNumber",19,1,,CInt(window.DataCombo_Property.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "AreaClassificationNumber",19,1,,CInt(window.DataCombo_Area.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectOwnerOccupied",19,1,,Cint(window.DataCombo_Owner.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectPropertyThatch",19,1,,CInt(window.DataCombo_Thatch.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectContactName",200,1,50,window.ProspectContactName.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectContactNumber",200,1,15,window.ProspectContactNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectHomeBuildingNumber",200,1,10,window.ProspectHomeBuildingNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ProspectHomeBuildingName",200,1,50,window.ProspectHomeBuildingName.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	
	
	set prm = com.CreateParameter ( "ProspectHomeStreetNumber",200,1,10,window.ProspectHomeStreetNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ProspectHomeStreetName",200,1,50,window.ProspectHomeStreetName.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ProspectHomeSuburb",200,1,50,window.ProspectHomeSuburb.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm	

	set prm = com.CreateParameter ( "ProspectHomeCity",200,1,50,window.ProspectHomeCity.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm
    
    if window.chk_foreign.checked = true then
        set prm = com.CreateParameter ( "ProspectHomeProvince",200,1,50,window.ProspectHomeProvince.Text)
    else
		set prm = com.CreateParameter ( "ProspectHomeProvince",200,1,50,window.DataCombo_ContactProvince.Text) 'AdVarchar , adParamInput
	end if
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectHomeCountry",200,1,50,window.ProspectHomeCountry.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectHomePostalCode",200,1,10,window.ProspectHomePostalCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "ProspectHomePhoneCode",200,1,10,window.ProspectHomePhoneCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectHomePhoneNumber",200,1,15,window.ProspectHomePhoneNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectWorkPhoneCode",200,1,10,window.ProspectWorkPhoneCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectWorkPhoneNumber",200,1,15,window.ProspectWorkPhoneNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectFaxPhoneCode",200,1,10,window.ProspectFaxPhoneCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectFaxPhoneNumber",200,1,15,window.ProspectFaxPhoneNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectCellularPhone",200,1,15,window.ProspectCellPhoneNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectEMailAddress",200,1,50,window.ProspectEMailAddress.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectBoxNumber",200,1,15,window.ProspectBoxNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
    set prm = com.CreateParameter ( "ProspectPostOffice",200,1,50,window.ProspectPostOffice.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "ProspectPostalCode",200,1,15,window.ProspectPostalCode.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm
    
    set prm = com.CreateParameter ( "ProspectSuretorName",200,1,50,window.ProspectSuretorName.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "ProspectSuretorIDNumber",200,1,20,window.ProspectSuretorIDNumber.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "ProspectTransferAttorney",200,1,50,window.ProspectTransferAttorney.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	
 	if trim(window.ProspectCurrentStartDate.Text) = "__/__/____" then
	    's_date = NULL
	    s_date = "01/01/1900"
	else
	   s_date = Mid(window.ProspectCurrentStartDate.Text, 4, 2) & "/" & Mid(window.ProspectCurrentStartDate.Text, 1, 2) & "/" & Mid(window.ProspectCurrentStartDate.Text, 7, 4)
	end if
    set prm = com.CreateParameter ( "ProspectCurrentStartDate",135,1,,s_date) 'AdVarchar , adParamInput
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "ProspectCurrentEmployer",200,1,50,window.ProspectCurrentEmployer.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

    if trim(window.ProspectPreviousStartDate.Text) = "__/__/____" then
	    's_date = NULL
	    s_date = "01/01/1900"
	else
	   s_date = Mid(window.ProspectPreviousStartDate.Text, 4, 2) & "/" & Mid(window.ProspectPreviousStartDate.Text, 1, 2) & "/" & Mid(window.ProspectPreviousStartDate.Text, 7, 4)
	end if
	
    set prm = com.CreateParameter ( "ProspectPreviousStartDate",135,1,,s_date) 'AdVarchar , adParamInput
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "ProspectPreviousEmployer",200,1,50,window.ProspectPreviousEmployer.Text) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "SPVNumber",19,1,,CInt(window.DataCombo_SPV.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "EmployeeTeamNumber",19,1,,CInt(window.DataCombo_EmployeeTeam.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "SAHLEmployeeNumber1",19,1,,CInt(window.DataCombo_AdminEmployee.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ProspectSourceNumber",19,1,,CInt(window.DataCombo_Source.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "SAHLEmployeeNumber",19,1,,CInt(window.DataCombo_TeleSales.BoundText)) ' Unknown TeleSales - (24/6/2001)
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "BrokerNumber",19,1,,CInt(window.DataCombo_Broker.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "LTV",5,1,,window.ProspectLoanRequired.Value/window.ProspectEstimatedPropertyValue) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "PTI",5,1,,i_InstallmentToIncome) ' AdUnsigned Int
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "FeesOverRide",19,1,,0) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "FactorOverRide",19,1,,0) ' AdUnsigned Int
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "LoanNumber",19,1,,i_LoanNumber) ' AdUnsigned Int
	com.Parameters.Append prm

    if s_Action = "Update" then
		set prm = com.CreateParameter ( "ProspectNumber",19,1,,window.ProspectNumber.Value)
	    com.Parameters.Append prm
	end if

    set prm = com.CreateParameter ( "MarketRateTypeNumber",19,1,,CInt(window.DataCombo_MarketRateType.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm
	
	set prm = com.CreateParameter ( "LinkRateType",5,1,3,CDbl(window.DataCombo_LinkRate.BoundText)) ' AdUnsigned Int
	com.Parameters.Append prm
    
    set rs_temp = com.Execute 

 
'    msgbox rs_Temp.Fields(0).Value
    
   

    set conn = createobject("ADODB.Connection")
    'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    'sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [manageprospect.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
    sDSN = GetConnectionString("[ManageProspect.asp 4]")
    conn.CursorLocation = 1
	conn.Open sDSN	


	

    sSQL = "SELECT ProspectNumber FROM PROSPECT WHERE ProspectNumber = " &  CStr(rs_Temp.Fields("ProspectNumber").Value)

   rs_Temp1.CursorLocation = 3

   rs_Temp1.Open sSQL ,conn,adOpenStatic

   if rs_Temp1.RecordCount > 0 then
      i_ProspectNumber =  rs_Temp1.Fields("ProspectNumber").Value
      window.ProspectNumber.Value =  rs_Temp1.Fields("ProspectNumber").Value
      if s_Action = "Add Further Loan" or s_Action = "Add" then 
        s_Action = "Update"
        window.lbl_ProspectNumber.style.visibility =  "visible"
        window.ProspectNumber.style.width = 100
        window.ProspectNumber.style.height = 24
      end if
  end if
  
  MaintainProspectRecord = 0
    
  document.body.style.cursor = "default"   
    
End Function

Sub window_onfocus

if b_CDone = false then 
	if s_Action = "Add"  then  'Add Prospect
		SetUpAddProspectData
		window.btn_Exit.value = "Cancel Add"
		window.btn_Exit.name = "btn_Exit"
		window.btn_Mode.value = "Commit Add"
		window.btn_Mode.name = "btn_Mode"
		EnableFields
		window.DataCombo_Gender.Enabled = true
		window.DataCombo_Gender.focus()
	elseif s_Action = "Update" or s_Action = "View"  then 
	  
		GetProspectBasicData

		GetProspectLoanData
	
		GetProspectPropertyData

		GetProspectContactData

		GetProspectSAHLAdminData
	    if s_Action = "Update" then
			window.btn_Exit.value = "Cancel Update"
			window.btn_Mode.value = "Commit Update"
	    elseif  s_Action = "View" then
	        window.btn_Exit.value = "Exit"
			window.btn_Mode.value = ""
		end if
		if s_Action = "Update" then
			EnableFields
			window.DataCombo_Gender.Enabled = true
			window.DataCombo_Gender.focus()
	    else
	        window.chk_foreign.disabled = true
		end if
		
		if Cint(rs_SelectedProspect2.Fields("PurposeNumber").Value) = gi_FurtherLoan then
	      'window.DataCombo_SPV.Enabled = false
	      ' Changed on 03/10/2001 re : Bruce Pillay - Further Loans
	    '  window.DataCombo_SPV.Enabled = true
	     end if
	     
	    if CInt(window.DataCombo_Purpose.BoundText) <>  gi_FurtherLoan then
	    
	 '   msgbox window.ProspectTermRequired.Value 
			i_Installment = CalculateInstallment(window.ProspectLoanRequired.Value,i_CurrentLoanRate,12,window.ProspectTermRequired.Value*12 ,0) 
			i_income= window.ProspectIncome.Value + window.ProspectSpouseIncome.Value

			window.lbl_BasicMessage.innerText =  "Basic Details (Installment - R " & CSTR(i_Installment) & " / PTI - " & CSTR(Round( (i_Installment/i_income) *100 ,2 )) &  " %)"

			window.lbl_LoanMessage.innerText = "Loan Details (LTV - " & CSTR(round(window.ProspectLoanRequired.Value/window.ProspectEstimatedPropertyValue * 100,2)) & ")"
		end if
	elseif s_Action = "Add Further Loan" then 
        
        window.lbl_ProspectNumber.style.visibility =  "hidden"
        window.ProspectNumber.style.width = 0
        window.ProspectNumber.style.height = 0
        
        SetUpFurtherLoanFields
        
		GetProspectBasicData

		GetProspectLoanData
		
		GetProspectPropertyData

		GetProspectContactData

		GetFurtherLoanSAHLAdminData
	
	    
		window.btn_Exit.value = "Cancel Further Loan"
		window.btn_Exit.name = "btn_Exit"
		window.btn_Mode.value = "Add Further Loan"
		window.btn_Mode.name = "btn_Mode"
		
		
		'window.ProspectTermRequired.Enabled = true
		'window.ProspectTermRequired.focus()
		window.ProspectBondRequired.Enabled = true
		window.ProspectBondRequired.focus()
		
    elseif s_Action = "Copy"  then 
        
		GetProspectBasicData
		GetProspectLoanData
		GetProspectPropertyData
		ClearPropertyFields
		GetProspectContactData
		GetProspectSAHLAdminData
		window.btn_Exit.value = "Cancel Copy"
		window.btn_Exit.name = "btn_Exit"
		window.btn_Mode.value = "Commit Copy"
		window.btn_Mode.value = "btn_Mode"
		EnableFields
		window.DataCombo_Gender.Enabled = true
		window.DataCombo_Gender.focus()
	end if
end if
 
 b_AllDataLoaded = true  
b_Loading1 = false
b_CDone = true

End Sub

Sub DataCombo_Source_Change

	if Cint(window.DataCombo_Source.BoundText) = 3 or Cint(window.DataCombo_Source.BoundText) = 8 then
	'   window.lbl_Broker.style.visibility = "visible"
    '       window.DataCombo_Broker.style.visibility = "visible"
	'   window.DataCombo_Broker.style.width = 200
	'   window.DataCombo_Broker.style.height = 27
	'   if s_Action <> "View" then
	'	  window.DataCombo_Broker.Enabled = true
	'   end if
	 else
	  '   window.lbl_Broker.style.visibility = "hidden"
      '       window.DataCombo_Broker.style.visibility = "hidden"
	 '    window.DataCombo_Broker.style.width = 0
	  '   window.DataCombo_Broker.style.height = 0
	 '    if s_Action <> "View" then
	'		window.DataCombo_Broker.enabled = true
	'	 end if
	
	  '  rs_broker.MoveFirst
	 '   rs_Broker.Find "BrokerNumber = 1"
	 '   DataCombo_Broker.BoundText = rs_Broker.Fields("BrokerNumber").Value
	 '   window.DataCombo_Broker.Refresh
	  
	 '  window.DataCombo_Broker.Enabled = false
	end if


End Sub

Sub DataCombo_ContactProvince_Change

window.ProspectHomeProvince.Text = DataCombo_ContactProvince.Text

End Sub

Function CalculateInstallment(ByVal d_Loan, ByVal d_PeriodRate, ByVal i_InterestPeriods , ByVal i_Term , ByVal i_Type) 
    'd_Loan  = loan amount
    'd_PeriodRate = interest rate for period as percentage
    'i_term = number of periods over which loan is to be repaid
    'i_Type = (0 = installment at end of period 1  = installment at beginning of period)

If i_Term = 0 Or d_PeriodRate <= 0.00 Then
   MsgBox "Application Error : Term and/or Rate is zero ... Check Values....!!!"
   CalculateInstallment = 0
   Exit Function
End If

If d_Loan <= 0 Then 
   MsgBox "Application Error : Loan Required cannot be zero  ... Check Loan Values....!!!"    
   Exit Function
End If

If i_Term > 360 Then 
   MsgBox "Application Error : Term exceeds permitted range ... Check Term Value....!!!"    
   Exit Function
End  If

	CalculateInstallment = Round((d_PeriodRate / i_InterestPeriods) * (d_Loan * (1 + d_PeriodRate / i_InterestPeriods) ^ i_Term) / ((1 + d_PeriodRate / i_InterestPeriods * i_Type) * (1 - (1 + d_PeriodRate / i_InterestPeriods) ^ i_Term)) * -1, 2)
	
	if CalculateInstallment < 0 then
	   CalculateInstallment = 0
	end if

End Function

Sub ClearPropertyFields

	window.ProspectPropertyBuildingNumber.text = ""
	window.ProspectPropertyBuildingName.text = ""
	window.ProspectPropertyStreetNumber.text = ""
	window.ProspectPropertyStreetName.text = ""
	window.ProspectPropertySuburb.text = ""
	window.ProspectPropertyCity.text = ""
	window.ProspectPropertyCountry.text = ""
	window.ProspectPropertyPostalCode.text = ""
	rs_Province.MoveFirst
	DataCombo_PropertyProvince.BoundText = rs_Province.Fields("ProvinceName").Value
	DataCombo_PropertyProvince.Refresh
	rs_Title.MoveFirst
	DataCombo_Title.BoundText = rs_Title.Fields("TitleTypeNumber").Value
	DataCombo_Title.Refresh
	rs_Property.MoveFirst
	DataCombo_Property.BoundText = rs_Property.Fields("PropertyTypeNumber").Value
	DataCombo_Property.Refresh
    rs_Area.MoveFirst
	DataCombo_Area.BoundText = rs_Area.Fields("AreaClassificationNumber").Value
	DataCombo_Area.Refresh
	rs_Thatch.MoveFirst
	DataCombo_Thatch.BoundText = rs_Thatch.Fields("ThatchNumber").Value
	DataCombo_Thatch.Refresh
	rs_Owner.MoveFirst
	DataCombo_Owner.BoundText = rs_Owner.Fields("OwnerNumber").Value
	DataCombo_Owner.Refresh
	window.ProspectContactName.text = ""
	window.ProspectContactNumber.text = ""
	
End sub


Sub btn_UsePropertyAddress_onclick

window.ProspectHomeBuildingNumber.Text = window.ProspectPropertyBuildingNumber.Text
window.ProspectHomeBuildingName.Text = window.ProspectPropertyBuildingName.Text
window.ProspectHomeStreetNumber.Text = window.ProspectPropertyStreetNumber.Text
window.ProspectHomeStreetName.Text = window.ProspectPropertyStreetName.Text
window.ProspectHomeSuburb.Text = window.ProspectPropertySuburb.Text
window.ProspectHomeCity.Text = window.ProspectPropertyCity.Text
window.ProspectHomeCountry.Text = window.ProspectPropertyCountry.Text
window.ProspectHomePostalCode.Text = window.ProspectPropertyPostalCode.Text

window.ProspectHomeProvince.Text = window.ProspectPropertyProvince.Text
rs_Province1.MoveFirst
'msgbox window.DataCombo_PropertyProvince.BoundText
rs_Province1.Find "ProvinceName = '" & window.DataCombo_PropertyProvince.BoundText & "'"
window.DataCombo_ContactProvince.BoundText = window.DataCombo_PropertyProvince.BoundText 

End Sub

Sub DataCombo_PropertyProvince_Change
	ProspectPropertyProvince.Text =  DataCombo_PropertyProvince.Text
End Sub




Sub btn_LoanDetails_onclick



 'if  b_LoanBtn = true then exit sub

  window.tbl_Property.style.postop = 1200
  window.tbl_Contact.style.postop = 1200
  window.tbl_SAHLDetails.style.postop = 1200
  window.tbl_TelecentreComment.style.postop = 1200
  
  window.tbl_LoanDetails.style.postop = 220
  

  window.btn_UsePropertyAddress.style.visibility = "hidden"
  window.btn_LoanDetails.style.BorderStyle = "groove"

  if window.DataCombo_Purpose.Enabled = true then 
      window.DataCombo_Purpose.focus
  elseif window.ProspectTermRequired.Enabled = true then
      window.ProspectTermRequired.focus
  end if
  
 
  
  b_LoanBtn = true
  b_Forward = true
End Sub


Sub btn_SAHLDetails_onclick

 window.tbl_LoanDetails.style.postop = 1200
 window.tbl_Property.style.postop = 1200
 window.tbl_Contact.style.postop = 1200
 window.tbl_TelecentreComment.style.postop = 1200

 window.tbl_SAHLDetails.style.postop = 220
  
 window.btn_UsePropertyAddress.style.visibility = "hidden"

 if s_Action = "Add"  then
	rs_Admin.MoveFirst
	DataCombo_AdminEmployee.BoundText = rs_Admin.Fields("SAHLEmployeeNumber").Value
	rs_Admin.MoveFirst
    'rs_Admin.Find "SAHLEmployeeNumber = " & CStr(i_SAHLEmployeeNumber)
    'window.DataCombo_AdminEmployee.BoundText = i_SAHLEmployeeNumber		
 else
    DataCombo_AdminEmployee.BoundText = rs_SelectedProspect5.Fields("SAHLEmployeeNumber1").Value
 end if

End Sub


Sub btn_PropertyDetails_onclick

  window.tbl_LoanDetails.style.postop = 1200
  window.tbl_Contact.style.postop = 1200
  window.tbl_SAHLDetails.style.postop = 1200
  window.tbl_TelecentreComment.style.postop = 1200
  
  window.tbl_Property.style.postop = 220
  
 
  window.btn_UsePropertyAddress.style.visibility = "hidden"

  b_Forward = true
  
End Sub

Sub btn_ContactDetails_onclick

  window.tbl_LoanDetails.style.postop = 1200
  window.tbl_Property.style.postop = 1200
  window.tbl_SAHLDetails.style.postop = 1200
  window.tbl_TelecentreComment.style.postop = 1200
  
  window.tbl_Contact.style.postop = 220

  window.btn_UsePropertyAddress.style.visibility = "visible"



End Sub



Sub btn_PropertyDetails_onfocus

  if b_Forward = false then
 
     window.btn_LoanDetails.click
     if window.ProspectCashDeposit.Enabled = true then
		window.ProspectCashDeposit.focus
	 end if
     b_Forward = true
     exit sub
  end if
  
  window.btn_PropertyDetails.click
  'if s_Action <> "Add Further Loan" then 
  if window.ProspectPropertyBuildingNumber.Enabled = true then
	window.ProspectPropertyBuildingNumber.focus
  end if
  'end if
  b_Forward = true
end sub


Sub btn_ContactDetails_onfocus

 if b_Forward = false then

     window.btn_PropertyDetails.click
   '  if s_Action <> "Add Further Loan" then 
     if window.ProspectContactNumber.Enabled = true then
		window.ProspectContactNumber.focus
	 end if
'	 end if
     b_Forward = true
     exit sub
  end if

 window.btn_ContactDetails.click
 
' if s_Action <> "Add Further Loan" then 
 if window.ProspectHomeBuildingNumber.Enabled = true then
	window.ProspectHomeBuildingNumber.focus
 end if

' end if

End Sub

Sub btn_SAHLDetails_onfocus

   if b_Forward = false then
     window.btn_ContactDetails.click
     if s_Action <> "Add Further Loan" and s_Action <> "View" then 
        window.ProspectPostalCode.Enabled = true
		window.ProspectPostalCode.focus
	 end if
     b_Forward = true
     exit sub
   End if

 window.btn_SAHLDetails.click
 if s_Action <> "Add Further Loan" and s_Action <> "View" then 
    window.ProspectSuretorName.Enabled = true
	window.ProspectSuretorName.focus
 end if

End Sub

Sub ProspectCashDeposit_onfocus
b_Forward = True
End Sub

Sub ProspectContactNumber_onfocus
b_Forward = true
End Sub


Sub ProspectHomeBuildingNumber_KeyDown(KeyCode , Shift)

if Shift  = 1 then
    b_Forward = false 
else
   b_Forward = false
end if

End Sub



Sub btn_LoanDetails_onfocus

if b_Forward = false then
    
   if s_Action <> "Add Further Loan" and s_Action <> "View" then 
      'window.ProspectSpouseIDNumber.Enabled = true 
	  'window.ProspectSpouseIDNumber.focus
		window.DataCombo_EmploymentType.Enabled = true
		window.DataCombo_EmploymentType.focus
   end if
	 
     b_Forward = true
     exit sub
  end if

window.btn_LoanDetails_onclick

b_Forward = true
End Sub

Sub ProspectSuretorName_KeyDown(KeyCode , Shift)

if Shift  = 1 then
    b_Forward = false 
else
   b_Forward = false
end if

End Sub

Sub ProspectCashDeposit_onfocus
b_Forward = true

End Sub

Sub ProspectPropertyBuildingNumber_KeyDown(KeyCode , Shift)
if Shift  = 1 then
    b_Forward = false 
else
   b_Forward = false
end if
End Sub

Sub ProspectSpouseIDNumber_onfocus
b_Forward = True

End Sub



Sub ProspectPostOffice_onfocus
b_Forward = true
End Sub




Sub btn_PropertyDetails_onmouseover
 b_Forward = true
End Sub

Sub btn_LoanDetails_onmouseover
 b_Forward = true
End Sub

Sub btn_ContactDetails_onmouseover
b_Forward = True

End Sub

Sub btn_SAHLDetails_onmouseover
b_Forward = True

End Sub


Sub DataCombo_Purpose_KeyDown(KeyCode , Shift)

if Shift  = 1 then
    b_Forward = false 
else
   b_Forward = false
end if

End Sub



Sub btn_Exit_onblur
window.btn_LoanDetails.click
if s_Action <> "Add Further Loan" and s_Action <> "View" then 
   window.DataCombo_Gender.Enabled = true 
   window.DataCombo_Gender.focus
end if

End Sub


Sub ProspectPropertyBuildingNumber_KeyPress(KeyAscii)


End Sub

Sub ProspectPropertyBuildingNumber_KeyExit(Action)
if action  = 1 then
   
    b_Forward =false 
  
end if

End Sub

Sub ProspectHomeBuildingNumber_KeyExit(Action)
if action  = 1 then
    b_Forward = false 
end if
End Sub

Sub ProspectSuretorName_KeyExit(Action)
if action  = 1 then
    b_Forward = false
 end if 

window.ProspectSuretorName.Text = ProperCase(window.ProspectSuretorName.Text)
 
End Sub

Sub ProspectCashDeposit_Keydown(i,s)


if i = 118 then
    
    i_Resp = MsgBox("Include Total Fees in Cash Deposit...??" , 4)
    if i_Resp= 7 then       
        
    else
       
         window.ProspectCashDeposit.Value =  window.ProspectCashDeposit.Value +  window.ProspectTotalFees.Value
       ' window.ProspectCashDeposit.Value  = window.ProspectCashDeposit.Value - (window.ProspectBondToRegister.Value - window.ProspectLoanRequired.Value)
        
        
    end if
    window.ProspectCashDeposit.Enabled = true
    window.ProspectCashDeposit.focus
elseif i = 119 then
    
    i_Resp = MsgBox("Make Total Loan Required = Bond To Register...??" , 4)
    if i_Resp= 7 then       
        
    else
        window.ProspectCashDeposit.Value =  window.ProspectCashDeposit.Value +  window.ProspectTotalFees.Value

        if d_FeeLastCalced > 0 then
            window.ProspectCashDeposit.Value  =  d_FeeLastCalced
            CalculateFees
            'window.ProspectLoanRequired.Value  =  window.ProspectLoanRequired.Value -window.ProspectTotalFees.Value
          
        end if    
        b_FeeCalced = true 
        d_FeeLastCalced = window.ProspectCashDeposit.Value
        ' window.ProspectCashDeposit.Value =  window.ProspectCashDeposit.Value +  window.ProspectTotalFees.Value
        window.ProspectCashDeposit.Value  = window.ProspectCashDeposit.Value - (window.ProspectBondToRegister.Value - window.ProspectLoanRequired.Value)
        
        
    end if
    window.ProspectCashDeposit.Enabled = true
    window.ProspectCashDeposit.focus
end if

End Sub

Sub ProspectBondToRegister_Change
 if window.ProspectBondRequired.Value  > window.ProspectBondToRegister.Value then
    window.ProspectBondToRegister.Value = window.ProspectBondRequired.Value 
 elseif (window.ProspectBondToRegister.Value-1000) = (window.ProspectLoanRequired.Value) then
    window.ProspectBondToRegister.Value = window.ProspectLoanRequired.Value
 end if
End Sub


Sub ProspectFirstNames_KeyExit(i)
window.ProspectFirstNames.Text = ProperCase(window.ProspectFirstNames.Text)
End Sub

Sub ProspectFirstNames_MouseExit
window.ProspectFirstNames.Text = ProperCase(window.ProspectFirstNames.Text)
End Sub

Sub ProspectSurname_KeyExit(i)
window.ProspectSurname.Text = ProperCase(window.ProspectSurname.Text)
End Sub

Sub ProspectSurname_MouseExit
window.ProspectSurname.Text = ProperCase(window.ProspectSurname.Text)
End Sub

Sub ProspectSalutation_KeyExit(i)
window.ProspectSalutation.Text = ProperCase(window.ProspectSalutation.Text)
End Sub

Sub ProspectSalutation_MouseExit
window.ProspectSalutation.Text = ProperCase(window.ProspectSalutation.Text)
End Sub

Sub ProspectSpouseFirstNames_KeyExit(Action)
window.ProspectSpouseFirstNames.Text = ProperCase(window.ProspectSpouseFirstNames.Text)
End Sub

Sub ProspectSpouseFirstNames_MouseExit
window.ProspectSpouseFirstNames.Text = ProperCase(window.ProspectSpouseFirstNames.Text)
End Sub

Sub ProspectPropertyBuildingName_KeyExit(Action)
window.ProspectPropertyBuildingName.Text = ProperCase(window.ProspectPropertyBuildingName.Text)
End Sub

Sub ProspectPropertyBuildingName_MouseExit
window.ProspectPropertyBuildingName.Text = ProperCase(window.ProspectPropertyBuildingName.Text)
End Sub

Sub ProspectPropertyStreetName_KeyExit(Action)
window.ProspectPropertyStreetName.Text = ProperCase(window.ProspectPropertyStreetName.Text)
End Sub

Sub ProspectPropertyStreetName_MouseExit
window.ProspectPropertyStreetName.Text = ProperCase(window.ProspectPropertyStreetName.Text)
End Sub

Sub ProspectPropertySuburb_KeyExit(Action)
window.ProspectPropertySuburb.Text = ProperCase(window.ProspectPropertySuburb.Text)
End Sub

Sub ProspectPropertySuburb_MouseExit
window.ProspectPropertySuburb.Text = ProperCase(window.ProspectPropertySuburb.Text)
End Sub

Sub ProspectPropertyCity_KeyExit(Action)
window.ProspectPropertyCity.Text = ProperCase(window.ProspectPropertyCity.Text)
End Sub

Sub ProspectPropertyCity_MouseExit
window.ProspectPropertyCity.Text = ProperCase(window.ProspectPropertyCity.Text)
End Sub

Sub ProspectPropertyCountry_KeyExit(Action)
window.ProspectPropertyCountry.Text = ProperCase(window.ProspectPropertyCountry.Text)
End Sub

Sub ProspectPropertyCountry_MouseExit
window.ProspectPropertyCountry.Text = ProperCase(window.ProspectPropertyCountry.Text)
End Sub

Sub ProspectContactName_KeyExit(Action)
window.ProspectContactName.Text = ProperCase(window.ProspectContactName.Text)
End Sub

Sub ProspectContactName_MouseExit
window.ProspectContactName.Text = ProperCase(window.ProspectContactName.Text)
End Sub

Sub ProspectHomeBuildingName_KeyExit(Action)
window.ProspectHomeBuildingName.Text = ProperCase(window.ProspectHomeBuildingName.Text)
End Sub

Sub ProspectHomeBuildingName_MouseExit
window.ProspectHomeBuildingName.Text = ProperCase(window.ProspectHomeBuildingName.Text)
End Sub

Sub ProspectHomeStreetName_KeyExit(Action)
window.ProspectHomeStreetName.Text = ProperCase(window.ProspectHomeStreetName.Text)
End Sub

Sub ProspectHomeStreetName_MouseExit
window.ProspectHomeStreetName.Text = ProperCase(window.ProspectHomeStreetName.Text)
End Sub

Sub ProspectHomeSuburb_KeyExit(Action)
window.ProspectHomeSuburb.Text = ProperCase(window.ProspectHomeSuburb.Text)
End Sub

Sub ProspectHomeSuburb_MouseExit
window.ProspectHomeSuburb.Text = ProperCase(window.ProspectHomeSuburb.Text)
End Sub

Sub ProspectHomeCity_KeyExit(Action)
window.ProspectHomeCity.Text = ProperCase(window.ProspectHomeCity.Text)
End Sub

Sub ProspectHomeCity_MouseExit
window.ProspectHomeCity.Text = ProperCase(window.ProspectHomeCity.Text)
End Sub

Sub ProspectHomeCountry_KeyExit(Action)
window.ProspectHomeCountry.Text = ProperCase(window.ProspectHomeCountry.Text)
End Sub

Sub ProspectHomeCountry_MouseExit
window.ProspectHomeCountry.Text = ProperCase(window.ProspectHomeCountry.Text)
End Sub

Sub ProspectPostOffice_KeyExit(Action)
window.ProspectPostOffice.Text = ProperCase(window.ProspectPostOffice.Text)
End Sub

Sub ProspectPostOffice_MouseExit
window.ProspectPostOffice.Text = ProperCase(window.ProspectPostOffice.Text)
End Sub

Sub ProspectSuretorName_MouseExit
window.ProspectSuretorName.Text = ProperCase(window.ProspectSuretorName.Text)
End Sub

Sub ProspectTransferAttorney_KeyExit(Action)
window.ProspectTransferAttorney.Text = ProperCase(window.ProspectTransferAttorney.Text)
End Sub

Sub ProspectTransferAttorney_MouseExit
window.ProspectTransferAttorney.Text = ProperCase(window.ProspectTransferAttorney.Text)
End Sub

Sub ProspectPreviousEmployer_KeyExit(Action)
window.ProspectPreviousEmployer.Text = ProperCase(window.ProspectPreviousEmployer.Text)
End Sub

Sub ProspectPreviousEmployer_MouseExit
window.ProspectPreviousEmployer.Text = ProperCase(window.ProspectPreviousEmployer.Text)
End Sub

Sub ProspectCurrentEmployer_KeyExit(Action)
window.ProspectCurrentEmployer.Text = ProperCase(window.ProspectCurrentEmployer.Text)
End Sub

Sub ProspectCurrentEmployer_MouseExit
window.ProspectCurrentEmployer.Text = ProperCase(window.ProspectCurrentEmployer.Text)
End Sub

Sub window_onunload


End Sub

Sub DataCombo_Property_Change
	'To Prevent sub activation  during window load...
	if  b_Loading = true then 
	   b_Loading = False
	   exit sub
	end if


	 if Cint(DataCombo_Property.BoundText) = 1 then 'Unknown
	        if b_Title = true then
	         rs_Title.close
	       end if
			sSQL = "SELECT * FROM TITLETYPE WHERE TitleTypeNumber = 1 OR TitleTypeNumber = 6"
			rs_Title.CursorLocation = 3
			rs_Title.Open sSQL ,conn,adOpenStatic
			b_Title= true
	elseif CInt(DataCombo_Property.BoundText) = 2 then 'House
	        if b_Title = true then
	          rs_Title.close
	        end if
			sSQL = "SELECT * FROM TITLETYPE WHERE TitleTypeNumber = 1 OR TitleTypeNumber = 2 OR TitleTypeNumber = 6"
			rs_Title.CursorLocation = 3
			rs_Title.Open sSQL ,conn,adOpenStatic
			b_Title= true
	elseif CInt(DataCombo_Property.BoundText) > 2 and  CInt(DataCombo_Property.BoundText) <= 7 then 'House
	        if b_Title = true then
	         rs_Title.close
	        end if
			sSQL = "SELECT * FROM TITLETYPE WHERE TitleTypeNumber = 1 OR (TitleTypeNumber >= 2 AND TitleTypeNumber <=7)"
			rs_Title.CursorLocation = 3
			rs_Title.Open sSQL ,conn,adOpenStatic
	        b_Title= true
	end if
	rs_Title.MoveFirst
	set DataCombo_Title.RowSource = rs_Title
	DataCombo_Title.Refresh
	DataCombo_Title.BoundText  = rs_Title.Fields("TitleTypeNumber").Value


End Sub

Sub DataCombo_MarketRateType_Change
'To Prevent sub activation  during window load...
if  b_Loading = true then 
   b_Loading = False
   exit sub
end if
i_CurrentMarketRate = rs_MarketRateType.Fields("MarketRateTypeRate").Value 
End Sub

Sub DataCombo_LinkRate_Change
'To Prevent sub activation  during window load...
if  b_Loading = true then 
   b_Loading = False
   exit sub
end if
i_CurrentLinkRate = CDbl(DataCombo_LinkRate.BoundText)


End Sub



Sub btn_TeleCentreComment_onclick

 window.tbl_LoanDetails.style.postop = 1200
 window.tbl_Property.style.postop = 1200
 window.tbl_Contact.style.postop = 1200
 window.tbl_SAHLDetails.style.postop = 1200

 window.tbl_TelecentreComment.style.postop = 220

if rs_commentopen = false then 
 
 	sSQL = "p_GetTeleCentreComment " & i_ProspectNumber 
	' msgbox sSQL
	rscomment.CursorLocation = 3
	rscomment.Open sSQL,conn,adOpenDynamic
	
	window.TeleCentreComment.Text = rscomment.Fields("TeleProspectComment").Value
	
	rscomment.Close
end if


rs_commentopen = true

End Sub

Function GetSAHLStaffDetailRecords

	GetSAHLStaffDetailRecords = 0
	
	set rstemp = createobject("ADODB.Recordset")
	
	sSQL = "SELECT ProspectDetailNumber FROM PROSPECTDETAIL WHERE ProspectNumber = " & CStr(i_ProspectNumber) & " AND DetailTypeNumber IN ( 237 )"
	
	rstemp.CursorLocation = 3
	rstemp.Open sSQL,conn,adOpenDynamic
	
	
	if rstemp.RecordCount > 0 then
		GetSAHLStaffDetailRecords = 1
	End if
	
End Function



-->
</script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<body class=Generic bottomMargin=0 leftMargin=0 topMargin=0 rightMargin=0><input class=button2 id=btn_LoanDetails style="Z-INDEX: 106; LEFT: 20px; WIDTH: 138px; CURSOR: hand; POSITION: absolute; TOP: 487px; HEIGHT: 49px" tabIndex=12 type=button value="Loan Details" name=btn_LoanDetails> 
<input class=button2 id=btn_UsePropertyAddress title="Use Property Address" style="Z-INDEX: 112; LEFT: 375px; VISIBILITY: hidden; WIDTH: 149px; CURSOR: hand; POSITION: absolute; TOP: 325px; HEIGHT: 49px" tabIndex=41 type=button value="Use Property Address" name=btn_UsePropertyAddress></TD> 
<input class=button3 id=btn_Exit title=Exit style="Z-INDEX: 111; LEFT: 734px; VERTICAL-ALIGN: sub; WIDTH: 131px; CURSOR: hand; POSITION: absolute; TOP: 163px; HEIGHT: 58px" tabIndex=75 type=button value=Exit name=btn_Exit> 
<input class=button2 id=btn_PropertyDetails style="Z-INDEX: 107; LEFT: 158px; WIDTH: 138px; CURSOR: hand; POSITION: absolute; TOP: 487px; HEIGHT: 49px" tabIndex=24 type=button value="Property Details" name=btn_PropertyDetails width="138"> 
<input class=button2 id=btn_ContactDetails style="Z-INDEX: 108; LEFT: 296px; WIDTH: 138px; CURSOR: hand; POSITION: absolute; TOP: 487px; HEIGHT: 49px" tabIndex=41 type=button value="Contact Details" name=btn_ContactDetails width="138"><input class=button2 id=btn_SAHLDetails style="Z-INDEX: 109; LEFT: 434px; WIDTH: 138px; CURSOR: hand; POSITION: absolute; TOP: 487px; HEIGHT: 49px" tabIndex=62 type=button value="Administration Details" name=btn_SAHLDetails width="138"> 

<table class=Table1 id=tbl_Basic 
style="Z-INDEX: 104; LEFT: 20px; WIDTH: 852px; POSITION: absolute; TOP: 0px; HEIGHT: 223px" 
cellSpacing=1 cellPadding=1 width="75%" background="" border=0>
  <TBODY style="designtimesp: 31641">
  <tr>
    <td class=Header1 id=lbl_BasicMessage noWrap align=middle colSpan=4 
    >Please Wait..... Loading prospect details......</TD></TR>
  <tr>
    <td id=lbl_ProspectNumber noWrap align=right>
    <td noWrap>
      <OBJECT id=ProspectNumber style="WIDTH: 100px; HEIGHT: 22px" 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2646"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="####0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="999999"><PARAM NAME="MinValue" VALUE="-99999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <td noWrap>Prospect Income(Monthly)</TD>
    <td noWrap>
      <OBJECT id=ProspectIncome 
      style="LEFT: 0px; WIDTH: 135px; TOP: 0px; HEIGHT: 22px" tabIndex=7 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3572"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="###,###,##0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="########0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="999999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR>
  <tr>
    <td noWrap align=right>Gender</TD>
    <td noWrap>
      <OBJECT id=DataCombo_Gender 
      style="LEFT: 0px; WIDTH: 149px; TOP: 0px; HEIGHT: 22px" tabIndex=1 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=149 height=22><PARAM NAME="_ExtentX" VALUE="3942"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD>
    <td noWrap align=right>Spouse Income (Monthly)</TD>
    <td noWrap>
      <OBJECT id=ProspectSpouseIncome 
      style="LEFT: 0px; WIDTH: 135px; TOP: 0px; HEIGHT: 22px" tabIndex=8 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3572"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="###,###,##0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="########0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="999999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR>
  <tr>
    <td noWrap align=right>Legal Status</TD>
    <td noWrap>
      <OBJECT id=DataCombo_Legal 
      style="LEFT: 0px; WIDTH: 325px; TOP: 0px; HEIGHT: 22px" tabIndex=2 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=325 height=22><PARAM NAME="_ExtentX" VALUE="8599"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD>
    <td noWrap align=right>Spouse First Names</TD>
    <td noWrap>
      <OBJECT id=ProspectSpouseFirstNames 
      style="LEFT: 0px; WIDTH: 246px; TOP: 0px; HEIGHT: 22px" tabIndex=9 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6509"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <td id=lbl_firstnames noWrap align=right>First 
    Names</TD>
    <td noWrap>
      <OBJECT id=ProspectFirstNames 
      style="CLEAR: both; BACKGROUND-ATTACHMENT: scroll; LEFT: 96px; WIDTH: 325px; TOP: 146px; HEIGHT: 22px" 
      tabIndex=3 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="8599"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="0"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="-1"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="200"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Spouse ID Number</TD>
    <td noWrap>
      <OBJECT id=ProspectSpouseIDNumber 
      style="LEFT: 0px; WIDTH: 246px; TOP: 0px; HEIGHT: 22px" tabIndex=10 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6509"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <td id=lbl_surname noWrap align=right>Surname</TD>
    <td noWrap>
      <OBJECT id=ProspectSurname 
      style="BACKGROUND-ATTACHMENT: scroll; LEFT: 0px; WIDTH: 261px; TOP: 0px; HEIGHT: 22px" 
      tabIndex=4 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6906"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Employment Type&nbsp;</TD>
    <td noWrap>
      <OBJECT id=DataCombo_EmploymentType 
      style="LEFT: 1px; WIDTH: 170px; TOP: 1px; HEIGHT: 26px" tabIndex=11 
      name=DataCombo_EmploymentType 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=170 height=26><PARAM NAME="_ExtentX" VALUE="4498"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <td id=lbl_salutation noWrap align=right>Salutation</TD>
    <td noWrap>
      <OBJECT id=ProspectSalutation 
      style="BACKGROUND-ATTACHMENT: fixed; LEFT: 0px; WIDTH: 129px; TOP: 0px; HEIGHT: 22px" 
      tabIndex=5 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3413"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right><IMG style="Z-INDEX: 124; LEFT: 768px; WIDTH: 19px; POSITION: absolute; TOP: 170px; HEIGHT: 23px" height=23 alt="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 >&nbsp;</TD>
    <td noWrap></TD></TR>
  <tr>
    <td noWrap align=right>ID Number</TD>
    <td noWrap>
      <OBJECT id=ProspectIDNumber 
      style="LEFT: 1px; WIDTH: 270px; TOP: 1px; HEIGHT: 27px" tabIndex=6 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="7144"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right><input class=button3 id=btn_Mode title=Exit style="Z-INDEX: 120; LEFT: 579px; VISIBILITY: hidden; VERTICAL-ALIGN: sub; WIDTH: 131px; CURSOR: hand; PADDING-TOP: 15px; POSITION: absolute; TOP: 161px; HEIGHT: 58px" disabled tabIndex=74 type=button name=btn_Mode><IMG id=pic_Mode style="Z-INDEX: 123; LEFT: 634px; VISIBILITY: hidden; WIDTH: 19px; POSITION: absolute; TOP: 163px; HEIGHT: 23px" height=23 alt="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 > 
    &nbsp;</TD>
    <td noWrap></TD></TR></TBODY></TABLE>&nbsp; <br><br>
<table class=Table1 id=tbl_LoanDetails 
style="Z-INDEX: 101; LEFT: 20px; WIDTH: 852px; POSITION: absolute; TOP: 225px; HEIGHT: 247px" 
cellSpacing=1 cellPadding=1 width="75%" border=0>
  
  <tr>
    <td class=Header1 id=lbl_LoanMessage noWrap align=middle bgColor=darkcyan 
    colSpan=6>Loan Details</TD></TR>
  <tr>
    <td noWrap></TD>
    <td noWrap></TD>
    <TD id=lbl_ExistingLoan style="designtimesp: 31690" noWrap align=right 
    >&nbsp;&nbsp; Existing Loan&nbsp;&nbsp;&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectExistingLoan 
      style="LEFT: 1px; WIDTH: 117px; TOP: 1px; HEIGHT: 24px" tabIndex=19 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=117><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3096"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="#########0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999999"><PARAM NAME="MinValue" VALUE="-9999999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="0"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <td noWrap></TD>
    <td noWrap></TD></TR>
  <tr>
    <TD style="designtimesp: 31695" noWrap align=right 
      >Loan Purpose</TD>
    <td noWrap>
      <OBJECT id=DataCombo_Purpose 
      style="LEFT: 1px; WIDTH: 158px; TOP: 1px; HEIGHT: 26px" tabIndex=13 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=158 height=26><PARAM NAME="_ExtentX" VALUE="4180"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD>
    <TD id=lbl_factor style="designtimesp: 31697" noWrap align=right 
    >+&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      Interim Interest</TD>
    <td noWrap>
      <OBJECT id=ProspectFactor 
      style="LEFT: 1px; WIDTH: 117px; TOP: 1px; HEIGHT: 24px" tabIndex=20 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=117><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3096"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="#########0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999"><PARAM NAME="MinValue" VALUE="-9999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <TD id=lbl_overridefactor style="designtimesp: 31699" noWrap 
    ><input id=chk_factor tabIndex=18 type=checkbox 
      name=chk_factor>Override Interim Interest</TD>
    <td noWrap></TD></TR>
  <tr>
    <TD id=lbl_Term style="designtimesp: 31703" noWrap align=right 
    >Term (Years)</TD>
    <td noWrap>
      <OBJECT id=ProspectTermRequired 
      style="LEFT: 0px; WIDTH: 56px; TOP: 0px; HEIGHT: 24px" tabIndex=14 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="1482"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="##0"><PARAM NAME="EditMode" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="###0"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="2400"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="20"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <TD id=lbl_cashrequired style="designtimesp: 31705" noWrap align=right 
    >+&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      Cash Required&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectCashRequired 
      style="LEFT: 1px; WIDTH: 117px; TOP: 1px; HEIGHT: 24px" tabIndex=21 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=117><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3096"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="#########0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999"><PARAM NAME="MinValue" VALUE="-9999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <TD style="designtimesp: 31707" noWrap><input 
      id=chk_Fees tabIndex=20 type=checkbox name=chk_Fees 
      >Override Fees</TD>
    <td noWrap 
      >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD></TR>
  <tr>
    <TD style="designtimesp: 31711" noWrap align=right 
      >&nbsp;&nbsp; Bond Required</TD>
    <td noWrap>
      <OBJECT id=ProspectBondRequired 
      style="LEFT: 0px; WIDTH: 143px; TOP: 0px; HEIGHT: 23px" tabIndex=15 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3784"><PARAM NAME="_ExtentY" VALUE="609"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="###,###,###,##0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="###########0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <td id=lbl_CashDeposit style="LIST-STYLE-POSITION: outside" noWrap 
    align=right>&nbsp;-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      &nbsp;&nbsp;&nbsp;&nbsp;Cash Deposit &nbsp;&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectCashDeposit 
      style="LEFT: 1px; WIDTH: 117px; TOP: 1px; HEIGHT: 24px" tabIndex=22 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=117><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3096"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="#########0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999"><PARAM NAME="MinValue" VALUE="-9999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <TD style="designtimesp: 31715" noWrap align=right 
      >&nbsp;&nbsp;&nbsp;&nbsp; Cancellation Fee</TD>
    <td noWrap>
      <OBJECT id=ProspectCancelFees 
      style="LEFT: 0px; WIDTH: 96px; TOP: 0px; HEIGHT: 24px" tabIndex=22 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2540"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="############0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999"><PARAM NAME="MinValue" VALUE="-9999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR>
  <tr>
    <TD id=lbl_EstimatedPropertyValue style="designtimesp: 31718" noWrap 
    align=right>Est. Property Value</TD>
    <td noWrap>
      <OBJECT id=ProspectEstimatedPropertyValue 
      style="LEFT: 0px; WIDTH: 143px; TOP: 0px; HEIGHT: 24px" tabIndex=16 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3784"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="###,###,###,##0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="###########0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <TD style="designtimesp: 31720" noWrap align=right 
      >+&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      Fees&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      &nbsp;&nbsp;&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectTotalFees 
      style="LEFT: 1px; WIDTH: 117px; TOP: 1px; HEIGHT: 24px" tabIndex=0 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=117><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3096"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="############0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999999"><PARAM NAME="MinValue" VALUE="-9999999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <TD style="designtimesp: 31722" noWrap align=right 
      >Transfer Fee</TD>
    <td noWrap>
      <OBJECT id=ProspectTransferFees 
      style="LEFT: 0px; WIDTH: 96px; TOP: 0px; HEIGHT: 24px" tabIndex=23 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2540"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="############0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999999"><PARAM NAME="MinValue" VALUE="-9999999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR>
  <tr>
    <td noWrap></TD>
    <td noWrap 
      >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
    <td noWrap></TD>
    <td noWrap></TD>
    <TD style="designtimesp: 31729" noWrap align=right 
      >Registration Fee</TD>
    <td noWrap>
      <OBJECT id=ProspectRegistrationFees 
      style="LEFT: 0px; WIDTH: 96px; TOP: 0px; HEIGHT: 24px" tabIndex=24 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2540"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="############0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999999"><PARAM NAME="MinValue" VALUE="-9999999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR>
  <tr>
    <td noWrap>Market Rate Type</TD>
    <td noWrap>
      <OBJECT id=DataCombo_MarketRateType 
      style="LEFT: 1px; WIDTH: 159px; TOP: 1px; HEIGHT: 26px" tabIndex=17 
      name=DataCombo_MarketRateType 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=159 height=26><PARAM NAME="_ExtentX" VALUE="4207"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD>
    <TD style="designtimesp: 31734" noWrap align=right>= 
      Total Loan Required</TD>
    <td noWrap>
      <OBJECT id=ProspectLoanRequired 
      style="LEFT: 1px; WIDTH: 117px; TOP: 1px; HEIGHT: 24px" tabIndex=0 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=117><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3096"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="############0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999999"><PARAM NAME="MinValue" VALUE="-9999999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <TD style="designtimesp: 31736" noWrap align=right 
      >Initiation Fee</TD>
    <td noWrap>
      <OBJECT id=ProspectSAHLFees 
      style="LEFT: 0px; WIDTH: 96px; TOP: 0px; HEIGHT: 24px" tabIndex=25 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2540"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="############0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999999"><PARAM NAME="MinValue" VALUE="-9999999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR>
  <tr>
    <td noWrap>Link Rate</TD>
    <td noWrap>
      <OBJECT id=DataCombo_LinkRate 
      style="LEFT: 0px; WIDTH: 144px; TOP: 0px; HEIGHT: 26px" tabIndex=18 
      name=DataCombo_LinkRate classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 
      width=144 height=26><PARAM NAME="_ExtentX" VALUE="3810"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD>
    <TD style="designtimesp: 31741" noWrap align=right 
      >Bond To Register</TD>
    <td noWrap>
      <OBJECT id=ProspectBondToRegister 
      style="LEFT: 1px; WIDTH: 117px; TOP: 1px; HEIGHT: 24px" tabIndex=0 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3096"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,###,##0"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="############0"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999999999"><PARAM NAME="MinValue" VALUE="-9999999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <TD style="designtimesp: 31743" noWrap align=right 
      >Valuation Fee</TD>
    <td noWrap>
      <OBJECT id=ProspectValuationFee 
      style="LEFT: 0px; WIDTH: 96px; TOP: 0px; HEIGHT: 24px" tabIndex=26 
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2540"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#,###,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="############0.00"><PARAM NAME="HighlightText" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999999"><PARAM NAME="MinValue" VALUE="-9999999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR></TABLE><br>
<table class=Table1 id=tbl_Property 
style="Z-INDEX: 102; LEFT: 20px; WIDTH: 852px; POSITION: absolute; TOP: 593px; HEIGHT: 230px" 
height=230 cellSpacing=1 cellPadding=1 width=852 background="" border=0>
  
  <tr>
    <td class=Header1 id=lbl_PropertyMessage noWrap align=middle colSpan=4 
    >Property&nbsp;&nbsp;&nbsp;Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD></TR>
  <tr>
    <TD style="designtimesp: 31750" noWrap align=right 
    >&nbsp;</TD>
    <td noWrap>Number&nbsp;&nbsp;&nbsp; Name</TD>
    <td noWrap></TD>
    <td noWrap></TD></TR>
  <tr>
    <TD style="designtimesp: 31755" noWrap align=right 
      >Building&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectPropertyBuildingNumber 
      style="LEFT: 0px; WIDTH: 60px; TOP: 0px; HEIGHT: 27px" tabIndex=25 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="1587"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
      <OBJECT id=ProspectPropertyBuildingName 
      style="LEFT: 0px; WIDTH: 227px; TOP: 0px; HEIGHT: 27px" tabIndex=26 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6006"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Property Type</TD>
    <td noWrap>
      <OBJECT id=DataCombo_Property 
      style="LEFT: 1px; WIDTH: 200px; TOP: 1px; HEIGHT: 27px" tabIndex=34 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=200 height=27><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31760" noWrap align=right 
      >Street&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectPropertyStreetNumber 
      style="LEFT: 0px; WIDTH: 60px; TOP: 0px; HEIGHT: 27px" tabIndex=27 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="1587"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
      <OBJECT id=ProspectPropertyStreetName 
      style="LEFT: 0px; WIDTH: 227px; TOP: 0px; HEIGHT: 27px" tabIndex=28 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6006"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Title Type</TD>
    <td noWrap>
      <OBJECT id=DataCombo_Title 
      style="LEFT: 1px; WIDTH: 200px; TOP: 1px; HEIGHT: 27px" tabIndex=35 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=200 height=27><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31765" noWrap align=right 
      >Suburb</TD>
    <td noWrap>
      <OBJECT id=ProspectPropertySuburb 
      style="LEFT: 0px; WIDTH: 227px; TOP: 0px; HEIGHT: 27px" tabIndex=29 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6006"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Area Classification</TD>
    <td noWrap>
      <OBJECT id=DataCombo_Area 
      style="LEFT: 1px; WIDTH: 200px; TOP: 1px; HEIGHT: 27px" tabIndex=36 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=200 height=27><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31770" noWrap align=right 
      >City&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectPropertyCity 
      style="LEFT: 0px; WIDTH: 227px; TOP: 0px; HEIGHT: 27px" tabIndex=30 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6006"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Owner Occupied</TD>
    <td noWrap>
      <OBJECT id=DataCombo_Owner 
      style="LEFT: 1px; WIDTH: 200px; TOP: 1px; HEIGHT: 27px" tabIndex=37 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=200 height=27><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31775" noWrap align=right 
      >Province&nbsp;</TD>
    <td noWrap>
      <OBJECT id=DataCombo_PropertyProvince 
      style="LEFT: 1px; WIDTH: 227px; TOP: 1px; HEIGHT: 27px" tabIndex=31 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=128 height=27><PARAM NAME="_ExtentX" VALUE="6006"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
      <OBJECT id=ProspectPropertyProvince 
      style="LEFT: 294px; VISIBILITY: hidden; WIDTH: 34px; TOP: 1px; HEIGHT: 27px" 
      tabIndex=31 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="900"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Thatched</TD>
    <td noWrap>
      <OBJECT id=DataCombo_Thatch 
      style="LEFT: 1px; WIDTH: 200px; TOP: 1px; HEIGHT: 27px" tabIndex=38 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=200 height=27><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31780" noWrap align=right 
      >Country&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectPropertyCountry 
      style="LEFT: 0px; WIDTH: 227px; TOP: 0px; HEIGHT: 27px" tabIndex=32 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6006"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE="Republic of South Africa"><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Inspection Contact Name</TD>
    <td noWrap>
      <OBJECT id=ProspectContactName 
      style="LEFT: 1px; WIDTH: 200px; TOP: 1px; HEIGHT: 27px" tabIndex=39 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D width=200><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31785" noWrap align=right 
      >Postal Code&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectPropertyPostalCode 
      style="LEFT: 1px; WIDTH: 227px; TOP: 1px; HEIGHT: 26px" tabIndex=33 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6006"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Inspection Telephone</TD>
    <td noWrap>
      <OBJECT id=ProspectContactNumber 
      style="LEFT: 1px; WIDTH: 192px; TOP: 1px; HEIGHT: 26px" tabIndex=40 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=26><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="5080"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR></TABLE><br>
<table class=Table1 id=tbl_Contact 
style="Z-INDEX: 103; LEFT: 20px; WIDTH: 852px; POSITION: absolute; TOP: 957px; HEIGHT: 279px">
  
  <tr>
    <td class=Header1 id=lbl_ContactMessage noWrap align=middle colSpan=4 
    >Contact Details&nbsp;&nbsp;&nbsp;&nbsp;</TD></TR>
  <tr>
    <TD style="designtimesp: 31794" noWrap align=right 
    >&nbsp;</TD>
    <td noWrap>Number&nbsp;&nbsp;&nbsp; Name</TD>
    <td noWrap></TD>
    <td style="HEIGHT: 0px" noWrap 
      >Code&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Number</TD></TR>
  <tr>
    <TD style="designtimesp: 31799" noWrap align=right 
      >Building&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectHomeBuildingNumber 
      style="LEFT: 0px; WIDTH: 60px; TOP: 0px; HEIGHT: 22px" tabIndex=42 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="1587"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
      <OBJECT id=ProspectHomeBuildingName 
      style="LEFT: 0px; WIDTH: 227px; TOP: 0px; HEIGHT: 22px" tabIndex=43 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6006"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Home Phone</TD>
    <td noWrap>
      <OBJECT id=ProspectHomePhoneCode 
      style="LEFT: 0px; WIDTH: 60px; TOP: 0px; HEIGHT: 22px" tabIndex=51 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="1587"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
      <OBJECT id=ProspectHomePhoneNumber 
      style="LEFT: 61px; WIDTH: 135px; TOP: 1px; HEIGHT: 22px" tabIndex=52 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D width=140 height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3572"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31804" noWrap align=right 
      >Street&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectHomeStreetNumber 
      style="LEFT: 0px; WIDTH: 60px; TOP: 0px; HEIGHT: 22px" tabIndex=44 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="1587"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
      <OBJECT id=ProspectHomeStreetName 
      style="LEFT: 0px; WIDTH: 227px; TOP: 0px; HEIGHT: 22px" tabIndex=45 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6006"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Work Phone</TD>
    <td noWrap>
      <OBJECT id=ProspectWorkPhoneCode 
      style="LEFT: 0px; WIDTH: 60px; TOP: 0px; HEIGHT: 22px" tabIndex=53 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="1587"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
      <OBJECT id=ProspectWorkPhoneNumber 
      style="LEFT: 61px; WIDTH: 135px; TOP: 1px; HEIGHT: 22px" tabIndex=54 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D width=140 height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3572"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31809" noWrap align=right 
      >Suburb&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectHomeSuburb 
      style="LEFT: 1px; WIDTH: 240px; TOP: 1px; HEIGHT: 22px" tabIndex=46 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6350"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Fax</TD>
    <td noWrap>
      <OBJECT id=ProspectFaxPhoneCode 
      style="LEFT: 0px; WIDTH: 60px; TOP: 0px; HEIGHT: 22px" tabIndex=55 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="1587"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
      <OBJECT id=ProspectFaxPhoneNumber 
      style="LEFT: 61px; WIDTH: 135px; TOP: 1px; HEIGHT: 22px" tabIndex=56 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D width=140 height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3572"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31814" noWrap align=right 
      >City&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectHomeCity 
      style="LEFT: 1px; WIDTH: 241px; TOP: 1px; HEIGHT: 22px" tabIndex=47 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6376"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Cell Phone</TD>
    <td noWrap>
      <OBJECT id=ProspectCellPhoneNumber 
      style="LEFT: 1px; WIDTH: 195px; TOP: 1px; HEIGHT: 22px" tabIndex=57 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D width=200 height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="5159"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31819" noWrap align=right 
      >Province</TD>
    <td style="VERTICAL-ALIGN: top">
      <OBJECT id=DataCombo_ContactProvince 
      style="LEFT: 1px; WIDTH: 241px; TOP: 2px; HEIGHT: 22px" tabIndex=48 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=241 height=22><PARAM NAME="_ExtentX" VALUE="6376"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
      <OBJECT id=ProspectHomeProvince 
      style="LEFT: 242px; VISIBILITY: hidden; WIDTH: 0px; TOP: 1px; HEIGHT: 0px" 
      tabIndex=46 classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="0"><PARAM NAME="_ExtentY" VALUE="0"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>&nbsp;&nbsp; <input 
      id=chk_foreign style="VERTICAL-ALIGN: text-bottom" type=checkbox 
      align=right name=chk_foreign>Foreign</TD>
    <td noWrap align=right>&nbsp;E-Mail Address</TD>
    <td noWrap>
      <OBJECT id=ProspectEMailAddress 
      style="LEFT: 1px; WIDTH: 195px; TOP: 1px; HEIGHT: 22px" tabIndex=58 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D width=200 height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="5159"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31825" noWrap align=right 
      >Country&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectHomeCountry 
      style="LEFT: 1px; WIDTH: 241px; TOP: 1px; HEIGHT: 22px" tabIndex=49 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6376"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Box Number</TD>
    <td noWrap>
      <OBJECT id=ProspectBoxNumber 
      style="LEFT: 1px; WIDTH: 195px; TOP: 1px; HEIGHT: 22px" tabIndex=59 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D width=200 height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="5159"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31830" noWrap align=right 
      >Postal Code&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectHomePostalCode 
      style="LEFT: 1px; WIDTH: 116px; TOP: 1px; HEIGHT: 22px" tabIndex=50 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3069"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Postal Office</TD>
    <td noWrap>
      <OBJECT id=ProspectPostOffice 
      style="LEFT: 1px; WIDTH: 195px; TOP: 1px; HEIGHT: 22px" tabIndex=60 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D width=195 height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="5159"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <TD style="designtimesp: 31835" noWrap align=right 
    >&nbsp;</TD>
    <td noWrap></TD>
    <td noWrap align=right>Post Code</TD>
    <td noWrap>
      <OBJECT id=ProspectPostalCode 
      style="LEFT: 1px; WIDTH: 124px; TOP: 1px; HEIGHT: 22px" tabIndex=61 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=22><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3281"><PARAM NAME="_ExtentY" VALUE="582"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <TD style="HEIGHT: 1px; designtimesp: 31840" noWrap align=right height=1 
    >&nbsp;</TD>
    <td style="HEIGHT: 1px" noWrap height=1>
    <td style="HEIGHT: 1px" noWrap height=1></TD>
    <td style="HEIGHT: 1px" noWrap height=1></TD></TR></TABLE><br>
<table class=Table1 id=tbl_SAHLDetails 
style="Z-INDEX: 105; LEFT: 20px; WIDTH: 852px; POSITION: absolute; TOP: 1281px; HEIGHT: 250px" 
height=250 width=852 border=0>
  
  <tr>
    <td class=Header1 id=lbl_SAHLAdminMessage noWrap align=middle colSpan=4 
    >&nbsp;&nbsp;&nbsp;&nbsp;SAHL Administration 
      Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD></TR>
  <tr>
    <td noWrap align=right>Suretor Name&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectSuretorName 
      style="LEFT: 1px; WIDTH: 290px; TOP: 1px; HEIGHT: 27px" tabIndex=63 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="7673"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Employee Team</TD>
    <td noWrap>
      <OBJECT id=DataCombo_EmployeeTeam 
      style="LEFT: 1px; VISIBILITY: inherit; WIDTH: 200px; TOP: 1px; HEIGHT: 26px" 
      tabIndex=71 classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=200 
      height=26><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <td noWrap align=right>Suretor ID Number&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectSuretorIDNumber 
      style="LEFT: 1px; WIDTH: 240px; TOP: 1px; HEIGHT: 27px" tabIndex=64 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="6350"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Admin Employee</TD>
    <td noWrap>
      <OBJECT id=DataCombo_AdminEmployee 
      style="LEFT: 1px; WIDTH: 200px; TOP: 1px; HEIGHT: 26px" tabIndex=72 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=200 height=26><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <td noWrap align=right>Transfer 
    Attorney&nbsp;&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectTransferAttorney 
      style="LEFT: 1px; WIDTH: 290px; TOP: 1px; HEIGHT: 27px" tabIndex=65 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="7673"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap align=right>Prospect Source</TD>
    <td noWrap>
      <OBJECT id=DataCombo_Source 
      style="LEFT: 1px; WIDTH: 200px; TOP: 1px; HEIGHT: 26px" tabIndex=73 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=200 height=26><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <td noWrap></TD>
    <td noWrap>Start 
      Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      Employer Name</TD>
    <td id=lbl_broker noWrap align=right>Broker</TD>
    <td noWrap>
      <OBJECT id=DataCombo_Broker 
      style="LEFT: 1px; WIDTH: 200px; TOP: 1px; HEIGHT: 26px" tabIndex=74 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=200 height=26><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <td noWrap align=right>Current 
Employer&nbsp;&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectCurrentStartDate 
      style="LEFT: 1px; WIDTH: 120px; TOP: 1px; HEIGHT: 27px" tabIndex=66 
      classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3175"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="dd/mm/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="2"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
      <OBJECT id=ProspectCurrentEmployer 
      style="LEFT: 121px; WIDTH: 221px; TOP: 1px; HEIGHT: 27px" tabIndex=67 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="5847"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td id=lbl_TeleSales style="VISIBILITY: visible" noWrap align=right 
    >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Tele-Consultant </TD>
    <td noWrap>
      <OBJECT id=DataCombo_TeleSales 
      style="LEFT: 1px; VISIBILITY: visible; WIDTH: 200px; TOP: 1px; HEIGHT: 26px" 
      tabIndex=75 classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=200 
      height=26><PARAM NAME="_ExtentX" VALUE="5292"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD></TR>
  <tr>
    <td noWrap align=right>Previous 
    Employer&nbsp;&nbsp;</TD>
    <td noWrap>
      <OBJECT id=ProspectPreviousStartDate 
      style="LEFT: 1px; WIDTH: 120px; TOP: 1px; HEIGHT: 27px" tabIndex=68 
      classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3175"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="dd/mm/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="2"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
      <OBJECT id=ProspectPreviousEmployer 
      style="LEFT: 121px; WIDTH: 221px; TOP: 1px; HEIGHT: 27px" tabIndex=69 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D width=221><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="5847"><PARAM NAME="_ExtentY" VALUE="714"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD>
    <td noWrap></TD>
    <td noWrap></TD></TR>
  <tr>
    <td noWrap align=right>SPV&nbsp;&nbsp;</TD>
    <td noWrap>
      <OBJECT id=DataCombo_SPV 
      style="LEFT: 1px; WIDTH: 278px; TOP: 1px; HEIGHT: 26px" tabIndex=70 
      classid=clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10 width=278 height=26><PARAM NAME="_ExtentX" VALUE="7355"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="_Version" VALUE="393216"><PARAM NAME="IntegralHeight" VALUE="-1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="Locked" VALUE="0"><PARAM NAME="MatchEntry" VALUE="-1"><PARAM NAME="SmoothScroll" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Style" VALUE="2"><PARAM NAME="CachePages" VALUE="3"><PARAM NAME="CachePageSize" VALUE="50"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="BackColor" VALUE="-1"><PARAM NAME="ForeColor" VALUE="-1"><PARAM NAME="ListField" VALUE=""><PARAM NAME="BoundColumn" VALUE=""><PARAM NAME="Text" VALUE=""><PARAM NAME="RightToLeft" VALUE="0"><PARAM NAME="DataMember" VALUE=""></OBJECT>
</TD>
    <td noWrap></TD>
    <td noWrap></TD></TR></TABLE><INPUT class=button2 id=btn_TeleCentreComment style="Z-INDEX: 110; LEFT: 573px; VISIBILITY: hidden; WIDTH: 138px; CURSOR: hand; POSITION: absolute; TOP: 487px; HEIGHT: 49px" tabIndex=62 type=button value="TeleCentre Comment" name=btn_TeleCentreComment width="138"> 

<TABLE class=Table1 id=tbl_TelecentreComment 
style="Z-INDEX: 113; LEFT: 20px; WIDTH: 852px; POSITION: absolute; TOP: 1591px; HEIGHT: 250px" 
height=250 cellSpacing=1 cellPadding=1 width=852 border=0 
  >
  <TR>
    <TD class=Header1 align=middle colSpan=2 
      >&nbsp;&nbsp;TeleCentre Comment</TD></TR>
  <TR>
    <TD>&nbsp;Comment</TD>
    <TD>
      <OBJECT id=TeleCentreComment 
      style="LEFT: 1px; WIDTH: 772px; TOP: 1px; HEIGHT: 210px" tabIndex=4 
      classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="20426"><PARAM NAME="_ExtentY" VALUE="5556"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="-1"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR></TABLE>
</body>
</html>
