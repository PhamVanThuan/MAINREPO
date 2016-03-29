function PerformTask()
{
	Write-Host "Checking Post Deploy"
	iisreset /start | Write-Host
	Write-Host "Warmup X2 Service"
	IsServiceUp X2EngineService
}

function IsServiceUp($serviceName)
{
    [System.Net.HttpWebRequest]$HTTP_Request = [System.Net.WebRequest]::Create("http://localhost/$serviceName/api/CommandHttpHandler/Invoke?methodName=GetServiceNames&typeName=%22%22")
    $HTTP_Request.Credentials = [System.Net.CredentialCache]::DefaultNetworkCredentials

    [int]$HTTP_Status = -1
    try
    {
      $HTTP_Response = $HTTP_Request.GetResponse()

      $HTTP_Status = [int]$HTTP_Response.StatusCode
    }
    catch  {
      [int]$HTTP_Status = ([System.Net.HttpWebResponse]([System.Net.WebException]$_.Exception.InnerException).Response).StatusCode
    }

  if ($HTTP_Status -eq 200) {
      Write-Host  "Service: $serviceName HTTPstatus: $HTTP_Status is up and running"
  }
  elseif($HTTP_Status -eq 404) {
    # Do nothing site doesn't exist on this server
  }
  else{
      Write-Warning   "Service: $serviceName HTTPstatus: $HTTP_Status failed to start"
      return $false
  }
  return $true
}