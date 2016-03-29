USE [2AM]
GO
IF not exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'ITCRequest' And TABLE_SCHEMA = 'capitec')
begin
CREATE TABLE [capitec].[ITCRequest](
	[Id] [uniqueidentifier] NOT NULL,
	[ITCDate] [datetime] NOT NULL,
	[ITCData] [xml] NOT NULL,
 CONSTRAINT [PK_ITCRequest] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [capitec].[ITCRequest] ADD  CONSTRAINT [DF_dbo_ITC_ITCDate]  DEFAULT (getdate()) FOR [ITCDate]
end
GO


GRANT SELECT ON [capitec].[ITCRequest] TO [AppRole]
GO
GRANT INSERT ON [capitec].[ITCRequest] TO [AppRole]
GO
GRANT UPDATE ON [capitec].[ITCRequest] TO [AppRole]
GO