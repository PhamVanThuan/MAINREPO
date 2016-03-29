Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Task.Common\Common.Variables.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Task.Common\Common.ODBC.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Task.Common\Common.SQL.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Task.Common\Common.Output.ps1)

	$SQLJobName = "Compo Pump"

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
		$JobEnabled = CheckJobEnabled $SQLJobName;
		
		if ($JobEnabled -eq 0)
		{
			Write-Host Enabling the sql job $SQLJobNdeame
			SetJobStatus $SQLJobName 1;
			$JobEnabled = CheckJobEnabled $SQLJobName;
			
			if ($JobEnabled -eq 1)
			{
				Write-Host The sql job $SQLJobName has been enabled
			}
		}
		else
		{
			Write-Host The sql job $SQLJobName is enabled
		}
	}
