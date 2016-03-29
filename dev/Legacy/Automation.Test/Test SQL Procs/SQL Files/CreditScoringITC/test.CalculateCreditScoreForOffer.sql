USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CalculateCreditScoreForOffer') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CalculateCreditScoreForOffer
	Print 'Dropped Proc test.CalculateCreditScoreForOffer'
End
Go

--exec test.CalculateCreditScoreForOffer 844306
create procedure test.CalculateCreditScoreForOffer

@offerkey int

as

begin

declare @accountkey int
declare @AT094 varchar(max)
declare @AT107 varchar(max)
declare @AT164 varchar(max)
declare @IN106 varchar(max)
declare @IN141 varchar(max)
declare @OT007 varchar(max)
declare @OT041 varchar(max)
declare @RE105 varchar(max)
declare @RE140 varchar(max)
declare @sbc_sum float
declare @score float
declare @LegalEntity int 
declare @table table (
legalEntityKey int, 
AT094 varchar(max), 
AT107 varchar(max),
AT164 varchar(max),
IN106 varchar(max),
IN141 varchar(max),
OT007 varchar(max),
OT041 varchar(max),
RE105 varchar(max),
RE140 varchar(max)   
)

select @accountkey = reservedaccountkey from [2am].dbo.offer where offerkey=@offerkey;

  WITH XMLNAMESPACES
  ( 'https://secure.transunion.co.za/TUBureau' AS  "TUBureau")
  insert into @table    
  select itc.legalEntityKey, 
  convert(varchar(max),ResponseXML.query('BureauResponse/TUBureau:StandardBatchCharsSB33/TUBureau:AT094/text()')) AT094,  
  convert(varchar(max),ResponseXML.query('BureauResponse/TUBureau:StandardBatchCharsSB33/TUBureau:AT107/text()')) AT107,  
  convert(varchar(max),ResponseXML.query('BureauResponse/TUBureau:StandardBatchCharsSB33/TUBureau:AT164/text()')) AT164,  
  convert(varchar(max),ResponseXML.query('BureauResponse/TUBureau:StandardBatchCharsSB33/TUBureau:IN106/text()')) IN106,  
  convert(varchar(max),ResponseXML.query('BureauResponse/TUBureau:StandardBatchCharsSB33/TUBureau:IN141/text()')) IN141,  
  convert(varchar(max),ResponseXML.query('BureauResponse/TUBureau:StandardBatchCharsSB33/TUBureau:OT007/text()')) OT007,  
  convert(varchar(max),ResponseXML.query('BureauResponse/TUBureau:StandardBatchCharsSB33/TUBureau:OT041/text()')) OT041,  
  convert(varchar(max),ResponseXML.query('BureauResponse/TUBureau:StandardBatchCharsSB33/TUBureau:RE105/text()')) RE105,  
  convert(varchar(max),ResponseXML.query('BureauResponse/TUBureau:StandardBatchCharsSB33/TUBureau:RE140/text()')) RE140
  from [2am].dbo.itc (nolock) where accountkey=@accountkey
  
  --we need to find the result the score card
  
Declare ITCLegalEntity Cursor for
	Select LegalEntityKey
	from @table 

Open ITCLegalEntity;

Fetch next from ITCLegalEntity into @LegalEntity

While @@fetch_status = 0

	begin
  
--set the base score
set @sbc_sum = 3.2828
declare @ScoreCardKey int

select @ScoreCardKey = max(ScoreCardKey) from [2am].dbo.ScoreCard
where GeneralStatusKey=1

--select the values from the table for that ITC  
  select @AT094 = AT094, @AT107 = AT107, @AT164 = AT164, @IN106=IN106, @IN141=IN141, @OT007=OT007, @OT041 = OT041, @RE105 = RE105, @RE140 = RE140
  from @table
  where legalEntityKey = @LegalEntity
  
