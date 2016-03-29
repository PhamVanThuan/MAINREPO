IF "%1" == "" (
	SET ParentFolder=Websites
) ELSE ( 
SET ParentFolder=%1
)

echo %ParentFolder%

powershell.exe -Command "& %~dp0\Build\Build.ps1 'CodeTestsTasks' 'default' @{ 'ParentFolder' = '%ParentFolder%' ; 'buildOutputDir'='buildoutput'; 'configuration'= 'Release'; 'reportCoverage'=$true; 'coverageFilters'='+[SAHL.*]SAHL.*  -[*]*.Specs* -[*]*.Tests* -[JetBrains*]* -[*]*.Console*'; 'coverageOutputDir'='Coverage'; 'reportOutputDir' = 'Coverage'}"  -Verb RunAs
 

pause