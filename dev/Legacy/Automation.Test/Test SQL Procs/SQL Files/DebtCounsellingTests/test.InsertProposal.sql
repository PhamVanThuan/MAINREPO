use [2am]
go

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertProposal') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	Begin
		Drop Procedure test.InsertProposal
		Print 'Dropped Proc test.InsertProposal'
	End
Go

CREATE PROCEDURE [test].[InsertProposal]
	@debtcounsellingkey int,
	@proposalstatuskey int,
	@adusername varchar(50),
	@proposalitems float=1,
	@hocinclusive int=0,
	@lifeinclusive int=0,
	@feesinclusive int=0
	
AS
begin

declare 
	@aduserkey int,
	@payment float,
	@interestrate float,
	@totalterm float,
	@interval int,
	@paymentLife float,
	@paymentHOC float,
	@serviceFee float,
	@accountKey int,
	@balance float
	
	set @paymentLife = 0
	set @paymentHOC = 0
	set @serviceFee = 0

exec test.DeleteProposal @debtcounsellingkey, 1, @proposalstatuskey

select 
	@interestrate = lb.interestrate, 
	@totalterm = lb.remaininginstalments,
	@accountKey = a.accountKey,
	@balance = b.Amount
from 
	[2am].debtcounselling.debtcounselling dc
	join [2am].dbo.account a on dc.accountkey = a.accountkey
	join [2am].dbo.financialservice fs on a.accountkey = fs.accountkey
		and fs.accountStatusKey=1
		and fs.financialservicetypekey = 1
	join [2am].fin.mortgageloan ml on fs.financialservicekey = ml.financialservicekey
	join [2am].fin.loanbalance lb on lb.financialservicekey=ml.financialservicekey
	join [2am].fin.Balance b on fs.financialservicekey = b.financialservicekey
where 
	dc.debtcounsellingkey = @debtcounsellingkey

set @payment = (select dbo.fLoanCalcInstalment(@balance, @interestrate, 12, @totalterm))

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

if (@hocinclusive = 1)
	begin
		set @payment = @payment + isnull(@paymentHOC,0)
	end
if (@lifeInclusive = 1)
	begin
		set @payment = @payment + isnull(@paymentLife,0)
	end
if (@feesinclusive = 1)
	begin
		set @payment = @payment + isnull(@serviceFee,0)
	end

set @interval = ceiling(@totalterm / @proposalitems)
print @interval

select top 1 @aduserKey=a.aduserKey from [2AM].dbo.aduser a
where adusername = @adusername

declare @ins_proposalkey int

insert into [2am].debtcounselling.proposal 
	(proposaltypekey, 
	proposalstatuskey, 
	debtcounsellingkey, 
	hocinclusive, 
	lifeinclusive, 
	aduserkey, 
	createdate, 
	reviewdate, 
	accepted,
	monthlyservicefee)
values 
	(1, 
	@proposalstatuskey, 
	@debtcounsellingkey, 
	@hocinclusive, 
	@lifeinclusive, 
	@aduserkey, 
	getdate(), 
	null, 
	null,
	@feesinclusive)

set @ins_proposalkey = scope_identity()

WHILE @proposalitems > 0
BEGIN
	set @proposalitems = @proposalitems - 1
	
	if (@totalterm - @interval) >= 0
		begin
			set @totalterm = @totalterm - @interval
		end
	else
		begin
			set @interval = @totalterm
		end

	insert into [2am].debtcounselling.proposalitem 
		(ProposalKey, 
		StartDate, 
		EndDate, 
		MarketRateKey, 
		InterestRate, 
		Amount, 
		AdditionalAmount, 
		ADUserKey, 
		CreateDate, 
		InstalmentPercent, 
		AnnualEscalation,
		StartPeriod,
		EndPeriod)
	select 
		a.proposalkey, 
		a.StartDate, 
		dateadd(dd, -1, dateadd(mm, @interval, a.startdate)), 
		null, 
		@interestrate, 
		@payment, 
		0, 
		@aduserkey, 
		getdate(), 
		1, 
		null, 
		a.startPeriod,
		(a.startPeriod + (@interval-1))
	from
		(select 
			p.proposalKey,
			isnull(dateadd(dd, 1, max(i.endDate)), convert(datetime, '01/' + cast(month(getdate()) as varchar(2)) + '/' + cast(year(getdate()) as varchar(4)), 103)) as startDate,
			isnull(max(i.endPeriod) + 1, 1) as startPeriod 
		from 
			[2am].debtcounselling.proposal p
			left join [2am].debtcounselling.proposalitem i on p.proposalkey = i.proposalkey
		where 
			p.debtcounsellingkey = @debtcounsellingkey
			and p.proposaltypekey = 1
			and p.proposalstatuskey = @proposalstatuskey
		group by 
			p.proposalkey) as a

--	select 'count', @proposalitems
END

update 
	[2am].debtcounselling.proposalItem
set 
	startdate = rtrim(convert(varchar(16), startdate, 102))+' 00:00:00.000',
	enddate = rtrim(convert(varchar(16), enddate, 102))+' 00:00:00.000'
where 
	proposalKey=@ins_proposalkey 

end
GO


