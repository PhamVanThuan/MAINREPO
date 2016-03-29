using SAHL.Core.Data;
using SAHL.Services.Interfaces.LifeDomain.Models;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class GetDisabilityClaimFurtherLendingExclusionsStatement : ISqlStatement<DisabilityClaimFurtherLendingExclusionModel>
    {
        public int AccountKey { get; protected set; }

        public GetDisabilityClaimFurtherLendingExclusionsStatement(int accountKey)
        {
            AccountKey = accountKey;
        }

        public string GetStatement()
        {
            return @"SELECT AccountKey, Description, Date, Amount FROM Process.[life].[fGetDisabilityPaymentFurtherLendingExceptions] (@AccountKey)";
        }
    }
}