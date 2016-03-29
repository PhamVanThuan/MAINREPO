set currentdir=%CD%

powershell.exe -Command "&..\..\Build\build.ps1 'PackageWorkflowMapTasks' 'RunBuild' @{ 'assemblyVersion' = '0.0.0.0'; 'target'= 'localhost'; 'nugetFeedType' = 'localGallery'; 'workflowMapsPath'='%currentdir%' }" -Verb RunAs

