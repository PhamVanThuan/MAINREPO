Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.Variables.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.ODBC.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.Output.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.IIS.ps1)

function PerformChecks()
{
	<# BEGIN WEBSERVER EWORK CHECKS #>
	
	<# Begin ODBC Checks #>
    
	Write-Begin-Check "Starting WebServer e-work  Checks"
	$odbcConns = get-odbc-connections $OctopusMachineName

	<# E-WORK #>
	check-odbc-exists $odbcConns "e-work"
	check-odbc-property $odbcConns "e-work" "Server" $MACHINENAMEDB
	
	<# SAHLS03 #>
	check-odbc-exists $odbcConns "SAHLS03"
	check-odbc-property $odbcConns "SAHLS03" "Server" $MACHINENAMEDB
	
	<# LIGHTHOUSE #>
	check-odbc-exists $odbcConns "LightHouse"
	check-odbc-property $odbcConns "LightHouse" "Server" $MACHINENAMEDB
	
	<# METASTORM #>
	check-odbc-exists $odbcConns "Metastorm"
	check-odbc-property $odbcConns "Metastorm" "Server" $MACHINENAMEDB
	
	<# End ODBC Checks #>
	
	<# Begin IIS Checks #>
	Write-Begin-Check " Check WebServer e-work apppools"
	check-apppool-exists "MetastormExtensionsAppPool"
	check-apppool-contains-website  "MetastormExtensionsAppPool" "escripts" $NETv2

	check-apppool-exists "MetastormWebAppPool"
	check-apppool-contains-website "MetastormWebAppPool" "Metastorm" $NETv2	
	
	<# End IIS Checks #>
	
	<# END WEBSERVER EWORK CHECKS #>
}