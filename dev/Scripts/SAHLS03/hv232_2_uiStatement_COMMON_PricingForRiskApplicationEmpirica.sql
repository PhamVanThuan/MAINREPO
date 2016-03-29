use [2am]
go

/**************************************************
 Statement  : COMMON\PricingForRiskApplicationEmpirica
 Change Date: 2015/09/21 09:54:20 AM - Version 2
 Change User: SAHL\NazirJ
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'PricingForRiskApplicationEmpirica' and ApplicationName = 'COMMON' and Version = 2) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'PricingForRiskApplicationEmpirica' and ApplicationName = 'COMMON' and Version > 2
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'PricingForRiskApplicationEmpirica' and ApplicationName = 'COMMON' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'PricingForRiskApplicationEmpirica' and ApplicationName = 'COMMON'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('COMMON', 'PricingForRiskApplicationEmpirica', GetDate(), 2, 'SAHL\NazirJ',
                '--PricingForRiskApplicationMaxEmpirica
declare @ElementMaxValue int, @ElementMinValue int
declare @OfferInformationVariableLoanCreditCriteriaKey int, @CreditCriteriaKey int, @ApplicationMaxEmpScore int, @capitecAttribute int

set @OfferInformationVariableLoanCreditCriteriaKey = -1
set @ApplicationMaxEmpScore = 0

--- get the offer information using offerkey being passed in
select
	@CreditCriteriaKey=isnull(max(oivl.CreditCriteriaKey),-1),
	 @capitecAttribute = max(capitecAttrType.OfferAttributeTypeGroupKey)
from 
	[2AM].dbo.OfferInformation (nolock) oi 
join
	[2am].dbo.OfferInformationVariableLoan (nolock) oivl 
on 
	oi.OfferInformationKey = oivl.OfferInformationKey
left join 
	[2AM].dbo.OfferAttribute attr on attr.OfferKey = oi.OfferKey
left join 
	[2AM].dbo.OfferAttributeType capitecAttrType 
on 
	capitecAttrType.OfferAttributeTypeKey = attr.OfferAttributeTypeKey and 
	capitecAttrType.OfferAttributeTypeGroupKey = 3 -- Capitec Offer Attribute Type Group Key
where 
	oi.OfferInformationKey = (select max(OfferInformationKey) from [2am].dbo.OfferInformation (nolock) where OfferKey = @OfferKey)

if (@CreditCriteriaKey > -1)
begin
	if exists (select 1 from [2am].dbo.RateAdjustmentElementCreditCriteria (nolock) 
				where RateAdjustmentElementKey = @RateAdjustmentElementKey and CreditCriteriaKey=@CreditCriteriaKey and @capitecAttribute is null)
	begin	
			--- get the rate adjustment element record by using rateadjustmentelementkey being passed in
			select 
				@ElementMinValue = rae.ElementMinValue,
				@ElementMaxValue = rae.ElementMaxValue
			from
				[2AM].dbo.RateAdjustmentElement (nolock) rae 
			where
				rae.RateAdjustmentElementKey = @RateAdjustmentElementKey
			and
				rae.generalstatuskey=1 -- only use these values if effective rate adjustment element active.
			and 
				rae.EffectiveDate <= getdate() -- only use these values if effective date older than now.
			
			-- Only get pricing adjustment element details if OfferInformationVariableLoanCreditCriteriaKey 
			select @ApplicationMaxEmpScore = isnull([dbo].[fGetMaxEmpiricaScoreForApplication](@OfferKey),0)
				
			if ((@ApplicationMaxEmpScore >= @ElementMinValue) and (@ApplicationMaxEmpScore <= @ElementMaxValue))
			begin   
				select @ApplicationMaxEmpScore
				return;
			end
			else
			begin
				select -1
				return;
			end
	end
	else
	begin
		select -1
		return;
	end
end
else
begin
		select -1
		return;
end',
                1, GetDate());
   end
end
