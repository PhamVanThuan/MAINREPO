$myScriptDirectory = [String]$PSScriptRoot
$myScriptDirectory = [String]$myScriptDirectory.replace("tests","")

[String]$appDirectory = ([String]$myScriptDirectory)+"src\SAHL.Website.Capitec\app\*.js"

[System.Array]$results = gci * -Path $appDirectory -recurse | Select-String -pattern "console.log" | group path | select name

[System.Array]$results | ForEach-Object { Write-Output ([String]$_.name).Replace($myScriptDirectory, "")}

if(([System.Array]$results).length -ne 0){
    throw "Console.log() statements are currently in the app directory."        
}


