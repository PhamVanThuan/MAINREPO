use [2am]
go
if exists (select * from INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'test' and TABLE_NAME = 'AutomationCAP2TestCases')
begin
	drop table test.AutomationCAP2TestCases
	PRINT 'Dropping table test.AutomationCAP2TestCases'
End

			CREATE TABLE test.AutomationCAP2TestCases
				(	
					TestIdentifier VARCHAR(50),
					TestType VARCHAR(50),
					AccountKey INT,
					PTI FLOAT,
					LTV FLOAT,
					CurrentBalance FLOAT,
					BondAmount FLOAT,
					LoanAgreementAmount FLOAT,
					SPVKey INT,
					SPVMaxLTV INT,
					SPVMaxPTI INT,
					NewBalanceOnePerc FLOAT,
					CapQualifyOnePerc VARCHAR(25),
					NewBalanceTwoPerc FLOAT,
					CapQualifyTwoPerc VARCHAR(25),
					NewBalanceThreePerc FLOAT,
					CapQualifyThreePerc VARCHAR(25),
					CAPApplicationTypeOnePerc VARCHAR(25),
					CAPApplicationTypeTwoPerc VARCHAR(25),
					CAPApplicationTypeThreePerc VARCHAR(25),
					MonthlyInstalmentOnePerc FLOAT,
					MonthlyInstalmentTwoPerc FLOAT,
					MonthlyInstalmentThreePerc FLOAT,
					HouseholdIncome FLOAT,
					LatestValuation FLOAT,
					UnderDebtCounselling BIT,
					CapTypeConfigurationKey INT,
					Premium1 FLOAT,
					Premium2 FLOAT,
					Premium3 FLOAT,
					CapOfferKey int,
					ExpectedEndState varchar(250),
					ScriptFile varchar(250),
					ScriptToRun varchar(250)
				)			
