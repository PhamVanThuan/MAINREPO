using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.UI.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.UI.Providers.Tiles
{
    public abstract class AbstractSqlTileDataProvider : AbstractSqlStatementDataProvider<TileDataModel>
    {
        public AbstractSqlTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public IEnumerable<BusinessKey> GetTileInstanceKeys(BusinessKey businessKey)
        {
            IEnumerable<TileDataModel> tileData = null;
            tileData = base.GetData(businessKey);

            if (tileData != null)
            {
                return tileData.Select<TileDataModel, BusinessKey>(x => new BusinessKey(x.BusinessKey, (GenericKeyType)x.GenericKeyType));
            }
            else
            {
                return new BusinessKey[] { };
            }
        }
    }
}