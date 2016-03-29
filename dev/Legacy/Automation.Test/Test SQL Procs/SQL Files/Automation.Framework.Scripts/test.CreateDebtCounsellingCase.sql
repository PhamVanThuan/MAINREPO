USE [2AM]
GO
/****** 
Object:  StoredProcedure [test].[CreateDebtCounsellingCase]    
Script Date: 10/19/2010 16:48:58 
******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[CreateDebtCounsellingCase]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].CreateDebtCounsellingCase

GO

create procedure test.CreateDebtCounsellingCase

@AccountKey int

as

--variable declaration
declare @debtCounsellingGroupKey int
declare @debtCounsellingKey int
declare @debtCounsellorLegalEntityKey int
declare @adUserKey int
declare @pdaLegalEntityKey int

select @adUserKey = adUserKey from dbo.ADUser
where adUserName = 'SAHL\ClintonS'

set @debtCounsellingGroupKey = 0

--check for existing case for LE
select @debtCounsellingGroupKey = debtcounsellinggroupkey from dbo.account a
join dbo.role r on a.accountkey=r.accountkey
join dbo.legalentity le on r.legalentitykey = le.legalentitykey
	and le.legalEntityTypeKey = 2
join dbo.role other on r.legalentitykey = other.legalentitykey
	and other.accountkey != @AccountKey
join dbo.account a_other on other.accountkey = a_other.accountkey
	and a_other.rrr_productkey in (1,2,5,6,9,11)
join debtcounselling.debtcounselling on a_other.accountkey = debtcounselling.accountkey
	and debtcounselling.debtcounsellingstatuskey=1
where a.accountKey = @AccountKey

if @debtCounsellingGroupKey = 0
begin
	--step 1: we need a new group key
	insert into debtcounselling.debtcounsellingGroup
	(CreatedDate)
	values
	(getdate())
	set @debtCounsellingGroupKey = scope_identity()
end

--step 2: insert into the debtcounselling.debtcounselling table
insert into debtCounselling.debtCounselling
(debtCounsellingGroupKey, AccountKey, debtCounsellingStatusKey, ReferenceNumber)
values 
(@debtCounsellingGroupKey, @accountKey, 1, 'AutomationTestCase')
set @debtCounsellingKey = scope_identity()

--step 3: get a debt counsellor and PDA
select top 1 @debtCounsellorLegalEntityKey = d.legalEntityKey 
from debtcounselling.debtcounsellordetail d
join dbo.legalEntityOrganisationStructure leos on d.legalEntityKey = leos.legalEntityKey
where len(NCRDCRegistrationNumber) > 0

select top 1 @pdaLegalEntityKey = leos.legalentitykey from legalEntityOrganisationStructure leos
join organisationStructure os on leos.organisationStructureKey = os.organisationStructureKey
join legalentity le on leos.legalentitykey = le.legalentitykey
where parentKey = 4728

--step 4: insert into externalRole table for Client, Debt Counsellor and PDA
--clients (active main applicants only)
insert into dbo.ExternalRole
(GenericKey, GenericKeyTypeKey, legalEntityKey, ExternalRoleTypeKey, GeneralStatusKey, ChangeDate)
select @debtCounsellingKey, 27, r.legalEntityKey,  1, 1, getdate() 
from [2am].dbo.role r
join legalEntity le on r.legalEntityKey = le.legalEntityKey
	and le.legalEntityTypeKey = 2
where accountkey= @accountKey
and r.generalStatusKey = 1 and r.roleTypeKey = 2
--debt counsellor
insert into dbo.ExternalRole
(GenericKey, GenericKeyTypeKey, legalEntityKey, ExternalRoleTypeKey, GeneralStatusKey, ChangeDate)
values
(@debtCounsellingKey, 27, @debtCounsellorLegalEntityKey, 2, 1, getdate())
--PDA
 insert into dbo.ExternalRole
(GenericKey, GenericKeyTypeKey, legalEntityKey, ExternalRoleTypeKey, GeneralStatusKey, ChangeDate)
values
(@debtCounsellingKey, 27, @pdaLegalEntityKey, 3, 1, getdate())

--step 5: insert 17.1 date
declare @17pt1SDSDGKey int
set @17pt1SDSDGKey = 4445
insert into dbo.StageTransition
(genericKey, aduserKey, transitionDate, Comments, StageDefinitionStageDefinitionGroupKey,
EndTransitionDate)
values
(@debtCounsellingKey, @adUserKey, getdate(), 'Test Script Insert', @17pt1SDSDGKey, getdate())

--step 6: update the automation table
update test.AutomationDebtCounsellingTestCases
set debtCounsellingKey = @debtCounsellingKey
where accountKey = @accountKey