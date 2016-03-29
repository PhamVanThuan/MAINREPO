USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetCasesInWorklistByStateAndType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetCasesInWorklistByStateAndType]
	Print 'Dropped procedure [test].[GetCasesInWorklistByStateAndType]'
End
Go

CREATE PROCEDURE [test].[GetCasesInWorklistByStateAndType]
/*
EXEC test.GetCasesInWorklistByStateAndUser 'Credit', 'Credit','SAHL\CUUser3' 
*/

	 @State VARCHAR(50), 
	 @WorkFlow VARCHAR(50),
	 @OfferTypeKey INT,
	 @Exclusions VARCHAR(50)

AS

DECLARE @OfferKeys TABLE (OfferKey INT)

IF @exclusions = 'FLAutomation'
	BEGIN
			INSERT INTO @OfferKeys
			SELECT ReadvOfferKey as OfferKey FROM
			test.AutomationFLTestCases
			WHERE ReadvOfferKey <> 0
			union all
			SELECT FadvOfferKey as OfferKey FROM
			test.AutomationFLTestCases
			WHERE FadvOfferKey <> 0
			union all
			SELECT FLOfferKey as OfferKey FROM
			test.AutomationFLTestCases
			WHERE FLOfferKey <> 0
			union all
			select distinct applicationkey from x2.x2data.readvance_payments data
			join x2.x2.instance i on data.instanceid=i.id
			join x2.x2.state s on i.stateid=s.id
			where s.name in ('Followup Hold','Ready To Followup')
	END

IF @exclusions = 'OrginationAutomation'
	BEGIN
			INSERT INTO @OfferKeys
			select OfferKey from test.OffersAtApplicationCapture where isnumeric(offerKey) = 1
	END	

IF (@exclusions = '' or @exclusions is null or @exclusions = '0') 
	BEGIN
			INSERT INTO @OfferKeys
			SELECT 0
	END

DECLARE  @WorkflowID INT 
DECLARE  @StateID INT 
--FETCH THE LATEST WORKFLOW ID
SELECT @WorkflowID = MAX(id) FROM   x2.x2.workflow WHERE  name = @WorkFlow 
--FIND THE STATE ID FOR THE WORKLOW/STATE
SELECT @StateID = s.id FROM   x2.x2.state s WHERE  workflowid = @WorkflowID AND s.name = @State 

--FETCH THE CASES IN THE WORKLIST
SELECT		  o.offerkey,
				  MAX(o.OfferStartDate) AS [Offer Start Date],	 
                  MAX(ot.DESCRIPTION) AS [Offer Type], 
                  MAX(p.DESCRIPTION) AS [Product], 
                  MAX(c.DESCRIPTION) AS [Category], 
                  ROUND(MAX(oivl.ltv * 100),2)  AS [LTV], 
                  ROUND(MAX(oivl.loanagreementamount),2) AS [LAA], 
                  ROUND(MAX(oivl.pti * 100),2)  AS [PTI],
                  MAX(wl.ADUserName) as [ADUserName],
				  a.GeneralStatusKey 
FROM     [X2].[X2].[Worklist] wl WITH (NOLOCK) 
         JOIN [X2].[X2].[Instance] i WITH (NOLOCK) 
           ON wl.instanceid = i.id 
         left JOIN [2am].[dbo].[ADUser] a WITH (NOLOCK)
		   ON wl.ADUserName = a.ADUserName
         JOIN [2am]..[Offer] o WITH (NOLOCK) 
           ON CONVERT(VARCHAR,o.offerkey) = i.[Name] 
         LEFT JOIN [2am]..[OfferMortgageLoan] oml WITH (NOLOCK) 
           ON o.offerkey = oml.offerkey
         JOIN [2am]..[OfferType] ot WITH (NOLOCK) 
           ON ot.offertypekey = o.offertypekey
         LEFT JOIN (SELECT MAX(offerInformationKey) oikey, offerKey 
         from OfferInformation oi
         group by oi.offerKey) as maxoi on o.offerkey=maxoi.offerkey 
         LEFT JOIN [2am]..[OfferInformation] oi WITH (NOLOCK) 
           ON maxoi.oikey = oi.offerinformationkey 
         LEFT JOIN [2am]..[OfferInformationVariableLoan] oivl WITH (NOLOCK) 
           ON oi.offerinformationkey = oivl.offerinformationkey 
         left JOIN [2am]..[Product] p WITH (NOLOCK) 
           ON oi.productkey = p.productkey
         left JOIN [2am]..[Category] c WITH (NOLOCK) 
           ON oivl.categorykey = c.categorykey  
		 LEFT JOIN [2am]..[Property] pr WITH (NOLOCK) 
           ON oml.propertykey = pr.propertykey 
         LEFT JOIN [2am]..[OfferInformationFinancialAdjustment] oifa  WITH (NOLOCK) 
           ON oi.offerinformationkey = oifa.offerinformationkey
	     LEFT JOIN [2am]..Detail d on o.ReservedAccountKey=d.AccountKey
	     and d.DetailTypeKey = 150 --d/o suspended
WHERE  
i.workflowid = @WorkflowID 
AND i.stateid = @StateID
AND o.OfferKey not in (SELECT OfferKey FROM @OfferKeys)
AND o.OfferTypeKey = @OfferTypeKey
AND d.AccountKey IS NULL
AND o.offerStatusKey=1
GROUP BY o.OfferKey, a.GeneralStatusKey
ORDER BY NEWID()

GO


