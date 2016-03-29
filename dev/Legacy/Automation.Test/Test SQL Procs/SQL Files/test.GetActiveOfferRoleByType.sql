USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetActiveOfferRoleByType') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetActiveOfferRoleByType
	Print 'Dropped procedure test.GetActiveOfferRoleByType'
End
Go

CREATE PROCEDURE test.GetActiveOfferRoleByType 
	@OfferRoleTypeKey INT,
	@OfferKey INT
AS
SELECT ofr.OfferRoleTypeKey, ad.ADUserName 
FROM [2am].dbo.OfferRole ofr
JOIN [2am].dbo.OfferRoleType ort on ofr.OfferRoleTypeKey=ort.OfferRoleTypeKey
JOIN [2am].dbo.ADUser ad ON ofr.LegalEntityKey=ad.LegalEntityKey
WHERE ofr.OfferKey = @OfferKey 
AND ofr.OfferRoleTypeKey = @OfferRoleTypeKey
AND ofr.GeneralStatusKey = 1 
