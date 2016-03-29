USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertCAPTypeConfiguration') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.InsertCAPTypeConfiguration
	Print 'Dropped procedure test.InsertCAPTypeConfiguration'
End
Go

CREATE PROCEDURE test.InsertCAPTypeConfiguration

AS


	--set all the records for inactive
	update CapTypeConfiguration
	Set GeneralStatusKey=2

declare @resetdate smalldatetime
declare @ctckey int
declare @resetconfigkey int

set @resetconfigkey = 1

--sync the market rates
update mr
set Value = sync.Value
from [2am].dbo.marketRate mr
join (
select marketRateKey, Value 
from [sahls15].[2am].dbo.marketRate
) as sync on mr.marketRateKey=sync.marketRateKey

--If resetdate has expired on resetconfiguration table, update resetconfiguration table with new dates from the reset table
if getdate() > (select ResetDate from ResetConfiguration where ResetConfigurationkey = @resetconfigkey)
begin 
	select @ResetDate = min(r.resetdate) 
	from reset r 
	where r.ResetDate >= 
	(select ResetDate from ResetConfiguration 
	where ResetConfigurationkey = @resetconfigkey) 
	and r.resetconfigurationkey = @resetconfigkey
	
	update resetConfiguration 
	set resetdate = @resetdate, actiondate = @resetdate 
	where resetconfigurationkey = @resetconfigkey 
end

select @resetdate = resetdate from resetconfiguration
where resetconfigurationkey = @resetconfigkey

insert into [2am].dbo.captypeconfiguration
select
getdate(), 
dateadd(mm,+1,getdate()),1,
@resetdate, 
dateadd(yy,+2,@resetdate),@resetconfigkey,
@resetdate,24,getdate(),'SAHL\ClintonS', NULL

select @ctckey = scope_identity()

insert into [2am].dbo.captypeconfigurationdetail
select @ctckey,1,(select value+0.01 from [2am].dbo.marketRate where marketratekey=1),
1,0.022,0.00335,0.01865,0.01309,getdate(),'SAHL\ClintonS'

insert into [2am].dbo.captypeconfigurationdetail
select @ctckey,2,(select value+0.02 from [2am].dbo.marketRate where marketratekey=1),
1,0.015,0.004311,0.010689,0.014113,getdate(),'SAHL\ClintonS'

insert into [2am].dbo.captypeconfigurationdetail
select @ctckey,3,(select value+0.03 from [2am].dbo.marketRate where marketratekey=1),
1,0.009,0.00247,0.00653,0.015137,getdate(),'SAHL\ClintonS'

--we need for the other reset config too
set @resetconfigkey = 2

--If resetdate has expired on resetconfiguration table, update resetconfiguration table with new dates from the reset table
if getdate() > (select ResetDate from ResetConfiguration where ResetConfigurationkey = @resetconfigkey)
begin 
	select @ResetDate = min(r.resetdate) 
	from reset r where r.ResetDate >= 
	(select ResetDate from ResetConfiguration 
	where ResetConfigurationkey = @resetconfigkey) 
	and r.resetconfigurationkey = @resetconfigkey
	update resetConfiguration 
	set resetdate = @resetdate, actiondate = @resetdate 
	where resetconfigurationkey = @resetconfigkey 
end

select @resetdate = resetdate from [2am].dbo.resetconfiguration
where resetconfigurationkey = @resetconfigkey

insert into [2am].dbo.captypeconfiguration
select
getdate(), 
dateadd(mm,+1,getdate()),1,
@resetdate, 
dateadd(yy,+2,@resetdate),@resetconfigkey,
@resetdate,24,getdate(),'SAHL\ClintonS', NULL

select @ctckey = scope_identity()

insert into [2am].dbo.captypeconfigurationdetail
select @ctckey,1,(select value+0.01 from [2am].dbo.marketRate where marketratekey=1),
1,0.022,0.00335,0.01865,0.01309,getdate(),'SAHL\ClintonS'

insert into [2am].dbo.captypeconfigurationdetail
select @ctckey,2,(select value+0.02 from [2am].dbo.marketRate where marketratekey=1),
1,0.015,0.004311,0.010689,0.014113,getdate(),'SAHL\ClintonS'

insert into [2am].dbo.captypeconfigurationdetail
select @ctckey,3,(select value+0.03 from [2am].dbo.marketRate where marketratekey=1),
1,0.009,0.00247,0.00653,0.015137,getdate(),'SAHL\ClintonS'

