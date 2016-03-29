<%
' changes 26 Jan 2005
'disabled stamp and transfer duties

  sDatabase =Session("SQLDatabase")
  sUid = Session("UserID")
  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_GenerateTxns = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Generate Txns",Session("UserName"))
  i_SaveTxnInfo = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Save Transaction Info",Session("UserName"))
  i_CATSCreate = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"CATS Create",Session("UserName"))

%>
<html>
<head>
    <!--#include virtual="/SAHL-MLSS/miscutils.inc"-->
    <!--#include virtual="/SAHL-MLSS/database.inc"-->
    <!--#include file="server.asp"-->
    <object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" viewastext id="Microsoft_Licensed_Class_Manager_1_0">
        <param name="LPKPath" value="APEX.lpk">
    </object>
    <meta name="VI60_DefaultClientScript" content="VBScript">
    <meta name="GENERATOR" content="Microsoft Visual Studio 6.0">
    <script id="clientEventHandlersVBS" language="vbscript">
<!--
Dim s_ReturnPage
Dim i_LoanNumber
dim b_AllDataLoaded

Dim sUserName
dim rs_WhichDB
dim sDatabaseName
Dim sSQL
Dim RuleOverride
Dim fQuickCashAmount
Dim NCAApplies
Dim QuickCashFees
Dim NoQC

if rs_open1 <> true then

   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"

	if sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   window.close
    end if

	x = "=<%= Session("SQLDatabase")%>"

    if x <> "=" then
		set conn = createobject("ADODB.Connection")
		set rs_Loan  = createobject("ADODB.Recordset")

		set rstemp = createobject("ADODB.Recordset")
		set rstemp1 = createobject("ADODB.Recordset")
		set rs_WhichDB = createobject("ADODB.Recordset")

		sDSN = GetConnectionString("[DisbursementTxns.asp]")
		conn.Open sDSN
		rs_open1 = false
	end if

end if

Sub GetDisbursementData()

   	if rs_open1 = true  then
       		rs_Loan.Close
		rs_open1 = false
	end if

    sSQL = "t_GetDisbursementDetails " & i_LoanNumber
    rs_Loan.CursorLocation = 3
	rs_Loan.Open sSQL,conn,adOpenStatic

    tbl_LoanDetails.focus
	window.LoanNumber.Text = rs_Loan.Fields("LoanNumber").Value
	window.ClientNames.Text = rs_Loan.Fields("ClientNames").Value
	window.RegistrationAmount.Value = rs_Loan.Fields("RegMailBondAmount").Value
	window.LoanAgreementAmount.Value = rs_Loan.Fields("LoanAgreementAmount").Value
    window.DeedsOffice.Text = rs_Loan.Fields("DeedsOfficeDescription").Value
    window.AttorneyName.Text = rs_Loan.Fields("AttorneyName").Value
    window.LoanPurpose.Text = rs_Loan.Fields("PurposeDescription").Value
    window.RegistrationDate.Value = Date()
    window.RegistrationDate.Enabled= false

	window.TxnAmount8.Enabled = false

    window.tbl_Txns.focus
    window.TxnAmount10.Value = rs_Loan.Fields("AdminFee").Value
    window.TxnAmount11.Value = rs_Loan.Fields("ValuationFee").Value
    window.TxnAmount12.Value = rs_Loan.Fields("QuickCashInterest").Value

    if rs_Loan.RecordCount > 0 then
   		if trim(lcase(rs_Loan.Fields("PurposeDescription").Value)) = "switch loan"  then
			window.TxnAmount1.Enabled = false
			window.TxnAmount2.Enabled = false
		end if
	end if

    tbl_SPV.focus
    window.SPVDescription.Text = rs_Loan.Fields("SPVName").Value
    window.LoanSPVDescription.Text = rs_Loan.Fields("LoanSPVName").Value
    tbl_Prospect.focus
    window.ProspectNumber.Value = rs_Loan.Fields("ProspectNumber").Value
	rs_open1 = true

end sub

Sub GetQuickCashDetails

	sSQL = "GetQuickCashDetails " & i_LoanNumber

	rstemp.CursorLocation = 3
	rstemp.Open sSQL,conn,adOpenStatic

	if rstemp.RecordCount > 0 then
		fQuickCashAmount = rstemp.Fields("DisbursementAmount").Value
		NCAApplies = rstemp.Fields("NCAInd").Value
		QuickCashFees =  rstemp.Fields("QuickCashFees").Value
		NoQC = rstemp.Fields("NoQC").Value
	else
		fQuickCashAmount = 0
		QuickCashFees = 0
		NCAApplies = "No"
		NoQC = "No"
	end if

	rstemp.close

End Sub

Sub GetTxnCodeData

    tbl_Txns.focus

    if trim(rs_Loan.Fields("PurposeDescription").Value) <> "Further Loan" then 'further loan
        sSQL = "t_GetDisbursementTxnInfo1"
    else
        sSQL = "t_GetDisbursementFurtherLoanTxnInfo1"
    end if

    rstemp.CursorLocation = 3
	rstemp.Open sSQL,conn,adOpenStatic

	window.TxnCode1.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc1.Text = rstemp.Fields("TransactionTypeLoanDescription").Value

	rstemp.MoveNext

	if trim(rs_Loan.Fields("PurposeDescription").Value) <> "Further Loan" then 'further loan

		window.TxnCode2.Value = rstemp.Fields("TransactionTypeNumber").Value
		window.TxnDesc2.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
		rstemp.MoveNext

		window.TxnCode3.Value = rstemp.Fields("TransactionTypeNumber").Value
		window.TxnDesc3.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
		rstemp.MoveNext

	else

		window.TxnCode3.Value = rstemp.Fields("TransactionTypeNumber").Value
		window.TxnDesc3.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
		rstemp.MoveNext

		window.TxnCode2.Value = rstemp.Fields("TransactionTypeNumber").Value
		window.TxnDesc2.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
		rstemp.MoveNext

	end if

	window.TxnCode5.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc5.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
	rstemp.MoveNext

	window.TxnCode4.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc4.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
	rstemp.MoveNext

	window.TxnCode12.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc12.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
	rstemp.MoveNext

	window.TxnCode6.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc6.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
	rstemp.MoveNext

	window.TxnCode7.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc7.Text = rstemp.Fields("TransactionTypeLoanDescription").Value

	rstemp.Close

	sSQL = "t_GetDisbursementTxnInfo2"
    rstemp.CursorLocation = 3
	rstemp.Open sSQL,conn,adOpenStatic

	window.TxnCode8.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc8.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
	rstemp.MoveNext

	window.TxnCode9.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc9.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
	rstemp.Close

	sSQL = "t_GetDisbursementTxnInfo3"
    	rstemp.CursorLocation = 3
	rstemp.Open sSQL,conn,adOpenStatic

	window.TxnCode10.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc10.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
	rstemp.MoveNext

	window.TxnCode11.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc11.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
	rstemp.MoveNext

	window.TxnCode14.Value = rstemp.Fields("TransactionTypeNumber").Value
	window.TxnDesc14.Text = rstemp.Fields("TransactionTypeLoanDescription").Value
	rstemp.MoveNext

	rstemp.close

End Sub

