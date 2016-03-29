[String]$databaseFrom = 'sahls03'
[String]$databaseTo = 'localhost'

[String]$messageBusServerFrom = 'sahls47'
[String]$messageBusServerTo = 'localhost'

[String]$directory = Split-Path -Parent $MyInvocation.MyCommand.Path

$include=@("CAP2 Offers.config","Debt Counselling.config","Help Desk.config","Life.Config","Loan Adjustments.config","Origination.config","Personal Loan.config")

$allConfigs = @(Get-ChildItem -Path $directory -include $include -Recurse)

Write-Output ("Changing Database Server from ( " + $databaseFrom + " ) to ( " + $databaseTo + " )")  
Write-Output ("Changing Message  Server from ( " + $messageBusServerFrom + " ) to ( " + $messageBusServerTo + " )")  

ForEach($configFile in $allConfigs)
{
	Write-Output ("" + $configFile)  
   
    #using simple text replacement to replace database server in connection string   
    $con = Get-Content $configFile
    $con | % { $_.Replace($databaseFrom, $databaseTo) } | % { $_.Replace($messageBusServerFrom, $messageBusServerTo) } | Set-Content $configFile
}