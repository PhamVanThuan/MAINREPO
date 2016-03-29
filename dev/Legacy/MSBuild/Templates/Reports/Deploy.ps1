$toolPath = [System.Environment]::GetEnvironmentVariable("SAHL_TOOLPATH", "Machine")
$toolCmd = "$($toolPath)SAHL.Tools.Reportenator.CommandLine.exe"
$toolArgs = "-s $REPORTSERVERDB -u ""$REPORTSERVER_UserName"" -p ""$REPORTSERVER_Password"" -o ""$REPORTSERVER_Domain"" -d ""$OctopusOriginalPackageDirectoryPath"" -f ""reportenator.xml"""
Invoke-Expression "$toolCmd $toolArgs" | Write-Host
