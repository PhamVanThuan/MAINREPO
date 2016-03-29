using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetDisabilityPaymentsQuery : ServiceQuery<DisabilityPaymentDataModel>, IFrontEndTestQuery, ISqlServiceQuery<DisabilityPaymentDataModel>
    {
        public GetDisabilityPaymentsQuery(int DisabilityClaimKey)
        {
            this.DisabilityClaimKey = DisabilityClaimKey;
        }

        [Required]
        public int DisabilityClaimKey { get; protected set; }
    }
}