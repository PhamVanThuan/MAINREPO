USE [2AM]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[pTestSetUpHOCAuditData]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [test].[pTestSetUpHOCAuditData]

GO

CREATE PROC [test].[pTestSetUpHOCAuditData]
AS
BEGIN

	if object_id('tempdb..#HOCIIXSetUpAuditData','U') is null
	begin
		create table #HOCIIXSetUpAuditData(ID int IDENTITY(1,1), AccKey int, HOCKey int, status varchar(10))
	end

	insert into #HOCIIXSetUpAuditData
	select distinct top 11 a.ParentAccountKey, h.FinancialServiceKey, 'U'
	from Account a
	inner join role r
		on r.AccountKey = a.AccountKey		
	inner join FinancialService fs
		on a.AccountKey = fs.AccountKey		
	inner join hoc h
		on h.FinancialServiceKey = fs.FinancialServiceKey							
	inner join FinancialService mfs
		on fs.ParentFinancialServiceKey = mfs.FinancialServiceKey
	inner join Account ma
		on ma.AccountKey = mfs.AccountKey
	inner join fin.MortgageLoan ml
		on mfs.FinancialServiceKey = ml.FinancialServiceKey		
	inner join Property p
		on ml.PropertyKey = p.PropertyKey		
	where 
		a.RRR_ProductKey = 3 
			and 
		ma.AccountStatusKey = 1 
			and 
		a.AccountStatusKey = 1 
			and 
		h.HOCInsurerKey in (2)
			and 
		p.TitleTypeKey not in (3)
			and 
		fs.FinancialServiceTypeKey = 4

	declare @counter int
	declare @rowcount int
	declare @HOCKey int
	declare @AccKey int

	select @rowcount = count(*) from #HOCIIXSetUpAuditData
	select @counter = 1

	while (@counter <= @rowcount)
	begin
		
		select @AccKey = AccKey, @HOCKey = HOCKey from #HOCIIXSetUpAuditData where ID = @counter
		SELECT @counter
		SELECT @AccKey
		SELECT @HOCKey
		update h
				set 
					h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
					h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1))
		FROM hoc h
		WHERE h.FinancialServiceKey = @HOCKey

		update h
				set 
					h.HOCMonthlyPremium = (h.HOCMonthlyPremium  + ((h.HOCMonthlyPremium + 100.00)  * 0.1)),
					h.HOCProrataPremium = (h.HOCProrataPremium + ((h.HOCProrataPremium  +100.00) * 0.1))
		FROM hoc h
		WHERE h.FinancialServiceKey = @HOCKey		

		DELETE FROM hoctransactions WHERE AccountKey = @AccKey

		insert into [2am].dbo.HOCTransactions (AccountKey,[Action], Reason, InsertDate)
		values(@AccKey,'A',NULL,getdate()-5)

		insert into [2am].dbo.HOCTransactions (AccountKey,[Action], Reason, InsertDate)
		values(@AccKey,'I',NULL,getdate()-4)
		
		if (@counter = 9)
		begin

			DELETE FROM hoctransactions WHERE AccountKey = @AccKey

			update #HOCIIXSetUpAuditData
			set status = 'A'
			where ID = @counter
		end
		
		if (@counter = 10)
		begin

			update hoc set hocinsurerKey = 5 where FinancialServiceKey = @HOCKey

			update #HOCIIXSetUpAuditData
			set status = 'R'
			where ID = @counter
		end

		set @counter = @counter + 1
	end

	select * from #HOCIIXSetUpAuditData
END

