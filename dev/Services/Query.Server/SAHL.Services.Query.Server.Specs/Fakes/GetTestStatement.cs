using SAHL.Core.Data;

namespace SAHL.Services.Query.Server.Specs.Fakes
{
    public class GetTestStatement : ISqlStatement<TestDataModel>
    {
        public string GetStatement()
        {
            return "Select T.Id, T.Description From Test T";
        }
    }
}