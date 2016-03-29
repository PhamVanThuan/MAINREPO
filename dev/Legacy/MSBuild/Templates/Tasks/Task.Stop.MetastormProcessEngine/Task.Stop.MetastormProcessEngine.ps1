function PerformTask()
{
	$ServiceName = "Metastorm Process Engine"
	$Service = Get-Service $ServiceName -ErrorAction SilentlyContinue

	if ($Service)
	{
		Write-Host "The Service exists"
		Write-Host "Stopping the service"
		
		Stop-Service $ServiceName -Force | Write-Host
	}
}
