using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration.TileContent
{
    public class HaloTileBaseContentMultipleDataProvider<T> : HaloTileBaseSqlDataProvider,
                                                              IHaloTileContentMultipleDataProvider<T> where T : IHaloTileModel
    {
        protected HaloTileBaseContentMultipleDataProvider(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<dynamic> Load(BusinessContext businessContext)
        {
            return businessContext.BusinessKey.Key == 0 
                        ? null 
                        : this.RetrieveSqlDataRecords<T>(businessContext);
        }
    }
}
