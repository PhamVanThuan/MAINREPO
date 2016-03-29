powershell.exe -Command "& %~dp0\Build\Build.ps1 'CodeTestsTasks' 'Default' @{ 'ParentFolder' = '.\Services\DomainProcessManager' ; 'buildOutputDir'='buildoutput'; 'configuration'= 'Release'; 'reportCoverage'=$true; 'coverageFilters'='+[SAHL.*]SAHL.*  -[*]*.Specs* -[*]*.Tests* -[JetBrains*]* -[*]*.Console*'; 'coverageOutputDir'='Coverage'; 'reportOutputDir' = 'Coverage'}"  -Verb RunAs
 


pause