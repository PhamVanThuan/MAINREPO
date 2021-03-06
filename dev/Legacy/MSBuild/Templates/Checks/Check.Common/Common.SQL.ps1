function exec-query-dataset( $sql, $parameters=@{}, $conn, $timeout=30, [switch]$help)
{
    if ($help)
    {
        $msg = "Execute a sql statement.&nbsp; Parameters are allowed.
        Input parameters should be a dictionary of parameter names and values.
        Return value will usually be a list of datarows.
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
    $ds=New-Object system.Data.DataSet
    $da=New-Object system.Data.SqlClient.SqlDataAdapter($cmd)
    $da.fill($ds) | Out-Null
    return $ds
}

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
    foreach($p in $parameters.Keys){
    [Void] $cmd.Parameters.AddWithValue("@$p",$parameters[$p])
    }
    
    $result = $cmd.ExecuteScalar()
    return $result
}