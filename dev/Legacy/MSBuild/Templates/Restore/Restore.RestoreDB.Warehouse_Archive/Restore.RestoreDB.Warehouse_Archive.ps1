Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "Warehouse_Archive";
$restorePath = $WarehouseArchiveRestorePath;

function PerformTask()
{
    RestoreDatabase $database $restorePath $backUpPath
} 
