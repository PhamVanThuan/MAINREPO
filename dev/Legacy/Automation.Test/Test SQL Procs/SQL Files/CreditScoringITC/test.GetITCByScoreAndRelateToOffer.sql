USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetITCByScoreAndRelateToOffer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetITCByScoreAndRelateToOffer]
	Print 'Dropped procedure [test].[GetITCByScoreAndRelateToOffer]'
End
Go

/*
This procedure will either fetch ITC records with a score greater than the one provided
OR when using the @RelateToOffer variable set to 1 and an OfferKey is provided, will
relate an ITC with a score greater than the one provided to your account.
E.G. 
1. EXEC test.GetITCByScoreAndRelateToOffer 695, 0, 0
2. EXEC test.GetITCByScoreAndRelateToOffer 695, 1, 726493
*/

CREATE PROCEDURE [test].[GetITCByScoreAndRelateToOffer]
@ITCScore INT,
@RelateToOffer INT,
@OfferKey INT

AS

BEGIN

DECLARE @ITCKey INT
DECLARE @LegalEntityKey INT
DECLARE @AccountKey INT

IF (@RelateToOffer = 0)

	BEGIN

			SET ARITHABORT ON; 
			WITH XMLNAMESPACES('https://secure.transunion.co.za/TUBureau' AS  "TUBureau")  
			SELECT TOP 10 *
			FROM ITC i
			WHERE 
			CONVERT(INT,CONVERT(VARCHAR(200),ResponseXML.query
			(N'/BureauResponse/TUBureau:EmpiricaEM05/TUBureau:EmpiricaScore/text()')))
			 > @ITCScore
			ORDER BY ChangeDate desc
	END

IF (@RelateToOffer = 1)

	BEGIN

			SET ARITHABORT ON; 
			WITH XMLNAMESPACES('https://secure.transunion.co.za/TUBureau' AS  "TUBureau")  
			SELECT TOP 1 @ITCKey = i.ITCKey
			FROM ITC i
			WHERE 
			CONVERT(INT,CONVERT(VARCHAR(200),ResponseXML.query
			(N'/BureauResponse/TUBureau:EmpiricaEM05/TUBureau:EmpiricaScore/text()')))
			 = @ITCScore
			ORDER BY ChangeDate desc
			--Find the AccountKey and a LegalEntityKey for an Applicant
			SELECT TOP 1 @AccountKey = ISNULL(o.AccountKey,o.ReservedAccountKey), @LegalEntityKey = ofr.LegalEntityKey 
			FROM Offer o 
			JOIN OfferRole ofr ON o.OfferKey=ofr.OfferKey
			JOIN OfferRoleType ort ON ofr.OfferRoleTypeKey=ort.OfferRoleTypeKey
			JOIN OfferRoleTypeGroup ortg on ort.OfferRoleTypeGroupKey=ortg.OfferRoleTypeGroupKey
			WHERE o.OfferKey=@OfferKey AND ortg.OfferRoleTypeGroupKey=3
			--Update the ITC table to link to our Offer
			UPDATE ITC
			SET LegalEntityKey=@LegalEntityKey, AccountKey=@AccountKey
			WHERE ITC.ITCKey = @ITCKey
			--Show the results
			SELECT * FROM ITC WHERE ITCKey=@ITCKey
	END

END
