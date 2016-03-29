USE [2AM]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CleanupLegalEntityOfferRoleDomicilium') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CleanupLegalEntityOfferRoleDomicilium
	Print 'Dropped procedure test.CleanupLegalEntityOfferRoleDomicilium'
End
Go

Create PROCEDURE test.CleanupLegalEntityOfferRoleDomicilium
	@offerKey int
AS
begin
	---------------------------GET TEST DATA FOR OFFER----------------------------
	if object_id('tempdb..#LegalEntityAddressTestData') is not null
	begin
		drop table #LegalEntityAddressTestData
	end
	if object_id('tempdb..#LegalEntityResidentialStreetAddresses') is not null
	begin
		drop table #LegalEntityResidentialStreetAddresses
	end
	-----------------------------------Insert Address with count if legal entity has more than one address------------------------------
	select 
		lea.legalentitykey,
		(select top 01 
			legalentityaddresskey
		  from 
			dbo.legalentityaddress
				join dbo.address as a
					on legalentityaddress.addresskey=a.addresskey
			 where 
				addressformatkey = 1 
				and addresstypekey = 1 
				and generalstatuskey = 1
				and legalentitykey = lea.legalentitykey) as LegalEntityAddressKey
	into #LegalEntityResidentialStreetAddresses
	from dbo.legalentityaddress as lea
		join dbo.address as a
			on lea.addresskey=a.addresskey
	where addressformatkey = 1
		and addresstypekey = 1 
		and generalstatuskey = 1
	group by
		lea.legalentitykey

	-----------------------------------Insert #LegalEntityAddressTestData ------------------------------
	select
		NULL as LegalEntityDomociliumKey,
		lea.LegalEntityAddressKey,
		3 as GeneralStatusKey ,-- Pending
		getdate() as ChangeDate,
		2791 as ADUserKey,
		ofr.OfferRoleKey,
		ofr.offerkey
	into #LegalEntityAddressTestData
	from dbo.offerrole as ofr
		join dbo.offerroletype as ort
			on ofr.offerroletypekey=ort.offerroletypekey
		join #LegalEntityResidentialStreetAddresses as lea
			on ofr.legalentitykey=lea.legalentitykey
	where 
		ort.offerroletypegroupkey = 3 
		and offerkey = @offerKey

	-----------------------------------Setup LegalEntity Offer Domicilium ------------------------------
	insert into dbo.legalentitydomicilium
	select LegalEntityAddressKey,GeneralStatusKey,ChangeDate,ADUserKey from #LegalEntityAddressTestData

	insert into dbo.offerroledomicilium
	select led.LegalEntityDomiciliumKey,lea.OfferRoleKey,lea.ChangeDate,lea.ADUserKey  from #LegalEntityAddressTestData as lea
		join dbo.legalentitydomicilium as led
			on lea.legalentityaddresskey=led.legalentityaddresskey
	where offerkey = @offerKey
end
