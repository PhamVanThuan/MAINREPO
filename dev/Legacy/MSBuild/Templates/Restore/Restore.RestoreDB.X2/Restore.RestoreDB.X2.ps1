Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "X2";
$restorePath = $X2RestorePath;

function PerformTask()
{
    RestoreDatabase $database $restorePath $backUpPath
} 
