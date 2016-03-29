Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Restore.Common\Common.Restore.ps1)

$database = "DWWarehousePre";
$restorePath = $DWWarehousePreRestorePath;

function PerformTask()
{
	if($ISREPORTSERVER)
	{
		RestoreDatabase $database $restorePath $backUpPath
	}
} 