Sub GetFinancialData

    sSQL = "r_GetDisbursementFinancialDetails " & i_LoanNumber

    rstemp1.CursorLocation = 3
	rstemp1.Open sSQL,conn,adOpenDynamic

	window.tbl_Txns.focus

	window.TxnAmount1.Value = rstemp1.Fields("RegMailGuaranteeAmount").Value

	window.TxnAmount2.Value = rstemp1.Fields("RegMailCashRequired").Value
	window.TxnAmount3.Value = rstemp1.Fields("RegMailCashDeposit").Value

	window.TxnAmount4.Value = rstemp1.Fields("QCAdminFee").Value
	window.TxnAmount5.Value = rstemp1.Fields("RegMailCancelFee").Value

	window.TxnAmount6.Value = rstemp1.Fields("RegMailConveyancingFee").Value

	window.TxnAmount7.Value = rstemp1.Fields("RegMailVAT").Value
	window.TxnAmount8.Value = rstemp1.Fields("RegMailStampDuty").Value
	window.TxnAmount9.Value = rstemp1.Fields("RegMailDeedsFee").Value

	window.TxnAmount12.Value = rstemp1.Fields("QuickCashInterest").Value

    if window.TxnAmount10.Value <=0.00 then
		window.TxnAmount10.Value = rstemp1.Fields("ProspectAdminFee").Value
	end if

	if window.TxnAmount11.Value <=0.00 then
		window.TxnAmount11.Value = rstemp1.Fields("ProspectValuationFee").Value
	end if

    window.ProspectLoanToleranceAmount.Value = rstemp1.Fields("ProspectLoanToleranceAmt").Value

    tbl_Msg.style.visibility = "hidden"

end sub

Sub GetInitiationFeeData
    HasDiscountedInitiationFeeOfferAttribute = 0
    DiscountedInitiationFeeDetermination = 0

    sSQL = "select OA.OfferAttributeKey FROM [2AM].[dbo].Offer O (nolock) join [2AM].[dbo].[OfferAttribute] OA (nolock) on OA.OfferKey = O.OfferKey	and OA.OfferAttributeTypeKey = 29 WHERE O.AccountKey = " & CStr(i_LoanNumber)

    set rstemp3 = createobject("ADODB.Recordset")

    rstemp3.CursorLocation = 3
    rstemp3.Open sSQL, conn, adOpenDynamic

    if rstemp3.RecordCount > 0 then
	    HasDiscountedInitiationFeeOfferAttribute = 1
    End if

    rstemp3.close

    sSQL = "select O.OfferKey FROM [2AM].[dbo].Offer O (nolock) cross apply [2AM].[dbo].[GetDiscountedInitiationFeeOfferAttribute](O.OfferKey) d WHERE d.OfferAttributeTypeKey = 29 AND d.[Remove] = 0 and O.AccountKey = " & CStr(i_LoanNumber)

    rstemp3.Open sSQL, conn, adOpenDynamic

    if rstemp3.RecordCount > 0 then
	    DiscountedInitiationFeeDetermination = 1
    End if

    rstemp3.close

    window.hDiscountedInitiationFeeDetermination.Value = DiscountedInitiationFeeDetermination

    sSQL = "if (isnull((select OfferStartDate from [2am].dbo.offer o where o.AccountKey = " & CStr(i_LoanNumber) & " and o.OfferStatusKey = 1 and o.OfferTypeKey in (4, 6, 7, 8)), getdate()) " & _
			" > (select ControlText from [Control] where ControlDescription = 'Discounted Initiation Fee Date Switch') ) " & _
			" begin " & _
            "	select ControlNumeric AS InitiationFeeDiscount  " & _
            "	from [2am].dbo.Control (nolock)  " & _
            "	where ControlDescription = 'Returning Main Applicant Initiation Fee Discount 2' " & _
			" end " & _
            "else " & _
			" begin " & _
            "	select ControlNumeric AS InitiationFeeDiscount  " & _
            "	from [2am].dbo.Control (nolock)  " & _
            "	where ControlDescription = 'Returning Main Applicant Initiation Fee Discount' " & _
			" end "

    rstemp3.Open sSQL, conn, adOpenDynamic

    if rstemp3.RecordCount > 0 then
        window.hInitiationFeeDiscount.Value = rstemp3.Fields("InitiationFeeDiscount").Value
    End if

    rstemp3.close

    if (HasDiscountedInitiationFeeOfferAttribute = 0 and DiscountedInitiationFeeDetermination = 0) then
        window.hAllowOverrideDiscount.Value = "no"
        window.trDiscount.style.display = "none"
        window.trDiscounted.style.display = "none"

        window.TxnAmount13.Value = 0
        window.hTxnAmount13.Value = 0
        window.TxnAmount14.Value = 0
        window.hTxnAmount14.Value = 0
    else
        sSQL = "t_GetInitiationFeeDetails " & i_LoanNumber

        rstemp3.CursorLocation = 3
        rstemp3.Open sSQL, conn, adOpenDynamic

	    if rstemp3.RecordCount > 0 then
            window.TxnAmount10.Value = rstemp3.Fields("InitiationFeeWithoutDiscount").Value
            if not IsNull(rstemp3.Fields("RegMailAdminFeeOveride").Value) then
                window.cbOverrideDiscount.checked = rstemp3.Fields("RegMailAdminFeeOveride").Value
                if rstemp3.Fields("RegMailAdminFeeOveride").Value = 0 then
                    window.TxnAmount13.Value = rstemp3.Fields("OfferExpenseInitiationFee").Value
                    window.TxnAmount14.Value = rstemp3.Fields("OfferExpenseInitiationFeeDiscountAmount").Value
                else
                    window.TxnAmount13.Value = rstemp3.Fields("RegMailAdminFee").Value - rstemp3.Fields("RegMailAdminFeeDiscountAmount").Value
                    window.TxnAmount14.Value = rstemp3.Fields("RegMailAdminFeeDiscountAmount").Value
                end if
            else
                window.TxnAmount13.Value = rstemp3.Fields("OfferExpenseInitiationFee").Value
                window.TxnAmount14.Value = rstemp3.Fields("OfferExpenseInitiationFeeDiscountAmount").Value
            end if

            if (HasDiscountedInitiationFeeOfferAttribute = 0 and DiscountedInitiationFeeDetermination = 1) then
                window.hAllowOverrideDiscount.Value = "yes"
                window.lblOverrideDiscount.innerHTML = "<b>Apply Discount</b>"
                window.hTxnAmount13.Value = rstemp3.Fields("InitiationFeeWithoutDiscount").Value
                window.hTxnAmount14.Value = 0
            else
                if HasDiscountedInitiationFeeOfferAttribute = 1 then
                    window.hAllowOverrideDiscount.Value = "yes"
                    window.lblOverrideDiscount.innerHTML = "<b>Remove Discount</b>"

                    InitiationFee = rstemp3.Fields("InitiationFeeWithoutDiscount").Value
                    InitiationFeeDiscount = window.hInitiationFeeDiscount.Value

                    window.hTxnAmount13.Value = InitiationFee - (InitiationFee * InitiationFeeDiscount)
                    window.hTxnAmount14.Value = InitiationFee * InitiationFeeDiscount
                end if
            end if
	    end if
        rstemp3.close
    end if

end sub

Sub window_onload

	b_AllDataLoaded = false

	SetAccessLightsServer

	window.RegistrationDate.DropDown.Visible = 1
	window.RegistrationDate.Spin.Visible = 1

	i_Nbr = "<%=Request.QueryString("Number")%>"

	s_Source = "<%=Request.QueryString("Source")%>"
	s_ReturnPage = s_Source
	i_LoanNumber = i_Nbr
	s_Purpose = "<%=Request.QueryString("purpose")%>"

    sSQL = "Select sahldb.dbo.SwitchDB(" & CStr(i_LoanNumber) & ") as DBName"
	rs_WhichDB.CursorLocation = 3
	rs_WhichDB.Open sSQL ,conn,adOpenStatic

	sDatabaseName = "SAHLDB"

	If Not rs_WhichDB.EOF Then
		sDatabaseName = rs_WhichDB.Fields("DBName").Value
		rs_WhichDB.Close
	Else
		msgbox "Error resolving database name"
	End If

	GetDisbursementData
	GetTxnCodeData
	GetFinancialData
	GetQuickCashDetails
    GetInitiationFeeData

	window.TotalTxnAmount.Value = 0.00

	CalculateTotalDisbursment

	tbl_Msg.style.visibility = "hidden"
	pnl_Msg.style.visibility = "hidden"
    pnl_Msg.Caption = ""

	b_AllDataLoaded = true

