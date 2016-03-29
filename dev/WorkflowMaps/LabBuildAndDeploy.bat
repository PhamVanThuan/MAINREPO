@echo off

set /p server="Target Database Environment? (LOCALHOST, DEVA, DEVB,..., SYSA, SYSB,...)  "
set /p config_version="Version of SAHL.Config.WorkflowMaps? (0.1.6.0-Dev)  "
%~dp0..\Build\Tools\Invoke-Build\ib.cmd -File %~dp0..\Build\Parallel\WorkflowMaps.Package.Build.ps1 -Task RunPublish -configuration debug -assemblyVersion 0.0.0.0  -config_version '%config_version%' -target '%server%' -nugetFeedType devGallery  -Summary