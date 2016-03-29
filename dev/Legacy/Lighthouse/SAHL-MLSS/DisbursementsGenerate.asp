
<%
Response.Expires = 0
sDatabase =Session("SQLDatabase") 
sUid = Session("UserID") 

set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
i_EditDisbursements= oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Edit Disbursements",Session("UserName"))
i_ViewActionDate = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"View Action Date",Session("UserName"))
i_GenerateCATSFile = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Generate CATS File",Session("UserName"))
i_ReverseCATSGeneration = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Reverse Generation",Session("UserName"))
i_ViewCATSExportFiles = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"View CATS Exports",Session("UserName"))
%>

<html>
<head>
<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->

<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>
<meta name="VI60_defaultClientScript" content="VBScript">
<meta name="VI60_DefaultLoanScript" Content="VBScript">

<meta NAME="GENERATOR" Content="Microsoft FrontPage 4.0">
<SCRIPT ID=clientEventHandlersVBS LANGUAGE=vbscript>
<!--

Sub TrueDBGrid_HeadClick(ColIndex)

    on error resume next
	set tmpCol =  window.TrueDBGrid.Columns.item(ColIndex)
	s =  tmpCol.DataField
    rs_GridDisbursement.Sort =  s
End Sub

-->
</SCRIPT>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<script ID="LoanEventHandlersVBS" LANGUAGE="vbscript">
<!--
dim i_CurrentLoanNbr
dim s_ReturnPage
dim so_Source
dim v_BookMark
dim s_CurrentLoanNbr
dim b_loading 
dim b_AllDataLoaded
dim s_Action
dim d_ActionDate
dim b_GridConfigured
dim i_cnt


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
		set rs_GridDisbursement  = createobject("ADODB.Recordset")
		set rs_DisbursementTotal  = createobject("ADODB.Recordset")
		'sDSN = "DSN=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
		sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [DisbursementsGenerate.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
		conn.Open sDSN
		rs_open = false
		rs_GridDisbursement_open = false
		b_AllDataLoaded = false 
	end if

end if

Sub SetAccessLightsServer
	
	sRes1 = "<%=i_EditDisbursements%>"
	if sRes1 = "Allowed" then
	  window.pic_EditDisbursements.src = "images/MLSAllowed.bmp"
	  window.pic_EditDisbursements.accesskey = "1"
	else
	  window.pic_EditDisbursements.src = "images/MLSDenied.bmp"
	  window.pic_EditDisbursements.accesskey = "0"
	end if

	sRes1 = "<%=i_GenerateCATSFile%>"
	if sRes1 = "Allowed" then
		window.pic_GenerateCATSFile.src = "images/MLSAllowed.bmp"
		window.pic_GenerateCATSFile.accesskey = "1"
	else
		window.pic_GenerateCATSFile.src = "images/MLSDenied.bmp"
		window.pic_GenerateCATSFile.accesskey = "0"
	end if
	
	sRes1 = "<%=i_ReverseCATSGeneration%>"
	if sRes1 = "Allowed" then
		window.pic_ReverseCATSGeneration.src = "images/MLSAllowed.bmp"
		window.pic_ReverseCATSGeneration.accesskey = "1"
	else
		window.pic_ReverseCATSGeneration.src = "images/MLSDenied.bmp"
		window.pic_ReverseCATSGeneration.accesskey = "0"	
	end if
	
	sRes1 = "<%=i_ViewCATSExportFiles%>"
	if sRes1 = "Allowed" then
		window.pic_ViewCATSExports.src = "images/MLSAllowed.bmp"
		window.pic_ViewCATSExports.accesskey = "1"
	else
		window.pic_ViewCATSExports.src = "images/MLSDenied.bmp"
		window.pic_ViewCATSExports.accesskey = "0"	
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
	tmpCol.Width = 100
	tmpCol.Caption = "Loan Nbr"
	tmpCol.DataField = rs_GridDisbursement.Fields("LoanNumber").name
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid.Columns.Add(1)
	tmpCol.Caption = "Prepared"
	tmpCol.Width =80
	tmpCol.DataField = rs_GridDisbursement.Fields("PreparedDate").name 
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid.Columns.Add(2)
	tmpCol.Caption = "Actioned"
	tmpCol.Width =80
	tmpCol.DataField = rs_GridDisbursement.Fields("ActionDate").name 
	tmpCol.Visible = True
	
	
	set tmpCol =  TrueDBGrid.Columns.Add(3)
	tmpCol.Caption = "SPV"
	tmpCol.Width = 50
	tmpCol.DataField = rs_GridDisbursement.Fields("SPVDescription").name 
	tmpCol.Visible = True
	
	set tmpCol =  TrueDBGrid.Columns.Add(4)
	tmpCol.Caption = "Account Name"
	tmpCol.Alignment = 3
    tmpCol.Width = 120
	tmpCol.DataField = rs_GridDisbursement.Fields("AccountName").name 
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(5)
	tmpCol.Caption = "Account Number"
	tmpCol.Width = 120
	tmpCol.DataField = rs_GridDisbursement.Fields("AccountNumber").name 
	tmpCol.Alignment = 3
	tmpCol.Visible = True

	set tmpCol =  TrueDBGrid.Columns.Add(6)
	tmpCol.Caption = "Bank"
	tmpCol.Width = 150
	tmpCol.DataField = rs_GridDisbursement.Fields("BankDescription").name 
	tmpCol.Alignment = 3
	tmpCol.Visible = True


	set tmpCol =  TrueDBGrid.Columns.Add(7)
	tmpCol.Caption = "Branch Code"
	tmpCol.Width = 60
	tmpCol.DataField = rs_GridDisbursement.Fields("BranchCode").name 
	tmpCol.Alignment = 3
	tmpCol.Visible = True
	
	
	set tmpCol =  TrueDBGrid.Columns.Add(8)
	tmpCol.Caption = "Amount"
	tmpCol.Width = 80
	tmpCol.DataField = rs_GridDisbursement.Fields("Amount").name 
	tmpCol.Alignment = 1
	tmpCol.Visible = True
	
	'Set the colors_GridDisbursement....
	TrueDBGrid.OddRowStyle.BackColor = &HC0FFFF
	TrueDBGrid.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBGrid.HoldFields
	
	document.body.style.cursor = "default"
	
	b_GridConfigured = true

End sub

Sub GetLoanDetails

'if i_cnt >= 1 then exit sub

	b_AllDataLoaded = false
	pnl_Message.BackColor = &H000000FF&
	pnl_Message.Caption= "Please Wait.................. Fetching Loans..!!"
	pnl_Message.style.visibility = "visible"

	if rs_open = true  then
	   rs_open = false
	end if
'msgbox "start"
	if window.rdb_ReadyForDisbursement.checked  = true then	
		sSQL = "r_GetLoansReadyForDisbursement '" & d_ActionDate & "'"
	elseif window.rdb_DisbursedToCats.checked = true then  	
		sSQL = "r_GetDisbursementsByActionDate '" & d_ActionDate & "'"
	end if
	
   
	rs_GridDisbursement.CursorLocation = 3
	rs_GridDisbursement.CacheSize  = 20
	    
	rs_GridDisbursement.Open sSQL,conn,adOpenStatic
	
	iTot = 0.00
		
	if rs_GridDisbursement.RecordCount >  0 then
	   rs_GridDisbursement.MoveFirst
	   Do while not  rs_GridDisbursement.EOF	
			
			iTot = iTot + rs_GridDisbursement.Fields("Amount").Value
			rs_GridDisbursement.MoveNext
	   Loop
	end if	


	set TrueDBGrid.DataSource = rs_GridDisbursement
	
    TrueDBGrid.Refresh
    
	rs_GridDisbursement_open = true
   
	window.tbl_Total.focus  
	
	window.DisbursementTotal.Value   =  iTot
			
	b_AllDataLoaded = true	    
			    
	 i_cnt = i_cnt + 1
	window.status = CSTR(i_cnt)

	if window.rdb_ReadyForDisbursement.checked = true then
		
		window.btn_ReverseCATSGeneration.disabled = true
		if rs_GridDisbursement.RecordCount = 0 then
			window.btn_EditDisbursements.disabled = true
			window.btn_GenerateCATSDisbursementFile.disabled = true  
		elseif rs_GridDisbursement.RecordCount <> 0 then
			window.btn_EditDisbursements.disabled = false
			window.btn_GenerateCATSDisbursementFile.disabled = false  
		end if
		
	end if

	if IsDate(window.ActionDate.Text) then
		dim dActionDate
		dim dTodayDate

		dActionDate = FormatDateTime(CDate(window.ActionDate.Text), 2)
		dTodayDate = FormatDateTime(date, 2)
		
		if window.rdb_DisbursedToCats.checked  = true then 
			window.btn_EditDisbursements.disabled = true
			window.btn_GenerateCATSDisbursementFile.disabled = true  
		
			if dActionDate = dTodayDate and rs_GridDisbursement.RecordCount <> 0 then
				window.btn_ReverseCATSGeneration.disabled = false
			end if
		
			if dActionDate = dTodayDate and rs_GridDisbursement.RecordCount = 0 then
				window.btn_ReverseCATSGeneration.disabled = true
			end if 			
		
			if dActionDate <> dTodayDate then
				window.btn_ReverseCATSGeneration.disabled = true
			end if
		end if
		
	end if
	
	b_AllDataLoaded = true	 

	pnl_Message.BackColor = &H00FF0000&
	pnl_Message.Caption= "Records Loaded..!!"
	pnl_Message.style.visibility = "visible"
	


end sub



Sub window_onload
   
        
    
	tbl_Date.focus 
	window.ActionDate.Text = date
	d_ActionDate = Mid(window.ActionDate.Text, 4, 2) & "/" & Mid(window.ActionDate.Text, 1, 2) & "/" & Mid(window.ActionDate.Text, 7, 4)
	window.ActionDate.Enabled = false 


	b_loading = true
	b_GridConfigured = false
	i_cnt = 0
	SetAccessLightsServer

	window.ActionDate.DropDown.Visible = 1
	window.ActionDate.Spin.Visible = 1
	
	so_Source = "<%=Request.QueryString("Source")%>"
	s_ReturnPage = "<%=Request.QueryString("returnpage")%>"
'	msgbox so_Source
'	msgbox s_ReturnPage
  
 '   msgbox "<%=Request.QueryString("purpose")%>"
    
	s_CurrentLoanNbr = "<%=Request.QueryString("Number")%>"

   GetLoanDetails
	
	window.tbl_Total.focus  
	'window.DisbursementTotal.Value   =  rs_DisbursementTotal.Fields("Total").Value		

	if b_GridConfigured = false then
		ConfigureDetailGrid
	end if
	
	window.TrueDBGrid.Style.visibility = "visible"
	
End Sub




Sub TrueDBGrid_RowColChange(LastRow, LastCol)

 	if b_AllDataLoaded = true then
   
		if rs_GridDisbursement.RecordCount > 0 then
			window.LoanNumber.Value =  rs_GridDisbursement.Fields("LoanNumber").Value
			
			i_CurrentLoanNbr =  rs_GridDisbursement.Fields("LoanNumber").Value

			sSQL = "c_GetLoanClientDetails '" & i_CurrentLoanNbr & "'"
			rs_Client.CursorLocation = 3
			rs_Client.CacheSize  =10
			rs_Client.Open sSQL,conn,adOpenDynamic
			
			window.LoanFirstNames.Text  = rs_Client.Fields("ClientFirstNames").Value
			window.LoanSurname.Text = rs_Client.Fields("ClientSurname").Value
			rs_Client.Close 
		else
			window.LoanNumber.Value = 0
			window.LoanFirstNames.Text = ""
			window.LoanSurname.Text = ""
		end if
		
	end if
	
End Sub

Function ValidateFields
	ValidateFields = -1

	if window.ActionDate  < date() then
		msgbox "Action Date cannot be less than today....!!!"
		window.ActionDate.focus 
		exit Function
	end if

	ValidateFields = 0 
End function


Sub btn_Exit_onclick
	
		pnl_Message.BackColor =&H000000FF&
		pnl_Message.Caption= "Please Wait............... Exiting.!!"
		pnl_Message.style.visibility = "visible"

	'	window.parent.frames("RegistrationPanel").location.href = "RegistrationCATS-old.asp"'s_ReturnPage' & "?Number= " & CStr(i_CurrentLoanNbr )
	   ' msgbox  s_ReturnPage & "?Number="  & CStr(s_CurrentLoanNbr) & "&purpose=<%=Request.QueryString("purpose")%>&Source=" & s_ReturnPage & "&returnpage="  & so_Source & "&RepliesReceived=<%=Request.QueryString("RepliesReceived")%>"  & "&Lodged=<%=Request.QueryString("Lodged")%>" & "&Disbursements=<%=Request.QueryString("Disbursements")%>" & "&Readvances=<%=Request.QueryString("Disbursements")%>"
		window.parent.frames("RegistrationPanel").location.href = s_ReturnPage & "?Number="  & CStr(s_CurrentLoanNbr) & "&purpose=<%=Request.QueryString("purpose")%>&Source=" & so_Source & "&returnpage="  & s_ReturnPage & "&RepliesReceived=<%=Request.QueryString("RepliesReceived")%>"  & "&Lodged=<%=Request.QueryString("Lodged")%>" & "&Disbursements=<%=Request.QueryString("Disbursements")%>" & "&Readvances=<%=Request.QueryString("Disbursements")%>" & "&UpForFees=<%=Request.QueryString("UpForFees")%>"
	 

		
End Sub

Sub GetData
	
	if Trim(window.ActionDate.Text) = "__/__/____"  then
		d_ActionDate = ""
	else
		d_ActionDate = Mid(window.ActionDate.Text, 4, 2) & "/" & Mid(window.ActionDate.Text, 1, 2) & "/" & Mid(window.ActionDate.Text, 7, 4)
	end if 
  
  if rs_GridDisbursement_open = true  then
       rs_GridDisbursement.Close
       rs_GridDisbursement_open = false
	end if
	
	GetLoanDetails
	
	window.tbl_Total.focus  
'	window.DisbursementTotal.Value   =  rs_DisbursementTotal.Fields("Total").Value		

End Sub

Sub rdb_DisbursedToCats_onclick
	window.ActionDate.Text = date
	window.ActionDate.Enabled = true
	window.btn_EditDisbursements.disabled = true
	window.btn_ReverseCATSGeneration.disabled = false
	window.btn_GenerateCATSDisbursementFile.disabled = true  

	GetData  
End Sub

Sub rdb_ReadyForDisbursement_onclick
 
  
    

	window.ActionDate.Text = date
	window.ActionDate.Enabled = false 
	window.btn_EditDisbursements.disabled = false
	window.btn_GenerateCATSDisbursementFile.disabled = false  
	window.btn_ReverseCATSGeneration.disabled = true

	GetData
End Sub

Sub ActionDate_Change

  if b_AllDataLoaded  = false then exit sub

	window.btn_EditDisbursements.disabled = true
	window.btn_ReverseCATSGeneration.disabled = true
	window.btn_GenerateCATSDisbursementFile.disabled = true  

	GetData
End Sub

Sub btn_EditDisbursements_onclick
 
	if window.pic_EditDisbursements.accesskey = "0" then
		window.status = "Access denied to " & window.btn_EditDisbursements.title 
		exit sub
	end if
	v_BookMark = window.TrueDBGrid.Bookmark
' window.parent.frames("RegistrationPanel").location.href = "DisbursementsCreate.asp?Number= " & window.LoanNumber.Value & "&Source=DisbursementsGenerate.asp&purpose=" & CStr("")
     i_CurrentLoanNbr =  rs_GridDisbursement.Fields("LoanNumber").Value
     
 	 window.parent.frames("RegistrationPanel").location.href = "DisbursementManage.asp?Number= " & CStr(i_CurrentLoanNbr) & "&purpose=<%=Request.QueryString("purpose")%>&Source=" & so_Source & "&Status=6"


End Sub

Sub btn_GenerateCATSDisbursementFile_onclick
	if window.pic_GenerateCATSFile.accesskey = "0" then
		window.status = "Access denied to " & window.btn_GenerateCATSDisbursementFile.title 
		exit sub
	end if
		v_BookMark = window.TrueDBGrid.Bookmark
	 window.parent.frames("RegistrationPanel").location.href = "DisbursementExport.asp?ActionDate= " & window.ActionDate.Text & "&returnpage=DisbursementsGenerate.asp"
End Sub

Sub btn_ReverseCATSGeneration_onclick

	if window.pic_ReverseCATSGeneration.accesskey = "0" then
		window.status = "Access denied to " & window.btn_ReverseCATSGeneration.title 
		exit sub
	end if

	i_response = msgbox("Are you sure you want to reverse CATs Disbursements for date " & window.ActionDate.Text, 4)

	if i_response = 7 then
		exit sub
	end if
	
	Dim i_res

	UpdateLoanRecord = -1

	document.body.style.cursor = "hand"
	   
	i_res = 0     
	set com = createobject("ADODB.Command")
	set prm = createobject("ADODB.Parameter")
	set rs_temp = createobject("ADODB.Recordset")

	'Cannot use OLE DB Provider as it appears that it does not return a recordset
	sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS [DisbursementsGenerate.asp 2];uid=<%= Session("UserID")%>"
	
	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc

	sSQL = "c_ReverseCATSDisbursements" '" & d_ActionDate & "'"
		
	com.CommandText = sSQL
	
	s_date = Mid(window.ActionDate.Text, 4, 2) & "/" & Mid(window.ActionDate.Text, 1, 2) & "/" & Mid(window.ActionDate.Text, 7, 4)
	set prm = com.CreateParameter ("DisbursementActionDate",129,1,10,s_date) 'AdVarchar , adParamInput
	com.Parameters.Append prm

	set rs_temp = com.Execute 
	
	GetData
	
	document.body.style.cursor = "default"

End Sub

Sub btn_ViewCATSExports_onclick

	if window.pic_ViewCATSExports.accesskey = "0" then
		window.status = "Access denied to " & window.btn_ViewCATSExports.title 
		exit sub
	end if
		v_BookMark = window.TrueDBGrid.Bookmark
	 window.parent.frames("RegistrationPanel").location.href = "DisbursementExport.asp?ActionDate= " & window.ActionDate.Text & "&returnpage=DisbursementsGenerate.asp&Action=View"

End Sub

-->
</script>


<body bottomMargin="0" rightMargin="0" topMargin="0" leftMargin="0" class="Generic">
<OBJECT classid=clsid:0BA686B9-F7D3-101A-993E-0000C0EF6F5E 
codeBase=../../OCX/Threed32.ocx height=21 id=pnl_Message 
style="HEIGHT: 21px; LEFT: 20px; POSITION: absolute; TOP: 442px; VISIBILITY: visible; WIDTH: 865px; Z-INDEX: 107"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="22886"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="_StockProps" VALUE="15"><PARAM NAME="Caption" VALUE=""><PARAM NAME="ForeColor" VALUE="16777215"><PARAM NAME="BackColor" VALUE="12632256"><PARAM NAME="BevelWidth" VALUE="1"><PARAM NAME="BorderWidth" VALUE="3"><PARAM NAME="BevelOuter" VALUE="2"><PARAM NAME="BevelInner" VALUE="0"><PARAM NAME="RoundedCorners" VALUE="-1"><PARAM NAME="Outline" VALUE="0"><PARAM NAME="FloodType" VALUE="0"><PARAM NAME="FloodColor" VALUE="16711680"><PARAM NAME="FloodPercent" VALUE="0"><PARAM NAME="FloodShowPct" VALUE="-1"><PARAM NAME="ShadowColor" VALUE="0"><PARAM NAME="Font3D" VALUE="0"><PARAM NAME="Alignment" VALUE="7"><PARAM NAME="Autosize" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"></OBJECT>
<p>
<table border="0" cellPadding="1" cellSpacing="1" style="HEIGHT: 86px; LEFT: 20px; POSITION: absolute; TOP: 1px; WIDTH: 461px; Z-INDEX: 107" width="75%" class="Table1">
  
  <tr>
    <td align="right" noWrap>
    &nbsp;Loan Number</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 id=LoanNumber 
      style="HEIGHT: 26px; LEFT: 1px; TOP: 1px; WIDTH: 99px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2619"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="####0;;Null"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="######0"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2012807169"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap>Client First Names</td>
    <td align="left" noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D 
      id=LoanFirstNames style="HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 283px" 
      tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="7488"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td align="right" noWrap>Client Surname</td>
    <td align="left" noWrap>
      <OBJECT classid=clsid:E2D000D6-2DA1-11D2-B358-00104B59D73D id=LoanSurname 
      style="HEIGHT: 25px; LEFT: 1px; TOP: 1px; WIDTH: 284px" tabIndex=0><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="7514"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="MultiLine" VALUE="0"><PARAM NAME="ScrollBars" VALUE="0"><PARAM NAME="PasswordChar" VALUE=""><PARAM NAME="AllowSpace" VALUE="-1"><PARAM NAME="Format" VALUE=""><PARAM NAME="FormatMode" VALUE="1"><PARAM NAME="AutoConvert" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="MaxLength" VALUE="0"><PARAM NAME="LengthAsByte" VALUE="0"><PARAM NAME="Text" VALUE=""><PARAM NAME="Furigana" VALUE="0"><PARAM NAME="HighlightText" VALUE="-1"><PARAM NAME="IMEMode" VALUE="0"><PARAM NAME="IMEStatus" VALUE="0"><PARAM NAME="DropWndWidth" VALUE="0"><PARAM NAME="DropWndHeight" VALUE="0"><PARAM NAME="ScrollBarMode" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"></OBJECT>
</td></tr></table>
<OBJECT classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0 
data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAYAAAABAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAHAAAAAQAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAgAAAAAAAAAAEAAAAwMDAAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAIAAAAAAAAAABAAAAP//xgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAAMoi2AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAwMDAAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAP//xgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAAMoi2AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAADKItgD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwAALAMAAChAQAAzBxDCZkAAAAAAAAAAAAAAB0AAABIZWFkaW5nAEAAAAAxAAAA4BV0BVDMSgkCAAAABAAAAB4AAABGb290aW5nAAAAAAAAAAAAAAAAAAAAAAAAAAAAABZ0BR8AAABTZWxlY3RlZAAAAAAFAAAAwAAAAP////8AAAAAAAAAACAAAABDYXB0aW9uAAAAAAAAFnQFAABICfAVdAVlcgAA9P///yEAAABIaWdobGlnaHRSb3cAAAAADPBKCSApSAkxAAAA4AAAACIAAABFdmVuUm93AAIAAACQ50IJuwAAAG9kZQBAAAAAMQAAACMAAABPZGRSb3cACTEAAABQAQAAyCdICcgnSAkOAACADQAAgCQAAAALAAAA//8AAAMAAAAAAAAACwAAAAAAAAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAP//AAADAAAAAAAAAAQAAAAAAAAAAwAAAOgDAAALAAAAAAAAAAsAAAAAAAAAAwAAAAEAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAIAAAAeAAAAMgAAAERyYWcgYSBjb2x1bW4gaGVhZGVyIGhlcmUgdG8gZ3JvdXAgYnkgdGhhdCBjb2x1bW4AAAADAAAAAAAAAAMAAAAAAAAAAwAAAMgAAAADAAAAAAAAAAMAAAD3ogYAAwAAAJDQAwA9AAAAAAAAAAsAAABUcnVlREJHcmlkAAIAAAAMAAAAQWxsb3dBZGROZXcALwAAAAwAAABBbGxvd0Fycm93cwABAAAADAAAAEFsbG93RGVsZXRlAAQAAAAMAAAAQWxsb3dVcGRhdGUAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQAI/v//DAAAAEJvcmRlclN0eWxlAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAnAAAAAsAAABEYXRhTWVtYmVyACUAAAAJAAAARGF0YU1vZGUAuwAAAAkAAABEYXRhVmlldwDEAAAAEgAAAERlYWRBcmVhQmFja0NvbG9yAAoAAAAMAAAARGVmQ29sV2lkdGgAUAAAAA0AAABFZGl0RHJvcERvd24AXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkADAAAAAPAAAARXhwb3NlQ2VsbE1vZGUAkQAAAAoAAABGb290TGluZXMAwgAAAA8AAABHcm91cEJ5Q2FwdGlvbgAMAAAACgAAAEhlYWRMaW5lcwCYAAAACwAAAEluc2VydE1vZGUAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAsQAAAAoAAABMYXlvdXRVUkwASgAAAA4AAABNYXJxdWVlVW5pcXVlAM4AAAAIAAAATWF4Um93cwCjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgCEAAAADAAAAE11bHRpU2VsZWN0AGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUAlwAAABEAAABQaWN0dXJlQWRkbmV3Um93AJUAAAASAAAAUGljdHVyZUN1cnJlbnRSb3cAswAAABEAAABQaWN0dXJlRm9vdGVyUm93ALIAAAARAAAAUGljdHVyZUhlYWRlclJvdwCWAAAAEwAAAFBpY3R1cmVNb2RpZmllZFJvdwCwAAAAEwAAAFBpY3R1cmVTdGFuZGFyZFJvdwC0AAAACwAAAFByaW50SW5mb3MADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAIwAAAAcAAABTcGxpdHMAMQAAABAAAABUYWJBY3Jvc3NTcGxpdHMAMgAAAAoAAABUYWJBY3Rpb24AmQAAABcAAABUcmFuc3BhcmVudFJvd1BpY3R1cmVzADMAAAAQAAAAV3JhcENlbGxQb2ludGVyANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA 
height=500 id=TrueDBGrid 
style="HEIGHT: 349px; LEFT: 20px; POSITION: absolute; TOP: 91px; VISIBILITY: hidden; WIDTH: 866px; Z-INDEX: 108" 
width=871></OBJECT>


<table border="0" cellPadding="1" cellSpacing="1" style="HEIGHT: 90px; LEFT: 563px; POSITION: absolute; TOP: 1px; WIDTH: 212px; Z-INDEX: 105" width="403" id="tbl_Date" class="Table1">
  
  <tr>
    <td width="250" colspan="2" id="td_Ready"><input CHECKED id="rdb_ReadyForDisbursement" name="viewdate" type="radio" value="Ready">&nbsp;Ready For Disbursement <input id="rdb_DisbursedToCats" name="viewdate" type="radio" value="Disbursed">&nbsp;Disbursed to CATS </td>
    </tr>
  <tr>
    <td width="20" align="right">Date : </td>
	 <td align="left" noWrap id="lbl_ActionDate" width="192">
      <OBJECT classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830 id=ActionDate 
      style="HEIGHT: 23px; LEFT: 1px; TOP: 1px; WIDTH: 119px" tabIndex=3><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3149"><PARAM NAME="_ExtentY" VALUE="609"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="dd/mm/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
&nbsp; 
    
</td>
	</tr>
  
</table>
<table border="0" cellPadding="1" cellSpacing="1" id="tbl_Total" style="HEIGHT: 27px; LEFT: 667px; POSITION: absolute; TOP: 479px; WIDTH: 215px; Z-INDEX: 106" width="75%" class="Table1">
  
  <tr>
    <td align="right" noWrap>Total</td>
    <td noWrap>
      <OBJECT classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 
      id=DisbursementTotal name=DisbursementTotal 
      style="HEIGHT: 26px; LEFT: 0px; TOP: 0px; WIDTH: 140px"><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3704"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="##,###,###,##0.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="###########0.00"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="999999999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2012807169"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
</td></tr></table>

<br><img alt ="" border="0" height="23" hspace="0" id="pic_GenerateCATSFile" name="pic_GenerateCATSFile" src="http://sahlnet/SAHL-MLSS/images/MLSDenied.bmp" style             ="CURSOR: hand; HEIGHT: 23px; LEFT: 202px; POSITION: absolute; TOP: 480px; WIDTH: 19px; Z-INDEX: 103" title="0" useMap="" width="19"><br><br>


<img alt ="" border="0" height="23" hspace="0" id="pic_EditDisbursements" src="http://sahlnet/SAHL-MLSS/images/MLSDenied.bmp" style             ="CURSOR: hand; HEIGHT: 23px; LEFT: 80px; POSITION: absolute; TOP: 480px; WIDTH: 19px; Z-INDEX: 104" title="0" useMap="" width="19"><br><br><br>
<table border="0" cellPadding="1" cellSpacing="1" class="Table1" style="HEIGHT: 74px; LEFT: 22px; POSITION: absolute; TOP: 464px; WIDTH: 627px" width="75%">
  
  <tr>
    <td><input class="button3" id="btn_EditDisbursements" name="btn_EditDisbursements" style="CURSOR: hand; HEIGHT: 55px; LEFT: 277px; PADDING-TOP: 28px; TOP: 993px; VISIBILITY: visible; WIDTH: 120px" title="Edit Disbursements" type="button" value="Edit Disbursements" width="120"></td>
    <td><input class="button3" id="btn_GenerateCATSDisbursementFile" name="btn_GenerateCATSDisbursementFile" style="CURSOR: hand; HEIGHT: 55px; LEFT: 277px; PADDING-TOP: 28px; TOP: 993px; VISIBILITY: visible; WIDTH: 120px" title="Generate CATS File" type="button" value="Generate CATS File" width="120"></td>
    <td><input class="button3" id="btn_ReverseCATSGeneration" name="btn_ReverseCATSGeneration" style="CURSOR: hand; HEIGHT: 55px; LEFT: 277px; PADDING-TOP: 28px; TOP: 992px; VISIBILITY: visible; WIDTH: 120px" title="Reverse Generation" type="button" value="Reverse Generation" width="120"><input class="button3" id="btn_ViewCATSExports" title="View CATS Export Files" style="CURSOR: hand; HEIGHT: 55px; LEFT: 277px; PADDING-TOP: 28px; TOP: 993px; VISIBILITY: visible; WIDTH: 120px" type="button" value="View CATS Exports" name="btn_ViewCATSExports" width="120"></td>
    <td><input class="button2" id="btn_Exit" name="btn_Exit" style="CURSOR: hand; HEIGHT: 55px; LEFT: 709px; TOP: 998px; VISIBILITY: visible; WIDTH: 120px; Z-INDEX: 101" title="Exit" type="button" value="Exit" width="120"></td></tr></table><br>

<img alt ="" border="0" height="23" hspace="0" id="pic_ReverseCATSGeneration" src="http://sahlnet/SAHL-MLSS/images/MLSDenied.bmp" style             ="CURSOR: hand; HEIGHT: 23px; LEFT: 326px; POSITION: absolute; TOP: 480px; WIDTH: 19px; Z-INDEX: 102" title="0" useMap="" width="19" name="pic_ReverseCATSGeneration">&nbsp;&nbsp; <img id="pic_ViewCATSExports" title="0" style="CURSOR: hand; HEIGHT: 23px; LEFT: 446px; POSITION: absolute; TOP: 480px; WIDTH: 19px; Z-INDEX: 103" height="23" alt ="" hspace="0" src="http://sahlnet/SAHL-MLSS/images/MLSDenied.bmp" width ="19" useMap="" border="0" name="pic_ViewCATSExports">

</p> 


</body>
</html>
