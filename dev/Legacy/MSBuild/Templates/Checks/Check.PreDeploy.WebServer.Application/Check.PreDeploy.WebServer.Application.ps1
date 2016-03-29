Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.Variables.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.ODBC.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.Output.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.IIS.ps1)

function PerformChecks()
{
	<# BEGIN WEBSERVER APPLICATION CHECKS #>
	
	<# Begin IIS Checks #>
    
	Write-Begin-Check " Check Application WebServer apppools"
	check-apppool-exists "Halo"
	check-apppool-contains-website "Halo" "HALO" $NETv4
	
	check-apppool-exists "web.services"
	check-apppool-contains-website "web.services" "" $NETv4  
    
	<# End IIS Checks #>
    	
	<# END WEBSERVER APPLICATION CHECKS #>
}