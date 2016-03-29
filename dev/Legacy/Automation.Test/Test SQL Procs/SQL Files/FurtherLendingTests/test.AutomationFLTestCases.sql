USE [2AM]
GO
IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.AutomationFLTestCases ') and OBJECTPROPERTY(id, N'IsTable') = 1)
Begin
	DROP TABLE test.AutomationFLTestCases 
	Print 'Dropped TABLE test.AutomationFLTestCases' 
End
Go
			CREATE TABLE test.AutomationFLTestCases 
			(
			TestIdentifier VARCHAR(150),
			TestGroup VARCHAR(150),
			AccountKey INT,
			ValuationDate DATETIME,
			LTV FLOAT,
			Product VARCHAR(150),
			ReadvanceAmount FLOAT,
			FurtherAdvanceAmount FLOAT,
			FurtherLoanAmount FLOAT,
			MaxFurtherLending FLOAT,
			SPV VARCHAR(150),
			OfferKey INT,
			ReadvOfferKey INT,
			FAdvOfferKey INT,
			FLOfferKey INT,
			AssignedFLAppProcUser VARCHAR(150),
			OrigConsultant VARCHAR(150),
			Password VARCHAR(150),
			ContactOption VARCHAR(150),
			AssignedFLSupervisor VARCHAR(150),
			AssignedCreditUser VARCHAR(150),
			Credit bit,
			ReadvancePayments bit,
			CurrentState varchar(50),
			ValuationAmount FLOAT,
			CurrentBalance FLOAT,
			InstanceID bigint,
			ProcessViaWorkflowAutomation INT
			)
