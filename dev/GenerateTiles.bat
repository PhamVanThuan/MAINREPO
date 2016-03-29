powershell.exe -Command "& %~dp0\Build\Build.ps1 'buildTasks' 'GeneralTools' @{ 'searchWildCard' = '*'; 'assemblyVersion' = '0.0.0.0'; 'configuration'= 'Debug';localBuild = $true}"  -Verb RunAs

powershell.exe -Command "& %~dp0\Build\Build.ps1 'codeGenerationTasks' 'GenerateHaloTileViews' @{ 'searchWildCard' = '*'; 'assemblyVersion' = '0.0.0.0'; 'configuration'= 'Debug';localBuild = $true}"  -Verb RunAs

.\runGulpFile.bat
