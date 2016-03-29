Param(
  [String]$mapsFolder
)

$currentPath = split-path $MyInvocation.MyCommand.Path
$rootPath = resolve-path "$currentPath\..\..\"
$toolsWorkflowBuilderExe = "$rootPath\Binaries\Tools\Workflow\SAHL.Tools.Workflow.Builder.exe"
$packageResolverExe = "$rootPath\Binaries\Tools\Workflow\SAHL.Tools.Workflow.PackageResolver.exe"
$toolsWorkflowPublisherExe = "$rootPath\Binaries\Tools\Workflow\SAHL.Tools.Workflow.Publisher.exe"
$nugetExePath = "$rootPath\Build\tools\NuGet.CommandLine.2.6.1\tools\Nuget.exe"
$assemblyVersion = "0.0.0.0"
$externalBinariesVersion = "0.0.0.0"
$externalBinariesName = "SAHL.DomainService.Binaries"
$x2LegacyMapsVersion = "0.0.0.0"
$x2LegacyMapsPackageName = "SAHL.Config.Legacy.X2.Maps"
$nugetSource = "$rootPath\DeployPackages"
$msbuildExePath = "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe"
$buildOutputTarget = "$rootPath\Legacy\BuildOutput\Target\$assemblyVersion\"
$localDatabase = "localhost"
$localMessageBusServer = "localhost"
$BuildServerMode = "false"

#$legacyMapsFolder = "$rootPath\Legacy\WorkflowMaps"
#$scenarioMapsFolder = "$rootPath\Services\X2.Shared\src\ScenarioMaps"

function UpdateMapReferences () {
    $allMaps = @(Get-ChildItem -Path "$mapsFolder" -include "*.x2p" -recurse)
    $workflowMaps = [System.String]::Join(",",$allMaps)
    & $packageResolverExe -m $workflowMaps -p "$externalBinariesName@$externalBinariesVersion,$x2LegacyMapsPackageName@$x2LegacyMapsVersion" -n "$nugetSource" 
}

function BuildMaps () {
    $allMaps = @(Get-ChildItem -Path "$mapsFolder" -include "*.x2p" -recurse)
    $workflowMaps = [System.String]::Join(",",$allMaps)
    & $toolsWorkflowBuilderExe -m "$workflowMaps" -b "$BuildServerMode" -n "$nugetSource"
}

function UpdateMapEnvironmentConfigSetting () {

    $configFiles = Get-ChildItem "$mapsFolder" -Filter "*.config" -Recurse | Select-String -pattern "sahls03", "sahls47" -list
    foreach($file in $configFiles) {
        (Get-Content $file.Path) |
            ForEach-Object {$_ -replace "sahls03", "$localDatabase"} |
            ForEach-Object {$_ -replace "sahls47", "$localMessageBusServer"} |
            Set-Content $file.path
    }
}

function PublishMaps () {
  $allMaps = @(Get-ChildItem -Path "$mapsFolder" -include "*.x2p" -recurse)
  foreach($map in $allMaps) {
    & $toolsWorkflowPublisherExe -m $map.FullName -s $localDatabase -u "EworkAdmin2" -p "W0rdpass" 
  }
}


function test (){
    Write-Host $currentPath
    Write-Host $rootPath
    Write-Host $legacyMapsFolder
}

test
UpdateMapReferences
BuildMaps
UpdateMapEnvironmentConfigSetting
PublishMaps
