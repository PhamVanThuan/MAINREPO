using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class AccountQuickCash : Account, IAccountQuickCash
    {
        public override SAHL.Common.Globals.AccountTypes AccountType
        {
            get { return SAHL.Common.Globals.AccountTypes.MortgageLoan; }
        }
    }
}