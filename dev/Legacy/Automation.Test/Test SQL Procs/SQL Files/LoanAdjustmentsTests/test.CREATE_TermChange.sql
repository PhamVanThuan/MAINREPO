Use [2am]
GO
IF OBJECT_ID('[2AM].test.TermChange') IS NOT NULL
BEGIN
	DROP TABLE test.TermChange
END
GO

CREATE TABLE test.TermChange
(
[TestIdentifier] VARCHAR(100),
[Account] INT,
[Product] INT,
[RateOverride] INT,
[SPVMaxTerm] INT,
[InitialInstalments] INT,
[RemainingInstalments] INT,
[CurrentSPV] INT,
[NewSPV] INT,
[SPVMaxTermIncrease] INT,									
[SAHLMaxTermIncrease] INT,
[InitialRepaymentAge] INT,
[AllowTermChange] INT,
[OpenOffer] INT,
[UnderDebtCounselling] INT
)