using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Application;

namespace SAHL.UI.Halo.DataProvider.Application
{
    public class ApplicationDynamicDataProvider : HaloTileBaseDynamicDataProvider,
                                                  IHaloTileDynamicDataProvider<ApplicationRootTileConfiguration>
    {
        public ApplicationDynamicDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.Offer;
            return string.Format(@"select 
                   o.OfferTypeKey as GenericKey, 
                   {1} as GenericKeyTypeKey, 
                   ot.Description as SubType 
            from 
                   [2am].[dbo].Offer o 
            join 
                   [2am].[dbo].OfferType ot on ot.OfferTypeKey = o.OfferTypeKey
            where 
                   o.OfferKey={0}", businessContext.BusinessKey.Key, genericKeyType);
        }
    }
}
