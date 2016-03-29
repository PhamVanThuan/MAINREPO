powershell.exe -Command "& %~dp0\Build\Build.ps1 'packageAndDeployTasks' 'PackageAndDeployWebsites' @{  'searchWildCard' = '*';'assemblyVersion' = '0.0.0.0'; 'configuration'= 'Debug';localBuild = $true}"  -Verb RunAs




