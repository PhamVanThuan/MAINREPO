[String]$NewMapVersion = $args
[String]$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
[String]$parent = Split-Path -Parent $scriptDirectory

$rootPath = Split-Path -Parent $parent
$workflowToolsPath = "$rootPath\Binaries\Tools\WorkFlow"

$mapVersionUpdaterExe = "$workflowToolsPath\SAHL.Tools.Workflow.MapVersionUpdater.exe"

$filter = '*.x2p'

$allMaps = @(Get-ChildItem -Path $scriptDirectory -include $filter -Recurse)

ForEach($map in $allMaps)
{
   Write-Output ("Updating " +$map+ " to version "+ $NewMapVersion)
   & $mapVersionUpdaterExe -m $map.FullName -v $NewMapVersion
}