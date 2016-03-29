Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.DWWarehousePre\Restore.RestoreDB.DWWarehousePre.ps1)
PerformTask;
