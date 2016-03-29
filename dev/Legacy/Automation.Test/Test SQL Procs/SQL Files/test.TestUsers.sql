USE [2AM]
GO
IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.TestUser') and OBJECTPROPERTY(id, N'IsTable') = 1)
Begin
	DROP TABLE test.TestUser
	Print 'Dropped TABLE test.TestUser'
End
Go

create table test.TestUser 
(
	Id int identity,
	UserName varchar(max),
	LockedUser bit
)

insert into test.TestUser 
select 'BCUser1',0