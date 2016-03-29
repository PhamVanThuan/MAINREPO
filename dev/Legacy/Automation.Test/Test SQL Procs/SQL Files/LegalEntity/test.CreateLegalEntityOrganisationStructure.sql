USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CreateLegalEntityOrganisationStructure') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CreateLegalEntityOrganisationStructure
	Print 'Dropped procedure test.CreateLegalEntityOrganisationStructure'
End
Go

CREATE PROCEDURE test.CreateLegalEntityOrganisationStructure
	@LENames varchar(max),
	@IdNumber_RegistrationNumber varchar(max),
	@LegalEntityType int,
	@ParentKey int
as

declare  @LegalEntityKey int
declare @OrganisationStructureKey int

if (@Parentkey = 0 or @Parentkey is null)
begin
	return 
end

if (@legalentitytype = 3 or @legalentitytype = 4 or @legalentitytype = 5)
begin
	insert into [2AM].[dbo].[LegalEntity]
	(
		[LegalEntityTypeKey], 
		[IntroductionDate], 
		[registrationnumber],
		[RegisteredName], 
		[TradingName],
		[WorkPhoneCode],
		[WorkPhoneNumber],
		[LegalEntityStatusKey],
		[LegalEntityExceptionStatusKey],
		[ChangeDate],
		[DocumentLanguageKey]
	)
	values
	(
		@legalentitytype,
		GetDate(),
		@IdNumber_RegistrationNumber, 
		@LENames,
		@LENames,
		'111',
		'1111111',
		1,
		1,
		GetDate(),
		2
	)
	set @LegalEntityKey = SCOPE_IDENTITY()
end
if (@legalentitytype = 2)
begin
	insert into [2AM].[dbo].[LegalEntity]
	(
		[LegalEntityTypeKey], 
		[IntroductionDate], 
		[idnumber],
		[FirstNames], 
		[Surname], 
		[HomePhoneCode],
		[HomePhoneNumber],
		[LegalEntityStatusKey],
		[LegalEntityExceptionStatusKey],
		[ChangeDate],
		[DocumentLanguageKey]
	)
	values
	(
		@legalentitytype,
		GetDate(),
		@IdNumber_RegistrationNumber, 
		@LENames,
		@LENames,
		'111',
		'1111111',
		1,
		1,
		GetDate(),
		2
	)
set @LegalEntityKey = SCOPE_IDENTITY()
end

insert into [2AM].[dbo].[OrganisationStructure] ([ParentKey],[Description],[OrganisationTypeKey],[GeneralStatusKey]) 
values (@Parentkey,@LENames ,1,1)
set @OrganisationStructureKey = SCOPE_IDENTITY()

insert into [2AM].[dbo].[LegalEntityOrganisationStructure]([LegalEntityKey],[OrganisationStructureKey]) 
values (@LegalEntityKey,@OrganisationStructureKey)


