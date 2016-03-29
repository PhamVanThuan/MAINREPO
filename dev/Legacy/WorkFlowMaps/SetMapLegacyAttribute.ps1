
$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$parent = Split-Path -Parent $scriptDirectory
$rootPath = Split-Path -Parent $parent
$mapLegacyUpdaterExe = "$rootPath\Binaries\Tools\Workflow\SAHL.Tools.Workflow.MapLegacyUpdater.exe"

$allMaps = @(Get-ChildItem -Path $scriptDirectory -include "*.x2p" -Recurse)
foreach($map in $allMaps) {
    Write-Host "Update Legacy Map Legacy Attribute - $map - true"
	& $mapLegacyUpdaterExe -m $map  -l "true"
}

exit $LASTEXITCODE
