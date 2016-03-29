


<SCRIPT ID=clientEventHandlersJS LANGUAGE=vbscript>
<!--
Sub GetScrollingMessage

	set conn1 = createobject("ADODB.Connection")
	set rs_temp  = createobject("ADODB.Recordset")
    sDSN = GetConnectionString("[ScrollMsg.inc]")
	conn1.Open sDSN
	sSQL = "Select ControlText from CONTROL where ControlDescription = 'Display Text 1' OR ControlDescription = 'Display Text 2'"
	rs_temp.CursorLocation = 3
	rs_temp.Open sSQL,conn1,adOpenDynamic
	
	window.Marquee0.innerHTML = rs_temp.Fields(0).Value + space(1)
	rs_temp.MoveLast
	window.Marquee0.innerHTML = window.Marquee0.innerHTML + rs_temp.Fields(0).Value 
	rs_temp.Close
	
	
End sub

//-->
</SCRIPT>