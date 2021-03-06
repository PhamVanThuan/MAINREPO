USE [2AM]
GO
IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InternetLeads') and OBJECTPROPERTY(id, N'IsTable') = 1)
Begin
	DROP TABLE test.InternetLeads
	Print 'Dropped TABLE test.InternetLeads'
End
Go

		CREATE TABLE test.InternetLeads
		(
			TestIdentifier VARCHAR(50),
			Income01 VARCHAR(50),
			Income02 VARCHAR(50),
			ProfitFromSale VARCHAR(50),
			OtherContribution VARCHAR(50),
			MonthlyInstallment VARCHAR(50),
			LoanTermYear VARCHAR(50),
			InterestRate VARCHAR(50),
			FirstNames VARCHAR(50),
			Surname VARCHAR(50),
			WorkPhoneCode VARCHAR(50),
			WorkPhoneNo VARCHAR(50),
			NoApplicants VARCHAR(50),
			EmailAddress VARCHAR(50),
			OfferKey VARCHAR(50),
			OrigConsultant VARCHAR(50),
			ReAssignedConsultant VARCHAR(50)
		)
	



