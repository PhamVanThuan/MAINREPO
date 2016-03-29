USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetLifePolicyAccounts') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetLifePolicyAccounts 
	Print 'Dropped Proc test.GetLifePolicyAccounts'
End

go
create procedure test.GetLifePolicyAccounts
	@noRoles int,
	@lifePolicyStatus int
as

---------------------< Get Life Accounts that are linked to mortgage and where Legalentities have life and main applicant roles >----------------
select
	loanAcc.accountkey as loanAcc,
	lifeAcc.accountkey as lifeAcc,
	lifeAssuredRole.roletypekey as lifeAssuredRole,
	mainAppRole.roletypekey as mainAppRole,
	lp.policystatuskey
into #lifePolicies
from dbo.account as loanAcc
	inner join dbo.account as lifeAcc
		on loanAcc.accountkey = lifeAcc.parentaccountkey
	inner join dbo.financialservice as lifeFS
		on lifeAcc.accountkey = lifeFS.accountkey
	inner join dbo.lifepolicy as lp
		on lifeFS.financialservicekey = lp.financialservicekey
	-- Life Assured
	inner join dbo.role as lifeAssuredRole
		on lifeAcc.accountkey = lifeAssuredRole.accountkey
		and lifeAssuredRole.roletypekey = 1
	-- Main Applicant
	inner join dbo.role as mainAppRole
		on loanAcc.accountkey = mainAppRole.accountkey
		and mainAppRole.roletypekey = 2
where loanAcc.rrr_originationsourcekey != 4 
and loanAcc.accountstatuskey not in(6,2)
and lifeAcc.rrr_productkey = 4

---------------------<Make sure that all the accounts selected have x number of lifeassured>---
delete from #lifePolicies
where lifeAcc not in
(
	select lifeAcc from #lifePolicies as lp
	where lp.lifeAcc in
	(
		select accountkey from role
		where accountkey = lp.lifeAcc
			and roletypekey = 1
		group by accountkey
		having count(accountkey) = @noRoles
	)
)
---------------------<Make sure that all the accounts selected have x number of main applicants>---
delete from #lifePolicies
where loanAcc not in
(
	select loanAcc from #lifePolicies as lp
	where lp.loanAcc in
	(
		select accountkey from role
		where accountkey = lp.loanAcc
			and roletypekey = 2 or roletypekey = 3
		group by accountkey
		having count(accountkey) = @noRoles
	)
)
---------------------< Filter on accepted life where the loan has been through instruct attorney and return >----------------
if (@lifePolicyStatus = 2)
begin
	-- Main Applicant
	select top 01 lifeAcc as accountkey from #lifePolicies
		inner join dbo.offer
			on loanAcc = offer.accountkey
		inner join dbo.stagetransition as st
			on offer.offerkey = st.generickey
	where st.stagedefinitionstagedefinitiongroupkey = 2120
	and #lifePolicies.policystatuskey = 2
	order by newid()
	return
end

---------------------< Select the first acocunt that has the desired lifepolicystatus >----------------
select top 01 lifeAcc as accountkey from #lifePolicies
where #lifePolicies.policystatuskey = @lifePolicyStatus
order by newid()
drop table #lifePolicies
go