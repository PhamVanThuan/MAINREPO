using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class CalculateApplicationScenarioNewPurchaseQuery : ServiceQuery<CalculateApplicationScenarioQueryResult>
    {
		public CalculateApplicationScenarioNewPurchaseQuery(NewPurchaseLoanDetails loanDetails)
        {
            this.LoanDetails = loanDetails;
        }

        public NewPurchaseLoanDetails LoanDetails { get; set; }

        public new CalculateApplicationScenarioQueryResult Result { get; set; }
    }
}
