using SAHL.Core.Data;
using SAHL.Services.Query.Models.Account;

namespace SAHL.Services.Query.DataManagers.Statements.Account
{
    public class GetAccountStatement : ISqlStatement<AccountDataModel>
    {
        public string GetStatement()
        {
            return @"Select Top 1000
                        AccountKey as Id,
	                    AccountStatusKey,
	                    ChangeDate,
	                    CloseDate,
	                    FixedPayment,
	                    OpenDate,
                        ParentAccountKey,
                        SPVKey
                      From [2am].[dbo].[Account]";
        }
    }
}