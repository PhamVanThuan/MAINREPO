using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Server.Tests.Models;

namespace SAHL.Services.Query.Server.Tests.DataManagers.Statements
{
    public class GetTestStatementWithTop : ISqlStatement<TestDataModel>
    {

        public string GetStatement()
        {
            return @"Select Top 10 T.Id as Id, T.Description as Description From Test T";
        }

    }

}