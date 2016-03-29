use [2am]
go

/**************************************************
 Statement  : Application\GetOfferAttributeTypesByParameters
 Change Date: 2014/01/19 11:09:10 AM - Version 6
 Change User: SAHL\ClintS
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'GetOfferAttributeTypesByParameters' and ApplicationName = 'Application' and Version = 6) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'GetOfferAttributeTypesByParameters' and ApplicationName = 'Application' and Version > 6
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'GetOfferAttributeTypesByParameters' and ApplicationName = 'Application' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'GetOfferAttributeTypesByParameters' and ApplicationName = 'Application'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('Application', 'GetOfferAttributeTypesByParameters', GetDate(), 6, 'SAHL\ClintS',
                'select 
		OfferAttributeTypeKey,
		OfferAttributeTypeGroupKey,
		Remove
	from 
		GetOfferAttributes (@ltv, @employmentTypeKey, @houseHoldIncome, @isStaffLoan, @isGEPF, 0)',
                1, GetDate());
   end
end
