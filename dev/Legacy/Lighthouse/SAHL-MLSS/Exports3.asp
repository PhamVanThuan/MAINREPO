<%
  sDatabase =Session("SQLDatabase")
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_RunCATSExtract = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Run CATS Extract",Session("UserName"))
  i_RunGLExtract = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Run GL Extract",Session("UserName"))
%>
<html>
<head>

<!--#include virtual="/SAHL-MLSS/miscutils.inc"-->
<!--#include virtual="/SAHL-MLSS/dateutils.inc"-->
<object classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331" VIEWASTEXT id="Microsoft_Licensed_Class_Manager_1_0" 1>
<param NAME="LPKPath" VALUE="APEX.lpk">
</object>
<meta name="VI60_DTCScriptingPlatform" content="Server (ASP)">
<meta name="VI60_defaultClientScript" content="VBScript">
<meta NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<script ID="clientEventHandlersVBS" LANGUAGE="vbscript">
<!--

Dim fso
Dim path
Dim folder
Dim file
Dim rs
Dim GeneratedFileName
Dim ExtractFileName

Dim i_days
Dim b_DataLoaded
Dim rs_open
Dim rs1_open

if rs_open <> true then

   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"

	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   return
    end if

	set conn = createobject("ADODB.Connection")
	set rs_Run  = createobject("ADODB.Recordset")
	set rs_temp  = createobject("ADODB.Recordset")

    set rs  = createobject("ADODB.Recordset")
    set rs1  = createobject("ADODB.Recordset")
    set rsloan  = createobject("ADODB.Recordset")
    set rsMBS  = createobject("ADODB.Recordset")
    set rsloanSPV  = createobject("ADODB.Recordset")
    set rsTransaction = createobject("ADODB.Recordset")
    set rs3  = createobject("ADODB.Recordset")
    set rs4  = createobject("ADODB.Recordset")
    set fso = createobject("Scripting.FileSystemObject")
    set rs_tmp  = createobject("ADODB.Recordset")

	sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS[Exports3.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	conn.Open sDSN

end if

Sub window_onload

   ConfigureExportsRunList
   rs_Run.MoveFirst

   tbl_WorkingStorage.style.top = 5000

   b_DataLoaded = false
   SetAccessLightsServer

   window.TDBDate_GLStartDate.DropDown.Visible = 1
   window.TDBDate_GLStartDate.Spin.Visible = 1
   window.TDBDate_GLEndDate.DropDown.Visible = 1

   window.focus

End Sub

Sub window_onfocus

  if b_DataLoaded = true then exit sub

   '********* GL ********************************************************

   window.TDBDate_GLStartDate.Value = Now()
   s_Date1 =   Mid(window.TDBDate_GLStartDate.DisplayText,7,4) & Mid(window.TDBDate_GLStartDate.DisplayText,4,2)  & Mid(window.TDBDate_GLStartDate.DisplayText,1,2)

   window.TDBDate_GLEndDate.Value = Now()
   s_Date2 =   Mid(window.TDBDate_GLEndDate.DisplayText,7,4) & Mid(window.TDBDate_GLEndDate.DisplayText,4,2)  & Mid(window.TDBDate_GLEndDate.DisplayText,1,2)

   rs_Run.MoveFirst
   window.ExportRunList.SelBookmarks.add window.ExportRunList.Getbookmark(0)
   ExportRunList.focus

   window.ExportRunList.SelectedItem = window.ExportRunList.GetBookmark(0)

   b_DataLoaded = true

End Sub

Sub SetAccessLightsServer

    sRes1 = "<%=i_RunGLExtract%>"
    if sRes1 = "Allowed" then
       window.pic_GLExtract.src = "images/MLSAllowed.bmp"
       window.pic_GLExtract.title = "1"
    else
       window.pic_GLExtract.src = "images/MLSDenied.bmp"
       window.pic_GLExtract.title = "0"
	end if

end Sub

''*******************************************************************************8

Sub ConfigureExportsRunList

    document.body.style.cursor = "hand"

    '*** Manually populate the End of Period run list
    rs_Run.Fields.Append "RunNumber",19
    rs_Run.Fields.Append "RunDetail",200,180
    rs_Run.Open

	rs_Run.AddNew
	rs_Run.fields("RunNumber").Value = 4
	rs_Run.fields("RunDetail").Value = "New General Ledger Extract"
	rs_Run.Update

	set ExportRunList.RowSource = rs_Run
	ExportRunList.ListField = rs_Run.Fields("RunDetail").name
	ExportRunList.BoundColumn = rs_Run.Fields("RunNumber").name
	ExportRunList.BoundText = rs_Run.Fields("RunNumber").Value
	ExportRunList.OddRowStyle.BackColor = &HC0FFFF
	ExportRunList.EvenRowStyle.BackColor = &HC0C0C0

	ExportRunList.Refresh

	For I = 0 to ExportRunList.Columns.Count - 1
		ExportRunList.Columns.Remove(0)
	Next

    'Create then necessary columns...
	set tmpCol =  ExportRunList.Columns.Add(0)
	tmpCol.Width = 80
	tmpCol.Caption = "Number"
	tmpCol.DataField = rs_Run.Fields("RunNumber").name
	tmpCol.Visible = false
	set tmpCol =  ExportRunList.Columns.Add(1)
	tmpCol.Caption = "Description"
	tmpCol.Width = 50
	tmpCol.DataField = rs_Run.Fields("RunDetail").name
	tmpCol.Visible = True

	ExportRunList.AlternatingRowStyle = True
	ExportRunList.OddRowStyle.BackColor = &HC0FFFF
	ExportRunList.EvenRowStyle.BackColor = &HC0C0C0
	ExportRunList.HoldFields
    ExportRunList.ReBind
    document.body.style.cursor = "default"

End sub

Sub ExportRunList_RowChange

    window.ExportRunList.SelBookmarks.add window.ExportRunList.Bookmark

    document.body.style.cursor = "wait"

    window.Message.bgColor = "#c0c0c0"

    window.Message.style.visibility = "hidden"

    window.pnl_Message.BackColor = &H00800000&

    window.pnl_Message.ForeColor = vbWhite

    window.pnl_Message.Caption = "Please Wait....Retrieving Data....."

    window.Message.innerText = ""

    window.Message.bgColor = "#c0c0c0"

    window.Message.style.visibility = "hidden"

	window.tbl_GLExtractParms.style.visibility = "hidden"
	window.btn_RunGLExtract.style.visibility = "hidden"
	window.pic_GLExtract.style.visibility = "hidden"

    window.btn_RunGLExtract.style.visibility = "hidden"
    window.btn_RunNewGLExtract.style.visibility = "hidden"

   if window.ExportRunList.Columns.item(1) = "New General Ledger Extract" then

       window.btn_RunCATSExtract.style.visibility = "hidden"
       window.pic_RunCATSExtract.style.visibility = "hidden"
       window.btn_RunCATSDebitOrderExtract.style.visibility = "hidden"

       window.tbl_CATSExtractParms.style.visibility = "hidden"

       window.tbl_GLExtractParms.style.posleft = 300
       window.tbl_GLExtractParms.style.visibility = "visible"
       window.btn_RunGLExtract.style.visibility = "hidden"
       window.pic_GLExtract.style.visibility = "visible"

		window.btn_RunGLExtract.style.visibility = "hidden"

        window.btn_RunNewGLExtract.style.visibility = "visible"
        window.btn_RunNewGLExtract.style.posleft = 690
        window.btn_RunGLExtract.title = "Run New GL Extract"
    end if

 window.pnl_Message.BackColor = &H00C0C0C0&
 window.pnl_Message.Caption = ""

 document.body.style.cursor = "default"
End Sub

Sub btn_RunNewGLExtract_onclick

 if window.pic_GLExtract.title = "0" then
      window.status = "Access denied to " & window.btn_RunNewGLExtract.title
      exit sub
 end if

   i_Resp = MsgBox("Are you sure you want to run the NEW General Ledger Extract for the period  " & window.TDBDate_GLStartDate.DisplayText & " to " & window.TDBDate_GLEndDate.DisplayText , 4)
    if i_Resp= 7 then
       exit sub
    end if

     pnl_Message.BackColor = &H00FF0000&
     pnl_Message.Caption = "Please Wait.. Generating GL Extract Data...This may take some time.."
     pnl_Message.style.visibility = "visible"

    s = window.TDBDate_GLStartDate.DisplayText
	s_date1 = Mid(s,4,2) & "/" & Mid(s,1,2) & "/" & Mid(s,7,4)
	s = window.TDBDate_GLEndDate.DisplayText
	s_date2 = Mid(s,4,2) & "/" & Mid(s,1,2) & "/" & Mid(s,7,4)

    Dim i_res

    i_res = 0
    set com = createobject("ADODB.Command")
    set prm = createobject("ADODB.Parameter")
    set rs_temp = createobject("ADODB.Recordset")

    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS[Exports3.asp 3];uid=<%= Session("UserID")%>"

	com.ActiveConnection = sDSN
	com.CommandType = 4 'AdCmdStoredPRoc
	com.CommandTimeout = 0

	sSQL = "b_GLDebtorIntegration"
	com.CommandText = sSQL

    set prm = com.CreateParameter ( "FromDate",129,1,10,s_date1) 'AdVarchar , adParamInput
	com.Parameters.Append prm
	set prm = com.CreateParameter ( "ToDate",129,1,10,s_date2) 'AdVarchar , adParamInput
	com.Parameters.Append prm

    set rs_temp = com.Execute

    if rs_temp(0).Value <> 0 then

       pnl_Message.BackColor = &H000000FF&
       pnl_Message.Caption= "Error : The New GL Extract stored procedure reported an error...!! Contact IT"
       pnl_Message.style.visibility = "visible"

       MsgBox "ERROR : The New GL Extract stored procedure reported an error...!!!" & chr(13) & chr(10) & Chr(13) & chr(10) & "Please contact IT.....This is important....."

       exit sub
    end if
    rs_temp.close

    'Update the Control File....
    set rs_temp1 = createobject("ADODB.Recordset")
    'Cannot use OLE DB Provider as it appears that it does not return a recordset
    sDSN = "DSN=<%= Session("SQLDatabase") %>;APP=SAHL-MLSS[Exports3.asp 4];uid=<%= Session("UserID")%>"

	set com1 = createobject("ADODB.Command")
    set prm1 = createobject("ADODB.Parameter")
	com1.ActiveConnection = sDSN
	com1.CommandType = 4 'AdCmdStoredPRoc

	sSQL = "y_UpdControlFileExportDate"

	com1.CommandText = sSQL

	s = window.TDBDate_GLEndDate.DisplayText
	s_date = Mid(s,4,2) & "/" & Mid(s,1,2) & "/" & Mid(s,7,4)

    set prm1 = com1.CreateParameter ( "ExportDate",129,1,10,s_date) 'AdVarchar , adParamInput
	com1.Parameters.Append prm1

    set rs_temp1 = com.Execute

    if rs_temp1(0).Value <> 0 then

       pnl_Message.BackColor = &H000000FF&
       pnl_Message.Caption= "Error : Incomplete Export Control File not Updated...!! Contact IT"
       pnl_Message.style.visibility = "visible"

       MsgBox "ERROR : An Error occured while updating the Control GL Export Date...!!!" & chr(13) & chr(10) & Chr(13) & chr(10) & "Please contact IT.....This is important....."

       exit sub
    end if

    rs_temp1.Close

	 pnl_Message.BackColor = &H0000C000&
	 pnl_Message.Caption = " The New General Ledger Extract Completed Successfully...!!"
	 pnl_Message.style.visibility = "visible"

End Sub

-->
</script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">

<body bottomMargin=0 class=Generic leftMargin=0 rightMargin=0 topMargin=0>
<OBJECT id=pnl_Message
style="Z-INDEX: 107; LEFT: 34px; VISIBILITY: visible; WIDTH: 825px; POSITION: relative; TOP: 149px; HEIGHT: 21px"
codeBase=OCX/Threed32.ocx classid=clsid:0BA686B9-F7D3-101A-993E-0000C0EF6F5E
VIEWASTEXT><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="21828"><PARAM NAME="_ExtentY" VALUE="556"><PARAM NAME="_StockProps" VALUE="15"><PARAM NAME="Caption" VALUE=""><PARAM NAME="ForeColor" VALUE="16777215"><PARAM NAME="BackColor" VALUE="12632256"><PARAM NAME="BevelWidth" VALUE="1"><PARAM NAME="BorderWidth" VALUE="3"><PARAM NAME="BevelOuter" VALUE="2"><PARAM NAME="BevelInner" VALUE="0"><PARAM NAME="RoundedCorners" VALUE="-1"><PARAM NAME="Outline" VALUE="0"><PARAM NAME="FloodType" VALUE="0"><PARAM NAME="FloodColor" VALUE="16711680"><PARAM NAME="FloodPercent" VALUE="0"><PARAM NAME="FloodShowPct" VALUE="-1"><PARAM NAME="ShadowColor" VALUE="0"><PARAM NAME="Font3D" VALUE="0"><PARAM NAME="Alignment" VALUE="7"><PARAM NAME="Autosize" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"></OBJECT>
<p>
<table border=0 cellPadding=1 cellSpacing=1 class=Table1 id=tbl_CATSExtractParms
style="Z-INDEX: 114; LEFT: 341px; VISIBILITY: visible; WIDTH: 558px; POSITION: absolute; TOP: 689px; HEIGHT: 85px"
width=558>

  <tr>
    <td align=right noWrap>Cats Extract Date</td>
    <td noWrap>
      <OBJECT id=TDBDate_CATSExtractDate style="LEFT: 1px; WIDTH: 128px; TOP: 1px; HEIGHT: 25px"
	tabIndex=1 classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830">
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3387">
	<PARAM NAME="_ExtentY" VALUE="661">
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
	<PARAM NAME="Enabled" VALUE="-1">
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
</td></tr>
  <tr>
    <td align=right noWrap></td>
    <td noWrap>
      <OBJECT id=TDBDate_CATSCurrentDate style="LEFT: 1px; VISIBILITY: hidden; WIDTH: 128px; TOP: 1px; HEIGHT: 25px"
	tabIndex=1 classid="clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830">
	<PARAM NAME="_Version" VALUE="65536">
	<PARAM NAME="_ExtentX" VALUE="3387">
	<PARAM NAME="_ExtentY" VALUE="661">
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
	<PARAM NAME="Enabled" VALUE="-1">
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
</td></tr>
  <tr>
    <td align=middle colSpan=2 noWrap></td></tr></table></p><input class=button3 id=btn_RunCATSExtract name=btn_RunCATSExtract style="Z-INDEX: 103; LEFT: 488px; VISIBILITY: hidden; VERTICAL-ALIGN: sub; WIDTH: 172px; CURSOR: hand; PADDING-TOP: 12px; POSITION: absolute; TOP: 177px; HEIGHT: 61px" title ="Run CATS Extract" type  =button value="Run CATS 3 Extract" height="48" width="132"><IMG id=pic_RunCATSExtract title=0 style="Z-INDEX: 105; LEFT: 560px; VISIBILITY: hidden; WIDTH: 19px; POSITION: absolute; TOP: 180px; HEIGHT: 23px" height    =23 alt="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 >
<table border=1 cellPadding=1 cellSpacing=1
style="Z-INDEX: 106; LEFT: 30px; WIDTH: 832px; POSITION: absolute; TOP: 145px; HEIGHT: 28px"
width="75%">

  <tr>
    <td id=Message noWrap></td></tr></table>
<p></p>
<p>
</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>
<table background="" border=0 cellPadding=1 cellSpacing=1 class=Table1
id=tbl_GLExtractParms
style="Z-INDEX: 108; LEFT: 300px; WIDTH: 558px; POSITION: absolute; TOP: 12px; HEIGHT: 121px"
width="75%">

  <tr>
    <td align=right noWrap>Start Date</td>
    <td noWrap>
      <OBJECT id=TDBDate_GLStartDate
      style="LEFT: 1px; WIDTH: 128px; TOP: 1px; HEIGHT: 25px" tabIndex=1
      classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3387"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="dd/mm/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td align=right noWrap>End Date</td>
    <td noWrap>
      <OBJECT id=TDBDate_GLEndDate
      style="LEFT: 1px; WIDTH: 128px; TOP: 1px; HEIGHT: 25px" tabIndex=1
      classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="3387"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="dd/mm/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</td></tr>
  <tr>
    <td align=right id=lbl_GLDestination noWrap></td>
    <td noWrap></td></tr>
  <tr>
    <td align=right id=lbl_GLDebtorRegistered noWrap
    >&nbsp;</td>
    <td align=left noWrap style="VERTICAL-ALIGN: top">
      <p id=lbl_Days>&nbsp;</p></td></tr></table><input class=button3 id=btn_RunGLExtract name=btn_RunGLExtract style="Z-INDEX: 109; LEFT: 690px; VERTICAL-ALIGN: sub; WIDTH: 172px; CURSOR: hand; PADDING-TOP: 12px; POSITION: absolute; TOP: 177px; HEIGHT: 61px" title ="Run GL Extract" type =button value="Run GL Extract" height="61"><IMG id=pic_GLExtract title=0 style="Z-INDEX: 111; LEFT: 763px; WIDTH: 19px; POSITION: absolute; TOP: 179px; HEIGHT: 23px" height    =23 alt="" hspace=0 src="images/MLSDenied.bmp" width=19 useMap="" border=0 >
<table border=1 cellPadding=1 cellSpacing=1 id=tbl_WorkingStorage
style="Z-INDEX: 113; LEFT: 694px; WIDTH: 29px; POSITION: absolute; TOP: 804px; HEIGHT: 34px"
width="75%">

  <tr>
    <td noWrap>
      <OBJECT id=TxnLoanNumber
      style="LEFT: 1px; WIDTH: 1px; TOP: 2px; HEIGHT: 25px"
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=1><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="26"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="0000000000"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="0000000000"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="9999999999"><PARAM NAME="MinValue" VALUE="-9999999999"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
      <OBJECT id=TxnSPVNumber
      style="LEFT: 159px; WIDTH: 1px; TOP: 1px; HEIGHT: 26px"
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=1><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="26"><PARAM NAME="_ExtentY" VALUE="688"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="00000"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="00000"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
      <OBJECT id=TxnMBSNumber
      style="LEFT: 220px; WIDTH: 1px; TOP: 2px; HEIGHT: 25px"
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=1><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="26"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="00000"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="00000"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
      <OBJECT id=TxnInvestorNumber
      style="LEFT: 284px; WIDTH: 1px; TOP: 2px; HEIGHT: 25px"
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=1><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="26"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="00000"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="00000"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="99999"><PARAM NAME="MinValue" VALUE="0"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
      <OBJECT id=TDBDate_WSDate
      style="LEFT: 324px; VISIBILITY: visible; WIDTH: 1px; TOP: 4px; HEIGHT: 23px"
      tabIndex=1 classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830 width=1><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="26"><PARAM NAME="_ExtentY" VALUE="609"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="dd/mm/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="mm/dd/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="__/__/____"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="1179649"><PARAM NAME="Value" VALUE="2.63417926253582E-308"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
      <OBJECT id=TxnAmount
      style="LEFT: 440px; WIDTH: 1px; TOP: 2px; HEIGHT: 25px"
      classid=clsid:49CBFCC2-1337-11D2-9BBF-00A024695830 width=1><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="26"><PARAM NAME="_ExtentY" VALUE="661"><PARAM NAME="AlignHorizontal" VALUE="1"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="ClearAction" VALUE="0"><PARAM NAME="DecimalPoint" VALUE="."><PARAM NAME="DisplayFormat" VALUE="00000000000.00"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="0"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="00000000000.00"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxValue" VALUE="999999999999.99"><PARAM NAME="MinValue" VALUE="-9999999999.99"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="NegativeColor" VALUE="255"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="Separator" VALUE=","><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ValueVT" VALUE="2011758597"><PARAM NAME="Value" VALUE="0"><PARAM NAME="MaxValueVT" VALUE="5"><PARAM NAME="MinValueVT" VALUE="5"></OBJECT>
      <OBJECT id=TDBDate_FromDate
      style="LEFT: 0px; WIDTH: 3px; TOP: 0px; HEIGHT: 33px"
      classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="79"><PARAM NAME="_ExtentY" VALUE="873"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="mm/dd/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="mm/dd/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="12/20/2000"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="7"><PARAM NAME="Value" VALUE="36880"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
      <OBJECT id=TDBDate_ToDate style="LEFT: 0px; TOP: 0px"
      classid=clsid:A49CE0E4-C0F9-11D2-B0EA-00A024695830><PARAM NAME="_Version" VALUE="65536"><PARAM NAME="_ExtentX" VALUE="2117"><PARAM NAME="_ExtentY" VALUE="873"><PARAM NAME="AlignHorizontal" VALUE="0"><PARAM NAME="AlignVertical" VALUE="0"><PARAM NAME="Appearance" VALUE="1"><PARAM NAME="BackColor" VALUE="-2147483643"><PARAM NAME="BorderStyle" VALUE="1"><PARAM NAME="BtnPositioning" VALUE="0"><PARAM NAME="ClipMode" VALUE="0"><PARAM NAME="CursorPosition" VALUE="0"><PARAM NAME="DataProperty" VALUE="0"><PARAM NAME="DisplayFormat" VALUE="mm/dd/yyyy"><PARAM NAME="EditMode" VALUE="0"><PARAM NAME="Enabled" VALUE="-1"><PARAM NAME="ErrorBeep" VALUE="0"><PARAM NAME="FirstMonth" VALUE="4"><PARAM NAME="ForeColor" VALUE="-2147483640"><PARAM NAME="Format" VALUE="mm/dd/yyyy"><PARAM NAME="HighlightText" VALUE="0"><PARAM NAME="IMEMode" VALUE="3"><PARAM NAME="MarginBottom" VALUE="1"><PARAM NAME="MarginLeft" VALUE="1"><PARAM NAME="MarginRight" VALUE="1"><PARAM NAME="MarginTop" VALUE="1"><PARAM NAME="MaxDate" VALUE="2958465"><PARAM NAME="MinDate" VALUE="-657434"><PARAM NAME="MousePointer" VALUE="0"><PARAM NAME="MoveOnLRKey" VALUE="0"><PARAM NAME="OLEDragMode" VALUE="0"><PARAM NAME="OLEDropMode" VALUE="0"><PARAM NAME="PromptChar" VALUE="_"><PARAM NAME="ReadOnly" VALUE="0"><PARAM NAME="ShowContextMenu" VALUE="-1"><PARAM NAME="ShowLiterals" VALUE="0"><PARAM NAME="TabAction" VALUE="0"><PARAM NAME="Text" VALUE="12/20/2000"><PARAM NAME="ValidateMode" VALUE="0"><PARAM NAME="ValueVT" VALUE="6815751"><PARAM NAME="Value" VALUE="36880"><PARAM NAME="CenturyMode" VALUE="0"></OBJECT>
</td></tr></table>
<OBJECT id=ExportRunList
style="Z-INDEX: 112; LEFT: 30px; WIDTH: 265px; POSITION: absolute; TOP: 3px; HEIGHT: 140px"
classid=clsid:0D62356A-DBA2-11D1-B5DF-0060976089D0
data=data:application/x-oleobject;base64,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//////////8AAAAACAAAAAEAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAkAAAABAAAAgAAAAAAAAAAEAAAAwMDAAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAKAAAAAQAAAIAAAAAAAAAABAAAAP//xgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAwAAAACAAAAAAAAAAAAAAAUAAAAQnu7AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAANAAAAAwAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADgAAAAUAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA8AAAAHAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAQAAAABgAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEQAAAAgAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABIAAAAJAAAAAAAAAAAAAAAEAAAAwMDAAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAATAAAACgAAAAAAAAAAAAAABAAAAP//xgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFAAAAAQAAAAAAAAAAAAAABEAAAAAgAAA////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABUAAAAMAAAAAAAAAAAAAAAUAAAAQnu7AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAWAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFwAAAA8AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABgAAAALAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAZAAAADAAAAAAAAAAAAAAAFAAAAEJ7uwD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABsAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAcAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB4AAAAdAAAAwAIBAAACAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAfAAAAHQAAAMAAAQAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIAAAAB0AAADAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACEAAAAeAAAAAAEAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAiAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIwAAAB0AAACAAAAAAAAAAAQAAAAA//8ACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACQAAAAdAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AQAAAAAAAAAAAAAAAQAAAP7///8EAAAAAAAAAAAAAAAFAAAA/f///wIAAAAAAAAAAAAAAAIAAADq////AwAAAAAAAAAAAAAAAwAAAPz///8FAAAAAAAAAAAAAAD/////+////wYAAAAAAAAAAAAAAAQAAAD6////BwAAAAAAAAAAAAAA//////H///8IAAAAAAAAAAAAAAAGAAAA7////wkAAAAAAAAAAAAAAAcAAADu////CgAAAAAAAAAAAAAACAAAAPn///8LAAAAAQAAAAAAAAD/////6////xQAAAABAAAAAAAAAP/////4////DAAAAAEAAAAAAAAA/////+n///8NAAAAAQAAAAAAAAD/////9////w4AAAABAAAAAAAAAP/////2////EAAAAAEAAAAAAAAA//////X///8PAAAAAQAAAAAAAAD/////8P///xEAAAABAAAAAAAAAP/////t////EgAAAAEAAAAAAAAA/////+z///8TAAAAAQAAAAAAAAD/////9P///xgAAAABAAAAAQAAAP/////z////FQAAAAEAAAABAAAA/////+j///8WAAAAAQAAAAEAAAD/////8v///xcAAAABAAAAAQAAAP/////0////HAAAAAEAAAACAAAA//////P///8ZAAAAAQAAAAIAAAD/////6P///xoAAAABAAAAAgAAAP/////y////GwAAAAEAAAACAAAA/////wgAAABOb3JtYWwABkDSOAbQ0TgGYNE4BpDQOAYApjgGYKU4Bh0AAABIZWFkaW5nAAAAAAAVAAAAAAAAAP////8AAAAAAAAAAB4AAABGb290aW5nAAgAAID//8YAAAAAAAAAAAAAAAAAUEjNBh8AAABTZWxlY3RlZAAAAABEcmFnHgAAAG9sdW0xAAAAEAAAACAAAABDYXB0aW9uADEAAAAgAAAA3NU4BgysOAYOAACADQAAgCEAAABIaWdobGlnaHRSb3cAAAAADKM4BgysOAYQAAAAIQAAACIAAABFdmVuUm93ADEAAABQAAAASCc2BkgnNgYAAAAAAAAAACMAAABPZGRSb3cAADEAAAAgAQAAsCc2BrAnNgYAAAAAAAAAACQAAAADAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAB4AAAABAAAAAAAAAAMAAAAAAAAACwAAAP//AAADAAAAAAAAAB4AAAAYAAAARW1wbG95ZWVUZWFtRGVzY3JpcHRpb24AHgAAABMAAABFbXBsb3llZVRlYW1OdW1iZXIAAAsAAAAAAAAABAAAAAAAAAADAAAA6AMAAB4AAAABAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAA0AcAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAAAAAAAeAAAAMgAAAERyYWcgYSBjb2x1bW4gaGVhZGVyIGhlcmUgdG8gZ3JvdXAgYnkgdGhhdCBjb2x1bW4AAAAtAAAAAAAAAA4AAABFeHBvcnRSdW5MaXN0AL0AAAAOAAAAQW5pbWF0ZVdpbmRvdwDAAAAAEwAAAEFuaW1hdGVXaW5kb3dDbG9zZQC+AAAAFwAAAEFuaW1hdGVXaW5kb3dEaXJlY3Rpb24AvwAAABIAAABBbmltYXRlV2luZG93VGltZQD4/f//CwAAAEFwcGVhcmFuY2UACP7//wwAAABCb3JkZXJTdHlsZQBzAAAADAAAAEJvdW5kQ29sdW1uAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAJQAAAAkAAABEYXRhTW9kZQC7AAAACQAAAERhdGFWaWV3AAoAAAAMAAAARGVmQ29sV2lkdGgAXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkADAAAAAPAAAARXhwb3NlQ2VsbE1vZGUAkQAAAAoAAABGb290TGluZXMAwgAAAA8AAABHcm91cEJ5Q2FwdGlvbgAMAAAACgAAAEhlYWRMaW5lcwBlAAAADwAAAEludGVncmFsSGVpZ2h0AF0AAAAPAAAATGF5b3V0RmlsZU5hbWUAXAAAAAsAAABMYXlvdXROYW1lALEAAAAKAAAATGF5b3V0VXJsAGMAAAAKAAAATGlzdEZpZWxkAIAAAAALAAAATWF0Y2hFbnRyeQC8AAAAEgAAAE1hdGNoRW50cnlUaW1lb3V0AKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAIQAAAAMAAAATXVsdGlTZWxlY3QAYQAAAA4AAABNdWx0aXBsZUxpbmVzALQAAAALAAAAUHJpbnRJbmZvcwAPAAAAEAAAAFJvd0RpdmlkZXJTdHlsZQCcAAAACgAAAFJvd01lbWJlcgAjAAAABwAAAFNwbGl0cwDTBwAACQAAAF9FeHRlbnRYANQHAAAJAAAAX0V4dGVudFkAAAIAAAwAAABfTGF5b3V0VHlwZQAQAAAACwAAAF9Sb3dIZWlnaHQAAQIAAAsAAABfU3R5bGVEZWZzAAQCAAAWAAAAX1dhc1BlcnNpc3RlZEFzUGl4ZWxzAA==
height=123></OBJECT>
 <input class=button3 id=btn_RunNewGLExtract name=btn_RunNewGLExtract style="Z-INDEX: 99; LEFT: 116px; VISIBILITY: hidden; VERTICAL-ALIGN: sub; WIDTH: 172px; CURSOR: hand; PADDING-TOP: 12px; POSITION: absolute; TOP: 177px; HEIGHT: 61px" title ="Run GL Extract" type  =button value="Run New GL Extract" height="61"><IMG style="Z-INDEX: 116; LEFT: 630px; POSITION: absolute; TOP: 189px" height   =40 alt="" src="images/downdisk.gif" width=50 ><INPUT class=button3 id=btn_RunCATSDebitOrderExtract title="Run CATS Debit OrderExtract" style="Z-INDEX: 103; LEFT: 689px; VISIBILITY: hidden; VERTICAL-ALIGN: sub; WIDTH: 172px; CURSOR: hand; PADDING-TOP: 12px; POSITION: absolute; TOP: 177px; HEIGHT: 61px" type=button value="Run CATS Debit Order Extract" name=btn_RunCATSDebitOrderExtract width="132" height="48"></p>

</body>
</html>