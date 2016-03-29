powershell.exe -Command "& %~dp0\Build\Build.ps1 'buildTasks' 'Web' @{  'assemblyVersion' = '0.0.0.0'; 'configuration'= 'Debug'} 'continue'"  -Verb RunAs 

powershell.exe -Command "& %~dp0\Build\Build.ps1 'codeGenerationTasks' 'GenerateHaloTileViews' @{  'assemblyVersion' = '0.0.0.0'; 'configuration'= 'Debug'} 'continue'"  -Verb RunAs 

powershell.exe -Command "& %~dp0\Build\Build.ps1 'packageAndDeployTasks' 'Deploy' @{  'searchWildCard' = '*Halo*'; 'assemblyVersion' = '0.0.0.0'; 'configuration'= 'Debug'} 'continue'"  -Verb RunAs

gulp --gulpfile .\Build\NodeJS\gulpfile.js