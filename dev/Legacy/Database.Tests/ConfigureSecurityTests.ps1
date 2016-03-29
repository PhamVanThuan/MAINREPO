
$ToolPath = [System.Environment]::GetEnvironmentVariable("SAHL_TOOLPATH", "Machine")
$ToolPath=$ToolPath.Substring(0, $ToolPath.Length-13)
#Write-Host $ToolPath
$scriptPath = $ToolPath.Substring(0, $ToolPath.Length-17)+"Database.tests\Scripts"
Write-Host $scriptPath
$toolBinPath = "\Tool Binaries\"
$tmpPath = $ToolPath+"temp"
$cmd = "mkdir " + """"+$tmpPath+""""
#Write-Host $cmd
Invoke-Expression $cmd | Write-Host


$cmd = "copy " + """"+$ToolPath+$toolBinPath+"*.*"" ""$tmpPath"""
#Write-Host $cmd
Invoke-Expression $cmd | Write-Host

$MACHINENAMEDB = "Sysa03"
$scriptenatorPath = $tmpPath+"\SAHL.Tools.Scriptenator.CommandLine.exe -c ""Data Source=$MACHINENAMEDB;Initial Catalog=2AM;Persist Security Info=True;User ID=eworkadmin2;Password=W0rdpass;"" -d ""$scriptPath"" -f ""Scriptenator.xml"""
Write-Host $scriptenatorPath 
Invoke-Expression $scriptenatorPath | Write-Host

$tmpPath = "rmdir """ + $tmpPath+"""" + " -Recurse"
#Write-Host $tmpPath
Invoke-Expression $tmpPath | Write-Host



