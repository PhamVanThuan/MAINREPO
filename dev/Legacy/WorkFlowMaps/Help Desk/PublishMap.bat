@echo off
set /p server="Publish to which database?"
powershell.exe ..\BuildAndPublishWorkingMaps.ps1 -database "%server%" -MapToBuild 'Help Desk'
pause