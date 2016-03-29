USE [2AM]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER FUNCTION [dbo].[GetAlphaHousingOfferAttributes]
(	
	@ltv float,
	@employmentTypeKey int,
	@houseHoldIncome float,
	@isStaffLoan bit,
	@isGEPF bit,
	@OfferKey int
)
RETURNS
	@offerAttributesToApply
TABLE (
	OfferAttributeTypeKey int,
	OfferAttributeTypeGroupKey int,
	Remove bit
)
AS
begin
	if exists(select 1 from [2am].dbo.Offer (nolock) where offerKey = @OfferKey and offerTypeKey in (2, 3, 4))
	begin
		insert into @offerAttributesToApply
		select 26 as OfferAttributeTypeKey, 1 as OfferAttributeTypeGroupKey, 1
	end
	else
	begin
		if (@isStaffLoan=0 and @isGEPF=0)
		begin
				-- Alpha Housing attribute should be added as this will be funded using alpha funding	
				if exists (select 1 from [2am].dbo.CreditCriteria (nolock) cc
						   join [2am].dbo.CreditCriteriaAttribute cca on cc.CreditCriteriaKey=cca.CreditCriteriaKey	and cca.CreditCriteriaAttributeTypeKey=2 -- Credit Criteria marked as Alpha funded categories for further lending are Alpha Categories
						   where 	CreditMatrixKey=(select max(creditMatrixKey) from [2am].dbo.CreditMatrix where NewBusinessIndicator = 'Y')
								    and @ltv <= cc.LTV 
								    and (@houseHoldIncome >= cc.MinIncomeAmount and @houseHoldIncome < cc.MaxIncomeAmount)
						  )
				begin
					insert into @offerAttributesToApply
						select 26,1,0
				end
		end
		if not exists (select 1 from @offerAttributesToApply) 
		begin
			--remove the attribute if no insert is done
			insert into @offerAttributesToApply
				select 26,1,1
		end
	end
	return
end;
GO


