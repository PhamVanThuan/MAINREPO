Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Task.PreDeploy.MSDTC\Task.PreDeploy.MSDTC.ps1)
PerformTask