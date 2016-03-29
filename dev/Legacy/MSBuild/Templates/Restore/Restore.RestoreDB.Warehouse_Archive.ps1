Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.Warehouse_Archive\Restore.RestoreDB.Warehouse_Archive.ps1)
PerformTask;
