using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.PersonalLoanAccount.Default
{
    public class PersonalLoanAccountMinorTileDataProvider : AbstractSqlTileDataProvider, ITileDataProvider<PersonalLoanAccountMinorTileModel>
    {
        public PersonalLoanAccountMinorTileDataProvider()
            : base()
        {
        }

        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"select DISTINCT a.accountkey AS BusinessKey, 2 as BusinessKeyType from [2am].[dbo].[role] r
	                                    inner join [2am].[dbo].[account] a on a.accountkey = r.accountkey
                                    where a.accountstatuskey = 1
                                    and a.rrr_productkey = 12
                                    and legalentitykey = {0}", businessKey.Key);
        }
    }
}