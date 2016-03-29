function RunScripts($connectionString,$scriptPath,$scriptenator)
{

    Write-Host "Start Restore Databases"

    SAHL.Tools.Scriptenator.CommandLine.exe -c $connectionString -d $scriptPath -f $scriptenator

    Write-Host "Complete Restore Databases"

}


function GenenerateMasterRestoreScripts($connectionString, $scriptPath,$restorePath, $backupPath)
{ 
    Write-Host "Start Generate Post Restore Script"

    SAHL.Tools.Restorenator.exe -c $connectionString -d $scriptPath  -r "2am|$restorePath;e-work|$restorePath;ImageIndex|$restorePath;MetroSQL|$restorePath;Process|$restorePath;SAHLDB|$restorePath;Staging|$restorePath;UIPState|$restorePath;Warehouse|$restorePath;Warehouse_Archive|$restorePath;X2|$restorePath;" -p $backupPath

    Write-Host "Complete Generate Post Restore Script"
}

function GenenerateRestoreScripts($connectionString, $scriptPath,$dbRestores, $backupPath)
{ 
    Write-Host "Start Generate Post Restore Script"

    SAHL.Tools.Restorenator.exe -c $connectionString -d $scriptPath  -r $dbRestores -p $backupPath

    Write-Host "Complete Generate Post Restore Script"
}

function RestoreDatabase($databaseName,$restorePath,$backUpPath)
{
	$conn=new-object data.sqlclient.sqlconnection ("Data Source=" + ($MACHINENAMEDB) + ";Initial Catalog=Master;Persist Security Info=True;User ID=$DBUSER;Password=$DBPASSWORD;")
	$conn.open()
	exec-query-scalar "EXEC [master].[dbo].[pRestoreDatabase]  '$databaseName','$backUpPath','$restorePath'" -conn $conn
	$conn.close()	
}

function exec-query-scalar( $sql, $parameters=@{}, $conn, $timeout=36000, [switch]$help)
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
    foreach($p in $parameters.Keys){
    [Void] $cmd.Parameters.AddWithValue("@$p",$parameters[$p])
    }
    
    $result = $cmd.ExecuteScalar()
    return $result
}