select @sbc_sum= @sbc_sum + Points from dbo.ScoreCardAttributeRange scar 
join dbo.ScoreCardAttribute sca on scar.ScoreCardAttributekey=
sca.ScoreCardAttributekey
where ScoreCardKey = @ScoreCardKey 
and code = 'AT094' and (@AT094 >= scar.Min and (@AT094 <= scar.Max or scar.Max is NULL))

select @sbc_sum= @sbc_sum + Points from dbo.ScoreCardAttributeRange scar 
join dbo.ScoreCardAttribute sca on scar.ScoreCardAttributekey=
sca.ScoreCardAttributekey
where ScoreCardKey = @ScoreCardKey and 
code = 'AT107' and (@AT107 >= scar.Min and (@AT107 <= scar.Max or scar.Max is NULL))

select @sbc_sum= @sbc_sum + Points from dbo.ScoreCardAttributeRange scar 
join dbo.ScoreCardAttribute sca on scar.ScoreCardAttributekey=
sca.ScoreCardAttributekey
where ScoreCardKey = @ScoreCardKey and 
code = 'AT164' and (@AT164 >= scar.Min and (@AT164<= scar.Max or scar.Max is NULL))

select @sbc_sum= @sbc_sum + Points from dbo.ScoreCardAttributeRange scar 
join dbo.ScoreCardAttribute sca on scar.ScoreCardAttributekey=
sca.ScoreCardAttributekey
where ScoreCardKey = @ScoreCardKey and 
code = 'IN106' and (@IN106 >= scar.Min and (@IN106<= scar.Max or scar.Max is NULL))

select @sbc_sum= @sbc_sum + Points from dbo.ScoreCardAttributeRange scar 
join dbo.ScoreCardAttribute sca on scar.ScoreCardAttributekey=
sca.ScoreCardAttributekey
where ScoreCardKey = @ScoreCardKey and 
code = 'IN141' and (@IN141 >= scar.Min and (@IN141 <= scar.Max or scar.Max is NULL))

select @sbc_sum= @sbc_sum + Points from dbo.ScoreCardAttributeRange scar 
join dbo.ScoreCardAttribute sca on scar.ScoreCardAttributekey=
sca.ScoreCardAttributekey
where ScoreCardKey = @ScoreCardKey and 
code = 'OT007' and (@OT007 >= scar.Min and (@OT007<= scar.Max or scar.Max is NULL))

select @sbc_sum= @sbc_sum + Points from dbo.ScoreCardAttributeRange scar 
join dbo.ScoreCardAttribute sca on scar.ScoreCardAttributekey=
sca.ScoreCardAttributekey
where ScoreCardKey = @ScoreCardKey and 
code = 'OT041' and (@OT041 >= scar.Min and (@OT041 <= scar.Max or scar.Max is NULL))

select @sbc_sum= @sbc_sum + Points from dbo.ScoreCardAttributeRange scar 
join dbo.ScoreCardAttribute sca on scar.ScoreCardAttributekey=
sca.ScoreCardAttributekey
where ScoreCardKey = @ScoreCardKey and 
code = 'RE105' and (@RE105 >= scar.Min and (@RE105 <= scar.Max or scar.Max is NULL))

select @sbc_sum= @sbc_sum + Points from dbo.ScoreCardAttributeRange scar 
join dbo.ScoreCardAttribute sca on scar.ScoreCardAttributekey=
sca.ScoreCardAttributekey
where ScoreCardKey = @ScoreCardKey and 
code = 'RE140' and (@RE140 >= scar.Min and (@RE140 <= scar.Max or scar.Max is NULL))

  set @score = round((1/(1+(exp((@sbc_sum/10)*-1))))*1000,0)
  select @LegalEntity as LegalEntityKey, @score as CreditScore 
    
  		fetch next from ITCLegalEntity into @LegalEntity

	end -- LE cursor

	close ITCLegalEntity
	deallocate ITCLegalEntity;
	
	end
  
  
  
  
  

  
  
  
  