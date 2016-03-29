using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Server.Tests.Models;

namespace SAHL.Services.Query.Server.Tests.DataManagers.Statements
{
    public class GetTestStatementWithTopAndInnerSelectWithTop : ISqlStatement<TestDataModel>
    {

        public string GetStatement()
        {
            return @"Select Top 10 T.Id as Id, T.Description as Description From Test T Inner Join (Select Top 5 Id From TestItem TI) Links On Links.Id = T.Id";
        }

    }

}