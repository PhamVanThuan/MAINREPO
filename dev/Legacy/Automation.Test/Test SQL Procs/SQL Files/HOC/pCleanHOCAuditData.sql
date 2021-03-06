USE [2AM]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[pCleanHOCAuditData]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [test].[pCleanHOCAuditData]
GO

CREATE PROC [test].[pCleanHOCAuditData]
@HOCKey int
AS
BEGIN

if object_id('tempdb..#CleanAuditData','U') is not null
	drop table #CleanAuditData

create table #CleanAuditData (FinancialServiceKey int,AuditNumber int)

insert into #CleanAuditData
select FinancialServiceKey,	max(AuditNumber) 
from [2am].dbo.AuditHOC with (nolock) 
where FinancialServiceKey = @HOCKey
group by FinancialServiceKey

delete ah 
from #CleanAuditData lar
join [2am].dbo.AuditHOC ah
	on lar.FinancialServiceKey = ah.FinancialServiceKey
where lar.AuditNumber <> ah.AuditNumber

update ah
set ah.AuditDate = (getdate()-5)
from [2am].dbo.AuditHOC ah
join #CleanAuditData lar
	on lar.AuditNumber = ah.AuditNumber

delete from [Warehouse].dbo.[2am_AuditHOC] where FinancialServiceKey = @HOCKey

END

