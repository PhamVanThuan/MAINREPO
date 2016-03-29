Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Task.PreDeploy.ShutdownVM\Task.PreDeploy.ShutdownVM.ps1)
PerformTask