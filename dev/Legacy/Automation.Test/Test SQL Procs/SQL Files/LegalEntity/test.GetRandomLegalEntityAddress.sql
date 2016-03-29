USE [2AM]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetRandomLegalEntityAddress]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetRandomLegalEntityAddress]
	Print 'Dropped procedure [test].[GetRandomLegalEntityAddress]'
End
Go
CREATE PROCEDURE [test].[GetRandomLegalEntityAddress]
	@idnumber varchar(max) = '',
	@registeredName varchar(max) = 'Manogh Maharaj Attorneys',
	@registrationNumber varchar(max) = ''
AS
BEGIN
	if object_id('tempdb..#legalentity') is not null
	begin
		drop table #legalentity select * from #legalentity
	end
	select * into #legalentity from dbo.legalentity
	if (@idnumber <> '' and @idnumber is not null)
	begin
		delete from #legalentity 
		where idnumber is null
		delete from #legalentity 
		where idnumber <> @idnumber
	end
	if (@registeredName <> '' and @registeredName is not null)
	begin
		delete from #legalentity 
		where registeredname is null
		delete from #legalentity 
		where registeredname <> @registeredName
	end
	if (@registrationNumber <> '' and @registrationNumber is not null)
	begin
		delete from #legalentity 
		where registrationnumber is null
		delete from #legalentity 
		where registrationnumber <> @registrationNumber
	end
	select *,dbo.fGetFormattedAddress(a.addresskey) as FormattedAddress from #legalentity le
		inner join dbo.legalentityaddress lea on le.legalentitykey = lea.legalentitykey
        inner join dbo.address a on lea.addresskey = a.addresskey
      
END