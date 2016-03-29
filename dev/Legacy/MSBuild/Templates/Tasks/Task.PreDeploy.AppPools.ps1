Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Task.PreDeploy.AppPools\Task.PreDeploy.AppPools.ps1)
PerformTask