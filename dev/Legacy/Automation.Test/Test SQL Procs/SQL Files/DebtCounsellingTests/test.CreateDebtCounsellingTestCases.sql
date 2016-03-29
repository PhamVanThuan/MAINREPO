USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CreateDebtCounsellingTestCases') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CreateDebtCounsellingTestCases
	Print 'Dropped Proc test.CreateDebtCounsellingTestCases'
End
Go

Create Procedure test.CreateDebtCounsellingTestCases

AS

--remove existing data
truncate table test.DebtCounsellingTestCases
truncate table test.DebtCounsellingAccounts
truncate table test.DebtCounsellingLegalEntities
--don't know if these tables get created or used anymore so putting in checks so script will still run if they don't
IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.DebtCounsellingProposals') and OBJECTPROPERTY(id, N'IsTable') = 1)
	truncate table test.DebtCounsellingProposals
IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.DebtCounsellingProposalItems') and OBJECTPROPERTY(id, N'IsTable') = 1)
	truncate table test.DebtCounsellingProposalItems

declare @legalentitykey int
declare @testIdentifier varchar(50)
declare @debtCounsellor varchar(50)
declare @NCRNum varchar(20)
declare @creatorAdUserName varchar(50)
declare @accountkey int
-----------------------------------------------------------------------------------------------------------------------------------------------
select top 1 @NCRNum = NCRDCRegistrationNumber, @debtCounsellor = dbo.legalentitylegalname(legalentitykey,1)
from debtcounselling.debtcounsellordetail
where NCRDCRegistrationNumber is not null and NCRDCRegistrationNumber <> ''
order by newid()

set @creatorAdUserName = 'SAHL\DCAUser1'
----------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_SingleCaseCreate'
select top 1 @legalentitykey = le.legalentitykey
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11)
left join debtcounselling.debtcounselling dc on a.accountkey=dc.accountkey and dc.debtcounsellingstatuskey=1
where a.accountStatusKey=1 and a.accountkey not in (select accountkey from test.DebtCounsellingAccounts)
and dc.accountkey is null
group by le.legalentitykey
having count(a.accountKey) = 1

--we need this persons id number
declare @idnumber varchar(20)
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Create',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey  and r.generalstatuskey=1 
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
case when roleTypeKey in (2, 3) then 1 else 0 end as UnderDebtCounselling 
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc


-------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_MultipleCaseCreate'

select top 1 @legalentitykey = le.legalentitykey
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey  and r.generalstatuskey=1 and r.roletypekey = 2
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join debtcounselling.debtcounselling dc on a.accountkey=dc.accountkey and dc.debtcounsellingstatuskey=1
where a.accountStatusKey=1 and a.accountkey not in (select accountkey from test.DebtCounsellingAccounts)
and dc.accountkey is null
group by le.legalentitykey
having count(a.accountKey) = 3

--we need this persons id number
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Create',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
case when roleTypeKey in (2, 3) then 1 else 0 end as UnderDebtCounselling 
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc
----------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_LEtestCreate'

select top 1 @legalentitykey = max(le.legalentitykey)
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey  and r.generalstatuskey=1 and r.roletypekey = 2
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join debtcounselling.debtcounselling dc on a.accountkey=dc.accountkey and dc.debtcounsellingstatuskey=1
where a.accountStatusKey=1 and a.accountkey not in (select accountkey from test.DebtCounsellingAccounts)
and dc.accountkey is null
group by a.accountkey
having count(r.legalentitykey) = 2

--we need this persons id number
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Create',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
case when row_number() over (partition by r.accountkey order by le.legalentitykey) % 2 = 0 and roleTypeKey in (2, 3) 
then 1 else 0 end as UnderDebtCounselling  
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc
-----------------------------------------------------------------------------------------------------------------------------------------------
create table #LEsWithSingleRole
(legalentitykey int,
roletypekey int
)
insert into #LEsWithSingleRole
select le.legalentitykey,max(roleTypeKey) from legalentity le
join role r on le.legalentitykey=r.legalentitykey
join account a on r.accountkey=a.accountkey and a.rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
and a.accountstatuskey=1
where le.legalentityTypeKey=2
group by le.legalentitykey
having count(le.legalentitykey) = 1

