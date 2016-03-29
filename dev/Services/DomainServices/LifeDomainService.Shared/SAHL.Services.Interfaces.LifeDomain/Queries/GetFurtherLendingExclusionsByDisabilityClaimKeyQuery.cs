using SAHL.Core.Data;
using SAHL.Core.Messaging.Shared;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.LifeDomain.Queries
{
    public class GetFurtherLendingExclusionsByDisabilityClaimKeyQuery 
        : ServiceQuery<DisabilityClaimFurtherLendingExclusionModel>, 
          ISqlServiceQuery<DisabilityClaimFurtherLendingExclusionModel>, 
          IServiceQuery<IServiceQueryResult<DisabilityClaimFurtherLendingExclusionModel>>, ILifeDomainQuery, IServiceQuery, IServiceCommand, IMessage
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        public GetFurtherLendingExclusionsByDisabilityClaimKeyQuery(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }
    }
}
