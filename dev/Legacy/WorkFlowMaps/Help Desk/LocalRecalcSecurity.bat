set currentdir=%CD%

powershell.exe -Command "&..\..\..\Build\build.ps1 'PackageWorkflowMapTasks' 'RunSecurityRecalculator' @{ 'assemblyVersion' = '0.0.0.0';'workflowMapsPath'='%currentdir%' }" -Verb RunAs

