Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.Staging\Restore.RestoreDB.Staging.ps1)
PerformTask;
