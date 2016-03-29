Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "2am";
$restorePath = $2AMRestorePath;

function PerformTask()
{
    RestoreDatabase $database $restorePath $backUpPath
} 

