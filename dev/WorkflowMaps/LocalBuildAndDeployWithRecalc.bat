powershell.exe -Command "&..\Build\build.ps1 'PackageWorkflowMapTasks' 'RunBuildAndPublishWithRecalc' @{ 'assemblyVersion' = '0.0.0.0'; 'target'= 'localhost'; 'nugetFeedType' = 'localGallery' }" -Verb RunAs

