use [2AM]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'test.AddPDALegalEntities') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
begin
	drop procedure test.AddPDALegalEntities
	print 'Dropped procedure test.AddPDALegalEntities'
end
GO

create procedure test.AddPDALegalEntities
(
					@LENames varchar(100),
					@altLENames varchar(100),
					@Surname varchar(100),
					@altSurname varchar(100),
					@OrgTypekey int,
					@OrgStructureKey int
)
as

declare @LegalEntityKey int
declare @StreetAddressKey int
declare @PostalAddressKey int
declare @registrationnumber varchar(100)

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
select @OrgTypekey = OrganisationTypeKey from organisationstructure where organisationstructurekey = @orgstructurekey
if (@OrgTypekey = 2)
	set @registrationnumber = @LENames
else 
	set @registrationnumber = ''

if (@OrgTypekey in (1,3,5))
begin
	--check if a legalentity with the given registered name is linked to the given organisationstructure record
	if (exists(select 1 from OrganisationStructure os
							left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
							left join legalentity le on leos.legalentitykey = le.legalentitykey
								where os.OrganisationStructureKey = @OrgStructureKey and (le.registeredname = @altLENames or le.registeredname = @LENames)))
	begin
		update le
			set le.registrationnumber = @registrationnumber, le.[RegisteredName] = @LENames, le.[TradingName] = @LENames, le.[WorkPhoneCode] = '111', le.[WorkPhoneNumber] = '1111111'
				from OrganisationStructure os
							left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
							left join legalentity le on leos.legalentitykey = le.legalentitykey
								where os.OrganisationStructureKey = @OrgStructureKey and (le.registeredname = @altLENames or le.registeredname = @LENames)
	end
	else
	begin
		insert into [2AM].[dbo].[LegalEntity]
			   ([LegalEntityTypeKey], [IntroductionDate], registrationnumber, [RegisteredName], [TradingName], [WorkPhoneCode], [WorkPhoneNumber], [LegalEntityStatusKey],
				[LegalEntityExceptionStatusKey], [ChangeDate], [DocumentLanguageKey])
		 values
			   (3,GetDate(),@registrationnumber, @LENames,@LENames,'111','1111111',1,1,GetDate(),2)
	end	
  
	-- link le to org structure
	set @LegalEntityKey = (select legalentitykey from dbo.legalentity where [RegisteredName] = @LENames and tradingname = @LENames)
	if (not exists(select * from [2am].dbo.LegalEntityOrganisationStructure where OrganisationStructureKey = @OrgStructureKey and LegalEntityKey = @LegalEntityKey))
		insert into [2AM].[dbo].[LegalEntityOrganisationStructure]([LegalEntityKey],[OrganisationStructureKey]) values (@LegalEntityKey,@OrgStructureKey)
	-- link the addresses to the legalentity
	if (not exists(select * from dbo.LegalEntityAddress where LegalEntityKey = @LegalEntityKey and AddressKey = @StreetAddressKey))
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
	if (exists(select 1 from OrganisationStructure os
							left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
							left join legalentity le on leos.legalentitykey = le.legalentitykey
								where os.OrganisationStructureKey = @OrgStructureKey and ((le.firstnames = @altLENames and le.surname = @altsurname) or (le.firstnames = @LENames and le.surname = @surname))))
	begin
		update le
			set le.[LegalEntityTypeKey] = 2, le.[Salutationkey] = 1, le.firstnames = @LENames, le.surname = @surname,
				le.[HomePhoneCode] = '111', le.[HomePhoneNumber] = '1111111', le.[LegalEntityStatusKey] = 1, le.[ChangeDate] = GetDate(), [DocumentLanguageKey] = 2
					from OrganisationStructure os
						left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
						left join legalentity le on leos.legalentitykey = le.legalentitykey
							where os.OrganisationStructureKey = @OrgStructureKey and ((le.firstnames = @altLENames and le.surname = @altsurname) or (le.firstnames = @LENames and le.surname = @surname))
	end
	else
	begin
				insert into [2AM].[dbo].[LegalEntity]
					   ([LegalEntityTypeKey],[IntroductionDate],[Salutationkey],[FirstNames],[Surname],
						[HomePhoneCode],[HomePhoneNumber],[LegalEntityStatusKey],[ChangeDate], [DocumentLanguageKey])
				 values
					   (2,GetDate(),1,@LENames,@Surname,'111','1111111',1,GetDate(), 2)  
	end   

	set @LegalEntityKey =  (select legalentitykey from dbo.legalentity where [FirstNames] = @LENames and [Surname] = @Surname); 
	-- link le to org structure
	if (not exists(select * from [2am].dbo.LegalEntityOrganisationStructure where OrganisationStructureKey = @OrgStructureKey and LegalEntityKey = @LegalEntityKey))
		insert into [2AM].[dbo].[LegalEntityOrganisationStructure]([LegalEntityKey],[OrganisationStructureKey]) values (@LegalEntityKey,@OrgStructureKey)
	-- link addresses