End Sub

Sub btn_Exit_onclick
	s_ReturnPage = "Disbursements.asp"
	window.parent.frames("RegistrationPanel").location.href = s_ReturnPage & "?Number= " & CStr(i_LoanNumber ) & "&RepliesReceived=<%=Request.QueryString("RepliesReceived")%>"  & "&Lodged=<%=Request.QueryString("Lodged")%>" & "&UpForFees=<%=Request.QueryString("UpForFees")%>"

End Sub

Sub SetAccessLightsServer

    sUserName = "<%= Session("UserID")%>"

	sRes1 = "<%=i_GenerateTxns%>"
	if sRes1 = "Allowed" then
          window.pic_GenerateTxns.src = "images/MLSAllowed.bmp"
          window.pic_GenerateTxns.title = "1"
	end if

	sRes1 = "<%=i_SaveTxnInfo%>"
	if sRes1 = "Allowed" then
          window.pic_SavTxnInfo.src = "images/MLSAllowed.bmp"
          window.pic_SavTxnInfo.title = "1"
	end if

	sRes1 = "<%=i_CATSCreate%>"
	if sRes1 = "Allowed" then
          window.pic_CATSCreate.src = "images/MLSAllowed.bmp"
          window.pic_CATSCreate.title = "1"
	end if

end Sub

Sub TxnAmount1_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount2_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount3_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount4_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount5_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount6_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount7_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount8_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount9_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount10_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount11_Change
	CalculateTotalDisbursment
End Sub

Sub TxnAmount14_Change
	CalculateTotalDisbursment
End Sub

Function CalculateTotalDisbursment
		window.TotalTxnAmount.Value = window.TxnAmount1.value + window.TxnAmount2.value - window.TxnAmount3.value  + window.TxnAmount5.value + window.TxnAmount6.value + window.TxnAmount7.value + window.TxnAmount8.value + window.TxnAmount9.value + (window.TxnAmount10.value - window.TxnAmount14.value) + window.TxnAmount11.value
End Function

Function GetSAHLStaffDetailRecords

	GetSAHLStaffDetailRecords = 0

	set rstemp2 = createobject("ADODB.Recordset")

	sSQL = "SELECT DetailNumber FROM DETAIL (nolock) WHERE LoanNumber = " & CStr(i_LoanNumber) & " AND DetailTypeNumber IN ( 237 )"

	rstemp2.CursorLocation = 3
	rstemp2.Open sSQL,conn,adOpenDynamic

	if rstemp2.RecordCount > 0 then
		GetSAHLStaffDetailRecords = 1
	End if

End Function

Sub btn_GenerateTxns_onclick

    if window.pic_GenerateTxns.title <> "1" then
	      window.status = "Access denied to " & window.btn_GenerateTxns.value
          exit sub
    end if

    if window.hAllowOverrideDiscount.Value = "yes" then
        if window.lblOverrideDiscount.innerHTML = "<B>Remove Discount</B>" then
            if window.cbOverrideDiscount.checked = false and window.hDiscountedInitiationFeeDetermination.Value = "0" then
		        i_Resp = msgbox ("This loan no longer qualifies for the discounted initiation fee. Do you wish to continue without removing the discount?", vbYesNo)
		        If i_Resp= 7 then
		            exit sub
		        end if
            end if
        else
            if window.lblOverrideDiscount.innerHTML = "<B>Apply Discount</B>" then
                if window.cbOverrideDiscount.checked = false and window.hDiscountedInitiationFeeDetermination.Value = "1" then
		            i_Resp = msgbox ("This loan qualifies for a discounted initiation fee. Do you wish to continue without applying the discount?", vbYesNo)
		            If i_Resp= 7 then
		                exit sub
		            end if
                end if
            end if
        end if
    end if

    if	NCAApplies = "Yes" then ' Rules only apply to loans taken on after 1 June
	    If trim(rs_Loan.Fields("PurposeDescription").Value) <> "Further Loan" then
		    set rstFees = createobject("ADODB.Recordset")
		    sSQL = "SELECT FeeAdmin FROM FEES (nolock) where FeeRange >=" & window.TotalTxnAmount.value & " order by FeeRange ASC "
		    rstFees.CursorLocation = 3
		    rstFees.Open sSQL,conn,adOpenDynamic
		    Adminfee = rstFees.Fields("FeeAdmin").Value
	    else
		    AdminFee = 0 ' No Admin Fee for Further Loans After 1 June
	    End if

	    If trim(rs_Loan.Fields("PurposeDescription").Value) <> "Further Loan" then
		    if cdbl(QuickCashFees) <> cdbl(window.TxnAmount4.value) then
			    Msgbox "QuickCash fees on QuickCash Amount of R" & fQuickCashAmount & " must be R" & QuickCashFees & ". System will amend accordingly. " & vbCRLF & vbCRLF & "Please ensure that Initiation Fee includes this Quick Cash Fee."
			    window.TxnAmount4.value = QuickCashFees
		    end if
	    else
		    if cdbl(window.TxnAmount4.value) > 0 then
			    Msgbox "No Quick Cash Processing Fee for Further Loans - System will amend to zero."
			    window.TxnAmount4.value = 0
		    end if
	    end if

    else
	    if 	(NCAApplies = "No" and NoQC = "Yes") then
		    if cdbl(window.TxnAmount4.value) > 0 then
			    Msgbox "No Quick Cash Processing Fees for Clients on New Fee Struture. System will amend to zero"
			    window.TxnAmount4.value = 0
		    end if

		    if cdbl(window.TxnAmount12.value) > 0 then
			    Msgbox "Quick Cash Interest not applicable to Clients on New Fee Struture. System will set to zero."
			    window.TxnAmount12.value = 0
		    end if

	    end if
    end if

    If NoQC <> "Yes" then
	    if TxnAmount12.Text = 0 Then
		    i_Resp = msgbox ("Are you sure you don't want to add Quick Cash Interest?",vbYesNo)
		    If i_Resp= 7 then
		    exit sub
		    End if
	    End if
    End if

	if window.LoanAgreementAmount.Value <= 0  then

		pnl_Msg.Caption = ""
	    tbl_Msg.style.visibility = "hidden"
		pnl_Msg.style.visibility = "hidden"

	    MsgBox "The Loan Agreement Amount cannot be less than or equal to zero...!!",,"Generate Disbursement Transactions"
	    window.LoanAgreementAmount.focus
	    exit sub

	end if

	if window.RegistrationAmount.Value <= 0  then

		pnl_Msg.Caption = ""
		tbl_Msg.style.visibility = "hidden"
		pnl_Msg.style.visibility = "hidden"

	    MsgBox "The Registration Amount cannot be less than or equal to zero...!!",,"Generate Disbursement Transactions"
	    window.RegistrationAmount.focus
	    exit sub

	end if

	if window.TotalTxnAmount.Value > window.ProspectLoanToleranceAmount.Value then
	   i_Resp = MsgBox("The Amount being disbursed exceeds the preset tolerance from the Client's Prospect Record Loan amount...!!" & chr(13) & chr(10) & "Are you sure you want to disburse these transactions..??"  ,4,Disbursement)
		if i_Resp= 7 then
			pnl_Msg.Caption = ""
			tbl_Msg.style.visibility = "hidden"
			pnl_Msg.style.visibility = "hidden"
		   exit sub
		end if
	end if

     i_Resp = MsgBox("Are you sure the Registration Date : " & window.RegistrationDate.DisplayText & " is correct"  ,4,Disbursement)
		if i_Resp= 7 then
			pnl_Msg.Caption = ""
			tbl_Msg.style.visibility = "hidden"
			pnl_Msg.style.visibility = "hidden"
		   exit sub
		end if

	If Cint(rs_Loan.Fields("SPVNumber").Value) <> Cint(rs_Loan.Fields("LoanSPVNumber").Value) then
	    i_Resp = MsgBox("This Loan will be transfered from " & rs_Loan.Fields("LoanSPVName").Value & " to " & rs_Loan.Fields("SPVName").Value & chr(13) & chr(10) & "Are you sure you want to disburse these transactions..??"  ,4,Disbursement)
		if i_Resp= 7 then
			pnl_Msg.Caption = ""
			tbl_Msg.style.visibility = "hidden"
			pnl_Msg.style.visibility = "hidden"
		   exit sub
		end if
	End If

	i_res = SaveFinancials()

	if i_res <> 0 then
		MsgBox "The Financial Info could not be saved to the Regmail Record...!!",,"Generate Disbursement Transactions"
		exit sub
	end if

	' Remove detail Type 456,457 if they exist.

	RemoveFurtherLoanReadvanceDetailTypes

	pnl_Msg.Caption = "Please Wait...Generating Txns...."
    tbl_Msg.style.visibility = "visible"
    pnl_Msg.style.visibility = "visible"

	if GenerateDisbursementTxns() = 0 then

		If sDatabaseName = "SAHLDB" Then
			window.parent.frames("RegistrationPanel").location.href = "LoanTransactions.asp?Number= " & CStr(i_LoanNumber) & "&Source=Disbursements.asp&Panel=RegistrationPanel"
		Else
			location.href = "<%=MANAGE_LOAN_TRANSACTION_PLUGIN%>" & CStr(i_LoanNumber)
		End If

	else
	   pnl_Msg.Caption = "Check Registration Date...!!!"
	   exit sub
	end if

	pnl_Msg.Caption = ""
    tbl_Msg.style.visibility = "hidden"
    pnl_Msg.style.visibility = "hidden"

