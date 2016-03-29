param
(
	[string]$replaceString
)

$configFiles=get-childitem . *.config -rec | Select-String -pattern "DEVA", "DEVB", "DEVC", "DEVS" -list 
foreach ($file in $configFiles)
{

(Get-Content $file.path) | 
	Foreach-Object {$_ -replace "DEVA", $replaceString} | 
	Foreach-Object {$_ -replace "DEVB", $replaceString} | 
	Foreach-Object {$_ -replace "DEVC", $replaceString} |	
	Foreach-Object {$_ -replace "DEVS", $replaceString} |
	Set-Content $file.path 
}

