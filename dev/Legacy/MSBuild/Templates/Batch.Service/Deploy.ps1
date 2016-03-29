$ServiceExec = "SAHL.Batch.Service.exe"

& .\"$ServiceExec" uninstall
& .\"$ServiceExec" install

Copy-Item .\Config\App.Config.$OctopusEnvironmentName "$ServiceExec.Config" | Write-Host

Remove-Item .\Config -Force -Recurse | Write-Host


