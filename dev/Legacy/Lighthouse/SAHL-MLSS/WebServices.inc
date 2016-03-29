<script language="VBScript">

' *********************************************************************************************************
' This function creates an HTTP Object (Client side) and sends
' *********************************************************************************************************
function SendHTTP( url )
	Set Http = CreateObject( "Microsoft.XMLHTTP" )
	Http.Open "GET", url, False
	Http.Send ""
	SendHTTP = Http.ResponseText
	set Http = nothing
end function

' *********************************************************************************************************
' This function removes the xml encoding from the returned value without creating a XML Document. The 
' better way is to create an xml document
' *********************************************************************************************************
function ClearReturn( retval )
	retval = replace( retval, "<string xmlns=""http://services.sahomeloans.com/sahlservices/"">", "" )
	retval = replace( retval, "<?xml version=""1.0"" encoding=""utf-8""?>", "" )
	retval = replace( retval, "</string>", "" )
	retval = replace( retval, vbCrLf, "" )
	ClearReturn = lcase( trim( retval ) )
end function

' *********************************************************************************************************
' This function checks and makes sure that the ID that you send is valid
' Call it with an ID Number and it will return true if valid or false if invalid
' *********************************************************************************************************
function CheckID( number )
	if len( number ) <> 13 then
		CheckID = false
		exit function
	end if

	if not isnumeric( number ) then
		CheckID = false
		exit function
	end if
	
	dim retval	
	retval = ClearReturn( SendHTTP( "http://sahls16/sahlservices/methods.asmx/ValidateID?IdNumber=" & number ) )
		
	if strcomp( cstr( retval ), "true" ) = 0 then
		CheckID = true
	else
		CheckID = false
	end if

	set Http = nothing
end function

' *********************************************************************************************************
' The following function will call a rule and return the result of that rule
' rulename is the name of the rule to be run
' parameternames is a comma delimeted list of parameters that you need to execute the rule
' parametervalues is a comma delimeted list of values in the same order as the names that specify the values
'    for the names- eg. parameternames = "ProspectNumber,Name", parametervalues = "1234567,Joe Blocks"
' *********************************************************************************************************
function RunRule( rulename, parameternames, parametervalues )
	RunRule = SendHTTP( "http://sahls11/qrule/services.asmx/RunRule?RuleName=" & rulename & "&ParameterNames=" & parameternames & "&ParameterValues=" & parametervalues )
end function

</script>