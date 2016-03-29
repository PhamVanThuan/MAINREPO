USE [2AM]
GO

/****** Object:  StoredProcedure [test].[InsertDeclarations]    Script Date: 02/15/2013 13:53:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertDeclarations') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE test.InsertDeclarations
	PRINT 'Dropped procedure test.InsertDeclarations'
END
GO

CREATE PROCEDURE [test].[InsertDeclarations]

@OfferKey int,
@GenericKeyTypeKey int,
@OriginationSourceProductKey int

AS

DECLARE @OfferDetails table (offerKey INT, StorageKey INT, legalEntityTypeKey INT, GenericKey INT, OriginationSourceProductKey INT, GenericKeyTypeKey INT)

IF @GenericKeyTypeKey = 33 
      BEGIN
            INSERT INTO @OfferDetails
            SELECT o.OfferKey, ol.OfferRoleKey as StorageKey, le.legalEntityTypeKey, ol.offerRoleTypeKey as GenericKey, 
            @OriginationSourceProductKey as OriginationSourceProductKey,
            @GenericKeyTypeKey as GenericKeyTypeKey
            FROM [2am].dbo.Offer AS o 
            INNER JOIN  [2am].dbo.OfferRole AS ol ON o.OfferKey = ol.OfferKey 
                  and ol.offerRoleTypeKey in (8,10,11,12)
            INNER JOIN  [2am].dbo.LegalEntity AS le ON ol.LegalEntityKey = le.LegalEntityKey
            LEFT JOIN [2am].dbo.OfferDeclaration AS od on ol.offerRoleKey = od.offerRoleKey 
            where o.offerKey =  @OfferKey 
            and od.offerRoleKey is null
      END

IF @GenericKeyTypeKey = 41
      BEGIN
            INSERT INTO @OfferDetails
            SELECT o.OfferKey, el.ExternalRoleKey as StorageKey, le.legalEntityTypeKey, el.externalRoleTypeKey as GenericKey, 
            @OriginationSourceProductKey as OriginationSourceProductKey,
            @GenericKeyTypeKey as GenericKeyTypeKey
            FROM [2am].dbo.Offer AS o 
            INNER JOIN  [2am].dbo.ExternalRole AS el ON o.OfferKey = el.genericKey 
                  and el.ExternalRoleTypeKey in (1)
                  and el.genericKeyTypeKey = 2
            INNER JOIN  [2am].dbo.LegalEntity AS le ON el.LegalEntityKey = le.LegalEntityKey
            LEFT JOIN [2am].dbo.ExternalRoleDeclaration AS erd on el.ExternalRoleKey = erd.ExternalRoleKey 
            where o.offerKey =  @OfferKey 
            and erd.ExternalRoleKey is null
      END

SELECT     details.StorageKey, odc.OfferDeclarationQuestionKey, (CASE WHEN odc.OfferDeclarationQuestionKey IN (2, 4) 
                      THEN 3 WHEN odc.OfferDeclarationQuestionKey = 7 THEN 1 ELSE 2 END) AS Expr1, NULL AS Expr2
INTO #declarationsToInsert
FROM @OfferDetails details
JOIN (
select d.OfferDeclarationQuestionKey, config.GenericKey, config.legalentityTypeKey, config.OriginationSourceProductKey, config.GenericKeyTypeKey 
from [2am].dbo.OfferDeclarationQuestion d
join [2am].dbo.offerdeclarationquestionanswerconfiguration config on d.OfferDeclarationQuestionKey = config.OfferDeclarationQuestionKey 
            ) as odc on details.GenericKey = odc.GenericKey  
            AND details.LegalEntityTypeKey = odc.LegalEntityTypeKey
            AND details.OriginationSourceProductKey = odc.OriginationSourceProductKey
            AND details.GenericKeyTypeKey = odc.GenericKeyTypeKey

IF @GenericKeyTypeKey = 41
      BEGIN
            INSERT INTO [2AM].dbo.ExternalRoleDeclaration
            SELECT * FROM  #declarationsToInsert
      END
IF @GenericKeyTypeKey = 33
      BEGIN
            INSERT INTO [2AM].dbo.OfferDeclaration
            SELECT * FROM  #declarationsToInsert
      END
                   
SET ANSI_NULLS OFF

GO