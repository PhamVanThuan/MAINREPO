using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.DomainServiceChecks.Managers.LifeDataManager.Statements
{
    public class GetDisabilityClaimByKeyStatement : ISqlStatement<DisabilityClaimDataModel>
    {
        public int DisabilityClaimKey { get; protected set; }

        public GetDisabilityClaimByKeyStatement(int DisabilityClaimKey)
        {
            this.DisabilityClaimKey = DisabilityClaimKey;
        }

        public string GetStatement()
        {
            var query = "Select * from [2AM].dbo.DisabilityClaim where DisabilityClaimKey = @DisabilityClaimKey";
            return query;
        }
    }
}