--	if (not exists(select * from dbo.LegalEntityAddress where LegalEntityKey = @LegalEntityKey and AddressKey = @StreetAddressKey))
--		insert into [2AM].[dbo].[LegalEntityAddress] ([LegalEntityKey],[AddressKey] ,[AddressTypeKey],[EffectiveDate],[GeneralStatusKey]) values (@LegalEntityKey,@StreetAddressKey,1,GetDate(),1)
end
go
print 'Created procedure test.AddPDALegalEntities'
--------------------------------------****************************************************--------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'test.AddPDAOrganisationStructure') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
begin 
	drop procedure test.AddPDAOrganisationStructure
	print 'Dropped procedure test.AddPDAOrganisationStructure'
end
GO

create procedure test.AddPDAOrganisationStructure
(
					@LENames varchar(100),
					@altLENames varchar(100),
					@Surname varchar(100),
					@altSurname varchar(100),
					@OrgTypekey int,
					@OrgStructureKey int Output
)
as

---------------------------------------------------
-- Insert Company, Branch or Department into organisation structure
---------------------------------------------------
if (@OrgTypekey = 1)
	Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'

if (@OrgTypekey in (1, 3, 5))
begin
	if (exists (select 1 from [2am].dbo.OrganisationStructure where [Description] = @altLENames))
		begin
			update [2am].dbo.OrganisationStructure 
				set [Description] = @LENames, ParentKey = @OrgStructureKey, OrganisationTypeKey = @OrgTypekey, GeneralStatusKey = 1 
					where [Description] = @altLENames
		end
	else if (exists (select 1 from [2am].dbo.OrganisationStructure where [Description] = @LENames))
		begin
			update [2am].dbo.OrganisationStructure 
				set ParentKey = @OrgStructureKey, OrganisationTypeKey = @OrgTypekey, GeneralStatusKey = 1 
					where [Description] = @LENames
		end
	else
		begin
			insert into [2am].dbo.OrganisationStructure (ParentKey, [Description], OrganisationTypeKey, GeneralStatusKey) 
				values (@OrgStructureKey, @LENames, @OrgTypekey, 1)
		end

	select @OrgStructureKey = OrganisationStructureKey from [2AM].[dbo].[OrganisationStructure] where [Description] = @LENames
	execute test.AddPDALegalEntities 
						@LENames = @LENames,
						@altLENames = @altLENames,
						@Surname = '',
						@altSurname = '',
						@OrgTypekey = @OrgTypekey,
						@OrgStructureKey = @OrgStructureKey
end
---------------------------------------------------
-- Insert Contact into organisation structure
---------------------------------------------------
if (@OrgTypekey = 7)
begin
	if (exists (select 1 from [2am].dbo.OrganisationStructure os
						left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
						left join legalentity le on leos.legalentitykey = le.legalentitykey
							where le.firstnames = @altLENames and le.Surname = @altSurname and os.ParentKey = @OrgStructureKey)) 
						
	begin
		update [2AM].[dbo].[OrganisationStructure] set ParentKey = @OrgStructureKey, description = @LENames + @surname, OrganisationTypeKey = @OrgTypekey, GeneralStatusKey = 1 
				from OrganisationStructure os
						left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
						left join legalentity le on leos.legalentitykey = le.legalentitykey
							where le.firstnames = @altLENames and le.Surname = @altSurname
	end
	else if (exists (select 1 from [2am].dbo.OrganisationStructure os
						left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
						left join legalentity le on leos.legalentitykey = le.legalentitykey
							where le.firstnames = @LENames and le.surname = @surname and os.ParentKey = @OrgStructureKey ))
						
	begin
		update [2AM].[dbo].[OrganisationStructure] set ParentKey = @OrgStructureKey, description = @LENames + @surname, OrganisationTypeKey = @OrgTypekey, GeneralStatusKey = 1 
				from OrganisationStructure os
						left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
						left join legalentity le on leos.legalentitykey = le.legalentitykey
							where le.firstnames = @LENames and le.surname = @surname
	end
	else
	begin		
		insert into [2AM].[dbo].[OrganisationStructure]([ParentKey],[Description],[OrganisationTypeKey],[GeneralStatusKey]) 
			values(@OrgStructureKey,@LENames + @surname,@OrgTypekey,1)
	end

	select @OrgStructureKey = OrganisationStructureKey from [2AM].[dbo].[OrganisationStructure] where [Description] = @LENames + @surname
	update [2am].[dbo].[OrganisationStructure] set description = 'Contact' where description = @LENames + @surname

	execute test.AddPDALegalEntities 
						@LENames = @LENames,
						@altLENames = @altLENames,
						@Surname = @Surname,
						@altSurname = @altSurname,
						@OrgTypekey = @OrgTypekey,
						@OrgStructureKey = @OrgStructureKey
