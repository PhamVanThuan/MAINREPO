<%@ Language=VBScript%>

<%
'see line 1276/1324 - changes made to add rounding
'Sonjak 28 june 2005

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
<!--#include virtual="/SAHL-MLSS/database.inc"-->

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
dim s_Source
dim s_Status
dim v_BookMark
dim s_CurrentLoanNbr
dim b_loading
dim b_AllDataLoaded
dim s_Action
dim i_currentxntype
dim b_GuaranteeBalCaptured
Dim d_BalancePayment
Dim d_GuaranteeTotal
Dim d_CashRequired
Dim i_Purpose

if rs_open <> true then
	'Make sure user has logged on properly...if nt redirect him to logon page...
	sessDSN= "DSN=<%= Session("DSN")%>"
	if  sessDSN = "DSN=" then
		window.top.location.href = "Default.asp"
		window.close
	end if
	sUserName = "<%= Session("UserID")%>"
	x = "=<%= Session("SQLDatabase")%>"
	if x <> "=" then
		set conn = createobject("ADODB.Connection")
		set rs_Client  = createobject("ADODB.Recordset")
		set rs_Loan  = createobject("ADODB.Recordset")
		set rs_GridDisbursement  = createobject("ADODB.Recordset")
		set rs_ACBBank = createobject("ADODB.Recordset")
		set rs_ACBBranch = createobject("ADODB.Recordset")
		set rs_ACBType = createobject("ADODB.Recordset")
		set rs_Guarantees = createobject("ADODB.Recordset")
		set rs_PaymentType = createobject("ADODB.Recordset")
		sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [DisbursementsManage.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
		conn.Open sDSN
		rs_open = false
		rs_GridDisbursement_open = false
		b_AllDataLoaded = false
	end if
end if

Sub SetAccessLightsServer

'Set default access to off
window.pic_AddDisbursement.accesskey = "0"
window.pic_UpdateDisbursement.accesskey = "0"
window.pic_DeleteDisbursement.accesskey = "0"
window.pic_GenerateDisbursements.accesskey = "0"

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
tmpCol.Caption = "Action Date"
tmpCol.Width =80
tmpCol.DataField = rs_GridDisbursement.Fields("ActionDate").name
tmpCol.Visible = True
set tmpCol =  TrueDBGrid.Columns.Add(3)
tmpCol.Caption = "Payment Type"
tmpCol.Alignment = 3
tmpCol.Width = 130
tmpCol.DataField = rs_GridDisbursement.Fields("DisbursementTypeDescription").name
tmpCol.Visible = True
set tmpCol =  TrueDBGrid.Columns.Add(4)
tmpCol.Caption = "Account Name"
tmpCol.Width = 100
tmpCol.DataField = rs_GridDisbursement.Fields("AccountName").name
tmpCol.Alignment = 3
tmpCol.Visible = True
set tmpCol =  TrueDBGrid.Columns.Add(5)
tmpCol.Caption = "Account Number"
tmpCol.Width = 130
tmpCol.DataField = rs_GridDisbursement.Fields("AccountNumber").name
tmpCol.Alignment = 3
tmpCol.Visible = True
set tmpCol =  TrueDBGrid.Columns.Add(5)
tmpCol.Caption = "+/-/="
tmpCol.Width = 40
tmpCol.DataField = rs_GridDisbursement.Fields("DisbursementInterestApplied").name
tmpCol.Alignment = 2
tmpCol.Visible = True
set tmpCol =  TrueDBGrid.Columns.Add(7)
tmpCol.Caption = "Disbursement Amount"
tmpCol.DataField = rs_GridDisbursement.Fields("DisbursementAmount").name
tmpCol.Alignment = 1
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
sSQL = "r_GetDisbursementsByLoan " & i_CurrentLoanNbr
rs_GridDisbursement.CursorLocation = 3
rs_GridDisbursement.CacheSize  =10
rs_GridDisbursement.Open sSQL,conn,adOpenDynamic
TrueDBGrid.DataSource = rs_GridDisbursement
rs_GridDisbursement_open = true
'''''''''''''''''ACBBANK'''''''''''''''''''''''''''''''''''''''''''''''''''

sSQL = "SELECT ACBBankCode, ACBBankDescription FROM ACBBANK (nolock)"
rs_ACBBank.CursorLocation = 3
rs_ACBBank.Open sSQL ,conn,adOpenStatic

set TrueDBCombo_ACBBank.RowSource = rs_ACBBank

TrueDBCombo_ACBBank.Columns.Remove(0)
TrueDBCombo_ACBBank.ListField = rs_ACBBank.Fields("ACBBankDescription").name
TrueDBCombo_ACBBank.BoundColumn= rs_ACBBank.Fields("ACBBankCode").Name
TrueDBCombo_ACBBank.OddRowStyle.BackColor = &HC0FFFF
TrueDBCombo_ACBBank.EvenRowStyle.BackColor = &HC0C0C0

if rs_GridDisbursement.RecordCount > 0 then
	TrueDBCombo_ACBBank.BoundText = rs_GridDisbursement.Fields("BankCode").Value
else
	TrueDBCombo_ACBBank.BoundText = 0
end if

TrueDBCombo_ACBBank.Refresh

'''''''''''ACBBRANCH''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
sSQL = "SELECT ACBBranchCode,ACBBranchDescription,ACBBankCode FROM ACBBRANCH (nolock) WHERE ACBBankCode = " &  rs_ACBBank.Fields("ACBBankCode") & " and ActiveIndicator = 0 ORDER BY ACBBranchDescription"
rs_ACBBranch.CursorLocation = 3
rs_ACBBranch.Open sSQL ,conn,adOpenStatic
set TrueDBCombo_ACBBranch.RowSource = rs_ACBBranch

TrueDBCombo_ACBBranch.Columns.Remove(0)

TrueDBCombo_ACBBranch.ListField = rs_ACBBranch.Fields("ACBBranchDescription").name
TrueDBCombo_ACBBranch.OddRowStyle.BackColor = &HC0FFFF
TrueDBCombo_ACBBranch.EvenRowStyle.BackColor = &HC0C0C0
TrueDBCombo_ACBBranch.BoundText = rs_ACBBranch.Fields("ACBBranchCode").Value
TrueDBCombo_ACBBranch.Refresh
'''''''''''''''''ACBTYPE'''''''''''''''''''''''''''''''''''''''''''''''''''

sSQL = "SELECT ACBTypeNumber, ACBTypeDescription FROM ACBTYPE (nolock)"

rs_ACBType.CursorLocation = 3
rs_ACBType.Open sSQL ,conn,adOpenStatic
TrueDBCombo_ACBType.RowSource = rs_ACBType
TrueDBCombo_ACBType.Columns.Remove(0)
TrueDBCombo_ACBType.ListField = rs_ACBType.Fields("ACBTypeDescription").Name
TrueDBCombo_ACBType.BoundColumn= rs_ACBType.Fields("ACBTypeNumber").Name
TrueDBCombo_ACBType.BoundText = rs_ACBType.Fields("ACBTypeNumber").Value
TrueDBCombo_ACBType.OddRowStyle.BackColor = &HC0FFFF
TrueDBCombo_ACBType.EvenRowStyle.BackColor = &HC0C0C0

if rs_GridDisbursement.RecordCount > 0 then
	TrueDBCombo_ACBType.BoundText = rs_GridDisbursement.Fields("AccountTypeNumber").Value
else
	TrueDBCombo_ACBType.BoundText = 0
end if

TrueDBCombo_ACBType.Refresh
'*** Manually populate the Payment Type RS
rs_PaymentType.Fields.Append "PaymentTypeNumber",19
rs_PaymentType.Fields.Append "PaymentTypeDescription",200,180
rs_PaymentType.Open

rs_PaymentType.AddNew
rs_PaymentType.fields("PaymentTypeNumber").Value = 0
rs_PaymentType.fields("PaymentTypeDescription").Value = "Payment (No Interest)"
rs_PaymentType.Update

rs_PaymentType.AddNew
rs_PaymentType.fields("PaymentTypeNumber").Value = 1
rs_PaymentType.fields("PaymentTypeDescription").Value = "Guarantee Payment"
rs_PaymentType.Update

rs_PaymentType.AddNew
rs_PaymentType.fields("PaymentTypeNumber").Value = 2
rs_PaymentType.fields("PaymentTypeDescription").Value = "Cash Required"
rs_PaymentType.Update

rs_PaymentType.AddNew
rs_PaymentType.fields("PaymentTypeNumber").Value = 3
rs_PaymentType.fields("PaymentTypeDescription").Value = "Other Disbursement"
rs_PaymentType.Update

rs_PaymentType.AddNew
rs_PaymentType.fields("PaymentTypeNumber").Value = 4
rs_PaymentType.fields("PaymentTypeDescription").Value = "Cancellation Refund"
rs_PaymentType.Update

rs_PaymentType.AddNew
rs_PaymentType.fields("PaymentTypeNumber").Value = 5
rs_PaymentType.fields("PaymentTypeDescription").Value = "ReAdvance"
rs_PaymentType.Update

rs_PaymentType.AddNew
rs_PaymentType.fields("PaymentTypeNumber").Value = 6
rs_PaymentType.fields("PaymentTypeDescription").Value = "Refund"
rs_PaymentType.Update

rs_PaymentType.AddNew
rs_PaymentType.fields("PaymentTypeNumber").Value = 7
rs_PaymentType.fields("PaymentTypeDescription").Value = "Cash Required (Interest)"
rs_PaymentType.Update

rs_PaymentType.AddNew
rs_PaymentType.fields("PaymentTypeNumber").Value = 8
rs_PaymentType.fields("PaymentTypeDescription").Value = "Quick Cash"
rs_PaymentType.Update

rs_PaymentType.MoveFirst
window.tbl_DisbursementType.focus
set window.DataCombo_PaymentType.RowSource = rs_PaymentType
DataCombo_PaymentType.ListField = rs_PaymentType.Fields("PaymentTypeDescription").name
DataCombo_PaymentType.BoundColumn = rs_PaymentType.Fields("PaymentTypeNumber").name
if rs_GridDisbursement.RecordCount > 0 then
	DataCombo_PaymentType.BoundText = rs_GridDisbursement.Fields("DisbursementType").Value
else
	DataCombo_PaymentType.BoundText = rs_PaymentType.Fields("PaymentTypeNumber").Value
end if
DataCombo_PaymentType.Refresh
end sub

Sub TrueDBCombo_ACBBank_ItemChange
rs_ACBBranch.Close
sSQL = "SELECT ACBBranchCode,ACBBranchDescription,ACBBankCode FROM ACBBRANCH (nolock) WHERE ACBBankCode = " & window.TrueDBCombo_ACBBank.BoundText & " and ActiveIndicator = 0 ORDER BY ACBBranchDescription"
rs_ACBBranch.CursorLocation = 3
rs_ACBBranch.Open sSQL ,conn,adOpenStatic
TrueDBCombo_ACBBranch.RowSource = rs_ACBBranch
TrueDBCombo_ACBBranch.ListField = rs_ACBBranch.Fields("ACBBranchDescription").name
' RSM: 2005-12-08: Added teh line to set the bound column - Why was it not added (I wonder!)
TrueDBCombo_ACBBranch.BoundColumn = rs_ACBBranch.Fields("ACBBranchCode").name

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
	window.DataCombo_PaymentType.Enabled = false
	if rs_GridDisbursement.RecordCount > 0 then
		rs_GridDisbursement.Requery
		window.TrueDBGrid.Bookmark =  v_BookMark
	end if
	exit sub
end if

window.pnl_Msg.style.visibility = "hidden"

window.btn_AddDisbursement.style.visibility = "hidden"
window.btn_UpdateDisbursement.style.visibility = "hidden"
window.btn_DeleteDisbursement.style.visibility = "hidden"
window.btn_FindBranch.style.visibility = "hidden"
window.btn_Exit.style.visibility = "hidden"
window.btn_GenerateDisbursements.style.visibility = "hidden"
window.btn_Print.style.visibility = "hidden"
window.TrueDBCombo_ACBBank.style.visibility = "hidden"
window.TrueDBCombo_ACBBranch.style.visibility = "hidden"
window.TrueDBCombo_ACBType.style.visibility = "hidden"
window.BranchCode.style.visibility = "hidden"
window.pic_AddDisbursement.style.visibility = "hidden"
window.pic_DeleteDisbursement.style.visibility = "hidden"
window.pic_GenerateDisbursements.style.visibility = "hidden"
window.pic_UpdateDisbursement.style.visibility = "hidden"
window.document.body.style.backgroundColor = "#FFD69D"
window.document.body.style.visibility = "hidden"
window.parent.frames("RegistrationPanel").location.href = s_ReturnPage & "?Number= " & CStr(i_CurrentLoanNbr ) & "&Source=" & s_Source  & "&RepliesReceived=<%=Request.QueryString("RepliesReceived")%>"  & "&Lodged=<%=Request.QueryString("Lodged")%>" & "&Disbursements=<%=Request.QueryString("Disbursements")%>" & "&Readvances=<%=Request.QueryString("Disbursements")%>" & "&UpForFees=<%=Request.QueryString("UpForFees")%>"
End Sub

Sub TrueDBCombo_ACBType_ItemChange
'window.AccountType.Text = window.TrueDBCombo_ACBType.BoundText
'TrueDBCombo_ACBType.Refresh
End Sub

Sub window_onload
	b_loading = true
	i_currentxntype = 0
	b_GuaranteeBalCaptured = false
	SetAccessLightsServer

	' RSM: 2005/12/08: Disable the bank and branch controls on load (until teh user decides to update)
	window.TrueDBCombo_ACBBank.Enabled = false
	window.TrueDBCombo_ACBBranch.Enabled = false
	window.TrueDBCombo_ACBType.Enabled = false

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
	window.InterestStartDate.DropDown.Visible = 1
	window.InterestStartDate.Spin.Visible = 1

	i_Nbr = "<%=Request.QueryString("Number")%>"
	s_Source = "<%=Request.QueryString("returnpage")%>"
	s_Status = "<%=Request.QueryString("Status")%>"

	i_Purpose = "<%=Request.QueryString("purpose")%>"
	s_ReturnPage = "<%=Request.QueryString("source")%>"

	i_CurrentLoanNbr = i_Nbr
	if trim(CStr(i_CurrentLoanNbr)) = "0"  or  trim(CStr(i_CurrentLoanNbr))  ="" then
		window.location.href ="RegistrationCATS-old.asp"
		window.close
		exit sub
	end if

	if trim(s_Status) = "" then s_Status = 0

	GetLoanDetails

	tbl_Client.focus
	window.LoanNumber.Value = rs_Client.Fields("LoanNumber").Value
	window.LoanFirstNames.Text = rs_Client.Fields("ClientFirstNames").Value
	window.LoanSurname.Text = rs_Client.Fields("ClientSurname").Value
	tbl_Total.focus

	window.SPVDescription.Text = rs_Loan.Fields("SPVDescription").Value

	ConfigureDetailGrid
	CalcTotal
	CalculateBalancePayment
	if rs_GridDisbursement.RecordCount > 0 then
		window.DataCombo_PaymentType.BoundText =Cint( rs_GridDisbursement.Fields("DisbursementType").Value)
	end if
	if i_Purpose <> 5 then
		if GetDisburseStatus(i_CurrentLoanNbr) > 0 then
			pnl_Msg.style.visibility = "visible"
			window.btn_AddDisbursement.disabled = true
			window.btn_DeleteDisbursement.disabled = true
			s_Status = 6
		end if
	end if

	ShowLoanPurpose
	DataCombo_PaymentType.Enabled = false
	b_AllDataLoaded = true
End Sub

Sub CalcTotal

	if rs_GridDisbursement.RecordCount > 0 then

		VBook = rs_GridDisbursement.Bookmark
		rs_GridDisbursement.MoveFirst
		iTot = 0
		d_GuaranteeTotal = 0
		d_CashRequired = 0
		b_GuaranteeBalCaptured = false

		do while not rs_GridDisbursement.EOF
			if rs_GridDisbursement.Fields("DisbursementLocked").Value  = 0  then

				if rs_GridDisbursement.Fields("DisbursementInterestApplied").Value = "=" then
					b_GuaranteeBalCaptured = true
				end if

				iTot = iTot + CDbl(CStr(rs_GridDisbursement.Fields("DisbursementAmount").Value))

				if rs_GridDisbursement.Fields("DisbursementType").Value = 0 or _
				   rs_GridDisbursement.Fields("DisbursementType").Value = 1 then

					d_GuaranteeTotal = d_GuaranteeTotal + rs_GridDisbursement.Fields("DisbursementAmount").Value

				elseif rs_GridDisbursement.Fields("DisbursementType").Value = 2 or _
				       rs_GridDisbursement.Fields("DisbursementType").Value = 7 or _
				       rs_GridDisbursement.Fields("DisbursementType").Value = 8 then

					d_CashRequired = d_CashRequired + rs_GridDisbursement.Fields("DisbursementAmount").Value

				end if
			end if

			rs_GridDisbursement.MoveNext
		loop

		rs_GridDisbursement.Bookmark =  VBook
		window.DisbursementTotal.Value = iTot
	end if
End Sub

Sub TrueDBGrid_RowColChange(LastRow, LastCol)

if b_AllDataLoaded = true and window.btn_AddDisbursement.value <> "Commit"  then
	if rs_GridDisbursement.RecordCount > 0 then
		i_PrevStatus = s_Status
		if rs_GridDisbursement.fields("DisbursementType").Value = 3 or rs_GridDisbursement.fields("DisbursementType").Value = 4 or rs_GridDisbursement.fields("DisbursementType").Value = 5 or  rs_GridDisbursement.fields("DisbursementType").Value = 6 then  'other disbursments
			s_Status = 0
			pnl_Msg.style.visibility = "hidden"
			window.btn_AddDisbursement.disabled = false
			window.btn_DeleteDisbursement.disabled = false
		elseif (rs_GridDisbursement.fields("DisbursementType").Value = 0 or rs_GridDisbursement.fields("DisbursementType").Value = 2 or rs_GridDisbursement.fields("DisbursementType").Value = 8 ) and (s_Status = 6 or s_Status = 343) then  'No Interest
			s_Status = 0
			pnl_Msg.style.visibility = "hidden"
			window.btn_AddDisbursement.disabled = false
			window.btn_DeleteDisbursement.disabled = false
		elseif i_PrevStatus = 6 then
			pnl_Msg.style.visibility = "visible"
			window.btn_AddDisbursement.disabled = true
			window.btn_DeleteDisbursement.disabled = true
			s_Status = 6
		else
			i_PrevStatus = s_Status
		end if
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
		window.TrueDBCombo_ACBType.BoundText = Cint(rs_GridDisbursement.Fields("AccountTypeNumber").Value)
		window.TrueDBCombo_ACBType.Refresh
		window.InterestApplied.Text =  rs_GridDisbursement.Fields("DisbursementInterestApplied").Value
		window.InterestRate.Value=  rs_GridDisbursement.Fields("DisbursementInterestRate").Value
		window.InterestStartDate.Value =  rs_GridDisbursement.Fields("DisbursementInterestStartDate").Value
		window.CapitalAmount.Value =  rs_GridDisbursement.Fields("DisbursementCapitalAmount").Value
		window.GuaranteeAmount.Value =  rs_GridDisbursement.Fields("DisbursementGuaranteeAmount").Value
		window.PaymentAmount.Value = rs_GridDisbursement.Fields("DisbursementAmount").value
		window.DataCombo_PaymentType.BoundText =Cint( rs_GridDisbursement.Fields("DisbursementType").Value)
	else
		window.DisbursementNumber.Value = 0
		window.PreparedDate.Value = now()
		window.ActionDate.Value = ""
		window.AccountName.Text  = ""
		window.AccountNumber.Text = ""
		window.DisbursementAmount.Value = 0.00
		window.InterestApplied.Text =  ""
		window.InterestRate.Value= 0
		window.InterestStartDate.Text =  "__/__/____"
		window.CapitalAmount.Value =  0
		window.GuaranteeAmount.Value =  0
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
if rs_GridDisbursement.RecordCount < 1 then
	window.btn_UpdateDisbursement.disabled = true
	window.btn_DeleteDisbursement.disabled = true
end if
window.TrueDBGrid.Enabled = true
window.btn_AddDisbursement.style.visibility = "visible"
window.btn_UpdateDisbursement.style.visibility = "visible"
window.btn_DeleteDisbursement.style.visibility = "visible"
window.pic_AddDisbursement.style.visibility = "visible"
window.pic_UpdateDisbursement.style.visibility = "visible"
window.pic_DeleteDisbursement.style.visibility = "visible"

if Cint(s_Status) =  6 or Cint(s_Status) =  343 then
	window.btn_AddDisbursement.disabled = true
	window.btn_DeleteDisbursement.disabled = true
end if
End Sub

sub ClearFields
window.DisbursementNumber.Value = 0
window.PreparedDate.Value = now()
window.ActionDate.Value = ""
window.AccountName.Text  = ""
window.AccountNumber.Text = ""
window.TrueDBCombo_ACBBank.BoundText = 0
window.TrueDBCombo_ACBBank.Refresh
window.BranchCode.Text = ""
window.DisbursementAmount.Value = 0.00
window.InterestApplied.Text = ""
window.InterestRate.Value = 0
window.InterestStartDate.Value = date
window.GuaranteeAmount.Value = 0
window.PaymentAmount.Value = 0
window.CapitalAmount.Value = 0
End Sub

Sub EnableFields
window.AccountName.Enabled = true
window.AccountNumber.Enabled = true
window.TrueDBCombo_ACBBank.Enabled = true
window.TrueDBCombo_ACBBranch.Enabled = true
window.TrueDBCombo_ACBType.Enabled = true
window.DisbursementAmount.Enabled = true
window.BranchCode.Enabled = true
window.DataCombo_PaymentType.Enabled = true
if i_currentxntype = 0 or i_currentxntype = 2 or i_currentxntype = 3 or i_currentxntype = 4 or i_currentxntype = 5  or i_currentxntype = 6 or i_currentxntype = 8 then
	window.DisbursementAmount.Enabled = true
	window.InterestApplied.Enabled = false
	window.InterestRate.Enabled = false
	window.InterestStartDate.Enabled = false
	window.GuaranteeAmount.Enabled = false
	window.PaymentAmount.Enabled = false
	window.CapitalAmount.Enabled = false
else
	if window.InterestApplied = "=" then
		window.GuaranteeAmount.Enabled = true
	else
		window.DisbursementAmount.Enabled = false
		window.InterestApplied.Enabled = true
		window.InterestRate.Enabled = true
		window.InterestStartDate.Enabled = true
		window.GuaranteeAmount.Enabled = true
		window.CapitalAmount.Enabled = true
	end if
end if
if pnl_Msg.style.visibility = "visible" then
	window.DisbursementAmount.Enabled = false
	window.InterestApplied.Enabled = false
	window.InterestRate.Enabled = false
	window.InterestStartDate.Enabled = false
	window.CapitalAmount.Enabled = false
	window.GuaranteeAmount.Enabled = false
	window.PaymentAmount.Enabled = false
end if
end sub

Sub DisableFields
window.AccountName.Enabled = false
window.AccountNumber.Enabled = false
window.TrueDBCombo_ACBBank.Enabled = false
window.TrueDBCombo_ACBBranch.Enabled = false
window.TrueDBCombo_ACBType.Enabled = false
window.DisbursementAmount.Enabled = false
window.BranchCode.Enabled = false
DataCombo_PaymentType.Enabled = false
window.InterestApplied.Enabled = false
window.InterestRate.Enabled = false
window.InterestStartDate.Enabled = false
window.GuaranteeAmount.Enabled = false
window.PaymentAmount.Enabled = false
window.CapitalAmount.Enabled = false
end sub

Function ValidateFields
ValidateFields = -1
if window.PreparedDate  < date() then
	msgbox "Prepared Date cannot be less than today....!!!",, "Disbursement"
	window.PrepareDate.focus
	exit Function
end if
if CLng(window.TrueDBCombo_ACBBank.BoundText)= 0 then
	msgbox "Bank cannot be Stop Order..!!",, "Disbursement"
	window.TrueDBCombo_ACBBank.focus
	exit Function
end if
if CLng(window.TrueDBCombo_ACBBranch.BoundText)= 0 then
	msgbox "Branch cannot be Stop Order..!!",, "Disbursement"
	window.TrueDBCombo_ACBBranch.focus
	exit Function
end if
if trim(window.AccountName.Text) = "" then
	msgbox "Account Name cannot be empty..!!",, "Disbursement"
	window.AccountName.focus
	exit Function
end if
if IsNumeric(window.AccountNumber.Text) = false then
	msgbox "Account Number must be a numeric field..!!",, "Disbursement"
	window.AccountNumber.focus
	exit Function
end if
if Cint(window.TrueDBCombo_ACBType.BoundText)= 0 then
	msgbox "Account Type cannot be unknown..!!",, "Disbursement"
	window.TrueDBCombo_ACBType.focus
	exit Function
end if
if i_currentxntype = 0 or i_currentxntype = 2 or i_currentxntype = 3  or i_currentxntype = 4 or i_currentxntype = 5   or i_currentxntype = 6  or i_currentxntype = 8  then
	if window.DisbursementAmount <= 0.0 then
		msgbox "Amount must be entered, and greater than zero..!!",, "Disbursement"
		window.DisbursementAmount.focus
		exit Function
	end if
else
	if b_GuaranteeBalCaptured = true and Trim(window.InterestApplied.Text) = "=" and window.btn_AddDisbursement.value = "Commit" then
		msgbox "Balancing Guarantee Transaction has already been captured..!!" ,, "Disbursement"
		window.InterestApplied.focus
		exit Function
	end if
	if Trim(window.InterestApplied.Text) = "+" or Trim(window.InterestApplied.Text) = "-" then
		if window.CapitalAmount.Value  <= 0.0 then
			msgbox "Capital Amount must be entered, and greater than zero..!!",, "Disbursement"
			window.CapitalAmount.focus
			exit Function
		end if
		if window.PaymentAmount.Value  <= 0.0 then
			msgbox "Payment Amount must  greater than zero..!!",, "Disbursement"
			window.PaymentAmount.focus
			exit Function
		end if
		if window.InterestRate.Value  <= 0.0 then
			msgbox "Interest rate must be entered, and greater than zero..!!",, "Disbursement"
			window.InterestRate.focus
			exit Function
		end if
		if  Trim(window.InterestStartDate.Text) = "__/__/____" then
			msgbox "Interest Start Date cannot be empty....!!!"
			window.InterestStartDate.focus
			exit Function
		end if
	end if
	if Trim(window.InterestApplied.Text) <> "+" and Trim(window.InterestApplied.Text) <> "-" and Trim(window.InterestApplied.Text) <> "=" then
		msgbox "Interest Applied must be + or - or = ....!!!",, "Disbursement"
		window.InterestApplied.focus
		exit Function
	end if
	if window.GuaranteeAmount.Value  <= 0.0 then
		msgbox "Guarantee Amount must be entered, and greater than zero..!!",, "Disbursement"
		window.GuaranteeAmount.focus
		exit Function
	end if
end if
ValidateFields = 0
End function

Sub btn_AddDisbursement_onclick

	if window.pic_AddDisbursement.accesskey = "0" then
		window.status = "Access denied to " & window.btn_AddDisbursement.title
		exit sub
	end if

	v_BookMark = window.TrueDBGrid.Bookmark

	if btn_AddDisbursement.value = "Add Disbursement" then

		DisableControls("Add")
		ClearFields
		EnableFields
		rs_PaymentType.MoveFirst
		btn_FindBranch.disabled = false
		window.btn_Exit.innerText = "Cancel"
		window.PreparedDate.value = date()
		if pnl_Msg.style.visibility = "visible" then
			window.TrueDBCombo_ACBBank.Enabled = true
			window.TrueDBCombo_ACBBank.focus
		else
			if Cint(DataCombo_PaymentType.BoundText)  = 0 or Cint(DataCombo_PaymentType.BoundText)  = 2 or Cint(DataCombo_PaymentType.BoundText)  = 3 OR Cint(DataCombo_PaymentType.BoundText) = 8 then
				window.DisbursementAmount.focus
			else
				window.InterestApplied.focus
			end if
		end if

		btn_AddDisbursement.value = "Commit"

	elseif btn_AddDisbursement.value = "Commit" then

		if ValidateFields = -1 then exit sub

		call MaintainDisbursementRecord("Add")

		'Clean up...
		btn_AddDisbursement.value = "Add Disbursement"
		btn_Exit.innerText = "Exit"
		window.DataCombo_PaymentType.Enabled = false
		DisableFields()
		b_loading  = true
		EnableControls
		btn_FindBranch.disabled = true
		rs_GridDisbursement.MoveLast
	End if
End Sub

Function MaintainDisbursementRecord(s_Action)
Dim i_res

	MaintainDisbursementRecord = -1
	document.body.style.cursor = "hand"
	i_res = 0
	set com = createobject("ADODB.Command")
	set prm = createobject("ADODB.Parameter")
	set rs_temp = createobject("ADODB.Recordset")

	sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS [DisbursementsManage.asp 2];uid=<%= Session("UserID")%>"

	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc
	if s_Action = "Add" then
		sSQL = "r_AddDisbursementTran"
	elseif s_Action = "Update" then
		sSQL = "r_UpdDisbursementTran"
	elseif s_Action = "Delete" then
		sSQL = "r_DelDisbursementTran"
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
		set prm = com.CreateParameter ( "LoanNumber",19,1,,window.LoanNumber.Value) 'AdVarchar , adParamInput
		com.Parameters.Append prm
	end if

if s_Action = "Add" or s_Action = "Update" then
	set prm = com.CreateParameter ("ACBBankCode",19,1,,window.TrueDBCombo_ACBBank.BoundText) 'AdVarchar , adParamInput
	com.Parameters.Append prm

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

	set prm = com.CreateParameter ("DisbursementType",19,1,,i_currentxntype) ' AdUnsigned Int
	com.Parameters.Append prm

	set prm = com.CreateParameter ("DisbursementInterestApplied",200,1,1,window.InterestApplied.Text)
	com.Parameters.Append prm

	set prm = com.CreateParameter ("DisbursementInterestRate",5,1,,window.InterestRate.Value)
	com.Parameters.Append prm

	s_date = Mid(window.InterestStartDate.Text, 4, 2) & "/" & Mid(window.InterestStartDate.Text, 1, 2) & "/" & Mid(window.InterestStartDate.Text, 7, 4)
	if window.InterestStartDate.DisplayText = "__/__/____" then
		s_date = NULL
	end if

	set prm = com.CreateParameter ("DisbursementInterestStartDate",129,1,10,s_date)
	com.Parameters.Append prm

	set prm = com.CreateParameter ("DisbursementCapitalAmount",5,1,,window.CapitalAmount.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ("DisbursementGuaranteeAmount",5,1,,window.GuaranteeAmount.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ("DisbursementPaymentAmount",5,1,,window.PaymentAmount.Value)
	com.Parameters.Append prm

end if

	set rs_temp = com.Execute
	rs_GridDisbursement.Requery

	CalcTotal

	if i_Purpose <> 3 then    'New Purchase so do not insert into regmail as Clerk will contruct the txn amts themselves
		UpdateRegMailDisburseAmounts
	end if

	document.body.style.cursor = "default"
	MaintainDisbursementRecord = 0
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
	window.DataCombo_PaymentType.Enabled = true

	window.PreparedDate.Value = now()
	window.btn_Exit.innerText = "Cancel"
	btn_UpdateDisbursement.value = "Commit"
	if pnl_Msg.style.visibility = "visible" then
		window.TrueDBCombo_ACBBank.Enabled = true
		window.TrueDBCombo_ACBBank.focus
	else
		if i_currentxntype = 0 or i_currentxntype = 2 or i_currentxntype = 3 or i_currentxntype = 4 or i_currentxntype = 5   or i_currentxntype = 6  or i_currentxntype = 8 then
			window.DisbursementAmount.Enabled = true
			window.DisbursementAmount.focus
		else
			if window.InterestApplied.Text = "=" then
				window.GuaranteeAmount.Enabled = true
				window.GuaranteeAmount.focus
			else
				window.InterestApplied.Enabled = true
				window.InterestApplied.focus
			end if
		end if
	end if
elseif btn_UpdateDisbursement.value = "Commit" then
	if ValidateFields = -1 then
		exit sub
	end if
	call MaintainDisbursementRecord("Update")
	'Clean up...
	btn_UpdateDisbursement.value = "Update Disbursement"
	btn_Exit.innerText = "Exit"
	EnableControls
	btn_FindBranch.disabled = true
	window.DataCombo_PaymentType.Enabled = false
	DisableFields()
	b_loading  = true
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
	call MaintainDisbursementRecord("Delete")
	'Clean up...
	btn_DeleteDisbursement.value = "Delete Disbursement"
	btn_Exit.innerText = "Exit"
	EnableControls
	b_loading  = true
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

window.tbl_DisbursementType.style.visibility = "hidden"
window.LoanPurpose.style.visibility = "hidden"
window.btn_FindBranch.style.visibility = "hidden"
window.tbl_Client.style.visibility = "hidden"
window.tbl_DisbursementDetails.style.visibility = "hidden"
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
window.parent.frames("RegistrationPanel").location.href = "DisbursementsGenerate.asp?returnpage=DisbursementManage.asp&Number= " & Trim(CStr(i_CurrentLoanNbr) ) & "&purpose=" & CStr(i_purpose) & "&Source=<%=Request.QueryString("Source")%>" & "&RepliesReceived=<%=Request.QueryString("RepliesReceived")%>"  & "&Lodged=<%=Request.QueryString("Lodged")%>" & "&Disbursements=<%=Request.QueryString("Disbursements")%>" & "&Readvances=<%=Request.QueryString("Disbursements")%>"
End Sub

Sub BranchCode_Change

End Sub

Sub BranchCode_KeyUp(i,	x )

End Sub

Sub btn_FindBranch_onclick
i_OrigBook = TrueDBCombo_ACBBranch.Bookmark

rs_ACBBranch.MoveFirst

i_Book = 0
do while not rs_ACBBranch.EOF
	if rs_ACBBranch.Fields(0).Value = trim(window.BranchCode.Text) then
		i_Book = rs_ACBBranch.Bookmark
		exit do
	end if
	rs_ACBBranch.MoveNext
loop
if i_Book = 0 then
	msgbox "Branch Not found",,"Disbursment Manage"
	window.TrueDBCombo_ACBBranch.Bookmark = i_OrigBook
else
	window.TrueDBCombo_ACBBranch.Bookmark = i_Book
end if

End Sub

Sub btn_Print_onclick

	sSQL = "r_GetDisbursementsByLoan " & i_CurrentLoanNbr

	set rs_LoanDisbursement  = createobject("ADODB.Recordset")
	rs_LoanDisbursement.CursorLocation = 3
	rs_LoanDisbursement.CacheSize  =10
	rs_LoanDisbursement.Open sSQL,conn,adOpenDynamic

	if rs_LoanDisbursement.RecordCount = 0 then
		msgbox  "Nothing to Print",, "CATS Disbursement"
		exit sub
	end if

	Dim aDateTime
	aDateTime = Now

	Dim HTML
	HTML = "<!DOCTYPE HTML PUBLIC '-//IETF//DTD HTML//EN'>"
	HTML = HTML & "<html>"
	HTML = HTML & "<head>"
	HTML = HTML & "<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>"
	HTML = HTML & "<title>Disbursements for Loan Number : " & window.LoanNumber.Value & "</title>"
	HTML = HTML & "</head>"

	HTML = HTML & "<body>"

	HTML = HTML & "<p><b><u><font face=Arial size=3>Disbursements for Loan Number : "  & window.LoanNumber.Value & "</font></u></b></p>"
	HTML = HTML & "Print Date: " & FormatDateTime( aDateTime, VbLongDate) & " " & FormatDateTime( aDateTime, VbLongTime) & "<p>"
	HTML = HTML & "Loan Number&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= " & window.LoanNumber.Value  & "<br>"
	HTML = HTML & "Client First Names&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= " & window.LoanFirstNames.Text & "<br>"
	HTML = HTML & "Client Surname&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;= " & window.LoanSurname.Text & "<br>"

	HTML = HTML & "<br>"
	HTML = HTML & "<br>"
	HTML = HTML & "<br>"
	HTML = HTML & "<br>"

	rs_LoanDisbursement.MoveFirst
	Do Until rs_LoanDisbursement.EOF
			HTML = HTML & "<TABLE border=1 ><TR><TD>Disbursement Number</TD><TD width=250>" & rs_LoanDisbursement.Fields("Number").Value & "</TD></TR>"
			HTML = HTML & "<TR><TD>Account Name</TD><TD>" & rs_LoanDisbursement.Fields("AccountName").Value & "</TD></TR>"
			HTML = HTML & "<TR><TD>Account Number</TD><TD>" & rs_LoanDisbursement.Fields("AccountNumber").Value & "</TD></TR>"
			HTML = HTML & "<TR><TD>Bank Description</TD><TD>" & rs_LoanDisbursement.Fields("BankDescription").Value & "</TD></TR>"
			HTML = HTML & "<TR><TD>Branch Code</TD><TD>" & rs_LoanDisbursement.Fields("BranchCode").Value & "</TD></TR>"
			if rs_LoanDisbursement.Fields("DisbursementType").Value = 0 or rs_LoanDisbursement.Fields("DisbursementType").Value = 2 then
				HTML = HTML & "<TR><TD>Disbursement Amount</TD><TD>" & rs_LoanDisbursement.Fields("DisbursementAmount").Value & "</TD></TR></TABLE><br>"
			else
				HTML = HTML & "<TR><TD>Interest Applied</TD><TD>" & rs_LoanDisbursement.Fields("DisbursementInterestApplied").Value & "</TD></TR>"
				HTML = HTML & "<TR><TD>Interest Rate</TD><TD>" & rs_LoanDisbursement.Fields("DisbursementInterestRate").Value & " %" & "</TD></TR>"
				HTML = HTML & "<TR><TD>Interest Start Date</TD><TD>" & rs_LoanDisbursement.Fields("DisbursementInterestStartDate").Value & "</TD></TR>"
				HTML = HTML & "<TR><TD>Capital Amount</TD><TD>" & rs_LoanDisbursement.Fields("DisbursementCapitalAmount").Value & "</TD></TR>"
				HTML = HTML & "<TR><TD>Guarantee Amount</TD><TD>" & rs_LoanDisbursement.Fields("DisbursementGuaranteeAmount").Value & "</TD></TR>"
				HTML = HTML & "<TR><TD>Payment Amount</TD><TD>" & rs_LoanDisbursement.Fields("DisbursementAmount").Value & "</TD></TR></TABLE><br>"
			end if
		rs_LoanDisbursement.MoveNext
		HTML = HTML & "<br>"
	Loop

	HTML = HTML & "<p>Total Amount = " & CStr(window.DisbursementTotal.Value) & "</p><br>"

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
	HTML = HTML & vbNewLine
	HTML = HTML & vbNewLine
	HTML = HTML & "End Sub"
	HTML = HTML & vbNewLine
	HTML = HTML & "Sub btnPrint_onclick"
	HTML = HTML & vbNewLine
	HTML = HTML & "window.Print(true)"
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

Sub InterestApplied_Change

	if ( window.btn_AddDisbursement.value = "Commit" )  or ( window.btn_UpdateDisbursement.value = "Commit" )  then
		if Trim(window.InterestApplied.Text) = "=" and b_GuaranteeBalCaptured = true then
			Msgbox "A Balancing payment has already been captured... ",,"Disbursement"
			window.btn_Exit.click
		elseif Trim(window.InterestApplied.Text) = "+" or Trim(window.InterestApplied.Text) = "-" then
			CalculatePayment
		end if
	end if

	if ( window.btn_AddDisbursement.value = "Commit" )  or ( window.btn_UpdateDisbursement.value = "Commit" )  then
		if (Trim(window.InterestApplied.Text) = "+" or Trim(window.InterestApplied.Text) = "-") and b_GuaranteeBalCaptured = true then
			Msgbox "A Balancing payment has already been captured...No further Guarantee Interest Payments Allowed ",,"Disbursement"
			window.btn_Exit.click
		end if
	end if

	if Trim(window.InterestApplied.Text) = "+" or Trim(window.InterestApplied.Text) = "-" then
		if window.btn_AddDisbursement.disabled = false or window.btn_DeleteDisbursement.disabled = false then
			CalculatePayment
		elseif window.btn_AddDisbursement.disabled = true then
			CalculatePayment
		end if
	elseif Trim(window.InterestApplied.Text) = "=" then

		window.CapitalAmount.Enabled = false
		window.CapitalAmount.Value = 0
		window.InterestRate.Enabled = false
		window.InterestRate.Value = 0
		window.InterestStartDate.Enabled = false
		window.InterestStartDate.Text = "__/__/____"
		window.GuaranteeAmount.Value = 0

		CalculateBalancePayment

		if ( window.btn_AddDisbursement.value = "Commit" ) and b_GuaranteeBalCaptured = true then
			Msgbox "A Balancing payment has already been captured... ",,"Disbursement"
			window.btn_Exit.click
		end if

		if ( window.btn_AddDisbursement.value = "Commit" or  window.btn_UpdateDisbursement.value = "Commit") and  d_BalancePayment = 0 then
			Msgbox "A Balancing payment cannot be captured until a '+' or '-' Transaction has been captured",,"Disbursement"
			window.btn_Exit.click
		end if

		exit sub
	end if
End Sub

Sub CalculatePayment
	Dim i_Days

	if ( window.btn_AddDisbursement.value <> "Commit" and window.btn_UpdateDisbursement.value <> "Commit" ) then exit sub
	if window.btn_AddDisbursement.disabled = true and window.btn_UpdateDisbursement.disabled = true and window.btn_DeleteDisbursement.disabled = false then exit sub
	if Trim(window.InterestApplied.Text) <> "+" and Trim(window.InterestApplied.Text) <> "-" then exit sub
	if window.InterestRate.Value = 0 then exit sub
	if window.CapitalAmount.Value = 0 then exit sub
	if window.GuaranteeAmount.Value = 0 then exit sub

	i_Days = (Date  - window.InterestStartDate.Value) + 1

	if i_Days < 0 then exit sub

	if Trim(window.InterestApplied.Text) = "+" then
		if i_currentxntype = 7 then
			window.PaymentAmount.Value = Round(window.GuaranteeAmount.Value + window.CapitalAmount.Value * (window.InterestRate.Value /100.00) * (i_Days/365.00),2)
		else
			window.PaymentAmount.Value = Round(window.GuaranteeAmount.Value  + window.CapitalAmount.Value * (window.InterestRate.Value /100.00) * (i_Days/365.00),2)
		end if
	elseif Trim(window.InterestApplied.Text) = "-" then
		if i_currentxntype = 7 then
			window.PaymentAmount.Value = Round(window.GuaranteeAmount.Value - window.CapitalAmount.Value * (window.InterestRate.Value /100.00) * (i_Days/365.00),2)
		else
			window.PaymentAmount.Value = Round(window.GuaranteeAmount.Value  - window.CapitalAmount.Value * (window.InterestRate.Value /100.00) * (i_Days/365.00),2)
		end if
	end if

End Sub

Sub CalculateBalancePayment
	Dim d_SumInterest
	Dim d_Interest
	d_SumInterest = 0.0

	if window.btn_AddDisbursement.disabled = true and _
	   window.btn_UpdateDisbursement.disabled = true and _
	   window.btn_DeleteDisbursement.disabled = true then exit sub

	sSQL = "r_GetDisburseGuaranteesByLoan " & i_CurrentLoanNbr
	rs_Guarantees.CursorLocation = 3
	rs_Guarantees.CacheSize = 10
	rs_Guarantees.Open sSQL, conn, adOpenDynamic

	if rs_Guarantees.RecordCount > 0 then
		b_GuaranteeBalCaptured = false
		rs_Guarantees.MoveFirst

		do while not rs_Guarantees.EOF
			if rs_Guarantees.Fields("DisbursementInterestApplied").Value <> "=" then
				i_Days = date - CDate(rs_Guarantees("DisbursementInterestStartDate").Value) + 1
				d_Interest = Round(rs_Guarantees.Fields("DisbursementCapitalAmount").Value * (rs_Guarantees.Fields("DisbursementInterestRate").Value /100) * (i_Days/365.00),2)

				if rs_Guarantees.Fields("DisbursementInterestApplied").Value = "-" then
					d_SumInterest = d_SumInterest - d_Interest
				else
					d_SumInterest = d_SumInterest + d_Interest
				end if
			else
				b_GuaranteeBalCaptured = true
			end if
			rs_Guarantees.MoveNext
		loop
		d_BalancePayment = window.GuaranteeAmount.Value - d_SumInterest
		if d_BalancePayment > 0 then
			window.PaymentAmount.Value = d_BalancePayment
		end if
	else
		window.PaymentAmount.Value = 0
	end if

	rs_Guarantees.Close
End Sub

Function UpdateRegMailDisburseAmounts
	dim i_PaymentNoInterestTot
	i_res = 0
	i_PaymentNoInterestTot = 0

	set com = createobject("ADODB.Command")
	set prm = createobject("ADODB.Parameter")
	set rs_temp = createobject("ADODB.Recordset")

	sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS [DisbursementsManage.asp 2];uid=<%= Session("UserID")%>"

	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPR

	sSQL = "r_UpdRegmailDisbursementAmounts"

	com.CommandText = sSQL

	set prm = com.CreateParameter ( "LoanNumber",19,1,,window.LoanNumber.Value) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set prm = com.CreateParameter ("GuaranteeAmount",5,1,, d_GuaranteeTotal )
	com.Parameters.Append prm

	set prm = com.CreateParameter ("CashRequired",5,1,,d_CashRequired) ' AdUnsigned Int
	com.Parameters.Append prm

	set rs_temp = com.Execute

End Function

Sub InterestRate_Change
if window.btn_AddDisbursement.disabled = false or window.btn_DeleteDisbursement.disabled = false then
	CalculatePayment
elseif window.btn_AddDisbursement.disabled = true then
	CalculatePayment
end if
End Sub

Sub GuaranteeAmount_Change
	if Trim(window.InterestApplied.Text) = "+" or Trim(window.InterestApplied.Text) = "-" then
		if window.btn_AddDisbursement.disabled = false or window.btn_DeleteDisbursement.disabled = false then
			CalculatePayment
		elseif window.btn_AddDisbursement.disabled = true then
			CalculatePayment
		end if

	elseif Trim(window.InterestApplied.Text) = "=" then
		window.CapitalAmount.Enabled = false
		window.InterestRate.Enabled = false

		if  b_AllDataLoaded = true then
			CalculateBalancePayment
		end if
		exit sub
	end if
End Sub

Sub CapitalAmount_Change
if window.btn_AddDisbursement.disabled = false or window.btn_DeleteDisbursement.disabled = false then
	CalculatePayment
elseif window.btn_AddDisbursement.disabled = true then
	CalculatePayment
end if
End Sub

Sub InterestStartDate_Change
if b_AllDataLoaded = false then exit sub
if isdate(window.InterestStartDate) then
	if window.btn_AddDisbursement.disabled = false or window.btn_DeleteDisbursement.disabled = false then
		if isdate(window.InterestStartDate) then
			CalculatePayment
		end if
	elseif window.btn_AddDisbursement.disabled = true then
		CalculatePayment
	end if
end if
End Sub

Sub DisbursementAmount_Change
if (i_currentxntype = 0 or i_currentxntype = 2 or i_currentxntype = 3 or i_currentxntype = 4 or i_currentxntype = 5   or i_currentxntype = 6 or i_currentxntype = 8 ) and window.InterestApplied.Text <> ""  then
	window.InterestApplied.Text = ""
	window.InterestRate.Value = 0
	window.InterestStartDate.Text = "__/__/____"
	window.GuaranteeAmount.Value = 0
	window.CapitalAmount.Value = 0
	window.PaymentAmount.Value = 0

end if
End Sub

Sub DataCombo_PaymentType_Change

if Cint(DataCombo_PaymentType.BoundText) = 0   then
	i_currentxntype = 0
	window.InterestApplied.Text = ""
	window.InterestRate.Value = 0
	window.InterestStartDate.Text = "__/__/____"
	window.CapitalAmount.Value = 0
	window.GuaranteeAmount.Value = 0
	window.PaymentAmount.Value = 0
	if window.btn_AddDisbursement.value <> "Commit" and window.btn_UpdateDisbursement.value <> "Commit" then
		exit sub
	else
		EnableFields
	end if
   	if window.btn_AddDisbursement.disabled = false or window.btn_UpdateDisbursement.disabled = false then
		if pnl_Msg.style.visibility = "visible" then
			window.TrueDBCombo_ACBBank.Enabled = true
			window.TrueDBCombo_ACBBank.focus
		else
			window.DisbursementAmount.Enabled = true
			window.DisbursementAmount.focus
		end if
	end if
end if

if Cint(DataCombo_PaymentType.BoundText) = 1 then

	i_currentxntype = 1

	if window.btn_AddDisbursement.value <> "Commit" and window.btn_UpdateDisbursement.value <> "Commit" then
		exit sub
	else
		EnableFields
	end if
	if window.btn_AddDisbursement.disabled = false or window.btn_UpdateDisbursement.disabled = false then
		if pnl_Msg.style.visibility = "visible" then
			window.TrueDBCombo_ACBBank.Enabled = true
			window.TrueDBCombo_ACBBank.focus
		else
			window.InterestApplied.Enabled = true
			window.InterestApplied.focus
		end if
	end if
end if

if Cint(DataCombo_PaymentType.BoundText) = 2  then
	i_currentxntype = 2
	window.InterestApplied.Text = ""
	window.InterestRate.Value = 0
	window.InterestStartDate.Text = "__/__/____"
	window.CapitalAmount.Value = 0
	window.GuaranteeAmount.Value = 0
	window.PaymentAmount.Value = 0
	if window.btn_AddDisbursement.value <> "Commit" and window.btn_UpdateDisbursement.value <> "Commit" then
		exit sub
	else
		EnableFields
	end if
	if window.btn_AddDisbursement.disabled = false or window.btn_UpdateDisbursement.disabled = false then
		if pnl_Msg.style.visibility = "visible" then
			window.TrueDBCombo_ACBBank.Enabled = true
			window.TrueDBCombo_ACBBank.focus
		else
			window.DisbursementAmount.Enabled = true
			window.DisbursementAmount.focus
		end if
	end if
end if

if  Cint(DataCombo_PaymentType.BoundText) = 3 then
	i_currentxntype = 3
	window.InterestApplied.Text = ""
	window.InterestRate.Value = 0
	window.InterestStartDate.Text = "__/__/____"
	window.CapitalAmount.Value = 0
	window.GuaranteeAmount.Value = 0
	window.PaymentAmount.Value = 0
	if window.btn_AddDisbursement.value <> "Commit" and window.btn_UpdateDisbursement.value <> "Commit" then
		exit sub
	else
		EnableFields
	end if
	if window.btn_AddDisbursement.disabled = false or window.btn_UpdateDisbursement.disabled = false then
		if pnl_Msg.style.visibility = "visible" then
			window.TrueDBCombo_ACBBank.Enabled = true
			window.TrueDBCombo_ACBBank.focus
		else
			window.DisbursementAmount.Enabled = true
			window.DisbursementAmount.focus
		end if
	end if
end if

if  Cint(DataCombo_PaymentType.BoundText) = 4 then

	i_currentxntype = 4
	window.InterestApplied.Text = ""
	window.InterestRate.Value = 0
	window.InterestStartDate.Text = "__/__/____"
	window.CapitalAmount.Value = 0
	window.GuaranteeAmount.Value = 0
	window.PaymentAmount.Value = 0
	if window.btn_AddDisbursement.value <> "Commit" and window.btn_UpdateDisbursement.value <> "Commit" then
		exit sub
	else
		EnableFields
	end if
	if window.btn_AddDisbursement.disabled = false or window.btn_UpdateDisbursement.disabled = false then
		if pnl_Msg.style.visibility = "visible" then
			window.TrueDBCombo_ACBBank.Enabled = true
			window.TrueDBCombo_ACBBank.focus
		else
			window.DisbursementAmount.Enabled = true
			window.DisbursementAmount.focus
		end if
	end if
end if

' added in to accommodate quick cash

if Cint(DataCombo_PaymentType.BoundText) = 8  then
	i_currentxntype = 8
	window.InterestApplied.Text = ""
	window.InterestRate.Value = 0
	window.InterestStartDate.Text = "__/__/____"
	window.CapitalAmount.Value = 0
	window.GuaranteeAmount.Value = 0
	window.PaymentAmount.Value = 0
	if window.btn_AddDisbursement.value <> "Commit" and window.btn_UpdateDisbursement.value <> "Commit" then
		exit sub
	else
		EnableFields
	end if
	if window.btn_AddDisbursement.disabled = false or window.btn_UpdateDisbursement.disabled = false then
		if pnl_Msg.style.visibility = "visible" then
			window.TrueDBCombo_ACBBank.Enabled = true
			window.TrueDBCombo_ACBBank.focus
		else
			window.DisbursementAmount.Enabled = true
			window.DisbursementAmount.focus
		end if
	end if
end if

if  Cint(DataCombo_PaymentType.BoundText) = 5 then
	i_currentxntype = 5
	window.InterestApplied.Text = ""
	window.InterestRate.Value = 0
	window.InterestStartDate.Text = "__/__/____"
	window.CapitalAmount.Value = 0
	window.GuaranteeAmount.Value = 0
	window.PaymentAmount.Value = 0
	if window.btn_AddDisbursement.value <> "Commit" and window.btn_UpdateDisbursement.value <> "Commit" then
		exit sub
	else
		EnableFields
	end if
	if window.btn_AddDisbursement.disabled = false or window.btn_UpdateDisbursement.disabled = false then
		if pnl_Msg.style.visibility = "visible" then
			window.TrueDBCombo_ACBBank.Enabled = true
			window.TrueDBCombo_ACBBank.focus
		else
			window.DisbursementAmount.Enabled = true
			window.DisbursementAmount.focus
		end if
	end if
end if

if  Cint(DataCombo_PaymentType.BoundText) = 6 then
	i_currentxntype = 6
	window.InterestApplied.Text = ""
	window.InterestRate.Value = 0
	window.InterestStartDate.Text = "__/__/____"
	window.CapitalAmount.Value = 0
	window.GuaranteeAmount.Value = 0
	window.PaymentAmount.Value = 0
	if window.btn_AddDisbursement.value <> "Commit" and window.btn_UpdateDisbursement.value <> "Commit" then
		exit sub
	else
		EnableFields
	end if
	if window.btn_AddDisbursement.disabled = false or window.btn_UpdateDisbursement.disabled = false then
		if pnl_Msg.style.visibility = "visible" then
			window.TrueDBCombo_ACBBank.Enabled = true
			window.TrueDBCombo_ACBBank.focus
		else
			window.DisbursementAmount.Enabled = true
			window.DisbursementAmount.focus
		end if
	end if
end if

if Cint(DataCombo_PaymentType.BoundText) = 7 then
	i_currentxntype = 7

	if window.btn_AddDisbursement.value <> "Commit" and window.btn_UpdateDisbursement.value <> "Commit" then
		exit sub
	else
		EnableFields
	end if
	if window.btn_AddDisbursement.disabled = false or window.btn_UpdateDisbursement.disabled = false then
		if pnl_Msg.style.visibility = "visible" then
			window.TrueDBCombo_ACBBank.Enabled = true
			window.TrueDBCombo_ACBBank.focus
		else
			window.InterestApplied.Enabled = true
			window.InterestApplied.focus
		end if
	end if
end if
End Sub

Sub ShowLoanPurpose
set rs_temp = createobject("ADODB.Recordset")
set conn = createobject("ADODB.Connection")
sDSN = GetConnectionString("[DisbursementManage.asp 1]")
conn.CursorLocation = 1
conn.Open sDSN
sSQL = "SELECT [Description] as  PurposeDescription FROM [2am].dbo.MortgageLoanPurpose (nolock) WHERE MortgageLoanPurposeKey = " &  i_Purpose

rs_temp.CursorLocation = 3
rs_temp.Open sSQL ,conn,adOpenStatic
window.LoanPurpose.Text = ""
if rs_temp.RecordCount > 0 then
	window.LoanPurpose.Text = rs_temp.fields(0).value
end if
End Sub

Function GetDisburseStatus( i_LoanNumber )
GetDisburseStatus = 0
set rs_temp = createobject("ADODB.Recordset")
set conn = createobject("ADODB.Connection")
sDSN = GetConnectionString("[DisbursementManage.asp 5]")
conn.CursorLocation = 1
conn.Open sDSN
sSQL =  "SELECT LoanInitialBalance FROM LOAN (nolock)  WHERE LoanNumber =  " & i_LoanNumber & " and LoanNumber in ( SELECT LoanNumber FROM vw_OpenLoans (nolock) WHERE LoanNumber = " & i_LoanNumber &  " )"
rs_temp.CursorLocation = 3
rs_temp.Open sSQL ,conn,adOpenStatic
if rs_temp.RecordCount > 0 then
	GetDisburseStatus = rs_temp.fields(0).value
end if
rs_temp.close
End Function

-->
</script>

</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<body bottomMargin=0 class=Generic leftMargin=0 rightMargin=0 topMargin=0>
<p>
<table cellPadding=1 cellSpacing=1 class=Table1 id=tbl_Client
style="HEIGHT: 86px; LEFT: 20px; POSITION: absolute; TOP: 0px; WIDTH: 635px; Z-INDEX: 110"
width="75%">

  <tr>
    <td align=right noWrap>&nbsp;Loan Number</TD>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 height=24
      id=LoanNumber style="HEIGHT: 24px; LEFT: 1px; TOP: 1px; WIDTH: 99px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2619"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0;;Null"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="-65535"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD></TR>
  <tr>
    <td align=right noWrap>Client First Names</TD>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=24
      id=LoanFirstNames style="HEIGHT: 24px; LEFT: 1px; TOP: 1px; WIDTH: 411px"
      tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="10874"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <td align=right noWrap>Client Surname</TD>
    <td noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D height=24
      id=LoanSurname style="HEIGHT: 24px; LEFT: 1px; TOP: 1px; WIDTH: 411px"
      tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="10874"><PARAM NAME="_ExtentY" VALUE="635"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR></TABLE>
<OBJECT classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0
data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAYAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAHAAAAAQAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAgAAAAAAAAAAEAAAAwMDAAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAIAAAAAAAAAABAAAAP//xgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAAMG24AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAwMDAAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAP//xgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAAMG24AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAADBtuAD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwAACEAAADQAAAAfCE6D3whOg8BAAAAAAAAAB0AAABIZWFkaW5nAAEAAAAAAAAAAAAAAAAAAAAhAAAA8AAAAB4AAABGb290aW5nACEAAAAwAQAArCE6D6whOg8AAAAAAAEAAB8AAABTZWxlY3RlZAAAAAAAAQAAAAAAAAAAAAAhAAAAUAEAACAAAABDYXB0aW9uACEAAACwAQAA7CE6D+whOg8AAAAAAAAAACEAAABIaWdobGlnaHRSb3cAQwoEkGU7D3BlOw8hAAAA0AEAACIAAABFdmVuUm93ACEAAAAgAAAADDs7DyQhOg8CAAAAAEAKBCMAAABPZGRSb3cADwAAAAAAQgoEAAAAAAAAAAAhAAAAQAAAACQAAAALAAAA//8AAAMAAAAAAAAACwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAP//AAADAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAAAAAAAAsAAAAAAAAAAwAAAAEAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAAeAAAAMgAAAERyYWcgYSBjb2x1bW4gaGVhZGVyIGhlcmUgdG8gZ3JvdXAgYnkgdGhhdCBjb2x1bW4AAAADAAAAAAAAAAMAAAAAAAAAAwAAAMgAAAADAAAAAAAAAAMAAAD3ogYAAwAAAJDQAwA9AAAAAAAAAAsAAABUcnVlREJHcmlkAAIAAAAMAAAAQWxsb3dBZGROZXcALwAAAAwAAABBbGxvd0Fycm93cwABAAAADAAAAEFsbG93RGVsZXRlAAQAAAAMAAAAQWxsb3dVcGRhdGUAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQAI/v//DAAAAEJvcmRlclN0eWxlAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAnAAAAAsAAABEYXRhTWVtYmVyACUAAAAJAAAARGF0YU1vZGUAuwAAAAkAAABEYXRhVmlldwDEAAAAEgAAAERlYWRBcmVhQmFja0NvbG9yAAoAAAAMAAAARGVmQ29sV2lkdGgAUAAAAA0AAABFZGl0RHJvcERvd24AXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkADAAAAAPAAAARXhwb3NlQ2VsbE1vZGUAkQAAAAoAAABGb290TGluZXMAwgAAAA8AAABHcm91cEJ5Q2FwdGlvbgAMAAAACgAAAEhlYWRMaW5lcwCYAAAACwAAAEluc2VydE1vZGUAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAsQAAAAoAAABMYXlvdXRVUkwASgAAAA4AAABNYXJxdWVlVW5pcXVlAM4AAAAIAAAATWF4Um93cwCjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgCEAAAADAAAAE11bHRpU2VsZWN0AGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUAlwAAABEAAABQaWN0dXJlQWRkbmV3Um93AJUAAAASAAAAUGljdHVyZUN1cnJlbnRSb3cAswAAABEAAABQaWN0dXJlRm9vdGVyUm93ALIAAAARAAAAUGljdHVyZUhlYWRlclJvdwCWAAAAEwAAAFBpY3R1cmVNb2RpZmllZFJvdwCwAAAAEwAAAFBpY3R1cmVTdGFuZGFyZFJvdwC0AAAACwAAAFByaW50SW5mb3MADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAIwAAAAcAAABTcGxpdHMAMQAAABAAAABUYWJBY3Jvc3NTcGxpdHMAMgAAAAoAAABUYWJBY3Rpb24AmQAAABcAAABUcmFuc3BhcmVudFJvd1BpY3R1cmVzADMAAAAQAAAAV3JhcENlbGxQb2ludGVyANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA
height=140 id=TrueDBGrid
style="HEIGHT: 140px; LEFT: 20px; POSITION: absolute; TOP: 86px; WIDTH: 865px; Z-INDEX: 109"
width=871></OBJECT>
</P>
<OBJECT classid="clsid:0D623583-DBA2-11D1-B5DF-0060976089D0" data=data:application/x-oleobject;base64,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AAAAAAAAAAD//////////wAAAAAHAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwABXRSb3cA8RkFYPEZBfDwGQWA8BkFEPAZBR0AAABIZWFkaW5nAGDLGQXwyhkFgMoZBbDJGQWAjhkF4I0ZBR4AAABGb290aW5nAAAAAABAD/8HAAD//zAP/wcxAAAAQAAAAB8AAABTZWxlY3RlZAAAAAAcAAAAAAAAAP////8AAAAAAAAAACAAAABDYXB0aW9uABIAAIAPAACAAAAAAAAAAAAAAAAAQA//ByEAAABIaWdobGlnaHRSb3cAAAAADPAZBSgjGAWgjRkFQI4ZBSIAAABFdmVuUm93AAAgQAEAAAAA9v///wAgQAExAAAAEAAAACMAAABPZGRSb3cAAO7fn/0AAAAAHQAAAO7fH/0xAAAAQAAAACQAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAsAAAD//wAAAwAAAAAAAAAeAAAAEwAAAEFDQkJhbmtEZXNjcmlwdGlvbgAAHgAAAAwAAABBQ0JCYW5rQ29kZQALAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAAAAAAAAsAAAD//wAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADQBwAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAAAAAAALAAAAAAAAADIAAAAAAAAAFAAAAFRydWVEQkNvbWJvX0FDQkJhbmsAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQCFAAAADwAAAEF1dG9Db21wbGV0aW9uAIIAAAANAAAAQXV0b0Ryb3Bkb3duAAj+//8MAAAAQm9yZGVyU3R5bGUAcwAAAAwAAABCb3VuZENvbHVtbgD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAIEAAAALAAAAQ29tYm9TdHlsZQAlAAAACQAAAERhdGFNb2RlAAoAAAAMAAAARGVmQ29sV2lkdGgAwQAAABEAAABEcm9wZG93blBvc2l0aW9uAIgAAAAJAAAARWRpdEZvbnQAXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkAJEAAAAKAAAARm9vdExpbmVzAAwAAAAKAAAASGVhZExpbmVzAGUAAAAPAAAASW50ZWdyYWxIZWlnaHQAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAhwAAAAwAAABMaW1pdFRvTGlzdABjAAAACgAAAExpc3RGaWVsZADFAAAABwAAAExvY2tlZAC8AAAAEgAAAE1hdGNoRW50cnlUaW1lb3V0AKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAnAAAAAoAAABSb3dNZW1iZXIAgwAAAAwAAABSb3dUcmFja2luZwAjAAAABwAAAFNwbGl0cwADAgAAEAAAAF9Ecm9wZG93bkhlaWdodAACAgAADwAAAF9Ecm9wZG93bldpZHRoANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA
	height=115 id=TrueDBCombo_ACBBank style="BACKGROUND-COLOR: #ffd69d; HEIGHT: 115px; LEFT: 410px; TOP: 261px; WIDTH: 241px; Z-INDEX: -97; POSITION: ABSOLUTE;"
	tabIndex=10 width=241></OBJECT>
<OBJECT id=TrueDBCombo_ACBBranch style="FONT-SIZE: smaller; Z-INDEX: 103; LEFT: 410px; VISIBILITY: visible; WIDTH: 275px; TOP: 289px; HEIGHT: 231px; BACKGROUND-COLOR: #ffd69d; POSITION: ABSOLUTE;"
	tabIndex=11 height=231 width=275 data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB0AAABIZWFkaW5nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB4AAABGb290aW5nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB8AAABTZWxlY3RlZAAAAAAAAAAAAAAAAAAAAAAAAAAA0PI3BCAAAABDYXB0aW9uAAAAAAAAAAAAAAAAAAAAAAAAAAAA0PI3BCEAAABIaWdobGlnaHRSb3cA8jcEAAAAAMDyNwQAAAAAAAAAACIAAABFdmVuUm93AAAAAAAAAAAAsPI3BFANKQUdAAAAHgAAACMAAABPZGRSb3cAAAAAAAAAAAAAsPI3BCAOKQUdAAAAHwAAACQAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAsAAAD//wAAAwAAAAAAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAALAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAAAAAAAAsAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADQBwAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAAAAAAAwAAAAAAAAABYAAABUcnVlREJDb21ib19BQ0JCcmFuY2gAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQCFAAAADwAAAEF1dG9Db21wbGV0aW9uAIIAAAANAAAAQXV0b0Ryb3Bkb3duAAj+//8MAAAAQm9yZGVyU3R5bGUAcwAAAAwAAABCb3VuZENvbHVtbgD6/f//CAAAAENhcHRpb24AYAAAAAkAAABDZWxsVGlwcwB+AAAADgAAAENlbGxUaXBzRGVsYXkAfQAAAA4AAABDZWxsVGlwc1dpZHRoAI8AAAAOAAAAQ29sdW1uRm9vdGVycwAHAAAADgAAAENvbHVtbkhlYWRlcnMACAAAAAgAAABDb2x1bW5zAIEAAAALAAAAQ29tYm9TdHlsZQAlAAAACQAAAERhdGFNb2RlAAoAAAAMAAAARGVmQ29sV2lkdGgAwQAAABEAAABEcm9wZG93blBvc2l0aW9uAIgAAAAJAAAARWRpdEZvbnQAXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkAJEAAAAKAAAARm9vdExpbmVzAAwAAAAKAAAASGVhZExpbmVzAGUAAAAPAAAASW50ZWdyYWxIZWlnaHQAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAhwAAAAwAAABMaW1pdFRvTGlzdABjAAAACgAAAExpc3RGaWVsZAC8AAAAEgAAAE1hdGNoRW50cnlUaW1lb3V0AKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAnAAAAAoAAABSb3dNZW1iZXIAgwAAAAwAAABSb3dUcmFja2luZwAjAAAABwAAAFNwbGl0cwADAgAAEAAAAF9Ecm9wZG93bkhlaWdodAACAgAADwAAAF9Ecm9wZG93bldpZHRoANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMA
	classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 VIEWASTEXT></OBJECT>
<OBJECT id=TrueDBCombo_ACBType style="FONT-SIZE: smaller; Z-INDEX: -95; LEFT: 410px; WIDTH: 181px; TOP: 344px; HEIGHT: 120px; BACKGROUND-COLOR: #ffd69d; POSITION: ABSOLUTE;"
	tabIndex=12 height=120 width=181 data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwAAAoAAAAAAAAAAAAfBQBYIgX//wAADOMTAB0AAABIZWFkaW5nAGVyU3R5bGUA8P///yEAAAAAAAAADwAAAB4AAABGb290aW5nAMAuKQWQLykFMC8pBbBFKQVgQykFAEMpBR8AAABTZWxlY3RlZAA0KQWgNCkF8DQpBUA1KQWQNSkF4DUpBSAAAABDYXB0aW9uAAAAAABGb290ZXJUZXh0AAVUChEGDOMTACEAAABIaWdobGlnaHRSb3cAoRsG0wcAAAAAAAAAAAMCAAAAACIAAABFdmVuUm93AAACAAAAAAAAAAASdwShGwYBAACAJEA3BCMAAABPZGRSb3cAAAAAAAAAAAAAAAAfBcTiEwAAAAAAlq4eBSQAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAsAAAD//wAAAwAAAAAAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAALAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAAAAAAAAsAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAADQBwAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAAAAAAAwAAAAAAAAABQAAABUcnVlREJDb21ib19BQ0JUeXBlAL0AAAAOAAAAQW5pbWF0ZVdpbmRvdwDAAAAAEwAAAEFuaW1hdGVXaW5kb3dDbG9zZQC+AAAAFwAAAEFuaW1hdGVXaW5kb3dEaXJlY3Rpb24AvwAAABIAAABBbmltYXRlV2luZG93VGltZQD4/f//CwAAAEFwcGVhcmFuY2UAhQAAAA8AAABBdXRvQ29tcGxldGlvbgCCAAAADQAAAEF1dG9Ecm9wZG93bgAI/v//DAAAAEJvcmRlclN0eWxlAHMAAAAMAAAAQm91bmRDb2x1bW4A+v3//wgAAABDYXB0aW9uAGAAAAAJAAAAQ2VsbFRpcHMAfgAAAA4AAABDZWxsVGlwc0RlbGF5AH0AAAAOAAAAQ2VsbFRpcHNXaWR0aACPAAAADgAAAENvbHVtbkZvb3RlcnMABwAAAA4AAABDb2x1bW5IZWFkZXJzAAgAAAAIAAAAQ29sdW1ucwCBAAAACwAAAENvbWJvU3R5bGUAJQAAAAkAAABEYXRhTW9kZQAKAAAADAAAAERlZkNvbFdpZHRoAMEAAAARAAAARHJvcGRvd25Qb3NpdGlvbgCIAAAACQAAAEVkaXRGb250AF8AAAAKAAAARW1wdHlSb3dzAP79//8IAAAARW5hYmxlZACRAAAACgAAAEZvb3RMaW5lcwAMAAAACgAAAEhlYWRMaW5lcwBlAAAADwAAAEludGVncmFsSGVpZ2h0AF0AAAAPAAAATGF5b3V0RmlsZU5hbWUAXAAAAAsAAABMYXlvdXROYW1lAIcAAAAMAAAATGltaXRUb0xpc3QAYwAAAAoAAABMaXN0RmllbGQAvAAAABIAAABNYXRjaEVudHJ5VGltZW91dACjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgBhAAAADgAAAE11bHRpcGxlTGluZXMAnwAAAAwAAABPTEVEcmFnTW9kZQCgAAAADAAAAE9MRURyb3BNb2RlAA8AAAAQAAAAUm93RGl2aWRlclN0eWxlAJwAAAAKAAAAUm93TWVtYmVyAIMAAAAMAAAAUm93VHJhY2tpbmcAIwAAAAcAAABTcGxpdHMAAwIAABAAAABfRHJvcGRvd25IZWlnaHQAAgIAAA8AAABfRHJvcGRvd25XaWR0aADTBwAACQAAAF9FeHRlbnRYANQHAAAJAAAAX0V4dGVudFkAAAIAAAwAAABfTGF5b3V0VHlwZQAQAAAACwAAAF9Sb3dIZWlnaHQAAQIAAAsAAABfU3R5bGVEZWZzAA==
	classid=clsid:0D623583-DBA2-11D1-B5DF-0060976089D0 VIEWASTEXT></OBJECT>
<table border=0 cellPadding=1 cellSpacing=1 class=Table1 height=286
id=tbl_Button
style="HEIGHT: 290px; LEFT: 721px; POSITION: absolute; TOP: 259px; WIDTH: 164px; Z-INDEX: 100"
width="75%">

  <tr>
    <td align=middle><input class=button3 id=btn_AddDisbursement name=btn_AddDisbursement style="CURSOR: hand; HEIGHT: 55px; LEFT: 673px; PADDING-TOP: 15px; TOP: 526px; VERTICAL-ALIGN: sub; VISIBILITY: visible; WIDTH: 136px; Z-INDEX: 102" title="Add Disbursement" type=button value="Add Disbursement"></TD></TR>
  <tr>
    <td align=middle><input class=button3 id=btn_UpdateDisbursement name=btn_UpdateDisbursement style="CURSOR: hand; HEIGHT: 55px; LEFT: 259px; PADDING-TOP: 15px; TOP: 510px; VERTICAL-ALIGN: sub; VISIBILITY: visible; WIDTH: 136px; Z-INDEX: 103" title="Update Disbursement" type=button value="Update Disbursement"></TD></TR>
  <tr>
    <td align=middle><input class=button3 id=btn_DeleteDisbursement name=button1 style="HEIGHT: 55px; PADDING-TOP: 25px; WIDTH: 136px" title="Delete Disbursement" type=button value="Delete Disbursement" width="136" height="55"></TD></TR>
  <tr>
    <td align=middle><input class=button2 id=btn_Exit name=btn_Exit style="CURSOR: hand; HEIGHT: 55px; LEFT: 709px; TOP: 505px; VISIBILITY: visible; WIDTH: 136px; Z-INDEX: 101" title=Exit type=button value=Exit></TD></TR></TABLE><img
alt="" border=0 height=23 hspace=0 id=pic_AddDisbursement
src="images/MLSDenied.bmp"
style="CURSOR: hand; HEIGHT: 23px; LEFT: 790px; POSITION: absolute; TOP: 279px; WIDTH: 19px; Z-INDEX: 104"
title=0 useMap="" width=19> <img alt="" border=0 height=23
hspace=0 id=pic_UpdateDisbursement
src="images/MLSDenied.bmp"
style="CURSOR: hand; HEIGHT: 23px; LEFT: 790px; POSITION: absolute; TOP: 354px; WIDTH: 19px; Z-INDEX: 105"
title=0 useMap="" width=19> <img alt="" border=0 height=23
hspace=0 id=pic_DeleteDisbursement
src="images/MLSDenied.bmp"
style="CURSOR: hand; HEIGHT: 23px; LEFT: 790px; POSITION: absolute; TOP: 420px; WIDTH: 19px; Z-INDEX: 116"
title=0 useMap="" width=19>&nbsp; <img alt="" border=0
height=23 hspace=0 id=pic_GenerateDisbursements
src="images/MLSDenied.bmp"
style="CURSOR: hand; HEIGHT: 23px; LEFT: 762px; POSITION: absolute; TOP: 16px; WIDTH: 19px; Z-INDEX: 107"
title=0 useMap="" width=19>
<table border=0 cellPadding=1 cellSpacing=1 class=Table1 height=31 id=tbl_Total
style="FONT-SIZE: smaller; HEIGHT: 31px; LEFT: 721px; POSITION: absolute; TOP: 227px; WIDTH: 100px; Z-INDEX: 108"
width=100>

  <tr>
    <td align=right noWrap>Total</TD>
    <td noWrap>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" height=22 id=DisbursementTotal
	name=DisbursementTotal style="HEIGHT: 22px; LEFT: 1px; TOP: 1px; WIDTH: 120px" width=120>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
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
	<PARAM NAME="DisplayFormat" VALUE="###,###,###,##0.00">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="-1">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="###########0.00">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxValue" VALUE="999999999999999">
	<PARAM NAME="MinValue" VALUE="-999999999999999">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="NegativeColor" VALUE="255">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="Separator" VALUE=",">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ValueVT" VALUE="139788289">
	<PARAM NAME="Value" VALUE="0">
	<PARAM NAME="MaxValueVT" VALUE="6750213">
	<PARAM NAME="MinValueVT" VALUE="3538949"></OBJECT>
</TD></TR></TABLE><input class=button2 id=btn_Print name=btn_Print style="CURSOR: hand; HEIGHT: 55px; LEFT: 568px; POSITION: absolute; TOP: 481px; VISIBILITY: visible; WIDTH: 136px; Z-INDEX: 112" type=button value="Print Preview">

<table border=0 cellPadding=1 cellSpacing=1 class=Table1 id=tbl_Generate
style="HEIGHT: 21px; LEFT: 681px; POSITION: absolute; TOP: 9px; WIDTH: 200px; Z-INDEX: 102"
width=200>

  <tr>
    <td align=middle><input class=button3 id=btn_GenerateDisbursements name=btn_GenerateDisbursements size=25 style="CURSOR: hand; HEIGHT: 61px; LEFT: 721px; PADDING-TOP: 24px; TOP: 18px; VERTICAL-ALIGN: middle; VISIBILITY: visible; WIDTH: 178px; Z-INDEX: 103" title="Generate Disbursements" type=button value="Generate CATS Transactions"></TD></TR></TABLE>
<table border=0 cellPadding=1 cellSpacing=1 class=Table1 height=100
id=tbl_DisbursementDetails
style="FONT-FAMILY: MS Sans Serif; HEIGHT: 100px; LEFT: 21px; POSITION: absolute; TOP: 258px; WIDTH: 700px; Z-INDEX: 111"
width=700>

  <tr>
    <td align=right>Disbursement Number</TD>
    <td>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" height=24 id=DisbursementNumber
	style="HEIGHT: 24px; LEFT: 1px; TOP: 1px; WIDTH: 100px" width=100>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="2646">
	<PARAM NAME="_ExtentY" VALUE="635">
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
	<PARAM NAME="ValueVT" VALUE="2012807169">
	<PARAM NAME="Value" VALUE="0">
	<PARAM NAME="MaxValueVT" VALUE="5">
	<PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <td align=right>ACB Bank</TD>
    <td>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" height=24 id=BankCode
	name=BankCode style="FONT-SIZE: smaller; HEIGHT: 24px; LEFT: 1px; TOP: 1px; VISIBILITY: hidden; WIDTH: 175px"
	width=175>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4630">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <td align=right>Prepared Date</TD>
    <td>
      <OBJECT classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830" height=26 id=PreparedDate
	style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 120px" tabIndex=1 width=120>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
	<PARAM NAME="_ExtentY" VALUE="688">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="0">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="CursorPosition" VALUE="0">
	<PARAM NAME="DataProperty" VALUE="0">
	<PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="FirstMonth" VALUE="4">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="dd/mm/yyyy">
	<PARAM NAME="HighlightText" VALUE="0">
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
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ShowLiterals" VALUE="0">
	<PARAM NAME="TabAction" VALUE="0">
	<PARAM NAME="Text" VALUE="__/__/____">
	<PARAM NAME="ValidateMode" VALUE="0">
	<PARAM NAME="ValueVT" VALUE="1179649">
	<PARAM NAME="Value" VALUE="2.63417926253582E-308">
	<PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</TD>
    <td align=right>ACB Branch</TD>
    <td></TD></TR>
  <tr>
    <td align=right>Action Date</TD>
    <td>
      <OBJECT classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830" id=ActionDate style="HEIGHT: 23px; LEFT: 1px; TOP: 1px; WIDTH: 120px"
	tabIndex=2 width=120>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
	<PARAM NAME="_ExtentY" VALUE="609">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="0">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="CursorPosition" VALUE="0">
	<PARAM NAME="DataProperty" VALUE="0">
	<PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="FirstMonth" VALUE="4">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="dd/mm/yyyy">
	<PARAM NAME="HighlightText" VALUE="0">
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
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ShowLiterals" VALUE="0">
	<PARAM NAME="TabAction" VALUE="0">
	<PARAM NAME="Text" VALUE="__/__/____">
	<PARAM NAME="ValidateMode" VALUE="0">
	<PARAM NAME="ValueVT" VALUE="1179649">
	<PARAM NAME="Value" VALUE="2.63417926253582E-308">
	<PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</TD>
    <td align=right>Code </TD>
    <td>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" height=24 id=BranchCode
	style="FONT-SIZE: smaller; HEIGHT: 24px; LEFT: 0px; TOP: 0px; VISIBILITY: visible"
	tabIndex=12>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="2117">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <td align=right>Disbursement Amt</TD>
    <td>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" id=DisbursementAmount
	style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 120px" tabIndex=3 width=120>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
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
	<PARAM NAME="DisplayFormat" VALUE="##,###,###,##0.00">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="#########0.00">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxValue" VALUE="99999999.99">
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
</TD>
    <td align=right>ACB Account Type</TD>
    <td>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" height=24 id=AccountType
	style="FONT-SIZE: smaller; HEIGHT: 24px; LEFT: 0px; TOP: 0px; VISIBILITY: hidden">
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="2117">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <td align=right style="COLOR: yellow">Interest
      Applied&nbsp;&nbsp;</TD>
    <td>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" height=26 id=InterestApplied
	style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 29px" tabIndex=4>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="767">
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
	<PARAM NAME="Format" VALUE="@">
	<PARAM NAME="FormatMode" VALUE="0">
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
</TD>
    <td align=right>Account Name</TD>
    <td>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" height=26 id=AccountName
	style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 300px" tabIndex=13 width=300>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="7937">
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
</TD></TR>
  <tr>
    <td align=right style="COLOR: yellow">Interest
Rate</TD>
    <td>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" id=InterestRate style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 57px"
	tabIndex=5>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="1508">
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
	<PARAM NAME="DisplayFormat" VALUE="#######0.00">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="######0.00">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxValue" VALUE="99999999.99">
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
</TD>
    <td align=right>Account Number</TD>
    <td>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" height=26 id=AccountNumber
	style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 204px" tabIndex=14>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="5397">
	<PARAM NAME="_ExtentY" VALUE="688">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="ShowContextMenu" VALUE="1">
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
	<PARAM NAME="MaxLength" VALUE="14">
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
</TD></TR>
  <tr>
    <td align=right style="COLOR: yellow">Interest Start
      Date</TD>
    <td>
      <OBJECT classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830" height=26 id=InterestStartDate
	style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 120px" tabIndex=6 width=120>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
	<PARAM NAME="_ExtentY" VALUE="688">
	<PARAM NAME="AlignHorizontal" VALUE="0">
	<PARAM NAME="AlignVertical" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="BackColor" VALUE="-2147483643">
	<PARAM NAME="BorderStyle" VALUE="1">
	<PARAM NAME="BtnPositioning" VALUE="0">
	<PARAM NAME="ClipMode" VALUE="0">
	<PARAM NAME="CursorPosition" VALUE="0">
	<PARAM NAME="DataProperty" VALUE="0">
	<PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="FirstMonth" VALUE="4">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="dd/mm/yyyy">
	<PARAM NAME="HighlightText" VALUE="0">
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
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ShowLiterals" VALUE="0">
	<PARAM NAME="TabAction" VALUE="0">
	<PARAM NAME="Text" VALUE="__/__/____">
	<PARAM NAME="ValidateMode" VALUE="0">
	<PARAM NAME="ValueVT" VALUE="1179649">
	<PARAM NAME="Value" VALUE="2.63417926253582E-308">
	<PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</TD>
    <td align=right>SPV Description</TD>
    <td>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" height=26 id=SPVDescription
	style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 290px" tabIndex=15 width=290>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="7672">
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
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR>
  <tr>
    <td align=right style="COLOR: yellow">Capital
    Amount</TD>
    <td>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" id=CapitalAmount style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 120px"
	tabIndex=7 width=120>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
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
	<PARAM NAME="DisplayFormat" VALUE="##,###,###,##0.00">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="#########0.00">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxValue" VALUE="99999999.99">
	<PARAM NAME="MinValue" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="NegativeColor" VALUE="255">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="Separator" VALUE=",">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ValueVT" VALUE="5">
	<PARAM NAME="Value" VALUE="0">
	<PARAM NAME="MaxValueVT" VALUE="5">
	<PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <td></TD>
    <td></TD></TR>
  <tr>
    <td align=right style="COLOR: yellow">Guarantee
    Amount</TD>
    <td>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" id=GuaranteeAmount style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 120px"
	tabIndex=8 width=120>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
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
	<PARAM NAME="DisplayFormat" VALUE="##,###,###,##0.00">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="#########0.00">
	<PARAM NAME="HighlightText" VALUE="-1">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxValue" VALUE="99999999.99">
	<PARAM NAME="MinValue" VALUE="0">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="NegativeColor" VALUE="255">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="Separator" VALUE=",">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ValueVT" VALUE="5">
	<PARAM NAME="Value" VALUE="0">
	<PARAM NAME="MaxValueVT" VALUE="5">
	<PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <td></TD>
    <td id=td_Msg></TD></TR>
  <tr>
    <td align=right style="COLOR: yellow">Payment
    Amount</TD>
    <td>
      <OBJECT classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" id=PaymentAmount style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 120px"
	tabIndex=9 width=120>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3175">
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
	<PARAM NAME="DisplayFormat" VALUE="##,###,###,##0.00">
	<PARAM NAME="EditMode" VALUE="0">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="ErrorBeep" VALUE="0">
	<PARAM NAME="ForeColor" VALUE="-2147483640">
	<PARAM NAME="Format" VALUE="##########0.00">
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="MarginBottom" VALUE="1">
	<PARAM NAME="MarginLeft" VALUE="1">
	<PARAM NAME="MarginRight" VALUE="1">
	<PARAM NAME="MarginTop" VALUE="1">
	<PARAM NAME="MaxValue" VALUE="99999999999.99">
	<PARAM NAME="MinValue" VALUE="-99999999999.99">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="NegativeColor" VALUE="255">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0">
	<PARAM NAME="ReadOnly" VALUE="0">
	<PARAM NAME="Separator" VALUE=",">
	<PARAM NAME="ShowContextMenu" VALUE="-1">
	<PARAM NAME="ValueVT" VALUE="5">
	<PARAM NAME="Value" VALUE="0">
	<PARAM NAME="MaxValueVT" VALUE="5">
	<PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</TD>
    <td></TD>
    <td></TD></TR></TABLE><input class=button2 disabled id=btn_FindBranch size=60 style="HEIGHT: 25px; LEFT: 583px; POSITION: absolute; TOP: 316px; WIDTH: 120px; Z-INDEX: 113" type=button value="Find Branch">

<table border=0 cellPadding=1 cellSpacing=1 class=Table2 id=tbl_DisbursementType
style="LEFT: 21px; POSITION: absolute; TOP: 227px; WIDTH: 700px; Z-INDEX: 114"
width=700>

  <tr>
    <td align=right>Payment Types&nbsp;</TD>
    <td>
      <OBJECT classid="clsid:F0D2F21C-CCB0-11D0-A316-00AA00688B10" height=21 id=DataCombo_PaymentType
	style="HEIGHT: 21px; LEFT: 1px; TOP: 1px; WIDTH: 209px" tabIndex=3 width=209>
	<PARAM NAME="_ExtentX" VALUE="5530">
	<PARAM NAME="_ExtentY" VALUE="741">
	<PARAM NAME="_Version" VALUE="393216">
	<PARAM NAME="IntegralHeight" VALUE="-1">
	<PARAM NAME="Enabled" VALUE="0">
	<PARAM NAME="Locked" VALUE="0">
	<PARAM NAME="MatchEntry" VALUE="0">
	<PARAM NAME="SmoothScroll" VALUE="0">
	<PARAM NAME="Appearance" VALUE="1">
	<PARAM NAME="MousePointer" VALUE="0">
	<PARAM NAME="Style" VALUE="2">
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
	<PARAM NAME="DataMember" VALUE=""></OBJECT>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </TD>
    <td align=right style="COLOR: blue">Loan Purpose </TD>
    <td>
      <OBJECT classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" height=21 id=LoanPurpose
	name=LoanPurpose style="FONT-SIZE: smaller; HEIGHT: 21px; LEFT: 1px; TOP: 1px; VISIBILITY: visible; WIDTH: 175px"
	width=175>
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="4630">
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
	<PARAM NAME="HighlightText" VALUE="0">
	<PARAM NAME="IMEMode" VALUE="0">
	<PARAM NAME="IMEStatus" VALUE="0">
	<PARAM NAME="DropWndWidth" VALUE="0">
	<PARAM NAME="DropWndHeight" VALUE="0">
	<PARAM NAME="ScrollBarMode" VALUE="0">
	<PARAM NAME="MoveOnLRKey" VALUE="0">
	<PARAM NAME="OLEDragMode" VALUE="0">
	<PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</TD></TR></TABLE>
<OBJECT classid=clsid:0BA686B9-F7D3-101A-993E-0000C0EF6F5E
codeBase=OCX/Threed32.ocx id=pnl_Msg
style="FONT-FAMILY: MS Sans Serif; FONT-SIZE: 10px; HEIGHT: 50px; LEFT: 338px; POSITION: absolute; TOP: 482px; VISIBILITY: hidden; WIDTH: 181px; Z-INDEX: 115"
VIEWASTEXT><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="4789"><PARAM NAME="_ExtentY" VALUE="1323"><PARAM NAME="_StockProps" VALUE="15"><PARAM NAME="Caption" VALUE="Transaction Financial Values cannot be changed as they have been Disbursed"><PARAM NAME="ForeColor" VALUE="65535"><PARAM NAME="BackColor" VALUE="255"><PARAM NAME="BevelWidth" VALUE="1"><PARAM NAME="BorderWidth" VALUE="1"><PARAM NAME="BevelOuter" VALUE="1"><PARAM NAME="BevelInner" VALUE="0"><PARAM NAME="RoundedCorners" VALUE="-1"><PARAM NAME="Outline" VALUE="0"><PARAM NAME="FloodType" VALUE="0"><PARAM NAME="FloodColor" VALUE="16711680"><PARAM NAME="FloodPercent" VALUE="0"><PARAM NAME="FloodShowPct" VALUE="-1"><PARAM NAME="ShadowColor" VALUE="0"><PARAM NAME="Font3D" VALUE="0"><PARAM NAME="Alignment" VALUE="7"><PARAM NAME="Autosize" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"></OBJECT>

</body>
</html>