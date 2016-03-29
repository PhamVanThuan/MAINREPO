USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.StartFinancialAdjustment') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.StartFinancialAdjustment
	Print 'Dropped procedure test.StartFinancialAdjustment'
End
Go

CREATE PROCEDURE test.StartFinancialAdjustment

@accountKey int,
@financialAdjustmentTypeSourceKey int,
@discount float

AS

begin try

begin tran

declare @financialServiceKey int
declare @financialAdjustmentTypeKey int
declare @financialAdjustmentSourceKey int
declare @financialAdjustmentKey int
DECLARE @RC int
DECLARE @UserId varchar(30)
DECLARE @Msg varchar(1024)

SET @UserID = 'SAHL\ClintonS'

select @financialAdjustmentTypeKey = financialAdjustmentTypeKey, @financialAdjustmentSourceKey = financialAdjustmentSourceKey
from [2am].fin.[FinancialAdjustmentTypeSource] fats 
where fats.financialAdjustmentTypeSourceKey = @financialAdjustmentTypeSourceKey 

select @financialServiceKey = financialServiceKey from [2am].dbo.financialService fs 
where fs.accountKey=@accountKey and fs.financialServiceTypeKey = 1

--for non performing lets rather use the opt in
if (@financialAdjustmentTypeSourceKey = 13)
 begin
		EXECUTE @RC = [Process].[halo].[pNonPerformingOptIn] 
		   @AccountKey
		  ,@UserId
		  ,@Msg OUTPUT
		
		if @RC > 0 or isnull(@Msg, '') <> ''		
			begin		
				raiserror(@Msg, 16, 1)			
			end			
 end
--staff 
else if (@financialAdjustmentTypeSourceKey = 3)
	begin
		if not exists(select 1 from [2am].dbo.Detail where accountKey = @AccountKey and detailTypeKey = 237)
		begin
			insert into [2am].dbo.Detail
			(DetailTypeKey, AccountKey, DetailDate, UserID, ChangeDate)
			values
			(237, @AccountKey, getdate(), 'SAHL\ClintonS', getdate())
		end
	
		EXECUTE @RC = [Process].[halo].[pStaffOptIn] 
		   @AccountKey
		  ,@UserId
		  ,@Msg OUTPUT
		
		if @RC > 0 or isnull(@Msg, '') <> ''		
			begin		
				raiserror(@Msg, 16, 1)			
			end						
	end 
else if (@financialAdjustmentTypeSourceKey = 4)
	begin
		EXECUTE @RC = [Process].[halo].[pDefendingDiscountOptIn] 
		   @AccountKey
		  ,@UserId
		  ,@Msg OUTPUT
		
		if @RC > 0 or isnull(@Msg, '') <> ''		
			begin		
				raiserror(@Msg, 16, 1)			
			end						
	end
else if (@financialAdjustmentTypeSourceKey = 5)
	begin
	
	declare @startDate datetime
	declare @endDate datetime
	set @startDate = getdate()
	set @endDate = dateadd(mm, +240, getdate())
	
		EXECUTE @RC = [Process].[halo].[pInterestOnlyOptIn] 
		  @AccountKey,
		  @UserId,
		  @startDate,
		  @endDate,
		  @financialAdjustmentSourceKey,
		  @Msg OUTPUT
		
		if @RC > 0 or isnull(@Msg, '') <> ''		
			begin		
				raiserror(@Msg, 16, 1)			
			end						
	end
else if (@financialAdjustmentTypeSourceKey in (8,32))
	begin
	
	EXECUTE @RC = [Process].[halo].[pDiscountedLinkRateOptIn]
		@FinancialServiceKey,
		@financialAdjustmentSourceKey,
		@discount,
		@UserId,
		@Msg OUTPUT
		
		if @RC > 0 or isnull(@Msg, '') <> ''		
		begin		
			raiserror(@Msg, 16, 1)			
		end			
		
	end
	
--we need to start what we have added
DECLARE @StartStop int
SET @StartStop  = 1

EXECUTE @RC = [Process].[batch].[pFinancialAdjustmentsTopLevelHandler] 
   @StartStop

if @RC > 0 or isnull(@Msg, '') <> ''		
begin		
	raiserror(@Msg, 16, 1)			
end		

commit

end try

begin catch
	select error_message()
	if @@trancount > 0 rollback
end catch
		