using SAHL.UI.Halo.Models.Common.BankAccount;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Detail.BankAccount
{
    public class BankAccountChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ClientDetailRootTileConfiguration>,
                                                            IHaloTileModel<BankAccountChildModel>
    {
        public BankAccountChildTileConfiguration()
            : base("Bank Account", "ClientBankAccount", 2, noOfRows: 2, noOfColumns: 3)
        {
        }
    }
}