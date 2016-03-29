Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.X2\Restore.RestoreDB.X2.ps1)
PerformTask;
