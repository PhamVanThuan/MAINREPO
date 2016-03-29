USE [2AM]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER Procedure [dbo].[GetFees]  @LoanAmount  Decimal(22,10), @BondRequired  Decimal(22,10), @LoanType int, @CashOut  Decimal(22,10), @OverRideCancelFee  Decimal(22,10), @CapitaliseFees bit, @NCACompliant bit, @IsBondExceptionAction bit, @QuickPay bit, @HouseholdIncome  Decimal(22,10), @EmploymentTypeKey int, @LTV  Decimal(22,10), @ApplicationParentAccountKey int, @IsStaffLoan bit, @IsDiscountedInitiationFee bit, @OfferStartDate datetime, @CapitaliseInitiationFee bit = 0, @isGEPF bit

as

DECLARE @LoanAmountExclFees AS  Decimal(22,10)
DECLARE @OfferType AS  Decimal(22,10)
DECLARE @BankRate AS  Decimal(22,10)
DECLARE @InterimInterestWeeks AS  Decimal(22,10)
DECLARE @_BondRequired AS  Decimal(22,10)
DECLARE @_fees AS  Decimal(22,10)
DECLARE @_CalculateInitiationFees AS  Decimal(22,10)
DECLARE @_CalculateInterimInterest AS  Decimal(22,10)
DECLARE @_Amount AS  Decimal(22,10)
DECLARE @Buffer AS  Decimal(22,10)    
DECLARE @_CancelFee AS  Decimal(22,10)
DECLARE @_LoanAmountInclFees AS  Decimal(22,10)
DECLARE @_Bondtoregister AS  Decimal(22,10)
DECLARE @RegFee AS  Decimal(22,10)
DECLARE @QuickPayFee AS  Decimal(22,10)
DECLARE @_QuickPayFee AS  Decimal(22,10)
DECLARE @IsAlphaHousing INT
DECLARE @InitiationFeeDiscount  Decimal(22,10)

-- SETTING THE Alpha Housing Value to False
SET @IsAlphaHousing = 0

SET @InitiationFeeDiscount = Null

SELECT @OfferType = @LoanType  
SELECT @BankRate = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control] (nolock) WHERE ControlDescription LIKE 'Banks Mortgage Rate')
SELECT @InterimInterestWeeks = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Interim Interest Weeks')
SELECT @Buffer = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - FurtherLoanCapacity')
SELECT @QuickPayFee = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - QuickPayFeePercentage')

SET @_QuickPayFee = 0
SET @_CalculateInitiationFees = 0

--First Pass, get fees on Loan Amount
---Calculate Interim Interest

--2,3,4 Offer Types will pass in the AccountKey
--check the offer attributes for alpha if we are unsure
select @IsAlphaHousing = count(1) from Detail (NOLOCK) WHERE AccountKey = @ApplicationParentAccountKey AND DetailTypeKey = 599

-- if alpha housing is still 0 check if the alpha housing attribute needs to be added.
if (@IsAlphaHousing = 0 AND @LoanType <> 4)
	select @IsAlphaHousing = count(1) FROM GetAlphaHousingOfferAttributes(@LTV, @EmploymentTypeKey, @HouseholdIncome, @IsStaffLoan, @isGEPF, 0) WHERE OfferAttributeTypeKey = 26 AND Remove = 0

SET @_CalculateInterimInterest = ( SELECT 
	CASE WHEN @OfferType = 6 THEN (((@LoanAmount - @CashOut )) * (@BankRate * 0.01) / 52 ) * @InterimInterestWeeks  
	ELSE  0 END )

SET @LoanAmount = @LoanAmount + @_CalculateInterimInterest

DECLARE @amount INT
SET @amount = round(@LoanAmount, 0)


SET @_CancelFee = (SELECT TOP 1  CASE WHEN @OfferType = 6  THEN (CASE WHEN @OverRideCancelFee > 0 THEN @OverRideCancelFee 
									ELSE FeeCancelDuty + FeeCancelConveyancing + FeeCancelVAT END )
					ELSE 0 END		
					FROM  FEES (nolock) WHERE FEERange >= @amount ORDER BY FEERange ASC) 

