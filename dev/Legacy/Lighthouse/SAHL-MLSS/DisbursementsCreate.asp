
<%
Response.Expires = 0
sDatabase =Session("SQLDatabase") 
sUid = Session("UserID") 

set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
i_AddDisbursement= oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Add Disbursement",Session("UserName"))
i_UpdDisbursement = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Update Disbursement",Session("UserName"))
i_DelDisbursement = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Delete Disbursement",Session("UserName"))
i_GenerateDisbursements = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Generate Disbursements",Session("UserName"))
%>

<html>
<head>
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>
<meta name="VI60_defaultClientScript" content="VBScript">
<meta name="VI60_DefaultLoanScript" Content="VBScript">

<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<script ID="LoanEventHandlersVBS" LANGUAGE="vbscript">
<!--
dim i_CurrentLoanNbr
dim s_ReturnPage
dim v_BookMark
dim s_CurrentLoanNbr
dim b_loading 
dim b_AllDataLoaded
dim s_Action

if rs_open <> true then
	'Make sure user has logged on properly...if nt redirect him to logon page...
	 sessDSN= "DSN=<%= Session("DSN")%>"
    
	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   window.close
    end if
     sUserName = "<%= Session("UserID")%>"
  
	set conn = createobject("ADODB.Connection")
	set rs_Client  = createobject("ADODB.Recordset")
	set rs_Loan  = createobject("ADODB.Recordset")
	set rs_GridDisbursement  = createobject("ADODB.Recordset")
	set rs_DisbursementTotal  = createobject("ADODB.Recordset")
	set rs_ACBBank = createobject("ADODB.Recordset")
	set rs_ACBBranch = createobject("ADODB.Recordset")
	set rs_ACBType = createobject("ADODB.Recordset")


	'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [DisbursementsCreate.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	conn.Open sDSN
	rs_open = false
	rs_GridDisbursement_open = false 
	b_AllDataLoaded = false
end if


Sub SetAccessLightsServer
    
	sRes1 = "<%=i_AddDisbursement%>"
	if sRes1 = "Allowed" then
	      window.pic_AddDisbursement.src = "images/MLSAllowed.bmp"
	      window.pic_AddDisbursement.accesskey = "1"
	end if
	
	sRes1 = "<%=i_UpdDisbursement%>"
    if sRes1 = "Allowed" then
          window.pic_UpdateDisbursement.src = "images/MLSAllowed.bmp"
          window.pic_UpdateDisbursement.accesskey = "1"
	end if
	
	sRes1 = "<%=i_DelDisbursement%>"
    if sRes1 = "Allowed" then
          window.pic_DeleteDisbursement.src = "images/MLSAllowed.bmp"
          window.pic_DeleteDisbursement.accesskey = "1"
	end if
	
	sRes1 = "<%=i_GenerateDisbursements%>"
    if sRes1 = "Allowed" then
          window.pic_GenerateDisbursements.src = "images/MLSAllowed.bmp"
          window.pic_GenerateDisbursements.accesskey = "1"
	end if
	
End Sub


Sub ConfigureDetailGrid
	'Reconfigure the Grid..
	'Remove all columns
	Dim I 
	    
	document.body.style.cursor = "hand"
    
	For I = 0 to TrueDBGrid.Columns.Count - 1
		TrueDBGrid.Columns.Remove(0)
	Next
     
   	'Create then necessary columns...
	set tmpCol =  TrueDBGrid.Columns.Add(0)
	tmpCol.Width = 60
	tmpCol.Caption = "Number"
	tmpCol.DataField = rs_GridDisbursement.Fields("Number").name
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid.Columns.Add(1)
	tmpCol.Caption = "Prepared"
	tmpCol.Width =80
	tmpCol.DataField = rs_GridDisbursement.Fields("PreparedDate").name 
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid.Columns.Add(2)
	tmpCol.Caption = "Account Number"
	tmpCol.Width =160
	tmpCol.DataField = rs_GridDisbursement.Fields("AccountNumber").name 
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid.Columns.Add(3)
	tmpCol.Caption = "Account"
	tmpCol.Alignment = 3
   tmpCol.Width = 180
	tmpCol.DataField = rs_GridDisbursement.Fields("AccountName").name 
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(4)
	tmpCol.Caption = "Bank"
	tmpCol.Width = 180
	tmpCol.DataField = rs_GridDisbursement.Fields("BankDescription").name 
	tmpCol.Alignment = 3
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(5)
	tmpCol.Caption = "Branch"
	tmpCol.Width = 80
	tmpCol.DataField = rs_GridDisbursement.Fields("BranchCode").name 
	tmpCol.Alignment = 3
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(6)
	tmpCol.Caption = "Amount"
	tmpCol.Width = 20
	tmpCol.DataField = rs_GridDisbursement.Fields("Amount").name 
	tmpCol.Visible = True

	
	'Set the colors_GridDisbursement....
	TrueDBGrid.OddRowStyle.BackColor = &HC0FFFF
	TrueDBGrid.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBGrid.HoldFields
	
	document.body.style.cursor = "default"

End sub

