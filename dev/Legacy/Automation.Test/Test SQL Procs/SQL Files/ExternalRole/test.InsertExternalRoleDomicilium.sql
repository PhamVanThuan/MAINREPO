USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertExternalRoleDomicilium') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.InsertExternalRoleDomicilium
	Print 'Dropped procedure test.InsertExternalRoleDomicilium'
End
Go

CREATE PROCEDURE test.InsertExternalRoleDomicilium
	@legalEntityDomiciliumKey int,
	@externalrolekey int
as

INSERT INTO [2am].[dbo].[ExternalRoleDomicilium]
		   ([LegalEntityDomiciliumKey]
		   ,[ExternalRoleKey]
		   ,[ChangeDate]
		   ,[ADUserKey]
		   )
	 VALUES
		   (@LegalEntityDomiciliumKey
		   ,@externalrolekey
		   ,getdate()
		   ,2791 --SAHL\HaloUser
			)
GO


