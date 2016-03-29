Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.UIP\Restore.RestoreDB.UIP.ps1)
PerformTask;
