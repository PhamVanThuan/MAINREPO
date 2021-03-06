cd Config\
$command = '.\Configure' + $OctopusEnvironmentName +'.bat'  
cmd /c $command | Write-Host

$configFileName = "SAHL.Web.Services.Endpoint.Client.config"
# now rename the valuations endpoint
IF (Test-Path ".\$configFileName")
{
	Remove-Item ".\$configFileName"
}
Rename-Item ".\SAHL.Web.Services.Valuation.Endpoint.Client.config" $configFileName -force