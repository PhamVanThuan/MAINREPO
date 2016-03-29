@echo off

IF [%1] == [] (goto error)
IF [%2] == [] (goto error)
IF "%1" == "help" (goto help)
IF NOT [%3] == [] (goto schemas)

%~dp0Build\Tools\Invoke-Build\ib.cmd -File %~dp0Build\Parallel\CodeGeneration.build.ps1 -Task DataModelGeneration -dbSourceServer %1 -properties @{ 'database' = '%2'}
exit

:schemas
%~dp0Build\Tools\Invoke-Build\ib.cmd -File %~dp0Build\Parallel\CodeGeneration.build.ps1 -Task DataModelGeneration -dbSourceServer %1 -properties @{ 'database' = '%2'; 'schemas' = '%3' }
exit

:error
echo.
%Windir%\System32\WindowsPowerShell\v1.0\Powershell.exe write-host -foregroundcolor Red "Please provide mandatory parameters [dbSourceServer] and [database]"

:help
echo.
echo Task 
echo ----
echo.
echo help			-- Displays this help menu
echo.
echo GenerateDataModels.bat [dbSourceServer] [database] [schemas]
echo ------------------------------------------------------------
echo.
echo [dbSourceServer]	-- mandatory	- name of the server the database is hosted on
echo [database]		-- mandatory	- name of database
echo [schemas]		-- optional	- an inverted-comma-bound, comma delimited list of schemas to include 
echo.
echo (e.g. GenerateDataModels.bat deva03 2am """dbo,debtcounselling""")
echo.
exit

