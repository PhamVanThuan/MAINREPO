use [2am]
go

/**************************************************
 Statement  : COMMON\GetWorkflowStateSummaryByUser
 Change Date: 2015/03/10 10:07:29 AM - Version 14
 Change User: SAHL\ClintonS
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'GetWorkflowStateSummaryByUser' and ApplicationName = 'COMMON' and Version = 14) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'GetWorkflowStateSummaryByUser' and ApplicationName = 'COMMON' and Version > 14
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'GetWorkflowStateSummaryByUser' and ApplicationName = 'COMMON' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'GetWorkflowStateSummaryByUser' and ApplicationName = 'COMMON'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('COMMON', 'GetWorkflowStateSummaryByUser', GetDate(), 14, 'SAHL\ClintonS',
                '---3 December 2014 - Ivor Jordan -- adjusted query to stop scanning for  state ids to excelude and put theis filter inline to main query 
--- this is a performance issue in absence of index on name for state table. FB 3721

-- Ignore registration Pipeline state
--declare @RegPipelineStateID int, @CapitecBranchDecline int
--SELECT @RegPipelineStateID = max(ID) from [x2].[x2].State (nolock) where [Name] = ''Registration Pipeline'';
--SELECT @CapitecBranchDecline = max(ID) from [x2].[x2].State (nolock) where [Name] = ''Capitec Branch Decline'';

with WorkFlows(ID, [WorkFlowName]) as (
    select max(w.ID), w.[Name]
    from [x2].[x2].WorkFlow w (nolock)
	join [x2].[x2].Process p (nolock) on w.ProcessID = p.ID
	where (p.ViewableOnUserInterfaceVersion is null or p.ViewableOnUserInterfaceVersion=''2'')
    group by w.[Name]
)
select 
min(t.WorkFlowID) as WorkFlowID, 
min(t.WorkFlowName) as WorkFlowName, 
min(t.StateID) as StateID, 
StateName, 
count(t.InstanceID) as InstanceCount from (
select distinct
i.ID as InstanceID,
wf.ID as WorkFlowID, 
wf.WorkFlowName as WorkFlowName, 
i.StateID as StateID,
s.[Name] as StateName
from [x2].[x2].Instance i (nolock)
inner join [x2].[x2].WorkList wl (nolock) on wl.InstanceID = i.ID
inner join WorkFlows wf (nolock) on wf.ID = i.WorkFlowID
left join [x2].[x2].State s (nolock) on i.StateID = s.ID 
where wl.ADUserName in ({0})
--Ignore registration Pipeline state - using in line for performance reasons FB3721
and s.name != ''Registration Pipeline'' and s.name != ''Capitec Branch Decline'' 
--and i.StateID Not in (@RegPipelineStateID, @CapitecBranchDecline)
) t
group by WorkFlowID,StateName
order by WorkFlowName, StateName 



',
                1, GetDate());
   end
end
