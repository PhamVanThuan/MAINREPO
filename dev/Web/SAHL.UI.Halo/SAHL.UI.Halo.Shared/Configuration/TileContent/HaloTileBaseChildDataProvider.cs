using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloTileBaseChildDataProvider : HaloTileBaseSqlDataProvider, IHaloTileChildDataProvider
    {
        protected HaloTileBaseChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public IEnumerable<BusinessContext> LoadSubKeys(BusinessContext businessContext)
        {
            List<HaloTileBusinessModel> tileKeyData = null;

            if (string.IsNullOrWhiteSpace(this.GetSqlStatement(businessContext)))
            {
                return new List<BusinessContext> { businessContext };
            }

            tileKeyData = this.RetrieveSqlDataRecord<HaloTileBusinessModel>(businessContext);
            return tileKeyData == null
                        ? new List<BusinessContext>()
                        : tileKeyData.Select<HaloTileBusinessModel, BusinessContext>(x => new BusinessContext(x.Context, (GenericKeyType)x.BusinessKeyType, x.BusinessKey));
        }
    }
}
