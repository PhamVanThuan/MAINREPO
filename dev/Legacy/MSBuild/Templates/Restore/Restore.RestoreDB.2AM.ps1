Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.2AM\Restore.RestoreDB.2AM.ps1)
PerformTask;
