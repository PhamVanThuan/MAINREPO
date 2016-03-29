Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.Variables.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.ODBC.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.SQL.ps1)
Import-Module (Join-Path (Split-Path $MyInvocation.MyCommand.Path) ..\Check.Common\Common.Output.ps1)

function check-db-owner($conn, $dbname)
{
    Write-Check "$dbname database owner should be dbo..."
    $result = get-dbownerfordb $conn "$dbname"
    if($result.Equals('dbo'))
    {
        write-pass "Owner is $result."
    }
    else
    {
        Write-fail "Owner is $result."
    }
}

function get-dbownerfordb($conn, $dbname)
{
    $result = exec-query-scalar "select OwnerName = dbp.name from sys.databases sdb inner join sys.database_principals dbp on sdb.owner_sid = dbp.sid where sdb.name = '$dbname'" -conn $conn
    return $result
}

function check-db-trustworthy($conn, $dbname)
{
    Write-Check "$dbname database should be trusted..."
    $result = get-dbtrustworthyfordb $conn "$dbname"
    if($result -eq 1)
    {
        write-pass "Trustworthy = True."
    }
    else
    {
        Write-fail "Trustworthy = False."
    }
}

function get-dbtrustworthyfordb($conn, $dbname)
{
    $result = exec-query-scalar "select IsTrustWorthy = sdb.is_trustworthy_on from sys.databases sdb where sdb.name = '$dbname'" -conn $conn
    return $result
}

function get-clr_enabled($conn)
{
	$result = exec-query-scalar "SELECT value FROM sys.configurations WHERE name = 'clr enabled'" -conn $conn
	return $result		
}

function check-clr-enabled($conn)
{
    Write-Check "CLR functionality should be enabled..."
	$result = get-clr_enabled $conn
	if($result -eq 1)
	{
    	write-pass "CLR functionality is enabled."
	}
	else
	{
		write-fail "CLR functionality is NOT enabled."
	}
}

function get-sahlaggregates-permissionset($conn)
{
	$result = exec-query-scalar "select permission_set from [2am].sys.assemblies where name = 'SAHLAggregates'" -conn $conn
	return $result
}

function check-clr-aggregates-enabled($conn)
{
    Write-Check "[2AM].SAHLAggregates permissions should be [SAFE_ACCESS]..."
	$result = get-sahlaggregates-permissionset $conn
	if($result -eq 1)
	{
    	write-pass "[2AM].SAHLAggregates permissions is [SAFE_ACCESS]."
	}
	else
	{
		write-fail "[2AM].SAHLAggregates permissions is NOT [SAFE_ACCESS]."
	}
}

