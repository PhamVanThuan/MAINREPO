Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Check.PreDeploy.Database\Check.PreDeploy.Database.ps1)
PerformChecks