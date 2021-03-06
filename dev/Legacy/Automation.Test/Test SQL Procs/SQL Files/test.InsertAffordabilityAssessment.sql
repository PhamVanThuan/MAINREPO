use [2AM]
go

set ansi_nulls on
go
set quoted_identifier on
go

if exists (select * from sys.objects where object_id = object_id(N'2AM.test.InsertAffordabilityAssessment') and type in (N'P',N'PC'))
drop procedure test.InsertAffordabilityAssessment
go

create procedure test.InsertAffordabilityAssessment

@offerKey int,
@affordabilityAssessmentStatusKey int

as
begin

	declare @affordabilityAssessmentKey int
	declare @modifiedDate date = getdate()
	declare @modifiedByUserId int = 1617
	declare @offerTypeKey int

	if exists (select * from [2AM].dbo.AffordabilityAssessment where GenericKey=@offerKey)
	begin

		delete from [2AM].dbo.AffordabilityAssessmentItem where AffordabilityAssessmentKey in (select AffordabilityAssessmentKey from [2AM].dbo.AffordabilityAssessment where GenericKey=@offerKey)
		delete from [2AM].dbo.AffordabilityAssessmentLegalEntity where AffordabilityAssessmentKey in (select AffordabilityAssessmentKey from [2AM].dbo.AffordabilityAssessment where GenericKey=@offerKey)
		delete from [2AM].dbo.AffordabilityAssessment where AffordabilityAssessmentKey in (select AffordabilityAssessmentKey from [2AM].dbo.AffordabilityAssessment where GenericKey=@offerKey)

	end

	select @offerTypeKey = OfferTypeKey from [2AM].dbo.Offer where OfferKey=@offerKey

	create table #legalentities (legalEntityKey int)

	if (@offerTypeKey in (2,3,4,6,7,8))
	begin

		insert into #legalentities
		select r.LegalEntityKey from [2AM].dbo.Offer o
		join [2AM].dbo.OfferRole r on o.OfferKey=r.OfferKey
		where o.OfferKey = @offerKey			  
		and r.OfferRoleTypeKey in (8,11)

	end
	else if (@offerTypeKey = 11)
	begin
	
		insert into #legalentities
		select r.LegalEntityKey from [2AM].dbo.Offer o
		join [2AM].dbo.ExternalRole r on o.OfferKey=r.GenericKey
		where o.OfferKey = @offerKey			  
		and r.ExternalRoleTypeKey=1

	end

	insert into [2AM].dbo.AffordabilityAssessment 
	(
		GenericKey, 
		GenericKeyTypeKey, 
		AffordabilityAssessmentStatusKey, 
		GeneralStatusKey,
		AffordabilityAssessmentStressFactorKey, 
		ModifiedDate,
		ModifiedByUserId, 
		NumberOfContributingApplicants, 
		NumberOfHouseholdDependants,
		MinimumMonthlyFixedExpenses, 
		ConfirmedDate, 
		Notes
	)
	Values
	(
		@offerKey,
		2,
		@affordabilityAssessmentStatusKey,
		1,
		2,
		@modifiedDate,
		@modifiedByUserId,
		(select count(legalEntityKey) from #legalentities),
		0,
		5580,
		(case 
			when @affordabilityAssessmentStatusKey = 1 then NULL
			when @affordabilityAssessmentStatusKey = 2 then @modifiedDate
			end),
		NULL
	)

	set @affordabilityAssessmentKey = scope_identity()

	insert into [2AM].dbo.AffordabilityAssessmentLegalEntity (AffordabilityAssessmentKey, LegalEntityKey)
	select @affordabilityAssessmentKey, legalEntityKey from #legalentities

	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,1,@modifiedDate,@modifiedByUserId,10000,10000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,2,@modifiedDate,@modifiedByUserId,10000,10000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,3,@modifiedDate,@modifiedByUserId,10000,10000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,4,@modifiedDate,@modifiedByUserId,10000,10000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,5,@modifiedDate,@modifiedByUserId,10000,10000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,6,@modifiedDate,@modifiedByUserId,10000,10000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,7,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,8,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,9,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,10,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,11,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,12,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,13,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,14,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,23,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,24,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,26,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,27,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,28,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,25,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,15,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,16,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,17,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,18,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,19,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,20,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,21,@modifiedDate,@modifiedByUserId,1000,1000)
	insert into [2AM].dbo.AffordabilityAssessmentItem (AffordabilityAssessmentKey,AffordabilityAssessmentItemTypeKey,ModifiedDate,ModifiedByUserId,ClientValue,CreditValue) VALUES(@affordabilityAssessmentKey,22,@modifiedDate,@modifiedByUserId,1000,1000)

	drop table #legalentities

end
go