USE [2AM]
GO

IF EXISTS (SELECT TOP 1 * FROM test.InternetLeads)
	BEGIN
		TRUNCATE TABLE test.InternetLeads
		PRINT 'TRUNCATED TABLE test.InternetLeads'
	END

INSERT INTO [2am].test.InternetLeads
(
TestIdentifier,Income01,Income02,ProfitFromSale,OtherContribution,
MonthlyInstallment,LoanTermYear,InterestRate,FirstNames,Surname,WorkPhoneCode,
WorkPhoneNo,NoApplicants,EmailAddress,OfferKey,OrigConsultant,ReAssignedConsultant
)
VALUES
(
'InternetLeadFromAffordability','80000','0','0','0','12000','20','9.8','Internet01',
'Lead 01','111','1234567','1','Lead01@mail.com','','SAHL\TELAUser',''
)	