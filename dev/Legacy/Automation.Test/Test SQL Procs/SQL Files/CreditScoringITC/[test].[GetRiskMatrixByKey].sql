USE [2AM]
GO
/****** Object:  StoredProcedure [test].[GetRiskMatrixByKey]    Script Date: 07/20/2011 11:46:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[GetRiskMatrixByKey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].[GetRiskMatrixByKey]

go

Create PROCEDURE [test].[GetRiskMatrixByKey]

      @RiskMatrixRevisionKey INT

AS

BEGIN



DECLARE @RiskMatrixDesc varchar(max)
DECLARE @PivotColumnHeaders VARCHAR(MAX)

SET @RiskMatrixDesc=(select rm.description from [2am].dbo.RiskMatrixRevision rmr
join [2am].dbo.RiskMatrix rm on rmr.RiskMatrixKey=rm.RiskMatrixKey where RiskMatrixRevisionKey=
@RiskMatrixRevisionKey)

----RiskMatrixKey 
----1 = Single Applicant
----2 = Joint Main Applicants
----3 = Joint Secondary Applicants
----4 = High LTV
----5 = Low LTV - Single
----6 = Low LTV - Joint

if object_id('tempdb..#csmatrix','U') is not null
      drop table #csmatrix

create table #csmatrix
(
riskmatrixcellkey int,
RiskMatrixKey int,
EmpiricaRange varchar(max),
SBCRange varchar(max)
,Decision varchar(max)
)

;with cells(riskmatrixcellkey,RiskMatrixKey,EmpriciaMin,EmpiricaMax,EmpiricaDesignation,EmpiricaDescision,SBCMin,SBCMax,SBCDesignation,SBCDescision) as
(
select  rmc.riskmatrixcellkey,rm.RiskMatrixKey,isnull(rmr.min,0) as EmpriciaMin, isnull(rmr.max,999) as EmpiricaMax, rmr.Designation as EmpiricaDesignation, csd.description as EmpiricaDescision,NULL as SBCMin, NULL as SBCMax, NULL as SBCDesignation, NULL as SBCDescision
from riskmatrixCell rmc
join dbo.RiskMatrixCellDimension rmcd on rmc.riskMatrixCellKey=rmcd.riskMatrixCellKey
join creditscoredecision csd on rmc.creditscoredecisionkey=csd.creditscoredecisionkey
join riskMatrixDimension rmd on rmcd.riskmatrixdimensionkey=rmd.riskmatrixdimensionkey
join riskMatrixRange rmr on rmcd.riskMatrixRangeKey=rmr.riskMatrixRangeKey
join riskMatrixRevision rev on rmc.riskMatrixRevisionKey=rev.riskMatrixRevisionKey
join riskMatrix rm on rev.riskMatrixKey=rm.riskMatrixKey
where rev.RiskMatrixRevisionKey = @RiskMatrixRevisionKey
and rmd.riskmatrixdimensionkey = 1

union all

select rmc.riskmatrixcellkey,rm.RiskMatrixKey,NULL, NULL, NULL, NULL,isnull(rmr.min,0) as SBCMin, 
isnull(rmr.max,999) as SBCMax, rmr.Designation as SBCDesignation, csd.description as SBCDescision
from riskmatrixCell rmc
join dbo.RiskMatrixCellDimension rmcd on rmc.riskMatrixCellKey=rmcd.riskMatrixCellKey
join creditscoredecision csd on rmc.creditscoredecisionkey=csd.creditscoredecisionkey
join riskMatrixDimension rmd on rmcd.riskmatrixdimensionkey=rmd.riskmatrixdimensionkey
join riskMatrixRange rmr on rmcd.riskMatrixRangeKey=rmr.riskMatrixRangeKey
join riskMatrixRevision rev on rmc.riskMatrixRevisionKey=rev.riskMatrixRevisionKey
join riskMatrix rm on rev.riskMatrixKey=rm.riskMatrixKey
where rev.RiskMatrixRevisionKey = @RiskMatrixRevisionKey
and rmd.riskmatrixdimensionkey = 2
)
insert into #csmatrix
select 
riskmatrixcellkey,
RiskMatrixKey,
convert(varchar(max),max(EmpriciaMin)) + ' - ' + convert(varchar(max),max(EmpiricaMax)) as EmpiricaRange,
convert(varchar(max),max(SBCMin)) + ' - ' + convert(varchar(max),max(SBCMax)) as SBCRange, 
Decision = case
                  when max(EmpiricaDescision) != max(SBCDescision) then 'CFG ERROR'
                  else max(EmpiricaDescision)
               end
from cells
group by
riskmatrixcellkey,RiskMatrixKey

;with SBCRange(SBCRange) as
(
      select distinct isnull(SBCRange,'NULL') from #csmatrix
)
SELECT @PivotColumnHeaders = 
  COALESCE(
    @PivotColumnHeaders + ',[' + cast(isnull(SBCRange,'NULL') as varchar) + ']',
    '[' + cast(isnull(SBCRange,'NULL') as varchar)+ ']'
  )
FROM SBCRange

DECLARE @PivotTableSQL NVARCHAR(MAX)
SET @PivotTableSQL = N'
    select *
    from (
            select EmpiricaRange ,SBCRange,Decision from #csmatrix
            ) AS PivotData
            PIVOT (
              MAX(Decision)
              FOR SBCRange IN (
                ' + @PivotColumnHeaders + '
            )
          ) AS PivotTable 
          order by cast(isnull(substring(EmpiricaRange, 1, CHARINDEX('' - '', EmpiricaRange)),-1) as int) 
       '
EXECUTE(@PivotTableSQL)

END
