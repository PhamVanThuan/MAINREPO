<%

'This is the new function that should be used on the SERVER side to check
'for the correct date format 

 Function CheckDateFormat

  CheckDateFormat = "U" 

  d = formatdatetime("31/12/2000",2)
  
  if Mid(CSTR(d),1,2) = "31" then
	  CheckDateFormat = "S" 'i.e. 31/12/2000 - SA
  elseif Mid(CSTR(d),1,2) = "12" then
       CheckDateFormat = "A" 'i.e. 12/31/2000 - USA
  else
       CheckDateFormat = "U" 'ie. probabably 2002/12/31 or 2000/31/12
  End If
 
 End Function

%>
<SCRIPT ID=clientEventHandlersVBS LANGUAGE=vbscript>
<!--
'This function has been retained as various system pages may still use it..
Function DetermineWorksStationDateFormat()

  d = formatdatetime("31/12/2000",2)
  'msgbox Cstr(d)
  'msgbox Cstr(month(d))
  i = instr(1,Cstr(d),Cstr(month(d)),1)
  
  if i = 1 then
     DetermineWorksStationDateFormat = "A"
     
  elseif i = 4 then
      DetermineWorksStationDateFormat = "S"
      
  else
      DetermineWorksStationDateFormat = "U"
  end if
  
 End function

'This is the new function that should be used on the CLIENT side to check
'for the correct date format 

 Function CheckDateFormat

  CheckDateFormat = "U" 

  d = formatdatetime("31/12/2000",2)
  
  if Mid(CSTR(d),1,2) = "31" then
	  CheckDateFormat = "S" 'i.e. 31/12/2000 - SA
  elseif Mid(CSTR(d),1,2) = "12" then
       CheckDateFormat = "A" 'i.e. 12/31/2000 - USA
  else
       CheckDateFormat = "U" 'ie. probabably 2002/12/31 or 2000/31/12
  End If
  
 
 End Function

-->
</SCRIPT>