set @testIdentifier = 'DC_CloseCorpTest'

select top 1 @legalentitykey = temp.legalentitykey from #LEsWithSingleRole temp
join role r on temp.legalentitykey=r.legalentitykey
join account a on r.accountkey=a.accountkey and a.rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
and a.accountstatuskey=1
join role r_1 on a.accountkey=r_1.accountkey and r.generalstatuskey=1
join legalentity le on r_1.legalentityKey=le.legalentitykey and le.legalentitytypeKey = 4
where temp.roleTypeKey = 3

--we need this persons id number
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Rule',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
case when r.RoleTypeKey in (2, 3) and le.legalentityTypeKey = 2 then 1 else 0 end as UnderDebtCounselling  
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc

-----------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_CompanyTest'

select top 1 @legalentitykey = temp.legalentitykey from #LEsWithSingleRole temp
join role r on temp.legalentitykey=r.legalentitykey
join account a on r.accountkey=a.accountkey and a.rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
and a.accountstatuskey=1
join role r_1 on a.accountkey=r_1.accountkey and r.generalstatuskey=1
join legalentity le on r_1.legalentityKey=le.legalentitykey and le.legalentitytypeKey = 3
where temp.roleTypeKey = 3

--we need this persons id number
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Rule',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
case when r.RoleTypeKey in (2, 3) and le.legalentityTypeKey = 2 then 1 else 0 end as UnderDebtCounselling   
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc
-----------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_TrustTest'

select top 1 @legalentitykey = temp.legalentitykey from #LEsWithSingleRole temp
join role r on temp.legalentitykey=r.legalentitykey
join account a on r.accountkey=a.accountkey and a.rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
and a.accountstatuskey=1
join role r_1 on a.accountkey=r_1.accountkey and r.generalstatuskey=1
join legalentity le on r_1.legalentityKey=le.legalentitykey and le.legalentitytypeKey = 5
where temp.roleTypeKey = 3

--we need this persons id number
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Rule',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
case when r.RoleTypeKey in (2, 3) and le.legalentityTypeKey = 2 then 1 else 0 end as UnderDebtCounselling  
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc
-----------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_RCSSearchTest'
select top 1 @legalentitykey = temp.legalentitykey from #LEsWithSingleRole temp
	inner join dbo.role 
		on temp.legalentitykey = role.legalentitykey		
	inner join dbo.account
		on role.accountkey = account.accountkey
where account.rrr_originationsourcekey = 4 and account.accountstatuskey = 1	

--we need this persons id number
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Search',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
0 as UnderDebtCounselling  
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc
-----------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_SAHLSearchTest'
select top 1 @legalentitykey = temp.legalentitykey from #LEsWithSingleRole temp
	inner join dbo.role 
		on temp.legalentitykey = role.legalentitykey		
	inner join dbo.account
		on role.accountkey = account.accountkey
where account.rrr_originationsourcekey = 1 and account.accountstatuskey = 1
--we need this persons id number
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Search',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
0 as UnderDebtCounselling  
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc
-----------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_SAHLRCSSearchTest'
select top 1 @legalentitykey = temp.legalentitykey from #LEsWithSingleRole temp
	--SAHL
	inner join
		(
			select 	role.legalentitykey, account.accountkey	from dbo.role 
				inner join dbo.account
					on role.accountkey = account.accountkey
					and account.rrr_originationsourcekey = 1
					and account.accountstatuskey = 1
		) as SAHLAccount
			on temp.legalentitykey = SAHLAccount.legalentitykey
	-- RCS
	inner join
		(
			select 	role.legalentitykey, account.accountkey	from dbo.role 
					inner join dbo.account
						on role.accountkey = account.accountkey
						and account.rrr_originationsourcekey = 4
						and account.accountstatuskey = 1
		) as RCSAccount
			on temp.legalentitykey = RCSAccount.legalentitykey		

