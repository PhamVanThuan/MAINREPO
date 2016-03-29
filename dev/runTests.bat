IF "%1" == "" (
	SET task=default
) ELSE ( 
SET task=%1
)

echo %task%


powershell.exe -Command "& %~dp0\Build\Build.ps1 'CodeTestsTasks' '%task%' @{ 'ParentFolder' = '' ; 'buildOutputDir'='buildoutput'; 'configuration'= 'Release'; 'reportCoverage'=$true; 'coverageFilters'='+[SAHL.*]SAHL.*  -[*]*.Specs* -[*]*.Tests* -[JetBrains*]* -[*]*.Console*'; 'coverageOutputDir'='Coverage'; 'reportOutputDir' = 'Coverage'}"  -Verb RunAs
 


pause