Sub GetLoanDetails
	'''''''Client'''''''''''''''''''''''''''''''''''''''''''''''
	sSQL = "c_GetLoanClientDetails " & i_CurrentLoanNbr
	rs_Client.CursorLocation = 3
	rs_Client.CacheSize  =10
	rs_Client.Open sSQL,conn,adOpenDynamic
	
	'''''''''Loan'''''''''''''''''''''''''''''''''''''''''''''''''''
	sSQL = "c_GetLoanDetails " & i_CurrentLoanNbr
	rs_Loan.CursorLocation = 3
	rs_Loan.CacheSize  =10
	rs_Loan.Open sSQL,conn,adOpenDynamic
	

	'''''''''''''Disbursements''''''''''''''''''''''''''''''''''''''''
	sSQL = "c_GetDisbursementsByLoan " & i_CurrentLoanNbr
	rs_GridDisbursement.CursorLocation = 3
	rs_GridDisbursement.CacheSize  =10
	rs_GridDisbursement.Open sSQL,conn,adOpenDynamic
		
		
	TrueDBGrid.DataSource = rs_GridDisbursement
	rs_GridDisbursement_open = true
	set rs_DisbursementTotal = rs_GridDisbursement.NextRecordset 
		
	'''''''''''''''''ACBBANK'''''''''''''''''''''''''''''''''''''''''''''''''''
	sSQL = "SELECT * FROM ACBBANK (nolock)"
	rs_ACBBank.CursorLocation = 3
	rs_ACBBank.Open sSQL ,conn,adOpenStatic
	TrueDBCombo_ACBBank.RowSource = rs_ACBBank
	TrueDBCombo_ACBBank.ListField = rs_ACBBank.Fields("ACBBankDescription").name
	TrueDBCombo_ACBBank.BoundText = rs_ACBBank.Fields("ACBBankCode").Value
	TrueDBCombo_ACBBank.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_ACBBank.EvenRowStyle.BackColor = &HC0C0C0
	TrueDBCombo_ACBBank.BoundText = rs_ACBBank.Fields("ACBBankCode").Value
	TrueDBCombo_ACBBank.Refresh

	'''''''''''ACBBRANCH''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	sSQL = "SELECT ACBBranchCode,ACBBranchDescription,ACBBankCode FROM ACBBRANCH (nolock) WHERE ACBBankCode = " &  rs_ACBBank.Fields("ACBBankCode")
	rs_ACBBranch.CursorLocation = 3
	rs_ACBBranch.Open sSQL ,conn,adOpenStatic
	TrueDBCombo_ACBBranch.RowSource = rs_ACBBranch
	TrueDBCombo_ACBBranch.ListField = rs_ACBBranch.Fields("ACBBranchDescription").name
	TrueDBCombo_ACBBranch.BoundText = rs_ACBBranch.Fields("ACBBranchCode").Value
	TrueDBCombo_ACBBranch.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_ACBBranch.EvenRowStyle.BackColor = &HC0C0C0
	TrueDBCombo_ACBBranch.BoundText = rs_ACBBranch.Fields("ACBBranchCode").Value
	TrueDBCombo_ACBBranch.Refresh
	
	'''''''''''''''''ACBTYPE'''''''''''''''''''''''''''''''''''''''''''''''''''
	sSQL = "SELECT * FROM ACBTYPE (nolock)"
	rs_ACBType.CursorLocation = 3
	rs_ACBType.Open sSQL ,conn,adOpenStatic
	TrueDBCombo_ACBType.RowSource = rs_ACBType
	TrueDBCombo_ACBType.ListField = rs_ACBType.Fields("ACBTypeDescription").Name 
	TrueDBCombo_ACBType.BoundText = rs_ACBType.Fields("ACBTypeNumber").Value
	TrueDBCombo_ACBType.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_ACBType.EvenRowStyle.BackColor = &HC0C0C0
	TrueDBCombo_ACBType.BoundText = rs_ACBType.Fields("ACBTypeNumber").Value
	TrueDBCombo_ACBType.Refresh

end sub


Sub TrueDBCombo_ACBBank_ItemChange
  rs_ACBBranch.Close
	sSQL = "SELECT ACBBranchCode,ACBBranchDescription,ACBBankCode FROM ACBBRANCH (nolock) WHERE ACBBankCode = " & window.TrueDBCombo_ACBBank.BoundText & " ORDER BY ACBBranchCode"
	rs_ACBBranch.CursorLocation = 3
	rs_ACBBranch.Open sSQL ,conn,adOpenStatic

	TrueDBCombo_ACBBranch.RowSource = rs_ACBBranch
	TrueDBCombo_ACBBranch.ListField = rs_ACBBranch.Fields("ACBBranchDescription").name
	TrueDBCombo_ACBBranch.BoundText = rs_ACBBranch.Fields("ACBBranchCode").Value
	TrueDBCombo_ACBBranch.OddRowStyle.BackColor = &HC0FFFF
	TrueDBCombo_ACBBranch.EvenRowStyle.BackColor = &HC0C0C0

	if Ucase(Left(window.AccountName.Text,4)) = "THEK" or Ucase(Left(window.AccountName.Text,4)) = "MAIN" then
		TrueDBCombo_ACBBranch.BoundText = "042826"		
		TrueDBCombo_ACBBranch.SelText = "DURBAN NORTH"
		TrueDBCombo_ACBBranch.Refresh 
	else
		if rs_GridDisbursement.RecordCount > 0 then
			TrueDBCombo_ACBBranch.BoundText = rs_GridDisbursement.Fields("BranchCode").Value
			TrueDBCombo_ACBBranch.Refresh
		end if
	end if
	
	window.BankCode.Text = TrueDBCombo_ACBBank.BoundText
	window.BranchCode.Text = TrueDBCombo_ACBBranch.BoundText
	
End Sub


Sub TrueDBCombo_ACBBranch_ItemChange
  	window.BranchCode.Text = TrueDBCombo_ACBBranch.BoundText
End Sub


Sub AccountName_Change
	if Ucase(Left(window.AccountName.Text,4)) = "THEK" then
		window.AccountName.Text = "Thekwini Fund"
		window.AccountNumber.Text = "051355779"
		window.TrueDBCombo_ACBBank.BoundText = 15
		window.TrueDBCombo_ACBBank.Refresh
		'calls TrueDBCombo_ACBBank_ItemChange because of bank change
	end if
	
	if Ucase(Left(window.AccountName.Text,4)) = "MAIN" then
		window.AccountName.Text = "Main Street 65"
		window.AccountNumber.Text = "051380331"
		window.TrueDBCombo_ACBBank.BoundText = 15
		window.TrueDBCombo_ACBBank.Refresh
		'calls TrueDBCombo_ACBBank_ItemChange because of bank change
	end if

End Sub

Sub btn_Exit_onclick
	if btn_Exit.value = "Cancel" then
		btn_AddDisbursement.value = "Add Disbursement"
		btn_UpdateDisbursement.value = "Update Disbursement"
		btn_DeleteDisbursement.value = "Delete Disbursement"
		btn_Exit.innerText = "Exit"
		EnableControls
	    btn_FindBranch.disabled = true	
		DisableFields()
		rs_GridDisbursement.Requery
		set rs_DisbursementTotal = rs_GridDisbursement.NextRecordset 
		window.DisbursementTotal.Value  =  rs_DisbursementTotal.Fields("Total").Value		
		if rs_GridDisbursement.RecordCount > 0 then
			window.TrueDBGrid.Bookmark =  v_BookMark
		end if
				exit sub
	end if
	'go back to the calling page
	
	'msgbox s_ReturnPage
	if trim(s_ReturnPage) = "DisbursementTxns.asp" then
		s_Source = "Disbursements.asp"
	elseif  trim(s_ReturnPage) = "" then
		s_ReturnPage = "DisbursementTxns.asp"
	end if
	window.parent.frames("RegistrationPanel").location.href = s_ReturnPage & "?Number= " & CStr(i_CurrentLoanNbr ) & "&Source=" & s_Source
End Sub


Sub TrueDBCombo_ACBType_ItemChange
	window.AccountType.Text = window.TrueDBCombo_ACBType.BoundText   
End Sub

Sub window_onload

	b_loading = true
	    
	SetAccessLightsServer
	
	window.TrueDBCombo_ACBBank.style.visibility = "hidden"
	window.TrueDBCombo_ACBBranch.style.visibility = "hidden"
	window.TrueDBCombo_ACBType.style.visibility = "hidden"
	
	window.TrueDBCombo_ACBBank.style.visibility = "visible"
	window.TrueDBCombo_ACBBranch.style.visibility = "visible"
	window.TrueDBCombo_ACBType.style.visibility = "visible"
	
	 
	window.PreparedDate.DropDown.Visible = 1
	window.PreparedDate.Spin.Visible = 1
	
	window.ActionDate.DropDown.Visible = 1
	window.ActionDate.Spin.Visible = 1

	i_Nbr = "<%=Request.QueryString("Number")%>"
	s_Source = "<%=Request.QueryString("Source")%>"
	s_ReturnPage = s_Source
	i_CurrentLoanNbr = i_Nbr

	GetLoanDetails
	
	tbl_Client.focus 
	window.LoanNumber.Value = rs_Client.Fields("LoanNumber").Value
	window.LoanFirstNames.Text = rs_Client.Fields("ClientFirstNames").Value
	window.LoanSurname.Text = rs_Client.Fields("ClientSurname").Value
	
	tbl_Total.focus
	window.DisbursementTotal.Value  =  rs_DisbursementTotal.Fields("Total").Value	
	
	window.SPVDescription.Text = rs_Loan.Fields("SPVDescription").Value	
	
	ConfigureDetailGrid
 
 	b_AllDataLoaded = true

End Sub



Sub TrueDBGrid_RowColChange(LastRow, LastCol)
	'msgbox "TrueDBGrid_RowColChange" & b_AllDataLoaded
if b_AllDataLoaded = true then

  if rs_GridDisbursement.RecordCount > 0 then
		window.DisbursementNumber.Value  = rs_GridDisbursement.Fields("Number").Value
		window.PreparedDate.Value  = rs_GridDisbursement.Fields("PreparedDate").Value
		window.ActionDate.Value = rs_GridDisbursement.Fields("ActionDate").Value
		window.SPVDescription.Text = rs_GridDisbursement.Fields("SPVDescription").Value
		window.AccountName.Text  = rs_GridDisbursement.Fields("AccountName").Value 
		window.AccountNumber.Text = rs_GridDisbursement.Fields("AccountNumber").Value
		window.AccountType.Text = rs_GridDisbursement.Fields("AccountTypeNumber").Value
		window.BankCode.Text = rs_GridDisbursement.Fields("BankCode").Value
		window.BranchCode.Text = rs_GridDisbursement.Fields("BranchCode").Value
		window.DisbursementAmount.Value  = rs_GridDisbursement.Fields("Amount").Value
	
		window.TrueDBCombo_ACBBank.BoundText = rs_GridDisbursement.Fields("BankCode").Value
		window.TrueDBCombo_ACBBank.Refresh
		'TrueDBCombo_ACBBank_ItemChange will be called becuase of this change
		window.TrueDBCombo_ACBType.BoundText = rs_GridDisbursement.Fields("AccountType").Value
		window.TrueDBCombo_ACBType.Refresh 

	else
		window.DisbursementNumber.Value = 0
		window.PreparedDate.Value = now()
		window.ActionDate.Value = ""
		'window.SPVDescription.Text = ""
		window.AccountName.Text  = ""
		window.AccountNumber.Text = ""
		'window.BankCode.Text = ""
		'window.BranchCode.Text = ""
		window.DisbursementAmount.Value = 0.00
		
		window.btn_UpdateDisbursement.disabled = true
		window.btn_DeleteDisbursement.disabled = true

	end if
	
	end if
End Sub

Sub DisableControls(s_Action)

	if s_Action = "Add" then
		window.btn_UpdateDisbursement.disabled = true
		window.btn_DeleteDisbursement.disabled = true
		window.btn_UpdateDisbursement.style.visibility = "hidden"
		window.btn_DeleteDisbursement.style.visibility = "hidden"
		window.pic_UpdateDisbursement.style.visibility = "hidden"
		window.pic_DeleteDisbursement.style.visibility = "hidden"
		
	elseif s_Action = "Update" then
		window.btn_AddDisbursement.disabled = true
		window.btn_DeleteDisbursement.disabled = true
		window.btn_AddDisbursement.style.visibility = "hidden"
		window.btn_DeleteDisbursement.style.visibility = "hidden"
		window.pic_AddDisbursement.style.visibility = "hidden"
		window.pic_DeleteDisbursement.style.visibility = "hidden"
	elseif s_Action = "Delete" then
		window.btn_UpdateDisbursement.disabled = true
		window.btn_AddDisbursement.disabled = true
		window.btn_UpdateDisbursement.style.visibility = "hidden"
		window.btn_AddDisbursement.style.visibility = "hidden"
		window.pic_UpdateDisbursement.style.visibility = "hidden"
		window.pic_AddDisbursement.style.visibility = "hidden"
	end if
		window.TrueDBGrid.Enabled = false

End Sub

Sub EnableControls

	window.btn_AddDisbursement.disabled = false 
	window.btn_UpdateDisbursement.disabled = false
	window.btn_DeleteDisbursement.disabled = false

	window.TrueDBGrid.Enabled = true
	     
	window.btn_AddDisbursement.style.visibility = "visible"
	window.btn_UpdateDisbursement.style.visibility = "visible"
	window.btn_DeleteDisbursement.style.visibility = "visible"
	window.pic_AddDisbursement.style.visibility = "visible"
	window.pic_UpdateDisbursement.style.visibility = "visible"
	window.pic_DeleteDisbursement.style.visibility = "visible"
  
End Sub


sub ClearFields
	window.DisbursementNumber.Value = 0
	window.PreparedDate.Value = now()
	window.ActionDate.Value = ""
	'window.SPVDescription.Text = ""
	window.AccountName.Text  = ""
	window.AccountNumber.Text = ""
	window.TrueDBCombo_ACBBank.BoundText = 0
	window.TrueDBCombo_ACBBank.Refresh
	'window.BranchDescription.Text = ""
	window.BranchCode.Text = ""
	window.DisbursementAmount.Value = 0.00
end sub


sub EnableFields
	'window.PreparedDate.Enabled = true
	'window.ActionDate.Enabled = true
	'window.SPVDescription.Enabled = true
	window.AccountName.Enabled = true
	window.AccountNumber.Enabled = true
	window.TrueDBCombo_ACBBank.Enabled = true
	window.TrueDBCombo_ACBBranch.Enabled = true
	window.TrueDBCombo_ACBType.Enabled = true
	window.DisbursementAmount.Enabled = true
	window.BranchCode.Enabled = true
end sub

sub DisableFields
   'window.PreparedDate.Enabled = false
   'window.ActionDate.Enabled = false
  	'window.SPVDescription.Enabled = false
	window.AccountName.Enabled = false
	window.AccountNumber.Enabled = false
	window.TrueDBCombo_ACBBank.Enabled = false
	window.TrueDBCombo_ACBBranch.Enabled = false
	window.TrueDBCombo_ACBType.Enabled = false
    window.DisbursementAmount.Enabled = false
	window.BranchCode.Enabled = false
   
end sub

Function ValidateFields
	ValidateFields = -1

	if window.PreparedDate  < date() then
		msgbox "Prepared Date cannot be less than today....!!!"
		window.PrepareDate.focus
		exit Function
	end if

	if trim(window.AccountName.Text) = "" then
		msgbox "Account Name cannot be empty..!!"
		window.AccountName.focus 
		exit Function
	end if
	
	if IsNumeric(window.AccountNumber.Text) = false then
		msgbox "Account Number must be a numeric field..!!"
		window.AccountNumber.focus 
		exit Function
	end if

	
	if window.DisbursementAmount <= 0.0 then
		msgbox "Amount must be entered, and greater than zero..!!"
		window.DisbursementAmount.focus 
		exit Function
	end if

	ValidateFields = 0 
End function

Sub btn_AddDisbursement_onclick

	if window.pic_AddDisbursement.accesskey = "0" then
		window.status = "Access denied to " & window.btn_AddDetail.title
		exit sub
	end if
	
	v_BookMark = window.TrueDBGrid.Bookmark
	
	if btn_AddDisbursement.value = "Add Disbursement" then
		DisableControls("Add")
		ClearFields
		EnableFields
		btn_FindBranch.disabled = false
		window.btn_Exit.innerText = "Cancel"
		window.PreparedDate.value = date()
		window.AccountName.focus 
		btn_AddDisbursement.value = "Commit"  
	elseif btn_AddDisbursement.value = "Commit" then
	
		if ValidateFields = -1 then
		   exit sub
		end if
				
		call MaintainDetailRecord("Add")
	
		'Clean up...
		btn_AddDisbursement.value = "Add Disbursement"
		btn_Exit.innerText = "Exit"
					
		DisableFields()
		b_loading  = true
		rs_GridDisbursement.Requery
		set rs_DisbursementTotal = rs_GridDisbursement.NextRecordset 
		window.DisbursementTotal.Value  =  rs_DisbursementTotal.Fields("Total").Value		
		EnableControls
		btn_FindBranch.disabled = true
	End if

End Sub


Function MaintainDetailRecord(s_Action)
	
	Dim i_res

	UpdateLoanRecord = -1

	document.body.style.cursor = "hand"
	   
	i_res = 0     
	set com = createobject("ADODB.Command")
	set prm = createobject("ADODB.Parameter")
	set rs_temp = createobject("ADODB.Recordset")

	'Cannot use OLE DB Provider as it appears that it does not return a recordset
	sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS [DisbursementsCreate.asp 2];uid=<%= Session("UserID")%>"
	'    sDSN = "Provider=SQLOLEDB.1;Application Name='MLS System Version1 [loandetail.asp]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	
	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc

	if s_Action = "Add" then
		sSQL = "c_AddDisbursementTran"  
	elseif s_Action = "Update" then
		sSQL = "c_UpdDisbursementTran" 
	elseif s_Action = "Delete" then
		sSQL = "c_DelDisbursementTran" 
	end if

	
	com.CommandText = sSQL
	if s_Action = "Add" then
		set prm = com.CreateParameter ( "LoanNumber",19,1,,window.LoanNumber.Value) 'AdVarchar , adParamInput
		com.Parameters.Append prm
	
	elseif s_Action = "Update" then
		set prm = com.CreateParameter ( "DisbursementNumber",19,1,,window.DisbursementNumber.Value) 'AdVarchar , adParamInput
		com.Parameters.Append prm
		set prm = com.CreateParameter ( "LoanNumber",19,1,,window.LoanNumber.Value) 'AdVarchar , adParamInput
		com.Parameters.Append prm
	
	elseif s_Action = "Delete" then
		set prm = com.CreateParameter ( "DisbursementNumber",19,1,,window.DisbursementNumber.Value) 'AdVarchar , adParamInput
		com.Parameters.Append prm
	end if

	if s_Action = "Add" or s_Action = "Update" then 
					
		'set prm = com.CreateParameter ("SPVDescription",200,1,50,window.SPVDescription.Text) 'AdVarchar , adParamInput
		'com.Parameters.Append prm
	
		'set prm = com.CreateParameter ("DisbursementBankDescription",200,1,50,window.BankDescription.Text) 'AdVarchar , adParamInput
		set prm = com.CreateParameter ("ACBBankCode",19,1,,window.TrueDBCombo_ACBBank.BoundText) 'AdVarchar , adParamInput
		com.Parameters.Append prm
	
		'set prm = com.CreateParameter ("DisbursementBranchDescription",200,1,50,window.BranchDescription.Text) 'AdVarchar , adParamInput
		set prm = com.CreateParameter ("ACBBranchCode",200,1,10,window.TrueDBCombo_ACBBranch.BoundText) 'AdVarchar , adParamInput
		com.Parameters.Append prm
		
		set prm = com.CreateParameter ("ACBTypeNumber",200,1,10,window.TrueDBCombo_ACBType.BoundText) 'AdVarchar , adParamInput
		com.Parameters.Append prm

		s_date = Mid(window.PreparedDate.Text, 4, 2) & "/" & Mid(window.PreparedDate.Text, 1, 2) & "/" & Mid(window.PreparedDate.Text, 7, 4)
		set prm = com.CreateParameter ("DisbursementPreparedDate",129,1,10,s_date) 'AdVarchar , adParamInput
		com.Parameters.Append prm
		
		set prm = com.CreateParameter ("DisbursementAccountName",200,1,50,window.AccountName.Text) 'AdVarchar , adParamInput
		com.Parameters.Append prm
	
		set prm = com.CreateParameter ("DisbursementAccountNumber",200,1,50,window.AccountNumber.Text) 'AdVarchar , adParamInput
		com.Parameters.Append prm

		set prm = com.CreateParameter ("DisbursementAmount",5,1,,window.DisbursementAmount.Value) ' AdUnsigned Int
		com.Parameters.Append prm
		
		'set prm = com.CreateParameter ("DisbursementLocked",19,1,,0) ' AdUnsigned Int
		'com.Parameters.Append prm

		
	end if

	set rs_temp = com.Execute 
	
	document.body.style.cursor = "default"
	UpdateLoanRecord = 0
	
End Function


Sub btn_UpdateDisbursement_onclick

	if window.pic_UpdateDisbursement.accesskey = "0" then
		window.status = "Access denied to " & window.btn_UpdateDisbursement.title
		exit sub
	 end if
	 
	v_BookMark = window.TrueDBGrid.Bookmark
 
	if btn_UpdateDisbursement.value = "Update Disbursement" then
		DisableControls("Update")
		btn_FindBranch.disabled = false
		EnableFields	
		window.PreparedDate.Value = now()
		window.btn_Exit.innerText = "Cancel"
		btn_UpdateDisbursement.value = "Commit"
   elseif btn_UpdateDisbursement.value = "Commit" then
   
		if ValidateFields = -1 then
		   exit sub
		end if
		
		call MaintainDetailRecord("Update")
		
	
		'Clean up...
		btn_UpdateDisbursement.value = "Update Disbursement"
		btn_Exit.innerText = "Exit"
		EnableControls
		btn_FindBranch.disabled = true
		DisableFields()
		b_loading  = true
		rs_GridDisbursement.Requery
		set rs_DisbursementTotal = rs_GridDisbursement.NextRecordset 
		window.DisbursementTotal.Value  =  rs_DisbursementTotal.Fields("Total").Value		
		window.TrueDBGrid.Bookmark =  v_BookMark
		
   End if
   
End Sub

Sub btn_DeleteDisbursement_onclick

	if window.pic_DeleteDisbursement.accesskey = "0" then
		window.status = "Access denied to " & window.btn_DeleteDisbursement.title
		exit sub
	end if

	v_BookMark = window.TrueDBGrid.Bookmark

	if btn_DeleteDisbursement.value = "Delete Disbursement" then
		
		DisableControls("Delete")
		window.btn_Exit.innerText = "Cancel"
		btn_DeleteDisbursement.value = "Commit"  
		
	elseif btn_DeleteDisbursement.value = "Commit" then
	
		call MaintainDetailRecord("Delete")
		'Clean up...
		btn_DeleteDisbursement.value = "Delete Disbursement"
		btn_Exit.innerText = "Exit"
		EnableControls
		b_loading  = true
		rs_GridDisbursement.Requery
		
		set rs_DisbursementTotal = rs_GridDisbursement.NextRecordset 
		window.DisbursementTotal.Value  =  rs_DisbursementTotal.Fields("Total").Value		
		
		if rs_GridDisbursement.RecordCount > v_BookMark then
			window.TrueDBGrid.Bookmark = v_BookMark
		end if
		
   End if
End Sub


Sub btn_GenerateDisbursements_onclick

   window.status = ""
	if window.pic_GenerateDisbursements.accesskey <> "1" then
	  window.status = "Access denied to " & window.btn_GenerateDisbursements.title
      exit sub
	end if
    'ActionDate=""
	 'window.parent.frames("RegistrationPanel").location.href = "DisbursementsView.asp?ActionDate= " & ActionDate & "&Source=Disbursements.asp"
	 
	 window.tbl_Client.style.visibility = "hidden"
 	 window.tbl_DisbursementDetals.style.visibility = "hidden"
 	 window.tbl_Generate.style.visibility = "hidden"
	 window.tbl_Button.style.visibility = "hidden"
	 window.tbl_Total.style.visibility = "hidden"
	 window.TrueDBGrid.style.visibility = "hidden"
	 window.TrueDBGrid.style.visibility = "hidden"
	 window.TrueDBCombo_ACBBank.style.visibility = "hidden"
	 window.TrueDBCombo_ACBBranch.style.visibility = "hidden"
	 window.TrueDBCombo_ACBType.style.visibility = "hidden"
	 window.BranchCode.style.visibility = "hidden"
	 window.btn_Print.style.visibility = "hidden"
	 window.btn_AddDisbursement.style.visibility = "hidden"
	 window.btn_GenerateDisbursements.style.visibility = "hidden"
	 window.btn_UpdateDisbursement.style.visibility = "hidden"
	 window.btn_DeleteDisbursement.style.visibility = "hidden"
	 window.btn_Exit.style.visibility = "hidden"
	 window.pic_AddDisbursement.style.visibility = "hidden"
	 window.pic_DeleteDisbursement.style.visibility = "hidden"
	 window.pic_GenerateDisbursements.style.visibility = "hidden"
	 window.pic_UpdateDisbursement.style.visibility = "hidden"
	  
 	  window.parent.frames("RegistrationPanel").location.href = "DisbursementsGenerate.asp?Source=DisbursementsCreate.asp&Number= " & CStr(i_CurrentLoanNbr) 

End Sub
	
Sub BranchCode_Change

End Sub
Sub BranchCode_KeyUp(i,	x )

End Sub

Sub btn_FindBranch_onclick

		rs_ACBBranch.MoveFirst
		
		rs_ACBBranch.Find "ACBBranchCode >='" & window.BranchCode.Text & "'"
		i_Book = rs_ACBBranch.Bookmark
		
	   window.TrueDBCombo_ACBBranch.Bookmark = i_Book


End Sub


Sub btn_Print_onclick
 
	sSQL = "c_GetDisbursementsByLoan " & i_CurrentLoanNbr
	set rs_LoanDisbursement  = createobject("ADODB.Recordset")
	rs_LoanDisbursement.CursorLocation = 3
	rs_LoanDisbursement.CacheSize  =10
	rs_LoanDisbursement.Open sSQL,conn,adOpenDynamic
	set rs_Total = rs_LoanDisbursement.NextRecordset 
	
	if rs_LoanDisbursement.RecordCount = 0 then
		exit sub
	end if
	
	Dim aDateTime
	aDateTime = Now
	
	Dim HTML
	HTML = "<!DOCTYPE HTML PUBLIC '-//IETF//DTD HTML//EN'>"
	HTML = HTML & "<html>"
	HTML = HTML & "<head>"
	HTML = HTML & "<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>"
	HTML = HTML & "<title>Disbursements Create</title>"
	HTML = HTML & "</head>"

	HTML = HTML & "<body>"
	HTML = HTML & "<!-- MeadCo ScriptX -->"
	HTML = HTML & "<object id='factory' viewastext  style='display:none'"
	HTML = HTML & "   classid='clsid:1663ed61-23eb-11d2-b92f-008048fdd814'"
	HTML = HTML & "      codebase='ocx/ScriptX.cab#Version=6,0,0,421'>"
	HTML = HTML & "</object>"

	HTML = HTML & "<p><b><u><font face=Arial size=3>Disbursements Create</font></u></b></p>"
	HTML = HTML & "Print Date: " & FormatDateTime( aDateTime, VbLongDate) & " " & FormatDateTime( aDateTime, VbLongTime) & "<p>"
	HTML = HTML & "Loan Number				= " & window.LoanNumber.Value  & "<br>"
	HTML = HTML & "Client First Names	= " & window.LoanFirstNames.Text & "<br>"
	HTML = HTML & "Client Surname			= " & window.LoanSurname.Text & "<br>"
	
	rs_LoanDisbursement.MoveFirst
	Do Until rs_LoanDisbursement.EOF
			HTML = HTML & "<p>Disbursement Number = " & rs_LoanDisbursement.Fields("Number").Value & "<br>"
			'HTML = HTML & "Prepared Date       = " & rs_LoanDisbursement.Fields("PreparedDate").Value & "<br>"
			'HTML = HTML & "Action Date         = " & rs_LoanDisbursement.Fields("ActionDate").Value & "<br>"
			'HTML = HTML & "SPV                 = " & rs_LoanDisbursement.Fields("SPVDescription").Value & "<br>"
			HTML = HTML & "Account Name        = " & rs_LoanDisbursement.Fields("AccountName").Value & "<br>"
			HTML = HTML & "Account Number      = " & rs_LoanDisbursement.Fields("AccountNumber").Value & "<br>"
			'HTML = HTML & "Account Type        = " & rs_LoanDisbursement.Fields("AccountType").Value & "<br>"
			'HTML = HTML & "Bank Code           = " & rs_LoanDisbursement.Fields("BankCode").Value & "<br>"
			HTML = HTML & "Bank Description    = " & rs_LoanDisbursement.Fields("BankDescription").Value & "<br>"
			HTML = HTML & "Branch Code         = " & rs_LoanDisbursement.Fields("BranchCode").Value & "<br>"
			'HTML = HTML & "BranchDescription   = " & rs_LoanDisbursement.Fields("BranchDescription").Value & "<br>"
			HTML = HTML & "Disbursement Amount = " & rs_LoanDisbursement.Fields("Amount").Value & "</p>"
		rs_LoanDisbursement.MoveNext 
	Loop
	
	HTML = HTML & "<p>Total Amount = " & rs_Total.Fields("Total").Value & "</p><br>"

	set rs_LoanDisbursement = nothing
	set rs_Total = nothing    	

	HTML = HTML & "<INPUT id=btnPrint type=button value=Print>"
	HTML = HTML & "<INPUT id=btnClose type=button value=Close>"

	HTML = HTML & vbNewLine
	HTML = HTML & "<" & "script language=VBScript" & ">"
	HTML = HTML & vbNewLine
	HTML = HTML & "<" & "!--"
	HTML = HTML & vbNewLine
	HTML = HTML & "Dim x"
	HTML = HTML & vbNewLine
	HTML = HTML & "x=vbNullString"
	HTML = HTML & vbNewLine
	HTML = HTML & "Sub window_onload"
	HTML = HTML & vbNewLine
	HTML = HTML & "window.factory.printing.header = vbNullString"
	HTML = HTML & vbNewLine
	HTML = HTML & "window.factory.printing.footer = vbNullString"
	HTML = HTML & vbNewLine
	HTML = HTML & "End Sub"
	HTML = HTML & vbNewLine
	HTML = HTML & "Sub btnPrint_onclick"
	HTML = HTML & vbNewLine
	HTML = HTML & "window.factory.printing.Print(true)"
	HTML = HTML & vbNewLine
	HTML = HTML & "End Sub"
	HTML = HTML & vbNewLine
	HTML = HTML & "Sub btnClose_onclick"
	HTML = HTML & vbNewLine
	HTML = HTML & "window.close"
	HTML = HTML & vbNewLine
	HTML = HTML & "End Sub"
	HTML = HTML & vbNewLine
	HTML = HTML & "-->"
	HTML = HTML & vbNewLine
	HTML = HTML & "<" & "/script>"
	HTML = HTML & vbNewLine
	
	HTML = HTML & "</body>"
	HTML = HTML & "</html>"
		
	Dim win1
	set win1 = window.open ("","Letter", "width=800,height=600,resizable=yes,scrollbars=yes,menubar=no,toolbar=no")
	win1.document.open ("text/html")
	win1.document.write(HTML)
	win1.document.close
	
End Sub

-->
</script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<body bottomMargin="0" rightMargin="0" topMargin="0" leftMargin="0" class="Generic">

<p>
<table border="0" cellPadding="1" cellSpacing="1" style="HEIGHT: 86px; LEFT: 20px; POSITION: absolute; TOP: 0px; WIDTH: 635px; Z-INDEX: 111" width="75%" id="tbl_Client" class="Table1">
  
  <tr>
    <td align="right" noWrap>
    &nbsp;Loan Number</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 id=LoanNumber 
      style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 99px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2619"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0;;Null"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2012807169"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap>Client First Names</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D 
      id=LoanFirstNames style="HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 411px" 
      tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="10874"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap>Client Surname</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D id=LoanSurname 
      style="HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 411px" tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="10874"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr></table>
<OBJECT classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAYAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAHAAAAAQAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAgAAAAAAAAAAEAAAAwMDAAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAIAAAAAAAAAABAAAAP//xgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAAMG24AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAwMDAAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAP//xgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAAMG24AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAADBtuAD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB0AAABIZWFkaW5nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB4AAABGb290aW5nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB8AAABTZWxlY3RlZAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAAABDYXB0aW9uAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACEAAABIaWdobGlnaHRSb3cAAAAAAAAAAAAAAAAAAAAAAAAAACIAAABFdmVuUm93AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACMAAABPZGRSb3cAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACQAAAALAAAA//8AAAMAAAAAAAAACwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAP//AAADAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAAAAAAAAsAAAAAAAAAAwAAAAEAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAAeAAAAMgAAAERyYWcgYSBjb2x1bW4gaGVhZGVyIGhlcmUgdG8gZ3JvdXAgYnkgdGhhdCBjb2x1bW4AAAADAAAAAAAAAAMAAAAAAAAAAwAAAMgAAAADAAAAAAAAAAMAAAD3ogYAAwAAAJDQAwA9AAAAAAAAAAsAAABUcnVlREJHcmlkAAIAAAAMAAAAQWxsb3dBZGROZXcALwAAAAwAAABBbGxvd0Fycm93cwABAAAADAAAAEFsbG93RGVsZXRlAAQAAAAMAAAAQWxsb3dVcGRhdGUAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQAI/v//DAAAAEJvcmRlclN0eWxlAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAnAAAAAsAAABEYXRhTWVtYmVyACUAAAAJAAAARGF0YU1vZGUAuwAAAAkAAABEYXRhVmlldwDEAAAAEgAAAERlYWRBcmVhQmFja0NvbG9yAAoAAAAMAAAARGVmQ29sV2lkdGgAUAAAAA0AAABFZGl0RHJvcERvd24AXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkADAAAAAPAAAARXhwb3NlQ2VsbE1vZGUAkQAAAAoAAABGb290TGluZXMAwgAAAA8AAABHcm91cEJ5Q2FwdGlvbgAMAAAACgAAAEhlYWRMaW5lcwCYAAAACwAAAEluc2VydE1vZGUAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAsQAAAAoAAABMYXlvdXRVUkwASgAAAA4AAABNYXJxdWVlVW5pcXVlAM4AAAAIAAAATWF4Um93cwCjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgCEAAAADAAAAE11bHRpU2VsZWN0AGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUAlwAAABEAAABQaWN0dXJlQWRkbmV3Um93AJUAAAASAAAAUGljdHVyZUN1cnJlbnRSb3cAswAAABEAAABQaWN0dXJlRm9vdGVyUm93ALIAAAARAAAAUGljdHVyZUhlYWRlclJvdwCWAAAAEwAAAFBpY3R1cmVNb2RpZmllZFJvdwCwAAAAEwAAAFBpY3R1cmVTdGFuZGFyZFJvdwC0AAAACwAAAFByaW50SW5mb3MADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAIwAAAAcAAABTcGxpdHMAMQAAABAAAABUYWJBY3Jvc3NTcGxpdHMAMgAAAAoAAABUYWJBY3Rpb24AmQAAABcAAABUcmFuc3BhcmVudFJvd1BpY3R1cmVzADMAAAAQAAAAV3JhcENlbGxQb2ludGVyANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA 
height=164 id=TrueDBGrid 
style="HEIGHT: 164px; LEFT: 20px; POSITION: absolute; TOP: 90px; WIDTH: 865px; Z-INDEX: 110" 
width=871></OBJECT>
</p>
<OBJECT classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAAAAAAD//////////wAAAAAHAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwABPj///8ZAAAAAAAAAPr9//8AAAAAAAAAAB0AAABIZWFkaW5nAIDSWgSg0VoEENJaBP///////////////x4AAABGb290aW5nAAACGAAAAAAAAAAAAAAAAAAAAAAAQAAAAB8AAABTZWxlY3RlZAAAAAD//xsAAAAAAAAAkQAAAAAA/f///yAAAABDYXB0aW9uAP///////////////wAAAAAAAH4AAAAAACEAAABIaWdobGlnaHRSb3cAAAAAAAAAAAAAAAABAAAAgICAACIAAABFdmVuUm93AHAAAABRAQAAAAAAAAEAAAAww1oE7P///yMAAABPZGRSb3cABEAAAAAxAAAAgIFMB6DUWgQMAAAAFQAAACQAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAsAAAD//wAAAwAAAAAAAAAeAAAAEwAAAEFDQkJhbmtEZXNjcmlwdGlvbgAAHgAAAAwAAABBQ0JCYW5rQ29kZQALAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAAAAAAAAsAAAD//wAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADQBwAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAAAAAAALAAAAAAAAADIAAAAAAAAAFAAAAFRydWVEQkNvbWJvX0FDQkJhbmsAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQCFAAAADwAAAEF1dG9Db21wbGV0aW9uAIIAAAANAAAAQXV0b0Ryb3Bkb3duAAj+//8MAAAAQm9yZGVyU3R5bGUAcwAAAAwAAABCb3VuZENvbHVtbgD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAIEAAAALAAAAQ29tYm9TdHlsZQAlAAAACQAAAERhdGFNb2RlAAoAAAAMAAAARGVmQ29sV2lkdGgAwQAAABEAAABEcm9wZG93blBvc2l0aW9uAIgAAAAJAAAARWRpdEZvbnQAXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkAJEAAAAKAAAARm9vdExpbmVzAAwAAAAKAAAASGVhZExpbmVzAGUAAAAPAAAASW50ZWdyYWxIZWlnaHQAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAhwAAAAwAAABMaW1pdFRvTGlzdABjAAAACgAAAExpc3RGaWVsZADFAAAABwAAAExvY2tlZAC8AAAAEgAAAE1hdGNoRW50cnlUaW1lb3V0AKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAnAAAAAoAAABSb3dNZW1iZXIAgwAAAAwAAABSb3dUcmFja2luZwAjAAAABwAAAFNwbGl0cwADAgAAEAAAAF9Ecm9wZG93bkhlaWdodAACAgAADwAAAF9Ecm9wZG93bldpZHRoANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA 
height=115 id=TrueDBCombo_ACBBank 
style="BACKGROUND-COLOR: #ffd69d; HEIGHT: 115px; LEFT: 176px; POSITION: absolute; TOP: 421px; WIDTH: 241px; Z-INDEX: -97" 
tabIndex=6 width=241></OBJECT>
<OBJECT classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAcAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAIAAAAAQAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACQAAAAEAAAAAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAoAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAALAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADAAAAAIAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA0AAAADAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAOAAAABQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADwAAAAcAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABAAAAAGAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAARAAAACAAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEgAAAAkAAAAAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABMAAAAKAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAUAAAABAAAAAAAAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABYAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAXAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABkAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAaAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABwAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHgAAAB0AAADAAgEAAAIAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB8AAAAdAAAAwAABAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAgAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIQAAAB4AAAAAAQAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACIAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAjAAAAHQAAAIAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAJAAAAB0AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAAAAAA//////////8BAAAAAAAAAAAAAAABAAAA/v///wQAAAAAAAAAAAAAAAUAAAD9////AgAAAAAAAAAAAAAAAgAAAOr///8DAAAAAAAAAAAAAAADAAAA/P///wUAAAAAAAAAAAAAAP/////7////BgAAAAAAAAAAAAAABAAAAPr///8HAAAAAAAAAAAAAAD/////8f///wgAAAAAAAAAAAAAAAYAAADv////CQAAAAAAAAAAAAAABwAAAO7///8KAAAAAAAAAAAAAAAIAAAA+f///wsAAAABAAAAAAAAAP/////r////FAAAAAEAAAAAAAAA//////j///8MAAAAAQAAAAAAAAD/////6f///w0AAAABAAAAAAAAAP/////3////DgAAAAEAAAAAAAAA//////b///8QAAAAAQAAAAAAAAD/////9f///w8AAAABAAAAAAAAAP/////w////EQAAAAEAAAAAAAAA/////+3///8SAAAAAQAAAAAAAAD/////7P///xMAAAABAAAAAAAAAP/////0////GAAAAAEAAAABAAAA//////P///8VAAAAAQAAAAEAAAD/////6P///xYAAAABAAAAAQAAAP/////y////FwAAAAEAAAABAAAA//////T///8cAAAAAQAAAAIAAAD/////8////xkAAAABAAAAAgAAAP/////o////GgAAAAEAAAACAAAA//////L///8bAAAAAQAAAAIAAAD/////CAAAAE5vcm1hbAAEAAAAABUAAAAAAAAA/////wAAAAAAAAAAHQAAAEhlYWRpbmcACAAAgAUAAIAAAAAAAAAAAAAAAABITUwHHgAAAEZvb3RpbmcAQAAAADEAAAAoTUwH0IJaBAkAAAASAAAAHwAAAFNlbGVjdGVkAAAAgAUAAIAhAAAAAAAAADEAAAAQAAAAIAAAAENhcHRpb24AMQAAACAAAAAwJ1gEMCdYBKDwWgSA8FoEIQAAAEhpZ2hsaWdodFJvdwAAAABIJ1gESCdYBAAAAAAAAAAAIgAAAEV2ZW5Sb3cAAAAAAAAAAAA0AAAAAAAAAIAEAAAxAAAAIwAAAE9kZFJvdwAHMQAAALAAAAB4J1gEeCdYBEAAAAAxAAAAJAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAP//AAADAAAAAAAAAB4AAAAVAAAAQUNCQnJhbmNoRGVzY3JpcHRpb24AAAAAHgAAAA4AAABBQ0JCcmFuY2hDb2RlAAAACwAAAAAAAAAEAAAAAAAAAAMAAADoAwAACwAAAAAAAAALAAAA//8AAB4AAAABAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAA0AcAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAMgAAAADAAAAAAAAAAMAAAAAAAAACwAAAAAAAAAyAAAAAAAAABYAAABUcnVlREJDb21ib19BQ0JCcmFuY2gAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQCFAAAADwAAAEF1dG9Db21wbGV0aW9uAIIAAAANAAAAQXV0b0Ryb3Bkb3duAAj+//8MAAAAQm9yZGVyU3R5bGUAcwAAAAwAAABCb3VuZENvbHVtbgD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAIEAAAALAAAAQ29tYm9TdHlsZQAlAAAACQAAAERhdGFNb2RlAAoAAAAMAAAARGVmQ29sV2lkdGgAwQAAABEAAABEcm9wZG93blBvc2l0aW9uAIgAAAAJAAAARWRpdEZvbnQAXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkAJEAAAAKAAAARm9vdExpbmVzAAwAAAAKAAAASGVhZExpbmVzAGUAAAAPAAAASW50ZWdyYWxIZWlnaHQAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAhwAAAAwAAABMaW1pdFRvTGlzdABjAAAACgAAAExpc3RGaWVsZADFAAAABwAAAExvY2tlZAC8AAAAEgAAAE1hdGNoRW50cnlUaW1lb3V0AKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAnAAAAAoAAABSb3dNZW1iZXIAgwAAAAwAAABSb3dUcmFja2luZwAjAAAABwAAAFNwbGl0cwADAgAAEAAAAF9Ecm9wZG93bkhlaWdodAACAgAADwAAAF9Ecm9wZG93bldpZHRoANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA 
height=231 id=TrueDBCombo_ACBBranch 
style="BACKGROUND-COLOR: #ffd69d; FONT-SIZE: smaller; HEIGHT: 231px; LEFT: 166px; POSITION: absolute; TOP: 449px; VISIBILITY: visible; WIDTH: 350px; Z-INDEX: 100" 
tabIndex=7 width=350></OBJECT>
<OBJECT classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAAcAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAIAAAAAQAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACQAAAAEAAAAAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAoAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAALAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADAAAAAIAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA0AAAADAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAOAAAABQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADwAAAAcAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABAAAAAGAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAARAAAACAAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEgAAAAkAAAAAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABMAAAAKAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAUAAAABAAAAAAAAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABYAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAXAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABkAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAaAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABwAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHgAAAB0AAADAAgEAAAIAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB8AAAAdAAAAwAABAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAgAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIQAAAB4AAAAAAQAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACIAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAjAAAAHQAAAIAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAJAAAAB0AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAAAAAA//////////8BAAAAAAAAAAAAAAABAAAA/v///wQAAAAAAAAAAAAAAAUAAAD9////AgAAAAAAAAAAAAAAAgAAAOr///8DAAAAAAAAAAAAAAADAAAA/P///wUAAAAAAAAAAAAAAP/////7////BgAAAAAAAAAAAAAABAAAAPr///8HAAAAAAAAAAAAAAD/////8f///wgAAAAAAAAAAAAAAAYAAADv////CQAAAAAAAAAAAAAABwAAAO7///8KAAAAAAAAAAAAAAAIAAAA+f///wsAAAABAAAAAAAAAP/////r////FAAAAAEAAAAAAAAA//////j///8MAAAAAQAAAAAAAAD/////6f///w0AAAABAAAAAAAAAP/////3////DgAAAAEAAAAAAAAA//////b///8QAAAAAQAAAAAAAAD/////9f///w8AAAABAAAAAAAAAP/////w////EQAAAAEAAAAAAAAA/////+3///8SAAAAAQAAAAAAAAD/////7P///xMAAAABAAAAAAAAAP/////0////GAAAAAEAAAABAAAA//////P///8VAAAAAQAAAAEAAAD/////6P///xYAAAABAAAAAQAAAP/////y////FwAAAAEAAAABAAAA//////T///8cAAAAAQAAAAIAAAD/////8////xkAAAABAAAAAgAAAP/////o////GgAAAAEAAAACAAAA//////L///8bAAAAAQAAAAIAAAD/////CAAAAE5vcm1hbAAEsAEAADEAAABNdWx0aXBsZUxpbmVzAPL/HQAAAEhlYWRpbmcAMQAAADAAAAAcwlkEMCNYBHMA9v///xsAHgAAAEZvb3RpbmcAYAAAADEAAABMYXlvdXROYW1lAAD0////HwAAAFNlbGVjdGVkAAAAADAAAADs41kEMCNYBEhlaWdodAAAIAAAAENhcHRpb24AMQAAAMAAAAB4I1gEeCNYBGxlTmFtZQAAIQAAAEhpZ2hsaWdodFJvdwAAAACQI1gEkCNYBGlzdAD0////IgAAAEV2ZW5Sb3cAMQAAACABAACoI1gEqCNYBGQA9v///xsAIwAAAE9kZFJvdwAEMQAAAFABAADAI1gEwCNYBPj///8ZAAAAJAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAP//AAADAAAAAAAAAB4AAAATAAAAQUNCVHlwZURlc2NyaXB0aW9uAAAeAAAADgAAAEFDQlR5cGVOdW1iZXIAAAALAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAAAAAAAAsAAAD//wAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADQBwAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAAAAAAALAAAAAAAAADIAAAAAAAAAFAAAAFRydWVEQkNvbWJvX0FDQlR5cGUAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQCFAAAADwAAAEF1dG9Db21wbGV0aW9uAIIAAAANAAAAQXV0b0Ryb3Bkb3duAAj+//8MAAAAQm9yZGVyU3R5bGUAcwAAAAwAAABCb3VuZENvbHVtbgD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAIEAAAALAAAAQ29tYm9TdHlsZQAlAAAACQAAAERhdGFNb2RlAAoAAAAMAAAARGVmQ29sV2lkdGgAwQAAABEAAABEcm9wZG93blBvc2l0aW9uAIgAAAAJAAAARWRpdEZvbnQAXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkAJEAAAAKAAAARm9vdExpbmVzAAwAAAAKAAAASGVhZExpbmVzAGUAAAAPAAAASW50ZWdyYWxIZWlnaHQAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAhwAAAAwAAABMaW1pdFRvTGlzdABjAAAACgAAAExpc3RGaWVsZADFAAAABwAAAExvY2tlZAC8AAAAEgAAAE1hdGNoRW50cnlUaW1lb3V0AKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAnAAAAAoAAABSb3dNZW1iZXIAgwAAAAwAAABSb3dUcmFja2luZwAjAAAABwAAAFNwbGl0cwADAgAAEAAAAF9Ecm9wZG93bkhlaWdodAACAgAADwAAAF9Ecm9wZG93bldpZHRoANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA 
height=120 id=TrueDBCombo_ACBType 
style="BACKGROUND-COLOR: #ffd69d; FONT-SIZE: smaller; HEIGHT: 120px; LEFT: 166px; POSITION: absolute; TOP: 476px; WIDTH: 350px; Z-INDEX: -95" 
tabIndex=8 width=350></OBJECT>
<table border="0" cellPadding="0" cellSpacing="1" style="FONT-SIZE: smaller; LEFT: 21px; POSITION: absolute; TOP: 255px; WIDTH: 75%; Z-INDEX: 109" width="75%" class="Table1" background ="" id="tbl_DisbursementDetals">
  
  <tr>
    <td align="right" noWrap id="lbl_Disbursementnbr">Disbursement Number</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 
      id=DisbursementNumber 
      style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 137px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3625"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0;;Null"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2012741633"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
  </td></tr>
  <tr>
    <td align="right" noWrap>Prepared Date</td>
    <td noWrap>
      <OBJECT classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830 height=26 
      id=PreparedDate style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 137px" 
      tabIndex=1><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3625"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="dd/mm/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
	</td></tr>
	 <tr>
    <td align="right" noWrap id="lbl_ActionDate">Action Date</td>
    <td noWrap>
      <OBJECT classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830 id=ActionDate 
      style="HEIGHT: 23px; LEFT: 1px; TOP: 1px; WIDTH: 137px" tabIndex=2><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3625"><PARAM NAME="_ExtentY" VALUE="609"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="dd/mm/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
&nbsp;
	</td></tr>
  <tr>
    <td align="right" noWrap>SPV Description</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=26 
      id=SPVDescription style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 364px" 
      tabIndex=3><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="9631"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap>Account Name</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=26 
      id=AccountName style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 365px" 
      tabIndex=4><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="9657"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap>Account Number</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=26 
      id=AccountNumber style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 204px" 
      tabIndex=5><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="5397"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
  </td></tr>
  <tr>
    <td align="right" noWrap>ACB Bank</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=26 
      id=BankCode name=BankCode 
      style="FONT-SIZE: smaller; HEIGHT: 26px; LEFT: 1px; TOP: 1px; VISIBILITY: hidden; WIDTH: 175px" 
      width=175><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="4630"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
      
</td></tr>
    <tr>
    <td align="right" noWrap>ACB Branch</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=26 
      id=BranchCodeDummy name=BranchCodeDummy 
      style="FONT-SIZE: smaller; HEIGHT: 26px; LEFT: 0px; TOP: 0px; VISIBILITY: hidden; WIDTH: 353px" 
      width=353><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="9340"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=26 
      id=BranchCode 
      style="HEIGHT: 26px; LEFT: 322px; TOP: 1px; VISIBILITY: visible; WIDTH: 150px" 
      width=150><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3969"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>   
      
</td></tr>
<tr>
    <td align="right" noWrap>ACB Account Type</td>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=26 
      id=AccountType 
      style="FONT-SIZE: smaller; HEIGHT: 26px; LEFT: 0px; TOP: 0px; VISIBILITY: hidden"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2117"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
<input class="button2" id="btn_FindBranch" style="HEIGHT: 31px; LEFT: 511px; POSITION: absolute; TOP: 224px; WIDTH: 120px; Z-INDEX: 100" type="button" size="60" value="Find Branch" disabled>   
      
</td></tr>
<tr>
    <td align="right" noWrap>Disbursement Amount</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 
      id=DisbursementAmount 
      style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 143px" tabIndex=9><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3784"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="#######0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0.00"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999.99"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
</table>
<table border="0" cellPadding="1" cellSpacing="1" style="HEIGHT: 220px; LEFT: 718px; POSITION: absolute; TOP: 289px; WIDTH: 164px" width="75%" class="Table1" height="220" id="tbl_Button">
  
  <tr>
    <td align="middle"><input id="btn_AddDisbursement" name="btn_AddDisbursement" style="CURSOR: hand; HEIGHT: 55px; LEFT: 673px; PADDING-TOP: 15px; TOP: 526px; VERTICAL-ALIGN: sub; VISIBILITY: visible; WIDTH: 136px; Z-INDEX: 102" title="Add Disbursement" type="button" value="Add Disbursement" class="button3"></td></tr>
  <tr>
    <td align="middle"><input id="btn_UpdateDisbursement" name="btn_UpdateDisbursement" style="CURSOR: hand; HEIGHT: 55px; LEFT: 259px; PADDING-TOP: 15px; TOP: 510px; VERTICAL-ALIGN: sub; VISIBILITY: visible; WIDTH: 136px; Z-INDEX: 103" title="Update Disbursement" type="button" value="Update Disbursement" class="button3"></td></tr>
  <tr>
    <td align="middle"><input id="btn_DeleteDisbursement" name="btn_DeleteDisbursement" style="CURSOR: hand; HEIGHT: 55px; LEFT: 442px; PADDING-TOP: 15px; TOP: 509px; VERTICAL-ALIGN: sub; VISIBILITY: visible; WIDTH: 136px; Z-INDEX: 104" title="Delete Disbursement" type="button" value="Delete Disbursement" class="button3"></td></tr>
  <tr>
    <td align="middle"><input id="btn_Exit" name="btn_Exit" style="CURSOR: hand; HEIGHT: 55px; LEFT: 709px; TOP: 505px; VISIBILITY: visible; WIDTH: 136px; Z-INDEX: 101" title="Exit" type="button" value="Exit" class="button2"></td></tr></table>


<img alt ="" border="0" height="23" hspace="0" id="pic_AddDisbursement" src="http://sahlnet/SAHL-MLSS/images/MLSDenied.bmp" style             ="CURSOR: hand; HEIGHT: 23px; LEFT: 790px; POSITION: absolute; TOP: 299px; WIDTH: 19px; Z-INDEX: 104" title="0" useMap="" width="19">
<img alt ="" border="0" height="23" hspace="0" id="pic_UpdateDisbursement" src="http://sahlnet/SAHL-MLSS/images/MLSDenied.bmp" style             ="CURSOR: hand; HEIGHT: 23px; LEFT: 790px; POSITION: absolute; TOP: 356px; WIDTH: 19px; Z-INDEX: 105" title="0" useMap="" width="19">
<img alt ="" border="0" height="23" hspace="0" id="pic_DeleteDisbursement" src="http://sahlnet/SAHL-MLSS/images/MLSDenied.bmp" style             ="CURSOR: hand; HEIGHT: 23px; LEFT: 790px; POSITION: absolute; TOP: 414px; WIDTH: 19px; Z-INDEX: 106" title="0" useMap="" width="19">&nbsp; 
<img alt ="" border="0" height="23" hspace="0" id="pic_GenerateDisbursements" src="http://sahlnet/SAHL-MLSS/images/MLSDenied.bmp" style             ="CURSOR: hand; HEIGHT: 23px; LEFT: 784px; POSITION: absolute; TOP: 17px; WIDTH: 19px; Z-INDEX: 107" title="0" useMap="" width="19">

<table WIDTH="100" BORDER="0" CELLSPACING="1" CELLPADDING="1" style="FONT-SIZE: smaller; HEIGHT: 34px; LEFT: 719px; POSITION: absolute; TOP: 255px; WIDTH: 100px; Z-INDEX: 108" id="tbl_Total" class="Table1">
	<tr>
		<td align="right" noWrap>Total</td>
		<td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 
      id=DisbursementTotal name=DisbursementTotal 
      style="HEIGHT: 24px; LEFT: 1px; TOP: 1px; WIDTH: 120px" width=120><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3175"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="##,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="##########0.00"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999999"><PARAM NAME="MinValue" VALUE="-99999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011693061"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="6750213"><PARAM NAME="MinValueVT" VALUE="3538949"></OBJECT>
</td>
	</tr>
</table><input class="button2" id="btn_Print" name="btn_Print" type="button" value="Print Preview" style="CURSOR: hand; HEIGHT: 55px; LEFT: 537px; POSITION: absolute; TOP: 262px; VISIBILITY: visible; WIDTH: 136px; Z-INDEX: 112"> 

<table border="0" cellPadding="1" cellSpacing="1" style="HEIGHT: 68px; LEFT: 716px; POSITION: absolute; TOP: 8px; WIDTH: 158px" width="75%" class="Table1" id="tbl_Generate">
  
  <tr>
    <td align="middle">

<input id="btn_GenerateDisbursements" name="btn_GenerateDisbursements" style="CURSOR: hand; HEIGHT: 61px; LEFT: 721px; PADDING-TOP: 12px; TOP: 18px; VERTICAL-ALIGN: sub; VISIBILITY: visible; WIDTH: 143px; Z-INDEX: 103" title="Generate Disbursements" type="button" value="Generate Disbursements" class="button3"></td></tr></table>

</body>
</html>
