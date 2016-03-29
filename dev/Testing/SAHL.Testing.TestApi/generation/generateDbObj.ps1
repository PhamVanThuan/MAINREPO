param(
    [string]$currentDir,
    [string]$objectName
)
cd $currentDir

$dbRecord = (Get-Content dbObjectTemplate.json) -join "`n" | ConvertFrom-Json

$field1 = .\generateDbField.ps1 -currentDir $currentDir -fieldName "AccountKey" -dbType "int" -isNullable $true
$field2 = .\generateDbField.ps1 -currentDir $currentDir -fieldName "AccountStatusKey" -dbType "int" -isNullable $true

$dbRecord.Properties = $field1, $field2 

return $dbRecord
