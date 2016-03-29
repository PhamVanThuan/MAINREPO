using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using System;

namespace SAHL.Services.Capitec.Managers.DecisionTreeResult.Statements
{
    [NolockConventionExclude]
    public class GetCreditPricingResultForApplicationQuery : ISqlStatement<CreditPricingTreeResultDataModel>
    {
        public Guid ApplicationID { get; protected set; }

        public GetCreditPricingResultForApplicationQuery(Guid applicationID)
        {
            this.ApplicationID = applicationID;
        }

        public string GetStatement()
        {
            return String.Format("SELECT * FROM [Capitec].dbo.CreditPricingTreeResult (NOLOCK) WHERE ApplicationID = @ApplicationID");
        }
    }
}