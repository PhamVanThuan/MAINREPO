﻿using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Queries
{
    public class GetDisabilityClaimHistoryForAccountQuery : ServiceQuery<DisabilityClaimDetailModel>, ILifeDomainQuery
    {
        [Required]
        public int AccountKey { get; protected set; }

        public GetDisabilityClaimHistoryForAccountQuery(int accountKey)
        {
            this.AccountKey = accountKey;
        }
    }
}