
#$BACKUPSOURCEPATH = "\\sahls36\SQL Backups\Daily"
#$BACKUPPATH = "\\sahlm02\Backup\Daily"

#$BACKUPSOURCEPATH = "E:\Development\Miscellaneous\RestoreScripts\Daily"
#$BACKUPPATH = "E:\Development\Miscellaneous\RestoreScripts\dest"

Write-Host "Start Copy"

Write-Host "Copy 2AM - $((Get-Date).ToString("G"))"
copy -Path "$BACKUPSOURCEPATH\2AM.bak" -Destination "$BACKUPPATH\2AM.bak" 

Write-Host "Copy EWork - $((Get-Date).ToString("G"))"
copy -Path "$BACKUPSOURCEPATH\EWork.bak" -Destination "$BACKUPPATH\EWork.bak" 

Write-Host "Copy X2 - $((Get-Date).ToString("G"))"
copy -Path "$BACKUPSOURCEPATH\X2.bak" -Destination "$BACKUPPATH\X2.bak" 

Write-Host "Copy Sahldb - $((Get-Date).ToString("G"))"
copy -Path "$BACKUPSOURCEPATH\Sahldb.bak" -Destination "$BACKUPPATH\Sahldb.bak" 

Write-Host "Copy Process - $((Get-Date).ToString("G"))"
copy -Path "$BACKUPSOURCEPATH\Process.bak" -Destination "$BACKUPPATH\Process.bak" 

Write-Host "Copy UIPState - $((Get-Date).ToString("G"))"
copy -Path "$BACKUPSOURCEPATH\UIPState.bak" -Destination "$BACKUPPATH\UIPState.bak" 

Write-Host "Copy METROSQL - $((Get-Date).ToString("G"))"
copy -Path "$BACKUPSOURCEPATH\METROSQL.bak" -Destination "$BACKUPPATH\METROSQL.bak" 

Write-Host "Copy ImageIndex - $((Get-Date).ToString("G"))"
copy -Path "$BACKUPSOURCEPATH\ImageIndex.bak" -Destination "$BACKUPPATH\ImageIndex.bak" 

Write-Host "Copy DWWarehousePre - $((Get-Date).ToString("G"))"
copy -Path "$BACKUPSOURCEPATH\DWWarehousePre.bak" -Destination "$BACKUPPATH\DWWarehousePre.bak" 

Write-Host "Copy DWStaging - $((Get-Date).ToString("G"))"
copy -Path "$BACKUPSOURCEPATH\DWStaging.bak" -Destination "$BACKUPPATH\DWStaging.bak" 

Write-Host "Copy Complete - $((Get-Date).ToString("G"))"

