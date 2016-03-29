using SAHL.Core.Data;
using SAHL.Core.UI.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.UI.Providers.Tiles
{
    public abstract class AbstractSqlTileContentProvider<T> : AbstractSqlStatementDataProvider<T>, ITileContentProvider<T>
        where T : ITileModel
    {
        public AbstractSqlTileContentProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public dynamic GetContent(BusinessModel.BusinessKey businessKey)
        {
            var result = base.GetData(businessKey);
            if (result != null)
            {
                result = ((IEnumerable<T>)result).SingleOrDefault();
            }

            return result;
        }
    }
}