@echo off
set /p core="Update to which version of SAHL.Core?"
powershell.exe  %CD%\UpdateMapPackageReferences.ps1 -newCoreVersion "%core%"
pause