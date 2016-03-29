use [2am]
go

/**************************************************
 Statement  : COMMON\GetOriginatableSPVList
 Change Date: 2015/09/15 14:22:59 PM - Version 1
 Change User: SAHL\NazirJ
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'GetOriginatableSPVList' and ApplicationName = 'COMMON' and Version = 1) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'GetOriginatableSPVList' and ApplicationName = 'COMMON' and Version > 1
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'GetOriginatableSPVList' and ApplicationName = 'COMMON' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'GetOriginatableSPVList' and ApplicationName = 'COMMON'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('COMMON', 'GetOriginatableSPVList', GetDate(), 1, 'SAHL\NazirJ',
                '
SELECT 
	s.*
FROM 
	[2AM].spv.SPV s
WHERE
	s.SPVKey in (116,117,157,160, 170) 
-- list of originatable SPVs
-- BlueBannerAgencyAccount, MainStreet65PtyLtd, AlphaHousingBlueBanner, AlphaHousingOldMutual, SA Mortgage Fund (RF) Proprietary Limited',
                1, GetDate());
   end
end
