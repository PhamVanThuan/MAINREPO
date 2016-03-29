param 
(
	[string] $Server,	
	[string] $UserName,
    [string] $Password,
    [string] $JOBNAME,
    [int] $sleepPeriod
)

Write-Host $JOBNAME
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) Common.SQL.ps1)

StartSQLServerAgentJob $Server $UserName $Password -JOBNAME $JOBNAME $sleepPeriod
