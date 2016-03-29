$serviceName = "SAHL.Batch.Service"

$service = Get-Service $serviceName -ErrorAction SilentlyContinue 
    
if ($service –ne $null){
    if($service.Status -ne "Stopped") {
        $service.Stop();
        $service.WaitForStatus("Stopped");
    }
}
else
{
    Write-Host "$serviceName does not exist"   
}