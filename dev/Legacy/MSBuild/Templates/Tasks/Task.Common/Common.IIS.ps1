function get-iisapppool($appPoolName)
{
    $appPool = Get-WmiObject -Namespace 'root\MicrosoftIISv2' -Class IIsApplicationPool  -Filter "Name = 'W3SVC/APPPOOLS/$appPoolName'"
    return $appPool
}

function get-iisvirtual-directory($virtualDirName)
{
    $virtualDir = Get-WmiObject -Namespace 'root\MicrosoftIISv2' -Class IIsWebVirtualDir -Filter "AppRoot LIKE '%$virtualDirName'"
    return $virtualDir
}

function get-iisweb-directory($directoryName)
{
    $virtualDir = Get-WmiObject -Namespace 'root\MicrosoftIISv2' -Class IIsWebDirectory -Filter "AppRoot LIKE '%$directoryName'"
    return $virtualDir
}

function get-root-iisweb-directory($directoryName)
{
    $virtualDir = Get-WmiObject -Namespace 'root\MicrosoftIISv2' -Class IIsWebVirtualDir -Filter "AppRoot ='$directoryName'"
    return $virtualDir
}

function get-iisvirtual-directory-setting($directoryName, $settingName)
{
    $virtualDirSettings = Get-WmiObject -Namespace 'root\MicrosoftIISv2' -Class IIsWebVirtualDirSetting -Filter "Name ='$directoryName'"
    $virtualDirSetting = $virtualDirSettings | Select -expandproperty $settingName
    return $virtualDirSetting
}

function get-iisweb-directory-setting($directoryName, $settingName)
{
    $virtualDirSettings = Get-WmiObject -Namespace 'root\MicrosoftIISv2' -Class IIsWebDirectorySetting -Filter "Name ='$directoryName'"
    $virtualDirSetting = $virtualDirSettings | Select -expandproperty $settingName
    return $virtualDirSetting
}

function get-iis-scriptmapping($virtualDirFullName, $extension)
{
    return get-iis-scriptmappingiis6 $virtualDirFullName $extension
}

function get-iis-scriptmappingiis6($virtualDirFullName, $extension)
{
    $virtualDirSettings = Get-WmiObject -Namespace 'root\MicrosoftIISv2' -Class IIsWebVirtualDirSetting -Filter "Name ='$virtualDirFullName'"
    if(-not $virtualDirSettings)
    {
        write-host "vdir is null - $virtualDirFullName"
        $virtualDirSettings = Get-WmiObject -Namespace 'root\MicrosoftIISv2' -Class IIsWebDirectorySetting -Filter "Name ='$virtualDirFullName'"
    }
    $scriptMaps = $virtualDirSettings | Select-Object ScriptMaps
    $processor = $scriptMaps.ScriptMaps | ? { $_.Extensions -eq "$extension" } | Select-Object ScriptProcessor
    return $processor
}

function check-apppool-exists($appPoolName)
{
    check-apppool-exists-iis6 $appPoolName
}

function check-apppool-exists-iis6($appPoolName)
{
	Write-Check "There should be an application pool named $appPoolName..."
    $result = get-iisapppool $appPoolName
    if($result)
    {
        Write-pass "The webserver does contain an application pool named $appPoolName."    
    }
    else
    {
        Write-fail "The webserver does NOT contain an application pool named $appPoolName."        
    }
}

function check-apppool-contains-website($appPoolName, $websiteName, $netVer)
{
    check-apppool-contains-website-iis6 $appPoolName $websiteName $netVer
}

function check-apppool-contains-website-iis6($appPoolName, $websiteName, $netVer)
{
    Write-Check "Application Pool named $appPoolName should contain a web application named $websiteName..."
    $appPool = get-iisapppool  $appPoolName
    If($appPool)
    {
        $apps = $appPool.EnumAppsInPool().Applications
        ForEach($app in $apps)
        {
            If ($app.EndsWith($websiteName + "/"))
            {
                Write-pass "$appPoolName contains an application named $websiteName."    
                
                Write-Check "Web application named $websiteName should be running the $netVer version of .NET..."
                
                $app = $app.Replace("/LM/","")
                $aspxSetting = get-iis-scriptmapping $app ".aspx"
                if($aspxSetting.ScriptProcessor.Contains($netVer))
                {
                    Write-Pass "Web application is running on the $netVer version of .NET"
                }
                else
                {
                    Write-Fail "Web application is NOT running on the $netVer version of .NET"
                }

                return
            }
        }
        Write-fail "The Application Pool does NOT contain an application named $websiteName."          
    }
    else
    {
        Write-fail "The webserver does NOT contain an application pool named $appPoolName, cannot search for website $websiteName."       
    }
}

function get-iis-website-path($websiteName)
{
    $website = get-iisvirtual-directory $websiteName
    if(-not $website)
    {
        $website = get-iisweb-directory $websiteName
        if($website)
        {
            <# we have a physical web dir #>
            #$website = get-root-iisweb-directory $website.AppRoot.ToUPPER().Replace("/" + $websiteName.ToUpper(), "")
            $path = get-iisvirtual-directory-setting $website.AppRoot.ToUPPER().Replace("/" + $websiteName.ToUpper(), "").Replace("/LM/","") "Path"
        }
        else
        {
        
        }
    }
    else
    {
        $path = get-iisvirtual-directory-setting $website.AppRoot.Replace("/LM/","") "Path"
    }

    <# we have a virtual web dir #>
    return $path
}