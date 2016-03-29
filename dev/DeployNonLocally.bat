IF "%1" == "" (
	set searchWildCard=*
) ELSE ( 
set searchWildCard=%1
)

echo "Deploying %searchWildCard%"

powershell.exe -Command "& %~dp0\Build\Build.ps1 'packageAndDeployTasks' 'PackageAndDeployWebsites' @{  'searchWildCard' = '*%searchWildCard%*'; 'assemblyVersion' = '0.0.0.0'; 'configuration'= 'Debug';}"  -Verb RunAs 