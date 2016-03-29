use [2AM]

update dbo.control
set controltext = 'false'
where controldescription = 'LightstoneValuationBypassProxy'

update dbo.control
set controltext = 'http://preprod.lightstone.co.za/avm/webservices/sahl.asmx'
where controldescription = 'LightstoneValuationWebServiceUrl'
