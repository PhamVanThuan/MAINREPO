$toolPath = [System.Environment]::GetEnvironmentVariable("SAHL_TOOLPATH", "Machine")
$MapsPath = [System.Environment]::GetEnvironmentVariable("SAHL_MAPSPATH", "Machine")

Write-Host "Recalculating Security for maps in" $MapsPath
# recalculate the security for the maps

$secureToolCmd = "$($toolPath)SAHL.Tools.Workflow.SecurityRecalculator.exe"
$allMaps = "Origination", "Life","Personal Loan","CAP2 Offers","Debt Counselling","Help Desk","Loan Adjustments"

ForEach($map in $allMaps)
{
    $buildServerMode = 'false'
	$secureToolArgs = "-m `"$map`" -d `"$buildServerMode`" -s `"$MACHINENAMEDB`" -u `"$DBUSER`" -p `"$DBPASSWORD`""
	Write-Host "$secureToolCmd $secureToolArgs"
	Invoke-Expression "$secureToolCmd $secureToolArgs" | Write-Host
}
