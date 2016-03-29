USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertEmploymentRecords******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'test.InsertOfferMailingAddress') AND type in (N'P', N'PC'))
DROP PROCEDURE test.InsertOfferMailingAddress
go

USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertOfferMailingAddress******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE test.InsertOfferMailingAddress 

@offerKey int

AS

declare @addressKey int

select top 1 @addressKey = a.addressKey 
from [2am].dbo.address a
where 
addressFormatKey=1
and StreetNumber is not null 
and StreetName is not null 
and SuburbKey is not null 
and RRR_CountryDescription is not null 
and RRR_ProvinceDescription is not null 
and RRR_CityDescription is not null 
and RRR_SuburbDescription is not null 
and RRR_PostalCode is not null 
order by newid() desc

declare @offerTypeKey int
select @offerTypeKey = offerTypeKey from [2am]..offer
where offerkey = @OfferKey

if @offerTypeKey <> 11

begin

delete from OfferMailingAddress where offerKey = @OfferKey

--WE NEED TO GIVE THE LEGAL ENTITY ADDRESS RECORDS
insert into [2am].dbo.legalEntityAddress (
LegalEntityKey,
AddressKey,
AddressTypeKey,
EffectiveDate,
GeneralStatusKey
)
select ofr.legalEntityKey, @addressKey,1,getdate(),1
from [2am].dbo.offer o
join [2am].dbo.offerRole ofr on o.offerKey=ofr.offerKey 
	AND offerroletypekey IN (8,10,12,11)  
where o.offerKey=@OfferKey
order by 1 desc

--Insert OfferMailingAddress
INSERT INTO OfferMailingAddress
( OfferKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey)

select top 1 o.offerKey,lea.Addresskey,1,1,1, ofr.legalEntityKey,1
from [2am].dbo.offer o
join [2am].dbo.offerRole ofr on o.offerKey=ofr.offerKey AND offerroletypekey IN (8,10,12,11)  
join [2am].dbo.legalEntityAddress lea on ofr.legalEntityKey=lea.legalentityKey
where o.offerKey=@OfferKey and lea.AddressKey = @AddressKey
order by 1 desc

end

if @OfferTypeKey = 11

begin

-- Clean Up
/*delete lea
from [2am].dbo.offer o
join [2am].dbo.externalRole er on o.offerKey=er.genericKey 
AND externalRoletypekey IN (1)  
and er.generickeyTypeKey = 2
join [2am].dbo.legalEntityAddress lea on er.legalEntityKey=lea.legalentityKey
where  o.offerKey=@OfferKey 
*/
delete 
from OfferMailingAddress 
where offerKey = @OfferKey

--WE NEED TO GIVE THE LEGAL ENTITY ADDRESS RECORDS
insert into [2am].dbo.legalEntityAddress (
LegalEntityKey,
AddressKey,
AddressTypeKey,
EffectiveDate,
GeneralStatusKey
)
select er.legalEntityKey, @AddressKey,1,getdate(),1
from [2am].dbo.offer o
join [2am].dbo.externalRole er on o.offerKey=er.genericKey 
AND externalRoletypekey IN (1) 
and er.generickeyTypeKey = 2
where o.offerKey=@OfferKey
order by 1 desc

--Insert OfferMailingAddress
INSERT INTO OfferMailingAddress
( OfferKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey)

select top 1 o.offerKey,lea.Addresskey,1,1,1, er.legalEntityKey,2
from [2am].dbo.offer o
join [2am].dbo.externalRole er on o.offerKey=er.genericKey 
AND externalRoletypekey IN (1) 
and er.generickeyTypeKey = 2
join [2am].dbo.legalEntityAddress lea on er.legalEntityKey=lea.legalentityKey
where o.offerKey=@OfferKey and lea.AddressKey = @AddressKey
order by 1 DESC

--ensure the user has an email address
UPDATE le 
SET EmailAddress = 'clintons@sahomeloans.com'
FROM dbo.OfferMailingAddress oma
JOIN dbo.LegalEntity le ON oma.LegalEntityKey = le.LegalEntityKey
WHERE oma.offerKey = @OfferKey

end