---Set Up LightStone Valuator Test Companies
use [2am]
go
if not exists(select * from legalentity where registeredname='ValCo1')
      Begin
            insert into [2am].[dbo].[LegalEntity]
                  select 3,NULL,NULL,NULL,'2011-07-01 00:00:00.000',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'','ValCo1','ValCo1','',NULL,NULL,NULL,'111','111111','','','','',NULL,1,1,NULL,NULL,'SAHL\bcuser1',NULL,NULL,NULL,2,NULL
      End
      
if not exists(select * from legalentity where registeredname='ValCo2')
      Begin
            insert into [2am].[dbo].[LegalEntity]
                  select 3,NULL,NULL,NULL,'2011-07-01 00:00:00.000',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'','ValCo2','ValCo2','',NULL,NULL,NULL,'111','111111','','','','',NULL,1,1,NULL,NULL,'SAHL\bcuser1',NULL,NULL,NULL,2,NULL
      End   

if not exists(select * from legalentity where registeredname='ValCo3')
      Begin
            insert into [2am].[dbo].[LegalEntity]
                  select 3,NULL,NULL,NULL,'2011-07-01 00:00:00.000',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'','ValCo3','ValCo3','',NULL,NULL,NULL,'111','111111','','','','',NULL,1,1,NULL,NULL,'SAHL\bcuser1',NULL,NULL,NULL,2,NULL
      End
      
declare @valco1 int     
declare @valco2 int
declare @valco3 int

select @valco1=legalentitykey from legalentity where registeredname='ValCo1'
select @valco2=legalentitykey from legalentity where registeredname='ValCo2'
select @valco3=legalentitykey from legalentity where registeredname='ValCo3'

if not exists(select * from Valuator where LegalEntityKey=@valco1)
      Begin
            insert into [2am].[dbo].[Valuator]
                  select 'ValCo1',NULL,0,1,@valco1
      End   
      
if not exists(select * from Valuator where LegalEntityKey=@valco2)
      Begin
            insert into [2am].[dbo].[Valuator]
                  select 'ValCo2',NULL,0,1,@valco2
      End   
      
if not exists(select * from Valuator where LegalEntityKey=@valco3)
      Begin
            insert into [2am].[dbo].[Valuator]
                  select 'ValCo3',NULL,0,1,@valco3
      End               
      
select * from dbo.Valuator
      
