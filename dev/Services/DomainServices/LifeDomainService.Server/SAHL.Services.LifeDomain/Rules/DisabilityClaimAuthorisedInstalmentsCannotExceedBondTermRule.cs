using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Extensions;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.LifeDomain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LifeDomain.Rules
{
    public class DisabilityClaimAuthorisedInstalmentsCannotExceedBondTermRule : IDomainRule<IDisabilityClaimApproveModel>
    {
        private ILifeDomainDataManager lifeDomainDataManager;
        private IDomainQueryServiceClient domainQueryClient;

        public DisabilityClaimAuthorisedInstalmentsCannotExceedBondTermRule(ILifeDomainDataManager lifeDomainDataManager, IDomainQueryServiceClient domainQueryClient)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
            this.domainQueryClient = domainQueryClient;
        }

        public void ExecuteRule(ISystemMessageCollection messages, IDisabilityClaimApproveModel ruleModel)
        {

            // get the mortage loan info using the loan number fom above
            var query = new GetMortgageLoanDetailsQuery(ruleModel.LoanNumber);
            domainQueryClient.PerformQuery(query);

            if (query.Result != null)
            {
                if (query.Result.Results.Any())
                {
                    var mortgageLoanDetails = query.Result.Results.First();
                    int debitOrderDay = ruleModel.PaymentStartDate.Day;

                    // work out the loans final payment date by adding the remaining instalments to today
                    DateTime loanFinalDate = DateTime.Now.AddMonths(mortgageLoanDetails.RemainingInstalments);
                    DateTime loanFinalPaymentDate = new DateTime(loanFinalDate.Year, loanFinalDate.Month, debitOrderDay);

                    if (ruleModel.PaymentEndDate > loanFinalPaymentDate)
                    {
                        messages.AddMessage(new SystemMessage("No. of Authorised Instalments exceeds the Term of the Bond."
                            + "<br/>Disability Payment End Date: " + ruleModel.PaymentEndDate.ToSAHLDateString()
                            + "<br/>Loan Final Payment Date    : " + loanFinalPaymentDate.ToSAHLDateString()
                            , SystemMessageSeverityEnum.Error));
                    }
                }
            }
        }
    }
}