using SAHL.Services.Capitec.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Service.Test
{
    class CapitecApplicationStubs
    {
        public static NewPurchaseApplication CreateDummyNewPurchaseApp()
        {
            return new NewPurchaseApplication(1, 1, DateTime.Now, new NewPurchaseLoanDetails(1, 1, 1, 1, true, 1), new List<Applicant>(), 1, new ConsultantDetails("Dummy", "Dummy"), new List<string>());
        }

        public static SwitchLoanApplication CreateDummySwitchLoanApp()
        {
            return new SwitchLoanApplication(1, 1, DateTime.Now, new SwitchLoanDetails(1, 1, 1, 1, 1, 1, true, 1), new List<Applicant>(), 1, new ConsultantDetails("Dummy", "Dummy"), new List<string>());
        } 
    }
}
