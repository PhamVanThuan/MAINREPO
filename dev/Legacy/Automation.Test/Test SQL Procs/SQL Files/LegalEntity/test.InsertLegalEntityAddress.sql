USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertLegalEntityAddress') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.InsertLegalEntityAddress
	Print 'Dropped procedure test.InsertLegalEntityAddress'
End
Go

CREATE PROCEDURE test.InsertLegalEntityAddress
	@LegalEntityKey int,
	@Addresskey int,
	@AddressTypeKey int,
	@EffectiveDate datetime,
	@GeneralStatusKey int
as
begin
	INSERT INTO [2AM].[dbo].[LegalEntityAddress]
		   ([LegalEntityKey]
		   ,[AddressKey]
		   ,[AddressTypeKey]
		   ,[EffectiveDate]
		   ,[GeneralStatusKey])
	VALUES (@LegalEntityKey,@Addresskey,@AddressTypeKey,@EffectiveDate,@GeneralStatusKey)
end