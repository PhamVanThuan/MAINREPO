using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.LifeDomain.Specs.RuleSpecs.ClientCanOnlyHaveOnePendingDisabilityClaim
{
    public class DisabilityClaimLifeAccountTestModel : IDisabilityClaimLifeAccountModel
    {
        public DisabilityClaimLifeAccountTestModel(int lifeAccountKey)
        {
            this.LifeAccountKey = lifeAccountKey;
        }

        public int LifeAccountKey { get; protected set; }
    }
}