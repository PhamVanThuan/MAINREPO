USE [FETest]
GO

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'OpenNewBusinessApplications' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].OpenNewBusinessApplications
	end
go
/****** Object:  Table [dbo].[OpenNewBusinessApplications]    Script Date: 16/01/2015 10:41:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OpenNewBusinessApplications](
	[Id] INT IDENTITY(1,1),
	[OfferKey] [int] NOT NULL,
	[LTV] [float] NOT NULL,
	[SPVKey] [int] NULL,
	[PropertyKey] [int] NULL,
	[HasDebitOrder] [bit] NULL,
	[HasMailingAddress] [bit] NULL,
	[HasProperty] [bit] NULL,
	[IsAccepted] [bit] NULL,
	[HouseholdIncome] [float] NULL,
	[EmploymentTypeKey] [int] NULL
) ON [PRIMARY]

GO


