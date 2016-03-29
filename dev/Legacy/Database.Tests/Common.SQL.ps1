[reflection.assembly]::LoadWithPartialName("Microsoft.SqlServer")
[reflection.assembly]::LoadWithPartialName("Microsoft.SqlServer.Agent")
[reflection.assembly]::LoadWithPartialName("Microsoft.SqlServer.ConnectionInfo")
[reflection.assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")

function exec-query-scalar( $sql, $parameters=@{}, $conn, $timeout=30, [switch]$help)
{
    if ($help)
    {
        $msg = "
        Execute a sql statement that returns a scalar value.&nbsp; Parameters are allowed.
        Input parameters should be a dictionary of parameter names and values.
        Return value will be a scalar value.
        "
        Write-Host $msg
        return
    }
    $cmd=new-object system.Data.SqlClient.SqlCommand($sql,$conn)
    $cmd.CommandTimeout=$timeout

    foreach($p in $parameters.Keys)
    {
        [Void] $cmd.Parameters.AddWithValue("@$p",$parameters[$p])
    }
    
    $result = $cmd.ExecuteNonQuery();
    return $result
}

function ExecuteSqlScript()
{
    Param([string]$userName,
          [string]$password,
          [string]$server,
          [string]$database,
          [System.Collections.Specialized.StringCollection]$sqlQuery)

    $conn = new-object Microsoft.SqlServer.Management.Common.ServerConnection
    $conn.LoginSecure = $FALSE
    $conn.Login = $userName
    $conn.Password = $password
    $conn.ServerInstance = $server
    $conn.DatabaseName = $database

   return $conn.ExecuteWithResults($sqlQuery)     
}

function StartSQLServerAgentJob($Server, $UserName, $Password, $jobName,$sleepPeriod)
{
    Write-Host "Server:" $Server "Username" $UserName "Password" $Password "Jobname" $jobName

	$conn = new-object Microsoft.SqlServer.Management.Common.ServerConnection
    $conn.LoginSecure = $FALSE
    $conn.Login = $UserName
    $conn.Password = $Password
    $conn.ServerInstance = $Server

    $sqlServer = new-object Microsoft.SqlServer.Management.Smo.Server($conn)
   
    $job = $sqlServer.JobServer.Jobs[$jobName];
    if($job -ne $null)
    {
        $job.Start();
        do{
            Start-Sleep -Seconds $sleepPeriod
            write-host "Job:"  $job.Name  "currently"  $job.CurrentRunStatus "Step" $job.CurrentRunStep               
            $job.Refresh();
           
        }
        while($job.CurrentRunStatus -ne "Idle")
	
	 Write-Host "Job Last outcome: "+ $job.LastRunOutcome 
     if($job.LastRunOutcome -ne "Succeeded")
     {
	    throw  "Job " + $job.LastRunOutcome
     }
     else
     {
		write-host "Job: " + $job.Name  + "Complete"
     }
    }
    else
    {    
       throw "Job: " + $jobName + " doesnt exist"
    }
}

function ExecuteSqlFile()
{
    Param([string]$userName,
          [string]$password,
          [string]$server,
          [string]$database,
          [string]$filePath)

    $intVar = 0
    $LineNunber = 0

    $intVar = sqlcmd -U "$userName" -P "$password" -h-1 -t 900 -S "$server" -d "$database" -i "$filePath"

    $LineNunber = select-string -inputobject $intVar -Pattern "msg" | Select-Object LineNumber

    $LineNunber = "" + $LineNunber
        
    if($LineNunber -ne "") 
    {
        throw $intVar
    }

}


function ExecuteSqlFilesInPath()
{
    Param([string]$path,
          [string]$userName,
          [string]$password,
          [string]$server,
          [string]$database,
          [string]$filePath)

    $fileExtention="*.sql"
    Write-Host "Executing sql files in "$rootPath 
    if ((Test-Path -path "$path") -eq $true)
    {
        $files = Get-ChildItem -Path $path -Filter $fileExtention
		if($files -ne $null)
		{
			foreach($file in $files)
			{
				if($file.FullName -ne  "")
				{

				Write-Host "Executing " $file.FullName " on $server"                   
				ExecuteSqlFile -userName $username -password $password -server $server -database $database -filePath $file.FullName
				}
			}
		}
    }
    else
    {
        Write-Host "$path does not exist"
    }
}