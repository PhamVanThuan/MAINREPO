

USE [2AM]
Go

IF not exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'ThirdPartyType' And TABLE_SCHEMA = 'dbo')
begin
	Create Table dbo.ThirdPartyType
	(
		ThirdPartyTypeKey int not null,
		[Description] varchar(100) not null,

		CONSTRAINT [PK_ThirdPartyTypeKey] PRIMARY KEY CLUSTERED 
		(
			[ThirdPartyTypeKey] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

	) ON [PRIMARY]

end
go

GRANT SELECT ON [dbo].[ThirdPartyType] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[ThirdPartyType] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ThirdPartyType] TO [AppRole] AS [dbo]
GO
