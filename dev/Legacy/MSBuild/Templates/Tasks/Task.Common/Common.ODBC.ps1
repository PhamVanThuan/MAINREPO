function get-odbc-connections($machineName)
{
   #Access Remote Registry Key using the static OpenRemoteBaseKey method.
   $rootKey = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey("LocalMachine",$machineName)
   if(-not $rootKey)
   {
        Write-Error "Can't open the remote $root registry hive" 
   }
   
   $strKeyPath = "SOFTWARE\ODBC\ODBC.INI\"
   
   $key = $rootKey.OpenSubKey( $strKeyPath )
   
   $odbcConns = @()
   if($key) 
   {
        $subKeyNames = $key.GetSubKeyNames()
        ForEach($subKeyName in $subKeyNames)
        {
            $subKey = $key.OpenSubKey( $subKeyName )
            if(($subkey) -and ($subKeyName -ne "ODBC Data Sources") -and ($subKeyName -ne "ODBC File DSN") -and ($subKeyName -ne "Xtreme Sample Database"))
            {
                $objODBCConn = New-Object System.Object
                $objODBCConn | Add-Member NoteProperty "Name" $subKeyName
                $odbcConns += $objODBCConn
                $subKeyValueNames = $SubKey.GetValueNames()
                ForEach($subKeyValueName in $subKeyValueNames)
                {
                    if($subKeyValueName.Length -gt 0)
                    {
                        $objODBCConn | Add-Member NoteProperty $subKeyValueName $SubKey.GetValue($subKeyValueName)
                    }
                }
            }
        }
   }  
   return $odbcConns
}

function check-odbc-exists($odbcConns, $odbcName)
{
	$odbcConn = $odbcConns | Where-Object {$_.Name -eq $odbcName}
	
	Write-Check "ODBC connection named ($odbcName) should exist..."
	
	if($odbcConn)
	{
		Write-Pass "ODBC Connection $odbcName does exist."
	}
	else
	{
		Write-Fail "ODBC Connection $odbcName does NOT exist."
	}
}

function check-odbc-property($odbcConns, $odbcName, $propertyName, $propertyRequiredValue)
{
	$odbcConn = $odbcConns | Where-Object {$_.Name -eq $odbcName}
	Write-Check "ODBC connection named ($odbcName) should have a [$propertyName] property of ($propertyRequiredValue)..."
	
	if($odbcConn)
	{
		$propertyValue = $odbcConn | Select -ExpandProperty $propertyName
		if($propertyValue -eq $propertyRequiredValue)
		{
			Write-Pass "ODBC Property => $propertyName = [$propertyValue] expected [$propertyRequiredValue]."
		}
		else
		{
			Write-fail "ODBC Property => $propertyName = [$propertyValue] expected [$propertyRequiredValue]."
		}
	}
	else
	{
		Write-Fail "ODBC Connection $odbcName does NOT exist."
	}	
}
