USE [2AM]

IF NOT EXISTS (SELECT schema_name
				FROM information_schema.schemata
				WHERE schema_name = 'doc')
BEGIN
	EXEC sp_executesql N'CREATE SCHEMA [doc] AUTHORIZATION [dbo]'
END
GO


USE [2AM]
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[doc].[JsonDocumentType]') AND type in (N'U'))
BEGIN
CREATE TABLE [doc].[JsonDocumentType](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_JsonDocumentType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

if(not exists(select * from [doc].[JsonDocumentType] where id = '1aa56229-298e-4db3-ab8e-a41f0078705c' and name =  'userprofile'))
Begin
INSERT INTO [doc].[JsonDocumentType] ([Id] ,[Name])  VALUES ('1aa56229-298e-4db3-ab8e-a41f0078705c','userprofile')
END
GO




IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[doc].[JsonDocument]') AND type in (N'U'))
BEGIN
	CREATE TABLE [doc].[JsonDocument](
		[Id] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](255) NULL,
		[Version] [int] NOT NULL,
		[DocumentFormatVersion] [nvarchar](10) NOT NULL,
		[DocumentType] [uniqueidentifier] NOT NULL,
		[Data] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_JsonDocument] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


	ALTER TABLE [doc].[JsonDocument]  WITH CHECK ADD  CONSTRAINT [FK_JsonDocument_JsonDocumentType] FOREIGN KEY([DocumentType])
	REFERENCES [doc].[JsonDocumentType] ([Id])


	ALTER TABLE [doc].[JsonDocument] CHECK CONSTRAINT [FK_JsonDocument_JsonDocumentType]



	CREATE UNIQUE NONCLUSTERED INDEX IX_JsonDocument ON doc.JsonDocument
		(
		Name
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


	ALTER TABLE doc.JsonDocument SET (LOCK_ESCALATION = TABLE)
END
GO





GO