$environment = "sysc"

$connectionString = "Data Source="+$environment+"03;Initial Catalog=2AM; Persist Security Info=True; User Id=eworkadmin2;Password=W0rdpass"
$X2URL = "tcp://"+$environment+"14:8089/x2engine"
$TestWebsite = "http://"+$environment+"02"
$SAHLDataBaseServer = $environment+"03"
$HaloWebServiceURL = "http://"+$environment+"FE01/halo"
$x2WebServiceURL = $environment+"14/X2EngineService/";

$SQLReportServer = $environment+"15";
$AttorneyWebAccess = "https://"+$environment+"52:443"
$AttorneyWebAccessSearchURL = "https://"+$environment+"52:443/case/search"
$AttorneyAccessLoginURL = "https://"+$environment+"52:443/Account/Login?ReturnUrl=%2f"
$AttorneyAccessCaseSearchURL = "https://"+$environment+"52:443/Account/Login?ReturnUrl=%2fCase%2fSearch"
$ClientSecureWebsiteURL = "https://"+$environment+"02/sahl-secure/Account/Login"
$ClientSecureWebsiteLoanStatementURL = "https://"+$environment+"02/sahl-secure/Report/LoanStatement"
$ClientSecureWebsiteResetPasswordURL = "https://"+$environment+"02/sahl-secure/Account/ResetPassword"

$EzValWebserviceUrl = "http://"+$environment+"FE01:81/Valuation.svc"
$x2connectionstring = "Data Source="+$environment+"03;Initial Catalog=X2;Persist Security Info=True;User Id=eworkadmin2;Password=W0rdpass;Connect Timeout=30"

$fullPathIncFileName = $MyInvocation.MyCommand.Definition
$currentScriptName = $MyInvocation.MyCommand.Name
$rootPath = $fullPathIncFileName.Replace($currentScriptName, "")

$configFilePath = $rootPath+'app.config'

$xml = [xml](get-content $configFilePath )

$root = $xml.get_DocumentElement()

$connectionStringNode = [System.Xml.XmlNodeList]$xml.configuration.connectionStrings.ChildNodes
$connectionStringNode[0].Attributes[1].Value = $connectionString 
$connectionStringNode[1].Attributes[1].Value = $connectionString 
$connectionStringNode[2].Attributes[1].Value = $x2connectionstring

$applicationSettingsNode = [System.Xml.XmlNodeList]$xml.configuration.applicationSettings.ChildNodes

foreach ($x in $applicationSettingsNode)
{
    $children = [System.Xml.XmlNodeList]$x.ChildNodes
    foreach ($y in $children) 
    {
        if ($y.Name.Equals('SAHLDataBaseServer'))
        {
            $y.value = $SAHLDataBaseServer
        }
        if ($y.Name.Equals('TestWebsite'))
        {
            $y.value = $TestWebsite
        }
        if ($y.Name.Equals('HaloWebServiceURL'))
        {
            $y.value = $HaloWebServiceURL
        }
        if ($y.Name.Equals('SQLReportServer'))
        {
            $y.value = $SQLReportServer
        }
        if ($y.Name.Equals('AttorneyWebAccess'))
        {
            $y.value = $AttorneyWebAccess
        }
        if ($y.Name.Equals('AttorneyWebAccessSearchURL'))
        {
            $y.value = $AttorneyWebAccessSearchURL
        }
        if ($y.Name.Equals('AttorneyAccessLoginURL'))
        {
            $y.value = $AttorneyAccessLoginURL
        }
        if ($y.Name.Equals('AttorneyAccessCaseSearchURL'))
        {
            $y.value = $AttorneyAccessCaseSearchURL
        }
        if ($y.Name.Equals('EzValWebserviceUrl'))
        {
            $y.value = $EzValWebserviceUrl
        }
        if ($y.Name.Equals('X2URL'))
        {
            $y.value = $X2URL
        }  
		if ($y.Name.Equals('ClientSecureWebsiteURL'))
        {
            $y.value = $ClientSecureWebsiteURL
        } 	
		if ($y.Name.Equals('ClientSecureWebsiteLoanStatementURL'))
        {
            $y.value = $ClientSecureWebsiteLoanStatementURL
        }
		if ($y.Name.Equals('ClientSecureWebsiteResetPasswordURL'))
        {
            $y.value = $ClientSecureWebsiteResetPasswordURL
        }
        if ($y.Name.Equals('X2WebHost_Url'))
        {
            $y.value = $x2WebServiceURL
        }		
    }
}
$xml.Save($configFilePath) 





