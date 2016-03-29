Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Task.PreDeploy.StopSqlServer\Task.PreDeploy.StopSqlServer.ps1)
PerformTask