End Sub

Sub RemoveFurtherLoanReadvanceDetailTypes
	Dim sSQl

	set connection = createobject("ADODB.Connection")

	sSQL = "Delete from Detail where LoanNumber = " & i_LoanNumber & " and detailtypenumber = 457"
	connection.Open sDSN
	connection.Execute sSQL

End Sub

Function GenerateDisbursementTxns()
    Dim i_res

	GenerateDisbursementTxns = -1

    document.body.style.cursor = "hand"

    i_res = 0
    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")
    set rs_temp1 = createobject("ADODB.Recordset")
    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS [DisbursementTxns.asp 1];uid=<%= Session("UserID")%>"

	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc

    if trim(rs_Loan.Fields("PurposeDescription").Value) = "Further Loan" then
        sSQL = "f_GenerateFurtherLoanTxns"
    else
	    sSQL = "f_GenerateDisbursementTxns"
	end if

	com.CommandText = sSQL

	set prm = com.CreateParameter ( "LoanNumber",19,1,, i_LoanNumber )
	com.Parameters.Append prm

	s = Date()

    s_date = Mid(s,4,2) & "/" & Mid(s,1,2) & "/" & Mid(s,7,4)

	set prm = com.CreateParameter ( "FromDate",129,1,10,s_date)
	com.Parameters.Append prm

	s_date = Mid(window.RegistrationDate.Text, 4, 2) & "/" & Mid(window.RegistrationDate.Text, 1, 2) & "/" & Mid(window.RegistrationDate.Text, 7, 4)

	set prm = com.CreateParameter ( "ToDate",129,1,10,s_date)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "UserID",200,1,25,sUserName)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TxnCode1",19,1,,window.TxnCode1.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt1",5,1,,window.TxnAmount1.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCode2",19,1,,window.TxnCode2.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt2",5,1,,window.TxnAmount2.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCode3",19,1,,window.TxnCode3.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt3",5,1,,window.TxnAmount3.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCode4",19,1,,window.TxnCode4.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt4",5,1,,window.TxnAmount4.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TxnCode5",19,1,,window.TxnCode5.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt5",5,1,,window.TxnAmount5.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TxnCode6",19,1,,window.TxnCode6.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt6",5,1,,window.TxnAmount6.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TxnCode7",19,1,,window.TxnCode7.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt7",5,1,,window.TxnAmount7.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TxnCode8",19,1,,window.TxnCode8.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt8",5,1,,window.TxnAmount8.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TxnCode9",19,1,,window.TxnCode9.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt9",5,1,,window.TxnAmount9.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TxnCode10",19,1,,window.TxnCode10.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt10",5,1,,window.TxnAmount10.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TxnCode11",19,1,,window.TxnCode11.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt11",5,1,,window.TxnAmount11.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "TxnCode12",19,1,,window.TxnCode12.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "TxnCodeAmt12",5,1,,window.TxnAmount12.Value)
	com.Parameters.Append prm

    if trim(rs_Loan.Fields("PurposeDescription").Value) <> "Further Loan" then
	    set prm = com.CreateParameter ( "TxnCode14",19,1,,window.TxnCode14.Value)
	    com.Parameters.Append prm

        set prm = com.CreateParameter ( "TxnCodeAmt14",5,1,,window.TxnAmount14.Value)
	    com.Parameters.Append prm
	end if

	set prm = com.CreateParameter ( "RegistrationAmt",5,1,,window.RegistrationAmount.value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "DeedsOfficeNbr",19,1,,rs_Loan.Fields("DeedsOfficeNumber").Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "AttorneyNbr",19,1,,rs_Loan.Fields("AttorneyNumber").Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "LoanAgreementAmt",5,1,,window.LoanAgreementAmount.value)
	com.Parameters.Append prm

	if trim(rs_Loan.Fields("PurposeDescription").Value) = "Further Loan" then
		set prm = com.CreateParameter ( "ProspectNumber",19,1,,rs_Loan.Fields("ProspectNumber").Value)
		com.Parameters.Append prm
	end if

    com.CommandTimeout= 0

	set rs_temp = com.Execute

    document.body.style.cursor = "default"

    ' if the stored proc is successfull the recordset will be closed
    ' if an error message is returned then there will be an open recordset containing the error message

    If rs_temp.EOF or trim(rs_temp.Fields(0).Value) = "" or trim(rs_temp.Fields(0).Value) = "0" Then ' success
        msgbox "Disbursement Transactions have been successfully generated for : "  & chr(13) & chr(10) & "Client Name : " & window.ClientNames.Text & chr(13) & chr(10) & "Loan Number : " & Cstr(window.LoanNumber)
        GenerateDisbursementTxns = 0
        Exit Function
    else ' error
        MsgBox ( trim(rs_temp.Fields(0).Value) )
        Exit Function
	end if

