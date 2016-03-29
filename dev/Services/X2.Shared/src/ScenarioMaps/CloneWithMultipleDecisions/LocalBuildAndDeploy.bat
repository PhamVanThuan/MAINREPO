set currentdir=%CD%

powershell.exe -Command "&..\..\..\..\..\Build\build.ps1 'LocalBuildAndPublishMapTasks' 'RunPublishMaps' @{ 'assemblyVersion' = '0.0.0.0';'workflowMapsPath'='%currentdir%' }" -Verb RunAs

