Param(
  [String]$BuildServerMode,
  [String]$Database,
  [String]$MapToRecalculateSecurityOn
)

[String]$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
[String]$parent = Split-Path -Parent $scriptDirectory

$rootPath = Split-Path -Parent $parent
$workflowToolsPath = "$rootPath\Binaries\Tools\WorkFlow"

$toolsSecurityRecalculator = "$workflowToolsPath\SAHL.Tools.Workflow.SecurityRecalculator.exe"

$filter = '*.x2p'
if ($MapToRecalculateSecurityOn -ne 'All')
{
    $filter = $MapToRecalculateSecurityOn+'.x2p'
}

$allMaps = @(Get-ChildItem -Path $scriptDirectory -include $filter -Recurse)

ForEach($map in $allMaps)
{
   Write-Output ("Performing Security Recalculation on " + $map.BaseName)
   & $toolsSecurityRecalculator -m $map.BaseName -d $BuildServerMode -s $Database -u "EworkAdmin2" -p "W0rdpass"
}