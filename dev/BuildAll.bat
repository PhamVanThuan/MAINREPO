@echo off

IF "%1" == "psake" (
	IF "%2" == "" (
		SET task=default
		) ELSE (
		SET task=%2
		)
	goto psake
) 
IF "%1" == "help" (
	goto help
)
IF "%1" == "" (
	SET task=Parallel 
	SET searchWildCard=*
) ELSE (
	SET task=%1
	SET searchWildCard=*%2*
)
goto invoke-build

exit

:psake
echo Running %task% Task
powershell.exe -Command "& %~dp0\Build\Build.ps1 'buildTasks' '%task%' @{ 'searchWildCard' = '*'; 'assemblyVersion' = '0.0.0.0'; 'configuration'= 'Debug';localBuild = $true}"  -Verb RunAs
exit

:invoke-build
echo Running %task% Task
%~dp0Build\Tools\Invoke-Build\ib.cmd -File %~dp0Build\Parallel\default.Build.ps1 -Task %task% -searchWildCard %searchWildCard% -configuration debug -assemblyVersion 0.0.0.0  -localBuild $true 
exit

:help
echo.
echo Task 
echo ----
echo.
echo help                       -- Displays this help menu and the task list for Invoke-Build
echo default(nothing supplied)  -- runs the build in parallel
echo [taskName]                 -- Executes specified Invoke-Build task
echo psake ?                    -- Display the task list for psake
echo psake                      -- Executes the default task of the original psake build scrpts
echo psake [taskName]           -- Executes specified psake task
echo.


%~dp0Build\Tools\Invoke-Build\ib.cmd -File %~dp0Build\Parallel\default.Build.ps1 -Task ? -searchWildCard * -configuration debug -assemblyVersion 0.0.0.0 -Summary
exit
