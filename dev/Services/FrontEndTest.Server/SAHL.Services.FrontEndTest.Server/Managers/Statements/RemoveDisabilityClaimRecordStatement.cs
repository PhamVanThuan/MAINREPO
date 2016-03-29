using SAHL.Core.Attributes;
using SAHL.Core.Data;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    [InsertConventionExclude]
    public class RemoveDisabilityClaimRecordStatement : ISqlStatement<int>
    {
        public int DisabilityClaimKey { get; protected set; }

        public RemoveDisabilityClaimRecordStatement(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }

        public string GetStatement()
        {
            return @"delete from [2AM].dbo.DisabilityClaim where DisabilityClaimKey = @DisabilityClaimKey";
        }
    }
}