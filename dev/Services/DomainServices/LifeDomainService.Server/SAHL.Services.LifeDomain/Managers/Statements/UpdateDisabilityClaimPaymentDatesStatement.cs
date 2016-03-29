using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class UpdateDisabilityClaimPaymentDatesStatement :  ISqlStatement<int>
    {
        public int DisabilityClaimKey { get; protected set; }
        public DateTime? PaymentStartDate { get; protected set; }
        public int? NumberOfInstalmentsAuthorised { get; protected set; }
        public DateTime? PaymentEndDate { get; protected set; }

        public UpdateDisabilityClaimPaymentDatesStatement(int disabilityClaimKey, DateTime? paymentStartDate, int? numberOfInstalmentsAuthorised, DateTime? paymentEndDate)
        {
            DisabilityClaimKey = disabilityClaimKey;
            PaymentStartDate = paymentStartDate;
            NumberOfInstalmentsAuthorised = numberOfInstalmentsAuthorised;
            PaymentEndDate = paymentEndDate;
        }

        public string GetStatement()
        {
            return @"update [2am].[dbo].DisabilityClaim set PaymentStartDate = @PaymentStartDate, NumberOfInstalmentsAuthorised = @NumberOfInstalmentsAuthorised, 
                        PaymentEndDate = @PaymentEndDate where DisabilityClaimKey = @DisabilityClaimKey";            
        }
    }
}