Param(
    [String]$x2LegacyMapsVersion,
    [String]$domainServiceBinaryVersion
)

$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$parent = Split-Path -Parent $scriptDirectory

$x2LegacyMapsPackageName = "SAHL.Config.Legacy.X2.Maps"
$domainServiceBinaryPackageName = 'SAHL.DomainService.Binaries'
$sahlnugetgallery = "http://sahldeploy/sahldevnugetgallery/api/v2"

$rootPath = Split-Path -Parent $parent
$packageResolverExePath = "$rootPath\Binaries\Tools\Workflow\SAHL.Tools.Workflow.PackageResolver.exe"
$mapLegacyUpdaterExe = "$rootPath\Binaries\Tools\Workflow\SAHL.Tools.Workflow.MapLegacyUpdater.exe"

$allMaps = @(Get-ChildItem -Path $scriptDirectory -include "*.x2p" -Recurse)
foreach($map in $allMaps) {
    Write-Host "Update Legacy Map Legacy Attribute - $map - true"
	& $mapLegacyUpdaterExe -m $map  -l "true"
}

Write-Output ("Updating all maps to use : $x2LegacyMapsPackageName $x2LegacyMapsVersion and $domainServiceBinaryPackageName $domainServiceBinaryVersion")

$workflowMaps = [System.String]::Join(",", $allMaps)
Write-Host "& "$packageResolverExePath" -m $workflowMaps -p "$x2LegacyMapsPackageName@$x2LegacyMapsVersion,$domainServiceBinaryPackageName@$domainServiceBinaryVersion" -n $sahlnugetgallery"
& "$packageResolverExePath" -m $workflowMaps -p "$x2LegacyMapsPackageName@$x2LegacyMapsVersion,$domainServiceBinaryPackageName@$domainServiceBinaryVersion" -n $sahlnugetgallery

exit $LASTEXITCODE
