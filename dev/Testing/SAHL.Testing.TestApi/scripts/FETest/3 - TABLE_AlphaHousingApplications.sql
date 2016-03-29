USE FETest

go

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'AlphaHousingApplications' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].[AlphaHousingApplications]
	end
go


CREATE TABLE [dbo].[AlphaHousingApplications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OfferKey] [int] NOT NULL
) ON [PRIMARY]

GO


