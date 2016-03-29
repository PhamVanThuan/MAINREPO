use [2am]
go
IF OBJECT_ID('[2am].test.FurtherLendingTestCases') IS NOT NULL
BEGIN

	DROP TABLE test.FurtherLendingTestCases

END

	CREATE TABLE test.FurtherLendingTestCases 
		(
		AccountKey INT,
		ValuationDate DATETIME,
		ValuationAmount FLOAT,
		LTV FLOAT,
		Product VARCHAR(50),
		ReadvanceAmount FLOAT,
		FurtherAdvanceAmount FLOAT,
		FurtherLoanAmount FLOAT,
		MaxFurtherLending FLOAT,
		SPV VARCHAR(100),
		RapidLTV FLOAT,
		InsertDate DATETIME DEFAULT GETDATE(),
		CommittedLoanValue FLOAT,
		BondValue FLOAT,
		LoanAgreementAmount FLOAT,
		FurtherAdvanceLimit FLOAT,
		PTI FLOAT,
		UnderDebtCounselling BIT,
		CurrentBalance FLOAT,
		HouseholdIncome FLOAT
		)