use [FETest]

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateSPVs')
	DROP PROCEDURE dbo.PopulateSPVs
GO

CREATE PROCEDURE dbo.PopulateSPVs

AS

BEGIN

	IF(EXISTS(SELECT 1 FROM FETest.dbo.SPVs))
	BEGIN
		truncate table FETest.dbo.SPVs
	END

	insert into FETest.dbo.SPVs
	select c.SPVKey, c.Description, c.ReportDescription, c.ParentSPVKey, p.Description as ParentSPVDescription, 
	c.ReportDescription as ParentSPVReportDescription, c.SPVCompanyKey, sc.Description as SPVCompanyDescription, c.GeneralStatusKey, 
	ba.BankAccountKey, b.ACBBankDescription as BankName, ba.ACBBranchCode as BranchCode,br. ACBBranchDescription as BranchName, ba.AccountName, ba.AccountNumber, at.ACBTypeDescription as AccountType
	from [2am].spv.spv c
	left join [2am].spv.SPV p on c.ParentSPVKey = p.SPVKey
	join [2am].spv.SPVCompany sc on c.SPVCompanyKey = sc.SPVCompanyKey
	join [2am].dbo.BankAccount ba on c.BankAccountKey = ba.BankAccountKey
	join [2AM].dbo.ACBType at on ba.ACBTypeNumber = at.ACBTypeNumber
	join [2AM].dbo.ACBBranch br on ba.ACBBranchCode = br.ACBBranchCode
	join [2AM].dbo.ACBBank b on br.ACBBankCode = b.ACBBankCode

END