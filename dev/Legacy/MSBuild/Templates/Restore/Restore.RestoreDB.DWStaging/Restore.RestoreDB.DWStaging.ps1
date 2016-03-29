Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "DWStaging";
$restorePath = $DWStagingRestorePath;

function PerformTask()
{
	if($ISREPORTSERVER)
	{
		RestoreDatabase $database $restorePath $backUpPath
	}
} 
