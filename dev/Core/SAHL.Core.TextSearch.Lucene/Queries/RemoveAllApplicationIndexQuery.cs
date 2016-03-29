using SAHL.Core.Data;
using SAHL.Core.TextSearch.Lucene.Models;

namespace SAHL.Core.TextSearch.Lucene.Queries
{
    public class RemoveAllApplicationIndexQuery : ISqlStatement<ApplicationIndexModel>
    {
        public string GetStatement()
        {
            return "DELETE FROM [Capitec].[staging].[ApplicationIndex]";
        }
    }
}