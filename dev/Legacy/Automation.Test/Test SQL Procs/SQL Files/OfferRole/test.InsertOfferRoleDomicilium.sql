USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertOfferRoleDomicilium') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.InsertOfferRoleDomicilium
	Print 'Dropped procedure test.InsertOfferRoleDomicilium'
End
Go

CREATE PROCEDURE test.InsertOfferRoleDomicilium
	@legalEntityDomiciliumKey int,
	@offerrolekey int
as

INSERT INTO [2am].[dbo].[OfferRoleDomicilium]
		   ([LegalEntityDomiciliumKey]
		   ,[OfferRoleKey]
		   ,[ChangeDate]
		   ,[ADUserKey]
		   )
	 VALUES
		   (@LegalEntityDomiciliumKey
		   ,@offerrolekey
		   ,getdate()
		   ,2791 --SAHL\HaloUser
			)
GO


