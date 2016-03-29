Param(
  [String]$BuildServerMode,
  [String]$Database,
  [Int32]$NewProcessID,
  [Int32]$OldProcessID
)

[String]$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
[String]$parent = Split-Path -Parent $scriptDirectory

$rootPath = Split-Path -Parent $parent
$workflowToolsPath = "$rootPath\Binaries\Tools\WorkFlow"

$toolsSecurityRecalculator = "$workflowToolsPath\SAHL.Tools.Workflow.SecurityRecalculator.exe"

Write-Output ("Performing Security Recalculation. OLD ProcessID: " + $OldProcessID + " NEW ProcessID: " + $NewProcessID)
& $toolsSecurityRecalculator -s $Database -u "EworkAdmin2" -p "W0rdpass" -n $NewProcessID -o $OldProcessID
