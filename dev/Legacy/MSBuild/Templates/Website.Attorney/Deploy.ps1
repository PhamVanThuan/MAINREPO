cd Config\
# rename the attorney endpoint
$configFileName = "SAHL.Web.Services.Endpoint.Client.config"

IF (Test-Path ".\$configFileName")
{
	Remove-Item ".\$configFileName"
}
Rename-Item ".\SAHL.Web.Services.Attorney.Endpoint.Client.config.$OctopusEnvironmentName" $configFileName