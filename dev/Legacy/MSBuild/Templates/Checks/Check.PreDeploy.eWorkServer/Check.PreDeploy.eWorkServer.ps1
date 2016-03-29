Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.Variables.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.ODBC.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.Output.ps1)

function PerformChecks()
{
	<# BEGIN EWORKSERVER CHECKS #>
	
	Write-Begin-Check "Starting eWork Server Checks"
	$odbcConns = get-odbc-connections $OctopusMachineName

	<# 2AM #>
	check-odbc-exists $odbcConns "2AM"
	check-odbc-property $odbcConns "2AM" "Server" $MACHINENAMEDB
	check-odbc-property $odbcConns "2AM" "Database" "2AM"
	
	<# E-WORK #>
	check-odbc-exists $odbcConns "e-work"
	check-odbc-property $odbcConns "e-work" "Server" $MACHINENAMEDB
	
	<# SAHLS03 #>
	check-odbc-exists $odbcConns "SAHLS03"
	check-odbc-property $odbcConns "SAHLS03" "Server" $MACHINENAMEDB
	check-odbc-property $odbcConns "SAHLS03" "Database" "SAHLDB"
	
	<# LIGHTHOUSE #>
	check-odbc-exists $odbcConns "LightHouse"
	check-odbc-property $odbcConns "LightHouse" "Server" $MACHINENAMEDB
	check-odbc-property $odbcConns "LightHouse" "Database" "SAHLDB"		
	
	<# METASTORM #>
	check-odbc-exists $odbcConns "Metastorm"
	check-odbc-property $odbcConns "Metastorm" "Server" $MACHINENAMEDB
	
	<# END EWORKSERVER CHECKS #>
}