if @NCACompliant = 0
BEGIN
	SET @_CalculateInitiationFees = (select ControlNumeric from [2am].dbo.Control (nolock) where ControlDescription = 'Bond Initiation Fee') --FB2231

	if @IsDiscountedInitiationFee = 1
	begin

		if (isnull(@OfferStartDate, getdate()) > (select ControlText from [2am].dbo.Control (nolock) where ControlDescription = 'Discounted Initiation Fee Date Switch')) 
        begin 
            set @InitiationFeeDiscount = (select ControlNumeric from [2am].dbo.Control (nolock) where ControlDescription = 'Returning Main Applicant Initiation Fee Discount 2') 
        end 
        else 
        begin 
            set @InitiationFeeDiscount = (select ControlNumeric from [2am].dbo.Control (nolock) where ControlDescription = 'Returning Main Applicant Initiation Fee Discount') 
        end


		if (@InitiationFeeDiscount <= 1 and @InitiationFeeDiscount >= 0)
		begin 
			SET @_CalculateInitiationFees = @_CalculateInitiationFees * (1 - @InitiationFeeDiscount)
		end
	end
END

--- QuickPayFee is applied if applicable
IF @QuickPay = 1
BEGIN
	SET @_QuickPayFee =  (@amount * @QuickPayFee)
END


if (@CapitaliseInitiationFee = 1)
begin
	set @amount = @amount + @_CalculateInitiationFees
end
else if (@CapitaliseFees = 1)
begin
	-- calc Registration Fee : the desc in the order by is so that the RegFee param is set from the last item in the recordset 
	SELECT @RegFee =
		CASE
			WHEN @IsAlphaHousing = 1 THEN FeeBondStamps + [FeeBondConveyancingNoFICA] + [FeeBondNoFICAVAT]  
			WHEN @LoanType in (4, 7) THEN FeeBondStamps + [FeeBondConveyancing80Pct] + [FeeBondVAT80Pct]  
			ELSE FeeBondStamps + [FeeBondConveyancing] + [FeeBondVAT] 
		END  
	FROM  Fees (nolock) 
	WHERE FeeRange >= @amount
	ORDER BY FeeRange DESC

	set @amount = @amount + @RegFee + @_CalculateInitiationFees   + @_CancelFee
end

--Calc Bond To Register
DECLARE @lval  Decimal(22,10)
DECLARE @dval  Decimal(22,10)

SET @_Bondtoregister = 0

if (@amount > 0 AND @IsBondExceptionAction = 0)
  BEGIN
	SET @amount = @amount + @Buffer
	SET @lval = round((@amount/1000), 0)
	SET @dval = @lval * 1000
	if @dval > @amount
	 SET @_Bondtoregister = @dval
	ELSE
	 SET @_Bondtoregister = (@lval + 1) * 1000
	 SELECT @_Bondtoregister = (SELECT TOP 1 FeeRange FROM  Fees (nolock) 
	 WHERE FeeRange >= @_Bondtoregister )
  END
ELSE
  BEGIN
	--BondToRegister must be at least the Loan Amount
	IF @BondRequired < @amount
		SET @_Bondtoregister = @amount
	ELSE
		SET @_Bondtoregister = @BondRequired
  END

--if the calculated bondtoreg < what the client wants, reset
if @_Bondtoregister < @BondRequired
	SET @_Bondtoregister = @BondRequired

SET @amount = round(@_Bondtoregister, 0)

SELECT TOP (1)
	@_CalculateInterimInterest AS InterimInterest,
	---Calculate CancellationFee
	@_CancelFee AS CancellationFee,
	---Calculate Initiation Fees
	CASE WHEN @QuickPay = 1 THEN @_QuickPayFee ELSE
	@_CalculateInitiationFees END as InitiationFee,
	@_Bondtoregister AS BondToRegister,
	---Calculate Registration Fee
	CASE
		WHEN @IsAlphaHousing = 1 THEN cast((FeeBondStamps + [FeeBondConveyancingNoFICA] + [FeeBondNoFICAVAT]) as  Decimal(22,10))  
		WHEN @LoanType in (4, 7) THEN cast((FeeBondStamps + [FeeBondConveyancing80Pct] + [FeeBondVAT80Pct]) as  Decimal(22,10)) 
		ELSE cast((FeeBondStamps + [FeeBondConveyancing] + [FeeBondVAT]) as  Decimal(22,10)) 
	END  
	AS RegistrationFee,
	isnull(@InitiationFeeDiscount, 0) AS InitiationFeeDiscount,
	@CapitaliseFees as CapitaliseFees,
	@CapitaliseInitiationFee as CapitaliseInitiationFee
FROM  Fees (nolock) 
WHERE FeeRange >= @amount
ORDER BY FeeRange ASC


