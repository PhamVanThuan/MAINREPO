$serviceName = "SAHL.Batch.Service"

$service = Get-Service $serviceName -ErrorAction SilentlyContinue 
    
if ($service –ne $null){
    if($service.Status -eq "Stopped") {
        $service.Start();
        $service.WaitForStatus("Running");
    }
    else{
    throw "$serviceName can't be started"
    }
}
else
{
    Write-Host "$serviceName does not exist"   
}