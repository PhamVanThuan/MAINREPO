Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.Process\Restore.RestoreDB.Process.ps1)
PerformTask;
