USE [2AM]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[CreateTermChangeTestCases]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].[CreateTermChangeTestCases]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [test].[CreateTermChangeTestCases]

AS
BEGIN

--remove existing data
TRUNCATE TABLE test.TermChange

CREATE TABLE #TermChangeCases
(
[Account] INT,
[Product] INT,
[RateOverride] INT,
[SPVMaxTerm] INT,
[InitialInstalments] INT,
[RemainingInstalments] INT,
[CurrentSPV] INT,
[NewSPV] INT,
[SPVMaxTermIncrease] INT,									
[SAHLMaxTermIncrease] INT,
[InitialRepaymentAge] INT,
[AllowTermChange] INT,
[OpenOffer] INT,
[UnderDebtCounselling] INT
)

INSERT INTO #TermChangeCases
select top 10000
	a.accountkey														AS [Account],
	a.rrr_productkey													AS [Product],
	ISNULL(fsts.financialadjustmenttypesourcekey,0)						AS [RateOverride],
	m.spvmaxterm														AS [SPVMaxTerm],
	lb.term																AS [InitialInstalments],
	lb.remaininginstalments												AS [RemainingInstalments],
	a.spvkey															AS [CurrentSPV],
	c.spvkey															AS [NewSPV],
	m.spvmaxterm-(lb.term-lb.remaininginstalments)						AS [SPVMaxTermIncrease],									
	360-(lb.term-lb.remaininginstalments)								AS [SAHLMaxTermIncrease],
	age.repaymentage													AS [InitialRepaymentAge],
	t.value																AS [AllowTermChange],
	case isnull(o.offerkey,0)
		when 0 then 0
		else 1
	end	 																AS [OpenOffer],
	[2AM].dbo.fIsAccountUnderDebtCounselling(a.accountkey)				AS [UnderDebtCounselling]
from [2AM].dbo.account a (nolock)
join 
	(
	select
		a.accountkey,
		max(datediff(yy,le.dateofbirth,dateadd(mm,lb.term,a.opendate))) as RepaymentAge
	from [2AM].dbo.account a (nolock)
	join [2AM].dbo.financialservice fs (nolock)
		on a.accountkey=fs.accountkey and fs.financialservicetypekey=1
	join [2AM].fin.mortgageloan ml (nolock)
		on fs.financialservicekey=ml.financialservicekey
	join [2AM].fin.loanbalance lb (nolock)
		on ml.financialservicekey=lb.financialservicekey
	join [2AM].dbo.role r (nolock)
		on a.accountkey=r.accountkey
	join [2AM].dbo.legalentity le (nolock)
		on r.legalentitykey=le.legalentitykey
	where a.rrr_productKey in (1,2,5,9)
	group by a.accountkey
	) as age 
	on a.accountkey=age.accountkey
join [2AM].dbo.financialservice fs (nolock)
		on a.accountkey=fs.accountkey and fs.financialservicetypekey=1
join [2AM].fin.mortgageloan ml (nolock)
	on fs.financialservicekey=ml.financialservicekey
join [2AM].fin.loanbalance lb (nolock)
	on ml.financialservicekey=lb.financialservicekey
join [2AM].spv.spv s (nolock)
	on a.spvkey=s.spvkey
join [2AM].spv.spv p (nolock)
	on s.parentspvkey=p.spvkey
join [2AM].spv.spvcompanyoriginatingspv c (nolock)
	on p.spvcompanykey=c.spvcompanykey
join [2AM].spv.spvmandate m (nolock)
	on s.spvkey=m.spvkey
join [2AM].spv.spvattribute t (nolock)
	on s.spvkey=t.spvkey and t.spvattributetypekey=2
left join [2AM].fin.financialadjustment f (nolock)
	on fs.financialservicekey=f.financialservicekey and f.financialadjustmentstatuskey=1
