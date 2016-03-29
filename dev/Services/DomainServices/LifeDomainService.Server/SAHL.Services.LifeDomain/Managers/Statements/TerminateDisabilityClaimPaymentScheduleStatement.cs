using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.LifeDomain.QueryStatements
{
    public class TerminateDisabilityClaimPaymentScheduleStatement : ISqlStatement<string>
    {
        public int DisabilityClaimKey { get; protected set; }
        public string UserName { get; protected set; }

        public TerminateDisabilityClaimPaymentScheduleStatement(int disabilityClaimKey, string adUserName)
        {
            DisabilityClaimKey = disabilityClaimKey;
            UserName = adUserName;
        }

        public string GetStatement()
        {
            return @"   DECLARE @Msg varchar(1024)

                        EXECUTE [process].[halo].[pTerminateLifeDisabilityPayment] 
                           @DisabilityClaimKey
                          ,@UserName
                          ,@Msg OUTPUT

                        select @Msg as Message;";
        }
    }
}
