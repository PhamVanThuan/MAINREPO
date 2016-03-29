USE [2AM]
GO

/****** Object:  Table [dbo].[ThirdPartyPaymentBankAccount]    Script Date: 2015-04-21 11:44:47 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'ThirdPartyPaymentBankAccount'))
BEGIN

CREATE TABLE [dbo].[ThirdPartyPaymentBankAccount](
	[ThirdPartyPaymentBankAccountKey] [int] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[BankAccountKey] [int] NOT NULL,
	[ThirdPartyKey] [int] NOT NULL,
	[GeneralStatusKey] [int] NOT NULL,
	[BeneficiaryBankCode] [varchar](5) NOT NULL,
 CONSTRAINT [PK_ThirdPartyPaymentBankAccount] PRIMARY KEY CLUSTERED 
(
	[ThirdPartyPaymentBankAccountKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[ThirdPartyPaymentBankAccount]  WITH CHECK ADD  CONSTRAINT [FK_ThirdPartyPaymentBankAccount_GeneralStatus] FOREIGN KEY([GeneralStatusKey])
REFERENCES [dbo].[GeneralStatus] ([GeneralStatusKey])
ALTER TABLE [dbo].[ThirdPartyPaymentBankAccount] CHECK CONSTRAINT [FK_ThirdPartyPaymentBankAccount_GeneralStatus]

ALTER TABLE [dbo].[ThirdPartyPaymentBankAccount]  WITH CHECK ADD  CONSTRAINT [FK_ThirdPartyPaymentBankAccount_BankAccount] FOREIGN KEY([BankAccountKey])
REFERENCES [dbo].[BankAccount] ([BankAccountKey])
ALTER TABLE [dbo].[ThirdPartyPaymentBankAccount] CHECK CONSTRAINT [FK_ThirdPartyPaymentBankAccount_BankAccount]

ALTER TABLE [dbo].[ThirdPartyPaymentBankAccount]  WITH CHECK ADD  CONSTRAINT [FK_ThirdPartyPaymentBankAccount_ThirdParty] FOREIGN KEY([ThirdPartyKey])
REFERENCES [dbo].[ThirdParty] ([ThirdPartyKey])
ALTER TABLE [dbo].[ThirdPartyPaymentBankAccount] CHECK CONSTRAINT [FK_ThirdPartyPaymentBankAccount_ThirdParty]

End
GO

GRANT SELECT ON [dbo].[ThirdPartyPaymentBankAccount] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[ThirdPartyPaymentBankAccount] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ThirdPartyPaymentBankAccount] TO [AppRole] AS [dbo]
GO
