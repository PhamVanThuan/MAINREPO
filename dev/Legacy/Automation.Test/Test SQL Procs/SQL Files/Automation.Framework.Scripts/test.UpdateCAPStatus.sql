USE [2AM]
GO
/****** 
Object:  StoredProcedure [test].[CreateCAP2Offer]    
Script Date: 10/19/2010 16:48:58 
******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[UpdateCAPStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].UpdateCAPStatus

GO

CREATE PROCEDURE test.UpdateCAPStatus

@CapOfferKey INT,
@CapStatusKey INT

AS

if (@CapStatusKey = 12)
	begin
		update dbo.CapOffer set CapStatusKey = @CapStatusKey where capOfferKey = @capOfferKey
	end
else
	begin
		update dbo.CapOffer set CapStatusKey = @CapStatusKey where capOfferKey = @capOfferKey
		update dbo.CapOfferDetail set CapStatusKey = @CapStatusKey where capOfferKey = @capOfferKey and capStatusKey <> 3
	end

