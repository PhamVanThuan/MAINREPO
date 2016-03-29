$toolPath = [System.Environment]::GetEnvironmentVariable("SAHL_TOOLPATH", "Machine")

# $MACHINENAMEDB = "DEV_03"
# $dbuser = "sa"
# $dbpassword = "W0rdpass"
# $restoreScripts = "D:\SAHL\Scripts\Data Restore\FrontEndDeveloperRestore"

 

 $secureToolCmd = "& '$($toolPath)SAHL.Tools.Scriptenator.CommandLine.exe'"
 $secureToolArgs = "-c `"Data Source=$MACHINENAMEDB; Initial Catalog=master; User Id=$dbuser; Password=$dbpassword;`" -d `"$toolPath\Scripts\`" -f `"Restore_Databases.xml`" -r `"true`""

Write-Host "$secureToolCmd $secureToolArgs"

Invoke-Expression "$secureToolCmd $secureToolArgs" | Write-Host
