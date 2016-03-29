Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "MetroSQL";
$restorePath = $MetroSQLRestorePath;

function PerformTask()
{
    RestoreDatabase $database $restorePath $backUpPath
} 
