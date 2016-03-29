using SAHL.UI.Halo.Models.Common.BankAccount;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.BankAccount
{
    public class BankAccountChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ThirdPartyRootTileConfiguration>,
                                                            IHaloTileModel<BankAccountChildModel>
    {
        public BankAccountChildTileConfiguration()
            : base("Third Party Bank Account", "ThirdPartyBankAccount", 1, noOfRows: 2, noOfColumns: 3)
        {
        }
    }
}