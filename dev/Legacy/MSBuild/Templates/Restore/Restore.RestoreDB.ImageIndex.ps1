Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Restore.RestoreDB.ImageIndex\Restore.RestoreDB.ImageIndex.ps1)
PerformTask;
