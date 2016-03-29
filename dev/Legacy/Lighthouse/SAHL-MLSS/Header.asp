
<%
'********************************************************************
' Name: Header.asp
'
' Purpose: Header include file used to start all pages
'   also includes global functions
'
'********************************************************************
  sDatabase =Session("SQLDatabase"   )
function GetDataConnection()
   dim oConn, strConn
   Set oConn = Server.CreateObject("ADODB.Connection")
   strConn = "Provider=SQLOLEDB;Data Source=" & sDatabase &  "; Initial Catalog=SAHLDB; "
   strConn = strConn & "User Id=crystal"
   oConn.Open strConn
   set GetDataConnection = oConn
end function    
  
%>


