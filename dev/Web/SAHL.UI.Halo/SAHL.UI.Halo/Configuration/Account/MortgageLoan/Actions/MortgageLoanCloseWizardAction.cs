using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Wizards;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Actions
{
    public class MortgageLoanCloseWizardAction : HaloTileWizardActionBase<MortgageLoanRootTileConfiguration, MortgageLoanCloseWizardConfiguration>
    {
        public MortgageLoanCloseWizardAction(string contextData = null)
            : base("Close Mortgage Loan", "icon-plus-2", "Mortgage Loan Actions", 1, contextData)
        {
        }
    }
}
