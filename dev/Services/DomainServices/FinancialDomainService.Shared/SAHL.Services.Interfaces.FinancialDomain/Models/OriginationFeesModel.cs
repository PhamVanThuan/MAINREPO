using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Models
{
    public class OriginationFeesModel : ValidatableModel
    {
        public OriginationFeesModel(decimal InterimInterest, decimal CancellationFee, decimal InitiationFee
            , decimal BondToRegister, decimal RegistrationFee, decimal InitiationFeeDiscount, bool CapitaliseFees, bool CapitaliseInitiationFee)
        {
            this.InterimInterest = InterimInterest;
            this.CancellationFee = CancellationFee;
            this.InitiationFee = InitiationFee;
            this.BondToRegister = BondToRegister;
            this.RegistrationFee = RegistrationFee;
            this.InitiationFeeDiscount = InitiationFeeDiscount;
            this.CapitaliseFees = CapitaliseFees;
            this.CapitaliseInitiationFee = CapitaliseInitiationFee;
            Validate();
        }
        
        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "InterimInterest must be 0 or greater.")]
        public decimal InterimInterest { get; protected set; }
       
        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "CancellationFee must be 0 or greater.")]
        public decimal CancellationFee { get; protected set; }
        
        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "InitiationFee must be 0 or greater.")]
        public decimal InitiationFee { get; protected set; }
        
        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "BondToRegister must be 0 or greater.")]
        public decimal BondToRegister { get; protected set; }
        
        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "RegistrationFee must be 0 or greater.")]
        public decimal RegistrationFee { get; protected set; }
        
        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "InitiationFeeDiscount must be 0 or greater.")]
        public decimal InitiationFeeDiscount { get; protected set; }

        [Required]
        public bool CapitaliseFees { get; protected set; }

        [Required]
        public bool CapitaliseInitiationFee { get; protected set; }

        public decimal TotalFees()
        {
            return InitiationFee + RegistrationFee + CancellationFee;
        }
    }
}
