param(
    $currentDir,
    $dbServer
)
cd $currentDir
Write-Host "Generating models from $dbServer"
$object = .\generateDbObj.ps1 -currentDir $currentDir -objectName "Account"

[string]$json = ($object | ConvertTo-Json)

Set-Content .\Test.json  $json

