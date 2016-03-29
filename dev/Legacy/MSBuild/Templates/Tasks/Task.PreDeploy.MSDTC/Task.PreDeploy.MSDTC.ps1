function PerformTask()
{
	$ServiceName = "MSDTC"
	$Service = Get-Service $ServiceName -ErrorAction SilentlyContinue

	if ($Service)
	{
		Write-Host "The Service exists"
		Write-Host "Stopping the service"
		
		Stop-Service $ServiceName -Force | Write-Host
	}
	else
	{
		Write-Host "The Service should exist"
	}
}
