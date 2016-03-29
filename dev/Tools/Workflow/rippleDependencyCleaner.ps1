[Reflection.Assembly]::LoadWithPartialName("System.Xml")
[Reflection.Assembly]::LoadWithPartialName("System.Collections")

$currentPath = Split-Path $Script:MyInvocation.MyCommand.Path

write-host $currentPath

$dependencies = New-Object System.Collections.ArrayList

Get-ChildItem -Path $currentPath -Filter ripple.dependencies.config -Recurse | Foreach -Begin{$i=1}`
    -Process{
        $rippleConfigFile = $_.FullName
        
        $a= get-content $rippleConfigFile

        foreach ($t in $a) {
            $null = $dependencies.Add($t) #assign to null to prevent it printing the index
        }
    }

Get-ChildItem -Path $currentPath -Filter ripple.config -Recurse | Foreach -Begin{$i=1}`
    -Process{
        $rippleConfigFile = $_.FullName
        $xmlDoc = New-Object -TypeName XML
        $xmlDoc.Load($rippleConfigFile)
        $xmlDoc.ripple.Nugets.Dependency | ? {

            $doesnothavevalue = $dependencies -cnotcontains $_.name

            if ($doesnothavevalue)
            {
               write-host "does not contain: " $_.name
               $xmlDoc.ripple.Nugets.RemoveChild($_)
               write-host "removed from ripple.config: " $_.name
               $xmlDoc.Save($rippleConfigFile);
            }

        }      
    }

    