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
	$conn.close()

	<# END DB CHECKS #>
}