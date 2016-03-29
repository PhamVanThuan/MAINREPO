use [2am]
GO
IF OBJECT_ID('[2AM].test.DebtCounsellingTestCases') IS NOT NULL
BEGIN
	DROP TABLE test.DebtCounsellingTestCases
END
GO

GO
IF OBJECT_ID('[2AM].test.DebtCounsellingAccounts') IS NOT NULL
BEGIN
	DROP TABLE test.DebtCounsellingAccounts
END
GO

GO
IF OBJECT_ID('[2AM].test.DebtCounsellingLegalEntities') IS NOT NULL
BEGIN
	DROP TABLE test.DebtCounsellingLegalEntities
END
GO

CREATE TABLE test.DebtCounsellingTestCases
(
	[TestIdentifier] VARCHAR(150) NOT NULL,
	[TestGroup] VARCHAR(100) NOT NULL,
	[IDNumber] VARCHAR(20) NOT NULL,
	[PassportNumber] INT NULL,
	[NCRDCRegNumber] VARCHAR(50) NULL,
	[DebtCounsellorName] VARCHAR(50),
	[CreatorADUserName] VARCHAR(50) NOT NULL,
	[CurrentCaseOwner] VARCHAR(50) NULL
)

CREATE TABLE test.DebtCounsellingAccounts
(
	[TestIdentifier] VARCHAR(100) NOT NULL,
	[AccountKey] INT NOT NULL,
	[DebtCounsellingKey] INT,
	[eWorkADUser] VARCHAR(50) NULL,
	[eFolderId] VARCHAR(100) NULL,
	[eWorkStage] VARCHAR(100) NULL
)

CREATE TABLE test.DebtCounsellingLegalEntities
(
	[AccountKey] INT NOT NULL,
	[LegalEntityKey] INT NOT NULL,
	[LegalEntityLegalName] VARCHAR(150) NOT NULL,
	[UnderDebtCounselling] BIT NOT NULL
)