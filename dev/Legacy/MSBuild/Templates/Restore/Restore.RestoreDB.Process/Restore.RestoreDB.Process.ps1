Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "Process";
$restorePath = $ProcessRestorePath;

function PerformTask()
{
    RestoreDatabase $database $restorePath $backUpPath
} 
