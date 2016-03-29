using System.Collections.Generic;
using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.BankAccount.Default
{
    public class LegalEntityBankAccountMinorTileDataProvider : ITileDataProvider<LegalEntityBankAccountMinorTileModel>
    {
        public LegalEntityBankAccountMinorTileDataProvider()
            : base()
        {
        }

        public IEnumerable<BusinessKey> GetTileInstanceKeys(BusinessKey businessKey)
        {
            return new BusinessKey[] { businessKey };
        }
    }
}