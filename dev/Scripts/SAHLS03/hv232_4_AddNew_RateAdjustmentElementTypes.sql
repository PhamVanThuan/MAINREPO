use [2am]
go

--Create new Rate Adjustement Element Type to call new generic Credit Criteria based statement for Application Max Empirica
if not exists(select 1 from [2AM].dbo.RateAdjustmentElementType where RateAdjustmentElementTypeKey=48)
begin
	insert into [2AM].dbo.RateAdjustmentElementType
		select 48, 'Credit Criteria Price Risk App Empirica','PricingForRiskApplicationEmpirica'
end
else
begin
	update 
		[2AM].dbo.RateAdjustmentElementType 
	set 
		description='Credit Criteria Price Risk App Empirica', 
		statementname='PricingForRiskApplicationEmpirica' 
	where 
		RateAdjustmentElementTypeKey = 48
end 

--Create new Rate Adjustement Element Type to call new generic Credit Criteria based statement for Loan Agreement Amount
if not exists(select 1 from [2AM].dbo.RateAdjustmentElementType where RateAdjustmentElementTypeKey=49)
begin
	insert into [2AM].dbo.RateAdjustmentElementType
		select 49, 'Credit Criteria Price Risk App LAA','PricingForRiskApplicationLoanAgreementAmount'
end
else
begin
	update 
		[2AM].dbo.RateAdjustmentElementType 
	set 
		description='Credit Criteria Price Risk App LAA', 
		statementname='PricingForRiskApplicationLoanAgreementAmount' 
	where 
		RateAdjustmentElementTypeKey = 49
end 

--Create new Rate Adjustement Element Type to call new Capitec Credit Criteria based statement for Application Max Empirica
if not exists(select 1 from [2AM].dbo.RateAdjustmentElementType where RateAdjustmentElementTypeKey=50)
begin
	insert into [2AM].dbo.RateAdjustmentElementType
		select 50, 'Credit Criteria Capitec Price Risk App Empirica','CapitecPricingForRiskApplicationEmpirica'
end
else
begin
	update 
		[2AM].dbo.RateAdjustmentElementType 
	set 
		description='Credit Criteria Capitec Price Risk App Empirica', 
		statementname='CapitecPricingForRiskApplicationEmpirica' 
	where 
		RateAdjustmentElementTypeKey = 50
end 