%~dp0..\Build\Tools\Invoke-Build\ib.cmd -File %~dp0..\Build\Parallel\WorkflowMaps.package.Build.ps1 -Task RunBuildAndPublish -configuration debug -assemblyVersion 0.0.0.0 -target localhost -nugetFeedType localGallery -Summary

