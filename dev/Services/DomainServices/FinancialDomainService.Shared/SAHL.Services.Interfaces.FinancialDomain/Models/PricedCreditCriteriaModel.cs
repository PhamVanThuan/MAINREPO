using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Models
{
    public class PricedCreditCriteriaModel : ValidatableModel
    {
        public PricedCreditCriteriaModel(int CreditCriteriaKey, int CreditMatrixKey, int CategoryKey)
        {
            this.CreditCriteriaKey = CreditCriteriaKey;
            this.CreditMatrixKey = CreditMatrixKey;
            this.CategoryKey = CategoryKey;
            Validate();
        }

        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "CreditCriteriaKey must be provided.")]
        public int CreditCriteriaKey { get; protected set; }

        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "CreditMatrixKey must be provided.")]
        public int CreditMatrixKey { get; protected set; }

        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "CategoryKey must be provided.")]
        public int CategoryKey { get; protected set; }
    }
}
