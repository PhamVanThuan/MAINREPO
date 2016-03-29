using SAHL.Core.Attributes;
using SAHL.Core.Data;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    [InsertConventionExclude]
    public class RemoveDisabilityPaymentRecordStatement : ISqlStatement<int>
    {
        public int DisabilityClaimKey { get; protected set; }

        public RemoveDisabilityPaymentRecordStatement(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }

        public string GetStatement()
        {
            return @"delete from [2AM].dbo.DisabilityPayment where DisabilityClaimKey=@DisabilityClaimKey";
        }
    }
}