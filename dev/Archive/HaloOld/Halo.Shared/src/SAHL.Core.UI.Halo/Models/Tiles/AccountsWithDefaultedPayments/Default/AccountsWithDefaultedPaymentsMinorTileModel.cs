using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Tiles.AccountsWithDefaultedPayments.Default
{
    public class AccountsWithDefaultedPaymentsMinorTileModel : IDrillPreviewTileModel
    {
        public int BusinessKeysCount { get; set; }
        public string Title { get; set; }
        public string BusinessKeys { get; set; }
        public BusinessKeyType BusinessKeysType { get; set; }
    }
}