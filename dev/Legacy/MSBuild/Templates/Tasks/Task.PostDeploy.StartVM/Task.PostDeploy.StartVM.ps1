$toolPath = [System.Environment]::GetEnvironmentVariable("VM_TOOLPATH", "Machine")

function PerformTask()
{
	Write-Host "Checking Post Deploy"

    #type - gui|headless
    $secureToolCmd = "& '$($toolPath)VBoxManage.exe'"
    $secureToolArgs = "startvm $VMNAME --type `"headless`""
	
    Write-Host "$secureToolCmd $secureToolArgs"
	
    Invoke-Expression "$secureToolCmd $secureToolArgs" | Write-Host
}