Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.PostRestoreScripts\Restore.PostRestoreScripts.ps1)
PerformTask;
