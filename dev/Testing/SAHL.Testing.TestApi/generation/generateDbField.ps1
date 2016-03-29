param(
    [string]$currentDir,
    [string]$modelsDir,
    [string]$fieldName,
    [string]$dbType,
    [bool]$isNullable
)

$dbField = (Get-Content dbFieldTemplate.json) -join "`n" | ConvertFrom-Json

$type = "GEN ERROR"
if ($dbType.toLower() -eq "bigint"){
    $type = "Number"
}
if ($dbType.toLower() -eq "int"){
    $type = "Number"
}
if ($dbType.toLower().Contains("varchar")){
    $type = "String"
}




$dbField.type = $type
$dbField.id = $false
$dbField.required = $false
$dbField.length = $null
$dbField.mssql.columnName = $fieldName
$dbField.mssql.dataType = $dbType



if ($isNullable -eq $true){
        $dbField.mssql.nullable='YES'
    }else{
        $dbField.mssql.nullable='NO'    
    }

return $dbField