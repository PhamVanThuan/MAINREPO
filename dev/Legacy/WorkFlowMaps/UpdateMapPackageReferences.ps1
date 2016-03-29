Param(
 [string]$x2LegacyMapsVersion,
 [string]$domainServiceBinariesVersion
)

[String]$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
[String]$parent = Split-Path -Parent $scriptDirectory

[String]$domainServiceBinaries = 'SAHL.Domainservice.Binaries'
[String]$x2LegacyMapsPackage = "SAHL.Config.Legacy.X2.Maps"
[String]$sahlnugetgallery = "http://sahldeploy/SAHLdevNuGetGallery/api/v2"
[string]$OfficialNugetSource = "https://www.nuget.org/api/v2"
[String]$nugetList = $sahlnugetgallery + "," + $OfficialNugetSource

$rootPath = Split-Path -Parent $parent
$workflowToolsPath = "$rootPath\Binaries\Tools\WorkFlow"

$PackageResolverExe = "$workflowToolsPath\SAHL.Tools.Workflow.PackageResolver.exe"
$toolsWorkflowBuilderExe = "$workflowToolsPath\SAHL.Tools.Workflow.Builder.exe"

$allMaps = @(Get-ChildItem -Path $scriptDirectory -include "*.x2p" -Recurse)

$a = new-object -ComObject wscript.shell
#$continueWithUpdate = $a.popup("This will update ALL the workflow maps to reference $x2LegacyMapsPackage $x2LegacyMapsVersion & $domainServiceBinaries $domainServiceBinariesVersion + ". Continue?", 0, "Update Map Packages and References", 4)

function Handle-Result{
	if($LASTEXITCODE -eq -1){
		Exit
	}
}

#If ($continueWithUpdate -eq 6)
#{
    $argument = "$x2LegacyMapsPackage@$x2LegacyMapsVersion,$domainServiceBinaries@$domainServiceBinariesVersion"
	$workflowMaps = [System.String]::Join(",", $allMaps)
	Write-Output ("Updating packages and related references for all maps")
	& $PackageResolverExe -m $workflowMaps -p $argument -n $nugetList | Handle-Result

	Write-Output ("Building all the maps against the updated packages and references")
	& $toolsWorkflowBuilderExe -m $workflowMaps -b $BuildServerMode | Handle-Result

#} else {
#    Exit
#}






