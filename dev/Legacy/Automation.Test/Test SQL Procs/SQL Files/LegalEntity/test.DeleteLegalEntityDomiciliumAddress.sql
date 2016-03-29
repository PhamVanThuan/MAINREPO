USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.DeleteLegalEntityDomiciliumAddress') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.DeleteLegalEntityDomiciliumAddress
	Print 'Dropped procedure test.DeleteLegalEntityDomiciliumAddress'
End
Go

CREATE PROCEDURE test.DeleteLegalEntityDomiciliumAddress
	@legalEntityKey int
as

select legalentitydomiciliumkey
into #legalentitydomiciliumtodelete
from dbo.legalentitydomicilium
where legalentityaddresskey in 
(
	select distinct led.legalentityaddresskey from dbo.legalentityaddress as lea
		join dbo.LegalEntityDomicilium as led
			on lea.legalentityaddresskey = led.legalentityaddresskey
	where legalentitykey = @legalEntityKey
)

delete from dbo.offerroledomicilium
where legalentitydomiciliumkey in (
select legalentitydomiciliumkey from #legalentitydomiciliumtodelete)

delete from dbo.externalroledomicilium
where legalentitydomiciliumkey in (
select legalentitydomiciliumkey from #legalentitydomiciliumtodelete)

delete from dbo.legalentitydomicilium
where legalentitydomiciliumkey in (
select legalentitydomiciliumkey from #legalentitydomiciliumtodelete)
