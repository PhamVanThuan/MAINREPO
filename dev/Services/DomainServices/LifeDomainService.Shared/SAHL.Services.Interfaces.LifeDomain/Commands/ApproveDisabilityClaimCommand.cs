using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Commands
{
    public class ApproveDisabilityClaimCommand : ServiceCommand, ILifeDomainCommand, IDisabilityClaimApproveModel
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        [Required]
        public int LoanNumber { get; protected set; }

        [Required]
        public DateTime PaymentStartDate { get; protected set; }

        [Required]
        public DateTime PaymentEndDate { get; protected set; }

        [Required]
        public int NumberOfInstalmentsAuthorised { get; protected set; }

        public ApproveDisabilityClaimCommand(int disabilityClaimKey, int loanNumber, DateTime paymentStartDate, int numberOfInstalmentsAuthorised, DateTime paymentEndDate)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
            this.LoanNumber = loanNumber;
            this.PaymentStartDate = paymentStartDate;
            this.NumberOfInstalmentsAuthorised = numberOfInstalmentsAuthorised;
            this.PaymentEndDate = paymentEndDate;
        }
    }
}