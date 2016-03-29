USE [2AM]
GO
IF  EXISTS (SELECT * FROM sys.objects  WHERE object_id = OBJECT_ID(N'[test].[getFeesForApplicationCreate]') 
            AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [test].[getFeesForApplicationCreate]
END
GO   
/****** Object:  UserDefinedFunction [test].[getTotalInstalmentForMortgageLoanAccount]    Script Date: 02/06/2012 11:36:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE   FUNCTION [test].[getFeesForApplicationCreate](
	@LoanAmount float,
	@BondRequired float,
	@LoanType int,
	@CashOut float,
	@OverRideCancelFee int,
	@capitaliseFees int,
	@NCACompliant int,
	@IsBondExceptionAction int,
	@QuickPay int
)

RETURNS @OfferFees table(InterimInterest float, CancelFee float, CalculateInitiationFees float, 
BONDtoregister float, RegistrationFee float, LoanExFees float, MinFeeRange float)
AS

begin

DECLARE @LoanAmountExclFees AS FLOAT
DECLARE @OfferType AS FLOAT
DECLARE @BankRate AS FLOAT
DECLARE @InterimInterestWeeks AS FLOAT
DECLARE @MinAmountForFees AS FLOAT
DECLARE @FeeBase AS FLOAT
DECLARE @FeePCT AS FLOAT
DECLARE @MaxFees AS FLOAT
DECLARE @MaxFeePCT AS FLOAT
DECLARE @_BondRequired AS FLOAT
DECLARE @_fees AS FLOAT
DECLARE @_CalculateInitiationFees AS FLOAT
DECLARE @_CalculateInterimInterest AS FLOAT
DECLARE @_Amount AS FLOAT
DECLARE @Buffer AS FLOAT    
DECLARE @_CancelFee AS FLOAT
DECLARE @_LoanAmountInclFees AS FLOAT
DECLARE @_Bondtoregister AS FLOAT
DECLARE @RegFee AS FLOAT
DECLARE @QuickPayFee AS FLOAT
DECLARE @_QuickPayFee AS FLOAT

SELECT @OfferType = @LoanType  
SELECT @BankRate = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control] (nolock) WHERE ControlDescription LIKE 'Banks Mortgage Rate')
SELECT @InterimInterestWeeks = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Interim Interest Weeks')
SELECT @MinAmountForFees = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - MinAmountForFees')
SELECT @FeeBase = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control] (nolock)  WHERE ControlDescription LIKE 'Calc - FeeBase')
SELECT @FeePCT = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - FeePercentage')
SELECT @MaxFees = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - MaxFees')
SELECT @MaxFeePCT = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - MaxFeePercentage')
SELECT @Buffer = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - FurtherLoanCapacity')
SELECT @QuickPayFee = (SELECT [ControlNumeric] FROM [2AM].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - QuickPayFeePercentage')

SET @_QuickPayFee = 0
SET @_CalculateInitiationFees = 0

--First Pass, get fees on Loan Amount

---Calculate Interim Interest
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
	if @amount  > @MinAmountForFees
	BEGIN
		SET @_CalculateInitiationFees = (@FeeBase + (@amount * @FeePCT)) 
	END
	ELSE
	BEGIN
		SET @_CalculateInitiationFees = @MaxFeePCT *  @amount
	END

	if @_CalculateInitiationFees > @MaxFees
		SET @_CalculateInitiationFees = @MaxFees

	if @_CalculateInitiationFees > (@amount * @MaxFeePCT)
		SET @_CalculateInitiationFees = (@amount * @MaxFeePCT)
END

--- QuickPayFee is applied if applicable
IF @QuickPay = 1
BEGIN
	SET @_QuickPayFee =  (@amount * @QuickPayFee)
END


IF @LoanType in (4, 7) 
BEGIN
	SET @RegFee  = (SELECT top 1 (FeeBondStamps + [FeeBondConveyancing80Pct] + [FeeBondVAT80Pct])
	FROM FEES (nolock) WHERE FeeRange > =  @amount ORDER BY feerange ASC)
END
ELSE
BEGIN
	SET @RegFee  = (SELECT top 1 (FeeBondStamps + [FeeBondConveyancing] + [FeeBondVAT])
	FROM FEES (nolock) WHERE FeeRange > =  @amount ORDER BY feerange ASC)
END


if @CapitaliseFees = 1
	set @amount = @amount + @RegFee + @_CalculateInitiationFees   + @_CancelFee

--Calc Bond To Register
DECLARE @lval FLOAT
DECLARE @dval FLOAT

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

INSERT INTO @OfferFees
SELECT     TOP (1)
	@_CalculateInterimInterest AS InterimInterest,
	---Calculate CancellationFee
	@_CancelFee AS CancelFee,
	---Calculate Initiation Fees
	CASE WHEN @QuickPay = 1 THEN @_QuickPayFee ELSE
	@_CalculateInitiationFees END as CalculateInitiationFees,
	@_Bondtoregister AS BONDtoregister,
	---Calculate Registration Fee
	CASE WHEN @LoanType in (4, 7) THEN FeeBondStamps + [FeeBondConveyancing80Pct] + [FeeBondVAT80Pct]  ELSE
	FeeBondStamps + [FeeBondConveyancing] + [FeeBondVAT] 
	END 
	AS RegistrationFee,
	@LoanAmount as LoanExFees_ ,
	(@LoanAmount * (@FeePCT + 1)) + ( @Feebase +  @Buffer) as MinFeeRange		
FROM  Fees (nolock) 
WHERE FeeRange >= @amount
ORDER BY FeeRange ASC

	RETURN

end



