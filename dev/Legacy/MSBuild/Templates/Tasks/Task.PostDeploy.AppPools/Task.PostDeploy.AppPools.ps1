function PerformTask()
{
	Write-Host "Checking Post Deploy"
	iisreset /start | Write-Host
}