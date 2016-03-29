USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.UpdateLegalEntityDomiciliumAddress') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.UpdateLegalEntityDomiciliumAddress
	Print 'Dropped procedure test.UpdateLegalEntityDomiciliumAddress'
End
Go

CREATE PROCEDURE test.UpdateLegalEntityDomiciliumAddress
	@legalentitydomiciliumkey int,
	@domiciliumStatusKey int
as

update dbo.legalentitydomicilium
set generalstatuskey = @domiciliumStatusKey
where legalentitydomiciliumkey = @legalentitydomiciliumkey



