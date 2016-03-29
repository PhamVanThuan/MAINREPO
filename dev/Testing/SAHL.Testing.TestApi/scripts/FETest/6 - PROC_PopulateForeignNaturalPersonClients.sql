use [FETest]
go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateForeignNaturalPersonClients')
	DROP PROCEDURE dbo.PopulateForeignNaturalPersonClients
GO

CREATE PROCEDURE dbo.PopulateForeignNaturalPersonClients

AS

BEGIN

IF(EXISTS(SELECT 1 FROM FETest.dbo.ForeignNaturalPersonClients))
	BEGIN
		truncate table FETest.dbo.ForeignNaturalPersonClients
	END

declare @ActiveClients table (LegalEntityKey int, idNumber varchar(20))

insert into @ActiveClients
select le.LegalEntityKey, le.IDNumber 
from [2am].dbo.Offer o
join [2am].dbo.OfferRole ofr on o.OfferKey = ofr.OfferKey
	and ofr.GeneralStatusKey = 1
join [2am].dbo.OfferRoleType ort on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
	and ort.OfferRoleTypeGroupKey = 3
join [2am].dbo.LegalEntity le on ofr.LegalEntityKey = le.LegalEntityKey
	and le.LegalEntityTypeKey = 2
where o.OfferStatusKey = 1
and len(le.FirstNames) > 5 and len(le.surname) > 5
and le.LegalEntityTypeKey = 2
and len(le.homePhoneCode) = 3 and len(le.homePhoneNumber) = 7
and len(le.workPhoneCode) = 3 and len(le.workPhoneNumber) = 7
and len(cellPhoneNumber) = 10
and len(emailAddress) > 10

insert into @ActiveClients
select le.LegalEntityKey, le.IDNumber 
from [2am].dbo.Account a
join [2am].dbo.Role r on a.AccountKey = r.AccountKey
	and r.GeneralStatusKey = 1
join [2am].dbo.RoleType rt on r.RoleTypeKey = rt.RoleTypeKey
	and rt.RoleTypeKey in (2,3)
join [2am].dbo.LegalEntity le on r.LegalEntityKey = le.LegalEntityKey
	and le.LegalEntityTypeKey = 2
left join @ActiveClients ac on le.LegalEntityKey = ac.LegalEntityKey
where a.AccountStatusKey in (1,4,5) 
and a.ParentAccountKey is null
and ac.LegalEntityKey is null
and len(le.FirstNames) > 5 and len(le.surname) > 5
and le.LegalEntityTypeKey = 2
and len(le.homePhoneCode) = 3 and len(le.homePhoneNumber) = 7
and len(le.workPhoneCode) = 3 and len(le.workPhoneNumber) = 7
and len(cellPhoneNumber) = 10
and len(emailAddress) > 10

insert into dbo.ForeignNaturalPersonClients (LegalEntityKey, CitizenshipTypeKey, PassportNumber)
select le.LegalEntityKey, le.CitizenTypeKey, PassportNumber 
from @ActiveClients ac
join [2am].dbo.LegalEntity le on ac.LegalEntityKey = le.LegalEntityKey
where len(le.FirstNames) > 5 and len(le.surname) > 5
and len(le.IDNumber) is null
and len(le.homePhoneCode) = 3 and len(le.homePhoneNumber) = 7
and len(le.workPhoneCode) = 3 and len(le.workPhoneNumber) = 7
and len(cellPhoneNumber) = 10
and len(emailAddress) > 10
and len(passportNumber) > 5
and le.CitizenTypeKey in 
(3,4,5,6,7,8,9,10)

END



