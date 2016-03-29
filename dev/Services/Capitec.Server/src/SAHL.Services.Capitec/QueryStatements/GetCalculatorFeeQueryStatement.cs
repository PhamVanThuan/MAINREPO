using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    [NolockConventionExclude]
    public class GetCalculatorFeeQueryStatement : IServiceQuerySqlStatement<GetCalculatorFeeQuery, GetCalculatorFeeQueryResult>
    {
        public string GetStatement()
        {
            return @"
DECLARE @CancellationFee DECIMAL(22,10)
      , @InitiationFee   DECIMAL(22,10)
      , @Buffer			 DECIMAL(22,10)
      , @lval			 DECIMAL(22,10)
      , @dval			 DECIMAL(22,10)
      , @_Bondtoregister DECIMAL(22,10)
      , @CalculatedLoanAmount DECIMAL(22,10)
      , @BankRate DECIMAL(22,10)
      , @WeeksForInterimInterest int
      , @_CalculateInterimInterest DECIMAL(22,10)
      , @RegistrationFee DECIMAL(22,10)
      , @CapitaliseFees BIT
      , @BondRequired DECIMAL(22,10)
      , @CalculatedCashOut DECIMAL(22,10)
      , @AddInterimInterest INT

--new params
SET @BondRequired = CONVERT(DECIMAL(22,10), @LoanAmount)
SET @CalculatedLoanAmount = CONVERT(DECIMAL(22,10), @LoanAmount)
SET @CalculatedCashOut = CONVERT(DECIMAL(22,10), @CashOut)

SET @CapitaliseFees = 0
IF (@OfferType = 6)
    BEGIN
        SET @CapitaliseFees = 1
    END

SET @BankRate = (SELECT [ControlNumeric] FROM [Capitec].[dbo].[Control] (nolock) WHERE ControlDescription LIKE 'Calc - Bank Mortgage Rate')
SET @WeeksForInterimInterest = (SELECT [ControlNumeric] FROM [Capitec].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - WeeksForInterimInterest')
SET @AddInterimInterest = (SELECT [ControlNumeric] FROM [Capitec].dbo.[Control] WHERE ControlDescription LIKE 'Calc - AddInterimInterest')

IF (@AddInterimInterest = 1)
BEGIN
        SET @_CalculateInterimInterest = ( SELECT
            CASE WHEN @OfferType = 6 THEN (((@CalculatedLoanAmount - @CalculatedCashOut )) * (@BankRate * 0.01) / 52 ) * @WeeksForInterimInterest
    ELSE  0 END )
    SET @CalculatedLoanAmount = @CalculatedLoanAmount + @_CalculateInterimInterest
end

SET @InitiationFee = (SELECT CONVERT(DECIMAL(22,10),(SELECT [ControlNumeric] FROM [Capitec].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - Initiation Fee Incl. VAT')))
SET @Buffer = CONVERT(DECIMAL(22,10),(SELECT [ControlNumeric] FROM [Capitec].[dbo].[Control]  (nolock) WHERE ControlDescription LIKE 'Calc - FurtherLoanCapacity'))

SET @CancellationFee = CONVERT(DECIMAL(22,10),(SELECT TOP 1  CASE WHEN @OfferType = 6
                                            THEN FeeCancelDuty + FeeCancelConveyancing + FeeCancelVAT
                                            ELSE 0
                                       END
                        FROM  Capitec.dbo.CapitecFees (nolock) WHERE FEERange >= @CalculatedLoanAmount ORDER BY FEERange ASC))

--WE NEED THE REG FEE
SELECT @RegistrationFee =
    CASE
        WHEN @OfferType in (4, 7) THEN FeeBondStamps + [FeeBondConveyancing80Pct] + [FeeBondVAT80Pct]
        ELSE FeeBondStamps + [FeeBondConveyancing] + [FeeBondVAT]
    END
--	AS RegistrationFee
FROM  Capitec.dbo.CapitecFees (nolock)
WHERE FeeRange >= @CalculatedLoanAmount
ORDER BY FeeRange DESC

if @CapitaliseFees = 1
    set @CalculatedLoanAmount = @CalculatedLoanAmount + @RegistrationFee + @InitiationFee  + @CancellationFee

SET @_Bondtoregister = 0

if (@CalculatedLoanAmount > 0)
  BEGIN
    SET @CalculatedLoanAmount = @CalculatedLoanAmount + @Buffer
    SET @lval = round((@CalculatedLoanAmount/1000), 0)
    SET @dval = @lval * 1000
    if @dval > @CalculatedLoanAmount
     SET @_Bondtoregister = @dval
    ELSE
     SET @_Bondtoregister = (@lval + 1) * 1000
     SELECT @_Bondtoregister = (SELECT TOP 1 FeeRange FROM  Capitec.dbo.CapitecFees (nolock)
     WHERE FeeRange >= @_Bondtoregister )
  END
ELSE
  BEGIN
    IF @BondRequired < @CalculatedLoanAmount
        SET @_Bondtoregister = @CalculatedLoanAmount
    ELSE
        SET @_Bondtoregister = @BondRequired
  END

if @_Bondtoregister < @BondRequired
    SET @_Bondtoregister = @BondRequired

SET @CalculatedLoanAmount = round(@_Bondtoregister, 0)

SELECT TOP (1)
    ROUND(@_CalculateInterimInterest,2) AS InterimInterest,
    @CancellationFee AS CancellationFee,
    @InitiationFee as InitiationFee,
    @_Bondtoregister AS BondToRegister,
    CASE
        WHEN @OfferType =7 THEN FeeBondStamps + [FeeBondConveyancing80Pct] + [FeeBondVAT80Pct]
        ELSE FeeBondStamps + [FeeBondConveyancing] + [FeeBondVAT]
    END
    AS RegistrationFee
FROM  Capitec.dbo.CapitecFees  (nolock)
WHERE FeeRange >= @CalculatedLoanAmount
ORDER BY FeeRange ASC";
        }
    }
}