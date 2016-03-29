using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;

using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.BankAccount
{
    [RuleDBTag("ApplicationDebitOrderPaymentBankAccountSelection",
    "Bank Account must be selected",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.BankAccount.ApplicationDebitOrderPaymentBankAccountSelection")]
    [RuleInfo]
    public class ApplicationDebitOrderPaymentBankAccountSelection : BusinessRuleBase
    {
       public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplicationDebitOrder))
                throw new ArgumentException("Parameter[0] is not of type IApplicationDebitOrder.");

            IApplicationDebitOrder appDO = (IApplicationDebitOrder)Parameters[0];

            if (appDO == null || appDO.FinancialServicePaymentType == null)
            {
                AddMessage("Please select Financial Service Payment Type","Please select Financial Service Payment Type",Messages);
            }
            else
                if(appDO.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DebitOrderPayment
                && appDO.BankAccount == null)
                AddMessage("Bank Account must be selected.", "Bank Account must be selected.", Messages);

            return 0;
        }
    }

    [RuleDBTag("ApplicationDebitOrderPaymentBankAccountNonDO",
  "Bank Account must be null for this Payment Type",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.BankAccount.ApplicationDebitOrderPaymentBankAccountNonDO")]
    [RuleInfo]
    public class ApplicationDebitOrderPaymentBankAccountNonDO : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplicationDebitOrder))
                throw new ArgumentException("Parameter[0] is not of type IApplicationDebitOrder.");

            IApplicationDebitOrder appDO = (IApplicationDebitOrder)Parameters[0];

            if (appDO == null || appDO.FinancialServicePaymentType == null)
            {
                AddMessage("Please select Financial Service Payment Type", "Please select Financial Service Payment Type",Messages);
            }
            else
            {
                if (appDO.FinancialServicePaymentType.Key != (int)FinancialServicePaymentTypes.DebitOrderPayment && appDO.BankAccount != null)
                    AddMessage("Bank Account must be null for this Payment Type.", "Bank Account must be null for this Payment Type.", Messages);
            }
            return 0;
        }
    }


    [RuleDBTag("ApplicationDebitOrderCollection",
  "Application Debit Order Details must be Captured",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.BankAccount.ApplicationDebitOrderCollection")]
    [RuleInfo]
    public class ApplicationDebitOrderCollection : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("Parameter[0] is not of type IApplication.");

            IApplication App = (IApplication)Parameters[0];

            if (App.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan || App.ApplicationType.Key == (int)OfferTypes.RefinanceLoan
                || App.ApplicationType.Key == (int)OfferTypes.SwitchLoan || App.ApplicationType.Key == (int)OfferTypes.UnsecuredLending)
            {
                if (App.ApplicationDebitOrders == null || App.ApplicationDebitOrders.Count == 0)
                    AddMessage("Application Debit Order Details must be Captured.", "Application Debit Order Details must be Captured.", Messages);
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationDebitOrderBankAccount",
    "Mortgage Loan Application Debit Order Bank Account must belong to an associated Legal Entity",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.BankAccount.ApplicationDebitOrderBankAccount")]
    [RuleInfo]
    public class ApplicationDebitOrderBankAccount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {

            IApplication App = Parameters[0] as IApplication;
            if (App == null)
                throw new ArgumentException("Parameter[0] is not of type IApplication.");

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            bool error = true; //assume this fails unless we can prove otherwise

            if (App.ApplicationDebitOrders != null && App.ApplicationDebitOrders.Count > 0)
            {
                IReadOnlyEventList<IApplicationRole> clientRoles = App.GetApplicationRolesByGroup(OfferRoleTypeGroups.Client);
                IList<IBankAccount> listLEBA = new List<IBankAccount>();

                //collect all the bank accounts for the clients associated with the account
                foreach (IApplicationRole apRl in clientRoles)
                {
                    foreach (ILegalEntityBankAccount leba in apRl.LegalEntity.LegalEntityBankAccounts)
                    {
                        if (leba.GeneralStatus.Key == (int)GeneralStatuses.Active)
                            listLEBA.Add(leba.BankAccount);
                    }
                }

                int nonDrOrderPayment = 0;
                int drOrderPayment = 0;

                if (listLEBA.Count > 0)
                {
                    //check to see if any bank account 
                    foreach (IApplicationDebitOrder dOrder in App.ApplicationDebitOrders)
                    {
                        if (dOrder.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DebitOrderPayment)
                        {
                            IBankAccount ba = dOrder.BankAccount;
                            foreach (IBankAccount leba in listLEBA)
                            {
                                if (ba.Key == leba.Key)
                                {
                                    error = false;
                                    break;
                                }
                            }
                            if (error == false)
                                break;

                            drOrderPayment += 1;
                        }
                        else
                            nonDrOrderPayment += 1;
                    }
                }
                if (drOrderPayment == 0 && nonDrOrderPayment > 0)
                    error = false; // There are no Dr Order Payments so this rule does not apply
            }
            else
                error = false;

            if (error)
                AddMessage("Application must have a valid Debit Order Bank Account from an associated Legal Entity.", "Application must have a valid Debit Order Bank Account from an associated Legal Entity.", Messages);

            return 0;
        }
    }

    [RuleDBTag("ApplicationDebitOrderPaymentTypeSubsidy",
   "There are no applicants with Employment Type Subsidy - Payment Type can not be subsidy",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.BankAccount.ApplicationDebitOrderPaymentTypeSubsidy")]
    [RuleInfo]
    public class ApplicationDebitOrderPaymentTypeSubsidy : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplicationDebitOrder appDO = (IApplicationDebitOrder)Parameters[0];

            if (appDO == null || appDO.FinancialServicePaymentType == null)
            {
                AddMessage("Please select Financial Service Payment Type", "Please select Financial Service Payment Type",Messages);
            }
            else
                if (appDO.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.SubsidyPayment)
                {
                    bool applicantWithEmploymentSubsidy = false;
                    double totalSubsidyStopOrderAmount = 0;

                    int[] roles = { (int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant, (int)SAHL.Common.Globals.OfferRoleTypes.Suretor, (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant, (int)SAHL.Common.Globals.OfferRoleTypes.LeadSuretor };

                    IReadOnlyEventList<ILegalEntityNaturalPerson> applicants = appDO.Application.GetNaturalPersonLegalEntitiesByRoleType(Messages, roles, GeneralStatusKey.Active);

                    for (int i = 0; i < applicants.Count; i++)
                    {
                        foreach (IEmployment emp in applicants[i].Employment)
                        {
                            if (emp is IEmploymentSubsidised && emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
                            {
                                IEmploymentSubsidised empSubsidy = emp as IEmploymentSubsidised;
                                if (empSubsidy.Subsidy != null)
                                    totalSubsidyStopOrderAmount += Convert.ToDouble(empSubsidy.Subsidy.StopOrderAmount);
                                applicantWithEmploymentSubsidy = true;
                            }
                        }
                    }

                    if (!applicantWithEmploymentSubsidy)
                    {
                        AddMessage("Payment Type can not be subsidy - There are no applicants on this application with Employment Type of Subsidy", "Payment Type can not be subsidy - There are no applicants on this application with Employment Type of Subsidy !", Messages);
                        return 0;
                    }

                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplicationInformation appInfo = appDO.Application.GetLatestApplicationInformation();
                    IApplicationInformationVariableLoan appVL = appRepo.GetApplicationInformationVariableLoan(appInfo.Key);

                    if (totalSubsidyStopOrderAmount < Convert.ToDouble(appVL.MonthlyInstalment))
                    {
                        AddMessage("The total Stop Order Amount of " + totalSubsidyStopOrderAmount.ToString(SAHL.Common.Constants.CurrencyFormat) + " is less than the monthly installment amount of " + Convert.ToDouble(appVL.MonthlyInstalment).ToString(SAHL.Common.Constants.CurrencyFormat) + ". Payment Type must be Debit Order", "", Messages);
                    }
                }
            return 0;
        }
    }
}
