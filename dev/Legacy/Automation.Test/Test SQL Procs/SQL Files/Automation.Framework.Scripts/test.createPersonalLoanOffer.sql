USE [2AM]
GO
/****** 
Object:  StoredProcedure [test].[createPersonalLoanOffer]    
Script Date: 10/19/2010 16:48:58 
******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[createPersonalLoanOffer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].createPersonalLoanOffer

GO

CREATE procedure test.createPersonalLoanOffer

@legalEntityKey int

as

declare @monthlyInstalment float
declare @ReservedAccountKey int
declare @loanAmount float
declare @term int
declare @CreditCriteriaUnsecuredLendingKey int
declare @marginKey int
declare @marginRate float
declare @marketRate float
declare @totalRate float
declare @lifePremium float
declare @lifeMultiplier float
declare @serviceFee float
declare @initFee float
declare @feesTotal float
declare @offerKey int
declare @offerInformationKey int


--set values
set @loanAmount = 0
set @term = 30

while @loanAmount < 10000
begin
select @loanAmount = round(Rand()*30000,2)
end

--get the values required

select @CreditCriteriaUnsecuredLendingKey = CreditCriteriaUnsecuredLendingKey, @marginKey = marginKey 
from [2am].dbo.creditcriteriaunsecuredlending
where (@loanAmount >=  minLoanAmount and  @loanAmount <= maxLoanAmount)
and term = @term

select @marginRate = value from [2am].dbo.margin where marginkey = @marginKey
select @marketRate = value from [2am].dbo.marketRate where marketRateKey = 6
select @lifeMultiplier = controlNumeric from [2am].dbo.[control] where controlDescription = 'PersonalLoanCreditLifePremium'
select @serviceFee = controlNumeric from [2am].dbo.[control] where controlDescription = 'PersonalLoanMonthlyFee'
select @initFee = controlNumeric from [2am].dbo.[control] where controlDescription = 'PersonalLoanInitiationFee'

set @totalRate = @marginRate + @marketRate

	--insert into accountSequence
	INSERT INTO dbo.AccountSequence (IsUsed) VALUES (0) 
	select @ReservedAccountKey = SCOPE_IDENTITY()
	
		SET @MonthlyInstalment = (@loanAmount+@initFee) *  (Power(1 + ((@totalRate) / 12),@Term)) * 
	(((@totalRate) / 12) / (Power(1 + ((@totalRate) / 12), @Term) - 1))
	
	set @lifePremium = @lifeMultiplier*(@loanAmount+@initFee)
	set @feesTotal = @initFee	

insert into [2am].dbo.Offer
(offerTypeKey, offerStatusKey, offerStartDate, reservedAccountKey,
originationSourceKey)
values
(11, 1, getdate(), @ReservedAccountKey, 1)

select @offerKey = SCOPE_IDENTITY()

declare @expectedEndState varchar(50)

select @expectedEndState = expectedEndState 
from test.personalLoanAutomationCases
where legalEntityKey = @legalEntityKey

if @expectedEndState <> 'Manage Lead'

begin

	insert into [2am].dbo.OfferInformation
	(offerInsertDate, offerKey, offerInformationTypeKey, userName, changeDate,
	productKey)
	values
	(getdate(), @offerKey, 1, 'SAHL\TestUser', getdate(), 12)

	select @offerInformationKey = SCOPE_IDENTITY()

	insert into [2am].dbo.OfferInformationPersonalLoan 
	select @offerInformationKey, @loanAmount, @term, @monthlyInstalment,
	@lifePremium, @feesTotal, @CreditCriteriaUnsecuredLendingKey, @marginKey,
	6

	declare @expenseTypeKey int
	select @expenseTypeKey = expenseTypeKey 
	from [2am]..expenseType where description = 'Personal Loan Initiation Fee'
	--add the offerexpense
	insert into [2am]..OfferExpense
	(offerKey, expenseTypeKey, totalOutstandingAmount, tobeSettled)
	values
	(@offerKey, @expenseTypeKey, @initFee, 1)
	
	declare @ExcludeCreditLife int
	set @ExcludeCreditLife = 0
	
	select @ExcludeCreditLife = isnull(ExcludeCreditLife, 0) from test.PersonalLoanAutomationCases where legalEntityKey = @legalentityKey
	
	if (@ExcludeCreditLife = 0)
		begin
			INSERT INTO dbo.OfferAttribute( OfferKey, OfferAttributeTypeKey )
			VALUES  ( @offerKey, 12  )

		end
	else
		begin
			--zero out the premium
			update [2am].dbo.OfferInformationPersonalLoan
			set LifePremium = 0
			where offerInformationKey = @offerInformationKey
			--insert the external life policy information
			insert into [2am].dbo.ExternalLifePolicy
			(InsurerKey, PolicyNumber, CommencementDate, LifePolicyStatusKey, SumInsured, PolicyCeded, LegalEntityKey)
			values( 7, 'TestPolicyNumber', dateadd(mm, -6, getdate()), 3, @loanAmount*2, 1, @legalentityKey)
			declare @ExternalLifePolicyKey int
			set @ExternalLifePolicyKey = SCOPE_IDENTITY()
			--insert the link
			insert into [2am].dbo.OfferExternalLife
			(offerKey, ExternalLifePolicyKey)
			values(
			@OfferKey, @ExternalLifePolicyKey
			)
		end


end

	--add the external role
	insert into [2am]..externalRole
	select @offerKey, 2, @legalentityKey, 1, 1, getdate()


--update table with offerkey

update test.personalLoanAutomationCases
set offerKey = @offerKey
where legalEntityKey = @legalEntityKey




 

 
