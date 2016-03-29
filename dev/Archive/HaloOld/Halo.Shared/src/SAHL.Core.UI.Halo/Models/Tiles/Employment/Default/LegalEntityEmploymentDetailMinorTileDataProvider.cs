using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
using System.Collections.Generic;
namespace SAHL.Core.UI.Halo.Tiles.Employment.Default
{
    public class LegalEntityEmploymentDetailMinorTileDataProvider : AbstractSqlTileDataProvider, ITileDataProvider<LegalEntityEmploymentDetailMinorTileModel>
    {
        public LegalEntityEmploymentDetailMinorTileDataProvider()
            : base()
        {
        }

        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"SELECT 
                                        leEmp.EmploymentKey AS BusinessKey,
                                        0 AS BusinessKeyType
                                        FROM
                                        [2AM].dbo.Employment leEmp
                                    WHERE 
                                        leEmp.EmploymentStatusKey = 1
                                    AND 
                                        leEmp.LegalEntityKey = {0}", businessKey.Key);
        }
    }
}