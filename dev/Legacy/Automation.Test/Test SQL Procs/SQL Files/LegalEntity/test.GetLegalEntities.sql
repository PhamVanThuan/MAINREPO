USE [2AM]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetLegalEntities]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetLegalEntities]
	Print 'Dropped procedure [test].[GetLegalEntities]'
End
Go
--exec test.getlegalentities 1,2, default
CREATE PROCEDURE [test].[GetLegalEntities]
	@citizenShipTypeKey int,
	@legalEntityTypeKey int,
	@legalEntityExceptionStatusKey int
AS
BEGIN

--Get the legalentity exceptions
--declare @legalentityExceptions table
--(
--	LegalEntityKey	int,
--	LegalEntityExceptionReasonKey int,
--	LegalEntityExceptionKey	int,
--	Description	varchar(250),
--	Priority tinyint
--)
--insert into @legalentityExceptions
--select leExc.*,leExcReason.description,leExcReason.priority from dbo.legalentityexception as leExc
--	inner join dbo.legalentityexceptionreason as leExcReason
--		on leExc.legalentityexceptionreasonkey = leExcReason.legalentityexceptionreasonkey
--order by priority asc
if (@legalEntityTypeKey =2 and @legalEntityExceptionStatusKey != 0)
	begin
		select DISTINCT top 100
			le.*,
			let.Description as LegalEntityTypeDescription,
			ms.Description as MaritalStatusDescription,
			g.Description as GenderDescription,
			pg.Description as PopulationGroupDescription,
			st.Description as SalutationDescription,
			ct.Description as CitizenTypeDescription,
			les.Description as LegalEntityStatusDescription,
			e.Description as EducationDescription,
			l.Description as HomeLanguageDescription,
			dl.Description as DocumentLanguageDescription
		from dbo.Account a 
			join dbo.Role r on a.accountKey = r.AccountKey
				and r.roleTypeKey = 2
			join dbo.LegalEntity le on r.legalEntityKey = le.legalEntityKey
			inner join dbo.LegalEntityType let on le.LegalEntityTypeKey = let.LegalEntityTypeKey
			inner join dbo.CitizenType ct on le.CitizenTypeKey = ct.CitizenTypeKey
			inner join dbo.LegalEntityStatus les on le.LegalEntityStatusKey = les.LegalEntityStatusKey
			inner join dbo.Language dl on le.DocumentLanguageKey = dl.LanguageKey
			left join dbo.MaritalStatus ms on le.MaritalStatusKey = ms.MaritalStatusKey
			left join dbo.Gender g on le.GenderKey = g.GenderKey
			left join dbo.PopulationGroup pg on le.PopulationGroupKey = pg.PopulationGroupKey	
			left join dbo.SalutationType st	on le.SalutationKey = st.SalutationKey	    
			left join dbo.Education e on le.EducationKey= e.EducationKey        
			left join dbo.Language l on le.HomeLanguageKey = l.LanguageKey
		where 
		a.AccountStatusKey = 1
		and ct.CitizenTypeKey = @citizenShipTypeKey 
		and le.legalEntityExceptionStatusKey = @legalEntityExceptionStatusKey
		and le.LegalEntityTypeKey = @legalEntityTypeKey
		and len(le.Idnumber) = 13
	end
if (@legalEntityTypeKey =2 and @legalEntityExceptionStatusKey = 0)
	begin
		select DISTINCT top 100
			le.*,
			let.Description as LegalEntityTypeDescription,
			ms.Description as MaritalStatusDescription,
			g.Description as GenderDescription,
			pg.Description as PopulationGroupDescription,
			st.Description as SalutationDescription,
			ct.Description as CitizenTypeDescription,
			les.Description as LegalEntityStatusDescription,
			e.Description as EducationDescription,
			l.Description as HomeLanguageDescription,
			dl.Description as DocumentLanguageDescription
		from 
			dbo.Account a 
			join dbo.Role r on a.accountKey = r.AccountKey
				and r.roleTypeKey = 2
			join dbo.LegalEntity le on r.legalEntityKey = le.legalEntityKey
			inner join dbo.LegalEntityType let on le.LegalEntityTypeKey = let.LegalEntityTypeKey
			inner join dbo.CitizenType ct on le.CitizenTypeKey = ct.CitizenTypeKey
			inner join dbo.LegalEntityStatus les on le.LegalEntityStatusKey = les.LegalEntityStatusKey
			inner join dbo.Language dl on le.DocumentLanguageKey = dl.LanguageKey
			left join dbo.MaritalStatus ms on le.MaritalStatusKey = ms.MaritalStatusKey
			left join dbo.Gender g on le.GenderKey = g.GenderKey
			left join dbo.PopulationGroup pg on le.PopulationGroupKey = pg.PopulationGroupKey	
			left join dbo.SalutationType st	on le.SalutationKey = st.SalutationKey	    
			left join dbo.Education e on le.EducationKey= e.EducationKey        
			left join dbo.Language l on le.HomeLanguageKey = l.LanguageKey
		where
		  a.AccountStatusKey = 1 
		  and ct.CitizenTypeKey = @citizenShipTypeKey 
		  and le.legalEntityExceptionStatusKey is null
		  and le.LegalEntityTypeKey = @legalEntityTypeKey
		  and len(le.Idnumber) = 13
	end
else if (@legalEntityTypeKey != 2)
	begin 
		select DISTINCT top 100
			le.*,
			let.Description as LegalEntityTypeDescription,
			ms.Description as MaritalStatusDescription,
			g.Description as GenderDescription,
			pg.Description as PopulationGroupDescription,
			st.Description as SalutationDescription,
			ct.Description as CitizenTypeDescription,
			les.Description as LegalEntityStatusDescription,
			e.Description as EducationDescription,
			l.Description as HomeLanguageDescription,
			dl.Description as DocumentLanguageDescription
		from dbo.Account a 
			join dbo.Role r on a.accountKey = r.AccountKey
				and r.roleTypeKey = 2
			join dbo.LegalEntity le on r.legalEntityKey = le.legalEntityKey
			inner join dbo.LegalEntityType let on le.LegalEntityTypeKey = let.LegalEntityTypeKey
			inner join dbo.CitizenType ct on le.CitizenTypeKey = ct.CitizenTypeKey
			inner join dbo.LegalEntityStatus les on le.LegalEntityStatusKey = les.LegalEntityStatusKey
			inner join dbo.Language dl on le.DocumentLanguageKey = dl.LanguageKey
			left join dbo.MaritalStatus ms on le.MaritalStatusKey = ms.MaritalStatusKey
			left join dbo.Gender g on le.GenderKey = g.GenderKey
			left join dbo.PopulationGroup pg on le.PopulationGroupKey = pg.PopulationGroupKey	
			left join dbo.SalutationType st	on le.SalutationKey = st.SalutationKey	    
			left join dbo.Education e on le.EducationKey= e.EducationKey        
			left join dbo.Language l on le.HomeLanguageKey = l.LanguageKey
		where a.accountStatusKey = 1 
		and le.LegalEntityTypeKey = @legalEntityTypeKey and len(le.RegistrationNumber) > 4
	end	   
END