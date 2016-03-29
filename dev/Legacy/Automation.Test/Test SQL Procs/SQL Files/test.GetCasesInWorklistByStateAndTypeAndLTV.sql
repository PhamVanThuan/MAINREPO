USE [2AM]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetCasesInWorklistByStateAndTypeAndLTV]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetCasesInWorklistByStateAndTypeAndLTV]
	Print 'Dropped procedure [test].[[GetCasesInWorklistByStateAndTypeAndLTV]]'
End
Go

CREATE PROCEDURE [test].[GetCasesInWorklistByStateAndTypeAndLTV]  
/*  
EXEC test.[GetCasesInWorklistByStateAndTypeAndLTV] 'Application Capture', 'Application Capture',7, '', 100, 2, 0, 1, 1, 1    
*/  
  
  @State VARCHAR(50),   
  @WorkFlow VARCHAR(50),  
  @OfferTypeKey INT,  
  @Exclusions VARCHAR(50),  
  @maxLTV float,  
  @maxLECount int,  
  @minLTV float = 0,  
  @occupancyTypeKey int = 1,  
  @employmentType int = 0,  
  @categoryKey int = -1,  
  @maxIncome float = 500000, 
  @minIncome float = 0 
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
SELECT DISTINCT 
	o.offerkey,  
    MAX(o.OfferStartDate) AS [Offer Start Date],    
    MAX(ot.DESCRIPTION) AS [Offer Type],   
    MAX(p.DESCRIPTION) AS [Product],   
    MAX(c.DESCRIPTION) AS [Category],   
    ROUND(MAX(oivl.ltv * 100),2)  AS [LTV],   
    ROUND(MAX(oivl.loanagreementamount),2) AS [LAA],   
    ROUND(MAX(oivl.pti * 100),2)  AS [PTI],  
    wl.ADUserName,
	MAX(oivl.HouseholdIncome) AS [HouseholdIncome]
FROM [X2].[X2].[Worklist] wl WITH (NOLOCK)   
         JOIN [X2].[X2].[Instance] i WITH (NOLOCK)   
           ON wl.instanceid = i.id   
         JOIN [2am]..[Offer] o WITH (NOLOCK)   
           ON CONVERT(VARCHAR,o.offerkey) = i.[Name]   
         JOIN [2am]..[OfferMortgageLoan] oml WITH (NOLOCK)   
           ON o.offerkey = oml.offerkey  
         JOIN [2am]..[OfferType] ot WITH (NOLOCK)   
           ON ot.offertypekey = o.offertypekey     
         JOIN (select OfferKey, max(OfferInformationKey) AS [OfferInformationKey] from [2am]..[OfferInformation] WITH (NOLOCK) Group By OfferKey) AS oimax
           ON o.offerkey = oimax.offerkey 
		 JOIN [2am]..[OfferInformation] oi WITH (NOLOCK)
           ON oimax.offerinformationkey = oi.offerinformationkey    
         JOIN [2am]..[OfferInformationVariableLoan] oivl WITH (NOLOCK)   
           ON oi.offerinformationkey = oivl.offerinformationkey   
         JOIN [2am]..[Product] p WITH (NOLOCK)   
           ON oi.productkey = p.productkey  
         JOIN [2am]..[Category] c WITH (NOLOCK)   
           ON oivl.categorykey = c.categorykey    
		 LEFT JOIN [2am]..[Property] pr WITH (NOLOCK)   
           ON oml.propertykey = pr.propertykey   
         LEFT JOIN [2am]..[OfferInformationFinancialAdjustment] oifa  WITH (NOLOCK)   
           ON oi.offerinformationkey = oifa.offerinformationkey  
	     LEFT JOIN [2am]..Detail d on o.ReservedAccountKey=d.AccountKey  and d.DetailTypeKey = 150 --d/o suspended  
		 JOIN [2am]..OfferRole ofr on o.offerkey = ofr.offerkey and ofr.offerroleTypeKey in (8, 10, 11, 12)  and ofr.GeneralStatusKey = 1  
		 JOIN [2AM]..Employment e on ofr.LegalEntityKey = e.LegalEntityKey and e.EmploymentStatusKey = 1
WHERE oi.offerinformationkey in (select max(offerinformationkey) from dbo.offerinformation where offerkey = o.offerkey)
AND i.workflowid = @WorkflowID  and wl.adusername like '%'   
AND i.stateid = @StateID  
AND o.OfferKey not in (SELECT OfferKey FROM @OfferKeys)  
AND o.OfferTypeKey = @OfferTypeKey  
AND d.AccountKey IS NULL  
AND isnull(pr.OccupancyTypeKey, 1) = @occupancyTypeKey  
and oivl.employmenttypekey = case when (@employmentType = 0) then oivl.employmenttypekey else @employmentType end  
and oivl.categoryKey = case when (@categoryKey = -1) then oivl.CategoryKey else @categoryKey end 
and (oivl.HouseholdIncome between @minIncome and @maxIncome)   
and oml.PropertyKey is not null
                  
GROUP BY o.OfferKey, wl.ADUserName  
HAVING Count(ofr.legalEntityKey) <= @maxLECount  
AND MAX(oivl.ltv * 100) BETWEEN @minLTV AND @maxLTV  

