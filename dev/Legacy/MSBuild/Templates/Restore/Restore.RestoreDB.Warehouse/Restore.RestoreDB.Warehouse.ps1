Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "Warehouse";
$restorePath = $WarehouseRestorePath;

function PerformTask()
{
    RestoreDatabase $database $restorePath $backUpPath
} 
