%~dp0..\Build\Tools\Invoke-Build\ib.cmd -File %~dp0..\Build\parallel\WorkflowMaps.package.Build.ps1 -Task RunBuildAndPackage -configuration debug -assemblyVersion 0.0.0.0 -target localhost -nugetFeedType localGallery -Summary

