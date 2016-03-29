use [2AM]
go

set ansi_nulls on
go
set quoted_identifier on
go

if exists (select * from sys.objects where object_id = object_id(N'[2AM].[test].[ApproveDisabilityClaim]') and type in (N'P',N'PC'))
drop procedure test.ApproveDisabilityClaim
go

create procedure test.ApproveDisabilityClaim

@disabilityClaimKey int

as
begin try

	begin tran	
	
	declare @numberOfAuthorisedInstalments int = ((abs(CHECKSUM(newid())) % 23) + 1)	

	declare @debitOrderDay int
	declare @lastDateWorked date
	declare @paymentStartDate datetime
	declare @paymentEndDate datetime

	select @lastDateWorked = dc.LastDateWorked, @debitOrderDay = fsb.DebitOrderDay from DisabilityClaim dc
	join Account life on dc.AccountKey = life.AccountKey
	join Account loan on life.ParentAccountKey = loan.AccountKey
	join FinancialService fs on loan.AccountKey=fs.AccountKey
		and fs.FinancialServiceTypeKey = 1
	join FinancialServiceBankAccount fsb on fs.FinancialServiceKey=fsb.FinancialServiceKey
		and fsb.GeneralStatusKey=1
	where dc.DisabilityClaimKey=@disabilityClaimKey

	declare @monthsToAdd int = 4
	if (datepart(dd, @lastDateWorked) <= @debitOrderDay)
		begin
			select @monthsToAdd = 3
		end

	select @paymentStartDate = dateadd(mm,@monthsToAdd, @lastDateWorked)
	select @paymentStartDate = datefromparts(datepart(yy, @paymentStartDate), datepart(mm, @paymentStartDate), @debitOrderDay)

	select @paymentEndDate = (dateadd(mm, -1, dateadd(mm, @numberOfAuthorisedInstalments, @paymentStartDate)))

	update [2AM].dbo.DisabilityClaim
	set NumberOfInstalmentsAuthorised = @numberOfAuthorisedInstalments,
	PaymentStartDate = @paymentStartDate,
	PaymentEndDate = @paymentEndDate,
	DisabilityClaimStatusKey = 3	
	where DisabilityClaimKey = @disabilityClaimKey

	declare @Ret int
	declare @Msg varchar(1024)
	execute @Ret = Process.halo.pApproveLifeDisabilityPayment @disabilityClaimKey, 'X2', @Msg output

	if (@Ret <> 0 or isnull(@Msg, '') <> '')
	begin
		raiserror(@Msg, 16, 1)
	end

	commit tran

end try

begin catch

	if @@trancount > 0
		rollback tran	

end catch
go