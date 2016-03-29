using SAHL.Core.Data;
using SAHL.Services.Interfaces.LifeDomain.Models;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class CreateDisabilityClaimPaymentScheduleStatement : ISqlStatement<string>
    {
        public int DisabilityClaimKey { get; protected set; }
        public string UserName { get; protected set; }

        public CreateDisabilityClaimPaymentScheduleStatement(int disabilityClaimKey, string adUserName)
        {
            DisabilityClaimKey = disabilityClaimKey;
            UserName = adUserName;
        }

        public string GetStatement()
        {
            return @"   DECLARE @Msg varchar(1024)

                        EXECUTE [process].[halo].[pApproveLifeDisabilityPayment] 
                           @DisabilityClaimKey
                          ,@UserName
                          ,@Msg OUTPUT

                        select @Msg as Message;";
        }
    }
}