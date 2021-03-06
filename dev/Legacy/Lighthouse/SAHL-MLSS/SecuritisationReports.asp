<%
 sDatabase =Session("SQLDatabase")
  sUid = Session("UserID")

  set oSecurity = Server.CreateObject("MLSSecurity.FunctionClass")
  i_ClientServicesReports = oSecurity.CheckFunctionAccess("DSN=" & sDatabase & ";uid=" & sUid,"Run Registration Report",Session("UserName"))
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

Dim fso
Dim i_switch
Dim gi_LegalEntity
Dim gi_JointOwner
Dim i_EmployeeType
Dim i_SAHLEmployeeNumber
Dim i_EmployeeTeamNumber
Dim i_CurrentProspect
Dim i_CurrentStage
Dim b_loading

Dim CrystalCtrl1

if rs_open <> true then

   	'Make sure user has logged on properly...if nt redirect him to logon page...
    sessDSN= "DSN=<%= Session("DSN")%>"

	if  sessDSN = "DSN=" then
	   window.top.location.href = "Default.asp"
	   return
    end if

	set conn = createobject("ADODB.Connection")
	set rs_Grid  = createobject("ADODB.Recordset")
	set rs_temp = createobject("ADODB.Recordset")

	sDSN = "Provider=SQLOLEDB.1;Application Name='SAHL-MLSS [SecuritisationReports.asp 1]';Data Source=<%= Session("SQLDatabase") %>;uid=<%= Session("UserID") %>"
	conn.Open sDSN
	rs_open = false
	rs_SelectedProspect_open = false
	b_loading = true

end if
Sub GetProspectReportData()

   document.body.style.cursor = "wait"
    i_switch = true

    if rs_open = true  then
       rs_Grid.Close
       rs_open = false
	end if

    sSQL = "s_GetSecuritisationReports"

    rs_Grid.CursorLocation = 3

	rs_Grid.Open sSQL,conn,adOpenDynamic

	TrueDBGrid.DataSource = rs_Grid

	rs_open = true

    document.body.style.cursor = "default"

End Sub
Sub ConfigureReportGrid
	'Reconfigure the Grid..
    'Remove all columns
    Dim I

    document.body.style.cursor = "hand"

	For I = 0 to TrueDBGrid.Columns.Count - 1
		TrueDBGrid.Columns.Remove(0)
	Next

   set Itm = CreateObject("TrueOleDBGrid60.ValueItem")
   set FormatStyle = CreateObject("TrueOleDBGrid60.Style")

    'Create then necessary columns...
	set tmpCol =  TrueDBGrid.Columns.Add(0)
	tmpCol.Width = 60
	tmpCol.Caption = "Number"
	tmpCol.DataField = rs_Grid.Fields("ReportNumber").name
	tmpCol.Visible = True
	set tmpCol =  TrueDBGrid.Columns.Add(1)
	tmpCol.Caption = "Report Name"
	tmpCol.Width =320
	tmpCol.DataField = rs_Grid.Fields("ReportName").name
	tmpCol.Visible = True
	set tmpCol =  TrueDBGrid.Columns.Add(2)
	tmpCol.Caption = "Description"
	tmpCol.Width = 130
	tmpCol.DataField = rs_Grid.Fields("ReportDescription").name
	tmpCol.Visible = True

	'*********STAGE********
	set tmpCol =  TrueDBGrid.Columns.Add(3)
	tmpCol.Caption = "File Name"
	tmpCol.DataField = rs_Grid.Fields("ReportFileName").name
	 tmpCol.Width = 220
	tmpCol.Visible = True

	'Set the colors_Grid....
	TrueDBGrid.OddRowStyle.BackColor = &HC0FFFF
	TrueDBGrid.EvenRowStyle.BackColor = &HC0C0C0

	TrueDBGrid.HoldFields

'	EnableAllControls
    document.body.style.cursor = "default"

