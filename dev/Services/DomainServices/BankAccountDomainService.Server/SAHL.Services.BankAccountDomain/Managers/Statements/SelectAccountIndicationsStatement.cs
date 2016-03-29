using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.BankAccountDomain.Managers.Statements
{
    public class SelectAccountIndicationsStatement : ISqlStatement<AccountIndicationDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT [AccountIndicationKey],[AccountIndicator],[AccountIndicationTypeKey],[Indicator],[UserID],[DateChange]
                            FROM [2AM].[dbo].[AccountIndication]";
        }
    }
}