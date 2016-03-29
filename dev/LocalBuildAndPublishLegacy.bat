@echo off

IF "%1" == "help" (
	goto help
)

IF "%1" == "" (
	SET task=LocalBuildAndPublishLegacy
) ELSE (
	SET task=%1
)


%~dp0Build\Tools\Invoke-Build\ib.cmd -File %~dp0Build\Parallel\legacy.Build.ps1 -Task %task% -configuration debug -assemblyVersion 0.0.0.0 -Summary


:help
echo.
echo Task 
echo ----
echo.
echo help                       -- Displays this help menu and the task list for Invoke-Build
echo default(nothing supplied)  -- runs the build in parallel
echo [taskName]                 -- Executes specified Invoke-Build task
echo.


%~dp0Build\Tools\Invoke-Build\ib.cmd -File %~dp0Build\Parallel\legacy.Build.ps1 -Task ? -configuration debug -assemblyVersion 0.0.0.0 -Summary

exit
