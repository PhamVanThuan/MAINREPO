using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Wizards
{
    public class MortgageLoanCloseWizardConfiguration : HaloWizardBaseTileConfiguration
    {
        public MortgageLoanCloseWizardConfiguration() 
            : base("Close Mortgage Loan", WizardType.Sequential)
        {
        }
    }
}
