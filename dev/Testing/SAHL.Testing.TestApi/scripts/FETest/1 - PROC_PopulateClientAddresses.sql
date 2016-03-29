use [FETest]

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateClientAddresses')
	DROP PROCEDURE dbo.PopulateClientAddresses
GO

CREATE PROCEDURE dbo.PopulateClientAddresses

AS

BEGIN

IF(EXISTS(SELECT 1 FROM [FETest].dbo.ClientAddresses))
	BEGIN
		truncate table [FETest].dbo.ClientAddresses
	END


insert into FETest.dbo.ClientAddresses
(LegalEntityAddressKey, AddressKey, AddressTypeKey, AddressFormatKey, LegalEntityKey)
select lea.LegalEntityAddressKey, a.AddressKey, lea.AddressTypeKey, a.AddressFormatKey, lea.LegalEntityKey 
from [2am].dbo.Address a
join [2am].dbo.LegalEntityAddress lea on a.AddressKey = lea.AddressKey
join [2am].dbo.Suburb s on a.SuburbKey = s.SuburbKey
where a.AddressFormatKey = 1
and a.StreetName is not null and len(StreetName) > 0
and a.StreetNumber is not null and len(StreetNumber) > 0
and a.SuburbKey is not null 
and a.RRR_CityDescription is not null and len(RRR_CityDescription) > 0
and a.RRR_CountryDescription is not null and len(RRR_CountryDescription) > 0
and a.RRR_PostalCode is not null and len(RRR_PostalCode) > 0
and a.RRR_ProvinceDescription is not null and len(RRR_ProvinceDescription) > 0
and a.RRR_SuburbDescription is not null and len(RRR_SuburbDescription) > 0
and lea.GeneralStatusKey = 1
order by newid()

insert into FETest.dbo.ClientAddresses
(LegalEntityAddressKey, AddressKey, AddressTypeKey, AddressFormatKey, LegalEntityKey)
select lea.LegalEntityAddressKey, a.AddressKey, lea.AddressTypeKey, a.AddressFormatKey, lea.LegalEntityKey 
from [2am].dbo.Address a
join [2am].dbo.LegalEntityAddress lea on a.AddressKey = lea.AddressKey
where a.AddressFormatKey = 2
and boxNumber is not null
and postOfficeKey is not null
and a.RRR_CityDescription is not null and len(RRR_CityDescription) > 0
and a.RRR_CountryDescription is not null and len(RRR_CountryDescription) > 0
and a.RRR_PostalCode is not null and len(RRR_PostalCode) > 0
and a.RRR_ProvinceDescription is not null and len(RRR_ProvinceDescription) > 0
and a.RRR_SuburbDescription is not null and len(RRR_SuburbDescription) > 0
and lea.GeneralStatusKey = 1
order by newid()

insert into FETest.dbo.ClientAddresses
(LegalEntityAddressKey, AddressKey, AddressTypeKey, AddressFormatKey, LegalEntityKey)
select lea.LegalEntityAddressKey, a.AddressKey, lea.AddressTypeKey, a.AddressFormatKey, lea.LegalEntityKey  
from [2am].dbo.Address a
join [2am].dbo.LegalEntityAddress lea on a.AddressKey = lea.AddressKey
where a.AddressFormatKey = 3
and boxNumber is not null
and postOfficeKey is not null
and SuiteNumber is not null
and a.RRR_CityDescription is not null and len(RRR_CityDescription) > 0
and a.RRR_CountryDescription is not null and len(RRR_CountryDescription) > 0
and a.RRR_PostalCode is not null and len(RRR_PostalCode) > 0
and a.RRR_ProvinceDescription is not null and len(RRR_ProvinceDescription) > 0
and a.RRR_SuburbDescription is not null and len(RRR_SuburbDescription) > 0
and lea.GeneralStatusKey = 1

insert into FETest.dbo.ClientAddresses
(LegalEntityAddressKey, AddressKey, AddressTypeKey, AddressFormatKey, LegalEntityKey)
select lea.LegalEntityAddressKey, a.AddressKey, lea.AddressTypeKey, a.AddressFormatKey, lea.LegalEntityKey  
from [2am].dbo.Address a
join [2am].dbo.LegalEntityAddress lea on a.AddressKey = lea.AddressKey
where a.AddressFormatKey = 4
and boxNumber is not null
and postOfficeKey is not null
and a.RRR_CityDescription is not null and len(RRR_CityDescription) > 0
and a.RRR_CountryDescription is not null and len(RRR_CountryDescription) > 0
and a.RRR_PostalCode is not null and len(RRR_PostalCode) > 0
and a.RRR_ProvinceDescription is not null and len(RRR_ProvinceDescription) > 0
and a.RRR_SuburbDescription is not null and len(RRR_SuburbDescription) > 0
and lea.GeneralStatusKey = 1
order by newid()

insert into FETest.dbo.ClientAddresses
(LegalEntityAddressKey, AddressKey, AddressTypeKey, AddressFormatKey, LegalEntityKey)
select lea.LegalEntityAddressKey, a.AddressKey, lea.AddressTypeKey, a.AddressFormatKey, lea.LegalEntityKey 
from [2am].dbo.Address a
join [2am].dbo.LegalEntityAddress lea on a.AddressKey = lea.AddressKey
where a.AddressFormatKey = 6
and boxNumber is not null
and postOfficeKey is not null
and a.RRR_CityDescription is not null and len(RRR_CityDescription) > 0
and a.RRR_CountryDescription is not null and len(RRR_CountryDescription) > 0
and a.RRR_PostalCode is not null and len(RRR_PostalCode) > 0
and a.RRR_ProvinceDescription is not null and len(RRR_ProvinceDescription) > 0
and a.RRR_SuburbDescription is not null and len(RRR_SuburbDescription) > 0
and lea.GeneralStatusKey = 1
order by newid()

insert into FETest.dbo.ClientAddresses
(LegalEntityAddressKey, AddressKey, AddressTypeKey, AddressFormatKey, LegalEntityKey)
select lea.LegalEntityAddressKey, a.AddressKey, lea.AddressTypeKey, a.AddressFormatKey, lea.LegalEntityKey  
from [2am].dbo.Address a
join [2am].dbo.LegalEntityAddress lea on a.AddressKey = lea.AddressKey
where a.AddressFormatKey = 5
and FreeText1 is not null and len(FreeText1) > 0
and PostOfficeKey is not null
and a.RRR_CountryDescription is not null and len(RRR_CountryDescription) > 0
and a.RRR_ProvinceDescription is not null and len(RRR_ProvinceDescription) > 0
and a.RRR_SuburbDescription is not null and len(RRR_SuburbDescription) > 0
and lea.GeneralStatusKey = 1

END

