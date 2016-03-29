Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.PreRestoreScripts\Restore.PreRestoreScripts.ps1)
PerformTask;
