USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertFinancialServiceBankAccount') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.InsertFinancialServiceBankAccount
	Print 'Dropped procedure test.InsertFinancialServiceBankAccount'
End
Go

CREATE PROCEDURE test.InsertFinancialServiceBankAccount

@accountKey INT,
@financialServicePaymentTypeKey INT

AS

DECLARE @financialServiceKey INT
DECLARE @bankAccountKey INT
DECLARE @legalEntityKey INT


SELECT TOP 1 @financialServiceKey = financialServiceKey
FROM [2am].dbo.FinancialService fs
WHERE fs.accountKey=@accountKey AND financialServiceTypeKey in (1,10)

SELECT TOP 1 @bankaccountkey = bankaccountkey, @LegalEntityKey = leba.legalentitykey
FROM [2am].dbo.Role r
JOIN [2am].dbo.LegalEntity le ON r.legalentitykey=le.legalentityKey
JOIN [2am].dbo.LegalEntityBankAccount leba ON le.legalEntityKey=leba.legalEntityKey
WHERE r.roletypeKey IN (2,3)
AND r.accountkey=@accountKey
AND leba.generalStatusKey=1

INSERT INTO [2am].dbo.FinancialServiceBankAccount
(financialServiceKey, bankAccountKey, percentage, debitOrderDay, generalStatusKey, 
userId, changeDate, financialServicePaymentTypeKey)
VALUES (@financialServiceKey, @bankAccountKey, 1, 25, 1, 'sahl\testUser', 
GETDATE(), @financialServicePaymentTypeKey)

declare @key int
SELECT @key = Scope_Identity()
--remove all except ours
DELETE FROM [2am].dbo.FinancialServiceBankAccount
WHERE financialServiceKey = @financialServiceKey and financialServiceBankAccountKey <> @key