'    Select Case rs_temp.Fields(0).Value
'	     Case -1111
'	             MsgBox("Total Disbursement cannot be be less than or equal to zero..!!" )
'                 Exit Function
'	     Case -1112
'	             MsgBox("Market Rate cannot be be less than or equal to zero..!!" )
'                 Exit Function
'	     Case -1113
'	             MsgBox("Link Rate cannot be be less than or equal to zero..!!" )
'                 Exit Function
'	     Case -1116
'	             MsgBox("Error while updating Loan Record..!!" )
'                 Exit Function
'	      Case 1112
'	             MsgBox("Error while generating Loan Transaction..!!" )
'                 Exit Function
'          Case 1113
'	             MsgBox("Error while generating Client Cash Required Transaction..!!" )
'                 Exit Function
'          Case 1114
'	             MsgBox("Error while generating Cash Deposit Transaction..!!" )
'                 Exit Function
'          Case 1115
'	             MsgBox("Error while generating Transfer Duty Transaction..!!" )
'                 Exit Function
'          Case 1116
'	             MsgBox("Error while generating Cancellation Fee Transaction..!!" )
'                 Exit Function
'          Case 1117
'	             MsgBox("Error while generating Conveyancing Fee Transaction..!!" )
'                 Exit Function
'          Case 1118
'	             MsgBox("Error while generating VAT Transaction..!!" )
'                 Exit Function
'          Case 1119
'	             MsgBox("Error while generating Stamp Duty Transaction..!!" )
'                 Exit Function
'          Case 11110
'	             MsgBox("Error while generating Deeds Fee Transaction..!!" )
'                 Exit Function
'          Case 11111
'	             MsgBox("Error while generating Initiation Fee Transaction..!!" )
'                 Exit Function
'          Case 11112
'	             MsgBox("Error while generating Valuation Fee Transaction..!!" )
'                 Exit Function
'          Case -1115
'	             MsgBox("Error while applying the Rate Change..!!..Check Dates" )
'                 Exit Function
'          Case 11191
'	             MsgBox("Error while the updating Bond Record..!!" )
'                 Exit Function
'          Case 11115
'	             MsgBox("Error while the updating the Prospect Client Take On Status...!!" )
'                 Exit Function
'          Case 11117
'	             MsgBox("Error while generating Transaction Type 399 (New Purchase)...!!" )
'                 Exit Function
'          Case 11114
'	             MsgBox("Error while the deleting the Client's Loan Registrations Details...!!" )
'                 Exit Function
'          Case 11120
'	             MsgBox("Error while Set the Final Guarantee Figure Values in the Disbursment Table...!!" )
'                 Exit Function
'          Case 11130
'	             MsgBox("Error while Updating Metro System Tables...!!" )
'                 Exit Function
'          Case -1121
'				MsgBox "A SPV Transfer Transaction has been generated with the Accrued Interest Value recorded as : R " & rs_temp.Fields(0).Value,,"Further Loan Disbursement"
'                Exit Function
'          case 0
'                 msgbox "Disbursement Transactions have been successfully generated for : "  & chr(13) & chr(10) & "Client Name : " & window.ClientNames.Text & chr(13) & chr(10) & "Loan Number : " & Cstr(window.LoanNumber)
'                 GenerateDisbursementTxns = rs_temp.Fields(0).Value
'                 Exit Function
'          case else
'                 MsgBox("An unexpected error was encountered while generating the Disbursment Transactions...!!" & Chr(13) & Chr(10) & Chr(13) & Chr(10) & "Check Registration Date (cannot be less than 1st of the month)" & Chr(13) & Chr(10) & Chr(13) & Chr(10) & "Returned Error Code from f_ProcessLoanTransactions is : " & rs_temp.Fields(0).Value )
'                 Exit Function
'	End Select

    GenerateDisbursementTxns = 0

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

Sub window_onunload
	rs_Loan.close
End Sub

Sub btn_SaveFinancials_onclick

    if window.pic_SavTxnInfo.title <> "1" then
          window.status = "Access denied to " & window.btn_SaveFinancials.value
          exit sub
    end if

    if window.hAllowOverrideDiscount.Value = "yes" then
        if window.lblOverrideDiscount.innerHTML = "<B>Remove Discount</B>" then
            if window.cbOverrideDiscount.checked = false and window.hDiscountedInitiationFeeDetermination.Value = "0" then
		        i_Resp = msgbox ("This loan no longer qualifies for the discounted initiation fee. Do you wish to continue without removing the discount?", vbYesNo)
		        If i_Resp= 7 then
		            exit sub
		        end if
            end if
        else
            if window.lblOverrideDiscount.innerHTML = "<B>Apply Discount</B>" then
                if window.cbOverrideDiscount.checked = false and window.hDiscountedInitiationFeeDetermination.Value = "1" then
		            i_Resp = msgbox ("This loan qualifies for a discounted initiation fee. Do you wish to continue without applying the discount?", vbYesNo)
		            If i_Resp= 7 then
		                exit sub
		            end if
                end if
            end if
        end if
    end if

    if	NCAApplies = "Yes" then ' Rules only apply to loans taken on after 1 June
	    If trim(rs_Loan.Fields("PurposeDescription").Value) <> "Further Loan" then
		    set rstFees = createobject("ADODB.Recordset")
		    sSQL = "SELECT FeeAdmin FROM FEES (nolock) where FeeRange >=" & window.TotalTxnAmount.value & " order by FeeRange ASC "
		    rstFees.CursorLocation = 3
		    rstFees.Open sSQL,conn,adOpenDynamic
		    Adminfee = rstFees.Fields("FeeAdmin").Value
	    else
		    AdminFee = 0 ' No Admin Fee for Further Loans After 1 June
	    End if

	    If trim(rs_Loan.Fields("PurposeDescription").Value) <> "Further Loan" then
		    if cdbl(QuickCashFees) <> cdbl(window.TxnAmount4.value) then
			    Msgbox "QuickCash fees on QuickCash Amount of R" & fQuickCashAmount & " must be R" & QuickCashFees & ". System will amend accordingly. " & vbCRLF & vbCRLF & "Please ensure that Initiation Fee includes this Quick Cash Fee."
			    window.TxnAmount4.value = QuickCashFees
		    end if
	    else
		    if cdbl(window.TxnAmount4.value) > 0 then
			    Msgbox "No Quick Cash Processing Fee for Further Loans - System will amend to zero."
			    window.TxnAmount4.value = 0
		    end if
	    end if

    else
	    if 	(NCAApplies = "No" and NoQC = "Yes") then
		    if cdbl(window.TxnAmount4.value) > 0 then
			    Msgbox "No Quick Cash Processing Fees for Clients on New Fee Struture. System will amend to zero."
			    window.TxnAmount4.value = 0
		    end if

		    if cdbl(window.TxnAmount12.value) > 0 then
			    Msgbox "Quick Cash Interest not applicable to Clients on New Fee Struture. System will set to zero."
			    window.TxnAmount12.value = 0
		    end if
	    end if
    end if

    If NoQC <> "Yes" then
	    if TxnAmount12.Text = 0 Then
		    i_Resp = msgbox ("Are you sure you don't want to add Quick Cash Interest?",vbYesNo)
		    If i_Resp= 7 then
		    exit sub
		    End if
	    End if
    End if

	i_res = SaveFinancials()

    Select Case i_res
	    Case -1
	            MsgBox("Could not update Regmail financial Info.....Updates have been rolled back...!!" )

                Exit Sub
	    Case -2
	            MsgBox("Could not update Detail DetailType field .....Updates have been rolled back...!!" )

                Exit Sub
	    Case -3
	            MsgBox("Could not update Regmail DetailType field.....Updates have been rolled back...!!" )

                Exit Sub
		case 0
                msgbox "Transaction Financial has been saved for : "  & chr(13) & chr(10) & "Client Name : " & window.ClientNames.Text & chr(13) & chr(10) & "Loan Number : " & Cstr(window.LoanNumber)

                Exit Sub
        case else
                MsgBox("An unexpected error was encountered while saving Financial Info to Regmail...!!" & Chr(13) & Chr(10) & Chr(13) & Chr(10) & "" & Chr(13) & Chr(10) & Chr(13) & Chr(10) & "Returned Error Code from t_UpdateRegmailFinancials is : " & rs_temp.Fields(0).Value )

                Exit Sub

	End Select

End Sub

