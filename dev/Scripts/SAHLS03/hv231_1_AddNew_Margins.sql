use [2am]
go
------------------------------------------------------------------------------ 
/* create helper proc */
------------------------------------------------------------------------------ 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pAddMargin]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[pAddMargin]
GO

CREATE PROC dbo.pAddMargin (@Value float, @Description VARCHAR(50))
AS
BEGIN
	declare @MarginKey int

	if not exists (select 1 from Margin where Value = @Value)
	begin
		insert into Margin([Value],[Description])
		select @Value,@Description
	end

	select @MarginKey = MarginKey from Margin where [value] = @Value

	insert into [2am].dbo.rateConfiguration(MarketRateKey,MarginKey)
	select mr.marketRateKey,@MarginKey
	from [2am].dbo.MarketRate mr (nolock)
	left join [2am].dbo.rateConfiguration rc (nolock)
		on mr.marketRateKey = rc.marketRateKey and rc.MarginKey = @MarginKey
	where  rc.RateConfigurationKey is null 
END

go

declare @value float
declare @margindesc varchar(50)

set @value = 0.033
set @margindesc = '03.30%'
exec dbo.pAddMargin @value,@margindesc

set @value = 0.040
set @margindesc = '04.00%'
exec dbo.pAddMargin @value,@margindesc

set @value = 0.045
set @margindesc = '04.50%'
exec dbo.pAddMargin @value,@margindesc

set @value = 0.049
set @margindesc = '04.90%'
exec dbo.pAddMargin @value,@margindesc

set @value = 0.052
set @margindesc = '05.20%'
exec dbo.pAddMargin @value,@margindesc

set @value = 0.065
set @margindesc = '06.50%'
exec dbo.pAddMargin @value,@margindesc

--- Update bad description if it exists
if exists (select 1 from [2am].dbo.margin where Value=0.0380 and Description='03.8%')
begin
	update [2am].dbo.margin set Description='03.80%' where Value=0.038 and Description='03.8%'
end

go
------------------------------------------------------------------------------ 
/* clean up */
------------------------------------------------------------------------------ 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pAddMargin]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[pAddMargin]
GO