Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.MetroSQL\Restore.RestoreDB.MetroSQL.ps1)
PerformTask;
