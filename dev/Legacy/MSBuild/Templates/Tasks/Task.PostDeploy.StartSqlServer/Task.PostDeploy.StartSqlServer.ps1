function PerformTask()
{
	$ServiceName = "MSSQLSERVER"
	$Service = Get-Service $ServiceName -ErrorAction SilentlyContinue

	if ($Service)
	{
		Write-Host "Starting the service"
		Start-Service $ServiceName | Write-Host
	}
	else
	{
		Write-Error "The Service : $ServiceName does not exist"
	}
}