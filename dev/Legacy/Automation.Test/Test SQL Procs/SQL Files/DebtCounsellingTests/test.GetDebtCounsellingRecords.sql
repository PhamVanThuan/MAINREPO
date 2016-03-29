USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetDebtCounsellingRecords') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetDebtCounsellingRecords 
	Print 'Dropped procedure test.GetDebtCounsellingRecords'
End
Go
--exec test.GetDebtCounsellingRecords 0, 0, 0, 5073
CREATE PROCEDURE test.GetDebtCounsellingRecords 
	@isArchivedCases bit = 0,
	@includeDebtReviewApproved bit = 0,
	@accountKey int = 0,
	@debtCounsellingKey int = 0 
AS

BEGIN

create table #debtCounsellingkeys
(
debtCounsellingKey int,
id bigint,
name varchar(250),
aduserkey int,
adusername varchar(250),
accountKey int
)

declare @debtcounsellingStatusKey int

	if (@isArchivedCases = 1)
		begin
			insert into #debtCounsellingkeys
			select debtCounsellingKey, i.id, s.name, a.aduserkey, a.adusername , d.accountKey  
			from x2.x2data.debt_counselling d
			join x2.x2.instance i on d.instanceid=i.id
				and parentinstanceid is null
			join x2.x2.state s on i.stateid = s.id
			join x2.x2.workflowRoleAssignment wra on i.id = wra.instanceid
				and wra.generalStatusKey = 1
			join [2am].dbo.AdUser a on wra.aduserKey = a.adUserKey
			where s.type = 5
		end
	else if (@isArchivedCases = 0 and @includeDebtReviewApproved = 1)
		begin
		    insert into #debtCounsellingkeys
			select debtCounsellingKey, i.id, s.name, a.aduserkey, a.adusername, d.accountKey    
			from x2.x2data.debt_counselling d
			join x2.x2.instance i on d.instanceid=i.id
				and parentinstanceid is null
			join x2.x2.state s on i.stateid = s.id
			join x2.x2.workflowRoleAssignment wra on i.id = wra.instanceid
				and wra.generalStatusKey = 1
			join [2am].dbo.AdUser a on wra.aduserKey = a.adUserKey
			where s.type != 5
			order by 1
		end
	else if (@isArchivedCases = 0 and @includeDebtReviewApproved = 0)
	begin
	    insert into #debtCounsellingkeys
		select debtCounsellingKey, i.id, s.name, a.aduserkey, a.adusername, d.accountKey  
		from x2.x2data.debt_counselling d
		join x2.x2.instance i on d.instanceid=i.id
			and parentinstanceid is null
		join x2.x2.state s on i.stateid = s.id
		join x2.x2.workflowRoleAssignment wra on i.id = wra.instanceid
				and wra.generalStatusKey = 1
		join [2am].dbo.AdUser a on wra.aduserKey = a.adUserKey
		where s.type <> 5 and s.name <> 'Debt Review Approved'
	end
	if (@AccountKey <> 0)
		begin
			delete from #debtCounsellingkeys where accountKey <> @AccountKey
		end
	if (@DebtCounsellingKey <> 0)
		begin
			delete from #debtCounsellingkeys where debtCounsellingKey <> @DebtCounsellingKey
		end		
	select distinct
		debtcounselling.debtCounsellingkey,
		debtcounselling.debtCounsellingstatuskey,
		#debtCounsellingkeys.name as stagename,
		#debtCounsellingkeys.aduserkey as aduserkey,
		#debtCounsellingkeys.adusername,
		#debtCounsellingkeys.adusername as AssignedUser,
		#debtCounsellingkeys.ID as InstanceID,
		legalentity.registeredname,
		externalrole.externalroletypekey,
		legalentity.idnumber,
		emailaddress,
		legalentity.legalentitykey,
		proposal.ProposalKey,
		proposal.ProposalTypeKey,
		proposal.ProposalStatusKey,
		proposal.HOCInclusive,
		proposal.LifeInclusive,
		proposal.CreateDate	,
		proposal.ReviewDate,
		proposal.Accepted,
		proposal.MonthlyServiceFee,
		account.AccountKey,
		account.FixedPayment,
		account.AccountStatusKey,
		account.InsertedDate,
		account.OriginationSourceProductKey,
		account.OpenDate,
		account.CloseDate,
		account.RRR_ProductKey,
		snapshotaccount.SnapShotAccountKey,
		snapshotaccount.RemainingInstallments,
		snapshotaccount.ProductKey,
		snapshotaccount.ValuationKey,
		snapshotaccount.InsertDate,
		snapshotaccount.HOCPremium,
		snapshotaccount.LifePremium
		into #debtCounselling
	from
		#debtCounsellingkeys 
		join debtcounselling.debtcounselling debtcounselling with (nolock) on #debtCounsellingkeys.debtCounsellingKey = debtcounselling.debtCounsellingKey
		inner join dbo.account account on debtcounselling.accountkey = account.accountkey 
			and account.accountStatusKey=1
		join dbo.externalrole with (nolock)on debtcounselling.debtcounsellingkey = externalrole.generickey 
				and externalRole.generalstatuskey=1
		left join dbo.legalentity with (nolock)	on externalrole.legalentitykey = legalentity.legalentitykey
		left join debtcounselling.proposal	on debtcounselling.debtCounsellingkey = proposal.debtCounsellingkey
			and proposal.proposalstatuskey != 2
		left join debtcounselling.snapshotaccount snapshotaccount with (nolock) on debtcounselling.debtcounsellingkey = snapshotaccount.debtcounsellingkey


drop table #debtCounsellingkeys

select * from #debtCounselling	

drop table #debtCounselling		

END