@echo off
set /p server="Recalculate security on which database?"
set /p oldprocess="Old ProcessID?"
set /p newprocess="New ProcessID?"

powershell.exe  %CD%\RecalculateSecurity_Specific_Process.ps1 -database "%server%" -newprocessid %newprocess% -oldprocessid %oldprocess% 
pause