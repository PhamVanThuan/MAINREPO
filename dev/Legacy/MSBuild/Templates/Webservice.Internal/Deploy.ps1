cd Config\
$command = '.\Configure' + $OctopusEnvironmentName +'.bat'  
cmd /c $command | Write-Host