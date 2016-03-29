use [FETest]
go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateAlphaHousingApplications')
	DROP PROCEDURE dbo.PopulateAlphaHousingApplications
GO

CREATE PROCEDURE dbo.PopulateAlphaHousingApplications

AS

BEGIN

IF(EXISTS(SELECT 1 FROM [FETest].dbo.AlphaHousingApplications))
	BEGIN
		truncate table [FETest].dbo.AlphaHousingApplications
	END

insert into [FETest].dbo.AlphaHousingApplications (OfferKey)
select app.offerKey 
from [FeTest].dbo.OpenNewBusinessApplications app
join [2am].dbo.OfferAttribute oa on app.offerKey = oa.offerKey
where oa.offerAttributeTypeKey = 26

END
