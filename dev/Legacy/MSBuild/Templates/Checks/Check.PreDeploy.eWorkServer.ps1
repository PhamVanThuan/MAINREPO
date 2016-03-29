Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Check.PreDeploy.eWorkServer\Check.PreDeploy.eWorkServer.ps1)
$OctopusMachineName = "SAHLS218A"
$MACHINENAMEDB = "SAHLS203A"
PerformChecks