Param(
  [string]$publisherDatabase,
  [string]$mapDatabase,
  [string]$workflowmap,
  [string]$newCoreVersion
)

function UpdateConfigFile([string]$newDatabase, [string]$configFilePath)
{
    $xml = [xml](get-content $configFilePath)
    $root = $xml.get_DocumentElement()
    $connectionStringNodes = [System.Xml.XmlNodeList]$xml.configuration.connectionStrings.ChildNodes
    ForEach($connString in $connectionStringNodes)
    {
        [String]$currentConnectionString = $connString.Attributes[1].Value
        [Int]$indexOfFirstEquals = $currentConnectionString.IndexOf('=')
        [Int]$indexOfFirstSemiColon = $currentConnectionString.IndexOf(';')
        [String]$currentDatabaseServer = $currentConnectionString.Substring($IndexOfFirstEquals+1, ($indexOfFirstSemiColon - $IndexOfFirstEquals)-1)
        [String]$newConnectionString = $currentConnectionString.Replace($currentDatabaseServer, $newDatabase)
        $connString.Attributes[1].Value = $newConnectionString
    }
    $xml.Save($configFilePath)
}

Write-Output ('Publish Database: '+$publisherDatabase)
Write-Output ('Map Database: '+$mapDatabase)
Write-Output ('WorkflowMap : '+$workflowmap)
Write-Output ('Core Version : '+$newCoreVersion)

[Boolean]$configFileUpdated = $false
[String]$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
[String]$parent = Split-Path -Parent $scriptDirectory
$parent = Split-Path -Parent $parent
$parent = Split-Path -Parent $parent

$NuGetCommandLine = $parent+"\Build\Tools\NuGet.CommandLine.2.6.1\tools"
$NugetExe= $NuGetCommandLine + '\Nuget.exe'

$workflowToolsPackage = 'SAHL.Tools.Workflow'
$WorkflowToolsNuGetServerUrl = "http://sahldeploy/SAHLDevNuGetGallery/api/v2"

& $NugetExe install $workflowToolsPackage -o 'C:\' -source $WorkflowToolsNuGetServerUrl

[System.Array]$matchingPackageVersions = & $NugetExe list $workflowToolsPackage -source $WorkflowToolsNuGetServerUrl
$workflowToolsPackage = $matchingPackageVersions[0]
$workflowtoolsDir = "C:\" + $workflowToolsPackage.Replace(' ', '.')

$toolsWorklowPackageResolver = $workflowtoolsDir + "\lib\SAHL.Tools.Workflow.PackageResolver.exe"
$toolsWorkflowBuilderExe = $workflowtoolsDir + "\lib\SAHL.Tools.Workflow.Builder.exe"
$toolsWorkflowPublisherExe = $workflowtoolsDir + "\lib\SAHL.Tools.Workflow.Publisher.exe"

[string]$OfficialNugetSource = "https://www.nuget.org/api/v2"
[String]$nugetList = $WorkflowToolsNuGetServerUrl + "," + $OfficialNugetSource

if ($workflowmap -eq 'All')
{
    $filter = '*.x2p'
}
else
{
    $filter = $workflowmap
}

$allMaps = @(Get-ChildItem -Path $scriptDirectory -include $filter -Recurse)

$workflowMaps = [System.String]::Join(",", $allMaps)

if ($newCoreVersion -ne "Skip"){
  Write-Output ("Updating packages and related references for all maps")
  & $toolsWorklowPackageResolver -m $workflowMaps -n $nugetList -c $newCoreVersion
}

Write-Output "Building Maps"
& $toolsWorkflowBuilderExe -m $workflowMaps -b "false"

ForEach($map in $allMaps)
{
  [String]$pathToConfigFile = $map.ToString().Replace('.x2p','')+".config"

  if ($mapDatabase.ToUpper() -ne "SAHLS03"){
      Write-Output "Updating the map configuration for the connection string setting"
      UpdateConfigFile $mapDatabase $pathToConfigFile
      $configFileUpdated = $true
  }
  Write-Output "Starting Map(s) Publish"
  & $toolsWorkflowPublisherExe -m $map.FullName -s $publisherDatabase -u "EworkAdmin2" -p "W0rdpass"
  if ($configFileUpdated)
  {
    Write-Output "Reverting the map configuration for the connection string setting back to the production setting."
  	UpdateConfigFile "sahls03" $pathToConfigFile
  }
}

Remove-Item $workflowtoolsDir -Recurse -Force