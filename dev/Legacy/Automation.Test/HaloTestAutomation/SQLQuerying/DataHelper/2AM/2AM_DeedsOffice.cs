using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        public string GetDeedsOfficeNameByAttorneyRegisteredName(string registeredName)
        {
            string query =
                string.Format(@"select DeedsOffice.Description
                                from dbo.legalentity
                                inner join dbo.attorney
                                on legalentity.legalentitykey = attorney.legalentitykey
                                inner join  dbo.DeedsOffice
			                    on attorney.DeedsOfficekey = DeedsOffice.DeedsOfficekey
                                where legalentity.registeredname = '{0}'", registeredName);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return results.SQLScalarValue;
        }

        public IEnumerable<Automation.DataModels.DeedsOffice> GetDeedsOffices()
        {
            var deedsOffices = dataContext.Query<Automation.DataModels.DeedsOffice>("select * from [2am].dbo.DeedsOffice");
            return deedsOffices;
        }
    }
}