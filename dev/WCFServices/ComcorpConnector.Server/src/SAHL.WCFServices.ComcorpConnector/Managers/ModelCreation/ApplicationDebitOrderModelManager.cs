using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class ApplicationDebitOrderModelManager : IApplicationDebitOrderModelManager
    {
        public ApplicationDebitOrderModel PopulateApplicationDebitOrder(List<ApplicantModel> applicants)
        {
            ApplicationDebitOrderModel applicationDebitOrderModel = null;
            int debitOrderDay = 1;
            ApplicantModel applicantWithDebitOrderAccount = applicants.Where(x => x.BankAccounts.Any(y => y.IsDebitOrderBankAccount)).FirstOrDefault();
            if (applicantWithDebitOrderAccount != null)
            {
                var debitOrderBankAccount = applicantWithDebitOrderAccount.BankAccounts.Where(x => x.IsDebitOrderBankAccount).FirstOrDefault();
                if (debitOrderBankAccount != null)
                {
                    if (applicantWithDebitOrderAccount.Employments != null && applicantWithDebitOrderAccount.Employments.Count<EmploymentModel>() > 0)
                    {
                        int salaryPaymentDay = applicantWithDebitOrderAccount.Employments.Where(x => x.EmploymentStatus == EmploymentStatus.Current).First().SalaryPaymentDay;
                        debitOrderDay = salaryPaymentDay >= 28 ? 1 : salaryPaymentDay;
                    }
                }

                applicationDebitOrderModel = new ApplicationDebitOrderModel(FinancialServicePaymentType.DebitOrderPayment, debitOrderDay, debitOrderBankAccount);
            }
            return applicationDebitOrderModel;
        }
    }
}