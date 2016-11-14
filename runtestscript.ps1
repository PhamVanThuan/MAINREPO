$parentFolder = 'Testing\SAHL.Testing.Services'

.\Build\build.ps1 'CodeTestsTasks' 'ServicesEndToEndTests' @{'buildOutputDir' = 'buildoutput'; 'parentFolder' = $parentFolder
'configuration' = 'Release'; 'reportCoverage'=$true; 'coverageFilters'='+[*]* '; 
'excludeIntegrationTests'='False';'environment'='SYSA';'dirNameSuffix'='DomainProcessManager.Tests';} 'continue' 

