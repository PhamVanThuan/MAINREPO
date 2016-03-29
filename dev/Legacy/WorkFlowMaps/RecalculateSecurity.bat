@echo off
set /p server="Recalculate security on which database?"
powershell.exe  %CD%\RecalculateSecurity.ps1 -database "%server%" -MapToRecalculateSecurityOn 'All'
pause