Function SaveFinancials

	Dim i_res

	SaveFinancials = -88

    pnl_Msg.Caption = "Please Wait...Saving Txn Info...."
    tbl_Msg.style.visibility = "visible"
    pnl_Msg.style.visibility = "visible"

    document.body.style.cursor = "hand"

    i_res = 0
    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")
    set rs_temp1 = createobject("ADODB.Recordset")
    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS [DisbursementTxns.asp 2];uid=<%= Session("UserID")%>"

	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc

    sSQL = "t_UpdateRegmailFinancials"

	com.CommandText = sSQL

	set prm = com.CreateParameter ( "LoanNumber",19,1,, i_LoanNumber )
	com.Parameters.Append prm

	s_date = Mid(window.RegistrationDate.Text, 4, 2) & "/" & Mid(window.RegistrationDate.Text, 1, 2) & "/" & Mid(window.RegistrationDate.Text, 7, 4)
	set prm = com.CreateParameter ( "RegistrationDate",129,1,10,s_date)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "RegistrationAmt",5,1,,window.RegistrationAmount.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "LoanAgreementAmt",5,1,,window.LoanAgreementAmount.value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "GuaranteeAmt",5,1,,window.TxnAmount1.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "AmtPaidToClient",5,1,,window.TxnAmount2.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "ClientDeposit",5,1,,window.TxnAmount3.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "CancelFee",5,1,,window.TxnAmount5.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "ConveyancingFee",5,1,,window.TxnAmount6.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "VAT",5,1,,window.TxnAmount7.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "StampDuty",5,1,,window.TxnAmount8.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "DeedsFee",5,1,,window.TxnAmount9.Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "QCAdminFee",5,1,,window.TxnAmount4.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "AdminFee",5,1,,window.TxnAmount10.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "ValuationAmt",5,1,,window.TxnAmount11.Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "LoanPurpose",19,1,, rs_Loan.Fields("PurposeNumber").Value)
	com.Parameters.Append prm

    set prm = com.CreateParameter ( "AttorneyNbr",19,1,,rs_Loan.Fields("AttorneyNumber").Value)
	com.Parameters.Append prm

	set prm = com.CreateParameter ( "QuickCashInterest",5,1,,window.TxnAmount12.Value)
	com.Parameters.Append prm

    if window.hAllowOverrideDiscount.Value = "no" then
	    set prm = com.CreateParameter ( "AdminFeeOveride",11,1,,null)
        com.Parameters.Append prm
    else
        if window.hAllowOverrideDiscount.Value = "yes" then
	        set prm = com.CreateParameter ( "AdminFeeOveride",11,1,,window.cbOverrideDiscount.checked)
            com.Parameters.Append prm
        end if
    end if

	set prm = com.CreateParameter ( "AdminFeeDiscountAmount",5,1,,window.TxnAmount14.Value)
	com.Parameters.Append prm

	set rs_temp = com.Execute

    document.body.style.cursor = "default"

    pnl_Msg.Caption = ""
    tbl_Msg.style.visibility = "hidden"
    pnl_Msg.style.visibility = "hidden"

    SaveFinancials = Cint(rs_temp.Fields(0).Value)

End Function

Sub document_onpropertychange

End Sub

Sub document_onreadystatechange
 pnl_Msg.Caption = "Please Wait...Loading Data...."
    tbl_Msg.style.visibility = "visible"
    pnl_Msg.style.visibility = "visible"

End Sub

Sub btn_SaveFinancials_onmousedown

    if window.pic_SavTxnInfo.title <> "1" then
	  window.status = "Access denied to " & window.btn_SaveFinancials.value
      exit sub
	end if

    pnl_Msg.Caption = "Please Wait...Saving Txn Info...."
    tbl_Msg.style.visibility = "visible"
    pnl_Msg.style.visibility = "visible"

End Sub

Sub btn_GenerateTxns_onmousedown

    if window.pic_GenerateTxns.title <> "1" then
	  window.status = "Access denied to " & window.btn_GenerateTxns.value
      exit sub
	end if

pnl_Msg.Caption = "Please Wait...Generating Txns...."
    tbl_Msg.style.visibility = "visible"
    pnl_Msg.style.visibility = "visible"
End Sub

Sub btn_CATSCreate_onclick
	window.parent.frames("RegistrationPanel").location.href = "DisbursementManage.asp?Number= " & CStr(i_LoanNumber) & "&Source=DisbursementTxns.asp&returnpage=DisbursementTxns.asp&purpose=" & CStr( rs_Loan.Fields("PurposeNumber").Value) '& "&RepliesReceived=" & Cint(window.chk_RepliesReceived.checked) & "&Lodged=" & Cint(window.chk_Lodged.checked) & "&Disbursements=" & Cint(window.chk_Disbursements.checked) & "&Readvances=" & Cint(window.chk_ReAdvances.checked) & "&Status=" &  rs_GridProspects.Fields("DetailTypeNumber").Value
End Sub

Sub cbOverrideDiscount_onclick

    if window.lblOverrideDiscount.innerHTML = "<B>Remove Discount</B>" then
        if window.cbOverrideDiscount.checked = true then
            window.TxnAmount13.Value = window.TxnAmount10.Value
            window.TxnAmount14.Value = 0
        else
            if window.cbOverrideDiscount.checked = false then
                window.TxnAmount13.Value = window.hTxnAmount13.Value
                window.TxnAmount14.Value = window.hTxnAmount14.Value
            end if
        end if
    else
        if window.lblOverrideDiscount.innerHTML = "<B>Apply Discount</B>" then
            if window.cbOverrideDiscount.checked = true then
                InitiationFee = window.TxnAmount10.Value
                InitiationFeeDiscount = window.hInitiationFeeDiscount.Value

                window.TxnAmount13.Value = InitiationFee - (InitiationFee * InitiationFeeDiscount)
                window.TxnAmount14.Value = InitiationFee * InitiationFeeDiscount
            else
                if window.cbOverrideDiscount.checked = false then
                    window.TxnAmount13.Value = window.hTxnAmount13.Value
                    window.TxnAmount14.Value = window.hTxnAmount14.Value
                end if
            end if
        end if
    end if

End Sub

-->
    </script>
    <link href="SAHL-MLSS.css" type="text/css" rel="stylesheet">
    <script id="clientEventHandlersJS" language="javascript">
<!--

    -->
    </script>
