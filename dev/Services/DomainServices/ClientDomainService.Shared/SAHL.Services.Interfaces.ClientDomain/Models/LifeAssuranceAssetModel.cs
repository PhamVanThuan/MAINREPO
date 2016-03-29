using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class LifeAssuranceAssetModel : ValidatableModel
    {
        public LifeAssuranceAssetModel(string companyName, double surrenderValue)
        {
            this.CompanyName = companyName;
            this.SurrenderValue = surrenderValue;
            Validate();
        }

        [Required]
        public string CompanyName { get; protected set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Surrender Value must be greater than zero.")]
        public double SurrenderValue { get; protected set; }
    }
}