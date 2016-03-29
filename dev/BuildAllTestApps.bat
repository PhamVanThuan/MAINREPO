@echo off

SET task=PackageAndDeployTestApps 
SET wildcard=*

goto invoke-build

exit

:invoke-build
echo Running %task% Task
%~dp0Build\Tools\Invoke-Build\ib.cmd -File %~dp0Build\Parallel\PackageAndDeploy.Build.ps1 -Task %task% -searchWildCard %wildcard% -configuration debug -assemblyVersion 0.0.0.0
exit