End
Go
print 'Created procedure test.AddPDAOrganisationStructure'
--------------------------------------****************************************************--------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'test.AddAndMaintainPDATestDataInOrganisationStructure') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
begin
	drop procedure test.AddAndMaintainPDATestDataInOrganisationStructure
	print 'Dropped procedure test.AddAndMaintainPDATestDataInOrganisationStructure'
end
go

create procedure test.AddAndMaintainPDATestDataInOrganisationStructure
as
--begin tran 
--rollback
--commit

---------------------------------------------------
--Add data for Update tests
---------------------------------------------------
Declare @OrgStructureKey int
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'CompanyToUpdate',
					@altLENames = 'CompanyUpdated',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Branch into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'BranchToUpdate',
					@altLENames = 'BranchUpdated',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 3,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Department into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'DepartmentToUpdate',
					@altLENames = 'DepartmentUpdated',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 5,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Contact into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'Contact To',
					@altLENames = 'Contact',
					@Surname = 'Update',
					@altSurname = 'Updated',
					@OrgTypekey = 7,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
--Add data for Remove tests
---------------------------------------------------
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'CompanyToRemove',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Branch into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'BranchToRemove',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 3,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Department into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'DepartmentToRemove',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 5,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Contact into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'Contact To',
					@altLENames = '',
					@Surname = 'Remove',
					@altSurname = '',
					@OrgTypekey = 7,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
--Clean data created by previous Add tests
---------------------------------------------------
declare @LEName varchar(100),
@surname varchar(100)
---------------------------------------------------
--Clean Company PDA details
---------------------------------------------------
set @LEName = 'CompanyAdded'

update [2am].dbo.OrganisationStructure 
	set GeneralStatusKey = 2 
		where [Description] = @LEName
---------------------------------------------------
--Clean Branch PDA details
---------------------------------------------------
set @LEName = 'BranchAdded'

update [2am].dbo.OrganisationStructure 
	set GeneralStatusKey = 2 
		where [Description] = @LEName
---------------------------------------------------
--Clean Department PDA details
---------------------------------------------------
set @LEName = 'DepartmentAdded'

update [2am].dbo.OrganisationStructure 
	set GeneralStatusKey = 2 
		where [Description] = @LEName
---------------------------------------------------
--Clean Contact PDA details
---------------------------------------------------
set @LEName = 'Contact'
set @surname = 'Added'

update os
			set os.GeneralStatuskey = 2
				from OrganisationStructure os
						left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
						left join legalentity le on leos.legalentitykey = le.legalentitykey
							where le.firstnames = @LEName and le.surname = @surname

---------------------------------------------------
--Add data for Add tests
---------------------------------------------------
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'CompanyToAddTo',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Branch into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'BranchToAddTo',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 3,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Department into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'DepartmentToAddTo',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 5,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
--Add data for mandatory and optional field validation tests on Update views
---------------------------------------------------
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'CompanyUpdateValidation',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Branch into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'BranchUpdateValidation',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 3,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Department into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'DepartmentUpdateValidation',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 5,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Contact into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'Contact',
					@altLENames = '',
					@Surname = 'UpdateValidation',
					@altSurname = '',
					@OrgTypekey = 7,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
--Add data for validation tests on Remove views
---------------------------------------------------
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'CompanyRemoveParentNodeValidation',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Insert Branch into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'BranchRemoveParentNodeValidation',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 3,
					@OrgStructureKey = @OrgStructureKey Output

