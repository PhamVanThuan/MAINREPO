using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetDisabilityPaymentsQueryStatement : IServiceQuerySqlStatement<GetDisabilityPaymentsQuery, DisabilityPaymentDataModel>
    {
        public string GetStatement()
        {
            return @"select * from [2AM].dbo.DisabilityPayment where DisabilityClaimKey=@DisabilityClaimKey";
        }
    }
}