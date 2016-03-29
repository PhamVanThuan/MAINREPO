use [2am]

declare @CreditCriteriaKeys table (id int, creditcriteriakey int)
declare @NewCreditMatrixKey int
declare @CategoryKey int
declare @EmploymentTypeKey int
declare @maxLTV int
declare @Margin float
declare @MarginKey int
declare @MinEmpiricaScore int
declare @MinimumIncomeAmount int
declare @MaximumIncomeAmount int
declare @isGEPF bit
declare @isNewBusiness bit
declare @isFurtherLendingAlpha bit
declare @loop int
declare @total int
declare @cckey int
set @loop=1
set @total=1
set @cckey=0


--- SALARIED Category 1
-- Credit Criteria
set @NewCreditMatrixKey	= 54
set @CategoryKey = 1
set @maxLTV = 80
set @EmploymentTypeKey = 1
set @Margin	= 0.032
set @MinEmpiricaScore = 575
set @MinimumIncomeAmount = 20000
--set MaximumIncomeAmount = null
-- Credit Criteria Attributes
set @isGEPF	= 'false'
set @isNewBusiness = 'true'
set @isFurtherLendingAlpha = 'false'


-- get the creditcriteriakeys we are updating
insert into @CreditCriteriaKeys
	select row_number() over(order by CreditCriteriaKey) as id,  CreditCriteriaKey from [2am].dbo.CreditCriteria where CreditMatrixKey=@NewCreditMatrixKey and EmploymentTypeKey = @EmploymentTypeKey and CategoryKey = @CategoryKey	
	
-- total number of credit criteria we are updating
select @total=count(creditcriteriakey) from @CreditCriteriaKeys

-- get the first match marginkey by Value
select @MarginKey = min(MarginKey) from [2am].dbo.Margin where Value = @Margin


--===>>> START UPDATING
-- Update Credit Criteria generic credit criteria elements accross mortgageloanpurpose and maxloanamounts
if (@total > 0)
begin
	update 
		[2am].dbo.CreditCriteria 
	set	
		LTV = @maxLTV,
		MinIncomeAmount = @MinimumIncomeAmount,  
		MaxIncomeAmount = null,  --@MaximumIncomeAmount
		MarginKey = @MarginKey,
		MinEmpiricaScore = @MinEmpiricaScore
	where 
		CreditCriteriaKey in (select CreditCriteriaKey from @CreditCriteriaKeys)

	--set credit criteria attributes correctly
	while (@loop <= @total)
	begin
			select @cckey=creditcriteriakey from @creditcriteriakeys where id=@loop
	
            EXEC [2AM].[dbo].[pAddCreditCriteriaAttribute]  @isNewBusiness ,@isFurtherLendingAlpha ,@isGEPF ,@cckey
		
			set @loop = @loop + 1
	end
end
----->>>>> END UPDATING
delete from @CreditCriteriaKeys
