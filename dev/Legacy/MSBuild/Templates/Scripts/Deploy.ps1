$toolPath = [System.Environment]::GetEnvironmentVariable("SAHL_TOOLPATH", "Machine")
$toolCmd = "$($toolPath)SAHL.Tools.Scriptenator.CommandLine.exe"
$toolArgs = "-c ""Data Source=$MACHINENAMEDB;Initial Catalog=2AM;Persist Security Info=True;User ID=eworkadmin2;Password=W0rdpass;"" -d ""$OctopusOriginalPackageDirectoryPath"" -f ""Scriptenator.xml"""
Invoke-Expression "$toolCmd $toolArgs" | Write-Host