function check-application-dblogin-users-and-roles-configured($conn)
{
    Write-Check "All DBLogin User and Role Checks across all Databases should be [PASS]..."
	$result = exec-query-scalar "USE [master]
								 GO
									---------------------
									-- What  should be --
									---------------------

									declare @RequiredUsersAndRoles table (RequiredMemberName varchar(100), RequiredDatabaseName varchar(50), RequiredRole varchar(50))

									insert into @RequiredUsersAndRoles
									select 'AndrewK','2AM','AppRole' union 
									select 'AndrewK','2AM','ProcessRole' union 
									select 'AndrewK','master','user' union 
									select 'Andrewk','Process','AppRole' union 
									select 'AnisaY','Process','AppRole' union 
									select 'AppPluginUser','2AM','AppPluginRole' union 
									select 'AvineshaM','Process','AppRole' union 
									select 'BackupExec','master','db_owner' union 
									select 'BarbaraC','Process','AppRole' union 
									select 'Batch','2AM','ProcessRole' union 
									select 'Batch','Process','ProcessRole' union 
									select 'Batch','SAHLDB','ProcessRole' union 
									select 'Batch','Warehouse','ProcessRole' union 
									select 'BridgetteP','Process','AppRole' union 
									select 'ChrisJ','Process','AppRole' union 
									select 'CIBatch','2AM','ProcessRole' union 
									select 'CIBatch','Process','ProcessRole' union 
									select 'CIBatch','SAHLDB','ProcessRole' union 
									select 'CraigF','Process','AppRole' union 
									select 'Crystal','2AM','db_datareader' union 
									select 'Crystal','2AM','db_datawriter' union 
									select 'Crystal','2AM','db_owner' union 
									select 'Crystal','2AM','user' union 
									select 'Crystal','Credit','db_datareader' union 
									select 'Crystal','Credit','db_datawriter' union 
									select 'crystal','e-work','db_owner' union 
									select 'crystal','e-work','eWork_Role' union 
									select 'Crystal','eWork_Staging','db_owner' union 
									select 'Crystal','eWork_Warehouse','db_owner' union 
									select 'Crystal','Helpdesk','db_datareader' union 
									select 'Crystal','Helpdesk','db_datawriter' union 
									select 'Crystal','Helpdesk','db_owner' union 
									select 'Crystal','Helpdesk','HelpDeskUsers' union 
									select 'Crystal','Helpdesk','Inet' union 
									select 'Crystal','ImageIndex','db_datareader' union 
									select 'Crystal','master','db_owner' union 
									select 'Crystal','master','RSExecRole' union 
									select 'Crystal','METROSQL','db_owner' union 
									select 'Crystal','Portal11','db_owner' union 
									select 'crystal','Process','AppRole' union 
									select 'Crystal','SAHLDB','db_owner' union 
									select 'Crystal','SAHLDB','SAHL$Mortgage$system$Loan$Adm' union 
									select 'Crystal','SAHLDB','SAHL$Mortgage$system$users' union 
									select 'Crystal','Staging','db_owner' union 
									select 'Crystal','Staging','user' union 
									select 'Crystal','WAREHOUSE','Audit' union 
									select 'Crystal','Warehouse','db_datareader' union 
									select 'Crystal','WareHouse_Archive','db_datareader' union 
									select 'Crystal','X2','db_datareader' union 
									select 'Crystal','X2','db_denydatawriter' union 
									select 'dbo','master','db_owner' union 
									select 'DerrickM','Process','AppRole' union 
									select 'DeviM','Process','AppRole' union 
									select 'Eworkadmin2','2AM','db_owner' union 
									select 'Eworkadmin2','2AM','user' union 
									select 'Eworkadmin2','e-work','db_owner' union 
									select 'Eworkadmin2','SAHLDB','db_owner' union 
									select 'Eworkadmin2','SAHLDB','SAHL$Mortgage$system$Loan$Adm' union 
									select 'Eworkadmin2','SAHLDB','SAHL$Mortgage$system$users' union 
									select 'Eworkadmin2','Warehouse','db_datareader' union 
									select 'Eworkadmin2','Warehouse','db_datawriter' union 
									select 'Eworkadmin2','X2','db_owner' union 
									select 'GenevieveF','Process','AppRole' union 
									select 'HaloProxy','2AM','AppRole' union 
									select 'HaloProxy','2AM','ProcessRole' union 
									select 'HaloProxy','Process','AppRole' union 
									select 'HaloProxy','Process','ProcessRole' union 
									select 'HaloProxy','SAHLDB','AppRole' union 
									select 'HaloProxy','SAHLDB','ProcessRole' union 
									select 'HaloProxy','Warehouse','AppRole' union 
									select 'HaloProxy','Warehouse','ProcessRole' union 
									select 'HaloProxy','X2','AppRole' union 
									select 'JobUser','master','db_owner' union 
									select 'LeonP','Process','AppRole' union 
									select 'LesleyJ','Process','AppRole' union 
									select 'MandyR','Process','AppRole' union 
									select 'NeeshaS','Process','AppRole' union 
									select 'NishanaP','Process','AppRole' union 
									select 'RajenN','Process','AppRole' union 
									select 'RamonaG','Process','AppRole' union 
									select 'RonalM','Process','AppRole' union 
									select 'SagieP','Process','AppRole' union 
									select 'SAHL\SAHLS05$','master','RSExecRole' union 
									select 'SAHL\sqltemp','master','db_datareader' union 
									select 'SarishenM','Process','AppRole' union 
									select 'ServiceArchitect','2AM','AppRole' union 
									select 'ServiceArchitect','e-work','db_datareader' union 
									select 'ServiceArchitect','ImageIndex','ApplicationUser' union 
									select 'ServiceArchitect','master','AppRole' union 
									select 'ServiceArchitect','MetroSQL','MetroSQL' union 
									select 'ServiceArchitect','msdb','SQLAgentOperatorRole' union 
									select 'ServiceArchitect','msdb','SQLAgentReaderRole' union 
									select 'ServiceArchitect','Process','AppRole' union 
									select 'ServiceArchitect','SAHLDB','AppRole' union 
									select 'ServiceArchitect','SAHLDB','SAHL$Mortgage$system$Loan$Adm' union 
									select 'ServiceArchitect','SAHLDB','SAHL$Mortgage$system$users' union 
									select 'ServiceArchitect','Staging','db_owner' union 
									select 'ServiceArchitect','Staging','ITDeveloper' union 
									select 'ServiceArchitect','UIPState','ITDeveloper' union 
									select 'ServiceArchitect','Warehouse','AppRole' union 
									select 'ServiceArchitect','WareHouse_Archive','db_datareader' union 
									select 'ServiceArchitect','X2','AppRole' union 
									select 'ServiceArchitect','X2','X2User' union 
									select 'ShobanaB','Process','AppRole' union 
									select 'sqlservice2','2AM','db_backupoperator' union 
									select 'sqlservice2','Archive','db_backupoperator' union 
									select 'sqlservice2','distribution','db_backupoperator' union 
									select 'sqlservice2','e-work','db_backupoperator' union 
									select 'sqlservice2','e-work','db_owner' union 
									select 'sqlservice2','Helpdesk','db_backupoperator' union 
									select 'sqlservice2','ImageIndex','db_backupoperator' union 
									select 'sqlservice2','ITCData','db_backupoperator' union 
									select 'sqlservice2','KnowledgeBase','db_backupoperator' union 
									select 'sqlservice2','master','db_backupoperator' union 
									select 'sqlservice2','METROSQL','db_backupoperator' union 
									select 'sqlservice2','model','db_backupoperator' union 
									select 'sqlservice2','msdb','db_backupoperator' union 
									select 'sqlservice2','Portal11','db_backupoperator' union 
									select 'sqlservice2','Process','db_backupoperator' union 
									select 'sqlservice2','QuestWorkDatabase','db_backupoperator' union 
									select 'sqlservice2','SAHLDB','db_backupoperator' union 
									select 'sqlservice2','SAHLDB','db_owner' union 
									select 'sqlservice2','SS_DBA_Dashboard','db_backupoperator' union 
									select 'sqlservice2','tempdb','db_backupoperator' union 
									select 'sqlservice2','UIPState','db_backupoperator' union 
									select 'sqlservice2','Warehouse','db_backupoperator' union 
									select 'sqlservice2','WareHouse_Archive','db_backupoperator' union 
									select 'sqlservice2','X2','db_backupoperator' union 
									select 'stephanien','Process','AppRole' union 
									select 'SuwaibahR','Process','AppRole' union 
									select 'user','master','db_datareader' union 
									select 'user','master','db_datawriter' union 
									select 'VegandrenM','Process','AppRole' union 
									select 'X2','2AM','user' union 
									select 'X2','e-work','eWork_Role' union 
									select 'X2','ImageIndex','ApplicationUser' union 
									select 'X2','SAHLDB','SAHL$Mortgage$system$Loan$Adm' union 
									select 'X2','SAHLDB','SAHL$Mortgage$system$users' union 
									select 'X2','X2','X2User' 

									--select * from @RequiredUsersAndRoles

									------------------
									-- What is      --
									------------------
									declare @RoleName varchar(50)
									declare @CMD varchar(1000)
									declare @sq char(1)
									declare @ret nvarchar(max)

									select @sq = ''''

									if not exists (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'#UserRoles') AND type in (N'U'))
										create Table #UserRoles(MemberName varchar(100), DatabaseName varchar(50), Role varchar(50))
										
									if not exists (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'#RoleMember') AND type in (N'U'))
										create table #RoleMember (DBRole varchar(100),MemberName varchar(100),MemberSid varbinary(2048))

									truncate table #UserRoles
									truncate table #RoleMember

									set @CMD = 'use [?]

									truncate table #RoleMember

									insert into #RoleMember
										exec sp_helprolemember 

									insert into #UserRoles (MemberName, DatabaseName, Role) 
										select MemberName, db_name(), dbRole
										from #RoleMember'

									exec sp_MSForEachDB @CMD

									--select * from #UserRoles

									----------------
									-- Lets Check --
									----------------

									select *,
									Case when MemberName is null then '[FAIL]: DBLoginUser '+char(39)+RequiredMemberName+char(39)+' in database '+char(39)+RequiredDatabaseName+char(39)+' does not exist or is not a member of the '+char(39)+RequiredRole+char(39)+' role on '+char(39)+RequiredDatabaseName+char(39)+ 'database.'
										else '[PASS]: DBLoginUser '+char(39)+MemberName+char(39)+' in database '+char(39)+DatabaseName+char(39)+' exists and is a member of the '+char(39)+Role+char(39)+' role on '+char(39)+DatabaseName+char(39)+ 'database.'
									end as Result 
									from @RequiredUsersAndRoles ruar 
									left join #UserRoles ur on ur.MemberName=ruar.RequiredMemberName and ur.DatabaseName=ruar.RequiredDatabaseName and ur.Role=ruar.RequiredRole

									drop table #UserRoles
									drop table #RoleMember

									" -conn $conn
    return $result
}

function PerformChecks()
{
	<# BEGIN DB CHECKS #>
	
	Write-Begin-Check "Starting Database Checks"

	$conn=new-object data.sqlclient.sqlconnection ("Data Source=" + ($MACHINENAMEDB) + ";Initial Catalog=2AM;Persist Security Info=True;User ID=eworkadmin2;Password=W0rdpass;")
	$conn.open()

	$dbsToCheck = @("2AM", "e-work", "imageindex", "process", "sahldb", "x2") 
	Foreach($dbToCheck in $dbsToCheck)
	{
		<# check db ownership #>
	    check-db-owner $conn $dbToCheck
		<# check db trustworthyness #>
	    check-db-trustworthy $conn $dbToCheck
	}
	
	<# check clr enabled #>
	check-clr-enabled $conn
	
	<# check SAHL Aggregates #>
	check-clr-aggregates-enabled $conn
	
	<# check DBLogin Users and Roles on all Databases configured  #>
	check-application-dblogin-users-and-roles-configured
	
	$conn.close()

	<# END DB CHECKS #>
}