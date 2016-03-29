USE [2AM]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID(N'[test].[InsertCounterProposalByStatus]', 'P') IS NOT NULL
	DROP PROCEDURE [test].[InsertCounterProposalByStatus] 
GO 

-- =============================================
-- Description:	The proc inserts a counter proposal record and the proposal items for a debt counselling case.
--
-- Parameters:
--		@debtCounsellingKey	(debt counselling case)
--		@proposalStatusKey	(Active,Inactive,Draft)
--		@proposalItems		(number of proposal items)					
-- =============================================

CREATE PROCEDURE [test].[InsertCounterProposalByStatus]
	@debtCounsellingKey int,
	@proposalStatusKey int,
	@proposalItems decimal=1
	
AS
BEGIN

SET NOCOUNT ON;
	
declare @aduserKey int,
		@payment float,
		@interestRate float,
		@remainingTerm decimal,
		@interval int,
		@proposalKey int,
		@paymentLife float,
		@paymentHOC float,
		@serviceFee float,
		@accountKey int,
		@balance float	

--delete the existing counter proposal of the same status
exec [2AM].test.DeleteProposal @debtCounsellingKey, 2, @proposalStatusKey

select 
	@interestRate=lb.interestRate,
	@remainingTerm=lb.remainingInstalments,
	@accountKey = a.accountKey,
	@balance = b.Amount
from [2AM].debtcounselling.debtCounselling dc
join [2AM].dbo.account a 
	on dc.accountKey=a.accountKey
join [2AM].dbo.financialService fs 
	on a.accountKey=fs.accountKey
	and fs.accountStatusKey=1
	and fs.financialservicetypekey = 1
join [2AM].fin.loanBalance lb
	on fs.financialServiceKey=lb.financialServiceKey
join [2AM].fin.Balance b on fs.financialServiceKey = b.financialServiceKey
where dc.debtCounsellingKey=@debtCounsellingKey


set @payment = (select dbo.fLoanCalcInstalment(@balance, @interestrate, 12, @remainingTerm))

--get child accounts payment - HOC
select @paymentHOC = dbo.fGetHOCMonthlyPremium(a.accountKey)
from [2am].dbo.account a 
where parentAccountKey = @accountKey
and rrr_productkey=3
and a.accountStatusKey=1
--get child accounts payment - Life
select @paymentLife = Process.halo.fLifeGetMonthlyPremium(a.accountKey)
from [2am].dbo.account a 
where parentAccountKey = @accountKey
and rrr_productkey=4 
and a.accountStatusKey=1
--get fee
select @serviceFee = isnull(f.Amount,0) from [2am].dbo.account a
join [2am].dbo.financialservice fs on a.accountkey=fs.accountkey
left join [2am].fin.fee f on fs.financialservicekey = f.financialservicekey
	and f.generalStatusKey=1
where a.accountkey = @accountKey

set @payment = @payment + isnull(@paymentHOC,0) + isnull(@paymentLife,0) + isnull(@serviceFee,0)

select top 1 @aduserKey=a.aduserKey from [2AM].dbo.aduser a
where adusername = 'SAHL\DCCUser'

select @interval = ceiling(@remainingTerm / @proposalItems)

--insert the proposal record
insert into [2AM].debtcounselling.proposal
(proposaltypekey,proposalstatuskey,debtcounsellingkey,hocinclusive,
lifeinclusive,aduserkey,createdate,reviewdate,accepted)
values
(2,@proposalStatusKey,@debtCounsellingKey,1,1,@aduserKey,getdate(),NULL,NULL)

set @proposalKey = scope_identity() 

--insert the proposal item records
while @proposalItems > 0 

	begin
		
		set @proposalItems = @proposalItems - 1
		
		if (@remainingTerm - @interval) >= 0
			begin
				set @remainingTerm = @remainingTerm - @interval
			end
		else
			begin
				set @interval = @remainingTerm
			end
			
		insert into [2AM].debtcounselling.proposalItem
		(proposalKey,startDate,endDate,marketRateKey,interestRate,amount,additionalAmount,
		aduserKey,createDate,instalmentPercent,annualEscalation,startPeriod,endPeriod)
		select
			a.proposalKey,
			a.startDate,
			dateadd(dd,-1,dateadd(mm,@interval,a.startDate)),
			NULL,
			@interestRate,
			@payment,
			0,
			@aduserKey,
			getdate(),
			1,
			NULL,
			a.startPeriod,
			(a.startPeriod + (@interval-1)) 
		from
		(
		select 
		p.proposalKey,
		ISNULL(dateadd(dd,1,max(i.endDate)),getdate()) as startDate,
		ISNULL(max(i.endPeriod)+1,1) as startPeriod
		from [2AM].debtcounselling.proposal p
		left join [2AM].debtcounselling.proposalItem i
			on p.proposalKey=i.proposalKey
		where debtcounsellingKey=@debtCounsellingKey
		and p.proposalKey=@proposalKey
		and proposalTypeKey=2
		and proposalStatusKey=@proposalStatusKey
		group by p.proposalKey
		) as a
	end
		
update [2AM].debtcounselling.proposalItem
set	startdate = rtrim(convert(varchar(16), startdate, 102))+' 00:00:00.000',
	enddate = rtrim(convert(varchar(16), enddate, 102))+' 00:00:00.000' 		
where proposalKey=@proposalKey

--insert a counter proposal comment
insert into [2am].dbo.Memo
(genericKeyTypeKey, GenericKey, InsertedDate, Memo, AduserKey, ChangeDate, generalStatusKey, reminderDate, expiryDate)
values 
(28, @proposalKey, getdate(), 'Counter Proposal added by automation testing',@aduserKey, null, 1, null, null) 

if @proposalStatusKey = 1
	
	begin
		
		declare @reasonDefinitionKey int

		select top 1
		@reasonDefinitionKey=reasondefinitionkey
		from [2AM].dbo.reasondefinition
		where reasontypekey=46
		and generalStatusKey=1

		insert into [2AM].dbo.reason
		(reasondefinitionkey, generickey, comment, stagetransitionkey)
		values
		(@reasonDefinitionKey, @proposalKey, 'Capture counter proposal reason.', NULL)
	
	end	

END
GO