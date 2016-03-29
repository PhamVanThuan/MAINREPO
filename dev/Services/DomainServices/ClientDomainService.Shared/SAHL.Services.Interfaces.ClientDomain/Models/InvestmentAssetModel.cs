using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class InvestmentAssetModel : ValidatableModel
    {
        public InvestmentAssetModel(AssetInvestmentType investmentType, string companyName, double assetValue)
        {
            this.InvestmentType = investmentType;
            this.CompanyName = companyName;
            this.AssetValue = assetValue;
            Validate();
        }

        [Required]
        public AssetInvestmentType InvestmentType { get; protected set; }

        [Required]
        public string CompanyName { get; protected set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Asset value must be greater than zero.")]
        public double AssetValue { get; protected set; }
    }
}