using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.MortgageLoanAccount.Default
{
    public class MortgageLoanAccountMinorTileDataProvider : AbstractSqlTileDataProvider, ITileDataProvider<MortgageLoanAccountMinorTileModel>
    {
        public MortgageLoanAccountMinorTileDataProvider()
            : base()
        {
        }

        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"select DISTINCT a.accountkey AS BusinessKey, 2 as BusinessKeyType from [2am].[dbo].[role] r
	                                inner join [2am].[dbo].[account] a on a.accountkey = r.accountkey
                                    where a.accountstatuskey = 1
                                    and a.rrr_productkey in (1,2,5,9,11)
                                    and a.ParentAccountKey is null
                                    and legalentitykey = {0}", businessKey.Key);
        }
    }
}