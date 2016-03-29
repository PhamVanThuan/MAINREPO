use [2am]
go

/**************************************************
 Statement  : Rule\NaedoDebitOrderPending
 Change Date: 2015/09/29 14:43:19 PM - Version 2
 Change User: SAHL\craigf
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'NaedoDebitOrderPending' and ApplicationName = 'Rule' and Version = 2) )
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'NaedoDebitOrderPending' and ApplicationName = 'Rule' and Version > 2
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'NaedoDebitOrderPending' and ApplicationName = 'Rule' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'NaedoDebitOrderPending' and ApplicationName = 'Rule';
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('Rule', 'NaedoDebitOrderPending', GetDate(), 2, 'SAHL\craigf',
                'select count(1)
from [2am].deb.Batch b (nolock)
join [2am].deb.DOTransaction d (nolock) on b.BatchKey = d.BatchKey
where d.AccountKey = @AccountKey
	and (
		b.BatchStatusKey < 5	-- Batch is not yet "Finished"
		OR d.ErrorKey = 128		-- or DOTran is "HELD FOR REPRESENTMENT" (Tracking)
	)',
                1, GetDate());
   end
end
