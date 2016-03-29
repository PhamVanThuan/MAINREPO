using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class LoanAmountMustBeAboveMinimumRequiredRule
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, decimal loanAmount, 
                                MortgageLoanPurpose mortgageLoanPurpose, IApplicationDataManager applicationDataManager)
        {
            var minLoanAmountRequired = applicationDataManager.GetMinimumLoanAmountForMortgageLoanPurpose(mortgageLoanPurpose);
            if (loanAmount < minLoanAmountRequired)
            {
                messages.AddMessage(new SystemMessage
                        (String.Format(@"The loan amount of {0} is below the minimum required ({1})"
                                                              , loanAmount.ToString("R #,###,##0.00")
                , minLoanAmountRequired.ToString("R #,###,##0.00")), SystemMessageSeverityEnum.Error));
            }
        }
    }
}
