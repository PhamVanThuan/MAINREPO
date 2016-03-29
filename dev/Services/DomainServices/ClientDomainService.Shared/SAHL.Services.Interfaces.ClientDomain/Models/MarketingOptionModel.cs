using System.ComponentModel.DataAnnotations;
using SAHL.Core.Validation;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class MarketingOptionModel : ValidatableModel
    {
        public MarketingOptionModel(int marketingOptionKey, string userID)
        {
            this.MarketingOptionKey = marketingOptionKey;
            this.UserID = userID;
        }

        [Required]
        public int MarketingOptionKey
        { get; protected set; }

        [Required]
        public string UserID
        { get; protected set; }
    }
}