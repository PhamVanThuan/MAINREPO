using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class DoesOfferExistQuery : ServiceQuery<DoesOfferExistQueryResult>, IDomainQueryQuery
    {
        [Required]
        public int OfferKey { get; protected set; }

        public DoesOfferExistQuery(int offerKey)
        {
            this.OfferKey = offerKey;
        }
    }
}