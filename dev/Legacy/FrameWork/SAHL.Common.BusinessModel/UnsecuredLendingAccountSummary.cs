using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
    public class UnsecuredLendingAccountSummary : IAccountInstallmentSummary
    {
        private IAccountPersonalLoan account;

        public UnsecuredLendingAccountSummary(IAccount account)
        {
            this.account = account as IAccountPersonalLoan;
        }

        public double MonthsInArrears
        {
            get
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                return accRepo.GetCurrentMonthsInArrears(this.account.Key);
            }
        }

        public double MonthlyServiceFee
        {
            get { return MortgageLoanAccountHelper.MonthlyServiceFee(account); }
        }

        public double TotalAmountDue
        {
            get
            {
                double totalAmountDue = Math.Round(this.TotalLoanInstallment + this.TotalShortTermLoanInstallment + this.TotalPremium + this.MonthlyServiceFee, 2);
                return totalAmountDue;
            }
        }

        public double TotalLoanInstallment
        {
            get
            {
                double totalInstalment = 0;
                totalInstalment += this.account.FinancialServices.Sum(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.PersonalLoan ? x.Payment : 0.0d);
                return totalInstalment;
            }
        }

        public double TotalPremium
        {
            get
            {
                var personalLoanFinancialServices = account.GetFinancialServicesByType(FinancialServiceTypes.PersonalLoan, new AccountStatuses[] { AccountStatuses.Open, AccountStatuses.Closed }).OrderByDescending(x => x.Key);
                var personalLoanFinancialService = personalLoanFinancialServices.First();
                var creditProtectionPlanFinancialService = personalLoanFinancialService.FinancialServices.Where(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.SAHLCreditProtectionPlan).FirstOrDefault();
                if (creditProtectionPlanFinancialService != null)
                {
                    return creditProtectionPlanFinancialService.Payment;
                }
                return 0D;
            }
        }

        public double TotalArrearsBalance
        {
            get
            {
                var arrearBalanceFinancialService = account.GetFinancialServicesByType(FinancialServiceTypes.ArrearBalance,
                                                new AccountStatuses[]
                                                                    {
                                                                        AccountStatuses.Open,
                                                                        AccountStatuses.Closed
                                                                    }).OrderByDescending(x => x.Key).FirstOrDefault();
                if (arrearBalanceFinancialService != null)
                {
                    return arrearBalanceFinancialService.Balance.Amount;
                }
                return 0D;
            }
        }

        public double CurrentBalance
        {
            get
            {
                var personalLoanFinancialServices = account.GetFinancialServicesByType(FinancialServiceTypes.PersonalLoan, new AccountStatuses[] { AccountStatuses.Open, AccountStatuses.Closed }).OrderByDescending(x => x.Key);
                var personalLoanFinancialService = personalLoanFinancialServices.First();
                return personalLoanFinancialService.Balance.Amount;
            }
        }

        /// <summary>
        /// Not applicable to UnsecuredLending
        /// </summary>
        public double AmortisingInstallment
        {
            get { return 0D; }
        }

        /// <summary>
        /// Not applicable to UnsecuredLending
        /// </summary>
        public bool IsInterestOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Not applicable to UnsecuredLending
        /// </summary>
        public double TotalRegentPremium
        {
            get { return 0D; }
        }

        /// <summary>
        /// Not applicable to UnsecuredLending
        /// </summary>
        public double TotalShortTermLoanInstallment
        {
            get { return 0D; }
        }
    }
}