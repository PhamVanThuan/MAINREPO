use [x2]
go

if not exists (select 1 from sys.columns 
where name = 'IsLegacy' and object_id = object_id('x2.x2.process'))

begin

alter table x2.x2.process
add IsLegacy bit not null
	constraint IsLegacyDefault Default 1

end

go

update x2.x2.process
set isLegacy = 0
where name in ('Third Party Invoices','LifeClaims')