End sub
Sub window_onload
 b_AllDataLoaded = false
 'If session has timed out x will be '=' and redirect to default.asp will occur..
 x = "=<%= Session("SQLDatabase")%>"
 if x = "=" then
   exit sub
 end if
 window.TrueDBGridCtrl1.style.left = 0
 window.TrueDBGridCtrl1.style.top = 0
 window.TrueDBGridCtrl1.style.width = 0
 window.TrueDBGridCtrl1.style.height = 0

 GetProspectReportData
 ConfigureReportGrid

 window.btn_RunReport.disabled = false

End Sub

Sub btn_RunReport_onclick

'update the report statistics field
 sSQL = "i_UpdReportStatistics '" & rs_Grid.Fields("ReportFileName").Value & "'"
 conn.Execute sSQL
 if rs_Grid.Fields("ReportDescription").value = "S2K Report" then
	'Get the report path from the Control File
	sSQL = "SELECT ControlText FROM CONTROL (nolock) WHERE ControlDescription = 'System Reports Path S2K'"
	rs_temp.CursorLocation = 3
	rs_temp.Open sSQL,conn,adOpenDynamic
	ReportPath = rs_temp.Fields("ControlText")
	rs_temp.close

	location.href = ReportPath & "/Securitisation/" & rs_Grid.Fields("ReportFileName").Value
else
 if window.btn_RunReport.value = "Close Report" then
   set CrystalCtrl1 = nothing
   TrueDBGridCtrl1_MouseMove 0,0,0,0
   exit sub
else
    set CrystalCtrl1 = CreateObject("Crystal.CrystalReport")
end if
 ReportPath = ""

'Get the report path from the Control File
 sSQL = "SELECT ControlText FROM CONTROL (nolock) WHERE ControlDescription = 'System Reports Path'"
 rs_temp.CursorLocation = 3
 rs_temp.Open sSQL,conn,adOpenDynamic
 ReportPath = rs_temp.Fields("ControlText")
 rs_temp.close

 ' Use the Grid as the display area of the report...
 CrystalCtrl1.WindowParentHandle = window.TrueDBGridCtrl1.hWnd ' window.TrueDBGrid.hWnd

 'Turn the Grid's scrollbars off....so that the do not interfere with the Reports scrollbars
 TrueDBGrid.ScrollBars =0
 window.TrueDBGridCtrl1.style.left = 30
 window.TrueDBGridCtrl1.style.top = 0
 window.TrueDBGridCtrl1.style.width = 861
 window.TrueDBGridCtrl1.style.height = 463
 'Set the size of the reports display area
 CrystalCtrl1.WindowLeft = 0
 CrystalCtrl1.WindowTop = 0
 CrystalCtrl1.WindowWidth = 860
 CrystalCtrl1.WindowHeight = 462

 ' Show various buttons...
 CrystalCtrl1.WindowShowCloseBtn = false
 CrystalCtrl1.WindowShowGroupTree = true
 CrystalCtrl1.WindowShowPrintSetupBtn = true
 CrystalCtrl1.WindowShowZoomCtl = true
 CrystalCtrl1.WindowAllowDrillDown = true

 CrystalCtrl1.ReportFileName = ReportPath & "Securitisation\" & rs_Grid.Fields("ReportFileName") '"C:\Program Files\SAHL\Crystal Reports\Prospects\Prospect Detail.rpt"

 CrystalCtrl1.Connect = "<<Use Integrated Security>>"

'The report needs integrated security so.....force NT security by using <<Use Integrated Security>>
 CrystalCtrl1.UserName = "<<Use Integrated Security>>"

 CrystalCtrl1.ProgressDialog =true
 CrystalCtrl1.WindowShowRefreshBtn = true

'Show and size the  repeort
 CrystalCtrl1.action =1

 CrystalCtrl1.PageZoom(75)

 window.focus
 window.btn_RunReport.value = "Close Report"
end if
End Sub

Sub TrueDBGrid_MouseMove(Button, Shift, X , Y )

