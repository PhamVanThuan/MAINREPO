Use [2am]
go

if exists (select * from INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'test' and TABLE_NAME = 'IDNumbers')
begin
drop table test.IDNumbers
End

create table test.IDNumbers
(
IDNumberKey int not null identity Primary Key,
IDNumber varchar(20) not null
)