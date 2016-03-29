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
    public class NewBusinessRequestedLoanAmountMustBeAboveMinimumRequiredRule : LoanAmountMustBeAboveMinimumRequiredRule, IDomainRule<NewPurchaseApplicationModel>
    {
        private IApplicationDataManager ApplicationDataManager;

        public NewBusinessRequestedLoanAmountMustBeAboveMinimumRequiredRule(IApplicationDataManager applictionDataManager)
        {
            this.ApplicationDataManager = applictionDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, NewPurchaseApplicationModel ruleModel)
        {
            base.ExecuteRule(messages, ruleModel.LoanAmountNoFees, MortgageLoanPurpose.Newpurchase, ApplicationDataManager);            
        }
    }
}
