Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "UIPState";
$restorePath = $UIPStateRestorePath;

function PerformTask()
{
    RestoreDatabase $database $restorePath $backUpPath
} 
