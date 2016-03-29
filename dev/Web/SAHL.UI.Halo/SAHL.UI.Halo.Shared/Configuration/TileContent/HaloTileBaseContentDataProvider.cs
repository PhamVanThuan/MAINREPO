using System;
using System.Linq;
using System.Collections.Generic;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloTileBaseContentDataProvider<T> : HaloTileBaseSqlDataProvider,
                                                               IHaloTileContentDataProvider<T> where T : IHaloTileModel
    {
        protected HaloTileBaseContentDataProvider(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public dynamic Load(BusinessContext businessContext)
        {
            if (businessContext.BusinessKey.Key == 0) { return null; }

            var result = this.RetrieveSqlDataRecord<T>(businessContext);
            if (result == null) { return null; }

            result = ((IEnumerable<T>) result).SingleOrDefault();
            return result;
        }
    }
}