USE [2AM]
GO

if not exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'CATSPaymentBatchType' And TABLE_SCHEMA = 'dbo')
begin

CREATE TABLE [dbo].[CATSPaymentBatchType](
	[CATSPaymentBatchTypeKey] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[CATSProfile] [nvarchar](5) NOT NULL,
	[CATSFileNamePrefix] [varchar](50) NOT NULL,
	[CATSEnvironment] [int] NOT NULL,
	[NextCATSFileSequenceNo] [int] NOT NULL CONSTRAINT [CATSFileSequenceNoDefault]  DEFAULT ((1)),
 CONSTRAINT [PK_CATSPaymentBatchType] PRIMARY KEY CLUSTERED 
(
	[CATSPaymentBatchTypeKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

end
GO

GRANT SELECT ON [dbo].[CATSPaymentBatchType] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[CATSPaymentBatchType] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[CATSPaymentBatchType] TO [AppRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO
GRANT INSERT ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO
GRANT UPDATE ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO

if not exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'CATSPaymentBatchStatus' And TABLE_SCHEMA = 'dbo')
begin

CREATE TABLE [dbo].[CATSPaymentBatchStatus](
	[CATSPaymentBatchStatusKey] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](25) NOT NULL,
 CONSTRAINT [PK_CATSPaymentBatchStatus] PRIMARY KEY CLUSTERED 
(
	[CATSPaymentBatchStatusKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

INSERT INTO [dbo].[CATSPaymentBatchStatus] ([Description])
	VALUES ('Processing'), ('Processed'), ('Failed'), ('Reversed')
end
go

GRANT SELECT ON [dbo].[CATSPaymentBatchStatus] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[CATSPaymentBatchStatus] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[CATSPaymentBatchStatus] TO [AppRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO
GRANT INSERT ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO
GRANT UPDATE ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO



if not exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'CATSPaymentBatch' And TABLE_SCHEMA = 'dbo')
begin
CREATE TABLE [dbo].[CATSPaymentBatch](
	[CATSPaymentBatchKey] [int] IDENTITY(1,1) NOT NULL,
	[CATSPaymentBatchTypeKey] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ProcessedDate] [datetime] NULL,
	[CATSPaymentBatchStatusKey] [int] NOT NULL,
	[CATSFileSequenceNo] [int] NULL,
	[CATSFileName] [varchar](100) NULL,
 CONSTRAINT [PK_CATSPaymentBatch] PRIMARY KEY CLUSTERED 
(
	[CATSPaymentBatchKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

end
Go

if not exists(Select * From INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where CONSTRAINT_NAME = 'FK_CATSPaymentBatch_CATSPaymentBatchType' And TABLE_NAME = 'CATSPaymentBatch' And TABLE_SCHEMA = 'dbo')
begin
ALTER TABLE [dbo].[CATSPaymentBatch]  WITH CHECK ADD  CONSTRAINT [FK_CATSPaymentBatch_CATSPaymentBatchType] FOREIGN KEY([CATSPaymentBatchTypeKey])
REFERENCES [dbo].[CATSPaymentBatchType] ([CATSPaymentBatchTypeKey])
end
GO


ALTER TABLE [dbo].[CATSPaymentBatch] CHECK CONSTRAINT [FK_CATSPaymentBatch_CATSPaymentBatchType]
GO


if not exists(Select * From INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where CONSTRAINT_NAME = 'FK_CATSPaymentBatchStatusKey' And TABLE_NAME = 'CATSPaymentBatch' And TABLE_SCHEMA = 'dbo')
begin
ALTER TABLE [dbo].[CATSPaymentBatch]  WITH NOCHECK ADD  CONSTRAINT [FK_CATSPaymentBatchStatusKey] FOREIGN KEY([CATSPaymentBatchStatusKey])
REFERENCES [dbo].[CATSPaymentBatchStatus] ([CATSPaymentBatchStatusKey])
end
GO

ALTER TABLE [dbo].[CATSPaymentBatch] CHECK CONSTRAINT [FK_CATSPaymentBatchStatusKey]
GO


GRANT SELECT ON [dbo].[CATSPaymentBatch] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[CATSPaymentBatch] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[CATSPaymentBatch] TO [AppRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO
GRANT INSERT ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO
GRANT UPDATE ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO



if not exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'CATSPaymentBatchItem' And TABLE_SCHEMA = 'dbo')
begin
CREATE TABLE [dbo].[CATSPaymentBatchItem](
	[CATSPaymentBatchItemKey] [int] IDENTITY(1,1) NOT NULL,
	[GenericKey] [int] NOT NULL,
	[GenericTypeKey] [int] NOT NULL,
	[AccountKey] [int] NOT NULL,
	[InvoiceTotal] [decimal](22, 10) NOT NULL,
	[SourceBankAccountKey] [int] NOT NULL,
	[TargetBankAccountKey] [int] NOT NULL,
	[CATSPaymentBatchKey] [int] NOT NULL,
	[SahlReferenceNumber] [varchar](200) NOT NULL,
	[SourceReferenceNumber] [varchar](200) NOT NULL,
	[TargetName] [varchar](200) NOT NULL,
	[ExternalReference] [varchar](100) NULL,
	[EmailAddress] [varchar](55) NULL,
	[LegalEntityKey] [int] NOT NULL,
 CONSTRAINT [PK_CATSPaymentBatchItem] PRIMARY KEY CLUSTERED 
(
	[CATSPaymentBatchItemKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
end
go

GO

SET ANSI_PADDING OFF
GO

if not exists(Select * From INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where CONSTRAINT_NAME = 'FK_CATSPaymentBatchKey' And TABLE_NAME = 'CATSPaymentBatchItem' And TABLE_SCHEMA = 'dbo')
begin
ALTER TABLE [dbo].[CATSPaymentBatchItem]  WITH NOCHECK ADD  CONSTRAINT [FK_CATSPaymentBatchKey] FOREIGN KEY([CATSPaymentBatchKey])
REFERENCES [dbo].[CATSPaymentBatch] ([CATSPaymentBatchKey])
end
GO

ALTER TABLE [dbo].[CATSPaymentBatchItem] CHECK CONSTRAINT [FK_CATSPaymentBatchKey]
GO

if not exists(Select * From INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where CONSTRAINT_NAME = 'FK_CATSPaymentBatchItem_LegalEntity' And TABLE_NAME = 'CATSPaymentBatchItem' And TABLE_SCHEMA = 'dbo')
begin
ALTER TABLE [dbo].[CATSPaymentBatchItem]  WITH NOCHECK ADD  CONSTRAINT [FK_CATSPaymentBatchItem_LegalEntity] FOREIGN KEY([LegalEntityKey])
REFERENCES [dbo].[LegalEntity] ([LegalEntityKey])
end
GO

ALTER TABLE [dbo].[CATSPaymentBatchItem] CHECK CONSTRAINT [FK_CATSPaymentBatchKey]
GO

GRANT SELECT ON [dbo].[CATSPaymentBatchItem] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[CATSPaymentBatchItem] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[CATSPaymentBatchItem] TO [AppRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[CATSPaymentBatchItem] TO [AppRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[CATSPaymentBatchItem] TO [eworkadmin2] AS [dbo]
GO
GRANT SELECT ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO
GRANT INSERT ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO
GRANT UPDATE ON [dbo].[CATSPaymentBatchType] TO [eworkadmin2] AS [dbo]
GO