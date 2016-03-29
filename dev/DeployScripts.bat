@echo off

SET dbSourceServer=%1

IF [%1] == [] (goto error)
IF "%1" == "help" (goto help)


SET task=DeployScripts
SET localDeploy=false
IF "%2" == "03" ( SET task=Deploy03Scripts) 
IF "%2" == "15" ( SET task=Deploy15Scripts) 
IF "%2" == "CDB01" ( SET task=DeployCDB01Scripts) 
IF "%2" == "local" ( SET localDeploy=true)
IF "%3" == "local" ( SET localDeploy=true)

goto invoke-build

:psake
echo Running %task% Task
powershell.exe -Command "& %~dp0\Build\Build.ps1 'deployTasks' '%task%' @{ 'dbSourceServer' = '%dbSourceServer%'; localDeploy = $%localDeploy% }"  -Verb RunAs
exit

:invoke-build
echo Running %task% Task
%~dp0Build\Tools\Invoke-Build\ib.cmd -File %~dp0Build\Parallel\deploy.Build.ps1 -Task %task% -dbSourceServer '%dbSourceServer%' -localDeploy $%localDeploy%
exit

:error
echo.
%Windir%\System32\WindowsPowerShell\v1.0\Powershell.exe write-host -foregroundcolor Red "Please provide mandatory parameters [dbSourceServer]"

:help
echo.
echo Task 
echo ----
echo.
echo help			-- Displays this help menu
echo.
echo DeployScripts.bat [dbSourceServer] [database] 
echo ------------------------------------------------------------
echo.
echo [dbSourceServer]  -- mandatory	- name of the enviroment e.g deva
echo [server suffix]   -- optional   - name of server e.g. 03,15,CDB01
echo [local]           -- optional   - flag to determine if scripts should be deployed to a local database
echo.
echo (e.g. DeployScripts.bat deva 03)
echo (e.g. DeployScripts.bat localhost 03 local)
echo (e.g. DeployScripts.bat localhost local)
echo.
exit


