$toolPath = [System.Environment]::GetEnvironmentVariable("SAHL_TOOLPATH", "Machine")
[System.Environment]::SetEnvironmentVariable("SAHL_MAPSPATH", "", "Machine")

# publish all the maps
[System.Environment]::SetEnvironmentVariable("SAHL_MAPSPATH", "$OctopusOriginalPackageDirectoryPath\", "Machine")

$filter = '*.x2p'
$allMaps = @(Get-ChildItem -Path $OctopusOriginalPackageDirectoryPath -include $filter -Recurse)
$toolCmd = "$($toolPath)SAHL.Tools.Workflow.Publisher.exe"
ForEach($map in $allMaps)
{
	[String]$MapFullName = $map.FullName
	[String]$deployMode = 'true'
	$toolArgs = "-s `"$MACHINENAMEDB`" -u `"$DBUSER`" -p `"$DBPASSWORD`" -m `"$MapFullName`""
	Write-Host "$toolCmd $toolArgs"
	Invoke-Expression "$toolCmd $toolArgs" | Write-Host
}