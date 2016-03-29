USE [2AM]
GO
/****** 
Object:  StoredProcedure [test].[MarkCAP2OfferForReadvanceRequired]    
Script Date: 10/19/2010 16:48:58 
******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[MarkCAP2OfferForReadvanceRequired]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].MarkCAP2OfferForReadvanceRequired

GO

CREATE PROCEDURE test.MarkCAP2OfferForReadvanceRequired

@CapOfferKey int

AS

--always set 2% to be the further advance decision
UPDATE cod
SET cod.CapStatusKey = 6
FROM
dbo.CapOfferDetail cod
join dbo.CapTypeConfigurationDetail ctcd on cod.capTypeConfigurationDetailKey = ctcd.capTypeConfigurationDetailKey
WHERE ctcd.CapTypeKey = 2 and cod.CapOfferKey = @CapOfferKey
--set the others to be not taken up
UPDATE cod
SET cod.CapStatusKey = 3
FROM
dbo.CapOfferDetail cod
join dbo.CapTypeConfigurationDetail ctcd on cod.capTypeConfigurationDetailKey = ctcd.capTypeConfigurationDetailKey
WHERE CapTypeKey in (1,3) and CapOfferKey = @CapOfferKey
--set the payment option
UPDATE dbo.CapOffer
SET CapPaymentOptionKey = 1, CapStatusKey = 6
WHERE capOfferKey = @CapOfferKey





