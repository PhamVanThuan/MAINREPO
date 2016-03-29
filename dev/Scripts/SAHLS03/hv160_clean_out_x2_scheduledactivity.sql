USE [X2]
GO


delete from x2.x2.scheduledactivity
where id in (select sa.ID
from x2.x2.scheduledactivity sa
join x2.x2.Activity a on a.ID = sa.ActivityID
where a.Type <> 4)
go

delete from x2.x2.scheduledactivity
where id in (select sa.ID
from x2.x2.scheduledactivity sa
join x2.x2.Activity a on a.ID = sa.ActivityID
where a.Type = 4
and ISNULL(WorkFlowProviderName,'')=''
and Time < DATEADD(d, -1, getdate()))
go
