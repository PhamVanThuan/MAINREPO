$toolPath = [System.Environment]::GetEnvironmentVariable("SAHL_TOOLPATH", "Machine")


# $MACHINENAMEDB = "DEV_03"
# $dbuser = "sa"
# $dbpassword = "W0rdpass"
# $restoreScripts = "D:\SAHL\Scripts\Data Restore\FrontEndDeveloperRestore"

if($ISREPORTSERVER)
{
    Write-Host "Report server = true"
    $dbRestores ="2am|$2AMRestorePath;e-work|$eWorkRestorePath;ImageIndex|$ImageIndexRestorePath;MetroSQL|$MetroSQLRestorePath;Process|$ProcessRestorePath;SAHLDB|$SAHLDBRestorePath;Staging|$StagingRestorePath;UIPState|$UIPStateRestorePath;Warehouse|$WarehouseRestorePath;Warehouse_Archive|$WarehouseArchiveRestorePath;X2|$X2RestorePath;"
}
else
{
    Write-Host "Report server = false"
    $dbRestores ="2am|$2AMRestorePath;e-work|$eWorkRestorePath;ImageIndex|$ImageIndexRestorePath;MetroSQL|$MetroSQLRestorePath;Process|$ProcessRestorePath;SAHLDB|$SAHLDBRestorePath;Staging|$StagingRestorePath;UIPState|$UIPStateRestorePath;Warehouse|$WarehouseRestorePath;Warehouse_Archive|$WarehouseArchiveRestorePath;X2|$X2RestorePath;DWWarehousePre|$DWWarehousePreRestorePath;DWStaging|$DWStagingRestorePath"
}

Write-Host $dbRestores

 $secureToolCmd = "& '$($toolPath)SAHL.Tools.Restorenator.exe'"
 $secureToolArgs = "-c `"Data Source=$MACHINENAMEDB; Initial Catalog=master; User Id=$dbuser; Password=$dbpassword;`" -d `"$toolPath\Scripts\`"  -r `"$dbRestores`" -p `"$backupPath`" "

Write-Host "$secureToolCmd $secureToolArgs"

Invoke-Expression "$secureToolCmd $secureToolArgs" | Write-Host


