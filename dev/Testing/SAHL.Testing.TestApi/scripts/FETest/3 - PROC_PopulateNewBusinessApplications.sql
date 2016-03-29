use [FETest]
go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateOpenNewBusinessApplications')
	DROP PROCEDURE dbo.PopulateOpenNewBusinessApplications
GO

CREATE PROCEDURE dbo.PopulateOpenNewBusinessApplications

AS

BEGIN

IF(EXISTS(SELECT 1 FROM [FeTest].dbo.OpenNewBusinessApplications ))
	BEGIN
		truncate table [FeTest].dbo.OpenNewBusinessApplications 
	END


insert into [FeTest].dbo.OpenNewBusinessApplications (
OfferKey, LTV, SPVKey, PropertyKey, HasDebitOrder, HasMailingAddress, HasProperty, IsAccepted, HouseholdIncome, EmploymentTypeKey)
select o.offerKey, isnull(oivl.ltv,0) as LTV, oivl.SPVKey, oml.PropertyKey,
case when odo.offerKey is null then 0 else 1 end as HasDebitOrder,
case when oma.offerKey is null then 0 else 1 end as HasMailingAddress,
case when oml.PropertyKey is null then 0 else 1 end as HasProperty,
case when oi.OfferInformationTypeKey = 3 then 1 else 0 end as IsAccepted,
0,
0
from [2am].dbo.Offer o
join x2.x2data.Application_Capture ac on o.offerKey = ac.ApplicationKey
join x2.x2.instance i on ac.InstanceID = i.ID
	and i.ParentInstanceID is null
join [2am].dbo.OfferMortgageLoan oml on o.offerKey=oml.OfferKey
join [2am].dbo.OfferInformation oi on o.offerKey = oi.offerKey
join [2am].dbo.OfferInformationVariableLoan oivl on oi.OfferInformationKey = oivl.OfferInformationKey
outer apply (select max(offerInformationKey) oiKey from [2am].dbo.offerInformation oi
	where oi.offerKey = o.offerKey) as maxoi
left join [2am].dbo.OfferDebitOrder odo on o.offerKey = odo.offerKey
left join [2am].dbo.OfferMailingAddress oma on o.offerKey = oma.offerKey
where o.offerTypeKey in (6,7,8) and o.OfferStatusKey in (1)
and OriginationSourceKey <> 4
and (reference not like '%Prospect%' or reference is null)
and oivl.OfferInformationKey = maxoi.oiKey

SELECT o.offerKey, 
CASE WHEN e.employmenttypekey = 5 THEN 1 ELSE e.employmenttypekey END AS EmploymentTypeKey, 
SUM(case when isnull(e.confirmedBasicIncome,0) = 0 then isnull(e.BasicIncome,0) else isnull(e.ConfirmedBasicIncome,0) end) AS confirmedincome,
ROW_NUMBER() OVER (PARTITION BY o.offerKey ORDER BY SUM(confirmedBasicIncome) DESC) AS ord, 
SUM(case when isnull(e.ConfirmedIncome,0) = 0 then isnull(e.BasicIncome,0) else isnull(e.ConfirmedIncome,0) end) as householdIncomeContribution
into #calculated
from [FeTest].dbo.OpenNewBusinessApplications o
join [2am].dbo.OfferRole ofr on o.offerKey = ofr.offerKey
	and ofr.OfferRoleTypeKey in (8,10,11,12)
	and ofr.GeneralStatusKey = 1
join [2am].dbo.OfferRoleAttribute ora on ofr.offerRoleKey = ora.offerRoleKey
	and ora.OfferRoleAttributeTypeKey = 1
JOIN [2am].[dbo].employment e ON ofr.legalentitykey=e.legalentitykey 
	AND employmentstatuskey=1
GROUP BY o.offerKey, e.employmenttypekey;

update [FeTest].dbo.OpenNewBusinessApplications
set EmploymentTypeKey = #calculated.EmploymentTypeKey
from #calculated
where OpenNewBusinessApplications.OfferKey = #calculated.OfferKey
and ord = 1

select offerKey, sum(#calculated.householdIncomeContribution) as HouseholdIncomeTotal
into #rolledUp
from #calculated
group by offerKey

update [FeTest].dbo.OpenNewBusinessApplications
set householdIncome = HouseholdIncomeTotal
from #rolledUp
where OpenNewBusinessApplications.OfferKey = #rolledUp.OfferKey

drop table #calculated
drop table #rolledUp

END