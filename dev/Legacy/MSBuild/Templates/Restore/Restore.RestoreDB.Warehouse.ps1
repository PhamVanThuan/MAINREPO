Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.Warehouse\Restore.RestoreDB.Warehouse.ps1)
PerformTask;
