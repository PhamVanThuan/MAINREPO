USE [2AM]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertValuationTestCases') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.InsertValuationTestCases 
	Print 'Dropped Proc test.InsertValuationTestCases'
End
GO
CREATE PROCEDURE test.InsertValuationTestCases
	 @adusername varchar(50)
AS

declare @offers table (OfferKey int not null)
declare @rowCount int
declare @count int

insert into @offers
select offer.offerkey from dbo.offer
	inner join dbo.offerrole
		on offerrole.offerkey = offer.offerkey
		and offerrole.generalstatuskey = 1
	inner join dbo.offerroletype
		on offerrole.offerroletypekey = offerroletype.offerroletypekey 
		and offerroletype.description = 'Valuations Administrator D'
	inner join dbo.legalentity
		on offerrole.legalentitykey = legalentity.legalentitykey
where legalentity.firstnames = @adusername
order by offer.offerkey desc

set @rowCount = (select count(*) from 
						(	
							select distinct ValuationTestCases.TestGroup
							from test.ValuationTestCases 
							group by ValuationTestCases.TestGroup
						) as ValuationTestCaseGroupCount)

set @count = 0

WHILE @count < @rowCount 
BEGIN
	UPDATE test.ValuationTestCases
	SET ValuationTestCases.ApplicationKey = (select top 01 OfferKey from @offers where OfferKey not in (select applicationkey from test.ValuationTestCases))
	WHERE ValuationTestCases.TestGroup = (select top 01 ValuationTestCases.TestGroup from test.ValuationTestCases where ApplicationKey = 0)
	
	PRINT @count
	set @count = @count + 1
END
