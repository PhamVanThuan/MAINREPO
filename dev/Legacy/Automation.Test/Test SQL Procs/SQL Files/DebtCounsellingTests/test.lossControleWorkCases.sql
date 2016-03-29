use [2AM]
go
if (object_id('test.lossControleWorkCases') is not null)
	begin
		drop TABLE test.lossControleWorkCases
		print 'Dropped TABLE: test.lossControleWorkCases'
	end

go


create table test.lossControleWorkCases
(eStageName varchar(100),
eFolderId varchar(100),
eFolderName varchar(100),
UserToDo varchar(100),
accountKey int
)

insert into test.lossControleWorkCases
select distinct
      ef.eStageName,
      ef.eFolderId,
      ef.eFolderName,
      isnull(ad.adUserName,''),
      cast(test.fnDigitsOnly(ef.eFolderName) as int)
from 
      [e-work]..eFolder ef (nolock) 
      join [e-work]..LossControl lc (nolock) on ef.eFolderId=lc.eFolderId
      join [2am].dbo.Account a on test.fnDigitsOnly(ef.eFolderName) = a.AccountKey and a.AccountStatusKey=1
      join [2am].dbo.Role r on a.AccountKey=r.AccountKey and r.roleTypeKey=2
      join [2am].dbo.LegalEntity le on r.legalentityKey=le.legalEntityKey and legalEntityTypeKey=2
      left join [2am].dbo.aduser ad (nolock) on 'SAHL\'+lc.UserToDo=ad.adusername
where 
      ef.eMapName = 'LossControl' 
and 
      eStageName in (select eStageName from [e-work]..eStage (nolock) 
                              where eStageType not in (4,6) and eStageName <> 'Collection Archive' 
                              and eStage.eMapName = 'LossControl') 
order by 
      ef.eFolderId desc, 
      ef.eStageName