'i.e. the report was opened in the grid and now has been closeds...so reset and show
'     the grids scrollbars
if TrueDBGrid.ScrollBars = 0 then
  TrueDBGrid.ScrollBars =2
end if

End Sub

Sub TrueDBGridCtrl1_MouseMove(m,s,x,y)

 window.TrueDBGridCtrl1.style.left = 0
 window.TrueDBGridCtrl1.style.top = 0
 window.TrueDBGridCtrl1.style.width = 0
 window.TrueDBGridCtrl1.style.height = 0

 if TrueDBGrid.ScrollBars = 0 then
  TrueDBGrid.ScrollBars =2
 end if

 window.btn_RunReport.value = "Run Securitisation Report"

End Sub

-->
</script>
</head>
<link href="SAHL-MLSS.css" rel="stylesheet" type="text/css">
<body class=Generic>

<p>
<OBJECT classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0
data=data:application/x-oleobject;base64,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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABgAAAAEAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAcAAAABAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAIAAAAAQAAAAAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACQAAAAEAAACAAAAAAAAAAAQAAADAwMAACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAoAAAABAAAAgAAAAAAAAAAEAAAA///GAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAALAAAAAQAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADAAAAAIAAAAAAAAAAAAAABQAAAA3kr0A////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA0AAAADAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAOAAAABQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADwAAAAcAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABAAAAAGAAAAAAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAARAAAACAAAAAAAAAAAAAAABAAAAAgAAIAFAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEgAAAAkAAAAAAAAAAAAAAAQAAADAwMAACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABMAAAAKAAAAAAAAAAAAAAAEAAAA///GAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAUAAAABAAAAAAAAAAAAAAAEQAAAACAAAD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFQAAAAwAAAAAAAAAAAAAABQAAAA3kr0A////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABYAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAXAAAADwAAAAAAAAAAAAAABAAAAPeiBgAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGAAAAAsAAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABkAAAAMAAAAAAAAAAAAAAAUAAAAN5K9AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAaAAAADQAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGwAAAA8AAAAAAAAAAAAAAAQAAAD3ogYACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABwAAAALAAAAAAAAAAAAAAAEAAAA96IGAAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAdAAAAAAAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHgAAAB0AAADAAgEAAAIAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB8AAAAdAAAAwAABAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAgAAAAHQAAAMAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIQAAAB4AAAAAAQAAAAAAABEAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACIAAAAdAAAAwAAAAAAAAAAEAAAACAAAgAUAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAjAAAAHQAAAIAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAJAAAAB0AAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAAAAAA//////////8BAAAAAAAAAAAAAAABAAAA/v///wQAAAAAAAAAAAAAAAUAAAD9////AgAAAAAAAAAAAAAAAgAAAOr///8DAAAAAAAAAAAAAAADAAAA/P///wUAAAAAAAAAAAAAAP/////7////BgAAAAAAAAAAAAAABAAAAPr///8HAAAAAAAAAAAAAAD/////8f///wgAAAAAAAAAAAAAAAYAAADv////CQAAAAAAAAAAAAAABwAAAO7///8KAAAAAAAAAAAAAAAIAAAA+f///wsAAAABAAAAAAAAAP/////r////FAAAAAEAAAAAAAAA//////j///8MAAAAAQAAAAAAAAD/////6f///w0AAAABAAAAAAAAAP/////3////DgAAAAEAAAAAAAAA//////b///8QAAAAAQAAAAAAAAD/////9f///w8AAAABAAAAAAAAAP/////w////EQAAAAEAAAAAAAAA/////+3///8SAAAAAQAAAAAAAAD/////7P///xMAAAABAAAAAAAAAP/////0////GAAAAAEAAAABAAAA//////P///8VAAAAAQAAAAEAAAD/////6P///xYAAAABAAAAAQAAAP/////y////FwAAAAEAAAABAAAA//////T///8cAAAAAQAAAAIAAAD/////8////xkAAAABAAAAAgAAAP/////o////GgAAAAEAAAACAAAA//////L///8bAAAAAQAAAAIAAAD/////CAAAAE5vcm1hbAAGEHi8BiBxvAYgILwGYFa8BjBrvAbQerwGHQAAAEhlYWRpbmcA/////wAAAAD/CQD//////2xsdW0AAAAAHgAAAEZvb3RpbmcAAAAAAAAAAAAAAAAAAAAAAAAAAAD+////HwAAAFNlbGVjdGVkAAAAAGj7uwYAAL0GWPu7BgAAAAAAAwAAIAAAAENhcHRpb24AIAAAACEAAAAAAAAAAQAAAAAAAAAAAAAGIQAAAEhpZ2hsaWdodFJvdwBlY3VyaXRpc2F0aW9uIFJlcG9yIgAAAEV2ZW5Sb3cAAAAAAGj7uwYAAAAAWPu7BgAAAAAAAAAAIwAAAE9kZFJvdwAAAAAAAAAAAAAAAO7/WPu7BjEAAABAAAAAJAAAAAsAAAD//wAAAwAAAAAAAAALAAAAAAAAAAMAAAAAAAAACwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAALAAAA//8AAAMAAAAAAAAABAAAAAAAAAADAAAA6AMAAAsAAAD//wAACwAAAAAAAAADAAAAAQAAAB4AAAABAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAB4AAAAyAAAARHJhZyBhIGNvbHVtbiBoZWFkZXIgaGVyZSB0byBncm91cCBieSB0aGF0IGNvbHVtbgAAAAMAAAAAAAAAAwAAAAAAAAADAAAAyAAAAAMAAAAAAAAAAwAAAPeiBgADAAAAkNADAD0AAAAAAAAACwAAAFRydWVEQkdyaWQAAgAAAAwAAABBbGxvd0FkZE5ldwAvAAAADAAAAEFsbG93QXJyb3dzAAEAAAAMAAAAQWxsb3dEZWxldGUABAAAAAwAAABBbGxvd1VwZGF0ZQC9AAAADgAAAEFuaW1hdGVXaW5kb3cAwAAAABMAAABBbmltYXRlV2luZG93Q2xvc2UAvgAAABcAAABBbmltYXRlV2luZG93RGlyZWN0aW9uAL8AAAASAAAAQW5pbWF0ZVdpbmRvd1RpbWUA+P3//wsAAABBcHBlYXJhbmNlAAj+//8MAAAAQm9yZGVyU3R5bGUA+v3//wgAAABDYXB0aW9uAGAAAAAJAAAAQ2VsbFRpcHMAfgAAAA4AAABDZWxsVGlwc0RlbGF5AH0AAAAOAAAAQ2VsbFRpcHNXaWR0aACPAAAADgAAAENvbHVtbkZvb3RlcnMABwAAAA4AAABDb2x1bW5IZWFkZXJzAAgAAAAIAAAAQ29sdW1ucwCcAAAACwAAAERhdGFNZW1iZXIAJQAAAAkAAABEYXRhTW9kZQC7AAAACQAAAERhdGFWaWV3AMQAAAASAAAARGVhZEFyZWFCYWNrQ29sb3IACgAAAAwAAABEZWZDb2xXaWR0aABQAAAADQAAAEVkaXREcm9wRG93bgBfAAAACgAAAEVtcHR5Um93cwD+/f//CAAAAEVuYWJsZWQAMAAAAA8AAABFeHBvc2VDZWxsTW9kZQCRAAAACgAAAEZvb3RMaW5lcwDCAAAADwAAAEdyb3VwQnlDYXB0aW9uAAwAAAAKAAAASGVhZExpbmVzAJgAAAALAAAASW5zZXJ0TW9kZQBdAAAADwAAAExheW91dEZpbGVOYW1lAFwAAAALAAAATGF5b3V0TmFtZQCxAAAACgAAAExheW91dFVSTABKAAAADgAAAE1hcnF1ZWVVbmlxdWUAzgAAAAgAAABNYXhSb3dzAKMAAAAKAAAATW91c2VJY29uAKQAAAANAAAATW91c2VQb2ludGVyAIQAAAAMAAAATXVsdGlTZWxlY3QAYQAAAA4AAABNdWx0aXBsZUxpbmVzAJ8AAAAMAAAAT0xFRHJhZ01vZGUAoAAAAAwAAABPTEVEcm9wTW9kZQCXAAAAEQAAAFBpY3R1cmVBZGRuZXdSb3cAlQAAABIAAABQaWN0dXJlQ3VycmVudFJvdwCzAAAAEQAAAFBpY3R1cmVGb290ZXJSb3cAsgAAABEAAABQaWN0dXJlSGVhZGVyUm93AJYAAAATAAAAUGljdHVyZU1vZGlmaWVkUm93ALAAAAATAAAAUGljdHVyZVN0YW5kYXJkUm93ALQAAAALAAAAUHJpbnRJbmZvcwAPAAAAEAAAAFJvd0RpdmlkZXJTdHlsZQAjAAAABwAAAFNwbGl0cwAxAAAAEAAAAFRhYkFjcm9zc1NwbGl0cwAyAAAACgAAAFRhYkFjdGlvbgCZAAAAFwAAAFRyYW5zcGFyZW50Um93UGljdHVyZXMAMwAAABAAAABXcmFwQ2VsbFBvaW50ZXIA0wcAAAkAAABfRXh0ZW50WADUBwAACQAAAF9FeHRlbnRZAAACAAAMAAAAX0xheW91dFR5cGUAEAAAAAsAAABfUm93SGVpZ2h0AAECAAALAAAAX1N0eWxlRGVmcwAEAgAAFgAAAF9XYXNQZXJzaXN0ZWRBc1BpeGVscwA=
height=500 id=TrueDBGrid
style="HEIGHT: 463px; LEFT: 30px; POSITION: absolute; TOP: -2px; WIDTH: 861px; Z-INDEX: 1000"
width=861></OBJECT>
<input id="btn_RunReport" name="btn_RunReport" style="CURSOR: hand; HEIGHT: 60px; LEFT: 330px; POSITION: absolute; TOP: 466px; WIDTH: 250px; Z-INDEX: 1003" title          ="Run Securitisation Report" type="button" value="Run Securitisation Reports" height="60" class=button2>
<OBJECT classid=clsid:0D6234D0-DBA2-11D1-B5DF-0060976089D0
data=data:application/x-oleobject;base64,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AAAAAAAAAAD//////////wAAAAAGAAAAAQAAAAAAAAAAAAAABAAAAA0AAIAOAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAABwAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAgAAAABAAAAAAAAAAAAAAAEAAAACAAAgAUAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAJAAAAAQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAACgAAAAEAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAAsAAAABAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAMAAAAAgAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAADQAAAAMAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAA4AAAAFAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAPAAAABwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEAAAAAYAAAAAAAAAAAAAAAQAAAANAACADgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABEAAAAIAAAAAAAAAAAAAAAEAAAACAAAgAUAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAASAAAACQAAAAAAAAAAAAAABAAAAAD//wAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAEwAAAAoAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABQAAAAEAAAAAAAAAAAAAAARAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAVAAAADAAAAAAAAAAAAAAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAFgAAAA0AAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABcAAAAPAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAYAAAACwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAGQAAAAwAAAAAAAAAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAABoAAAANAAAAAAAAAAAAAAAUAAAADwAAgBIAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAbAAAADwAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHAAAAAsAAAAAAAAAAAAAAAQAAAAFAACACAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAAB0AAAAAAAAAAAAAAAAAAAAEAAAABQAAgAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAeAAAAHQAAAMACAQAAAgAAFAAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHwAAAB0AAADAAAEAAAAAABQAAAAPAACAEgAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACAAAAAdAAAAwAAAAAAAAAAEAAAADQAAgA4AAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAhAAAAHgAAAAABAAAAAAAAEQAAAA8AAIASAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAIgAAAB0AAADAAAAAAAAAAAQAAAAIAACABQAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////AAAAACMAAAAdAAAAgAAAAAAAAAAEAAAAAP//AAgAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////wAAAAAkAAAAHQAAAAAAAAAAAAAABAAAAAUAAIAIAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////8AAAAAHQAAAAAAAAAAAAAAAAAAAAAAAAD//////////wEAAAAAAAAAAAAAAAEAAAD+////BAAAAAAAAAAAAAAABQAAAP3///8CAAAAAAAAAAAAAAACAAAA6v///wMAAAAAAAAAAAAAAAMAAAD8////BQAAAAAAAAAAAAAA//////v///8GAAAAAAAAAAAAAAAEAAAA+v///wcAAAAAAAAAAAAAAP/////x////CAAAAAAAAAAAAAAABgAAAO////8JAAAAAAAAAAAAAAAHAAAA7v///woAAAAAAAAAAAAAAAgAAAD5////CwAAAAEAAAAAAAAA/////+v///8UAAAAAQAAAAAAAAD/////+P///wwAAAABAAAAAAAAAP/////p////DQAAAAEAAAAAAAAA//////f///8OAAAAAQAAAAAAAAD/////9v///xAAAAABAAAAAAAAAP/////1////DwAAAAEAAAAAAAAA//////D///8RAAAAAQAAAAAAAAD/////7f///xIAAAABAAAAAAAAAP/////s////EwAAAAEAAAAAAAAA//////T///8YAAAAAQAAAAEAAAD/////8////xUAAAABAAAAAQAAAP/////o////FgAAAAEAAAABAAAA//////L///8XAAAAAQAAAAEAAAD/////9P///xwAAAABAAAAAgAAAP/////z////GQAAAAEAAAACAAAA/////+j///8aAAAAAQAAAAIAAAD/////8v///xsAAAABAAAAAgAAAP////8IAAAATm9ybWFsAAAhAAAAUAAAAEQluwZEJbsG3Fv4BkluZm8dAAAASGVhZGluZwAdAAAAHgAAAMACAQACAAAAIAAAAAAAAAAeAAAARm9vdGluZwAAAAAACEWeBwAAuwb4RJ4H4Ez3BqhyHQAfAAAAU2VsZWN0ZWQAAAAAqHIdAB8AAADwab0GMQAAAKAAAAAgAAAAQ2FwdGlvbgAAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAhAAAASGlnaGxpZ2h0Um93AAAAAAAAAAAAAAAAAAAAAAAAAAAiAAAARXZlblJvdwAAAAAAAAAAAAAAcCAAAAAAAAAAAAHDtgYjAAAAT2RkUm93AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAkAAAACwAAAP//AAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAALAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAAwAAAAAAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAAeAAAAAQAAAAAAAAADAAAAAAAAAAsAAAAAAAAAAwAAAAAAAAAEAAAAAAAAAAMAAADoAwAACwAAAP//AAALAAAAAAAAAAMAAAABAAAAHgAAAAEAAAAAAAAAAwAAAAAAAAADAAAAAAAAAAMAAAAAAAAAHgAAADIAAABEcmFnIGEgY29sdW1uIGhlYWRlciBoZXJlIHRvIGdyb3VwIGJ5IHRoYXQgY29sdW1uAAAAAwAAAAAAAAADAAAAAAAAAAMAAADIAAAAAwAAAAAAAAADAAAAwMDAAAMAAACQ0AMAPQAAAAAAAAAQAAAAVHJ1ZURCR3JpZEN0cmwxAAIAAAAMAAAAQWxsb3dBZGROZXcALwAAAAwAAABBbGxvd0Fycm93cwABAAAADAAAAEFsbG93RGVsZXRlAAQAAAAMAAAAQWxsb3dVcGRhdGUAvQAAAA4AAABBbmltYXRlV2luZG93AMAAAAATAAAAQW5pbWF0ZVdpbmRvd0Nsb3NlAL4AAAAXAAAAQW5pbWF0ZVdpbmRvd0RpcmVjdGlvbgC/AAAAEgAAAEFuaW1hdGVXaW5kb3dUaW1lAPj9//8LAAAAQXBwZWFyYW5jZQAI/v//DAAAAEJvcmRlclN0eWxlAPr9//8IAAAAQ2FwdGlvbgBgAAAACQAAAENlbGxUaXBzAH4AAAAOAAAAQ2VsbFRpcHNEZWxheQB9AAAADgAAAENlbGxUaXBzV2lkdGgAjwAAAA4AAABDb2x1bW5Gb290ZXJzAAcAAAAOAAAAQ29sdW1uSGVhZGVycwAIAAAACAAAAENvbHVtbnMAnAAAAAsAAABEYXRhTWVtYmVyACUAAAAJAAAARGF0YU1vZGUAuwAAAAkAAABEYXRhVmlldwDEAAAAEgAAAERlYWRBcmVhQmFja0NvbG9yAAoAAAAMAAAARGVmQ29sV2lkdGgAUAAAAA0AAABFZGl0RHJvcERvd24AXwAAAAoAAABFbXB0eVJvd3MA/v3//wgAAABFbmFibGVkADAAAAAPAAAARXhwb3NlQ2VsbE1vZGUAkQAAAAoAAABGb290TGluZXMAwgAAAA8AAABHcm91cEJ5Q2FwdGlvbgAMAAAACgAAAEhlYWRMaW5lcwCYAAAACwAAAEluc2VydE1vZGUAXQAAAA8AAABMYXlvdXRGaWxlTmFtZQBcAAAACwAAAExheW91dE5hbWUAsQAAAAoAAABMYXlvdXRVUkwASgAAAA4AAABNYXJxdWVlVW5pcXVlAM4AAAAIAAAATWF4Um93cwCjAAAACgAAAE1vdXNlSWNvbgCkAAAADQAAAE1vdXNlUG9pbnRlcgCEAAAADAAAAE11bHRpU2VsZWN0AGEAAAAOAAAATXVsdGlwbGVMaW5lcwCfAAAADAAAAE9MRURyYWdNb2RlAKAAAAAMAAAAT0xFRHJvcE1vZGUAlwAAABEAAABQaWN0dXJlQWRkbmV3Um93AJUAAAASAAAAUGljdHVyZUN1cnJlbnRSb3cAswAAABEAAABQaWN0dXJlRm9vdGVyUm93ALIAAAARAAAAUGljdHVyZUhlYWRlclJvdwCWAAAAEwAAAFBpY3R1cmVNb2RpZmllZFJvdwCwAAAAEwAAAFBpY3R1cmVTdGFuZGFyZFJvdwC0AAAACwAAAFByaW50SW5mb3MADwAAABAAAABSb3dEaXZpZGVyU3R5bGUAIwAAAAcAAABTcGxpdHMAMQAAABAAAABUYWJBY3Jvc3NTcGxpdHMAMgAAAAoAAABUYWJBY3Rpb24AmQAAABcAAABUcmFuc3BhcmVudFJvd1BpY3R1cmVzADMAAAAQAAAAV3JhcENlbGxQb2ludGVyANMHAAAJAAAAX0V4dGVudFgA1AcAAAkAAABfRXh0ZW50WQAAAgAADAAAAF9MYXlvdXRUeXBlABAAAAALAAAAX1Jvd0hlaWdodAABAgAACwAAAF9TdHlsZURlZnMABAIAABYAAABfV2FzUGVyc2lzdGVkQXNQaXhlbHMA
id=TrueDBGridCtrl1
style="LEFT: 10px; POSITION: absolute; TOP: 4488px; Z-INDEX: 1002"></OBJECT>
</p>

</body>
</html>