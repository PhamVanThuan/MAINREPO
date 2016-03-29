USE [FETest]
GO


IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateActiveNewBusinessApplicants')
	DROP PROCEDURE dbo.PopulateActiveNewBusinessApplicants
GO

CREATE PROCEDURE dbo.PopulateActiveNewBusinessApplicants

AS

BEGIN

IF(EXISTS(SELECT 1 FROM [FETest].dbo.ActiveNewBusinessApplicants))
	BEGIN
		truncate table [FETest].dbo.ActiveNewBusinessApplicants
	END

insert into [FETest].dbo.ActiveNewBusinessApplicants 
(offerKey, offerRoleKey, LegalEntityKey, OfferRoleTypeKey, IsIncomeContributor, HasDeclarations, HasAffordabilityAssessment,
HasAssetsLiabilities, HasBankAccount, HasEmployment, HasResidentialAddress, HasPostalAddress, HasDomicilium)
select distinct ofr.OfferKey, ofr.OfferRoleKey, ofr.LegalEntityKey, OfferRoleTypeKey,
case when ora.offerRoleKey is null then 0 else 1 end as IncomeContributing,
case when od.offerRoleKey is null then 0 else 1 end as HasDeclarations,
case when lea.LegalEntityAffordabilityKey is null then 0 else 1 end as HasAffordabilityAssessment,
case when al.LegalEntityKey is null then 0 else 1 end as HasAssetsLiabilities,
case when leba.LegalEntityKey is null then 0 else 1 end as HasBankAccount,
case when e.EmploymentKey is null then 0 else 1 end as HasEmployment,
0 as HasResidentialAddress,
0 as HasPostalAddress,
case when ord.OfferRoleKey is null then 0 else 1 end as HasDomicilium
from FETest.dbo.OpenNewBusinessApplications o
join [2am].dbo.OfferRole ofr on o.offerKey = ofr.offerKey
	and ofr.OfferRoleTypeKey in (8,10,11,12)
	and ofr.GeneralStatusKey = 1
left join [2am].dbo.OfferRoleAttribute ora on ofr.OfferRoleKey = ora.OfferRoleKey
	and OfferRoleAttributeTypeKey = 1
left join [2am].dbo.OfferDeclaration od on ofr.OfferRoleKey = od.OfferRoleKey
left join [2am].dbo.LegalEntityAffordability lea on ofr.LegalEntityKey = lea.LegalEntityKey 
	and ofr.OfferKey = lea.OfferKey
left join [2am].dbo.LegalEntityAssetLiability al on ofr.LegalEntityKey = al.LegalEntityKey
left join [2am].dbo.LegalEntityBankAccount leba on ofr.legalEntityKey = leba.LegalEntityKey
left join [2am].dbo.Employment e on ofr.LegalEntityKey = e.LegalEntityKey
	and (e.MonthlyIncome > 0 or e.ConfirmedIncome > 0)
	and e.EmploymentStatusKey = 1
	and e.EmploymentTypeKey not in (4,5)
left join [2am].dbo.OfferRoleDomicilium ord on ofr.OfferRoleKey=ord.offerRoleKey

select distinct a.legalEntityKey, ca.AddressTypeKey
into #hasAddresses
from [FETest].dbo.ActiveNewBusinessApplicants a
join [FETest].dbo.ClientAddresses ca on a.LegalEntityKey=ca.LegalEntityKey
	and ca.AddressTypeKey in (1,2)
join [2am].dbo.LegalEntityAddress lea on ca.LegalEntityAddressKey=lea.LegalEntityAddressKey
	and lea.GeneralStatusKey = 1

update [FETest].dbo.ActiveNewBusinessApplicants
set hasResidentialAddress = 1
from #hasAddresses
where ActiveNewBusinessApplicants.legalEntityKey = #hasAddresses.legalEntityKey
and #hasAddresses.AddressTypeKey = 1

update [FETest].dbo.ActiveNewBusinessApplicants
set hasPostalAddress = 1
from #hasAddresses
where ActiveNewBusinessApplicants.legalEntityKey = #hasAddresses.legalEntityKey
and #hasAddresses.AddressTypeKey = 2

drop table #hasAddresses

END