</head>
<body class="Generic" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <input class="button3" id="btn_Exit" title="Cancel" style="z-index: 103; left: 680px; width: 160px; cursor: hand; position: absolute; top: 492px; height: 55px"
        type="button"
        value="Exit" name="btn_Exit" height="60">
    &nbsp;
    <table class="Table2" id="tbl_LoanDetails" style="z-index: 111; left: 13px; width: 469px; border-bottom: red 0px; position: absolute; top: 5px; height: 212px"
        cellspacing="1"
        cellpadding="1" width="75%" align="center" border="0">
        <tr>
            <td nowrap align="right">Loan Number
            </td>
            <td nowrap>
                <object id="LoanNumber" style="left: 1px; width: 99px; top: 1px; height: 22px" height="22"
                    classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="2619">
                    <param name="_ExtentY" value="582">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
        </tr>
        <tr>
            <td nowrap align="right">Client Name
            </td>
            <td nowrap>
                <object id="ClientNames" style="width: 331px; height: 22px" classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="8758">
                    <param name="_ExtentY" value="582">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td nowrap align="right">Registration Amount
            </td>
            <td nowrap>
                <object id="RegistrationAmount" style="left: 1px; width: 145px; top: 1px; height: 23px"
                    tabindex="1" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3836">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="#######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="5">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
        </tr>
        <tr>
            <td nowrap align="right">Loan Agreement Amount
            </td>
            <td nowrap>
                <object id="LoanAgreementAmount" style="left: 1px; width: 145px; top: 1px; height: 23px"
                    tabindex="2" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3836">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="-1">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="#######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="60882945">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <table id="tbl_Msg" style="border-right: white 0px groove; border-top: white 0px groove; font-size: 10px; z-index: 100; left: 272px; visibility: hidden; border-left: white 0px groove; width: 190px; border-bottom: white 0px groove; font-family: MS Sans Serif; position: absolute; top: 49px; height: 55px"
                    cellspacing="1" cellpadding="1" width="100" align="center"
                    border="0">
                    <tr>
                        <td id="td_Msg" nowrap>
                            <object id="pnl_Msg" style="left: 1px; visibility: visible; width: 181px; top: 0px; height: 50px"
                                codebase="OCX/Threed32.ocx" classid="clsid:0BA686B9-F7D3-101A-993E-0000C0EF6F5E">
                                <param name="_Version" value="65536">
                                <param name="_ExtentX" value="4789">
                                <param name="_ExtentY" value="1323">
                                <param name="_StockProps" value="15">
                                <param name="Caption" value="Please Wait Loading Data...">
                                <param name="ForeColor" value="65535">
                                <param name="BackColor" value="255">
                                <param name="BevelWidth" value="1">
                                <param name="BorderWidth" value="3">
                                <param name="BevelOuter" value="2">
                                <param name="BevelInner" value="0">
                                <param name="RoundedCorners" value="-1">
                                <param name="Outline" value="0">
                                <param name="FloodType" value="0">
                                <param name="FloodColor" value="16711680">
                                <param name="FloodPercent" value="0">
                                <param name="FloodShowPct" value="-1">
                                <param name="ShadowColor" value="0">
                                <param name="Font3D" value="0">
                                <param name="Alignment" value="7">
                                <param name="Autosize" value="0">
                                <param name="MousePointer" value="0">
                                <param name="Enabled" value="-1">
                            </object>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td nowrap align="right">Deeds Office
            </td>
            <td nowrap>
                <object id="DeedsOffice" style="left: 1px; width: 331px; top: 1px; height: 22px"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="8758">
                    <param name="_ExtentY" value="582">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td nowrap align="right">Attorney Name
            </td>
            <td nowrap>
                <object id="AttorneyName" style="left: 1px; width: 331px; top: 1px; height: 22px"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="8758">
                    <param name="_ExtentY" value="582">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td nowrap align="right">Loan Purpose
            </td>
            <td nowrap>
                <object id="LoanPurpose" style="left: 1px; width: 254px; top: 1px; height: 22px"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="6720">
                    <param name="_ExtentY" value="582">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="0">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td align="right">Registration Date
            </td>
            <td>
                <object id="RegistrationDate" style="left: 1px; width: 143px; top: 1px; height: 23px"
                    tabindex="3" classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="CursorPosition" value="0">
                    <param name="DataProperty" value="0">
                    <param name="DisplayFormat" value="dd/mm/yyyy">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="FirstMonth" value="4">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="dd/mm/yyyy">
                    <param name="HighlightText" value="2">
                    <param name="IMEMode" value="3">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxDate" value="2958465">
                    <param name="MinDate" value="-657434">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="PromptChar" value="_">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ShowLiterals" value="0">
                    <param name="TabAction" value="0">
                    <param name="Text" value="__/__/____">
                    <param name="ValidateMode" value="0">
                    <param name="ValueVT" value="1179649">
                    <param name="Value" value="2.63417926253582E-308">
                    <param name="CenturyMode" value="0">
                </object>
            </td>
        </tr>
    </table>
    <table class="Table2" id="tbl_Txns" style="font-size: 12px; z-index: 108; background-attachment: fixed; left: 13px; position: absolute; top: 217px"
        height="335" cellspacing="1" cellpadding="1"
        width="660" align="center" border="0">
        <tr>
            <td width="111"></td>
            <td align="center" width="3"></td>
            <td align="center"></td>
            <td align="center">Transaction Information
            </td>
        </tr>
        <tr>
            <td width="111">&nbsp; Guarantee Amount&nbsp;&nbsp;
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount1" style="left: 1px; width: 143px; top: 1px; height: 23px" tabindex="4"
                    height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="#######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="5">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td>
                <object id="TxnCode1" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012741633">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc1" style="left: 72px; width: 302px; top: 1px; height: 23px" tabindex="0"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" name="TxnDesc1">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="-1">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td width="111">&nbsp;+ Cash Required
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount2" style="left: 1px; width: 143px; top: 1px; height: 23px" tabindex="5"
                    height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td>
                <object id="TxnCode2" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc2" style="left: 1px; width: 302px; top: 25px; height: 23px" classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td width="111">&nbsp;- Cash Deposit
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount3" style="left: 1px; width: 143px; top: 1px; height: 23px" tabindex="6"
                    height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td>
                <object id="TxnCode3" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc3" style="left: 72px; width: 302px; top: 1px; height: 23px" width="302"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td width="111" height="24">&nbsp;+ QC Proc Fee
            </td>
            <td width="3" height="24"></td>
            <td height="24">
                <object id="TxnAmount4" style="left: 1px; width: 143px; top: 1px; height: 23px" tabindex="7"
                    height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td height="24">
                <object id="TxnCode4" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc4" style="left: 72px; width: 301px; top: 1px; height: 23px" classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7964">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td width="111" height="26">&nbsp;+ Interest on QC
            </td>
            <td width="3" height="26"></td>
            <td height="26">
                <object id="TxnAmount12" style="left: 1px; width: 143px; top: 1px; height: 23px"
                    tabindex="7" height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td height="26">
                <object id="TxnCode12" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc12" style="left: 72px; width: 301px; top: 1px; height: 23px" classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7964">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td width="111">&nbsp;+ Cancel Fee
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount5" style="left: 1px; width: 143px; top: 1px; height: 23px" tabindex="8"
                    height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td>
                <object id="TxnCode5" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc5" style="left: 0px; width: 302px; top: 0px; height: 23px" width="302"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td width="111">&nbsp;+ Conveyancing Fee
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount6" style="left: 1px; width: 143px; top: 1px; height: 23px" tabindex="9"
                    height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td>
                <object id="TxnCode6" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc6" style="left: 0px; width: 302px; top: 0px; height: 23px" width="302"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td width="111" height="25">&nbsp;+ VAT
            </td>
            <td width="3" height="25"></td>
            <td height="25">
                <object id="TxnAmount7" style="left: 1px; width: 143px; top: 1px; height: 23px" tabindex="10"
                    height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td height="25">
                <object id="TxnCode7" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc7" style="left: 0px; width: 302px; top: 0px; height: 23px" width="302"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td width="111">&nbsp;+ Stamp Duty
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount8" style="left: 1px; width: 143px; top: 1px; height: 23px" tabindex="11"
                    height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td>
                <object id="TxnCode8" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc8" style="left: 0px; width: 302px; top: 0px; height: 23px" width="302"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td width="111">&nbsp;+ Deeds Fee
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount9" style="left: 1px; width: 143px; top: 1px; height: 23px" tabindex="12"
                    height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td>
                <object id="TxnCode9" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012741633">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc9" style="left: 0px; width: 302px; top: 0px; height: 23px" width="302"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td width="111">&nbsp;+ Initiation Fee
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount10" style="left: 1px; width: 143px; top: 1px; height: 23px"
                    tabindex="13" height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td>
                <object id="TxnCode10" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc10" style="left: 0px; width: 302px; top: 0px; height: 23px" width="302"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr id="trDiscount">
            <td width="111">&nbsp;- Discount
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount14" style="left: 1px; width: 143px; top: 1px; height: 23px"
                    tabindex="14" height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <input id="hTxnAmount14" type="hidden" />
            </td>
            <td>
                <object id="TxnCode14" style="left: 1px; width: 71px; top: 1px; height: 23px" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc14" style="left: 0px; width: 302px; top: 0px; height: 23px" width="302"
                    classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr id="trDiscounted">
            <td width="111">&nbsp;&nbsp;Total Initiation Fee
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount13" style="left: 1px; width: 143px; top: 1px; height: 23px"
                    tabindex="15" height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <input id="hTxnAmount13" type="hidden" />
            </td>
            <td>
                <input id="cbOverrideDiscount" type="checkbox" />&nbsp;<label for="cbOverrideDiscount" id="lblOverrideDiscount">~</label>
                <input id="hInitiationFeeDiscount" type="hidden" />
                <input id="hAllowOverrideDiscount" type="hidden" />
                <input id="hDiscountedInitiationFeeDetermination" type="hidden" />
            </td>
        </tr>
        <tr>
            <td width="111">&nbsp;+ Valuation Fee
            </td>
            <td width="3"></td>
            <td>
                <object id="TxnAmount11" style="left: 1px; width: 143px; top: 1px; height: 23px"
                    tabindex="16" height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="##,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="-1">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td>
                <object id="TxnCode11" style="left: 1px; width: 71px; top: 1px; height: 23px" height="24"
                    classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="1879">
                    <param name="_ExtentY" value="609">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012741633">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
                <object id="TxnDesc11" style="left: 0px; width: 302px; top: 0px; height: 23px" height="24"
                    width="302" classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" name="TxnDesc11">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="7990">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td align="right" width="111">Total
            </td>
            <td width="3"></td>
            <td>
                <object id="TotalTxnAmount" style="left: 1px; width: 143px; top: 1px; height: 21px"
                    height="24" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="556">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="###,###,##0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="########0.00">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="9999999999.99">
                    <param name="MinValue" value="-999999999.99">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
            <td>
                <object id="ProspectLoanToleranceAmount" style="left: 432px; visibility: visible; width: 143px; top: 317px; height: 21px"
                    classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="3784">
                    <param name="_ExtentY" value="556">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="#######0.00">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0.00">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999.99">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2011693061">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
        </tr>
    </table>
    <input class="button3" id="btn_CATSCreate" title="CATS Create" style="z-index: 101; left: 680px; vertical-align: sub; width: 160px; cursor: hand; padding-top: 12px; position: absolute; top: 292px; height: 55px"
        type="button" value="Manage CATS Transactions"
        name="btn_CATSCreate" height="60">
    <img id="pic_CATSCreate" title="0" style="z-index: 105; left: 750px; width: 19px; position: absolute; top: 294px; height: 23px"
        height="23" alt="" hspace="0" src="images/MLSDenied.bmp"
        width="19" usemap="" border="0">
    <input class="button3" id="btn_GenerateTxns" title="Generate Txns" style="z-index: 100; left: 680px; vertical-align: sub; width: 160px; cursor: hand; padding-top: 12px; position: absolute; top: 432px; height: 55px"
        type="button" value="Generate Transactions"
        name="btn_GenerateTxns" height="60">
    <img id="pic_GenerateTxns" title="0" style="z-index: 104; left: 750px; width: 19px; position: absolute; top: 437px; height: 23px"
        height="23" alt="" hspace="0" src="images/MLSDenied.bmp"
        width="19" usemap="" border="0">
    <table class="Table2" id="tbl_SPV" style="z-index: 109; left: 510px; width: 250px; position: absolute; top: 92px; height: 57px"
        cellspacing="1" cellpadding="1"
        width="250" border="0">
        <tr>
            <td nowrap align="right">Prospect SPV Name
            </td>
            <td nowrap>
                <object id="SPVDescription" style="left: 1px; width: 250px; top: 1px; height: 23px"
                    width="250" classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" name="SPVDescription">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="6615">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td nowrap align="right">Loan SPV Name
            </td>
            <td>
                <object id="LoanSPVDescription" style="left: 1px; width: 250px; top: 1px; height: 23px"
                    width="250" classid="clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D" name="LoanSPVDescription">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="6615">
                    <param name="_ExtentY" value="609">
                    <param name="BackColor" value="-2147483643">
                    <param name="EditMode" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="ReadOnly" value="0">
                    <param name="ShowContextMenu" value="-1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MarginBottom" value="1">
                    <param name="Enabled" value="0">
                    <param name="MousePointer" value="0">
                    <param name="Appearance" value="1">
                    <param name="BorderStyle" value="1">
                    <param name="AlignHorizontal" value="0">
                    <param name="AlignVertical" value="0">
                    <param name="MultiLine" value="0">
                    <param name="ScrollBars" value="0">
                    <param name="PasswordChar" value="">
                    <param name="AllowSpace" value="-1">
                    <param name="Format" value="">
                    <param name="FormatMode" value="1">
                    <param name="AutoConvert" value="-1">
                    <param name="ErrorBeep" value="0">
                    <param name="MaxLength" value="0">
                    <param name="LengthAsByte" value="0">
                    <param name="Text" value="">
                    <param name="Furigana" value="0">
                    <param name="HighlightText" value="0">
                    <param name="IMEMode" value="0">
                    <param name="IMEStatus" value="0">
                    <param name="DropWndWidth" value="0">
                    <param name="DropWndHeight" value="0">
                    <param name="ScrollBarMode" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                </object>
            </td>
        </tr>
    </table>
    <table class="Table2" id="tbl_Prospect" style="z-index: 110; left: 595px; width: 200px; position: absolute; top: 59px; height: 27px"
        cellspacing="1" cellpadding="1"
        width="200" border="0">
        <tr>
            <td nowrap>Prospect Nbr
            </td>
            <td nowrap>
                <object id="ProspectNumber" style="left: 1px; width: 99px; top: 1px; height: 22px"
                    height="22" classid="clsid:49CBFCC2-1337-11D2-9BBF-00A024695830" name="ProspectNumber">
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="2619">
                    <param name="_ExtentY" value="582">
                    <param name="AlignHorizontal" value="1">
                    <param name="AlignVertical" value="0">
                    <param name="Appearance" value="1">
                    <param name="BackColor" value="-2147483643">
                    <param name="BorderStyle" value="1">
                    <param name="BtnPositioning" value="0">
                    <param name="ClipMode" value="0">
                    <param name="ClearAction" value="0">
                    <param name="DecimalPoint" value=".">
                    <param name="DisplayFormat" value="####0;;Null">
                    <param name="EditMode" value="0">
                    <param name="Enabled" value="0">
                    <param name="ErrorBeep" value="0">
                    <param name="ForeColor" value="-2147483640">
                    <param name="Format" value="######0">
                    <param name="HighlightText" value="0">
                    <param name="MarginBottom" value="1">
                    <param name="MarginLeft" value="1">
                    <param name="MarginRight" value="1">
                    <param name="MarginTop" value="1">
                    <param name="MaxValue" value="99999999">
                    <param name="MinValue" value="0">
                    <param name="MousePointer" value="0">
                    <param name="MoveOnLRKey" value="0">
                    <param name="NegativeColor" value="255">
                    <param name="OLEDragMode" value="0">
                    <param name="OLEDropMode" value="0">
                    <param name="ReadOnly" value="0">
                    <param name="Separator" value=",">
                    <param name="ShowContextMenu" value="-1">
                    <param name="ValueVT" value="2012807169">
                    <param name="Value" value="0">
                    <param name="MaxValueVT" value="5">
                    <param name="MinValueVT" value="5">
                </object>
            </td>
        </tr>
    </table>
    <input class="button3" id="btn_SaveFinancials" title="Save Transaction Info" style="z-index: 102; left: 680px; vertical-align: sub; width: 160px; cursor: hand; padding-top: 12px; position: absolute; top: 224px; height: 55px"
        type="button" value="Save Transaction Info"
        name="btn_SaveFinancials" height="60">
    <img alt="" border="0" height="23" hspace="0" id="pic_SavTxnInfo" src="images/MLSDenied.bmp"
        style="z-index: 106; left: 750px; width: 19px; position: absolute; top: 226px; height: 23px"
        title="0" usemap="" width="19">
</body>
</html>