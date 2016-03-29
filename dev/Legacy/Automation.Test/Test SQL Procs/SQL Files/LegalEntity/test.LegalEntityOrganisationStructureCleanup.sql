USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.LegalEntityOrganisationStructureCleanup') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.LegalEntityOrganisationStructureCleanup
	Print 'Dropped procedure test.LegalEntityOrganisationStructureCleanup'
End
Go

CREATE PROCEDURE test.LegalEntityOrganisationStructureCleanup
	@firstnames_registeredname varchar(250)
as

create table #LegalEntitiesToDelete(legalentitykey int)
create table #OrgStructureToDelete(organisationstructurekey int)

insert #LegalEntitiesToDelete
select 
	le.legalentitykey 
from dbo.legalentityorganisationstructure as leos
		inner join dbo.legalentity as le
			on leos.legalentitykey = le.legalentitykey
		inner join dbo.organisationstructure as os
			on leos.organisationstructurekey = os.organisationstructurekey
where registeredname = @firstnames_registeredname or firstnames = @firstnames_registeredname

delete from dbo.legalEntityAddress
where legalentitykey in (select legalentitykey 
							from #LegalEntitiesToDelete)

delete from dbo.externalrole
where legalentitykey in (select legalentitykey 
							from #LegalEntitiesToDelete)

delete from debtcounselling.DebtCounsellorDetail
where legalentitykey in (select legalentitykey 
							from #LegalEntitiesToDelete)

insert into #OrgStructureToDelete
select organisationstructurekey from dbo.legalentityorganisationstructure
where legalentitykey in (select legalentitykey from #LegalEntitiesToDelete) and organisationstructurekey <> 2340

delete from dbo.legalentityorganisationstructure
where legalentitykey in (select legalentitykey 
							from #LegalEntitiesToDelete)

delete from dbo.organisationstructure
where organisationstructurekey in (select organisationstructurekey from #OrgStructureToDelete) 


delete from dbo.Correspondence
where legalentitykey in (select legalentitykey
							from #LegalEntitiesToDelete)

delete from dbo.legalentity
where legalentitykey in (select legalentitykey 
							from #LegalEntitiesToDelete)

drop table #LegalEntitiesToDelete
drop table #OrgStructureToDelete