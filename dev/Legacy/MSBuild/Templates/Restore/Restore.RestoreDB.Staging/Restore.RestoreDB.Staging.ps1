Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "Staging";
$restorePath = $StagingRestorePath;

function PerformTask()
{
    RestoreDatabase $database $restorePath $backUpPath
} 
