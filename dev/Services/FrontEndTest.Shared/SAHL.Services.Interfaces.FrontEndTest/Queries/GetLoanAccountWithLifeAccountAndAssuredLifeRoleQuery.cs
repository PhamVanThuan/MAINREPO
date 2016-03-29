using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetLoanAccountWithLifeAccountAndAssuredLifeRoleQuery : ServiceQuery<GetLoanAccountWithLifeAccountAndAssuredLifeRoleQueryResult>, 
        IFrontEndTestQuery, ISqlServiceQuery<GetLoanAccountWithLifeAccountAndAssuredLifeRoleQueryResult>
    {
    }
}