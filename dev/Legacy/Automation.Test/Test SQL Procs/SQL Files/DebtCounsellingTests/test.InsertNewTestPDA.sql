USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertNewTestPDA') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.InsertNewTestPDA
	Print 'Dropped procedure test.InsertNewTestPDA'
End
Go


CREATE PROCEDURE test.InsertNewTestPDA

@PDAName varchar(250),
@regNumber varchar(250)

as

declare @parentkey int
declare @orgStructKey int
declare @LEKey int

set @parentkey = (select organisationstructureKey from OrganisationStructure os
where os.description =  'Payment Distribution Agencies')
--org structure record
insert into OrganisationStructure
(ParentKey, Description, OrganisationTypeKey, GeneralStatusKey)
values
(@parentKey, @PDAName, 1,1)
set @orgStructKey = scope_identity()
--LE record
insert into [2am]..LegalEntity
(legalentityTypeKey, introductionDate, RegistrationNumber, RegisteredName, WorkPhoneCode, WorkPhoneNumber,
EmailAddress, CitizenTypeKey, LegalEntityStatusKey, UserID, DocumentLanguageKey)
values
(3,getdate(), @regNumber, @PDAName, '011', '22323232', 'test@test.co.za',1,1,'SAHL\TestUser',2)
set @LEKey = scope_identity()
--LegalEntityOrganisationStructure
insert into [2am]..LegalEntityOrganisationStructure
select @LEKey, @orgStructKey
--we also need a legal entity address
declare @addressKey int

select top 1 @addressKey = a.addressKey from [2am].dbo.address a
left join [2am]..legalentityaddress lea on a.addressKey=lea.addressKey
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
and lea.addresskey is null
order by 1 desc

--WE NEED TO GIVE THE LEGAL ENTITY ADDRESS RECORDS
insert into [2am].dbo.legalEntityAddress (
LegalEntityKey,
AddressKey,
AddressTypeKey,
EffectiveDate,
GeneralStatusKey
)
values (
@LEKey, @addressKey,1,getdate(),1
)