left join [2AM].fin.financialadjustmenttypesource fsts (nolock)	
	on f.financialadjustmenttypekey=fsts.financialadjustmenttypekey
	and f.financialadjustmentsourcekey=fsts.financialadjustmentsourcekey
	and fsts.financialadjustmenttypesourcekey in (2,5)
left join [2AM].dbo.offer o (nolock)
	on a.accountkey=o.accountkey and o.offertypekey in (2,3,4) and o.offerstatuskey=1
left join x2.x2data.loan_adjustments l (nolock)
	on a.accountkey=l.accountkey
where a.accountstatuskey=1
and a.rrr_originationsourcekey=1
and l.accountkey is null

declare @table table (accountkey int)

--1
insert into test.TermChange
select top 1 'ApproveNoSPVTransfer',* 
from #TermChangeCases t
where [InitialInstalments]<[SPVMaxTerm] 
and [SPVMaxTerm]<360
and [Product]<>2
and [OpenOffer]<>1
and [UnderDebtCounselling]<>1
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

--2
insert into test.TermChange
select top 1 'DeclineTermChange',* 
from #TermChangeCases t
where [InitialInstalments]<[SPVMaxTerm] 
and [SPVMaxTerm]<360
and [Product]<>2
and [OpenOffer]<>1
and [UnderDebtCounselling]<>1
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

--3
insert into test.TermChange
select top 1 'TermChangeNoLongerRequired',* 
from #TermChangeCases t
where [InitialInstalments]<[SPVMaxTerm] 
and [SPVMaxTerm]<360
and [Product]<>2
and [OpenOffer]<>1
and [UnderDebtCounselling]<>1
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

--4
insert into test.TermChange
select top 1 'NoTermChangeVarifix',* 
from #TermChangeCases t
where [Product]=2
and [OpenOffer]<>1
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

--5
insert into test.TermChange  
select top 1 'ApproveWithSPVTransfer',* 
from #TermChangeCases t  
where [InitialInstalments]=[SPVMaxTerm]   
and [SPVMaxTerm]=240  
and [Product]<>2
and [OpenOffer]<>1
and [AllowTermChange]=1
and [UnderDebtCounselling]<>1  
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

--6
insert into test.TermChange  
select top 1 'TermChangeRequestTimeout',* 
from #TermChangeCases t  
where [InitialInstalments]=[SPVMaxTerm]   
and [SPVMaxTerm]=240  
and [Product]<>2
and [OpenOffer]<>1
and [AllowTermChange]=1
and [UnderDebtCounselling]<>1  
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

--7
insert into test.TermChange  
select top 1 'TermChangeZeroTerm',* 
from #TermChangeCases t
where [RemainingInstalments]=0
and [OpenOffer]<>1
and [UnderDebtCounselling]<>1
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

--8
insert into test.TermChange  
select top 1 'TermChangeRequestDisagreeApproval',* 
from #TermChangeCases t  
where [InitialInstalments]<[SPVMaxTerm]   
and [SPVMaxTerm]<360  
and [Product]<>2
and [OpenOffer]<>1
and [UnderDebtCounselling]<>1  
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

--9
insert into test.TermChange  
select top 1 'TermChangeFLinProgress',* 
from #TermChangeCases t  
where [OpenOffer]=1
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

--10
insert into test.TermChange  
select top 1 'TermChangeTerm360',* 
from #TermChangeCases t  
where [InitialInstalments]<[SPVMaxTerm]   
and [SPVMaxTerm]<360  
and [Product]<>2
and [OpenOffer]<>1
and [UnderDebtCounselling]<>1
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

--11
insert into test.TermChange
select top 1 'UnderDebtCounselling',* 
from #TermChangeCases t
where [InitialInstalments]<[SPVMaxTerm] 
and [SPVMaxTerm]<360
and [Product]<>2
and [OpenOffer]<>1
and [UnderDebtCounselling]=1
and t.account not in (select accountkey from @table)
order by newid()

insert into @table
select account from test.TermChange t
where t.account not in (select accountkey from @table)

DROP TABLE #TermChangeCases
END
GO  