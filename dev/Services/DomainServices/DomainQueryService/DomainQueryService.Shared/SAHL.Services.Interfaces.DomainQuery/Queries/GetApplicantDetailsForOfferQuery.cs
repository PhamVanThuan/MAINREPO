using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class GetApplicantDetailsForOfferQuery : ServiceQuery<GetApplicantDetailsForOfferQueryResult>, IDomainQueryQuery, ISqlServiceQuery<GetApplicantDetailsForOfferQueryResult>
    {
        [Required]
        public int OfferKey { get; protected set; }

        public GetApplicantDetailsForOfferQuery(int offerKey)
        {
            this.OfferKey = offerKey;
        }
    }
}