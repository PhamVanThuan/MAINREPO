use [2am]
go

/**************************************************
 Statement  : COMMON\CBOGetAffordabilityAssessmentUnconfirmedByAppKey
 Change Date: 2015/06/26 10:33:40 AM - Version 1
 Change User: SAHL\craigf
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'CBOGetAffordabilityAssessmentUnconfirmedByAppKey' and ApplicationName = 'COMMON' and Version = 1) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'CBOGetAffordabilityAssessmentUnconfirmedByAppKey' and ApplicationName = 'COMMON' and Version > 1
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'CBOGetAffordabilityAssessmentUnconfirmedByAppKey' and ApplicationName = 'COMMON' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'CBOGetAffordabilityAssessmentUnconfirmedByAppKey' and ApplicationName = 'COMMON'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('COMMON', 'CBOGetAffordabilityAssessmentUnconfirmedByAppKey', GetDate(), 1, 'SAHL\craigf',
                'select 
	ass.AffordabilityAssessmentKey as GenericKey,
	[dbo].[fGetAffordabilityAssessmentContributors] (ass.AffordabilityAssessmentKey,0,1) as Description,
	CONVERT(varchar,ass.ModifiedDate, 103) + '' ('' + ast.[Description] + '')'' as LongDescription,
	null as OriginationSourceKey
from
	[2am].dbo.AffordabilityAssessment ass (nolock)
join
	[2am]..AffordabilityAssessmentStatus ast (nolock) on ast.AffordabilityAssessmentStatusKey = ass.AffordabilityAssessmentStatusKey
join
	Offer o (nolock) on o.OfferKey = ass.GenericKey and ass.GenericKeyTypeKey = 2 -- offer
where
	o.OfferKey = @OfferKey
and
	ass.GeneralStatusKey = 1 -- Active
and 
	ass.AffordabilityAssessmentStatusKey = 1 -- Unconfirmed',
                1, GetDate());
   end
end
