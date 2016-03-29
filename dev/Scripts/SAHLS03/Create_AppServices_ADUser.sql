use [2am]

go
if not exists (select 1 from [2am].dbo.AdUser where adusername = 'sahl\appservices')

begin

insert into [2am].dbo.AdUser
(ADUserName, GeneralStatusKey)
values  (
'SAHL\AppServices', 2
)

end