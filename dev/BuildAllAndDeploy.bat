@echo off

IF "%1" == "psake" (
	IF "%2" == "" (
	SET task=BuildAndDeploy 
	SET wildcard=*
) 
	goto psake
) 
IF "%1" == "help" (
	goto help
)
IF "%1" == "" (
	SET task=BuildAndDeploy 
	SET wildcard=*
)
goto invoke-build

exit

:psake
echo Running %task% Task
powershell.exe -Command "& %~dp0\Build\Build.ps1 'packageAndDeployTasks' '%task%' @{ 'searchWildCard' = '%wildcard%'; 'assemblyVersion' = '0.0.0.0'; 'configuration'= 'Debug';localBuild = $true}"  -Verb RunAs
exit

:invoke-build
echo Running %task% Task
%~dp0Build\Tools\Invoke-Build\ib.cmd -File %~dp0Build\Parallel\PackageAndDeploy.Build.ps1 -Task %task% -searchWildCard %wildcard% -configuration debug -assemblyVersion 0.0.0.0
exit

:help
echo.
echo Task 
echo ----
echo.
echo help                       -- Displays this help menu and the task list for Invoke-Build
echo default(nothing supplied)  -- Builds and Deploy's everything
rem echo [wildcard]                 -- Builds and Deploy's service's and websites that match the wildcard
echo psake ?                    -- Display the task list for psake
echo psake                      -- Executes the deploy task of the original psake build scrpts
rem echo psake [wildcard]           -- Deploy service's and websites that match the wildcard
echo.
%~dp0Build\Tools\Invoke-Build\ib.cmd -File %~dp0Build\Parallel\PackageAndDeploy.Build.ps1 -Task ? -searchWildCard * -configuration debug -assemblyVersion 0.0.0.0 -Summary
exit
