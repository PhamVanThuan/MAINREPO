USE [2AM]
GO

/****** Object:  StoredProcedure [test].[AddPDALegalEntities]    Script Date: 01/20/2011 10:00:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

if exists (select * from dbo.sysobjects where id = object_id(N'test.AddPDALegalEntities') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
begin
	drop procedure test.AddPDALegalEntities
	print 'Dropped procedure test.AddPDALegalEntities'
end
GO

create procedure [test].[AddPDALegalEntities]
(
					@LENames varchar(100),
					@Surname varchar(100),
					@OrgStructureKey int
)
as
/*
begin tran test
--rollback
declare @LENames varchar(100),
			@Surname varchar(100),
					@OrgStructureKey int
					
set @LENames = 'Test'
set @Surname = 'Tester'
set @orgStructureKey = 2797
*/			
declare @OrgTypekey int,
	@LegalEntityKey int,
	@StreetAddressKey int,
	@PostalAddressKey int,
	@registrationnumber varchar(100)

--------------------------------------------
-- insert street address for the PDA
--------------------------------------------
if (not exists(select 1 from dbo.Address where streetnumber = '111' AND streetname = 'PDA Update Tests' and suburbkey = 73462 ))
begin
	insert into [2AM].[dbo].[Address]
		   ([AddressFormatKey],[StreetNumber],[StreetName],[SuburbKey],[PostOfficeKey],[RRR_CountryDescription]
		   ,[RRR_ProvinceDescription],[RRR_CityDescription],[RRR_SuburbDescription],[RRR_PostalCode],[ChangeDate])
	values
		   (1,'111','PDA Update Tests',73462,1682,'South Africa','Guateng','Johannesburg','Midrand','1682',GetDate())
end

set @StreetAddressKey = (select  addresskey  from [2AM].[dbo].[Address] where streetnumber = '111' AND streetname = 'PDA Update Tests' and suburbkey = 73462);

------------------------------------------------
-- insert legalentity of type company
------------------------------------------------
select @OrgTypekey = OrganisationTypeKey from [2am]..organisationstructure where organisationstructurekey = @orgstructurekey

if (@OrgTypekey = 2)
begin
	set @registrationnumber = @LENames
end
else
begin
	set @registrationnumber = null
end

if (@OrgTypekey in (1,3,5))
begin
	--check if a legalentity with the given registered name is linked to the given organisationstructure record
	if not exists (	select 1 
					from OrganisationStructure os
							join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
							join legalentity le on leos.legalentitykey = le.legalentitykey
					where os.OrganisationStructureKey = @OrgStructureKey)
	begin
		print 'Inserting LE'
		
		create table #new (legalentitykey int not null)
		
		insert into [2AM].[dbo].[LegalEntity]
			output inserted.legalentitykey into #new
		values	(3, null, null, null, GetDate(), null, null, null, null, null, null, null, null, @registrationnumber, @LENames,@LENames, null, null, null,
				'111','1111111', null, null, null, null, null, null, 1, null, 1, null, GetDate(), null, null, 2, null)

		select @LegalEntityKey = legalentitykey from #new
		drop table #new
		
		-- link le to org structure
		insert into [2AM].[dbo].[LegalEntityOrganisationStructure]([LegalEntityKey],[OrganisationStructureKey]) values (@LegalEntityKey,@OrgStructureKey)
	end	
	else
	begin
		print 'Updating LE'
		
		Update le
		Set	le.LegalEntityTypeKey = 3, le.RegistrationNumber = @registrationnumber, le.RegisteredName = @LENames, le.TradingName = @LENames,
				le.WorkPhoneCode = '111', le.WorkPhoneNumber = '1111111', le.LegalEntityStatusKey = 1, le.LegalEntityExceptionStatusKey = 1, le.DocumentLanguageKey = 2
		From OrganisationStructure os
					join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
					join legalentity le on leos.legalentitykey = le.legalentitykey
		where	os.OrganisationStructureKey = @OrgStructureKey
		
		Select	@legalentitykey = le.legalentitykey 
		From	OrganisationStructure os
					join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
					join legalentity le on leos.legalentitykey = le.legalentitykey
		Where	os.OrganisationStructureKey = @OrgStructureKey
	end
	
	-- link the addresses to the legalentity
	if (@OrgTypekey in (1,3) and not exists(select 1 from dbo.LegalEntityAddress where LegalEntityKey = @LegalEntityKey))
	begin
		insert into [2AM].[dbo].[LegalEntityAddress] ([LegalEntityKey],[AddressKey] ,[AddressTypeKey],[EffectiveDate],[GeneralStatusKey]) values (@LegalEntityKey,@StreetAddressKey,1,GetDate(),1)
	end
end

---------------------------------------------------------------------------------------
-- insert legalentity and legalentityorganisationstructure records for the PDA contacts
---------------------------------------------------------------------------------------
if (@OrgTypekey = 7)
begin
	--check if a legalentity with the given registered name is linked to the given organisationstructure record
	if not exists (	select 1 
					from OrganisationStructure os
							join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
							join legalentity le on leos.legalentitykey = le.legalentitykey
					where os.OrganisationStructureKey = @OrgStructureKey)
	begin
		print 'Inserting LE'
		
		create table #new1 (legalentitykey int not null)
		
		insert into [2AM].[dbo].[LegalEntity]
			output inserted.legalentitykey into #new1
		values	(2, null, null, null, GetDate(), 1, @LENames, null, @Surname, null, null, null, null, null, null,null, null, null, null,
				'111','1111111', null, null, null, null, null, null, 1, null, 1, null, GetDate(), null, null, 2, null)

		select @LegalEntityKey = legalentitykey from #new1
		drop table #new1
		
		-- link le to org structure
		insert into [2AM].[dbo].[LegalEntityOrganisationStructure]([LegalEntityKey],[OrganisationStructureKey]) values (@LegalEntityKey,@OrgStructureKey)
	end
	else
	begin		
		print 'Updating LE'
		
		Update le
		Set	le.LegalEntityTypeKey = 2, SalutationKey = 1, FirstNames = @LENames, Surname = Surname,
				le.WorkPhoneCode = '111', le.WorkPhoneNumber = '1111111', le.LegalEntityStatusKey = 1, le.LegalEntityExceptionStatusKey = 1, le.DocumentLanguageKey = 2
		From OrganisationStructure os
					join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
					join legalentity le on leos.legalentitykey = le.legalentitykey
		where	os.OrganisationStructureKey = @OrgStructureKey
	end
end

GO


