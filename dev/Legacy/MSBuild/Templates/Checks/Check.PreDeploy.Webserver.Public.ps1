Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Check.PreDeploy.WebServer.Public\Check.PreDeploy.WebServer.Public.ps1)
PerformChecks