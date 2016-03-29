﻿using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Queries
{
    public class GetDisabilityClaimByKeyQuery : ServiceQuery<DisabilityClaimDetailModel>, ILifeDomainQuery
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        public GetDisabilityClaimByKeyQuery(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }
    }
}