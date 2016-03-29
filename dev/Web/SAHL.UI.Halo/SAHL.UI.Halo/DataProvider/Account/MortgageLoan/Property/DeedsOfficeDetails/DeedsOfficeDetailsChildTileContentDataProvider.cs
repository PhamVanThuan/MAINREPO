using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan.Property.DeedsOfficeDetails
{
    public class DeedsOfficeDetailsChildTileContentDataProvider : HaloTileBaseContentDataProvider<DeedsOfficeDetailsModel>
    {
        public DeedsOfficeDetailsChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
                            return string.Format(@"select
                                 tt.Description as PropertyTitleType,
                                  dpt.Description as DeedsPropertyType,
                                 p.SectionalUnitNumber,
                                 p.SectionalSchemeName,
                                 p.ErfNumber,
                                 p.ErfPortionNumber,
                                 p.ErfMetroDescription,
                                 p.ErfSuburbDescription
                              from 
                                 [2am].[dbo].[Property] p
                                    join
                                         [2am].[dbo].[TitleType] tt on tt.TitleTypeKey=p.TitleTypeKey
                                    join
                                         [2am].[dbo].[DeedsPropertyType] dpt on dpt.DeedsPropertyTypeKey=p.DeedsPropertyTypeKey
                                    where 
                                    p.PropertyKey = {0} ", businessContext.BusinessKey.Key);
        }
    }
}
