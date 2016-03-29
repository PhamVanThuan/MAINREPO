
declare @OldProcessID int
declare @NewProcessID int

set @NewProcessID = :NewProcessID
set @OldProcessID = :OldProcessID


IF OBJECT_ID('tempdb..#InstancesToRecalculate') IS NOT NULL
    DROP TABLE #InstancesToRecalculate

CREATE TABLE #InstancesToRecalculate (InstanceID BIGINT)

INSERT INTO #InstancesToRecalculate
SELECT * FROM [x2].[x2].fGetActivitySecurityChanges(@NewProcessID, @OldProcessID)

INSERT INTO #InstancesToRecalculate
SELECT * FROM [x2].[x2].fGetStateSecurityChanges(@NewProcessID, @OldProcessID)

INSERT INTO [x2].staging.SecurityRecalculation
SELECT DISTINCT temp.InstanceID FROM #InstancesToRecalculate temp
left join  [x2].staging.SecurityRecalculation recalc on temp.InstanceID = recalc.instanceID
where recalc.InstanceID is null

select distinct InstanceID from #InstancesToRecalculate 