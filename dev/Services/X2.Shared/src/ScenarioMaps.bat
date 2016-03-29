@echo off
set /p server="Publish to which database?"
set /p mapDB="Update map configuration to which database?"
set /p map="Map to publish? <Enter 'All' for every scenario map, or 'file.x2p' for specific map> "
set /p core="Update to which version of SAHL.Core? <Enter version number or 'Skip' if not required> "
powershell.exe  %CD%\ScenarioMaps.ps1 -publisherDatabase %server%  -mapDatabase %mapDB% -workflowmap %map% -newCoreVersion %core%
pause