USE [2AM]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER FUNCTION [dbo].[GetOfferAttributes]
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
	[Remove] bit
)
AS
BEGIN
	-- Alpha Housing
	insert into @offerAttributesToApply
	select 
		OfferAttributeTypeKey,
		OfferAttributeTypeGroupKey,
		[Remove]
	from
		[dbo].[GetAlphaHousingOfferAttributes](@ltv, @employmentTypeKey, @houseHoldIncome, @isStaffLoan, @isGEPF, @OfferKey)

	-- Discounted Initiation Fee
	insert into @offerAttributesToApply
	select 
		OfferAttributeTypeKey,
		OfferAttributeTypeGroupKey,
		[Remove]
	from
		[dbo].[GetDiscountedInitiationFeeOfferAttribute](@OfferKey)
	return
END;
GO


