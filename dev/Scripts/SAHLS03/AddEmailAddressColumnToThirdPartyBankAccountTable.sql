 USE [2AM]
 GO

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'EmailAddress' AND Object_ID = Object_ID(N'ThirdPartyPaymentBankAccount'))
BEGIN
    ALTER TABLE [2AM].[dbo].[ThirdPartyPaymentBankAccount]
 	ADD EmailAddress VARCHAR(100) NULL
END

