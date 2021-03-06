USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetOfferData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetOfferData] 
	Print 'Dropped procedure [test].[GetOfferData]'
End
Go

CREATE PROCEDURE [test].[GetOfferData] 
	
		@OfferKey INT

AS
BEGIN

--Fetch Offer records
SELECT o.offerkey, 
       ISNULL(o.accountkey,o.reservedaccountkey) AS [Account Key], 
       c.DESCRIPTION  AS [Category], 
       mlp.DESCRIPTION AS [Loan Purpose], 
       p.DESCRIPTION  AS [Product], 
       et.DESCRIPTION AS [Employment Type], 
       CASE 
         WHEN oa.offerkey IS NULL 
         THEN 'No' 
         ELSE 'Yes' 
       END  AS [Capitalise Fees], 
       oivl.householdincome AS [Household Income], 
       oml.clientestimatepropertyvaluation AS [Property Valuation], 
       'Revision ' + CAST(ROW_NUMBER() 
       OVER(ORDER BY oi.offerinformationkey ASC) AS VARCHAR(2)) AS [Revision], 
       oi.offerinsertdate AS [Revision Date], 
	   oit.description AS [Revision Description],
       ISNULL(oml.purchaseprice,0) AS [Purchase Price], 
       ISNULL(oivl.existingloan,0) AS [Existing Loan], 
       ISNULL(oivl.cashdeposit,0) AS [Cash Deposit], 
       ROUND(initfee.totaloutstandingamount,2) AS [Initiation Fee], 
       ROUND(regfee.totaloutstandingamount,2) AS [Registration Fee], 
       ROUND(cancfee.totaloutstandingamount,2)  AS [Cancellation Fee], 
       ROUND(oivl.interiminterest,2) AS [Interim Interest], 
       ISNULL(oivl.requestedcashamount,0) AS [Requested Cash], 
       ROUND(oivl.loanagreementamount,2) AS [Loan Agreement Amount],
       ROUND(oivl.feestotal,2) AS [Fees Total], 
       ROUND(oivl.loanamountnofees,2) AS [Loan Agreement Amount excl. Fees], 
       (m.VALUE * 100) AS [Link Rate], 
       (mr.VALUE * 100) AS [Market Rate], 
	   ISNULL(Discounts.Discount*100,0) AS [Discount],	
       ROUND((mr.VALUE + m.VALUE + ISNULL(Discounts.Discount,0)) * 100,2) AS [Total Rate], 
       oivl.term AS [Term], 
       ROUND(oivl.monthlyinstalment,2) AS [Instalment], 
       ISNULL(oivf.fixedinstallment,0) AS [Fixed Instalment], 
       CASE 
         WHEN oiio.installment IS NOT NULL 
         THEN 'Yes' 
         ELSE 'No' 
       END    AS [Interest Only?], 
       ISNULL(oiio.installment,0) AS [Interest Only Instalment], 
       ROUND((oivl.ltv * 100),2) AS [LTV], 
       ROUND((oivl.pti * 100),2) AS [PTI], 
       ISNULL((oivf.fixedpercent * 100),0) AS [Fixed Percent], 
       ISNULL((1 - oivf.fixedpercent) * 100,0) AS [Variable Percent], 
       AT.DESCRIPTION      AS [Applicant Type], 
       oml.numapplicants   AS [No. of Applicants],
       ISNULL(oivl.bondtoregister,0) AS [Bond to Register] 
FROM   offer (NOLOCK) o 
	   LEFT JOIN offermortgageloan (NOLOCK) oml ON o.offerkey = oml.offerkey 
	   LEFT JOIN offerinformation (NOLOCK) oi ON o.offerkey = oi.offerkey 
	   LEFT JOIN offerinformationtype (NOLOCK) oit ON oi.OfferInformationTypeKey=oit.OfferInformationTypeKey
	   LEFT JOIN offerinformationvariableloan (NOLOCK) oivl ON oi.offerinformationkey = oivl.offerinformationkey 
	   LEFT JOIN rateconfiguration (NOLOCK) rc ON oivl.rateconfigurationkey = rc.rateconfigurationkey 
	   LEFT JOIN margin (NOLOCK) m ON rc.marginkey = m.marginkey 
	   LEFT JOIN marketrate (NOLOCK) mr ON rc.marketratekey = mr.marketratekey 
	   LEFT JOIN category (NOLOCK) c  ON oivl.categorykey = c.categorykey 
	   LEFT JOIN product (NOLOCK) p ON oi.productkey = p.productkey 
	   LEFT JOIN offertype (NOLOCK) ot ON o.offertypekey = ot.offertypekey 
	   LEFT JOIN mortgageloanpurpose (NOLOCK) mlp ON oml.mortgageloanpurposekey = mlp.mortgageloanpurposekey 
	   LEFT JOIN employmenttype (NOLOCK) et ON oivl.employmenttypekey = et.employmenttypekey 
	   LEFT JOIN offerexpense (NOLOCK) initfee ON o.offerkey = initfee.offerkey  AND initfee.expensetypekey = 1 
	   LEFT JOIN offerexpense (NOLOCK) regfee ON o.offerkey = regfee.offerkey AND regfee.expensetypekey = 4 
	   LEFT JOIN offerexpense (NOLOCK) cancfee ON o.offerkey = cancfee.offerkey  AND cancfee.expensetypekey = 5 
	   LEFT JOIN offerattribute (NOLOCK) oa  ON o.offerkey = oa.offerkey  AND oa.offerattributetypekey = 3 
	   LEFT JOIN offerinformationinterestonly (NOLOCK) oiio ON oi.offerinformationkey = oiio.offerinformationkey 
	   LEFT JOIN offerinformationvarifixloan oivf (NOLOCK) ON oi.offerinformationkey = oivf.offerinformationkey 
	   LEFT JOIN applicanttype AT (NOLOCK) ON oml.applicanttypekey = AT.applicanttypekey
	   LEFT JOIN	(SELECT SUM(discount) as [Discount], oi.OfferInformationKey FROM OfferInformation (NOLOCK) oi JOIN offerinformationfinancialadjustment (NOLOCK) oifa
					 ON oi.OfferInformationKey=oifa.OfferInformationKey WHERE oi.OfferKey=@OfferKey GROUP BY oi.OfferInformationKey) as Discounts
	   ON Discounts.OfferInformationKey = oi.OfferInformationKey
WHERE  o.offerkey = @OfferKey

--Fetch OfferRole records
SELECT 
		o.OfferKey,
		o.OfferRoleTypeKey,	
		ort.Description, 
		g.Description, 
		ISNULL(ADuserName,dbo.LegalEntityLegalName(o.LegalEntityKey,0))
FROM OfferRole (NOLOCK) o
	JOIN OfferRoleType (NOLOCK) ort ON o.OfferRoleTypeKey=ort.OfferRoleTypeKey
	JOIN GeneralStatus (NOLOCK) g on o.GeneralStatusKey=g.GeneralStatusKey
	LEFT JOIN ADUser (NOLOCK) ad on o.LegalEntityKey=ad.LegalEntityKey
WHERE o.OfferKey=@OfferKey
ORDER BY 2

--Fetch Stage Transitions
EXEC test.GetStageTransitionsByGenericKey @OfferKey

END
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

