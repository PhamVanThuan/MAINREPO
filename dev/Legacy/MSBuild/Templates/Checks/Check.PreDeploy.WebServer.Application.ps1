Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Check.PreDeploy.WebServer.Application\Check.PreDeploy.WebServer.Application.ps1)
PerformChecks