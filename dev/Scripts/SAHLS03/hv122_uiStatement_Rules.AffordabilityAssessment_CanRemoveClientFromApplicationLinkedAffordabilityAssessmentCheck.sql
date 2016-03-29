use [2am]
go

/**************************************************
 Statement  : Rules.AffordabilityAssessment\CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck
 Change Date: 2015/06/29 10:19:48 AM - Version 1
 Change User: SAHL\ClintS
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck' and ApplicationName = 'Rules.AffordabilityAssessment' and Version = 1) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck' and ApplicationName = 'Rules.AffordabilityAssessment' and Version > 1
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck' and ApplicationName = 'Rules.AffordabilityAssessment' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck' and ApplicationName = 'Rules.AffordabilityAssessment'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('Rules.AffordabilityAssessment', 'CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck', GetDate(), 1, 'SAHL\ClintS',
                'if exists(select 1
			from [2am].[dbo].[AffordabilityAssessment] aa (nolock)
			join [2am].[dbo].[AffordabilityAssessmentLegalEntity] aale (nolock) on aale.AffordabilityAssessmentKey = aa.AffordabilityAssessmentKey
				and aale.LegalEntityKey = @LegalEntityKey
			where aa.GenericKey = @ApplicationKey
			and aa.GenericKeyTypeKey = 2
			union
			select 1
			from [2am].[dbo].[AffordabilityAssessment] aa (nolock)
			join [2am].[dbo].[AffordabilityAssessmentLegalEntity] aale (nolock) on aale.AffordabilityAssessmentKey = aa.AffordabilityAssessmentKey
			join [2am].[dbo].[LegalEntityRelationship] ler (nolock) on ler.RelatedLegalEntityKey = aale.LegalEntityKey
				and ler.LegalEntityKey = @LegalEntityKey
			where aa.GenericKey = @ApplicationKey
			and aa.GenericKeyTypeKey = 2)
begin
	select 0
end
else
begin
	select 1
end', 1, GetDate());
   end
end