--we need this persons id number
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Search',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
0 as UnderDebtCounselling  
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc
-----------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_DuplicateCaseCreate_Original'
select top 1 @legalentitykey = le.legalentitykey
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join debtcounselling.debtcounselling dc on a.accountkey=dc.accountkey and dc.debtcounsellingstatuskey=1
where a.accountStatusKey=1 and a.accountkey not in (select accountkey from test.DebtCounsellingAccounts)
and dc.accountkey is null
group by le.legalentitykey
having count(a.accountKey) = 1

--we need this persons id number
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Rule',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey  and r.generalstatuskey=1 
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
case when roleTypeKey in (2, 3) then 1 else 0 end as UnderDebtCounselling 
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc

-----------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_DuplicateCaseCreate_Second'
--select the legalentity from the original case (DC_DuplicateCaseCreate_Original) where the UnderDebtCounselling indicator = 0
select top 1 @legalentitykey = DebtCounsellingLegalEntities.legalentitykey from test.DebtCounsellingAccounts
	inner join test.DebtCounsellingLegalEntities
		on DebtCounsellingAccounts.AccountKey = DebtCounsellingLegalEntities.AccountKey
	inner join dbo.role
		on DebtCounsellingLegalEntities.legalentitykey = role.legalentitykey
		and role.roletypekey = 2
where DebtCounsellingAccounts.TestIdentifier ='DC_DuplicateCaseCreateOriginal' 
and DebtCounsellingLegalEntities.UnderDebtCounselling = 0

--use the idnumber of one of the 
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Rule',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey  and r.generalstatuskey=1 
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

-------------------------------------------------------------------------------------------------------------------------------------------
set @testIdentifier = 'DC_PersonalLoanCreate'

select top 1 @legalentitykey = le.legalentitykey
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1 and r.roletypekey = 2
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey=12 and a.accountStatusKey=1
left join debtcounselling.debtcounselling dc on a.accountkey=dc.accountkey and dc.debtcounsellingstatuskey=1
where a.accountStatusKey=1 and a.accountkey not in (select accountkey from test.DebtCounsellingAccounts)
and dc.accountkey is null
group by le.legalentitykey
having count(a.accountKey) = 1

--we need this persons id number
select @idnumber = (case when citizentypekey = 3 then passportnumber else isnull(idnumber,passportnumber) end) 
from [2am].dbo.legalentity where legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingTestCases
(
TestIdentifier,
TestGroup,
IDNumber,
NCRDCRegNumber,
DebtCounsellorName,
CreatorADUserName,
CurrentCaseOwner
)
values
(
@testIdentifier,
'Create',
@idnumber,
@NCRNum,
@debtCounsellor,
@creatorAdUserName,
@creatorAdUserName
)

insert into [2am].test.DebtCounsellingAccounts
select distinct @testidentifier, r.accountkey, null, loss.UserToDo, loss.efolderid, loss.eStageName
from [2am].dbo.legalentity le
join [2am].dbo.role r on le.legalentitykey=r.legalentitykey and r.generalstatuskey=1
join [2am].dbo.account a on r.accountkey=a.accountkey and rrr_productkey in (1,2,5,6,9,11,12) and a.accountStatusKey=1
left join test.losscontroleworkcases loss on a.accountkey=loss.accountkey
where le.legalentitykey=@legalentitykey

insert into [2am].test.DebtCounsellingLegalEntities
select r.accountkey,le.legalentitykey, dbo.legalentitylegalname(le.legalentitykey,0) as LegalName,
case when roleTypeKey in (2, 3) then 1 else 0 end as UnderDebtCounselling 
from [2am].test.DebtCounsellingAccounts dc
join [2am]..Role r on dc.accountkey=r.accountkey and r.generalstatuskey=1
join [2am].dbo.legalentity le on r.legalentitykey=le.legalentitykey
where testIdentifier=@testIdentifier
order by r.accountkey asc
----------------------------------------------------------------------------------------------------------------------------------------------
drop table #LEsWithSingleRole

