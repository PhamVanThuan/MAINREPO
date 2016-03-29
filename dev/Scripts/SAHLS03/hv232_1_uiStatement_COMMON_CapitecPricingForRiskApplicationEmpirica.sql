use [2am]
go

/**************************************************
 Statement  : COMMON\CapitecPricingForRiskApplicationEmpirica
 Change Date: 2015/09/04 14:23:52 PM - Version 1
 Change User: SAHL\ivorj
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'CapitecPricingForRiskApplicationEmpirica' and ApplicationName = 'COMMON' and Version = 1) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'CapitecPricingForRiskApplicationEmpirica' and ApplicationName = 'COMMON' and Version > 1
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'CapitecPricingForRiskApplicationEmpirica' and ApplicationName = 'COMMON' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'CapitecPricingForRiskApplicationEmpirica' and ApplicationName = 'COMMON'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('COMMON', 'CapitecPricingForRiskApplicationEmpirica', GetDate(), 1, 'SAHL\ivorj',
                '--CapitecPricingForRiskApplicationMaxEmpirica
declare @ElementMaxValue int, @ElementMinValue int, @isCapitec bit
declare @OfferInformationVariableLoanCreditCriteriaKey int, @CreditCriteriaKey int, @ApplicationMaxEmpScore int

set @isCapitec = 0
set @OfferInformationVariableLoanCreditCriteriaKey = -1

--- get the offer information using offerkey being passed in
select
	@CreditCriteriaKey=isnull(oivl.CreditCriteriaKey,-1),
	@isCapitec=case when oa.offerattributekey is not null then 1 else 0 end
from 
	[2AM].dbo.OfferInformation (nolock) oi 
join
	[2am].dbo.OfferInformationVariableLoan (nolock) oivl on oi.OfferInformationKey = oivl.OfferInformationKey
left join
	[2am].dbo.OfferAttribute (nolock) oa on oa.Offerkey = oi.Offerkey and oa.OfferAttributeTypeKey = 30 -- Capitec offer attribute
where 
	oi.OfferInformationKey = (select max(OfferInformationKey) from [2am].dbo.OfferInformation (nolock) where OfferKey = @OfferKey)

if ((@CreditCriteriaKey > -1) and (@isCapitec = 1))
begin
	if exists (select 1 from [2am].dbo.RateAdjustmentElementCreditCriteria (nolock) where RateAdjustmentElementKey = @RateAdjustmentElementKey and CreditCriteriaKey=@CreditCriteriaKey)
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
			select @ApplicationMaxEmpScore = [dbo].[fGetMaxEmpiricaScoreForApplication](@OfferKey)		
				
			if (@ApplicationMaxEmpScore is not null)
				begin
					if (@ApplicationMaxEmpScore >= @ElementMinValue and @ApplicationMaxEmpScore <= @ElementMaxValue) 
					begin   
						select @ApplicationMaxEmpScore
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
