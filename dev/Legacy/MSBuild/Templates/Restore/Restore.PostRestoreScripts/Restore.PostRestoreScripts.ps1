Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

function PerformTask()
{
	$toolPath = [System.Environment]::GetEnvironmentVariable("SAHL_TOOLPATH", "Machine")
	$secureToolCmd = "& '$($toolPath)SAHL.Tools.Scriptenator.CommandLine.exe'"
	$secureToolArgs = " -c `"Data Source=$MACHINENAMEDB; Initial Catalog=master; User Id=$dbuser; Password=$dbpassword;`" -d `"$toolPath\Scripts\`" -f `"Post_Restore_Scripts.xml`""

	Write-Host "$secureToolCmd $secureToolArgs"
	Invoke-Expression "$secureToolCmd $secureToolArgs" | Write-Host
}