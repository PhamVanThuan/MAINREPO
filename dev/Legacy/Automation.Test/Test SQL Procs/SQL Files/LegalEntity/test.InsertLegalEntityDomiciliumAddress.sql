USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertLegalEntityDomiciliumAddress') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.InsertLegalEntityDomiciliumAddress
	Print 'Dropped procedure test.InsertLegalEntityDomiciliumAddress'
End
Go

CREATE PROCEDURE test.InsertLegalEntityDomiciliumAddress
	@generalStatusKey int,
	@legalentityaddresskey int
as

INSERT INTO [2am].[dbo].[LegalEntityDomicilium]
		   ([LegalEntityAddressKey]
		   ,[GeneralStatusKey]
		   ,[ChangeDate]
		   ,[ADUserKey])
	 VALUES
		   (@legalentityaddresskey
		   ,@generalStatusKey
		   ,getdate()
		   ,2791 --SAHL\HaloUser
		   )
GO


