using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth.Statements
{
    public class GetAttorneyInvoicesNotProcessedThisMonthStatement : ISqlStatement<AttorneyInvoicesNotProcessedThisMonthDataModel>
    {

        public GetAttorneyInvoicesNotProcessedThisMonthStatement()
        {
        }

        public string GetStatement()
        {
            return @"SELECT [Count]
                        ,[Value]
                    FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisMonth]";
        }
    }
}