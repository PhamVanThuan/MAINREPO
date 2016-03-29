use [2am]
go

--STEP 1: truncate the master table for the further lending offers
IF (OBJECT_ID('[2am].test.FurtherLendingTestCases') IS NOT NULL)
	BEGIN
		PRINT 'Truncating test.FurtherLendingTestCases'
		TRUNCATE TABLE test.FurtherLendingTestCases
	END
GO
--STEP 3: truncate the master table for the CAP offers
IF (OBJECT_ID('[2am].test.CAPTestCases') IS NOT NULL)
	BEGIN
		PRINT 'Truncating test.CAPTestCases'
		TRUNCATE TABLE test.CAPTestCases
	END
GO
--STEP 4: insert new data for the master table
PRINT 'Inserting CAP Configuration Records'
 EXEC test.InsertCAPTypeConfiguration
GO
PRINT 'Inserting CAP Master File'
 EXEC test.InsertCAPTestCases
GO
--STEP 5: truncate the master table for FL automation and CAP automation
IF (OBJECT_ID('[2am].test.AutomationFLTestCases') IS NOT NULL)
	BEGIN
		PRINT 'Truncating test.AutomationFLTestCases'
		TRUNCATE TABLE test.AutomationFLTestCases
	END
GO
IF (OBJECT_ID('[2am].test.AutomationCAP2TestCases') IS NOT NULL)
	BEGIN
		PRINT 'Truncating test.AutomationCAP2TestCases'
		TRUNCATE TABLE test.AutomationCAP2TestCases
	END
GO
--STEP 6: create the further lending data for automation
PRINT 'Inserting FL Automation Data'
 EXEC test.CreateFurtherLendingTestCases
GO
--STEP 7: create the CAP data for automation
PRINT 'Inserting CAP Automation Data'
 EXEC test.CreateCAP2TestCases
GO
--STEP 8: flush out dynamic data from the static lookup tables
UPDATE test.AutomationLeads
SET OfferKey = ''
GO
UPDATE test.InternetLeads
SET OfferKey = ''
GO
UPDATE test.OffersAtApplicationCapture
SET OfferKey = ''
GO
UPDATE test.AutomationFLTestCases
SET OfferKey='',ReAdvOfferKey='',FAdvOfferKey='',FLOfferKey=''
GO

--STEP 9: setup term change cases
GO
 exec test.CreateTermChangetestCases
GO
--STEP 10: remove any existing data from the testMethod and testMethodParameter tables
delete from test.testMethod
delete from test.testMethodParameter
go
--STEP 11: insert the debt counselling test cases
 exec test.createDebtCounsellingTestCases





