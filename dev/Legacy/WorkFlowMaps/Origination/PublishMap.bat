cd..
@echo off
set /p server="Publish to which database?"
powershell.exe %cd%\BuildAndPublishWorkingMaps.ps1 -database "%server%" -MapToBuild 'Origination'
pause