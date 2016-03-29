USE [2AM]
GO
IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.AutomationLeads') and OBJECTPROPERTY(id, N'IsTable') = 1)
Begin
	DROP TABLE test.AutomationLeads
	Print 'Dropped TABLE test.AutomationLeads' 
End
Go
		
		CREATE TABLE test.AutomationLeads
		(
		Role varchar(50),
		IDNumber varchar(50),
		Salutation varchar(50),
		Initials varchar(50),
		FirstNames varchar(50),
		Surname varchar(50),
		PreferredName varchar(50),
		Gender varchar(50),
		MaritalStatus varchar(50),
		PopulationGroup varchar(50),
		Education varchar(50),
		CitizenshipType varchar(50),
		PassportNumber varchar(50),
		TaxNumber varchar(50),
		HomeLanguage varchar(50),
		DocumentLanguage varchar(50),
		Status varchar(50),
		TestIdentifier varchar(50),
		OfferKey int,
		OrigConsultant varchar(50),
		ReAssignedConsultant varchar(50),
		IsIncomeContributor bit,
		TestGroup varchar(50)
		)

