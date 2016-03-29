$fullPathIncFileName = $MyInvocation.MyCommand.Definition
$currentScriptName = $MyInvocation.MyCommand.Name
$rootPath = $fullPathIncFileName.Replace($currentScriptName, "")

$toolPath = "$rootPath\Tools\SAHL.Tools\SAHL.Tools.Reportenator.CommandLine\bin\Debug\"
$REPORTSERVERDB = "deva15"
$REPORTSERVER_UserName = "sqlservice2"
$REPORTSERVER_Password = "W0rdpass"
$REPORTSERVER_Domain = "SAHL"
$OctopusPackageDirectoryPath = "$rootPath\Reports\"
$toolCmd = "$($toolPath)SAHL.Tools.Reportenator.CommandLine.exe"
$toolArgs = "-s $REPORTSERVERDB -u ""$REPORTSERVER_UserName"" -p ""$REPORTSERVER_Password"" -o ""$REPORTSERVER_Domain"" -d ""$OctopusPackageDirectoryPath"" -f ""reportenator.xml"""
Invoke-Expression "$toolCmd $toolArgs" | Write-Host
