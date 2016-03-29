using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class GetOfferBranchConsultantUsernameQuery : ServiceQuery<GetOfferBranchConsultantUsernameQueryResult>, IDomainQueryQuery, ISqlServiceQuery<GetOfferBranchConsultantUsernameQueryResult>
    {
        [Required]
        public int OfferKey { get; protected set; }

        public GetOfferBranchConsultantUsernameQuery(int offerKey)
        {
            this.OfferKey = offerKey;
        }
    }
}