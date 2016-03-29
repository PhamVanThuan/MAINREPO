USE [2AM];

IF NOT EXISTS (
SELECT  schema_name
FROM    information_schema.schemata
WHERE   schema_name = 'datastor' ) 

BEGIN
EXEC sp_executesql N'CREATE SCHEMA datastor'
END

IF  NOT EXISTS (SELECT '1' FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[2AM].[datastor].[ThirdPartyInvoicesSTOR]')) 
BEGIN
CREATE TABLE [2AM].[datastor].[ThirdPartyInvoicesSTOR](
	[AccountKey] [int] NOT NULL,
	[ThirdPartyInvoiceKey] [int] NOT NULL,
	[EmailSubject] [varchar](100) NULL,
	[FromEmailAddress] [varchar](100) NULL,
	[InvoiceFileName] [varchar](200) NOT NULL,
	[Category] [varchar](50) NULL,
	[DateReceived] [datetime] NOT NULL,
	[DateProcessed] [datetime] NULL,
	[STORKey] [int] NOT NULL,
	[DocumentGuid] [uniqueidentifier] NOT NULL
)
END


GRANT SELECT, INSERT, ALTER, UPDATE, DELETE  
	ON [2AM].[datastor].[ThirdPartyInvoicesSTOR] TO ServiceArchitect
Go