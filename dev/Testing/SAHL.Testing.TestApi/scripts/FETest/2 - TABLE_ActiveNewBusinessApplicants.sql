USE [FETest]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'ActiveNewBusinessApplicants')
	DROP TABLE dbo.[ActiveNewBusinessApplicants]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ActiveNewBusinessApplicants](
    [Id] [int] IDENTITY(1,1) NOT NULL,
	[OfferKey] [int] NOT NULL,
	[OfferRoleKey] [int] NOT NULL,
	[LegalEntityKey] [int] NOT NULL,
	[OfferRoleTypeKey] [int] NOT NULL,
	[IsIncomeContributor] [bit] NOT NULL CONSTRAINT [DF_ActiveNewBusinessApplicants_IsIncomeContributor]  DEFAULT ((0)),
	[HasDeclarations] [bit] NULL CONSTRAINT [DF_ActiveNewBusinessApplicants_HasDeclarations]  DEFAULT ((0)),
	[HasAffordabilityAssessment] [bit] NULL,
	[HasAssetsLiabilities] [bit] NULL,
	[HasBankAccount] [bit] NULL,
	[HasEmployment] [bit] NULL,
	[HasResidentialAddress] [bit] NULL,
	[HasPostalAddress] [bit] NULL,
	[HasDomicilium] [bit] NULL
) ON [PRIMARY]

GO


