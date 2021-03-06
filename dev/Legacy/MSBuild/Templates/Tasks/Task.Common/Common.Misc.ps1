function get-IsNet4framework-available($machinename)
{
	return Test-Path 'HKLM:\SOFTWARE\MICROSOFT\NET Framework Setup\NDP\v4\Full'
}

function check-netframework-four($machinename)
{
    Write-Host "$indent $machinename should have .NET Framework 4 installed..."
    $result = get-isnet4framework-available $machinename
    if($result -eq $True)
    {
        write-pass ".NET Framework 4 Installed."    
    }
    else
    {
        Write-fail ".NET Framework 4 NOT Installed."
        $FailureCount++            
    }
}

function get-controltable-shares($conn)
{
    $datatable = exec-query-dataset "select controltext from [2am].dbo.control where controltext like '%\\%'" -conn $conn
    
    $shares = @()
    ForEach ($Row in $datatable.Tables[0].Rows) 
    { 
        $shares += $Row[0]
    }
    return $shares
}

function check-networkshareexists($sharename, $indent)
{
    Write-Host "$global:indent network share [$sharename] should exist..."
    $result = get-networkshare-exists $sharename
    if($result -eq $True)
    {
        write-pass "network share exists."
    }
    else
    {
        Write-fail "network share does not exist."
        $FailureCount++    
    }
}