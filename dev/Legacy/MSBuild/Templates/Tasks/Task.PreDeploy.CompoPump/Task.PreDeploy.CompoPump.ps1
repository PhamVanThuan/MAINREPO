Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Task.Common\Common.Variables.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Task.Common\Common.ODBC.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Task.Common\Common.SQL.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Task.Common\Common.Output.ps1)

	$SQLJobName = "Compo Pump"

	function CheckJobExists($jobname)
	{
		$conn=new-object data.sqlclient.sqlconnection ("Data Source=" + ($MACHINENAMEDB) + ";Initial Catalog=2AM;Persist Security Info=True;User ID=$DBUSER;Password=$DBPASSWORD;")
		$conn.open()
		$result = exec-query-scalar "select [name] from msdb.dbo.sysjobs where name = '$SQLJobName'" -conn $conn
		$conn.close()
		
		return $result
	}

	function CheckJobEnabled($jobname)
	{
		$conn=new-object data.sqlclient.sqlconnection ("Data Source=" + ($MACHINENAMEDB) + ";Initial Catalog=2AM;Persist Security Info=True;User ID=$DBUSER;Password=$DBPASSWORD;")
		$conn.open()
		$result = exec-query-scalar "select [enabled] from msdb.dbo.sysjobs where name = '$SQLJobName'" -conn $conn
		$conn.close()	

		return $result
	}

	function SetJobStatus($jobname,$enabled)
	{
		$conn=new-object data.sqlclient.sqlconnection ("Data Source=" + ($MACHINENAMEDB) + ";Initial Catalog=2AM;Persist Security Info=True;User ID=$DBUSER;Password=$DBPASSWORD;")
		$conn.open()
		$result = exec-query-scalar "update msdb.dbo.sysjobs  set enabled = $enabled where name = '$SQLJobName'" -conn $conn
		$conn.close()	

		return $result
	}

	function PerformTask()
	{
		$JobName =  CheckJobExists $SQLJobName;
		if (!$JobName)
		{
			Write-Error The sql job $SQLJobName does not exist
		}
		else
		{
			Write-Host The sql job $SQLJobName exists
			$JobEnabled = CheckJobEnabled $SQLJobName;
			if ($JobEnabled -eq 1)
			{
				Write-Host Disabling the sql job $SQLJobName
				SetJobStatus $SQLJobName 0;
				$JobEnabled = CheckJobEnabled $SQLJobName;
				if ($JobEnabled -eq 0)
				{
					Write-Host The sql job $SQLJobName has been disabled
				}
			}
			else
			{
				Write-Host The sql job $SQLJobName is disabled
			}
		}
	}

