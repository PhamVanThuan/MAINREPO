using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloTileBaseDynamicDataProvider : HaloTileBaseSqlDataProvider, IHaloTileDynamicDataProvider
    {
        protected HaloTileBaseDynamicDataProvider(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public HaloDynamicTileDataModel LoadDynamicData(BusinessContext businessContext)
        {
            if (string.IsNullOrWhiteSpace(this.GetSqlStatement(businessContext))) { return null; }

            var result = this.RetrieveSqlDataRecord<HaloDynamicTileDataModel>(businessContext);
            return ((IEnumerable<HaloDynamicTileDataModel>) result).FirstOrDefault();
        }
    }
}
