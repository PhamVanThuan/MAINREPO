Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.DWStaging\Restore.RestoreDB.DWStaging.ps1)
PerformTask;
