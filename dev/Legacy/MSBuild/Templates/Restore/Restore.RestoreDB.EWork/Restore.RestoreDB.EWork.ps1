Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "e-work";
$restorePath = $eWorkRestorePath;

function PerformTask()
{
    RestoreDatabase $database $restorePath $backUpPath
} 
