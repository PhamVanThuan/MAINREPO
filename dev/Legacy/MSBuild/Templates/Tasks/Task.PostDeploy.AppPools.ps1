Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Task.PostDeploy.AppPools\Task.PostDeploy.AppPools.ps1)
PerformTask