function PerformTask()
{
	Write-Host "Checking Pre Deploy"
	iisreset /stop | Write-Host
}