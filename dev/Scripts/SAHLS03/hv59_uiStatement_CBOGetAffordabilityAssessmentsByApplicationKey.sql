use [2am]
go

/**************************************************
 Statement  : COMMON\CBOGetAffordabilityAssessmentsByApplicationKey
 Change Date: 2015/05/12 12:05:54 PM - Version 1
 Change User: SAHL\craigf
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'CBOGetAffordabilityAssessmentsByApplicationKey' and ApplicationName = 'COMMON' and Version = 1) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'CBOGetAffordabilityAssessmentsByApplicationKey' and ApplicationName = 'COMMON' and Version > 1
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'CBOGetAffordabilityAssessmentsByApplicationKey' and ApplicationName = 'COMMON' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'CBOGetAffordabilityAssessmentsByApplicationKey' and ApplicationName = 'COMMON'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('COMMON', 'CBOGetAffordabilityAssessmentsByApplicationKey', GetDate(), 1, 'SAHL\craigf',
                'select 
	O.OfferKey as GenericKey,
	''Affordability Assessments'' as Description,
	''Affordability Assessments (Loan: '' + convert(varchar(10),O.ReservedAccountKey) + '' Application: '' + convert(varchar(10),O.OfferKey) + '')'' as LongDescription,
	NULL as OriginationSourceKey
from
	Offer O (nolock)
where
	O.OfferKey = @OfferKey',
                1, GetDate());
   end
end
