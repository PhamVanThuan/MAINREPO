USE [2AM]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetRandomLegalEntity]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetRandomLegalEntity]
	Print 'Dropped procedure [test].[GetRandomLegalEntity]'
End
Go

CREATE PROCEDURE [test].[GetRandomLegalEntity]
AS
BEGIN
	if object_id('tempdb..#legalentitykeys') is not null
	begin
		drop table #legalentitykeys
	end

	if object_id('tempdb..#legalentityroles') is not null
	begin
		drop table #legalentitykeys
	end

	declare @count int
	declare @maxcount int
	set @maxcount = (select count(*) from dbo.legalentity)

	select le.legalentitykey
	into #legalentitykeys 
	from dbo.legalentity le
	where le.legalentitytypekey = 2

	select r.legalentitykey 
	into #legalentityroles
	from dbo.role r

	delete from #legalentitykeys
	where legalentitykey not in (select legalentitykey from #legalentityroles)

	select top 1 * from legalentity 
	where legalentitykey in (select legalentitykey from #legalentitykeys)
	order by newid()
END