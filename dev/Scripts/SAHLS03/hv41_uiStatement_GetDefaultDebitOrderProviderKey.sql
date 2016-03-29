use [2am]
go

/**************************************************
 Statement  : COMMON\GetDefaultDebitOrderProviderKey
 Change Date: 2015/05/06 11:26:23 AM - Version 1
 Change User: SAHL\craigf
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'GetDefaultDebitOrderProviderKey' and ApplicationName = 'COMMON' and Version = 1) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'GetDefaultDebitOrderProviderKey' and ApplicationName = 'COMMON' and Version > 1
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'GetDefaultDebitOrderProviderKey' and ApplicationName = 'COMMON' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'GetDefaultDebitOrderProviderKey' and ApplicationName = 'COMMON'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('COMMON', 'GetDefaultDebitOrderProviderKey', GetDate(), 1, 'SAHL\craigf',
                'select top 1 ProviderKey from [2am].deb.Provider where IsDefault = 1 and GeneralStatusKey = 1',
                1, GetDate());
   end
end