---------------------------------------------------
--Add data for adding PDA bank details tests
---------------------------------------------------
--declare @OrgStructureKey varchar(100)
Select @OrgStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot'
---------------------------------------------------
-- Insert Company into organisation structure
---------------------------------------------------
execute test.AddPDAOrganisationStructure
					@LENames = 'AddPDABankDetails',
					@altLENames = '',
					@Surname = '',
					@altSurname = '',
					@OrgTypekey = 1,
					@OrgStructureKey = @OrgStructureKey Output
---------------------------------------------------
-- Set all bank details for the legal entity to inactive
---------------------------------------------------
update leba set leba.generalstatuskey = 2 from OrganisationStructure os
							join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
							join legalentity le on leos.legalentitykey = le.legalentitykey
							join legalentitybankaccount leba on le.legalentitykey = leba.legalentitykey
where os.OrganisationStructureKey = @OrgStructureKey and le.RegisteredName = 'AddPDABankDetails'

/*
declare	@OrganisationStructureKey int
				
Select @OrganisationStructureKey = ControlNumeric from [2am].dbo.[Control] where ControlDescription = 'PaymentDistributionAgenciesRoot';

with SubTree as ( 
				Select	os.organisationstructurekey, os.ParentKey, 
							CASE le.legalentitytypekey
								WHEN 1 THEN le.RegisteredName
								WHEN 2 THEN dbo.LegalEntityLegalName(le.legalentitykey, 0)
								WHEN 3 THEN le.RegisteredName
								ELSE ''
							END as DisplayName,
							os.OrganisationTypeKey
				from OrganisationStructure os
							left join legalentityorganisationstructure leos on os.organisationstructurekey = leos.organisationstructurekey
							left join legalentity le on leos.legalentitykey = le.legalentitykey 
				where	os.generalstatuskey = 1
				)  
	Select	os2.DisplayName as Tier1, os2.OrganisationTypeKey as OrganisationTypeKey1, 
				os3.DisplayName as Tier2,  os3.OrganisationTypeKey as OrganisationTypeKey2, 
				os4.DisplayName as Tier3,  os4.OrganisationTypeKey as OrganisationTypeKey3, 
				os5.DisplayName as Tier4,  os5.OrganisationTypeKey as OrganisationTypeKey4, 
				os6.DisplayName as Tier5,  os6.OrganisationTypeKey as OrganisationTypeKey5, 
				os7.DisplayName as Tier6,  os7.OrganisationTypeKey as OrganisationTypeKey6, 
				os8.DisplayName as Tier7,  os8.OrganisationTypeKey as OrganisationTypeKey7, 
				os9.DisplayName as Tier8,  os9.OrganisationTypeKey as OrganisationTypeKey8, 
				os10.DisplayName as Tier9,  os10.OrganisationTypeKey as OrganisationTypeKey9, 
				os11.DisplayName as Tier10,  os11.OrganisationTypeKey as OrganisationTypeKey10 
	from	(select * 
			from	subtree 
			where	organisationstructurekey = @OrganisationStructureKey) as os1 left join 
				subtree as os2 on os1.organisationstructurekey = os2.parentkey left join 
				subtree as os3 on os2.organisationstructurekey = os3.parentkey left join 
				subtree as os4 on os3.organisationstructurekey = os4.parentkey left join 
				subtree as os5 on os4.organisationstructurekey = os5.parentkey left join 
				subtree as os6 on os5.organisationstructurekey = os6.parentkey left join 
				subtree as os7 on os6.organisationstructurekey = os7.parentkey left join 
				subtree as os8 on os7.organisationstructurekey = os8.parentkey left join 
				subtree as os9 on os8.organisationstructurekey = os9.parentkey left join 
				subtree as os10 on os9.organisationstructurekey = os10.parentkey left join 
				subtree as os11 on os10.organisationstructurekey = os11.parentkey 
	--Order By 2, 4, 6, 8, 10, 12, 14, 16, 18, 20
GO

select top 10 * from legalentity le (nolock) join
legalentityaddress lea on le.legalentitykey = lea.legalentitykey join
address a on lea.addresskey = a.addresskey 
order by le.legalentitykey desc

select top 10 * from address (nolock) order by addresskey desc

execute test.AddAndMaintainPDATestDataInOrganisationStructure
--begin tran
select * from legalentity where surname = 'UpdateValidation'
*/
go
print 'Created procedure test.AddAndMaintainPDATestDataInOrganisationStructure'