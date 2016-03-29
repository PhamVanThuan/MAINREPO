using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations.PreviousValuationDetails;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan.Property.PreviousValuations.PreviousValuationDetails
{
    public class PreviousValuationDetailsChildDataProvider : HaloTileBaseChildDataProvider,
                                                     IHaloTileChildDataProvider<PreviousValuationDetailsChildTileConfiguration>
    {
        public PreviousValuationDetailsChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.Valuation;
            return string.Format(@"SELECT
                                        v.ValuationKey as BusinessKey,
                                        {1} as BusinessKeyType
                                    FROM
                                        [2am].[dbo].[Valuation] v
                                     WHERE
                                        v.PropertyKey = {0}
                                        and v.isActive = 0
                                    ORDER BY v.ValuationDate desc", businessContext.BusinessKey.Key, genericKeyType);
        }
    }
}