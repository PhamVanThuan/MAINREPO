Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.SAHLDB\Restore.RestoreDB.SAHLDB.ps1)
PerformTask;
